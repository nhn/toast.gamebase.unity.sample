#if UNITY_EDITOR || UNITY_IOS
namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebaseCommunity : NativeGamebaseCommunity
    {
        override protected void Init()
        {
            CLASS_NAME = "TCGBCommunityPlugin";
            messageSender = IOSMessageSender.Instance;

            base.Init();
        }
    }
}
#endif