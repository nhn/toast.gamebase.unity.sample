using System;
using System.Reflection;
using Toast.Internal;

namespace Toast
{
    public static class ToastSdk
    {
        private const string ServiceName = "core";
        private static bool _isInitialize = false;

        internal static event Action<string> UserIdChanged;

        public static void Initialize()
        {
            if (_isInitialize)
            {
                ToastLog.Warn("Already initialize " + typeof(ToastSdk).Name);
                return;
            }

            var methodName = MethodBase.GetCurrentMethod().Name;
            var uri = ToastUri.Create(ServiceName, methodName.ToLower());
            var methodCall = MethodCall.CreateSyncCall(uri);
            var response = ToastNativeSender.SyncSendMessage(methodCall);

            if (response != null)
            {
                _isInitialize = response.Result.IsSuccessful;
            }
        }

        public static bool DebugMode
        {
            get
            {
                var uri = ToastUri.Create(ServiceName, "isDebugMode");
                var methodCall = MethodCall.CreateSyncCall(uri);
                var response = ToastNativeSender.SyncSendMessage(methodCall);
                return response != null && response.Body["debugMode"].AsBool;
            }
            set
            {
                var uri = ToastUri.Create(ServiceName, "setDebugMode");
                var methodCall = MethodCall.CreateSyncCall(uri);
                methodCall.AddParameter("debugMode", value);
                var response = ToastNativeSender.SyncSendMessage(methodCall);
                if (response != null && response.Result.IsSuccessful)
                {
                    ToastLog.Level = value ? ToastLog.LogLevel.Debug : ToastLog.LogLevel.Warn;
                }
            }
        }

        public static string UserId
        {
            get
            {
                var uri = ToastUri.Create(ServiceName, "getUserId");
                var methodCall = MethodCall.CreateSyncCall(uri);
                var response = ToastNativeSender.SyncSendMessage(methodCall);
                if (response != null)
                {
                    return response.Body["userId"];
                }

                return null;
            }
            set
            {
                var uri = ToastUri.Create(ServiceName, "setUserId");
                var methodCall = MethodCall.CreateSyncCall(uri);
                methodCall.AddParameter("userId", value);
                var response = ToastNativeSender.SyncSendMessage(methodCall);
                if (response != null && response.Result.IsSuccessful)
                {
                    if (UserIdChanged != null)
                    {
                        UserIdChanged(value);
                    }
                }
            }
        }

        public static void SetOptionalPolicies(params string[] properties)
        {
            var propertiesJsonArray = new JSONArray();
            foreach (var property in properties)
            {
                propertiesJsonArray.Add(property);
            }

            var uri = ToastUri.Create(ServiceName, MethodBase.GetCurrentMethod().Name);
            var methodCall = MethodCall.CreateSyncCall(uri);
            methodCall.AddParameter("properties", propertiesJsonArray);
            ToastNativeSender.SyncSendMessage(methodCall);
        }
    }
}