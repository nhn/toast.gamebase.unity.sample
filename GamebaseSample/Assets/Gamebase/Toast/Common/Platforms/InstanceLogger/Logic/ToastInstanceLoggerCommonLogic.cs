using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast.Core;

namespace Toast.Internal
{
    public static class ToastInstanceLoggerCommonLogic
    {
        public const string LOG_TYPE_NORMAL = "NORMAL";

        public static string AppKey { get; set; }
        public static ServiceZone ServiceZone = ServiceZone.REAL;
        public static Dictionary<string, string> Fileds = new Dictionary<string, string>();
        public static string CollectorUrl = LogUrlConstants.COLLECTOR_REAL_URL;        

        private static string GetCollectorURL(string uri)
        {
            // settins uri : v2
            return uri + "/" + LogVersion.VERSION + "/log";
        }

        private static string GetSettingsURL(string uri, string appKey)
        {
            // settins uri : v2
            return uri + "/" + LogVersion.VERSION + "/" + appKey + "/logsconf";
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

            if (userFields != null)
            {
                logData.SetUserFields(userFields);
            }

            SetCommonData(logData, loggerType);

            LogSendQueue.Instance.AddToastLoggerLogObject(logData);
        }

        private static void SetCommonData(LogObject logObject, string loggerType)
        {
            logObject.Put(LogFields.PROJECT_VERSION, Application.version);
            logObject.Put(LogFields.DEVICE_ID, ToastDeviceInfo.GetDeviceUniqueIdentifier());
            logObject.Put(LogFields.PLATFORM_NAME, ToastApplicationInfo.GetPlatformName());
            logObject.Put(LogFields.LAUNCHED_ID, ToastApplicationInfo.GetLaunchedId());
            logObject.Put(LogFields.SDK_VERSION, ToastApplicationInfo.GetSDKVersion());
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

            Fileds.Add(key, value);
        }
    }
}