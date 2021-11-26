#if UNITY_EDITOR || UNITY_ANDROID

using System;
using UnityEngine;

namespace Toast.Internal
{
    public class ToastAndroidMessenger : IToastPlatformMessenger
    {
        private const string ToastUnityClassFullName = "com.toast.android.unity.core.ToastUnity";

        private AndroidJavaObject _androidPlugin;

        public ToastAndroidMessenger()
        {
            if (_androidPlugin == null)
            {
                try
                {
                    _androidPlugin = new AndroidJavaObject(ToastUnityClassFullName);
                }
                catch (Exception e)
                {
                    ToastLog.Exception(e);
                }
            }
        }

        public string SendMessage(MethodCall methodCall)
        {
            if (_androidPlugin != null)
            {
                return _androidPlugin.CallStatic<string>("unityMessage", methodCall.ToJsonString());
            }

            return null;
        }

        public string SyncSendMessage(MethodCall methodCall)
        {
            if (_androidPlugin != null)
            {
                return _androidPlugin.CallStatic<string>("unityMessage", methodCall.ToJsonString());
            }

            return null;
        }
    }
}
#endif