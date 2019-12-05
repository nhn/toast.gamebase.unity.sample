#if UNITY_EDITOR || UNITY_WEBGL

using System.Runtime.InteropServices;

namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebaseWebview : CommonGamebaseWebview
    {
        [DllImport("__Internal")]
        private extern static void OpenBrowser(string url);

        public WebGLGamebaseWebview()
        {
            Domain = typeof(WebGLGamebaseWebview).Name;
        }
        
        public override void OpenWebBrowser(string url)
        {
            OpenBrowser(url);
        }
    }
}
#endif