using GamePlatform.Logger;
using System.Collections.Generic;
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

        private bool isInitialized;

        private string appKey;

        public void Initialize(string appKey, string zone)
        {
            isInitialized = true;
            this.appKey = appKey;

            GamePlatform.Logger.ServiceZone zoneType = GamePlatform.Logger.ServiceZone.REAL;

            if (zone.ToLower().Equals("beta") == true)
            {
                zoneType = GamePlatform.Logger.ServiceZone.ALPHA;
            }
            else if (zone.ToLower().Equals("alpha") == true)
            {
                zoneType = GamePlatform.Logger.ServiceZone.ALPHA;
            }

            var param = new GpLoggerParams.Initialization(appKey)
            {
                serviceZone = zoneType
            };

            GpLogger.Initialize(param, false);
        }

        public void Debug(string logType, string message, IDictionary<string, string> userFields = null)
        {
            if (isInitialized == false)
            {
                GamebaseLog.Error("InstanceLogger not initialized", this);
                return;
            }

            GpLogger.Debug(appKey, message, MakeFields(userFields), logType);
        }

        public void Info(string logType, string message, IDictionary<string, string> userFields = null)
        {
            if (isInitialized == false)
            {
                GamebaseLog.Error("InstanceLogger not initialized", this);
                return;
            }

            GpLogger.Info(appKey, message, MakeFields(userFields), logType);
        }

        public void Warn(string logType, string message, IDictionary<string, string> userFields = null)
        {
            if (isInitialized == false)
            {
                GamebaseLog.Error("InstanceLogger not initialized", this);
                return;
            }

            GpLogger.Warn(appKey, message, MakeFields(userFields), logType);
        }

        public void Error(string logType, string message, IDictionary<string, string> userFields = null)
        {
            if (isInitialized == false)
            {
                GamebaseLog.Error("InstanceLogger not initialized", this);
                return;
            }

            GpLogger.Error(appKey, message, MakeFields(userFields), logType);
        }

        public void Fatal(string logType, string message, IDictionary<string, string> userFields = null)
        {
            if (isInitialized == false)
            {
                GamebaseLog.Error("InstanceLogger not initialized", this);
                return;
            }

            GpLogger.Fatal(appKey, message, MakeFields(userFields), logType);
        }

        public Dictionary<string, string> MakeFields(IDictionary<string, string> userFields)
        {
            return new Dictionary<string, string>(userFields)
            {
                {"GBPlatform", GamebaseSystemInfo.Platform},
                {"GBProjectAppID", GamebaseUnitySDK.AppID},
                {"GBAppClientVersion", GamebaseUnitySDK.AppVersion},
                {"GBLaunchingZone", GamebaseUnitySDK.ZoneType.ToLower()},
                {"GBUnitySDKVersion", GamebaseUnitySDK.SDKVersion},
                {"GBNativeSDKVersion", Gamebase.GetSDKVersion()},
                {"GBServerAPIVersion", ""},
                {"GBServerStaticsStoreCode", GamebaseUnitySDK.StoreCode},
                {"GBInternalReportVersion", "v1"},
                {"GBLastLoggedInIDP", Gamebase.GetLastLoggedInProvider()},
                {"GBGuestUUID", GamebaseSystemInfo.UUID},            
                {"GBDeviceLanguageCode", Gamebase.GetDeviceLanguageCode()},
                {"GBDisplayLanguageCode", Gamebase.GetDisplayLanguageCode()},
                {"GBCountryCodeUSIM", Gamebase.GetCountryCodeOfUSIM()},
                {"GBCountryCodeDevice", Gamebase.GetCountryCodeOfDevice()},
                {"GBNetworkType", Gamebase.Network.GetNetworkTypeName()},
            };
        }
    }
}