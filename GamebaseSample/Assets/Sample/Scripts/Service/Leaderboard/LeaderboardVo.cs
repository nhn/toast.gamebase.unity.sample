using System.Collections.Generic;

namespace GamebaseSample
{
    public class LeaderboardVo
    {
        public class SetSingleUserScoreRequest
        {
            public long transactionId;
            public double score;
            public string extra;
        }

        public class Response
        {
            public class ResponseHeader
            {
                public bool isSuccessful;
                public int resultCode;
                public string resultMessage;
            }

            public ResponseHeader header;
            public long transactionId;
        }

        public class UserInfo
        {
            public int resultCode;
            public string userId;
            public double score;
            public int rank;
            public int preRank;
            public string extra;
            public string date;
        }

        public class UserInfosByRange
        {
            public int resultCode;
            public int factor;
            public List<UserInfo> userInfos;
        }

        public class GetSingleUserInfoResponse : Response
        {
            public UserInfo userInfo;
        }

        public class GetMultipleUserInfoByRangeResponse : Response
        {
            public UserInfosByRange userInfosByRange;
        }

        public class SetSingleUserScoreResponse : Response
        {
            public class ResultInfo
            {
                public int resultCode;
                public string userId;
            }

            public ResultInfo resultInfo;
        }
    }
}