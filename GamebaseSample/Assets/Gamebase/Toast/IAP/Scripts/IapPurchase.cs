using System;
using System.Collections.Generic;
using Toast.Iap.Extensions;
using Toast.Internal;

namespace Toast.Iap
{
    public class IapPurchase
    {
        public string PaymentId { get; set; }
        public string PaymentSequence { get; set; }
        public string OriginalPaymentId { get; set; }

        public string ItemSeq { get; set; }
        public string ProductId { get; set; }

        public ProductType ProductType { get; set; }

        public string UserId { get; set; }

        public float Price { get; set; }
        public string PriceCurrencyCode { get; set; }

        public string AccessToken { get; set; }

        public long PurchaseTime { get; set; }
        public long ExpiryTime { get; set; }

        public string DeveloperPayload { get; set; }

        public string LinkedPaymentId { get; set; }
        public bool IsStorePayment { get; set; }

        internal static IapPurchase From(JSONObject purchaseJson)
        {
            if (purchaseJson.ContainsKey("developerPayload"))
            {
                return new IapPurchase
                {
                    PaymentId = purchaseJson["paymentId"],
                    PaymentSequence = purchaseJson["paymentSeq"],
                    OriginalPaymentId = purchaseJson["originalPaymentId"],
                    ItemSeq = purchaseJson["itemSeq"],
                    ProductId = purchaseJson["productId"],
                    ProductType = purchaseJson["productType"].ToProductType(),
                    UserId = purchaseJson["userId"],
                    Price = purchaseJson["price"],
                    PriceCurrencyCode = purchaseJson["currencyCode"],
                    AccessToken = purchaseJson["accessToken"],
                    PurchaseTime = purchaseJson["purchaseTime"],
                    ExpiryTime = purchaseJson["expiryTime"],
                    DeveloperPayload = purchaseJson["developerPayload"],
#if UNITY_IOS
                    IsStorePayment = purchaseJson["isStorePayment"],
                    LinkedPaymentId = "",
#else
                    IsStorePayment = false,
                    LinkedPaymentId = purchaseJson["linkedPaymentId"],
#endif
                };
            }
            else
            {
                return new IapPurchase
                {
                    PaymentId = purchaseJson["paymentId"],
                    PaymentSequence = purchaseJson["paymentSeq"],
                    OriginalPaymentId = purchaseJson["originalPaymentId"],
                    ItemSeq = purchaseJson["itemSeq"],
                    ProductId = purchaseJson["productId"],
                    ProductType = purchaseJson["productType"].ToProductType(),
                    UserId = purchaseJson["userId"],
                    Price = purchaseJson["price"],
                    PriceCurrencyCode = purchaseJson["currencyCode"],
                    AccessToken = purchaseJson["accessToken"],
                    PurchaseTime = purchaseJson["purchaseTime"],
                    ExpiryTime = purchaseJson["expiryTime"],
#if UNITY_IOS
                    IsStorePayment = purchaseJson["isStorePayment"],
                    LinkedPaymentId = "",
#else
                    IsStorePayment = false,
                    LinkedPaymentId = purchaseJson["linkedPaymentId"],
#endif
                };
            }
        }

        public override string ToString()
        {
            var expiryTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            expiryTime = expiryTime.AddMilliseconds(ExpiryTime).ToLocalTime();

            if (string.IsNullOrEmpty(DeveloperPayload))
            {
                var json = JSONObject.FromDictionary(new Dictionary<string, object>
                {
                    {"paymentId", PaymentId},
                    {"paymentSequence", PaymentSequence},
                    {"itemSeq", ItemSeq},
                    {"productId", ProductId},
                    {"productType", ProductType},
                    {"expiryTime", expiryTime.ToString("s")},
                    {"userId", UserId},
                    {"IsStorePayment", IsStorePayment},
                    {"linkedPaymentId", LinkedPaymentId},
                });

                return json.ToString(2);
            }
            else
            {
                var json = JSONObject.FromDictionary(new Dictionary<string, object>
                {
                    {"paymentId", PaymentId},
                    {"paymentSequence", PaymentSequence},
                    {"itemSeq", ItemSeq},
                    {"productId", ProductId},
                    {"productType", ProductType},
                    {"expiryTime", expiryTime.ToString("s")},
                    {"userId", UserId},
                    {"IsStorePayment", IsStorePayment},
                    {"developerPayload", DeveloperPayload},
                    {"linkedPaymentId", LinkedPaymentId},
                });

                return json.ToString(2);
            }
        }

        public static string ToString(List<IapPurchase> purchases)
        {
            if (purchases == null || purchases.Count <= 0)
            {
                return "(empty)";
            }

            var jsonArray = new JSONArray();
            purchases.ForEach(p => jsonArray.Add(JSONNode.Parse(p.ToString())));

            return jsonArray.ToString(2);
        }
    }
}