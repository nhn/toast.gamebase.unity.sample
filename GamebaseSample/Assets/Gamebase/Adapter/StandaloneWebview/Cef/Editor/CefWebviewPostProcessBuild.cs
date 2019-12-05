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

    [PostProcessBuild(110)]
    public static void CopyCefResources(BuildTarget target, string path)
    {
        string[] pathData = path.Split('/');
        string exeName = pathData[pathData.Length - 1].Replace(".exe", "");
        string exeRootPath = path.Replace(pathData[pathData.Length - 1], "");
        string exeDataPath = string.Format("{0}{1}_Data/Plugins", exeRootPath, exeName);

        var pluginPath = new StringBuilder(Application.dataPath).Append("/Gamebase/Adapter/StandaloneWebview/Cef/Plugins/");

        if (target == BuildTarget.StandaloneWindows64)
        {
            pluginPath.Append("x86_64");
        }
        else
        {
            pluginPath.Append("x86");
        }

        Debug.Log(string.Format("pluginPath:{0}", pluginPath));

        foreach (string resource in resources)
        {
            FileUtil.CopyFileOrDirectory(string.Format("{0}/{1}", pluginPath, resource), string.Format("{0}/{1}", exeDataPath, resource));
        }
    }
}

#endif