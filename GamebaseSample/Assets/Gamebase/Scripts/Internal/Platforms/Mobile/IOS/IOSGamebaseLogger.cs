#if UNITY_EDITOR || UNITY_IOS
using Toast.Gamebase.Internal.Mobile.IOS;

namespace Toast.Gamebase.Mobile.IOS
{
    public class IOSGamebaseLogger : NativeGamebaseLogger
    {
        override protected void Init()
        {
            CLASS_NAME = "TCGBLoggerPlugin";
            messageSender = IOSMessageSender.Instance;

            base.Init();
        }
    }
}
#endif