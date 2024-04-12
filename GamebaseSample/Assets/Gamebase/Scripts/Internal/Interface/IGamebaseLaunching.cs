namespace Toast.Gamebase.Internal
{
    public interface IGamebaseLaunching
    {
        LaunchingResponse.LaunchingInfo GetLaunchingInformations();
        int GetLaunchingStatus();
    }
}