using System.Collections.Generic;
using Toast.Gamebase.Internal.Auth;
using Toast.Gamebase.Internal.Auth.Browser;

#if UNITY_EDITOR || UNITY_WEBGL

namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebaseAuth : CommonGamebaseAuth
    {
        public WebGLGamebaseAuth()
        {
            Domain = typeof(WebGLGamebaseAuth).Name;
            
            _browserLoginService = new BrowserLoginService(new WebGLBrowser());
        }
    }
}
#endif