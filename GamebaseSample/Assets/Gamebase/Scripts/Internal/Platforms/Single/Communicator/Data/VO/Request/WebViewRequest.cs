#if (UNITY_EDITOR || UNITY_STANDALONE)
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class WebViewRequest
    {
        public class Configuration
        {
            public enum WebviewType
            {
                Window,
                FloatingPopup
            }

            public void SetGamebaseRequest(GamebaseRequest.Webview.Configuration configuration)
            {
                webviewType = WebviewType.Window;
                viewRect = new Rect(0, 0, Screen.width, Screen.height);
                bgColor = GamebaseColor.RGB255(255, 255, 255);
                
                isNavigationBarVisible = configuration.isNavigationBarVisible;
                title = configuration.title;
                navigationColor = configuration.navigationColor;
                navigationTitleColor = configuration.navigationTitleColor;
                navigationIconTintColor = configuration.navigationIconTintColor;
                if(configuration.barHeight == -1)
                {
                    barHeight = WebViewConst.TITLEBAR_DEFAULT_HIGHT;
                }
                else
                {
                    barHeight = configuration.barHeight;
                }
                isBackButtonVisible = configuration.isBackButtonVisible;
                
                backButtonImageResource = configuration.backButtonImageResource;
                closeButtonImageResource = configuration.closeButtonImageResource;
            }

            public WebviewType webviewType = WebviewType.Window;
            
            public Rect viewRect = new Rect(0, 0, Screen.width, Screen.height);
            public GamebaseColor bgColor = GamebaseColor.RGB255(255, 255, 255);

            public bool isNavigationBarVisible = true;
            public string title = string.Empty;
            public GamebaseColor navigationColor = GamebaseColor.RGB255(18, 93, 230);
            public GamebaseColor navigationTitleColor = GamebaseColor.RGB255(255, 255, 255);
            public GamebaseColor navigationIconTintColor = null;
            public int barHeight = WebViewConst.TITLEBAR_DEFAULT_HIGHT;
            public bool isBackButtonVisible = true;
            public string backButtonImageResource = string.Empty;
            public string closeButtonImageResource = string.Empty;
        }
    }
}
#endif
