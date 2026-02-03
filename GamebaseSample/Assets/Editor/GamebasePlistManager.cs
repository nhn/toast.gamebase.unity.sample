#if UNITY_IOS

using GamebaseSample;
using System.Collections.Generic;
using System.IO;

using UnityEditor.iOS.Xcode;

public class GamebasePlistManager
{
	private const string KEY_URL_TYPES = "CFBundleURLTypes";
	private const string KEY_URL_NAME = "CFBundleURLName";
	private const string KEY_URL_SCHEMES = "CFBundleURLSchemes";
	private const string KEY_LS_APPLICATION_QUERIES_SCHEMES = "LSApplicationQueriesSchemes";

	private static GamebasePlistManager instance;

	public List<string> facebookWhiteList;

	private PlistDocument plist;
	private PlistElementDict rootDict;
	private PlistElementArray URLTypesArray;

	public static GamebasePlistManager GetInstance()
	{
		if (instance == null)
			instance = new GamebasePlistManager();

		return instance;
	}

    public void LoadPlist(string path)
    {
		if (plist != null)
			return;

        const string fileName = "Info.plist";
        string filePath = Path.Combine(path, fileName);

		plist = new PlistDocument();
		plist.ReadFromString(File.ReadAllText(filePath));

		rootDict = plist.root;

        if (null == rootDict)
        {
            Logger.Error(string.Format("Plist parsing error:{0}", filePath), this, "LoadPlist");
        }
    }

    public void SetIDPAppID(string IDPIdKey, string IDPId)
    {
		if (rootDict.values.ContainsKey (IDPIdKey)) {
			rootDict.values.Remove (IDPIdKey);
		}

		rootDict.SetString(IDPIdKey, IDPId);
    }

	public void SetWhiteList(string[] whiteList)
	{
		PlistElementArray LSApplicationQueriesSchemesArray;

		if (rootDict.values.ContainsKey(KEY_LS_APPLICATION_QUERIES_SCHEMES))
			LSApplicationQueriesSchemesArray = rootDict.values[KEY_LS_APPLICATION_QUERIES_SCHEMES].AsArray();
		else
			LSApplicationQueriesSchemesArray = rootDict.CreateArray(KEY_LS_APPLICATION_QUERIES_SCHEMES);
		
		for (int i = 0; i < whiteList.Length; i++)
		{
			if (HasStringValue(LSApplicationQueriesSchemesArray, whiteList[i]) == false)
				LSApplicationQueriesSchemesArray.AddString(whiteList[i]);
		}
	}

	public void SetList(string key, string[] list)
	{
		PlistElementArray array;

		if (rootDict.values.ContainsKey(key))
			array = rootDict.values[key].AsArray();
		else
			array = rootDict.CreateArray(key);

		for (int i = 0; i < list.Length; i++)
		{
			if (HasStringValue(array, list[i]) == false)
				array.AddString(list[i]);
		}
	}

	public void SetURLSchemes(string URLSchemesValue, bool hasURLIdentifier = false, string URLIdentifier = null)
    {
        if (rootDict.values.ContainsKey(KEY_URL_TYPES))
        {
			if (URLTypesArray == null)
				URLTypesArray = rootDict.values[KEY_URL_TYPES].AsArray();

			if (hasURLIdentifier)
				CreateURLSchemes(URLSchemesValue, hasURLIdentifier, URLIdentifier);
			else
				AddURLSchemes(URLSchemesValue);
        }
        else
        {
			URLTypesArray = rootDict.CreateArray(KEY_URL_TYPES);
			CreateURLSchemes(URLSchemesValue, hasURLIdentifier, URLIdentifier);
        }
    }
        
    public void SavePlist(string path)
    {
        const string fileName = "Info.plist";
        string filePath = Path.Combine(path, fileName);

		File.WriteAllText(filePath, plist.WriteToString());
    }

	private void CreateURLSchemes(string URLSchemesValue, bool hasURLIdentifier = false, string URLIdentifier = "")
	{
		for (var i = 0; i < URLTypesArray.values.Count; i++)
		{
			var dict = URLTypesArray.values[i].AsDict();
			if (string.IsNullOrEmpty(URLIdentifier) == false && dict.values.ContainsKey(KEY_URL_NAME))
			{
				if (dict.values[KEY_URL_NAME].AsString() == URLIdentifier)
					return;
			}
		}

		PlistElementDict URLTypesDict = URLTypesArray.AddDict();

		if (hasURLIdentifier)
			URLTypesDict.SetString(KEY_URL_NAME, URLIdentifier);

		PlistElementArray innerArray = URLTypesDict.CreateArray(KEY_URL_SCHEMES);
		innerArray.AddString(URLSchemesValue);
	}

	private void AddURLSchemes(string URLSchemesValue)
	{
		int i;

		for (i=0 ; i<URLTypesArray.values.Count ; i++)
		{
			if (URLTypesArray.values[i].AsDict().values.ContainsKey(KEY_URL_NAME))
				continue;
			else
			{
				PlistElementArray URLSchemesArray = URLTypesArray.values[i].AsDict()[KEY_URL_SCHEMES].AsArray();
				if (HasStringValue(URLSchemesArray, URLSchemesValue) == false)
					URLSchemesArray.AddString(URLSchemesValue);

				return;
			}
		}

		CreateURLSchemes(URLSchemesValue);
	}        

	private bool HasStringValue(PlistElementArray array, string value)
	{
		for (var i = 0; i < array.values.Count; i++)
		{
			if (array.values[i].AsString() == value)
				return true;
		}

		return false;
	}
}

#endif