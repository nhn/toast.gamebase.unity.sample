#import "TCGBCallback.h"

@interface TCGBPluginData : NSObject {
    NSString* _jsonData;
    SendCompletion _completion;
}

@property (nonatomic, strong) NSString* jsonData;
@property (nonatomic, strong) SendCompletion completion;

-(id)initWithJsonData:(NSString*)jsonData completion:(SendCompletion)completion;
@end
