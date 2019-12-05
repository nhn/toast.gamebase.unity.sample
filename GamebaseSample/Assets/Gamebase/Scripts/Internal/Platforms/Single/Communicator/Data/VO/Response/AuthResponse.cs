#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class AuthResponse
    {
        public class LoginInfo : BaseVO
        {
            public class Token
            {
                public string accessToken;
                public string accessTokenSecret;
                public string sourceIdPCode;
            }

            public class Ban : BaseVO
            {
                public string userId;
                public string banType;
                public long beginDate;
                public long endDate;
                public string message;
            }

            public Token token;
            public CommonResponse.Header header;
            public CommonResponse.Member member;
            public Ban ban;
        }

        public class LogoutInfo : BaseVO
        {
            public CommonResponse.Header header;
        }

        public class WithdrawInfo : BaseVO
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