#if UNITY_EDITOR || UNITY_IOS
namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebase : NativeGamebase
    {
        override protected void Init()
        {
            CLASS_NAME      = "TCGBGamebasePlugin";
            messageSender   = IOSMessageSender.Instance;

            base.Init();
        }
    }
}
#endif