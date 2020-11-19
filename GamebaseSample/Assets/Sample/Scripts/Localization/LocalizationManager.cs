using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Toast.Gamebase;
using UnityEngine;
using UnityEngine.Networking;

namespace GamebaseSample
{
    public class LocalizationManager
    {
        private const string LOCALIZED_STRING_EMPTY = "The localizedstring is empty";
        private const string LOCALIZED_STRING_NOT_FOUND = "The localizedstring file not found";

        private const string FILE_NAME = "Sample/localizedString.json";

        private static readonly LocalizationManager instance = new LocalizationManager();

        private HashSet<System.Action> updateTextEvents = new HashSet<System.Action>();
        private Dictionary<string, string> localizedString = null;

        public static LocalizationManager Instance
        {
            get { return instance; }
        }

        public IEnumerator LoadLocalizedStrings(MonoBehaviour mono, System.Action callback)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, FILE_NAME);

            Logger.Debug(string.Format("filePath:{0}", filePath), this);

            if (IsWebFilePath(filePath) == true)
            {
                yield return mono.StartCoroutine(LoadWebFile(filePath, LoadJsonString));
            }
            else
            {
                LoadLocalFile(filePath, LoadJsonString);
            }

            callback();
        }

        private void LoadJsonString(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString) == true)
            {
                return;
            }

            string languageCode = Gamebase.GetDisplayLanguageCode();

            Dictionary<string, Dictionary<string, string>> jsonData = JsonMapper.ToObject<Dictionary<string, Dictionary<string, string>>>(jsonString);

            if (jsonData != null)
            {
                if (jsonData.ContainsKey(languageCode) == true)
                {
                    localizedString = jsonData[languageCode];
                    if (localizedString != null)
                    {
                        List<string> stringKeys = new List<string>(localizedString.Keys);
                        foreach (string key in stringKeys)
                        {
                            localizedString[key] = localizedString[key].Replace("\\n", "\n");
                        }
                    }
                }
            }
        }

        public string GetLocalizedValue(string key)
        {
            if (localizedString != null && localizedString.ContainsKey(key) == true)
            {
                return localizedString[key];
            }

            return key;
        }

        public void AddUpdateTextEvent(System.Action updateTextEvent)
        {
            updateTextEvents.Add(updateTextEvent);
        }

        public void RemoveUpdateTextEvent(System.Action updateTextEvent)
        {
            updateTextEvents.Remove(updateTextEvent);
        }

        public void RemoveAllUpdateTextEvent(System.Action updateTextEvent)
        {
            updateTextEvents.Clear();
        }

        public void UpdateText()
        {
            if (updateTextEvents == null || updateTextEvents.Count == 0)
            {
                return;
            }

            foreach (var updateTextEvent in updateTextEvents)
            {
                updateTextEvent();
            }
        }

        private bool IsWebFilePath(string filePath)
        {
            return (filePath.Contains("://") == true) || (filePath.Contains(":///") == true);
        }

        private IEnumerator LoadWebFile(string filePath, System.Action<string> callback)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            WWW www = new WWW(filePath);
            yield return www;
            callback(www.text);
#else
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            www.timeout = 10;

            yield return UnityCompatibility.UnityWebRequest.Send(www);

            if (www.isDone == true)
            {
                Logger.Debug(string.Format("text:{0}", www.downloadHandler.text), this);

                if (www.responseCode == 200)
                {
                    if (UnityCompatibility.UnityWebRequest.IsError(www) == true)
                    {
                        Logger.Debug(string.Format("error:{0}", www.error), this);
                        callback(null);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(www.downloadHandler.text) == true)
                        {
                            Logger.Debug(string.Format("error:{0}", LOCALIZED_STRING_EMPTY), this);
                            callback(null);
                            yield break;
                        }

                        callback(www.downloadHandler.text);
                    }
                }
                else
                {
                    Logger.Debug(string.Format("{0}. responseCode:{1}", LOCALIZED_STRING_NOT_FOUND, www.responseCode), this);
                    callback(null);
                }
            }
#endif
        }

        private void LoadLocalFile(string filePath, System.Action<string> callback)
        {
            if (File.Exists(filePath) == false)
            {
                Logger.Debug(LOCALIZED_STRING_NOT_FOUND, this);

                callback(null);
            }

            callback(File.ReadAllText(filePath));
        }
    }
}