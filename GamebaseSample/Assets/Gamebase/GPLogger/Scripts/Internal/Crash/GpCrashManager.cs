using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GamePlatform.Logger.Internal
{
    public class GpCrashManager
    {
        private const string SERVICE_NAME = "logger";

        private bool isInitialized = false;
        private bool enableCrashReporter = false;
        private bool enableCrashErrorLog = false;

        private List<GpLogger.CrashFilter> crashFilterList = new List<GpLogger.CrashFilter>();

        private ICrashDataAdapter crashDataAdapter;
        private Action<CrashData> crashHandler;

        private static readonly GpCrashManager instance = new GpCrashManager();

        public static GpCrashManager Instance
        {
            get { return instance; }
        }

        public void Initialize(bool enableCrashReporter, bool enableCrashErrorLog)
        {
            this.enableCrashReporter = enableCrashReporter;
            this.enableCrashErrorLog = enableCrashErrorLog;

            if (isInitialized == true)
            {
                return;
            }

            Application.logMessageReceived += OnLogCallback;

            isInitialized = true;
        }

        public void AddCrashHandler(Action<CrashData> crashHandler)
        {
            this.crashHandler = crashHandler;
        }

        public void EnableLogCallback(bool isEnabled)
        {
            enableCrashReporter = isEnabled;
        }

        public void SetCrashDataAdapter(ICrashDataAdapter crashDataAdapter)
        {
            this.crashDataAdapter = crashDataAdapter;
        }

        public void AddCrashFilter(GpLogger.CrashFilter filter)
        {
            if (crashFilterList.Contains(filter) == true)
            {
                GpLog.Warn("This filter is already registered.", GetType(), "AddCrashFilter");
                return;
            }

            crashFilterList.Add(filter);
        }

        public void RemoveCrashFilter(GpLogger.CrashFilter filter)
        {
            if (crashFilterList.Contains(filter) == false)
            {
                GpLog.Warn("This is an unregistered filter.", GetType(), "RemoveCrashFilter");
                return;
            }

            crashFilterList.Remove(filter);
        }

        public void OnLogCallback(string logString, string stackTrace, LogType type)
        {
            if (enableCrashReporter == false)
            {
                return;
            }

            switch (type)
            {
                case LogType.Error:
                    {
                        if (enableCrashErrorLog == false)
                        {
                            return;
                        }

                        if (IsFilteredCrash(logString, stackTrace, type) == true)
                        {
                            return;
                        }

                        Exception(GpLogLevel.ERROR, logString, logString, string.Format("{0}\n{1}", logString, stackTrace));
                        break;
                    }
                case LogType.Assert:
                case LogType.Exception:
                    {
                        if (IsFilteredCrash(logString, stackTrace, type) == true)
                        {
                            return;
                        }

                        Exception(GpLogLevel.FATAL, logString, logString, string.Format("{0}\n{1}", logString, stackTrace));
                        break;
                    }
            }
        }

        private bool IsFilteredCrash(string logString, string stackTrace, LogType type)
        {
            if (crashFilterList.Count <= 0)
            {
                return false;
            }

            var data = new CrashLogData(type, logString, stackTrace);
            return crashFilterList.Any(filter => filter(data));
        }

        private void Exception(GpLogLevel logLevel, string message, string logString, string stackTrace)
        {
            var userFields = new Dictionary<string, string>
            {
                { "Unity", Application.unityVersion }
            };

            if (crashDataAdapter != null)
            {
                var crashDataAdapterUserFields = crashDataAdapter.GetUserFields();

                if (crashDataAdapterUserFields != null)
                {
                    foreach (string key in crashDataAdapterUserFields.Keys)
                    {
                        userFields.Add(key, crashDataAdapterUserFields[key]);
                    }
                }
            }

            if (crashHandler != null)
            {
                var crashData = new CrashData
                {
                    logType = GpLoggerType.CRASH_FROM_UNITY,
                    logLevel = logLevel.ToString().ToUpper(),
                    message = string.IsNullOrEmpty(message) ? "Raises a exception, but message is empty." : message,
                    userFields = userFields
                };

                crashData.SetDmpData(logString, stackTrace);
                crashHandler(crashData);
            }
        }
    }
}