using System;
using System.Runtime.Serialization;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class Lighthouse
    {
        public enum ZoneType
        {
            [EnumMember(Value = "wss://alpha-gamebase-lh.cloud.toast.com:11443/lh")]
            ALPHA,
            [EnumMember(Value = "wss://beta-gamebase-lh.cloud.toast.com:11443/lh")]
            BETA,
            [EnumMember(Value = "wss://gslb-gamebase-lh.cloud.toast.com:11443/lh")]
            REAL
        }
        
        public static string URI { get; set; }

        public static long CreateTransactionId()
        {
            long i64Guid = 0;
            long buf = 0;
            Guid guid = Guid.NewGuid();

            byte[] guidBytes = guid.ToByteArray();

            for (int i = 0; i < 8; i++)
            {
                i64Guid |= guidBytes[i];
                i64Guid <<= 8;
            }

            for (int i = 8; i < 16; i++)
            {
                buf |= guidBytes[i];
                buf <<= 8;
            }

            i64Guid ^= buf;

            return i64Guid;
        }

        public class API
        {
            public const string VERSION = "v1.3.2";

            public class Launching
            {
                public const string PRODUCT_ID = "launching";

                public class ID
                {
                    public const string GET_LAUNCHING = "getLaunching";
                    public const string GET_LAUNCHING_STATUS = "getLaunchingStatus";
                    public const string GET_IMAGE_NOTICES = "getImageNotices";
                }
            }

            public class Gateway
            {
                public const string PRODUCT_ID = "gateway";

                public class ID
                {
                    public const string IDP_LOGIN = "idPLogin";
                    public const string TOKEN_LOGIN = "tokenLogin";
                    public const string WITHDRAW = "withdraw";
                    public const string TEMPORARY_WITHDRAWAL = "temporaryWithdrawal";
                    public const string CANCEL_TEMPORARY_WITHDRAWAL = "cancelTemporaryWithdrawal";
                    public const string LOGOUT = "logout";
                    public const string REMOVE_MAPPING = "removeMapping";
                    public const string ADD_MAPPING = "addMapping";
                    public const string ISSUE_SHORT_TERM_TICKET = "issueShortTermTicket";
                    public const string INTROSPECT_ACCESS_TOKEN = "introspectAccessToken";

                    /// <summary>
                    /// HEALTH_CHECK is used only to check the Internet connection status on the WebGL platform.
                    /// </summary>
                    public const string HEALTH_CHECK = "healthCheck";
                }
            }

            public class Presence
            {
                public const string PRODUCT_ID = "presence";

                public class ID
                {
                    public const string HEARTBEAT = "heartbeat";
                }
            }
        }
    }
}