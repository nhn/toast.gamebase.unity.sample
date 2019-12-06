#if !UNITY_EDITOR && UNITY_ANDROID
using Toast.Gamebase.Internal.Mobile.Android;
#elif !UNITY_EDITOR && UNITY_IOS
using Toast.Gamebase.Internal.Mobile.IOS;
#elif !UNITY_EDITOR && UNITY_WEBGL
using Toast.Gamebase.Internal.Single.WebGL;
#else
using Toast.Gamebase.Internal.Single.Standalone;
#endif
using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public sealed class GamebaseAuthImplementation
    {        
        private const string KEY_CREDENTIAL_PROVIDER_NAME = "provider_name";

        private static readonly GamebaseAuthImplementation instance = new GamebaseAuthImplementation();
        
        public static GamebaseAuthImplementation Instance
        {
            get { return instance; }
        }

        private IGamebaseAuth auth;

        private GamebaseAuthImplementation()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            auth = new AndroidGamebaseAuth();
#elif !UNITY_EDITOR && UNITY_IOS
            auth = new IOSGamebaseAuth();
#elif !UNITY_EDITOR && UNITY_WEBGL
            auth = new WebGLGamebaseAuth();
#else
            auth = new StandaloneGamebaseAuth();
#endif
        }

        public void Login(string providerName, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> loginCallback = (authToken, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    SetUserIdOfIndicatorReport();
                }
                callback(authToken, error);
            };

            int handle = GamebaseCallbackHandler.RegisterCallback(loginCallback);
            auth.Login(providerName, handle);
        }

        public void Login(Dictionary<string, object> credentialInfo, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> loginCallback = (authToken, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    SetUserIdOfIndicatorReport();
                }
                callback(authToken, error);
            };

            int handle = GamebaseCallbackHandler.RegisterCallback(loginCallback);
            auth.Login(credentialInfo, handle);
        }

        public void Login(string providerName, Dictionary<string, object> additionalInfo, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> loginCallback = (authToken, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    SetUserIdOfIndicatorReport();
                }
                callback(authToken, error);
            };

            int handle = GamebaseCallbackHandler.RegisterCallback(loginCallback);
            auth.Login(providerName, additionalInfo, handle);
        }

        public void LoginForLastLoggedInProvider(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> loginCallback = (authToken, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    SetUserIdOfIndicatorReport();
                }
                callback(authToken, error);
            };

            int handle = GamebaseCallbackHandler.RegisterCallback(loginCallback);
            auth.LoginForLastLoggedInProvider(handle);
        }

        public void AddMapping(string providerName, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.AddMapping(providerName, handle);
        }

        public void AddMapping(string providerName, Dictionary<string, object> additionalInfo, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.AddMapping(providerName, additionalInfo, handle);
        }

        public void AddMapping(Dictionary<string, object> credentialInfo, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.AddMapping(credentialInfo, handle);
        }

        public void AddMappingForcibly(string providerName, string forcingMappingKey, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.AddMappingForcibly(providerName, forcingMappingKey, handle);
        }

        public void AddMappingForcibly(string providerName, string forcingMappingKey, Dictionary<string, object> additionalInfo, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.AddMappingForcibly(providerName, forcingMappingKey, additionalInfo, handle);
        }

        public void AddMappingForcibly(Dictionary<string, object> credentialInfo, string forcingMappingKey, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.AddMappingForcibly(credentialInfo, forcingMappingKey, handle);
        }

        public void RemoveMapping(string providerName, GamebaseCallback.ErrorDelegate callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.RemoveMapping(providerName, handle);
        }

        public void Logout(GamebaseCallback.ErrorDelegate callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.Logout(handle);
        }

        public void Withdraw(GamebaseCallback.ErrorDelegate callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.Withdraw(handle);
        }

        public void IssueTransferAccount(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.TransferAccountInfo> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.IssueTransferAccount(handle);
        }

        public void QueryTransferAccount(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.TransferAccountInfo> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.QueryTransferAccount(handle);
        }

        public void RenewTransferAccount(GamebaseRequest.Auth.TransferAccountRenewConfiguration configuration, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.TransferAccountInfo> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.RenewTransferAccount(configuration, handle);
        }

        public void TransferAccountWithIdPLogin(string accountId, string accountPassword, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.TransferAccountWithIdPLogin(accountId, accountPassword, handle);
        }

        public List<string> GetAuthMappingList()
        {
            return auth.GetAuthMappingList();
        }

        public string GetAuthProviderUserID(string providerName)
        {
            return auth.GetAuthProviderUserID(providerName);
        }

        public string GetAuthProviderAccessToken(string providerName)
        {
            return auth.GetAuthProviderAccessToken(providerName);
        }

        public GamebaseResponse.Auth.AuthProviderProfile GetAuthProviderProfile(string providerName)
        {
            return auth.GetAuthProviderProfile(providerName);
        }

        public GamebaseResponse.Auth.BanInfo GetBanInfo()
        {
            return auth.GetBanInfo();
        }

        public void IssueShortTermTicket( GamebaseCallback.GamebaseDelegate<string> callback)
        {
            int handle = GamebaseCallbackHandler.RegisterCallback(callback);
            auth.IssueShortTermTicket(handle);
        }       

        #region IndicatorReport

        private void SetUserIdOfIndicatorReport()
        {
            GamebaseLogReport.Instance.SetUserId(Gamebase.GetUserID());                
        }
#endregion
    }
}
 
 
 