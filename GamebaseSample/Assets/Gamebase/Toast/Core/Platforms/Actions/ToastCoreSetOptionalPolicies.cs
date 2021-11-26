using System.Collections.Generic;

namespace Toast.Internal
{
    public class ToastCoreSetOptionalPolicies : ToastUnityAction
    {
        public static string ACTION_URI = "toast://core/setoptionalpolicies";

        protected override string GetUri()
        {
            return ACTION_URI;
        }

        protected override string Action(JSONObject payload)
        {
            var native = ToastNativeMessage.CreateSuccessMessage(this.GetUri(),
                                                                 this.GetTransactionId());

            List<string> properties = new List<string>();
            JSONArray array = payload["properties"].AsArray;

            foreach (JSONNode item in array)
            {
                properties.Add(item.ToString());
            }

            ToastCoreSdk.Instance.NativeCore.SetOptionalPolicies(properties);

            return native.ToJsonString();
        }
    }
}