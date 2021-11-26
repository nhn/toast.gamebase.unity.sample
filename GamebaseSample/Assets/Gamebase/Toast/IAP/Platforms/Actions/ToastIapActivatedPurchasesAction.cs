namespace Toast.Internal
{
    public class ToastIapActivatedPurchasesAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://iap/purchases/activated";

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
