#if UNITY_EDITOR || UNITY_STANDALONE

using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single
{
    public interface IWebviewAdapter
    {
        void ShowWebView(string url, GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration = null, GamebaseCallback.ErrorDelegate closeCallback = null, List<string> schemeList = null, GamebaseCallback.GamebaseDelegate<string> schemeEvent = null);
        void CloseWebView();        
    }
}
#endif