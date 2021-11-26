using System.Collections.Generic;
using Toast.Iap.Extensions;
using Toast.Internal;

namespace Toast.Iap
{
    public class IapProduct
    {
        public string ProductSeq { get; set; }
        public string ProductId { get; set; }
        [System.Obsolete]
        public string ProductName { get; set; }
        public ProductType ProductType { get; set; }
        public bool IsActive { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
        public string LocalizedPrice { get; set; }
        public string LocalizedTitle { get; set; }
        public string LocalizedDescription{ get; set; }

        internal static IapProduct From(JSONObject jsonObject)
        {
            return new IapProduct
            {
                ProductSeq = jsonObject["seq"],
                ProductId = jsonObject["id"],
#pragma warning disable 0612
                ProductName = jsonObject["name"],
#pragma warning restore 0612
                ProductType = jsonObject["type"].ToProductType(),
                IsActive = jsonObject["isActive"],
                Price = jsonObject["price"],
                Currency = jsonObject["currency"],
                LocalizedPrice = jsonObject["localizedPrice"],
                LocalizedTitle = jsonObject["localizedTitle"],
                LocalizedDescription = jsonObject["localizedDescription"]

            };
        }

        public override string ToString()
        {
            return JSONObject.FromDictionary(new Dictionary<string, object>
            {
                {"seq", ProductSeq},
                {"id", ProductId},
#pragma warning disable 0612
                {"name", ProductName},
#pragma warning restore 0612
                {"type", ProductType},
                {"isActive", IsActive},
                {"price", Price},
                {"currency", Currency},
                {"localizedPrice", LocalizedPrice},
                {"localizedTitle", LocalizedTitle},
                {"localizedDescription", LocalizedDescription},
            }).ToString(2);
        }
    }
}