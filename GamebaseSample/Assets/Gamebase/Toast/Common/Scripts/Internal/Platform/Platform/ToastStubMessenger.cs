namespace Toast.Internal
{
    public class ToastStubMessenger : IToastPlatformMessenger
    {
        public string SendMessage(MethodCall methodCall)
        {
            return string.Empty;
        }

        public string SyncSendMessage(MethodCall methodCall)
        {
            return string.Empty;
        }
    }
}