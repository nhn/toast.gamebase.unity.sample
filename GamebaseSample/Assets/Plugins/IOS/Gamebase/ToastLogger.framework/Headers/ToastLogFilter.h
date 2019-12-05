//
//  ToastLogFilter.h
//  ToastLogger
//
//  Created by Hyup on 2017. 9. 18..
//  Copyright © 2017년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ToastLog.h"

NS_ASSUME_NONNULL_BEGIN

/**
 # ToastLogFilter
 
 The base class for several filter classes.
 
 Types of filter :
 
 * level filter
 * type filter
 * duplicate filter
 * session log filter
 * crash log filter
 * normal log filter
 
 */
@interface ToastLogFilter : NSObject

/// ---------------------------------
/// @name Get Methods
/// ---------------------------------

/**
 Whether or not to filter the log.

 @param log Log to be judged whether to filter.
 @return If `YES`, enables filtering. If `NO`, disable it.
 */
- (BOOL)filter:(ToastLog *)log;

/**
 Gets name of filter class.

 @return The name of filter class
 */
- (NSString *)name;

@end

NS_ASSUME_NONNULL_END
