#if UNITY_EDITOR || UNITY_IOS
namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebaseAuth : NativeGamebaseAuth
    {
        override protected void Init()
        {
            CLASS_NAME      = "TCGBAuthPlugin";
            messageSender   = IOSMessageSender.Instance;

            base.Init();
        }
    }
}
#endif