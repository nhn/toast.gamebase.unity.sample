#if UNITY_EDITOR || UNITY_WEBGL
namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebaseContact : CommonGamebaseContact
    {
        public WebGLGamebaseContact()
        {
            Domain = typeof(WebGLGamebaseContact).Name;
        }
    }
}
#endif