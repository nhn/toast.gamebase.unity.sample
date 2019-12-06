//
//  TCGBLaunching.h
//  Gamebase
//
//  Created by NHN on 2016. 6. 27..
//  © NHN Corp. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "TCGBError.h"

#ifndef TCGBLaunching_h
#define TCGBLaunching_h

/// JSONPath for Launching Informations

typedef NS_ENUM(NSUInteger, TCGBLaunchingStatus) {
    IN_SERVICE = 200,
    RECOMMEND_UPDATE = 201,
    IN_SERVICE_BY_QA_WHITE_LIST = 202,
    IN_TEST = 203,
    IN_REVIEW = 204,
    
    REQUIRE_UPDATE = 300,
    BLOCKED_USER = 301,
    TERMINATED_SERVICE = 302,
    INSPECTING_SERVICE = 303,               // 상태 및 Raw Data를 모두 넘겨줄 수 있을 경우
    INSPECTING_ALL_SERVICES = 304,   // 상태만 넘겨줄 수 있을 경우
    
    INTERNAL_SERVER_ERROR = 500,
    
//    UNREGISTERED_GAME = 400,              // 400 대 에러는 TCGBError 로 설정
//    UNREGISTERED_CLIENT = 401,
};

// launching정보에서 statuscode가 변경되었을 때만 Noti해줌.
extern NSString* const kTCGBLaunchingStatusCodeChangedNotification;

typedef void(^launchingCompletion)(id launchingData, TCGBError *error);
typedef void(^changedStatusNotificationHandler)(NSDictionary *launchingStatus);


@class TCGBWebSocket;

/** The TCGBLaunching class contains several informations that is received from the server after application is successfully launched.
 */
@interface TCGBLaunching : NSObject
{
    __weak TCGBWebSocket*        _webSocket;
    NSMutableArray*             _statusNotificationHandlers;
    NSDictionary*               _launchingStatusData;
    NSDate*                     _dateOfLastUpdatedStatus;
    TCGBLaunchingStatus          _launchingStatus;
    
    BOOL                        _registeredScheduleForStatus;
    NSTimer*                    _refreshTimer;
    launchingCompletion         _completionHandler;
    
    NSDictionary*               _launchingInformations;
    NSDictionary*               _TCGBLaunchingInformations;
    NSArray*                    _TCIAPInformations;
    NSString*                   _TCLaunchingInformations;
    NSDictionary*               _TCProductInformations;
    
    /**
     Last Checked Notification Time. Notification could be set in the Toast Cloud Console.
     */
    NSNumber*                   _lastLCNT;
}

/**---------------------------------------------------------------------------------------
 * @name Launching Information
 *  ---------------------------------------------------------------------------------------
 */

/**
 @return Launching Status which indicates whether the application is playable or not.
 @see TCGBLaunchingStatus
 */
+ (TCGBLaunchingStatus)launchingStatus;

/**
 @return All Launching Information from the server.
 */
+ (NSDictionary *)launchingInformations;

/**
 @return Launching Information related with TCGB launching service.
 */
+ (NSDictionary *)TCGBLaunchingInformations;

/**
 @return Launching Information related with toast cloud product.
 */
+ (NSDictionary *)tcProductInformations;

/**
 @return Launching Information related with toast cloud launching.
 */
+ (NSString *)tcLaunchingInformations;

/**
 @return Launching Information related with toast cloud IAP.
 */
+ (NSArray *)tcIAPInformations;

@end

#endif // TCGBLaunching_h
