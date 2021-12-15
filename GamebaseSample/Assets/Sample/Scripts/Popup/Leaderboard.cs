using UnityEngine;

namespace GamebaseSample
{
    public class Leaderboard : MonoBehaviour
    {
        public const int FACTOR_SCORE = 1;
        private const int TOP_RANK_LIST_COUNT = 10;
        private const int LEADERBOARD_NO_RECORD_HEIGHT = 680;
        private const int LEADERBOARD_RECORD_HEIGHT = 940;

        [SerializeField]
        private LeaderboardRankItem myRankItem = null;
        [SerializeField]
        private LeaderboardRankView topRankView = null;

        [SerializeField]
        private RectTransform rectTransform = null;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            Initialize();
        }

        private void Initialize()
        {
            LeaderboardApi.GetSingleUserInfo(FACTOR_SCORE, DataManager.User.Id,
                (userData) =>
                {
                    if (userData == null)
                    {
                        myRankItem.SetInfo(DataManager.User.Id, 0, 0, DataManager.User.IdP);
                    }
                    else
                    {
                        myRankItem.SetInfo(userData);
                    }
                });

            LeaderboardApi.GetMultipleUserInfoByRange(FACTOR_SCORE, 1, TOP_RANK_LIST_COUNT,
                (userInfosByRange) =>
                {
                    if (userInfosByRange == null || userInfosByRange.userInfos == null || userInfosByRange.userInfos.Count == 0)
                    {
                        Vector2 sizeDelta = rectTransform.sizeDelta;
                        sizeDelta.y = LEADERBOARD_NO_RECORD_HEIGHT;
                        rectTransform.sizeDelta = sizeDelta;
                    }

                    topRankView.SetInfo(userInfosByRange);
                });
        }

        #region UIButton.onClick
        public void ClickCloseButton()
        {
            Destroy(gameObject);
        }
        #endregion
    }
}