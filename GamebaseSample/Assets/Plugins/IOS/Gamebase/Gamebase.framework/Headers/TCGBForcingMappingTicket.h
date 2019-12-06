//
//  TCGBForcingMappingTicket.h
//  Gamebase
//
//  Created by NHN on 18/02/2019.
//  © NHN Corp. All rights reserved.
//

#ifndef TCGBForcingMappingTicket_h
#define TCGBForcingMappingTicket_h

#import <Foundation/Foundation.h>
#import "TCGBError.h"

@interface TCGBForcingMappingTicket : NSObject

@property (nonatomic, strong, nonnull, readonly) NSString* userId;
@property (nonatomic, strong, nonnull, readonly) NSString* mappedUserId;
@property (nonatomic, strong, nonnull, readonly) NSString* idPCode;
@property (nonatomic, strong, nonnull, readonly) NSString* forcingMappingKey;
@property (nonatomic, assign, readonly) long long expirationDate;

- (nonnull instancetype)init __attribute__((unavailable("init not available.")));
+ (nullable TCGBForcingMappingTicket *)forcingMappingTicketWithError:(nonnull TCGBError *)error;

@end


#endif /* TCGBForcingMappingTicket_h */
