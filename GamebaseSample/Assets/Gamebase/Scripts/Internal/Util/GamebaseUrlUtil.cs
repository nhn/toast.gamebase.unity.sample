using System;
using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public class GamebaseUrlUtil
    {
        public class SchemeInfo
        {
            public string scheme;
            public Dictionary<string, string> parameterDictionary;
        }

        public static SchemeInfo ConvertURLToSchemeInfo(string url)
        {
            string[] urlParameters = url.Split(new char[] { '?', '&' });
            if (urlParameters == null || urlParameters.Length == 0)
                return null;

            SchemeInfo schemeInfo = new SchemeInfo();
            schemeInfo.scheme = Uri.UnescapeDataString(urlParameters[0]);

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            for (int i = 1; i < urlParameters.Length; i++)
            {
                string urlParameter = urlParameters[i];
                string[] parameter = urlParameter.Split('=');

                if (parameter == null || parameter.Length <= 1)
                {
                    continue;
                }

                string key = Uri.UnescapeDataString(parameter[0]);
                string value = Uri.UnescapeDataString(parameter[1]);

                if (parameters.ContainsKey(key) == true)
                {
                    continue;
                }

                parameters.Add(key, value);
            }

            schemeInfo.parameterDictionary = parameters;

            return schemeInfo;
        }
    }
}
