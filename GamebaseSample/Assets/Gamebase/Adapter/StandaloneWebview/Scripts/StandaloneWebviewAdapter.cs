#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

using System.Collections.Generic;
using Toast.Gamebase.Internal;
using Toast.Gamebase.Internal.Single;

namespace Toast.Gamebase.Adapter
{
    public class StandaloneWebviewAdapter : AdapterBase, IWebviewAdapter
    {
        private const string ADAPTER_VERSION = "2.1.1";

        public override string domain
        {
            get { return typeof(StandaloneWebviewAdapter).ToString(); }
        }

        public override string version
        {
            get { return ADAPTER_VERSION; }
        }
        
        public void ShowWebView(string url, GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration = null, GamebaseCallback.ErrorDelegate closeCallback = null, List<string> schemeList = null, GamebaseCallback.GamebaseDelegate<string> schemeEvent = null)
        {
            StandaloneWebview.Instance.ShowWebView(url, configuration, closeCallback, schemeList, schemeEvent);
        }

        public void CloseWebView()
        {
            StandaloneWebview.Instance.CloseWebView();
        }        
    }
}
#endif