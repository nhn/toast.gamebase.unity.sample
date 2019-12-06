//
//  TCGBSystemInfo.h
//  Gamebase
//
//  Created by NHN on 2016. 6. 30..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface TCGBSystemInfo : NSObject

+ (NSString *)zoneType;
+ (NSString *)getUUID;
+ (NSString *)UDID;
+ (NSString *)ADID;
+ (NSString *)carrierCode;
+ (NSString *)carrierName;
+ (NSString *)countryCode;
+ (NSString *)languageCode;

@end
