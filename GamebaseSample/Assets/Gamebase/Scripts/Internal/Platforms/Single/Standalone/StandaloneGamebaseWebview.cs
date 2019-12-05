#if UNITY_EDITOR || UNITY_STANDALONE

using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseWebview : CommonGamebaseWebview
    {
        public StandaloneGamebaseWebview()
        {
            Domain = typeof(StandaloneGamebaseWebview).Name;
        }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        public override void ShowWebView(string url, GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration = null, int closeCallbackHandle = -1, List<string> schemeList = null, int schemeEventHandle = -1)
        {
            GamebaseCallback.ErrorDelegate closeCallback = null;
            if (-1 != closeCallbackHandle)
            {
                closeCallback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(closeCallbackHandle);
                GamebaseCallbackHandler.UnregisterCallback(closeCallbackHandle);
            }

            GamebaseCallback.GamebaseDelegate<string> schemeEvent = null;
            if (-1 != schemeEventHandle)
            {
                schemeEvent = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<string>>(schemeEventHandle);
                GamebaseCallbackHandler.UnregisterCallback(schemeEventHandle);
            }

            bool hasAdapter = WebviewAdapterManager.Instance.CreateWebviewAdapter("standalonewebviewadapter");
            if (false == hasAdapter)
            {
                GamebaseLog.Warn(GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND, this);
                if (null != closeCallback)
                {
                    closeCallback(new GamebaseError(GamebaseErrorCode.WEBVIEW_UNKNOWN_ERROR, message: GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND));
                }

                return;
            }

            WebviewAdapterManager.Instance.ShowWebView(url, configuration, closeCallback, schemeList, schemeEvent);
        }

        public override void CloseWebView()
        {
            bool hasAdapter = WebviewAdapterManager.Instance.CreateWebviewAdapter("standalonewebviewadapter");
            if (false == hasAdapter)
            {
                GamebaseLog.Warn(GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND, this);

                return;
            }

            WebviewAdapterManager.Instance.CloseWebView();
        }
#endif
    }
}
#endif