#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using GamePlatform.Logger;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Toast.Gamebase.Internal.Single;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public sealed class GamebaseIndicatorReport
    {
        public enum LogLevel
        {
            DEBUG,
            INFO,
            WARN,
            ERROR,
            FATAL,
            NONE,
        }

        private static class Platform
        {
            public const string WINDOWS = "WINDOWS";
            public const string WEB = "WEB";
            public const string IOS = "IOS";
            public const string ANDROID = "ANDROID";
        }

        public const string GAME_ENGINE = "UNITY";

        #region Indicator key
        public const string GB_GAME_NAME = "GBGameName";
        public const string GB_STABILITY_CODE = "GBStabilityCode";
        public const string GB_PLATFORM = "GBPlatform";
        public const string GB_PROJECT_APP_ID = "GBProjectAppID";
        public const string GB_SUB_CATEGORY1 = "GBSubCategory1";
        public const string GB_APP_CLIENT_VERSION = "GBAppClientVersion";
        public const string GB_LAUNCHING_ZONE = "GBLaunchingZone";
        public const string GB_UNITY_SDK_VERSION = "GBUnitySDKVersion";
        public const string GB_SERVER_API_VERSION = "GBServerAPIVersion";
        public const string GB_ONGAME_SERVER_API_VERSION = "GBONGAMEServerApiVersion";
        public const string GB_INTERNAL_REPORT_VERSION = "GBInternalReportVersion";
        public const string GB_LAST_LOGGEDIN_IDP = "GBLastLoggedInIDP";
        public const string GB_LAST_LOGGEDIN_USER_ID = "GBLastLoggedInUserID";
        public const string GB_GUEST_UUID = "GBGuestUUID";        
        public const string GB_DEVICE_LANGUAGE_CODE = "GBDeviceLanguageCode";
        public const string GB_DISPLAY_LANGUAGE_CODE = "GBDisplayLanguageCode";
        public const string GB_COUNTRY_CODE_USIM = "GBCountryCodeUSIM";
        public const string GB_COUNTRY_CODE_DEVICE = "GBCountryCodeDevice";
        public const string GB_NETWORK_TYPE = "GBNetworkType";
        public const string GB_GAME_ENGINE = "GBGameEngine";
        public const string GB_CARRIER = "GBCarrier";
        public const string GB_DEVICE_MODEL = "GBDeviceModel";



        public const string GB_EXCEPTION = "txtGBException";
        public const string GB_ERROR_CODE = "GBErrorCode";
        public const string GB_ERROR_DOMAIN = "GBErrorDomain";
        public const string GB_DETAIL_ERROR_CODE = "GBDetailErrorCode";
        public const string GB_DETAIL_ERROR_MESSAGE = "GBDetailErrorMessage";
        public const string GB_IS_USER_CANCELED = "GBIsUserCanceled";
        public const string GB_IS_EXTERNAL_LIBRARY_ERROR = "GBIsExternalLibraryError";
        #endregion
        
        public const string KEY_INDICATOR_STABILITY = "KEY_INDICATOR_STABILITY";

        public const string KEY_LAST_LOGGEDIN_IDP = "KEY_LAST_LOGGEDIN_IDP";
        public const string KEY_LAST_LOGGEDIN_USERID = "KEY_LAST_LOGGEDIN_USERID";

        private const int APP_KEY_VERSION = 1;
        private const int INIT_FAIL_COUNT = 5;

        public static LaunchingResponse.LaunchingInfo.Launching.TCGBClient.Stability stability = new LaunchingResponse.LaunchingInfo.Launching.TCGBClient.Stability();
        
        public static Dictionary<string, string> basicDataDictionary;

        private static string platform = string.Empty;

        public static void Initialize(LaunchingResponse.LaunchingInfo.Launching.TCGBClient.Stability stability, System.Action callback)
        {

#if UNITY_ANDROID
            platform = Platform.ANDROID;
#elif UNITY_IOS
            platform = Platform.IOS;
#elif UNITY_WEBGL
            platform = Platform.WEB;
#elif UNITY_STANDALONE
            platform = Platform.WINDOWS;
#endif

            string filePath = Path.Combine(Application.streamingAssetsPath, "Gamebase/defaultstability.json");

            GamebaseStringLoader localFileLoader = new GamebaseStringLoader();
            localFileLoader.LoadStringFromFile(filePath, (jsonString) => 
            {
                if (string.IsNullOrEmpty(jsonString) == false)
                {
                    jsonString = GamebaseEncryptUtilHelper.DecryptDefaultStabilityEncryptedKey(jsonString);
                    Dictionary<string, LaunchingResponse.LaunchingInfo.Launching.TCGBClient.Stability> stabilityDictionary = JsonMapper.ToObject<Dictionary<string, LaunchingResponse.LaunchingInfo.Launching.TCGBClient.Stability>>(jsonString);
                    if (stabilityDictionary != null)
                    {
                        if(string.IsNullOrEmpty(GamebaseUnitySDK.ZoneType) == true)
                        {
                            GamebaseUnitySDK.ZoneType = "real";
                        }

                        GamebaseIndicatorReport.stability = stabilityDictionary[GamebaseUnitySDK.ZoneType.ToLower()];
                    }
                }

                CreateBaseData();
                SetStability(stability);
                CreateInstanceLogger();

                callback();

                GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.CORE_TYPE, SendIndicatorInQueue());
            });           
        }

        public static void SetLastLoggedInInfo(string idP, string userId)
        {
            Dictionary<string, string> lastLoggedInInfoDictionary = new Dictionary<string, string>();
            lastLoggedInInfoDictionary.Add(GB_LAST_LOGGEDIN_IDP, idP);
            lastLoggedInInfoDictionary.Add(GB_LAST_LOGGEDIN_USER_ID, userId);

            MergeDictionary(ref basicDataDictionary, lastLoggedInInfoDictionary);
        }

        private static Queue<IndicatorItem> queue = new Queue<IndicatorItem>();

        public class IndicatorItem
        {
            public string logType;
            public string stabilityCode;
            public string logLevel;
            public Dictionary<string, string> customFields;
            public GamebaseError error = null;
            public bool isUserCanceled = false;
            public bool isExternalLibraryError = false;
        }

        public static void AddIndicatorItem(IndicatorItem item)
        {
            queue.Enqueue(item);
        }

        private static IEnumerator SendIndicatorInQueue()
        {
            IndicatorItem item;

            while (true)
            {
                if (queue.Count > 0)
                {
                    item = queue.Dequeue();
                    SendIndicatorData(
                        item.logType,
                        item.stabilityCode,
                        item.logLevel,
                        item.customFields,
                        item.error,
                        item.isUserCanceled,
                        item.isExternalLibraryError);
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        public static void SendIndicatorData(
            string logType,
            string stabilityCode,
            string logLevel,
            Dictionary<string, string> customFields,
            GamebaseError error = null, 
            bool isUserCanceled = false, 
            bool isExternalLibraryError = false)
        {
            if (stability.useFlag == false)
            {
                return;
            }            

            if(ConvertLogLevelToEnum(stability.logLevel) > ConvertLogLevelToEnum(logLevel))
            {
                return;
            }

            Dictionary<string, string> userFields =  MakeindicatorDictionary(stabilityCode, customFields, error, isUserCanceled, isExternalLibraryError);


            GpLogLevel level = (GpLogLevel)ConvertLogLevelToEnum(logLevel);

            switch (level)
            {
                case GpLogLevel.DEBUG:
                    {
                        GpLogger.Debug(stability.appKey, stabilityCode, userFields, logType);
                        break;
                    }
                case GpLogLevel.INFO:
                    {
                        GpLogger.Info(stability.appKey, stabilityCode, userFields, logType);
                        break;
                    }
                case GpLogLevel.WARN:
                    {
                        GpLogger.Warn(stability.appKey, stabilityCode, userFields, logType);
                        break;
                    }
                case GpLogLevel.ERROR:
                    {
                        GpLogger.Error(stability.appKey, stabilityCode, userFields, logType);
                        break;
                    }
                case GpLogLevel.FATAL:
                    {
                        GpLogger.Fatal(stability.appKey, stabilityCode, userFields, logType);
                        break;
                    }
            }
        }

        public static LogLevel GetLogLevel()
        {
            return ConvertLogLevelToEnum(stability.logLevel);
        }

        private static Dictionary<string, string> MakeindicatorDictionary(
            string stabilityCode,
            Dictionary<string, string> customFields,
            GamebaseError error,
            bool isUserCanceled,
            bool isExternalLibraryError)
        {
            Dictionary<string, string> indicatorDictionary = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(stabilityCode) == false)
            {
                indicatorDictionary.Add(GB_STABILITY_CODE, stabilityCode);
            }

            if (error != null)
            {
                indicatorDictionary.Add(GB_EXCEPTION, error.ToString());

                indicatorDictionary.Add(GB_ERROR_CODE, error.code.ToString());
                indicatorDictionary.Add(GB_ERROR_DOMAIN, error.domain);

                if (error.error != null)
                {
                    indicatorDictionary.Add(GB_DETAIL_ERROR_CODE, error.error.code.ToString());
                    indicatorDictionary.Add(GB_DETAIL_ERROR_MESSAGE, error.error.message);
                }
            }

            if (isUserCanceled == true)
            {
                indicatorDictionary.Add(GB_IS_USER_CANCELED, "Y");
            }

            if (isExternalLibraryError == true)
            {
                indicatorDictionary.Add(GB_IS_EXTERNAL_LIBRARY_ERROR, "Y");
            }

            MergeDictionary(ref indicatorDictionary, basicDataDictionary);
            MergeDictionary(ref indicatorDictionary, customFields);

            return indicatorDictionary;
        }

        private static void SetStability(LaunchingResponse.LaunchingInfo.Launching.TCGBClient.Stability stability)
        {
            string stabilityKey = string.Format(
                "{0}_{1}",
                KEY_INDICATOR_STABILITY,
                GamebaseUnitySDK.ZoneType.ToString().ToUpper());

            if (stability != null)
            {
                GamebaseIndicatorReport.stability = stability;
                PlayerPrefs.SetString(
                    stabilityKey, 
                    JsonMapper.ToJson(stability));
                GamebaseLog.Debug("Launching Stability", typeof(GamebaseIndicatorReport));
            }
            else
            {
                if(PlayerPrefs.HasKey(stabilityKey) == true)
                {
                    LaunchingResponse.LaunchingInfo.Launching.TCGBClient.Stability stabilityPreference = JsonMapper.ToObject<LaunchingResponse.LaunchingInfo.Launching.TCGBClient.Stability>(PlayerPrefs.GetString(stabilityKey));
                    if(stabilityPreference.appKeyVersion >= GamebaseIndicatorReport.stability.appKeyVersion)
                    {
                        GamebaseIndicatorReport.stability = stabilityPreference;
                        GamebaseLog.Debug("Preference Stability", typeof(GamebaseIndicatorReport));
                    }
                    else
                    {
                        PlayerPrefs.SetString(
                            stabilityKey,
                            JsonMapper.ToJson(GamebaseIndicatorReport.stability));
                        GamebaseLog.Debug("Default Stability", typeof(GamebaseIndicatorReport));
                    }
                }
                else
                {
                    PlayerPrefs.SetString(
                            stabilityKey,
                            JsonMapper.ToJson(GamebaseIndicatorReport.stability));
                    GamebaseLog.Debug("Default Stability", typeof(GamebaseIndicatorReport));
                }
            }

            MergeDictionary(
                ref basicDataDictionary,
                new Dictionary<string, string>()
                {
                    { GB_INTERNAL_REPORT_VERSION, GamebaseIndicatorReport.stability.appKeyVersion.ToString() }
                });
        }

        private static void CreateInstanceLogger()
        {
            GamePlatform.Logger.ServiceZone zone;
            switch (GamebaseUnitySDK.ZoneType.ToLower())
            {
                case "alpha":
                    {
                        zone = GamePlatform.Logger.ServiceZone.ALPHA;
                        break;
                    }
                case "beta":
                    {
                        zone = GamePlatform.Logger.ServiceZone.ALPHA;
                        break;
                    }
                default:
                    {
                        zone = GamePlatform.Logger.ServiceZone.REAL;
                        break;
                    }
            }

            var param = new GpLoggerParams.Initialization(stability.appKey)
            {
                serviceZone = zone
            };

            GpLogger.Initialize(param, false);

        }

        private static void CreateBaseData()
        {
            basicDataDictionary = new Dictionary<string, string>()
            {
                { GB_GAME_NAME, Application.productName },
                { GB_PLATFORM, platform },
                { GB_PROJECT_APP_ID, GamebaseUnitySDK.AppID },
                { GB_APP_CLIENT_VERSION, GamebaseUnitySDK.AppVersion },
                { GB_LAUNCHING_ZONE, GamebaseUnitySDK.ZoneType.ToUpper() },
                { GB_UNITY_SDK_VERSION, Gamebase.GetSDKVersion() },
                { GB_SERVER_API_VERSION, Lighthouse.API.VERSION },
                { GB_LAST_LOGGEDIN_IDP, ""},
                { GB_LAST_LOGGEDIN_USER_ID, ""},
                { GB_GUEST_UUID, GamebaseSystemInfo.UUID },
                { GB_DEVICE_LANGUAGE_CODE, Gamebase.GetDeviceLanguageCode() },
                { GB_DISPLAY_LANGUAGE_CODE, Gamebase.GetDisplayLanguageCode() },
                { GB_COUNTRY_CODE_USIM, string.Empty },
                { GB_COUNTRY_CODE_DEVICE, Gamebase.GetCountryCodeOfDevice() },
                { GB_NETWORK_TYPE, Gamebase.Network.GetNetworkType().ToString().Replace("TYPE_","") },
                { GB_GAME_ENGINE, GAME_ENGINE },
                { GB_CARRIER, "NONE" },
                { GB_DEVICE_MODEL, SystemInfo.deviceModel }
            };
            
            if (PlayerPrefs.HasKey(KEY_LAST_LOGGEDIN_IDP) == true)
            {
                string idPPreference = PlayerPrefs.GetString(KEY_LAST_LOGGEDIN_IDP);
                basicDataDictionary.Add(GB_LAST_LOGGEDIN_IDP, idPPreference);
                
            }

            if (PlayerPrefs.HasKey(KEY_LAST_LOGGEDIN_USERID) == true)
            {
                string userIdPreference = PlayerPrefs.GetString(KEY_LAST_LOGGEDIN_USERID);
                basicDataDictionary.Add(GB_LAST_LOGGEDIN_USER_ID, userIdPreference);
            }
        }

        private static void MergeDictionary(ref Dictionary<string, string> originalData, Dictionary<string, string> additionalData)
        {
            if (additionalData == null || additionalData.Count == 0)
            {
                return;
            }

            if (originalData == null)
            {
                originalData = new Dictionary<string, string>();
            }

            foreach (string key in additionalData.Keys)
            {
                if (originalData.ContainsKey(key) == false)
                {
                    originalData.Add(key, additionalData[key]);
                }
                else
                {
                    originalData[key] = additionalData[key];
                }
            }
        }

        private static LogLevel ConvertLogLevelToEnum(string logLevel)
        {
            switch(logLevel)
            {
                case GamebaseIndicatorReportType.LogLevel.DEBUG:
                    {
                        return LogLevel.DEBUG;
                    }
                case GamebaseIndicatorReportType.LogLevel.INFO:
                    {
                        return LogLevel.INFO;
                    }
                case GamebaseIndicatorReportType.LogLevel.WARN:
                    {
                        return LogLevel.WARN;
                    }
                case GamebaseIndicatorReportType.LogLevel.ERROR:
                    {
                        return LogLevel.ERROR;
                    }
                case GamebaseIndicatorReportType.LogLevel.FATAL:
                    {
                        return LogLevel.FATAL;
                    }
                case GamebaseIndicatorReportType.LogLevel.NONE:
                    {
                        return LogLevel.NONE;
                    }
                default:
                    {
                        return LogLevel.DEBUG;
                    }
            }
        }
    }
}
#endif
