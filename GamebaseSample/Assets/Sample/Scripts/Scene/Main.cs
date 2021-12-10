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
            Gamebase.AddEventHandler(GamebaseEventHandeler);
        }

        private void OnDestroy()
        {
            Gamebase.RemoveEventHandler(GamebaseEventHandeler);
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

        private void GamebaseEventHandeler(GamebaseResponse.Event.GamebaseEventMessage message)
        {
            switch (message.category)
            {
                case GamebaseEventCategory.SERVER_PUSH_APP_KICKOUT:
                case GamebaseEventCategory.SERVER_PUSH_TRANSFER_KICKOUT:
                    {
                        GamebaseResponse.Event.GamebaseEventServerPushData serverPushData = GamebaseResponse.Event.GamebaseEventServerPushData.From(message.data);
                        if (serverPushData != null)
                        {
                            CheckServerPush(message.category, serverPushData);
                        }
                        break;
                    }
                case GamebaseEventCategory.OBSERVER_LAUNCHING:
                    {
                        GamebaseResponse.Event.GamebaseEventObserverData observerData = GamebaseResponse.Event.GamebaseEventObserverData.From(message.data);
                        if(observerData != null)
                        {
                            CheckLaunchingStatus(observerData);
                        }
                        break;
                    }
                case GamebaseEventCategory.OBSERVER_NETWORK:
                    {
                        GamebaseResponse.Event.GamebaseEventObserverData observerData = GamebaseResponse.Event.GamebaseEventObserverData.From(message.data);
                        if (observerData != null)
                        {
                            CheckNetwork(observerData);
                        }
                        break;
                    }
                case GamebaseEventCategory.OBSERVER_HEARTBEAT:
                    {
                        GamebaseResponse.Event.GamebaseEventObserverData observerData = GamebaseResponse.Event.GamebaseEventObserverData.From(message.data);
                        if (observerData != null)
                        {
                            CheckHeartbeat(observerData);
                        }
                        break;
                    }
                case GamebaseEventCategory.OBSERVER_WEBVIEW:
                    {
                        GamebaseResponse.Event.GamebaseEventObserverData observerData = GamebaseResponse.Event.GamebaseEventObserverData.From(message.data);
                        if (observerData != null)
                        {
                            CheckWebView(observerData);
                        }
                        break;
                    }
                case GamebaseEventCategory.OBSERVER_INTROSPECT:
                    {
                        // Introspect error
                        break;
                    }
                case GamebaseEventCategory.PURCHASE_UPDATED:
                    {
                        // If the user got item by 'Promotion Code',
                        // this event will be occurred.
                        break;
                    }
                case GamebaseEventCategory.PUSH_RECEIVED_MESSAGE:
                    {
                        GamebaseResponse.Event.PushMessage pushMessage = GamebaseResponse.Event.PushMessage.From(message.data);
                        if (pushMessage != null)
                        {
                            Gamebase.Util.ShowToast(
                                string.Format("Received push message.\ntitle:{0}\nmessage:{1}", pushMessage.title, pushMessage.body),
                                GamebaseUIToastType.TOAST_LENGTH_LONG);
                        }
                        break;
                    }
                case GamebaseEventCategory.PUSH_CLICK_MESSAGE:
                    {
                        GamebaseResponse.Event.PushMessage pushMessage = GamebaseResponse.Event.PushMessage.From(message.data);
                        if (pushMessage != null)
                        {
                            Gamebase.Util.ShowToast(
                                string.Format("Clicked push message.\ntitle:{0}\nmessage:{1}", pushMessage.title, pushMessage.body),
                                GamebaseUIToastType.TOAST_LENGTH_LONG);
                        }
                        break;
                    }
                case GamebaseEventCategory.PUSH_CLICK_ACTION:
                    {
                        GamebaseResponse.Event.PushAction pushAction = GamebaseResponse.Event.PushAction.From(message.data);
                        if (pushAction != null)
                        {
                            // When you clicked action button by 'Rich Message'.
                            Gamebase.Util.ShowToast(
                                string.Format("Clicked action button by 'Rich Message'.\nactionType:{0}\nuserText:{1}\nmessage:{2}",
                                    pushAction.actionType, pushAction.userText, pushAction.message),
                                GamebaseUIToastType.TOAST_LENGTH_LONG);
                        }
                        break;
                    }
            }
        }

        void CheckServerPush(string category, GamebaseResponse.Event.GamebaseEventServerPushData data)
        {
            if (category.Equals(GamebaseEventCategory.SERVER_PUSH_APP_KICKOUT) == true)
            {
                Gamebase.Util.ShowToast(
                    string.Format(TEXT_ENTER_MESSAGE, "APP_KICKOUT"),
                    GamebaseUIToastType.TOAST_LENGTH_LONG);

                EndSession();
            }
            else if (category.Equals(GamebaseEventCategory.SERVER_PUSH_TRANSFER_KICKOUT) == true)
            {
                Gamebase.Util.ShowToast(
                    string.Format(TEXT_ENTER_MESSAGE, "TRANSFER_KICKOUT"),
                    GamebaseUIToastType.TOAST_LENGTH_LONG);

                EndSession();
            }
        }
        
        private void CheckLaunchingStatus(GamebaseResponse.Event.GamebaseEventObserverData observerData)
        {
            Gamebase.Util.ShowToast(
                string.Format("{0}, launching status:{1}", observerData.message, observerData.code),
                GamebaseUIToastType.TOAST_LENGTH_LONG);

            if (IsPlayable(observerData.code) == false)
            {
                EndSession();
            }
        }
        
        private void CheckNetwork(GamebaseResponse.Event.GamebaseEventObserverData observerData)
        {
            Gamebase.Util.ShowToast(
                string.Format("The network type has been changed to {0}, network status is {1}", observerData.message, observerData.code),
                GamebaseUIToastType.TOAST_LENGTH_LONG);
        }
        
        private void CheckHeartbeat(GamebaseResponse.Event.GamebaseEventObserverData observerData)
        {
            switch (observerData.code)
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
                    Logger.Debug(string.Format("The heartbeat status has been changed to {0}", observerData.code), this);
                    break;
                }
            }
        }

        private void CheckWebView(GamebaseResponse.Event.GamebaseEventObserverData observerData)
        {
            switch (observerData.code)
            {
                case GamebaseWebViewEventType.OPENED:
                    {
                        // WebView opened.
                        break;
                    }
                case GamebaseWebViewEventType.CLOSED:
                    {
                        // WebView closed.
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