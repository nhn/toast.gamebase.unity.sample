#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System.Collections.Generic;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal
{
    public partial class GamebaseIndicatorReport
    {
        public static class Auth
        {
            public static void LoginWithProvider(string providerName, GamebaseResponse.Auth.AuthToken authToken, GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGIN },
                    { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, providerName }
                };

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.AUTH,
                    customFields = customFields,
                };
                

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGIN_SUCCESS;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                    
                    SetLastLoggedInInfo(authToken.token.sourceIdPCode, authToken.member.userId);
                }
                else
                {
                    if(error.code == GamebaseErrorCode.AUTH_USER_CANCELED)
                    {
                        item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGIN_CANCELED;
                        item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                        item.isUserCanceled = true;
                    }
                    else
                    {
                        item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGIN_FAILED;
                        item.logLevel = GamebaseIndicatorReportType.LogLevel.ERROR;
                    }
                    item.error = error;
                }
                
                AddIndicatorItem(item);
            }

            public static void LoginWithCredential(Dictionary<string, object> credentialInfo, GamebaseResponse.Auth.AuthToken authToken, GamebaseError error)
            {
                string providerName = string.Empty;
                if (credentialInfo.TryGetValue(GamebaseAuthProviderCredential.PROVIDER_NAME, out object value))
                {
                    providerName = value as string;
                }

                if (string.IsNullOrEmpty(providerName))
                {
                    return;
                }
                
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGIN },
                    { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, providerName}
                };

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.AUTH,
                    customFields = customFields,
                };

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_CREDENTIAL_LOGIN_SUCCESS;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                    
                    SetLastLoggedInInfo(authToken.token.sourceIdPCode, authToken.member.userId);
                }
                else
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_CREDENTIAL_LOGIN_FAILED;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.ERROR;
                    item.error = error;
                }
                
                AddIndicatorItem(item);
            }
            
            public static void ChangeLogin(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, GamebaseResponse.Auth.AuthToken authToken, GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGIN },
                    { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, forcingMappingTicket.idPCode },
                    { GamebaseIndicatorReportType.AdditionalKey.GB_FORCING_MAPPING_TICKET, JsonMapper.ToJson(forcingMappingTicket) }
                };

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.AUTH,
                    customFields = customFields,
                };

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_CHANGE_LOGIN_SUCCESS;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                    
                    SetLastLoggedInInfo(authToken.token.sourceIdPCode, authToken.member.userId);
                }
                else
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_CHANGE_LOGIN_FAILED;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.ERROR;
                    item.error = error;
                }
                
                AddIndicatorItem(item);
            }
            
            public static void MappingWithProvider(string providerName, GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.ADDMAPPING },
                    { GamebaseIndicatorReportType.AdditionalKey.GB_MAPPING_IDP, providerName },
                    { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, providerName }
                };

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.AUTH,
                    customFields = customFields,
                };

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_MAPPING_SUCCESS;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                }
                else
                {
                    if(error.code == GamebaseErrorCode.AUTH_USER_CANCELED)
                    {
                        item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_MAPPING_CANCELED;
                        item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                        item.isUserCanceled = true;
                    }
                    else
                    {
                        item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_MAPPING_FAILED;
                        item.logLevel = GamebaseIndicatorReportType.LogLevel.ERROR;
                    }
                    item.error = error;
                }
                
                AddIndicatorItem(item);
            }

            public static void MappingWithCredential(Dictionary<string, object> credentialInfo, GamebaseError error)
            {
                string providerName = string.Empty;
                if (credentialInfo.TryGetValue(GamebaseAuthProviderCredential.PROVIDER_NAME, out object value))
                {
                    providerName = value as string;
                }

                if (string.IsNullOrEmpty(providerName))
                {
                    return;
                }
                
                MappingWithProvider(providerName, error);
            }
            
            public static void ForcingMapping(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.ADDMAPPING },
                    { GamebaseIndicatorReportType.AdditionalKey.GB_MAPPING_IDP, forcingMappingTicket.idPCode },
                    { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, forcingMappingTicket.idPCode },
                    { GamebaseIndicatorReportType.AdditionalKey.GB_FORCING_MAPPING_TICKET, JsonMapper.ToJson(forcingMappingTicket) }
                };

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.AUTH,
                    customFields = customFields,
                };

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_MAPPING_FORCIBLY_SUCCESS;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                    
                    SetLastLoggedInInfo(forcingMappingTicket.idPCode, forcingMappingTicket.idPCode);
                }
                else
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_MAPPING_FORCIBLY_FAILED;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.ERROR;
                    item.error = error;
                }
                
                AddIndicatorItem(item);
            }
            
            public static void Logout(Dictionary<string, object> additionalInfo, GamebaseError error)
            {
                bool isInternalCall = false;
                if (additionalInfo != null)
                {
                    if (additionalInfo.TryGetValue(GamebaseAuthProviderCredential.IS_INTERNAL_CALL, out object value))
                    {
                        isInternalCall = value is bool ? (bool)value : false;
                    }
                }
                
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGOUT }
                };
                if (isInternalCall)
                {
                    customFields.Add(GamebaseIndicatorReportType.AdditionalKey.GB_CREDENTIAL, JsonMapper.ToJson(additionalInfo));
                }

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.AUTH,
                    customFields = customFields,
                };

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = isInternalCall ? 
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_INTERNAL_LOGOUT_SUCCESS :
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGOUT_SUCCESS;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                }
                else
                {
                    item.stabilityCode = isInternalCall ? 
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_INTERNAL_LOGOUT_FAILED :
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGOUT_FAILED;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.ERROR;
                    item.error = error;
                }
                
                AddIndicatorItem(item);
            }
            
            public static void Withdraw(GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.WITHDRAW }
                };

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.AUTH,
                    customFields = customFields,
                };

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_WITHDRAW_SUCCESS;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                }
                else
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_WITHDRAW_FAILED;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.ERROR;
                    item.error = error;
                }
                
                AddIndicatorItem(item);
            }
            
            public static void RequestTemporaryWithdraw(GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.TEMPORARYWITHDRAW }
                };

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.AUTH,
                    customFields = customFields,
                };

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_REQUEST_TEMPORARY_WITHDRAWAL_SUCCESS;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                }
                else
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_REQUEST_TEMPORARY_WITHDRAWAL_FAILED;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.ERROR;
                    item.error = error;
                }
                
                AddIndicatorItem(item);
            }
            
            public static void CancelTemporaryWithdraw(GamebaseError error)
            {
                var customFields = new Dictionary<string, string>
                {
                    { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.TEMPORARYWITHDRAW }
                };

                var item = new IndicatorItem
                {
                    logType = GamebaseIndicatorReportType.LogType.AUTH,
                    customFields = customFields,
                };

                if (Gamebase.IsSuccess(error))
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_CANCEL_TEMPORARY_WITHDRAWAL_SUCCESS;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                }
                else
                {
                    item.stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_CANCEL_TEMPORARY_WITHDRAWAL_FAILED;
                    item.logLevel = GamebaseIndicatorReportType.LogLevel.ERROR;
                    item.error = error;
                }
                
                AddIndicatorItem(item);
            }
        }
    }
}
#endif