namespace Toast.Internal
{
    public class ToastIapOngateConsumablePurchasesAction : ToastUnityAction
    {
        public static string ACTION_URI = "toast://ongate/isdebugmode";

        protected override string GetUri()
        {
            return ACTION_URI;
        }

        protected override string Action(JSONObject payload)
        {
            var native = ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                                 this.GetTransactionId());

            bool result = true;

            native.AddExtraData("debugMode", result ? "True" : "False");
            return native.ToJsonString();
        }
    }
}
