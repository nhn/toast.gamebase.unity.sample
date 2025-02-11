#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseAuth : CommonGamebaseAuth
    {
        #region scheme
        private const string SCHEME_AUTH_LOGIN = "gamebase://toast.gamebase/auth";
        private const string SCHEME_WEBVIEW_CLOSE = "gamebase://dismiss";

        private const string SCHEME_RESPONSE_KEY_SESSION = "session";
        #endregion

        #region member url
        private const string MEMBER_URL_KEY_SOCIAL_NETWORKING_SERVICE_CODE = "socialNetworkingServiceCode";
        private const string MEMBER_URL_KEY_SOCIAL_NETWORKING_SERVICE_SUB_CODE = "socialNetworkingServiceSubCode";
        private const string MEMBER_URL_KEY_SOCIAL_AUTHORIZATION_PROTOCOL = "authorizationProtocol";
        private const string MEMBER_URL_KEY_SOCIAL_CODE_CHALLENGE = "codeChallenge";
        private const string MEMBER_URL_KEY_CLIENT_ID = "clientId";
        private const string MEMBER_URL_KEY_SVC_URL = "svcUrl";

        private const string MEMBER_URL_VALUE_SIGN_IN_WITH_APPLE_JS = "sign_in_with_apple_js";
        private const string MEMBER_URL_VALUE_X_OAUTH2 = "oauth2";
        #endregion

        private readonly List<string> schemeList;

        public StandaloneGamebaseAuth()
        {
            Domain = typeof(StandaloneGamebaseAuth).Name;
            schemeList = new List<string>() { SCHEME_AUTH_LOGIN, SCHEME_WEBVIEW_CLOSE };
        }

        public override void Login(string providerName, int handle)
        {
            CheckLaunchingStatusExpire(() =>
            {
                GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> providerLoginCallback = (authToken, error) =>
                {
                    #region indicator
                    var item = GetLoginIndicatorCommonData(isLoginSuccess: Gamebase.IsSuccess(error), providerName: providerName);

                    if (Gamebase.IsSuccess(error))
                    {
                        GamebaseIndicatorReport.SetLastLoggedInInfo(providerName, authToken.member.userId);
                    }
                    else
                    {
                        item.error = error;
                        item.isUserCanceled = true;
                    }
                    GamebaseIndicatorReport.AddIndicatorItem(item);
                    #endregion

                    var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                    callback?.Invoke(authToken, error);

                    GamebaseCallbackHandler.UnregisterCallback(handle);
                };

                int providerLoginHandle = GamebaseCallbackHandler.RegisterCallback(providerLoginCallback);

                if (providerName.Equals(GamebaseAuthProvider.GUEST) || GamebaseUnitySDK.UseWebViewLogin == false || IsSupportedIDPByWebview(providerName) == false)
                {
                    base.Login(providerName, providerLoginHandle);
                }
                else
                {
                    WebViewLoginWithIdP(providerName, providerLoginHandle);
                }
            });
        }

        public override void Login(string providerName, Dictionary<string, object> additionalInfo, int handle)
        {
            CheckLaunchingStatusExpire(() =>
            {
                GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> providerLoginCallback = (authToken, error) =>
                {
                    #region indicator
                    var item = GetLoginIndicatorCommonData(isLoginSuccess: Gamebase.IsSuccess(error), providerName: providerName);
                    item.customFields.Add(GamebaseIndicatorReportType.AdditionalKey.GB_CREDENTIAL, JsonMapper.ToJson(additionalInfo));

                    if (Gamebase.IsSuccess(error) == true)
                    {
                        GamebaseIndicatorReport.SetLastLoggedInInfo(providerName, authToken.member.userId);
                    }
                    else
                    {
                        item.error = error;
                        item.isUserCanceled = true;
                    }
                    GamebaseIndicatorReport.AddIndicatorItem(item);
                    #endregion

                    var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                    callback?.Invoke(authToken, error);

                    GamebaseCallbackHandler.UnregisterCallback(handle);
                };

                int providerLoginHandle = GamebaseCallbackHandler.RegisterCallback(providerLoginCallback);

                if (providerName.Equals(GamebaseAuthProvider.GUEST) || GamebaseUnitySDK.UseWebViewLogin == false || IsSupportedIDPByWebview(providerName) == false)
                {
                    base.Login(providerName, additionalInfo, providerLoginHandle);
                }
                else
                {
                    WebViewLoginWithIdP(providerName, providerLoginHandle, additionalInfo);
                }
            });
        }

        private void WebViewLoginWithIdP(string providerName, int handle, Dictionary<string, object> additionalInfo = null)
        {
            if (CanLogin(handle) == false)
            {
                return;
            }

            bool hasAdapter = WebviewAdapterManager.Instance.CreateWebviewAdapter("standalonewebviewadapter");
            if (hasAdapter == false)
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                GamebaseCallbackHandler.UnregisterCallback(handle);

                callback?.Invoke(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, message: GamebaseStrings.WEBVIEW_ADAPTER_NOT_FOUND));
                return;
            }

            isAuthenticationAlreadyProgress = true;

            GetAccessToken(providerName, (requestVO) =>
            {
                if (requestVO is null)
                {
                    isAuthenticationAlreadyProgress = false;
                    LoginFailedCallback(handle);
                    return;
                }

                RequestGamebaseLogin(requestVO, handle);
            }, additionalInfo);
        }

        private string GetLineRegion(string providerName, Dictionary<string, object> additionalInfo)
        {
            if (providerName.Equals(GamebaseAuthProvider.LINE) is false)
            {
                return string.Empty;
            }

            if (additionalInfo == null)
            {
                return string.Empty;
            }

            if (additionalInfo.TryGetValue(GamebaseAuthProviderCredential.LINE_CHANNEL_REGION, out var lineRegion))
            {
                return lineRegion.ToString();
            }

            return string.Empty;
        }

        private void GetAccessToken(string providerName, Action<WebSocketRequest.RequestVO> callback, Dictionary<string, object> additionalInfo = null)
        {
            var memberUrl = GetMemberUrl(providerName, additionalInfo);

            if (string.IsNullOrEmpty(memberUrl))
            {
                callback(null);
                return;
            }

            var loginWebView = LaunchingInfoHelper.GetLoginWebView(providerName);
            WebViewRequest.TitleBarConfiguration titleBarConfig = null;
            WebSocketRequest.RequestVO requestVO = null;

            if (loginWebView != null)
            {
                titleBarConfig = new WebViewRequest.TitleBarConfiguration
                {
                    title = loginWebView.title,
                    barHeight = WebViewConst.TITLEBAR_DEFAULT_HIGHT
                };

                ColorUtility.TryParseHtmlString(loginWebView.titleTextColor, out titleBarConfig.titleColor);
                ColorUtility.TryParseHtmlString(loginWebView.titleBgColor, out titleBarConfig.bgColor);
            }

            WebviewAdapterManager.Instance.ShowLoginWebView(
                memberUrl,
                titleBarConfig,
                (error) =>
                {
                    callback(requestVO);
                },
                schemeList,
                (scheme, error) =>
                {
                    WebviewAdapterManager.SchemeInfo schemeInfo = WebviewAdapterManager.Instance.ConvertURLToSchemeInfo(scheme);

                    if (schemeInfo.scheme.Equals(SCHEME_AUTH_LOGIN))
                    {
                        if (schemeInfo.parameterDictionary.TryGetValue(SCHEME_RESPONSE_KEY_SESSION, out var session))
                        {                            
                            requestVO = AuthMessage.GetWebViewLoginMessage(
                                providerName:providerName,
                                session:session,
                                region:GetLineRegion(providerName, additionalInfo));
                            WebviewAdapterManager.Instance.CloseWebView();
                        }
                    }
                    else if(schemeInfo.scheme.Equals(SCHEME_WEBVIEW_CLOSE))
                    {
                        WebviewAdapterManager.Instance.CloseWebView();
                    }
                });
        }

        private string GetMemberUrl(string providerName, Dictionary<string, object> additionalInfo = null)
        {
            var gamebaseUrl = LaunchingInfoHelper.GetGamebaseUrl();

            if (string.IsNullOrEmpty(gamebaseUrl))
            {
#if UNITY_EDITOR
                GamebaseLog.Error("You need to switch platform the Standalone.", this);
#else                
                GamebaseLog.Debug("launchingInfo.launching.standalone is null.", this);
#endif
                return string.Empty;
            }

            var gbIdInfo = LaunchingInfoHelper.GetIdPInfo("gbid");

            if (gbIdInfo is null)
            {
                GamebaseLog.Warn("gbid is null.", this);
                return string.Empty;
            }

            var memberUrl = new StringBuilder(gamebaseUrl);
            memberUrl.AppendFormat("?{0}={1}", MEMBER_URL_KEY_SOCIAL_NETWORKING_SERVICE_CODE, providerName);
            memberUrl.AppendFormat("&{0}={1}", MEMBER_URL_KEY_CLIENT_ID, gbIdInfo.clientId);
            memberUrl.AppendFormat("&{0}={1}", MEMBER_URL_KEY_SVC_URL, Uri.EscapeDataString(SCHEME_AUTH_LOGIN));
            memberUrl.Append(GetSignInWithAppleJsFormat(providerName));
            memberUrl.Append(GetLineRegionFormat(providerName, additionalInfo));
            memberUrl.Append(GetXFormat(providerName));

            #region This setting is for Hangame JP only.
            //if (providerName.Equals(GamebaseAuthProvider.HANGAME_JP))
            //{
            //    memberUrl.AppendFormat("&{0}={1}", "isMobile", "true");
            //}
            #endregion

            return memberUrl.ToString();
        }

        private string GetSignInWithAppleJsFormat(string providerName)
        {
            if (providerName.Equals(GamebaseAuthProvider.APPLEID))
            {
                return string.Format("&{0}={1}", MEMBER_URL_KEY_SOCIAL_NETWORKING_SERVICE_SUB_CODE, MEMBER_URL_VALUE_SIGN_IN_WITH_APPLE_JS);
            }

            return string.Empty;
        }

        private string GetLineRegionFormat(string providerName, Dictionary<string, object> additionalInfo = null)
        {
            var region = GetLineRegion(providerName, additionalInfo);
            if (string.IsNullOrEmpty(region))
            {
                return string.Empty;
            }

            return string.Format("&{0}={1}", MEMBER_URL_KEY_SOCIAL_NETWORKING_SERVICE_SUB_CODE, region);
        }

        private string GetXFormat(string providerName)
        {
            if (providerName.Equals(GamebaseAuthProvider.TWITTER))
            {
                return string.Format(
                    "&{0}={1}&{2}={3}",
                    MEMBER_URL_KEY_SOCIAL_AUTHORIZATION_PROTOCOL,
                    MEMBER_URL_VALUE_X_OAUTH2,
                    MEMBER_URL_KEY_SOCIAL_CODE_CHALLENGE,
                    GeneratePKCECodeChallenge());
            }

            return string.Empty;
        }

        /// <summary>
        /// cryptographically random string using the characters A-Z, a-z, 0-9, and the punctuation characters -._~ (hyphen, period, underscore, and tilde),
        /// between 43 and 128 characters long.
        /// <para><see href="https://www.oauth.com/oauth2-servers/pkce/authorization-request/">Protecting Apps with PKCE: Authorization Request</see></para>
        /// 
        /// Code Challenge has two modes (plain or SHA-256). Plain is used to maintain the same specifications as the member server.
        /// <para><see href="https://nhnent.dooray.com/share/pages/ItYTumRbQ16jSjpEc0rfog/3919056334930446922">Member spec</see></para>
        /// </summary>
        /// <returns>code challenge</returns>
        public string GeneratePKCECodeChallenge()
        {
            using var generator = RandomNumberGenerator.Create();
            var randomBytes = new byte[33];
            generator.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }

        private bool IsSupportedIDPByWebview(string providerName)
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE
            return true;
#else
            return false;
#endif
        }

        private void LoginFailedCallback(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseCallbackHandler.UnregisterCallback(handle);

            callback?.Invoke(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED));
        }
    }
}
#endif