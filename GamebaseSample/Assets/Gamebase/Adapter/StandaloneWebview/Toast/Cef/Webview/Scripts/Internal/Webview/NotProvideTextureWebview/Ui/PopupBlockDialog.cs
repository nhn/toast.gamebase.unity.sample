using UnityEngine;

namespace Toast.Cef.Webview.Internal.Ui
{
    public class PopupBlockDialog : Dialog
    {
        private const string DEFAULT_MESSAGE = "게임내에서 사용할 수 없는 링크입니다.";

        public override void SetDialog(Rect viewRect, string message)
        {
            if (string.IsNullOrEmpty(message) == true)
            {
                message = DEFAULT_MESSAGE;
            }

            var dialogSize = CalculateDialogSize(message, referenceValue, (int)viewRect.width, (int)viewRect.height, minHeight);

            DialogRect = new Rect
            {
                width = dialogSize.x,
                height = dialogSize.y,
                x = (viewRect.width - dialogSize.x) * 0.5f,
                y = (viewRect.height - dialogSize.y) * 0.5f
            };
        }

        protected override void SetTexture()
        {
            Color32 cornerColor = new Color32(0xff, 0xff, 0xff, 0x00);
            Color32 borderColor = new Color32(0xb7, 0xa2, 0x73, 0xff);
            Color32 bgColor = new Color32(0xf9, 0xf8, 0xc3, 0xff);
            Bg = new Texture2D(4, 4, TextureFormat.ARGB32, false, false);

            Bg.SetPixels32(new Color32[] { cornerColor, borderColor, borderColor, cornerColor,
                                                           borderColor, bgColor,     bgColor,     borderColor,
                                                           borderColor, bgColor,     bgColor,     borderColor,
                                                           cornerColor, borderColor, borderColor, cornerColor });
            Bg.Apply(false, false);
        }

        protected override void SetStyle()
        {
            Style = new GUIStyle();
            Style.alignment = TextAnchor.MiddleCenter;
            Style.border = new RectOffset(2, 2, 2, 2);
            Style.normal.background = Bg;
            Style.normal.textColor = new Color(0.4705f, 0.3451f, 0.1921f);
        }
    }
}