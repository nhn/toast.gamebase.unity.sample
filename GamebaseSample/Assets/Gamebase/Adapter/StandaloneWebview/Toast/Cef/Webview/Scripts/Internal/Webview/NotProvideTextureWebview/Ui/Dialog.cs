using UnityEngine;

namespace Toast.Cef.Webview.Internal.Ui
{
    public abstract class Dialog
    {
        public Texture2D Bg
        {
            get;
            set;
        }

        public GUIStyle Style
        {
            get;
            set;
        }

        public Rect DialogRect
        {
            get;
            set;
        }

        protected int referenceValue = 0;
        protected int minHeight = 0;

        public Dialog()
        {
            DialogRect = new Rect();
            minHeight = 60;

            SetTexture();
            SetStyle();
        }

        public abstract void SetDialog(Rect viewRect, string message);
        protected abstract void SetTexture();
        protected abstract void SetStyle();

        public void Initialize(int fontSize)
        {
            referenceValue = fontSize;
        }

        protected Vector2 CalculateDialogSize(string text, int referenceValue, int maxWidth, int maxHeight, int minHeight, int blankLineCount = 0)
        {
            string[] lines = text.Split('\n');
            int lineCount = lines.Length;

            var dialogSize = new Vector2
            {
                x = 0,
                y = (lineCount + blankLineCount) * referenceValue
            };

            int textLength = 0;
            int i;
            for (i = 0; i < lines.Length; ++i)
            {
                textLength = lines[i].Length;
                if (dialogSize.x < textLength)
                {
                    dialogSize.x = textLength;
                }
            }

            dialogSize.x *= (int)(referenceValue * 1.5f);

            if (dialogSize.y > maxHeight)
            {
                dialogSize.y = maxHeight;
            }
            else if (dialogSize.y < minHeight)
            {
                dialogSize.y = minHeight;
            }

            if (dialogSize.x > maxWidth)
            {
                dialogSize.x = maxWidth;
            }
            else if (dialogSize.x < (int)(maxWidth * 0.3f))
            {
                dialogSize.x = (int)(maxWidth * 0.3f);
            }

            return dialogSize;
        }
    }
}
