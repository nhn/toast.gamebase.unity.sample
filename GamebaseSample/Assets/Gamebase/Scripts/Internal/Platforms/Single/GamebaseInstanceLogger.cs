using System.Collections.Generic;
using Toast.Logger;
using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public class GamebaseInstanceLogger : MonoBehaviour
    {
        private static readonly GamebaseInstanceLogger instance = new GamebaseInstanceLogger();

        public static GamebaseInstanceLogger Instance
        {
            get { return instance; }
        }

        private InstanceLogger logger;
        private bool isInitialized;

        public void Initialize(string appKey, string zone)
        {
            isInitialized = true;

            ToastServiceZone zoneType = ToastServiceZone.REAL;

            if (zone.ToLower().Equals("beta") == true)
            {
                zoneType = ToastServiceZone.BETA;
            }
            else if (zone.ToLower().Equals("alpha") == true)
            {
                zoneType = ToastServiceZone.ALPHA;
            }

            logger = new InstanceLogger(appKey, zoneType);
        }

        public void Debug(string logType, string message, IDictionary<string, string> userFields = null)
        {
            if (isInitialized == false)
            {
                GamebaseLog.Error("InstanceLogger not initialized", this);
                return;
            }

            logger.Debug(logType, message, MakeFields(userFields));
        }

        public void Info(string logType, string message, IDictionary<string, string> userFields = null)
        {
            if (isInitialized == false)
            {
                GamebaseLog.Error("InstanceLogger not initialized", this);
                return;
            }

            logger.Info(logType, message, MakeFields(userFields));
        }

        public void Warn(string logType, string message, IDictionary<string, string> userFields = null)
        {
            if (isInitialized == false)
            {
                GamebaseLog.Error("InstanceLogger not initialized", this);
                return;
            }

            logger.Warn(logType, message, MakeFields(userFields));
        }

        public void Error(string logType, string message, IDictionary<string, string> userFields = null)
        {
            if (isInitialized == false)
            {
                GamebaseLog.Error("InstanceLogger not initialized", this);
                return;
            }

            logger.Error(logType, message, MakeFields(userFields));
        }

        public void Fatal(string logType, string message, IDictionary<string, string> userFields = null)
        {
            if (isInitialized == false)
            {
                GamebaseLog.Error("InstanceLogger not initialized", this);
                return;
            }

            logger.Fatal(logType, message, MakeFields(userFields));
        }

        public Dictionary<string, string> MakeFields(IDictionary<string, string> userFields)
        {
            return new Dictionary<string, string>(userFields)
            {
                {"GBPlatform", GamebaseUnitySDK.Platform},
                {"GBProjectAppID", GamebaseUnitySDK.AppID},
                {"GBAppClientVersion", GamebaseUnitySDK.AppVersion},
                {"GBLaunchingZone", GamebaseUnitySDK.ZoneType.ToLower()},
                {"GBUnitySDKVersion", GamebaseUnitySDK.SDKVersion},
                {"GBNativeSDKVersion", Gamebase.GetSDKVersion()},
                {"GBServerAPIVersion", ""},
                {"GBServerStaticsStoreCode", GamebaseUnitySDK.StoreCode},
                {"GBInternalReportVersion", "v1"},
                {"GBLastLoggedInIDP", Gamebase.GetLastLoggedInProvider()},
                {"GBGuestUUID", GamebaseUnitySDK.UUID},            
                {"GBDeviceLanguageCode", Gamebase.GetDeviceLanguageCode()},
                {"GBDisplayLanguageCode", Gamebase.GetDisplayLanguageCode()},
                {"GBCountryCodeUSIM", Gamebase.GetCountryCodeOfUSIM()},
                {"GBCountryCodeDevice", Gamebase.GetCountryCodeOfDevice()},
                {"GBNetworkType", Gamebase.Network.GetNetworkTypeName()},
            };
        }
    }
}