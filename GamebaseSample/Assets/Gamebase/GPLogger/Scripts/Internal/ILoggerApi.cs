using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public interface ILoggerApi
    {
        void Initialize(GpLoggerParams.Initialization param);
        void SetLoggerListener(IGpLoggerListener listener);
        void SetCrashListener(GpLogger.CrashListener listener);
        void SetUserField(string key, string value);
        void AddCrashFilter(GpLogger.CrashFilter filter);
        void RemoveCrashFilter(GpLogger.CrashFilter filter);
        void Debug(string message, Dictionary<string, string> userFields = null, string logType = "");
        void Info(string message, Dictionary<string, string> userFields = null, string logType = "");
        void Warn(string message, Dictionary<string, string> userFields = null, string logType = "");
        void Error(string message, Dictionary<string, string> userFields = null, string logType = "");
        void Fatal(string message, Dictionary<string, string> userFields = null, string logType = "");
        void Report(GpLogLevel logLevel, string message, string logString, string stackTrace);
    }
}