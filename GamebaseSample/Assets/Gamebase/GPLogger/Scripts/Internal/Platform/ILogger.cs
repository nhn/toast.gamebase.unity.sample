using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public interface ILogger
    {
        void Initialize(GpLoggerParams.Initialization param);
        void Log(GpLogLevel logLevel, string message, Dictionary<string, string> userFields, string logType = "");
        void Report(CrashData crashData);
        void SetUserField(string key, string value);
        void SetLoggerListener(IGpLoggerListener listener);
        void SetCrashListener(GpLogger.CrashListener listener);
    }
}