//
//  TCGBAuthProviderProfile.h
//  Gamebase
//
//  Created by NHN on 2017. 3. 16..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface TCGBAuthProviderProfile : NSObject

@property (nonatomic, strong, readonly) NSString*     userID;
@property (nonatomic, strong, readonly) NSDictionary* information;

+ (TCGBAuthProviderProfile *)authProviderProfileWithUserID:(NSString *)userID information:(NSDictionary *)information;

@end
