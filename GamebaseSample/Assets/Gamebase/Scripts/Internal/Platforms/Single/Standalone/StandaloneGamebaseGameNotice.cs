#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseGameNotice : CommonGamebaseGameNotice
    {
        public StandaloneGamebaseGameNotice()
        {
            Domain = typeof(StandaloneGamebaseGameNotice).Name;
        }

        public override void OpenGameNotice(GamebaseRequest.GameNotice.Configuration configuration, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
            if (callback == null)
            {
                return;
            }
            GamebaseCallbackHandler.UnregisterCallback(handle);
            
            OpenGameNotice(configuration, (gameNoticeInfo, error)=>
            {
                callback?.Invoke(error);
            });
        }
        
        public override void RequestGameNoticeInfo(GamebaseRequest.GameNotice.Configuration configuration, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GameNoticeResponse.GameNoticeInfo>>(handle);
            if (callback == null)
            {
                return;
            }

            GamebaseCallbackHandler.UnregisterCallback(handle);
            
            RequestInfo(configuration, callback);
        }
        
        public void OpenGameNotice(GamebaseRequest.GameNotice.Configuration configuration, GamebaseCallback.GamebaseDelegate<GameNoticeResponse.GameNoticeInfo> callback)
        {
            RequestInfo(configuration, (gameNoticeInfo, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    if (gameNoticeInfo == null)
                    {
                        callback?.Invoke(
                            null,
                            new GamebaseError(
                                GamebaseErrorCode.UI_CONTACT_FAIL_INVALID_URL,
                                message: GamebaseStrings.UI_CONTACT_FAIL_INVALID_URL));
                        return;
                    }
                    
                    GamebaseLog.Debug(string.Format("URL : {0}", gameNoticeInfo.url), this);

                    ShowWebview(gameNoticeInfo.url, (error)=>
                    {
                        callback?.Invoke(gameNoticeInfo, error);
                    });
                }
                else
                {
                    callback?.Invoke(null, error);    
                }
            });
        }
        
        public void RequestInfo(GamebaseRequest.GameNotice.Configuration configuration, GamebaseCallback.GamebaseDelegate<GameNoticeResponse.GameNoticeInfo> callback)
        {
            if (GamebaseUnitySDK.IsInitialized == false)
            {
                callback?.Invoke(null, new GamebaseError(GamebaseErrorCode.NOT_INITIALIZED, message: GamebaseStrings.NOT_INITIALIZED));
                return;
            }
            
            var requestVO = GameNoticeMessage.GetGameNoticesMessage(configuration);
            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                if (Gamebase.IsSuccess(error) == false)
                {
                    callback?.Invoke(null, error);
                    return;
                }

                if (string.IsNullOrEmpty(response))
                {
                    callback?.Invoke(null, new GamebaseError(GamebaseErrorCode.SERVER_UNKNOWN_ERROR, Domain));
                    return;
                }
                
                var vo = JsonMapper.ToObject<GameNoticeResponse>(response);
            
                if (vo.header.isSuccessful == false)
                {
                    callback?.Invoke(null, GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain));
                    return;
                }
                
                callback?.Invoke(vo.gameNotice, null);
            });
        }
        
        private void ShowWebview(string url, GamebaseCallback.ErrorDelegate callback)
        {
            bool hasAdapter = WebviewAdapterManager.Instance.CreateWebviewAdapter("standalonewebviewadapter");
            if (hasAdapter == false)
            {
                callback?.Invoke(new GamebaseError(GamebaseErrorCode.NOT_SUPPORTED, Domain, GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND));
                return;
            }

            var webviewRect = GetWebViewRect(0);
            
            var configuration = new WebViewRequest.Configuration
            {
                barHeight = 0,
                bgColor = new Color(0f, 0f, 0f, 0.5f),
                viewRect = webviewRect,
                webviewType = WebViewRequest.Configuration.WebviewType.FloatingPopup,
                isBackButtonVisible = false,
                isNavigationBarVisible = false
            };

            WebviewAdapterManager.Instance.ShowWebView(
               url,
               configuration,
               callback);
        }
        
        private const float SCREEN_WIDTH = 1920f;
        private const float SCREEN_HEIGHT = 1080f;
        
        private const float STANDARD_IMAGE_WIDTH = 572;
        private const float STANDARD_IMAGE_HEIGHT = 700;
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
            return Mathf.Min(scale.x, scale.y);
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
    }
}
#endif