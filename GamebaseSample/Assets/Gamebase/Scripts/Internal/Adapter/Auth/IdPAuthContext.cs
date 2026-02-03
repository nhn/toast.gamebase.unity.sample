using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Auth
{
    public class IdPAuthContext
    {
        public string idPCode;
        public string accessToken = null;
        public string authorizationCode = null;
        public string session = null;
        public string codeVerifier = null;

        public string subCode = null;
        public string redirectUri = null;
        
        public IdPAuthContext(string idPCode)
        {
            this.idPCode = idPCode;
            if (idPCode.Equals(GamebaseAuthProvider.GUEST))
            {
                accessToken = string.Format("GAMEBASE{0}", GamebaseSystemInfo.UUID);
            }
        }
        
        public IdPAuthContext(Dictionary<string, object> credentialInfo)
        {
            idPCode = GetValueInDict<string>(GamebaseAuthProviderCredential.PROVIDER_NAME, credentialInfo);
            accessToken = GetValueInDict<string>(GamebaseAuthProviderCredential.ACCESS_TOKEN, credentialInfo);
            authorizationCode = GetValueInDict<string>(GamebaseAuthProviderCredential.AUTHORIZATION_CODE, credentialInfo);
            codeVerifier = GetValueInDict<string>(GamebaseAuthProviderCredential.CODE_VERIFIER, credentialInfo);
            
            subCode = GetValueInDict<string>(GamebaseAuthProviderCredential.SUB_CODE, credentialInfo);
            redirectUri = GetValueInDict<string>(GamebaseAuthProviderCredential.REDIRECT_URI, credentialInfo);
        }
        
        private static T GetValueInDict<T>(string key, Dictionary<string, object> dict)
        {
            if (dict.TryGetValue(key, out object value))
            {
                if (value is T castingValue)
                {
                    return castingValue;
                }
            }
            
            return default(T);
        }
    }
}