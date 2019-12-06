namespace Toast.Internal
{
    public class ToastCoreInitializeAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://core/initialize";

        protected override string GetUri()
        {
            return ACTION_URI;
        }

        protected override string Action(JSONObject payload)
        {
            var native = ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                                 this.GetTransactionId());

            ToastCoreSdk.Instance.NativeCore.Initialize();

            return native.ToJsonString();
        }
    }
}
