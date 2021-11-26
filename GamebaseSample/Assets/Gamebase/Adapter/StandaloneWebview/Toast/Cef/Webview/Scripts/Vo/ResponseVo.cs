using UnityEngine;

namespace Toast.Cef.Webview
{
    public static class ResponseVo
    {
        public class WebviewInfo
        {
            public int index;
            public Texture2D texture;
        }

        public class WebviewStatus
        {
            public int status;
            public PopupBlock popupBlock;
            public JsDialog jsDialog;
            public LoadEnd loadEnd;
            public PassPopupInfo passPopupInfo;

            public class PopupBlock
            {
                public string message;
            }

            public class JsDialog
            {
                public delegate void ClickButtonDelegate(bool isOkButton);

                public string message;
                public int type;
                public ClickButtonDelegate clickButtonDelegate;
            }

            public class LoadEnd
            {
                public string url;
            }

            public class PassPopupInfo
            {
                public string name;
                public string url;
                public int left;
                public int top;
                public int width;
                public int height;
                public bool menubar;
                public bool status;
                public bool toolbar;
                public bool location;
                public bool scrollbars;
                public bool resizable;
            }
        }
    }
}