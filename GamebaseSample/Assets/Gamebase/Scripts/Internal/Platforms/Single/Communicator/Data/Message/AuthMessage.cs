#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System;
using System.Collections.Generic;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class AuthMessage
    {
        private const string SUB_CODE_SIGN_IN_WITH_APPLE_JS = "sign_in_with_apple_js";
        private const string SUB_CODE_WEB = "web";
        private const string AUTHORIZATION_PROTOCOL_X_OAUTH2 = "oauth2";

        public static WebSocketRequest.RequestVO GetWebViewLoginMessage(
            string providerName,
            string session = null,
            string authorizationCode = null,
            string redirectUri = null,
            string region = null)
        {
            var vo = GetIDPLoginMessage(providerName, string.Empty, authorizationCode, redirectUri);
            var payload = JsonMapper.ToObject<AuthRequest.LoginVO.Payload>(vo.payload);

            payload.idPInfo.session = session;

            switch (providerName)
            {
                case GamebaseAuthProvider.APPLEID:
                    {
                        payload.idPInfo.subCode = SUB_CODE_SIGN_IN_WITH_APPLE_JS;
                        break;
                    }
                case GamebaseAuthProvider.HANGAME:
                    {
                        payload.idPInfo.subCode = SUB_CODE_WEB;
                        break;
                    }
                case GamebaseAuthProvider.LINE:
                    {
                        if (string.IsNullOrEmpty(region))
                        {
                            break;
                        }

                        payload.idPInfo.subCode = region;
                        payload.idPInfo.clientId = LaunchingInfoHelper.GetClientIdForLineRegion(region);
                        payload.idPInfo.extraParams = new Dictionary<string, string>() { { GamebaseAuthProviderCredential.CLIENT_ID, LaunchingInfoHelper.GetClientIdForLineRegion(region) } };

                        break;
                    }
                case GamebaseAuthProvider.TWITTER:
                    {
                        payload.idPInfo.authorizationProtocol = AUTHORIZATION_PROTOCOL_X_OAUTH2;
                        break;
                    }
            }


            vo.payload = JsonMapper.ToJson(payload);
            return vo;
        }

        public static WebSocketRequest.RequestVO GetIDPLoginMessage(
            string providerName, 
            string accessToken = null,
            string authorizationCode = null,
            string redirectUri = null)
        {
            var launchingInfoVO = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
            var idpDic = launchingInfoVO.launching.app.idP;

            var vo = new AuthRequest.LoginVO();
            vo.parameter.appId = GamebaseUnitySDK.AppID;
            
            if (providerName.Equals(GamebaseAuthProvider.GUEST, StringComparison.Ordinal) == true)
            {
                vo.payload.idPInfo.accessToken = string.Format("GAMEBASE{0}", GamebaseSystemInfo.UUID);
            }
            else
            {
                vo.payload.idPInfo.accessToken = accessToken;
            }

            vo.payload.idPInfo.redirectUri = redirectUri;
            vo.payload.idPInfo.authorizationCode = authorizationCode;

            if (idpDic.ContainsKey(providerName))
            {
                vo.payload.idPInfo.clientId = idpDic[providerName].clientId;
            }

            vo.payload.idPInfo.idPCode = providerName;
            vo.payload.member.clientVersion = GamebaseUnitySDK.AppVersion;
            vo.payload.member.deviceCountryCode = GamebaseSystemInfo.CountryCode;
            vo.payload.member.deviceKey = GamebaseSystemInfo.DeviceKey;
            vo.payload.member.deviceModel = GamebaseSystemInfo.DeviceModel;
            vo.payload.member.osVersion = GamebaseSystemInfo.OsVersion;
            vo.payload.member.deviceLanguage = GamebaseSystemInfo.DeviceLanguageCode;
            vo.payload.member.displayLanguage = GamebaseUnitySDK.DisplayLanguageCode;
            vo.payload.member.network = Application.internetReachability.ToString();
            vo.payload.member.osCode = GamebaseSystemInfo.Platform;
            vo.payload.member.sdkVersion = GamebaseUnitySDK.SDKVersion;
            vo.payload.member.storeCode = launchingInfoVO.launching.app.storeCode;
            vo.payload.member.telecom = string.Empty;
            vo.payload.member.usimCountryCode = "ZZ";
            vo.payload.member.uuid = GamebaseSystemInfo.UUID;

            var requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.IDP_LOGIN,
                parameters = vo.parameter,
                payload = JsonMapper.ToJson(vo.payload)
            };

            return requestVO;
        }
        
        public static WebSocketRequest.RequestVO GetLogoutMessage()
        {
            var loginInfoVO = DataContainer.GetData<AuthResponse.LoginInfo>(VOKey.Auth.LOGIN_INFO);
            var vo = new AuthRequest.LogoutVO();
            vo.parameter.appId = GamebaseUnitySDK.AppID;
            vo.parameter.userId = loginInfoVO.member.userId;
            vo.parameter.accessToken = loginInfoVO.token.accessToken;

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.LOGOUT,
                parameters = vo.parameter
            };

            return requestVO;
        }

        public static WebSocketRequest.RequestVO GetWithdrawMessage()
        {
            var vo = new AuthRequest.WithdrawVO();
            vo.parameter.userId = Gamebase.GetUserID();

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.WITHDRAW,
                parameters = vo.parameter
            };

            return requestVO;
        }

        public static WebSocketRequest.RequestVO GetTemporaryWithdrawalMessage()
        {
            var vo = new AuthRequest.TemporaryWithdrawalVO();
            vo.parameter.userId = Gamebase.GetUserID();

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.TEMPORARY_WITHDRAWAL,
                parameters = vo.parameter
            };

            return requestVO;
        }

        public static WebSocketRequest.RequestVO GetCancelTemporaryWithdrawalMessage()
        {
            var vo = new AuthRequest.CancelTemporaryWithdrawalVO();
            vo.parameter.userId = Gamebase.GetUserID();

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.CANCEL_TEMPORARY_WITHDRAWAL,
                parameters = vo.parameter
            };

            return requestVO;
        }

        public static WebSocketRequest.RequestVO GetIssueShortTermTicketMessage(string purpose, int expiresIn)
        {
            AuthRequest.IssueShortTermTicketVO vo = new AuthRequest.IssueShortTermTicketVO();

            vo.parameter.userId = Gamebase.GetUserID();
            vo.parameter.purpose = purpose;
            vo.parameter.expiresIn = expiresIn;

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.ISSUE_SHORT_TERM_TICKET,
                parameters = vo.parameter
            };

            return requestVO;
        }
    }
}
#endif