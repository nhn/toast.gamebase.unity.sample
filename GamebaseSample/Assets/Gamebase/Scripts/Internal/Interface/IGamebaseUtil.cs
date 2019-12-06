using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public interface IGamebaseUtil
    {
        void ShowAlert(string title, string message);
        void ShowAlert(string title, string message, int handle);
        void ShowToast(string message, GamebaseUIToastType type);
        void ShowAlert(Dictionary<string, string> parameters, GamebaseUtilAlertType alertType, int handle);
    }
}