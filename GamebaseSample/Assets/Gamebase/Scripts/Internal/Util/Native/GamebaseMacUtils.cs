#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

using System.Runtime.InteropServices;
using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public class GamebaseMacUtils : IGamebaseNativeUtils
    {
        public string DeviceUniqueIdentifier
        {
            get { return SystemInfo.deviceUniqueIdentifier; }
        }

        string IGamebaseNativeUtils.TwoLetterCountryCode
        {
            get { return GamebaseCultureUtil.GetTwoLetterCountryCode((int)NativeBridge.GetWindowsLocaleCode()); }
        }

        string IGamebaseNativeUtils.TwoLetterIsoCode
        {
            get { return GamebaseCultureUtil.GetTwoLetterIsoCode((int)NativeBridge.GetWindowsLocaleCode()); }
        }

        bool IGamebaseNativeUtils.IsNetworkConnected
        {
            get { return NativeBridge.IsInternetConnection(); }
        }
        
        public int ShowPopup(GamebasePopupInfo info)
        {
            return NativeBridge.ShowMessageBox(info.Title, info.Message, info.Buttons.ToArray(), info.Buttons.Count);
        }

        private static class NativeBridge
        {
            private const string BUNDLE_NAME = "GamebaseUtils";
        
            [DllImport(BUNDLE_NAME)]
            public static extern int ShowMessageBox(string title, string message, string[] buttons, int buttonCount);
        
            [DllImport(BUNDLE_NAME)]
            public static extern bool IsInternetConnection();
        
            [DllImport(BUNDLE_NAME)]
            public static extern uint GetWindowsLocaleCode();
        }
    }
}

#endif