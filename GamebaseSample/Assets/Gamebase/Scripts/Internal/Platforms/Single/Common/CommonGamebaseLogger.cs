#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.Internal;

namespace Toast.Gamebase.Single
{
    public class CommonGamebaseLogger : IGamebaseLogger
    {
        public void Initialize(GamebaseRequest.Logger.Configuration configuration)
        {
            UnityLoggerController.Instance.Initialize(configuration);
        }

        public void Debug(string message, Dictionary<string, string> userFields = null)
        {
            UnityLoggerController.Instance.Debug(message, userFields);
        }

        public void Info(string message, Dictionary<string, string> userFields = null)
        {
            UnityLoggerController.Instance.Info(message, userFields);
        }

        public void Warn(string message, Dictionary<string, string> userFields = null)
        {
            UnityLoggerController.Instance.Warn(message, userFields);
        }

        public void Error(string message, Dictionary<string, string> userFields = null)
        {
            UnityLoggerController.Instance.Error(message, userFields);
        }

        public void Fatal(string message, Dictionary<string, string> userFields = null)
        {
            UnityLoggerController.Instance.Fatal(message, userFields);
        }

        public void Report(GamebaseLoggerConst.LogLevel logLevel, string message, string logString, string stackTrace)
        {
            UnityLoggerController.Instance.Report(logLevel, message, logString, stackTrace);
        }

        public void SetUserField(string key, string value)
        {
            UnityLoggerController.Instance.SetUserField(key, value);
        }

        public void SetLoggerListener(GamebaseCallback.Logger.ILoggerListener listener)
        {
            UnityLoggerController.Instance.SetLoggerListener(listener);
        }

        public void SetCrashListener(GamebaseCallback.Logger.CrashListener listener)
        {
            UnityLoggerController.Instance.SetCrashListener(listener);
        }

        public void AddCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
        {
            UnityLoggerController.Instance.AddCrashFilter(filter);
        }

        public void RemoveCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
        {
            UnityLoggerController.Instance.RemoveCrashFilter(filter);
        }
    }
}
#endif
