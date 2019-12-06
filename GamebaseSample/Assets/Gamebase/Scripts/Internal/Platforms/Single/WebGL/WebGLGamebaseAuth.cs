#if UNITY_EDITOR || UNITY_WEBGL

namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebaseAuth : CommonGamebaseAuth
    {
        public WebGLGamebaseAuth()
        {
            Domain = typeof(WebGLGamebaseAuth).Name;
        }
    }
}
#endif