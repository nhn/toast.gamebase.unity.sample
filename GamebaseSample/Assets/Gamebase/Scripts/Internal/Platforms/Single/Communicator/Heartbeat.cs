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
            Start   = 0,
            Stop    = 1
        }

        public static Heartbeat Instance
        {
            get { return instance; }
        }

        private HeartbeatStatus status;
        private DateTime lastSentTime;
        private const float waitTime = 0.5f;

        public Heartbeat()
        {
            status          = HeartbeatStatus.Stop;
            lastSentTime    = DateTime.Now;
        }

        public void StartHeartbeat()
        {
            if (false == IsSendable())
            {
                return;
            }

            GamebaseLog.Debug("Start Heartbeat", this);

            status = HeartbeatStatus.Start;
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.HEARTBEAT_TYPE, ExecuteHeartbeat());
        }

        public void StopHeartbeat()
        {
            if (HeartbeatStatus.Stop == status)
            {
                return;
            }

            GamebaseLog.Debug("Stop Heartbeat", this);

            status = HeartbeatStatus.Stop;
        }

        private IEnumerator ExecuteHeartbeat()
        {
            if (HeartbeatStatus.Stop == status)
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
            
            yield return new WaitForSecondsRealtime(waitTime);
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.HEARTBEAT_TYPE, ExecuteHeartbeat());
        }

        private void SendHeartbeat()
        {
            var requestVO = HeartbeatMessage.GetHeartbeatMessage();
            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                GamebaseError heartbeatError = error;

                if (null == heartbeatError)
                {
                    var vo = JsonMapper.ToObject<LaunchingResponse.HeartbeatInfo>(response);
                    if (true == vo.header.isSuccessful)
                    {
                        GamebaseLog.Debug("Send heartbeat succeeded", this);
                    }
                    else
                    {
                        heartbeatError = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, string.Empty);
                    }
                }

                if(null != heartbeatError)
                {
                    if (GamebaseErrorCode.BANNED_MEMBER == heartbeatError.code || GamebaseErrorCode.INVALID_MEMBER == heartbeatError.code)
                    {
                        var vo = new GamebaseResponse.SDK.ObserverMessage();
                        vo.type = GamebaseObserverType.HEARTBEAT;
                        vo.data = new System.Collections.Generic.Dictionary<string, object>();
                        vo.data.Add("code", heartbeatError.code);

                        if (GamebaseErrorCode.BANNED_MEMBER == heartbeatError.code)
                        {
                            GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.EVENT,
                            GamebaseIndicatorReportType.StabilityCode.GB_EVENT_OBSERVER_BANNED_MEMBER,
                            GamebaseIndicatorReportType.LogLevel.INFO,
                            new Dictionary<string, string>()
                            {
                                { GamebaseIndicatorReportType.AdditionalKey.GB_OBSERVER_DATA, JsonMapper.ToJson(vo) }
                            });
                        }
                        else
                        {
                            GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.EVENT,
                            GamebaseIndicatorReportType.StabilityCode.GB_EVENT_OBSERVER_INVALID_MEMBER,
                            GamebaseIndicatorReportType.LogLevel.INFO,
                            new Dictionary<string, string>()
                            {
                                { GamebaseIndicatorReportType.AdditionalKey.GB_OBSERVER_DATA, JsonMapper.ToJson(vo) }
                            });
                        }

                        GamebaseSystemPopup.Instance.ShowHeartbeatErrorPopup(heartbeatError);

                        GamebaseObserverManager.Instance.OnObserverEvent(vo);
                        SendEventMessage(vo);
                    }
                    GamebaseLog.Debug(string.Format("Send heartbeat failed. error:{0}", GamebaseJsonUtil.ToPrettyJsonString(heartbeatError)), this);
                }

                lastSentTime = DateTime.Now;
            });
        }

        private void SendEventMessage(GamebaseResponse.SDK.ObserverMessage message)
        {
            GamebaseResponse.Event.GamebaseEventObserverData observerData = new GamebaseResponse.Event.GamebaseEventObserverData();

            object codeData = null;
            if (message.data.TryGetValue("code", out codeData) == true)
            {
                observerData.code = (int)codeData;
            }

            GamebaseResponse.Event.GamebaseEventMessage eventMessage = new GamebaseResponse.Event.GamebaseEventMessage();
            eventMessage.category = string.Format("observer{0}", GamebaseStringUtil.Capitalize(message.type));
            eventMessage.data = JsonMapper.ToJson(observerData);

            GamebaseEventHandlerManager.Instance.OnEventHandler(eventMessage);
        }

        private bool IsSendable()
        {
            if (HeartbeatStatus.Start == status)
            {
                return false;
            }

            if (false == IsLoggedin())
            {
                return false;
            }

            return true;
        }

        private bool IsLoggedin()
        {
            if (true == string.IsNullOrEmpty(Gamebase.GetUserID()))
            {
                return false;
            }

            return true;
        }
    }
}
#endif