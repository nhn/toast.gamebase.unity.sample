#import "TCGBUnityInterface.h"
#import <Gamebase/Gamebase.h>

@implementation TCGBUnityInterface

+ (TCGBUnityInterface *)sharedUnityInterface {
    static dispatch_once_t onceToken;
    static TCGBUnityInterface* instance;
    dispatch_once(&onceToken, ^{
        instance = [[TCGBUnityInterface alloc] init];
    });
    return instance;
}

- (id)init {
    if (self = [super init]) {
        UnityRegisterAppDelegateListener(self);
    }
    return self;
}

- (void)dealloc {
    UnityUnregisterAppDelegateListener(self);
}

#pragma mark - LifeCycleListener
- (void)didFinishLaunching:(NSNotification*)notification {
    [TCGBGamebase application:[UIApplication sharedApplication] didFinishLaunchingWithOptions:[notification userInfo]];
}

- (void)didBecomeActive:(NSNotification*)notification {
    [TCGBGamebase applicationDidBecomeActive:[UIApplication sharedApplication]];
}

- (void)willResignActive:(NSNotification*)notification {
    [TCGBGamebase applicationWillResignActive:[UIApplication sharedApplication]];
}

- (void)didEnterBackground:(NSNotification*)notification {
    [TCGBGamebase applicationDidEnterBackground:[UIApplication sharedApplication]];
}

- (void)willEnterForeground:(NSNotification*)notification {
    [TCGBGamebase applicationWillEnterForeground:[UIApplication sharedApplication]];
}

- (void)willTerminate:(NSNotification*)notification {
    [TCGBGamebase applicationWillTerminate:[UIApplication sharedApplication]];
}

#pragma mark - AppDelegateListener
- (void)onOpenURL:(NSNotification*)notification {
    NSURL* url = [notification userInfo][@"url"];
    [TCGBGamebase application:[UIApplication sharedApplication] openURL:url sourceApplication:[notification userInfo][@"sourceApplication"] annotation:[notification userInfo][@"annotation"]];
    //TODO:??
}

// these are just hooks to existing notifications
- (void)applicationDidReceiveMemoryWarning:(NSNotification*)notification {
    [TCGBGamebase applicationDidReceiveMemoryWarning:[UIApplication sharedApplication]];
}

- (void)applicationSignificantTimeChange:(NSNotification*)notification {
    [TCGBGamebase applicationSignificantTimeChange:[UIApplication sharedApplication]];
}
@end
