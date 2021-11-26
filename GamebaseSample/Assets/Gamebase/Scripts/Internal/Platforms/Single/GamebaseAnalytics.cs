#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Single
{
    public sealed class GamebaseAnalytics
    {
        private string domain;

        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain) == true)
                {
                    return typeof(GamebaseAnalytics).Name;
                }

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        private const string PRODUCT_ID = "presence";

        private const string KEY_USER_LEVEL = "userLevel";
        private const string KEY_LEVEL_UP_TIME = "levelUpTime";
        private const string KEY_CHARACTER_ID = "characterId";
        private const string KEY_CHANNEL_ID = "channelId";
        private const string KEY_CLASS_ID = "classId";

        private static class ID
        {
            public const string COMPLETE_PURCHASE = "completePurchase";
            public const string SET_USER_META = "setUserMeta";
        }

        public enum RequestType
        {
            USER_DATA,
            LEVEL_UP
        }

        public int UserLevel { get; set; }
        public long LevelUpTime { get; set; }
        public string IdPCode { get; set; }
        public string ChannelId { get; set; }
        public string CharacterId { get; set; }
        public string CharacterClassId { get; set; }

        public static GamebaseAnalytics Instance = new GamebaseAnalytics();

        public void CompletePurchase(GamebaseResponse.Purchase.PurchasableReceipt purchasableReceipt, GamebaseCallback.ErrorDelegate callback)
        {
            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
            {
                GamebaseLog.Warn("Not LoggedIn", this, "CompletePurchase");
                callback(new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }

            GamebaseLog.Debug("Start", this, "CompletePurchase");

            AnalyticsRequest.PurchaseVO vo = new AnalyticsRequest.PurchaseVO();
            vo.parameter.userId = Gamebase.GetUserID();

            vo.payload.appId = GamebaseUnitySDK.AppID;
            vo.payload.paySeq = purchasableReceipt.paymentSeq;
            vo.payload.clientVersion = GamebaseUnitySDK.AppVersion;
            vo.payload.idPCode = IdPCode;
            vo.payload.deviceModel = GamebaseUnitySDK.DeviceModel;
            vo.payload.osCode = GamebaseUnitySDK.Platform;
            vo.payload.usimCountryCode = "ZZ";
            vo.payload.deviceCountryCode = GamebaseUnitySDK.CountryCode;
            vo.payload.userMetaData = MakeUserMetaData();

            var requestVO = new WebSocketRequest.RequestVO(PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID);
            requestVO.apiId = ID.COMPLETE_PURCHASE;
            requestVO.parameters = vo.parameter;
            requestVO.payload = JsonMapper.ToJson(vo.payload);

            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                if (Gamebase.IsSuccess(error) == false)
                {
                    GamebaseLog.Warn(
                        string.Format(
                            "{0}\n{1}",
                            "Failed request.",
                            GamebaseJsonUtil.ToPrettyJsonString(error)),
                        this,
                        "CompletePurchase");
                    callback(error);
                    return;
                }
                callback(error);
            });
        }

        public void SetUserMeta(RequestType type, GamebaseCallback.ErrorDelegate callback)
        {
            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
            {
                GamebaseLog.Warn("Not LoggedIn", this, "SetUserMeta");
                callback(new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }

            GamebaseLog.Debug(
                string.Format("SetUserMeta Type : {0}", type.ToString()),
                this,
                "SetUserMeta");

            AnalyticsRequest.UserMetaVO vo = new AnalyticsRequest.UserMetaVO();
            vo.payload.appId = GamebaseUnitySDK.AppID;
            vo.parameter.userId = Gamebase.GetUserID();

            switch (type)
            {
                case RequestType.USER_DATA:
                    {
                        vo.payload.userMetaData = MakeUserMetaData();
                        break;
                    }
                case RequestType.LEVEL_UP:
                    {
                        vo.payload.userMetaData = MakeLevelUpData();
                        break;
                    }
            }

            var requestVO = new WebSocketRequest.RequestVO(PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID);
            requestVO.apiId = ID.SET_USER_META;
            requestVO.payload = JsonMapper.ToJson(vo.payload);
            requestVO.parameters = vo.parameter;

            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                if (Gamebase.IsSuccess(error) == false)
                {
                    GamebaseLog.Warn(
                        string.Format(
                            "{0}\n{1}",
                            "Failed request.",
                            GamebaseJsonUtil.ToPrettyJsonString(error)),
                        this,
                        "SetUserMeta");
                    callback(error);
                    return;
                }
                callback(error);
            });
        }

        private Dictionary<string, object> MakeUserMetaData()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add(KEY_USER_LEVEL, UserLevel);

            if (string.IsNullOrEmpty(ChannelId) == false)
            {
                dict.Add(KEY_CHANNEL_ID, ChannelId);
            }

            if (string.IsNullOrEmpty(CharacterId) == false)
            {
                dict.Add(KEY_CHARACTER_ID, CharacterId);
            }

            if (string.IsNullOrEmpty(CharacterClassId) == false)
            {
                dict.Add(KEY_CLASS_ID, CharacterClassId);
            }

            return dict;
        }

        private Dictionary<string, object> MakeLevelUpData()
        {
            Dictionary<string, object> dict = MakeUserMetaData();

            dict.Add(KEY_LEVEL_UP_TIME, LevelUpTime);

            return dict;
        }

        public void ResetUserMeta(GamebaseCallback.VoidDelegate callback)
        {
            UserLevel = 0;
            LevelUpTime = -1;
            IdPCode = string.Empty;
            ChannelId = string.Empty;
            CharacterId = string.Empty;
            CharacterClassId = string.Empty;
            callback();
        }
    }
}
#endif