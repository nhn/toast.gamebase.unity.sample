using System.Collections.Generic;
using Toast.Internal;

namespace Toast.Logger
{
    public class InstanceLogger
    {
        private string _appKey;

        public InstanceLogger(string appKey, ToastServiceZone zone = ToastServiceZone.REAL) : this(appKey, new DefaultSettings(), zone)
        {
        }

        public InstanceLogger(string appKey, ILoggerSettings settings, ToastServiceZone zone = ToastServiceZone.REAL)
        {
            _appKey = appKey;

            var uri = ToastUri.Create("logger", new ToastUri.VariableSegment(appKey), "initialize");
            var methodCall = MethodCall.CreateSyncCall(uri);
            methodCall.AddParameter("projectKey", appKey)
                .AddParameter("serviceZone", zone.ToString())
                .AddParameter("setting", settings.SettingName.ToUpper());
            ToastNativeSender.SyncSendMessage(methodCall);
        }

        public void Log(string type, ToastLogLevel level, string message, IDictionary<string, string> userFields = null)
        {
            if (string.IsNullOrEmpty(type))
            {
                type = "NORMAL";
            }

            var uri = ToastUri.Create("logger", new ToastUri.VariableSegment(_appKey), "log");
            var methodCall = MethodCall.CreateSyncCall(uri);
            methodCall.AddParameter("level", level.ToString())
                .AddParameter("type", type)
                .AddParameter("message", message);

            if (userFields != null)
            {
                foreach (var items in userFields)
                {
                    if (string.IsNullOrEmpty(items.Key))
                    {
                        ToastLog.Error("20002 : key is null or empty string");
                        return;
                    }
                }

                methodCall.AddParameter("userFields", userFields);
            }

            var response = ToastNativeSender.SyncSendMessage(methodCall);
            ToastLog.Debug("InstanceLogger.Log.Response={0}", response);
        }

        public void Debug(string type, string message, IDictionary<string, string> userFields = null)
        {
            Log(type, ToastLogLevel.DEBUG, message, userFields);
        }

        public void Info(string type, string message, IDictionary<string, string> userFields = null)
        {
            Log(type, ToastLogLevel.INFO, message, userFields);
        }

        public void Warn(string type, string message, IDictionary<string, string> userFields = null)
        {
            Log(type, ToastLogLevel.WARN, message, userFields);
        }

        public void Error(string type, string message, IDictionary<string, string> userFields = null)
        {
            Log(type, ToastLogLevel.ERROR, message, userFields);
        }

        public void Fatal(string type, string message, IDictionary<string, string> userFields = null)
        {
            Log(type, ToastLogLevel.FATAL, message, userFields);
        }

        public void SetUserField(string key, string value)
        {
            var uri = ToastUri.Create("logger", new ToastUri.VariableSegment(_appKey), "setUserField");
            var methodCall = MethodCall.CreateSyncCall(uri);
            methodCall.AddParameter("key", key)
                .AddParameter("value", value);
            var response = ToastNativeSender.SyncSendMessage(methodCall);
            ToastLog.Debug("InstanceLogger.SetUserField.Response={0}", response);
        }
    }
}