using System.Globalization;

namespace Toast.Gamebase.Internal
{
    public static class GamebaseCultureUtil
    {   
        public static string GetTwoLetterCountryCode(int locale)
        {
            CultureInfo currentCulture = new CultureInfo(locale);
            string currentCultureName = currentCulture.Name;
            int index = currentCultureName.IndexOf('-');

            string countryCode = null;
            
            if (0 < index && index + 1 < currentCultureName.Length)
            {
                countryCode = currentCultureName.Substring(index + 1);
            }

            return countryCode ?? "ZZ";
        }
        
        public static string GetTwoLetterIsoCode(int locale)
        {
            CultureInfo currentCulture = new CultureInfo(locale);
            
            string language = currentCulture.TwoLetterISOLanguageName;
            if (string.IsNullOrEmpty(language))
            {
                language = "zz";
            }

            return language;
        }
    }
}