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