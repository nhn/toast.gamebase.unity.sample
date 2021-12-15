#if UNITY_EDITOR || UNITY_WEBGL

namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebaseImageNotice : CommonGamebaseImageNotice
    {
        public WebGLGamebaseImageNotice()
        {
            Domain = typeof(WebGLGamebaseImageNotice).Name;
        }
    }
}
#endif
