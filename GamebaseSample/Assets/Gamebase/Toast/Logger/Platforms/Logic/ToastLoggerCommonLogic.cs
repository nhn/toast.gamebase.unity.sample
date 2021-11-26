using System;
using System.Collections.Generic;
using Toast.Core;
using Toast.Internal;
using UnityEngine;

namespace Toast.Logger
{
    public static class ToastLoggerCommonLogic
    {
        public static string AppKey = "";
        public static ToastServiceZone ServiceZone = ToastServiceZone.REAL;
        public static Dictionary<string, string> Fields = new Dictionary<string, string>();
        public static string CollectorUrl = ToastLoggerUrlConstants.COLLECTOR_REAL_URL;
        public static string SettingUrl = ToastLoggerUrlConstants.SETTINGS_REAL_URL;
        public static bool IsLoggerListener { get; set; }

        private static bool _isCreateSessionLog = false;

        private static string GetCollectorURL(string uri)
        {
            // settins uri : v2
            return uri + "/" + ToastLoggerVersion.VERSION + "/log";
        }

        private static string GetSettingsURL(string uri, string appKey)
        {
            // settins uri : v2
            return uri + "/" + ToastLoggerVersion.VERSION + "/" + appKey + "/logsconf";
        }

        public static void Initialize(ToastLoggerConfiguration loggerConfiguration)
        {
            AppKey = loggerConfiguration.AppKey;
            ServiceZone = loggerConfiguration.ServiceZone;

            if (ServiceZone == ToastServiceZone.ALPHA)
            {
                CollectorUrl = GetCollectorURL(ToastLoggerUrlConstants.COLLECTOR_ALPHA_URL);
                SettingUrl = GetSettingsURL(ToastLoggerUrlConstants.SETTINGS_ALPHA_URL, AppKey);
            }
            else if (ServiceZone == ToastServiceZone.BETA)
            {
                CollectorUrl = GetCollectorURL(ToastLoggerUrlConstants.COLLECTOR_BETA_URL);
                SettingUrl = GetSettingsURL(ToastLoggerUrlConstants.SETTINGS_BETA_URL, AppKey);
            }
            else if (ServiceZone == ToastServiceZone.REAL)
            {
                CollectorUrl = GetCollectorURL(ToastLoggerUrlConstants.COLLECTOR_REAL_URL);
                SettingUrl = GetSettingsURL(ToastLoggerUrlConstants.SETTINGS_REAL_URL, AppKey);
            }
            else
            {
                ToastLog.Error("20003 : ServiceZone is missing!! (LoggerServiceZone is a strange value!!)");
                return;
            }

            ToastLoggerSettings.Instance.LoadToastLoggerSettings(()=>
            {
                ToastLoggerLogSender.Instance.StartSender();
            });

            if (loggerConfiguration.EnableCrashReporter)
            {
                if (!_isCreateSessionLog)
                {
                    SendSessionData();
                    _isCreateSessionLog = true;
                }
            }
#if UNITY_STANDALONE || UNITY_EDITOR
            BackupLogManager.RemoveOldFiles(AppKey);
#endif
        }

        private static void SendSessionData()
        {
            ToastLoggerSessionData logData = new ToastLoggerSessionData();

            string loggerType = ToastLoggerType.SESSION;
            logData.SetLogObject(AppKey, loggerType, ToastLogLevel.NONE, "SESSION");

            SetCommonData(logData, loggerType);

            ToastLoggerSendQueue.Instance.AddToastLoggerLogObject(logData);
        }

        public static void Log(string logLevel, string message, Dictionary<string, string> userFields)
        {
            ToastLoggerLogData logData = new ToastLoggerLogData();

            string loggerType = ToastLoggerType.NORMAL;
            logData.SetLogObject(AppKey, loggerType, (ToastLogLevel)Enum.Parse(typeof(ToastLogLevel), logLevel), message);

            if (!string.IsNullOrEmpty(ToastCoreCommonLogic.UserId))
            {
                logData.SetUserId(ToastCoreCommonLogic.UserId);
            }

            SetCommonData(logData, loggerType);

            if (userFields != null)
            {
                logData.SetUserFields(userFields);
            }

            ToastLoggerSendQueue.Instance.AddToastLoggerLogObject(logData);
        }

        public static void Report(string logType, string logLevel, string message, string dumpData, Dictionary<string, string> userFields)
        {
            ToastLoggerCrashData crashData = new ToastLoggerCrashData();

            crashData.SetLogObject(AppKey, logType, (ToastLogLevel)Enum.Parse(typeof(ToastLogLevel), logLevel), message);
            crashData.SetCrashDump(dumpData);
            crashData.SetCrashStyle("unity-cs");
            crashData.SetCrashSymbol("none");
            crashData.SetLogSource("CrashDump");

            if (!string.IsNullOrEmpty(ToastCoreCommonLogic.UserId))
            {
                crashData.SetUserId(ToastCoreCommonLogic.UserId);
            }

            SetCommonData(crashData, logType);

            if (userFields != null)
            {
                crashData.SetUserFields(userFields);
            }

            ToastLoggerSendQueue.Instance.AddToastLoggerLogObject(crashData);
        }

        private static void SetCommonData(ToastLoggerLogObject logObject, string loggerType)
        {
            logObject.Put(LogFields.PROJECT_VERSION, Application.version);
            logObject.Put(LogFields.DEVICE_ID, ToastDeviceInfo.GetDeviceUniqueIdentifier());
            logObject.Put(LogFields.PLATFORM_NAME, ToastApplicationInfo.GetPlatformName());
            logObject.Put(LogFields.LAUNCHED_ID, ToastApplicationInfo.GetLaunchedId());
            logObject.Put(LogFields.SDK_VERSION, ToastApplicationInfo.GetSDKVersion());
            logObject.Put(LogFields.SESSION_ID, ToastApplicationInfo.GetSessionId());

            foreach (var item in Fields)
            {
                logObject.SetUserField(item.Key, item.Value);
            }

            if (ToastLoggerType.SESSION == loggerType
                || ToastLoggerType.CRASH == loggerType
                || ToastLoggerType.HANDLED == loggerType
                || ToastLoggerType.CRASH_FROM_INACTIVATED_STATE == loggerType
                || ToastLoggerType.CRASH_FROM_UNITY == loggerType)
            {
                logObject.Put(LogFields.DEVICE_MODEL, ToastApplicationInfo.GetDeviceModel());
                logObject.Put(LogFields.COUNTRY_CODE, ToastApplicationInfo.GetCountryCode());
            }
        }

        public static void SetUserField(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            if (value == null)
            {
                return;
            }

            string convertedKey = LogFields.ConvertField(key);

            if (Fields.ContainsKey(convertedKey))
            {
                Fields.Remove(convertedKey);
            }

            Fields.Add(convertedKey, value);
        }
    }
}