#if UNITY_EDITOR || UNITY_STANDALONE

using System;
using System.Collections.Generic;

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
            if (null != adapter)
            {
                return true;
            }

            adapter = AdapterFactory.CreateAdapter<IWebviewAdapter>(moduleName);

            if (null == adapter)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ShowWebView(string url, GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration = null, GamebaseCallback.ErrorDelegate closeCallback = null, List<string> schemeList = null, GamebaseCallback.GamebaseDelegate<string> schemeEvent = null)
        {
            if (adapter == null)
            {
                if(null != closeCallback)
                {
                    closeCallback(new GamebaseError(GamebaseErrorCode.WEBVIEW_UNKNOWN_ERROR, message: GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND));
                }
                return;
            }

            adapter.ShowWebView(url, configuration, closeCallback, schemeList, schemeEvent);
        }

        public void CloseWebView()
        {
            if (null == adapter)
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

                if (null == parameter || 1 >= parameter.Length)
                {
                    continue;
                }
                                
                string key = Uri.UnescapeDataString(parameter[0]);
                string value = Uri.UnescapeDataString(parameter[1]);                

                if (true == parameters.ContainsKey(key))
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