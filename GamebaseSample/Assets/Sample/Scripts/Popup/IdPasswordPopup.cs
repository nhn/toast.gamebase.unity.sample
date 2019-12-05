using System;
using UnityEngine;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class IdPasswordPopup : MonoBehaviour
    {
        [SerializeField]
        private Text titleText;
        [SerializeField]
        private Text messageText;
        [SerializeField]
        private InputField idInput;
        [SerializeField]
        private InputField passwordInput;
        [SerializeField]
        private Text okButtonText;
        [SerializeField]
        private Text cancelButtonText;

        private Action<string, string> okCallback;
        private Action cancelCallback;

        public void SetPopup(
            string title,
            string message,
            string id,
            string password,
            string okText,
            Action<string, string> okCallback,
            string cancelText = null,
            Action cancelCallback = null)
        {
            SetText(title, message, id, password);
            SetButton(okText, okCallback, cancelText, cancelCallback);
        }

        private void SetButton(string okText, Action<string, string> okCallback, string cancelText, Action cancelCallback = null)
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
        }

        private void SetText(string title, string message, string id, string password)
        {
            titleText.text = title;
            messageText.text = message;
            idInput.text = id;
            passwordInput.text = password;
        }

        #region UIButton.onClick
        public void ClickOKButton()
        {
            if (okCallback != null)
            {
                okCallback(idInput.text, passwordInput.text);
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
        #endregion
    }
}