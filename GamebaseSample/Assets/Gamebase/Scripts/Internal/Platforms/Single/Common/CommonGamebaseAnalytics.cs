#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
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

            GamebaseAnalytics.Instance.SetUserMeta(GamebaseAnalytics.RequestType.USER_DATA);
        }

        virtual public void TraceLevelUp(GamebaseRequest.Analytics.LevelUpData levelUpData)
        {
            GamebaseAnalytics.Instance.UserLevel = levelUpData.userLevel;
            GamebaseAnalytics.Instance.LevelUpTime = levelUpData.levelUpTime;

            GamebaseAnalytics.Instance.SetUserMeta(GamebaseAnalytics.RequestType.LEVEL_UP);
        }
    }
}
#endif