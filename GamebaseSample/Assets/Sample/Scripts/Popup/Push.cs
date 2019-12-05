using System;
using Toast.Gamebase;
using UnityEngine;

namespace GamebaseSample
{
    public class Push : MonoBehaviour
    {
        [SerializeField]
        private GameObject pushEnabled;
        [SerializeField]
        private GameObject adAgreement;
        [SerializeField]
        private GameObject adAgreementNight;
        [SerializeField]
        private GameObject cancelButton;

        public const string KEY_PUSH_SETTING = "KEY_PUSH_SETTING";

        private bool isPushEnabled = true;
        private bool isADAgreement = true;
        private bool isADAgreementNight = true;

        private Action callback = null;

        public static void CheckPushSettings(GameObject parent, Action callback)
        {
            var hasPushSetting = PlayerPrefs.GetString(KEY_PUSH_SETTING, "false");

            if (hasPushSetting.Equals("true", StringComparison.Ordinal) == true)
            {
                callback();
            }
            else
            {
                OpenPush(
                    parent,
                    callback);
            }
        }

        public static void OpenPush(GameObject parent, Action callback, bool isShowCancelButton = false)
        {
            Push push = PopupManager.ShowPopup(parent, "PushPopup").GetComponent<Push>();
            push.SetPush(callback, isShowCancelButton);
        }

        private void SetPush(Action callback, bool isShowCancelButton = false)
        {
            this.callback = callback;

            cancelButton.SetActive(isShowCancelButton);

            gameObject.SetActive(true);

            var hasPushSetting = PlayerPrefs.GetString(KEY_PUSH_SETTING, "false");
            if (hasPushSetting.Equals("true", StringComparison.Ordinal) == true)
            {
                QueryPush();
            }
            else
            {
                pushEnabled.SetActive(true);
                adAgreement.SetActive(true);
                adAgreementNight.SetActive(true);
            }
        }

        #region UIButton.onClick
        public void ClickOKButton()
        {
            RegisterPush();
        }

        public void ClickCancelButton()
        {
            ClosePopup();
        }

        public void ClickPushEnabledButton()
        {
            isPushEnabled = !isPushEnabled;
            pushEnabled.SetActive(isPushEnabled);
        }

        public void ClickADAgreementButton()
        {
            isADAgreement = !isADAgreement;
            adAgreement.SetActive(isADAgreement);
        }

        public void ClickADAgreementNightButton()
        {
            isADAgreementNight = !isADAgreementNight;
            adAgreementNight.SetActive(isADAgreementNight);
        }
        #endregion

        public void ClosePopup()
        {
            if (null != callback)
            {
                callback();
                callback = null;
            }

            Destroy(gameObject);
        }

        private void QueryPush()
        {
            Loading.GetInstance().ShowLoading(gameObject);

            Logger.Debug(string.Format("{0}: Gamebase.QueryPush", "Begin"), this);
            Gamebase.Push.QueryPush((pushAdvertisements, error) =>
            {
                Loading.GetInstance().HideLoading();

                Logger.Debug(string.Format("{0}: Gamebase.QueryPush", Gamebase.IsSuccess(error)), this);
                if (Gamebase.IsSuccess(error) == true)
                {
                    isPushEnabled = pushAdvertisements.pushEnabled;
                    isADAgreement = pushAdvertisements.adAgreement;
                    isADAgreementNight = pushAdvertisements.adAgreementNight;

                    pushEnabled.SetActive(isPushEnabled);
                    adAgreement.SetActive(isADAgreement);
                    adAgreementNight.SetActive(isADAgreementNight);
                }
                else
                {
                    PopupManager.ShowErrorPopup(
                        gameObject,
                        GameStrings.QUERY_PUSH_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        null);
                }
            });
        }

        private void RegisterPush()
        {
            Loading.GetInstance().ShowLoading(gameObject);

            GamebaseRequest.Push.PushConfiguration pushConfiguration = new GamebaseRequest.Push.PushConfiguration();
            pushConfiguration.pushEnabled = isPushEnabled;
            pushConfiguration.adAgreement = isADAgreement;
            pushConfiguration.adAgreementNight = isADAgreementNight;
            pushConfiguration.displayLanguageCode = Gamebase.GetDisplayLanguageCode();

            Gamebase.Push.RegisterPush(pushConfiguration, (error) =>
            {
                Loading.GetInstance().HideLoading();
                if (Gamebase.IsSuccess(error) == true)
                {
                // It is recommended to save the push status to local.

                PlayerPrefs.SetString(KEY_PUSH_SETTING, "true");
                    ClosePopup();
                }
                else
                {
                    Logger.Debug("Check user settings for push notification in the TOAST Console", this);

                    PopupManager.ShowErrorPopup(
                        gameObject,
                        GameStrings.REGISTER_PUSH_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        () =>
                        {
                            ClosePopup();
                        });
                }
            });
        }
    }
}