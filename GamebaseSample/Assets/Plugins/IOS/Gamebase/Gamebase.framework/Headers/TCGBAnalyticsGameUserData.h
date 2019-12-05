//
//  TCGBAnalyticsGameUserData.h
//  Gamebase
//
//  Created by NHN on 21/11/2018.
//  © NHN Corp. All rights reserved.
//
#import <Foundation/Foundation.h>

#ifndef TCGBAnalyticsGameUserData_h
#define TCGBAnalyticsGameUserData_h

@interface TCGBAnalyticsGameUserData : NSObject

// Property
@property (nonatomic, assign) int userLevel; // Required.
@property (nonatomic, strong, nullable) NSString* channelId; // Optional. Default value is nil. Accept only positive values.
@property (nonatomic, strong, nullable) NSString* characterId; // Optional. Default value is nil.
@property (nonatomic, strong, nullable) NSString* classId; // Optional. Default value is nil.

// Initializer
- (nonnull instancetype)init __attribute__((unavailable("Must use initWithUserLevel: instead.")));
- (nonnull instancetype)initWithUserLevel:(int)userLevel;
+ (nonnull instancetype)gameUserDataWithUserLevel:(int)userLevel;

@end

#endif /* TCGBAnalyticsGameUserData_h */
