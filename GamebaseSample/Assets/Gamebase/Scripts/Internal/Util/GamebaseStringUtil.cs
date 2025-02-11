﻿
namespace Toast.Gamebase.Internal
{
    public static class GamebaseStringUtil
    {
        public static string Capitalize(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            return char.ToUpper(text[0]) +
                   ((text.Length > 1) ? text.Substring(1) : string.Empty);
        }
    }
}