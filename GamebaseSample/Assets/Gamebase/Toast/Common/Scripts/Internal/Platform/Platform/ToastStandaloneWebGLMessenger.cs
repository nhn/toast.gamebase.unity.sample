#if (UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR)

using UnityEngine;
using System.Collections;

namespace Toast.Internal
{
    public class ToastStandaloneWebGLMessenger : IToastPlatformMessenger
    {
        public string SendMessage(MethodCall methodCall)
        {
            return ToastSDK.CallMessage(methodCall.ToJsonString());
        }

        public string SyncSendMessage(MethodCall methodCall)
        {
            return ToastSDK.CallMessage(methodCall.ToJsonString());
        }
    }
}

#endif