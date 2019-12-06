#import <Foundation/Foundation.h>
#import "DelegateManager.h"
#import "TCGBGamebasePlugin.h"
#import "TCGBAuthPlugin.h"
#import "TCGBLaunchingPlugin.h"
#import "TCGBPurchasePlugin.h"
#import "TCGBPushPlugin.h"
#import "TCGBWebviewPlugin.h"
#import "TCGBUtilPlugin.h"
#import "TCGBNetworkPlugin.h"

@implementation DelegateManager

@synthesize syncDelegateDictionary = _syncDelegateDictionary;
@synthesize asyncDelegateDictionary = _asyncDelegateDictionary;
@synthesize classArray = _classArray;

+(DelegateManager*)sharedDelegateManager {
    static dispatch_once_t onceToken;
    static DelegateManager* instance = nil;
    dispatch_once(&onceToken, ^{
        instance = [[DelegateManager alloc] init];
        instance.syncDelegateDictionary = [NSMutableDictionary dictionary];
        instance.asyncDelegateDictionary =[NSMutableDictionary dictionary];
        instance.classArray = [NSMutableArray array];

    });
    return instance;
}


-(void) addAsyncDelegate:(NSString*)scheme target:(NSObject*)target selector:(SEL)selector{
    NSMethodSignature *signature = [target methodSignatureForSelector:selector];
    NSInvocation *invocation = [NSInvocation invocationWithMethodSignature:signature];
    [invocation setTarget:target];
    [invocation setSelector:selector];
    [_asyncDelegateDictionary setObject:invocation forKey:scheme];
}

-(void) addSyncDelegate:(NSString*)scheme target:(NSObject*)target selector:(SEL)selector{
    NSMethodSignature *signature = [target methodSignatureForSelector:selector];
    NSInvocation *invocation = [NSInvocation invocationWithMethodSignature:signature];
    [invocation setTarget:target];
    [invocation setSelector:selector];
    [_syncDelegateDictionary setObject:invocation forKey:scheme];
}

-(NSInvocation*) getAsyncDelegate:(NSString*)scheme{
    return [_asyncDelegateDictionary objectForKey:scheme];
}

-(NSInvocation*) getSyncDelegate:(NSString*)scheme{
    return [_syncDelegateDictionary objectForKey:scheme];
}

-(void) addClass:(id)clazz{
    [_classArray addObject:clazz];
}
@end

