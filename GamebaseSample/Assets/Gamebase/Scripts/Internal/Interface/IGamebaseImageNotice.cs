namespace Toast.Gamebase.Internal
{
    public interface IGamebaseImageNotice
    {
        void ShowImageNotices(GamebaseRequest.ImageNotice.Configuration configuration, int closeHandle, int eventHandle);
        void CloseImageNotices();
    }
}
