namespace Toast.Gamebase.Internal.Auth.Browser
{
    public interface IBrowser
    {
        public void OpenLoginWindow(string url);
        public void CloseLoginWindow();
    }
}
