#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebaseAnalytics : IGamebaseAnalytics
    {
        private string domain = string.Empty;
        protected bool isAuthenticationAlreadyProgress = false;
        
        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain) == true)
                {
                    return typeof(CommonGamebaseAuth).Name;
                }

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        virtual public void SetGameUserData(GamebaseRequest.Analytics.GameUserData gameUserData)
        {
            GamebaseAnalytics.Instance.UserLevel = gameUserData.userLevel;
            GamebaseAnalytics.Instance.ChannelId = gameUserData.channelId;
            GamebaseAnalytics.Instance.CharacterId = gameUserData.characterId;
            GamebaseAnalytics.Instance.CharacterClassId = gameUserData.characterClassId;

            GamebaseAnalytics.Instance.SetUserMeta(GamebaseAnalytics.RequestType.USER_DATA,
                (error)=>
                {
                    string stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_SET_GAME_USER_DATA_SUCCESS;
                    if (Gamebase.IsSuccess(error) == false)
                    {
                        stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_SET_GAME_USER_DATA_FAILED;
                    }

                    GamebaseIndicatorReport.SendIndicatorData(
                        GamebaseIndicatorReportType.LogType.TAA,
                        stabilityCode,
                        GamebaseIndicatorReportType.LogLevel.DEBUG,
                        new Dictionary<string, string>()
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_TAA_USER_LEVEL, GamebaseAnalytics.Instance.UserLevel.ToString()},
                            { GamebaseIndicatorReportType.AdditionalKey.GB_GAME_USER_DATA, JsonMapper.ToJson(gameUserData) }
                        },
                        error);
                });            
        }

        virtual public void TraceLevelUp(GamebaseRequest.Analytics.LevelUpData levelUpData)
        {
            GamebaseAnalytics.Instance.UserLevel = levelUpData.userLevel;
            GamebaseAnalytics.Instance.LevelUpTime = levelUpData.levelUpTime;

            GamebaseAnalytics.Instance.SetUserMeta(GamebaseAnalytics.RequestType.LEVEL_UP,
                (error)=>
                {
                    string stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_TRACE_LEVEL_UP_SUCCESS;
                    if (Gamebase.IsSuccess(error) == false)
                    {
                        stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_TAA_TRACE_LEVEL_UP_FAILED;
                    }

                    GamebaseIndicatorReport.SendIndicatorData(
                        GamebaseIndicatorReportType.LogType.TAA,
                        stabilityCode,
                        GamebaseIndicatorReportType.LogLevel.DEBUG,
                        new Dictionary<string, string>()
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_TAA_USER_LEVEL, GamebaseAnalytics.Instance.UserLevel.ToString()},
                            { GamebaseIndicatorReportType.AdditionalKey.GB_GAME_USER_DATA, JsonMapper.ToJson(levelUpData) }
                        },
                        error);
                });            
        }
    }
}
#endif