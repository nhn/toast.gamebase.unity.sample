#import "EngineMessage.h"
#import "NativeMessage.h"

@interface TCGBCallback

typedef void(^SendCompletion)(EngineMessage* engineMessage, NativeMessage* nativeMessage);

@end
