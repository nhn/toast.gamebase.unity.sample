#if UNITY_EDITOR || UNITY_IOS
namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebaseWebview : NativeGamebaseWebview
    {
        override protected void Init()
        {
            CLASS_NAME      = "TCGBWebviewPlugin";
            messageSender   = IOSMessageSender.Instance;
            
            base.Init();
        }
    }
}
#endif