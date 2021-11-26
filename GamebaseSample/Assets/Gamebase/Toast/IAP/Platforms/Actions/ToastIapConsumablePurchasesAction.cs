namespace Toast.Internal
{
    public class ToastIapConsumablePurchasesAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://iap/purchase/consumable";

        protected override string GetUri()
        {
            return ACTION_URI;
        }

        protected override string Action(JSONObject payload)
        {
            var native = ToastNativeMessage.CreateErrorMessage(this.GetUri(),
                this.GetTransactionId(),
                false,
                9999,
                "Not supported error!!");

            return native.ToJsonString();
        }
    }
}