using System.Collections.Generic;

namespace Toast.Gamebase
{
    internal interface IGamebaseLogger
    {
        void Initialize(GamebaseRequest.Logger.Configuration loggerConfiguration);        
        void Debug(string message, Dictionary<string, string> userFields = null);
        void Info(string message, Dictionary<string, string> userFields = null);
        void Warn(string message, Dictionary<string, string> userFields = null);
        void Error(string message, Dictionary<string, string> userFields = null);
        void Fatal(string message, Dictionary<string, string> userFields = null);
        void SetUserField(string key, string value);
        void SetLoggerListener(GamebaseCallback.Logger.ILoggerListener listener);

        // Unity only
        void SetCrashListener(GamebaseCallback.Logger.CrashListener listener);
        void AddCrashFilter(GamebaseCallback.Logger.CrashFilter filter);
        void RemoveCrashFilter(GamebaseCallback.Logger.CrashFilter filter);
    }
}
