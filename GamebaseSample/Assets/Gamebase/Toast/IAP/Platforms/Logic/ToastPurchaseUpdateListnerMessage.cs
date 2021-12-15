using Toast.Internal;

namespace Toast.Iap
{
    public class ToastPurchaseUpdateListnerMessage
    {
        private readonly string _uri;
        private readonly string _transactionId;
        private readonly ToastIapResponse.Header _header;
        private readonly IapPurchase _iapPurchase;

        public ToastPurchaseUpdateListnerMessage(string uri, string transactionId, ToastIapResponse.Header header, IapPurchase iapPurchase)
        {
            _uri = uri;
            _transactionId = transactionId;
            _header = header;
            _iapPurchase = iapPurchase;
        }

        public string ToJsonString()
        {
            JSONNode root = new JSONObject();
            JSONNode header = new JSONObject();
            JSONNode body = new JSONObject();
            JSONNode purchase = new JSONObject();

            header.Add(JsonKeys.TransactionId, _transactionId);

            body.Add(JsonKeys.IsSuccessful, _header._isSuccessful);
            body.Add(JsonKeys.ResultCode, _header._resultCode);
            body.Add(JsonKeys.ResultMessage, _header._resultMessage);

            if (_iapPurchase != null)
            {
                if (string.IsNullOrEmpty(_iapPurchase.PaymentId) == false)
                {
                    purchase.Add("paymentId", _iapPurchase.PaymentId);
                }
                purchase.Add("paymentSeq", _iapPurchase.PaymentSequence);
                if (string.IsNullOrEmpty(_iapPurchase.OriginalPaymentId) == false)
                {
                    purchase.Add("originalPaymentId", _iapPurchase.OriginalPaymentId);
                }
                purchase.Add("productId", _iapPurchase.ProductId);
                purchase.Add("productType", _iapPurchase.ProductType.ToString().ToUpper());
                purchase.Add("userId", _iapPurchase.UserId);
                purchase.Add("price", _iapPurchase.Price);
                purchase.Add("currencyCode", _iapPurchase.PriceCurrencyCode);
                purchase.Add("accessToken", _iapPurchase.AccessToken);
                purchase.Add("purchaseTime", _iapPurchase.PurchaseTime);
                purchase.Add("expiryTime", _iapPurchase.ExpiryTime);
                if (string.IsNullOrEmpty(_iapPurchase.DeveloperPayload) == false)
                {
                    purchase.Add("developerPayload", _iapPurchase.DeveloperPayload);
                }
#if UNTIY_ANDROID
                purchase.Add("linkedPaymentId", _iapPurchase.LinkedPaymentId);
#endif  // UNTIY_ANDROID
                body.Add("purchase", purchase);
            }

            root.Add(JsonKeys.Uri, _uri);
            root.Add(JsonKeys.Header, header);
            root.Add(JsonKeys.Body, body);
            return root.ToString();
        }
    }
}