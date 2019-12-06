#if UNITY_EDITOR || UNITY_IOS
namespace Toast.Gamebase.Internal.Mobile.IOS
{
    class IOSGamebaseNetwork : NativeGamebaseNetwork
    {
        override protected void Init()
        {
            CLASS_NAME      = "TCGBNetworkPlugin";
            messageSender   = IOSMessageSender.Instance;

            base.Init();
        }
    }
}
#endif