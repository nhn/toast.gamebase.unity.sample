#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class PurchaseRequest
    {
        public class Configuration : BaseVO
        {
            public string appKey;
            public string storeCode;
        }
    }
}
#endif
