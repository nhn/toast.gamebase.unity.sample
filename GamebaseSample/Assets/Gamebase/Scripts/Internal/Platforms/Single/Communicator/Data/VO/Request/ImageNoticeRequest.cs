#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single.Communicator
{

    public class ImageNoticeRequest
    {

        public class ReqImageNoticeInfoVO : BaseVO
        {
            public class Parameter
            {
                public string osCode;
                public string clientVersion;
                public string storeCode;
                public string deviceLanguage;
                public string displayLanguage;
                public string deviceCountryCode;
                public string usimCountryCode;
            }

            public Parameter parameter;

            public ReqImageNoticeInfoVO()
            {
                parameter = new Parameter();
            }
        }
    }
}
#endif
