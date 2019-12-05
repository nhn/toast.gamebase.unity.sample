using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public interface IGamebaseWebview
    {
        void OpenWebBrowser(string url);
        void ShowWebView(string url, GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration = null, int closeCallback = -1, List<string> schemeList = null, int schemeEvent = -1);        
        void CloseWebView();
    }
}