#if UNITY_EDITOR || UNITY_WEBGL

namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebaseLaunching : CommonGamebaseLaunching
    {
        public WebGLGamebaseLaunching()
        {
            Domain = typeof(WebGLGamebaseLaunching).Name;
        }
    }
}
#endif