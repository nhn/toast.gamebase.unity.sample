#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)

using System;
using System.Collections;
using System.Collections.Generic;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public sealed class Heartbeat
    {
        private static readonly Heartbeat instance = new Heartbeat();
        
        private enum HeartbeatStatus
        {
            START   = 0,
            STOP    = 1
        }

        public static Heartbeat Instance
        {
            get { return instance; }
        }

        private HeartbeatStatus status;
        private DateTime lastSentTime;
        private const float WAIT_TIME = 0.5f;

        public Heartbeat()
        {
            status          = HeartbeatStatus.STOP;
            lastSentTime    = DateTime.Now;
        }

        public void StartHeartbeat()
        {
            if (IsSendable() == false)
            {
                return;
            }

            GamebaseLog.Debug("Start Heartbeat", this);

            status = HeartbeatStatus.START;
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.HEARTBEAT_TYPE, ExecuteHeartbeat());
        }

        public void StopHeartbeat()
        {
            if (HeartbeatStatus.STOP == status)
            {
                return;
            }

            GamebaseLog.Debug("Stop Heartbeat", this);

            status = HeartbeatStatus.STOP;
        }

        private IEnumerator ExecuteHeartbeat()
        {
            if (HeartbeatStatus.STOP == status)
            {
                yield break;
            }

            var timestamp = (int)(DateTime.Now - lastSentTime).TotalSeconds;
            if (CommunicatorConfiguration.heartbeatInterval <= timestamp)
            {
                GamebaseLog.Debug(
                    string.Format(
                        "HeartbeatElaspedTime : {0} (Standard is more than {1})",
                        timestamp,
                        CommunicatorConfiguration.heartbeatInterval),
                    this);
                SendHeartbeat();
            }
            
            yield return new WaitForSecondsRealtime(WAIT_TIME);
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.HEARTBEAT_TYPE, ExecuteHeartbeat());
        }

        private void SendHeartbeat()
        {
            // WebSocket Request 후 Response의 응답 속도가 0.5초를 넘길 경우, SendHeartbeat 메소드가 한번 더 호출된다.
            // 이에 대한 예외 처리로 Request 호출 즉시 lastSentTime을 초기화한다.
            lastSentTime = DateTime.Now;

            var requestVO = HeartbeatMessage.GetHeartbeatMessage();
            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                GamebaseError heartbeatError = error;

                if (heartbeatError == null)
                {
                    var vo = JsonMapper.ToObject<LaunchingResponse.HeartbeatInfo>(response);
                    if (vo.header.isSuccessful == true)
                    {
                        GamebaseLog.Debug("Send heartbeat succeeded", this);
                    }
                    else
                    {
                        heartbeatError = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, string.Empty);
                    }
                }

                if (heartbeatError != null)
                {
                    if (heartbeatError.code == GamebaseErrorCode.BANNED_MEMBER ||
                        heartbeatError.code == GamebaseErrorCode.INVALID_MEMBER)
                    {
                        SendObserverEvent(heartbeatError.code);

                        var observerData = new GamebaseResponse.Event.GamebaseEventObserverData()
                        {
                            code = heartbeatError.code,
                            message = heartbeatError.message,
                            extras = JsonMapper.ToJson(heartbeatError)
                        };

                        SendEventMessage(GamebaseEventCategory.OBSERVER_HEARTBEAT, JsonMapper.ToJson(observerData));

                        if (heartbeatError.code == GamebaseErrorCode.BANNED_MEMBER)
                        {
                            GamebaseIndicatorReport.SendIndicatorData(
                                GamebaseIndicatorReportType.LogType.EVENT,
                                GamebaseIndicatorReportType.StabilityCode.GB_EVENT_OBSERVER_BANNED_MEMBER,
                                GamebaseIndicatorReportType.LogLevel.INFO,
                                new Dictionary<string, string>()
                                {
                                        { GamebaseIndicatorReportType.AdditionalKey.GB_OBSERVER_DATA, JsonMapper.ToJson(observerData) }
                                });
                        }
                        else if (heartbeatError.code == GamebaseErrorCode.INVALID_MEMBER)
                        {
                            GamebaseIndicatorReport.SendIndicatorData(
                                GamebaseIndicatorReportType.LogType.EVENT,
                                GamebaseIndicatorReportType.StabilityCode.GB_EVENT_OBSERVER_INVALID_MEMBER,
                                GamebaseIndicatorReportType.LogLevel.INFO,
                                new Dictionary<string, string>()
                                {
                                        { GamebaseIndicatorReportType.AdditionalKey.GB_OBSERVER_DATA, JsonMapper.ToJson(observerData) }
                                });
                        }

                        if (GamebaseUnitySDK.EnablePopup == true)
                        {
                            GamebaseSystemPopup.Instance.ShowHeartbeatErrorPopup(heartbeatError);
                        }
                    }
                    else if (heartbeatError.code == GamebaseErrorCode.AUTH_INVALID_GAMEBASE_TOKEN)
                    {
                        var loggedOutData = new GamebaseResponse.Event.GamebaseEventLoggedOutData()
                        {
                            message = heartbeatError.message,
                            extras = JsonMapper.ToJson(heartbeatError)
                        };

                        SendEventMessage(GamebaseEventCategory.LOGGED_OUT, JsonMapper.ToJson(loggedOutData));

                        GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.EVENT,
                            GamebaseIndicatorReportType.StabilityCode.GB_EVENT_LOGGED_OUT,
                            GamebaseIndicatorReportType.LogLevel.WARN,
                            new Dictionary<string, string>()
                            {
                                { GamebaseIndicatorReportType.AdditionalKey.GB_EVENT_LOGGED_OUT_DATA, loggedOutData.extras }
                            });
                    }

                    GamebaseLog.Debug(string.Format("Send heartbeat failed. error:{0}", GamebaseJsonUtil.ToPrettyJsonString(heartbeatError)), this);
                }

                lastSentTime = DateTime.Now;
            });
        }

        private void SendObserverEvent(int heartbeatErrorCode)
        {
            var message = new GamebaseResponse.SDK.ObserverMessage
            {
                data = new Dictionary<string, object>(),
                type = GamebaseObserverType.HEARTBEAT
            };

            message.data.Add("code", heartbeatErrorCode);
            GamebaseObserverManager.Instance.OnObserverEvent(message);
        }

        private void SendEventMessage(string category, string data)
        {
            var message = new GamebaseResponse.Event.GamebaseEventMessage()
            {
                category = category,
                data = data
            };

            GamebaseEventHandlerManager.Instance.OnEventHandler(message);
        }

        private bool IsSendable()
        {
            if (HeartbeatStatus.START == status)
            {
                return false;
            }

            if (IsLoggedin() == false)
            {
                return false;
            }

            return true;
        }

        private bool IsLoggedin()
        {
            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
            {
                return false;
            }

            return true;
        }
    }
}
#endif