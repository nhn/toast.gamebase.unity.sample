#if UNITY_EDITOR || UNITY_WEBGL
namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebaseAnalytics : CommonGamebaseAnalytics
    {
        public WebGLGamebaseAnalytics()
        {
            Domain = typeof(WebGLGamebaseAnalytics).Name;
        }
    }
}
#endif