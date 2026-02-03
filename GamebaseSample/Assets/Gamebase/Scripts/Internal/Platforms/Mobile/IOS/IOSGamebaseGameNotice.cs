#if UNITY_EDITOR || UNITY_IOS
namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebaseGameNotice : NativeGamebaseGameNotice
    {
        override protected void Init()
        {
            CLASS_NAME = "TCGBGameNoticePlugin";
            messageSender = IOSMessageSender.Instance;

            base.Init();
        }
    }
}
#endif