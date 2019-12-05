#if UNITY_IOS || UNITY_EDITOR

namespace Toast.Internal
{
    using System;
    using System.Runtime.InteropServices;

    public class ToastIosMessenger : IToastPlatformMessenger
    {
        [DllImport("__Internal")]
        private static extern IntPtr _toast_setApiData(string data);

        public string SendMessage(MethodCall methodCall)
        {
            var ret = _toast_setApiData(methodCall.ToJsonString());
            return Marshal.PtrToStringAnsi(ret);
        }

        public string SyncSendMessage(MethodCall methodCall)
        {
            var ret = _toast_setApiData(methodCall.ToJsonString());
            return Marshal.PtrToStringAnsi(ret);
        }
    }
}
#endif