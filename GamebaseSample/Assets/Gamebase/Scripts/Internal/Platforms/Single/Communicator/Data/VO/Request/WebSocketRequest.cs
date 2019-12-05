#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class WebSocketRequest
    {
        public class RequestVO : BaseVO
        {
            public string productId;
            public string apiId;
            public string version;
            public string appId;
            public Dictionary<string, string> headers;
            public object parameters;
            public string payload;

            public string transactionId;

            public RequestVO(
                string productId,
                string version,
                string appId)
            {
                this.productId = productId;
                this.version = version;
                this.appId = appId;

                transactionId = Lighthouse.CreateTransactionId().ToString().ToLower();
                headers = new Dictionary<string, string>();
                headers.Add("X-TCGB-Transaction-Id", transactionId);
                headers.Add("X-TCGB-Access-Token", Gamebase.GetAccessToken());
            }
        }
    }
}
#endif