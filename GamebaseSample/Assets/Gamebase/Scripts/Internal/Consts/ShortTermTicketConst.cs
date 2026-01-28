#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single
{
    public static class ShortTermTicketConst
    {
        public const string PURPOSE_OPEN_CONTACT = "openContact";
        public const string PURPOSE_OPEN_CONTACT_FOR_BANNED_USER = "openContactForBannedUser";
        
        public const int EXPIRESIN = 10;
    }
}
#endif
