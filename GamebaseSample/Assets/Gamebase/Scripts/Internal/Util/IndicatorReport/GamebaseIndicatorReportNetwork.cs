#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public partial class GamebaseIndicatorReport
    {
        public static class Network
        {
            public static void ChangeDomainSuccess(string domain)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_DOMAIN, domain }
                };

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.NETWORK,
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_NETWORK_CHANGE_DOMAIN_SUCCESS,
                    logLevel = GamebaseIndicatorReportType.LogLevel.INFO,
                    customFields = customFields
                };

                AddIndicatorItem(item);
            }

            public static void DomainConnectionFailed(GamebaseError error)
            {
                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.NETWORK,
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_NETWORK_DOMAIN_CONNECTION_FAILED,
                    logLevel = GamebaseIndicatorReportType.LogLevel.ERROR,
                    error = error,
                };

                AddIndicatorItem(item);
            }
        }
    }
}
#endif