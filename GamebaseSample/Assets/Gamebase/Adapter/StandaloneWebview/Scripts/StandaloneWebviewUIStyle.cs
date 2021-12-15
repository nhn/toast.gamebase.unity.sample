using UnityEngine;

namespace Toast.Gamebase.Adapter.Ui
{
    public static class StandaloneWebviewUIStyle
    {
        public static readonly GUIStyle TitleLabel;
        public static readonly GUIStyle CloseButton;
        public static readonly GUIStyle BackButton;

        static StandaloneWebviewUIStyle()
        {
            TitleLabel = new GUIStyle()
            {
                name = "TitleLabelStyle",
                normal = { textColor = Color.white },
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter
            };

            CloseButton = new GUIStyle()
            {
                name = "CloseButtonStyle",
            };

            BackButton = new GUIStyle()
            {
                name = "BackButtonStyle",
            };
        }

        static public void SetTitleTextColor(Color color)
        {
            TitleLabel.normal.textColor = color;
        }
    }
}