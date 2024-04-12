@protocol TCGBUnityPluginDelegate <NSObject>

- (void)didFinishLaunching;

@optional
+ (NSString *)versionString;

@end