#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class TermsResponse
    {
        public class QueryTerms : BaseVO
        {
            public CommonResponse.Header header;
            public Terms terms;

            public class Terms
            {
                public int termsSeq;
                public string termsVersion;
                public string termsCountryType;
                public List<Contents> contents;

                public class Contents
                {
                    public int termsContentSeq;
                    public string name;
                    public bool required;
                    public string agreePush;
                    public bool agreed;
                    public int node1DepthPosition;
                    public int node2DepthPosition = -1;
                    public string detailPageUrl;
                }
            }
        }

        public class UpdateTerms : BaseVO
        {
            public CommonResponse.Header header;
        }
    }
}
#endif