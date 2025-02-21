#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;
using UnityEngine;

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

        private const string TYPE_ROLLING = "ROLLING";

        private const string ERROR_SCHEME = "cef://error";
        private const string DISMISS_SCHEME = "gamebase://dismiss";
        private const string IMAGE_NOTICE_SCHEME = "gamebase://imagenotice";

        private const string ACTION = "action";
        private const string ACTION_CLICK = "click";
        private const string ACTION_ID = "id";
        private const string ACTION_NEVER_SHOW_TODAY = "nevershowtoday";

        private const string FIXED_ROLLING_OPTION = "&orientation=landscape";

        private const string NEVER_SHOW_TODAY_ROLLING_STATE_KEY = "NEVER_SHOW_TODAY_ROLLING_STATE_KEY";

        private const string MESSAGE_CANNOT_BN_OPENED = "The image notice cannot be opened due to the 'Don't ask again today' setting.";
        private const string MESSAGE_TURNED_OFF = "The 'Don't ask again today' setting is turned off.";
        private const string MESSAGE_REMOVED_NOTICE = "The exposure has been discontinued and the ID has been removed.";
        private const string MESSAGE_NO_IMAGE_NOTICE = "No image notice to display.";
        private const string MESSAGE_NO_DATA_MATCHING_THE_ID = "No data matching the ID passed in the scheme.";
        private const string MESSAGE_NEXT_POPUP_TIME_MILLIS_IS_NULL = "The nextPopupTimeMillis is null.";
        private const string MESSAGE_INVALID_ID = "Invalid ID.";

        private const float SCREEN_WIDTH = 1920f;
        private const float SCREEN_HEIGHT = 1080f;

        private const float STANDARD_IMAGE_WIDTH = 600f;
        private const float STANDARD_IMAGE_HEIGHT = 450f;

        private const int TITLE_BAR_HEIGHT = 41;

        private GamebaseCallback.ErrorDelegate closeCallback;
        private GamebaseCallback.GamebaseDelegate<string> eventCallback;
        private Color bgColor;

        private ImageNoticeResponse.ImageNotices.ImageNoticeWeb imageNotices;

        private Dictionary<string, string> neverShowTodayState;

        private WebSocketRequest.RequestVO requestVO;

        public StandaloneGamebaseImageNotice()
        {
            Domain = typeof(StandaloneGamebaseImageNotice).Name;
            requestVO = ImageNoticeMessage.GetImageNoticesMessage();
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

            RequestImageNoticeData();
        }

        public override void CloseImageNotices()
        {
            closeCallback = null;
            eventCallback = null;
            WebviewAdapterManager.Instance.CloseWebView();
        }

        private void RequestImageNoticeData()
        {
            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                imageNotices = null;

                if (Gamebase.IsSuccess(error) == false)
                {
                    closeCallback?.Invoke(error);
                    return;
                }

                if (string.IsNullOrEmpty(response))
                {
                    closeCallback?.Invoke(new GamebaseError(GamebaseErrorCode.SERVER_UNKNOWN_ERROR, Domain));
                    return;
                }

                IsValidServerResponse(response, (responseError) =>
                {
                    if (Gamebase.IsSuccess(responseError))
                    {
                        if (HasImageNotice() == false)
                        {
                            GamebaseLog.Debug(MESSAGE_NO_IMAGE_NOTICE, this);
                            closeCallback(null);
                            return;
                        }

                        InitNeverShowTodayState(NEVER_SHOW_TODAY_ROLLING_STATE_KEY);
                        ShowRolling();
                    }
                    else
                    {
                        closeCallback?.Invoke(responseError);
                    }
                });
            });
        }

        private void ShowRolling()
        {
            if (CheckNeverShowToday(imageNotices.rollingImageNoticeId) == true)
            {
                GamebaseLog.Debug(
                    string.Format("{0} id:{1}", MESSAGE_CANNOT_BN_OPENED, imageNotices.rollingImageNoticeId),
                    this);

                return;
            }

            ShowWebview(string.Concat(imageNotices.address, FIXED_ROLLING_OPTION));
        }

        private void ShowWebview(string url)
        {
            bool hasAdapter = WebviewAdapterManager.Instance.CreateWebviewAdapter("standalonewebviewadapter");
            if (hasAdapter == false)
            {
                closeCallback?.Invoke(new GamebaseError(GamebaseErrorCode.NOT_SUPPORTED, Domain, GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND));
                return;
            }

            var webviewRect = GetWebViewRect(TITLE_BAR_HEIGHT);

            WebviewAdapterManager.Instance.ShowWebView(
               url,
               null,
               (error) =>
               {
               },
               new List<string>()
               {
                    DISMISS_SCHEME,
                    IMAGE_NOTICE_SCHEME
                },
               (scheme, error) =>
               {
                   WebviewAdapterManager.SchemeInfo schemeInfo = WebviewAdapterManager.Instance.ConvertURLToSchemeInfo(scheme);
                   GamebaseLog.Debug(string.Format("scheme:{0}, data:{1}", scheme, JsonMapper.ToJson(schemeInfo.parameterDictionary)), this);

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
                                           if (string.IsNullOrEmpty(schemeInfo.parameterDictionary[ACTION_ID]))
                                           {
                                               GamebaseLog.Warn(MESSAGE_INVALID_ID, this);
                                               return;
                                           }
                                           var imageNoticeId = (long)Convert.ToDouble(schemeInfo.parameterDictionary[ACTION_ID]);
                                           var currentImageNotice = imageNotices.pageList.Find(info => info.imageNoticeId == imageNoticeId);

                                           if (currentImageNotice == null)
                                           {
                                               GamebaseLog.Warn(MESSAGE_NO_DATA_MATCHING_THE_ID, this);
                                               return;
                                           }

                                           OnActionClick(currentImageNotice);
                                           break;
                                       }
                                   case ACTION_NEVER_SHOW_TODAY:
                                       {
                                           OnActionNeverShowToday(imageNotices.rollingImageNoticeId);

                                           WebviewAdapterManager.Instance.CloseWebView();
                                           break;
                                       }
                               }
                               break;
                           }
                   }
               },
               () =>
               {
                   WebviewAdapterManager.Instance.SetBgColor(bgColor);
                   WebviewAdapterManager.Instance.SetWebViewRect(TITLE_BAR_HEIGHT, webviewRect);
                   WebviewAdapterManager.Instance.SetTitleBarColor(bgColor);
                   WebviewAdapterManager.Instance.SetTitleBarButton(false, null, null);
                   WebviewAdapterManager.Instance.SetTitleVisible(false);
               });
        }

        private void OnActionClick(ImageNoticeResponse.ImageNotices.ImageNoticeWeb.ImageNoticeInfo currentImageNotice)
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
                        eventCallback?.Invoke(UnityCompatibility.WebRequest.UnEscapeURL(currentImageNotice.clickScheme), null);
                        break;
                    }
            }
        }

        private void OnActionNeverShowToday(long imageNoticeId)
        {
            var strImageNoticeId = imageNoticeId.ToString();

            if (imageNotices.nextPopupTimeMillis == -1)
            {
                GamebaseLog.Warn(MESSAGE_NEXT_POPUP_TIME_MILLIS_IS_NULL, this);
                return;
            }

            var strNow = imageNotices.nextPopupTimeMillis.ToString();

            if (neverShowTodayState.ContainsKey(strImageNoticeId))
            {
                neverShowTodayState[strImageNoticeId] = strNow;
            }
            else
            {
                neverShowTodayState.Add(strImageNoticeId, strNow);
            }

            PlayerPrefs.SetString(NEVER_SHOW_TODAY_ROLLING_STATE_KEY, JsonMapper.ToJson(neverShowTodayState));
        }

        private void InitNeverShowTodayState(string key)
        {
            var localData = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(localData))
            {
                neverShowTodayState = new Dictionary<string, string>();
            }
            else
            {
                neverShowTodayState = JsonMapper.ToObject<Dictionary<string, string>>(localData);
                RemoveUnusedId();
            }
        }

        private void RemoveUnusedId()
        {
            if (neverShowTodayState.Count == 0)
            {
                return;
            }

            var strImageNoticeId = imageNotices.rollingImageNoticeId.ToString();

            if (neverShowTodayState.ContainsKey(strImageNoticeId))
            {
                if (IsExpired(neverShowTodayState[strImageNoticeId]) == false)
                {
                    return;
                }

                GamebaseLog.Debug(string.Format(
                    "{0}, id:{1}",
                    MESSAGE_TURNED_OFF,
                    strImageNoticeId), this);
            }
            else
            {
                // unused id
                GamebaseLog.Debug(string.Format(
                    "{0}, id:{1}",
                    MESSAGE_REMOVED_NOTICE,
                    strImageNoticeId), this);
            }

            neverShowTodayState.Clear();
            PlayerPrefs.DeleteKey(NEVER_SHOW_TODAY_ROLLING_STATE_KEY);
        }

        private bool IsExpired(string imageNoticeId)
        {
            var savedTime = new DateTime(1970, 1, 1).AddMilliseconds(long.Parse(imageNoticeId)).ToLocalTime();
            var now = DateTime.UtcNow.ToLocalTime();

            return now >= savedTime;
        }

        private bool CheckNeverShowToday(long imageNoticeId)
        {
            return neverShowTodayState.ContainsKey(imageNoticeId.ToString());
        }

        private void IsValidServerResponse(string response, GamebaseCallback.ErrorDelegate callback)
        {
            var vo = JsonMapper.ToObject<ImageNoticeResponse.ImageNotices>(response);
            
            if (vo.header.isSuccessful == false)
            {
                callback(GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain));
                return;
            }

            imageNotices = vo.imageNoticeWeb;

            //1. has image notice
            if (HasImageNotice() == false)
            {
                callback(null);
                return;
            }

            // 2. check popup type.
            if (IsVaildType(imageNotices.type) == false )
            {
                callback(new GamebaseError(GamebaseErrorCode.SERVER_INVALID_RESPONSE, Domain));
                return;
            }

            // 3. check rollingImageNoticeId.
            if (IsValidRollingImageNoticeId() == false)
            {
                callback(new GamebaseError(GamebaseErrorCode.SERVER_INVALID_RESPONSE, Domain));
                return;
            }

            callback(null);
        }

        private bool HasImageNotice()
        {
            return imageNotices.hasImageNotice;
        }

        private bool IsVaildType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return false;
            }

            return type.Equals(TYPE_ROLLING);
        }

        private bool IsValidRollingImageNoticeId()
        {
            return (imageNotices.rollingImageNoticeId != -1);
        }

        #region Get size and position
        private Rect GetWebViewRect(int titleBarHeight)
        {
            var scale = GetScale();

            Vector2 imageSize;

            // Formula for calculating image size: ROLLING
            // width(fixed): ROLLING_IMAGE_WIDTH *  scale
            // height(fixed): ROLLING_IMAGE_HEIGHT * scale + titleBarHeight
            imageSize = new Vector2(STANDARD_IMAGE_WIDTH * scale, STANDARD_IMAGE_HEIGHT * scale + titleBarHeight);

            var size = GetWebViewSize(imageSize);
            var position = GetWebViewPosition(size);

            return new Rect(position, size);
        }

        private float GetScale()
        {
            Vector2 scale = new Vector2(Screen.width / SCREEN_WIDTH, Screen.height / SCREEN_HEIGHT);
            return Math.Min(scale.x, scale.y);
        }

        private Vector2 GetWebViewSize(Vector2 imageSize)
        {
            float ratio = 1;

            if (imageSize.x > STANDARD_IMAGE_WIDTH)
            {
                ratio = STANDARD_IMAGE_WIDTH / imageSize.x;
            }
            else if (imageSize.y > STANDARD_IMAGE_HEIGHT)
            {
                ratio = STANDARD_IMAGE_HEIGHT / imageSize.y;
            }

            imageSize *= ratio;

            return new Vector2((int)imageSize.x, (int)imageSize.y);
        }

        private Vector2 GetWebViewPosition(Vector2 imageSize)
        {
            return new Vector2(
                (int)((Screen.width - imageSize.x) * 0.5f),
                (int)((Screen.height - imageSize.y) * 0.5f));
        }
        #endregion
    }
}
#endif
