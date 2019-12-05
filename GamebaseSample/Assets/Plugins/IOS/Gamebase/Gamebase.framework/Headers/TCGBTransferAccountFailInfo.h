//
//  TCGBTransferAccountFailInfo.h
//  Gamebase
//
//  Created by NHN on 07/02/2019.
//  © NHN Corp. All rights reserved.
//
#import <Foundation/Foundation.h>
#import "TCGBError.h"

#ifndef TCGBTransferAccountFailInfo_h
#define TCGBTransferAccountFailInfo_h

@interface TCGBTransferAccountFailInfo : NSObject

@property (nonatomic, strong, readonly, nonnull) NSString* appId;
@property (nonatomic, strong, readonly, nonnull) NSString* accountId;
@property (nonatomic, strong, readonly, nonnull) NSString* status;
@property (nonatomic, assign, readonly) NSInteger failCount;
@property (nonatomic, assign, readonly) long long blockEndDate;
@property (nonatomic, assign, readonly) long long regDate;

- (nonnull instancetype)init __attribute__((unavailable("init not available.")));
+ (nonnull TCGBTransferAccountFailInfo *)resultWithTCGBError:(nonnull TCGBError *)error;

@end

#endif /* TCGBTransferAccountFailInfo_h */
