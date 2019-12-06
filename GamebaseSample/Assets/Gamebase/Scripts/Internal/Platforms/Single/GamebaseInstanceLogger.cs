using System.Collections.Generic;
using Toast.Logger;
using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public class GamebaseInstanceLogger : MonoBehaviour
    {
#region Log Message
        // Init
        public const string GB_INIT_FAILED_MULTIPLE_TIMES = "GB_INIT_FAILED_MULTIPLE_TIMES";
        // Auth 
        public const string GB_AUTH_LAST_PROVIDER_LOGIN_SUCCESS = "GB_AUTH_LAST_PROVIDER_LOGIN_SUCCESS";
        public const string GB_AUTH_LAST_PROVIDER_LOGIN_FAILED = "GB_AUTH_LAST_PROVIDER_LOGIN_FAILED";
        public const string GB_AUTH_LOGIN_SUCCESS = "GB_AUTH_LOGIN_SUCCESS";
        public const string GB_AUTH_LOGIN_FAILED = "GB_AUTH_LOGIN_FAILED";
        public const string GB_AUTH_MAPPING_SUCCESS = "GB_AUTH_MAPPING_SUCCESS";
        public const string GB_AUTH_MAPPING_FAILED = "GB_AUTH_MAPPING_FAILED";
        public const string GB_AUTH_LOGOUT_SUCCESS = "GB_AUTH_LOGOUT_SUCCESS";
        public const string GB_AUTH_LOGOUT_FAILED = "GB_AUTH_LOGOUT_FAILED";
        public const string GB_AUTH_WITHDRAW_SUCCESS = "GB_AUTH_WITHDRAW_SUCCESS";
        public const string GB_AUTH_WITHDRAW_FAILED = "GB_AUTH_WITHDRAW_FAILED";
        public const string GB_AUTH_ISSUE_TRANSFER_SUCCESS = "GB_AUTH_ISSUE_TRANSFER_SUCCESS";
        public const string GB_AUTH_ISSUE_TRANSFER_FAILED = "GB_AUTH_ISSUE_TRANSFER_FAILED";
        public const string GB_AUTH_QUERY_TRANSFER_SUCCESS = "GB_AUTH_QUERY_TRANSFER_SUCCESS";
        public const string GB_AUTH_QUERY_TRANSFER_FAILED = "GB_AUTH_QUERY_TRANSFER_FAILED";
        public const string GB_AUTH_RENEW_TRANSFER_SUCCESS = "GB_AUTH_RENEW_TRANSFER_SUCCESS";
        public const string GB_AUTH_RENEW_TRANSFER_FAILED = "GB_AUTH_RENEW_TRANSFER_FAILED";
        public const string GB_AUTH_TRANSFER_ACCOUNT_SUCCESS = "GB_AUTH_TRANSFER_ACCOUNT_SUCCESS";
        public const string GB_AUTH_TRANSFER_ACCOUNT_FAILED = "GB_AUTH_TRANSFER_ACCOUNT_FAILED";
        // Event
        public const string GB_EVENT_OBSERVER_BANNED_MEMBER = "GB_EVENT_OBSERVER_BANNED_MEMBER";
        public const string GB_EVENT_SERVERPUSH_TRANSFER_KICKOUT = "GB_EVENT_SERVERPUSH_TRANSFER_KICKOUT";
        //Purchase
        public const string GB_IAP_PURCHASE_SUCCESS = "GB_IAP_PURCHASE_SUCCESS";
        public const string GB_IAP_PURCHASE_FAILED = "GB_IAP_PURCHASE_FAILED";
        public const string GB_IAP_SET_UPDATE_LISTENER_BEFORE_LOGIN = "GB_IAP_SET_UPDATE_LISTENER_BEFORE_LOGIN";
        public const string GB_IAP_UPDATE_LISTENER_NOT_REGISTERED = "GB_IAP_UPDATE_LISTENER_NOT_REGISTERED";
        public const string GB_IAP_WRONG_CHANGED_STORECODE = "GB_IAP_WRONG_CHANGED_STORECODE";
        //Push 
        public const string GB_PUSH_REGISTER_SUCCESS = "GB_PUSH_REGISTER_SUCCESS";
        public const string GB_PUSH_REGISTER_FAILED = "GB_PUSH_REGISTER_FAILED";
        public const string GB_PUSH_REGISTER_QUERY_FAILED = "GB_PUSH_REGISTER_QUERY_FAILED";
        public const string GB_PUSH_UNREGISTER_SUCCESS = "GB_PUSH_UNREGISTER_SUCCESS";
        public const string GB_PUSH_UNREGISTER_FAILED = "GB_PUSH_UNREGISTER_FAILED";
        //Common 
        public const string GB_COMMON_WRONG_USAGE = "GB_COMMON_WRONG_USAGE";
#endregion

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