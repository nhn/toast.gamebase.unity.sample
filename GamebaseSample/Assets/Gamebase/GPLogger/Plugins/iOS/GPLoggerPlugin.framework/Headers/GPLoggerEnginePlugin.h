//
//  GPLoggerEnginePlugin.h
//  GPLoggerPlugin
//
//  Created by NHN on 3/14/24.
//

#import <Foundation/Foundation.h>

@protocol GPLoggerEnginePlugin <NSObject>

@required

@property (nonatomic, readonly, strong) NSString *engineName;

- (void)sendMessage:(const char* )method message:(const char* )message;

@end

@interface GPLoggerEngineController : NSObject

+ (void)registerPlugin:(id<GPLoggerEnginePlugin>)enginePlugin;
+ (id<GPLoggerEnginePlugin>)getPlugin;

@end
