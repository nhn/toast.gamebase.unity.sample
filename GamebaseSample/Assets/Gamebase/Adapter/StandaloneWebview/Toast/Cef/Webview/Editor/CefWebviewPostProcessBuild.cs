#if UNITY_STANDALONE_WIN

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static class CefWebviewPostProcessBuild
{
    private static readonly List<string> resources = new List<string>{
        // ----------------------------------------
        //  directory
        // ----------------------------------------
        "locales",
        // ----------------------------------------
        //  dat
        // ----------------------------------------        
        "icudtl.dat",
        // ----------------------------------------
        //  bin
        // ----------------------------------------
        "natives_blob.bin",
        "snapshot_blob.bin",
        // ----------------------------------------
        //  pak
        // ----------------------------------------
        "cef.pak",
        "cef_100_percent.pak",
        "cef_200_percent.pak",
        "cef_extensions.pak",
        "devtools_resources.pak"
    };

    private const string PROCESSOR_TYPE_32 = "x86";
    private const string PROCESSOR_TYPE_64 = "x86_64";

    [PostProcessBuild(110)]
    public static void CopyCefResources(BuildTarget target, string path)
    {
        string[] pathData = path.Split('/');
        string exeName = pathData[pathData.Length - 1].Replace(".exe", "");
        string exeRootPath = path.Replace(pathData[pathData.Length - 1], "");
        string exeDataPath = string.Format("{0}{1}_Data/Plugins", exeRootPath, exeName);

        var pluginPath = new StringBuilder(Application.dataPath).Append("/Gamebase/Adapter/StandaloneWebview/Toast/Cef/Webview/Plugins/");

        if (target == BuildTarget.StandaloneWindows64)
        {
            pluginPath.Append(PROCESSOR_TYPE_64);
            exeDataPath = GetExeDataPath(exeDataPath, PROCESSOR_TYPE_64);
        }
        else
        {
            pluginPath.Append(PROCESSOR_TYPE_32);
            exeDataPath = GetExeDataPath(exeDataPath, PROCESSOR_TYPE_32);
        }

        Debug.Log(string.Format("pluginPath:{0}", pluginPath));
        Debug.Log(string.Format("exeDataPath:{0}", exeDataPath));
        
        foreach (string resource in resources)
        {
            FileUtil.CopyFileOrDirectory(string.Format("{0}/{1}", pluginPath, resource), string.Format("{0}/{1}", exeDataPath, resource));
        }
    }

    private static string GetExeDataPath(string exeDataPath, string processorType)
    {
        StringBuilder cefFullFileName = new StringBuilder(exeDataPath);
        cefFullFileName.Append("/");
        cefFullFileName.Append(processorType);
        cefFullFileName.Append("/nhncef.dll");

        if (File.Exists(cefFullFileName.ToString()) == true)
        {
            return string.Concat(exeDataPath, "/", processorType);
        }
        else
        {
            return exeDataPath;
        }
    }
}

#endif