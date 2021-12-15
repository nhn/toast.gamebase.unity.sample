namespace Toast.Cef.Webview.Internal
{
    public class ProvideTextureWebview : Webview
    {
        private string blockPopupMessage;

        protected override void OpenDialog(int type, string message)
        {
            if (statusDelegate == null)
            {
                return;
            }

            var vo = new ResponseVo.WebviewStatus
            {
                status = WebUpdateStatus.JSDIALOG,
                jsDialog = new ResponseVo.WebviewStatus.JsDialog
                {
                    message = message,
                    type = type,
                    clickButtonDelegate = (isOkButton) =>
                    {
                        if (isOkButton == true)
                        {
                            NativeMethods.InputWeb(index, WebInput.JSDIALOG, 1, 0);
                        }
                        else
                        {
                            NativeMethods.InputWeb(index, WebInput.JSDIALOG, 0, 0);
                        }
                    }
                }      
            };

            statusDelegate(vo);
        }

        protected override void OpenPopupBlock()
        {
            if (statusDelegate == null)
            {
                return;
            }

            var vo = new ResponseVo.WebviewStatus
            {
                status = WebUpdateStatus.POPUPBLOCK,
                popupBlock = new ResponseVo.WebviewStatus.PopupBlock
                {
                    message = blockPopupMessage
                }
            };

            statusDelegate(vo);
        }

        protected override void SetBlockPopupMessage(string message)
        {
            blockPopupMessage = message;
        }

        protected override void OnGUIMessage()
        {
        }
    }
}