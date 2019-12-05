//
//  TCGBToastLoggerDelegate.h
//  Gamebase
//
//  Created by NHNEnt on 04/06/2019.
//  Copyright © 2019 NHN Corp. All rights reserved.
//

#ifndef TCGBToastLoggerDelegate_h
#define TCGBToastLoggerDelegate_h

#import "TCGBLog.h"
#import "TCGBLogFilter.h"

@protocol TCGBLoggerDelegate<NSObject>

@optional
/**
 Called after the log has been successfully sent.
 
 @param log The log that sent successfully
 */
- (void)tcgbLogDidSuccess:(TCGBLog *)log;

/**
 Called after the log fails to send.
 
 @param log The log that sent failed
 @param error The error about the cause of the failure
 */
- (void)tcgbLogDidFail:(TCGBLog *)log error:(TCGBError *)error;

/**
 Called after the log has been successfully saved.
 
 @param log The log that saved successfully
 */
- (void)tcgbLogDidSave:(TCGBLog *)log;

/**
 Called after the log has been successfully filtered.
 
 @param log The log that filtered successfully
 @param logFilter The filter used when filtering log
 */
- (void)tcgbLogDidFilter:(TCGBLog *)log logFilter:(TCGBLogFilter *)logFilter;

@end

#endif /* TCGBToastLoggerDelegate_h */
