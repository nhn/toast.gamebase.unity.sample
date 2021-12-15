#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using System;
using UnityEngine;

namespace Toast.Gamebase.Adapter.Ui
{
    public class StandaloneWebviewUI : MonoBehaviour
    {
        public string Title
        {
            get;
            set;
        }

        public int TitleBarHeight
        {
            get;
            private set;
        }

        public bool IsActivated
        {
            private get;
            set;
        }

        public bool IsTitleVisible
        {
            private get;
            set;
        }

        public bool IsBackButtonVisible
        {
            private get;
            set;
        }        

        public Action OnBackButton
        {
            private get;
            set;
        }

        public Action OnCloaseButton
        {
            private get;
            set;
        }

        private const int PADDING = 9;
        private const int TITLE_BAR_TEXTURE_WIDTH = 100;
        private const int TITLE_BAR_TEXTURE_HEIGHT = 100;

        private const string DEFAILT_TEXTURE_CLOSE = "gamebase-close-white";
        private const string DEFAILT_TEXTURE_BACK = "gamebase-back-white";

        private Texture bgTexture;
        private Rect webViewRect;

        private Texture titleBarTexture;
        private Rect titleBarRect;

        private Vector2 backButtonSize;
        private Vector2 closeButtonSize;

        private string loadedBackButtonTextureName;
        private string loadedCloseButtonTextureName;
        
        public void Initialize()
        {
            SetTitleBarColor(new Color(75f / 255f, 150f / 255f, 230f / 255f, 255f / 255f));
        }

        public void SetTitleBarEnable(bool enable)
        {
            IsActivated = enable;
        }

        public void SetTitleTextColor(Color color)
        {
            StandaloneWebviewUIStyle.SetTitleTextColor(color);
        }

        public void SetTitleVisible(bool isTitleVisible)
        {
            IsTitleVisible = isTitleVisible;
        }

        public void SetTitleText(string title)
        {
            Title = title;
        }

        public void SetTitleBarRect(Rect rect)
        {
            titleBarRect = rect;
            TitleBarHeight = (int)rect.height;
        }

        public void SetTitleBarColor(Color barColor)
        {            
            titleBarTexture = MakeTexture(barColor);
        }

        public void SetTitleBarButton(bool isBackButtonVisible, string backButtonName, string closeButtonName)
        {
            IsBackButtonVisible = isBackButtonVisible;
            SetButton(backButtonName, DEFAILT_TEXTURE_BACK, ref loadedBackButtonTextureName, ref backButtonSize, StandaloneWebviewUIStyle.BackButton);
            SetButton(closeButtonName, DEFAILT_TEXTURE_CLOSE, ref loadedCloseButtonTextureName, ref closeButtonSize, StandaloneWebviewUIStyle.CloseButton);
        }

        public void SetBgColor(Color bgColor)
        {
            bgTexture = MakeTexture(bgColor);
        }

        public void SetBGRect(Rect webViewRect)
        {
            this.webViewRect = webViewRect;
        }

        private void OnGUI()
        {
            if (IsActivated == false)
            {
                return;
            }
            
            DrawBG();
            DrawTitleBarBG();
            DrawTitleText();
            DrawBackButton();
            DrawCloseButton();
        }

        private void DrawBG()
        {            
            // TOP
            GUI.DrawTexture(
                new Rect(
                    0, 
                    0, 
                    Screen.width, 
                    webViewRect.y), 
                bgTexture);

            // BOTTOM
            GUI.DrawTexture(
                new Rect(
                    0,
                    webViewRect.y + webViewRect.height,
                    Screen.width,
                    Screen.height - (webViewRect.y + webViewRect.height)),
                bgTexture);

            // LEFT
            GUI.DrawTexture(
                new Rect(
                    0,
                    webViewRect.y,
                    (Screen.width - webViewRect.width) / 2,
                    webViewRect.height),
                bgTexture);

            // RIGHT
            GUI.DrawTexture(
                new Rect(
                    webViewRect.width + ((Screen.width - webViewRect.width) / 2),
                    webViewRect.y,
                    (Screen.width - webViewRect.width) / 2,
                    webViewRect.height),
                bgTexture);
        }

        private void DrawTitleBarBG()
        {
            GUI.DrawTexture(titleBarRect, titleBarTexture);
        }

        private void DrawTitleText()
        {
            if (string.IsNullOrEmpty(Title) == true || IsTitleVisible == false)
            {
                return;
            }

            int textWidth = (int)titleBarRect.width - (PADDING * 2);
            Vector2 textVector = StandaloneWebviewUIStyle.TitleLabel.CalcSize(new GUIContent(Title));

            int textTop = (int)titleBarRect.y + (TitleBarHeight - StandaloneWebviewUIStyle.TitleLabel.fontSize) / 2;

            string titleText = string.Empty;

            if (textVector.x > textWidth)
            {
                for (int i = 0; i < Title.Length; i++)
                {
                    Vector2 size = StandaloneWebviewUIStyle.TitleLabel.CalcSize(new GUIContent(Title.Substring(0, i) + "..."));
                    if (size.x > textWidth)
                    {
                        break;
                    }
                    titleText = Title.Substring(0, i) + "...";
                }
            }
            else
            {
                titleText = Title;
            }

            GUI.Label(
                new Rect(titleBarRect.x + PADDING, textTop, textWidth, 20),
                titleText,
                StandaloneWebviewUIStyle.TitleLabel);
        }
                
        public void DrawBackButton()
        {
            if (IsBackButtonVisible == false || OnBackButton == null)
            {
                return;
            }

            var rect = new Rect(
                titleBarRect.x + PADDING,
                titleBarRect.y + (TitleBarHeight - backButtonSize.y) / 2, 
                backButtonSize.x, 
                backButtonSize.y);
            if (GUI.Button(rect, string.Empty, StandaloneWebviewUIStyle.BackButton) == true)
            {
                OnBackButton();
            }
        }

        private void DrawCloseButton()
        {
            var rect = new Rect(
                titleBarRect.x + titleBarRect.width - closeButtonSize.x - PADDING,
                titleBarRect.y + (TitleBarHeight - closeButtonSize.y) / 2,
                closeButtonSize.x,
                closeButtonSize.y);
            if (GUI.Button(rect, string.Empty, StandaloneWebviewUIStyle.CloseButton) == true)
            {
                if (OnCloaseButton != null)
                {
                    OnCloaseButton();
                }
            }
        }

        private Texture2D MakeTexture(Color color)
        {
            Texture2D texture = new Texture2D(TITLE_BAR_TEXTURE_WIDTH, TITLE_BAR_TEXTURE_HEIGHT);

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            return texture;
        }

        private void SetButton(
            string resourceName,
            string defaultResourceName,
            ref string loadedResourceName,
            ref Vector2 buttonSize,
            GUIStyle style)
        {
            if (string.IsNullOrEmpty(resourceName) == true)
            {
                resourceName = defaultResourceName;
            }

            if (resourceName.Equals(loadedResourceName) == false)
            {
                var texture = Resources.Load(resourceName) as Texture2D;
                style.normal.background = texture;
                style.hover.background = AddTextureColor(texture, new Color(-0.1f, -0.1f, -0.1f, 0));
                style.active.background = AddTextureColor(texture, new Color(-0.2f, -0.2f, -0.2f, 0));
                buttonSize = new Vector2(texture.width, texture.height);

                loadedResourceName = resourceName;
            }
        }

        private Texture2D AddTextureColor(Texture2D targetTexture, Color addColor)
        {
            Texture2D texture = new Texture2D(targetTexture.width, targetTexture.height);

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    Color color = targetTexture.GetPixel(x, y);
                    color += addColor;
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            return texture;
        }
    }
}
#endif