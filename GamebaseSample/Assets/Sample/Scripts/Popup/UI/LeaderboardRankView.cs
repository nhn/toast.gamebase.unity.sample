using System.Collections.Generic;
using UnityEngine;

namespace GamebaseSample
{
    public class LeaderboardRankView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform ItemPrefab;
        [SerializeField]
        private GameObject EmptyView;

        private List<LeaderboardRankItem> items = new List<LeaderboardRankItem>();

        public void SetInfo(LeaderboardVo.UserInfosByRange userInfosByRange)
        {
            if (userInfosByRange == null || userInfosByRange.userInfos == null)
            {
                ShowEmptyView(true);
                return;
            }

            var count = userInfosByRange.userInfos.Count;
            if (count == 0)
            {
                ShowEmptyView(true);
            }

            for (int i = 0; i < count; i++)
            {
                LeaderboardVo.UserInfo userInfo = userInfosByRange.userInfos[i];
                if (i < items.Count)
                {
                    items[i].SetInfo(userInfo);
                }
                else
                {
                    var newObject = Instantiate(ItemPrefab, transform, true);
                    newObject.transform.localPosition = Vector3.zero;
                    newObject.transform.localScale = Vector3.one;

                    var item = newObject.GetComponentInChildren<LeaderboardRankItem>();
                    if (item != null)
                    {
                        item.SetInfo(userInfo);
                        items.Add(item);
                    }
                }
            }
        }

        private void Awake()
        {
            ShowEmptyView(false);
        }


        private void ShowEmptyView(bool isShow)
        {
            if (EmptyView != null)
            {
                EmptyView.SetActive(isShow);
            }
        }
    }
}