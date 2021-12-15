using System.Collections.Generic;
using System.Linq;

namespace Toast.Cef.Webview.Internal
{
    public class WebviewIndexManager
    {
        private static readonly WebviewIndexManager instance = new WebviewIndexManager();

        public static WebviewIndexManager Instance
        {
            get
            {
                return instance;
            }
        }

        private int index = 0;
        private Dictionary<int, Webview> webviewDictionary = new Dictionary<int, Webview>();

        public int AddWebview(Webview webview)
        {
            if (webviewDictionary.ContainsValue(webview) == true)
            {
                return GetWebviewIndex(webview);
            }

            index++;
            webviewDictionary[index] = webview;
            return index;
        }

        public void RemoveWebview(int webviewIndex)
        {
            if (webviewDictionary.ContainsKey(webviewIndex) == true)
            {
                webviewDictionary.Remove(webviewIndex);
            }
        }

        public Webview GetWebview(int index)
        {
            Webview webview = null;
            webviewDictionary.TryGetValue(index, out webview);
            return webview;
        }

        public int GetWebviewIndex(Webview webview)
        {
            return webviewDictionary.FirstOrDefault(pair => pair.Value == webview).Key;
        }

        public int GetWebviewCount()
        {
            return webviewDictionary.Count;
        }
    }
}