#if UNITY_EDITOR || UNITY_IOS
namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebaseImageNotice : NativeGamebaseImageNotice
    {
        override protected void Init()
        {
            CLASS_NAME = "TCGBImageNoticePlugin";
            messageSender = IOSMessageSender.Instance;

            base.Init();
        }
    }
}
#endif
