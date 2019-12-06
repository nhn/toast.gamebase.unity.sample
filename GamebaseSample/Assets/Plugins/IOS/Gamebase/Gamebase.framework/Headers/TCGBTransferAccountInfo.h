//
//  TCGBTransferAccountInfo.h
//  Gamebase
//
//  Created by NHN on 07/02/2019.
//  © NHN Corp. All rights reserved.
//
#import <Foundation/Foundation.h>
#import "TCGBTransferAccountConstants.h"

#ifndef TCGBTransferAccountInfo_h
#define TCGBTransferAccountInfo_h

@interface TCGBTransferAccountInfoAccount : NSObject

@property (nonatomic, strong, readonly, nonnull) NSString * accountId;
@property (nonatomic, strong, readonly, nullable) NSString * accountPassword;

- (nonnull instancetype)init __attribute__((unavailable("init not available.")));

- (nonnull NSString *)JSONString;
- (nonnull NSString *)JSONPrettyString;

@end


@interface TCGBTransferAccountInfoCondition : NSObject

@property (nonatomic, assign, readonly, nonnull) NSString* transferAccountType;
@property (nonatomic, assign, readonly, nonnull) NSString* expirationType;
@property (nonatomic, assign, readonly) long long expirationDate;

- (nonnull instancetype)init __attribute__((unavailable("init not available.")));

- (nonnull NSString *)JSONString;
- (nonnull NSString *)JSONPrettyString;

@end


@interface TCGBTransferAccountInfo : NSObject

@property (nonatomic, assign, readonly, nonnull) NSString* issuedType;
@property (nonatomic, strong, readonly, nonnull) TCGBTransferAccountInfoAccount* account;
@property (nonatomic, strong, readonly, nonnull) TCGBTransferAccountInfoCondition* condition;

- (nonnull instancetype)init __attribute__((unavailable("init not available.")));

- (nonnull NSString *)JSONString;
- (nonnull NSString *)JSONPrettyString;

@end

#endif /* TCGBTransferAccountInfo_h */
