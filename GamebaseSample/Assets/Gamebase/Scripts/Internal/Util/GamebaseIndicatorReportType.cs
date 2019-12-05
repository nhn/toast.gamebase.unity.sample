using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Gamebase
{
    public static class GamebaseIndicatorReportType
    {
        public static class LogType
        {
            public const string INIT = "Init";
            public const string AUTH = "Auth";
            public const string EVENT = "Event";
            public const string PURCHASE = "Purchase";
            public const string PUSH = "Push";
            public const string COMMON = "Common";
        }

        public static class LogLevel
        {
            public const string DEBUG = "DEBUG";
            public const string INFO = "INFO";
            public const string WARN = "WARN";
            public const string ERROR = "ERROR";
            public const string FATAL = "FATAL";
            public const string NONE = "NONE";
        }

        public static class StabilityCode
        {
            public const string GB_INIT_FAILED_MULTIPLE_TIMES = "GB_INIT_FAILED_MULTIPLE_TIMES";
            public const string GB_AUTH_LOGIN_SUCCESS = "GB_AUTH_LOGIN_SUCCESS";
            public const string GB_AUTH_LOGIN_FAILED = "GB_AUTH_LOGIN_FAILED";
            public const string GB_AUTH_LOGIN_CANCELED = "GB_AUTH_LOGIN_CANCELED";
            public const string GB_AUTH_LOGOUT_SUCCESS = "GB_AUTH_LOGOUT_SUCCESS";
            public const string GB_AUTH_LOGOUT_FAILED = "GB_AUTH_LOGOUT_FAILED";
            public const string GB_AUTH_WITHDRAW_SUCCESS = "GB_AUTH_WITHDRAW_SUCCESS";
            public const string GB_AUTH_WITHDRAW_FAILED = "GB_AUTH_WITHDRAW_FAILED";
            public const string GB_EVENT_OBSERVER_BANNED_MEMBER = "GB_EVENT_OBSERVER_BANNED_MEMBER";
            public const string GB_EVENT_OBSERVER_INVALID_MEMBER = "GB_EVENT_OBSERVER_INVALID_MEMBER";
            public const string GB_IAP_PURCHASE_SUCCESS = "GB_IAP_PURCHASE_SUCCESS";
            public const string GB_IAP_PURCHASE_FAILED = "GB_IAP_PURCHASE_FAILED";
            public const string GB_IAP_PURCHASE_CANCELED = "GB_IAP_PURCHASE_CANCELED";
            public const string GB_COMMON_WRONG_USAGE = "GB_COMMON_WRONG_USAGE";
        }

        public static class AdditionalKey
        {
            public const string GB_CONFIGURATION = "txtGBConfiguration";
            public const string GB_LOGIN_IDP = "GBLoginIDP";
            public const string GB_CREDENTIAL = "txtGBCredential";
            public const string GB_TCIAP_APP_KEY = "GBTCIapAppKey";
            public const string GB_ITEM_SEQ = "GBItemSeq";
            public const string GB_OBSERVER_DATA = "txtGBObserverData";
            public const string GB_SUB_CATEGORY1 = "GBSubCategory1";
        }

        public static class SubCategory
        {
            public const string LOGIN = "Login";
            public const string LOGOUT = "Logout";
            public const string ADDMAPPING = "AddMapping";
            public const string WITHDRAW = "Withdraw";
            public const string TRANSFERACCOUNT = "TransferAccount";
        }
    }
}