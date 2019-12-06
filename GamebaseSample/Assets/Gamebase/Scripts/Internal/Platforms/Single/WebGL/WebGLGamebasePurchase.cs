#if UNITY_EDITOR || UNITY_WEBGL

namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebasePurchase : CommonGamebasePurchase
    {
        public WebGLGamebasePurchase()
        {
            Domain = typeof(WebGLGamebasePurchase).Name;
        }
    }
}
#endif