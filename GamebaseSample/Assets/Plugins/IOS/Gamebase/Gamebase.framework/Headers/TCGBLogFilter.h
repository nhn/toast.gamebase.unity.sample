//
//  TCGBToastLoggerFilter.h
//  Gamebase
//
//  Created by NHNEnt on 04/06/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#ifndef TCGBToastLogFilter_h
#define TCGBToastLogFilter_h

#import <Foundation/Foundation.h>

@interface TCGBLogFilter : NSObject

/**
 Gets name of filter class.
 
 @return The name of filter class
 */
- (NSString *)name;

@end

#endif /* TCGBToastLogFilter_h */
