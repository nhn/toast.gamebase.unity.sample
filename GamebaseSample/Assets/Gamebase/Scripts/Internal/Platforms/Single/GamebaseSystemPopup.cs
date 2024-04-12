﻿#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System;
using System.Text;
using System.Collections.Generic;
using Toast.Gamebase.Internal.Single.Communicator;
using UnityEngine.Networking;
#if !UNITY_WEBGL
using UnityEngine;
#endif

namespace Toast.Gamebase.Internal.Single
{
    public class GamebaseSystemPopup
    {
        public const string KEY_URL             = "url";
        public const string KEY_TITLE           = "title";
        public const string KEY_MESSAGE         = "message";
        public const string KEY_BUTTON_LEFT     = "buttonLeft";
        public const string KEY_BUTTON_RIGHT    = "buttonRight";
        public const string KEY_EXTRA           = "extra";

        private static readonly GamebaseSystemPopup instance = new GamebaseSystemPopup();

        public static GamebaseSystemPopup Instance
        {
            get { return instance; }
        }

        public void ShowLaunchingPopup(LaunchingResponse.LaunchingInfo launchingInfo)
        {
            if (false == GamebaseUnitySDK.EnablePopup || false == GamebaseUnitySDK.EnableLaunchingStatusPopup)
            {
                return;
            }

            GamebaseLog.Debug(string.Format("LaunchingStatusCode : {0}", launchingInfo.launching.status.code), this);

            CheckNotice(launchingInfo);

            var status = launchingInfo.launching.status;
            switch (status.code)
            {
                case GamebaseLaunchingStatus.IN_SERVICE:
                case GamebaseLaunchingStatus.IN_TEST:
                case GamebaseLaunchingStatus.IN_REVIEW:
                    {
                        break;
                    }
                
                case GamebaseLaunchingStatus.IN_SERVICE_BY_QA_WHITE_LIST:
                case GamebaseLaunchingStatus.IN_BETA:
                    {
                        GamebaseLog.Debug(status.message, this);
                        break;
                    }
                
                case GamebaseLaunchingStatus.RECOMMEND_UPDATE:
                    {
#if !UNITY_WEBGL
                        ShowRecommendUpdatePopup(launchingInfo);
#endif
                        break;
                    }

                case GamebaseLaunchingStatus.REQUIRE_UPDATE:
                    {
#if !UNITY_WEBGL
                        ShowRequireUpdatePopup(
                            DisplayLanguage.Instance.GetString("launching_update_required_title"),
                            status.message, 
                            launchingInfo.launching.app.install.url, 
                            DisplayLanguage.Instance.GetString("launching_update_now_label"));                        
#endif
                        break;
                    }

                case GamebaseLaunchingStatus.BLOCKED_USER:
                    {
                        ShowBlockedUserPopup(status.message);
                        break;
                    }

                case GamebaseLaunchingStatus.TERMINATED_SERVICE:
                    {
                        ShowTerminatedServicePopup(launchingInfo);
                        break;
                    }
                    
                case GamebaseLaunchingStatus.INSPECTING_SERVICE:
                case GamebaseLaunchingStatus.INSPECTING_ALL_SERVICES:
                    {
                        ShowInspectingServicePopup(launchingInfo);
                        break;
                    }

                case GamebaseLaunchingStatus.INTERNAL_SERVER_ERROR:
                    {
                        ShowLaunchingStatusPopup(status.code);
                        break;
                    }
                
                default:
                    {
                        if (GamebaseUtil.IsLaunchingPlayable(status.code))
                        {
                            GamebaseLog.Warn(string.Format("Playable but undefined (status: {0})", status.code), this);
                        } 
                        else
                        {
                            ShowLaunchingStatusPopup(status.code);
                        }
                        break;
                    }
            }
        }

        public void ShowErrorPopup(GamebaseError error, BaseVO vo = null)
        {
            if (true == Gamebase.IsSuccess(error))
            {
                return;
            }

            GamebaseLog.Debug(string.Format("ErrorCode : {0}", error.code), this);

            switch (error.code)
            {
                case GamebaseErrorCode.BANNED_MEMBER:              // 이용정지
                    {
                        ShowBanPopup((AuthResponse.LoginInfo)vo);
                        break;
                    }
                case GamebaseErrorCode.LAUNCHING_UNREGISTERED_CLIENT:
                    {
                        GamebaseResponse.Launching.UpdateInfo updateInfo = GamebaseResponse.Launching.UpdateInfo.From(error);
                        if(updateInfo == null)
                        {
                            return;
                        }

                        ShowRequireUpdatePopup(
                            DisplayLanguage.Instance.GetString("launching_unregistered_client_version"),
                            updateInfo.message, 
                            updateInfo.installUrl, 
                            DisplayLanguage.Instance.GetString("common_ok_button"));
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void ShowHeartbeatErrorPopup(GamebaseError error)
        {
            if (Gamebase.IsSuccess(error) == true)
            {
                return;
            }

            GamebaseLog.Debug(string.Format("code:{0}", error.code), this);

            switch (error.code)
            {
                case GamebaseErrorCode.INVALID_MEMBER: // 잘못된 사용자 KickOut
                    {
                        break;
                    }
                case GamebaseErrorCode.BANNED_MEMBER: // 이용정지 KickOut
                    {
                        if (GamebaseUnitySDK.EnableBanPopup == true)
                        {
                            ShowKickOutPopup();
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void ShowKickOutPopup()
        {
            GamebaseLog.Debug("ShowKickOutPopup", this);
            
            ShowSystemPopup(
                GamebaseUtilAlertType.ALERT_OK,
                DisplayLanguage.Instance.GetString("common_notice_title"),
                DisplayLanguage.Instance.GetString("ban_user_message"),
                string.Empty,
                DisplayLanguage.Instance.GetString("common_close_button"),
                string.Empty,
                (buttonID) => { });
        }

        public void ShowServerPushPopup(
            ServerPush.ServerPushMessage.ServerPushPopup popup,
            GamebaseCallback.VoidDelegate callback = null)
        {
            GamebaseLog.Debug("ShowServerPushPopup", this);

            if (popup == null || popup.messages == null || popup.messages.Count == 0)
            {
                return;
            }

            string languageCode = GamebaseImplementation.Instance.GetDisplayLanguageCode();
            if(string.IsNullOrEmpty(languageCode) == true || popup.messages.ContainsKey(languageCode) == false)
            {
                languageCode = GamebaseImplementation.Instance.GetDeviceLanguageCode();
                if (string.IsNullOrEmpty(languageCode) == true || popup.messages.ContainsKey(languageCode) == false)
                {
                    languageCode = popup.defaultLanguage;
                    if (string.IsNullOrEmpty(languageCode) == true || popup.messages.ContainsKey(languageCode) == false)
                    {
                        return;
                    }
                }
            }

            ServerPush.ServerPushMessage.ServerPushPopup.Message message = popup.messages[languageCode];

            if(message == null)
            {
                return;
            }

            ShowSystemPopup(
                GamebaseUtilAlertType.ALERT_OK,
                message.title,
                message.message,
                string.Empty,
                DisplayLanguage.Instance.GetString("common_ok_button"),
                string.Empty,
                (buttonID) =>
                {
                    if (callback != null)
                    {
                        callback();
                    }
                });
        }

        private void CheckNotice(LaunchingResponse.LaunchingInfo launchingInfo)
        {
            if (null != launchingInfo.launching.notice)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine(launchingInfo.launching.notice.title);
                msg.AppendLine(string.Empty);
                msg.AppendLine(launchingInfo.launching.notice.message);

                ShowSystemPopup(
                    GetButtonTypeWithURL(launchingInfo.launching.notice.url),
#if !UNITY_WEBGL
                    DisplayLanguage.Instance.GetString("common_notice_title"),
#else
                    string.Empty,
#endif
                    msg.ToString(),
                    DisplayLanguage.Instance.GetString("common_show_detail_button"),
                    DisplayLanguage.Instance.GetString("common_close_button"),
                    DisplayLanguage.Instance.GetString("common_show_detail_message"),
                    (buttonID) =>
                    {
                        
                        if (GamebaseUtilAlertButtonID.BUTTON_ONE == buttonID)
                        {
                            if (false == string.IsNullOrEmpty(launchingInfo.launching.notice.url))
                            {
                                GamebaseWebviewImplementation.Instance.OpenWebBrowser(launchingInfo.launching.notice.url);
                            }
                        }
                    });
            }
        }

        private void ShowInspectingServicePopup(LaunchingResponse.LaunchingInfo launchingInfo)
        {
            var maintenanceInfo = launchingInfo.launching.maintenance;

            if (maintenanceInfo != null)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine(DisplayLanguage.Instance.GetString("launching_maintenance_message"));

                if (maintenanceInfo.hideDate == false)
                {
                    msg.AppendLine();
                    msg.AppendLine(MakePeriod(maintenanceInfo.localBeginDate, maintenanceInfo.localEndDate));
                }

                ShowSystemPopup(
                    GetButtonTypeWithPageTypeCode(maintenanceInfo.pageTypeCode),
                    DisplayLanguage.Instance.GetString("launching_maintenance_title"),
                    msg.ToString(),
                    DisplayLanguage.Instance.GetString("common_show_detail_button"),
                    DisplayLanguage.Instance.GetString("common_close_button"),
                    DisplayLanguage.Instance.GetString("common_show_detail_message"),
                    (buttonID) =>
                    {
                        if (GamebaseUtilAlertButtonID.BUTTON_ONE == buttonID)
                        {
                            if (string.IsNullOrEmpty(maintenanceInfo.url) == false)
                            {
                                GamebaseWebviewImplementation.Instance.OpenWebBrowser(maintenanceInfo.url);
                            }
                        }
                    });
            }
        }

        private void ShowTerminatedServicePopup(LaunchingResponse.LaunchingInfo launchingInfo)
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine(launchingInfo.launching.status.message);

            ShowSystemPopup(
                GamebaseUtilAlertType.ALERT_OK,
                DisplayLanguage.Instance.GetString("launching_service_closed_title"),
                msg.ToString(),
                string.Empty,
                DisplayLanguage.Instance.GetString("common_close_button"),
                string.Empty,
                (buttonID) => { });
        }

        private void ShowRecommendUpdatePopup(LaunchingResponse.LaunchingInfo launchingInfo)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append(launchingInfo.launching.status.message);

            ShowSystemPopup(
                GamebaseUtilAlertType.ALERT_OKCANCEL,
                DisplayLanguage.Instance.GetString("launching_update_recommended_title"),
                msg.ToString(),
                DisplayLanguage.Instance.GetString("launching_update_now_label"),
                DisplayLanguage.Instance.GetString("launching_update_later_label"),
                string.Empty,
                (buttonID) =>
                {
                    if (GamebaseUtilAlertButtonID.BUTTON_ONE == buttonID)
                    {
#if !UNITY_WEBGL
                        GamebaseWebviewImplementation.Instance.OpenWebBrowser(launchingInfo.launching.app.install.url);
                        Application.Quit();
#endif
                    }
                });
        }
                    
        private void ShowRequireUpdatePopup(string title, string message, string url, string button)
        {
            if (false == GamebaseUnitySDK.EnableLaunchingStatusPopup || false == GamebaseUnitySDK.EnablePopup)
            {
                return;
            }

            StringBuilder msg = new StringBuilder();
            msg.Append(message);

            ShowSystemPopup(
                GamebaseUtilAlertType.ALERT_OK,
                title,
                msg.ToString(),
                string.Empty,
                button,
                string.Empty,
                (buttonID) =>
                {
                    if (GamebaseUtilAlertButtonID.BUTTON_ONE == buttonID)
                    {
#if !UNITY_WEBGL
                        GamebaseWebviewImplementation.Instance.OpenWebBrowser(url);
                        Application.Quit();
#endif
                    }
                });
        }

        private void ShowBanPopup(AuthResponse.LoginInfo vo)
        {
            if (false == GamebaseUnitySDK.EnableBanPopup || false == GamebaseUnitySDK.EnablePopup)
            {
                return;
            }

            string messageString = UnityCompatibility.WebRequest.UnEscapeURL(vo.errorExtras.ban.message);


            StringBuilder message = new StringBuilder();
            message.Append(DisplayLanguage.Instance.GetString("ban_user_message"));
            message.Append("\n\n");
            message.Append(DisplayLanguage.Instance.GetString("ban_user_id_label"));
            message.Append(" : ");
            message.Append(vo.errorExtras.ban.userId);
            message.Append("\n");
            message.Append(DisplayLanguage.Instance.GetString("ban_reason_label"));
            message.Append(" : ");
            message.Append(messageString.ToString());
            message.Append("\n");
            message.Append(DisplayLanguage.Instance.GetString("ban_date_label"));
            message.Append(" : ");
            if (true == vo.errorExtras.ban.banType.Equals("TEMPORARY", StringComparison.Ordinal))
            {
                message.Append(new DateTime(1970, 1, 1).AddHours(9).AddMilliseconds(vo.errorExtras.ban.beginDate).ToString());
                message.Append(" ~ ");
                message.Append(new DateTime(1970, 1, 1).AddHours(9).AddMilliseconds(vo.errorExtras.ban.endDate).ToString());
            }
            else
            {
                message.Append(DisplayLanguage.Instance.GetString("ban_permanent_label"));
            }

            message.Append("\n\n");
            message.Append(DisplayLanguage.Instance.GetString("ban_detailpage_cs_guide_message"));
            message.Append("\n\n");
            message.Append(DisplayLanguage.Instance.GetString("ban_show_cs_message"));
            ShowSystemPopup(
                GamebaseUtilAlertType.ALERT_OKCANCEL,
                DisplayLanguage.Instance.GetString("ban_title"),
                message.ToString(),
                DisplayLanguage.Instance.GetString("ban_cs_label"),
                DisplayLanguage.Instance.GetString("common_close_button"),
                string.Empty,
                (buttonID) =>
                {
                    if (GamebaseUtilAlertButtonID.BUTTON_ONE == buttonID)
                    {
                        GamebaseWebviewImplementation.Instance.OpenWebBrowser(Gamebase.Launching.GetLaunchingInformations().launching.app.customerService.url);
                    }
                });
        }
        
        private void ShowBlockedUserPopup(string message)
        {
            ShowSystemPopup(
                GamebaseUtilAlertType.ALERT_OK,
                DisplayLanguage.Instance.GetString("launching_blocked_user_title"),
                message,
                string.Empty,
                DisplayLanguage.Instance.GetString("common_close_button"),
                string.Empty,
                (buttonID) => { });
        }
        
        private void ShowLaunchingStatusPopup(int statusCode)
        {
            string message = string.Format("{0}({1})",
                DisplayLanguage.Instance.GetString("common_unknown_error_message"), statusCode);

            ShowSystemPopup(
                GamebaseUtilAlertType.ALERT_OK,
                DisplayLanguage.Instance.GetString("common_error_label"),
                message,
                string.Empty,
                DisplayLanguage.Instance.GetString("common_close_button"),
                string.Empty,
                (buttonID) => { });
        }

        private void ShowSystemPopup(GamebaseUtilAlertType alertType, string title, string message, string buttonLeft = "", string buttonRight = "", string extra = "", GamebaseCallback.DataDelegate<GamebaseUtilAlertButtonID> callback= null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            
            parameters.Add(KEY_TITLE,           title);
            parameters.Add(KEY_MESSAGE,         message);
            parameters.Add(KEY_BUTTON_LEFT,     buttonLeft);
            parameters.Add(KEY_BUTTON_RIGHT,    buttonRight);
            parameters.Add(KEY_EXTRA,           extra);
            
            GamebaseUtilImplementation.Instance.ShowAlert(parameters, alertType, callback);
        }

        private GamebaseUtilAlertType GetButtonTypeWithURL(string url)
        {
            GamebaseUtilAlertType alertType = GamebaseUtilAlertType.ALERT_OKCANCEL;

            if (true == string.IsNullOrEmpty(url))
            {
                alertType = GamebaseUtilAlertType.ALERT_OK;
            }

            return alertType;
        }

        private GamebaseUtilAlertType GetButtonTypeWithPageTypeCode(string pageTypeCode)
        {
            GamebaseUtilAlertType alertType = GamebaseUtilAlertType.ALERT_OKCANCEL;

            switch (pageTypeCode)
            {
                case "DEFAULT":
                    alertType = GamebaseUtilAlertType.ALERT_OK;
                    break;
                case "DEFAULT_HTML":
                    alertType = GamebaseUtilAlertType.ALERT_OK;
                    break;
                case "URL":
                    break;
                case "URL_PARAM":
                    break;
            }

            return alertType;
        }

        /// <summary>
        /// Make combination date string with long date string and long time string.
        /// </summary>
        /// <param name="beginEpochTime"></param>
        /// <param name="endEpochTime"></param>
        private string MakePeriod(double beginEpochTime, double endEpochTime)
        {
            DateTime utcDateTime    = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime beginDateTime  = utcDateTime.AddMilliseconds(beginEpochTime).ToLocalTime();
            DateTime endDateTime    = utcDateTime.AddMilliseconds(endEpochTime).ToLocalTime();

            StringBuilder sb        = new StringBuilder();

            sb.Append(beginDateTime.ToLongDateString());

            if ((beginDateTime.Year != endDateTime.Year) || (beginDateTime.Month != endDateTime.Month) || (beginDateTime.Day != endDateTime.Day))
            {
                sb.Append(", ");
                sb.AppendLine(beginDateTime.ToLongTimeString());
                sb.AppendLine("~");
                sb.AppendFormat("{0}, {1}", endDateTime.ToLongDateString(), endDateTime.ToLongTimeString());
            }
            else
            {
                sb.AppendLine();
                sb.AppendFormat("{0} ~ {1}", beginDateTime.ToLongTimeString(), endDateTime.ToLongTimeString());
            }

            return sb.ToString();
        }
    }
}
#endif