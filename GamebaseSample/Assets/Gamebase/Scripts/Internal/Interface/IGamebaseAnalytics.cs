namespace Toast.Gamebase.Internal
{
    public interface IGamebaseAnalytics
    {
        void SetGameUserData(GamebaseRequest.Analytics.GameUserData gameUserData);
        void TraceLevelUp(GamebaseRequest.Analytics.LevelUpData levelUpData);
    }
}