using LitJson;
using System;
using Toast.Gamebase;
using UnityEngine;

namespace GamebaseSample
{
    public static class DataManager
    {
        private const string LAUNCHING_URI = "https://api-lnc.cloud.toast.com/launching/v3.0";
        private const string LAUNCHING_APP_KEY = "8EmpQanVc5R1nABi";

        private const string LANGUAGE_SAVE_KEY = "toast.gbsample.language";
        private const string GAMEBASE_DEBUG_MODE_KEY = "toast.gbsample.gamebase.debugmode";

        private static readonly UserData user = new UserData();

        public static UserData User
        {
            get { return user; }
        }

        public static string LanguageCode
        {
            get
            {
                if (PlayerPrefs.HasKey(LANGUAGE_SAVE_KEY) == false)
                {
                    string language = string.Empty;
                    switch (Application.systemLanguage)
                    {
                        case SystemLanguage.Korean:
                            {
                                language = GameConstants.LANGUAGE_KOREAN;
                                break;
                            }
                        case SystemLanguage.Japanese:
                            {
                                language = GameConstants.LANGUAGE_JAPANESE;
                                break;
                            }
                        default:
                            {
                                language = GameConstants.LANGUAGE_ENGLISH;
                                break;
                            }
                    }

                    LanguageCode = language;

                    return language;
                }

                return PlayerPrefs.GetString(LANGUAGE_SAVE_KEY);
            }
            set
            {
                PlayerPrefs.SetString(LANGUAGE_SAVE_KEY, value);
            }
        }

        public static bool GamebaseDebugMode
        {
            get
            {
                if (PlayerPrefs.HasKey(GAMEBASE_DEBUG_MODE_KEY) == false)
                {
                    return true;
                }

                return PlayerPrefs.GetString(GAMEBASE_DEBUG_MODE_KEY, "TRUE").Equals("TRUE");
            }
            set
            {
                PlayerPrefs.SetString(GAMEBASE_DEBUG_MODE_KEY, value ? "TRUE" : "FALSE");
                ApplyDebugMode();
            }
        }

        public static LaunchingData.Sample Launching
        {
            get;
            private set;
        }

        public static void InitializeLaunching(Action<bool> callback)
        {
            SampleWebRequestObject.Instance.Get(
                string.Format("{0}/appkeys/{1}/configurations", LAUNCHING_URI, LAUNCHING_APP_KEY),
                (message) =>
                {
                    if (string.IsNullOrEmpty(message) == true)
                    {
                        callback(false);
                        return;
                    }

                    var vo = JsonMapper.ToObject<LaunchingVo>(message);
                    if (vo.header.isSuccessful == false)
                    {
                        callback(false);
                        return;
                    }

                    Launching = vo.launching.sample;

                    ApplyDebugMode();
                    callback(true);
                });
        }

        private static void ApplyDebugMode()
        {
            bool isDebugMode = GamebaseDebugMode;

            Gamebase.SetDebugMode(isDebugMode);
            Logger.SetDebugLog(isDebugMode);
        }
    }
}