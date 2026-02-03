#import "EngineMessage.h"

@class TCGBError;

@interface NativeMessage : NSObject {
    NSString* _scheme;
    NSInteger _handle;
    NSString* _gamebaseError;
    NSString* _jsonData;
    NSString* _extraData;
}

@property (nonatomic, strong) NSString* scheme;
@property (nonatomic, assign) NSInteger handle;
@property (nonatomic, strong) NSString* gamebaseError;
@property (nonatomic, strong) NSString* jsonData;
@property (nonatomic, strong) NSString* extraData;

-(instancetype)initWithMessage:(EngineMessage*)engineMessage TCGBError:(TCGBError*)gamebaseError jsonData:(NSString*)jsonData extraData:(NSString*)extraData;
-(instancetype)initWithMessage:(NSString*)scheme handle:(NSInteger)handle TCGBError:(TCGBError*)gamebaseError jsonData:(NSString*)jsonData extraData:(NSString*)extraData;

-(NSString*)toJsonString;
@end
