using System.Collections.Generic;
using Toast.Logger;
           
namespace Toast.Logger
{
    public interface IToastNativeLogger
    {
        void Initialize(ToastLoggerConfiguration loggerConfiguration);
        void Log(string logLevel, string message, Dictionary<string, string> userFields);
        void Report(string logLevel, string message, string dumpData, Dictionary<string, string> userFields);
        void SetUserField(string key, string value);
        void SetLoggerListener();
    }
}