#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Toast.Cef.Webview;
using Toast.Gamebase.Internal;

namespace Toast.Gamebase
{
    public class StandaloneWebview
    {
        private const int NONE_HANDLE = -1;
        private GamebaseCallback.ErrorDelegate closeCallback;
        private GamebaseCallback.GamebaseDelegate<string> schemeEvent;

        private static IMECompositionMode imeCompositionModeBackup;

        private StandaloneWebviewUI webviewUI;
        private List<string> schemeList;

        private readonly string popupBlockMessage = "New window is not supported!!";

        private static readonly StandaloneWebview instance = new StandaloneWebview();

        public static StandaloneWebview Instance
        {
            get { return instance; }
        }

        private StandaloneWebview()
        {
            if (webviewUI == null)
            {
                webviewUI = GamebaseComponentManager.AddComponent<StandaloneWebviewUI>(GamebaseGameObjectManager.GameObjectType.CORE_TYPE);                
                webviewUI.SetCallback(OnBackButton, OnCloseButton);
            }                
        }

#region Button
        private void OnBackButton()
        {
            CefManager.GetInstance().GoBack();
        }

        private void OnCloseButton()
        {
            CloseWebView();
        }
#endregion

        public void ShowWebView(string url, GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration, GamebaseCallback.ErrorDelegate closeCallback = null, List<string> schemeList = null, GamebaseCallback.GamebaseDelegate<string> schemeEvent = null)
        {
            imeCompositionModeBackup = Input.imeCompositionMode;
            Input.imeCompositionMode = IMECompositionMode.On;

            if (null != closeCallback)
            {
                this.closeCallback = closeCallback;
            }

            if (null != schemeEvent)
            {
                this.schemeEvent = schemeEvent;                
            }

            this.schemeList = schemeList;
            WebTitleDelegate webTitleDelegate = webviewUI.SetTitle;
            if(null != configuration && string.IsNullOrEmpty(configuration.title) == false)
            {
                webTitleDelegate = null;
                webviewUI.SetTitle(configuration.title);
            }
            
            SetConfiguration(configuration);

            webviewUI.SetActiveWebview(true);

            GamebaseLog.Debug(string.Format("url : {0}", url), this, "ShowWebView");

            GamebaseObserverManager.Instance.OnObserverEvent(
                new GamebaseResponse.SDK.ObserverMessage()
                {
                    type = GamebaseObserverType.WEBVIEW,
                    data = new Dictionary<string, object>()
                    {
                        { "code", GamebaseWebViewEventType.OPEN }
                    }
                }
                );

            CefManager.GetInstance().ShowWeb(
                            new Rect(0, webviewUI.GetTitleBarHeight(), Screen.width, Screen.height - webviewUI.GetTitleBarHeight()),
                            new StringBuilder(url),
                            SchemeEvent,
                            new StringBuilder(Gamebase.GetDeviceLanguageCode()),
                            webTitleDelegate,
                            WebInputElementFocus,
                            popupBlockMessage
                            );
            
            CefManager.GetInstance().SetInvalidRedirectUrlScheme(ConvertSchemeListToString(schemeList));            
        }

        public void CloseWebView()
        {
            Input.imeCompositionMode = imeCompositionModeBackup;

            webviewUI.HideWebviewUI();

            CefManager.GetInstance().HideWeb();

            if (null != closeCallback)
            {
                closeCallback(null);                
            }

            if(null != schemeEvent)
            {
                schemeEvent = null;
            }

            GamebaseObserverManager.Instance.OnObserverEvent(
                new GamebaseResponse.SDK.ObserverMessage()
                {
                    type = GamebaseObserverType.WEBVIEW,
                    data = new Dictionary<string, object>()
                    {
                        { "code", GamebaseWebViewEventType.CLOSE }
                    }
                }
                );
        }

        private void WebInputElementFocus(int type)
        {
            if (type == (int)INPUT_ELEMENT_TYPE.PASSWORD)
            {
                Input.imeCompositionMode = IMECompositionMode.Off;
            }
            else
            {
                Input.imeCompositionMode = IMECompositionMode.On;
            }
        }

        private void SetConfiguration(GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration)
        {
            if (null != configuration)
            {
                webviewUI.SetTitleBar(configuration.barHeight, new Color(configuration.colorR / 255f, configuration.colorG / 255f, configuration.colorB / 255f, 1));
                webviewUI.SetBackButtonVisible(configuration.isBackButtonVisible);
                webviewUI.SetButtonTexture(configuration.closeButtonImageResource, configuration.backButtonImageResource);
                webviewUI.SetTitle(configuration.title);
            }
            else
            {
                webviewUI.SetDefault();
            }
        }

        private void SchemeEvent(string url)
        {
            if (null != schemeEvent && null != schemeList)
            {
                foreach (string scheme in schemeList)
                {
                    if (url.Substring(0, scheme.Length).Equals(scheme) == true)
                    {
                        schemeEvent(url, null);
                        break;
                    }
                }
            }

            GamebaseLog.Debug(string.Format("SchemeEvent url : {0}", url), this, "SchemeEvent");
        }

        private StringBuilder ConvertSchemeListToString(List<string> schemeList)
        {
            StringBuilder schemes = new StringBuilder();

            if(null != schemeList)
            {
                foreach (string scheme in schemeList)
                {
                    schemes.Append(scheme).Append(";");
                }
            }

            return schemes;
        }
    }
}
#endif