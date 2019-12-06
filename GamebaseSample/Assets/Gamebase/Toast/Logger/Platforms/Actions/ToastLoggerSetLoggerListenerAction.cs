namespace Toast.Internal
{
    public class ToastLoggerSetLoggerListenerAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://logger/setLoggerListenner";

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

            ToastLoggerSdk.Instance.NativeLogger.SetLoggerListener();

            return ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                           this.GetTransactionId()).ToJsonString();
        }
    }
}
