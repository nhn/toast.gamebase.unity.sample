#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class WebViewRequest
    {
        public class TitleBarConfiguration
        {
            public string title;
            public Color bgColor;
            public Color titleColor;
            public int barHeight;
        }
    }
}
#endif
