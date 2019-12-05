namespace Toast.Internal
{
    public interface IToastPlatformMessenger
    {
        string SendMessage(MethodCall methodCall);

        string SyncSendMessage(MethodCall methodCall);
    }
}