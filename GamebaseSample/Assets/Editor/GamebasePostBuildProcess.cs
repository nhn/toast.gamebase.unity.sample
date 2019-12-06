using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Collections.Generic;
using NE.XcodeEditor;

public class GamebasePostBuildProcess
{
    [PostProcessBuild(110)]
    public static void EditXcodeProject(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            string[] frameworks = new string[]
            {
                "GameKit.framework",
                "AdSupport.framework",
                "CoreTelephony.framework",
                "StoreKit.framework",
                "ImageIO.framework",
                "MobileCoreServices.framework"
            };

            string[] tbds = new string[]
            {
                "libicucore.tbd",
                "libsqlite3.tbd",
                "libz.tbd",
                "libstdc++.tbd"
            };

            string[] ldFlags = new string[]
            {
                "-ObjC"
            };

            string[] whiteList = new string[]
            {
                "fbapi",
                "fb-messenger-api",
                "fbauth2",
                "fbshareextension",
            };

            SetPBXProject(
                path + "/Unity-iPhone.xcodeproj/project.pbxproj",
                ldFlags,
                frameworks,
                tbds
            );
            SetPlistProperties(path, whiteList);
        }
    }

    public static void SetPBXProject(string path, string[] ldFlags, string[] frameworks, string[] tbds)
    {

        PBXProject proj = LoadPBXProject(path);
        string projGuid = proj.TargetGuidByName("Unity-iPhone");

        List<string> configGuidList = new List<string>();
        configGuidList.Add(proj.BuildConfigByName(projGuid, "Debug"));
        configGuidList.Add(proj.BuildConfigByName(projGuid, "Release"));
#if UNITY_5_5_OR_NEWER
        configGuidList.Add(proj.BuildConfigByName(projGuid, "ReleaseForProfiling"));
        configGuidList.Add(proj.BuildConfigByName(projGuid, "ReleaseForRunning"));
#endif
        SetOtherLinkFlags(proj, configGuidList, ldFlags);
        SetFrameworks(proj, projGuid, frameworks);
        SetTbds(proj, projGuid, tbds);
        proj.AddBuildPropertyForConfig(configGuidList, "ENABLE_BITCODE", "NO");

        AddCapability(proj, projGuid);

        SavePBXProject(proj, path, tbds);
    }

    private static PBXProject LoadPBXProject(string path)
    {
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(path);
        return proj;
    }

    private static void SavePBXProject(PBXProject proj, string path, string[] tbds)
    {
        proj.WriteToFile(path);
        File.WriteAllText(path, ModifyTBDLastKnownFileType(proj.WriteToString(), tbds));
    }

    private static string ModifyTBDLastKnownFileType(string projText, string[] tbds)
    {
        string[] lines = projText.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            for (int k = 0; k < tbds.Length; k++)
            {
                if (lines[i].IndexOf(tbds[k]) > -1)
                {
                    if (lines[i].IndexOf("lastKnownFileType = text") > -1)
                        lines[i] = lines[i].Replace("lastKnownFileType = text;", "lastKnownFileType = \"sourcecode.text-based-dylib-definition\";");
                }
            }
        }

        return string.Join("\n", lines);
    }

    private static void SetOtherLinkFlags(PBXProject proj, List<string> configGuidList, string[] ldFlags)
    {
        foreach (var ldFlag in ldFlags)
        {
            proj.AddBuildPropertyForConfig(configGuidList, "OTHER_LDFLAGS", ldFlag);
        }
    }

    private static void SetFrameworks(PBXProject proj, string projGuid, string[] frameworks)
    {
        foreach (var framework in frameworks)
        {
            proj.AddFrameworkToProject(projGuid, framework, false);
        }
    }

    private static void SetTbds(PBXProject proj, string projGuid, string[] tbds)
    {
        foreach (var tbd in tbds)
        {
            proj.AddFileToBuild(projGuid, proj.AddFile("usr/lib/" + tbd, "Frameworks/" + tbd, PBXSourceTree.Sdk));
        }
    }

    private static void SetPlistProperties(string path, string[] whiteList)
    {
        string[] localizations = new string[]
        {
            "en",
            "ko",
        };

        GamebasePlistManager.GetInstance().LoadPlist(path);
        GamebasePlistManager.GetInstance().SetURLSchemes("fb834403209993340");
        GamebasePlistManager.GetInstance().SetURLSchemes("paycoadapter");
        GamebasePlistManager.GetInstance().SetIDPAppID("FacebookAppID", "834403209993340");
        GamebasePlistManager.GetInstance().SetIDPAppID("TOAST_IAP_SERVER", "BETA");
        GamebasePlistManager.GetInstance().SetIDPAppID("NSPhotoLibraryUsageDescription", "Test");
        GamebasePlistManager.GetInstance().SetWhiteList(whiteList);
        GamebasePlistManager.GetInstance().SetList("CFBundleLocalizations", localizations);
        GamebasePlistManager.GetInstance().SavePlist(path);
    }

    // Unity 2017 or NE.XcodeEditor Only
    private static void AddCapability(PBXProject proj, string target)
    {
        proj.AddCapability(target, PBXCapabilityType.GameCenter);
        proj.AddCapability(target, PBXCapabilityType.InAppPurchase);
        proj.AddCapability(target, PBXCapabilityType.PushNotifications);
    }
}