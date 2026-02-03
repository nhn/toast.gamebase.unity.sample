#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal
{
    public partial class GamebaseIndicatorReport
    {
        public static class TTA
        {
            public static void SetGameUserData(GamebaseRequest.Analytics.GameUserData gameUserData, GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_TAA_USER_LEVEL, gameUserData.userLevel.ToString() },
                    { GamebaseIndicatorReportType.AdditionalKey.GB_GAME_USER_DATA, JsonMapper.ToJson(gameUserData) }
                };

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.TAA,
                    customFields = customFields,
                    logLevel = GamebaseIndicatorReportType.LogLevel.DEBUG
                };

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_SET_GAME_USER_DATA_SUCCESS;
                }
                else
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_SET_GAME_USER_DATA_FAILED;
                    item.error = error;
                }

                AddIndicatorItem(item);
            }
            
            public static void TraceLevelUp(GamebaseRequest.Analytics.LevelUpData levelUpData, GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_TAA_USER_LEVEL, levelUpData.userLevel.ToString()},
                    { GamebaseIndicatorReportType.AdditionalKey.GB_LEVEL_UP_DATA, JsonMapper.ToJson(levelUpData) }
                };

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.TAA,
                    logLevel = GamebaseIndicatorReportType.LogLevel.DEBUG,
                    customFields = customFields
                };

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_TRACE_LEVEL_UP_SUCCESS;
                }
                else
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_TRACE_LEVEL_UP_FAILED;
                    item.error = error;
                }
                
                AddIndicatorItem(item);
            }
            
            public static void PurchaseComplete(int userLevel, GamebaseResponse.Purchase.PurchasableReceipt purchasableReceipt, GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_TAA_USER_LEVEL, userLevel.ToString()},
                    { GamebaseIndicatorReportType.AdditionalKey.GB_STORE_CODE, GamebaseUnitySDK.StoreCode},
                };
                if (purchasableReceipt != null)
                {
                    customFields.Add(GamebaseIndicatorReportType.AdditionalKey.GB_PAYMENT_SEQ, purchasableReceipt.paymentSeq);
                }

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.TAA,
                    logLevel = GamebaseIndicatorReportType.LogLevel.DEBUG,
                    customFields = customFields
                };

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_PURCHASE_COMPLETE_SUCCESS;
                }
                else
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_PURCHASE_COMPLETE_FAILED;
                    item.error = error;
                }

                AddIndicatorItem(item);
            }
            
            public static void ResetUserLevel()
            {
                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.TAA,
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_RESET_USER_LEVEL,
                    logLevel = GamebaseIndicatorReportType.LogLevel.DEBUG
                };

                AddIndicatorItem(item);
            }
        }
    }
}
#endif