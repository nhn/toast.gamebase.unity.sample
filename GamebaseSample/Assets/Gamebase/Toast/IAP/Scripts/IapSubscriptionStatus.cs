using System;
using Toast.Internal;
using Toast.Iap.Extensions;

namespace Toast.Iap
{
    public class IapSubscriptionStatus
    {
        public enum Status
        {
            Active = 0,
            Canceled = 3,
            OnHold = 5,
            InGracePeriod = 6,
            Paused = 10,
            Revoked = 12,
            Expired = 13,
            Unknown = 9999
        }

        private readonly JSONObject _originJson;

        private readonly string _productId;
        private readonly ProductType _productType;
        private readonly string _paymentId;
        private readonly string _originalPaymentId;
        private readonly string _paymentSequence;
        private readonly string _userId;
        private readonly float _price;
        private readonly string _priceCurrencyCode;
        private readonly string _accessToken;
        private readonly long _purchaseTime;
        private readonly long _expiryTime;
        private readonly string _developerPayload;
        private readonly Status _status;
        private readonly string _statusDescription;

        public IapSubscriptionStatus(JSONObject jsonObject)
        {
            _originJson = jsonObject;
            _productId = jsonObject["productId"];
            _productType = jsonObject["productType"].ToProductType();
            _paymentId = jsonObject["paymentId"];
            _originalPaymentId = jsonObject["originalPaymentId"];
            _paymentSequence = jsonObject["paymentSeq"];
            _userId = jsonObject["userId"];
            _price = jsonObject["price"];
            _priceCurrencyCode = jsonObject["currencyCode"];
            _accessToken = jsonObject["accessToken"];
            _purchaseTime = jsonObject["purchaseTime"];
            _expiryTime = jsonObject["expiryTime"];
            if (jsonObject.ContainsKey("developerPayload"))
            {
                _developerPayload = jsonObject["developerPayload"];
            }
            _status = (Status)jsonObject["statusCode"].AsInt;
            _statusDescription = jsonObject["statusDescription"];
        }

        public static IapSubscriptionStatus From(JSONObject jsonObject)
        {
            return new IapSubscriptionStatus(jsonObject);
        }

        public string GetProductId()
        {
            return this._productId;
        }

        public ProductType GetProductType()
        {
            return this._productType;
        }

        public string GetPaymentId()
        {
            return this._paymentId;
        }

        public string GetOriginalPaymentId()
        {
            return this._originalPaymentId;
        }

        public string GetPaymentSequence()
        {
            return this._paymentSequence;
        }

        public string GetUserId()
        {
            return this._userId;
        }

        public float GetPrice()
        {
            return this._price;
        }

        public string GetPriceCurrencyCode()
        {
            return this._priceCurrencyCode;
        }

        public string GetAccessToken()
        {
            return this._accessToken;
        }

        public long GetPurchaseTime()
        {
            return this._purchaseTime;
        }

        public long GetExpiryTime()
        {
            return this._expiryTime;
        }

        public string GetDeveloperPayload()
        {
            return this._developerPayload;
        }

        public Status GetStatus()
        {
            return this._status;
        }

        public string GetStatusDescription()
        {
            return this._statusDescription;
        }

        public override string ToString()
        {
            return this._originJson.ToString();
        }

        public string ToString(int indent)
        {
            return this._originJson.ToString(indent);
        }
    }
}

