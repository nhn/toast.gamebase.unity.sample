#if UNITY_EDITOR || UNITY_STANDALONE

using System;
using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single
{
    public class WebviewAdapterManager
    {
        public class SchemeInfo
        {
            public string scheme;
            public Dictionary<string, string> parameterDictionary;
        }

        private static readonly WebviewAdapterManager instance = new WebviewAdapterManager();

        public static WebviewAdapterManager Instance
        {
            get { return instance; }
        }

        public IWebviewAdapter adapter;

        public bool CreateWebviewAdapter(string moduleName)
        {
            if (adapter != null)
            {
                return true;
            }

            adapter = AdapterFactory.CreateAdapter<IWebviewAdapter>(moduleName);

            if (adapter ==  null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ShowWebView(
            string url,
            GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration = null,
            GamebaseCallback.ErrorDelegate closeCallback = null,
            List<string> schemeList = null,
            GamebaseCallback.GamebaseDelegate<string> schemeEvent = null)
        {
            if (adapter == null)
            {
                if (null != closeCallback)
                {
                    closeCallback(new GamebaseError(GamebaseErrorCode.WEBVIEW_UNKNOWN_ERROR, message: GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND));
                }
                return;
            }

            adapter.ShowWebView(url, configuration, closeCallback, schemeList, schemeEvent);
            int height = WebViewConst.TITLEBAR_DEFAULT_HIGHT;
            if (configuration != null)
            {
                height = configuration.barHeight;
            }
            adapter.SetWebViewRect(height, new Rect(0, 0, Screen.width, Screen.height));
        }

        public void ShowLoginWebView(
            string url,
            WebViewRequest.TitleBarConfiguration configuration = null,
            GamebaseCallback.ErrorDelegate closeCallback = null,
            List<string> schemeList = null,
            GamebaseCallback.GamebaseDelegate<string> schemeEvent = null)
        {
            if (adapter == null)
            {
                if (null != closeCallback)
                {
                    closeCallback(new GamebaseError(GamebaseErrorCode.WEBVIEW_UNKNOWN_ERROR, message: GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND));
                }
                return;
            }

            adapter.ShowLoginWebView(url, configuration, closeCallback, schemeList, schemeEvent);
            int height = WebViewConst.TITLEBAR_DEFAULT_HIGHT;
            if (configuration != null)
            {
                height = configuration.barHeight;
            }
            adapter.SetWebViewRect(height, new Rect(0, 0, Screen.width, Screen.height));
        }

        public void SetTitleBarEnable(bool enable)
        {
            adapter.SetTitleBarEnable(enable);
        }

        public void SetTitleVisible(bool isTitleVisible)
        {
            adapter.SetTitleVisible(isTitleVisible);
        }

        public void SetTitleText(string title)
        {
            adapter.SetTitleText(title);
        }

        public void SetTitleBarColor(Color bgColor)
        {
            adapter.SetTitleBarColor(bgColor);
        }

        public void SetTitleBarButton(bool isBackButtonVisible, string backButtonName, string homeButtonName)
        {
            adapter.SetTitleBarButton(isBackButtonVisible, backButtonName, homeButtonName);
        }


        public void SetWebViewRect(int titleBarHeight, Rect rect)
        {
            adapter.SetWebViewRect(titleBarHeight, rect);
        }

        public void SetBgColor(Color color)
        {
            adapter.SetBgColor(color);
        }

        public void CloseWebView()
        {
            if (adapter == null)
            {
                return;
            }

            adapter.CloseWebView();
        }

        public SchemeInfo ConvertURLToSchemeInfo(string url)
        {
            string[] urlParameters = url.Split(new char[] { '?', '&' });
            if (urlParameters == null || urlParameters.Length == 0)
                return null;

            SchemeInfo schemeInfo = new SchemeInfo();
            schemeInfo.scheme = Uri.UnescapeDataString(urlParameters[0]);

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            for (int i = 1; i < urlParameters.Length; i++)
            {
                string urlParameter = urlParameters[i];
                string[] parameter = urlParameter.Split('=');

                if (parameter == null || parameter.Length <= 1)
                {
                    continue;
                }

                string key = Uri.UnescapeDataString(parameter[0]);
                string value = Uri.UnescapeDataString(parameter[1]);

                if (parameters.ContainsKey(key) == true)
                {
                    continue;
                }

                parameters.Add(key, value);
            }

            schemeInfo.parameterDictionary = parameters;

            return schemeInfo;
        }
    }
}
#endif