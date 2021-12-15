using Toast.Cef.Webview.Internal.Ui;
using UnityEngine;

namespace Toast.Cef.Webview.Internal
{
    public class NotProvideTextureWebview : Webview
    {
        private JsDialog jsDialog;
        private PopupBlockDialog popupBlockDialog;

        private int jsDialogType;
        private string jsDialogMessage;

        private bool isOpenPopupDialog;
        private string popupDialogMessage;
        private float popupBlockNotiTime = 1.0f;
        private float popupBlockNotiStartTime = 0.0f;

        private int fontSize;

        public void Awake()
        {
            jsDialog = new JsDialog();
            popupBlockDialog = new PopupBlockDialog();
        }

        #region override
        protected override Texture2D GetTexture()
        {
            return null;
        }

        protected override void OpenDialog(int type, string message)
        {
            jsDialogType = type;
            jsDialogMessage = message;

            jsDialog.SetDialog(viewRect, message);
        }

        protected override void OpenPopupBlock()
        {
            isOpenPopupDialog = true;
            popupBlockNotiStartTime = Time.realtimeSinceStartup;
        }

        protected override void SetBlockPopupMessage(string message)
        {
            popupDialogMessage = message;
            jsDialog.SetDialog(viewRect, message);
        }

        protected override void OnGUIMessage()
        {
            if (fontSize == 0)
            {
                fontSize = GUI.skin.font.fontSize;

                jsDialog.Initialize(fontSize);
                popupBlockDialog.Initialize(fontSize);

                popupBlockDialog.SetDialog(viewRect, "");
            }

            if (string.IsNullOrEmpty(jsDialogMessage) == false)
            {
                JsDialogUI();
                return;
            }

            if (isOpenPopupDialog == true)
            {
                PopupDialogUI();
            }

            float mouseX = Input.mousePosition.x - viewRect.x;
            float mouseY = Screen.height - Input.mousePosition.y - viewRect.y;
            SetMousePosition(new Vector2(mouseX, mouseY), (error) =>
            {
                if (CefWebview.IsSuccess(error) == false)
                {
                    CefWebviewLogger.Debug(string.Format("SetMousePosition failed. error:{0}", error), GetType());
                }
            });
        }

        protected override void RenderTexture(Color32[] buffer)
        {
            base.RenderTexture(buffer);

            if (isShow == true)
            {
                GUI.DrawTexture(viewRect, texture, ScaleMode.ScaleAndCrop, false, 0);
            }
        }
        #endregion

        #region OnGUI
        private void JsDialogUI()
        {
            GUI.Box(jsDialog.DialogRect, jsDialogMessage, jsDialog.Style);

            if (jsDialogType == JsDialogType.ALERT)
            {
                if (GUI.Button(jsDialog.RightButtonRect, "OK", jsDialog.ButtonStyle) == true)
                {
                    jsDialogMessage = string.Empty;

                    NativeMethods.InputWeb(index, WebInput.JSDIALOG, 1, 0);
                }
            }
            else
            {
                if (GUI.Button(jsDialog.LeftButtonRect, "OK", jsDialog.ButtonStyle) == true)
                {
                    jsDialogMessage = string.Empty;

                    NativeMethods.InputWeb(index, WebInput.JSDIALOG, 1, 0);
                }
                else if (GUI.Button(jsDialog.RightButtonRect, "Cancel", jsDialog.ButtonStyle) == true)
                {
                    jsDialogMessage = string.Empty;
                    NativeMethods.InputWeb(index, WebInput.JSDIALOG, 0, 0);
                }
            }
        }

        private void PopupDialogUI()
        {
            GUI.Box(popupBlockDialog.DialogRect, popupDialogMessage, popupBlockDialog.Style);

            if (Time.realtimeSinceStartup - popupBlockNotiStartTime >= popupBlockNotiTime)
            {
                isOpenPopupDialog = false;
            }
        }
        #endregion
    }
}