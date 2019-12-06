//
//  TCGBNetwork.h
//  Gamebase
//
//  Created by NHN on 2017. 1. 9..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "TCGBConstants.h"

#ifndef TCGBNETWORK_H
#define TCGBNETWORK_H

typedef void(^NetworkChangedHandler)(NetworkStatus status);

/** The TCGBNetwork class indicates Network status.
 */
@interface TCGBNetwork : NSObject

/**---------------------------------------------------------------------------------------
 * @name Network Status
 *  ---------------------------------------------------------------------------------------
 */

/**
 @return NetworkStatus
 @see NetworkStatus
 */
+ (NetworkStatus)type;

/**
 @return Stringify to NetworkStatus
 @see NetworkStatus
 */
+ (NSString *)typeName;

/**
 @return `YES` if network is reachable.
 */
+ (BOOL)isConnected;

@end

#endif
