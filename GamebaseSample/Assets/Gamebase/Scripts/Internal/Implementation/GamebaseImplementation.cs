#if !UNITY_EDITOR && UNITY_ANDROID
using Toast.Gamebase.Internal.Mobile.Android;
#elif !UNITY_EDITOR && UNITY_IOS
using Toast.Gamebase.Internal.Mobile.IOS;
#elif !UNITY_EDITOR && UNITY_WEBGL
using Toast.Gamebase.Internal.Single.WebGL;
#else
using Toast.Gamebase.Internal.Single.Standalone;
#endif

#if !UNITY_EDITOR
using UnityEngine;
#endif

using System.Collections.Generic;
using System;
using Toast.Gamebase.LitJson;
using System.Text;

namespace Toast.Gamebase.Internal
{
    public sealed class GamebaseImplementation
    {        
        private const string GAMEBASE_INDICATOR_REPORT_UNITY_EDITOR_VERSION = "UNITY_EDITOR_VERSION";
        private const string GAMEBASE_INDICATOR_REPORT_SDK_VERSION = "GAMEBASE_INDICATOR_REPORT_SDK_VERSION";
        private const string SEND_DATA_KEY_LAUNCHING_ZONE = "LaunchingZone";
        public const string SEND_DATA_KEY_UNITY_EDITOR_VERSION = "UnityEditorVersion";
        public const string SEND_DATA_KEY_UNITY_SDK_VERSION = "UnitySDKVersion";
        public const string SEND_DATA_KEY_PLATFORM_SDK_VERSION = "PlatformSDKVersion";

        private static readonly GamebaseImplementation instance = new GamebaseImplementation();

        public static GamebaseImplementation Instance
        {
            get { return instance; }
        }

        IGamebase sdk;

        private GamebaseImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            sdk = new AndroidGamebase();
#elif !UNITY_EDITOR && UNITY_IOS
            sdk = new IOSGamebase();
#elif !UNITY_EDITOR && UNITY_WEBGL
            sdk = new WebGLGamebase();
#else
            sdk = new StandaloneGamebase();
#endif
        }

        public void SetDebugMode(bool isDebugMode)
        {
            sdk.SetDebugMode(isDebugMode);
        }

        public void Initialize(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo> callback)
        {
            Initialize(GetGamebaseConfiguration(), callback);
        }

        public void Initialize(GamebaseRequest.GamebaseConfiguration configuration, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo> callback)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Gamebase VERSION:{0}", GamebaseUnitySDK.SDKVersion));
            sb.AppendLine(string.Format("Gamebase Configuration:{0}", GamebaseJsonUtil.ToPrettyJsonString(configuration)));
            GamebaseLog.Debug(string.Format("{0}", sb), this);
            
            InitializeUnitySDK();

            configuration.zoneType = GetVerifiedZoneType(configuration.zoneType);

            SetGamebaseConfiguration(configuration);

            GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo> initializeCallback = (launchingInfo, error) =>
            {
                if ((error == null || error.code == GamebaseErrorCode.SUCCESS))
                {
                    GamebaseDebugSettings.Instance.SetRemoteSettings(launchingInfo);

                    GamebaseWaterMark.ShowWaterMark();
                }

                SendUnityEditorInfomation();                

                callback(launchingInfo, error);
            };

            int handle = GamebaseCallbackHandler.RegisterCallback(initializeCallback);            
            sdk.Initialize(configuration, handle);
        }      
               
        public string GetSDKVersion()
        {
            return sdk.GetSDKVersion();
        }

        public string GetUserID()
        {
            return sdk.GetUserID();
        }

        public string GetAccessToken()
        {
            return sdk.GetAccessToken();
        }

        public string GetLastLoggedInProvider()
        {
            return sdk.GetLastLoggedInProvider();
        }
                        
        public string GetDeviceLanguageCode()
        {
            return sdk.GetDeviceLanguageCode();
        }

        public string GetCarrierCode()
        {
            return sdk.GetCarrierCode();
        }

        public string GetCarrierName()
        {
            return sdk.GetCarrierName();
        }

        public string GetCountryCode()
        {
            return sdk.GetCountryCode();
        }

        public string GetCountryCodeOfUSIM()
        {
            return sdk.GetCountryCodeOfUSIM();
        }

        public string GetCountryCodeOfDevice()
        {
            return sdk.GetCountryCodeOfDevice();
        }

        public bool IsSandbox()
        {
            return sdk.IsSandbox();
        }

        public void SetDisplayLanguageCode(string languageCode)
        {
            sdk.SetDisplayLanguageCode(languageCode);
        }
        
        public string GetDisplayLanguageCode()
        {
            return sdk.GetDisplayLanguageCode();
        }
        
        private void InitializeUnitySDK()
        {
            InitializeLitJson();
        }

        private void InitializeLitJson()
        {
            JsonMapper.RegisterExporter<float>((obj, writer) => writer.Write(Convert.ToDouble(obj)));
            JsonMapper.RegisterImporter<float, int>((float input) => { return (int)input; });
            JsonMapper.RegisterImporter<float, long>((float input) => { return (long)input; });
            JsonMapper.RegisterImporter<int, long>((int input) => { return (long)input; });
            JsonMapper.RegisterImporter<int, double>((int input) => { return (double)input; });
            JsonMapper.RegisterImporter<int, float>((int input) => { return (float)input; });
            JsonMapper.RegisterImporter<double, int>((double input) => { return (int)input; });
            JsonMapper.RegisterImporter<double, long>((double input) => { return (long)input; });
            JsonMapper.RegisterImporter<double, float>(input => Convert.ToSingle(input));
        }

        private GamebaseRequest.GamebaseConfiguration GetGamebaseConfiguration()
        {
            var configuration                               = new GamebaseRequest.GamebaseConfiguration();
            configuration.appID                             = GamebaseUnitySDK.AppID;
            configuration.appVersion                        = GamebaseUnitySDK.AppVersion;
            configuration.zoneType                          = GamebaseUnitySDK.ZoneType;
            configuration.displayLanguageCode               = GamebaseUnitySDK.DisplayLanguageCode;
            configuration.enablePopup                       = GamebaseUnitySDK.EnablePopup;
            configuration.enableLaunchingStatusPopup        = GamebaseUnitySDK.EnableLaunchingStatusPopup;
            configuration.enableBanPopup                    = GamebaseUnitySDK.EnableBanPopup;
            configuration.enableKickoutPopup                = GamebaseUnitySDK.EnableKickoutPopup;
            configuration.fcmSenderId                       = GamebaseUnitySDK.FcmSenderId;
            configuration.storeCode                         = GamebaseUnitySDK.StoreCode;
            configuration.useWebViewLogin                   = GamebaseUnitySDK.UseWebViewLogin;
            return configuration;
        }

        private void SetGamebaseConfiguration(GamebaseRequest.GamebaseConfiguration configuration)
        {
            GamebaseUnitySDK.AppID                          = configuration.appID;
            GamebaseUnitySDK.AppVersion                     = configuration.appVersion;
            GamebaseUnitySDK.ZoneType                       = configuration.zoneType;
            GamebaseUnitySDK.DisplayLanguageCode            = configuration.displayLanguageCode;
            GamebaseUnitySDK.EnablePopup                    = configuration.enablePopup;
            GamebaseUnitySDK.EnableLaunchingStatusPopup     = configuration.enableLaunchingStatusPopup;
            GamebaseUnitySDK.EnableBanPopup                 = configuration.enableBanPopup;
            GamebaseUnitySDK.EnableKickoutPopup             = configuration.enableKickoutPopup;
            GamebaseUnitySDK.FcmSenderId                    = configuration.fcmSenderId;
            GamebaseUnitySDK.StoreCode                      = configuration.storeCode;
            GamebaseUnitySDK.UseWebViewLogin                = configuration.useWebViewLogin;
        }

        private string GetVerifiedZoneType(string  zoneType)
        {
            if (string.IsNullOrEmpty(zoneType) == false)
            {
                if (zoneType.ToLower().Equals("alpha") == true)
                {
                    return "alpha";
                }
                else if (zoneType.ToLower().Equals("beta") == true)
                {
                    return "beta";
                }
            }

            return "real";
        }

        public void AddObserver(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage> observer)
        {
            GamebaseObserverManager.Instance.AddObserver(observer);
            sdk.AddObserver(GamebaseObserverManager.Instance.Handle);
        }

        public void RemoveObserver(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage> observer)
        {
            GamebaseObserverManager.Instance.RemoveObserver(observer);
            sdk.RemoveObserver();
        }

        public void RemoveAllObserver()
        {
            GamebaseObserverManager.Instance.RemoveAllObserver();
            sdk.RemoveAllObserver();
        }

        public void AddServerPushEvent(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage> serverPushEvent)
        {
            GamebaseServerPushEventManager.Instance.AddServerPushEvent(serverPushEvent);
            sdk.AddServerPushEvent(GamebaseServerPushEventManager.Instance.Handle);
        }
        
        public void RemoveServerPushEvent(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage> serverPushEvent)
        {
            GamebaseServerPushEventManager.Instance.RemoveServerPushEvent(serverPushEvent);
            sdk.RemoveServerPushEvent();
        }
        
        public void RemoveAllServerPushEvent()
        {
            GamebaseServerPushEventManager.Instance.RemoveAllServerPushEvent();
            sdk.RemoveAllServerPushEvent();
        }

        private void SendUnityEditorInfomation()
        {
#if !UNITY_EDITOR
            string unityEditorVersion = PlayerPrefs.GetString(GAMEBASE_INDICATOR_REPORT_UNITY_EDITOR_VERSION, string.Empty);
            string gamebaseSDKVersion = PlayerPrefs.GetString(GAMEBASE_INDICATOR_REPORT_SDK_VERSION, string.Empty);
            
            if (unityEditorVersion.Equals(Application.unityVersion) == true && gamebaseSDKVersion.Equals(GamebaseUnitySDK.SDK_VERSION) == true)
            {
                return;
            }

            PlayerPrefs.SetString(GAMEBASE_INDICATOR_REPORT_UNITY_EDITOR_VERSION, Application.unityVersion);
            PlayerPrefs.SetString(GAMEBASE_INDICATOR_REPORT_SDK_VERSION, GamebaseUnitySDK.SDK_VERSION);

            GamebaseLogReport.Instance.SendIndicatorReport(
                GamebaseLogReport.IndicatorReport.LevelType.INDICATOR_REPORT,
                GAMEBASE_INDICATOR_REPORT_UNITY_EDITOR_VERSION,
                new Dictionary<string, string>
                {
                    { SEND_DATA_KEY_LAUNCHING_ZONE,    GamebaseUnitySDK.ZoneType },
                    { SEND_DATA_KEY_UNITY_EDITOR_VERSION, Application.unityVersion },
                    { SEND_DATA_KEY_UNITY_SDK_VERSION, GamebaseUnitySDK.SDKVersion },
                    { SEND_DATA_KEY_PLATFORM_SDK_VERSION, GetSDKVersion() }
                });
#endif
        }
    }
}