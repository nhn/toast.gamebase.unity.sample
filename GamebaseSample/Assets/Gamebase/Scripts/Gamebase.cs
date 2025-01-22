using System;
using System.Collections.Generic;
using Toast.Gamebase.Internal;

namespace Toast.Gamebase
{
    /// <summary>
    /// The Gamebase class is core of Gamebase service.
    /// </summary>
    public sealed class Gamebase
    {
        /// <summary>
        /// Gamebase shows warning and error logs only.
        /// To turn on system logs for the reference of development, call Gamebase.SetDebugMode(true).
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="isDebugMode">Debug features (like logging) are enabled if true, disabled if false.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SetDebugModeSample(bool isDebugMode)
        /// {
        ///     Gamebase.SetDebugMode(isDebugMode);
        /// }
        /// </code>
        /// </example>
        public static void SetDebugMode(bool isDebugMode)
        {
            GamebaseImplementation.Instance.SetDebugMode(isDebugMode);
        }

        /// <summary>
        /// Return true if the GamebaseException object is null or the error code is zero. 
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="error">The exception that occurred.</param>
        /// <returns>True if success, false if not.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void IsSuccessSample()
        /// {
        ///     Gamebase.Logout((error) => 
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("Logout succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("Logout failed. error is {0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static bool IsSuccess(GamebaseError error)
        {
            return (error == null || error.code == GamebaseErrorCode.SUCCESS);
        }

        /// <summary>
        /// This function initialize the Gamebase SDK with inspector settings.
        /// If this function is not called, the Gamebase SDK function will not work.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="callback">The callback provided will return a LaunchingInfo object that contains the infomations of Gamebase.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void InitializeWithInspectorSettingsSample()
        /// {
        ///     Gamebase.Initialize((launchingInfo, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("Initialization succeeded.");
        /// 
        ///             //Following notices are registered in the Gamebase Console
        ///             var notice = launchingInfo.launching.notice;
        ///             if (notice != null)
        ///             {
        ///                 if (string.IsNullOrEmpty(notice.message) == false)
        ///                 {
        ///                     Debug.Log(string.Format("title:{0}", notice.title));
        ///                     Debug.Log(string.Format("message:{0}", notice.message));
        ///                     Debug.Log(string.Format("url:{0}", notice.url));
        ///                 }
        ///             }
        /// 
        ///             // Status information of game app version set in the Gamebase Unity SDK initialization.
        ///             var status = launchingInfo.launching.status;
        /// 
        ///             // Game status code (e.g. Under maintenance, Update is required, Service has been terminated)
        ///             // refer to GamebaseLaunchingStatus
        ///             if (status.code == GamebaseLaunchingStatus.IN_SERVICE)
        ///             {
        ///                 // Service is now normally provided.
        ///             }
        ///             else
        ///             {
        ///                 switch (status.code)
        ///                 {
        ///                     case GamebaseLaunchingStatus.RECOMMEND_UPDATE:
        ///                     {
        ///                         // Update is recommended.
        ///                         break;
        ///                     }
        ///                     // ... 
        ///                     case GamebaseLaunchingStatus.INTERNAL_SERVER_ERROR:
        ///                     {
        ///                         // Error in internal server.
        ///                         break;
        ///                     }
        ///                 }
        ///             }
        ///         }
        ///         else
        ///         {
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("Initialization failed. error is {0}", error));
        ///             
        ///             if (error.code == GamebaseErrorCode.LAUNCHING_UNREGISTERED_CLIENT)
        ///             {
        ///                 GamebaseResponse.Launching.UpdateInfo updateInfo = GamebaseResponse.Launching.UpdateInfo.From(error);
        ///                 if (updateInfo != null)
        ///                 {
        ///                     // Update is require.
        ///                 }
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void Initialize(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo> callback)
        {
            GamebaseImplementation.Instance.Initialize(callback);
        }

        /// <summary>
        /// This function initialize the Gamebase SDK with Gamebase configuration. 
        /// If this function is not called, the Gamebase SDK function will not work.
        /// @since Added 1.6.0
        /// </summary>
        /// <param name="configuration">Settings for the Gamebase SDK.</param>
        /// <param name="callback">The callback provided will return a LaunchingInfo object that contains the infomations of Gamebase.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void InitializeWithConfigurationSample(string appID, string appVersion)
        /// {
        ///     var configuration = new GamebaseRequest.GamebaseConfiguration();
        ///     configuration.appID = appID;
        ///     configuration.appVersion = appVersion;
        ///     configuration.displayLanguageCode = GamebaseDisplayLanguageCode.English;
        /// #if UNITY_ANDROID
        ///     configuration.storeCode = GamebaseStoreCode.GOOGLE;
        /// #elif UNITY_IOS
        ///     configuration.storeCode = GamebaseStoreCode.APPSTORE;
        /// #elif UNITY_WEBGL
        ///     configuration.storeCode = GamebaseStoreCode.WEBGL;
        /// #elif UNITY_STANDALONE
        ///     configuration.storeCode = GamebaseStoreCode.WINDOWS;
        /// #else
        ///     configuration.storeCode = GamebaseStoreCode.WINDOWS;
        /// #endif
        ///     Gamebase.Initialize(configuration, (launchingInfo, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("Initialization succeeded.");
        /// 
        ///             //Following notices are registered in the Gamebase Console
        ///             var notice = launchingInfo.launching.notice;
        ///             if (notice != null)
        ///             {
        ///                 if (string.IsNullOrEmpty(notice.message) == false)
        ///                 {
        ///                     Debug.Log(string.Format("title:{0}", notice.title));
        ///                     Debug.Log(string.Format("message:{0}", notice.message));
        ///                     Debug.Log(string.Format("url:{0}", notice.url));
        ///                 }
        ///             }
        /// 
        ///             //Status information of game app version set in the Gamebase Unity SDK initialization.
        ///             var status = launchingInfo.launching.status;
        /// 
        ///             // Game status code (e.g. Under maintenance, Update is required, Service has been terminated)
        ///             // refer to GamebaseLaunchingStatus
        ///             if (status.code == GamebaseLaunchingStatus.IN_SERVICE)
        ///             {
        ///                 // Service is now normally provided.
        ///             }
        ///             else
        ///             {
        ///                 switch (status.code)
        ///                 {
        ///                     case GamebaseLaunchingStatus.RECOMMEND_UPDATE:
        ///                     {
        ///                         // Update is recommended.
        ///                         break;
        ///                     }
        ///                     // ... 
        ///                     case GamebaseLaunchingStatus.INTERNAL_SERVER_ERROR:
        ///                     {
        ///                         // Error in internal server.
        ///                         break;
        ///                     }
        ///                 }
        ///             }
        ///         }
        ///         else
        ///         {
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("Initialization failed. error is {0}", error));
        ///             
        ///             if (error.code == GamebaseErrorCode.LAUNCHING_UNREGISTERED_CLIENT)
        ///             {
        ///                 GamebaseResponse.Launching.UpdateInfo updateInfo = GamebaseResponse.Launching.UpdateInfo.From(error);
        ///                 if (updateInfo != null)
        ///                 {
        ///                     // Update is require.
        ///                 }
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void Initialize(GamebaseRequest.GamebaseConfiguration configuration, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Launching.LaunchingInfo> callback)
        {
            GamebaseImplementation.Instance.Initialize(configuration, callback);
        }

        /// <summary>
        /// Add a Gamebase event handler to be called when every events are arrived.
        /// You have to convert the message data to VO below according to the category value.
        ///  - GamebaseEventCategory.LOGGED_OUT : GamebaseEventLoggedOutData
        ///  - GamebaseEventCategory.IDP_REVOKED : GamebaseEventIdPRevokedData
        ///  - GamebaseEventCategory.SERVER_PUSH_APP_KICKOUT : GamebaseEventServerPushData
        ///  - GamebaseEventCategory.SERVER_PUSH_APP_KICKOUT_MESSAGE_RECEIVED : GamebaseEventServerPushData
        ///  - GamebaseEventCategory.SERVER_PUSH_TRANSFER_KICKOUT : GamebaseEventServerPushData
        ///  - GamebaseEventCategory.OBSERVER_LAUNCHING : GamebaseEventObserverData
        ///  - GamebaseEventCategory.OBSERVER_HEARTBEAT : GamebaseEventObserverData
        ///  - GamebaseEventCategory.OBSERVER_NETWORK : GamebaseEventObserverData
        ///  - GamebaseEventCategory.OBSERVER_WEBVIEW : GamebaseEventObserverData
        ///  - GamebaseEventCategory.OBSERVER_INTROSPECT : GamebaseEventObserverData
        ///  - GamebaseEventCategory.PURCHASE_UPDATED : PurchasableReceipt
        ///  - GamebaseEventCategory.PUSH_RECEIVED_MESSAGE : PushMessage
        ///  - GamebaseEventCategory.PUSH_CLICK_MESSAGE : PushMessage
        ///  - GamebaseEventCategory.PUSH_CLICK_ACTION : PushAction
        ///  
        /// SERVER_PUSH : Receive messages from the Gamebase server. 
        /// OBSERVER : This is an event that fires when the launch, login account(hearbeat), or network connection 'status changes', webview event, introspect fail event. 
        /// PURCHASE_UPDATED : Promotion payment events can be received. 
        /// PUSH_RECEIVED_MESSAGE : This event operates when a Push message is received.
        /// PUSH_CLICK_MESSAGE : This event is executed when the Push message is clicked.
        /// PUSH_CLICK_ACTION : This is an event that is triggered when the action button added through the rich message function is clicked.
        /// 
        /// @since Added 2.10.0
        /// </summary>
        /// <param name="eventHandler">The callback that will run.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void AddEventHandlerSample()
        /// {
        ///     Gamebase.AddEventHandler(GamebaseEventHandler);
        /// }
        /// 
        /// private void GamebaseEventHandler(GamebaseResponse.Event.GamebaseEventMessage message)
        /// {
        ///     switch (message.category)
        ///     {
        ///         case GamebaseEventCategory.LOGGED_OUT:
        ///             {
        ///                 GamebaseResponse.Event.GamebaseEventLoggedOutData loggedData = GamebaseResponse.Event.GamebaseEventLoggedOutData.From(message.data);
        ///                 if (loggedData != null)
        ///                 {
        ///                     // There was a problem with the access token.
        ///                     // Call login again.
        ///                 }
        ///                 break;
        ///             }
        ///         case GamebaseEventCategory.IDP_REVOKED:
        ///             {
        ///                 GamebaseResponse.Event.GamebaseEventIdPRevokedData idPRevokedData = GamebaseResponse.Event.GamebaseEventIdPRevokedData.From(message.data);
        ///                 if (idPRevokedData != null)
        ///                 {
        ///                     CheckIdpRevoked(idPRevokedData);
        ///                 }
        ///                 break;
        ///             }
        ///         case GamebaseEventCategory.SERVER_PUSH_APP_KICKOUT:
        ///         case GamebaseEventCategory.SERVER_PUSH_APP_KICKOUT_MESSAGE_RECEIVED:
        ///         case GamebaseEventCategory.SERVER_PUSH_TRANSFER_KICKOUT:
        ///             {
        ///                 GamebaseResponse.Event.GamebaseEventServerPushData serverPushData = GamebaseResponse.Event.GamebaseEventServerPushData.From(message.data);
        ///                 if (serverPushData != null)
        ///                 {
        ///                     CheckServerPush(message.category, serverPushData);
        ///                 }
        ///                 break;
        ///             }
        ///         case GamebaseEventCategory.OBSERVER_LAUNCHING:
        ///             {
        ///                 GamebaseResponse.Event.GamebaseEventObserverData observerData = GamebaseResponse.Event.GamebaseEventObserverData.From(message.data);
        ///                 if(observerData != null)
        ///                 {
        ///                     CheckLaunchingStatus(observerData);
        ///                 }
        ///                 break;
        ///             }
        ///         case GamebaseEventCategory.OBSERVER_NETWORK:
        ///             {
        ///                 GamebaseResponse.Event.GamebaseEventObserverData observerData = GamebaseResponse.Event.GamebaseEventObserverData.From(message.data);
        ///                 if (observerData != null)
        ///                 {
        ///                     CheckNetwork(observerData);
        ///                 }
        ///                 break;
        ///             }
        ///         case GamebaseEventCategory.OBSERVER_HEARTBEAT:
        ///             {
        ///                 GamebaseResponse.Event.GamebaseEventObserverData observerData = GamebaseResponse.Event.GamebaseEventObserverData.From(message.data);
        ///                 if (observerData != null)
        ///                 {
        ///                     CheckHeartbeat(observerData);
        ///                 }
        ///                 break;
        ///             }
        ///         case GamebaseEventCategory.OBSERVER_WEBVIEW:
        ///             {
        ///                 GamebaseResponse.Event.GamebaseEventObserverData observerData = GamebaseResponse.Event.GamebaseEventObserverData.From(message.data);
        ///                 if (observerData != null)
        ///                 {
        ///                     CheckWebView(observerData);
        ///                 }
        ///                 break;
        ///             }
        ///         case GamebaseEventCategory.OBSERVER_INTROSPECT:
        ///             {
        ///                 // Introspect error
        ///                 GamebaseResponse.Event.GamebaseEventObserverData observerData = GamebaseResponse.Event.GamebaseEventObserverData.From(message.data);
        ///                 int errorCode = observerData.code;
        ///                 string errorMessage = observerData.message;
        ///                 break;
        ///             }
        ///         case GamebaseEventCategory.PURCHASE_UPDATED:
        ///             {
        ///                 GamebaseResponse.Event.PurchasableReceipt purchasableReceipt = GamebaseResponse.Event.PurchasableReceipt.From(message.data);
        ///                 if (purchasableReceipt != null)
        ///                 {
        ///                     // If the user got item by 'Promotion Code',
        ///                     // this event will be occurred.
        ///                 }
        ///                 break;
        ///             }
        ///         case GamebaseEventCategory.PUSH_RECEIVED_MESSAGE:
        ///             {
        ///                 GamebaseResponse.Event.PushMessage pushMessage = GamebaseResponse.Event.PushMessage.From(message.data);
        ///                 if (pushMessage != null)
        ///                 {
        ///                     // When you received push message.
        ///                     
        ///                     // By converting the extras field of the push message to JSON,
        ///                     // you can get the custom information added by the user when sending the push.
        ///                     // (For Android, an 'isForeground' field is included so that you can check if received in the foreground state.
        ///                 }
        ///                 break;
        ///             }
        ///         case GamebaseEventCategory.PUSH_CLICK_MESSAGE:
        ///             {
        ///                 GamebaseResponse.Event.PushMessage pushMessage = GamebaseResponse.Event.PushMessage.From(message.data);
        ///                 if (pushMessage != null)
        ///                 {
        ///                     // When you clicked push message.
        ///                 }
        ///                 break;
        ///             }
        ///         case GamebaseEventCategory.PUSH_CLICK_ACTION:
        ///             {
        ///                 GamebaseResponse.Event.PushAction pushAction = GamebaseResponse.Event.PushAction.From(message.data);
        ///                 if (pushAction != null)
        ///                 {
        ///                     // When you clicked action button by 'Rich Message'.
        ///                 }
        ///                 break;
        ///             }
        ///     }
        /// }
        /// 
        /// private void CheckIdpRevoked(GamebaseResponse.Event.GamebaseEventIdPRevokedData idPRevokedData)
        /// {
        ///     switch (idPRevokedData.code)
        ///     {
        ///         case GamebaseIdPRevokedCode.WITHDRAW:
        ///             {
        ///                 // Call Withdraw API.
        ///                 Gamebase.Withdraw((error) => { });
        ///                 break;
        ///             }
        ///         case GamebaseIdPRevokedCode.OVERWRITE_LOGIN_AND_REMOVE_MAPPING:
        ///             {
        ///                 // You must call RemoveMapping after calling overwrite login.
        ///                 foreach (var idp in idPRevokedData.authMappingList)
        ///                 {
        ///                     var additional = new Dictionary<string, object>();
        ///                     additional.Add(GamebaseAuthProviderCredential.IGNORE_ALREADY_LOGGED_IN, true);
        ///    
        ///                     Gamebase.Login(idp, additional, (authToken, loginError) =>
        ///                     {
        ///                         if (Gamebase.IsSuccess(loginError) == true)
        ///                         {
        ///                             Gamebase.RemoveMapping(idPRevokedData.idPType, (mappingError) => { });
        ///                         }
        ///                     });
        ///                 }
        ///                 break;
        ///             }
        ///         case GamebaseIdPRevokedCode.REMOVE_MAPPING:
        ///             {
        ///                 // Call RemoveMapping API.
        ///                 Gamebase.RemoveMapping(idPRevokedData.idPType, (error) => { });
        ///                 break;
        ///             }
        ///     }
        /// }
        /// 
        /// private void CheckServerPush(string category, GamebaseResponse.Event.GamebaseEventServerPushData data)
        /// {
        ///     if (category.Equals(GamebaseEventCategory.SERVER_PUSH_APP_KICKOUT) == true)
        ///     {
        ///         // Kicked out from Gamebase server.(Maintenance, banned or etc.)
        ///         // And the game user closes the kickout pop-up.
        ///         // Return to title and initialize Gamebase again.
        ///     }
        ///     else if (category.Equals(GamebaseEventCategory.SERVER_PUSH_APP_KICKOUT_MESSAGE_RECEIVED) == true)
        ///     {
        ///         // Currently, the kickout pop-up is displayed.
        ///         // If your game is running, stop it.
        ///     }
        ///     else if (category.Equals(GamebaseEventCategory.SERVER_PUSH_TRANSFER_KICKOUT) == true)
        ///     {
        ///         // If the user wants to move the guest account to another device,
        ///         // if the account transfer is successful,
        ///         // the login of the previous device is released,
        ///         // so go back to the title and try to log in again.
        ///     }
        /// }
        /// 
        /// private void CheckLaunchingStatus(GamebaseResponse.Event.GamebaseEventObserverData observerData)
        /// {
        ///     switch (observerData.code)
        ///     {
        ///         case GamebaseLaunchingStatus.IN_SERVICE:
        ///             {
        ///                 // Service is now normally provided.
        ///                 break;
        ///             }
        ///         // ... 
        ///         case GamebaseLaunchingStatus.INTERNAL_SERVER_ERROR:
        ///             {
        ///                 // Error in internal server.
        ///                 break;
        ///             }
        ///     }
        /// }
        /// 
        /// private void CheckNetwork(GamebaseResponse.Event.GamebaseEventObserverData observerData)
        /// {
        ///     switch ((GamebaseNetworkType)observerData.code)
        ///     {
        ///         case GamebaseNetworkType.TYPE_NOT:
        ///             {
        ///                 // Network disconnected.
        ///                 break;
        ///             }
        ///         case GamebaseNetworkType.TYPE_MOBILE:
        ///         case GamebaseNetworkType.TYPE_WIFI:
        ///         case GamebaseNetworkType.TYPE_ANY:
        ///             {
        ///                 // Network connected.
        ///                 break;
        ///             }
        ///     }
        /// }
        /// 
        /// private void CheckHeartbeat(GamebaseResponse.Event.GamebaseEventObserverData observerData)
        /// {
        ///     switch (observerData.code)
        ///     {
        ///         case GamebaseErrorCode.INVALID_MEMBER:
        ///             {
        ///                 // You can check the invalid user session in here.
        ///                 // ex) After transferred account to another device.
        ///                 break;
        ///             }
        ///         case GamebaseErrorCode.BANNED_MEMBER:
        ///             {
        ///                 // You can check the banned user session in here.
        ///                 break;
        ///             }
        ///     }
        /// }
        /// 
        /// private void CheckWebView(GamebaseResponse.Event.GamebaseEventObserverData observerData)
        /// {
        ///     switch (observerData.code)
        ///     {
        ///         case GamebaseWebViewEventType.OPENED:
        ///             {
        ///                 // WebView opened.
        ///                 break;
        ///             }
        ///         case GamebaseWebViewEventType.CLOSED:
        ///             {
        ///                 // WebView closed.
        ///                 break;
        ///             }
        ///     }
        /// }
        /// </code>
        /// </example>
        public static void AddEventHandler(GamebaseCallback.DataDelegate<GamebaseResponse.Event.GamebaseEventMessage> eventHandler)
        {
            GamebaseImplementation.Instance.AddEventHandler(eventHandler);
        }

        /// <summary>
        /// Remove a event handler.
        /// @since Added 2.10.0
        /// </summary>
        /// <param name="eventHandler">The callback that will run.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void RemoveEventHandlerSample(GamebaseCallback.DataDelegate<GamebaseResponse.Event.GamebaseEventMessage> eventHandler)
        /// {
        ///     Gamebase.RemoveEventHandler(eventHandler);
        /// }        
        /// </code>
        /// </example>
        public static void RemoveEventHandler(GamebaseCallback.DataDelegate<GamebaseResponse.Event.GamebaseEventMessage> eventHandler)
        {
            GamebaseImplementation.Instance.RemoveEventHandler(eventHandler);
        }

        /// <summary>
        /// Remove all event handler.
        /// @since Added 2.10.0
        /// </summary>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void RemoveAllEventHandlerSample()
        /// {
        ///     Gamebase.RemoveAllEventHandler();
        /// }  
        /// </code>
        /// </example>
        public static void RemoveAllEventHandler()
        {
            GamebaseImplementation.Instance.RemoveAllEventHandler();
        }

        /// <summary>
        /// Try to authenticate by specifying an IdP type.
        /// Types that can be authenticated are declared in the GamebaseAuthProvider class.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="providerName">The provider name witch is authentication provider. </param>
        /// <param name="callback">Login result callback, returns the authentication token as a result of login.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void LoginSample()
        /// {
        ///     Gamebase.Login(GamebaseAuthProvider.XXX, (authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             if(authToken.member.temporaryWithdrawal != null)
        ///             {
        ///                 long gracePeriodDate = authToken.member.temporaryWithdrawal.gracePeriodDate;
        ///                 Debug.Log(string.Format("User is under temporary withdrawa. GracePeriodDate : {0}", error));
        ///             }            
        ///             else
        ///             {
        ///                 string userId = authToken.member.userId;
        ///                 Debug.Log(string.Format("Login succeeded. Gamebase userId is {0}", userId));
        ///             }
        ///         }
        ///         else
        ///         {
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("Login failed. error is {0}", error));
        ///             if (error.code == GamebaseErrorCode.BANNED_MEMBER)
        ///             {
        ///                 GamebaseResponse.Auth.BanInfo banInfo = GamebaseResponse.Auth.BanInfo.From(error);
        ///                 if (banInfo != null)
        ///                 {
        ///                 }
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void Login(string providerName, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.Login(providerName, callback);
        }

        /// <summary>
        /// There is information which must be included for login with some IdPs.
        /// In order to set such necessary information, this API is provided.
        /// You can enter those information to additionalInfo in the dictionary type. 
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="providerName">The providerName which is authentication provider.</param>
        /// <param name="additionalInfo">
        /// The additionalInfo which is additional information using for login.
        /// Refer to the following document for the entire additionalInfo.
        /// http://docs.toast.com/en/Game/Gamebase/en/oper-app/#authentication-information
        /// </param>
        /// <param name="callback">Login result callback, returns the authentication token as a result of login.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void LoginWithAdditionalInfoSample()
        /// {
        ///     var additionalInfo = new Dictionary&lt;string, object>
        ///     {
        ///         { "key", "value" }
        ///     };
        /// 
        ///     Gamebase.Login(GamebaseAuthProvider.XXX, additionalInfo, (authToken, error) => 
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             if(authToken.member.temporaryWithdrawal != null)
        ///             {
        ///                 long gracePeriodDate = authToken.member.temporaryWithdrawal.gracePeriodDate;
        ///                 Debug.Log(string.Format("User is under temporary withdrawa. GracePeriodDate : {0}", error));
        ///             }            
        ///             else
        ///             {
        ///                 string userId = authToken.member.userId;
        ///                 Debug.Log(string.Format("Login succeeded. Gamebase userId is {0}", userId));
        ///             }
        ///         }
        ///         else
        ///         {
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("Login failed. error is {0}", error));
        ///             if (error.code == GamebaseErrorCode.BANNED_MEMBER)
        ///             {
        ///                 GamebaseResponse.Auth.BanInfo banInfo = GamebaseResponse.Auth.BanInfo.From(error);
        ///                 if (banInfo != null)
        ///                 {
        ///                 }
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void Login(string providerName, Dictionary<string, object> additionalInfo, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.Login(providerName, additionalInfo, callback);
        }

        /// <summary>
        /// This game interface allows authentication to be made with SDK provided by IdP, before login to Gamebase with provided access token.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="credentialInfo">
        /// The credentialInfo which is credential of authentication provider.
        /// Refer to the following document for the entire credentialInfo.
        /// http://docs.toast.com/en/Game/Gamebase/en/unity-authentication/#login-with-credential
        /// </param>
        /// <param name="callback">Login result callback, returns the authentication token as a result of login.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void LoginWithCredentialInfoSample()
        /// {
        ///     var credentialInfo = new Dictionary&lt;string, object>
        ///     {
        ///         { GamebaseAuthProviderCredential.PROVIDER_NAME, GamebaseAuthProvider.XXX },
        ///         { GamebaseAuthProviderCredential.ACCESS_TOKEN, "${AccessToken}" }
        ///     };
        /// 
        ///     Gamebase.Login(credentialInfo, (authToken, error) => 
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             if(authToken.member.temporaryWithdrawal != null)
        ///             {
        ///                 long gracePeriodDate = authToken.member.temporaryWithdrawal.gracePeriodDate;
        ///                 Debug.Log(string.Format("User is under temporary withdrawa. GracePeriodDate : {0}", error));
        ///             }            
        ///             else
        ///             {
        ///                 string userId = authToken.member.userId;
        ///                 Debug.Log(string.Format("Login succeeded. Gamebase userId is {0}", userId));
        ///             }
        ///         }
        ///         else
        ///         {
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("Login failed. error is {0}", error));
        ///             if (error.code == GamebaseErrorCode.BANNED_MEMBER)
        ///             {
        ///                 GamebaseResponse.Auth.BanInfo banInfo = GamebaseResponse.Auth.BanInfo.From(error);
        ///                 if (banInfo != null)
        ///                 {
        ///                 }
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void Login(Dictionary<string, object> credentialInfo, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.Login(credentialInfo, callback);
        }

        /// <summary>
        /// Try login with the most recently logged-in IdP.
        /// If a token is expired or its authentication fails, return failure.
        /// Note that a login for the IdP should be implemented.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="callback">Login result callback, returns the authentication token as a result of login.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void LoginForLastLoggedInProviderSample()
        /// {
        ///     Gamebase.LoginForLastLoggedInProvider((authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             if(authToken.member.temporaryWithdrawal != null)
        ///             {
        ///                 long gracePeriodDate = authToken.member.temporaryWithdrawal.gracePeriodDate;
        ///                 Debug.Log(string.Format("User is under temporary withdrawa. GracePeriodDate : {0}", error));
        ///             }            
        ///             else
        ///             {
        ///                 string userId = authToken.member.userId;
        ///                 Debug.Log(string.Format("LoginForLastLoggedInProvider succeeded. Gamebase userId is {0}", userId));
        ///             }
        ///         }
        ///         else
        ///         {
        ///             if (error.code == GamebaseErrorCode.SOCKET_ERROR || error.code == GamebaseErrorCode.SOCKET_RESPONSE_TIMEOUT)
        ///             {
        ///                 Debug.Log(string.Format("Retry LoginForLastLoggedInProvider or notify an error message to the user. : {0}", error.message));
        ///             }
        ///             else
        ///             {
        ///                 if (string.IsNullOrEmpty(Gamebase.GetLastLoggedInProvider()) == true)
        ///                 {
        ///                     // Display the IdP select menu to user.
        ///                 }
        ///                 else
        ///                 {
        ///                     Gamebase.Login(Gamebase.GetLastLoggedInProvider(), (innerAuthToken, innerError) =>
        ///                     {
        ///                         if (Gamebase.IsSuccess(innerError) == true)
        ///                         {
        ///                             string userId = innerAuthToken.member.userId;
        ///                             Debug.Log(string.Format("LoginForLastLoggedInProvider succeeded. Gamebase userId is {0}", userId));
        ///                         }
        ///                         else
        ///                         {
        ///                             // Check the error code and handle the error appropriately.
        ///                             Debug.Log(string.Format("LoginForLastLoggedInProvider failed. error is {0}", innerError));
        ///                         }
        ///                     });
        ///                 }
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void LoginForLastLoggedInProvider(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.LoginForLastLoggedInProvider(callback);
        }
        
        /// <summary>
        /// Try login with the most recently logged-in IdP.
        /// If a token is expired or its authentication fails, return failure.
        /// Note that a login for the IdP should be implemented.
        /// @since Added 2.54.0.
        /// </summary>
        /// <param name="additionalInfo">The additionalInfo which is additional information using for login.</param>
        /// <param name="callback">Login result callback, returns the authentication token as a result of login.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void LoginForLastLoggedInProviderSample()
        /// {
        ///     var additionalInfo = new Dictionary<string, object>
        ///     {
        ///         { GamebaseAuthProviderCredential.SHOW_LOADING_ANIMATION, true }         // Android only
        ///     };
        ///
        ///     Gamebase.LoginForLastLoggedInProvider(additionalInfo, (authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             if(authToken.member.temporaryWithdrawal != null)
        ///             {
        ///                 long gracePeriodDate = authToken.member.temporaryWithdrawal.gracePeriodDate;
        ///                 Debug.Log(string.Format("User is under temporary withdrawa. GracePeriodDate : {0}", error));
        ///             }            
        ///             else
        ///             {
        ///                 string userId = authToken.member.userId;
        ///                 Debug.Log(string.Format("LoginForLastLoggedInProvider succeeded. Gamebase userId is {0}", userId));
        ///             }
        ///         }
        ///         else
        ///         {
        ///             if (error.code == GamebaseErrorCode.SOCKET_ERROR || error.code == GamebaseErrorCode.SOCKET_RESPONSE_TIMEOUT)
        ///             {
        ///                 Debug.Log(string.Format("Retry LoginForLastLoggedInProvider or notify an error message to the user. : {0}", error.message));
        ///             }
        ///             else
        ///             {
        ///                 if (string.IsNullOrEmpty(Gamebase.GetLastLoggedInProvider()) == true)
        ///                 {
        ///                     // Display the IdP select menu to user.
        ///                 }
        ///                 else
        ///                 {
        ///                     Gamebase.Login(Gamebase.GetLastLoggedInProvider(), (innerAuthToken, innerError) =>
        ///                     {
        ///                         if (Gamebase.IsSuccess(innerError) == true)
        ///                         {
        ///                             string userId = innerAuthToken.member.userId;
        ///                             Debug.Log(string.Format("LoginForLastLoggedInProvider succeeded. Gamebase userId is {0}", userId));
        ///                         }
        ///                         else
        ///                         {
        ///                             // Check the error code and handle the error appropriately.
        ///                             Debug.Log(string.Format("LoginForLastLoggedInProvider failed. error is {0}", innerError));
        ///                         }
        ///                     });
        ///                 }
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void LoginForLastLoggedInProvider(Dictionary<string, object> additionalInfo, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.LoginForLastLoggedInProvider(additionalInfo, callback);
        }

        /// <summary>
        /// Change logged in account with ForcingMappingTicket.
        /// @since Added 2.30.0.
        /// </summary>
        /// <param name="forcingMappingTicket">The mapping information which is necessary to log in another account.</param>
        /// <param name="callback">Resume mapping result callback, returns the authentication token as a result of mapping.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void ChangeLoginSample()
        /// {
        ///     Gamebase.AddMapping(GamebaseAuthProvider.XXX, (authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             // AddMapping succeeded.
        ///         }
        ///         else
        ///         {
        ///             // If you got this error code(AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) that means this user already has another account of the AuthProvider.XXX),
        ///             // You can call this method, Gamebase.addMappingForcibly() which can try to map forcibly with the AuthProvider.XXX.
        ///             if (error.code.Equals(GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) == true)
        ///             {
        ///                 // Before calling the mapping forcibly api, You should get this ForcingMappingTicket for this method parameter.
        ///                 GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket = GamebaseResponse.Auth.ForcingMappingTicket.From(error);
        /// 
        ///                 if (forcingMappingTicket == null)
        ///                 {
        ///                     // Unexpected error occurred. Contact Administrator.
        ///                 }
        /// 
        ///                 // Try to change log in account with the ForcingMappingTicket.
        ///                 Gamebase.ChangeLogin(forcingMappingTicket, (innerAuthToken, innerError) =>
        ///                 {
        ///                     if (Gamebase.IsSuccess(innerError) == true)
        ///                     {
        ///                         // Log in account changed successfully.
        ///                         string userId = innerAuthToken.member.userId;
        ///                         Debug.Log(string.Format("ChangeLogin succeeded. Gamebase userId is {0}", userId));
        ///                     }
        ///                     else
        ///                     {
        ///                         // Change login failed.
        ///                         // The UserID will not changed.
        ///                         Debug.Log(string.Format("ChangeLogin failed. error is {0}", innerError));
        ///                     }
        ///                 });
        ///             }
        ///             else
        ///             {
        ///                 // Add Mapping Forcibly Failed.
        ///                 // Check the error code and handle the error appropriately.
        ///                 Debug.Log(string.Format("AddMapping failed. error is {0}", error));
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void ChangeLogin(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.ChangeLogin(forcingMappingTicket, callback);
        }
        
        /// <summary>
        /// Try to log out from logged-in IdP. In many cases, the logout button is located on the game configuration screen. 
        /// Even if a logout is successful, a game user's data remain. 
        /// When it is successful, as authentication records with a corresponding IdP are removed, ID and passwords will be required for the next log-in process.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="callback">Callbacks the result of logout.(GamebaseError error)</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void LogoutSample()
        /// {
        ///     Gamebase.Logout((error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("Logout succeeded.");
        ///         }
        ///         else
        ///         {
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("Logout failed. error is {0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void Logout(GamebaseCallback.ErrorDelegate callback)
        {
            GamebaseAuthImplementation.Instance.Logout(callback);
        }

        /// <summary>
        /// Below shows an example of how a game user withdraws while logged-in.
        /// When a user is successfully withdrawn, the user's data interfaced with a login IdP will be deleted.
        /// The user can log in with the IdP again, and a new user's data will be created.
        /// It means user's withdrawal from Gamebase, not from IdP account.
        /// After a successful withdrawal, a log-out from IdP will be tried.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="callback">Callbacks the result of withdraw. (GamebaseError error)</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void WithdrawSample()
        /// {
        ///     Gamebase.Withdraw((error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("Withdraw succeeded.");
        ///         }
        ///         else
        ///         {
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("Withdraw failed. error is {0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void Withdraw(GamebaseCallback.ErrorDelegate callback)
        {
            GamebaseAuthImplementation.Instance.Withdraw(callback);
        }
        
        /// <summary>
        /// Try mapping to another IdP while logged-in to a specific IdP. 
        /// If an IdP account to map has already been integrated to another account, return the AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER (3302) error.
        /// Even if a mapping is successful, 'currently logged-in IdP' does not change.
        /// Mapping simply adds IdP integration.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="providerName">The providerName which is authentication provider.</param>
        /// <param name="callback">Mapping result callback, returns the authentication token as a result of mapping.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void AddMappingSample()
        /// {
        ///     Gamebase.AddMapping(GamebaseAuthProvider.XXX, (authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             string userId = authToken.member.userId;
        ///             Debug.Log(string.Format("AddMapping succeeded. Gamebase userId is {0}", userId));
        ///         }
        ///         else
        ///         {
        ///             // If you got this error code(AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) that means this user already has another account of the AuthProvider.XXX),
        ///             // You can call this method, Gamebase.addMappingForcibly() which can try to map forcibly with the AuthProvider.XXX.
        ///             if (error.code.Equals(GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) == true)
        ///             {
        ///                 // See AddMappingForcibly API.
        ///             }
        ///             else
        ///             {
        ///                 // Check the error code and handle the error appropriately.
        ///                 Debug.Log(string.Format("AddMapping failed. error is {0}", error));
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void AddMapping(string providerName, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.AddMapping(providerName, callback);
        }

        /// <summary>
        /// There is information which must be included for mapping with some IdPs.
        /// In order to set such necessary information, this API is provided.
        /// You can enter those information to additionalInfo in the dictionary type.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="providerName">The providerName which is authentication provider.</param>
        /// <param name="additionalInfo">
        /// The additionalInfo which is additional information using for mapping.
        /// Refer to the following document for the entire additionalInfo.
        /// http://docs.toast.com/en/Game/Gamebase/en/oper-app/#authentication-information
        /// </param>
        /// <param name="callback">Callbacks the results of mappings, returns the authentication token as a result of mappings.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void AddMappingWithAdditionalInfoSample()
        /// {
        ///     var additionalInfo = new Dictionary&lt;string, object>
        ///     {
        ///         { "key", "value" }
        ///     };
        /// 
        ///     Gamebase.AddMapping(GamebaseAuthProvider.XXX, additionalInfo, (authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             string userId = authToken.member.userId;
        ///             Debug.Log(string.Format("AddMapping succeeded. Gamebase userId is {0}", userId));
        ///         }
        ///         else
        ///         {
        ///             // If you got this error code(AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) that means this user already has another account of the AuthProvider.XXX),
        ///             // You can call this method, Gamebase.addMappingForcibly() which can try to map forcibly with the AuthProvider.XXX.
        ///             if (error.code.Equals(GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) == true)
        ///             {
        ///                 // See AddMappingForcibly with additionalInfo API.
        ///             }
        ///             else
        ///             {
        ///                 // Check the error code and handle the error appropriately.
        ///                 Debug.Log(string.Format("AddMapping failed. error is {0}", error));
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void AddMapping(string providerName, Dictionary<string, object> additionalInfo, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.AddMapping(providerName, additionalInfo, callback);
        }

        /// <summary>
        /// This game interface allows authentication to be made with SDK provided by IdP, before applying Gamebase AddMapping with provided access token.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="credentialInfo">The credentialInfo which is credential of authentication provider.</param>                
        /// <param name="callback">Mapping result callback, returns the authentication token as a result of mapping.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void AddMappingWithCredentialInfoSample()
        /// {
        ///     var credentialInfo = new Dictionary&lt;string, object>
        ///     {
        ///         { GamebaseAuthProviderCredential.PROVIDER_NAME, GamebaseAuthProvider.XXX },
        ///         { GamebaseAuthProviderCredential.ACCESS_TOKEN, "${AccessToken}" }
        ///     };
        /// 
        ///     Gamebase.AddMapping(credentialInfo, (authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             string userId = authToken.member.userId;
        ///             Debug.Log(string.Format("AddMapping succeeded. Gamebase userId is {0}", userId));
        ///         }
        ///         else
        ///         {
        ///             // If you got this error code(AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) that means this user already has another account of the AuthProvider.XXX),
        ///             // You can call this method, Gamebase.addMappingForcibly() which can try to map forcibly with the AuthProvider.XXX.
        ///             if (error.code.Equals(GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) == true)
        ///             {
        ///                 // See AddMappingForcibly with credentialInfo API.
        ///             }
        ///             else
        ///             {
        ///                 // Check the error code and handle the error appropriately.
        ///                 Debug.Log(string.Format("AddMapping failed. error is {0}", error));
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void AddMapping(Dictionary<string, object> credentialInfo, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.AddMapping(credentialInfo, callback);
        }

        /// <summary>
        /// This game interface allows authentication to be made with SDK provided by IdP, before applying Gamebase AddMapping with provided access token.
        /// @since Added 2.2.0.
        /// </summary>
        /// <param name="providerName">The providerName which is authentication provider.</param>
        /// <param name="forcingMappingKey">The key string which is necessary to map forcibly with the provider that is already mappped with another account.</param>
        /// <param name="callback">Mapping result callback, returns the authentication token as a result of mapping.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void AddMappingForciblySample(string idPName)
        /// {
        ///     Gamebase.AddMapping(idPName, (authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             string userId = authToken.member.userId;
        ///             Debug.Log(string.Format("AddMapping succeeded. Gamebase userId is {0}", userId));
        ///         }
        ///         else
        ///         {
        ///             // If you got this error code(AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) that means this user already has another account of the AuthProvider.XXX),
        ///             // You can call this method, Gamebase.addMappingForcibly() which can try to map forcibly with the AuthProvider.XXX.
        ///             if (error.code.Equals(GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) == true)
        ///             {
        ///                 // Before calling the mapping forcibly api, You should get this ForcingMappingTicket and ForcingMappingKey string for this method parameter.
        ///                 GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket = GamebaseResponse.Auth.ForcingMappingTicket.From(error);
        /// 
        ///                 if (forcingMappingTicket == null)
        ///                 {
        ///                     // Unexpected error occurred. Contact Administrator.
        ///                 }
        /// 
        ///                 // Try to add mapping forcibly with the ForcingMappingKey.
        ///                 Gamebase.AddMappingForcibly(idPName, forcingMappingTicket.forcingMappingKey, (innerAuthToken, innerError) =>
        ///                 {
        ///                     if (Gamebase.IsSuccess(innerError) == true)
        ///                     {
        ///                         string userId = innerAuthToken.member.userId;
        ///                         Debug.Log(string.Format("AddMappingForcibly succeeded. Gamebase userId is {0}", userId));
        ///                     }
        ///                     else
        ///                     {
        ///                         // Check the error code and handle the error appropriately.
        ///                         Debug.Log(string.Format("AddMappingForcibly failed. error is {0}", innerError));
        ///                     }
        ///                 });
        ///             }
        ///             else
        ///             {
        ///                 // Check the error code and handle the error appropriately.
        ///                 Debug.Log(string.Format("AddMapping failed. error is {0}", error));
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void AddMappingForcibly(string providerName, string forcingMappingKey, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.AddMappingForcibly(providerName, forcingMappingKey, callback);
        }

        /// <summary>
        /// Forcibly trying to map the currently authenticated user identifier of Gamebase with the credential of external authentication provider.
        /// @since Added 2.2.0.
        /// </summary>
        /// <param name="providerName">The providerName which is authentication provider.</param>
        /// <param name="forcingMappingKey">The key string which is necessary to map forcibly with the provider that is already mappped with another account.</param>
        /// <param name="additionalInfo">The additionalInfo which is additional information using for mapping.</param>
        /// <param name="callback">Callbacks the results of mappings, returns the authentication token as a result of mappings.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void AddMappingForciblyWithAdditionalInfoSample(string idPName, Dictionary&lt;string, object> additional)
        /// {
        ///     Gamebase.AddMapping(idPName, additional, (authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             // AddMapping succeeded.
        ///         }
        ///         else
        ///         {
        ///             // If you got this error code(AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) that means this user already has another account of the AuthProvider.XXX),
        ///             // You can call this method, Gamebase.addMappingForcibly() which can try to map forcibly with the AuthProvider.XXX.
        ///             if (error.code.Equals(GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) == true)
        ///             {
        ///                 // Before calling the mapping forcibly api, You should get this ForcingMappingTicket and ForcingMappingKey string for this method parameter.
        ///                 GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket = GamebaseResponse.Auth.ForcingMappingTicket.From(error);
        /// 
        ///                 if (forcingMappingTicket == null)
        ///                 {
        ///                     // Unexpected error occurred. Contact Administrator.
        ///                 }
        /// 
        ///                 // Try to add mapping forcibly with the ForcingMappingKey.
        ///                 Gamebase.AddMappingForcibly(idPName, forcingMappingTicket.forcingMappingKey, additional, (innerAuthToken, innerError) =>
        ///                 {
        ///                     if (Gamebase.IsSuccess(innerError) == true)
        ///                     {
        ///                         string userId = innerAuthToken.member.userId;
        ///                         Debug.Log(string.Format("AddMappingForcibly succeeded. Gamebase userId is {0}", userId));
        ///                     }
        ///                     else
        ///                     {
        ///                         // Check the error code and handle the error appropriately.
        ///                         Debug.Log(string.Format("AddMappingForcibly failed. error is {0}", innerError));
        ///                     }
        ///                 });
        ///             }
        ///             else
        ///             {
        ///                 // Check the error code and handle the error appropriately.
        ///                 Debug.Log(string.Format("AddMapping failed. error is {0}", error));
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void AddMappingForcibly(string providerName, string forcingMappingKey, Dictionary<string, object> additionalInfo, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.AddMappingForcibly(providerName, forcingMappingKey, additionalInfo, callback);
        }

        /// <summary>
        /// Forcibly trying to map the currently authenticated user identifier of Gamebase with the credential of external authentication provider.
        /// @since Added 2.2.0.
        /// </summary>
        /// <param name="credentialInfo">The credentialInfo which is credential of authentication provider.</param>
        /// <param name="forcingMappingKey">The key string which is necessary to map forcibly with the provider that is already mappped with another account.</param>
        /// <param name="callback">Mapping result callback, returns the authentication token as a result of mapping.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void AddMappingForciblyWithCredentialInfoSample(Dictionary&lt;string, object> credential)
        /// {
        ///     Gamebase.AddMapping(credential, (authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             // AddMapping succeeded.
        ///         }
        ///         else
        ///         {
        ///             // If you got this error code(AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) that means this user already has another account of the AuthProvider.XXX),
        ///             // You can call this method, Gamebase.addMappingForcibly() which can try to map forcibly with the AuthProvider.XXX.
        ///             if (error.code.Equals(GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) == true)
        ///             {
        ///                 // Before calling the mapping forcibly api, You should get this ForcingMappingTicket and ForcingMappingKey string for this method parameter.
        ///                 GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket = GamebaseResponse.Auth.ForcingMappingTicket.From(error);
        /// 
        ///                 if (forcingMappingTicket == null)
        ///                 {
        ///                     // Unexpected error occurred. Contact Administrator.
        ///                 }
        /// 
        ///                 // Try to add mapping forcibly with the ForcingMappingKey.
        ///                 Gamebase.AddMappingForcibly(credential, forcingMappingTicket.forcingMappingKey, (innerAuthToken, innerError) =>
        ///                 {
        ///                     if (Gamebase.IsSuccess(innerError) == true)
        ///                     {
        ///                         string userId = innerAuthToken.member.userId;
        ///                         Debug.Log(string.Format("AddMappingForcibly succeeded. Gamebase userId is {0}", userId));
        ///                     }
        ///                     else
        ///                     {
        ///                         // Check the error code and handle the error appropriately.
        ///                         Debug.Log(string.Format("AddMappingForcibly failed. error is {0}", innerError));
        ///                     }
        ///                 });
        ///             }
        ///             else
        ///             {
        ///                 // Check the error code and handle the error appropriately.
        ///                 Debug.Log(string.Format("AddMapping failed. error is {0}", error));
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void AddMappingForcibly(Dictionary<string, object> credentialInfo, string forcingMappingKey, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.AddMappingForcibly(credentialInfo, forcingMappingKey, callback);
        }

        /// <summary>
        /// Forcibly trying to map the currently authenticated user identifier of Gamebase with the credential of external authentication provider.
        /// @since Added 2.30.0.
        /// </summary>
        /// <param name="forcingMappingTicket">The mapping information which is necessary to map forcibly with the provider that is already mappped with another account.</param>
        /// <param name="callback">Mapping result callback, returns the authentication token as a result of mapping.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void AddMappingForciblySample()
        /// {
        ///     Gamebase.AddMapping(GamebaseAuthProvider.XXX, (authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             // AddMapping succeeded.
        ///         }
        ///         else
        ///         {
        ///             // If you got this error code(AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) that means this user already has another account of the AuthProvider.XXX),
        ///             // You can call this method, Gamebase.addMappingForcibly() which can try to map forcibly with the AuthProvider.XXX.
        ///             if (error.code.Equals(GamebaseErrorCode.AUTH_ADD_MAPPING_ALREADY_MAPPED_TO_OTHER_MEMBER) == true)
        ///             {
        ///                 // Before calling the mapping forcibly api, You should get this ForcingMappingTicket and ForcingMappingKey string for this method parameter.
        ///                 GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket = GamebaseResponse.Auth.ForcingMappingTicket.From(error);
        /// 
        ///                 if (forcingMappingTicket == null)
        ///                 {
        ///                     // Unexpected error occurred. Contact Administrator.
        ///                 }
        /// 
        ///                 // Try to add mapping forcibly with the ForcingMappingTicket.
        ///                 Gamebase.AddMappingForcibly(forcingMappingTicket, (innerAuthToken, innerError) =>
        ///                 {
        ///                     if (Gamebase.IsSuccess(innerError) == true)
        ///                     {
        ///                         string userId = innerAuthToken.member.userId;
        ///                         Debug.Log(string.Format("AddMappingForcibly succeeded. Gamebase userId is {0}", userId));
        ///                     }
        ///                     else
        ///                     {
        ///                         // Check the error code and handle the error appropriately.
        ///                         Debug.Log(string.Format("AddMappingForcibly failed. error is {0}", innerError));
        ///                     }
        ///                 });
        ///             }
        ///             else
        ///             {
        ///                 // Add Mapping Forcibly Failed.
        ///                 // Check the error code and handle the error appropriately.
        ///                 Debug.Log(string.Format("AddMapping failed. error is {0}", error));
        ///             }
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void AddMappingForcibly(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.AddMappingForcibly(forcingMappingTicket, callback);
        }
        
        /// <summary>
        /// Forcibly trying to map the currently authenticated user identifier of Gamebase with the credential of external authentication provider.
        /// Remove mapping with a specific IdP. 
        /// If IdP mapping is not removed, error will occur.  
        /// After mapping is removed, Gamebase processes logout of the IdP.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="providerName">The providerName which is authentication provider.</param>
        /// <param name="callback">Callbacks the result of removing the mapping.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void RemoveMappingSample()
        /// {
        ///     Gamebase.RemoveMapping(GamebaseAuthProvider.XXX, (error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("RemoveMapping succeeded.");
        ///         }
        ///         else
        ///         {
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("RemoveMapping failed. error is {0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void RemoveMapping(string providerName, GamebaseCallback.ErrorDelegate callback)
        {
            GamebaseAuthImplementation.Instance.RemoveMapping(providerName, callback);
        }

        /// <summary>
        /// Try to query the transfer account to Gamebase. If the transfer account is already issued, it returns.
        /// @since Added 2.2.0.
        /// </summary>
        /// <param name="callback">Callbacks the result of QueryTransferAccount</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void QueryTransferAccountSample()
        /// {
        ///     Gamebase.QueryTransferAccount((transferAccountInfo, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             // You already have ID/password for account transfer.
        /// 
        ///             var id = transferAccountInfo.account.id;
        ///             Debug.Log(string.Format("QueryTransferAccount succeeded. ID:{0}", id));
        ///         }
        ///         else
        ///         {
        ///             if (error.code == GamebaseErrorCode.AUTH_TRANSFERACCOUNT_NOT_EXIST)
        ///             {
        ///                 // You don't have ID for account transfer.
        ///                 return;
        ///             }
        /// 
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("QueryTransferAccount failed. error is {0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void QueryTransferAccount(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.TransferAccountInfo> callback)
        {
            GamebaseAuthImplementation.Instance.QueryTransferAccount(callback);
        }

        /// <summary>
        /// Try to issue the transfer account.
        /// @since Added 2.2.0.
        /// </summary>
        /// <param name="callback">Callbacks the result of IssueTransferAccount</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void IssueTransferAccountSample()
        /// {
        ///     Gamebase.QueryTransferAccount((transferAccountInfo, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             // You already have ID/password for account transfer.
        /// 
        ///             var id = transferAccountInfo.account.id;
        ///             Debug.Log(string.Format("QueryTransferAccount succeeded. ID:{0}", id));
        ///         }
        ///         else
        ///         {
        ///             if (error.code == GamebaseErrorCode.AUTH_TRANSFERACCOUNT_NOT_EXIST)
        ///             {
        ///                 IssueTransferAccount();
        ///                 return;
        ///             }
        /// 
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("QueryTransferAccount failed. error is {0}", error));
        ///         }
        ///     });
        /// }
        /// 
        /// public void IssueTransferAccount()
        /// {
        ///     Gamebase.IssueTransferAccount((transferAccountInfo, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             var id = transferAccountInfo.account.id;
        ///             var password = transferAccountInfo.account.password;
        ///             Debug.Log(string.Format("IssueTransferAccount succeeded. ID:{0}, Password:{1}", id, password));
        ///         }
        ///         else
        ///         {
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("IssueTransferAccount failed. error is {0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void IssueTransferAccount(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.TransferAccountInfo> callback)
        {
            GamebaseAuthImplementation.Instance.IssueTransferAccount(callback);
        }

        /// <summary>
        /// Try to renew the transfer account.
        /// If you want to renew the account automatically or manually,
        /// you should pass the proper TransferAccountRenewConfiguration object to the first parameter.
        /// @since Added 2.2.0.
        /// </summary>
        /// <param name="configuration">Configuration for Transfer Account renewal.</param>
        /// <param name="callback">Callbacks the result of RenewTransferAccountManualIdPassword</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void RenewTransferAccountSample(string accountId, string accountPassword)
        /// {
        ///     // Manual settings
        ///     GamebaseRequest.Auth.TransferAccountRenewConfiguration configuration = GamebaseRequest.Auth.TransferAccountRenewConfiguration.MakeManualRenewConfiguration(accountId, accountPassword);
        ///     //GamebaseRequest.Auth.TransferAccountRenewConfiguration configuration = GamebaseRequest.Auth.TransferAccountRenewConfiguration.MakeManualRenewConfiguration(accountPassword);
        /// 
        ///     // Auto settings
        ///     //var type = GamebaseRequest.Auth.TransferAccountRenewConfiguration.RenewalTargetType.ID_PASSWORD;
        ///     //GamebaseRequest.Auth.TransferAccountRenewConfiguration configuration = GamebaseRequest.Auth.TransferAccountRenewConfiguration.MakeAutoRenewConfiguration(type);
        /// 
        ///     Gamebase.RenewTransferAccount(configuration, (transferAccountInfo, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             // Renewing TransferAccount success.
        ///             Debug.Log(string.Format("RenewTransferAccount succeeded. ID:{0}, Password:{1}", transferAccountInfo.account.id, transferAccountInfo.account.password));
        ///         }
        ///         else
        ///         {
        ///             // Check the error code and handle the error appropriately.
        ///             Debug.Log(string.Format("RenewTransferAccount failed. error is {0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void RenewTransferAccount(GamebaseRequest.Auth.TransferAccountRenewConfiguration configuration, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.TransferAccountInfo> callback)
        {
            GamebaseAuthImplementation.Instance.RenewTransferAccount(configuration, callback);
        }

        /// <summary>
        /// Try to transfer account with transfer account id and password.
        /// If this process is finished successfully, it proceeds login process and return AuthToken.
        /// @since Added 2.2.0.
        /// </summary>
        /// <param name="accountId">TransferAccount id received from old device.</param>
        /// <param name="accountPassword">TransferAccount password from old device.</param>
        /// <param name="callback">Callbacks the result of TransferAccountWithIdPLogin</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void TransferAccountWithIdPLoginSample(string accountId, string accountPassword)
        /// {
        ///     Gamebase.TransferAccountWithIdPLogin(accountId, accountPassword, (authToken, error) =>
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("TransferAccountWithIdPLogin succeeded.");
        ///         }
        ///         else
        ///         {
        ///             // Check the error code and handle the error appropriately.
        ///             var log = new StringBuilder();
        ///             log.AppendLine(string.Format("TransferAccountWithIdPLogin failed. error is {0}", error));
        ///             
        ///             if (error.code == GamebaseErrorCode.AUTH_TRANSFERACCOUNT_BLOCK)
        ///             {
        ///                 GamebaseResponse.Auth.TransferAccountFailInfo transferAccountFailInfo = GamebaseResponse.Auth.TransferAccountFailInfo.From(error);
        ///                 if (transferAccountFailInfo != null)
        ///                 {
        ///                     log.AppendLine("---TransferAccountFailInfo---");
        ///                     log.AppendLine(GamebaseJsonUtil.ToPrettyJsonString(transferAccountFailInfo));
        ///                 }
        ///             }
        ///             
        ///             Debug.Log(log);
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void TransferAccountWithIdPLogin(string accountId, string accountPassword, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Auth.AuthToken> callback)
        {
            GamebaseAuthImplementation.Instance.TransferAccountWithIdPLogin(accountId, accountPassword, callback);
        }

        /// <summary>
        /// Return the list of IdPs mapped to user IDs.
        /// @since Added 1.4.0.
        /// </summary>
        /// <returns>The list of external authentication providers mapped to the current user identifier.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetAuthMappingListSample()
        /// {
        ///     List<string> mappingList = Gamebase.GetAuthMappingList();
        /// }
        /// </code>
        /// </example>
        public static List<string> GetAuthMappingList()
        {
            return GamebaseAuthImplementation.Instance.GetAuthMappingList();
        }

        /// <summary>
        /// Get User ID from externally authenticated SDK.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="providerName">The providerName which is authentication provider.</param>
        /// <returns>The user ID from the authentication provider.(</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetAuthProviderUserIDSample(string providerName)
        /// {
        ///     string authProviderUserID = Gamebase.GetAuthProviderUserID(providerName);
        ///     Debug.Log(string.Format("authProviderUserID:{0}", authProviderUserID));
        /// }
        /// </code>
        /// </example>
        public static string GetAuthProviderUserID(string providerName)
        {
            return GamebaseAuthImplementation.Instance.GetAuthProviderUserID(providerName);
        }

        /// <summary>
        /// Get Access Token from externally authentication SDK.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="providerName">The providerName which is authentication provider.</param>
        /// <returns>The access token from the authentication provider.(</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetAuthProviderAccessTokenSample(string providerName)
        /// {
        ///     string authProviderAccessToken = Gamebase.GetAuthProviderAccessToken(providerName);
        ///     Debug.Log(string.Format("authProviderAccessToken:{0}", authProviderAccessToken));
        /// }
        /// </code>
        /// </example>
        public static string GetAuthProviderAccessToken(string providerName)
        {
            return GamebaseAuthImplementation.Instance.GetAuthProviderAccessToken(providerName);
        }

        /// <summary>
        /// Get Profile from externally authenticated SDK.
        /// @since Added 1.4.0.
        /// </summary>
        /// <param name="providerName">The providerName which is authentication provider.</param>
        /// <returns>The profile from the authentication provider.(</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetAuthProviderProfileSample(string providerName)
        /// {
        ///     GamebaseResponse.Auth.AuthProviderProfile profile = Gamebase.GetAuthProviderProfile(providerName);
        /// }
        /// </code>
        /// </example>
        public static GamebaseResponse.Auth.AuthProviderProfile GetAuthProviderProfile(string providerName)
        {
            return GamebaseAuthImplementation.Instance.GetAuthProviderProfile(providerName);
        }

        /// <summary>
        /// Get the current version of the Gamebase SDK for Unity as a string.
        /// @since Added 1.4.0.
        /// </summary>
        /// <returns>The current version of the Gamebase SDK.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetSDKVersionSample()
        /// {
        ///     string sdkVersion = Gamebase.GetSDKVersion();
        ///     Debug.Log(string.Format("sdkVersion:{0}", sdkVersion));
        /// }
        /// </code>
        /// </example>
        public static string GetSDKVersion()
        {
            return GamebaseImplementation.Instance.GetSDKVersion();
        }

        /// <summary>
        /// Get User ID issued by Gamebase.
        /// @since Added 1.4.0.
        /// </summary>
        /// <returns>The user id that is currently logged in.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetUserIDSample()
        /// {
        ///     string userID = Gamebase.GetUserID();
        ///     Debug.Log(string.Format("userID:{0}", userID));
        /// }
        /// </code>
        /// </example>
        public static string GetUserID()
        {
            return GamebaseImplementation.Instance.GetUserID();
        }

        /// <summary>
        /// Get AccessToken issued by Gamebase.
        /// @since Added 1.4.0.
        /// </summary>
        /// <returns>The access token for Gamebase platform.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetAccessTokenSample()
        /// {
        ///     string accessToken = Gamebase.GetAccessToken();
        ///     Debug.Log(string.Format("accessToken:{0}", accessToken));
        /// }
        /// </code>
        /// </example>
        public static string GetAccessToken()
        {
            return GamebaseImplementation.Instance.GetAccessToken();
        }
        
        /// <summary>
        /// Get the last logged-in Provider Name in Gamebase. 
        /// @since Added 2.69.0.
        /// </summary>
        /// <param name="callback">Callback pass to API result.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void RequestLastLoggedInProviderSample()
        /// {
        ///     Gamebase.RequestLastLoggedInProvider(lastLoggedInProvider, error)=> 
        ///     {
        ///         if (Gamebase.IsSuccess(error) == true) 
        ///         {
        ///             Debug.Log(string.Format("lastLoggedInProvider:{0}", lastLoggedInProvider));
        ///         }
        ///         else 
        ///         {
        ///             // Failed.
        ///         }
        ///     });
        /// }
        ///     
        /// </code>
        /// </example>
        public static void RequestLastLoggedInProvider(GamebaseCallback.GamebaseDelegate<string> callback)
        {
            GamebaseImplementation.Instance.RequestLastLoggedInProvider(callback);
        }

        /// <summary>
        /// Get the last logged-in Provider Name in Gamebase.
        /// If the value has not been initialized yet, 'NOT_INITIALIZED_YET'.
        /// @since Added 1.4.0.
        /// </summary>
        /// <returns>The name that is last logged-in authentication provider.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetLastLoggedInProviderSample()
        /// {
        ///     string lastLoggedInProvider = Gamebase.GetLastLoggedInProvider();
        ///     Debug.Log(string.Format("lastLoggedInProvider:{0}", lastLoggedInProvider));
        /// } 
        /// </code>
        /// </example>
        public static string GetLastLoggedInProvider()
        {
            return GamebaseImplementation.Instance.GetLastLoggedInProvider();
        }

        /// <summary>
        /// Gets the language code set for the current device.
        /// @since Added 1.14.0.
        /// </summary>
        /// <returns>The language code that is currently set.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetDeviceLanguageCodeSample()
        /// {
        ///     string languageCode = Gamebase.GetDeviceLanguageCode();
        ///     Debug.Log(string.Format("languageCode:{0}", languageCode));
        /// }
        /// </code>
        /// </example>                 
        public static string GetDeviceLanguageCode()
        {
            return GamebaseImplementation.Instance.GetDeviceLanguageCode();
        }

        /// <summary>
        /// Gets the carrier code set for the current device.
        /// @since Added 1.4.0.
        /// </summary>
        /// <returns>The carrier code set for the current device.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetCarrierCodeSample()
        /// {
        ///     string carrierCode = Gamebase.GetCarrierCode();
        ///     Debug.Log(string.Format("carrierCode:{0}", carrierCode));
        /// }
        /// </code>
        /// </example>
        public static string GetCarrierCode()
        {
            return GamebaseImplementation.Instance.GetCarrierCode();
        }

        /// <summary>
        /// Gets the carrier name set for the current device.
        /// @since Added 1.4.0.
        /// </summary>
        /// <returns>The carrier name set for the current device.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetCarrierNameSample()
        /// {
        ///     string carrierName = Gamebase.GetCarrierName();
        ///     Debug.Log(string.Format("carrierName:{0}", carrierName));
        /// }  
        /// </code>
        /// </example>
        public static string GetCarrierName()
        {
            return GamebaseImplementation.Instance.GetCarrierName();
        }

        /// <summary>
        /// Gets the country code.
        /// First, get the country code set in USIM, 
        /// and if there is no USIM, get the country code set in the device.
        /// @since Added 1.4.0.
        /// </summary>
        /// <returns>The country code.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetCountryCodeSample()
        /// {
        ///     string countryCode = Gamebase.GetCountryCode();
        ///     Debug.Log(string.Format("countryCode:{0}", countryCode));
        /// }
        /// </code>
        /// </example>
        public static string GetCountryCode()
        {
            return GamebaseImplementation.Instance.GetCountryCode();
        }

        /// <summary>
        /// Gets the country code set in USIM. 
        /// @since Added 1.4.0.
        /// </summary>
        /// <returns>The country code that is set in USIM.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetCountryCodeOfUSIMSample()
        /// {
        ///     string countryCodeOfUSIM = Gamebase.GetCountryCodeOfUSIM();
        ///     Debug.Log(string.Format("countryCodeOfUSIM:{0}", countryCodeOfUSIM));
        /// }
        /// </code>
        /// </example>
        public static string GetCountryCodeOfUSIM()
        {
            return GamebaseImplementation.Instance.GetCountryCodeOfUSIM();
        }

        /// <summary>
        /// Gets the country code currently set on the device.
        /// @since Added 1.4.0.
        /// </summary>
        /// <returns>The country code currently set on the device.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetCountryCodeOfDeviceSample()
        /// {
        ///     string countryCodeOfDevice = Gamebase.GetCountryCodeOfDevice();
        ///     Debug.Log(string.Format("countryCodeOfDevice:{0}", countryCodeOfDevice));
        /// }
        /// </code>
        /// </example>
        public static string GetCountryCodeOfDevice()
        {
            return GamebaseImplementation.Instance.GetCountryCodeOfDevice();
        }

        /// <summary>
        /// Returns whether this project is sandbox mode or not.
        /// @since Added 1.4.0.
        /// </summary>
        /// <returns>Bool value whether this project is sandbox mode.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void IsSandboxSample()
        /// {
        ///     bool isSandbox = Gamebase.IsSandbox();
        ///     Debug.Log(string.Format("isSandbox:{0}", isSandbox));
        /// }
        /// </code>
        /// </example>
        public static bool IsSandbox()
        {
            return GamebaseImplementation.Instance.IsSandbox();
        }

        /// <summary>
        /// Sets the Gamebase displayLanguage.
        /// @since Added 1.7.0
        /// </summary>
        /// <param name="languageCode">The Gamebase displayLanguage. Please use the GamebaseDisplayLanguageCode class.</param>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void SetDisplayLanguageCodeSample()
        /// {
        ///     Gamebase.SetDisplayLanguageCode(GamebaseDisplayLanguageCode.English);
        /// }
        /// </code>
        /// </example>
        public static void SetDisplayLanguageCode(string languageCode)
        {
            GamebaseImplementation.Instance.SetDisplayLanguageCode(languageCode);
        }

        /// <summary>
        /// Gets the Gamebase displayLanguage.
        /// @since Added 1.7.0
        /// </summary>
        /// <returns>The Gamebase displayLanguage.</returns>
        /// <example> 
        /// Example Usage : 
        /// <code>
        /// public void GetDisplayLanguageCodeSample()
        /// {
        ///     string displayLanguage = Gamebase.GetDisplayLanguageCode();
        ///     Debug.Log(string.Format("displayLanguage:{0}", displayLanguage));
        /// }
        /// </code>
        /// </example>
        public static string GetDisplayLanguageCode()
        {
            return GamebaseImplementation.Instance.GetDisplayLanguageCode();
        }

        /// <summary>
        /// The Launching class contains several informations that is received from the server after application is successfully launched.
        /// @since Added 1.4.0.
        /// </summary>
        public static class Launching
        {
            /// <summary>
            /// Get various information provided by the launching server.
            /// @since Added 1.4.0.
            /// </summary>
            /// <returns>The Launching information.</returns>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void GetLaunchingInformationsSample()
            /// {            
            ///     GamebaseResponse.Launching.LaunchingInfo launchingInfo = Gamebase.Launching.GetLaunchingInformations();                
            /// }
            /// </code>
            /// </example>
            public static GamebaseResponse.Launching.LaunchingInfo GetLaunchingInformations()
            {
                return GamebaseLaunchingImplementation.Instance.GetLaunchingInformations();
            }

            /// <summary>
            /// Gets the launching state.
            /// @since Added 1.4.0.
            /// </summary>
            /// <returns>The launching status.</returns>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void GetLaunchingStatusSample()
            /// {
            ///     int launchingStatus = Gamebase.Launching.GetLaunchingStatus();
            ///     Debug.Log(string.Format("launchingStatus:{0}", launchingStatus));
            /// }
            /// </code>
            /// </example>
            public static int GetLaunchingStatus()
            {
                return GamebaseLaunchingImplementation.Instance.GetLaunchingStatus();
            }
        }

        /// <summary>
        /// The Purchase class provides several APIs related to purchasing processes.
        /// Before using these APIs, You should be logged in. Because every Purchase API need informations that are obtained from GamebaseServer.
        /// If you do not have been authenticated, you will get NOT_LOGGED_IN error.
        /// Refer to the following document for the Purchase flow.
        /// http://docs.toast.com/en/Game/Gamebase/en/unity-purchase/#purchase-flow
        /// @since Added 1.4.0.
        /// </summary>
        public static class Purchase
        {
            /// <summary>
            /// Call following API of an item to purchase by using itemSeq to send a purchase request. 
            /// When a game user cancels purchasing, the PURCHASE_USER_CANCELED error will be returned.
            /// @since Added 1.4.0.
            /// </summary>
            /// <param name="itemSeq">Represent to item ID.</param>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void RequestPurchaseSample(long itemSeq)
            /// {
            ///     Gamebase.Purchase.RequestPurchase(itemSeq, (purchasableReceipt, error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             var message = new StringBuilder();
            ///             message.AppendLine("RequestPurchase succeeded.");
            ///             message.AppendLine(string.Format("itemSeq:{0}", purchasableReceipt.itemSeq));
            ///             message.AppendLine(string.Format("price:{0}", purchasableReceipt.price));
            ///             message.AppendLine(string.Format("currency:{0}", purchasableReceipt.currency));
            /// 
            ///             // You will need paymentSeq and purchaseToken when calling the Consume API.
            ///             // Refer to the following document for the Consume API.
            ///             // http://docs.toast.com/en/Game/Gamebase/en/api-guide/#purchaseiap
            ///             message.AppendLine(string.Format("paymentSeq:{0}", purchasableReceipt.paymentSeq));
            ///             message.AppendLine(string.Format("purchaseToken:{0}", purchasableReceipt.purchaseToken));
            ///             Debug.Log(message);
            ///         }
            ///         else
            ///         {
            ///             if (error.code == GamebaseErrorCode.PURCHASE_USER_CANCELED)
            ///             {
            ///                 Debug.Log("User canceled purchase.");
            ///             }
            ///             else
            ///             {
            ///                 // Check the error code and handle the error appropriately.
            ///                 Debug.Log(string.Format("RequestPurchase failed. error is {0}", error));
            ///             }
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RequestPurchase(long itemSeq, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt> callback)
            {
                GamebasePurchaseImplementation.Instance.RequestPurchase(itemSeq, callback);
            }

            /// <summary>
            /// Call following API of an item to purchase by using gamebaseProductId to send a purchase request. 
            /// When a game user cancels purchasing, the PURCHASE_USER_CANCELED error will be returned.
            /// @since Added 2.11.0.
            /// </summary>
            /// <param name="gamebaseProductId">Represent to gamebase product ID.</param>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void RequestPurchaseSample(string gamebaseProductId)
            /// {
            ///     Gamebase.Purchase.RequestPurchase(gamebaseProductId, (purchasableReceipt, error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             var message = new StringBuilder();
            ///             message.AppendLine("RequestPurchase succeeded.");
            ///             message.AppendLine(string.Format("itemSeq:{0}", purchasableReceipt.itemSeq));
            ///             message.AppendLine(string.Format("price:{0}", purchasableReceipt.price));
            ///             message.AppendLine(string.Format("currency:{0}", purchasableReceipt.currency));
            /// 
            ///             // You will need paymentSeq and purchaseToken when calling the Consume API.
            ///             // Refer to the following document for the Consume API.
            ///             // http://docs.toast.com/en/Game/Gamebase/en/api-guide/#purchaseiap
            ///             message.AppendLine(string.Format("paymentSeq:{0}", purchasableReceipt.paymentSeq));
            ///             message.AppendLine(string.Format("purchaseToken:{0}", purchasableReceipt.purchaseToken));
            ///             Debug.Log(message);
            ///         }
            ///         else
            ///         {
            ///             if (error.code == GamebaseErrorCode.PURCHASE_USER_CANCELED)
            ///             {
            ///                 Debug.Log("User canceled purchase.");
            ///             }
            ///             else
            ///             {
            ///                 // Check the error code and handle the error appropriately.
            ///                 Debug.Log(string.Format("RequestPurchase failed. error is {0}", error));
            ///             }
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RequestPurchase(string gamebaseProductId, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt> callback)
            {
                GamebasePurchaseImplementation.Instance.RequestPurchase(gamebaseProductId, callback);
            }

            /// <summary>
            /// Call following API of an item to purchase by using gamebaseProductId to send a purchase request. 
            /// When a game user cancels purchasing, the PURCHASE_USER_CANCELED error will be returned.
            /// @since Added 2.11.0.
            /// </summary>
            /// <param name="gamebaseProductId">Represent to gamebase product ID.</param>
            /// <param name="payload">The input payload is delivered when the purchase is completed.</param>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void RequestPurchaseSample(string gamebaseProductId)
            /// {
            ///     string userPayload = "{\"description\":\"This is example\",\"channelId\":\"delta\",\"characterId\":\"abc\"}";
            ///     Gamebase.Purchase.RequestPurchase(gamebaseProductId, userPayload, (purchasableReceipt, error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             var message = new StringBuilder();
            ///             message.AppendLine("RequestPurchase succeeded.");
            ///             message.AppendLine(string.Format("itemSeq:{0}", purchasableReceipt.itemSeq));
            ///             message.AppendLine(string.Format("price:{0}", purchasableReceipt.price));
            ///             message.AppendLine(string.Format("currency:{0}", purchasableReceipt.currency));
            /// 
            ///             // You will need paymentSeq and purchaseToken when calling the Consume API.
            ///             // Refer to the following document for the Consume API.
            ///             // http://docs.toast.com/en/Game/Gamebase/en/api-guide/#purchaseiap
            ///             message.AppendLine(string.Format("paymentSeq:{0}", purchasableReceipt.paymentSeq));
            ///             message.AppendLine(string.Format("purchaseToken:{0}", purchasableReceipt.purchaseToken));
            ///             
            ///             // userPayload value entered when calling API
            ///             message.AppendLine(string.Format("userPayload:{0}", purchasableReceipt.payload));
            ///             Debug.Log(message);
            ///         }
            ///         else
            ///         {
            ///             if (error.code == GamebaseErrorCode.PURCHASE_USER_CANCELED)
            ///             {
            ///                 Debug.Log("User canceled purchase.");
            ///             }
            ///             else
            ///             {
            ///                 // Check the error code and handle the error appropriately.
            ///                 Debug.Log(string.Format("RequestPurchase failed. error is {0}", error));
            ///             }
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RequestPurchase(string gamebaseProductId, string payload, GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt> callback)
            {
                GamebasePurchaseImplementation.Instance.RequestPurchase(gamebaseProductId, payload, callback);
            }

            /// <summary>
            /// Request for a list of non-consumed items, which have not been normally consumed (delivered, or provided) after purchase.
            /// In case of non-purchased items, ask the game server (item server) to proceed with item delivery (supply).
            /// @since Added 2.45.0.
            /// </summary>
            /// <param name="configuration">Set query conditions.</param>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example>
            /// Example Usage : 
            /// <code>
            /// public void RequestItemListOfNotConsumedSample(bool allStores)
            /// {
            ///     var configuration = new GamebaseRequest.Purchase.PurchasableConfiguration
            ///     {
            ///         allStores = allStores
            ///     };
            /// 
            ///     Gamebase.Purchase.RequestItemListOfNotConsumed(configuration, (purchasableReceiptList, error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("RequestItemListOfNotConsumed succeeded.");
            /// 
            ///             foreach (GamebaseResponse.Purchase.PurchasableReceipt purchasableReceipt in purchasableReceiptList)
            ///             {
            ///                 var message = new StringBuilder();
            ///                 message.AppendLine("RequestPurchase succeeded.");
            ///                 message.AppendLine(string.Format("itemSeq:{0}", purchasableReceipt.itemSeq));
            ///                 message.AppendLine(string.Format("price:{0}", purchasableReceipt.price));
            ///                 message.AppendLine(string.Format("currency:{0}", purchasableReceipt.currency));
            ///                 
            ///                 // You will need paymentSeq and purchaseToken when calling the Consume API.
            ///                 // Refer to the following document for the Consume API.
            ///                 // http://docs.toast.com/en/Game/Gamebase/en/api-guide/#purchaseiap
            ///                 message.AppendLine(string.Format("paymentSeq:{0}", purchasableReceipt.paymentSeq));
            ///                 message.AppendLine(string.Format("purchaseToken:{0}", purchasableReceipt.purchaseToken));
            ///                 Debug.Log(message);
            /// 
            ///                 // Calling the Consume API.
            ///             }
            ///         }
            ///         else
            ///         {
            ///             // Check the error code and handle the error appropriately.
            ///             Debug.Log(string.Format("RequestItemListOfNotConsumed failed. error is {0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RequestItemListOfNotConsumed(GamebaseRequest.Purchase.PurchasableConfiguration configuration, GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableReceipt>> callback)
            {
                GamebasePurchaseImplementation.Instance.RequestItemListOfNotConsumed(configuration, callback);
            }
            
            /// <summary>
            /// Request for a list of non-consumed items, which have not been normally consumed (delivered, or provided) after purchase.
            /// In case of non-purchased items, ask the game server (item server) to proceed with item delivery (supply).
            /// @since Added 1.4.0.
            /// @deprecated As of release 2.45.0.
            /// </summary>
            /// <param name="callback">Callback pass to API result.</param>
            [Obsolete]
            public static void RequestItemListOfNotConsumed(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableReceipt>> callback)
            {
                GamebasePurchaseImplementation.Instance.RequestItemListOfNotConsumed(new GamebaseRequest.Purchase.PurchasableConfiguration(), callback);
            }

            /// <summary>
            /// Request a item list which is purchasable. 
            /// This list has items which are registered in both Market(AppStore) and ToastCloud IAP Console.
            /// @since Added 1.4.0.
            /// </summary>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void RequestItemListPurchasableSample()
            /// {
            ///     Gamebase.Purchase.RequestItemListPurchasable((purchasableItemList, error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("RequestItemListPurchasable succeeded");
            /// 
            ///             foreach (GamebaseResponse.Purchase.PurchasableItem purchasableItem in purchasableItemList)
            ///             {
            ///                 var message = new StringBuilder();
            ///                 message.AppendLine(string.Format("itemSeq:{0}", purchasableItem.itemSeq));
            ///                 message.AppendLine(string.Format("price:{0}", purchasableItem.price));
            ///                 message.AppendLine(string.Format("currency:{0}", purchasableItem.currency));
            ///                 message.AppendLine(string.Format("itemName:{0}", purchasableItem.itemName));
            ///                 message.AppendLine(string.Format("marketItemId:{0}", purchasableItem.marketItemId));
            ///                 Debug.Log(message);
            ///             }
            ///         }
            ///         else
            ///         {
            ///             // Check the error code and handle the error appropriately.
            ///             Debug.Log(string.Format("RequestItemListPurchasable failed. error is {0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RequestItemListPurchasable(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableItem>> callback)
            {
                GamebasePurchaseImplementation.Instance.RequestItemListPurchasable(callback);
            }

            /// <summary>
            /// Request a item list which is purchasable. 
            /// This list has items which are only registered in ToastCloud IAP Console, not Market(Google, TStore)
            /// @since Added 1.4.0.
            /// </summary>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void RequestItemListAtIAPConsoleSample()
            /// {
            ///     Gamebase.Purchase.RequestItemListAtIAPConsole((purchasableItemList, error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("RequestItemListAtIAPConsole succeeded");
            /// 
            ///             foreach (GamebaseResponse.Purchase.PurchasableItem purchasableItem in purchasableItemList)
            ///             {
            ///                 var message = new StringBuilder();
            ///                 message.AppendLine(string.Format("itemSeq:{0}", purchasableItem.itemSeq));
            ///                 message.AppendLine(string.Format("price:{0}", purchasableItem.price));
            ///                 message.AppendLine(string.Format("currency:{0}", purchasableItem.currency));
            ///                 message.AppendLine(string.Format("itemName:{0}", purchasableItem.itemName));
            ///                 message.AppendLine(string.Format("marketItemId:{0}", purchasableItem.marketItemId));
            ///                 Debug.Log(message);
            ///             }
            ///         }
            ///         else
            ///         {
            ///             // Check the error code and handle the error appropriately.
            ///             Debug.Log(string.Format("RequestItemListAtIAPConsole failed. error is {0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RequestItemListAtIAPConsole(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableItem>> callback)
            {
                GamebasePurchaseImplementation.Instance.RequestItemListAtIAPConsole(callback);
            }

            /// <summary>
            /// If you want to deliver some items that have been purchased to user, you should implement this handler.
            /// You can receive a GamebaseResponse.Purchase.PurchasableReceipt object and receive whether error or success by callback.
            /// This API return same information with 'RequestPurchase'
            /// You should call this method after `logged in`, and you should call this before an other APIs in `Gamebase.Purchase`.
            /// This API can work on iOS11 or later.
            /// @since Added 1.13.0.
            /// </summary>
            /// <param name="callback">Callback may return the result of purchase by App Store Promotion.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SetPromotionIAPHandlerSample()
            /// {
            ///     Gamebase.Login(GamebaseAuthProvider.XXX, (authToken, error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Gamebase.Purchase.SetPromotionIAPHandler((purchasableReceipt, innerError) =>
            ///             {
            ///                 if (Gamebase.IsSuccess(innerError) == true)
            ///                 {
            ///                     Debug.Log("SetPromotionIAPHandler succeeded");
            /// 
            ///                     var message = new StringBuilder();
            ///                     message.AppendLine(string.Format("itemSeq:{0}", purchasableReceipt.itemSeq));
            ///                     message.AppendLine(string.Format("price:{0}", purchasableReceipt.price));
            ///                     message.AppendLine(string.Format("currency:{0}", purchasableReceipt.currency));
            /// 
            ///                     // You will need paymentSeq and purchaseToken when calling the Consume API.
            ///                     // Refer to the following document for the Consume API.
            ///                     // http://docs.toast.com/en/Game/Gamebase/en/api-guide/#purchaseiap
            ///                     message.AppendLine(string.Format("paymentSeq:{0}", purchasableReceipt.paymentSeq));
            ///                     message.AppendLine(string.Format("purchaseToken:{0}", purchasableReceipt.purchaseToken));
            ///                     Debug.Log(message);
            ///                 }
            ///                 else
            ///                 {
            ///                     if (innerError.code == GamebaseErrorCode.PURCHASE_USER_CANCELED)
            ///                     {
            ///                         Debug.Log("User canceled purchase.");
            ///                     }
            ///                     else
            ///                     {
            ///                         // Check the error code and handle the error appropriately.
            ///                         Debug.Log(string.Format("SetPromotionIAPHandler failed. innerError is {0}", innerError));
            ///                     }
            ///                 }
            ///             });
            ///         }
            ///         else
            ///         {
            ///             // Check the error code and handle the error appropriately.
            ///             Debug.Log(string.Format("Login failed. error is {0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void SetPromotionIAPHandler(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableReceipt> callback)
            {
                GamebasePurchaseImplementation.Instance.SetPromotionIAPHandler(callback);
            }

            /// <summary>
            /// Request a list of payment products that are currently subscribed.
            /// If there is a subscription purchased from another platform (such as iOS)
            /// @since Added 2.45.0.            
            /// </summary> 
            /// <param name="configuration">Set query conditions.</param>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void RequestActivatedPurchasesSample(bool allStores)
            /// {
            ///     var configuration = new GamebaseRequest.Purchase.PurchasableConfiguration
            ///     {
            ///         allStores = allStores
            ///     };
            /// 
            ///     Gamebase.Purchase.RequestActivatedPurchases(configuration, (purchasableReceiptList, error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("RequestItemListPurchasable succeeded");
            /// 
            ///             foreach (GamebaseResponse.Purchase.PurchasableReceipt purchasableReceipt in purchasableReceiptList)
            ///             {
            ///                 var message = new StringBuilder();
            ///                 message.AppendLine(string.Format("itemSeq:{0}", purchasableReceipt.itemSeq));
            ///                 message.AppendLine(string.Format("price:{0}", purchasableReceipt.price));
            ///                 message.AppendLine(string.Format("currency:{0}", purchasableReceipt.currency));
            ///                 
            ///                 // You will need paymentSeq and purchaseToken when calling the Consume API.
            ///                 // Refer to the following document for the Consume API.
            ///                 // http://docs.toast.com/en/Game/Gamebase/en/api-guide/#purchaseiap
            ///                 message.AppendLine(string.Format("paymentSeq:{0}", purchasableReceipt.paymentSeq));
            ///                 message.AppendLine(string.Format("purchaseToken:{0}", purchasableReceipt.purchaseToken));
            ///                 message.AppendLine(string.Format("marketItemId:{0}", purchasableReceipt.marketItemId));
            ///                 Debug.Log(message);
            ///             }
            ///         }
            ///         else
            ///         {
            ///             // Check the error code and handle the error appropriately.
            ///             Debug.Log(string.Format("RequestItemListPurchasable failed. error is {0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RequestActivatedPurchases(GamebaseRequest.Purchase.PurchasableConfiguration configuration,
                GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableReceipt>> callback)
            {
                GamebasePurchaseImplementation.Instance.RequestActivatedPurchases(configuration, callback);
            }

            /// <summary>
            /// Request a list of purchased subscription status.
            /// This API will work with Google store. (Android Only)
            /// @since Added 2.46.0.            
            /// </summary> 
            /// <param name="configuration">Sets additional conditions for retrieving results.</param>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void RequestSubscriptionsStatusSample(bool includeExpiredSubscriptions)
            /// {
            ///    var configuration = new GamebaseRequest.Purchase.PurchasableConfiguration
            ///    {
            ///        includeExpiredSubscriptions = includeExpiredSubscriptions
            ///     };
            /// 
            ///     Gamebase.Purchase.RequestSubscriptionsStatus(configuration, (subscriptionStatusList, error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("RequestSubscriptionsStatus succeeded");
            /// 
            ///             foreach (GamebaseResponse.Purchase.PurchasableSubscriptionStatus subscriptionStatus in subscriptionStatusList)
            ///             {
            ///                 var message = new StringBuilder();
            ///                 message.AppendLine(string.Format("storeCode:{0}", subscriptionStatus.storeCode));
            ///                 message.AppendLine(string.Format("paymentId:{0}", subscriptionStatus.paymentId));
            ///                 message.AppendLine(string.Format("originalPaymentId:{0}", subscriptionStatus.originalPaymentId));
            ///                 message.AppendLine(string.Format("paymentSeq:{0}", subscriptionStatus.paymentSeq));
            ///                 message.AppendLine(string.Format("marketItemId:{0}", subscriptionStatus.marketItemId));
            ///                 message.AppendLine(string.Format("itemSeq:{0}", subscriptionStatus.itemSeq));
            ///                 message.AppendLine(string.Format("price:{0}", subscriptionStatus.price));
            ///                 message.AppendLine(string.Format("currency:{0}", subscriptionStatus.currency));
            ///                 message.AppendLine(string.Format("purchaseTime:{0}", subscriptionStatus.purchaseTime));
            ///                 message.AppendLine(string.Format("expiryTime:{0}", subscriptionStatus.expiryTime));
            /// 
            ///                 // Subscription status
            ///                 // Refer to the following document for the entire status code.
            ///                 // https://docs.nhncloud.com/en/TOAST/en/toast-sdk/iap-unity/#iapsubscriptionstatusstatus
            ///                 message.AppendLine(string.Format("statusCode:{0}", subscriptionStatus.statusCode));
            ///                 message.AppendLine(string.Format("statusDescription:{0}", subscriptionStatus.statusDescription));
            ///                 message.AppendLine(string.Format("gamebaseProductId:{0}", subscriptionStatus.gamebaseProductId));
            ///                 Debug.Log(message);
            ///             }
            ///         }
            ///         else
            ///         {
            ///             // Check the error code and handle the error appropriately.
            ///             Debug.Log(string.Format("RequestSubscriptionsStatus failed. error is {0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RequestSubscriptionsStatus(GamebaseRequest.Purchase.PurchasableConfiguration configuration,
                GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableSubscriptionStatus>> callback)
            {
                GamebasePurchaseImplementation.Instance.RequestSubscriptionsStatus(configuration, callback);
            }

            /// <summary>
            /// Request a list of payment products that are currently subscribed.
            /// If there is a subscription purchased from another platform (such as iOS)
            /// @since Added 2.6.0.
            /// @deprecated As of release 2.45.0.
            /// </summary> 
            /// <returns>List of payment products that are currently subscribed.</returns>
            [Obsolete]
            public static void RequestActivatedPurchases(GamebaseCallback.GamebaseDelegate<List<GamebaseResponse.Purchase.PurchasableReceipt>> callback)
            {
                GamebasePurchaseImplementation.Instance.RequestActivatedPurchases(new GamebaseRequest.Purchase.PurchasableConfiguration(), callback);
            }

            /// <summary>
            /// In case a purchase is not normally completed after a successful purchase at a store due to failure of authentication of TOAST IAP server, try to reprocess by using API.
            /// Based on the latest success of purchase, reprocessing is required by calling an API for item delivery(supply).
            /// @since Added 1.4.0.
            /// @deprecated As of release 2.6.0.
            /// </summary>
            [Obsolete("Do not use this API.")]
            public static void RequestRetryTransaction(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Purchase.PurchasableRetryTransactionResult> callback)
            {
                GamebasePurchaseImplementation.Instance.RequestRetryTransaction(callback);
            }

            /// <summary>
            /// Set the store code of the current app. 
            /// @since Added 1.4.0.
            /// @deprecated As of release 2.6.0.
            /// </summary>
            [Obsolete("Do not use this API.")]
            public static void SetStoreCode(string storeCode)
            {
                GamebasePurchaseImplementation.Instance.SetStoreCode(storeCode);
            }

            /// <summary>
            /// Gets the store code of the current app. 
            /// This should only be called after the SDK has been initialized by calling Gamebase.initialize().
            /// @since Added 1.4.0.
            /// @deprecated As of release 2.6.0.
            /// </summary> 
            [Obsolete("Do not use this API.")]
            public static string GetStoreCode()
            {
                return GamebasePurchaseImplementation.Instance.GetStoreCode();
            }
        }

        /// <summary>
        /// The Push class provides registering push token API to ToastCloud Push Server and querying push token API.
        /// @since Added 1.4.0.
        /// </summary>
        public static class Push
        {
            /// <summary>
            /// With user's agreement to enablePush, enableAdPush, and enableAdNightPush, call following API to complete registration.
            /// @since Added 1.4.0.
            /// </summary>
            /// <param name="pushConfiguration">Settings of the notification.</param>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void RegisterPushSample(bool pushEnabled, bool adAgreement, bool adAgreementNight)
            /// {
            ///     var configuration = new GamebaseRequest.Push.PushConfiguration
            ///     {
            ///         pushEnabled = pushEnabled,
            ///         adAgreement = adAgreement,
            ///         adAgreementNight = adAgreementNight,
            ///         displayLanguageCode = GamebaseDisplayLanguageCode.Korean
            ///     };
            /// 
            ///     Gamebase.Push.RegisterPush(configuration, (error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("RegisterPush succeeded.");
            ///         }
            ///         else
            ///         {
            ///             // Check the error code and handle the error appropriately.
            ///             Debug.Log(string.Format("RegisterPush failed. error is {0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>            
            public static void RegisterPush(GamebaseRequest.Push.PushConfiguration pushConfiguration, GamebaseCallback.ErrorDelegate callback)
            {
                GamebasePushImplementation.Instance.RegisterPush(pushConfiguration, callback);
            }

            /// <summary>
            /// Get push settings from the the push server.
            /// @since Added 1.4.0.
            /// </summary>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void QueryPushSample()
            /// {
            ///     Gamebase.Push.QueryPush((pushAdvertisements, error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("QueryPush succeeded.");
            /// 
            ///             var message = new StringBuilder();
            ///             message.AppendLine(string.Format("pushEnabled:{0}", pushAdvertisements.pushEnabled));
            ///             message.AppendLine(string.Format("adAgreement:{0}", pushAdvertisements.adAgreement));
            ///             message.AppendLine(string.Format("adAgreementNight:{0}", pushAdvertisements.adAgreementNight));
            ///             message.AppendLine(string.Format("displayLanguageCode:{0}", pushAdvertisements.displayLanguageCode));
            ///             Debug.Log(message);
            ///         }
            ///         else
            ///         {
            ///             // Check the error code and handle the error appropriately.
            ///             Debug.Log(string.Format("QueryPush failed. error is {0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>            
            public static void QueryPush(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Push.PushConfiguration> callback)
            {
                GamebasePushImplementation.Instance.QueryPush(callback);
            }

            /// <summary>
            /// Set SandboxMode.(iOS Only)
            /// @since Added 1.4.0.
            /// </summary>
            /// <param name="isSandbox">"true" if application is on the sandbox mode.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SetSandboxModeSample(bool isSandbox)
            /// {
            ///     Gamebase.Push.SetSandboxMode(isSandbox);
            /// }
            /// </code>
            /// </example>
            public static void SetSandboxMode(bool isSandbox)
            {
                GamebasePushImplementation.Instance.SetSandboxMode(isSandbox);
            }

            /// <summary>
            /// Register push information to the push server.
            /// @since Added 2.15.0.
            /// </summary>
            /// <param name="pushConfiguration">Settings of the push from server.</param>
            /// <param name="options">Settings of the local notification.</param>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void RegisterPushSample(bool pushEnabled, bool adAgreement, bool adAgreementNight)
            /// {
            ///     var configuration = new GamebaseRequest.Push.PushConfiguration
            ///     {
            ///         pushEnabled = pushEnabled,
            ///         adAgreement = adAgreement,
            ///         adAgreementNight = adAgreementNight,
            ///         displayLanguageCode = GamebaseDisplayLanguageCode.Korean
            ///     };
            /// 
            ///     var notificationOptions = new GamebaseRequest.Push.NotificationOptions
            ///     {
            ///         foregroundEnabled = true,
            ///         priority = GamebaseNotificationPriority.HIGH
            ///     };
            /// 
            ///     Gamebase.Push.RegisterPush(configuration, notificationOptions, (error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("RegisterPush succeeded.");
            ///         }
            ///         else
            ///         {
            ///             // Check the error code and handle the error appropriately.
            ///             Debug.Log(string.Format("RegisterPush failed. error is {0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RegisterPush(GamebaseRequest.Push.PushConfiguration pushConfiguration, GamebaseRequest.Push.NotificationOptions options, GamebaseCallback.ErrorDelegate callback)
            {
                GamebasePushImplementation.Instance.RegisterPush(pushConfiguration, options, callback);
            }

            /// <summary>
            /// Get push settings from the the push server.
            /// @since Added 2.15.0.
            /// </summary>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void QueryTokenInfoSample(bool isSandbox)
            /// {
            ///     Gamebase.Push.QueryTokenInfo((data, error)=> 
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true) 
            ///         {
            ///             // Succeeded.
            ///             bool enablePush = data.agreement.pushEnabled;
            ///             bool enableAdPush = data.agreement.adAgreement;
            ///             bool enableAdNightPush = data.agreement.adAgreementNight;
            ///             string token = data.token;
            ///         }
            ///         else 
            ///         {
            ///             // Failed.
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void QueryTokenInfo(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Push.TokenInfo> callback)
            {
                GamebasePushImplementation.Instance.QueryTokenInfo(callback);
            }

            /// <summary>
            /// Get notification options in device.
            /// @since Added 2.15.0.
            /// </summary>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void GetNotificationOptionsSample()
            /// {
            ///     GamebaseResponse.Push.NotificationOptions notificationOptions = Gamebase.Push.GetNotificationOptions();
            /// 
            ///     GamebaseRequest.Push.NotificationOptions options = new GamebaseRequest.Push.NotificationOptions(notificationOptions);
            ///     options.foregroundEnabled = true;
            ///     options.smallIconName = "notification_icon_name";
            /// 
            ///     GamebaseRequest.Push.PushConfiguration configuration = new GamebaseRequest.Push.PushConfiguration();
            ///     Gamebase.Push.RegisterPush(configuration, options, (error)=> { });
            /// }
            /// </code>
            /// </example>
            public static GamebaseResponse.Push.NotificationOptions GetNotificationOptions()
            {
                return GamebasePushImplementation.Instance.GetNotificationOptions();
            }
            
            /// <summary>
            /// This API tells that the user has allowed the device to display notification.
            /// @since Added 2.34.1.
            /// </summary>
            /// <param name="callback">Callback pass to API result.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void QueryNotificationAllowedSample()
            /// {
            ///     Gamebase.Push.QueryNotificationAllowed((isAllowed, error)=> 
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true) 
            ///         {
            ///             if (isAllowed == true)
            ///             {
            ///                 // The user allowed notification.
            ///             }
            ///             else
            ///             {
            ///                 // The user blocked notification.
            ///             }
            ///         }
            ///         else 
            ///         {
            ///             // Failed to check device setting.
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void QueryNotificationAllowed(GamebaseCallback.GamebaseDelegate<bool> callback)
            {
                GamebasePushImplementation.Instance.QueryNotificationAllowed(callback);
            }
        }

        /// <summary>
        /// The WebView class represents the entry point for launching WebView.
        /// @since Added 1.4.0.
        /// </summary>
        public static class Webview
        {
            /// <summary>
            /// This method to create the external web browser and display it on screen.
            /// @since Added 1.5.0.
            /// </summary>
            /// <param name="url">The url of the resource to load.</param>            
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void OpenWebBrowserSample(string url)
            /// {
            ///     Gamebase.Webview.OpenWebBrowser(url);
            /// }
            /// </code>
            /// </example>
            public static void OpenWebBrowser(string url)
            {
                GamebaseWebviewImplementation.Instance.OpenWebBrowser(url);
            }

            /// <summary>
            /// This method to create the webview and display it on screen.
            /// @since Added 1.5.0.
            /// </summary>
            /// <param name="url">The url of the resource to load.</param>
            /// <param name="configuration">The configuration of webview.</param>
            /// <param name="closeCallback">This callback would be called when webview is closed</param>
            /// <param name="schemeList">This schemeList would be filtered every web view request and call schemeEvent</param>
            /// <param name="schemeEvent">This schemeEvent would be called when web view request matches one of the schemeLlist</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void ShowWebViewSample(GamebaseCallback.ErrorDelegate closeCallback, List<string> schemeList, GamebaseCallback.GamebaseDelegate<string> schemeEvent)
            /// {
            ///     var configuration = new GamebaseRequest.Webview.GamebaseWebViewConfiguration
            ///     {
            ///         title = "Title",
            ///         orientation = GamebaseScreenOrientation.UNSPECIFIED,
            ///         colorR = 128,
            ///         colorG = 128,
            ///         colorB = 128,
            ///         colorA = 255,
            ///         barHeight = 40,
            ///         isBackButtonVisible = true
            ///     };
            /// 
            ///     Gamebase.Webview.ShowWebView("http://www.naver.com", configuration, closeCallback, schemeList, schemeEvent);
            /// }
            /// </code>
            /// </example>
            public static void ShowWebView(string url, GamebaseRequest.Webview.Configuration configuration = null, GamebaseCallback.ErrorDelegate closeCallback = null, List<string> schemeList = null, GamebaseCallback.GamebaseDelegate<string> schemeEvent = null)
            {
                GamebaseWebviewImplementation.Instance.ShowWebView(url, configuration, closeCallback, schemeList, schemeEvent);
            }

            /// <summary>
            /// This method to close the webview in display on a screen.
            /// @since Added 1.5.0.
            /// </summary>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void CloseWebViewSample()
            /// {
            ///     Gamebase.Webview.CloseWebView();
            /// }
            /// </code>
            /// </example>
            public static void CloseWebView()
            {
                GamebaseWebviewImplementation.Instance.CloseWebView();
            }
        }

        /// <summary>
        /// The Util class provides convenient and useful methods.
        /// @since Added 1.4.0.
        /// </summary>
        public static class Util
        {
            /// <summary>
            /// This method to create the dialog and display it on screen.
            /// @since Added 1.4.0.
            /// </summary>
            /// <param name="title">The title to be shown in the alert.</param>
            /// <param name="message">The message to be shown in the alert.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void ShowAlertSample(string title, string message)
            /// {
            ///     Gamebase.Util.ShowAlert(title, message);
            /// }
            /// </code>
            /// </example>
            public static void ShowAlert(string title, string message)
            {
                GamebaseUtilImplementation.Instance.ShowAlert(title, message);
            }

            /// <summary>
            /// This method to create the dialog and display it on screen.
            /// Show Alert View with async completion callback.
            /// @since Added 1.4.0.
            /// </summary>
            /// <param name="title">The title to be shown in the alert.</param>
            /// <param name="message">The message to be shown in the alert.</param>
            /// <param name="buttonCallback">The buttonCallback to be executed after click ok button.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void ShowAlertSample(string title, string message)
            /// {
            ///     Gamebase.Util.ShowAlert(title, message, () =>
            ///     {
            ///         Debug.Log("The button was clicked.");
            ///     });
            /// }
            /// </code>
            /// </example>            
            public static void ShowAlert(string title, string message, GamebaseCallback.VoidDelegate buttonCallback)
            {
                GamebaseUtilImplementation.Instance.ShowAlert(title, message, buttonCallback);
            }

            /// <summary>
            /// Show a standard toast that just contains a text view.
            /// @since Added 1.4.0.
            /// </summary>
            /// <param name="message">The message to be shown in the alert.</param>
            /// <param name="type">The time interval to be exposed. GamebaseUIToastType.TOAST_LENGTH_LONG (3.5 seconds), GamebaseUIToastType.TOAST_LENGTH_SHORT (2 seconds)</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void ShowToastSample(string message)
            /// {
            ///     // GamebaseUIToastType.TOAST_LENGTH_SHORT or 0 : 2 seconds
            ///     // GamebaseUIToastType.TOAST_LENGTH_LONG or 1 : 3.5 seconds
            ///     // All the rest values : 2 seconds
            /// 
            ///     var type = GamebaseUIToastType.TOAST_LENGTH_SHORT;
            ///     Gamebase.Util.ShowToast(message, type);
            /// }
            /// </code>
            /// </example>           
            public static void ShowToast(string message, GamebaseUIToastType type)
            {
                GamebaseUtilImplementation.Instance.ShowToast(message, type);
            }

            /// <summary>
            /// Request app-tracking permission status for user authorization.
            /// @since Added 2.64.0.
            /// </summary>
            /// <returns>One of GamebaseAppTrackingAuthorizationStatus.AUTHORIZED, GamebaseAppTrackingAuthorizationStatus.DENIED, GamebaseAppTrackingAuthorizationStatus.NOT_DETERMINED, GamebaseAppTrackingAuthorizationStatus.RESTRICTED, GamebaseAppTrackingAuthorizationStatus.UNKNOWN</returns>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void GetAppTrackingAuthorizationStatusSample()
            /// {
            /// #if UNITY_IOS
            ///     switch (Gamebase.Util.GetAppTrackingAuthorizationStatus() ) Ios only
            ///     {
            ///         case GamebaseAppTrackingAuthorizationStatus.AUTHORIZED:
            ///         {
            /// 
            ///         }
            ///         break;  
            ///         
            ///         case GamebaseAppTrackingAuthorizationStatus.DENIED:
            ///         {
            /// 
            ///         }
            ///         break;
            /// 
            ///         case GamebaseAppTrackingAuthorizationStatus.NOT_DETERMINED:
            ///         {
            /// 
            ///         }
            ///         break;
            /// 
            ///         case GamebaseAppTrackingAuthorizationStatus.RESTRICTED:
            ///         {
            /// 
            ///         }
            ///         break;
            /// 
            ///         case GamebaseAppTrackingAuthorizationStatus.UNKNOWN:
            ///         {
            /// 
            ///         }
            ///         break;
            ///     }
            /// #endif
            /// }
            /// </code>
            /// </example>           
            public static GamebaseAppTrackingAuthorizationStatus GetAppTrackingAuthorizationStatus()
            {
                return GamebaseUtilImplementation.Instance.GetAppTrackingAuthorizationStatus();
            }
        }

        /// <summary>
        /// The Network class indicates Network status.
        /// @since Added 1.4.0.
        /// </summary>
        public static class Network
        {
            /// <summary>
            /// Reports the type of network.
            /// @since Added 1.4.0.
            /// </summary>
            /// <returns>One of GamebaseNetworkType.TYPE_NOT, GamebaseNetworkType.TYPE_MOBILE, GamebaseNetworkType.TYPE_WIFI, GamebaseNetworkType.TYPE_ANY.</returns>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void GetNetworkTypeSample()
            /// {
            ///     // GamebaseNetworkType.TYPE_NOT : Not connected.
            ///     // GamebaseNetworkType.TYPE_MOBILE : A Mobile data connection. (3G, 4G, 5G, LTE ...)
            ///     // GamebaseNetworkType.TYPE_WIFI : A WIFI data connection.
            ///     // GamebaseNetworkType.TYPE_ANY : All the rest values.
            /// 
            ///     GamebaseNetworkType networkType = Gamebase.Network.GetNetworkType();
            ///     Debug.Log(string.Format("networkType:{0}", networkType));
            /// }
            /// </code>
            /// </example>
            public static GamebaseNetworkType GetNetworkType()
            {
                return GamebaseNetworkImplementation.Instance.GetNetworkType();
            }

            /// <summary>
            /// Return a human-readable name describe the type of the network, for example "WIFI" or "MOBILE".
            /// @since Added 1.4.0.
            /// </summary>
            /// <returns>The name of the network type.</returns>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void GetNetworkTypeNameSample()
            /// {
            ///     string networkName = Gamebase.Network.GetNetworkTypeName();
            ///     Debug.Log(string.Format("networkName:{0}", networkName));
            /// }
            /// </code>
            /// </example>
            public static string GetNetworkTypeName()
            {
                return GamebaseNetworkImplementation.Instance.GetNetworkTypeName();
            }

            /// <summary>
            /// Indicates whether network connectivity exists and it is possible to establish connections and pass data. (Platforms other than WebGL)
            /// @since Added 1.4.0.
            /// </summary>
            /// <returns> "true" if network connectivity exists, "false" otherwise.</returns>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void IsConnectedSample()
            /// {
            ///     bool isConnected = Gamebase.Network.IsConnected();
            ///     Debug.Log(string.Format("isConnected:{0}", isConnected));
            /// }
            /// </code>
            /// </example>
            public static bool IsConnected()
            {
                return GamebaseNetworkImplementation.Instance.IsConnected();
            }

            /// <summary>
            /// Indicates whether network connectivity exists and it is possible to establish connections and pass data. (Only WebGL platform)
            /// @since Added 1.4.0.
            /// </summary>
            /// <param name="callback">"true" if network connectivity exists, "false" otherwise.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void IsConnectedSample()
            /// {
            ///     Gamebase.Network.IsConnected((reachable) => 
            ///     {
            ///         Debug.Log(string.Format("Internet reachability is {0}", reachable));
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void IsConnected(GamebaseCallback.DataDelegate<bool> callback)
            {
                GamebaseNetworkImplementation.Instance.IsConnected(callback);
            }
        }

        /// <summary>
        /// The Analytics class can send the game indicator to the Gamebase Server.
        /// @since Added 2.0.0.
        /// </summary>
        public static class Analytics
        {
            /// <summary>
            /// User level information can be transferred to the indicator after game login.
            /// @since Added 2.0.0.
            /// </summary>
            /// <param name="gameUserData">Level information of the user received from the game server after login.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SetGameUserDataSample(int userLevel, string channelId, string characterId, string characterClassId)
            /// {
            ///     var gameUserData = new GamebaseRequest.Analytics.GameUserData(userLevel);
            ///     gameUserData.channelId = channelId;
            ///     gameUserData.characterId = characterId;
            ///     gameUserData.characterClassId = characterClassId;
            /// 
            ///     Gamebase.Analytics.SetGameUserData(gameUserData);
            /// }
            /// </code>
            /// </example>
            public static void SetGameUserData(GamebaseRequest.Analytics.GameUserData gameUserData)
            {
                GamebaseAnalyticsImplementation.Instance.SetGameUserData(gameUserData);
            }

            /// <summary>
            /// When level up, user level information can be transmitted as indicator.
            /// @since Added 2.0.0.
            /// </summary>
            /// <param name="levelUpData">User level up information</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void TraceLevelUpSample(int userLevel, long levelUpTime)
            /// {
            ///     var levelUpData = new GamebaseRequest.Analytics.LevelUpData(userLevel, levelUpTime);
            ///     Gamebase.Analytics.TraceLevelUp(levelUpData);
            /// }
            /// </code>
            /// </example>
            public static void TraceLevelUp(GamebaseRequest.Analytics.LevelUpData levelUpData)
            {
                GamebaseAnalyticsImplementation.Instance.TraceLevelUp(levelUpData);
            }
        }

        /// <summary>
        /// Send a log to Log &amp; Crash Server.
        /// The log levels are as follows: debug &lt; info &lt; warn &lt; error &lt; fatal.
        /// Log &amp; Crash Search allows fast and patterned search of any logs you want among a large volume.
        /// @since Added 2.6.0.
        /// </summary>
        public static class Logger
        {
            /// <summary>
            /// This function initializes the TOAST Logger SDK. 
            /// The behavior of TOAST Logger SDK functions are undetermined if this function is not called.
            /// The appKey of the Log &amp; Crash project is required to initialize Logger.
            /// This function should be called when the application is run.
            /// @since Added 2.6.0.
            /// </summary>
            /// <param name="configuration">The configuration of Logger.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public static void InitializeSample()
            /// {
            ///     var configuration = new GamebaseRequest.Logger.Configuration("USER_LOGGER_APP_KEY");
            ///     // configuration.enableCrashReporter = true;    // Whether to send crash logs.
            ///     // configuration.enableCrashErrorLog =  true;   // Error log is excluded by default. Use it if you want to collect error logs.
            ///     Gamebase.Logger.Initialize(configuration);
            /// }
            /// </code>
            /// </example>
            public static void Initialize(GamebaseRequest.Logger.Configuration configuration)
            {
                GamebaseLoggerImplementation.Instance.Initialize(configuration);
            }

            /// <summary>
            /// Send a log to Log &amp; Crash Server for analyzing the string message with debug level.
            /// @since Added 2.6.0.
            /// </summary>
            /// <param name="message">Message to send to the log</param>
            /// <param name="userFields">Additional information to send to the log</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void DebugSample()
            /// {
            ///     Dictionary<string, string> userFields = new Dictionary&lt;string, string>()
            ///     {
            ///         { "KEY_1", "VALUE_1" },
            ///         { "KEY_2", "VALUE_2" },
            ///     };
            ///         
            ///     Gamebase.Logger.Debug("MESSAGE", userFields);
            /// }
            /// </code>
            /// </example>
            public static void Debug(string message, Dictionary<string, string> userFields = null)
            {
                GamebaseLoggerImplementation.Instance.Debug(message, userFields);
            }

            /// <summary>
            /// Send a log to Log &amp; Crash Server for analyzing the string message with info level.
            /// @since Added 2.6.0.
            /// </summary>
            /// <param name="message">Message to send to the log</param>
            /// <param name="userFields">Additional information to send to the log</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void InfoSample()
            /// {
            ///     Dictionary<string, string> userFields = new Dictionary&lt;string, string>()
            ///     {
            ///         { "KEY_1", "VALUE_1" },
            ///         { "KEY_2", "VALUE_2" },
            ///     };
            ///         
            ///     Gamebase.Logger.Info("MESSAGE", userFields);
            /// }
            /// </code>
            /// </example>
            public static void Info(string message, Dictionary<string, string> userFields = null)
            {
                GamebaseLoggerImplementation.Instance.Info(message, userFields);
            }

            /// <summary>
            /// Send a log to Log &amp; Crash Server for analyzing the string message with warn level.
            /// @since Added 2.6.0.
            /// </summary>
            /// <param name="message">Message to send to the log</param>
            /// <param name="userFields">Additional information to send to the log</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void WarnSample()
            /// {
            ///     Dictionary<string, string> userFields = new Dictionary&lt;string, string>()
            ///     {
            ///         { "KEY_1", "VALUE_1" },
            ///         { "KEY_2", "VALUE_2" },
            ///     };
            ///         
            ///     Gamebase.Logger.Warn("MESSAGE", userFields);
            /// }
            /// </code>
            /// </example>
            public static void Warn(string message, Dictionary<string, string> userFields = null)
            {
                GamebaseLoggerImplementation.Instance.Warn(message, userFields);
            }

            /// <summary>
            /// Send a log to Log &amp; Crash Server for analyzing the string message with error level.
            /// @since Added 2.6.0.
            /// </summary>
            /// <param name="message">Message to send to the log</param>
            /// <param name="userFields">Additional information to send to the log</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void ErrorSample()
            /// {
            ///     Dictionary<string, string> userFields = new Dictionary&lt;string, string>()
            ///     {
            ///         { "KEY_1", "VALUE_1" },
            ///         { "KEY_2", "VALUE_2" },
            ///     };
            ///         
            ///     Gamebase.Logger.Error("MESSAGE", userFields);
            /// }
            /// </code>
            /// </example>
            public static void Error(string message, Dictionary<string, string> userFields = null)
            {
                GamebaseLoggerImplementation.Instance.Error(message, userFields);
            }

            /// <summary>
            /// Send a log to Log &amp; Crash Server for analyzing the string message with fatal level.
            /// @since Added 2.6.0.
            /// </summary>
            /// <param name="message">Message to send to the log</param>
            /// <param name="userFields">Additional information to send to the log</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void FatalSample()
            /// {
            ///     Dictionary<string, string> userFields = new Dictionary&lt;string, string>()
            ///     {
            ///         { "KEY_1", "VALUE_1" },
            ///         { "KEY_2", "VALUE_2" },
            ///     };
            ///         
            ///     Gamebase.Logger.Fatal("MESSAGE", userFields);
            /// }
            /// </code>
            /// </example>
            public static void Fatal(string message, Dictionary<string, string> userFields = null)
            {
                GamebaseLoggerImplementation.Instance.Fatal(message, userFields);
            }

            /// <summary>
            /// Exceptions from a try/catch sentence, as well as general/crash logs, can be sent by using Report API.
            /// @since Added 2.57.0.
            /// </summary>
            /// <param name="logLevel">logLevel</param>
            /// <param name="message">Message to send to the log</param>
            /// <param name="logString">logString</param>
            /// <param name="stackTrace">stackTrace</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void ReportSample(Exception e)
            /// {
            ///     Gamebase.Logger.Report(GamebaseLoggerConst.LogLevel.ERROR, "message", e.Message, e.StackTrace);
            /// }
            /// </code>
            /// </example>
            public static void Report(GamebaseLoggerConst.LogLevel logLevel, string message, string logString, string stackTrace)
            {
                GamebaseLoggerImplementation.Instance.Report(logLevel, message, logString, stackTrace);
            }

            /// <summary>
            /// Used to predefine additional information.
            /// The value set here is sent together without being set every time.
            /// @since Added 2.6.0.
            /// </summary>
            /// <param name="key">The key of the additional information.</param>
            /// <param name="value">The value of the additional information.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SetUserFieldSample()
            /// {
            ///     Gamebase.Logger.SetUserField("KEY", "VALUE");
            /// }
            /// </code>
            /// </example>
            public static void SetUserField(string key, string value)
            {
                GamebaseLoggerImplementation.Instance.SetUserField(key, value);
            }

            /// <summary>
            /// You can be notified of the result of the log transfer through this listener.
            /// @since Added 2.6.0.
            /// </summary>
            /// <param name="listener">The listener that is notified of the results of the log transfer.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public class LoggerListener : GamebaseCallback.Logger.ILoggerListener
            /// {
            ///     public void OnError(GamebaseResponse.Logger.LogEntry log, string errorMessage)
            ///     {
            ///         // Sending logs failed
            ///     }
            /// 
            ///     public void OnFilter(GamebaseResponse.Logger.LogEntry log, GamebaseResponse.Logger.LogFilter filter)
            ///     {
            ///         // The logs were filtered out and not sent.(Refer to AddClashFilter API Guide)
            ///     }
            /// 
            ///     public void OnSave(GamebaseResponse.Logger.LogEntry log)
            ///     {
            ///         // If log transmission fails due to network disconnection, the log is saved in a file for log retransmission.(The saved file cannot be checked.)
            ///     }
            /// 
            ///     public void OnSuccess(GamebaseResponse.Logger.LogEntry log)
            ///     {
            ///         // Sending logs succeeded
            ///     }
            /// }                        
            /// 
            /// public void SetLoggerListenerSample()
            /// {
            ///     LoggerListener listener = new LoggerListener();
            ///     Gamebase.Logger.SetLoggerListener(listener);
            /// }
            /// </code>
            /// </example>
            public static void SetLoggerListener(GamebaseCallback.Logger.ILoggerListener listener)
            {
                GamebaseLoggerImplementation.Instance.SetLoggerListener(listener);
            }

            /// <summary>
            /// This function adds a CrashFilter.
            /// When a crash occurs, the filter lets you decide whether to send logs to the Log &amp; Crash server.
            /// When return value is true, log is not sent to Log &amp; Crash server.
            /// @since Added 2.6.0.
            /// </summary>
            /// <param name="filter">This is a filter to check whether logs are sent to the Log &amp; Crash server when a crash occurs.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public GamebaseCallback.Logger.CrashFilter crashFilter = (crashLogData) =>
            /// {
            ///     // If the return value is true, no log is sent.
            ///     // The properties of CrashLogData are the same as the parameters of Application.LogCallback on Unity.
            ///     // https://docs.unity3d.com/ScriptReference/Application.LogCallback.html
            ///     return crashLogData.condition.Contains("UnityEngine.Debug.Log");
            /// };
            /// 
            /// public void AddCrashFilterSample()
            /// {
            ///     Gamebase.Logger.AddCrashFilter(crashFilter);
            /// }
            /// </code>
            /// </example>
            public static void AddCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
            {
                GamebaseLoggerImplementation.Instance.AddCrashFilter(filter);
            }

            /// <summary>
            /// This function removes the CrashFilter.
            /// When a crash occurs, the filter lets you decide whether to send logs to the Log &amp; Crash server.
            /// When return value is true, log is not sent to Log &amp; Crash server.
            /// @since Added 2.6.0.
            /// </summary>
            /// <param name="filter">This is a filter to check whether logs are sent to the Log &amp; Crash server when a crash occurs.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public GamebaseCallback.Logger.CrashFilter crashFilter = (crashLogData) =>
            /// {
            ///     // If the return value is true, no log is sent.
            ///     // The properties of CrashLogData are the same as the parameters of Application.LogCallback on Unity.
            ///     // https://docs.unity3d.com/ScriptReference/Application.LogCallback.html
            ///     return crashLogData.condition.Contains("UnityEngine.Debug.Log");
            /// };
            /// 
            /// public void RemoveCrashFilterSample()
            /// {
            ///     Gamebase.Logger.RemoveCrashFilter(crashFilter);
            /// }
            /// </code>
            /// </example>
            public static void RemoveCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
            {
                GamebaseLoggerImplementation.Instance.RemoveCrashFilter(filter);
            }
        }

        /// <summary>
        /// This class provides wrapping of function execution related to TOAST Contact.
        /// @since Added 2.5.0.
        /// </summary>
        public static class Contact
        {
            /// <summary>
            /// Open TOAST Contact WebView with CS URL which is wrote in TOAST Gamebase App Console.
            /// If the CS URL is empty or invalid, you will receive a GamebaseError.
            /// @since Added 2.5.0.
            /// </summary>
            /// <param name="callback">Callbacks the result of OpenContact.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleOpenContact()
            /// {
            ///     Gamebase.Contact.OpenContact((error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             // A user close the contact web view.
            ///         }
            ///         else if (error.code == GamebaseErrorCode.UI_CONTACT_FAIL_INVALID_URL)
            ///         {
            ///             // Gamebase Console Service Center URL is invalid.
            ///             // Please check the url field in the TOAST Gamebase Console.
            ///         } 
            ///         else if (error.code == GamebaseErrorCode.UI_CONTACT_FAIL_ANDROID_DUPLICATED_VIEW)
            ///         { 
            ///             // The customer center web view is already opened.
            ///         } 
            ///         else 
            ///         {
            ///             // An error occur when opening the contact web view.
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void OpenContact(GamebaseCallback.ErrorDelegate callback)
            {
                GamebaseContactImplementation.Instance.OpenContact(callback);
            }

            /// <summary>
            /// Open TOAST Contact WebView with CS URL which is wrote in TOAST Gamebase App Console.
            /// If the CS URL is empty or invalid, you will receive a GamebaseError.
            /// @since Added 2.16.0.
            /// </summary>
            /// <param name="configuration">The init settings of contact URL.</param>
            /// <param name="callback">Callbacks the result of OpenContact.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleOpenContact()
            /// {
            ///     var configuration = new GamebaseRequest.Contact.Configuration()
            ///     {
            ///         userName = "User Name"
            ///     };
            /// 
            ///     Gamebase.Contact.OpenContact(configuration, (error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             // A user close the contact web view.
            ///         }
            ///         else if (error.code == GamebaseErrorCode.UI_CONTACT_FAIL_INVALID_URL)
            ///         {
            ///             // Gamebase Console Service Center URL is invalid.
            ///             // Please check the url field in the TOAST Gamebase Console.
            ///         } 
            ///         else if (error.code == GamebaseErrorCode.UI_CONTACT_FAIL_ANDROID_DUPLICATED_VIEW)
            ///         { 
            ///             // The customer center web view is already opened.
            ///         } 
            ///         else 
            ///         {
            ///             // An error occur when opening the contact web view.
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void OpenContact(GamebaseRequest.Contact.Configuration configuration, GamebaseCallback.ErrorDelegate callback)
            {
                GamebaseContactImplementation.Instance.OpenContact(configuration, callback);
            }

            /// <summary>
            /// URL is passed through callback.
            /// You can open webview with this URL.
            /// @since Added 2.16.0.
            /// </summary>
            /// <param name="callback">Returns the customer service URL.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleRequestContactURL()
            /// {
            ///     Gamebase.Contact.RequestContactURL((url, error) => 
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true) 
            ///         {
            ///             // Open webview with 'contactUrl'
            ///         } 
            ///         else if (error.code == GamebaseErrorCode.UI_CONTACT_FAIL_INVALID_URL) // 6911
            ///         { 
            ///             // Gamebase Console Service Center URL is invalid.
            ///             // Please check the url field in the TOAST Gamebase Console.
            ///         } 
            ///         else 
            ///         {
            ///             // An error occur when requesting the contact web view url.
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RequestContactURL(GamebaseCallback.GamebaseDelegate<string> callback)
            {
                GamebaseContactImplementation.Instance.RequestContactURL(callback);
            }

            /// <summary>
            /// URL is passed through callback.
            /// You can open webview with this URL.
            /// @since Added 2.16.0.
            /// </summary>
            /// <param name="configuration">The init settings of contact URL.</param>
            /// <param name="callback">Returns the customer service URL.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleRequestContactURL()
            /// {
            ///     var configuration = new GamebaseRequest.Contact.Configuration()
            ///     {
            ///         userName = "User Name"
            ///     };
            /// 
            ///     Gamebase.Contact.RequestContactURL(configuration, (url, error) => 
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true) 
            ///         {
            ///             // Open webview with 'contactUrl'
            ///         } 
            ///         else if (error.code == GamebaseErrorCode.UI_CONTACT_FAIL_INVALID_URL) // 6911
            ///         { 
            ///             // Gamebase Console Service Center URL is invalid.
            ///             // Please check the url field in the TOAST Gamebase Console.
            ///         } 
            ///         else 
            ///         {
            ///             // An error occur when requesting the contact web view url.
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RequestContactURL(GamebaseRequest.Contact.Configuration configuration, GamebaseCallback.GamebaseDelegate<string> callback)
            {
                GamebaseContactImplementation.Instance.RequestContactURL(configuration, callback);
            }
        }

        /// <summary>
        /// This class provides wrapping of function execution related to temporary withdrawal.
        /// @since Added 2.9.0.
        /// </summary>
        public static class TemporaryWithdrawal
        {
            /// <summary>
            /// This function ignores the withdrawal grace period and proceeds withdrawal immediately.
            /// @since Added 2.9.0.
            /// </summary>
            /// <param name="callback">Callbacks the result of WithdrawImmediately.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleWithdrawImmediately()
            /// {
            ///     Gamebase.TemporaryWithdrawal.WithdrawImmediately((error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("WithdrawImmediately succeeded.");
            ///         }
            ///         else
            ///         {
            ///             Debug.Log(string.Format("SampleWithdrawImmediately failed. error:{0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void WithdrawImmediately(GamebaseCallback.ErrorDelegate callback)
            {
                GamebaseAuthImplementation.Instance.WithdrawImmediately(callback);
            }

            /// <summary>
            /// Request to Temporary Withdrawal.
            /// If the user who requested the temporary withdrawal logs in,
            /// you can check the grace expiration time through the AuthToken.member.temporaryWithdrawalInfo.gracePeriodDate.            
            /// @since Added 2.9.0.
            /// </summary>
            /// <param name="callback">Callbacks the result of OpenContact.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleRequestWithdrawal()
            /// {
            ///     Gamebase.TemporaryWithdrawal.RequestWithdrawal((data, error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             long gracePeriodDate = data.gracePeriodDate;
            ///             Debug.Log(string.Format("RequestWithdrawal succeeded. The date when you can withdraw your withdrawal is {0}", gracePeriodDate));
            ///         }
            ///         else
            ///         {
            ///             Debug.Log(string.Format("RequestWithdrawal failed. error:{0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void RequestWithdrawal(GamebaseCallback.GamebaseDelegate<GamebaseResponse.TemporaryWithdrawalInfo> callback)
            {
                GamebaseAuthImplementation.Instance.RequestTemporaryWithdrawal(callback);
            }

            /// <summary>
            /// If the user has requested to withdraw, cancel it.
            /// If the user has never requested to leave, the function call will fail.            
            /// @since Added 2.9.0.
            /// </summary>
            /// <param name="callback">Callbacks the result of CancelWithdrawal.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleCancelWithdrawal()
            /// {
            ///     Gamebase.TemporaryWithdrawal.CancelWithdrawal((error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("CancelWithdrawal succeeded.");
            ///         }
            ///         else
            ///         {
            ///             Debug.Log(string.Format("CancelWithdrawal failed. error:{0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void CancelWithdrawal(GamebaseCallback.ErrorDelegate callback)
            {
                GamebaseAuthImplementation.Instance.CancelTemporaryWithdrawal(callback);
            }
        }

        /// <summary>
        /// This class provides functionality related to image notices.
        /// @since Added 2.12.0.
        /// </summary>
        public static class ImageNotice
        {
            /// <summary>
            /// Show image notice web pages.            
            /// @since Added 2.12.0.
            /// </summary>
            /// <param name="configuration">The initial settings of image notices.</param>
            /// <param name="closeCallback">Called when the browser closed.</param>
            /// <param name="eventCallback">Click event with notice type 'Custom'.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleShowImageNotices()
            /// {
            ///     GamebaseRequest.ImageNotice.Configuration configuration = new GamebaseRequest.ImageNotice.Configuration();
            ///     configuration.colorR = 255;
            ///     configuration.colorG = 255;
            ///     configuration.colorB = 255;
            ///     configuration.colorA = 255;
            ///     configuration.timeout = 5000;
            ///     
            ///     Gamebase.ImageNotice.ShowImageNotices(
            ///         configuration,
            ///         (error) =>
            ///         {
            ///             // Called when the entire imageNotice is closed.
            ///         },
            ///         (scheme, error) =>
            ///         {
            ///             // Called when custom event occurred.
            ///         });
            /// }
            /// </code>
            /// </example>
            public static void ShowImageNotices(GamebaseRequest.ImageNotice.Configuration configuration, GamebaseCallback.ErrorDelegate closeCallback, GamebaseCallback.GamebaseDelegate<string> eventCallback = null)
            {
                GamebaseImageNoticeImplementation.Instance.ShowImageNotices(configuration, closeCallback, eventCallback);
            }

            /// <summary>
            /// This method to close the image notice in display on a screen.          
            /// @since Added 2.12.0.
            /// </summary>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleCloseImageNotices()
            /// {
            ///     Gamebase.ImageNotice.CloseImageNotices();
            /// }
            /// </code>
            /// </example>
            public static void CloseImageNotices()
            {
                GamebaseImageNoticeImplementation.Instance.CloseImageNotices();
            }
        }

        /// <summary>
        /// This class provides wrapping of function execution related to TOAST Community.
        /// @since Added 2.64.0.
        /// </summary>
        public static class Community
        {
            /// <summary>
            /// Open Community WebView with URL.
            /// If the CS URL is empty or invalid, you will receive a GamebaseError.
            /// @since Added 2.64.0.
            /// </summary>
            /// <param name="configuration">The init settings of contact URL.</param>
            /// <param name="callback">Callbacks the result of Community.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleOpenCommunity()
            /// {
            ///     var configuration = new GamebaseRequest.Community.Configuration()
            ///     {
            ///         forcedURL = "https://www.nhncloud.com"
            ///     };
            /// 
            ///     Gamebase.Community.OpenCommunity(configuration, (error) =>
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             // A user close the contact web view.
            ///         }
            ///         else 
            ///         {
            ///             // An error occur when opening the contact web view.
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void OpenCommunity(GamebaseRequest.Community.Configuration configuration, GamebaseCallback.ErrorDelegate callback)
            {
                GamebaseCommunityImplementation.Instance.OpenCommunity(configuration, callback);
            }
        }

            /// <summary>
            /// Displays the terms and conditions set in the Gamebase console.
            /// @since Added 2.20.0.
            /// </summary>
            public static class Terms
        {
            /// <summary>
            /// Displays the terms and conditions window on the screen.
            /// If the user agrees to the terms and conditions, it registers the consent or not on the server.
            /// If you agree to the terms and conditions calling ShowTermsView again does not display the terms and conditions and immediately returns a success callback.
            /// @since Added 2.20.0.
            /// </summary>
            /// <param name="callback">After agreeing to the terms and conditions, when the terms and conditions window is closed, the user is notified by a callback.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleShowTermsView()
            /// {
            ///     Gamebase.Terms.ShowTermsView((data, error) => 
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("ShowTermsView succeeded.");
            ///             GamebaseResponse.Terms.ShowTermsViewResult result = GamebaseResponse.Terms.ShowTermsViewResult.From(data);
            ///         }
            ///         else
            ///         {
            ///             Debug.Log(string.Format("ShowTermsView failed. error:{0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void ShowTermsView(GamebaseCallback.GamebaseDelegate<GamebaseResponse.DataContainer> callback)
            {
                ShowTermsView(null, callback);
            }
            
            /// <summary>
            /// Displays the terms and conditions window on the screen.
            /// If the user agrees to the terms and conditions, it registers the consent or not on the server.
            /// @since Added 2.33.0.
            /// </summary>
            /// <param name="configuration">The initial settings of terms view.</param>
            /// <param name="callback">After agreeing to the terms and conditions, when the terms and conditions window is closed, the user is notified by a callback.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleShowTermsView(bool forceShow)
            /// {
            ///     var configuration = new GamebaseRequest.Terms.GamebaseTermsConfiguration
            ///     {
            ///         forceShow = forceShow
            ///     };
            /// 
            ///     Gamebase.Terms.ShowTermsView(configuration, (data, error) => 
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("ShowTermsView succeeded.");
            ///             GamebaseResponse.Terms.ShowTermsViewResult result = GamebaseResponse.Terms.ShowTermsViewResult.From(data);
            ///         }
            ///         else
            ///         {
            ///             Debug.Log(string.Format("ShowTermsView failed. error:{0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void ShowTermsView(GamebaseRequest.Terms.GamebaseTermsConfiguration configuration, GamebaseCallback.GamebaseDelegate<GamebaseResponse.DataContainer> callback)
            {
                GamebaseTermsImplementation.Instance.ShowTermsView(configuration, callback);
            }

            /// <summary>
            /// If you have created your own UI with the terms and conditions information downloaded through the QueryTerms API,
            /// Please send the game user's agreement to the terms and conditions to the Gamebase server through the UpdateTerms API.
            /// You can also use it for the purpose of changing the details of your agreement to the terms, such as canceling the agreement to the optional terms and conditions.
            /// @since Added 2.20.0.
            /// </summary>            
            /// <param name="configuration">This is the information on the user's option to register on the server.</param>
            /// <param name="callback">The optional terms and conditions are registered on the server and notified to the user via a callback.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleUpdateTerms()
            /// {
            ///     List<GamebaseRequest.Terms.Content> list = new List<GamebaseRequest.Terms.Content>();
            ///     list.Add(new GamebaseRequest.Terms.Content()
            ///     {
            ///         termsContentSeq = 0,
            ///         agreed = true
            ///     });
            ///     
            ///     Gamebase.Terms.UpdateTerms(
            ///         new GamebaseRequest.Terms.UpdateTermsConfiguration()
            ///         {
            ///             termsSeq = 0,
            ///             termsVersion = "1.0.0",
            ///             contents = list
            ///         },
            ///         (error) =>
            ///         {
            ///             if (Gamebase.IsSuccess(error) == true)
            ///             {
            ///                 Debug.Log("UpdateTerms succeeded.");
            ///             }
            ///             else
            ///             {
            ///                 Debug.Log(string.Format("UpdateTerms failed. error:{0}", error));
            ///             }
            ///         });
            /// }
            /// </code>
            /// </example>
            public static void UpdateTerms(GamebaseRequest.Terms.UpdateTermsConfiguration configuration, GamebaseCallback.ErrorDelegate callback)
            {
                GamebaseTermsImplementation.Instance.UpdateTerms(configuration, callback);
            }

            /// <summary>            
            /// If you develop the terms and conditions for the game UI yourself, you can call queryTerms API to receive and use the terms and conditions set in the Gamebase console.
            /// If you call the API after logging in, you can also check whether the game user has agreed to the terms and conditions.
            /// @since Added 2.20.0.
            /// </summary>
            /// <param name="callback">The result of the API call is notified to the user as a callback. You can get the terms and conditions set in the console with QueryTermsResult that comes as a callback.</param>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void SampleQueryTerms()
            /// {
            ///     Gamebase.Terms.QueryTerms((data, error) => 
            ///     {
            ///         if (Gamebase.IsSuccess(error) == true)
            ///         {
            ///             Debug.Log("QueryTerms succeeded.");
            ///         }
            ///         else
            ///         {
            ///             Debug.Log(string.Format("QueryTerms failed. error:{0}", error));
            ///         }
            ///     });
            /// }
            /// </code>
            /// </example>
            public static void QueryTerms(GamebaseCallback.GamebaseDelegate<GamebaseResponse.Terms.QueryTermsResult> callback)
            {
                GamebaseTermsImplementation.Instance.QueryTerms(callback);
            }
            
            /// <summary>
            /// This is the method to check if terms view is being shown.
            /// @since Added 2.35.0.
            /// </summary>
            /// <returns>Whether the terms view is currently show.</returns>
            /// <example> 
            /// Example Usage : 
            /// <code>
            /// public void IsShowingTermsView()
            /// {
            ///     bool isShowingTermsView = Gamebase.Terms.IsShowingTermsView();
            ///     Debug.Log(string.Format("isShowingTermsView:{0}", isShowingTermsView));
            /// }
            /// </code>
            /// </example>
            public static bool IsShowingTermsView()
            {
                return GamebaseTermsImplementation.Instance.IsShowingTermsView();
            }
        }

        ////////////////////////////////////////////////////////////
        //
        //  Lagacy API (As of version 2.22.0)
        //
        ////////////////////////////////////////////////////////////

        /// <summary>
        /// Integrated with AddEventHandler. Use AddEventHandler instead of AddObserver.
        /// <see cref="AddEventHandler"/>
        /// @since Added 1.8.0.
        /// </summary>
        public static void AddObserver(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage> observer)
        {
            GamebaseImplementation.Instance.AddObserver(observer);
        }

        public static void RemoveObserver(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ObserverMessage> observer)
        {
            GamebaseImplementation.Instance.RemoveObserver(observer);
        }

        public static void RemoveAllObserver()
        {
            GamebaseImplementation.Instance.RemoveAllObserver();
        }

        /// <summary>
        /// Integrated with AddEventHandler. Use AddEventHandler instead of AddServerPushEvent.
        /// <see cref="AddEventHandler"/>
        /// @since Added 1.8.0.
        /// </summary>
        public static void AddServerPushEvent(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage> serverPushEvent)
        {
            GamebaseImplementation.Instance.AddServerPushEvent(serverPushEvent);
        }

        public static void RemoveServerPushEvent(GamebaseCallback.DataDelegate<GamebaseResponse.SDK.ServerPushMessage> serverPushEvent)
        {
            GamebaseImplementation.Instance.RemoveServerPushEvent(serverPushEvent);
        }

        public static void RemoveAllServerPushEvent()
        {
            GamebaseImplementation.Instance.RemoveAllServerPushEvent();
        }

        /// <summary>
        /// Use GamebaseResponse.Auth.BanInfo.From(GamebaseError error) instead of GetBanInfo.
        /// @since Added 1.4.0.
        /// </summary>
        public static GamebaseResponse.Auth.BanInfo GetBanInfo()
        {
            return GamebaseAuthImplementation.Instance.GetBanInfo();
        }
    }
}