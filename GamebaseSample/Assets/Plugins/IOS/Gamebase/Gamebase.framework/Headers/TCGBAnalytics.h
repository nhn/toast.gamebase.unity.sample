//
//  TCGBAnalytics.h
//  Gamebase
//
//  Created by NHN on 16/11/2018.
//  © NHN Corp. All rights reserved.
//
#import <Foundation/Foundation.h>
#import "TCGBAnalyticsGameUserData.h"
#import "TCGBAnalyticsLevelUpData.h"

#ifndef TCGBAnalytics_h
#define TCGBAnalytics_h

@interface TCGBAnalytics : NSObject

/**
 Send a game user data to Gamebase Server for analyzing the data.
 This method should be called after login.
 
 @param gameUserData Game User Data
 @since Added 2.0.0
 */
+ (void)setGameUserData:(nonnull TCGBAnalyticsGameUserData *)gameUserData;

/**
 Send a level up data to Gamebase server for analyzing the data.
 This method should be called after level up event.
 
 @param levelUpData Level Up Data
 @since Added 2.0.0
 */
+ (void)traceLevelUpWithLevelUpData:(nonnull TCGBAnalyticsLevelUpData *)levelUpData;

@end

#endif /* TCGBAnalytics_h */
