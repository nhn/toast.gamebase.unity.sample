#if UNITY_EDITOR || UNITY_IOS
namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebaseUtil : NativeGamebaseUtil
    {
        override protected void Init()
        {
            CLASS_NAME      = "TCGBUtilPlugin";
            messageSender   = IOSMessageSender.Instance;
            
            base.Init();
        }
    }
}
#endif