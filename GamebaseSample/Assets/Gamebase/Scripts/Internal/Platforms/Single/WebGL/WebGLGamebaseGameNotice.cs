#if UNITY_EDITOR || UNITY_WEBGL
namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebaseGameNotice : CommonGamebaseGameNotice
    {
        public WebGLGamebaseGameNotice()
        {
            Domain = typeof(WebGLGamebaseGameNotice).Name;
        }
    }
}
#endif