using UnityEngine;

namespace Toast.Internal
{
    public abstract class ToastUnityAction
    {
        private string _transactionId;

        protected abstract string GetUri();

        protected abstract string Action(JSONObject payload);

        public string OnMessage(ToastUnityMessage unityMessage)
        {
            string transactionId = unityMessage.TransactionId;

            if (string.IsNullOrEmpty(transactionId))
            {
                throw new UnityException("Required field does not exist. (empty transactionId).");
            }

            _transactionId = transactionId;
            JSONObject payload = unityMessage.GetPayload();

            return Action(payload);
        }

        public string GetTransactionId()
        {
            return _transactionId;
        }
    }
}
