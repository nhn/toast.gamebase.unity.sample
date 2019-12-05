//
//  TCGBAuthGoogleProfile.h
//  GamebaseAuthGoogleAdapter
//
//  Created by NHN on 2018. 5. 24..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface TCGBAuthGoogleProfile : NSObject <NSCopying>
@property (nonatomic, strong) NSString* ID;
@property (nonatomic, strong) NSString* email;
@property (nonatomic, strong) NSString* displayName;
@property (nonatomic, strong) NSString* imageURL;

+ (TCGBAuthGoogleProfile *)googleProfileWithDictionary:(NSDictionary *)dictionary;
- (id)initWithDictionary:(NSDictionary *)dictionary;
@end


