using System.Collections.Generic;
using Toast.Gamebase;
using Toast.Gamebase.Internal;


public static class LaunchingInfoHelper
{
    private static LaunchingResponse.LaunchingInfo GetLaunchingInfo()
    {
        return DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
    }

    public static LaunchingResponse.LaunchingInfo.Launching.App.IDP GetIdPInfo(string idPName)
    {
        var launchingInfo = GetLaunchingInfo();
        if (launchingInfo is null)
        {
            return null;
        }

        if (launchingInfo.launching.app.idP.TryGetValue(idPName, out var idPInfo))
        {
            if (idPInfo != null)
            {
                return idPInfo;
            }
        }

        return null;
    }

    public static string GetIdPAdditional(string idPName)
    {
        return GetIdPInfo(idPName)?.additional;
    }

    public static string GetGamebaseUrl()
    {
        return GetLaunchingInfo()?.launching.app.loginUrls.gamebaseUrl;
    }

    public static LaunchingResponse.LaunchingInfo.Launching.App.IDP.LoginWebView GetLoginWebView(string idPName)
    {
        return GetIdPInfo(idPName)?.loginWebView;
    }

    public static string GetClientIdForLineRegion(string lineRegion)
    {
        var line = GetIdPInfo(GamebaseAuthProvider.LINE);
        if (line == null)
        {
            return string.Empty;
        }

        foreach (var channel in line.channels)
        {
            if (channel.region.Equals(lineRegion))
            {
                return channel.clientId;
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// This is a list of masking processing for the authentication information entered by the user.
    /// It is included in launchingInfo, so the server manager must request it when adding it.
    /// </summary>
    public static List<string> GetSecurityBlacklist()
    {
        if (GetLaunchingInfo()?.launching.tcgbClient.stability.securityBlacklist == null)
        {
            return new List<string> {
                GamebaseAuthProviderCredential.ACCESS_TOKEN,
                GamebaseAuthProviderCredential.GAMEBASE_ACCESS_TOKEN,
                GamebaseAuthProviderCredential.ACCESS_TOKEN_SECRET,
                GamebaseAuthProviderCredential.AUTHORIZATION_CODE,
                GamebaseAuthProviderCredential.CODE_VERIFIER,
            };
        }

        return GetLaunchingInfo()?.launching.tcgbClient.stability.securityBlacklist;
    }
}