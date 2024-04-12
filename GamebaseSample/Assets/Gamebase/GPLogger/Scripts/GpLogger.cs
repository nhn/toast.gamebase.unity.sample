using GamePlatform.Logger.Internal;
using GamePlatform.Logger.ThirdParty;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GamePlatform.Logger
{
    public static class GpLogger
    {
        public const string VERSION = "1.2.0";

        public delegate void CrashListener(bool isSuccess, LogEntry logEntry);
        public delegate bool CrashFilter(CrashLogData logData);
        public static string UserId { get; set; }
        
        static GpLogger()
        {
            InitilizeJson();
        }

        public static bool DebugMode
        {
            get
            {
                return (GpLog.Level < GpLog.LogLevel.WARN) ? true : false;
            }
            set
            {
                if (value == true)
                {
                    GpLog.Level = GpLog.LogLevel.DEBUG;
                }
                else
                {
                    GpLog.Level = GpLog.LogLevel.WARN;
                }
            }
        }

        public static void Initialize(GpLoggerParams.Initialization param, bool isUserAccess)
        {
            if (IsValidAppKey(param.appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.CreateLoggerInstance(param.appKey, isUserAccess).Initialize(param);
        }

        public static void SetLoggerListener(string appKey, IGpLoggerListener listener)
        {
            if (HasLoggerInstance(appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.GetLoggerInstance(appKey).SetLoggerListener(listener);
        }

        public static void SetCrashListener(string appKey, CrashListener listener)
        {
            if (HasLoggerInstance(appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.GetLoggerInstance(appKey).SetCrashListener(listener);
        }

        public static void SetUserField(string appKey, string key, string value)
        {
            if (HasLoggerInstance(appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.GetLoggerInstance(appKey).SetUserField(key, value);
        }

        public static void AddCrashFilter(string appKey, CrashFilter filter)
        {
            if (HasLoggerInstance(appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.GetLoggerInstance(appKey).AddCrashFilter(filter);
        }

        public static void RemoveCrashFilter(string appKey, CrashFilter filter)
        {
            if (HasLoggerInstance(appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.GetLoggerInstance(appKey).RemoveCrashFilter(filter);
        }

        public static void Debug(string appKey, string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            if (HasLoggerInstance(appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.GetLoggerInstance(appKey).Debug(message, userFields, logType);
        }

        public static void Info(string appKey, string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            if (HasLoggerInstance(appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.GetLoggerInstance(appKey).Info(message, userFields, logType);
        }

        public static void Warn(string appKey, string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            if (HasLoggerInstance(appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.GetLoggerInstance(appKey).Warn(message, userFields, logType);
        }

        public static void Error(string appKey, string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            if (HasLoggerInstance(appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.GetLoggerInstance(appKey).Error(message, userFields, logType);
        }

        public static void Fatal(string appKey, string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            if (HasLoggerInstance(appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.GetLoggerInstance(appKey).Fatal(message, userFields, logType);
        }

        public static void Report(string appKey, GpLogLevel logLevel, string message, string logString, string stackTrace)
        {
            if (HasLoggerInstance(appKey, MethodBase.GetCurrentMethod().Name) == false)
            {
                return;
            }

            GpLoggerInstanceManager.GetLoggerInstance(appKey).Report(logLevel, message, logString, stackTrace);
        }

        private static bool HasLoggerInstance(string appKey, string apiName)
        {
            if (IsValidAppKey(appKey, apiName) == false)
            {
                return false;
            }

            var loggerApi = GpLoggerInstanceManager.GetLoggerInstance(appKey);

            if (loggerApi == null)
            {
                var type = typeof(GpLogger);
                var error = new GpLoggerError(GpLoggerErrorCode.NOT_INITIALIZED_LOGGER, type.Name);
                GpLog.Warn(error, type, apiName);

                return false;
            }

            return true;
        }

        private static bool IsValidAppKey(string appKey, string methodName)
        {
            if (string.IsNullOrEmpty(appKey) == true)
            {
                var type = typeof(GpLogger);
                var error = new GpLoggerError(GpLoggerErrorCode.INVALID_PARAMETER, type.Name, GpLoggerStrings.INVALID_PARAMETER_APP_KEY_IS_NULL_OR_EMPTY);
                GpLog.Warn(error, type, methodName);

                return false;
            }

            return true;
        }

        private static void InitilizeJson()
        {
            JsonMapper.RegisterExporter<float>((obj, writer) => writer.Write(Convert.ToDouble(obj)));
            JsonMapper.RegisterImporter((float input) => { return (int)input; });
            JsonMapper.RegisterImporter((float input) => { return (long)input; });
            JsonMapper.RegisterImporter((int input) => { return (long)input; });
            JsonMapper.RegisterImporter((int input) => { return (double)input; });
            JsonMapper.RegisterImporter((int input) => { return (float)input; });
            JsonMapper.RegisterImporter((double input) => { return (int)input; });
            JsonMapper.RegisterImporter((double input) => { return (long)input; });
            JsonMapper.RegisterImporter<double, float>(input => Convert.ToSingle(input));
        }
    }
}