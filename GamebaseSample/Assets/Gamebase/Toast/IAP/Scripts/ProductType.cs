using Toast.Internal;

namespace Toast.Iap
{
    public enum ProductType
    {
        Unknown,
        Consumable,
        AutoRenewable,
        ConsumableAutoRenewable

    }

    namespace Extensions
    {
        internal static class ProductTypeExtension
        {
            internal static ProductType ToProductType(this JSONNode productType)
            {
                string productTypeStr = productType;
                switch (productTypeStr.ToUpper())
                {
                    case "CONSUMABLE":
                        return ProductType.Consumable;
                    case "AUTO_RENEWABLE":
                        return ProductType.AutoRenewable;
                    case "CONSUMABLE_AUTO_RENEWABLE":
                        return ProductType.ConsumableAutoRenewable;
                    default:
                        return ProductType.Unknown;
                }
            }
        }
    }
}