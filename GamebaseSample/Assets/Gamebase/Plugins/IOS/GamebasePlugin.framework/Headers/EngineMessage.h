@interface EngineMessage : NSObject {
    NSString* _scheme;
    NSInteger _handle;
    NSString* _jsonData;
    NSString* _extraData;
}

@property (nonatomic, strong) NSString* scheme;
@property (nonatomic, assign) NSInteger handle;
@property (nonatomic, strong) NSString* jsonData;
@property (nonatomic, strong) NSString* extraData;

-(id)initWithJsonString:(NSString*)jsonString;

@end
