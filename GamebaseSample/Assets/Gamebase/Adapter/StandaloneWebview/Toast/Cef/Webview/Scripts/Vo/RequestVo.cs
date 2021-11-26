using UnityEngine;

namespace Toast.Cef.Webview
{
    public static class RequestVo
    {
        public class WebviewConfiguration
        {
            public bool useTexture;
            public Rect viewRect;

            // See the Toast.Cef.Webview.BgType
            public int bgType;
            public PopupOption popupOption;

            public class PopupOption
            {
                // See the Toast.Cef.Webview.PopupType class.
                public int type;
                public string blockMessage;
            }
        }
    }
}