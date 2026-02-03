namespace Toast.Gamebase.Internal
{
    public interface IGamebaseGameNotice
    {
        void OpenGameNotice(GamebaseRequest.GameNotice.Configuration configuration, int handle);
        
        void RequestGameNoticeInfo(GamebaseRequest.GameNotice.Configuration configuration, int handle);
    }
}
