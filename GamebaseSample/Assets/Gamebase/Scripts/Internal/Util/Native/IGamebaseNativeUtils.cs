using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public class GamebasePopupInfo
    {
        public readonly string Title;
        public readonly string Message;
        public readonly List<string> Buttons;
        
        public GamebasePopupInfo(string title, string message, List<string> buttons = null)
        {
            Title = title;
            Message = message;
            
            if (buttons == null || buttons.Count == 0)
            {
                Buttons = new List<string> { "OK" };
            }
            else
            {
                Buttons = buttons;
            }
        }
    }
    
    public interface IGamebaseNativeUtils
    {
        bool IsNetworkConnected { get; }
        
        string DeviceUniqueIdentifier { get; }

        string TwoLetterCountryCode { get; } 
        string TwoLetterIsoCode { get; } 
        
        int ShowPopup(GamebasePopupInfo info);
    }
}