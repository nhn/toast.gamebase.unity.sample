#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebaseLaunching : IGamebaseLaunching
    {
        private string domain;

        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain) == true)
                {
                    return typeof(CommonGamebaseLaunching).Name;
                }

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        private WebSocketRequest.RequestVO requestVO;
        private int scheduleHandle                  = -1;
        private bool isPlaySchedule                 = false;
        private float statusElaspedTime       = 0;

        public CommonGamebaseLaunching()
        {
            requestVO = LaunchingMessage.GetLaunchingInfoMessage();
        }
        
        public LaunchingResponse.LaunchingInfo GetLaunchingInformations()
        {
            if (GamebaseUnitySDK.IsInitialized == false)
            {
                GamebaseLog.Warn(GamebaseStrings.NOT_INITIALIZED, this);
                return null;
            }

            var vo = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
            return JsonMapper.ToObject<LaunchingResponse.LaunchingInfo>(JsonMapper.ToJson(vo));
        }

        public int GetLaunchingStatus()
        {
            if (GamebaseUnitySDK.IsInitialized == false)
            {
                GamebaseLog.Warn(GamebaseStrings.NOT_INITIALIZED, this);
                return 0;
            }

            var vo = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
            return vo.launching.status.code;
        }

        public void GetLaunchingInfo(int handle)
        {
            requestVO.apiId = Lighthouse.API.Launching.ID.GET_LAUNCHING;
            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                if (error == null)
                {
                    var vo = JsonMapper.ToObject<LaunchingResponse.LaunchingInfo>(response);

                    if (vo.header.isSuccessful == true)
                    {
                        GamebaseSystemPopup.Instance.ShowLaunchingPopup(vo);

                        if (vo.launching.tcgbClient.introspection != null)
                        {
                            Introspect.Instance.SetInterval(vo.launching.tcgbClient.introspection.intervalSeconds);
                        }
                    }
                    else
                    {
                        error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain);

                        var errorExtras = vo.errorExtras;
                        if (errorExtras != null)
                        {
                            var updateInfo = errorExtras.updateInfo;
                            if (updateInfo != null)
                            {
                                error.extras.Add(GamebaseErrorExtras.UPDATE_INFO, JsonMapper.ToJson(updateInfo));
                            }

                            var language = errorExtras.language;
                            if (language != null)
                            {
                                var deviceLanguage = language.deviceLanguage;

                                if (DisplayLanguage.Instance.HasLocalizedStringVO(deviceLanguage) == true)
                                {
                                    // STEP 1: The device language in the NHN Cloud Console.
                                    GamebaseUnitySDK.DisplayLanguageCode = deviceLanguage;
                                }
                                else
                                {
                                    var defaultLanguage = language.defaultLanguage;

                                    if (DisplayLanguage.Instance.HasLocalizedStringVO(defaultLanguage) == true)
                                    {
                                        // STEP 2: The default language in the NHN Cloud Console.
                                        GamebaseUnitySDK.DisplayLanguageCode = defaultLanguage;
                                    }
                                    else
                                    {
                                        // STEP 3: "en"
                                        GamebaseUnitySDK.DisplayLanguageCode = GamebaseDisplayLanguageCode.English;
                                    }
                                }
                            }
                        }

                        GamebaseSystemPopup.Instance.ShowErrorPopup(error);
                    }
                }
                else
                {
                    GamebaseSystemPopup.Instance.ShowErrorPopup(error);
                }

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<LaunchingResponse.LaunchingInfo>>(handle);
                if (callback == null)
                {
                    return;
                }

                if (error == null)
                {
                    GamebaseUnitySDK.IsInitialized = true;
                    callback(JsonMapper.ToObject<LaunchingResponse.LaunchingInfo>(response), error);
                    ExecuteSchedule();
                }   
                else
                {
                    callback(null, error);
                }
            });
        }
        
        #region Schedule
        private void ExecuteSchedule()
        {
            GamebaseLog.Debug("Start Launching Status Schedule", this);

            if (isPlaySchedule == true)
            {
                return;
            }

            isPlaySchedule = true;
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.LAUNCHING_TYPE, ScheduleLaunchingStatus());            
        }

        private IEnumerator ScheduleLaunchingStatus()
        {
            GamebaseLog.Debug("Start Launching Status Schedule", this);

            statusElaspedTime = 0f;

            while (isPlaySchedule == true)
            {
                statusElaspedTime += Time.unscaledDeltaTime;

                if( CommunicatorConfiguration.launchingInterval <= statusElaspedTime)
                {
                    GamebaseLog.Debug(
                        string.Format(
                            "LaunchingStatusElaspedTime : {0} (Standard is more than {1})",
                            statusElaspedTime,
                            CommunicatorConfiguration.launchingInterval),
                        this);
                    RequestLaunchingStatus();
                }
                
                yield return null;
            }            
        }

        public void RequestLaunchingStatus(int handle = -1)
        {
            GamebaseLog.Debug("Check Launching Status", this);
            statusElaspedTime = 0;

            requestVO.apiId = Lighthouse.API.Launching.ID.GET_LAUNCHING_STATUS;
            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                GamebaseSystemPopup.Instance.ShowErrorPopup(error);

                if (null != error)
                {
                    return;
                }

                var launchingInfo = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
                var launchingStatus = JsonMapper.ToObject<LaunchingResponse.LaunchingStatus>(response);
                if (launchingInfo.launching.status.code == launchingStatus.launching.status.code)
                {
                    OnLaunchingInfoCallback(handle);
                    return;
                }

                GamebaseLog.Debug("Check Launching Info", this);

                GamebaseCallback.GamebaseDelegate<LaunchingResponse.LaunchingInfo> launchingInfoCallback = (launchingInfoTemp, errorTemp) =>
                {
                    var launchingStatusCallback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.DataDelegate<GamebaseResponse.Launching.LaunchingStatus>>(scheduleHandle);
                    if (null != launchingStatusCallback)
                    {
                        var vo = new GamebaseResponse.Launching.LaunchingStatus();
                        vo.code = launchingStatus.launching.status.code;
                        vo.message = launchingStatus.launching.status.message;

                        launchingStatusCallback(vo);
                    }                    
                    
                    var observerMessage = new GamebaseResponse.SDK.ObserverMessage();
                    observerMessage.type = GamebaseObserverType.LAUNCHING;
                    observerMessage.data = new System.Collections.Generic.Dictionary<string, object>();
                    observerMessage.data.Add("code", launchingStatus.launching.status.code);
                    observerMessage.data.Add("message", launchingStatus.launching.status.message);
                    GamebaseObserverManager.Instance.OnObserverEvent(observerMessage);
                    SendEventMessage(observerMessage);

                    OnLaunchingInfoCallback(handle);
                };

                int handleTemp = GamebaseCallbackHandler.RegisterCallback(launchingInfoCallback);

                GetLaunchingInfo(handleTemp);
            });
        }
        #endregion

        private void SendEventMessage(GamebaseResponse.SDK.ObserverMessage message)
        {
            GamebaseResponse.Event.GamebaseEventObserverData observerData = new GamebaseResponse.Event.GamebaseEventObserverData();

            object codeData = null;
            if (message.data.TryGetValue("code", out codeData) == true)
            {
                observerData.code = (int)codeData;
            }

            object messageData = null;
            if (message.data.TryGetValue("message", out messageData) == true)
            {
                observerData.message = (string)messageData;
            }

            GamebaseResponse.Event.GamebaseEventMessage eventMessage = new GamebaseResponse.Event.GamebaseEventMessage();
            eventMessage.category = string.Format("observer{0}", GamebaseStringUtil.Capitalize(message.type));
            eventMessage.data = JsonMapper.ToJson(observerData);

            GamebaseEventHandlerManager.Instance.OnEventHandler(eventMessage);
        }

        private void OnLaunchingInfoCallback(int handle)
        {
            if(-1 == handle)
            {
                return;
            }

            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<LaunchingResponse.LaunchingInfo>>(handle);
            if (null == callback)
            {
                return;
            }
            callback(null, null);
        }

        public static bool IsPlayable()
        {
            var status = Gamebase.Launching.GetLaunchingStatus();
            // 200 ~ 299 playable
            if (200 <= status && 300 > status)
            {
                return true;
            }

            return false;
        }

        public float GetStatusElaspedTime()
        {
            return statusElaspedTime;
        }
    }
}
#endif