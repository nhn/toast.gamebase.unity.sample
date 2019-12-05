namespace Toast.Gamebase.Internal
{
    internal interface IGamebasePush
    {
        void RegisterPush(GamebaseRequest.Push.PushConfiguration pushConfiguration, int handle);
        void QueryPush(int handle);
        void SetSandboxMode(bool isSandbox);
    }
}
