#if UNITY_EDITOR || UNITY_IOS
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Mobile.IOS
{
    public class IOSGamebasePush : NativeGamebasePush
    {
        override protected void Init()
        {
            CLASS_NAME      = "TCGBPushPlugin";
            messageSender   = IOSMessageSender.Instance;
            
            base.Init();
        }

        override public void SetSandboxMode(bool isSandbox)
        {
            var vo          = new NativeRequest.Push.IsSandboxMode();
            vo.isSandbox    = isSandbox;
            string jsonData = JsonMapper.ToJson(new UnityMessage(GamebasePush.PUSH_API_SET_SANDBOX_MODE, jsonData: JsonMapper.ToJson(vo)));
            messageSender.GetAsync(jsonData);
        }
    }
}
#endif