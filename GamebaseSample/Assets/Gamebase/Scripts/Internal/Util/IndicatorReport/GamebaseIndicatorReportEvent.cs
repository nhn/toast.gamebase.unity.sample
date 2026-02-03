#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal
{
    public partial class GamebaseIndicatorReport
    {
        public static class Event
        {
            public static void ObserverBannedMember(GamebaseResponse.Event.GamebaseEventObserverData observerData)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_OBSERVER_DATA, JsonMapper.ToJson(observerData) }
                };
                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.EVENT,
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_EVENT_OBSERVER_BANNED_MEMBER,
                    logLevel = GamebaseIndicatorReportType.LogLevel.INFO,
                    customFields = customFields
                };

                AddIndicatorItem(item);
            }
            
            public static void ObserverInvalidMember(GamebaseResponse.Event.GamebaseEventObserverData observerData)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_OBSERVER_DATA, JsonMapper.ToJson(observerData) }
                };
                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.EVENT,
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_EVENT_OBSERVER_INVALID_MEMBER,
                    logLevel = GamebaseIndicatorReportType.LogLevel.INFO,
                    customFields = customFields
                };

                AddIndicatorItem(item);
            }
            
            public static void Logedout(GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_EVENT_LOGGED_OUT_DATA, JsonMapper.ToJson(error) }
                };
                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.EVENT,
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_EVENT_LOGGED_OUT,
                    logLevel = GamebaseIndicatorReportType.LogLevel.WARN,
                    customFields = customFields
                };

                AddIndicatorItem(item);
            }
        }
    }
}
#endif