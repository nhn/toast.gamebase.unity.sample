using UnityEngine;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class CommonPopup : MonoBehaviour
    {
        [SerializeField]
        private Text title = null;
        [SerializeField]
        private Text message = null;
        [SerializeField]
        private Text okButtonText = null;
        [SerializeField]
        private Text cancelButtonText = null;

        [SerializeField]
        private GameObject closeGameObject = null;

        private System.Action okCallback;
        private System.Action cancelCallback;

        public void SetPopup(string title, string message, string okText, System.Action okCallback, string cancelText = null, System.Action cancelCallback = null)
        {
            SetText(title, message);
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
                title.transform.localPosition = new Vector3(0f, title.transform.localPosition.y, 0f);
            }
        }

        private void SetText(string title, string message)
        {
            this.title.text = title;
            this.message.text = message;
        }

        #region UIButton.onClick
        public void ClickOKButton()
        {
            if (null != okCallback)
            {
                okCallback();
            }

            Destroy(gameObject);
        }

        public void ClickCloseButton()
        {
            if (null != cancelCallback)
            {
                cancelCallback();
            }

            Destroy(gameObject);
        }
        #endregion
    }
}