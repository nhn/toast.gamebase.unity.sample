#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using System;
using System.Collections.Generic;
using System.Text;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebaseAuth : IGamebaseAuth
    {
        private string domain;
        protected bool isAuthenticationAlreadyProgress = false;

        private const string ISSUE_SHORT_TERM_TICKET_PURPOSE = "PURCHASED_LIST";
        private const int ISSUE_SHORT_TERM_TICKET_EXPIRESIN = 60;
        private const string KEY_IAP_EXTRA_USER_ID = "UserID";
        private const int LAUNCHING_STATUS_EXPIRE_TIME = 30;

        public string Domain
        {
            get
            {
                if (true == string.IsNullOrEmpty(domain))
                {
                    return typeof(CommonGamebaseAuth).Name;
                }

                return domain;
            }
            set
            {
                domain = value;
            }
        }
        
        public virtual void Login(string providerName, int handle)
        {
            CheckLaunchingStatusExpire(()=> 
            {
                GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> providerLoginCallback = (authToken, error) =>
                {
                    if(Gamebase.IsSuccess(error) == true)
                    {
                        GamebaseIndicatorReport.SetLastLoggedInInfo(providerName, authToken.member.userId);
                        GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.AUTH,
                            GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGIN_SUCCESS,
                            GamebaseIndicatorReportType.LogLevel.INFO,
                            new Dictionary<string, string>()
                            {
                                { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGIN },
                                { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, providerName }
                            });
                    }
                    else
                    {
                        GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.AUTH,
                            GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGIN_CANCELED,
                            GamebaseIndicatorReportType.LogLevel.INFO,
                            new Dictionary<string, string>()
                            {
                                { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGIN },
                                { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, providerName }
                            },
                            error,
                            true);
                    }

                    var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);

                    if (callback != null)
                    {
                        callback(authToken, error);
                    }

                    GamebaseCallbackHandler.UnregisterCallback(handle);
                };

                int providerLoginHandle = GamebaseCallbackHandler.RegisterCallback(providerLoginCallback);

                LoginWithProviderName(providerName, providerLoginHandle);
            });
        }
        
        public void Login(string providerName, Dictionary<string, object> additionalInfo, int handle)
        {
            GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> providerLoginCallback = (authToken, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    GamebaseIndicatorReport.SetLastLoggedInInfo(providerName, authToken.member.userId);
                    GamebaseIndicatorReport.SendIndicatorData(
                        GamebaseIndicatorReportType.LogType.AUTH,
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGIN_SUCCESS,
                        GamebaseIndicatorReportType.LogLevel.INFO,
                        new Dictionary<string, string>()
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGIN },
                            { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, providerName },
                            { GamebaseIndicatorReportType.AdditionalKey.GB_CREDENTIAL, JsonMapper.ToJson(additionalInfo)}
                        });
                }
                else
                {
                    GamebaseIndicatorReport.SendIndicatorData(
                        GamebaseIndicatorReportType.LogType.AUTH,
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGIN_CANCELED,
                        GamebaseIndicatorReportType.LogLevel.INFO,
                        new Dictionary<string, string>()
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGIN },
                            { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, providerName },
                            { GamebaseIndicatorReportType.AdditionalKey.GB_CREDENTIAL, JsonMapper.ToJson(additionalInfo)}
                        },
                        error,
                        true);
                }

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);

                if (callback != null)
                {
                    callback(authToken, error);
                }

                GamebaseCallbackHandler.UnregisterCallback(handle);
            };

            int providerLoginHandle = GamebaseCallbackHandler.RegisterCallback(providerLoginCallback);

            if (AuthAdapterManager.Instance.IsSupportedIDP(providerName) == false)
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(providerLoginHandle);
                GamebaseErrorNotifier.FireNotSupportedAPI(
                    this,
                    callback,
                    string.Format("LoginWithAdditionalInfo({0})", providerName));
                GamebaseCallbackHandler.UnregisterCallback(providerLoginHandle);
                return;
            }

            if (CanLogin(providerLoginHandle) == false)
            {
                return;
            }

            bool hasAdapter = AuthAdapterManager.Instance.CreateIDPAdapter(providerName);

            if (hasAdapter == false)
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(providerLoginHandle);
                callback(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, message: GamebaseStrings.AUTH_ADAPTER_NOT_FOUND_NEED_SETUP));
                GamebaseCallbackHandler.UnregisterCallback(providerLoginHandle);
            }

            AuthAdapterManager.Instance.IDPLogin(additionalInfo, (adapterError) =>
            {
                if (Gamebase.IsSuccess(adapterError) == true)
                {
                    var idPAccessToken = AuthAdapterManager.Instance.GetIDPData<string>(providerName, AuthAdapterManager.MethodName.GET_IDP_ACCESS_TOKEN);
                    var requestVO = AuthMessage.GetIDPLoginMessage(providerName, idPAccessToken);
                    RequestGamebaseLogin(requestVO, providerLoginHandle);

                    return;
                }

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(providerLoginHandle);
                if (callback == null)
                {
                    return;
                }

                GamebaseCallbackHandler.UnregisterCallback(providerLoginHandle);
                callback(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, Domain, error: adapterError));
                AuthAdapterManager.Instance.IDPLogout(providerName);
            });
        }
        
        public void Login(Dictionary<string, object> credentialInfo, int handle)
        {
            CheckLaunchingStatusExpire(() =>
            {
                GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> providerLoginCallback = (authToken, error) =>
                {
                    string providerName = string.Empty;
                    if (credentialInfo.ContainsKey(GamebaseAuthProviderCredential.PROVIDER_NAME) == true)
                    {
                        providerName = (string)credentialInfo[GamebaseAuthProviderCredential.PROVIDER_NAME];
                    }

                    if (Gamebase.IsSuccess(error) == true)
                    {
                        GamebaseIndicatorReport.SetLastLoggedInInfo(providerName, authToken.member.userId);
                        GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.AUTH,
                            GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGIN_SUCCESS,
                            GamebaseIndicatorReportType.LogLevel.INFO,
                            new Dictionary<string, string>()
                            {
                                { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGIN },
                                { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, providerName },                                
                                { GamebaseIndicatorReportType.AdditionalKey.GB_CREDENTIAL, JsonMapper.ToJson(credentialInfo)}
                            });
                    }
                    else
                    {
                        GamebaseIndicatorReport.SendIndicatorData(
                            GamebaseIndicatorReportType.LogType.AUTH,
                            GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGIN_CANCELED,
                            GamebaseIndicatorReportType.LogLevel.INFO,
                            new Dictionary<string, string>()
                            {
                                { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGIN },
                                { GamebaseIndicatorReportType.AdditionalKey.GB_LOGIN_IDP, providerName },
                                { GamebaseIndicatorReportType.AdditionalKey.GB_CREDENTIAL, JsonMapper.ToJson(credentialInfo)}
                            },
                            error,
                            true);
                    }

                    var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);

                    if (callback != null)
                    {
                        callback(authToken, error);
                    }

                    GamebaseCallbackHandler.UnregisterCallback(handle);
                };

                int providerLoginHandle = GamebaseCallbackHandler.RegisterCallback(providerLoginCallback);

                LoginWithCredentialInfo(credentialInfo, providerLoginHandle);
            });
        }       

        public void LoginForLastLoggedInProvider(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void ChangeLogin(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void AddMapping(string providerName, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void AddMapping(string providerName, Dictionary<string, object> additionalInfo, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback, "AddMapping(additionalInfo)");
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void AddMapping(Dictionary<string, object> credentialInfo, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback, "AddMapping(credentialInfo)");
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void AddMappingForcibly(string providerName, string forcingMappingKey, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void AddMappingForcibly(Dictionary<string, object> credentialInfo, string forcingMappingKey, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback, "AddMapping(credentialInfo)");
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void AddMappingForcibly(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void AddMappingForcibly(string providerName, string forcingMappingKey, Dictionary<string, object> additionalInfo, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback, "AddMapping(additionalInfo)");
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void RemoveMapping(string providerName, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void Logout(int handle)
        {
            GamebaseCallback.ErrorDelegate logoutCallback = (error) =>
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);

                if (callback != null)
                {
                    callback(error);
                    GamebaseCallbackHandler.UnregisterCallback(handle);
                }

                string stabilityCode = string.Empty;
                string logLevel = string.Empty;

                if (Gamebase.IsSuccess(error) == true)
                {
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGOUT_SUCCESS;
                    logLevel = GamebaseIndicatorReportType.LogLevel.INFO;
                }
                else
                {
                    stabilityCode = GamebaseIndicatorReportType.StabilityCode.GB_AUTH_LOGOUT_FAILED;
                    logLevel = GamebaseIndicatorReportType.LogLevel.ERROR;
                }

                GamebaseIndicatorReport.SendIndicatorData(
                    GamebaseIndicatorReportType.LogType.AUTH,
                    stabilityCode,
                    logLevel,
                    new Dictionary<string, string>()
                    {
                        { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.LOGOUT }
                    },
                    error);
            };

            int logoutHandle = GamebaseCallbackHandler.RegisterCallback(logoutCallback);

            if (CanLogout(logoutHandle) == false)
            {
                return;
            }

            var requestVO = AuthMessage.GetLogoutMessage();

            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                GamebaseAnalytics.Instance.IdPCode = string.Empty;

                GamebaseSystemPopup.Instance.ShowErrorPopup(error);

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(logoutHandle);
                if (callback == null)
                {
                    return;
                }

                GamebaseCallbackHandler.UnregisterCallback(logoutHandle);

                RemoveLoginData();

                callback(error);
            });
        }

        public void Withdraw(int handle)
        {
            GamebaseCallback.ErrorDelegate withdrawCallback = (error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    GamebaseIndicatorReport.SendIndicatorData(
                        GamebaseIndicatorReportType.LogType.AUTH,
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_WITHDRAW_SUCCESS,
                        GamebaseIndicatorReportType.LogLevel.INFO,
                        new Dictionary<string, string>()
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.WITHDRAW }
                        });
                }
                else
                {
                    GamebaseIndicatorReport.SendIndicatorData(
                        GamebaseIndicatorReportType.LogType.AUTH,
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_WITHDRAW_FAILED,
                        GamebaseIndicatorReportType.LogLevel.ERROR,
                        new Dictionary<string, string>()
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.WITHDRAW }
                        },
                        error);
                }

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);

                if (callback != null)
                {
                    callback(error);
                }

                GamebaseCallbackHandler.UnregisterCallback(handle);
            };


            int withdrawHandle = GamebaseCallbackHandler.RegisterCallback(withdrawCallback);

            if (CanLogout(withdrawHandle) == false)
            {
                return;
            }

            var requestVO = AuthMessage.GetWithdrawMessage();

            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                GamebaseAnalytics.Instance.IdPCode = string.Empty;

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(withdrawHandle);
                if (callback == null)
                {
                    return;
                }
                GamebaseCallbackHandler.UnregisterCallback(withdrawHandle);

                if (error == null)
                {
                    var vo = JsonMapper.ToObject<AuthResponse.WithdrawInfo>(response);
                    if (vo.header.isSuccessful == true)
                    {
                        RemoveLoginData();
                    }
                    else
                    {
                        if (GamebaseServerErrorCode.MEMBER_ALREADY_WITHDRAWN == vo.header.resultCode)
                        {
                            RemoveLoginData();
                        }
                        else
                        {
                            error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain);
                            GamebaseSystemPopup.Instance.ShowErrorPopup(error);
                        }
                    }
                }
                else
                {
                    GamebaseSystemPopup.Instance.ShowErrorPopup(error);
                }
                callback(error);
            });
        }

        public void WithdrawImmediately(int handle)
        {
            Withdraw(handle);
        }

        public void RequestTemporaryWithdrawal(int handle)
        {
            GamebaseCallback.GamebaseDelegate<GamebaseResponse.TemporaryWithdrawalInfo> requestTemporaryWithdrawalCallback = (data, error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    GamebaseIndicatorReport.SendIndicatorData(
                        GamebaseIndicatorReportType.LogType.AUTH,
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_REQUEST_TEMPORARY_WITHDRAWAL_SUCCESS,
                        GamebaseIndicatorReportType.LogLevel.INFO,
                        new Dictionary<string, string>()
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.WITHDRAW }
                        });
                }
                else
                {
                    GamebaseIndicatorReport.SendIndicatorData(
                        GamebaseIndicatorReportType.LogType.AUTH,
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_REQUEST_TEMPORARY_WITHDRAWAL_FAILED,
                        GamebaseIndicatorReportType.LogLevel.ERROR,
                        new Dictionary<string, string>()
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.WITHDRAW }
                        },
                        error);
                }

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.TemporaryWithdrawalInfo>>(handle);

                if (callback != null)
                {
                    callback(data, error);
                }

                GamebaseCallbackHandler.UnregisterCallback(handle);
            };


            int requestTemporaryWithdrawalHandle = GamebaseCallbackHandler.RegisterCallback(requestTemporaryWithdrawalCallback);

            if (CanTemporaryWithdrawal(requestTemporaryWithdrawalHandle) == false)
            {
                return;
            }

            var requestVO = AuthMessage.GetTemporaryWithdrawalMessage();

            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                GamebaseAnalytics.Instance.IdPCode = string.Empty;

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate < GamebaseResponse.TemporaryWithdrawalInfo >> (requestTemporaryWithdrawalHandle);
                if (callback == null)
                {
                    return;
                }
                GamebaseCallbackHandler.UnregisterCallback(requestTemporaryWithdrawalHandle);

                var vo = JsonMapper.ToObject<AuthResponse.TemporaryWithdrawalInfo>(response);
                GamebaseResponse.TemporaryWithdrawalInfo temporaryWithdrawal = null;
                
                if (error == null)
                {
                    if (vo.header.isSuccessful == true)
                    {
                        temporaryWithdrawal = new GamebaseResponse.TemporaryWithdrawalInfo();
                        temporaryWithdrawal.gracePeriodDate = vo.member.temporaryWithdrawal.gracePeriodDate;
                    }
                    else
                    {
                        error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain);
                    }                        
                }

                callback(temporaryWithdrawal, error);
            });
        }

        public void CancelTemporaryWithdrawal(int handle)
        {
            GamebaseCallback.ErrorDelegate cancelTemporaryWithdrawalCallback = (error) =>
            {
                if (Gamebase.IsSuccess(error) == true)
                {
                    GamebaseIndicatorReport.SendIndicatorData(
                        GamebaseIndicatorReportType.LogType.AUTH,
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_CANCEL_TEMPORARY_WITHDRAWAL_SUCCESS,
                        GamebaseIndicatorReportType.LogLevel.INFO,
                        new Dictionary<string, string>()
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.WITHDRAW }
                        });
                }
                else
                {
                    GamebaseIndicatorReport.SendIndicatorData(
                        GamebaseIndicatorReportType.LogType.AUTH,
                        GamebaseIndicatorReportType.StabilityCode.GB_AUTH_CANCEL_TEMPORARY_WITHDRAWAL_FAILED,
                        GamebaseIndicatorReportType.LogLevel.ERROR,
                        new Dictionary<string, string>()
                        {
                            { GamebaseIndicatorReportType.AdditionalKey.GB_SUB_CATEGORY1, GamebaseIndicatorReportType.SubCategory.WITHDRAW }
                        },
                        error);
                }

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);

                if (callback != null)
                {
                    callback(error);
                }

                GamebaseCallbackHandler.UnregisterCallback(handle);
            };


            int cancelTemporaryWithdrawalHandle = GamebaseCallbackHandler.RegisterCallback(cancelTemporaryWithdrawalCallback);

            if (CanLogout(cancelTemporaryWithdrawalHandle) == false)
            {
                return;
            }

            var requestVO = AuthMessage.GetCancelTemporaryWithdrawalMessage();

            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                GamebaseAnalytics.Instance.IdPCode = string.Empty;

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(cancelTemporaryWithdrawalHandle);
                if (callback == null)
                {
                    return;
                }
                GamebaseCallbackHandler.UnregisterCallback(cancelTemporaryWithdrawalHandle);

                if (error == null)
                {
                    var vo = JsonMapper.ToObject<AuthResponse.CancelTemporaryWithdrawalInfo>(response);

                    if (vo.header.isSuccessful == true)
                    {
                    }
                    else
                    {
                        error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain);
                    }
                }

                callback(error);
            });
        }


        public void IssueTransferAccount(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.TransferAccountInfo>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void QueryTransferAccount(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.TransferAccountInfo>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void RenewTransferAccount(GamebaseRequest.Auth.TransferAccountRenewConfiguration configuration, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.TransferAccountInfo>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void TransferAccountWithIdPLogin(string accountId, string accountPassword, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }
        
        public List<string> GetAuthMappingList()
        {
            GamebaseErrorNotifier.FireNotSupportedAPI(this);
            return null;
        }

        public string GetAuthProviderUserID(string providerName)
        {
            if (true == providerName.Equals(GamebaseAuthProvider.GUEST, StringComparison.Ordinal))
            {
                return string.Empty;
            }

            return AuthAdapterManager.Instance.GetIDPData<string>(providerName, AuthAdapterManager.MethodName.GET_IDP_USER_ID);
        }

        public string GetAuthProviderAccessToken(string providerName)
        {
            if (true == providerName.Equals(GamebaseAuthProvider.GUEST, StringComparison.Ordinal))
            {
                return string.Empty;
            }

            return AuthAdapterManager.Instance.GetIDPData<string>(providerName, AuthAdapterManager.MethodName.GET_IDP_ACCESS_TOKEN);
        }

        public GamebaseResponse.Auth.AuthProviderProfile GetAuthProviderProfile(string providerName)
        {
            if (GamebaseAuthProvider.GUEST == providerName)
            {
                GamebaseLog.Debug("Guest does not have profile information.", this);
                return null;
            }   

            if (true == string.IsNullOrEmpty(Gamebase.GetUserID()))
            {
                GamebaseLog.Debug(GamebaseStrings.NOT_LOGGED_IN, this);
                return null;
            }

            return AuthAdapterManager.Instance.GetIDPData<GamebaseResponse.Auth.AuthProviderProfile>(providerName, AuthAdapterManager.MethodName.GET_IDP_PROFILE);
        }

        public GamebaseResponse.Auth.BanInfo GetBanInfo()
        {
            var vo = DataContainer.GetData<AuthResponse.LoginInfo.ErrorExtras.Ban>(VOKey.Auth.BAN_INFO);
            var launchingVO = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
            var banVO = new GamebaseResponse.Auth.BanInfo();

            banVO.userId = vo.userId;
            banVO.banType = vo.banType;
            banVO.beginDate = vo.beginDate;
            banVO.endDate = vo.endDate;
            banVO.message = vo.message;
            banVO.csInfo = launchingVO.launching.app.customerService.accessInfo;
            banVO.csUrl = launchingVO.launching.app.customerService.url;
            return banVO;
        }

        public void IssueShortTermTicket(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<string>>(handle);
            if (callback == null)
            {
                return;
            }

            GamebaseCallbackHandler.UnregisterCallback(handle);

            byte[] bytesForEncoding = Encoding.UTF8.GetBytes(ISSUE_SHORT_TERM_TICKET_PURPOSE);
            string encodedString = Convert.ToBase64String(bytesForEncoding);

            GamebaseUtil.IssueShortTermTicket(
                encodedString,
                ISSUE_SHORT_TERM_TICKET_EXPIRESIN,
                Domain,
                callback);
        }

        private void LoginWithProviderName(string providerName, int handle)
        {
            if (false == AuthAdapterManager.Instance.IsSupportedIDP(providerName))
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                GamebaseErrorNotifier.FireNotSupportedAPI(
                    this,
                    callback,
                    string.Format("Login({0})", providerName));
                GamebaseCallbackHandler.UnregisterCallback(handle);
                return;
            }

            if (false == CanLogin(handle))
            {
                return;
            }

            if (GamebaseAuthProvider.GUEST == providerName)
            {
                var requestVO = AuthMessage.GetIDPLoginMessage(providerName);
                RequestGamebaseLogin(requestVO, handle);

                return;
            }

            bool hasAdapter = AuthAdapterManager.Instance.CreateIDPAdapter(providerName);

            if (false == hasAdapter)
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                callback(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, message: GamebaseStrings.AUTH_ADAPTER_NOT_FOUND_NEED_SETUP));
                GamebaseCallbackHandler.UnregisterCallback(handle);
            }

            AuthAdapterManager.Instance.IDPLogin((adapterError) =>
            {
                if (Gamebase.IsSuccess(adapterError))
                {
                    var IDPAccessToken = AuthAdapterManager.Instance.GetIDPData<string>(providerName, AuthAdapterManager.MethodName.GET_IDP_ACCESS_TOKEN);
                    var requestVO = AuthMessage.GetIDPLoginMessage(providerName, IDPAccessToken);
                    RequestGamebaseLogin(requestVO, handle);

                    return;
                }

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                if (null == callback)
                {
                    return;
                }

                GamebaseCallbackHandler.UnregisterCallback(handle);
                callback(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, Domain, error: adapterError));
                AuthAdapterManager.Instance.IDPLogout(providerName);
            });
        }

        private void LoginWithCredentialInfo(Dictionary<string, object> credentialInfo, int handle)
        {
            if (false == CanLogin(handle))
            {
                return;
            }

            if (null == credentialInfo ||
                false == credentialInfo.ContainsKey(GamebaseAuthProviderCredential.PROVIDER_NAME) ||
                (false == credentialInfo.ContainsKey(GamebaseAuthProviderCredential.ACCESS_TOKEN) && false == credentialInfo.ContainsKey(GamebaseAuthProviderCredential.AUTHORIZATION_CODE)))
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                callback(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_INVALID_IDP_INFO, Domain));
                GamebaseCallbackHandler.UnregisterCallback(handle);
                return;
            }

            var providerName = (string)credentialInfo[GamebaseAuthProviderCredential.PROVIDER_NAME];
            var accessToken = string.Empty;
            var authCode = string.Empty;

            if (true == credentialInfo.ContainsKey(GamebaseAuthProviderCredential.ACCESS_TOKEN))
            {
                accessToken = (string)credentialInfo[GamebaseAuthProviderCredential.ACCESS_TOKEN];
            }

            if (true == credentialInfo.ContainsKey(GamebaseAuthProviderCredential.AUTHORIZATION_CODE))
            {
                authCode = (string)credentialInfo[GamebaseAuthProviderCredential.AUTHORIZATION_CODE];
            }

            var requestVO = AuthMessage.GetIDPLoginMessage(providerName, accessToken, authCode);
            RequestGamebaseLogin(requestVO, handle);
        }

        protected bool CanLogin(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);

            if (true == GamebaseUnitySDK.IsInitialized)
            {
                if (false == CommonGamebaseLaunching.IsPlayable())
                {
                    if (null == callback)
                    {
                        GamebaseLog.Warn(GamebaseStrings.AUTH_NOT_PLAYABLE, this);
                        return false;
                    }
                    GamebaseCallbackHandler.UnregisterCallback(handle);
                    callback(null, new GamebaseError(GamebaseErrorCode.AUTH_NOT_PLAYABLE, Domain));
                    return false;
                }
            }
            else
            {
                if (null == callback)
                {
                    GamebaseLog.Warn(GamebaseStrings.NOT_INITIALIZED, this);
                    return false;
                }
                GamebaseCallbackHandler.UnregisterCallback(handle);
                callback(null, new GamebaseError(GamebaseErrorCode.NOT_INITIALIZED, Domain));
                return false;
            }

            if (true == string.IsNullOrEmpty(Gamebase.GetUserID()))
            {
                if(false == isAuthenticationAlreadyProgress)
                {
                    return true;
                }
                else
                {
                    if (null == callback)
                    {
                        GamebaseLog.Warn(GamebaseStrings.AUTH_ALREADY_IN_PROGRESS_ERROR, this);
                        return false;
                    }
                    GamebaseCallbackHandler.UnregisterCallback(handle);
                    callback(null, new GamebaseError(GamebaseErrorCode.AUTH_ALREADY_IN_PROGRESS_ERROR, Domain));
                    return false;
                }
            }

            if (null == callback)
            {
                return false;
            }

            GamebaseCallbackHandler.UnregisterCallback(handle);
            callback(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, Domain, GamebaseStrings.ALREADY_LOGGED_IN));
            return false;
        }

        protected bool CanLogout(int handle)
        {
            if (true == string.IsNullOrEmpty(Gamebase.GetUserID()))
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
                if (null == callback)
                {
                    return false;
                }
                GamebaseCallbackHandler.UnregisterCallback(handle);
                callback(new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return false;
            }

            return true;
        }

        protected bool CanTemporaryWithdrawal(int handle)
        {
            if (true == string.IsNullOrEmpty(Gamebase.GetUserID()))
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.TemporaryWithdrawalInfo>>(handle);
                if (null == callback)
                {
                    return false;
                }
                GamebaseCallbackHandler.UnregisterCallback(handle);
                callback(null, new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return false;
            }

            return true;
        }

        protected void RequestGamebaseLogin(WebSocketRequest.RequestVO requestVO, int handle)
        {
            isAuthenticationAlreadyProgress = true;

            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                if (null == callback)
                {
                    return;
                }

                GamebaseCallbackHandler.UnregisterCallback(handle);

                if (null == error)
                {
                    AuthRequest.LoginVO.Payload payload = JsonMapper.ToObject<AuthRequest.LoginVO.Payload>(requestVO.payload);
                    GamebaseAnalytics.Instance.IdPCode = payload.idPInfo.idPCode;

                    var vo = JsonMapper.ToObject<AuthResponse.LoginInfo>(response);
                    if (true == vo.header.isSuccessful)
                    {
                        DataContainer.SetData(VOKey.Auth.LOGIN_INFO, vo);
                        Heartbeat.Instance.StartHeartbeat();
                        Introspect.Instance.StartIntrospect();
                    }
                    else
                    {
                        error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain);
                        
                        if (vo.errorExtras != null && vo.errorExtras.ban != null)
                        {
                            DataContainer.SetData(VOKey.Auth.BAN_INFO, vo.errorExtras.ban);

                            GamebaseResponse.Auth.BanInfo banInfo = MakeBanInfo(vo.errorExtras.ban);
                            if (banInfo != null)
                            {
                                error.extras.Add("ban", JsonMapper.ToJson(banInfo));
                            }
                            GamebaseSystemPopup.Instance.ShowErrorPopup(error, vo);
                        }
                    }
                }
                else
                {
                    GamebaseSystemPopup.Instance.ShowErrorPopup(error);
                }

                isAuthenticationAlreadyProgress = false;

                if (error == null)
                {
                    GamebaseLog.Debug("ToastSdk UserId", this);
                    GamebaseResponse.Auth.AuthToken authToken = JsonMapper.ToObject<GamebaseResponse.Auth.AuthToken>(response);                    
                    ToastSdk.UserId = authToken.member.userId;
                    AuthRequest.LoginVO.Payload payload = JsonMapper.ToObject<AuthRequest.LoginVO.Payload>(requestVO.payload);
                    SetIapExtraData(payload.idPInfo.idPCode);
                    GamebaseLog.Debug("GamebaseIapManager Initialize", this);

                    PurchaseAdapterManager.Instance.Initialize();

                    callback(authToken, error);
                }
                else
                {
                    callback(null, error);
                }
            });
        }

        private GamebaseResponse.Auth.BanInfo MakeBanInfo(AuthResponse.LoginInfo.ErrorExtras.Ban vo)
        {           
            var launchingVO = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(VOKey.Launching.LAUNCHING_INFO);
            var banVO = new GamebaseResponse.Auth.BanInfo();

            banVO.userId = vo.userId;
            banVO.banType = vo.banType;
            banVO.beginDate = vo.beginDate;
            banVO.endDate = vo.endDate;
            banVO.message = vo.message;
            banVO.csInfo = launchingVO.launching.app.customerService.accessInfo;
            banVO.csUrl = launchingVO.launching.app.customerService.url;
            return banVO;
        }

        protected void CheckLaunchingStatusExpire(Action callback)
        {
            if (CanLaunchingStatusUpdate() == true)
            {
                GamebaseCallback.GamebaseDelegate<LaunchingResponse.LaunchingInfo> launchingInfoCallback = (launchingInfo, error) =>
                {
                    callback();
                };

                int handle = GamebaseCallbackHandler.RegisterCallback(launchingInfoCallback);

                GamebaseLaunchingImplementation.Instance.RequestLaunchingStatus(handle);

                return;
            }

            callback();
        }       

        private void SetIapExtraData(string providerName)
        {
            GamebaseLog.Debug("SetIapExtraData", this);
            ToastSdk.UserId = Gamebase.GetUserID();

            var userId = Gamebase.GetAuthProviderUserID(providerName);
            
            PurchaseAdapterManager.Instance.SetExtraData(
                new Dictionary<string, string>()
                {
                    {KEY_IAP_EXTRA_USER_ID,  userId}
                });
        }

        private void RemoveLoginData()
        {
            DataContainer.RemoveData(VOKey.Auth.LOGIN_INFO);            
            Heartbeat.Instance.StopHeartbeat();
            Introspect.Instance.StopIntrospect();
            AuthAdapterManager.Instance.IDPLogoutAll();
            PurchaseAdapterManager.Instance.Destroy();
            GamebaseAnalytics.Instance.ResetUserMeta(() =>
            {
                GamebaseIndicatorReport.SendIndicatorData(
                    GamebaseIndicatorReportType.LogType.TAA,
                    GamebaseIndicatorReportType.StabilityCode.GB_TAA_RESET_USER_LEVEL,
                    GamebaseIndicatorReportType.LogLevel.DEBUG,
                    new Dictionary<string, string>());
            });
        }

        public bool CanLaunchingStatusUpdate()
        {
            float statusElaspedTime = GamebaseLaunchingImplementation.Instance.GetStatusElaspedTime();
            if (statusElaspedTime >= LAUNCHING_STATUS_EXPIRE_TIME)
            {
                GamebaseLog.Debug(
                    string.Format(
                        "LaunchingStatusElaspedTime : {0} (Standard is more than {1})",
                        statusElaspedTime,
                        LAUNCHING_STATUS_EXPIRE_TIME),
                    this);
                return true;
            }

            return false;
        }
    }
}
#endif