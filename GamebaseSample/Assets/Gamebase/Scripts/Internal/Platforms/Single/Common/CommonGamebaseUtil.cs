#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebaseUtil : IGamebaseUtil
    {
        private string domain;

        public string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(domain))
                    return typeof(CommonGamebaseUtil).Name;

                return domain;
            }
            set
            {
                domain = value;
            }
        }

        public virtual void ShowAlert(string title, string message)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
        }

        public virtual void ShowAlert(string title, string message, int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this, GamebaseCallbackHandler.GetCallback<GamebaseCallback.VoidDelegate>(handle));
        }        
        
        public void ShowToast(string message, GamebaseUIToastType type)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
        }

        public virtual void ShowAlert(Dictionary<string, string> parameters, GamebaseUtilAlertType alertType, int handle)
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
        }

        protected string GetDictionaryValue(Dictionary<string, string> dictionary, string key)
        {
            string value = string.Empty;
            if (true == dictionary.ContainsKey(key))
            {
                value = dictionary[key];
            }
            return value;
        }
    }
}
#endif