//
//  TCGBTransferAccountRenewConfiguration.h
//  Gamebase
//
//  Created by NHN on 07/02/2019.
//  © NHN Corp. All rights reserved.
//
#import <Foundation/Foundation.h>
#import "TCGBTransferAccountConstants.h"

#ifndef TCGBTransferAccountRenewConfiguration_h
#define TCGBTransferAccountRenewConfiguration_h

@interface TCGBTransferAccountRenewConfiguration : NSObject

@property (nonatomic, assign) TCGBTransferAccountRenewalModeType renewalMode;
@property (nonatomic, assign) TCGBTransferAccountRenewalTargetType renewalTarget;
@property (nonatomic, strong, nullable) NSString* accountId;
@property (nonatomic, strong, nullable) NSString* accountPassword;

- (nonnull instancetype)init __attribute__((unavailable("init not available")));
- (nonnull instancetype)initWithRenewalMode:(TCGBTransferAccountRenewalModeType)renewalMode
                      renewalTarget:(TCGBTransferAccountRenewalTargetType)renewalTarget
                          accountId:(nullable NSString *)accountId
                    accountPassword:(nullable NSString *)accountPassword;

+ (nonnull TCGBTransferAccountRenewConfiguration *)autoRenewConfigurationWithRenewalTarget:(TCGBTransferAccountRenewalTargetType)renewalTarget;
+ (nonnull TCGBTransferAccountRenewConfiguration *)manualRenewConfigurationWithAccountPassword:(nonnull NSString *)accountPassword;
+ (nonnull TCGBTransferAccountRenewConfiguration *)manualRenewConfigurationWithAccountId:(nonnull NSString *)accountId
                                                              accountPassword:(nonnull NSString *)accountPassword;

- (nonnull NSString *)JSONString;
- (nonnull NSString *)JSONPrettyString;

@end

#endif /* TCGBTransferAccountRenewConfiguration_h */
