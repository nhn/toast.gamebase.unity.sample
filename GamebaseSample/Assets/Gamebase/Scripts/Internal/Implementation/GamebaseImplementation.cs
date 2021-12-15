#if !UNITY_EDITOR && UNITY_ANDROID
using Toast.Gamebase.Internal.Mobile.Android;
#elif !UNITY_EDITOR && UNITY_IOS
using Toast.Gamebase.Internal.Mobile.IOS;
#elif !UNITY_EDITOR && UNITY_WEBGL
using Toast.Gamebase.Internal.Single.WebGL;
#else
using Toast.Gamebase.Internal.Single.Standalone;
#endif

using System.Collections.Generic;
using System;
using Toast.Gamebase.LitJson;
using System.Text;

namespace Toast.Gamebase.Internal
{
    public sealed class GamebaseImplementation
    {
        private const string INITIALIZE_WITH_CONFIGURATION = "INITIALIZE_WITH_CONFIGURATION";
        private const string INITIALIZE_WITH_INSPECTOR = "INITIALIZE_WITH_INSPECTOR";

        private string initializeType = INITIALIZE_WITH_CONFIGURATION;

        private static readonly GamebaseImplementation instance = new GamebaseImplementation();

        public static GamebaseImplementation Instance
        {
            get { return instance; }
        }

        private IGamebase sdk;

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
            GamebaseGameInformationReport.Instance.AddApiName();
            sdk.SetDebugMode(isDebugMode);
        }

        public void Initialize(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo> callback)
        {
            initializeType = INITIALIZE_WITH_INSPECTOR;

            Initialize(GetGamebaseConfiguration(), callback);
        }

        public void Initialize(GamebaseRequest.GamebaseConfiguration configuration, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo> callback)
        {
            if(initializeType.Equals(INITIALIZE_WITH_INSPECTOR) == true)
            {
                GamebaseGameInformationReport.Instance.AddApiName("InitializeWithInspector");
            }
            else
            {
                GamebaseGameInformationReport.Instance.AddApiName("InitializeWithConfiguration");
            }            

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Gamebase VERSION:{0}", GamebaseUnitySDK.SDKVersion));
            sb.AppendLine(string.Format("Gamebase Configuration:{0}", GamebaseJsonUtil.ToPrettyJsonString(configuration)));
            GamebaseLog.Debug(string.Format("{0}", sb), this);
            
            InitializeUnitySDK();
#pragma warning disable 0618
            configuration.zoneType = GetVerifiedZoneType(configuration.zoneType);
#pragma warning restore 0618
            SetGamebaseConfiguration(configuration);

            GamebaseCallback.GamebaseDelegate<LaunchingResponse.LaunchingInfo> initializeCallback = (launchingInfo, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    GamebaseDebugSettings.Instance.SetRemoteSettings(launchingInfo);

                    GamebaseWaterMark.ShowWaterMark();
                }

                GamebaseInternalReport.Instance.SetAppId(GamebaseUnitySDK.AppID);
                
                GamebaseGameInformationReport.Instance.SendGameInformation();

                if (Gamebase.IsSuccess(error) == true)
                {
                    string jsonString = JsonMapper.ToJson(launchingInfo);
                    callback(
                        JsonMapper.ToObject<GamebaseResponse.Launching.LaunchingInfo>(jsonString),
                        error);

                    return;
                }
                else
                {
                    callback(null, error);
                }
            };

            int handle = GamebaseCallbackHandler.RegisterCallback(initializeCallback);            
            sdk.Initialize(configuration, handle);
        }      
               
        public string GetSDKVersion()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return sdk.GetSDKVersion();
        }

        public string GetUserID()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return sdk.GetUserID();
        }

        public string GetAccessToken()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return sdk.GetAccessToken();
        }

        public string GetLastLoggedInProvider()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return sdk.GetLastLoggedInProvider();
        }
                        
        public string GetDeviceLanguageCode()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return sdk.GetDeviceLanguageCode();
        }

        public string GetCarrierCode()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return sdk.GetCarrierCode();
        }

        public string GetCarrierName()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return sdk.GetCarrierName();
        }

        public string GetCountryCode()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return sdk.GetCountryCode();
        }

        public string GetCountryCodeOfUSIM()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return sdk.GetCountryCodeOfUSIM();
        }

        public string GetCountryCodeOfDevice()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return sdk.GetCountryCodeOfDevice();
        }

        public bool IsSandbox()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            return sdk.IsSandbox();
        }

        public void SetDisplayLanguageCode(string languageCode)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            sdk.SetDisplayLanguageCode(languageCode);
        }
        
        public string GetDisplayLanguageCode()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
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
#pragma warning disable 0618
            configuration.zoneType                          = GamebaseUnitySDK.ZoneType;
#pragma warning restore 0618
            configuration.displayLanguageCode               = GamebaseUnitySDK.DisplayLanguageCode;
            configuration.enablePopup                       = GamebaseUnitySDK.EnablePopup;
            configuration.enableLaunchingStatusPopup        = GamebaseUnitySDK.EnableLaunchingStatusPopup;
            configuration.enableBanPopup                    = GamebaseUnitySDK.EnableBanPopup;
            configuration.enableKickoutPopup                = GamebaseUnitySDK.EnableKickoutPopup;
#pragma warning disable 0618
            configuration.fcmSenderId                       = GamebaseUnitySDK.FcmSenderId;
#pragma warning restore 0618
            configuration.storeCode                         = GamebaseUnitySDK.StoreCode;
            configuration.useWebViewLogin                   = GamebaseUnitySDK.UseWebViewLogin;
            return configuration;
        }

        private void SetGamebaseConfiguration(GamebaseRequest.GamebaseConfiguration configuration)
        {
            GamebaseUnitySDK.AppID                          = configuration.appID;
            GamebaseUnitySDK.AppVersion                     = configuration.appVersion;
#pragma warning disable 0618
            GamebaseUnitySDK.ZoneType                       = configuration.zoneType;
#pragma warning restore 0618
            GamebaseUnitySDK.DisplayLanguageCode            = configuration.displayLanguageCode;
            GamebaseUnitySDK.EnablePopup                    = configuration.enablePopup;
            GamebaseUnitySDK.EnableLaunchingStatusPopup     = configuration.enableLaunchingStatusPopup;
            GamebaseUnitySDK.EnableBanPopup                 = configuration.enableBanPopup;
            GamebaseUnitySDK.EnableKickoutPopup             = configuration.enableKickoutPopup;
#pragma warning disable 0618
            GamebaseUnitySDK.FcmSenderId                    = configuration.fcmSenderId;
#pragma warning restore 0618
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
            GamebaseGameInformationReport.Instance.AddApiName();
            GamebaseObserverManager.Instance.AddObserver(observer);
            sdk.AddObserver(GamebaseObserverManager.Instance.Handle);
        }

        public void RemoveObserver(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage> observer)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            GamebaseObserverManager.Instance.RemoveObserver(observer);
            sdk.RemoveObserver();
        }

        public void RemoveAllObserver()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            GamebaseObserverManager.Instance.RemoveAllObserver();
            sdk.RemoveAllObserver();
        }

        public void AddServerPushEvent(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage> serverPushEvent)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            GamebaseServerPushEventManager.Instance.AddServerPushEvent(serverPushEvent);
            sdk.AddServerPushEvent(GamebaseServerPushEventManager.Instance.Handle);
        }
        
        public void RemoveServerPushEvent(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage> serverPushEvent)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            GamebaseServerPushEventManager.Instance.RemoveServerPushEvent(serverPushEvent);
            sdk.RemoveServerPushEvent();
        }
        
        public void RemoveAllServerPushEvent()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            GamebaseServerPushEventManager.Instance.RemoveAllServerPushEvent();
            sdk.RemoveAllServerPushEvent();
        }

        public void AddEventHandler(GamebaseCallback.DataDelegate<GamebaseResponse.Event.GamebaseEventMessage> eventHandler)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            GamebaseEventHandlerManager.Instance.AddEventHandler(eventHandler);
            sdk.AddEventHandler(GamebaseEventHandlerManager.Instance.Handle);
        }

        public void RemoveEventHandler(GamebaseCallback.DataDelegate<GamebaseResponse.Event.GamebaseEventMessage> eventHandler)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            GamebaseEventHandlerManager.Instance.RemoveEventHandler(eventHandler);
            sdk.RemoveEventHandler();
        }

        public void RemoveAllEventHandler()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            GamebaseEventHandlerManager.Instance.RemoveAllEventHandler();
            sdk.RemoveAllEventHandler();
        }
    }
}