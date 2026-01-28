#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal
{
    public partial class GamebaseIndicatorReport
    {
        public static class Common
        {
            public static void WrongUsage(string functionName, string errorLog, GamebaseError error = null)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_FUNCTION_NAME, functionName},
                    { GamebaseIndicatorReportType.AdditionalKey.GB_ERROR_LOG, errorLog},
                };
                
                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.COMMON,
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_COMMON_WRONG_USAGE,
                    logLevel = GamebaseIndicatorReportType.LogLevel.ERROR,
                    customFields = customFields
                };

                AddIndicatorItem(item);
            }
        }
    }
}
#endif