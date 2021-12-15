#if !UNITY_EDITOR && UNITY_ANDROID
using Toast.Gamebase.Internal.Mobile.Android;
#elif !UNITY_EDITOR && UNITY_IOS
using Toast.Gamebase.Internal.Mobile.IOS;
#elif !UNITY_EDITOR && UNITY_WEBGL
using Toast.Gamebase.Internal.Single.WebGL;
#else
using Toast.Gamebase.Internal.Single.Standalone;
#endif
using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public sealed class GamebaseWebviewImplementation
    {
        private static readonly GamebaseWebviewImplementation instance = new GamebaseWebviewImplementation();
        
        public static GamebaseWebviewImplementation Instance
        {
            get { return instance; }
        }

        IGamebaseWebview webview;

        private GamebaseWebviewImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            webview = new AndroidGamebaseWebview();
#elif !UNITY_EDITOR && UNITY_IOS
            webview = new IOSGamebaseWebview();
#elif !UNITY_EDITOR && UNITY_WEBGL
            webview = new WebGLGamebaseWebview();
#else
            webview = new StandaloneGamebaseWebview();
#endif
        }

        public void OpenWebBrowser(string url)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            webview.OpenWebBrowser(url);
        }

        public void ShowWebView(string url, GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration = null, GamebaseCallback.ErrorDelegate closeCallback = null, List<string> schemeList = null, GamebaseCallback.GamebaseDelegate<string> schemeEvent = null)
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            int closeCallbackHandle = -1;
            int schemeEventHandle = -1;

            if (null != closeCallback)
            {
                closeCallbackHandle =  GamebaseCallbackHandler.RegisterCallback(closeCallback);
            }

            if (null != schemeEvent)
            {
                schemeEventHandle = GamebaseCallbackHandler.RegisterCallback(schemeEvent);
            }

            webview.ShowWebView(url, configuration, closeCallbackHandle, schemeList, schemeEventHandle);
        }

        public void CloseWebView()
        {
            GamebaseGameInformationReport.Instance.AddApiName();
            webview.CloseWebView();
        }
    }
}