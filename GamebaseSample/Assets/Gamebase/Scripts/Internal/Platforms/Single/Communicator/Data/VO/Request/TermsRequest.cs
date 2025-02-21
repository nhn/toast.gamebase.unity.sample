#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class TermsRequest
    {
        public class QueryTermsVO : BaseVO
        {
            public class Parameter
            {
                public string userId;
                public string usimCountryCode;
                public string deviceCountryCode;
                public string displayLanguage;
                public string deviceLanguage;
            }

            public Parameter parameter;

            public QueryTermsVO()
            {
                parameter = new Parameter();
            }
        }

        public class UpdateTermsVO : BaseVO
        {
            public class Parameter
            {
                public string userId;
                public int termsSeq;
            }

            public class Payload
            {
                public List<GamebaseRequest.Terms.Content> contents;
            }

            public Parameter parameter;
            public Payload payload;

            public UpdateTermsVO()
            {
                parameter = new Parameter();
                payload = new Payload();
            }
        }
    }
}
#endif