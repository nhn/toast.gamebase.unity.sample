#import "TCGBUnityInterface.h"
#import "TCGBUnityPluginDelegate.h"
#import "TCGBUnityViewController.h"
#import <Gamebase/Gamebase.h>
#import <GamebasePlugin/GamebasePlugin.h>
#import <objc/runtime.h>

@interface TCGBUnityInterface()

@property (atomic, strong) NSMutableDictionary<NSString *, id<TCGBUnityPluginDelegate>>* adapterInstance;
@property (atomic, strong) id<TCGBViewControllerDelegate> viewControllerDelegate;

@end

@implementation TCGBUnityInterface

static bool _startUnityScheduled = false;

+ (void)load {
    UnityRegisterAppDelegateListener([TCGBUnityInterface sharedUnityInterface]);
}

+ (TCGBUnityInterface *)sharedUnityInterface {
    static dispatch_once_t onceToken;
    static TCGBUnityInterface* instance;
    dispatch_once(&onceToken, ^{
        instance = [[TCGBUnityInterface alloc] init];
    });
    return instance;
}

- (void)addUnityPluginAdapter:(NSString *)type {
    if ([_adapterInstance objectForKey:type] != nil) {
        return;
    }
    
    id<TCGBUnityPluginDelegate> adapterInstance = [[NSClassFromString(type) alloc] init];
    [_adapterInstance setObject:adapterInstance forKey:type];
    
    if ([[adapterInstance class] respondsToSelector:@selector(versionString)] == YES) {
        NSString *adapterVersion = [[adapterInstance class] versionString];
        [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][TCGBUnityInterface] Add unity plugin adapter. (type= %@, version= %@)", type, adapterVersion];
    }
}

- (instancetype)init {
    if (self = [super init]) {
        _adapterInstance = [NSMutableDictionary dictionary];
    }
    
    return self;
}

- (void)setupViewController {
    [TCGBUtil logDebugWithFormat:@"[TCGB][Plugin][TCGBUnityInterface] setupViewController"];

    _viewControllerDelegate = [[TCGBUnityViewController alloc] init];
    [[TCGBViewControllerManager sharedGamebaseViewControllerManager] setDelegate:_viewControllerDelegate];
}

#pragma mark - LifeCycleListener
- (void)didFinishLaunching:(NSNotification*)notification {
    [TCGBGamebase application:[UIApplication sharedApplication] didFinishLaunchingWithOptions:[notification userInfo]];
    
    for (NSString* adapterName in _adapterInstance) {
        id<TCGBUnityPluginDelegate> adapter = [_adapterInstance objectForKey:adapterName];
        if ([adapter respondsToSelector:@selector(didFinishLaunching)] == YES) {
            [adapter didFinishLaunching];
        }
    }
}

- (void)didBecomeActive:(NSNotification*)notification {
    if (!_startUnityScheduled)
    {
        _startUnityScheduled = true;
        [self performSelector: @selector(setupViewController) withObject: nil afterDelay: 0];
    }
    
    if ([TCGBGamebase appID] != nil) {
        [TCGBGamebase applicationDidBecomeActive:[UIApplication sharedApplication]];
    }
}

- (void)willResignActive:(NSNotification*)notification {
    if ([TCGBGamebase appID] != nil) {
        [TCGBGamebase applicationWillResignActive:[UIApplication sharedApplication]];
    }
}

- (void)didEnterBackground:(NSNotification*)notification {
    if ([TCGBGamebase appID] != nil) {
        [TCGBGamebase applicationDidEnterBackground:[UIApplication sharedApplication]];
    }
}

- (void)willEnterForeground:(NSNotification*)notification {
    if ([TCGBGamebase appID] != nil) {
        [TCGBGamebase applicationWillEnterForeground:[UIApplication sharedApplication]];
    }
}

- (void)willTerminate:(NSNotification*)notification {
    if ([TCGBGamebase appID] != nil) {
        [TCGBGamebase applicationWillTerminate:[UIApplication sharedApplication]];
    }
}

#pragma mark - AppDelegateListener
- (void)onOpenURL:(NSNotification*)notification {
    NSURL* url = [notification userInfo][@"url"];
    [TCGBGamebase application:[UIApplication sharedApplication] openURL:url sourceApplication:[notification userInfo][@"sourceApplication"] annotation:[notification userInfo][@"annotation"]];
}

// these are just hooks to existing notifications
- (void)applicationDidReceiveMemoryWarning:(NSNotification*)notification {
    [TCGBGamebase applicationDidReceiveMemoryWarning:[UIApplication sharedApplication]];
}

- (void)applicationSignificantTimeChange:(NSNotification*)notification {
    [TCGBGamebase applicationSignificantTimeChange:[UIApplication sharedApplication]];
}
@end
