#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Text;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseAuth : CommonGamebaseAuth
    {
        private const string SCHEME_AUTH_LOGIN = "gamebase://toast.gamebase/auth";
        private const string ACCESS_TOKEN_KEY = "token";
        private const string FACEBOOK_PREMISSION = "facebook_permission";
        private const string SERVICE_CODE = "service_code";

        public StandaloneGamebaseAuth()
        {
            Domain = typeof(StandaloneGamebaseAuth).Name;
        }

        public override void Login(string providerName, int handle)
        {
            CheckLaunchingStatusExpire(() =>
            {
                GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> providerLoginCallback = (authToken, error) =>
                {
                    if (Gamebase.IsSuccess(error) == true)
                    {
                        GamebaseIndicatorReport.SetLastLoggedInInfo(providerName, authToken.member.userId);
                        GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.AUTH,
                            GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGIN_SUCCESS,
                            GamebaseIndicatorReportType.LogLevel.INFO,
                            new Dictionary<string, string>()
                            {
                                { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGIN },
                                { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, providerName }
                            });
                    }
                    else
                    {
                        GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.AUTH,
                            GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGIN_CANCELED,
                            GamebaseIndicatorReportType.LogLevel.DEBUG,
                            new Dictionary<string, string>()
                            {
                                { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGIN },
                                { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, providerName }
                            });
                    }

                    var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);

                    if (callback != null)
                    {
                        callback(authToken, error);
                    }

                    GamebaseCallbackHandler.UnregisterCallback(handle);
                };

                int providerLoginHandle = GamebaseCallbackHandler.RegisterCallback(providerLoginCallback);

                LoginWithProviderName(providerName, providerLoginHandle);
            });
        }

        private void LoginWithProviderName(string providerName, int handle)
        {
            if (GamebaseUnitySDK.UseWebViewLogin == false)
            {
                base.Login(providerName, handle);
                return;
            }

            if (IsSupportedIDPByWebview(providerName) == false)
            {
                base.Login(providerName, handle);
                return;
            }

            if (CanLogin(handle) == false)
            {
                return;
            }

            if (providerName.Equals(GamebaseAuthProvider.GUEST, StringComparison.Ordinal) == true)
            {
                var requestVO = AuthMessage.GetIDPLoginMessage(providerName);
                RequestGamebaseLogin(requestVO, handle);
                return;
            }

            bool hasAdapter = WebviewAdapterManager.Instance.CreateWebviewAdapter("standalonewebviewadapter");
            if (hasAdapter == false)
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                GamebaseCallbackHandler.UnregisterCallback(handle);
                callback(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, message: GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND));
                return;
            }

            isAuthenticationAlreadyProgress = true;

            GetAccessToken(providerName, (requestVO) =>
            {
                if (requestVO == null)
                {
                    isAuthenticationAlreadyProgress = false;
                    LoginFailedCallback(handle);
                    return;
                }

                RequestGamebaseLogin(requestVO, handle);
            });
        }

        private void GetAccessToken(string providerName, System.Action<WebSocketRequest.RequestVO> callback)
        {
            WebSocketRequest.RequestVO requestVO = null;

            GamebaseResponse.Launching.LaunchingInfo launchingInfo = Gamebase.Launching.GetLaunchingInformations();

            if (launchingInfo.launching.app.loginUrls == null||
                string.IsNullOrEmpty(launchingInfo.launching.app.loginUrls.gamebaseUrl) == true)
            {
#if UNITY_EDITOR
                GamebaseLog.Error("You need to switch platform the Standalone.", this);
#else                
                GamebaseLog.Debug("launchingInfo.launching.standalone is null.", this);
#endif
                callback(requestVO);
                return;
            }

            if (launchingInfo == null)
            {
                GamebaseLog.Debug("launchingInfo is null.", this);
                callback(requestVO);
                return;
            }

            GamebaseResponse.Launching.LaunchingInfo.GamebaseLaunching.APP.LaunchingIDPInfo launchingIDPInfo = launchingInfo.launching.app.idP["gbid"];

            if (launchingIDPInfo == null)
            {
                GamebaseLog.Debug("gbid is null.", this);
                callback(requestVO);
                return;
            }

            string clientID = launchingIDPInfo.clientId;

            StringBuilder url = new StringBuilder(launchingInfo.launching.app.loginUrls.gamebaseUrl);
            url.AppendFormat("?clientId={0}", clientID);
            url.AppendFormat("&snsCd={0}", providerName);
            url.AppendFormat("&svcUrl={0}", Uri.EscapeDataString(SCHEME_AUTH_LOGIN));
            url.AppendFormat("&tokenKind={0}", "SNS");
            url.Append("&isMobile=true");
            url.Append(MakeProviderAdditionalInfo(providerName));

            GamebaseObserverManager.Instance.OnObserverEvent(
                    new GamebaseResponse.SDK.ObserverMessage()
                    {
                        type = GamebaseObserverType.WEBVIEW,
                        data = new Dictionary<string, object>()
                        {
                            { "code ", GamebaseWebViewEventType.OPEN }
                        }
                    });

            WebviewAdapterManager.Instance.ShowWebView(
                url.ToString(),
                null,
                (error) =>
                {
                    callback(requestVO);

                    GamebaseObserverManager.Instance.OnObserverEvent(
                    new GamebaseResponse.SDK.ObserverMessage()
                    {
                        type = GamebaseObserverType.WEBVIEW,
                        data = new Dictionary<string, object>()
                        {
                            { "code ", GamebaseWebViewEventType.CLOSE }
                        }
                    });
                },
                new List<string>() { SCHEME_AUTH_LOGIN },
                (scheme, error) =>
                {
                    WebviewAdapterManager.SchemeInfo schemeInfo = WebviewAdapterManager.Instance.ConvertURLToSchemeInfo(scheme);

                    var idPAccessToken = string.Empty;
                    if (schemeInfo.parameterDictionary.TryGetValue(ACCESS_TOKEN_KEY, out idPAccessToken) == true)
                    {
                        idPAccessToken = schemeInfo.parameterDictionary[ACCESS_TOKEN_KEY];
                        requestVO = AuthMessage.GetIDPLoginMessage(providerName, idPAccessToken);

                        WebviewAdapterManager.Instance.CloseWebView();
                    }
                }
                );
        }

        private bool IsSupportedIDPByWebview(string providerName)
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            return providerName.Equals(GamebaseAuthProvider.GUEST, StringComparison.Ordinal) == true ||
                   providerName.Equals(GamebaseAuthProvider.FACEBOOK, StringComparison.Ordinal) == true ||
                   providerName.Equals(GamebaseAuthProvider.GOOGLE, StringComparison.Ordinal) == true ||
                   providerName.Equals(GamebaseAuthProvider.TWITTER, StringComparison.Ordinal) == true ||
                   providerName.Equals(GamebaseAuthProvider.LINE, StringComparison.Ordinal) == true ||
                   providerName.Equals(GamebaseAuthProvider.NAVER, StringComparison.Ordinal) == true ||
                   providerName.Equals(GamebaseAuthProvider.PAYCO, StringComparison.Ordinal) == true;

#elif UNITY_FACEBOOK || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            return providerName == GamebaseAuthProvider.GUEST;
#endif
        }

        private void LoginFailedCallback(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseCallbackHandler.UnregisterCallback(handle);
            callback(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED));
        }

        private string MakeProviderAdditionalInfo(string providerName)
        {
            StringBuilder additionalInfo = new StringBuilder();

            var launchingInfo = Gamebase.Launching.GetLaunchingInformations();
            if (launchingInfo == null)
            {
                return additionalInfo.ToString();
            }

            var idP = launchingInfo.launching.app.idP[providerName];
            if (idP == null)
            {
                return additionalInfo.ToString();
            }

            var additional = launchingInfo.launching.app.idP[providerName].additional;
            if (string.IsNullOrEmpty(additional) == true)
            {
                return additionalInfo.ToString();
            }

            switch (providerName)
            {
                case GamebaseAuthProvider.FACEBOOK:
                    {
                        JsonData data = JsonMapper.ToObject(additional);
                        if (data != null && data.Keys.Contains(FACEBOOK_PREMISSION) == true)
                        {
                            additionalInfo.Append("&snsScope=");

                            foreach (var scope in data[FACEBOOK_PREMISSION])
                            {
                                additionalInfo.Append(scope.ToString()).Append(",");
                            }

                            additionalInfo.Remove(additionalInfo.Length - 1, 1);
                        }
                        break;
                    }
                case GamebaseAuthProvider.PAYCO:
                    {
                        JsonData data = JsonMapper.ToObject(additional);
                        if (data != null && data.Keys.Contains(SERVICE_CODE) == true)
                        {
                            string service_provider_code = data[SERVICE_CODE].ToString();
                            if (string.IsNullOrEmpty(service_provider_code) == false)
                            {
                                additionalInfo.Append("&serviceProviderCode=").Append(service_provider_code);
                            }
                        }
                        break;
                    }
            }

            return additionalInfo.ToString();
        }
    }
}
#endif