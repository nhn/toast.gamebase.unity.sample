#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class AnalyticsRequest
    {
        public class UserMetaVO : BaseVO
        {
            public class Payload
            {
                public string appId;
                public Dictionary<string, object> userMetaData;
            }

            public class Parameter
            {
                public string userId;
            }

            public Parameter parameter;
            public Payload payload;

            public UserMetaVO()
            {
                parameter = new Parameter();
                payload = new Payload();
            }
        }

        public class PurchaseVO : BaseVO
        {
            public class Parameter
            {
                public string userId;
            }

            public class Payload
            {
                public string appId;
                public string paySeq;
                public string clientVersion;
                public string idPCode;
                public string deviceModel;
                public string osCode;                
                public string usimCountryCode;
                public string deviceCountryCode;
                public string subStoreCode = null;

                public Dictionary<string, object> userMetaData;
            }

            public Parameter parameter;
            public Payload payload;

            public PurchaseVO()
            {
                parameter = new Parameter();
                payload = new Payload();
            }
        }
    }
}
#endif