namespace Toast.Internal
{
    public class ToastCoreSetUserIdAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://core/setuserid";

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

            string userId = payload["userId"];

            if (string.IsNullOrEmpty(userId))
            {
                return ToastNativeMessage.CreateErrorMessage(this.GetUri(),
                                                             this.GetTransactionId(),
                                                             false,
                                                             ToastNativeCommonErrorCode.InvalidParameter.Code,
                                                             "userId is empty").ToJsonString();
            }

            ToastCoreSdk.Instance.NativeCore.SetUserId(userId);

            return ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                           this.GetTransactionId()).ToJsonString();
        }
    }
}
