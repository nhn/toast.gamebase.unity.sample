#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class ShortTermTicketResponse
    {
        public class IssueShortTermTicketInfo : BaseVO
        {
            public CommonResponse.Header header;
            public string ticket;
        }
    }
}
#endif