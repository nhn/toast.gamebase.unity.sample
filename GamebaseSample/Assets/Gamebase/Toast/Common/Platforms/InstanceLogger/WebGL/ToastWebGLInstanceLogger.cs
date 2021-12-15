using System.Collections.Generic;

namespace Toast.Internal
{
    public class ToastWebGLInstanceLogger : IToastNativeInstanceLogger
    {
        public void Initialize(ServiceZone serviceZone)
        {
            ToastInstanceLoggerCommonLogic.Initialize(serviceZone);
        }

        public void Log(string type, string logLevel, string message, Dictionary<string, string> userFields)
        {
            ToastInstanceLoggerCommonLogic.Log(type, logLevel, message, userFields);
        }

        public void SetUserField(string key, string value)
        {
            ToastInstanceLoggerCommonLogic.SetUserField(key, value);
        }

        public void SetAppKey(string appKey)
        {
            ToastInstanceLoggerCommonLogic.AppKey = appKey;
        }
    }
}
