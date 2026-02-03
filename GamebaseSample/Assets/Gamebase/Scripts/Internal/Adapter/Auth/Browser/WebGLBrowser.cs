using System.Runtime.InteropServices;

namespace Toast.Gamebase.Internal.Auth.Browser
{
# if UNITY_EDITOR || UNITY_WEBGL
    class WebGLBrowser : IBrowser
    {
        [DllImport("__Internal")]
        private static extern void OpenLoginBrowser(string url);
        
        public void OpenLoginWindow(string url)
        {
            OpenLoginBrowser(url);
        }

        public void CloseLoginWindow()
        {
        }
    }
#endif
}