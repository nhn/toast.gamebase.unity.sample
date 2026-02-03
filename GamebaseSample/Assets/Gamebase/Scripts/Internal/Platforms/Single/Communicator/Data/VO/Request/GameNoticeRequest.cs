#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single.Communicator
{

    public class GameNoticeRequest
    {

        public class ReqGameNoticeInfoVO : BaseVO
        {
            public class Parameter
            {
                public string clientVersion;
                public string osCode;
                public string storeCode;
                public string deviceLanguage;
                public string displayLanguage;
                public string deviceCountryCode;
                public string usimCountryCode;
                public bool filterCategory = true;
                public string[] categoryNames;
            }

            public Parameter parameter;

            public ReqGameNoticeInfoVO()
            {
                parameter = new Parameter();
            }
        }
    }
}
#endif
