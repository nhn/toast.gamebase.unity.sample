using System;
using System.Globalization;

namespace Toast.Gamebase.Internal
{
    public static class GamebaseCultureUtil
    {   
        public static string GetTwoLetterCountryCode(int locale)
        {
            string countryCode = null;
            try
            {
                CultureInfo currentCulture = new CultureInfo(locale);
                string currentCultureName = currentCulture.Name;
                int index = currentCultureName.IndexOf('-');
            
                if (0 < index && index + 1 < currentCultureName.Length)
                {
                    countryCode = currentCultureName.Substring(index + 1);
                }
            }
            catch
            {
            }

            return countryCode ?? "ZZ";
        }
        
        public static string GetTwoLetterIsoCode(int locale)
        {
            string language = string.Empty;
            try
            {
                CultureInfo currentCulture = new CultureInfo(locale);
                language = currentCulture.TwoLetterISOLanguageName;
            }
            catch
            {
            }
            
            if (string.IsNullOrEmpty(language))
            {
                language = "zz";
            }

            return language;
        }
    }
}