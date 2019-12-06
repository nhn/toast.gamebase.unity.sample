namespace Toast.Internal
{
    public class ToastCoreSetDebugModeAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://core/setdebugmode";

        protected override string GetUri()
        {
            return ACTION_URI;
        }

        protected override string Action(JSONObject payload)
        {
            if (payload == null)
            {
                return ToastNativeMessage.CreateErrorMessage(this.GetUri(),
                                                             this.GetTransactionId(),
                                                             false,
                                                             ToastNativeCommonErrorCode.InvalidParameter.Code,
                                                             this.GetUri() + " action not found").ToJsonString();
            }

            bool debugMode = payload["debugMode"].AsBool;

            ToastLog.Info("ToastCoreSetDebugModeAction : " + debugMode);

            ToastCoreSdk.Instance.NativeCore.SetDebugMode(debugMode);

            return ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                           this.GetTransactionId()).ToJsonString();
        }
    }
}
