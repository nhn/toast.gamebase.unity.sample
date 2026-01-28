@interface EngineMessage : NSObject {
    NSString* _scheme;
    NSInteger _handle;
}

@property (nonatomic, strong) NSString* scheme;
@property (nonatomic, assign) NSInteger handle;
@property (nonatomic, strong) NSString* gameObjectName;
@property (nonatomic, strong) NSString* responseMethodName;

-(id)initWithJsonString:(NSString*)jsonString;

- (NSDictionary *)getData;
- (NSDictionary *)getExtraData;
- (NSString *)getDataJSONString;
- (NSString *)getExtraDataJSONString;
- (NSString *)getOriginalString;

@end
