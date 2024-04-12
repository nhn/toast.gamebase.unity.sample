#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

using GamePlatform.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
            GpLogger.DebugMode = isDebugMode;
        }
        
        public void Initialize(GamebaseRequest.GamebaseConfiguration configuration, int handle)
        {
            if (initializeHandle != -1)
            {
                GamebaseCallbackHandler.UnregisterCallback(initializeHandle);
            }

            GamebaseCallback.GamebaseDelegate<LaunchingResponse.LaunchingInfo> initializeCallback = (launchingInfo, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    GamebaseEncryptUtilHelper.Initialize(GamebaseUnitySDK.AppID);
                    DecryptLaunchingInfo(launchingInfo);

                    DataContainer.SetData(VOKey.Launching.LAUNCHING_INFO, launchingInfo);
                    Gamebase.SetDisplayLanguageCode(launchingInfo.request.displayLanguage);

                    IapAdapterInitialize(configuration, launchingInfo);
                }

                IndicatorReportInitialize(
                    configuration,
                    launchingInfo,
                    error,
                    () =>
                    {
                        var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<LaunchingResponse.LaunchingInfo>>(handle);

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

        /// <summary>
        /// "launching.tcgbClient.stability.appKey"
        /// "tcProduct.*.appKey"
        /// "launching.app.idP.*.clientSecret"
        /// "launching.tcgbClient.forceRemoteSettings.log.appKeyIndicator"
        /// "launching.tcgbClient.forceRemoteSettings.log.appKeyLog"
        /// 
        /// line channel is not used in WebGL, Standalone.
        /// "launching.app.idP.line.channels.[].clientSecret"
        /// </summary>
        /// <param name="launchingInfo"></param>

        private void DecryptLaunchingInfo(LaunchingResponse.LaunchingInfo launchingInfo)
        {
            try
            {
                DecryptString(ref launchingInfo.launching.tcgbClient.stability.appKey);
                DecryptString(ref launchingInfo.launching.tcgbClient.forceRemoteSettings.log.appKeyIndicator);
                DecryptString(ref launchingInfo.launching.tcgbClient.forceRemoteSettings.log.appKeyLog);
                DecryptTcProduct(launchingInfo.tcProduct);
                DecryptIdP(launchingInfo.launching.app.idP);
            }
            catch (Exception e)
            {
                GamebaseLog.Warn(string.Format("An error occurred while decrypting LaunchInfo. message:{0}, stackTrace:{1}", e.Message, e.StackTrace), this);
            }
        }

        private void DecryptTcProduct(LaunchingResponse.LaunchingInfo.TCProduct tcProduct)
        {
            var fieldInfos = tcProduct.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fieldInfos)
            {
                if (field.FieldType == typeof(LaunchingResponse.LaunchingInfo.TCProduct.AppKeyInfo))
                {
                    var appkeyInfo = (LaunchingResponse.LaunchingInfo.TCProduct.AppKeyInfo)field.GetValue(tcProduct);
                    if (appkeyInfo != null)
                    {
                        DecryptString(ref appkeyInfo.appKey);
                    }
                }
            }
        }

        private void DecryptIdP(Dictionary<string, LaunchingResponse.LaunchingInfo.Launching.App.IDP> idPs)
        {
            foreach (var idP in idPs)
            {
                DecryptString(ref idP.Value.clientSecret);
            }
        }

        private void DecryptString(ref string encryptedString)
        {
            if (string.IsNullOrEmpty(encryptedString) == true)
            {
                return;
            }

            encryptedString = GamebaseEncryptUtilHelper.DecryptLaunchingEncryptedKey(encryptedString);
        }

        public string GetSDKVersion()
        {
            return GamebaseUnitySDK.SDKVersion;
        }

        public string GetUserID()
        {
            var vo = DataContainer.GetData<AuthResponse.LoginInfo>(VOKey.Auth.LOGIN_INFO);
            if (vo == null)
            {
                return string.Empty;
            }

            return vo.member.userId;
        }

        public string GetAccessToken()
        {
            var vo = DataContainer.GetData<AuthResponse.LoginInfo>(VOKey.Auth.LOGIN_INFO);
            if (vo == null)
            {
                return string.Empty;
            }

            return vo.token.accessToken;
        }

        public string GetLastLoggedInProvider()
        {
            var vo = DataContainer.GetData<AuthResponse.LoginInfo>(VOKey.Auth.LOGIN_INFO);
            if (vo == null)
            {
                return string.Empty;
            }

            return vo.token.sourceIdPCode;
        }
        
        public string GetDeviceLanguageCode()
        {
            return GamebaseSystemInfo.DeviceLanguageCode;
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
            return GamebaseSystemInfo.CountryCode;
        }

        public bool IsSandbox()
        {
            if (GamebaseUnitySDK.IsInitialized == false)
            {
                GamebaseLog.Warn(GamebaseStrings.NOT_INITIALIZED, this);
                return false;
            }

            var vo = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
            return vo.launching.app.typeCode.Equals("SANDBOX", StringComparison.Ordinal);
        }

        public void SetDisplayLanguageCode(string languageCode)
        {
            if (DisplayLanguage.Instance.HasLocalizedStringVO(languageCode) == true)
            {
                // STEP 1: User input
                GamebaseUnitySDK.DisplayLanguageCode = languageCode;
            }
            else
            {
                var launchingInfo = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
                if (launchingInfo == null)
                {
                    // If initialization fails, data is null.
                    // Return after setting en.
                    GamebaseUnitySDK.DisplayLanguageCode = GamebaseDisplayLanguageCode.English;
                    return;
                }

                var languageData = launchingInfo.launching.app.language;
                var deviceLanguageCode = languageData.deviceLanguage;

                if (DisplayLanguage.Instance.HasLocalizedStringVO(deviceLanguageCode) == true)
                {
                    // STEP 2: The device language in the NHN Cloud Console.
                    GamebaseUnitySDK.DisplayLanguageCode = deviceLanguageCode;
                    GamebaseLog.Warn(GamebaseStrings.DISPLAY_LANGUAGE_CODE_NOT_FOUND, this);
                }
                else
                {
                    var defaultLanguage = languageData.defaultLanguage;

                    if (DisplayLanguage.Instance.HasLocalizedStringVO(defaultLanguage) == true)
                    {
                        // STEP 3: The default language in the NHN Cloud Console.
                        GamebaseUnitySDK.DisplayLanguageCode = defaultLanguage;
                        GamebaseLog.Warn(GamebaseStrings.SET_CONSOLE_DEFAULT_DISPLAY_LANGUAGE_CODE, this);
                    }
                    else
                    {
                        // STEP 4: "en"
                        GamebaseUnitySDK.DisplayLanguageCode = GamebaseDisplayLanguageCode.English;
                        GamebaseLog.Warn(GamebaseStrings.SET_DEFAULT_DISPLAY_LANGUAGE_CODE, this);
                    }                    
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

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<LaunchingResponse.LaunchingInfo>>(initializeHandle);

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

        public void AddEventHandler(int handle)
        {

        }

        public void RemoveEventHandler()
        {

        }

        public void RemoveAllEventHandler()
        {

        }
        
        private void IapAdapterInitialize(GamebaseRequest.GamebaseConfiguration configuration, LaunchingResponse.LaunchingInfo launchingInfo)
        {
            if (PurchaseAdapterManager.Instance.CreateIAPAdapter("iapadapter") == true)
            {
                var iapConfiguration = new PurchaseRequest.Configuration();
                iapConfiguration.appKey = launchingInfo.tcProduct.iap.appKey;
                iapConfiguration.storeCode = configuration.storeCode;
                PurchaseAdapterManager.Instance.SetConfiguration(iapConfiguration);
            }
        }

        private void IndicatorReportInitialize(GamebaseRequest.GamebaseConfiguration configuration, LaunchingResponse.LaunchingInfo launchingInfo, GamebaseError error, Action callback)
        {
            LaunchingResponse.LaunchingInfo.Launching.TCGBClient.Stability stability = null;

            if (error == null || error.code == GamebaseErrorCode.SUCCESS)
            {
                stability = launchingInfo.launching.tcgbClient.stability;
            }

            GamebaseIndicatorReport.Initialize(
                stability,
                () => {
                    if (Gamebase.IsSuccess(error) == false)
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

                    callback();
                });
        }
    }
}
#endif