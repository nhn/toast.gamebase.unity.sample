using System;
using System.Collections.Generic;
using Toast.Gamebase;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamebaseSample
{
    public class Main : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainUI;
        [SerializeField]
        private GameObject popupRoot;

        [SerializeField]
        private Transform touchEffectPosition;
        [SerializeField]
        private GameObject touchEffect;

        private const string KEY_CODE = "code";
        private const string KEY_MESSAGE = "message";
        private const string TEXT_ENTER_MESSAGE = "Please enter a message. -{0}";
        private const string WEBVIEW_URL = "https://www.toast.com/kr/service/game/gamebase";

        private void Start()
        {
            Gamebase.AddObserver(GamebaseObserverHandler);
            Gamebase.AddServerPushEvent(GamebaseServerPushHandler);
        }

        private void OnDestroy()
        {
            Gamebase.RemoveObserver(GamebaseObserverHandler);
            Gamebase.RemoveServerPushEvent(GamebaseServerPushHandler);
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

        private void GamebaseObserverHandler(GamebaseResponse.SDK.ObserverMessage message)
        {
            switch (message.type)
            {
                case GamebaseObserverType.LAUNCHING:
                    {
                        CheckLaunchingStatus(message.data);
                        break;
                    }
                case GamebaseObserverType.HEARTBEAT:
                    {
                        CheckHeartbeat(message.data);
                        break;
                    }
                case GamebaseObserverType.NETWORK:
                    {
                        CheckNetworkStatus(message.data);
                        break;
                    }
            }
        }

        private void CheckLaunchingStatus(Dictionary<string, object> data)
        {
            int launchingStatus = GetDictionaryValue<int>(data, KEY_CODE);
            string launghingMessage = GetDictionaryValue<string>(data, KEY_MESSAGE);

            Gamebase.Util.ShowToast(
                string.Format("{0}, launching status:{1}", launghingMessage, launchingStatus),
                GamebaseUIToastType.TOAST_LENGTH_LONG);

            if (IsPlayable(launchingStatus) == false)
            {
                EndSession();
            }
        }

        private void CheckHeartbeat(Dictionary<string, object> data)
        {
            int heartbeatCode = GetDictionaryValue<int>(data, KEY_CODE);

            switch (heartbeatCode)
            {
                case GamebaseErrorCode.INVALID_MEMBER:
                    {
                        Gamebase.Util.ShowToast(
                            string.Format(TEXT_ENTER_MESSAGE, "INVALID_MEMBER"),
                            GamebaseUIToastType.TOAST_LENGTH_LONG);
                        EndSession();
                        break;
                    }
                case GamebaseErrorCode.BANNED_MEMBER:
                    {
                        Gamebase.Util.ShowToast(
                            string.Format(TEXT_ENTER_MESSAGE, "BANNED_MEMBER"),
                            GamebaseUIToastType.TOAST_LENGTH_LONG);
                        EndSession();
                        break;
                    }
                default:
                    {
                        Logger.Debug(string.Format("The heartbeat status has been changed to {0}", heartbeatCode), this);
                        break;
                    }
            }
        }

        private void CheckNetworkStatus(Dictionary<string, object> data)
        {
            int networkStatus = GetDictionaryValue<int>(data, KEY_CODE);
            string networkType = GetDictionaryValue<string>(data, KEY_MESSAGE);

            Gamebase.Util.ShowToast(
                string.Format("The network type has been changed to {0}, network status is {1}", networkType, networkStatus),
                GamebaseUIToastType.TOAST_LENGTH_LONG);
        }

        private void GamebaseServerPushHandler(GamebaseResponse.SDK.ServerPushMessage message)
        {
            switch (message.type)
            {
                case GamebaseServerPushType.APP_KICKOUT:
                    {
                        Gamebase.Util.ShowToast(
                            string.Format(TEXT_ENTER_MESSAGE, "APP_KICKOUT"),
                            GamebaseUIToastType.TOAST_LENGTH_LONG);

                        EndSession();
                        break;
                    }
                case GamebaseServerPushType.TRANSFER_KICKOUT:
                    {
                        Gamebase.Util.ShowToast(
                            string.Format(TEXT_ENTER_MESSAGE, "TRANSFER_KICKOUT"),
                            GamebaseUIToastType.TOAST_LENGTH_LONG);

                        EndSession();
                        break;
                    }
            }
        }

        private void EndSession()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            SceneManager.LoadSceneAsync("login");
#else
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

        private T GetDictionaryValue<T>(Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.ContainsKey(key) == true)
            {
                return (T)Convert.ChangeType(dictionary[key], typeof(T));
            }
            else
            {
                return default(T);
            }
        }

        #region UIButton.onClick
        public void ClickGameStartButton()
        {
            PopupManager.ShowPopup(popupRoot, "DownloadView");

            ResourceDownloader.Instance.StartDownload(
                (result) =>
                {
                    if (result == true)
                    {
                        SceneManager.LoadSceneAsync("ingame");
                    }
                    else
                    {
                        PopupManager.ShowErrorPopup(
                            gameObject,
                            GameStrings.RESOURCE_DOWNLOAD_FAILED_MESSAGE,
                            null,
                            GameStrings.COMMON_OK_BUTTON,
                            null);
                    }
                },
                "/Sample/stage");
        }

        public void ClickStoreButton()
        {
            PopupManager.ShowPopup(popupRoot, "StorePopup");
        }

        public void ClickSettingsButton()
        {
            PopupManager.ShowPopup(popupRoot, "SettingsPopup");
        }

        public void ClickNoticeButton()
        {
            Gamebase.Webview.ShowWebView(
                WEBVIEW_URL,
                null,
                (error) =>
                {
                    Logger.Debug("Close WebView.", this);
                });
        }

        public void ClickGameInfoButton()
        {
            PopupManager.ShowPopup(popupRoot, "GameInfoPopup");
        }
        public void ClickLeaderboardButton()
        {
            PopupManager.ShowPopup(popupRoot, "LeaderboardPopup");
        }
        #endregion
    }
}