#if UNITY_EDITOR || UNITY_STANDALONE

using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;

namespace Toast.Gamebase.Internal.Single
{
    public interface IWebviewAdapter
    {
        void ShowWebView(
            string url,
            WebViewRequest.Configuration configuration,
            GamebaseCallback.ErrorDelegate closeCallback = null, List<string> schemeList = null,
            GamebaseCallback.GamebaseDelegate<string> schemeEvent = null, 
            GamebaseCallback.VoidDelegate callback = null);

        void CloseWebView();
    }
}
#endif