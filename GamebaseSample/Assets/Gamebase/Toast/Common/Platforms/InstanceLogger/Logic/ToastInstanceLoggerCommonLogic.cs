using System;
using System.Collections.Generic;
using Toast.Core;
using UnityEngine;

namespace Toast.Internal
{
    public static class ToastInstanceLoggerCommonLogic
    {
        public const string LOG_TYPE_NORMAL = "NORMAL";

        public static string AppKey { get; set; }
        public static ServiceZone ServiceZone = ServiceZone.REAL;
        public static Dictionary<string, string> Fields = new Dictionary<string, string>();
        public static string CollectorUrl = LogUrlConstants.COLLECTOR_REAL_URL;

        private static string GetCollectorURL(string uri)
        {
            // settins uri : v2
            return uri + "/" + LogVersion.VERSION + "/log";
        }

        public static void Initialize(ServiceZone serviceZone)
        {
            ServiceZone = serviceZone;

            if (ServiceZone == ServiceZone.ALPHA)
            {
                CollectorUrl = GetCollectorURL(LogUrlConstants.COLLECTOR_ALPHA_URL);
            }
            else if (ServiceZone == ServiceZone.BETA)
            {
                CollectorUrl = GetCollectorURL(LogUrlConstants.COLLECTOR_BETA_URL);
            }
            else if (ServiceZone == ServiceZone.REAL)
            {
                CollectorUrl = GetCollectorURL(LogUrlConstants.COLLECTOR_REAL_URL);
            }
            else
            {
                ToastLog.Error("20003 : ServiceZone is missing!! (LoggerServiceZone is a strange value!!)");
                return;
            }

            LogTransfer.Instance.StartSender();

#if UNITY_STANDALONE || UNITY_EDITOR
            BackupLogManager.RemoveOldFiles(AppKey);
#endif
        }

        public static void Log(string type, string logLevel, string message, Dictionary<string, string> userFields)
        {
            LogData logData = new LogData();

            string loggerType = LOG_TYPE_NORMAL;

            if (!string.IsNullOrEmpty(type))
            {
                loggerType = type;
            }

            logData.SetLogObject(AppKey, loggerType, (LogLevel)Enum.Parse(typeof(LogLevel), logLevel), message);

            if (!string.IsNullOrEmpty(ToastCoreCommonLogic.UserId))
            {
                logData.SetUserId(ToastCoreCommonLogic.UserId);
            }

            SetCommonData(logData);

            if (userFields != null)
            {
                logData.SetUserFields(userFields);
            }

            LogSendQueue.Instance.AddToastLoggerLogObject(logData);
        }

        private static void SetCommonData(LogObject logObject)
        {
            logObject.Put(LogFields.PROJECT_VERSION, Application.version);
            logObject.Put(LogFields.DEVICE_ID, ToastDeviceInfo.GetDeviceUniqueIdentifier());
            logObject.Put(LogFields.PLATFORM_NAME, ToastApplicationInfo.GetPlatformName());
            logObject.Put(LogFields.LAUNCHED_ID, ToastApplicationInfo.GetLaunchedId());
            logObject.Put(LogFields.SDK_VERSION, ToastApplicationInfo.GetSDKVersion());

            foreach (var item in Fields)
            {
                logObject.SetUserField(item.Key, item.Value);
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

            string convertedKey = LogFields.ConvertField(key);

            if (Fields.ContainsKey(convertedKey))
            {
                Fields.Remove(convertedKey);
            }

            Fields.Add(convertedKey, value);
        }
    }
}