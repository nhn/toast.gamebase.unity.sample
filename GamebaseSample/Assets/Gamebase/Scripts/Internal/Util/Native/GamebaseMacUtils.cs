#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

using System;
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
            get { return NativeBridge.GetRegionCode(); }
        }

        string IGamebaseNativeUtils.TwoLetterIsoCode
        {
            get { return NativeBridge.GetLanguageCode(); }
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
            private static extern IntPtr _GetRegionCode();

            public static string GetRegionCode()
            {
                IntPtr result = _GetRegionCode();

                string region = IntPtr.Zero != result ? Marshal.PtrToStringAnsi(result) : "";
                if(string.IsNullOrEmpty(region))
                    region = "ZZ";
                
                return region;
            }
            
            [DllImport(BUNDLE_NAME)]
            private static extern IntPtr _GetLanguageCode();
            
            public static string GetLanguageCode()
            {
                IntPtr result = _GetLanguageCode();
                string language = IntPtr.Zero != result ? Marshal.PtrToStringAnsi(result) : "";
                if(string.IsNullOrEmpty(language))
                    language = "zz";
                
                return language;
            }
            
            [DllImport(BUNDLE_NAME)]
            public static extern uint GetWindowsLocaleCode();
        }
    }
}

#endif