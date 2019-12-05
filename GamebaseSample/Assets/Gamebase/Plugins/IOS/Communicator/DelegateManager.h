#import <Foundation/Foundation.h>

@interface DelegateManager : NSObject {
    NSMutableDictionary* _syncDelegateDictionary;
    NSMutableDictionary* _asyncDelegateDictionary;
}

@property (nonatomic, strong) NSMutableArray *classArray;
@property (nonatomic, strong) NSMutableDictionary* syncDelegateDictionary;
@property (nonatomic, strong) NSMutableDictionary* asyncDelegateDictionary;

+(DelegateManager*)sharedDelegateManager;


-(void) addAsyncDelegate:(NSString*)scheme target:(NSObject*)target selector:(SEL)selector;
-(void) addSyncDelegate:(NSString*)scheme target:(NSObject*)target selector:(SEL)selector;
-(NSInvocation*) getAsyncDelegate:(NSString*)scheme;
-(NSInvocation*) getSyncDelegate:(NSString*)scheme;
-(void) addClass:(id)clazz;
@end
