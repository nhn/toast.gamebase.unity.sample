using UnityEngine;

namespace GamebaseSample
{
    public class PopupManager : MonoBehaviour
    {
        public enum CommonPopupType
        {
            SMALL_SIZE,
            LARGE_SIZE
        }

        public static GameObject ShowPopup(GameObject parent, string popupName)
        {
            string path = string.Format("Prefabs/UI/{0}", popupName);

            GameObject popupGameObject = Resources.Load(path) as GameObject;

            popupGameObject = Instantiate(popupGameObject, parent.transform, true);
            popupGameObject.transform.localScale = new Vector3(1, 1, 1);
            popupGameObject.transform.localPosition = Vector3.zero;

            popupGameObject.transform.SetAsLastSibling();

            return popupGameObject;
        }

        public static GameObject ShowCommonPopup(GameObject parent, CommonPopupType popupType, string title, string message, string okText, System.Action okCallback, string cancelText = null, System.Action cancelCallback = null)
        {
            CommonPopup commonPopup = null;

            if (CommonPopupType.SMALL_SIZE == popupType)
            {
                commonPopup = ShowPopup(parent, "CommonSPopup").GetComponent<CommonPopup>();
            }
            else
            {
                commonPopup = ShowPopup(parent, "CommonLPopup").GetComponent<CommonPopup>();
            }

            commonPopup.SetPopup(
                title,
                message,
                okText,
                okCallback,
                cancelText,
                cancelCallback
            );

            return commonPopup.gameObject;
        }

        public static GameObject ShowErrorPopup(GameObject parent, string message, string detailMessage, string okText, System.Action okCallback, string cancelText = null, System.Action cancelCallback = null)
        {
            ErrorPopup commonPopup = ShowPopup(parent, "ErrorPopup").GetComponent<ErrorPopup>();

            commonPopup.SetPopup(
                LocalizationManager.Instance.GetLocalizedValue(message),
                detailMessage,
                LocalizationManager.Instance.GetLocalizedValue(okText),
                okCallback,
                string.IsNullOrEmpty(cancelText) == true ? null : LocalizationManager.Instance.GetLocalizedValue(cancelText),
                cancelCallback
            );

            return commonPopup.gameObject;
        }

        public static GameObject ShowErrorDetailPopup(GameObject parent, string title, string message, string okText, System.Action okCallback)
        {
            CommonPopup commonPopup = ShowPopup(parent, "ErrorDetailPopup").GetComponent<CommonPopup>();

            commonPopup.SetPopup(
                title,
                message,
                okText,
                okCallback,
                null,
                null
            );

            return commonPopup.gameObject;
        }
    }
}