#if UNITY_STANDALONE || UNITY_EDITOR

using System.Collections.Generic;
using Toast.Logger;

namespace Toast.Logger
{
    public class ToastStandaloneLogger : IToastNativeLogger
    {
        public void Initialize(ToastLoggerConfiguration loggerConfiguration)
        {
            ToastLoggerCommonLogic.Initialize(loggerConfiguration);
        }

        public void Log(string logLevel, string message, Dictionary<string, string> userFields = null)
        {
            ToastLoggerCommonLogic.Log(logLevel, message, userFields);
        }

        public void Report(string logLevel, string message, string dumpData, Dictionary<string, string> userFields = null)
        {
            ToastLoggerCommonLogic.Report(logLevel, message, dumpData, userFields);
        }

        public void SetUserField(string key, string value)
        {
            ToastLoggerCommonLogic.SetUserField(key, value);
        }

        public void SetLoggerListener()
        {
            ToastLoggerCommonLogic.IsLoggerListener = true;
        }
    }
}

#endif