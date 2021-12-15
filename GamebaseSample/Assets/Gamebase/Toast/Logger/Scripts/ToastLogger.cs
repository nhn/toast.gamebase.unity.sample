using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Toast.Internal;
using UnityEngine;

namespace Toast.Logger
{
    public enum ToastLogLevel
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        ERROR = 3,
        FATAL = 4,
        NONE = 5
    }

    public static class ToastLogger
    {
        private const string SERVICE_NAME = "logger";

        private static bool _isInitialized = false;

        public delegate void CrashListener(bool isSuccess, LogEntry logEntry);

        public delegate bool CrashFilter(CrashLogData logData);

        public static bool IsInitialized()
        {
            return _isInitialized;
        }

        public static void Initialize(ToastLoggerConfiguration loggerConfiguration)
        {
            if (string.IsNullOrEmpty(loggerConfiguration.AppKey))
            {
                int errorCode = ToastLoggerErrorCode.InvalidUserKey.Code;
                string errroMessage = "AppKey is null or empty string";
                ToastLog.Error(errorCode + " : " + errroMessage);
                return;
            }

            if (_isInitialized)
            {
                ToastLog.Warn("ToastLogger has already been initialized.");
                return;
            }

            string methodName = MethodBase.GetCurrentMethod().Name;
            string uri = ToastUri.Create(SERVICE_NAME, methodName.ToLower());
            MethodCall methodCall = MethodCall.CreateSyncCall(uri);
            methodCall.AddParameter("projectKey", loggerConfiguration.AppKey);
            methodCall.AddParameter("serviceZone", loggerConfiguration.ServiceZone.ToString().ToUpper());
            methodCall.AddParameter("enableCrashReporter", loggerConfiguration.EnableCrashReporter);
            var result = ToastNativeSender.SyncSendMessage(methodCall);
            if (result == null || !result.Result.IsSuccessful) return;
            ToastCrashManager.Instance.Initialize(
                loggerConfiguration.EnableCrashReporter,
                loggerConfiguration.EnableCrashErrorLog);

            _isInitialized = true;

            ToastAuditLog.SendUsageLog();
        }

        public static void Log(ToastLogLevel logLevel, string message, Dictionary<string, string> userFields = null)
        {
            if (!IsInitialized())
            {
                return;
            }

            if (userFields != null)
            {
                foreach (var items in userFields)
                {
                    if (string.IsNullOrEmpty(items.Key))
                    {
                        int errorCode = ToastLoggerErrorCode.InvalidUserKey.Code;
                        string errroMessage = "key is null or empty string";
                        ToastLog.Error(errorCode + " : " + errroMessage);
                        return;
                    }
                }
            }

            string methodName = MethodBase.GetCurrentMethod().Name;
            string uri = ToastUri.Create(SERVICE_NAME, methodName.ToLower());
            MethodCall methodCall = MethodCall.CreateSyncCall(uri);
            methodCall.AddParameter("level", logLevel.ToString().ToUpper());
            methodCall.AddParameter("message", message);
            if (userFields != null)
            {
                methodCall.AddParameter("userFields", userFields);
            }

            Dispatcher.Instance.Post(() => ToastNativeSender.SyncSendMessage(methodCall));
        }

        public static void Debug(string message, Dictionary<string, string> userFields = null)
        {
            Log(ToastLogLevel.DEBUG, message, userFields);
        }

        public static void Info(string message, Dictionary<string, string> userFields = null)
        {
            Log(ToastLogLevel.INFO, message, userFields);
        }

        public static void Warn(string message, Dictionary<string, string> userFields = null)
        {
            Log(ToastLogLevel.WARN, message, userFields);
        }

        public static void Error(string message, Dictionary<string, string> userFields = null)
        {
            Log(ToastLogLevel.ERROR, message, userFields);
        }

        public static void Fatal(string message, Dictionary<string, string> userFields = null)
        {
            Log(ToastLogLevel.FATAL, message, userFields);
        }

        public static void SetUserField(string key, string value)
        {
            if (!IsInitialized())
            {
                return;
            }

            if ((key == null) || (key == ""))
            {
                int errorCode = ToastLoggerErrorCode.InvalidUserKey.Code;
                string errroMessage = "key is null or empty string";
                ToastLog.Error(errorCode + " : " + errroMessage);
                return;
            }

            string methodName = MethodBase.GetCurrentMethod().Name;
            string uri = ToastUri.Create(SERVICE_NAME, methodName.ToLower());
            MethodCall methodCall = MethodCall.CreateSyncCall(uri);
            methodCall.AddParameter("key", key);
            methodCall.AddParameter("value", value);

            Dispatcher.Instance.Post(() => ToastNativeSender.SyncSendMessage(methodCall));
        }

        public static void Report(ToastLogLevel logLevel, string message, string logString, string stackTrace)
        {
            string dmpData = EncodeDmpData(logString, stackTrace);
            Report(logLevel, message, dmpData);
        }

        public static void Report(ToastLogLevel logLevel, string message, Exception e)
        {
            Report(logLevel, message, ConvertExceptionToString(e));
        }

        private static void Report(ToastLogLevel logLevel, string message, string dumpData)
        {
            if (!IsInitialized())
            {
                return;
            }

            string methodName = "exception";
            string uri = ToastUri.Create(SERVICE_NAME, methodName.ToLower());
            MethodCall methodCall = MethodCall.CreateSyncCall(uri);
            methodCall.AddParameter("logType", ToastLoggerType.HANDLED);
            methodCall.AddParameter("logLevel", logLevel.ToString().ToUpper());
            methodCall.AddParameter("message", message);
            methodCall.AddParameter("dmpData", dumpData);

            methodCall.AddParameter("userFields", new Dictionary<string, string>
            {
                {"Unity", Application.unityVersion}
            });

            Dispatcher.Instance.Post(() => ToastNativeSender.SyncSendMessage(methodCall));
        }

        public static void SetLoggerListener(IToastLoggerListener listener)
        {
            if (CrashLoggerListenerReceiver.TryAttachReceiver())
            {
                SetCrashLoggerListener();
            }

            CrashLoggerListenerReceiver.SetLoggerListener(listener);
        }

        public static void SetCrashListener(CrashListener listener)
        {
            if (CrashLoggerListenerReceiver.TryAttachReceiver())
            {
                SetCrashLoggerListener();
            }

            CrashLoggerListenerReceiver.SetCrashListener(listener);
        }

        public static void AddCrashFilter(CrashFilter filter)
        {
            ToastCrashManager.Instance.AddCrashFilter(filter);
        }

        public static void RemoveCrashFilter(CrashFilter filter)
        {
            ToastCrashManager.Instance.RemoveCrashFilter(filter);
        }

        private static string ConvertExceptionToString(Exception e)
        {
            StackTrace stackTrace = new StackTrace(e, true);
            string strCondition = e.GetType().ToString();
            string strMessage = e.Message.ToString();
            string strStackTrace = stackTrace.ToString();

            strCondition = strCondition.Split('.')[1];
            strStackTrace = strStackTrace.Split(' ')[4];
            string logString = strCondition + ":" + strMessage;

            return EncodeDmpData(logString, strStackTrace);
        }

        private static string EncodeDmpData(string logString, string stackTrace)
        {
            string dmp = logString + "\n" + stackTrace;
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(dmp);
            string encodedText = Convert.ToBase64String(bytesToEncode);

            return encodedText;
        }

        private static void SetCrashLoggerListener()
        {
            var methodName = "SetLoggerListener";
            var uri = ToastUri.Create(SERVICE_NAME, methodName.ToLower());
            var methodCall = MethodCall.CreateSyncCall(uri)
                .AddParameter("success", "OnLogSuccess")
                .AddParameter("filter", "OnLogFilter")
                .AddParameter("save", "OnLogSave")
                .AddParameter("error", "OnLogError");

            Dispatcher.Instance.Post(() => ToastNativeSender.SyncSendMessage(methodCall));
        }

        public static void SetCrashDataAdapter(ICrashDataAdapter crashDataAdapter)
        {
            ToastCrashManager.Instance.SetCrashDataAdapter(crashDataAdapter);
        }
    }
}