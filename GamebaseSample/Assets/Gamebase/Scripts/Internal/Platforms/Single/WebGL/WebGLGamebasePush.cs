#if UNITY_EDITOR || UNITY_WEBGL

namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebasePush : CommonGamebasePush
    {
        public WebGLGamebasePush()
        {
            Domain = typeof(WebGLGamebasePush).Name;
        }
    }
}
#endif