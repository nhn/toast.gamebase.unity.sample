#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseImageNotice : CommonGamebaseImageNotice
    {
        private class ClickType
        {
            public const string NONE = "none";
            public const string OPEN_URL = "openUrl";
            public const string CUSTOM = "custom";
        }

        private const string ERROR_SCHEME = "cef://error";
        private const string DISMISS_SCHEME = "gamebase://dismiss";
        private const string IMAGE_NOTICE_SCHEME = "gamebase://imagenotice";

        private const string ACTION = "action";
        private const string ACTION_CLICK = "click";
        private const string ACTION_NEVER_SHOW_TODAY = "nevershowtoday";

        private const string NEVER_SHOW_TODAY_STATE_KEY = "NEVER_SHOW_TODAY_STATE_KEY";

        private const float STANDARD_SCREEN_WIDTH = 1920f;
        private const float STANDARD_SCREEN_HEIGHT = 1080f;
        private const float STANDARD_WEBVIEW_WIDTH = 640f;
        private const float STANDARD_WEBVIEW_HEIGHT = 480f;

        private GamebaseCallback.ErrorDelegate closeCallback;
        private GamebaseCallback.GamebaseDelegate<string> eventCallback;
        private Color bgColor;

        private ImageNoticeResponse.ImageNotices.ImageNoticeWeb imageNotices = null;

        private Dictionary<string, string> neverShowTodayState = new Dictionary<string, string>();
        private ImageNoticeResponse.ImageNotices.ImageNoticeWeb.ImageNoticeInfo currentImageNotice = null;
        private int currentIndex = 0;

        private WebSocketRequest.RequestVO requestVO;

        public StandaloneGamebaseImageNotice()
        {
            Domain = typeof(StandaloneGamebaseImageNotice).Name;

            requestVO = ImageNoticeMessage.GetImageNoticesMessage();

            string jsonString = PlayerPrefs.GetString(NEVER_SHOW_TODAY_STATE_KEY);
            if (string.IsNullOrEmpty(jsonString) == false)
            {
                neverShowTodayState = JsonMapper.ToObject<Dictionary<string, string>>(jsonString);
            }
        }
        
        public override void ShowImageNotices(GamebaseRequest.ImageNotice.Configuration configuration, int closeHandle, int eventHandle)
        {
            if (configuration == null)
            {
                bgColor = new Color(0f, 0f, 0f, 0.5f);                
            }
            else
            {
                bgColor = new Color(
                    configuration.colorR / 255f,
                    configuration.colorG / 255f,
                    configuration.colorB / 255f,
                    configuration.colorA / 255f);
            }

            closeCallback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(closeHandle);
            GamebaseCallbackHandler.UnregisterCallback(closeHandle);
            eventCallback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<string>>(eventHandle);
            GamebaseCallbackHandler.UnregisterCallback(eventHandle);
            currentIndex = 0;

            RequestImageNotices(() =>
            {
                ShowNextImageNotice();
            });
        }

        public override void CloseImageNotices()
        {
            closeCallback = null;
            eventCallback = null;
            currentIndex = 0;
            WebviewAdapterManager.Instance.CloseWebView();
        }

        private void ShowWebview(string url)
        {
            bool hasAdapter = WebviewAdapterManager.Instance.CreateWebviewAdapter("standalonewebviewadapter");
            if (hasAdapter == false)
            {
                if (closeCallback != null)
                {
                    closeCallback(new GamebaseError(GamebaseErrorCode.NOT_SUPPORTED, message: GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND));
                }
                return;
            }

            WebviewAdapterManager.Instance.ShowWebView(
               url.ToString(),
               null,
               (error) =>
               {
                   ShowNextImageNotice();
               },
               new List<string>()
               {
                    DISMISS_SCHEME,
                    IMAGE_NOTICE_SCHEME
                },
               (scheme, error) =>
               {
                   GamebaseLog.Debug(
                       string.Format(
                           "scheme : {0}", scheme),
                       this);

                   WebviewAdapterManager.SchemeInfo schemeInfo = WebviewAdapterManager.Instance.ConvertURLToSchemeInfo(scheme);
                   if (schemeInfo.scheme == ERROR_SCHEME)
                   {
                       WebviewAdapterManager.Instance.CloseWebView();
                       return;
                   }

                   switch (schemeInfo.scheme)
                   {
                       case DISMISS_SCHEME:
                           {
                               WebviewAdapterManager.Instance.CloseWebView();
                               break;
                           }
                       case IMAGE_NOTICE_SCHEME:
                           {
                               switch (schemeInfo.parameterDictionary[ACTION])
                               {
                                   case ACTION_CLICK:
                                       {
                                           OnActionClick();
                                           WebviewAdapterManager.Instance.CloseWebView();
                                           break;
                                       }
                                   case ACTION_NEVER_SHOW_TODAY:
                                       {
                                           OnActionNeverShowToday();
                                           WebviewAdapterManager.Instance.CloseWebView();
                                           break;
                                       }
                               }
                               break;
                           }
                   }
               });

            WebviewAdapterManager.Instance.SetBgColor(bgColor);
            WebviewAdapterManager.Instance.SetWebViewRect(41, GetWebViewSize(41));
            WebviewAdapterManager.Instance.SetTitleBarColor(bgColor);
            WebviewAdapterManager.Instance.SetTitleBarButton(false, null, null);
            WebviewAdapterManager.Instance.SetTitleVisible(false);
        }

        private void RequestImageNotices(Action callback)
        {
            requestVO.apiId = Lighthouse.API.Launching.ID.GET_IMAGE_NOTICES;
            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                if (error == null)
                {
                    var vo = JsonMapper.ToObject<ImageNoticeResponse.ImageNotices>(response);

                    if (vo.header.isSuccessful == true)
                    {
                        imageNotices = vo.imageNoticeWeb;

                        RemoveUnusedId();
                    }
                    else
                    {
                        imageNotices = null;
                        GamebaseLog.Debug(
                            string.Format("Error : {0}", JsonMapper.ToJson(vo.header)),
                            this);
                    }                    
                }
                else
                {
                    imageNotices = null;
                    GamebaseLog.Debug(
                            string.Format("Error : {0}", error.ToString()),
                            this);
                }
                callback();
            });
        }

        private Rect GetWebViewSize(int titleBarHeight)
        {
            float scaleWidth;
            float scaleHeight;

            if (Screen.width > Screen.height)
            {
                scaleWidth = Screen.width / STANDARD_SCREEN_WIDTH;
                scaleHeight = Screen.height / STANDARD_SCREEN_HEIGHT;
            }
            else
            {
                scaleWidth = Screen.width / STANDARD_SCREEN_HEIGHT;
                scaleHeight = Screen.height / STANDARD_SCREEN_WIDTH;
            }

            float scale = scaleWidth;

            if (scaleWidth > scaleHeight)
            {
                scale = scaleHeight;
            }

            Rect rect = new Rect();

            if (currentImageNotice.imageInfo.width > currentImageNotice.imageInfo.height)
            {
                rect.x = (Screen.width - (STANDARD_WEBVIEW_WIDTH * scale)) * 0.5f;
                rect.y = (Screen.height - (STANDARD_WEBVIEW_HEIGHT * scale)) * 0.5f - titleBarHeight;
                rect.width = STANDARD_WEBVIEW_WIDTH * scale;
                rect.height = (STANDARD_WEBVIEW_HEIGHT + imageNotices.footerHeight + titleBarHeight) * scale;
            }
            else
            {
                rect.x = (Screen.width - (STANDARD_WEBVIEW_HEIGHT * scale)) * 0.5f;
                rect.y = (Screen.height - (STANDARD_WEBVIEW_WIDTH * scale)) * 0.5f - titleBarHeight;
                rect.width = STANDARD_WEBVIEW_HEIGHT * scale;
                rect.height = (STANDARD_WEBVIEW_WIDTH + imageNotices.footerHeight + titleBarHeight) * scale;
            }

            return rect;
        }

        private void RemoveUnusedId()
        {
            List<string> removeIds = new List<string>();

            foreach (string id in neverShowTodayState.Keys)
            {
                bool isExist = false;
                for (int i = 0; i < imageNotices.pageList.Count; i++)
                {
                    var imageNotice = imageNotices.pageList[i];

                    if (id.Equals(imageNotice.imageNoticeId.ToString()) == true)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (isExist == false)
                {
                    removeIds.Add(id);
                }
            }

            foreach (string id in removeIds)
            {
                neverShowTodayState.Remove(id);
            }

            if (removeIds.Count > 0)
            {
                PlayerPrefs.SetString(NEVER_SHOW_TODAY_STATE_KEY, JsonMapper.ToJson(neverShowTodayState));
            }
        }

        private bool ShowNextImageNotice()
        {
            if (imageNotices == null || imageNotices.pageList == null || imageNotices.pageList.Count <= currentIndex)
            {
                closeCallback(null);
                return false;
            }

            currentImageNotice = imageNotices.pageList[currentIndex];
            currentIndex++;

            if (CheckNeverShowToday(currentImageNotice.imageNoticeId.ToString()) == false)
            {
                return ShowNextImageNotice();
            }

            string url = string.Format(
                "{0}{1}",
                imageNotices.domain,
                currentImageNotice.path);

            ShowWebview(url);
            return true;
        }

        private void OnActionClick()
        {
            switch (currentImageNotice.clickType)
            {
                case ClickType.NONE:
                    {
                        break;
                    }
                case ClickType.OPEN_URL:
                    {
                        Application.OpenURL(UnityCompatibility.WebRequest.UnEscapeURL(currentImageNotice.clickScheme));
                        break;
                    }
                case ClickType.CUSTOM:
                    {
                        if (eventCallback != null)
                        {
                            eventCallback(UnityCompatibility.WebRequest.UnEscapeURL(currentImageNotice.clickScheme), null);
                        }
                        break;
                    }
            }
        }

        private void OnActionNeverShowToday()
        {
            if (neverShowTodayState.ContainsKey(currentImageNotice.imageNoticeId.ToString()) == true)
            {
                neverShowTodayState.Remove(currentImageNotice.imageNoticeId.ToString());
            }

            neverShowTodayState.Add(currentImageNotice.imageNoticeId.ToString(), DateTime.UtcNow.Ticks.ToString());

            PlayerPrefs.SetString(NEVER_SHOW_TODAY_STATE_KEY, JsonMapper.ToJson(neverShowTodayState));
        }

        private bool CheckNeverShowToday(string id)
        {
            string ticks;
            if (neverShowTodayState.TryGetValue(id, out ticks) == false)
            {
                return true;
            }

            long elapsedTicks = DateTime.UtcNow.Ticks - long.Parse(ticks);
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            if (elapsedSpan.TotalDays >= 1)
            {
                neverShowTodayState.Remove(id);
                PlayerPrefs.SetString(NEVER_SHOW_TODAY_STATE_KEY, JsonMapper.ToJson(neverShowTodayState));
                return true;
            }

            return false;
        }
    }
}
#endif
