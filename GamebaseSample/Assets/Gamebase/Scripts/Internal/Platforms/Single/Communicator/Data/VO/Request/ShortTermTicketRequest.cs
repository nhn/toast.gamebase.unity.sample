#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class ShortTermTicketRequest
    {
        public class IssueShortTermTicketVO : BaseVO
        {
            public class Parameter
            {
                public string userId;
                public string purpose;
                public int expiresIn;
            }

            public Parameter parameter;

            public IssueShortTermTicketVO()
            {
                parameter = new Parameter();
            }
        }
    }
}
#endif