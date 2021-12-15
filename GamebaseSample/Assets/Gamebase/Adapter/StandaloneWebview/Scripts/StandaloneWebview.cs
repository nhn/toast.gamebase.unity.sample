#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using System;
using System.Collections.Generic;
using Toast.Cef.Webview;
using Toast.Gamebase.Adapter.Ui;
using Toast.Gamebase.Internal;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Adapter
{
    public class StandaloneWebview
    {
        private const string POPUP_BLOCK_MESSAGE = "New window is not supported!!";

        /// <summary>
        /// Save User setting before webview open.
        /// </summary>
        private IMECompositionMode imeCompositionModeBackup;

        private StandaloneWebviewUI webviewUi;

        private GamebaseCallback.ErrorDelegate closeCallback;
        private GamebaseCallback.GamebaseDelegate<string> schemeEvent;

        private int webviewIndex = -1;

        private static readonly StandaloneWebview instance = new StandaloneWebview();

        public static StandaloneWebview Instance
        {
            get { return instance; }
        }

        public StandaloneWebview()
        {
            InitializeWebViewUI();
        }

        public void ShowWebView(string webviewUrl,
            GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration,
            GamebaseCallback.ErrorDelegate closeCallback = null,
            List<string> schemeList = null,
            GamebaseCallback.GamebaseDelegate<string> schemeEvent = null)
        {
            SetWebViewUI(configuration);

            this.closeCallback = closeCallback;
            this.schemeEvent = schemeEvent;
            
            SendObserverMessage(GamebaseWebViewEventType.OPEN);

            if (CefWebview.IsInitialized() == true)
            {
                CreateCefWebview((isSuccess) =>
                {
                    if (isSuccess == true)
                    {
                        ShowWebview(webviewUrl, schemeList, HasTitle(configuration));
                    }
                });
            }
            else
            {
                CefWebview.SetDebugEnable(GamebaseLog.isDebugLog);

                InitializeCefWebview((isInitSuccess) =>
                {
                    if (isInitSuccess == true)
                    {
                        CreateCefWebview((isSuccess) =>
                        {
                            if (isSuccess == true)
                            {
                                ShowWebview(webviewUrl, schemeList, HasTitle(configuration));
                            }
                        });
                    }
                });
            }
        }

        public void ShowLoginWebView(
            string webviewUrl,
            WebViewRequest.TitleBarConfiguration configuration,
            GamebaseCallback.ErrorDelegate closeCallback = null,
            List<string> schemeList = null,
            GamebaseCallback.GamebaseDelegate<string> schemeEvent = null)
        {
            webviewUi.Title = configuration.title;
            webviewUi.IsActivated = true;
            webviewUi.SetTitleVisible(true);
            webviewUi.SetTitleText(configuration.title);            
            webviewUi.SetTitleBarRect(new Rect(0, 0, Screen.width, configuration.barHeight));
            webviewUi.SetTitleTextColor(configuration.titleColor);
            webviewUi.SetTitleBarColor(configuration.bgColor);
            webviewUi.SetBgColor(new Color());
            webviewUi.SetTitleBarButton(true, string.Empty, string.Empty);            

            this.closeCallback = closeCallback;
            this.schemeEvent = schemeEvent;

            SendObserverMessage(GamebaseWebViewEventType.OPEN);

            if (CefWebview.IsInitialized() == true)
            {
                CreateCefWebview((isSuccess) =>
                {
                    if (isSuccess == true)
                    {
                        ShowWebview(webviewUrl, schemeList, true);
                    }
                });
            }
            else
            {
                CefWebview.SetDebugEnable(GamebaseLog.isDebugLog);

                InitializeCefWebview((isInitSuccess) =>
                {
                    if (isInitSuccess == true)
                    {
                        CreateCefWebview((isSuccess) =>
                        {
                            if (isSuccess == true)
                            {
                                ShowWebview(webviewUrl, schemeList, true);
                            }
                        });
                    }
                });
            }
        }

        public void SetTitleBarEnable(bool enable)
        {
            webviewUi.SetTitleBarEnable(enable);
        }

        public void SetTitleVisible(bool isTitleVisible)
        {
            webviewUi.SetTitleVisible(isTitleVisible);
        }

        public void SetTitleText(string title)
        {
            webviewUi.SetTitleText(title);
        }

        public void SetTitleBarColor(Color bgColor)
        {
            webviewUi.SetTitleBarColor(bgColor);
        }

        public void SetTitleBarButton(bool isBackButtonVisible, string backButtonName, string homeButtonName)
        {
            webviewUi.SetTitleBarButton(isBackButtonVisible, backButtonName, homeButtonName);
        }

        public void SetWebViewRect(int titleBarHeight, Rect rect)
        {
            webviewUi.SetTitleBarRect(new Rect(rect.x, rect.y, rect.width, titleBarHeight));
            webviewUi.SetBGRect(rect);
            CefWebview.ResizeWebview(
                webviewIndex, 
                new Rect(new Rect(rect.x, rect.y + titleBarHeight, rect.width, rect.height - titleBarHeight)), 
                (error) => { });
        }

        public void SetBgColor(Color bgColor)
        {
            webviewUi.SetBgColor(bgColor);
        }

        public void CloseWebView()
        {
            Input.imeCompositionMode = imeCompositionModeBackup;

            CefWebview.HideWebview(webviewIndex, (error) => 
            {
                if(CefWebview.IsSuccess(error) == true)
                {
                    if (schemeEvent != null)
                    {
                        schemeEvent = null;
                    }

                    webviewUi.IsActivated = false;

                    if (closeCallback != null)
                    {
                        closeCallback(null);
                    }
                    SendObserverMessage(GamebaseWebViewEventType.CLOSE);
                }
            });            
        }

        private void InitializeWebViewUI()
        {
            if (webviewUi == null)
            {
                webviewUi = GamebaseComponentManager.AddComponent<StandaloneWebviewUI>(GamebaseGameObjectManager.GameObjectType.CORE_TYPE);
                webviewUi.Initialize();
                webviewUi.OnBackButton = () =>
                {
                    CefWebview.GoBack(webviewIndex, (error) =>
                    {
                        if (CefWebview.IsSuccess(error) == true)
                        {
                            GamebaseLog.Debug("CefWebview::GoBack succeeded.", this);
                        }
                        else
                        {
                            GamebaseLog.Debug(string.Format("CefWebview::GoBack failed. \n", error), this);
                        }
                    });
                };
                webviewUi.OnCloaseButton = () =>
                {
                    CloseWebView();
                };
            }
        }

        private void InitializeCefWebview(Action<bool> callback)
        {
            if (CefWebview.IsInitialized() == true)
            {
                callback(true);
                return;
            }

            imeCompositionModeBackup = Input.imeCompositionMode;
            Input.imeCompositionMode = IMECompositionMode.On;

            CefWebview.Initialize(Gamebase.GetDisplayLanguageCode(), (error) =>
            {
                if (CefWebview.IsSuccess(error) == true)
                {
                    GamebaseLog.Debug("CefWebview::Initialize succeeded.", this);
                }
                else
                {
                    GamebaseLog.Debug(string.Format("CefWebview::Initialize failed. \n", error), this);
                }

                callback(CefWebview.IsSuccess(error));
            });
        }

        private void CreateCefWebview(Action<bool> callback)
        {
            if (webviewIndex != -1)
            {
                callback(true);
                return;
            }

            var configuration = new RequestVo.WebviewConfiguration()
            {
                useTexture = false,
                viewRect = new Rect(0, webviewUi.TitleBarHeight, Screen.width, Screen.height - webviewUi.TitleBarHeight),
                bgType = BgType.OPAQUE,
                popupOption = new RequestVo.WebviewConfiguration.PopupOption
                {
                    type = PopupType.BLOCK,
                    blockMessage = POPUP_BLOCK_MESSAGE
                }
            };

            CefWebview.CreateWebview(configuration, (webviewInfo, error) =>
            {
                if (CefWebview.IsSuccess(error) == true)
                {
                    GamebaseLog.Debug("CefWebview::CreateWebview succeeded.", this);
                    webviewIndex = webviewInfo.index;
                }
                else
                {
                    GamebaseLog.Debug(string.Format("CefWebview::CreateWebview failed. \n", error), this);
                }

                callback(CefWebview.IsSuccess(error));
            });
        }

        private void ShowWebview(string webviewUrl, List<string> schemeList, bool hasTitle = false)
        {
            CefWebview.ShowWebview(
                webviewIndex,
                webviewUrl,
                true,
                (error) =>
                {
                    if (CefWebview.IsSuccess(error) == true)
                    {
                        GamebaseLog.Debug("CefWebview::ShowWebview succeeded.", this);
                    }
                    else
                    {
                        GamebaseLog.Debug(string.Format("CefWebview::ShowWebview failed. \n{0}", error), this);
                    }
                },
                (url) =>
                {
                    if (schemeEvent == null || schemeList == null)
                    {
                        return;
                    }

                    foreach (string scheme in schemeList)
                    {
                        if (scheme.Length <= url.Length)
                        {
                            if (url.Substring(0, scheme.Length).Equals(scheme) == true)
                            {
                                schemeEvent(url, null);
                                break;
                            }
                        }
                    }
                },
                (title) =>
                {
                    if (hasTitle == false)
                    {
                        webviewUi.Title = title;
                    }
                },
                (type) =>
                {
                    if (type == InputElementType.PASSWORD)
                    {
                        Input.imeCompositionMode = IMECompositionMode.Off;
                    }
                    else
                    {
                        Input.imeCompositionMode = IMECompositionMode.On;
                    }
                },
                (webviewStatus) =>
                {
                    GamebaseLog.Debug(string.Format("status:{0}", webviewStatus.status), this);
                });

            CefWebview.SetFocus(webviewIndex, true, (error) =>
            {
                if (CefWebview.IsSuccess(error) == true)
                {
                    GamebaseLog.Debug("CefWebview::SetFocus succeeded.", this);
                }
                else
                {
                    GamebaseLog.Debug(string.Format("CefWebview::SetFocus failed. \n", error), this);
                }
            });

            if (schemeList != null)
            {
                CefWebview.SetInvalidRedirectUrlScheme(schemeList, (error) =>
                {
                    if (CefWebview.IsSuccess(error) == true)
                    {
                        GamebaseLog.Debug("CefWebview::SetInvalidRedirectUrlScheme succeeded.", this);
                    }
                    else
                    {
                        GamebaseLog.Debug(string.Format("CefWebview::SetInvalidRedirectUrlScheme failed. \n", error), this);
                    }
                });
            }
        }

        private void SetWebViewUI(GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration)
        {
            if (configuration == null)
            {
                configuration = new GamebaseRequest.Webview.GamebaseWebViewConfiguration
                {
                    orientation = -1,
                    colorR = 75,
                    colorG = 150,
                    colorB = 230,
                    colorA = 255,
                    barHeight = 41,
                    isBackButtonVisible = true
                };
            }
            else
            {
                if (string.IsNullOrEmpty(configuration.title) == false)
                {
                    webviewUi.Title = configuration.title;
                }
            }

            if (configuration.barHeight <= 0)
            {
                webviewUi.IsActivated = false;
            }
            else
            {
                webviewUi.IsActivated = true;
            }

            webviewUi.SetTitleVisible(true);
            webviewUi.SetTitleText(configuration.title);
            webviewUi.SetTitleTextColor(Color.white);
            webviewUi.SetTitleBarRect(new Rect(0, 0, Screen.width, configuration.barHeight));
            webviewUi.SetTitleBarColor(
                new Color(
                    configuration.colorR / 255f, 
                    configuration.colorG / 255f, 
                    configuration.colorB / 255f, 
                    configuration.colorA / 255f));
            webviewUi.SetBgColor(new Color());
            if (configuration.isBackButtonVisible == true)
            {
                webviewUi.SetTitleBarButton(
                    configuration.isBackButtonVisible,
                    configuration.backButtonImageResource,
                    configuration.closeButtonImageResource
                    );
            }
        }

        private void SendObserverMessage(string code)
        {
            GamebaseResponse.SDK.ObserverMessage observerMessage = new GamebaseResponse.SDK.ObserverMessage()
            {
                type = GamebaseObserverType.WEBVIEW,
                data = new Dictionary<string, object>()
                {
                    { "code", code }
                }
            };

            GamebaseObserverManager.Instance.OnObserverEvent(observerMessage);
            SendEventMessage(observerMessage);
        }

        private void SendEventMessage(GamebaseResponse.SDK.ObserverMessage message)
        {
            GamebaseResponse.Event.GamebaseEventObserverData observerData = new GamebaseResponse.Event.GamebaseEventObserverData();

            object codeData = null;
            if (message.data.TryGetValue("code", out codeData) == true)
            {
                string code = (string)codeData;
                switch (code)
                {
                    case GamebaseWebViewEventType.OPEN:
                        {
                            observerData.code = GamebaseWebViewEventType.OPENED;
                            break;
                        }
                    case GamebaseWebViewEventType.CLOSE:
                        {
                            observerData.code = GamebaseWebViewEventType.CLOSED;
                            break;
                        }
                }
            }

            GamebaseResponse.Event.GamebaseEventMessage eventMessage = new GamebaseResponse.Event.GamebaseEventMessage();
            eventMessage.category = string.Format("observer{0}", GamebaseStringUtil.Capitalize(message.type));
            eventMessage.data = JsonMapper.ToJson(observerData);

            GamebaseEventHandlerManager.Instance.OnEventHandler(eventMessage);
        }

        private bool HasTitle(GamebaseRequest.Webview.GamebaseWebViewConfiguration configuration)
        {
            return (configuration != null && string.IsNullOrEmpty(configuration.title) == false);
        }

        private bool HasTitle(string title)
        {
            return (string.IsNullOrEmpty(title) == false);
        }
    }
}
#endif