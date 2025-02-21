#if UNITY_EDITOR || UNITY_WEBGL
namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebaseCommunity : CommonGamebaseCommunity
    {
        public WebGLGamebaseCommunity()
        {
            Domain = typeof(WebGLGamebaseCommunity).Name;
        }
    }
}
#endif