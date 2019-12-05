using System;
using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public class AuthAdapterManager
    {
        private static readonly AuthAdapterManager instance = new AuthAdapterManager();

        public static AuthAdapterManager Instance
        {
            get { return instance; }
        }

        public IAuthAdapter adapter;
        private Dictionary<string, IAuthAdapter> adapterDict = new Dictionary<string, IAuthAdapter>();

        public bool CreateIDPAdapter(string providerName)
        {
            string authAdapterName = string.Format("{0}adapter", providerName);
            adapter = AdapterFactory.CreateAdapter<IAuthAdapter>(authAdapterName);

            if (adapter == null)
            {
                GamebaseLog.Warn(GamebaseStrings.AUTH_ADAPTER_NOT_FOUND, this);
                return false;
            }

            AddAdapter(providerName, adapter);
            return true;
        }

        public void IDPLogin(Action<GamebaseError> callback)
        {
            if (adapter == null)
            {
                callback(new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, message: GamebaseStrings.AUTH_ADAPTER_NOT_FOUND_NEED_SETUP));
                return;
            }

            adapter.IDPLogin(callback);
        }

        public void IDPLogin(Dictionary<string, object> additionalInfo, Action<GamebaseError> callback)
        {
            if (adapter == null)
            {
                callback(new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, message: GamebaseStrings.AUTH_ADAPTER_NOT_FOUND_NEED_SETUP));
                return;
            }

            adapter.IDPLogin(additionalInfo, callback);
        }

        public void IDPLogout(string providerName)
        {
            if (IsSupportedIDP(providerName) == false)
            {
                return;
            }

            if (HasAdapter(providerName) == false)
            {
                GamebaseLog.Warn(GamebaseStrings.AUTH_ADAPTER_NOT_FOUND_NEED_SETUP, this);
                return;
            }

            GetAdapter(providerName).IDPLogout();
        }

        public void IDPLogoutAll()
        {
            foreach (KeyValuePair<string, IAuthAdapter> kvp in adapterDict)
            {
                kvp.Value.IDPLogout();
            }
        }

        public class MethodName
        {
            public const string GET_IDP_NAME = "GET_IDP_NAME";
            public const string GET_IDP_USER_ID = "GET_IDP_USER_ID";
            public const string GET_IDP_ACCESS_TOKEN = "GET_IDP_ACCESS_TOKEN";
            public const string GET_IDP_PROFILE = "GET_IDP_PROFILE";
        }

        public T GetIDPData<T>(string providerName, string methodName)
        {
            if (IsSupportedIDP(providerName) == false)
            {
                GamebaseErrorNotifier.FireNotSupportedAPI(this, string.Format("GetIDPData - {0}", methodName));
                return default(T);
            }

            if (HasAdapter(providerName) == false)
            {
                GamebaseLog.Warn(GamebaseStrings.AUTH_ADAPTER_NOT_FOUND_NEED_SETUP, this, string.Format("GetIDPData - {0}", methodName));
                return default(T);
            }

            switch (methodName)
            {
                case MethodName.GET_IDP_NAME:
                    return (T)Convert.ChangeType(GetIDPName(providerName), typeof(T));
                case MethodName.GET_IDP_USER_ID:
                    return (T)Convert.ChangeType(GetIDPUserID(providerName), typeof(T));
                case MethodName.GET_IDP_ACCESS_TOKEN:
                    return (T)Convert.ChangeType(GetIDPAccessToken(providerName), typeof(T));
                case MethodName.GET_IDP_PROFILE:
                    return (T)Convert.ChangeType(GetAuthProviderProfile(providerName), typeof(T));
                default:
                    return default(T);
            }
        }
        
        public void GetIDPCredentialInfo(string providerName, GamebaseCallback.GamebaseDelegate<Dictionary<string, object>> callback)
        {
            Dictionary<string, object> credentialInfo = null;
            IDPLogin((adapterError) =>
            {
                if (Gamebase.IsSuccess(adapterError))
                {
                    credentialInfo = new Dictionary<string, object>();
                    credentialInfo.Add(GamebaseAuthProviderCredential.PROVIDER_NAME, providerName);
                    credentialInfo.Add(GamebaseAuthProviderCredential.ACCESS_TOKEN, GetIDPAccessToken(providerName));
                }
                else
                {
                    GamebaseLog.Debug(string.Format("error:{0}", GamebaseJsonUtil.ToPrettyJsonString(adapterError)), this);
                }

                callback(credentialInfo, adapterError);
            });
        }

        #region Get method for IDP
        private string GetIDPName(string providerName)
        {
            return GetAdapter(providerName).GetIDPName();
        }

        private string GetIDPUserID(string providerName)
        {
            return GetAdapter(providerName).GetIDPUserID();
        }

        private string GetIDPAccessToken(string providerName)
        {
            return GetAdapter(providerName).GetIDPAccessToken();
        }

        private GamebaseResponse.Auth.AuthProviderProfile GetAuthProviderProfile(string providerName)
        {
            return GetAdapter(providerName).GetIDPProfile();
        }
        #endregion
        
        private void AddAdapter(string providerName, IAuthAdapter adapter)
        {
            if (adapterDict.ContainsKey(providerName))
            {
                adapterDict[providerName] = adapter;
                return;
            }

            adapterDict.Add(providerName, adapter);
        }

        private IAuthAdapter GetAdapter(string providerName)
        {
            return adapterDict[providerName];
        }

        private bool HasAdapter(string providerName)
        {
            return adapterDict.ContainsKey(providerName);
        }

        public bool IsSupportedIDP(string providerName)
        {
#if UNITY_EDITOR
            return providerName == GamebaseAuthProvider.GUEST || 
                   providerName == GamebaseAuthProvider.FACEBOOK ||
                   providerName == "ongame";
#else
    #if UNITY_ANDROID
            return providerName == GamebaseAuthProvider.FACEBOOK;
    #elif UNITY_IOS
            return providerName == GamebaseAuthProvider.FACEBOOK;
    #elif UNITY_WEBGL
            return providerName == GamebaseAuthProvider.GUEST || 
                   providerName == GamebaseAuthProvider.FACEBOOK || 
                   providerName =="ongame";
    #elif UNITY_STANDALONE
            return providerName == GamebaseAuthProvider.GUEST ||
                   providerName == "ongame";
    #endif
#endif
        }

        public bool IsUsableAdapter(string providerName)
        {
            return IsSupportedIDP(providerName) && HasAdapter(providerName);
        }
    }
}