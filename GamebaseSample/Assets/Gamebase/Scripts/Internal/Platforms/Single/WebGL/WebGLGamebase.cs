#if UNITY_EDITOR || UNITY_WEBGL

namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebase : CommonGamebase
    {
        public WebGLGamebase()
        {
            Domain = typeof(WebGLGamebase).Name;
        }
    }
}
#endif