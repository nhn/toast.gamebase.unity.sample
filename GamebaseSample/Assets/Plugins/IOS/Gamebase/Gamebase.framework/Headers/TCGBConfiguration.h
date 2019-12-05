//
//  TCGBConfiguration.h
//  Gamebase
//
//  Created by NHN on 2016. 12. 28..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

extern NSString* const kConfigurationShowBlockingPopup;
extern NSString* const kConfigurationLaunchingStatusShowBlockingPopup;
extern NSString* const kConfigurationBanShowBlockingPopup;
extern NSString* const kConfigurationKickoutShowBlockingPopup;

extern NSString* const kConfigurationDisplayLanguageCode;

/** TCGBConfiguration configures essential Gamebase settings.
 */
@interface TCGBConfiguration : NSObject

/**---------------------------------------------------------------------------------------
 * @name Properties
 *  ---------------------------------------------------------------------------------------
 */

/**
 ApplicationID which is Toast Cloud Project ID.
 */
@property (nonatomic, strong) NSString* appID;

/**
 Application Version that is registered at Toast Cloud Console.
 */
@property (nonatomic, strong) NSString* appVersion;

/**
 Zone Type that is nomally "REAL".
 */
@property (nonatomic, strong) NSString* zoneType;

/**
 Server Address which your application communicates with.
 */
@property (nonatomic, strong, readonly) NSURL*    serverAddress;

/**
 Extra Options such as whether Showing Blocking PopUp enabled or not.
 */
@property (nonatomic, strong)           NSMutableDictionary*     options;

/**---------------------------------------------------------------------------------------
 * @name Initialization
 *  ---------------------------------------------------------------------------------------
 */

/**
 Creates TCGBConfiguration instance
 
 @param appID ApplicationID which is Toast Cloud Project ID.
 @param appVersion Application Version that is registered at Toast Cloud Console.
 */
+ (TCGBConfiguration *)configurationWithAppID:(NSString *)appID appVersion:(NSString *)appVersion;

/**
 Creates TCGBConfiguration instance
 
 @param appID ApplicationID which is Toast Cloud Project ID.
 @param appVersion Application Version that is registered at Toast Cloud Console.
 @param zoneType Zone Type that is nomally "REAL". It is only used to ToastCloud Beta/Alaha Test.
 */
+ (TCGBConfiguration *)configurationWithAppID:(NSString *)appID appVersion:(NSString *)appVersion zoneType:(NSString *)zoneType;

/**---------------------------------------------------------------------------------------
 * @name Setting Options
 *  ---------------------------------------------------------------------------------------
 */

/**
 Enable Show Blocking PopUp.
 Default set value is enable status popup.
 
 @param enable `YES` if Shown Popup blocking another processes.
 */
- (void)enablePopup:(BOOL)enable;

/**
 Method that returns whether the popup will be show or not.
 
 @return Boolean value that whether the popup will be show.
 */
- (BOOL)isEnablePopup;

/**
 Enable Show Blocking PopUp.
 Default set value is enable status popup.
 
 @param enable `YES` if Shown Popup blocking another processes.
 */
- (void)enableLaunchingStatusPopup:(BOOL)enable;

/**
 Method that returns whether the popup will be show or not.
 
 @return Boolean value that whether the popup will be show.
 */
- (BOOL)isEnableLaunchingStatusPopup;

/**
 Enable Show Blocking PopUp.
 Default set value is enable status popup.
 
@param enable `YES` if Shown Popup blocking another processes.
*/
- (void)enableBanPopup:(BOOL)enable;

/**
 Method that returns whether the popup will be show or not.
 
 @return Boolean value that whether the popup will be show.
 */
- (BOOL)isEnableBanPopup;

- (void)enableKickoutPopup:(BOOL)enable;
- (BOOL)isEnableKickoutPopup;

/**
 StoreCode that is needed when using purchasing APIs.
 
 @param storeCode   It represent storeCode, actually "AS"(AppStore).
 @warning           If you did not set this value, default will set to AS(AppStore).
 */
- (void)setStoreCode:(NSString *)storeCode;

/**
 @return    StoreCode that is needed when using purchasing APIs.
 */
- (NSString *)storeCode;

/**
 Set Display Language Code when initializing Gamebase SDK.
 
 @param languageCode    It represent language code (ISO-639)
 */
- (void)setDisplayLanguageCode:(NSString *)languageCode;

/**
 Method that returns displayLanguage that you have set.
 
 @return String value of language code (ISO-639)
 */
- (NSString *)displayLanguageCode;

@end
