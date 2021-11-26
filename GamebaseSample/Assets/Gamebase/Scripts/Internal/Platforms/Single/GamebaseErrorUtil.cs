#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System;
using Toast.Gamebase.Internal.Single.Communicator;

namespace Toast.Gamebase.Internal.Single
{
    public static class GamebaseErrorUtil
    {
        public static GamebaseError CreateGamebaseErrorByServerErrorCode(string transactionId, string apiId, CommonResponse.Header header, string domain = null)
        {
            int errorCode = 0;

            switch (header.resultCode)
            {
                //----------------------------------------
                //  Common
                //----------------------------------------
                case GamebaseServerErrorCode.TYPE_MISS_MATCH:
                case GamebaseServerErrorCode.ILLEGAL_ARGUMENT:
                case GamebaseServerErrorCode.HTTP_MESSAGE_NOT_READABLE:
                case GamebaseServerErrorCode.MISSING_SERVLET_REQUEST_PARAMETER:
                case GamebaseServerErrorCode.METHOD_ARGUMENT_NOT_VALID:
                case GamebaseServerErrorCode.METHOD_ARGUMENT_TYPE_MISMATCH:
                case GamebaseServerErrorCode.INVALID_APP_ID:
                case GamebaseServerErrorCode.INVALID_APP_KEY:
                case GamebaseServerErrorCode.INVALID_SECRET_KEY:
                    {
                        errorCode = GamebaseErrorCode.INVALID_PARAMETER;
                        break;
                    }
                case GamebaseServerErrorCode.NOT_AUTHENTICATED:
                    {
                        errorCode = GamebaseErrorCode.NOT_LOGGED_IN;
                        break;
                    }
                case GamebaseServerErrorCode.UNKNOWN_SYSTEM:
                    {
                        errorCode = GamebaseErrorCode.SERVER_INTERNAL_ERROR;
                        break;
                    }
                case GamebaseServerErrorCode.REMOTE_SYSTEM:
                    {
                        errorCode = GamebaseErrorCode.SERVER_REMOTE_SYSTEM_ERROR;
                        break;
                    }
                case GamebaseServerErrorCode.BANNED_MEMBER:
                    {
                        errorCode = GamebaseErrorCode.BANNED_MEMBER;
                        break;
                    }
                case GamebaseServerErrorCode.INVALID_MEMBER:
                    {
                        errorCode = GamebaseErrorCode.INVALID_MEMBER;
                        break;
                    }

                //----------------------------------------
                //  Lighthouse
                //----------------------------------------
                case GamebaseServerErrorCode.LIGHT_HOUSE_NOT_AUTHENTICATED:
                    {
                        errorCode = GamebaseErrorCode.NOT_LOGGED_IN;
                        break;
                    }
                case GamebaseServerErrorCode.LIGHT_HOUSE_NO_SUCH_REQUEST_API:
                    {
                        errorCode = GamebaseErrorCode.NOT_SUPPORTED;
                        break;
                    }
                case GamebaseServerErrorCode.LIGHT_HOUSE_JSON_PARSING_ERROR:
                    {
                        errorCode = GamebaseErrorCode.INVALID_JSON_FORMAT;
                        break;
                    }
                case GamebaseServerErrorCode.LIGHT_HOUSE_GATEWAY_CONNECTION_ERROR:
                    {
                        errorCode = GamebaseErrorCode.SERVER_INTERNAL_ERROR;
                        break;
                    }

                //----------------------------------------
                //  Gateway
                //----------------------------------------
                case GamebaseServerErrorCode.GATEWAY_JSON_PARSING_ERROR:
                    {
                        errorCode = GamebaseErrorCode.INVALID_JSON_FORMAT;
                        break;
                    }
                case GamebaseServerErrorCode.GATEWAY_MISSING_REQUEST_PARAMETER:
                case GamebaseServerErrorCode.GATEWAY_INVALID_APP_ID:
                    {
                        errorCode = GamebaseErrorCode.INVALID_PARAMETER;
                        break;
                    }
                case GamebaseServerErrorCode.GATEWAY_INVALID_ACCESS_TOKEN:
                    {
                        errorCode = GamebaseErrorCode.AUTH_TOKEN_LOGIN_INVALID_TOKEN_INFO;
                        break;
                    }
                case GamebaseServerErrorCode.GATEWAY_PRODUCT_DATA_NOT_FOUND:
                    {
                        errorCode = GamebaseErrorCode.INVALID_PARAMETER;
                        break;
                    }
                case GamebaseServerErrorCode.GATEWAY_REQUEST_API_NOT_FOUND:
                    {
                        errorCode = GamebaseErrorCode.NOT_SUPPORTED;
                        break;
                    }
                case GamebaseServerErrorCode.GATEWAY_LOGGED_IN_IDP_COULD_NOT_DELETED:
                    {
                        errorCode = GamebaseErrorCode.AUTH_REMOVE_MAPPING_LOGGED_IN_IDP;
                        break;
                    }
                case GamebaseServerErrorCode.GATEWAY_UNKNOWN_SYSTEM:
                    {
                        errorCode = GamebaseErrorCode.SERVER_UNKNOWN_ERROR;
                        break;
                    }
                case GamebaseServerErrorCode.GATEWAY_REQUEST_WORKER_ERROR:
                case GamebaseServerErrorCode.GATEWAY_QUEUE_TIME_OUT:
                case GamebaseServerErrorCode.GATEWAY_QUEUE_CAPACITY_FULL:
                    {
                        errorCode = GamebaseErrorCode.SERVER_INTERNAL_ERROR;
                        break;
                    }
                case GamebaseServerErrorCode.GATEWAY_GB_LNC_SYSTEM_ERROR:
                    {
                        errorCode = GamebaseErrorCode.LAUNCHING_SERVER_ERROR;
                        break;
                    }
                case GamebaseServerErrorCode.GATEWAY_GB_ID_SYSTEM_ERROR:
                    {
                        errorCode = GamebaseErrorCode.SERVER_INTERNAL_ERROR;
                        break;
                    }

                //----------------------------------------
                //  Launching
                //----------------------------------------
                case GamebaseServerErrorCode.LAUNCHING_NOT_EXIST_CLIENT_ID:
                    {
                        errorCode = GamebaseErrorCode.LAUNCHING_NOT_EXIST_CLIENT_ID;
                        break;
                    }
                case GamebaseServerErrorCode.LAUNCHING_UNREGISTERED_APP:
                    {
                        errorCode = GamebaseErrorCode.LAUNCHING_UNREGISTERED_APP;
                        break;
                    }
                case GamebaseServerErrorCode.LAUNCHING_UNREGISTERED_CLIENT:
                    {
                        errorCode = GamebaseErrorCode.LAUNCHING_UNREGISTERED_CLIENT;
                        break;
                    }

                //----------------------------------------
                //  Member
                //----------------------------------------
                case GamebaseServerErrorCode.MEMBER_APP_ID_MISS_MATCH:
                    {
                        errorCode = GamebaseErrorCode.INVALID_PARAMETER;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_USER_ID_MISS_MATCH:
                case GamebaseServerErrorCode.MEMBER_INVALID_MEMBER:
                    {
                        errorCode = GamebaseErrorCode.INVALID_MEMBER;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_INVALID_AUTH:
                    {
                        if (apiId.Equals(Lighthouse.API.Gateway.ID.TOKEN_LOGIN, StringComparison.Ordinal) == true)
                        {
                            errorCode = GamebaseErrorCode.AUTH_TOKEN_LOGIN_INVALID_TOKEN_INFO;
                        }
                        else if (apiId.Equals(Lighthouse.API.Gateway.ID.IDP_LOGIN, StringComparison.Ordinal) == true)
                        {
                            errorCode = GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED;
                        }
                        else if (apiId.Equals(Lighthouse.API.Gateway.ID.ADD_MAPPING, StringComparison.Ordinal) == true)
                        {
                            errorCode = GamebaseErrorCode.AUTH_ADD_MAPPING_FAILED;
                        }
                        else if (apiId.Equals(Lighthouse.API.Gateway.ID.REMOVE_MAPPING, StringComparison.Ordinal) == true)
                        {
                            errorCode = GamebaseErrorCode.AUTH_REMOVE_MAPPING_FAILED;
                        }
                        else if (apiId.Equals(Lighthouse.API.Gateway.ID.WITHDRAW, StringComparison.Ordinal) == true)
                        {
                            errorCode = GamebaseErrorCode.AUTH_WITHDRAW_FAILED;
                        }
                        else
                        {
                            errorCode = GamebaseErrorCode.AUTH_UNKNOWN_ERROR;
                        }
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_CANNOT_TEMPORARY_WITHDRAW:
                    {
                        errorCode = GamebaseErrorCode.AUTH_WITHDRAW_ALREADY_TEMPORARY_WITHDRAW;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_NOT_TEMPORARY_WITHDRAW:
                    {
                        errorCode = GamebaseErrorCode.AUTH_WITHDRAW_NOT_TEMPORARY_WITHDRAW;
                        break;
                    }
                case GamebaseServerErrorCode.BANNED_MEMBER_LOGIN:
                case GamebaseServerErrorCode.AUTHKEY_BELONG_TO_BANNED_MEMBER:
                    {
                        errorCode = GamebaseErrorCode.BANNED_MEMBER;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_NOT_EXIST:
                    {
                        errorCode = GamebaseErrorCode.AUTH_NOT_EXIST_MEMBER;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_LAST_MAPPED_IDP:
                    {
                        errorCode = GamebaseErrorCode.AUTH_REMOVE_MAPPING_LAST_MAPPED_IDP;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_TRANSFERACCOUNT_ALREADY_USED:
                    {
                        errorCode = GamebaseErrorCode.AUTH_TRANSFERACCOUNT_ALREADY_USED;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_ALREADY_MAPPED_MEMBER:
                    {
                        errorCode = GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_ALREADY_MAPPED_IDP:
                    {
                        errorCode = GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_HAS_SAME_IDP;
                        break;
                    }
                case GamebaseServerErrorCode.CAN_NOT_ADD_GUEST_IDP:
                    {
                        errorCode = GamebaseErrorCode.AUTH_ADD_MAPPING_CANNOT_ADD_GUEST_IDP;
                        break;
                    }

                 //
                 // Add Mapping Forcibly
                 //
                case GamebaseServerErrorCode.MEMBER_FORCING_MAPPING_TICKET_NOT_EXIST:
                    {
                        errorCode = GamebaseErrorCode.AUTH_ADD_MAPPING_FORCIBLY_NOT_EXIST_KEY;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_CONSUMED_FORCING_MAPPING_TICKET:
                    {
                        errorCode = GamebaseErrorCode.AUTH_ADD_MAPPING_FORCIBLY_ALREADY_USED_KEY;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_EXPIRED_FORCING_MAPPING_TICKET:
                    {
                        errorCode = GamebaseErrorCode.AUTH_ADD_MAPPING_FORCIBLY_EXPIRED_KEY;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_IDP_MISS_MATCH_FOR_FORCING_MAPPING:
                    {
                        errorCode = GamebaseErrorCode.AUTH_ADD_MAPPING_FORCIBLY_DIFFERENT_IDP;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_AUTHKEY_MISS_MATCH_FOR_FORCING_MAPPING:
                    {
                        errorCode = GamebaseErrorCode.AUTH_ADD_MAPPING_FORCIBLY_DIFFERENT_AUTHKEY;
                        break;
                    }
                case GamebaseServerErrorCode.GATEWAY_USER_MUST_HAVE_ONLY_GUEST_IDP:
                case GamebaseServerErrorCode.MEMBER_IS_NOT_GUEST:
                    {
                        errorCode = GamebaseErrorCode.NOT_GUEST_OR_HAS_OTHERS;
                        break;
                    }               
                
                //----------------------------------------
                //  Transfer Account
                //----------------------------------------
                case GamebaseServerErrorCode.MEMBER_TRANSFERACCOUNT_EXPIRED:
                    {
                        errorCode = GamebaseErrorCode.AUTH_TRANSFERACCOUNT_EXPIRED;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_BLOCK_ID:
                    {
                        errorCode = GamebaseErrorCode.AUTH_TRANSFERACCOUNT_BLOCK;
                        break;
                    }                
                case GamebaseServerErrorCode.MEMBER_NEOID_CHECK_PASSWORD_NOT_EXIST_ID:
                    {
                        errorCode = GamebaseErrorCode.AUTH_TRANSFERACCOUNT_INVALID_ID;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_NEOID_CHECK_PASSWORD_INVALID_PASSWORD:
                    {
                        errorCode = GamebaseErrorCode.AUTH_TRANSFERACCOUNT_INVALID_PASSWORD;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_CONSOLE_NO_CONDITION:
                    {
                        errorCode = GamebaseErrorCode.AUTH_TRANSFERACCOUNT_CONSOLE_NO_CONDITION;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_TRANSFERACCOUNT_NOT_EXIST:
                    {
                        errorCode = GamebaseErrorCode.AUTH_TRANSFERACCOUNT_NOT_EXIST;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_NEOID_SIGNUP_ALREADY_EXIST_ID:
                    {
                        errorCode = GamebaseErrorCode.AUTH_TRANSFERACCOUNT_ALREADY_EXIST_ID;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_SAME_REQUESTOR:
                    {
                        errorCode = GamebaseErrorCode.SAME_REQUESTOR;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_ALREADY_WITHDRAWN:
                    {
                        errorCode = GamebaseErrorCode.AUTH_WITHDRAW_FAILED;
                        break;
                    }
                case GamebaseServerErrorCode.MEMBER_NEOID_SIGNUP_UNKNOWN:
                case GamebaseServerErrorCode.MEMBER_NEOID_CHECK_PASSWORD_UNKNOWN:
                case GamebaseServerErrorCode.MEMBER_NEOID_QUERY:
                case GamebaseServerErrorCode.MEMBER_NEOID_QUERY_INFO:
                case GamebaseServerErrorCode.MEMBER_NEOID_CREATE_PASSWORD:
                case GamebaseServerErrorCode.MEMBER_NEOID_CONNECTION:
                    {
                        errorCode = GamebaseErrorCode.SERVER_INTERNAL_ERROR;
                        break;
                    }

                default:
                    {
                        errorCode = GamebaseErrorCode.UNKNOWN_ERROR;
                        break;
                    }
            }
            
            var traceError = header.traceError;

            if (IsRecursive(traceError) == true)
            {
                var gamebaseServerError = new GamebaseError(header.resultCode, traceError.productId, header.resultMessage);
                CreateGamebaseErrorByTraceErrorRecursion(gamebaseServerError, traceError);

                return new GamebaseError(errorCode, domain, string.Empty, gamebaseServerError, transactionId);
            }
            else
            {
                return new GamebaseError(errorCode, domain, string.Empty, new GamebaseError(header.resultCode, traceError.throwPoint, header.resultMessage), transactionId);
            }
        }

        private static bool IsRecursive(CommonResponse.Header.TraceError traceError)
        {
            if(traceError == null)
            {
                return false;
            }

            return traceError.traceError != null;
        }

        private static void CreateGamebaseErrorByTraceErrorRecursion(GamebaseError gamebaseError, CommonResponse.Header.TraceError traceError)
        {
            gamebaseError.error = new GamebaseError(traceError.resultCode, traceError.throwPoint, traceError.resultMessage);

            if (traceError.traceError != null)
            {
                CreateGamebaseErrorByTraceErrorRecursion(gamebaseError.error, traceError.traceError);
            }
        }
    }
}
#endif