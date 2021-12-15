using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public class ProtocolResponse
    {
        public CommonResponse.Header header;
    }

    public class CommonResponse
    {
        public class Header
        {
            public bool isSuccessful;
            public int resultCode;
            public string resultMessage;
            public TraceError traceError;
            public string transactionId;
            public ServerPush serverPush;

            public class ServerPush
            {
                public string type;
                public string version;
                public string stamp;
                public bool disconnect;
                public bool stopHeartbeat;
                public bool logout;
                public bool popup;
            }

            public class TraceError
            {
                public string apiId;
                public string apiVersion;
                public string appId;
                public string gatewayApiVersion;
                public string productId;
                public string throwPoint;
                public int throwPointErrorCode;
                public long trackingTime;
                public string uri;
                public int resultCode;
                public string resultMessage;
                public TraceError traceError;
            }
        }

        public class Member
        {
            public class AuthMappingInfo
            {
                public string userId;
                public string authSystem;
                public string idPCode;
                public string authKey;
                public long regDate;
            }

            public class TemporaryWithdrawal
            {
                public long gracePeriodDate;
            }

            public string appId;
            public List<AuthMappingInfo> authList;
            public long lastLoginDate;
            public long regDate;
            public string userId;
            public string valid;
            public TemporaryWithdrawal temporaryWithdrawal;
        }
    }
}
