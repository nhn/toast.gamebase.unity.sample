#if UNITY_EDITOR || UNITY_STANDALONE

using System;
using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;

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
            WebViewRequest.Configuration configuration,
            GamebaseCallback.ErrorDelegate closeCallback = null,
            List<string> schemeList = null,
            GamebaseCallback.GamebaseDelegate<string> schemeEvent = null,
            GamebaseCallback.VoidDelegate callback = null)
        {
            if (adapter == null)
            {
                if (null != closeCallback)
                {
                    closeCallback(new GamebaseError(GamebaseErrorCode.WEBVIEW_UNKNOWN_ERROR, message: GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND));
                }
                return;
            }

            //--------------------------------------------------
            //
            //  override callback.
            //
            //--------------------------------------------------
            adapter.ShowWebView(url, configuration, closeCallback, schemeList, schemeEvent, () =>
            {
                callback?.Invoke();
            });
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