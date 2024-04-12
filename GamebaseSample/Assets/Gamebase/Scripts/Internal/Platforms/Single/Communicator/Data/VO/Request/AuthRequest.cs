#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class AuthRequest
    {
        public class LoginVO : BaseVO
        {
            public class Parameter
            {
                public string appId;
            }

            public class Payload
            {
                public class IDPInfo
                {
                    public string accessToken;
                    public string accessTokenSecret;
                    public string authorizationCode;
                    /// <summary>
                    /// Only Unity Standalone
                    /// Google
                    /// </summary>
                    public string redirectUri;
                    public string clientId;
                    public string clientSecret;
                    public Dictionary<string, string> extraParams;
                    public string idPCode;
                    /// <summary>
                    /// 웹로그인시 회원에서 내려주는 로그인 session 값.
                    /// Gamebase Server로 로그인할때 사용된다.
                    /// </summary>
                    public string session;
                    public string subCode;
                }

                public class Member
                {
                    public string clientVersion;
                    public string deviceCountryCode;
                    public string deviceKey;
                    public string deviceModel;
                    public string deviceLanguage;
                    public string displayLanguage;
                    public string network;
                    public string osCode;
                    public string osVersion;
                    public string sdkVersion;
                    public string storeCode;
                    public string telecom;
                    public string usimCountryCode;
                    public string uuid;
                }

                public class TokenInfo
                {
                    public string idPCode;
                    public string accessToken;
                }

                public IDPInfo idPInfo;
                public Member member;
                public TokenInfo tokenInfo;

                public Payload()
                {
                    idPInfo = new IDPInfo();
                    member = new Member();
                    tokenInfo = new TokenInfo();
                }
            }

            public Parameter parameter;
            public Payload payload;

            public LoginVO()
            {
                parameter = new Parameter();
                payload = new Payload();
            }
        }

        public class LogoutVO : BaseVO
        {
            public class Parameter
            {
                public string appId;
                public string userId;
                public string accessToken;
            }

            public Parameter parameter;

            public LogoutVO()
            {
                parameter = new Parameter();
            }
        }

        public class WithdrawVO : BaseVO
        {
            public class Parameter
            {                
                public string userId;
            }

            public Parameter parameter;

            public WithdrawVO()
            {
                parameter = new Parameter();
            }
        }

        public class TemporaryWithdrawalVO : BaseVO
        {
            public class Parameter
            {
                public string userId;
            }

            public Parameter parameter;

            public TemporaryWithdrawalVO()
            {
                parameter = new Parameter();
            }
        }

        public class CancelTemporaryWithdrawalVO : BaseVO
        {
            public class Parameter
            {
                public string userId;
            }

            public Parameter parameter;

            public CancelTemporaryWithdrawalVO()
            {
                parameter = new Parameter();
            }
        }

        

        public class IssueShortTermTicketVO : BaseVO
        {
            public class Parameter
            {
                public string userId;
                public string purpose;
                public int expiresIn;
            }

            public Parameter parameter;

            public IssueShortTermTicketVO()
            {
                parameter = new Parameter();
            }
        }
    }
}
#endif