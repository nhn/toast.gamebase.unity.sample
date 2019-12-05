//
//  TCGBToastLoggerConfiguration.h
//  Gamebase
//
//  Created by NHNEnt on 08/07/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#ifndef TCGBToastLoggerConfiguration_h
#define TCGBToastLoggerConfiguration_h

#import <Foundation/Foundation.h>

@interface TCGBLoggerConfiguration : NSObject

@property (strong, nonatomic, readonly) NSString *appKey;
@property (assign, nonatomic, readonly) BOOL enableCrashReporter;
@property (assign, nonatomic, readonly) NSString *serviceZone;

- (instancetype)init __attribute__((unavailable("Must use initWithAppKey: instead.")));

- (instancetype)initWithAppKey:(NSString *)appKey;

- (instancetype)initWithAppKey:(NSString *)appKey
           enableCrashReporter:(BOOL)enableCrashReporter;

- (void)setServiceZone:(NSString *)serviceZone;

+ (TCGBLoggerConfiguration *)configurationWithAppKey:(NSString *)appKey;

+ (TCGBLoggerConfiguration *)configurationWithAppKey:(NSString *)appKey
                                 enableCrashReporter:(BOOL)enableCrashReporter;

@end

#endif /* TCGBToastLoggerConfiguration_h */
