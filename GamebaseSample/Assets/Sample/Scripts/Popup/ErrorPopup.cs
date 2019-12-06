using UnityEngine;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class ErrorPopup : MonoBehaviour
    {
        [SerializeField]
        private Text message;
        [SerializeField]
        private Text okButtonText;
        [SerializeField]
        private Text cancelButtonText;
        [SerializeField]
        private GameObject okGameObject;
        [SerializeField]
        private GameObject closeGameObject;
        [SerializeField]
        private GameObject detailGameObject;

        private System.Action okCallback;
        private System.Action cancelCallback;

        private string detailMessage;


        public void SetPopup(string message, string detailMessage, string okText, System.Action okCallback, string cancelText = null, System.Action cancelCallback = null)
        {
            this.message.text = message;

            if (string.IsNullOrEmpty(detailMessage) == true)
            {
                this.detailMessage = string.Empty;
            }
            else
            {
                try
                {
                    this.detailMessage = JsonUtil.ToPrettyJsonString(detailMessage);
                }
                catch
                {
                    this.detailMessage = detailMessage;
                }
            }

            SetButton(okText, okCallback, cancelText, cancelCallback);
        }

        private void SetButton(string okText, System.Action okCallback, string cancelText, System.Action cancelCallback = null)
        {
            this.okCallback = okCallback;
            this.cancelCallback = cancelCallback;

            if (string.IsNullOrEmpty(okText) == false)
            {
                okButtonText.text = okText;
            }

            if (string.IsNullOrEmpty(cancelText) == false)
            {
                cancelButtonText.text = cancelText;
            }

            if (string.IsNullOrEmpty(cancelText) == true)
            {
                closeGameObject.SetActive(false);
            }

            if (string.IsNullOrEmpty(detailMessage) == true)
            {
                detailGameObject.SetActive(false);
            }
        }

        #region UIButton.onClick
        public void ClickOKButton()
        {
            if (okCallback != null)
            {
                okCallback();
            }

            Destroy(gameObject);
        }

        public void ClickCloseButton()
        {
            if (cancelCallback != null)
            {
                cancelCallback();
            }

            Destroy(gameObject);
        }

        public void ClickDetailButton()
        {
            PopupManager.ShowErrorDetailPopup(
                transform.parent.gameObject,
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.DETAIL_BUTTON),
                detailMessage,
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                null);
        }
        #endregion
    }
}