#import "NativeMessage.h"

@interface TCGBCallback

typedef void(^SendCompletion)(NSString* jsonData, NativeMessage* message);

@end
