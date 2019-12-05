//
//  ToastInfoUtil.h
//  ToastCommon
//
//  Created by Hyup on 2017. 8. 25..
//  Copyright © 2017년 NHN. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface ToastInfoUtil : NSObject

#pragma mark - Application
+ (NSString *)bundleID;
+ (NSString *)bundleName;
+ (NSString *)displayName;
+ (NSString *)appVersion;
+ (NSString *)rootViewControllerTitle;
+ (NSString *)buildNumber;
+ (NSString *)appState;

#pragma mark - Account
+ (NSString *)iCloudToken;

#pragma mark - Device
+ (NSString *)carrier;
+ (NSString *)deviceName;
+ (NSString *)deviceModel;
+ (NSString *)deviceLocalizedModel;
+ (NSString *)deviceSystemName;
+ (NSString *)deviceSystemVersion;
+ (NSNumber *)deviceScreenWidth;
+ (NSNumber *)deviceScreenHeight;
+ (NSString *)deviceScreenResolution;
+ (NSString *)deviceOrientation;
+ (NSString *)deviceLanguageCode;
+ (NSString *)countryCode;
+ (NSString *)countryCodeFromLocale;
+ (NSString *)countryCodeFromUsim;
+ (NSString *)localeDisplayName;
+ (NSString *)languageCode;
+ (NSString *)os;
+ (NSString *)osVersion;
+ (NSString *)timezone;
+ (NSString *)freeMemory;
+ (NSString *)freeSpace;
+ (NSString *)hardwareMachine;
+ (NSString *)hardwareModel;
+ (NSString *)kernelUUID;
+ (NSString *)kernelVersion;
+ (NSString *)kernelBootSessionUUID;
+ (NSString *)kernelBootSignature;
+ (NSNumber *)kernelHostID;
+ (NSString *)kernelHostName;
+ (NSString *)kernelOSType;
+ (NSString *)kernelOSRelease;
+ (NSNumber *)kernelOSRevision;
+ (NSString *)mobileCountryCode;
+ (NSString *)mobileNetworkCode;
+ (BOOL)isSimulator;
+ (NSString *)cpuArchitecture;
+ (BOOL)isAvailableSystemVersion:(NSString *)version;

#pragma mark - Identifier
+ (NSString *)idfa;
+ (NSString *)idfv;
+ (NSString *)deviceUUID;
+ (NSString *)launchedID;
+ (NSString *)keychainUUID;

#pragma mark - JailbreakInfo
+ (NSArray *)jailbreakFilePath;
+ (NSArray *)jailbreakDyld;
+ (NSArray *)jailbreakSymlinked;
+ (NSArray *)jailbreakWritable;
+ (NSArray *)jailbreakUrlSchemesOpenable;
+ (NSArray *)jailbreakSystemCall;

#pragma mark - Network
+ (NSString *)networkType;
+ (NSString *)ip;
+ (NSString *)cellIP;
+ (NSString *)wifiIP;
+ (NSString *)netmask;
+ (NSString *)ssid;
+ (NSString *)bssid;
+ (NSString *)proxy;

#pragma mark - Common
+ (NSString *)userAgent;
+ (NSString *)userAgentWithSDKName:(NSString *)aName currentVersion:(NSString *)aVersion;

#pragma mark - Version
+ (NSString *)version;
@end
