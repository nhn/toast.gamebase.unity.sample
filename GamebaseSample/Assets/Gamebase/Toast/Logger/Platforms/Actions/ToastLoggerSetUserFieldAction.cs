namespace Toast.Internal
{
    public class ToastLoggerSetUserFieldAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://logger/setUserField";

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

            string key = payload["key"].Value;
            string value = payload["value"].Value;

            if (string.IsNullOrEmpty(key) || value == null)
            {
                return ToastNativeMessage.CreateErrorMessage(this.GetUri(),
                                                             this.GetTransactionId(),
                                                             false,
                                                             ToastNativeCommonErrorCode.InvalidParameter.Code,
                                                             "Invalid Parameter").ToJsonString();
            }

            ToastLoggerSdk.Instance.NativeLogger.SetUserField(key, value);

            return ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                           this.GetTransactionId()).ToJsonString();
        }
    }
}
