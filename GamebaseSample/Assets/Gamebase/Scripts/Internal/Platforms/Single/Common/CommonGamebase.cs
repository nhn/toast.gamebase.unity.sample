#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

using System;
using System.Collections;
using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebase : IGamebase
    {
        private string domain;

        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain))
                    return typeof(CommonGamebase).Name;

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        private int initializeHandle = -1;                
        private int initializeFailCount = 0;

        public void SetDebugMode(bool isDebugMode)
        {
            GamebaseDebugSettings.Instance.SetDebugMode(isDebugMode);
            ToastSdk.DebugMode = isDebugMode;
        }
        
        public void Initialize(GamebaseRequest.GamebaseConfiguration configuration, int handle)
        {
            if(initializeHandle != -1)
            {
                GamebaseCallbackHandler.UnregisterCallback(initializeHandle);
            }

            GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo> initializeCallback = (launchingInfo, error) =>
            {
                GamebaseResponse.Launching.LaunchingInfo.GamebaseLaunching.TCGBClient.Stability stability = null;
                if (error == null || error.code == GamebaseErrorCode.SUCCESS)
                {                   
#region Iap Setting
                    GamebaseLog.Debug("ToastSdk Initialize", this);
                    ToastSdk.Initialize();

                    if (PurchaseAdapterManager.Instance.CreateIDPAdapter("iapadapter") == true)
                    {
                        var iapConfiguration = new GamebaseRequest.Purchase.Configuration();
                        iapConfiguration.appKey = launchingInfo.tcProduct.iap.appKey;
                        iapConfiguration.storeCode = configuration.storeCode;
                        PurchaseAdapterManager.Instance.SetConfiguration(iapConfiguration);
                    }

                    stability = launchingInfo.launching.tcgbClient.stability;                    
                    #endregion
                }

                GamebaseIndicatorReport.Initialize(
                    stability,
                    ()=>{
                        if (Gamebase.IsSuccess(error)  == false)
                        {
                            initializeFailCount++;
                            if (initializeFailCount > GamebaseIndicatorReport.stability.initFailCount)
                            {
                                GamebaseIndicatorReport.SendIndicatorData(
                                    GamebaseIndicatorReportType.LogType.INIT,
                                    GamebaseIndicatorReportType.StabilityCode.GB_INIT_FAILED_MULTIPLE_TIMES,
                                    GamebaseIndicatorReportType.LogLevel.WARN,
                                    new Dictionary<string, string>()
                                    {
                                 { GamebaseIndicatorReportType.AdditionalKey.GB_CONFIGURATION, JsonMapper.ToJson(configuration) }
                                    });
                                initializeFailCount = 0;
                            }
                        }
                        else
                        {
                            initializeFailCount = 0;
                        }

                        var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo>>(handle);

                        if (callback != null)
                        {
                            callback(launchingInfo, error);
                        }

                        GamebaseCallbackHandler.UnregisterCallback(handle);
                    });                
            };

            initializeHandle = GamebaseCallbackHandler.RegisterCallback(initializeCallback); 
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.CORE_TYPE, Init());
        }

        public string GetSDKVersion()
        {
            return GamebaseUnitySDK.SDKVersion;
        }

        public string GetUserID()
        {
            var vo = DataContainer.GetData<AuthResponse.LoginInfo>(VOKey.Auth.LOGIN_INFO);
            if (vo == null)
                return string.Empty;

            return vo.member.userId;
        }

        public string GetAccessToken()
        {
            var vo = DataContainer.GetData<AuthResponse.LoginInfo>(VOKey.Auth.LOGIN_INFO);
            if (vo == null)
                return string.Empty;

            return vo.token.accessToken;
        }

        public string GetLastLoggedInProvider()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return string.Empty;
        }
        
        public string GetDeviceLanguageCode()
        {
            return GamebaseUnitySDK.DeviceLanguageCode;
        }

        public string GetCarrierCode()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return string.Empty;
        }

        public string GetCarrierName()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return string.Empty;
        }

        public string GetCountryCode()
        {
            string countryCode = GetCountryCodeOfUSIM();
            if(string.IsNullOrEmpty(countryCode) == true)
            {
                countryCode = GetCountryCodeOfDevice();
            }
            return countryCode;
        }

        public string GetCountryCodeOfUSIM()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return string.Empty;
        }

        public string GetCountryCodeOfDevice()
        {
            return GamebaseUnitySDK.CountryCode;
        }

        public bool IsSandbox()
        {
            if (GamebaseUnitySDK.IsInitialized == false)
            {
                GamebaseLog.Warn(GamebaseStrings.NOT_INITIALIZED, this);
                return false;
            }

            var vo = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
            return vo.launching.app.typeCode.Equals("SANDBOX", System.StringComparison.Ordinal);
        }

        public void SetDisplayLanguageCode(string languageCode)
        {
            if (true == DisplayLanguage.Instance.HasLocalizedStringVO(languageCode))
            {
                GamebaseUnitySDK.DisplayLanguageCode = languageCode;
            }
            else
            {
                var launchingVO = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
                var launchingDeviceLanguageCode = launchingVO.launching.app.language.deviceLanguage;

                if (true == DisplayLanguage.Instance.HasLocalizedStringVO(launchingDeviceLanguageCode))
                {
                    GamebaseUnitySDK.DisplayLanguageCode = launchingDeviceLanguageCode;
                    GamebaseLog.Warn(GamebaseStrings.DISPLAY_LANGUAGE_CODE_NOT_FOUND, this);
                }
                else
                {
                    GamebaseUnitySDK.DisplayLanguageCode = GamebaseDisplayLanguageCode.English;
                    GamebaseLog.Warn(GamebaseStrings.SET_DEFAULT_DISPLAY_LANGUAGE_CODE, this);
                }
            }
        }

        public string GetDisplayLanguageCode()
        {
            return GamebaseUnitySDK.DisplayLanguageCode;
        }

        private IEnumerator Init()
        {
            yield return DisplayLanguage.Instance.DisplayLanguageInitialize();

            WebSocket.Instance.Initialize();
            yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.CORE_TYPE, WebSocket.Instance.Connect((error) =>
            {
                GamebaseSystemPopup.Instance.ShowErrorPopup(error);

                if (error == null)
                {
                    GamebaseLaunchingImplementation.Instance.RequestLaunchingInfo(initializeHandle);
                    return;
                }

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo>>(initializeHandle);

                if (callback != null)
                {
                    error.domain = Domain;
                    callback(null, error);
                }

                GamebaseCallbackHandler.UnregisterCallback(initializeHandle);
                initializeHandle = -1;
            }));
        }

        public string GetDisplayLanguage()
        {
            return string.Empty;
        }

        public void AddObserver(int handle)
        {

        }

        public void RemoveObserver()
        {

        }

        public void RemoveAllObserver()
        {

        }

        public void AddServerPushEvent(int handle)
        {

        }

        public void RemoveServerPushEvent()
        {

        }

        public void RemoveAllServerPushEvent()
        {

        }    
    }
}
#endif