using UnityEngine;

namespace Toast.Cef.Webview.Internal.Ui
{
    public class JsDialog : Dialog
    {
        public Texture2D ButtonBg
        {
            get;
            set;
        }

        public GUIStyle ButtonStyle
        {
            get;
            set;
        }

        public Rect RightButtonRect
        {
            get;
            set;
        }

        public Rect LeftButtonRect
        {
            get;
            set;
        }

        public override void SetDialog(Rect viewRect, string message)
        {
            if (string.IsNullOrEmpty(message) == true)
            {
                return;
            }

            int buttonWidth = referenceValue * 4;
            int buttonHeight = referenceValue * 3;
            int buttonHeightWeight = (int)(buttonHeight * 1.5f);
            int topPadding = 5;

            var dialogSize = CalculateDialogSize(message, referenceValue, (int)viewRect.width, (int)viewRect.height, minHeight, 1);

            DialogRect = new Rect
            {
                width = dialogSize.x,
                height = dialogSize.y + buttonHeightWeight + topPadding,
                x = (viewRect.width - dialogSize.x) * 0.5f,
                y = viewRect.y
            };

            Style.padding = new RectOffset(0, 0, topPadding, buttonHeightWeight);

            RightButtonRect = new Rect
            {
                width = buttonWidth,
                height = buttonHeight,
                x = (DialogRect.x + DialogRect.width) - (int)(buttonWidth * 1.5f),
                y = DialogRect.y + DialogRect.height - buttonHeightWeight
            };

            LeftButtonRect = new Rect
            {
                width = RightButtonRect.width,
                height = RightButtonRect.height,
                x = RightButtonRect.x - RightButtonRect.width - topPadding,
                y = RightButtonRect.y
            };
        }

        protected override void SetTexture()
        {
            Color32 cornerColor = new Color32(0xff, 0xff, 0xff, 0x00);
            Color32 borderColor = new Color32(0xc5, 0xc5, 0xc5, 0xff);
            Color32 bgColor = new Color32(0xfb, 0xfb, 0xfb, 0xff);
            Bg = new Texture2D(4, 4, TextureFormat.ARGB32, false, false);

            Bg.SetPixels32(new Color32[]
            {
                cornerColor, borderColor, borderColor, cornerColor,
                borderColor, bgColor,     bgColor,     borderColor,
                borderColor, bgColor,     bgColor,     borderColor,
                cornerColor, borderColor, borderColor, cornerColor
            });

            Bg.Apply(false, false);

            Color32 buttonnCornerColor = new Color32(0xc5, 0xd7, 0xfa, 0x00);
            Color32 buttonnBorderColor = new Color32(0x3e, 0x7e, 0xf8, 0xff);
            Color32 buttonnBgColor = new Color32(0xf9, 0xf9, 0xf9, 0xff);
            ButtonBg = new Texture2D(4, 4, TextureFormat.ARGB32, false, false);

            ButtonBg.SetPixels32(new Color32[]
            {
                buttonnCornerColor, buttonnBorderColor, buttonnBorderColor, buttonnCornerColor,
                buttonnBorderColor, buttonnBgColor,     buttonnBgColor,     buttonnBorderColor,
                buttonnBorderColor, buttonnBgColor,     buttonnBgColor,     buttonnBorderColor,
                buttonnCornerColor, buttonnBorderColor, buttonnBorderColor, buttonnCornerColor
            });

            ButtonBg.Apply(false, false);
        }

        protected override void SetStyle()
        {
            Style = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                border = new RectOffset(2, 2, 2, 2)
            };
            Style.normal.background = Bg;
            Style.normal.textColor = Color.black;

            ButtonStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                border = new RectOffset(2, 2, 2, 2)
            };
            ButtonStyle.normal.background = ButtonBg;
            ButtonStyle.normal.textColor = Color.black;
        }
    }
}