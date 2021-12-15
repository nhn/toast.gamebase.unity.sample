using System.Collections.Generic;
using System.Linq;
using Toast.Internal;
using UnityEngine;

namespace Toast.Logger
{
    public static class ToastAuditLog
    {
        private static InstanceLogger _logger = null;

        private const string AuditPrefix = "com.toast.unity.audit";
        private static readonly string AppVersionPrefKey = Combine(AuditPrefix, "AppVersion");

        private static bool Initialize()
        {
            if (_logger == null)
            {
                string auditProejctKey = ToastApplicationInfo.GetAuditKey();

                if (string.IsNullOrEmpty(auditProejctKey))
                {
                    return false;
                }

                _logger = new InstanceLogger(auditProejctKey);
            }

            return true;
        }

        private static void SetCommonData(Dictionary<string, string> userFields)
        {
            if (string.IsNullOrEmpty(ToastSdk.UserId) == false)
            {
                userFields.Add(ToastAuditLoggerContants.AUDIT_USER_ID, ToastSdk.UserId);
            }
            userFields.Add(ToastAuditLoggerContants.AUDIT_APP_ID, ToastLoggerCommonLogic.AppKey);
            userFields.Add(ToastAuditLoggerContants.AUDIT_APP_NAME, Application.productName);
            userFields.Add(ToastAuditLoggerContants.AUDIT_APP_VERSION, Application.version);
            userFields.Add(ToastAuditLoggerContants.AUDIT_SDK_FUNCTION, ToastAuditLoggerContants.AUDIT_VALUE_SDK_FUNCTION);
            userFields.Add(ToastAuditLoggerContants.AUDIT_OS, ToastAuditLoggerContants.AUDIT_VALUE_OS);
            userFields.Add(ToastAuditLoggerContants.AUDIT_OS_VERSION, SystemInfo.operatingSystem);
            userFields.Add(ToastAuditLoggerContants.AUDIT_SESSION_ID, ToastApplicationInfo.GetSessionId());
            userFields.Add(ToastAuditLoggerContants.AUDIT_COUNTRY_CODE, ToastApplicationInfo.GetCountryCode());
        }

        public static void SendUsageLog()
        {
            if (Initialize())
            {
                if (IsEqualAppVersion())
                {
                    return;
                }

                Dictionary<string, string> userFields = new Dictionary<string, string>();
                SetCommonData(userFields);
                _logger.Log(ToastAuditLoggerContants.USAGE_AUDIT_LOG_TYPE, ToastLogLevel.INFO, ToastAuditLoggerContants.USAGE_AUDIT_LOG_MESSAGE, userFields);
            }
        }

        private static bool IsEqualAppVersion()
        {
            string appVersion = PlayerPrefs.GetString(AppVersionPrefKey, "");
            if (Application.version == appVersion)
            {
                return true;
            }
            else
            {
                PlayerPrefs.SetString(AppVersionPrefKey, Application.version);
            }

            return false;
        }

        private static string Combine(params string[] args)
        {
            return args.Aggregate("", (a1, a2) => a1.TrimEnd('.') + a2);
        }
    }
}