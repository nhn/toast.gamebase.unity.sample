namespace Toast.Internal
{
    public class ToastCoreIsDebugModeAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://core/isdebugmode";

        protected override string GetUri()
        {
            return ACTION_URI;
        }

        protected override string Action(JSONObject payload)
        {
            var native = ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                                 this.GetTransactionId());

            bool result = ToastCoreSdk.Instance.NativeCore.IsDebugMode();

            native.AddExtraData("debugMode", result ? "True" : "False");
            return native.ToJsonString();
        }
    }
}
