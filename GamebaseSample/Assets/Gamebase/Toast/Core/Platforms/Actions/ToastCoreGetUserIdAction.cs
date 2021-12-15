namespace Toast.Internal
{
    public class ToastCoreGetUserIdAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://core/getUserId";

        protected override string GetUri()
        {
            return ACTION_URI;
        }

        protected override string Action(JSONObject payload)
        {
            var native = ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                                 this.GetTransactionId());

            string result = ToastCoreSdk.Instance.NativeCore.GetUserId();

            if (!string.IsNullOrEmpty(result))
            {
                native.AddExtraData("userId", result);
            }

            return native.ToJsonString();
        }
    }
}
