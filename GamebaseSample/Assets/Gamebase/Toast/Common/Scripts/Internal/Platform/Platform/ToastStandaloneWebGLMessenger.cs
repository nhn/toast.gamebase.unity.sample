#if (UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR)


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