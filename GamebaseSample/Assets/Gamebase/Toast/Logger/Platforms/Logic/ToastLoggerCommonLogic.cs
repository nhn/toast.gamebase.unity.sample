using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast.Internal;
using Toast.Core;

namespace Toast.Logger
{
    public static class ToastLoggerCommonLogic
    {
        public static string AppKey = "";        
        public static ToastServiceZone ServiceZone = ToastServiceZone.REAL;
        public static Dictionary<string, string> Fileds = new Dictionary<string, string>();
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

            ToastLoggerSettings.Instance.LoadToastLoggerSettings(ServiceZone);            

            ToastLoggerLogSender.Instance.StartSender();

            if (loggerConfiguration.EnableCrashReporter)
            {
                if (!_isCreateSessionLog)
                {
                    SendSessionData();
                    _isCreateSessionLog = true;
                }
            }
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

            if (userFields != null)
            {
                logData.SetUserFields(userFields);
            }

            SetCommonData(logData, loggerType);

            ToastLoggerSendQueue.Instance.AddToastLoggerLogObject(logData);
        }

        public static void Report(string logLevel, string message, string dumpData, Dictionary<string, string> userFields)
        {
            ToastLoggerCrashData crashData = new ToastLoggerCrashData();

            string loggerType = ToastLoggerType.CRASH_FROM_UNITY;
            crashData.SetLogObject(AppKey, loggerType, (ToastLogLevel)Enum.Parse(typeof(ToastLogLevel), logLevel), message);
            crashData.SetCrashDump(dumpData);
            crashData.SetCrashStyle("unity-cs");
            crashData.SetCrashSymbol("none");
            crashData.SetLogSource("CrashDump");            

            if (!string.IsNullOrEmpty(ToastCoreCommonLogic.UserId))
            {
                crashData.SetUserId(ToastCoreCommonLogic.UserId);
            }

            if (userFields != null)
            {
                crashData.SetUserFields(userFields);
            }

            SetCommonData(crashData, loggerType);

            ToastLoggerSendQueue.Instance.AddToastLoggerLogObject(crashData);
        }

        private static void SetCommonData(ToastLoggerLogObject logObject, string loggerType)
        {
            logObject.Put(ToastLoggerFields.PROJECT_VERSION, Application.version);
            logObject.Put(ToastLoggerFields.DEVICE_ID, ToastDeviceInfo.GetDeviceUniqueIdentifier());
            logObject.Put(ToastLoggerFields.PLATFORM_NAME, ToastApplicationInfo.GetPlatformName());
            logObject.Put(ToastLoggerFields.LAUNCHED_ID, ToastApplicationInfo.GetLaunchedId());
            logObject.Put(ToastLoggerFields.SDK_VERSION, ToastApplicationInfo.GetSDKVersion());
            logObject.Put(ToastLoggerFields.SESSION_ID, ToastApplicationInfo.GetSessionId());

            foreach(var item in Fileds)
            {
                logObject.SetUserField(item.Key, item.Value);
            }

            if (ToastLoggerType.SESSION == loggerType
                || ToastLoggerType.CRASH == loggerType
                || ToastLoggerType.HANDLED == loggerType
                || ToastLoggerType.CRASH_FROM_INACTIVATED_STATE == loggerType
                || ToastLoggerType.CRASH_FROM_UNITY == loggerType)
            {
                logObject.Put(ToastLoggerFields.DEVICE_MODEL, ToastApplicationInfo.GetDeviceModel());
                logObject.Put(ToastLoggerFields.COUNTRY_CODE, ToastApplicationInfo.GetCountryCode());
            }
        }

        public static void SetUserField(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            string convertKey = ToastLoggerFields.ConvertField(key);

            if (Fileds.ContainsKey(convertKey))
            {
                ToastLog.Warn("The field's key already exists.");
            }
            else
            { 
                Fileds.Add(convertKey, value);
            }
        }
    }
}