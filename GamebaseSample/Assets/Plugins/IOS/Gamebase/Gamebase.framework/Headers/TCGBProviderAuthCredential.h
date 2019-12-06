//
//  TCGBAuthCredential.h
//  Gamebase
//
//  Created by NHN on 2016. 11. 29..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

/** The TCGBProviderAuthToken class has several access token information.
 There are OAuth 2.0 Type properties.
 */
@interface TCGBProviderAuthToken : NSObject <NSCopying>

/**---------------------------------------------------------------------------------------
 * @name Properties
 *  ---------------------------------------------------------------------------------------
 */

/** 
 Accss Token
 */
@property (nonatomic, strong) NSString*             accessToken;

/**
 Refresh Token
 */
@property (nonatomic, strong) NSString*             refreshToken;

/**
 Access Token Secret
 */
@property (nonatomic, strong) NSString*             accessTokenSecret;

/**
 Expiration Time
 */
@property (nonatomic, assign) long                  expirationTime;

/**
 Custom Auth Type
 @warning If identity provider is not supported OAuth 2.0 such as iOS GameCenter, You should use this property for senting auth information toward TCGB Server.
 */
@property (nonatomic, assign, getter=isCustomAuthType) BOOL                  customAuthType;             /// iosgamecenter와 같이 oauth 형식에서 크게 벗어나는 인증 서비스는 TCGBProviderAuthExtra를 바라보도록 한다.

/**---------------------------------------------------------------------------------------
 * @name Initialization
 *  ---------------------------------------------------------------------------------------
 */

/**
 Creates TCGBProviderAuthToken instance.
 
 @param accessToken Access Token
 @param refreshToken Refresh Token
 @param accessTokenSecret Access Token Secret
 @param expirationTime Expiration Time
 */
+ (TCGBProviderAuthToken *)authTokenWithAccessToken:(NSString *)accessToken refreshToken:(NSString *)refreshToken acessTokenSecret:(NSString *)accessTokenSecret expirationTime:(long)expirationTime;
- (NSString *)description;
@end

/** The TCGBProviderAUthProfile has user information who logged in.
 */
@interface TCGBProviderAuthProfile : NSObject <NSCopying>

/**---------------------------------------------------------------------------------------
 * @name Properties
 *  ---------------------------------------------------------------------------------------
 */

/**
 User ID
 */
@property (nonatomic, strong) NSString*             userID;

/**
 User Name
 */
@property (nonatomic, strong) NSString*             name;

/**
 User Email
 */
@property (nonatomic, strong) NSString*             email;

/**
 User Profile Image URL String
 */
@property (nonatomic, strong) NSString*             profileImageURL;

/**---------------------------------------------------------------------------------------
 * @name Initialization
 *  ---------------------------------------------------------------------------------------
 */

/**
 Creates TCGBProviderAuthProfile instance.
 
 @param userID User ID
 @param name User Name
 @param email User Image Adress
 @param profileImageURL User Profile Image URL String
 */
+ (TCGBProviderAuthProfile *)authProfileWithUserID:(NSString *)userID name:(NSString *)name email:(NSString *)email profileImageURL:(NSString *)profileImageURL;

- (NSString *)description;

@end


/** The TCGBProviderAuthExtra class indicates a result of the identity provider's authentification information.
 */
@interface TCGBProviderAuthExtra : NSObject <NSCopying>

/**---------------------------------------------------------------------------------------
 * @name Properties
 *  ---------------------------------------------------------------------------------------
 */

/**
 Extra information that is needed to store
 */
@property (nonatomic, strong) id         extra;

/**---------------------------------------------------------------------------------------
 * @name Initialization
 *  ---------------------------------------------------------------------------------------
 */

/**
 Creates TCGBProviderAuthExtra instance.
 
 @param extra Result dictionary of authentification
 */
+ (TCGBProviderAuthExtra *)authExtraWithExtraDictionary:(NSDictionary *)extra;
- (NSString *)description;
- (NSDictionary *)extraDictionary;
@end

/** The TCGBProviderAuthCredential represents an authenticated token and user information.
 This is an identify provider's information, not TCGB's.
 */
@interface TCGBProviderAuthCredential : NSObject <NSCopying>

/**---------------------------------------------------------------------------------------
 * @name Properties
 *  ---------------------------------------------------------------------------------------
 */

/**
 Auth Token
 */
@property (nonatomic, strong) TCGBProviderAuthToken*         authToken;

/**
 Auth Profile
 */
@property (nonatomic, strong) TCGBProviderAuthProfile*       authProfile;

/**
 Auth Extra information
 */
@property (nonatomic, strong) TCGBProviderAuthExtra*         authExtra;

/**---------------------------------------------------------------------------------------
 * @name Initialization
 *  ---------------------------------------------------------------------------------------
 */

/**
 Creates TCGBProviderAuthCredential instance.
 
 @param token Auth Token
 @param profile User proifle
 @param extra Extra auth information
 */
+ (TCGBProviderAuthCredential *)authCredentialWithAuthToken:(TCGBProviderAuthToken *)token authProfile:(TCGBProviderAuthProfile *)profile authExtra:(TCGBProviderAuthExtra *)extra;
- (NSString *)description;
@end
