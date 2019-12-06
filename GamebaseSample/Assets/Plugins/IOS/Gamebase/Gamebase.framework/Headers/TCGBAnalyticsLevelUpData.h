//
//  TCGBAnalyticsLevelUpData.h
//  Gamebase
//
//  Created by NHN on 21/11/2018.
//  © NHN Corp. All rights reserved.
//
#import <Foundation/Foundation.h>

#ifndef TCGBAnalyticsLevelUpData_h
#define TCGBAnalyticsLevelUpData_h

@interface TCGBAnalyticsLevelUpData : NSObject

// Property
@property (nonatomic, assign) int userLevel; // Required.
@property (nonatomic, assign) long long levelUpTime; // Required. epochTime in millis. Accept only positive values.


// Initializer
- (nonnull instancetype)init __attribute__((unavailable("Must use initWithUserLevel:levelUpTime: instead.")));
- (nonnull instancetype)initWithUserLevel:(int)userLevel levelUpTime:(long long)levelUpTime;;
+ (nonnull instancetype)levelUpDataWithUserLevel:(int)userLevel levelUpTime:(long long)levelUpTime;

// for iOS Only
- (void)setLevelUpTimeWithDate:(nonnull NSDate *)now; // Convert NSDate to epoch time and set levelUpTime.

@end

#endif /* TCGBAnalyticsLevelUpData_h */
