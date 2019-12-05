//
//  TCGBTransferAccountConstants.h
//  Gamebase
//
//  Created by NHN on 07/02/2019.
//  © NHN Corp. All rights reserved.
//

#ifndef TCGBTransferAccountConstants_h
#define TCGBTransferAccountConstants_h

typedef NS_ENUM(NSUInteger, TCGBTransferAccountRenewalModeType) {
    TCGBTransferAccountRenewalModeTypeAuto = 0,
    TCGBTransferAccountRenewalModeTypeManual
};

typedef NS_ENUM(NSUInteger, TCGBTransferAccountRenewalTargetType) {
    TCGBTransferAccountRenewalTargetTypePassword = 0,
    TCGBTransferAccountRenewalTargetTypeIdPassword
};

#endif /* TCGBTransferAccountConstants_h */
