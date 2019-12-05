using System;
using System.Collections.Generic;
using Toast.Gamebase;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class Login : MonoBehaviour
    {
        [SerializeField]
        private GameObject loginButtonContainer;
        [SerializeField]
        private GameObject googleButton;
        [SerializeField]
        private GameObject gamecenterButton;
        [SerializeField]
        private GameObject otherButtonContainer;

        [SerializeField]
        private GameObject cacheDeleteButton;

        [SerializeField]
        private Text facebookButtonText;
        [SerializeField]
        private Text googleButtonText;
        [SerializeField]
        private Text gamecenterButtonText;
        [SerializeField]
        private Text paycoButtonText;
        [SerializeField]
        private Text guestButtonText;

        [SerializeField]
        private GameObject popupRoot;

        [SerializeField]
        private Transform touchEffectPosition;
        [SerializeField]
        private GameObject touchEffect;

        private void Start()
        {
            ActiveButton(false);

            JsonUtil.InitializeLitJson();

            DataManager.InitializeLaunching(result =>
            {
                InitializeGamebase(() =>
                {
                    Gamebase.SetDisplayLanguageCode(DataManager.LanguageCode);

                    StartCoroutine(
                        LocalizationManager.Instance.LoadLocalizedStrings(
                            this,
                            () =>
                            {
                                LocalizationManager.Instance.UpdateText();
                                CheckLaunchingStatus();
                            }));
                });
            });
        }

        private void Update()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
			Application.Quit();
        }
#endif

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        if (0 < Input.touchCount)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    CreateTouchEffect(Input.GetTouch(i).position);
                }
            }
        }
#else
            if (Input.GetMouseButtonDown(0) == true)
            {
                CreateTouchEffect(Input.mousePosition);
            }
#endif
        }

        private void CreateTouchEffect(Vector2 position)
        {
            GameObject touch = Instantiate(touchEffect, Vector3.zero, Quaternion.identity);
            touch.transform.parent = touchEffectPosition;
            touch.transform.localScale = Vector3.one;

            Vector2 localpoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform as RectTransform,
                Input.mousePosition,
                GetComponentInParent<Canvas>().worldCamera,
                out localpoint);
            touch.transform.localPosition = localpoint;
        }

        #region UIButton.Click
        public void ClickFacebookLoginButton()
        {
            LoginWithProviderName(GamebaseAuthProvider.FACEBOOK);
        }

        public void ClickPaycoLoginButton()
        {
            LoginWithProviderName(GamebaseAuthProvider.PAYCO);
        }

        public void ClickGuestLoginButton()
        {
            LoginWithProviderName(GamebaseAuthProvider.GUEST);
        }

        public void ClickGoogleLoginButton()
        {
            LoginWithProviderName(GamebaseAuthProvider.GOOGLE);
        }

        public void ClickGamecenterLoginButton()
        {
            LoginWithProviderName(GamebaseAuthProvider.GAMECENTER);
        }

        public void ClickDeleteCacheButton()
        {
            ResourceDownloader.Instance.DeleteCaches();
        }
        #endregion

        /// <summary>
        /// Step 1 : Gamebase initialization.
        /// </summary>
        private void InitializeGamebase(Action callback)
        {
            Loading.GetInstance().ShowLoading(gameObject);

            Logger.Debug(string.Format("{0}: Gamebase.Initialize", "Begin"), this);

            Gamebase.Initialize((launchingInfo, error) =>
            {
                Logger.Debug(string.Format("{0}: Gamebase.Initialize", Gamebase.IsSuccess(error)), this);

                if (Gamebase.IsSuccess(error) == true)
                {
                    callback();
                }
                else
                {
                    PopupManager.ShowErrorPopup(
                        popupRoot,
                        GameStrings.GAMEBASE_INITIALIZE_ERROR,
                        error.ToString(),
                        GameStrings.RETRY,
                        () => { InitializeGamebase(callback); },
                        GameStrings.EXIT,
                        () => { Application.Quit(); });

                    Loading.GetInstance().HideLoading();
                }
            });
        }

        /// <summary>
        /// Step 2 : Check launchingStatus
        /// If the game is playable, move to the next step.
        /// </summary>
        private void CheckLaunchingStatus()
        {
            var status = Gamebase.Launching.GetLaunchingInformations().launching.status;
            if (IsPlayable(status.code) == true)
            {
                GamebaseLogin();
            }
            else
            {
                Logger.Debug(string.Format("code:{0}, message:{1}", status.code, status.message), this);
            }
        }

        private void GamebaseLogin()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        LoginForLastLoggedInProvider();
#else
            ActiveButton(true);
            Loading.GetInstance().HideLoading();
#endif
        }

        /// <summary>
        /// Step 3 : LoginForLastLoggedInProvider
        /// </summary>
        private void LoginForLastLoggedInProvider()
        {
            if (string.IsNullOrEmpty(Gamebase.GetLastLoggedInProvider()) == true)
            {
                ActiveButton(true);
                Loading.GetInstance().HideLoading();

                return;
            }

            Logger.Debug(string.Format("{0}: Gamebase.LoginForLastLoggedInProvider. loggedIn type:{1}", "Begin", Gamebase.GetLastLoggedInProvider()), this);
            Gamebase.LoginForLastLoggedInProvider((authToken, error) =>
            {
                Loading.GetInstance().HideLoading();

                Logger.Debug(string.Format("{0}: Gamebase.LoginForLastLoggedInProvider", Gamebase.IsSuccess(error)), this);
                if (Gamebase.IsSuccess(error) == true)
                {
                    Push.CheckPushSettings(gameObject, () => { OnLogin(); });
                }
                else
                {
                    ActiveButton(true);
                }
            });
        }

        /// <summary>
        /// Step 4 : Login with providerName
        /// </summary>
        private void LoginWithProviderName(string loginType)
        {
            ActiveButton(false);
            Loading.GetInstance().ShowLoading(gameObject);

            Logger.Debug(string.Format("{0}: Gamebase.Login({1})", "Begin", loginType), this);
            Gamebase.Login(loginType, (authToken, error) =>
            {
                Loading.GetInstance().HideLoading();

                Logger.Debug(string.Format("{0}: Gamebase.Login", Gamebase.IsSuccess(error)), this);
                if (Gamebase.IsSuccess(error) == true)
                {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
                ActiveButton(false);                
                Push.CheckPushSettings(gameObject, ()=> { OnLogin(); });
#else
                OnLogin();
#endif
            }
                else
                {
                    PopupManager.ShowErrorPopup(
                        popupRoot,
                        GameStrings.LOGIN_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        null
                    );
                    ActiveButton(true);
                }
            });
        }

        private void OnLogin()
        {
            SendLevelAfterLogin();

            Gamebase.Util.ShowToast(
                string.Format("{0} {1}", Gamebase.GetLastLoggedInProvider().ToUpper(), LocalizationManager.Instance.GetLocalizedValue(GameStrings.LOGIN_SUCCESS_CONTEXT)),
                GamebaseUIToastType.TOAST_LENGTH_LONG);

            LoadMainScene();
        }

        private void LoadMainScene()
        {
            PopupManager.ShowPopup(popupRoot, "DownloadView");

            ResourceDownloader.Instance.StartDownload(
                (result) =>
                {
                    if (result == true)
                    {
                        DataManager.User.Id = Gamebase.GetUserID();
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
                    DataManager.User.IdP = Gamebase.GetLastLoggedInProvider();
#else
                    DataManager.User.IdP = "guest";
#endif

                    SceneManager.LoadSceneAsync("main");
                    }
                    else
                    {
                        PopupManager.ShowErrorPopup(
                            transform.parent.parent.gameObject,
                            GameStrings.RESOURCE_DOWNLOAD_FAILED_MESSAGE,
                            null,
                            GameStrings.COMMON_OK_BUTTON,
                            () => { Application.Quit(); });
                    }
                },
                "/Sample/default");
        }

        private void SendLevelAfterLogin()
        {
            var userData = new GamebaseRequest.Analytics.GameUserData(PlayerPrefs.GetInt(UserData.KEY_LEVEL, 1));
            Gamebase.Analytics.SetGameUserData(userData);
        }

        private void ActiveButton(bool isActive)
        {
            loginButtonContainer.SetActive(isActive);
            cacheDeleteButton.SetActive(isActive);

#if !UNITY_EDITOR && UNITY_IOS
        gamecenterButton.SetActive(isActive);
        otherButtonContainer.SetActive(isActive);
#elif !UNITY_EDITOR && UNITY_ANDROID
        googleButton.SetActive(isActive);
        otherButtonContainer.SetActive(isActive);
#elif UNITY_EDITOR
            googleButton.SetActive(false);
            gamecenterButton.SetActive(false);
            otherButtonContainer.SetActive(false);
#else
#endif
        }

        private bool IsPlayable(int status)
        {
            if (status >= 200 && status < 300)
            {
                return true;
            }

            return false;
        }

        private bool HasItemList(List<GamebaseResponse.Purchase.PurchasableReceipt> list)
        {
            if (list == null)
            {
                return false;
            }

            if (0 == list.Count)
            {
                return false;
            }

            return true;
        }
    }
}