using NhnCloud.GamebaseTools.SettingTool.Util.Ad.Internal;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Util.Ad
{
    public static class Advertisement
    {
        public static void Initialize(AdvertisementConfigurations advertisementInfo, string languageCode)
        {
            AdvertisementImplementation.Instance.Initialize(advertisementInfo, languageCode);
        }

        public static void Draw(Rect drawRect)
        {
            AdvertisementImplementation.Instance.Draw(drawRect);
        }
        
        public static void SetSelectAdvertisementInfoCallback(System.Action<string, string> selectAdvertisementInfoCallback)
        {
            AdvertisementImplementation.Instance.SetSelectAdvertisementInfoCallback(selectAdvertisementInfoCallback);
        }

        public static void SetLanguageCode(string languageCode)
        {
            AdvertisementImplementation.Instance.SetLanguageCode(languageCode);
        }

        public static void Destroy()
        {
            AdvertisementImplementation.Instance.OnDestroy();
        }
    }
}
