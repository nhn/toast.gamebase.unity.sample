using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class LeaderboardRankItem : MonoBehaviour
    {
        private const string NONE_RANK_STRING = "-";

        private static Dictionary<string, Sprite> idPTextures = new Dictionary<string, Sprite>();

        [SerializeField]
        private Text rankText = null;
        [SerializeField]
        private Text userIdText = null;
        [SerializeField]
        private Text scoreText = null;
        [SerializeField]
        private Image idPImage = null;
        [SerializeField]
        private Image MyRankMark = null;

        public void SetInfo(LeaderboardVo.UserInfo userInfo)
        {
            SetInfo(userInfo.userId, userInfo.rank, userInfo.score, userInfo.extra);
        }

        public void SetInfo(string userId, int rank, double score, string idP)
        {
            rankText.text = rank == 0 ? NONE_RANK_STRING : rank.ToString();
            userIdText.text = userId;
            scoreText.text = score.ToString();

            if (MyRankMark != null)
            {
                MyRankMark.gameObject.SetActive(DataManager.User.Id.Equals(userId));
            }

            var idPTexture = GetIdPImage(idP);
            if (idPTexture != null)
            {
                idPImage.overrideSprite = idPTexture;
                idPImage.gameObject.SetActive(true);
            }
            else
            {
                idPImage.gameObject.SetActive(false);
            }
        }

        private Sprite GetIdPImage(string idP)
        {
            if (idP.Contains("guest") == true)
            {
                return null;
            }

            Sprite sprite = null;
            if (idPTextures.TryGetValue(idP, out sprite) == true)
            {
                return sprite;
            }

            sprite = Resources.Load<Sprite>(string.Format("Texture/UI/sns-{0}", idP));
            if (sprite == null)
            {
                return null;
            }

            idPTextures.Add(idP, sprite);
            return sprite;
        }
    }
}