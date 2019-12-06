#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class AuthMessage
    {
        public static WebSocketRequest.RequestVO GetIDPLoginMessage(
            string providerName, 
            string accessToken = null,
            string authorizationCode = null)
        {
            var launchingInfoVO = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
            var idpDic = launchingInfoVO.launching.app.idP;

            var vo = new AuthRequest.LoginVO();
            vo.parameter.appId = GamebaseUnitySDK.AppID;
            
            if (providerName == GamebaseAuthProvider.GUEST)
                vo.payload.idPInfo.accessToken = string.Format("GAMEBASE{0}", GamebaseUnitySDK.UUID);
            else
                vo.payload.idPInfo.accessToken = accessToken;

            vo.payload.idPInfo.authorizationCode = authorizationCode;
            vo.payload.idPInfo.clientId = idpDic[providerName].clientId;
            vo.payload.idPInfo.clientSecret = idpDic[providerName].clientSecret;
            vo.payload.idPInfo.idPCode = providerName;
            vo.payload.member.clientVersion = GamebaseUnitySDK.AppVersion;
            vo.payload.member.deviceCountryCode = GamebaseUnitySDK.CountryCode;
            vo.payload.member.deviceKey = GamebaseUnitySDK.DeviceKey;
            vo.payload.member.deviceModel = GamebaseUnitySDK.DeviceModel;
            vo.payload.member.osVersion = GamebaseUnitySDK.OsVersion;
            vo.payload.member.deviceLanguage = GamebaseUnitySDK.DeviceLanguageCode;
            vo.payload.member.displayLanguage = GamebaseUnitySDK.DisplayLanguageCode;
            vo.payload.member.network = Application.internetReachability.ToString();
            vo.payload.member.osCode = GamebaseUnitySDK.Platform;
            vo.payload.member.sdkVersion = GamebaseUnitySDK.SDKVersion;
            vo.payload.member.storeCode = launchingInfoVO.launching.app.storeCode;
            vo.payload.member.telecom = string.Empty;
            vo.payload.member.usimCountryCode = "ZZ";
            vo.payload.member.uuid = GamebaseUnitySDK.UUID;

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(
                Lighthouse.API.Gateway.PRODUCT_ID,
                Lighthouse.API.VERSION,
                GamebaseUnitySDK.AppID);

            requestVO.apiId = Lighthouse.API.Gateway.ID.IDP_LOGIN;
            requestVO.parameters = vo.parameter;
            requestVO.payload = JsonMapper.ToJson(vo.payload);
            
            return requestVO;
        }
        
        public static WebSocketRequest.RequestVO GetLogoutMessage()
        {
            var loginInfoVO = DataContainer.GetData<AuthResponse.LoginInfo>(VOKey.Auth.LOGIN_INFO);
            var vo = new AuthRequest.LogoutVO();
            vo.parameter.appId = GamebaseUnitySDK.AppID;
            vo.parameter.userId = loginInfoVO.member.userId;
            vo.parameter.accessToken = loginInfoVO.token.accessToken;

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(
                Lighthouse.API.Gateway.PRODUCT_ID,
                Lighthouse.API.VERSION,
                GamebaseUnitySDK.AppID);

            requestVO.apiId = Lighthouse.API.Gateway.ID.LOGOUT;
            requestVO.parameters = vo.parameter;

            return requestVO;
        }

        public static WebSocketRequest.RequestVO GetWithdrawMessage()
        {
            var vo = new AuthRequest.WithdrawVO();
            vo.parameter.appId = GamebaseUnitySDK.AppID;
            vo.parameter.userId = Gamebase.GetUserID();

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(
                Lighthouse.API.Gateway.PRODUCT_ID,
                Lighthouse.API.VERSION,
                GamebaseUnitySDK.AppID);

            requestVO.apiId = Lighthouse.API.Gateway.ID.WITHDRAW;
            requestVO.parameters = vo.parameter;

            return requestVO;
        }

        public static WebSocketRequest.RequestVO GetIssueShortTermTicketMessage(string purpose, int expiresIn)
        {
            AuthRequest.IssueShortTermTicketVO vo = new AuthRequest.IssueShortTermTicketVO();

            vo.parameter.userId = Gamebase.GetUserID();
            vo.parameter.purpose = purpose;
            vo.parameter.expiresIn = expiresIn;

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(
               Lighthouse.API.Gateway.PRODUCT_ID,
               Lighthouse.API.VERSION,
               GamebaseUnitySDK.AppID);

            requestVO.apiId = Lighthouse.API.Gateway.ID.ISSUE_SHORT_TERM_TICKET;
            requestVO.parameters = vo.parameter;

            return requestVO;
        }
    }
}
#endif