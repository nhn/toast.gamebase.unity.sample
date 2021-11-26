#if UNITY_EDITOR || UNITY_IOS
namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebaseTerms : NativeGamebaseTerms
    {
        override protected void Init()
        {
            CLASS_NAME = "TCGBTermsPlugin";
            messageSender = IOSMessageSender.Instance;

            base.Init();
        }
    }
}
#endif
