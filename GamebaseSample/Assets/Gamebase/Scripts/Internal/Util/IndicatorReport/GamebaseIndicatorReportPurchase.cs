#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.LitJson;
using System.Globalization;

namespace Toast.Gamebase.Internal
{
    public partial class GamebaseIndicatorReport
    {
        public static class Purchase
        {
            public static void IapPurchase(string iapAppKey, long itemSeq, GamebaseResponse.Purchase.PurchasableReceipt purchasableReceipt, GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_TCIAP_APP_KEY, iapAppKey},
                    { GamebaseIndicatorReportType.AdditionalKey.GB_ITEM_SEQ, itemSeq.ToString() },
                };
                
                if (purchasableReceipt != null)
                {
                    customFields.Add(GamebaseIndicatorReportType.AdditionalKey.GB_PAYMENT_SEQ, purchasableReceipt.paymentSeq);
                    customFields.Add(GamebaseIndicatorReportType.AdditionalKey.GB_PURCHASABLE_PAYLOAD, purchasableReceipt.payload);
                    customFields.Add(GamebaseIndicatorReportType.AdditionalKey.GB_PURCHASABLE_RECEIPT, JsonMapper.ToJson(purchasableReceipt));
                    if (Gamebase.IsSuccess(error))
                    {
                        customFields.Add(GamebaseIndicatorReportType.AdditionalKey.GB_PRICE, purchasableReceipt.price.ToString(CultureInfo.InvariantCulture));
                    }
                }
                
                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.PURCHASE,
                    customFields = customFields,
                };
                
                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_IAP_PURCHASE_SUCCESS;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                }
                else
                {
                    if (error.code == GamebaseErrorCode.PURCHASE_USER_CANCELED)
                    {
                        item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_IAP_PURCHASE_CANCELED;
                        item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                        item.isUserCanceled = true;
                    }
                    else
                    {
                        item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_IAP_PURCHASE_FAILED;
                        item.logLevel = GamebaseIndicatorReportType.LogLevel.ERROR;
                    }
                    item.error = error;
                }

                AddIndicatorItem(item);
            }
            
            public static void IapPurchaseInvalidReceipt(GamebaseResponse.Purchase.PurchasableReceipt purchasableReceipt)
            {
                var customFields = new Dictionary<string, string>();
                
                if (purchasableReceipt != null)
                {
                    customFields.Add(GamebaseIndicatorReportType.AdditionalKey.GB_PURCHASABLE_RECEIPT, JsonMapper.ToJson(purchasableReceipt));
                    customFields.Add(GamebaseIndicatorReportType.AdditionalKey.GB_PRICE, purchasableReceipt.price.ToString(CultureInfo.InvariantCulture));
                }
                
                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.PURCHASE,
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_IAP_PURCHASE_INVALID_RECEIPT,
                    logLevel = GamebaseIndicatorReportType.LogLevel.ERROR,
                    customFields = customFields
                };

                AddIndicatorItem(item);
            }
        }
    }
}
#endif