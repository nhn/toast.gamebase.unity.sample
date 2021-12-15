#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

using System.Collections.Generic;
using Toast.Gamebase.Internal;
using Toast.Gamebase.Internal.Single;
using Toast.Gamebase.Internal.Single.Communicator;
using UnityEngine;

namespace Toast.Gamebase.Adapter
{
    public class StandaloneWebviewAdapter : AdapterBase, IWebviewAdapter
    {
        private const string ADAPTER_VERSION = "2.2.10";

        public override string domain
        {
            get { return typeof(StandaloneWebviewAdapter).ToString(); }
        }

        public override string version
        {
            get { return ADAPTER_VERSION; }
        }
        
        public void ShowWebView(
            string url, 
            GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration = null, 
            GamebaseCallback.ErrorDelegate closeCallback = null, 
            List<string> schemeList = null, 
            GamebaseCallback.GamebaseDelegate<string> schemeEvent = null)
        {
            StandaloneWebview.Instance.ShowWebView(url, configuration, closeCallback, schemeList, schemeEvent);
        }

        public void ShowLoginWebView(
            string url,
            WebViewRequest.TitleBarConfiguration configuration,
            GamebaseCallback.ErrorDelegate closeCallback = null,
            List<string> schemeList = null,
            GamebaseCallback.GamebaseDelegate<string> schemeEvent = null)
        {
            StandaloneWebview.Instance.ShowLoginWebView(url, configuration, closeCallback, schemeList, schemeEvent);
        }

        public void SetTitleBarEnable(bool enable)
        {
            StandaloneWebview.Instance.SetTitleBarEnable(enable);
        }

        public void SetTitleVisible(bool isTitleVisible)
        {
            StandaloneWebview.Instance.SetTitleVisible(isTitleVisible);
        }

        public void SetTitleText(string title)
        {
            StandaloneWebview.Instance.SetTitleText(title);
        }

        public void SetTitleBarColor(Color bgColor)
        {
            StandaloneWebview.Instance.SetTitleBarColor(bgColor);
        }

        public void SetTitleBarButton(bool isBackButtonVisible, string backButtonName, string homeButtonName)
        {
            StandaloneWebview.Instance.SetTitleBarButton(isBackButtonVisible, backButtonName, homeButtonName);
        }

        public void SetWebViewRect(int titleBarHeight, Rect rect)
        {
            StandaloneWebview.Instance.SetWebViewRect(titleBarHeight, rect);
        }

        public void SetBgColor(Color bgColor)
        {
            StandaloneWebview.Instance.SetBgColor(bgColor);
        }

        public void CloseWebView()
        {
            StandaloneWebview.Instance.CloseWebView();
        }        
    }
}
#endif