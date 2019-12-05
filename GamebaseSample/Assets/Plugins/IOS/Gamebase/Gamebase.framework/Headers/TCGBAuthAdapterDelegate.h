//
//  TCGBAuthAdapterDelegate.h
//  Gamebase
//
//  Created by NHN on 2016. 11. 16..
//  © NHN Corp. All rights reserved.
//
#import <UIKit/UIKit.h>

#ifndef TCGBAuthAdapterDelegate_h
#define TCGBAuthAdapterDelegate_h

typedef NSString * UIApplicationOpenURLOptionsKey;

@class TCGBProviderAuthCredential;
@class TCGBAuthProviderProfile;
@class TCGBError;

/** The protocol TCGBAuthAdapterDelegate is for developing Auth Adapter.<br/>
 This protocol contains several methods such as to login, logout and so on.
 */
@protocol TCGBAuthAdapterDelegate <NSObject>

@optional
/**---------------------------------------------------------------------------------------
 * @name Initilaize Auth Adapter
 *  ---------------------------------------------------------------------------------------
 */

/** This method ininialize the Auth Adapter class.
 
 @param options isDebugMode is for setting debug mode of ToastCloud IAP module.
 */
- (void)initializeWithOptions:(NSDictionary *)options;



@required
/**---------------------------------------------------------------------------------------
 * @name Authentication API
 *  ---------------------------------------------------------------------------------------
 */

/** Login to identity service provider.
 
 @param sender sender instance
 @param target target instance
 @param additionalInfo If identity provider needs to set something when it is initialized, this additionanlInfo is set.
 @param viewController presenting view controller
 @param completion completion handler when auth process(login, logout, withdraw) is completed.
 */
- (void)loginWithSender:(id)sender target:(id<TCGBAuthAdapterDelegate>)target additionalInfo:(NSDictionary *)additionalInfo viewController:(UIViewController *)viewController completion:(void(^)(id credential, TCGBError *error))completion;

/** Logout to identity service provider.
 
 @param sender sender instance
 @param target target instance
 @param completion completion handler when auth process(login, logout, withdraw) is completed.
 */
- (void)logoutWithSender:(id)sender target:(id<TCGBAuthAdapterDelegate>)target completion:(void(^)(TCGBError *error))completion;

/** Withdrwal to identity service provider.
 
 @param sender sender instance
 @param target target instance
 @param completion completion handler when auth process(login, logout, withdraw) is completed.
 */
- (void)withdrawWithSender:(id)sender target:(id<TCGBAuthAdapterDelegate>)target completion:(void(^)(TCGBError *error))completion;


@optional // when IDP do not support mapping
/** Mapping to identity service provider.
 
 @param sender sender instance
 @param target target instance
 @param additionalInfo If identity provider needs to set something when it is initialized, this additionanlInfo is set.
 @param completion completion handler when auth process(login, logout, withdraw) is completed.
 */
- (void)addMappingWithSender:(id)sender target:(id<TCGBAuthAdapterDelegate>)target additionalInfo:(NSDictionary *)additionalInfo viewController:(UIViewController *)viewController completion:(void(^)(id credential, TCGBError *error))completion;

/**---------------------------------------------------------------------------------------
 * @name Provider information
 *  ---------------------------------------------------------------------------------------
 */
@optional // gettingIDP auth infos after logging in
/** 
 @return Provider's authenticated credential
 */
- (TCGBProviderAuthCredential *)authInfosFromProvider;

/**
 @return Provider's name
 */
- (NSString *)providerName;

@optional
- (NSString *)accessToken;
- (NSString *)userID;
- (TCGBAuthProviderProfile *)profile;

/**---------------------------------------------------------------------------------------
 * @name Adapter information
 *  ---------------------------------------------------------------------------------------
 */
@required
/**
 @return semantic version string of Auth Adapter
 */
+ (NSString *)versionString;

#pragma mark - System Events
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
/**---------------------------------------------------------------------------------------
 * @name Application life cycle for identify provider's sdk setting
 *  ---------------------------------------------------------------------------------------
 */

@optional
/// Managing State Transitions
// launch time
- (BOOL)application:(UIApplication *)application willFinishLaunchingWithOptions:(NSDictionary *)launchOptions;
- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions;
// transitioning to foreground
- (void)applicationDidBecomeActive:(UIApplication *)application;
// transitioning to background
- (void)applicationDidEnterBackground:(UIApplication *)application;
// transitioning  to the inactive state
- (void)applicationWillResignActive:(UIApplication *)application;
- (void)applicationWillEnterForeground:(UIApplication *)application;
// termination
- (void)applicationWillTerminate:(UIApplication *)application;

/// Responding to Notifications and Events
// When a remote notification arrives, the system calls the
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))completionHandler;
// When a local notification fires, the system calls the
- (void)application:(UIApplication *)application didReceiveLocalNotification:(UILocalNotification *)notification;
// When the user TCGBs a custom action in the alert for a remote or local notification’s, the system calls the
- (void)application:(UIApplication *)application handleActionWithIdentifier:(NSString *)identifier forRemoteNotification:(NSDictionary *)userInfo completionHandler:(void (^)(void))completionHandler;
- (void)application:(UIApplication *)application handleActionWithIdentifier:(NSString *)identifier forLocalNotification:(UILocalNotification *)notification completionHandler:(void (^)(void))completionHandler;
// For apps that want to initiate background downloads, the system calls the
- (void)application:(UIApplication *)application performFetchWithCompletionHandler:(void (^)(UIBackgroundFetchResult result))completionHandler;
// For apps that use the NSURLSession class to perform background downloads, the system calls the
- (void)application:(UIApplication *)application handleEventsForBackgroundURLSession:(NSString *)identifier completionHandler:(void (^)(void))completionHandler;
// When a low-memory condition occurs, the system notifies the app delegate by calling its
- (void)applicationDidReceiveMemoryWarning:(UIApplication *)application;
//When a significant change in time occurs, the system notifies the app delegate by calling its
- (void)applicationSignificantTimeChange:(UIApplication *)application;
// When the user locks the device, the system calls the app delegate’s
- (void)applicationProtectedDataWillBecomeUnavailable:(UIApplication *)application;
// you can reestablish your references to the data in the app delegate’s
- (void)applicationProtectedDataDidBecomeAvailable:(UIApplication *)application;


/// handle OpenURL
- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options; // available iOS9 and later
- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation; // available iOS 4.2 ~ iOS8(deprecated at 9.0)
- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url; // available iOS2 ~ iOS8(deprecated at 9.0)
#pragma clang diagnostic pop

@end


#endif /* TCGBAuthAdapterDelegate_h */
