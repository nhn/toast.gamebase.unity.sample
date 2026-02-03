#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single.Communicator
{
    using System.Collections.Generic;
    public class AuthResponse
    {
        public class BrowserLoginTicket : BaseVO
        {
            public CommonResponse.Header header;
            public string sessionTicketId;
        }

        public class BrowserLoginResult : BaseVO
        {
            public CommonResponse.Header header;
            public string session;
        }

        public class CancelBrowserLoginTicket : BaseVO
        {
            public CommonResponse.Header header;
        }

        public class LoginInfo : BaseVO
        {
            public class Token
            {
                public string sourceIdPCode;
                public string accessToken;
                public string accessTokenSecret;
                public string subCode;
                public Dictionary<string, string> extraParams;
                public string thirdIdPCode;
                public int expiresIn;
            }

            public class ErrorExtras
            {
                public class Ban : BaseVO
                {
                    public string userId;
                    public string banType;
                    public long beginDate;
                    public long endDate;
                    public string message;
                }

                public Ban ban;
            }

            public Token token;
            public CommonResponse.Header header;
            public CommonResponse.Member member;
            public ErrorExtras errorExtras;
        }
        
        public class MappingInfo : BaseVO
        {
            public class ErrorExtras
            {
                public GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket;
            }
            
            public CommonResponse.Header header;
            public ErrorExtras errorExtras;
        }
        
        

        public class LogoutInfo : BaseVO
        {
            public CommonResponse.Header header;
        }

        public class WithdrawInfo : BaseVO
        {
            public CommonResponse.Header header;
        }

        public class TemporaryWithdrawalInfo : BaseVO
        {
            public CommonResponse.Header header;
            public CommonResponse.Member member;
        }

        public class CancelTemporaryWithdrawalInfo : BaseVO
        {
            public CommonResponse.Header header;
        }

        public class IssueShortTermTicketInfo : BaseVO
        {
            public CommonResponse.Header header;
            public string ticket;
        }
    }
}
#endif