namespace Toast.Gamebase.Internal
{
    public interface IGamebaseLaunching
    {
        GamebaseResponse.Launching.LaunchingInfo GetLaunchingInformations();
        int GetLaunchingStatus();
    }
}