#import <Foundation/Foundation.h>

#define GAMEBASE_API_NO_VIEW_CONTROLLER      @"gamebase://noViewController"

@interface DelegateManager : NSObject {
    NSMutableDictionary* _syncDelegateDictionary;
    NSMutableDictionary* _asyncDelegateDictionary;
}

@property (nonatomic, strong) NSMutableArray *classArray;
@property (nonatomic, strong) NSMutableDictionary* syncDelegateDictionary;
@property (nonatomic, strong) NSMutableDictionary* asyncDelegateDictionary;

+(DelegateManager*)sharedDelegateManager;

-(void) addAsyncDelegate:(NSString*)scheme target:(NSObject*)target selector:(SEL)selector;
-(void) addAsyncDelegate:(NSString*)scheme target:(NSObject*)target selector:(SEL)selector isViewController:(bool)isViewController;
-(void) addSyncDelegate:(NSString*)scheme target:(NSObject*)target selector:(SEL)selector;
-(NSInvocation*) getAsyncDelegate:(NSString*)scheme;
-(NSInvocation*) getSyncDelegate:(NSString*)scheme;
-(void) addClass:(id)clazz;

@end
