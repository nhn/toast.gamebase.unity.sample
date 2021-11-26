#if UNITY_EDITOR || UNITY_STANDALONE

using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single
{
    public interface IWebviewAdapter
    {
        void ShowWebView(string url, GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration = null, GamebaseCallback.ErrorDelegate closeCallback = null, List<string> schemeList = null, GamebaseCallback.GamebaseDelegate<string> schemeEvent = null);
        void ShowLoginWebView(string url, WebViewRequest.TitleBarConfiguration configuration = null, GamebaseCallback.ErrorDelegate closeCallback = null, List<string> schemeList = null, GamebaseCallback.GamebaseDelegate<string> schemeEvent = null);
        void CloseWebView();

        void SetTitleBarEnable(bool enable);
        void SetTitleVisible(bool isTitleVisible);
        void SetTitleText(string title);
        void SetTitleBarColor(Color bgColor);
        void SetTitleBarButton(bool isBackButtonVisible, string backButtonName, string homeButtonName);
        void SetWebViewRect(int titleBarHeight, Rect rect);
        void SetBgColor(Color bgColor);
    }
}
#endif