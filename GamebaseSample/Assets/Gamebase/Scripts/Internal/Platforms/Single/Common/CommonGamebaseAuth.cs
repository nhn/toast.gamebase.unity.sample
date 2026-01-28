#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using GamePlatform.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using Toast.Gamebase.Internal.Auth;
using Toast.Gamebase.Internal.Result;
using Toast.Gamebase.Internal.Single.Communicator;
using Toast.Gamebase.LitJson;
using Toast.Gamebase.Internal.Auth;
using Toast.Gamebase.Internal.Auth.Browser;

namespace Toast.Gamebase.Internal.Single
{
    public class CommonGamebaseAuth : IGamebaseAuth
    {
        private string domain;
        private bool isAuthenticationAlreadyProgress = false;

        private const string KEY_IAP_EXTRA_USER_ID = "UserID";
        private const int LAUNCHING_STATUS_EXPIRE_TIME = 30;

        private GamebaseResponse.Auth.BanInfo banInfo;
        
        protected BrowserLoginService _browserLoginService = null;

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
            Login(providerName, null, handle);
        }

        public virtual void Login(string providerName, Dictionary<string, object> additionalInfo, int handle)
        {
            CheckCanLogin(additionalInfo, (res) =>
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                GamebaseCallbackHandler.UnregisterCallback(handle);
                
                if (Gamebase.IsSuccess(res.Error) == false)
                {
                    callback?.Invoke(null, res.Error);
                    return;
                }
                
                SignInWithIdp(providerName, additionalInfo, (idPContext, signInError) =>
                {
                    if (Gamebase.IsSuccess(signInError))
                    {
                        LoginWithIdp(idPContext, additionalInfo, (res) =>
                        {
                            OnLoginResult(res.Value, res.Error);

                            callback?.Invoke(res.Value, res.Error);

                            GamebaseIndicatorReport.Auth.LoginWithProvider(providerName, res.Value, res.Error);
                        });
                    }
                    else
                    {
                        callback?.Invoke(null, signInError);
                    }
                });
            });
        }

        protected virtual void SignInWithIdp(string providerName, Dictionary<string, object> additionalInfo, GamebaseCallback.GamebaseDelegate<IdPAuthContext> callback)
        {
            if (providerName.Equals(GamebaseAuthProvider.GUEST))
            {
                callback?.Invoke(new IdPAuthContext(GamebaseAuthProvider.GUEST), null);
            }
            else
            {
                BrowserLogin(providerName, additionalInfo, (idPContext, error) =>
                {
                    if (Gamebase.IsSuccess(error))
                    {
                        callback?.Invoke(idPContext, null);
                    }
                    else
                    {
                        callback?.Invoke(null, error);
                    }
                });
            }
        }
        
        private bool isLoginInProgress;
        private void BrowserLogin(string providerName, Dictionary<string, object> additionalInfo, GamebaseCallback.GamebaseDelegate<IdPAuthContext> callback)
        {
            if (isLoginInProgress)
            {
                callback?.Invoke(null, new GamebaseError(GamebaseErrorCode.AUTH_ALREADY_IN_PROGRESS_ERROR, Domain));
                return;
            }

            var launching = GamebaseLaunchingImplementation.Instance.GetLaunchingInformations();
            if(launching == null)
            {
                callback?.Invoke(null, new GamebaseError(GamebaseErrorCode.NOT_INITIALIZED));
                return;
            }
            if (launching.launching.app.idP.TryGetValue("gbid", out var value) == false)
            {
                callback?.Invoke(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, message: GamebaseStrings.AUTH_NOT_EXIST_MEMBER));
                return;
            }

            string subCode = null;
            if (string.Equals(providerName, GamebaseAuthProvider.LINE))
            {
                if (additionalInfo != null && additionalInfo.TryGetValue(GamebaseAuthProviderCredential.LINE_CHANNEL_REGION, out var region))
                {
                    subCode = region.ToString();
                }
            }

            isLoginInProgress = true;
            IdPUriBuilder uriBuilder = new IdPUriBuilder(providerName, value.clientId, subCode);
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, _browserLoginService.LoginWithBrowser(uriBuilder, (result) =>
            {
                if (result.IsSuccess)
                {
                    var idPContext = new IdPAuthContext(providerName)
                    {
                        session = result.Value,
                        subCode = subCode
                    };
                    callback?.Invoke(idPContext, null);
                }
                else
                {
                    callback?.Invoke(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, Domain, error: result.Error));
                }

                isLoginInProgress = false;
            }));
        }
   
        public virtual void CancelLoginWithExternalBrowser()
        {
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, _browserLoginService.CancelBrowserLogin());
        }
        
        public void Login(Dictionary<string, object> credentialInfo, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseCallbackHandler.UnregisterCallback(handle);
            if (IsValidCredential(credentialInfo) == false)
            {
                callback?.Invoke(null, new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_INVALID_IDP_INFO, Domain));
                return;
            }
            
            CheckCanLogin(credentialInfo, (res) =>
            {
                if (Gamebase.IsSuccess(res.Error) == false)
                {
                    callback?.Invoke(null, res.Error);
                    return;
                }

                if (credentialInfo.ContainsKey(GamebaseAuthProviderCredential.GAMEBASE_ACCESS_TOKEN))
                {
                    var provider = (string)credentialInfo.GetValueOrDefault(GamebaseAuthProviderCredential.PROVIDER_NAME);
                    var token = (string)credentialInfo.GetValueOrDefault(GamebaseAuthProviderCredential.GAMEBASE_ACCESS_TOKEN);
                    var subcode = (string)credentialInfo.GetValueOrDefault(GamebaseAuthProviderCredential.SUB_CODE);
                    var extraParams = (Dictionary<string, string>)credentialInfo.GetValueOrDefault(GamebaseAuthProviderCredential.EXTRA_PARAMS);
                    LoginWithToken(provider, token, subcode, extraParams, (res) =>
                    {
                        OnLoginResult(res.Value, res.Error);

                        callback?.Invoke(res.Value, res.Error);

                        GamebaseIndicatorReport.Auth.LoginWithCredential(credentialInfo, res.Value, res.Error);
                    });
                }
                else
                {
                    LoginWithCredential(credentialInfo, (res) =>
                    {
                        OnLoginResult(res.Value, res.Error);

                        callback?.Invoke(res.Value, res.Error);

                        GamebaseIndicatorReport.Auth.LoginWithCredential(credentialInfo, res.Value, res.Error);
                    });
                }
            });
        }       

        public void LoginForLastLoggedInProvider(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback);
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void LoginForLastLoggedInProvider(Dictionary<string, object> additionalInfo, int handle)
        {
            LoginForLastLoggedInProvider(handle);
        }

        public void ChangeLogin(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, int handle)
        {
            LoginWithToken(forcingMappingTicket.idPCode, forcingMappingTicket.accessToken, forcingMappingTicket.subCode, forcingMappingTicket.extraParams, (res) =>
            {
                OnLoginResult(res.Value, res.Error);

                var callback = GamebaseCallbackHandler
                    .GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                GamebaseCallbackHandler.UnregisterCallback(handle);
                callback?.Invoke(res.Value, res.Error);

                GamebaseIndicatorReport.Auth.ChangeLogin(forcingMappingTicket, res.Value, res.Error);
            });
        }

        public void AddMapping(string providerName, int handle)
        {
            AddMapping(providerName, null, handle);
        }

        public void AddMapping(string providerName, Dictionary<string, object> additionalInfo, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseCallbackHandler.UnregisterCallback(handle);
            
            CheckCanMapping(providerName, (error) =>
            {
                if (Gamebase.IsSuccess(error) == false)
                {
                    callback?.Invoke(null, error);
                    return;
                }

                SignInWithIdp(providerName, additionalInfo, (context, error) =>
                {
                    if (Gamebase.IsSuccess(error))
                    {
                        var payload = AuthMessage.GetIDPLoginPayload(context);
                        MappingFromPayload(JsonMapper.ToJson(payload), (authToken, error) =>
                        {
                            OnMappingResult(authToken, error);
                            
                            callback?.Invoke(null, error);
                            
                            GamebaseIndicatorReport.Auth.MappingWithProvider(providerName, error);
                        });
                    }
                    else
                    {
                        callback?.Invoke(null, error);
                    
                        GamebaseIndicatorReport.Auth.MappingWithProvider(providerName, error);
                    }
                });
            });
        }

        public void AddMapping(Dictionary<string, object> credentialInfo, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseCallbackHandler.UnregisterCallback(handle);
            
            if (IsValidCredential(credentialInfo) == false)
            {
                callback?.Invoke(null, new GamebaseError(GamebaseErrorCode.AUTH_ADD_MAPPING_INVALID_IDP_INFO, Domain));
                
                return;
            }

            IdPAuthContext authContext = new IdPAuthContext(credentialInfo);
            CheckCanMapping(authContext.idPCode, (error) =>
            {
                if (Gamebase.IsSuccess(error) == false)
                {
                    callback?.Invoke(null, error);
                    return;
                }
                
                var payload = AuthMessage.GetIDPLoginPayload(authContext);
                MappingFromPayload(JsonMapper.ToJson(payload), (authToken, error) =>
                {
                    OnMappingResult(authToken, error);
                            
                    callback?.Invoke(null, error);

                    GamebaseIndicatorReport.Auth.MappingWithCredential(credentialInfo, error);
                });
            });
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
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback, "AddMappingForcibly(credentialInfo)");
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }
        
        public void AddMappingForcibly(string providerName, string forcingMappingKey, Dictionary<string, object> additionalInfo, int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
            GamebaseErrorNotifier.FireNotSupportedAPI(this, callback, "AddMappingForcibly(additionalInfo)");
            GamebaseCallbackHandler.UnregisterCallback(handle);
        }

        public void AddMappingForcibly(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, int handle)
        {
            CheckCanMapping(forcingMappingTicket.idPCode, (error) =>
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken>>(handle);
                GamebaseCallbackHandler.UnregisterCallback(handle);
                
                if (Gamebase.IsSuccess(error) == false)
                {
                    callback?.Invoke(null, error);
                    return;
                }

                GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE,
                    WaitForAddMappingForcibly(forcingMappingTicket, (authToken, error) =>
                    {
                        OnMappingResult(authToken, error);

                        callback?.Invoke(authToken, error);
                        
                        GamebaseIndicatorReport.Auth.ForcingMapping(forcingMappingTicket, error);
                    }));
            });
        }

        public void RemoveMapping(string providerName, int handle)
        {
            CheckCanRemoveMapping(providerName, (error) =>
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
                GamebaseCallbackHandler.UnregisterCallback(handle);
                if (Gamebase.IsSuccess(error) == false)
                {
                    callback?.Invoke(error);
                    
                    return;
                }

                GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE,
                    WaitForRemoveMapping(providerName, (authToken, error) =>
                    {
                        OnMappingResult(authToken, error);

                        callback?.Invoke(error);
                    }));
            });
        }

        public void Logout(Dictionary<string, object> additionalInfo, int handle)
        {
            GamebaseCallback.ErrorDelegate logoutCallback = (error) =>
            {
                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);

                if (callback != null)
                {
                    callback(error);
                    GamebaseCallbackHandler.UnregisterCallback(handle);
                }
                
                GamebaseIndicatorReport.Auth.Logout(additionalInfo, error);
            };

            if (CanLogout() == false)
            {
                logoutCallback(new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }

            bool skipExpireGamebaseToken = false;
            if (additionalInfo != null)
            {
                if (additionalInfo.TryGetValue(GamebaseAuthProviderCredential.SKIP_EXPIRE_GAMEBASE_TOKEN, out object value))
                {
                    skipExpireGamebaseToken = value is bool ? (bool)value : false;
                }
            }

            if (skipExpireGamebaseToken)
            {
                RemoveLoginData();
                logoutCallback(null);
                
                return;
            }
            
            LogoutIdp(additionalInfo, (error => 
            {
                if (Gamebase.IsSuccess(error) == false)
                {
                    logoutCallback(error);
                    return;
                }

                var requestVO = AuthMessage.GetLogoutMessage();
                WebSocket.Instance.Request(requestVO, (response, error) =>
                {
                    GamebaseAnalytics.Instance.IdPCode = string.Empty;

                    GamebaseSystemPopup.Instance.ShowErrorPopup(error);

                    RemoveLoginData();

                    logoutCallback(error);
                });
            }));
            
            
        }

        public void Withdraw(int handle)
        {
            GamebaseCallback.ErrorDelegate withdrawCallback = (error) =>
            {
                GamebaseIndicatorReport.Auth.Withdraw(error);

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
                callback?.Invoke(error);

                GamebaseCallbackHandler.UnregisterCallback(handle);
            };

            if (CanLogout() == false)
            {
                withdrawCallback(new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }

            var requestVO = AuthMessage.GetWithdrawMessage();

            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                GamebaseAnalytics.Instance.IdPCode = string.Empty;
                
                if (error == null)
                {
                    var vo = JsonMapper.ToObject<AuthResponse.WithdrawInfo>(response);
                    if (vo.header.isSuccessful == true ||
                        // [Gamebase-Client/15] MEMBER_ALREADY_WITHDRAWN에러 일 경우 성공 처리
                        vo.header.resultCode == GamebaseServerErrorCode.MEMBER_ALREADY_WITHDRAWN)
                    {
                        RemoveLoginData();
                        AuthAdapterManager.Instance.IDPWithdrawAll();
                    }
                    else
                    {
                        error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain);
                        GamebaseSystemPopup.Instance.ShowErrorPopup(error);
                    }
                }
                else
                {
                    GamebaseSystemPopup.Instance.ShowErrorPopup(error);
                }
                withdrawCallback(error);
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
                GamebaseIndicatorReport.Auth.RequestTemporaryWithdraw(error);

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.GamebaseDelegate<GamebaseResponse.TemporaryWithdrawalInfo>>(handle);
                callback?.Invoke(data, error);

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
                GamebaseIndicatorReport.Auth.CancelTemporaryWithdraw(error);

                var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.ErrorDelegate>(handle);
                callback?.Invoke(error);

                GamebaseCallbackHandler.UnregisterCallback(handle);
            };

            if (CanLogout() == false)
            {
                cancelTemporaryWithdrawalCallback(new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }

            var requestVO = AuthMessage.GetCancelTemporaryWithdrawalMessage();

            WebSocket.Instance.Request(requestVO, (response, error) =>
            {
                GamebaseAnalytics.Instance.IdPCode = string.Empty;

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

                cancelTemporaryWithdrawalCallback(error);
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
            List<string> authMappingList = new List<string>();
            var authToken = DataContainer.GetData<GamebaseResponse.Auth.AuthToken>(PlatformKey.Auth.LOGIN_INFO);
            if (authToken != null)
            {
                foreach (var mapping in authToken.member.authList)
                {
                    authMappingList.Add(mapping.idPCode);
                }    
            }
                
            return authMappingList;
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
            return banInfo;
        }
        
        protected WebSocketOperation RequestBanContactUrl(string userId, Action<string, string> callback)
        {
            var launchingVO = DataContainer.GetData<LaunchingResponse.LaunchingInfo>(PlatformKey.Launching.LAUNCHING_INFO);
            if (launchingVO?.launching?.app?.customerService != null)
            {
                var customerService = launchingVO.launching.app.customerService;

                var parameter = new ShortTermTicketRequest.IssueShortTermTicketVO.Parameter()
                {
                    userId = userId,
                    purpose = ShortTermTicketConst.PURPOSE_OPEN_CONTACT_FOR_BANNED_USER,
                    expiresIn = ShortTermTicketConst.EXPIRESIN
                };
                    
                return GamebaseContact.Instance.RequestContactURL(parameter, null, (csUrl, error) =>
                {
                    if (Gamebase.IsSuccess(error))
                    {
                        callback(customerService.accessInfo, csUrl);
                    }
                    else
                    {
                        callback(customerService.accessInfo, string.Empty);
                    }
                });
            }
            else
            {
                callback(string.Empty, string.Empty);
            }

            return null;
        }

        private bool IsValidCredential(Dictionary<string, object> credentialInfo)
        {
            if (credentialInfo == null)
            {
                return false;
            }

            if (credentialInfo.ContainsKey(GamebaseAuthProviderCredential.PROVIDER_NAME) == false)
            {
                return false;
            }

            if (credentialInfo.ContainsKey(GamebaseAuthProviderCredential.ACCESS_TOKEN) == false &&
                credentialInfo.ContainsKey(GamebaseAuthProviderCredential.AUTHORIZATION_CODE) == false &&
                credentialInfo.ContainsKey(GamebaseAuthProviderCredential.GAMEBASE_ACCESS_TOKEN) == false)
            {
                return false;
            }

            return true;
        }

        protected bool CanLogout()
        {
            if (true == string.IsNullOrEmpty(Gamebase.GetUserID()))
            {
                return false;
            }

            return true;
        }
        
        protected void LogoutIdp(Dictionary<string, object> additionalInfo, GamebaseCallback.ErrorDelegate callback)
        {
            bool skipIdpLogout = false;
            if (additionalInfo != null)
            {
                if (additionalInfo.TryGetValue(GamebaseAuthProviderCredential.SKIP_IDP_LOGOUT, out object value))
                {
                    skipIdpLogout = value is bool ? (bool)value : false;
                }
            }

            if (skipIdpLogout)
            {
                callback?.Invoke(null);
                return;
            }
            
            AuthAdapterManager.Instance.IDPLogoutAll(callback);
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
    
        protected void CheckCanLogin(Dictionary<string, object> additionalInfo, Action<GamebaseResult<bool>> completed)
        {
            if (!GamebaseUnitySDK.IsInitialized)
            {
                completed?.Invoke(GamebaseResult<bool>.Failure(new GamebaseError(GamebaseErrorCode.NOT_INITIALIZED, Domain)));
                return;
            }
            
            if (isAuthenticationAlreadyProgress)
            {
                completed?.Invoke(GamebaseResult<bool>.Failure(new GamebaseError(GamebaseErrorCode.AUTH_ALREADY_IN_PROGRESS_ERROR, Domain)));
                return;
            }
            
            bool ignoreAlreadyLoggedIn = false;
            if (additionalInfo != null)
            {
                if (additionalInfo.TryGetValue(GamebaseAuthProviderCredential.IGNORE_ALREADY_LOGGED_IN, out object value))
                {
                    ignoreAlreadyLoggedIn = value is bool ? (bool)value : false;
                }
            }
            
            if (ignoreAlreadyLoggedIn == false && !string.IsNullOrEmpty(Gamebase.GetUserID()))
            {
                completed?.Invoke(GamebaseResult<bool>.Failure(new GamebaseError(GamebaseErrorCode.AUTH_IDP_LOGIN_FAILED, Domain, GamebaseStrings.ALREADY_LOGGED_IN)));
                return;
            }
            
            if (CanLaunchingStatusUpdate())
            {
                GamebaseCallback.GamebaseDelegate<LaunchingResponse.LaunchingInfo> launchingInfoCallback = (launchingInfo, error) =>
                {
                    if (!CommonGamebaseLaunching.IsPlayable())
                    {
                        completed?.Invoke(GamebaseResult<bool>.Failure(new GamebaseError(GamebaseErrorCode.AUTH_NOT_PLAYABLE, Domain)));
                    }
                    else
                    {
                        completed?.Invoke(GamebaseResult<bool>.Success(true));
                    }
                };

                int handle = GamebaseCallbackHandler.RegisterCallback(launchingInfoCallback);
                GamebaseLaunchingImplementation.Instance.RequestLaunchingStatus(handle);
            }
            else
            {
                completed?.Invoke(GamebaseResult<bool>.Success(true));
            }
        }
        
        protected void LoginWithIdp(IdPAuthContext authContext, Dictionary<string, object> additionalInfo, Action<GamebaseResult<GamebaseResponse.Auth.AuthToken>> callback)
        {
            WebSocketRequest.RequestVO vo = null;
            if (additionalInfo is not null && additionalInfo.TryGetValue("long_term_gamebase_access_token", out var isLongTerm))
            {
                vo = AuthMessage.GetIDPLoginMessage(authContext, Convert.ToBoolean(isLongTerm));
            }
            else
            {
                vo = AuthMessage.GetIDPLoginMessage(authContext, false);
            }

            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, WaitForLogin(vo, callback));
        }

        protected void LoginWithCredential(Dictionary<string, object> credentialInfo, Action<GamebaseResult<GamebaseResponse.Auth.AuthToken>> callback)
        {
            var vo = AuthMessage.GetIDPLoginMessage(new IdPAuthContext(credentialInfo));
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, WaitForLogin(vo, callback));
        }

        protected void LoginWithToken(string providerName, string accessToken, string subCode, Dictionary<string, string> extraParams,
            Action<GamebaseResult<GamebaseResponse.Auth.AuthToken>> callback)
        {
            var vo = AuthMessage.GetTokenLoginMessage(providerName, accessToken, subCode, extraParams);
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, WaitForLogin(vo, callback));
        }
        
        private IEnumerator WaitForLogin(WebSocketRequest.RequestVO requestVO, Action<GamebaseResult<GamebaseResponse.Auth.AuthToken>> completed)
        {
            GamebaseResponse.Auth.AuthToken authToken = null;
            GamebaseError error = null;

            try
            {
                isAuthenticationAlreadyProgress = true;
                
                string response = string.Empty;
                yield return WebSocket.Instance.Request(requestVO, (socketResponse, socketError) =>
                {
                    response = socketResponse;
                    error = socketError;
                });
                
                if (null != error)
                {
                    completed?.Invoke(GamebaseResult<GamebaseResponse.Auth.AuthToken>.Failure(new GamebaseError(GamebaseErrorCode.AUTH_TOKEN_LOGIN_FAILED, error: error)));
                    yield break;
                }
                
                var vo = JsonMapper.ToObject<AuthResponse.LoginInfo>(response);
                if (vo.header.isSuccessful)
                {
                    authToken = JsonMapper.ToObject<GamebaseResponse.Auth.AuthToken>(response);
                    completed?.Invoke(GamebaseResult<GamebaseResponse.Auth.AuthToken>.Success(authToken));
                }
                else
                {
                    error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain);
                    if (vo.errorExtras != null && vo.errorExtras.ban != null)
                    {
                        yield return RequestBanContactUrl(vo.errorExtras.ban .userId, (csInfo, csUrl) =>
                        {
                            var banVo = vo.errorExtras.ban;
                            banInfo = new GamebaseResponse.Auth.BanInfo
                            {
                                userId = banVo.userId,
                                banType = banVo.banType,
                                beginDate = banVo.beginDate,
                                endDate = banVo.endDate,
                                message = banVo.message,
                                csInfo = csInfo,
                                csUrl = csUrl
                            };
                        
                            error.extras.Add(GamebaseErrorExtras.BAN_INFO, JsonMapper.ToJson(banInfo));
                        });
                    }
                    completed?.Invoke(GamebaseResult<GamebaseResponse.Auth.AuthToken>.Failure(new GamebaseError(GamebaseErrorCode.AUTH_TOKEN_LOGIN_FAILED, error: error)));
                }
            }
            finally
            {
                isAuthenticationAlreadyProgress = false;
            }
        }
    
        void OnLoginResult(GamebaseResponse.Auth.AuthToken authToken, GamebaseError error)
        {
            if (Gamebase.IsSuccess(error))
            {
                DataContainer.SetData(PlatformKey.Auth.LOGIN_INFO, authToken);
                GpLogger.UserId = authToken.member.userId;

                banInfo = null;
            
                GamebaseLog.Debug("ToastSdk UserId", this);
                GamebaseAnalytics.Instance.IdPCode = authToken.token.sourceIdPCode;
                SetIapExtraData(authToken.token.sourceIdPCode);
                GamebaseLog.Debug("GamebaseIapManager Initialize", this);

                PurchaseAdapterManager.Instance.Initialize();

                Heartbeat.Instance.StartHeartbeat();

                if (authToken.token.extraParams is not null && authToken.token.extraParams.TryGetValue("useIntrospection", out var value) && value == "False")
                { }
                else
                {
                    Introspect.Instance.StartIntrospect();  
                }

                // Gamebase-Client/1590 : 초기화 이후 점검이 걸리고 30초 안에 로그인을 할 경우,
                //                        최대 2분동안 점검 상태인 것을 모르고 게임에 정상 진입할 수 있기 때문에,
                //                        로그인 직후에 Launching Status를 갱신한다.
                GamebaseLaunchingImplementation.Instance.RequestLaunchingStatus();
            }
            else
            {
                GamebaseSystemPopup.Instance.ShowErrorPopup(error);
            }
        }

        protected void CheckCanMapping(string providerName, GamebaseCallback.ErrorDelegate callback)
        {
            if (!GamebaseUnitySDK.IsInitialized)
            {
                callback?.Invoke(new GamebaseError(GamebaseErrorCode.NOT_INITIALIZED, Domain));
                return;
            }
            
            if (string.IsNullOrEmpty(Gamebase.GetUserID()))
            {
                callback?.Invoke( new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }

            if (providerName == GamebaseAuthProvider.GUEST)
            {
                callback?.Invoke(new GamebaseError(GamebaseErrorCode.AUTH_ADD_MAPPING_CANNOT_ADD_GUEST_IDP, Domain));
                return;
            }
            
            if (isAuthenticationAlreadyProgress)
            {
                callback?.Invoke(new GamebaseError(GamebaseErrorCode.AUTH_ALREADY_IN_PROGRESS_ERROR, Domain));
                return;
            }

            var mappingList = Gamebase.GetAuthMappingList();
            if (mappingList.Contains(providerName))
            {
                callback?.Invoke(new GamebaseError(GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_HAS_SAME_IDP, Domain));
                return;
            }
            
            
            if (CanLaunchingStatusUpdate())
            {
                GamebaseCallback.GamebaseDelegate<LaunchingResponse.LaunchingInfo> launchingInfoCallback = (launchingInfo, error) =>
                {
                    if (!CommonGamebaseLaunching.IsPlayable())
                    {
                        callback?.Invoke(new GamebaseError(GamebaseErrorCode.AUTH_NOT_PLAYABLE, Domain));
                    }
                    else
                    {
                        callback?.Invoke(null);    
                    }
                };

                int handle = GamebaseCallbackHandler.RegisterCallback(launchingInfoCallback);
                GamebaseLaunchingImplementation.Instance.RequestLaunchingStatus(handle);
            }
            else
            {
                callback?.Invoke(null);
            }
        }
        
        protected void CheckCanRemoveMapping(string providerName, GamebaseCallback.ErrorDelegate callback)
        {
            if (!GamebaseUnitySDK.IsInitialized)
            {
                callback?.Invoke(new GamebaseError(GamebaseErrorCode.NOT_INITIALIZED, Domain));
                return;
            }
            
            if (string.IsNullOrEmpty(Gamebase.GetUserID()))
            {
                callback?.Invoke( new GamebaseError(GamebaseErrorCode.NOT_LOGGED_IN, Domain));
                return;
            }
            
            if (Gamebase.GetLastLoggedInProvider() == providerName)
            {
                callback?.Invoke(new GamebaseError(GamebaseErrorCode.AUTH_REMOVE_MAPPING_LOGGED_IN_IDP, Domain));
                return;
            }
            
            if (CanLaunchingStatusUpdate())
            {
                GamebaseCallback.GamebaseDelegate<LaunchingResponse.LaunchingInfo> launchingInfoCallback = (launchingInfo, error) =>
                {
                    if (!CommonGamebaseLaunching.IsPlayable())
                    {
                        callback?.Invoke(new GamebaseError(GamebaseErrorCode.AUTH_NOT_PLAYABLE, Domain));
                    }
                    else
                    {
                        callback?.Invoke(null);    
                    }
                };

                int handle = GamebaseCallbackHandler.RegisterCallback(launchingInfoCallback);
                GamebaseLaunchingImplementation.Instance.RequestLaunchingStatus(handle);
            }
            else
            {
                callback?.Invoke(null);
            }
        }
        
        protected void MappingFromPayload(string loginPayload, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, WaitForAddMapping(loginPayload, (authToken, error) =>
            {
                callback?.Invoke(authToken, error);
            }));
        }
        
        void OnMappingResult(GamebaseResponse.Auth.AuthToken mappingAuthToken, GamebaseError error)
        {
            if (Gamebase.IsSuccess(error))
            {
                var authToken = DataContainer.GetData<GamebaseResponse.Auth.AuthToken>(PlatformKey.Auth.LOGIN_INFO);
                if (authToken != null)
                {
                    if (authToken.token.sourceIdPCode == GamebaseAuthProvider.GUEST)
                    {
                        DataContainer.SetData(PlatformKey.Auth.LOGIN_INFO, mappingAuthToken);
                    }
                    else
                    {
                        authToken.member.authList = mappingAuthToken.member.authList;
                    }    
                }
            }
            else
            {
                GamebaseSystemPopup.Instance.ShowErrorPopup(error);
            }
        }

        private IEnumerator WaitForAddMapping(string loginPayload, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseResponse.Auth.AuthToken mappingAuthToken = null;
            GamebaseError error = null;

            try
            {
                var mappingRequestVO = AuthMessage.GetAddMappingMessage(loginPayload);
                yield return WebSocket.Instance.Request(mappingRequestVO, (response, socketError) =>
                {
                    if (null != socketError)
                    {
                        error = socketError;
                    }
                    else
                    {
                        var vo = JsonMapper.ToObject<AuthResponse.MappingInfo>(response);
                        if(vo.header.isSuccessful == false)
                        {
                            error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(mappingRequestVO.transactionId, mappingRequestVO.apiId, vo.header, Domain);
                            if (error.code == GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER)
                            {
                                if (vo.errorExtras?.forcingMappingTicket != null)
                                {
                                    error.extras.Add(GamebaseErrorExtras.FORCING_MAPPING_TICKET,
                                        JsonMapper.ToJson(vo.errorExtras.forcingMappingTicket));
                                }
                            }
                        }
                        else
                        {
                            mappingAuthToken = JsonMapper.ToObject<GamebaseResponse.Auth.AuthToken>(response);
                        }
                    }
                });
            }
            finally
            {
                callback?.Invoke(mappingAuthToken, error);
            }
        }
        
        private IEnumerator WaitForAddMappingForcibly(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseResponse.Auth.AuthToken authToken = null;
            GamebaseError error = null;

            try
            {
                var requestVO = AuthMessage.GetAddMappingForciblyMessage(forcingMappingTicket);
                yield return WebSocket.Instance.Request(requestVO, (response, socketError) =>
                {
                    if (null != socketError)
                    {
                        error = socketError;
                    }
                    else
                    {
                        var vo = JsonMapper.ToObject<AuthResponse.MappingInfo>(response);
                        if(vo.header.isSuccessful == false)
                        {
                            error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain);
                            authToken = null;
                        }
                        else
                        {
                            authToken = JsonMapper.ToObject<GamebaseResponse.Auth.AuthToken>(response);    
                        }
                    }
                });
            }
            finally
            {
                callback?.Invoke(authToken, error);
            }
        }
        
        private IEnumerator WaitForRemoveMapping(string providerName, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseResponse.Auth.AuthToken authToken = null;
            GamebaseError error = null;

            try
            {
                var requestVO = AuthMessage.GetRemoveMappingMessage(providerName);
                yield return WebSocket.Instance.Request(requestVO, (response, socketError) =>
                {
                    if (null != socketError)
                    {
                        error = socketError;
                    }
                    else
                    {
                        var vo = JsonMapper.ToObject<AuthResponse.MappingInfo>(response);
                        if(vo.header.isSuccessful == false)
                        {
                            error = GamebaseErrorUtil.CreateGamebaseErrorByServerErrorCode(requestVO.transactionId, requestVO.apiId, vo.header, Domain);
                            authToken = null;
                        }
                        else
                        {
                            authToken = JsonMapper.ToObject<GamebaseResponse.Auth.AuthToken>(response);    
                        }
                    }
                });
            }
            finally
            {
                callback?.Invoke(authToken, error);
            }
        }

        private void SetIapExtraData(string providerName)
        {
            GamebaseLog.Debug("SetIapExtraData", this);

            var userId = Gamebase.GetAuthProviderUserID(providerName);
            
            PurchaseAdapterManager.Instance.SetExtraData(
                new Dictionary<string, string>()
                {
                    {KEY_IAP_EXTRA_USER_ID,  userId}
                });
        }

        private void RemoveLoginData()
        {
            DataContainer.RemoveData(PlatformKey.Auth.LOGIN_INFO);            
            Heartbeat.Instance.StopHeartbeat();
            Introspect.Instance.StopIntrospect();
            PurchaseAdapterManager.Instance.Destroy();
            GamebaseAnalytics.Instance.ResetUserMeta(() =>
            {
                GamebaseIndicatorReport.TTA.ResetUserLevel();
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