#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace Toast.Gamebase.Internal.Single
{
    public class DisplayLanguage
    {
        private static readonly DisplayLanguage instance = new DisplayLanguage();

        public static DisplayLanguage Instance
        {
            get { return instance; }
        }

        private Dictionary<string, LocalizedStringVO> localizedStrings;
        private string jsonString;

        public DisplayLanguage()
        {
        }

        public IEnumerator DisplayLanguageInitialize()
        {
            if (null == localizedStrings)
            {
                yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.DISPLAY_LANGUAGE_TYPE, LoadLocalizedString());
            }
        }

        public bool HasLocalizedStringVO(string displayLanguageCode)
        {
            if (null == localizedStrings)
            {
                return false;
            }

            return localizedStrings.ContainsKey(displayLanguageCode);
        }

        public string GetString(string key)
        {
            if (null == localizedStrings)
            {
                return string.Empty;
            }

            FieldInfo fieldInfo = typeof(LocalizedStringVO).GetField(key);

            if (null == fieldInfo)
            {
                GamebaseLog.Error(string.Format("`{0}` {1}", key, GamebaseStrings.LOCALIZED_STRING_KEY_NOT_SUPPORTED), this);
                return string.Empty;
            }

            if (true == string.IsNullOrEmpty((string)fieldInfo.GetValue(GetLocalizedStringVO())))
            {
                if (true == HasKey(key))
                {
                    return string.Empty;
                }
                else
                {
                    if (true == string.IsNullOrEmpty((string)fieldInfo.GetValue(GetDefaultLocalizedStringVO())))
                    {
                        GamebaseLog.Warn(string.Format("`{0}` {1}", key, GamebaseStrings.LOCALIZED_STRING_KEY_NOT_FOUND), this);
                        return string.Empty;
                    }
                    else
                    {
                        return fieldInfo.GetValue(GetDefaultLocalizedStringVO()).ToString();
                    }
                }
            }
            else
            {
                return fieldInfo.GetValue(GetLocalizedStringVO()).ToString();
            }
        }

        private bool HasKey(string key)
        {
            if (true == HasLocalizedStringVO(GamebaseUnitySDK.DisplayLanguageCode))
            {
                var JsonData = JsonMapper.ToObject(jsonString);
                return JsonData[GamebaseUnitySDK.DisplayLanguageCode].Keys.Contains(key);
            }

            return false;
        }

        #region Load LocalizedString.json
        private IEnumerator LoadLocalizedString()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Gamebase/localizedstring.json");
            
            if (true == IsWebFilePath(filePath))
            {
                yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.DISPLAY_LANGUAGE_TYPE, LoadWebFile(filePath));
            }
            else
            {
                LoadLocalFile(filePath);
            }
        }

        private bool IsWebFilePath(string filePath)
        {
            return (true == filePath.Contains("://")) || (true == filePath.Contains(":///"));
        }
        
        private IEnumerator LoadWebFile(string filePath)
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            www.timeout = CommunicatorConfiguration.timeout;

            yield return UnityCompatibility.UnityWebRequest.Send(www);

            if (true == www.isDone)
            {
                if (200 == www.responseCode)
                {
                    if (true == UnityCompatibility.UnityWebRequest.IsError(www))
                    {
                        GamebaseLog.Warn(string.Format("error:{0}", www.error), this);
                    }
                    else
                    {
                        if (true == string.IsNullOrEmpty(www.downloadHandler.text))
                        {
                            GamebaseLog.Warn(GamebaseStrings.LOCALIZED_STRING_EMPTY, this);
                            yield break;
                        }

                        jsonString = www.downloadHandler.text;
                        localizedStrings = JsonMapper.ToObject<Dictionary<string, LocalizedStringVO>>(jsonString);
                        GamebaseLog.Debug(GamebaseStrings.LOCALIZED_STRING_LOAD_SUCCEEDED, this);
                    }
                }
                else
                {
                    if (404 == www.responseCode)
                    {
                        GamebaseLog.Warn(string.Format("{0} responseCode:{1}.", GamebaseStrings.LOCALIZED_STRING_NOT_FOUND, www.responseCode), this);
                    }
                    else
                    {
                        GamebaseLog.Warn(string.Format("{0} responseCode:{1}.", GamebaseStrings.LOCALIZED_STRING_LOAD_FAILED, www.responseCode), this);
                    }
                }
            }
        }

        private void LoadLocalFile(string filePath)
        {
            if (true == File.Exists(filePath))
            {
                jsonString = File.ReadAllText(filePath);

                if (true == string.IsNullOrEmpty(jsonString))
                {
                    GamebaseLog.Warn(GamebaseStrings.LOCALIZED_STRING_EMPTY, this);
                }
                else
                {
                    localizedStrings = JsonMapper.ToObject<Dictionary<string, LocalizedStringVO>>(jsonString);
                }
            }
            else
            {
                GamebaseLog.Warn(GamebaseStrings.LOCALIZED_STRING_NOT_FOUND, this);
            }
        }
        #endregion

        private LocalizedStringVO GetLocalizedStringVO()
        {
            if (true == HasLocalizedStringVO(GamebaseUnitySDK.DisplayLanguageCode))
            {
                return localizedStrings[GamebaseUnitySDK.DisplayLanguageCode];
            }
            else
            {
                return GetDefaultLocalizedStringVO();
            }
        }

        private LocalizedStringVO GetDefaultLocalizedStringVO()
        {
            if (true == HasLocalizedStringVO(GamebaseDisplayLanguageCode.English))
            {
                return localizedStrings[GamebaseDisplayLanguageCode.English];
            }
            else
            {
                return null;
            }
        }
    }
}
#endif