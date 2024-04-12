using System;
using Toast.Gamebase.Internal;
using UnityEngine;
using UnityEngine.Serialization;

namespace Toast.Gamebase
{
    [Obsolete("Inspector settings are no longer supported, check out our initialization guide. " +
              "(https://docs.nhncloud.com/en/Game/Gamebase/en/unity-initialization)")]
    public class GamebaseUnitySDKSettings : MonoBehaviour
    {
        public string   appID                       = string.Empty;
        public string   appVersion                  = string.Empty;
        public bool     isDebugMode                 = false;
        
        public string   displayLanguageCode         = string.Empty;
        public string   zoneType                    = string.Empty;

        public bool     enablePopup                 = false;
        public bool     enableLaunchingStatusPopup  = true;
        public bool     enableBanPopup              = true;

        public string   storeCodeIOS                = string.Empty;        
        public string   storeCodeAndroid            = string.Empty;
        public string   storeCodeWebGL              = string.Empty;
        [FormerlySerializedAs("storeCodeStandalone")]
        public string   storeCodeStandaloneWindows  = string.Empty;
        public string   storeCodeStandaloneMacOS    = string.Empty;

        public string   fcmSenderId                 = string.Empty;
        
        public bool     useWebViewLogin             = false;

        void Awake()
        {
            GamebaseUnitySDK.AppID = appID;
            GamebaseUnitySDK.AppVersion = appVersion;
            GamebaseUnitySDK.DisplayLanguageCode = displayLanguageCode;
            GamebaseUnitySDK.ZoneType = (string.IsNullOrEmpty(zoneType)) ? "real" : zoneType.ToLower();
            GamebaseUnitySDK.EnablePopup = enablePopup;
            GamebaseUnitySDK.EnableLaunchingStatusPopup = enableLaunchingStatusPopup;
            GamebaseUnitySDK.EnableBanPopup = enableBanPopup;
            GamebaseUnitySDK.FcmSenderId = fcmSenderId;

#if UNITY_ANDROID
            GamebaseUnitySDK.StoreCode = storeCodeAndroid;
#elif UNITY_IOS
            GamebaseUnitySDK.StoreCode = storeCodeIOS;
#elif UNITY_WEBGL
            GamebaseUnitySDK.StoreCode = storeCodeWebGL;
#elif UNITY_STANDALONE_WIN
            GamebaseUnitySDK.StoreCode = storeCodeStandaloneWindows;
#elif UNITY_STANDALONE_OSX
            GamebaseUnitySDK.StoreCode = storeCodeStandaloneMacOS;
#else
            GamebaseUnitySDK.StoreCode = storeCodeStandalone;
#endif

            GamebaseUnitySDK.StoreCode = GamebaseUnitySDK.StoreCode.ToUpper();

            GamebaseUnitySDK.UseWebViewLogin = useWebViewLogin;
        }

        private void Start()
        {
            GamebaseImplementation.Instance.SetDebugMode(isDebugMode);
        }
    }
}