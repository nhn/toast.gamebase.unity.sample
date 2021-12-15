#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single
{
    public class GamebaseContact
    {
        private class CsType
        {
            public const string GAMEBASE = "GAMEBASE";
            public const string TOAST = "TOAST";
            public const string CUSTOM = "CUSTOM";
        }

        private const string ISSUE_SHORT_TERM_TICKET_PURPOSE = "openContact";
        private const int ISSUE_SHORT_TERM_TICKET_EXPIRESIN = 10;

        private static readonly GamebaseContact instance = new GamebaseContact();
        private GamebaseResponse.Launching.LaunchingInfo launchingInfo;

        public static GamebaseContact Instance
        {
            get { return instance; }
        }

        public void OpenContact(int handle)
        {
            OpenContact(null, handle);
        }

        public void OpenContact(GamebaseRequest.Contact.Configuration configuration, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
            if (callback == null)
            {
                return;
            }

            GamebaseCallbackHandler.UnregisterCallback(handle);

            if (GamebaseUnitySDK.IsInitialized == false)
            {
                callback(
                    new GamebaseError(
                        GamebaseErrorCode.NOT_INITIALIZED,
                        message: GamebaseStrings.NOT_INITIALIZED
                        ));
                return;
            }

            GetCsUrl(
               configuration,
               (url, error) =>
               {
                   if (Gamebase.IsSuccess(error) == true)
                   {
                       GamebaseLog.Debug(
                           string.Format("URL : {0}", url),
                           this);

                       Application.OpenURL(url);
                   }

                   callback(error);
               });
        }

        public void RequestContactURL(int handle)
        {
            RequestContactURL(null, handle);
        }

        public void RequestContactURL(GamebaseRequest.Contact.Configuration configuration, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<string>>(handle);
            if (callback == null)
            {
                return;
            }

            GamebaseCallbackHandler.UnregisterCallback(handle);

            if (GamebaseUnitySDK.IsInitialized == false)
            {
                callback(
                    null,
                    new GamebaseError(
                        GamebaseErrorCode.NOT_INITIALIZED,
                        message: GamebaseStrings.NOT_INITIALIZED
                        ));
                return;
            }

            GetCsUrl(
                configuration,
                (url, error) =>
                {
                    if (Gamebase.IsSuccess(error) == true)
                    {
                        GamebaseLog.Debug(
                           string.Format("URL : {0}", url),
                           this);
                    }

                    callback(url, error);
                });
        }

        private void GetCsUrl(GamebaseRequest.Contact.Configuration configuration, System.Action<string, GamebaseError> callback)
        {
            launchingInfo = GamebaseLaunchingImplementation.Instance.GetLaunchingInformations();

            if (launchingInfo.launching.app.customerService == null)
            {
                callback(
                    string.Empty,
                    new GamebaseError(
                        GamebaseErrorCode.UI_CONTACT_FAIL_INVALID_URL,
                        message: GamebaseStrings.UI_CONTACT_FAIL_INVALID_URL));
                return;
            }

            switch (launchingInfo.launching.app.customerService.type)
            {
                case CsType.TOAST:
                    {
                        if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
                        {
                            callback(GetToastCsUrlNotLogin(configuration), null);
                            return;
                        }

                        GetShortTermTicket(
                            (ticket, error) =>
                            {
                                if (Gamebase.IsSuccess(error) == true)
                                {
                                    callback(GetToastCsUrlLogin(configuration, ticket), null);
                                    return;
                                }

                                callback(string.Empty, error);
                            });
                        break;
                    }
                case CsType.GAMEBASE:
                    {
                        if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
                        {
                            callback(GetGamebaseCsUrl(configuration, string.Empty), null);
                            return;
                        }

                        GetShortTermTicket(
                            (ticket, error) =>
                            {
                                if (Gamebase.IsSuccess(error) == true)
                                {
                                    callback(GetGamebaseCsUrl(configuration, ticket), null);
                                    return;
                                }

                                callback(string.Empty, error);
                            });
                        break;
                    }
                case CsType.CUSTOM:
                    {
                        callback(GetCustomCsUrl(configuration), null);
                        break;
                    }
                default:
                    {
                        callback(GetDefaultCsUrl(configuration), null);
                        break;
                    }
            }
        }

        private void GetShortTermTicket(GamebaseCallback.GamebaseDelegate<string> callback)
        {
            GamebaseUtil.IssueShortTermTicket(
                ISSUE_SHORT_TERM_TICKET_PURPOSE,
                ISSUE_SHORT_TERM_TICKET_EXPIRESIN,
                "GamebaseContact",
                (shortTermTicket, error) =>
                {
                    if (Gamebase.IsSuccess(error) == true)
                    {
                        callback(shortTermTicket, null);
                    }
                    else
                    {
                        callback(
                            string.Empty,
                            new GamebaseError(
                            GamebaseErrorCode.UI_CONTACT_FAIL_ISSUE_SHORT_TERM_TICKET,
                            message: GamebaseStrings.UI_CONTACT_FAIL_ISSUE_SHORT_TERM_TICKET,
                            domain: error.domain,
                            error: error));
                    }
                });
        }

        private string GetToastCsUrlLogin(GamebaseRequest.Contact.Configuration configuration, string shortTermTicket)
        {
            string url = launchingInfo.launching.app.customerService.url;
            if (configuration != null && string.IsNullOrEmpty(configuration.additionalURL) == false)
            {
                url = string.Format(
                    "{0}{1}",
                    url,
                    configuration.additionalURL);
            }

            string userName = GetUserName(configuration);

            url = string.Format(
                "{0}ticket={1}&purpose={2}&userId={3}&userName={4}&osCode={5}",
                MakeBaseUrl(url),
                shortTermTicket,
                ISSUE_SHORT_TERM_TICKET_PURPOSE,
                GamebaseImplementation.Instance.GetUserID(),
                userName,
                UnityCompatibility.WebRequest.EscapeURL(GamebaseUnitySDK.OsVersion)
                );

            if (configuration != null && configuration.extraData != null && configuration.extraData.Count > 0)
            {
                string extraData = JsonMapper.ToJson(configuration.extraData);
                url = string.Format(
                    "{0}&extraData={1}",
                    url,
                    UnityCompatibility.WebRequest.EscapeURL(extraData));
            }

            return url;
        }

        private string GetToastCsUrlNotLogin(GamebaseRequest.Contact.Configuration configuration)
        {
            string url = launchingInfo.launching.app.customerService.url;
            if (configuration != null && string.IsNullOrEmpty(configuration.additionalURL) == false)
            {
                url = string.Format(
                    "{0}{1}",
                    url,
                    configuration.additionalURL);
            }

            string userName = GetUserName(configuration);

            url = string.Format(
                "{0}userName={1}&osCode={2}",
                MakeBaseUrl(url),
                userName,
                UnityCompatibility.WebRequest.EscapeURL(GamebaseUnitySDK.OsVersion)
                );

            if (configuration != null && configuration.extraData != null && configuration.extraData.Count > 0)
            {
                string extraData = JsonMapper.ToJson(configuration.extraData);
                url = string.Format(
                    "{0}&extraData={1}",
                    url,
                    UnityCompatibility.WebRequest.EscapeURL(extraData));
            }

            return url;
        }

        private string GetGamebaseCsUrl(GamebaseRequest.Contact.Configuration configuration, string shortTermTicket)
        {
            string url = launchingInfo.launching.app.customerService.url;

            if (configuration != null && string.IsNullOrEmpty(configuration.additionalURL) == false)
            {
                url = string.Format(
                    "{0}{1}",
                    url,
                    configuration.additionalURL);
            }

            string userName = GetUserName(configuration);

            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == false)
            {
                url = string.Format(
                    "{0}ticket={1}&purpose={2}&userId={3}&userName={4}&storeCode={5}&clientVersion={6}&sdkVersion={7}&deviceModel={8}&osVersion={9}&deviceCountryCode={10}&usimCountryCode={11}",
                    MakeBaseUrl(url),
                    shortTermTicket,
                    ISSUE_SHORT_TERM_TICKET_PURPOSE,
                    GamebaseImplementation.Instance.GetUserID(),
                    userName,
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseUnitySDK.StoreCode),
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseUnitySDK.AppVersion),
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseUnitySDK.SDKVersion),
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseUnitySDK.DeviceModel),
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseUnitySDK.OsVersion),
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseImplementation.Instance.GetCountryCodeOfDevice()),
                    string.Empty
                    );
            }

            if (configuration != null && configuration.extraData != null && configuration.extraData.Count > 0)
            {
                string extraData = JsonMapper.ToJson(configuration.extraData);
                url = string.Format(
                    "{0}&extraData={1}",
                    url,
                    UnityCompatibility.WebRequest.EscapeURL(extraData));
            }

            return url;
        }

        private string GetCustomCsUrl(GamebaseRequest.Contact.Configuration configuration)
        {
            string url = launchingInfo.launching.app.customerService.url;

            if (configuration != null && string.IsNullOrEmpty(configuration.additionalURL) == false)
            {
                url = string.Format(
                    "{0}{1}",
                    url,
                    configuration.additionalURL);
            }

            if (configuration != null && configuration.extraData != null && configuration.extraData.Count > 0)
            {
                string extraData = JsonMapper.ToJson(configuration.extraData);
                url = string.Format(
                    "{0}?extraData={1}",
                    url,
                    UnityCompatibility.WebRequest.EscapeURL(extraData));
            }

            return url;
        }

        private string GetDefaultCsUrl(GamebaseRequest.Contact.Configuration configuration)
        {
            GamebaseIndicatorReport.SendIndicatorData(
                GamebaseIndicatorReportType.LogType.COMMON,
                GamebaseIndicatorReportType.StabilityCode.GB_COMMON_WRONG_USAGE,
                GamebaseIndicatorReportType.LogLevel.ERROR,
                new Dictionary<string, string>()
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_FUNCTION_NAME, "GamebaseContact.OpenContact" },
                    { GamebaseIndicatorReportType.AdditionalKey.GB_ERROR_LOG, string.Format("Invalid csType : {0}", launchingInfo.launching.app.customerService.type) }
                });

            return GetCustomCsUrl(configuration);
        }

        private string MakeBaseUrl(string url)
        {
            string baseUrl = url;

            GamebaseUrlUtil.SchemeInfo schemeInfo = GamebaseUrlUtil.ConvertURLToSchemeInfo(baseUrl);
            if (schemeInfo.parameterDictionary.Count > 0)
            {
                baseUrl += "&";
            }
            else
            {
                baseUrl += "?";
            }

            return baseUrl;
        }

        private string GetUserName(GamebaseRequest.Contact.Configuration configuration)
        {
            string userName = string.Empty;

            if (configuration != null && string.IsNullOrEmpty(configuration.userName) == false)
            {
                userName = UnityCompatibility.WebRequest.EscapeURL(configuration.userName);
            }

            return userName;
        }
    }
}
#endif
