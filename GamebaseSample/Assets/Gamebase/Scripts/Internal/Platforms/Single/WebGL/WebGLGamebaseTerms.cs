#if UNITY_EDITOR || UNITY_WEBGL

namespace Toast.Gamebase.Internal.Single.WebGL
{
	public class WebGLGamebaseTerms : CommonGamebaseTerms
	{

		public WebGLGamebaseTerms()
		{
			Domain = typeof(WebGLGamebaseTerms).Name;
		}
	}
}
#endif
