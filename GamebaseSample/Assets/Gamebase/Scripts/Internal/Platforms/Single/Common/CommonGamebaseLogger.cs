#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.Internal;

namespace Toast.Gamebase.Single
{
    public class CommonGamebaseLogger : IGamebaseLogger
    {
        public void Initialize(GamebaseRequest.Logger.Configuration configuration)

        {
            UnityLoggerControlle.Instance.Initialize(configuration);
        }

        public void Debug(string message, Dictionary<string, string> userFields = null)
        {
            UnityLoggerControlle.Instance.Debug(message, userFields);
        }

        public void Info(string message, Dictionary<string, string> userFields = null)
        {
            UnityLoggerControlle.Instance.Info(message, userFields);
        }

        public void Warn(string message, Dictionary<string, string> userFields = null)
        {
            UnityLoggerControlle.Instance.Warn(message, userFields);
        }

        public void Error(string message, Dictionary<string, string> userFields = null)
        {
            UnityLoggerControlle.Instance.Error(message, userFields);
        }

        public void Fatal(string message, Dictionary<string, string> userFields = null)
        {
            UnityLoggerControlle.Instance.Fatal(message, userFields);
        }

        public void SetUserField(string key, string value)
        {
            UnityLoggerControlle.Instance.SetUserField(key, value);
        }

        public void SetLoggerListener(GamebaseCallback.Logger.ILoggerListener listener)
        {
            UnityLoggerControlle.Instance.SetLoggerListener(listener);
        }

        public void SetCrashListener(GamebaseCallback.Logger.CrashListener listener)
        {
            UnityLoggerControlle.Instance.SetCrashListener(listener);
        }

        public void AddCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
        {
            UnityLoggerControlle.Instance.AddCrashFilter(filter);
        }

        public void RemoveCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
        {
            UnityLoggerControlle.Instance.RemoveCrashFilter(filter);
        }
    }
}
#endif
