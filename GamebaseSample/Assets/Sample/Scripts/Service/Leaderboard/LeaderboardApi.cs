using LitJson;
using System;
using System.Text;
using UnityEngine.Networking;

namespace GamebaseSample
{
    public static class LeaderboardApi
    {
        private const string HEADER_CONTENT_TYPE = "Content-Type";
        private const string HEADER_CONTENT_VALUE_JSON = "application/json";

        private const long TRANSACTION_ID = 12345;

        private const int LEADERBOARD_SUCCESS = 0;
        private const int LEADERBOARD_SUCCESS_BUT_NOT_UPDATE = 1;

        private const string ERROR_MESSAGE_GET_SINGLE_USER_INFO = "Failed to get single user info.";
        private const string ERROR_MESSAGE_GET_MULTIPLE_USER_INFO_BY_RANGE = "Failed to get multiple user info by range.";
        private const string ERROR_MESSAGE_SET_SINGLE_USER_SCORE = "Failed to save user score.";
        private const string ERROR_MESSAGE_DELETE_SINGLE_USER_INFO = "Failed to delete user info.";

        public static void GetSingleUserInfo(int factor, string userId, Action<LeaderboardVo.UserInfo> callback)
        {
            string url = string.Format("{0}/factors/{1}/users?userId={2}", DataManager.Launching.leaderboard.FullUrl, factor, userId);

            SampleWebRequestObject.Instance.Request(
                UnityWebRequest.Get(url),
                (message) =>
                {
                    if (string.IsNullOrEmpty(message) == true)
                    {
                        Logger.Debug(ERROR_MESSAGE_GET_SINGLE_USER_INFO, typeof(LeaderboardApi));
                        callback(null);
                        return;
                    }

                    var vo = JsonMapper.ToObject<LeaderboardVo.GetSingleUserInfoResponse>(message);
                    if (vo.header.resultCode != LEADERBOARD_SUCCESS && vo.header.resultCode != LEADERBOARD_SUCCESS_BUT_NOT_UPDATE)
                    {
                        Logger.Debug(string.Format("{0} (Code={1})", ERROR_MESSAGE_GET_SINGLE_USER_INFO, vo.header.resultCode), typeof(LeaderboardApi));
                        callback(null);
                        return;
                    }

                    callback(vo.userInfo);
                });
        }

        public static void GetMultipleUserInfoByRange(int factor, int start, int size, Action<LeaderboardVo.UserInfosByRange> callback)
        {
            string url = string.Format("{0}/factors/{1}/users?start={2}&size={3}", DataManager.Launching.leaderboard.FullUrl, factor, start, size);

            SampleWebRequestObject.Instance.Request(
                UnityWebRequest.Get(url),
                (message) =>
                {
                    if (string.IsNullOrEmpty(message) == true)
                    {
                        Logger.Debug(ERROR_MESSAGE_GET_MULTIPLE_USER_INFO_BY_RANGE, typeof(LeaderboardApi));
                        callback(null);
                        return;
                    }

                    var vo = JsonMapper.ToObject<LeaderboardVo.GetMultipleUserInfoByRangeResponse>(message);
                    if (vo.header.resultCode != LEADERBOARD_SUCCESS && vo.header.resultCode != LEADERBOARD_SUCCESS_BUT_NOT_UPDATE)
                    {
                        Logger.Debug(string.Format("{0} (Code={1})", ERROR_MESSAGE_GET_MULTIPLE_USER_INFO_BY_RANGE, vo.header.resultCode), typeof(LeaderboardApi));
                        callback(null);
                        return;
                    }

                    callback(vo.userInfosByRange);
                });
        }

        public static void SetSingleUserScore(int factor, string userId, int userScore, string idP)
        {
            string url = string.Format("{0}/factors/{1}/users/{2}/score-with-extra", DataManager.Launching.leaderboard.FullUrl, factor, userId);

            LeaderboardVo.SetSingleUserScoreRequest requestVo = new LeaderboardVo.SetSingleUserScoreRequest()
            {
                transactionId = TRANSACTION_ID,
                score = userScore,
                extra = idP
            };

            byte[] body = Encoding.UTF8.GetBytes(JsonMapper.ToJson(requestVo));

            UnityWebRequest request = UnityWebRequest.Put(url, body);
            request.method = UnityWebRequest.kHttpVerbPOST;

            SampleWebRequestObject.Instance.Request(
                request,
                (message) =>
                {
                    if (string.IsNullOrEmpty(message) == true)
                    {
                        Logger.Debug(ERROR_MESSAGE_SET_SINGLE_USER_SCORE, typeof(LeaderboardApi));
                        return;
                    }

                    var vo = JsonMapper.ToObject<LeaderboardVo.SetSingleUserScoreResponse>(message);
                    if (vo.header.resultCode != LEADERBOARD_SUCCESS && vo.header.resultCode != LEADERBOARD_SUCCESS_BUT_NOT_UPDATE)
                    {
                        Logger.Debug(string.Format(" {0} (Code={1})", ERROR_MESSAGE_SET_SINGLE_USER_SCORE, vo.header.resultCode), typeof(LeaderboardApi));
                    }
                });
        }

        public static void DeleteSingleUserInfo(int factor, string userId)
        {
            string url = string.Format("{0}/factors/{1}/users?userId={2}", DataManager.Launching.leaderboard.FullUrl, factor, userId);

            UnityWebRequest request = UnityWebRequest.Delete(url);
            request.downloadHandler = new DownloadHandlerBuffer();

            SampleWebRequestObject.Instance.Request(
                request,
                (message) =>
                {
                    if (string.IsNullOrEmpty(message) == true)
                    {
                        Logger.Debug(ERROR_MESSAGE_DELETE_SINGLE_USER_INFO, typeof(LeaderboardApi));
                        return;
                    }

                    var vo = JsonMapper.ToObject<LeaderboardVo.Response>(message);
                    if (vo.header.resultCode != LEADERBOARD_SUCCESS && vo.header.resultCode != LEADERBOARD_SUCCESS_BUT_NOT_UPDATE)
                    {
                        Logger.Debug(string.Format(" {0} (Code={1})", ERROR_MESSAGE_DELETE_SINGLE_USER_INFO, vo.header.resultCode), typeof(LeaderboardApi));
                    }
                });
        }
    }
}