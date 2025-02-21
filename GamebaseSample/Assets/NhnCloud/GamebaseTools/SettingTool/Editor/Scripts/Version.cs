using System.Text.RegularExpressions;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class VersionUtility
    {
        static public bool IsInvalidVersionString(string str)
        {
            if(string.IsNullOrEmpty(str) == true)
            {
                return true;
            }

            // Major.Minor.Patch
            Regex VersionMatcher = new Regex(@"^[0-9]+\.[0-9]+\.[0-9]+");
            return VersionMatcher.IsMatch(str) == false;
        }

        static public int CompareVersion(string currentVersionString, string targetVersionString)
        {
            int[] currentVersionArray = ConvertVersionStringToIntArray(currentVersionString);
            int[] targetVersionArray  = ConvertVersionStringToIntArray(targetVersionString);
            
            int count = currentVersionArray.Length;
            if (count > targetVersionArray.Length)
            {
                count = targetVersionArray.Length;
            }
            
            for (int index = 0; index < count; index++)
            {
                if (currentVersionArray[index] != targetVersionArray[index])
                {
                    if (currentVersionArray[index] > targetVersionArray[index])
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            if (currentVersionArray.Length == targetVersionArray.Length)
            {
                return 0;
            }
            else if (currentVersionArray.Length > targetVersionArray.Length)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        
        static public bool IsUpdateRequired(string currentVersionString, string targetVersionString)
        {
            int[] currentVersionArray = ConvertVersionStringToIntArray(currentVersionString);
            int[] targetVersionArray  = ConvertVersionStringToIntArray(targetVersionString);

            bool isUpdateRequired = false;
            for (int index = 0; index < currentVersionArray.Length; index++)
            {
                if (currentVersionArray[index] != targetVersionArray[index])
                {
                    isUpdateRequired = currentVersionArray[index] < targetVersionArray[index];
                    break;
                }
            }
            return isUpdateRequired;
        }


        static public bool IsUpdateRequired(params string[][] versions)
        {
            bool isUpdateRequired = false;
            foreach (string[] versionPair in versions)
            {
                if (versionPair.Length != 2)
                {
                    continue;
                }
                string current = versionPair[0];
                string target  = versionPair[1];
                isUpdateRequired = isUpdateRequired || IsUpdateRequired(current, target);
            }
            return isUpdateRequired;
        }

        static private int[] ConvertVersionStringToIntArray(string version)
        {
            if (string.IsNullOrEmpty(version) == false)
            {
                string[] splitedVersionString = version.Split('.', 'f');

                if (splitedVersionString.Length > 0)
                {
                    int[] versions = new int[splitedVersionString.Length];
                    for (int i = 0; i < versions.Length; i++)
                    {
                        if (int.TryParse(splitedVersionString[i], out versions[i]) == false)
                        {
                            versions[i] = 0;
                        }
                    }

                    return versions;
                }
            }

            return new int[0];
        }

        static public bool IsInvalidParameter(params string[] inputs)
        {
            if (inputs == null)
            {
                return true;
            }

            foreach (string input in inputs)
            {
                if (IsInvalidVersionString(input) == true)
                {
                    return true;
                }
            }

            return false;
        }
    }
}