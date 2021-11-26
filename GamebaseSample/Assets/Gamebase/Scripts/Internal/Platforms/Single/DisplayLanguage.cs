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
            if (localizedStrings == null)
            {
                yield return GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.DISPLAY_LANGUAGE_TYPE, LoadLocalizedString());
            }
        }

        public bool HasLocalizedStringVO(string displayLanguageCode)
        {
            if (localizedStrings == null)
            {
                return false;
            }

            return localizedStrings.ContainsKey(displayLanguageCode);
        }

        public string GetString(string key)
        {
            if (localizedStrings == null)
            {
                return string.Empty;
            }

            FieldInfo fieldInfo = typeof(LocalizedStringVO).GetField(key);

            if (fieldInfo == null)
            {
                // Not found in LocalizedStringVO
                GamebaseLog.Error(string.Format("`{0}` {1}", key, GamebaseStrings.LOCALIZED_STRING_KEY_NOT_SUPPORTED), this);
                return string.Empty;
            }

            if (HasKey(GamebaseUnitySDK.DisplayLanguageCode, key) == true)
            {
                return fieldInfo.GetValue(GetLocalizedStringVO()).ToString();
            }
            else
            {
                if (HasKey(GamebaseDisplayLanguageCode.English, key) == true)
                {
                    return fieldInfo.GetValue(GetDefaultLocalizedStringVO()).ToString();
                }
                else
                {
                    return key;
                }
            }
        }

        private bool HasKey(string displayLanguageCode, string key)
        {
            if (HasLocalizedStringVO(displayLanguageCode) == true)
            {
                var JsonData = JsonMapper.ToObject(jsonString);
                return JsonData[displayLanguageCode].Keys.Contains(key);
            }

            return false;
        }

        #region Load LocalizedString.json
        private IEnumerator LoadLocalizedString()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Gamebase/localizedstring.json");
            
            if (IsWebFilePath(filePath) == true)
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

            yield return UnityCompatibility.WebRequest.Send(www);

            if (www.isDone == true)
            {
                if (www.responseCode == 200)
                {
                    if (UnityCompatibility.WebRequest.IsError(www) == true)
                    {
                        GamebaseLog.Warn(string.Format("error:{0}", www.error), this);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(www.downloadHandler.text) == true)
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
            if (File.Exists(filePath) == true)
            {
                jsonString = File.ReadAllText(filePath);

                if (string.IsNullOrEmpty(jsonString) == true)
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
            if (HasLocalizedStringVO(GamebaseUnitySDK.DisplayLanguageCode) == true)
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
            if (HasLocalizedStringVO(GamebaseDisplayLanguageCode.English) == true)
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