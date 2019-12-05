//
//  TCGBAuthFacebookLogin.h
//  TCGBAuthFacebookAdapter
//
//  Created by NHN on 2016. 6. 7..
//  © NHN Corp. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <Gamebase/Gamebase.h>

#define GamebaseAuthFacebookAdapterVersion @"2.0.1"

@class TCGBAuth;
@class FBSDKLoginManager;
@class TCGBProviderAuthCredential;
@protocol TCGBAuthAdapterDelegate;


@interface TCGBAuthFacebookLogin : NSObject <TCGBAuthAdapterDelegate>

@property (nonatomic, strong)       NSString*   clientId;
@property (nonatomic, strong)       NSString*   clientSecret;
@property (nonatomic, strong)       NSArray*    permissions;
@property (nonatomic, strong)       FBSDKLoginManager* loginManager;
@property (nonatomic, strong)       NSDictionary* options;
@property (nonatomic, strong)       TCGBProviderAuthCredential* credential;
@property (nonatomic)               BOOL          calledDidFinishLaunching;

+ (NSDictionary *)dictionaryMergedByPriorityWithAdditionalInfo:(NSDictionary *)additionalInfo;

@end
