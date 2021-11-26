#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebaseWebview : IGamebaseWebview
    {
        protected static class GamebaseWebview
        {
            public const string WEBVIEW_API_OPEN_WEBBROWSER              = "gamebase://openWebBrowser";
            public const string WEBVIEW_API_SHOW_WEBVIEW                 = "gamebase://showWebView";            
            public const string WEBVIEW_API_CLOSE_WEBVIEW                = "gamebase://closeWebView";
            public const string WEBVIEW_API_SCHEME_EVENT                 = "gamebase://schemeEvent";
        }

        public class ExtraData
        {
            public List<string> schemeList;
            public int schemeEvent;

            public ExtraData(List<string> schemeList, int schemeEvent)
            {
                this.schemeList = schemeList;
                this.schemeEvent = schemeEvent;
            }
        }

        protected INativeMessageSender  messageSender   = null;
        protected string                CLASS_NAME      = string.Empty;

        public NativeGamebaseWebview()
        {
            Init();
        }

        protected virtual void Init()
        {
            messageSender.Initialize(CLASS_NAME);

            DelegateManager.AddDelegate(GamebaseWebview.WEBVIEW_API_SHOW_WEBVIEW, DelegateManager.SendErrorDelegateOnce, OnCloseCallback);
            DelegateManager.AddDelegate(GamebaseWebview.WEBVIEW_API_SCHEME_EVENT, DelegateManager.SendGamebaseDelegate<string>);
        }

        public virtual void OpenWebBrowser(string url)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseWebview.WEBVIEW_API_OPEN_WEBBROWSER, 
                    jsonData: url
                    ));
            messageSender.GetAsync(jsonData);
        }

        public virtual void ShowWebView(string url, GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration = null, int closeCallback = -1, List<string> schemeList = null, int schemeEvent = -1)
        {
            NativeRequest.Webview.WebviewConfiguration webviewConfiguration = new NativeRequest.Webview.WebviewConfiguration();
            webviewConfiguration.url                                        = url;
            webviewConfiguration.configuration                              = configuration;

            string extraData    = JsonMapper.ToJson(new ExtraData(schemeList, schemeEvent));
            string jsonData     = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseWebview.WEBVIEW_API_SHOW_WEBVIEW, 
                    handle: closeCallback, 
                    jsonData: JsonMapper.ToJson(webviewConfiguration), 
                    extraData: extraData
                    ));
            messageSender.GetAsync(jsonData);
        }

        public virtual void CloseWebView()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(GamebaseWebview.WEBVIEW_API_CLOSE_WEBVIEW));
            messageSender.GetAsync(jsonData);
        }

        protected virtual void OnCloseCallback(NativeMessage message)
        {
            if (string.IsNullOrEmpty(message.extraData) == false)
            {
                int schemeEventHandle = int.Parse(message.extraData);
                GamebaseCallbackHandler.UnregisterCallback(schemeEventHandle);
            }
        }
    }
}
#endif