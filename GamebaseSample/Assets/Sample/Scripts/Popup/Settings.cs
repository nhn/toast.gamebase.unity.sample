using System.Collections.Generic;
using System.Text;
using Toast.Gamebase;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class Settings : MonoBehaviour
    {
        private const string TEXT_AUTO = "AUTO";

        private static readonly Color TAB_DESELECTED_COLOR = new Color(0.592f, 0.592f, 0.592f);
        private static readonly Color PUSH_BUTTON_DIMMED = new Color(0.714f, 0.698f, 0.651f);

        private bool isGoogleMapping = false;
        private bool isGamecenterMapping = false;
        private bool isFacebookMapping = false;
        private bool isPaycoMapping = false;

        [SerializeField]
        private Image pushSettingButton;

        [SerializeField]
        private GameObject koreanCheckImage;
        [SerializeField]
        private GameObject englishCheckImage;
        [SerializeField]
        private GameObject japaneseCheckImage;

        [SerializeField]
        private GameObject mappingView;

        [SerializeField]
        private GameObject googleButton;
        [SerializeField]
        private GameObject gamecenterButton;
        [SerializeField]
        private GameObject facebookButton;
        [SerializeField]
        private GameObject paycoButton;

        [SerializeField]
        private GameObject transferAccountView;

        [SerializeField]
        private Image googleMappingMark;
        [SerializeField]
        private Image gamecenterMappingMark;
        [SerializeField]
        private Image facebookMappingMark;
        [SerializeField]
        private Image paycoMappingMark;

        [SerializeField]
        private Sprite mappingOnSprite;

        [SerializeField]
        private Toggle gamebaseDebugMode;

        [SerializeField]
        private Slider smartDlThreadCountSlider;
        [SerializeField]
        private Text smartDlThreadCountText;

        private string accountId;
        private string accountPassword;

        private void ChangeDisplayLanguage(string language)
        {
            DataManager.LanguageCode = language;
            Gamebase.SetDisplayLanguageCode(language);

            ChangeLanguageButton();
            StartCoroutine(
                LocalizationManager.Instance.LoadLocalizedStrings(
                    this,
                    () =>
                    {
                        LocalizationManager.Instance.UpdateText();
                    }));
        }

        private void ChangeLanguageButton()
        {
            string displayLanguageCode = Gamebase.GetDisplayLanguageCode();

            switch (displayLanguageCode)
            {
                case GameConstants.LANGUAGE_KOREAN:
                    {
                        koreanCheckImage.SetActive(true);
                        englishCheckImage.SetActive(false);
                        japaneseCheckImage.SetActive(false);
                        break;
                    }
                case GameConstants.LANGUAGE_ENGLISH:
                    {
                        koreanCheckImage.SetActive(false);
                        englishCheckImage.SetActive(true);
                        japaneseCheckImage.SetActive(false);
                        break;
                    }
                case GameConstants.LANGUAGE_JAPANESE:
                    {
                        koreanCheckImage.SetActive(false);
                        englishCheckImage.SetActive(false);
                        japaneseCheckImage.SetActive(true);
                        break;
                    }
            }
        }

        private void OnEnable()
        {
            ChangeLanguageButton();

#if !UNITY_EDITOR && UNITY_IOS
            mappingView.SetActive(true);
            googleButton.SetActive(false);
            gamecenterButton.SetActive(true);
            facebookButton.SetActive(true);
            paycoButton.SetActive(true);
#elif !UNITY_EDITOR && UNITY_ANDROID
            mappingView.SetActive(true);
            googleButton.SetActive(true);
            gamecenterButton.SetActive(false);
            facebookButton.SetActive(true);
            paycoButton.SetActive(true);
#else
            pushSettingButton.color = PUSH_BUTTON_DIMMED;

            mappingView.SetActive(false);
            googleButton.SetActive(false);
            gamecenterButton.SetActive(false);
            facebookButton.SetActive(false);
            paycoButton.SetActive(false);
#endif
            gamebaseDebugMode.isOn = DataManager.GamebaseDebugMode;

            GetAuthMappingList();
            ActiveTransferAccountButtons();

            smartDlThreadCountSlider.minValue = 0;
            smartDlThreadCountSlider.maxValue = System.Environment.ProcessorCount;
            smartDlThreadCountSlider.value = DataManager.SmartDlThreadCount;
            RefreshThreadCountText();
        }

        #region UIButton.onClick
        public void ClickCloseButton()
        {
            ClosePopup();
        }

        public void ClickLogoutButton()
        {
            PopupManager.ShowCommonPopup(
                gameObject,
                PopupManager.CommonPopupType.SMALL_SIZE,
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_LOGOUT_TITLE),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_LOGOUT_CONTEXT),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                () => { Logout(); },
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_CANCEL_BUTTON),
                null);
        }

        public void ClickWithdrawButton()
        {
            PopupManager.ShowCommonPopup(
                gameObject,
                PopupManager.CommonPopupType.SMALL_SIZE,
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_WITHDRAW_TITLE),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_WITHDRAW_CONTEXT),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                () => { Withdraw(); },
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_CANCEL_BUTTON),
                null);
        }

        public void ClickIssueTransferAccountButton()
        {
            QueryTransferAccount();
        }

        public void ClickTransferAccountWithIdPLoginButton()
        {
            var popup = PopupManager.ShowPopup(gameObject, "IdPasswordPopup");
            var idPasswordPopup = popup.GetComponent<IdPasswordPopup>();
            idPasswordPopup.SetPopup(
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_IDPLOGIN),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_ENTER_ACCOUNT_INFO),
                "",
                "",
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                (inputId, inputPassword) =>
                {
                    if (string.IsNullOrEmpty(inputId) == true || string.IsNullOrEmpty(inputPassword) == true)
                    {
                        PopupManager.ShowErrorPopup(
                            gameObject,
                            GameStrings.SETTINGS_TRANSFER_ACCOUNT_INVALID_ID_PASSWORD,
                            null,
                            GameStrings.COMMON_OK_BUTTON,
                            null);

                        return;
                    }

                    TransferAccountWithIdPLogin(inputId, inputPassword);
                },
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_CANCEL_BUTTON),
                null);
        }

        public void ClickPushSettingButton()
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

            PopupManager.ShowCommonPopup(
                gameObject,
                PopupManager.CommonPopupType.SMALL_SIZE,
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_PUSH_NOTICE_TITLE),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_PUSH_NOTICE_MESSAGE),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                null);
#else
            Push.OpenPush(gameObject, () => { }, true);
#endif
        }

        public void ClickGoogleMappingButton()
        {
            Mapping(GamebaseAuthProvider.GOOGLE, isGoogleMapping);
        }

        public void ClickGamecenterMappingButton()
        {
            Mapping(GamebaseAuthProvider.GAMECENTER, isGamecenterMapping);
        }

        public void ClickFacebookMappingButton()
        {
            Mapping(GamebaseAuthProvider.FACEBOOK, isFacebookMapping);
        }

        public void ClickPaycoMappingButton()
        {
            Mapping(GamebaseAuthProvider.PAYCO, isPaycoMapping);
        }

        public void ClickKoreanButton()
        {
            koreanCheckImage.SetActive(true);
            englishCheckImage.SetActive(false);
            japaneseCheckImage.SetActive(false);
            ChangeDisplayLanguage(GameConstants.LANGUAGE_KOREAN);
        }

        public void ClickEnglishButton()
        {
            koreanCheckImage.SetActive(false);
            englishCheckImage.SetActive(true);
            japaneseCheckImage.SetActive(false);
            ChangeDisplayLanguage(GameConstants.LANGUAGE_ENGLISH);
        }

        public void ClickJapaneseButton()
        {
            koreanCheckImage.SetActive(false);
            englishCheckImage.SetActive(false);
            japaneseCheckImage.SetActive(true);
            ChangeDisplayLanguage(GameConstants.LANGUAGE_JAPANESE);
        }

        public void ClickLeaderboardSetDummyDataButton()
        {
            string[] idP = { "guest", "google", "facebook", "gamecenter", "payco" };

            for (int i = 0; i < 15; i++)
            {
                LeaderboardApi.SetSingleUserScore(Leaderboard.FACTOR_SCORE, "Dummy" + i, Random.Range(0, 81), idP[Random.Range(0, idP.Length)]);
            }
        }

        public void ClickLeaderboardDeleteDummyDataButton()
        {
            for (int i = 0; i < 15; i++)
            {
                LeaderboardApi.DeleteSingleUserInfo(Leaderboard.FACTOR_SCORE, "Dummy" + i);
            }
        }

        public void ChangeGamebaseDebugMode(bool isDebugMode)
        {
            DataManager.GamebaseDebugMode = isDebugMode;
        }

        public void ChangeTab(Toggle toggle)
        {
            toggle.targetGraphic.color = toggle.isOn ? Color.white : TAB_DESELECTED_COLOR;
        }

        public void ChangeSmartDlThreadCount()
        {
            int threadCount = (int)smartDlThreadCountSlider.value;

            DataManager.SmartDlThreadCount = threadCount;
            RefreshThreadCountText();
        }
        #endregion

        private void RefreshThreadCountText()
        {
            int threadCount = (int)smartDlThreadCountSlider.value;

            smartDlThreadCountText.text = threadCount > 0 ? threadCount.ToString() : TEXT_AUTO;
        }

        private void Logout()
        {
            Loading.GetInstance().ShowLoading(gameObject);

            Gamebase.Logout((error) =>
            {
                Loading.GetInstance().HideLoading();

                if (Gamebase.IsSuccess(error) == true)
                {
                    SceneManager.LoadSceneAsync("login");
                }
                else
                {
                    PopupManager.ShowErrorPopup(
                        gameObject,
                        GameStrings.LOGOUT_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        null);
                }
            });
        }

        private void Withdraw()
        {
            Loading.GetInstance().ShowLoading(gameObject);

            Gamebase.Withdraw((error) =>
            {
                Loading.GetInstance().HideLoading();

                if (Gamebase.IsSuccess(error) == true)
                {
                    PlayerPrefs.DeleteKey(Push.KEY_PUSH_SETTING);
                    SceneManager.LoadSceneAsync("login");
                }
                else
                {
                    PopupManager.ShowErrorPopup(
                        gameObject,
                        GameStrings.WITHDRAW_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        null);
                }
            });
        }

        private void ActiveTransferAccountButtons()
        {
            bool isGuestLoggedIn = Gamebase.GetLastLoggedInProvider().Equals(GamebaseAuthProvider.GUEST);

            transferAccountView.SetActive(isGuestLoggedIn);
        }

        private void QueryTransferAccount()
        {
            Gamebase.QueryTransferAccount((transferAccountInfo, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    var message = new StringBuilder();
                    message.AppendLine(LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_ALREADY_CREATED));
                    message.AppendLine(LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_CREATE));
                    message.AppendLine(string.Format("ID:{0}", transferAccountInfo.account.id));

                    PopupManager.ShowCommonPopup(
                        gameObject,
                        PopupManager.CommonPopupType.SMALL_SIZE,
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_ISSUE),
                        message.ToString(),
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                        () => { IssueTransferAccount(); },
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_CANCEL_BUTTON),
                        null);
                }
                else
                {
                    if (error.code == GamebaseErrorCode.AUTH_TRANSFERACCOUNT_NOT_EXIST)
                    {
                        IssueTransferAccount();
                        return;
                    }

                    PopupManager.ShowErrorPopup(
                        gameObject,
                        GameStrings.QUERY_TRANSFER_ACCOUNT_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        null);
                }
            });
        }

        private void IssueTransferAccount()
        {
            Gamebase.IssueTransferAccount((transferAccountInfo, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    accountId = transferAccountInfo.account.id;
                    accountPassword = transferAccountInfo.account.password;

                    var message = new StringBuilder();
                    message.AppendLine(LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_CREATED));
                    message.Append(LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_CHANGABLE_ID_PASSWORD));

                    var popup = PopupManager.ShowPopup(gameObject, "IdPasswordPopup");
                    var idPasswordPopup = popup.GetComponent<IdPasswordPopup>();
                    idPasswordPopup.SetPopup(
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_ISSUE),
                        message.ToString(),
                        accountId,
                        accountPassword,
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                        (inputId, inputPassword) =>
                        {
                            if (accountId.Equals(inputId) == true && accountPassword.Equals(inputPassword) == true)
                            {
                                CompleteTransferAccount(transferAccountInfo, error);
                            }
                            else
                            {
                                var configuration = CreateTransferAccountRenewConfiguration(inputId, inputPassword);
                                if (configuration == null)
                                {
                                    PopupManager.ShowErrorPopup(
                                        gameObject,
                                        GameStrings.SETTINGS_TRANSFER_ACCOUNT_INVALID_ID_PASSWORD,
                                        null,
                                        GameStrings.COMMON_OK_BUTTON,
                                        null);

                                    return;
                                }

                                renewTransferAccount(configuration);
                            }
                        },
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_CANCEL_BUTTON),
                        null);
                }
                else
                {
                    PopupManager.ShowErrorPopup(
                        gameObject,
                        GameStrings.ISSUE_TRANSFER_ACCOUNT_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        null);
                }
            });
        }

        private GamebaseRequest.Auth.TransferAccountRenewConfiguration CreateTransferAccountRenewConfiguration(string id = "", string password = "")
        {
            GamebaseRequest.Auth.TransferAccountRenewConfiguration configuration = null;

            if (string.IsNullOrEmpty(id) == false)
            {
                if (string.IsNullOrEmpty(password) == false)
                {
                    configuration = GamebaseRequest.Auth.TransferAccountRenewConfiguration.MakeManualRenewConfiguration(id, password);
                }
                else
                {
                    // ID만 갱신할 수 없습니다.
                }
            }
            else
            {
                if (string.IsNullOrEmpty(password) == false)
                {
                    configuration = GamebaseRequest.Auth.TransferAccountRenewConfiguration.MakeManualRenewConfiguration(password);
                }
                else
                {
                    configuration = GamebaseRequest.Auth.TransferAccountRenewConfiguration.MakeAutoRenewConfiguration(
                    GamebaseRequest.Auth.TransferAccountRenewConfiguration.RenewalTargetType.ID_PASSWORD);
                }
            }

            return configuration;
        }

        private void renewTransferAccount(GamebaseRequest.Auth.TransferAccountRenewConfiguration configuration)
        {
            Gamebase.RenewTransferAccount(configuration, (transferAccountInfo, error) =>
            {
                CompleteTransferAccount(transferAccountInfo, error);
            });
        }

        private void TransferAccountWithIdPLogin(string inputId, string inputPassword)
        {
            Gamebase.TransferAccountWithIdPLogin(inputId, inputPassword, (authToken, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    var message = new StringBuilder();
                    message.AppendLine(LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_SUCCEEDED));
                    message.Append(LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_LOGOUT_EXISTING_ACCOUNT));

                    PopupManager.ShowCommonPopup(
                        gameObject,
                        PopupManager.CommonPopupType.SMALL_SIZE,
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_ISSUE),
                        message.ToString(),
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                        null);
                }
                else
                {
                    PopupManager.ShowErrorPopup(
                        gameObject,
                        GameStrings.ISSUE_TRANSFER_ACCOUNT_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        null);
                }
            });
        }

        private void CompleteTransferAccount(GamebaseResponse.Auth.TransferAccountInfo transferAccountInfo, GamebaseError error)
        {
            if (Gamebase.IsSuccess(error) == true)
            {
                var message = new StringBuilder();
                message.AppendLine(LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_CREATED));

                switch (transferAccountInfo.condition.transferAccountType)
                {
                    case "ONETIME":
                        {
                            message.AppendLine(LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_ONCE));
                            break;
                        }
                    case "UNLIMITED":
                        {
                            message.AppendLine(LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_MULTIPLE_TIMES));
                            break;
                        }
                }

                message.AppendLine(string.Format("ID:{0}", transferAccountInfo.account.id));
                message.Append(string.Format("PASSWORD:{0}", transferAccountInfo.account.password));

                PopupManager.ShowCommonPopup(
                    gameObject,
                    PopupManager.CommonPopupType.SMALL_SIZE,
                    LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_TRANSFER_ACCOUNT_ISSUE),
                    message.ToString(),
                    LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                    null);
            }
            else
            {
                PopupManager.ShowErrorPopup(
                    gameObject,
                    GameStrings.ISSUE_TRANSFER_ACCOUNT_ERROR,
                    error.ToString(),
                    GameStrings.COMMON_OK_BUTTON,
                    null);
            }
        }

        private void Mapping(string provider, bool isMapped)
        {
            if (isMapped == true)
            {
                if (Gamebase.GetLastLoggedInProvider().Equals(provider) == true)
                {
                    PopupManager.ShowCommonPopup(
                        gameObject,
                        PopupManager.CommonPopupType.SMALL_SIZE,
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_REMOVE_MAPPING_TITLE),
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_REMOVE_MAPPING_NOTICE),
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                        null);
                    return;
                }

                PopupManager.ShowCommonPopup(
                    gameObject,
                    PopupManager.CommonPopupType.SMALL_SIZE,
                    LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_REMOVE_MAPPING_TITLE),
                    LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_REMOVE_MAPPING_CONTEXT),
                    LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                    () => { RemoveMapping(provider); },
                    LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_CANCEL_BUTTON),
                    null);
            }
            else
            {
                PopupManager.ShowCommonPopup(
                    gameObject,
                    PopupManager.CommonPopupType.SMALL_SIZE,
                    LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_ADD_MAPPING_TITLE),
                    LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_ADD_MAPPING_CONTEXT),
                    LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                    () => { AddMapping(provider); },
                    LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_CANCEL_BUTTON),
                    null);
            }
        }

        private void AddMapping(string providerName)
        {
            Loading.GetInstance().ShowLoading(gameObject);

            Gamebase.AddMapping(providerName, (authToken, error) =>
            {
                Loading.GetInstance().HideLoading();

                if (Gamebase.IsSuccess(error) == true)
                {
                    GetAuthMappingList();
                }
                else
                {
                    if (error.code == GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER)
                    {
                        ForcingMapping(providerName, error);
                        return;
                    }

                    PopupManager.ShowErrorPopup(
                        gameObject,
                        GameStrings.ADD_MAPPING_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        null);
                }
            });
        }

        private void ForcingMapping(string providerName, GamebaseError error)
        {
            var forcingMappingTicket = GamebaseResponse.Auth.ForcingMappingTicket.MakeForcingMappingTicket(error);

            var message = new StringBuilder();
            message.AppendLine(LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_FORCING_MAPPING_ALREADY_LINKED));
            message.AppendLine(string.Format("User ID:{0}", forcingMappingTicket.mappedUserId));
            message.Append(LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_FORCING_MAPPING_CHANGE_EXISTING_ACCOUNT));

            PopupManager.ShowCommonPopup(
                gameObject,
                PopupManager.CommonPopupType.SMALL_SIZE,
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.SETTINGS_ADD_MAPPING_TITLE),
                message.ToString(),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                () =>
                {
                    Gamebase.AddMappingForcibly(providerName, forcingMappingTicket.forcingMappingKey, (authToken, mappingError) =>
                    {
                        if (Gamebase.IsSuccess(mappingError) == true)
                        {
                            GetAuthMappingList();
                        }
                        else
                        {
                            PopupManager.ShowErrorPopup(
                                gameObject,
                                GameStrings.FORCHING_MAPPING_ERROR,
                                error.ToString(),
                                GameStrings.COMMON_OK_BUTTON,
                                null);
                        }
                    });
                },
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_CANCEL_BUTTON),
                null);
        }

        private void RemoveMapping(string providerName)
        {
            Loading.GetInstance().ShowLoading(gameObject);

            Gamebase.RemoveMapping(providerName, (error) =>
            {
                Loading.GetInstance().HideLoading();

                if (Gamebase.IsSuccess(error) == true)
                {
                    GetAuthMappingList();
                }
                else
                {
                    PopupManager.ShowErrorPopup(
                        gameObject,
                        GameStrings.REMOVE_MAPPING_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        null);
                }
            });
        }

        private void GetAuthMappingList()
        {
            List<string> mappingList = Gamebase.GetAuthMappingList();

            if (mappingList == null)
            {
                return;
            }

            isGoogleMapping = false;
            isGamecenterMapping = false;
            isFacebookMapping = false;
            isPaycoMapping = false;

            googleMappingMark.overrideSprite = null;
            gamecenterMappingMark.overrideSprite = null;
            facebookMappingMark.overrideSprite = null;
            paycoMappingMark.overrideSprite = null;

            foreach (string providerName in mappingList)
            {
                switch (providerName)
                {
                    case GamebaseAuthProvider.GOOGLE:
                        {
                            isGoogleMapping = true;
                            googleMappingMark.overrideSprite = mappingOnSprite;
                            break;
                        }
                    case GamebaseAuthProvider.GAMECENTER:
                        {
                            isGamecenterMapping = true;
                            gamecenterMappingMark.overrideSprite = mappingOnSprite;
                            break;
                        }
                    case GamebaseAuthProvider.FACEBOOK:
                        {
                            isFacebookMapping = true;
                            facebookMappingMark.overrideSprite = mappingOnSprite;
                            break;
                        }
                    case GamebaseAuthProvider.PAYCO:
                        {
                            isPaycoMapping = true;
                            paycoMappingMark.overrideSprite = mappingOnSprite;
                            break;
                        }
                }
            }
        }

        private void ClosePopup()
        {
            Destroy(gameObject);
        }
    }
}