using JetBrains.Annotations;
using System;

namespace Toast.Internal
{
    public static class ToastNativeSender
    {
        public static NativeResponse SendMessage(MethodCall methodCall)
        {
            try
            {
                var retMessage = ToastNativePlugin.Instance.NativePlugin.SendMessage(methodCall);
                var response = NativeResponse.FromJson(retMessage);
                if (response != null)
                {
                    LogResponse(response);
                }
                return response;
            }
            catch (Exception e)
            {
                ToastLog.Error("ToastNativePlugin.Instance.NativePlugin.sendMessage Failed!");
                ToastLog.Exception(e);
            }

            return null;
        }

        [CanBeNull]
        public static NativeResponse SyncSendMessage(MethodCall methodCall)
        {
            try
            {
                ToastLog.Debug(methodCall.ToJsonString());
                var retMessage = ToastNativePlugin.Instance.NativePlugin.SyncSendMessage(methodCall);
                if (string.IsNullOrEmpty(retMessage))
                {
                    return null;
                }

                ToastLog.Debug(retMessage);
                var response = NativeResponse.FromJson(retMessage);
                LogResponse(response);
                return response;
            }
            catch (Exception e)
            {
                ToastLog.Error("ToastNativePlugin.Instance.NativePlugin.sendMessage Failed!");
                ToastLog.Exception(e);
                ToastLog.Developer("Raise exception when calling a method : {0}\n{1}", e.Message, e.StackTrace);
            }

            return null;
        }

        private static void LogResponse(NativeResponse response)
        {
            var isSuccessful = response.Result.IsSuccessful;
            if (isSuccessful)
            {
                ToastLog.Developer("Call : Success");
            }
            else
            {
                ToastLog.Developer("Call : Fail\nCode : {0}\nMessage : {1}",
                    response.Result.Code,
                    response.Result.Message);
            }
        }
    }
}