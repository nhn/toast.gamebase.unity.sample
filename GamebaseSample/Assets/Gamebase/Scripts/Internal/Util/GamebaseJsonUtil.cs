using System;
using System.Collections.Generic;
using System.IO;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal
{
    public static class GamebaseJsonUtil
    {
        private const string MESSAGE_EMPTY_DATA = "The data is empty.";

        public static string ToPretty(object jsonObject)
        {
            if (jsonObject == null)
            {
                GamebaseLog.Warn(MESSAGE_EMPTY_DATA, typeof(GamebaseJsonUtil));
                return string.Empty;
            }

            jsonObject = JsonMapper.ToObject(MaskingBlackList(JsonMapper.ToJson(jsonObject)));

            string prettyJson;

            using (var stringWriter = new StringWriter())
            {
                var jsonWriter = new JsonWriter(stringWriter)
                {
                    PrettyPrint = true
                };

                JsonMapper.ToJson(jsonObject, jsonWriter);
                prettyJson = stringWriter.ToString();
            }

            return prettyJson;
        }

        public static string ToPretty(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                GamebaseLog.Warn(MESSAGE_EMPTY_DATA, typeof(GamebaseJsonUtil));
                return string.Empty;
            }

            return ToPretty(JsonMapper.ToObject(jsonString));
        }

        public static string MaskingBlackList(string jsonString)
        {
            if (securityBlacklist == null)
            {
                securityBlacklist = defaultBlackList;
            }

            try
            {
                var jsonData = JsonMapper.ToObject(jsonString);
                Masking(ref jsonData);

                return jsonData.ToJson();
            }
            catch (Exception e)
            {
                GamebaseLog.Warn(string.Format(MESSAGE_MASKING_FAILED, e), typeof(GamebaseJsonUtil));
                return jsonString;
            }
        }

        /// <summary>
        /// The BlackList is set before SDK initialization, and additional BlackList are set afterwards via this function.
        ///
        /// - The BlackList is added after initialization.
        /// - If initialization fails, information from the defaultStability.json file is added.
        /// </summary>
        public static void AddBlackList(List<string> blackList)
        {
            if (blackList == null || blackList.Count == 0)
            {
                return;
            }

            securityBlacklist.AddRange(blackList);
        }

        #region MaskingBlackList
        /// <summary>
        /// A list of blacklists that need to be additionally processed within the SDK.
        /// </summary>
        private static readonly List<string> defaultBlackList = new List<string>()
        {
            "accessToken",
            "X-TCGB-Access-Token",
            "authorizationCode",
            "clientId",
            "iosClientId",
            "clientSecret",
            "uuid",
            "facebook_client_token",
            "authKey",
            "appKeyIndicator",
            "appKeyLog",
            "appKey"
        };

        private const string TEXT_ELLIPSIS = "...";
        private const string MESSAGE_MASKING_FAILED = "An error occurred during data masking processing. error:{0}";

        private static List<string> securityBlacklist;

        private static void Masking(ref JsonData data)
        {
            JsonData item;

            foreach (var blackList in securityBlacklist)
            {
                if (data.ContainsKey(blackList))
                {
                    item = data[blackList];

                    if (IsValidStringValue(item))
                    {
                        data[blackList] = TEXT_ELLIPSIS;
                    }
                }
            }

            // Using the foreach loop will result in an InvalidOperationException error, so implement it with a for loop.
            // - System.InvalidOperationException: Collection was modified; enumeration operation may not execute.
            var keyList = new List<string>(data.Keys);

            for (var i = 0; i < keyList.Count; i++)
            {
                item = data[keyList[i]];

                if (item == null)
                {
                    break;
                }

                if (IsValidStringValue(item))
                {
                    data[keyList[i]] = ParseStringTypeObject(item);
                }

                if (item.IsObject)
                {
                    Masking(ref item);
                }

                if (item.IsArray)
                {
                    ParseArrayTypeObject(item);
                }
            }
        }

        private static bool IsValidStringValue(JsonData item)
        {
            if (item == null)
            {
                return false;
            }

            if (item.IsString == false)
            {
                return false;
            }

            if (string.IsNullOrEmpty(item.ToString()) == true)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Parses a JSON Object of string type.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static string ParseStringTypeObject(JsonData item)
        {
            try
            {
                var stringTypeObject = JsonMapper.ToObject(item.ToString());
                if (stringTypeObject.IsObject)
                {
                    Masking(ref stringTypeObject);
                    return stringTypeObject.ToJson();
                }
            }
            catch (Exception e)
            {
                if (e is JsonException)
                {
                    // It is not a JSON object type.
                    return item.ToString();
                }

                GamebaseLog.Warn(string.Format(MESSAGE_MASKING_FAILED, e), typeof(GamebaseJsonUtil));
            }

            return item.ToString();
        }

        /// <summary>
        /// Parses objects inside an Array.
        /// </summary>
        /// <param name="itemArray"></param>
        private static void ParseArrayTypeObject(JsonData itemArray)
        {
            JsonData item;

            // Using the foreach loop will result in an InvalidOperationException error, so implement it with a for loop.
            // - CS 1657: Cannot use "item" as a ref or out value because it is a "foreach iteration variable"

            for (var i = 0; i < itemArray.Count; i++)
            {
                item = itemArray[i];

                if (item.IsObject)
                {
                    Masking(ref item);
                }
            }
        }
        #endregion
    }
}