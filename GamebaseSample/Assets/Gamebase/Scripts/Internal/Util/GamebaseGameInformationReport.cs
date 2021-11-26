using System.Collections.Generic;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public sealed class GamebaseGameInformationReport : MonoBehaviour
    {
        #region PlayerPrefs key
        private const string GAMEBASE_KEY_USED_API_LIST = "GAMEBASE_KEY_USED_API_LIST";
        #endregion

        #region Indicatot report body key
        public const string GB_GAME_INFORMATION = "GB_GAME_INFORMATION";
        #endregion

        #region Indicatot report additional key
        private const string GB_LAUNCHING_ZONE = "GBLaunchingZone";
        private const string GB_PLATFORM = "GBPlatform";
        private const string GB_PLATFORM_SDK_VERSION = "GBPlatformSDKVersion";
        private const string GB_PLATFORM_VERSION = "GBPlatformVersion";
        private const string GB_ENGINE_TYPE = "GBEngineType";
        private const string GB_ENGINE_SDK_VERSION = "GBEngineSDKVersion";
        private const string GB_ENGINE_VERSION = "GBEngineVersion";
        private const string GB_COUNTRY_CODE = "GBCountryCode";
        private const string GB_USED_API_LIST = "txtGBUsedApiList";
        #endregion

        private static GamebaseGameInformationReport instance;

        public static GamebaseGameInformationReport Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = GamebaseComponentManager.AddComponent<GamebaseGameInformationReport>(GamebaseGameObjectManager.GameObjectType.INDICATOR_REPORT_TYPE);
                }

                return instance;
            }
        }

        private HashSet<string> usedApiList = new HashSet<string>();

        public void AddApiName([System.Runtime.CompilerServices.CallerMemberName] string apiName = "")
        {
            usedApiList.Add(apiName);
        }
        
        public void SendGameInformation()
        {
            Send();
            DeleteUsedApiList();
        }

        private void Send()
        {
            string userApiListJsonString = LoadUsedApiList();

            if (string.IsNullOrEmpty(userApiListJsonString) == true)
            {
                return;
            }
            
            GamebaseInternalReport.Instance.SendIndicatorReport(
                GamebaseInternalReport.IndicatorReport.LevelType.INDICATOR_REPORT,
                GB_GAME_INFORMATION,
                new Dictionary<string, string>
                {
                    { GB_ENGINE_TYPE,             "Unity" },
                    { GB_ENGINE_VERSION,          Application.unityVersion },
                    { GB_ENGINE_SDK_VERSION,      GamebaseUnitySDK.SDKVersion },
                    { GB_COUNTRY_CODE,            GamebaseImplementation.Instance.GetCountryCode() },
                    { GB_PLATFORM,                Application.platform.ToString() },
                    { GB_PLATFORM_VERSION,        SystemInfo.operatingSystem },                    
                    { GB_PLATFORM_SDK_VERSION,    GamebaseImplementation.Instance.GetSDKVersion() },
                    { GB_LAUNCHING_ZONE,          GamebaseUnitySDK.ZoneType },
                    { GB_USED_API_LIST,           userApiListJsonString }
                });            
        }

        private void SaveUsedApiList()
        {
            if (usedApiList == null || usedApiList.Count == 0)
            {
                return;
            }

            List<string> data = new List<string>(usedApiList);

            PlayerPrefs.SetString(GAMEBASE_KEY_USED_API_LIST, JsonMapper.ToJson(data));
        }

        private string LoadUsedApiList()
        {
            return PlayerPrefs.GetString(GAMEBASE_KEY_USED_API_LIST, string.Empty);
        }

        private void DeleteUsedApiList()
        {
            PlayerPrefs.DeleteKey(GAMEBASE_KEY_USED_API_LIST);
        }

        private void OnDestroy()
        {
            SaveUsedApiList();
        }
    }
}
