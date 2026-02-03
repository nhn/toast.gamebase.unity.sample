#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System;
using System.Collections.Generic;
using Toast.Gamebase.Internal.Auth;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class AuthMessage
    {
        private const string SUB_CODE_SIGN_IN_WITH_APPLE_JS = "sign_in_with_apple_js";
        private const string SUB_CODE_WEB = "web";
        private const string AUTHORIZATION_PROTOCOL_X_OAUTH2 = "oauth2";

        public static AuthRequest.LoginVO.Payload GetIDPLoginPayload(IdPAuthContext idPAuthContext)
        {
            var launchingInfoVO = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(PlatformKey.Launching.LAUNCHING_INFO);
            var idpDic = launchingInfoVO.launching.app.idP;
            
            var payload = new AuthRequest.LoginVO.Payload();
      
            payload.idPInfo.idPCode = idPAuthContext.idPCode;
            payload.idPInfo.accessToken = idPAuthContext.accessToken;
            payload.idPInfo.authorizationCode = idPAuthContext.authorizationCode;
            payload.idPInfo.codeVerifier = idPAuthContext.codeVerifier;
            payload.idPInfo.session = idPAuthContext.session;
            payload.idPInfo.redirectUri = idPAuthContext.redirectUri;
            
            if (idpDic.ContainsKey(idPAuthContext.idPCode))
            {
                payload.idPInfo.clientId = idpDic[idPAuthContext.idPCode].clientId;
            }
            
            switch (payload.idPInfo.idPCode)
            {
                case GamebaseAuthProvider.APPLEID:
                {
                    payload.idPInfo.subCode = SUB_CODE_SIGN_IN_WITH_APPLE_JS;
                    break;
                }
                case GamebaseAuthProvider.TWITTER:
                {
                    payload.idPInfo.authorizationProtocol = AUTHORIZATION_PROTOCOL_X_OAUTH2;
                    break;
                }
                case GamebaseAuthProvider.HANGAME:
                {
                    payload.idPInfo.subCode = SUB_CODE_WEB;
                    break;
                }
                case GamebaseAuthProvider.LINE:
                {
                    payload.idPInfo.subCode = idPAuthContext.subCode;
                    var clientId = LaunchingInfoHelper.GetClientIdForLineRegion(idPAuthContext.subCode);
                    payload.idPInfo.clientId = clientId;
                    payload.idPInfo.extraParams = new Dictionary<string, string>() { { GamebaseAuthProviderCredential.CLIENT_ID, clientId } };

                    break;
                }
            }

            payload.member.clientVersion = GamebaseUnitySDK.AppVersion;
            payload.member.deviceCountryCode = GamebaseSystemInfo.CountryCode;
            payload.member.deviceKey = GamebaseSystemInfo.DeviceKey;
            payload.member.deviceModel = GamebaseSystemInfo.DeviceModel;
            payload.member.osVersion = GamebaseSystemInfo.OsVersion;
            payload.member.deviceLanguage = GamebaseSystemInfo.DeviceLanguageCode;
            payload.member.displayLanguage = GamebaseUnitySDK.DisplayLanguageCode;
            payload.member.network = Application.internetReachability.ToString();
            payload.member.osCode = GamebaseSystemInfo.Platform;
            payload.member.sdkVersion = GamebaseUnitySDK.SDKVersion;
            payload.member.storeCode = launchingInfoVO.launching.app.storeCode;
            payload.member.telecom = string.Empty;
            payload.member.usimCountryCode = "ZZ";
            payload.member.uuid = GamebaseSystemInfo.UUID;

            return payload;
        }
        
        public static WebSocketRequest.RequestVO GetIDPLoginMessage(IdPAuthContext idPAuthContext, bool isLongTermGamebaseAccessToken = false)
        {
            var vo = new AuthRequest.LoginVO();
            vo.parameter.appId = GamebaseUnitySDK.AppID;
            vo.payload = GetIDPLoginPayload(idPAuthContext);

            if (isLongTermGamebaseAccessToken)
            {
                vo.payload.idPInfo.extraParams.Add("useIntrospection", false.ToString().ToLower());
            }
            
            var requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.IDP_LOGIN,
                parameters = vo.parameter,
                payload = JsonMapper.ToJson(vo.payload)
            };

            return requestVO;
        }
        
        public static WebSocketRequest.RequestVO GetIDPLoginMessage(Dictionary<string, object> credentialInfo)
        {
            return GetIDPLoginMessage(new IdPAuthContext(credentialInfo));
        }

        public static WebSocketRequest.RequestVO GetTokenLoginMessage(string providerName, string accessToken, string subCode = null, Dictionary<string, string> extraParams = null)
        {
            var launchingInfoVO = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(PlatformKey.Launching.LAUNCHING_INFO);

            var vo = new AuthRequest.LoginVO();
            vo.parameter.appId = GamebaseUnitySDK.AppID;

            vo.payload.tokenInfo.idPCode = providerName;
            vo.payload.tokenInfo.accessToken = accessToken;
            vo.payload.tokenInfo.subCode = subCode;
            vo.payload.tokenInfo.extraParams = extraParams;

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
                apiId = Lighthouse.API.Gateway.ID.TOKEN_LOGIN,
                parameters = vo.parameter,
                payload = JsonMapper.ToJson(vo.payload)
            };

            return requestVO;
        }
        
        public static WebSocketRequest.RequestVO GetLogoutMessage()
        {
            var vo = new AuthRequest.LogoutVO();
            vo.parameter.appId = GamebaseUnitySDK.AppID;
            var authToken = DataContainer.GetData<GamebaseResponse.Auth.AuthToken>(PlatformKey.Auth.LOGIN_INFO);
            if (authToken != null)
            {
                vo.parameter.userId = authToken.member.userId;
                vo.parameter.accessToken = authToken.token.accessToken;    
            }

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.LOGOUT,
                parameters = vo.parameter
            };

            return requestVO;
        }
        
        public static WebSocketRequest.RequestVO GetRemoveMappingMessage(string providerName)
        {
            var vo = new AuthRequest.RemoveMappingVO();
            vo.parameter.idPCode = providerName;
            var authToken = DataContainer.GetData<GamebaseResponse.Auth.AuthToken>(PlatformKey.Auth.LOGIN_INFO);
            if (authToken != null)
            {
                vo.parameter.userId = authToken.member.userId;
                vo.parameter.accessToken = authToken.token.accessToken;    
            }

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.REMOVE_MAPPING,
                parameters = vo.parameter
            };

            return requestVO;
        }

        public static WebSocketRequest.RequestVO GetAddMappingMessage(string loginPayload)
        {
            var vo = new AuthRequest.AddMappingVO();
            var authToken = DataContainer.GetData<GamebaseResponse.Auth.AuthToken>(PlatformKey.Auth.LOGIN_INFO);
            if (authToken != null)
            {
                vo.parameter.userId = authToken.member.userId;    
            }
            vo.parameter.forcing = false;

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.ADD_MAPPING,
                parameters = vo.parameter,
                payload = loginPayload
            };

            Debug.Log(requestVO.payload);
            
            return requestVO;
        }

        public static WebSocketRequest.RequestVO GetAddMappingForciblyMessage(
            GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket)
        {
            var vo = new AuthRequest.AddMappingForciblyVO();
            var authToken = DataContainer.GetData<GamebaseResponse.Auth.AuthToken>(PlatformKey.Auth.LOGIN_INFO);
            if (authToken != null)
            {
                vo.parameter.userId = authToken.member.userId;    
            }
            vo.parameter.forcingMappingKey = forcingMappingTicket.forcingMappingKey;
            vo.parameter.mappedUserId = forcingMappingTicket.mappedUserId;
            
            AuthRequest.AddMappingForciblyVO.Payload payload = new AuthRequest.AddMappingForciblyVO.Payload();
            payload.tokenInfo.accessToken = forcingMappingTicket.accessToken;
            payload.tokenInfo.idPCode = forcingMappingTicket.idPCode;
           
            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.ADD_MAPPING_FORCIBLY,
                parameters = vo.parameter,
                payload = JsonMapper.ToJson(payload)
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

        public static WebSocketRequest.RequestVO GetIssueShortTermTicketMessage(string userID, string purpose, int expiresIn)
        {
            ShortTermTicketRequest.IssueShortTermTicketVO vo = new ShortTermTicketRequest.IssueShortTermTicketVO();
            vo.parameter.userId = userID;
            vo.parameter.purpose = purpose;
            vo.parameter.expiresIn = expiresIn;
            
            string apiId = Lighthouse.API.Gateway.ID.ISSUE_SHORT_TERM_TICKET; 
            if (purpose.Equals(ShortTermTicketConst.PURPOSE_OPEN_CONTACT_FOR_BANNED_USER))
            {
                apiId = Lighthouse.API.Gateway.ID.ISSUE_SHORT_TERM_TICKET_WITHOUT_LOGIN;
            }

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = apiId,
                parameters = vo.parameter
            };

            return requestVO;
        }
    }
}
#endif