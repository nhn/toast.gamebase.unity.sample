#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
using System;
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile
{
    public class NativeGamebaseAuth : IGamebaseAuth
    {
        protected class GamebaseAuth
        {
            public const string AUTH_API_LOGIN                                  = "gamebase://login";
            public const string AUTH_API_LOGIN_ADDITIONAL_INFO                  = "gamebase://loginWithAdditionalInfo";
            public const string AUTH_API_LOGIN_CREDENTIAL_INFO                  = "gamebase://loginWithCredentialInfo";
            public const string AUTH_API_LOGIN_FOR_LAST_LOGGED_IN_PROVIDER      = "gamebase://loginForLastLoggedInProvider";
            public const string AUTH_API_CHANGE_LOGIN                           = "gamebase://changeLoginWithForcingMappingTicket";
            public const string AUTH_API_LOGOUT                                 = "gamebase://logout";
            public const string AUTH_API_ADD_MAPPING                            = "gamebase://addMapping";
            public const string AUTH_API_ADD_MAPPING_CREDENTIAL_INFO            = "gamebase://addMappingWithCredentialInfo";
            public const string AUTH_API_ADD_MAPPING_ADDITIONAL_INFO            = "gamebase://addMappingWithAdditionalInfo";
            public const string AUTH_API_ADD_MAPPING_FORCIBLY                   = "gamebase://addMappingForcibly";
            public const string AUTH_API_ADD_MAPPING_FORCIBLY_CREDENTIAL_INFO   = "gamebase://addMappingForciblyWithCredentialInfo";
            public const string AUTH_API_ADD_MAPPING_FORCIBLY_ADDITIONAL_INFO   = "gamebase://addMappingForciblyWithAdditionalInfo";
            public const string AUTH_API_ADD_MAPPING_FORCIBLY_MAPPING_TICKET    = "gamebase://addMappingForciblyWithForcingMappingTicket";
            public const string AUTH_API_REMOVE_MAPPING                         = "gamebase://removeMapping";
            public const string AUTH_API_WITHDRAW_ACCOUT                        = "gamebase://withdraw";
            public const string AUTH_API_WITHDRAW_IMMEDIATELY_ACCOUT            = "gamebase://withdrawImmediately";
            public const string AUTH_API_REQUEST_TEMPORARY_WITHDRAWAL_ACCOUT    = "gamebase://requestTemporaryWithdrawal";
            public const string AUTH_API_CANCEL_TEMPORARY_WITHDRAWAL_ACCOUT     = "gamebase://cancelTemporaryWithdrawal";
            public const string AUTH_API_ISSUE_TRANSFER_ACCOUNT                 = "gamebase://issueTransferAccount";
            public const string AUTH_API_QUERY_TRANSFER_ACCOUNT                 = "gamebase://queryTransferAccount";
            public const string AUTH_API_RENEW_TRANSFER_ACCOUNT                 = "gamebase://renewTransferAccount";
            public const string AUTH_API_TRANSFER_ACCOUNT_WITH_IDP_LOGIN        = "gamebase://transferAccountWithIdPLogin";
            public const string AUTH_API_GET_AUTH_MAPPING_LIST                  = "gamebase://getAuthMappingList";
            public const string AUTH_API_GET_AUTH_PROVIDER_USERID               = "gamebase://getAuthProviderUserID";
            public const string AUTH_API_GET_AUTH_PROVIDER_ACCESSTOKEN          = "gamebase://getAuthProviderAccessToken";
            public const string AUTH_API_GET_AUTH_PROVIDER_PROFILE              = "gamebase://getAuthProviderProfile";
            public const string AUTH_API_GET_BAN_INFO                           = "gamebase://getBanInfo";
        }

        protected INativeMessageSender  messageSender   = null;
        protected string                CLASS_NAME      = string.Empty;
        

        public NativeGamebaseAuth()
        {
            Init();
        }

        virtual protected void Init()
        {
            messageSender.Initialize(CLASS_NAME);

            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_LOGIN,                                DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_LOGIN_ADDITIONAL_INFO,                DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_LOGIN_CREDENTIAL_INFO,                DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>, OnLogin);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_LOGIN_FOR_LAST_LOGGED_IN_PROVIDER,    DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_CHANGE_LOGIN,                         DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_ADD_MAPPING,                          DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_ADD_MAPPING_CREDENTIAL_INFO,          DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>, OnAddMapping);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_ADD_MAPPING_FORCIBLY,                 DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_ADD_MAPPING_FORCIBLY_CREDENTIAL_INFO, DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_ADD_MAPPING_FORCIBLY_ADDITIONAL_INFO, DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_ADD_MAPPING_FORCIBLY_MAPPING_TICKET,  DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_ADD_MAPPING_ADDITIONAL_INFO,          DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>);            
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_ISSUE_TRANSFER_ACCOUNT,               DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.TransferAccountInfo>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_QUERY_TRANSFER_ACCOUNT,               DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.TransferAccountInfo>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_RENEW_TRANSFER_ACCOUNT,               DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.TransferAccountInfo>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_TRANSFER_ACCOUNT_WITH_IDP_LOGIN,      DelegateManager.SendGamebaseDelegateOnce<GamebaseResponse.Auth.AuthToken>);            
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_REMOVE_MAPPING,                       DelegateManager.SendErrorDelegateOnce, OnRemoveMapping);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_LOGOUT,                               DelegateManager.SendErrorDelegateOnce, OnLogout);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_WITHDRAW_ACCOUT,                     DelegateManager.SendErrorDelegateOnce, OnWithdraw);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_WITHDRAW_IMMEDIATELY_ACCOUT,         DelegateManager.SendErrorDelegateOnce, OnWithdraw);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_REQUEST_TEMPORARY_WITHDRAWAL_ACCOUT,  DelegateManager.SendGamebaseDelegate<GamebaseResponse.TemporaryWithdrawalInfo>);
            DelegateManager.AddDelegate(GamebaseAuth.AUTH_API_CANCEL_TEMPORARY_WITHDRAWAL_ACCOUT,   DelegateManager.SendErrorDelegateOnce);
        }

        virtual public void Login(string providerName, int handle)
        {
            if (AuthAdapterManager.Instance.IsSupportedIDP(providerName) == true)
            {
                InvokeCredentialInfoMethod(providerName, handle, "Login");
            }
            else
            {
                CallNativeLogin(providerName, handle);
            }
        }

        virtual public void Login(string providerName, Dictionary<string, object> additionalInfo, int handle)
        {
            var vo              = new NativeRequest.Auth.LoginWithAdditionalInfo();
            vo.providerName     = providerName;
            vo.additionalInfo   = additionalInfo;

            string jsonData     = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_LOGIN_ADDITIONAL_INFO,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(vo)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void Login(Dictionary<string, object> credentialInfo, int handle)
        {

            string jsonData     = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_LOGIN_CREDENTIAL_INFO,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(credentialInfo)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void LoginForLastLoggedInProvider(int handle)
        {
            string jsonData     = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_LOGIN_FOR_LAST_LOGGED_IN_PROVIDER,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void ChangeLogin(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_CHANGE_LOGIN,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(forcingMappingTicket)
                ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void AddMapping(string providerName, int handle)
        {
            if (AuthAdapterManager.Instance.IsSupportedIDP(providerName) == true)
            {
                InvokeCredentialInfoMethod(providerName, handle, "AddMapping");
            }
            else
            {
                CallNativeMapping(providerName, handle);
            }
        }

        virtual public void AddMapping(Dictionary<string, object> credentialInfo, int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_ADD_MAPPING_CREDENTIAL_INFO,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(credentialInfo)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void AddMapping(string providerName, Dictionary<string, object> additionalInfo, int handle)
        {
            var vo              = new NativeRequest.Auth.AddMappingWithAdditionalInfo();
            vo.providerName     = providerName;
            vo.additionalInfo   = additionalInfo;

            string jsonData     = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_ADD_MAPPING_ADDITIONAL_INFO,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(vo)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void AddMappingForcibly(string providerName, string forcingMappingKey, int handle)
        {
            if (AuthAdapterManager.Instance.IsSupportedIDP(providerName) == true)
            {
                InvokeCredentialInfoMethod(providerName, handle, "AddMappingForcibly", forcingMappingKey);
            }
            else
            {
                CallNativeMappingForcibly(providerName, forcingMappingKey, handle);
            }            
        }

        virtual public void AddMappingForcibly(Dictionary<string, object> credentialInfo, string forcingMappingKey, int handle)
        {
            var vo                  = new NativeRequest.Auth.AddMappingForciblyWithCredentialInfo();            
            vo.forcingMappingKey    = forcingMappingKey;
            vo.credentialInfo       = credentialInfo;

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_ADD_MAPPING_FORCIBLY_CREDENTIAL_INFO,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(vo)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void AddMappingForcibly(string providerName, string forcingMappingKey, Dictionary<string, object> additionalInfo, int handle)
        {            
            var vo                  = new NativeRequest.Auth.AddMappingForciblyWithAdditionalInfo();
            vo.providerName         = providerName;
            vo.forcingMappingKey    = forcingMappingKey;
            vo.additionalInfo       = additionalInfo;

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_ADD_MAPPING_FORCIBLY_ADDITIONAL_INFO,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(vo)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void AddMappingForcibly(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_ADD_MAPPING_FORCIBLY_MAPPING_TICKET,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(forcingMappingTicket)
                ));
            messageSender.GetAsync(jsonData);   
        }

        virtual public void RemoveMapping(string providerName, int handle)
        {
            var vo              = new NativeRequest.Auth.RemoveMapping();
            vo.providerName     = providerName;

            GamebaseExtraDataHandler.RegisterExtraData(handle, providerName);

            string jsonData     = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_REMOVE_MAPPING,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(vo)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void Logout(int handle)
        {
            string jsonData     = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_LOGOUT,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void Withdraw(int handle)
        {
            string jsonData     = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_WITHDRAW_ACCOUT,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void WithdrawImmediately(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_WITHDRAW_IMMEDIATELY_ACCOUT,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void RequestTemporaryWithdrawal(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_REQUEST_TEMPORARY_WITHDRAWAL_ACCOUT,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public void CancelTemporaryWithdrawal(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_CANCEL_TEMPORARY_WITHDRAWAL_ACCOUT,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void IssueTransferAccount(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_ISSUE_TRANSFER_ACCOUNT,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void QueryTransferAccount(int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_QUERY_TRANSFER_ACCOUNT,
                    handle: handle
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void RenewTransferAccount(GamebaseRequest.Auth.TransferAccountRenewConfiguration configuration, int handle)
        {
            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_RENEW_TRANSFER_ACCOUNT,
                    handle: handle,
                    jsonData: configuration.ToJsonString()
                    ));
            messageSender.GetAsync(jsonData);
        }

        public void TransferAccountWithIdPLogin(string accountId, string accountPassword, int handle)
        {
            var vo = new NativeRequest.Auth.TransferAccount();
            vo.accountId = accountId;
            vo.accountPassword = accountPassword;

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_TRANSFER_ACCOUNT_WITH_IDP_LOGIN,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(vo)
                    ));
            messageSender.GetAsync(jsonData);
        }

        virtual public List<string> GetAuthMappingList()
        {
            string jsonData = JsonMapper.ToJson(new UnityMessage(GamebaseAuth.AUTH_API_GET_AUTH_MAPPING_LIST));
            string jsonString = messageSender.GetSync(jsonData);

            if (string.IsNullOrEmpty(jsonString) == true)
            {
                return null;
            }

            return JsonMapper.ToObject<List<string>>(jsonString);
        }

        virtual public string GetAuthProviderUserID(string providerName)
        {
            if (AuthAdapterManager.Instance.IsUsableAdapter(providerName) == true)
            {
                return AuthAdapterManager.Instance.GetIDPData<string>(providerName, AuthAdapterManager.MethodName.GET_IDP_USER_ID);
            }
            else
            {
                string jsonData = JsonMapper.ToJson(new UnityMessage(GamebaseAuth.AUTH_API_GET_AUTH_PROVIDER_USERID, jsonData: providerName));
                return messageSender.GetSync(jsonData);
            }            
        }

        virtual public string GetAuthProviderAccessToken(string providerName)
        {
            if (AuthAdapterManager.Instance.IsUsableAdapter(providerName) == true)
            {
                return AuthAdapterManager.Instance.GetIDPData<string>(providerName, AuthAdapterManager.MethodName.GET_IDP_ACCESS_TOKEN);
            }
            else
            {
                string jsonData = JsonMapper.ToJson(new UnityMessage(GamebaseAuth.AUTH_API_GET_AUTH_PROVIDER_ACCESSTOKEN, jsonData: providerName));
                return messageSender.GetSync(jsonData);
            }
        }

        virtual public GamebaseResponse.Auth.AuthProviderProfile GetAuthProviderProfile(string providerName)
        {
            if (AuthAdapterManager.Instance.IsUsableAdapter(providerName) == true)
            {
                return AuthAdapterManager.Instance.GetIDPData<GamebaseResponse.Auth.AuthProviderProfile>(providerName, AuthAdapterManager.MethodName.GET_IDP_PROFILE);
            }
            else
            {
                string jsonData = JsonMapper.ToJson(new UnityMessage(GamebaseAuth.AUTH_API_GET_AUTH_PROVIDER_PROFILE, jsonData: providerName));
                string jsonString = messageSender.GetSync(jsonData);

                if (string.IsNullOrEmpty(jsonString) == true)
                {
                    return null;
                }

                return JsonMapper.ToObject<GamebaseResponse.Auth.AuthProviderProfile>(jsonString);
            }
        }

        virtual public GamebaseResponse.Auth.BanInfo GetBanInfo()
        {
            string jsonData     = JsonMapper.ToJson(new UnityMessage(GamebaseAuth.AUTH_API_GET_BAN_INFO));
            string jsonString   = messageSender.GetSync(jsonData);

            if (string.IsNullOrEmpty(jsonString) == true)
            {
                return null;
            }

            return JsonMapper.ToObject<GamebaseResponse.Auth.BanInfo>(jsonString);
        }

        #region Handlers for IDP logout
        virtual protected void OnLogin(NativeMessage message)
        {
            GamebaseError error = message.GetGamebaseError();

            string providerName = GamebaseExtraDataHandler.GetExtraData(message.handle);
            GamebaseExtraDataHandler.UnregisterExtraData(message.handle);

            if (Gamebase.IsSuccess(error) == false)
            {
                AuthAdapterManager.Instance.IDPLogout(providerName);
            }
        }

        virtual protected void OnAddMapping(NativeMessage message)
        {
            GamebaseError error = message.GetGamebaseError();

            string providerName = GamebaseExtraDataHandler.GetExtraData(message.handle);
            GamebaseExtraDataHandler.UnregisterExtraData(message.handle);

            if (Gamebase.IsSuccess(error) == false)
            {
                AuthAdapterManager.Instance.IDPLogout(providerName);
            }
        }

        virtual protected void OnRemoveMapping(NativeMessage message)
        {
            GamebaseError error = message.GetGamebaseError();

            string providerName = GamebaseExtraDataHandler.GetExtraData(message.handle);
            GamebaseExtraDataHandler.UnregisterExtraData(message.handle);

            if (Gamebase.IsSuccess(error) == true)
            {
                AuthAdapterManager.Instance.IDPLogout(providerName);
            }
        }

        virtual protected void OnWithdraw(NativeMessage message)
        {
            GamebaseError error = message.GetGamebaseError();
            if (Gamebase.IsSuccess(error) == true)
            {
                AuthAdapterManager.Instance.IDPLogoutAll();
            }
        }

        virtual protected void OnLogout(NativeMessage message)
        {
            GamebaseError error = message.GetGamebaseError();
            if (Gamebase.IsSuccess(error) == true)
            {
                AuthAdapterManager.Instance.IDPLogoutAll();
            }
        }
        #endregion

        virtual protected void InvokeCredentialInfoMethod(string providerName, int handle, string methodName, string forcingMappingKey = null)
        {
            bool hasAdapter = AuthAdapterManager.Instance.CreateIDPAdapter(providerName);

            if (hasAdapter == true)
            {
                AuthAdapterManager.Instance.GetIDPCredentialInfo(providerName, (credentialInfo, adapterError) =>
                {
                    if (Gamebase.IsSuccess(adapterError) == true)
                    {
                        GamebaseExtraDataHandler.RegisterExtraData(handle, providerName);

                        if (methodName.Equals("Login", StringComparison.Ordinal) == true)
                        {
                            Login(credentialInfo, handle);
                        }
                        else if (methodName.Equals("AddMapping", StringComparison.Ordinal) == true)
                        {
                            AddMapping(credentialInfo, handle);
                        }
                        else if (methodName.Equals("AddMappingForcibly", StringComparison.Ordinal) == true)
                        {
                            AddMappingForcibly(credentialInfo, forcingMappingKey, handle);
                        }
                    }
                    else
                    {
                        var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);

                        if (callback == null)
                        {
                            return;
                        }

                        AuthAdapterManager.Instance.IDPLogout(providerName);
                        callback(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, "AndroidGamebaseAuth", error: adapterError));
                    }
                });
            }
            else
            {
                GamebaseLog.Debug("Call native method", this);

                if (methodName.Equals("Login", StringComparison.Ordinal) == true)
                {
                    CallNativeLogin(providerName, handle);
                }
                else if (methodName.Equals("AddMapping", StringComparison.Ordinal) == true)
                {
                    CallNativeMapping(providerName, handle);
                }
                else if (methodName.Equals("AddMappingForcibly", StringComparison.Ordinal) == true)
                {
                    CallNativeMappingForcibly(providerName, forcingMappingKey, handle);
                }
            }
        }

        protected void CallNativeLogin(string providerName, int handle)
        {
            var vo          = new NativeRequest.Auth.Login();
            vo.providerName = providerName;

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_LOGIN,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(vo)
                    ));
            messageSender.GetAsync(jsonData);
        }

        protected void CallNativeMapping(string providerName, int handle)
        {
            var vo          = new NativeRequest.Auth.AddMapping();
            vo.providerName = providerName;

            string jsonData = JsonMapper.ToJson(
                new UnityMessage(
                    GamebaseAuth.AUTH_API_ADD_MAPPING,
                    handle: handle,
                    jsonData: JsonMapper.ToJson(vo)
                    ));
            messageSender.GetAsync(jsonData);
        }

        protected void CallNativeMappingForcibly(string providerName, string forcingMappingKey, int handle)
        {            
            var vo = new NativeRequest.Auth.AddMappingForcibly();
            vo.providerName = providerName;
            vo.forcingMappingKey = forcingMappingKey;

            string jsonData = JsonMapper.ToJson(
               new UnityMessage(
                   GamebaseAuth.AUTH_API_ADD_MAPPING_FORCIBLY,
                   handle: handle,
                   jsonData: JsonMapper.ToJson(vo)
                   ));
            messageSender.GetAsync(jsonData);
        }

        public void IssueShortTermTicket(int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<string>>(handle));
        }
    }
}
#endif