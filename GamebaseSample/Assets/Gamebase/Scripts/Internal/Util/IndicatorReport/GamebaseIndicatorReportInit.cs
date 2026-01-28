#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal
{
    public partial class GamebaseIndicatorReport
    {
        public static class Init
        {
            private static int initializeFailCount = 0;
            public static void InitFailedMultipleTimes(GamebaseRequest.GamebaseConfiguration configuration, GamebaseError error)
            {
                if (Gamebase.IsSuccess(error) == false)
                {
                    initializeFailCount++;
                    if (initializeFailCount > stability.initFailCount)
                    {
                        var customFields = new Dictionary<string, string>
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_CONFIGURATION, JsonMapper.ToJson(configuration) }
                        };
                        var item = new IndicatorItem
                        {
                            logType = GamebaseIndicatorReportType.LogType.INIT,
                            stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_INIT_FAILED_MULTIPLE_TIMES,
                            logLevel = GamebaseIndicatorReportType.LogLevel.WARN,
                            customFields = customFields,
                            error = error
                        };
                        
                        AddIndicatorItem(item);
                        
                        initializeFailCount = 0;
                    }
                }
                else
                {
                    initializeFailCount = 0;
                }
                
                
            }
        }
    }
}
#endif