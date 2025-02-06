namespace Toast.Gamebase.Internal
{
    internal interface IGamebasePush
    {
        void RegisterPush(GamebaseRequest.Push.PushConfiguration pushConfiguration, int handle);
        void QueryPush(int handle);
        void SetSandboxMode(bool isSandbox);
        void RegisterPush(GamebaseRequest.Push.PushConfiguration pushConfiguration, GamebaseRequest.Push.NotificationOptions options, int handle);
        void QueryTokenInfo(int handle);
        GamebaseResponse.Push.NotificationOptions GetNotificationOptions();
        void QueryNotificationAllowed(int handle);
    }
}
