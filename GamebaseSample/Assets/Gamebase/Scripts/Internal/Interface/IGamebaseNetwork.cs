namespace Toast.Gamebase.Internal
{
    public interface IGamebaseNetwork
    {
        GamebaseNetworkType GetNetworkType();
        string GetNetworkTypeName();
        bool IsConnected();
        void IsConnected(int handle);
    }
}