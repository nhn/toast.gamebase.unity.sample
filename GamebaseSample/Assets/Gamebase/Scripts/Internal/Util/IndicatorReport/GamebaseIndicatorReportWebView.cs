#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal
{
    public partial class GamebaseIndicatorReport
    {
        public static class WebView
        {
            public static void OpenFailed(string url, GamebaseRequest.Webview.Configuration configuration, GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_URL, url }
                };
                if(configuration != null)
                {
                    customFields.Add(GamebaseIndicatorReportType.AdditionalKey.GB_WEBVIEW_CONFIGURATION, JsonMapper.ToJson(configuration));
                }

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.WEBVIEW,
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_WEBVIEW_OPEN_FAILED,
                    logLevel = GamebaseIndicatorReportType.LogLevel.ERROR,
                    customFields = customFields,
                    error = error,
                };

                AddIndicatorItem(item);
            }
        }
    }
}
#endif