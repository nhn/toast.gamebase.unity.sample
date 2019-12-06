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
        
        public GamebaseResponse.Launching.LaunchingInfo GetLaunchingInformations()
        {
            if (GamebaseUnitySDK.IsInitialized == false)
            {
                GamebaseLog.Warn(GamebaseStrings.NOT_INITIALIZED, this);
                return null;
            }

            var vo = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
            return JsonMapper.ToObject<GamebaseResponse.Launching.LaunchingInfo>(JsonMapper.ToJson(vo));
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
                if (null == error)
                {
                    var vo = JsonMapper.ToObject<LaunchingResponse.LaunchingInfo>(response);                    

                    if (vo.header.isSuccessful == true)
                    {
                        DataContainer.SetData(VOKey.Launching.LAUNCHING_INFO, vo);
                        Gamebase.SetDisplayLanguageCode(vo.request.displayLanguage);

                        GamebaseSystemPopup.Instance.ShowLaunchingPopup(vo);
                    }
                    else
                    {
                        error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain);

                        GamebaseSystemPopup.Instance.ShowErrorPopup(error);
                    }
                }
                else
                {
                    GamebaseSystemPopup.Instance.ShowErrorPopup(error);
                }

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo>>(handle);
                if (null == callback)
                {
                    return;
                }

                if (null == error)
                {
                    GamebaseUnitySDK.IsInitialized = true;
                    callback(JsonMapper.ToObject<GamebaseResponse.Launching.LaunchingInfo>(response), error);
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

                GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo> launchingInfoCallback = (launchingInfoTemp, errorTemp) =>
                {
                    var launchingStatusCallback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.DataDelegate<GamebaseResponse.Launching.LaunchingStatus>>(scheduleHandle);
                    if (null != launchingStatusCallback)
                    {
                        var vo = new GamebaseResponse.Launching.LaunchingStatus();
                        vo.code = launchingStatus.launching.status.code;
                        vo.message = launchingStatus.launching.status.message;

                        launchingStatusCallback(vo);
                    }
                    
                    var observerCallback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage>>(GamebaseObserverManager.Instance.Handle);
                    if (null != observerCallback)
                    {
                        var vo = new GamebaseResponse.SDK.ObserverMessage();
                        vo.type = GamebaseObserverType.LAUNCHING;
                        vo.data = new System.Collections.Generic.Dictionary<string, object>();
                        vo.data.Add("code", launchingStatus.launching.status.code);
                        vo.data.Add("message", launchingStatus.launching.status.message);
                        observerCallback(vo);
                    }

                    OnLaunchingInfoCallback(handle);
                };

                int handleTemp = GamebaseCallbackHandler.RegisterCallback(launchingInfoCallback);

                GetLaunchingInfo(handleTemp);
            });
        }
        #endregion
        
        private void OnLaunchingInfoCallback(int handle)
        {
            if(-1 == handle)
            {
                return;
            }

            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo>>(handle);
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