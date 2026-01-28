#import "TCGBCallback.h"
#import "EngineMessage.h"

@interface TCGBPluginData : NSObject {
    EngineMessage* _engineMessage;
    SendCompletion _completion;
}

@property (nonatomic, strong) EngineMessage* engineMessage;
@property (nonatomic, strong) SendCompletion completion;

-(id)initWithJsonData:(NSString*)jsonData completion:(SendCompletion)completion;
-(id)initWithEngineMessage:(EngineMessage*)engineMessage completion:(SendCompletion)completion;

- (NSDictionary *)getData;
- (NSDictionary *)getExtraData;
- (NSString *)getDataJSONString;
- (NSString *)getExtraDataJSONString;
- (NSString *)getOriginalString;

@end
