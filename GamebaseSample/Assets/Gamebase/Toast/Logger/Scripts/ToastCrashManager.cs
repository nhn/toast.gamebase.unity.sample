#define UNITY_4_OR_LATER
#define UNITY_5_OR_LATER

#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
#undef UNITY_4_OR_LATER
#undef UNITY_5_OR_LATER
#endif

#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6
#undef UNITY_5_OR_LATER
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Toast.Internal;
using UnityEngine;

namespace Toast.Logger
{
    public class ToastCrashManager
    {
        private const string SERVICE_NAME = "logger";

        private bool _isInitialize = false;
        private bool _enableCrash = false;
        private bool _enableCrashErrorLog = false;

        private List<ToastLogger.CrashFilter> _crashFilters = new List<ToastLogger.CrashFilter>();

        private ICrashDataAdapter _crashDataAdapter;

        private ToastCrashManager()
        {
        }

        static ToastCrashManager()
        {
            Instance = new ToastCrashManager();
        }

        public static ToastCrashManager Instance { get; private set; }

        public void Initialize(bool enableCrash, bool enableCrashErrorLog)
        {
            _enableCrash = enableCrash;
            _enableCrashErrorLog = enableCrashErrorLog;

            if (_isInitialize)
            {
                return;
            }

#if UNITY_5_OR_LATER
            Application.logMessageReceived += OnHandleException;
#else
            Application.RegisterLogCallback(OnHandleException);
#endif

            _isInitialize = true;
        }

        public void EnableLogCallback(bool isEnabled)
        {
            _enableCrash = isEnabled;
        }

        public void SetCrashDataAdapter(ICrashDataAdapter crashDataAdapter)
        {
            _crashDataAdapter = crashDataAdapter;
        }

        internal void AddCrashFilter(ToastLogger.CrashFilter filter)
        {
            _crashFilters.Add(filter);
        }

        internal void RemoveCrashFilter(ToastLogger.CrashFilter filter)
        {
            _crashFilters.Remove(filter);
        }

        internal void OnHandleException(string logString, string stackTrace, LogType type)
        {
            if (!_enableCrash)
            {
                return;
            }

            switch (type)
            {
                case LogType.Error:
                    if (!_enableCrashErrorLog)
                    {
                        return;
                    }

                    if (IsFilteredCrash(logString, stackTrace, type))
                    {
                        return;
                    }

                    Exception(ToastLogLevel.ERROR, logString, logString, logString + "\n" + stackTrace);
                    break;
                case LogType.Assert:
                case LogType.Exception:
                    if (IsFilteredCrash(logString, stackTrace, type))
                    {
                        return;
                    }

                    Exception(ToastLogLevel.FATAL, logString, logString, logString + "\n" + stackTrace);
                    break;
            }
        }

        private bool IsFilteredCrash(string logString, string stackTrace, LogType type)
        {
            if (_crashFilters.Count <= 0)
            {
                return false;
            }

            var logData = new CrashLogData(type, logString, stackTrace);
            return _crashFilters.Any(filter => filter(logData));
        }

        private static void Exception(ToastLogLevel logLevel, string message, string logString, string stackTrace)
        {
            var userFields = new Dictionary<string, string>();
            userFields.Add("Unity", Application.unityVersion);

            if (Instance._crashDataAdapter != null)
            {
                var crashDataAdapterUserFields = Instance._crashDataAdapter.GetUserFields();

                if (crashDataAdapterUserFields != null)
                {
                    foreach (string key in crashDataAdapterUserFields.Keys)
                    {
                        userFields.Add(key, crashDataAdapterUserFields[key]);
                    }
                }
            }

            var dmpData = EncodeDmpData(logString, stackTrace);

            var methodName = MethodBase.GetCurrentMethod().Name;
            var uri = ToastUri.Create(SERVICE_NAME, methodName.ToLower());
            var methodCall = MethodCall.CreateSyncCall(uri)
                .AddParameter("logType", ToastLoggerType.CRASH_FROM_UNITY)
                .AddParameter("logLevel", logLevel.ToString().ToUpper())
                .AddParameter("message",
                    string.IsNullOrEmpty(message) ? "Raises a exception, but message is empty" : message)
                .AddParameter("dmpData", dmpData)
                .AddParameter("userFields", userFields);

            Dispatcher.Instance.Post(() => ToastNativeSender.SyncSendMessage(methodCall));
        }

        private static string EncodeDmpData(string logString, string stackTrace)
        {
            var dmp = logString + "\n" + stackTrace;
            var bytesToEncode = Encoding.UTF8.GetBytes(dmp);
            var encodedText = Convert.ToBase64String(bytesToEncode);

            return encodedText;
        }
    }
}