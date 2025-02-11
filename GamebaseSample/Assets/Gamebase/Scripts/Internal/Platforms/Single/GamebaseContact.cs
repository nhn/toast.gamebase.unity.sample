#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.LitJson;
using UnityEngine;

namespace Toast.Gamebase.Internal.Single
{
    public class GamebaseContact
    {
        private static class CsType
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
                        GamebaseLog.Debug(string.Format("CS URL : {0}", url), this);
                        Application.OpenURL(url);
                    }

                    callback(error);
                });
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
                callback(null, new GamebaseError(GamebaseErrorCode.NOT_INITIALIZED, message: GamebaseStrings.NOT_INITIALIZED));
                return;
            }

            GetCsUrl(
                configuration,
                (url, error) =>
                {
                    if (Gamebase.IsSuccess(error) == true)
                    {
                        GamebaseLog.Debug(string.Format("CS URL : {0}", url), this);
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
                        GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.COMMON,
                            GamebaseIndicatorReportType.StabilityCode.GB_COMMON_WRONG_USAGE,
                            GamebaseIndicatorReportType.LogLevel.ERROR,
                            new Dictionary<string, string>()
                            {
                                { GamebaseIndicatorReportType.AdditionalKey.GB_FUNCTION_NAME, "GamebaseContact.OpenContact" },
                                { GamebaseIndicatorReportType.AdditionalKey.GB_ERROR_LOG, string.Format("Invalid csType : {0}", launchingInfo.launching.app.customerService.type) }
                            });

                        callback(GetCustomCsUrl(configuration), null);
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
            string url = AddAdditionalUrlToCsUrl(configuration);

            url = string.Format(
                "{0}ticket={1}&purpose={2}&userId={3}&userName={4}&osCode={5}",
                MakeBaseUrl(url),
                shortTermTicket,
                ISSUE_SHORT_TERM_TICKET_PURPOSE,
                GamebaseImplementation.Instance.GetUserID(),
                GetUserNameInConfiguration(configuration),
                UnityCompatibility.WebRequest.EscapeURL(GamebaseSystemInfo.OsVersion));
            
            url = AddAdditionalParametersToCsUrl(url, configuration);

            return AddExtraDataToCsUrl(url, configuration);
        }

        private string GetToastCsUrlNotLogin(GamebaseRequest.Contact.Configuration configuration)
        {
            string url = AddAdditionalUrlToCsUrl(configuration);

            url = string.Format(
                "{0}userName={1}&osCode={2}",
                MakeBaseUrl(url),
                GetUserNameInConfiguration(configuration),
                UnityCompatibility.WebRequest.EscapeURL(GamebaseSystemInfo.OsVersion)
                );
            
            url = AddAdditionalParametersToCsUrl(url, configuration);

            return AddExtraDataToCsUrl(url, configuration);
        }

        private string GetGamebaseCsUrl(GamebaseRequest.Contact.Configuration configuration, string shortTermTicket)
        {
            string url = AddAdditionalUrlToCsUrl(configuration);

            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == false)
            {
                url = string.Format(
                    "{0}ticket={1}&purpose={2}&userId={3}&userName={4}&storeCode={5}&clientVersion={6}&sdkVersion={7}&deviceModel={8}&osVersion={9}&deviceCountryCode={10}&usimCountryCode={11}&osCode={12}",
                    MakeBaseUrl(url),
                    shortTermTicket,
                    ISSUE_SHORT_TERM_TICKET_PURPOSE,
                    GamebaseImplementation.Instance.GetUserID(),
                    GetUserNameInConfiguration(configuration),
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseUnitySDK.StoreCode),
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseUnitySDK.AppVersion),
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseUnitySDK.SDKVersion),
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseSystemInfo.DeviceModel),
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseSystemInfo.OsVersion),
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseImplementation.Instance.GetCountryCodeOfDevice()),
                    string.Empty,
                    UnityCompatibility.WebRequest.EscapeURL(GamebaseSystemInfo.Platform)
                    );
            }
            
            url = AddAdditionalParametersToCsUrl(url, configuration);

            return AddExtraDataToCsUrl(url, configuration);
        }

        private string GetCustomCsUrl(GamebaseRequest.Contact.Configuration configuration)
        {
            string url = AddAdditionalUrlToCsUrl(configuration);
            
            url = AddAdditionalParametersToCsUrl(url, configuration);

            return AddExtraDataToCsUrl(url, configuration);
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

        private string GetUserNameInConfiguration(GamebaseRequest.Contact.Configuration configuration)
        {
            if (configuration == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(configuration.userName) == true)
            {
                return string.Empty;
            }
            
            return UnityCompatibility.WebRequest.EscapeURL(configuration.userName);
        }

        private string AddAdditionalUrlToCsUrl(GamebaseRequest.Contact.Configuration configuration)
        {
            var url = launchingInfo.launching.app.customerService.url;

            if (configuration == null)
            {
                return url;
            }

            if (string.IsNullOrEmpty(configuration.additionalURL) == true)
            {
                return url;
            }

            return string.Concat(url, configuration.additionalURL);
        }

        private string AddAdditionalParametersToCsUrl(string url, GamebaseRequest.Contact.Configuration configuration)
        {
            if (configuration == null)
            {
                return url;
            }

            var additionalParameters = configuration.additionalParameters;
            if (additionalParameters == null || additionalParameters.Count == 0)
            {
                return url;
            }

            string urlParams = string.Empty;

            foreach (KeyValuePair<string, string> additionalParameter in additionalParameters)
            {
                if (string.IsNullOrEmpty(additionalParameter.Key) == true || string.IsNullOrEmpty(additionalParameter.Value) == true)
                {
                    continue;
                }

                urlParams = string.Format(
                    "{0}&{1}={2}",
                    urlParams,
                    UnityCompatibility.WebRequest.EscapeURL(additionalParameter.Key),
                    UnityCompatibility.WebRequest.EscapeURL(additionalParameter.Value));
            }

            return string.Concat(url, urlParams);
        }

        private string AddExtraDataToCsUrl(string url, GamebaseRequest.Contact.Configuration configuration)
        {
            if (configuration == null)
            {
                return url;
            }

            if (configuration.extraData == null || configuration.extraData.Count == 0)
            {
                return url;
            }

            return string.Format(
                "{0}&extraData={1}",
                url,
                UnityCompatibility.WebRequest.EscapeURL(JsonMapper.ToJson(configuration.extraData)));
        }
    }
}
#endif
