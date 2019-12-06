//
//  TCGBPushDelegate.h
//  Gamebase
//
//  Created by NHNEnt on 28/08/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#ifndef TCGBPushDelegate_h
#define TCGBPushDelegate_h

#import "TCGBPush.h"
#import "TCGBError.h"

/** The protocol TCGBPushDelegate is for developing Push Adapter.
 */
@protocol TCGBPushDelegate <NSObject>

- (void)registerPushWithPushConfiguration:(TCGBPushConfiguration *)configuration completion:(void(^)(TCGBError *error))completion;

- (void)queryPushWithCompletion:(void(^)(TCGBPushConfiguration *configuration, TCGBError *error))completion;

- (void)setSandboxMode:(BOOL)isSandbox;

- (void)setAppKey:(NSString *)appKey serviceZone:(NSString *)serviceZone displayLanguageCode:(NSString *)displayLanguageCode;

@end

#endif /* TCGBPushDelegate_h */
