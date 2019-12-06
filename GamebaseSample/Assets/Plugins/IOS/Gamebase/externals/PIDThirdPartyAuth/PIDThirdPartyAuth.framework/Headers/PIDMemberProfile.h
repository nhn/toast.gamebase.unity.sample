//
//  PIDMemberProfile.h
//  PIDAuth
//
//  Created by NHNEnt on 08/07/2019.
//  Copyright © 2019 NHN Entertainment Corp. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

/// @brief isNeedsAgreement:에 사용되는 값으로 사용자의 동의 여부를 확인하는 데 사용되는 항목입니다.
extern NSString *const PIDMemberIdNo;
extern NSString *const PIDMemberEmail;
extern NSString *const PIDMemberMaskedEmail;
extern NSString *const PIDMemberMobile;
extern NSString *const PIDMemberMaskedMobile;
extern NSString *const PIDMemberName;
extern NSString *const PIDMemberGenderCode;
extern NSString *const PIDMemberBirthday;
extern NSString *const PIDMemberAgeGroup;

@class PIDMemberProfile;
@class PIDError;

typedef void (^PIDMemberProfileHandler)(PIDMemberProfile * _Nullable memberProfile, PIDError * _Nullable error);

/**
 PAYCO의 사용자 정보를 나타내는 클래스
 
 @discussion 사용자의 동의를 받은 정보만 조회할 수 있습니다.
             각 항목은 사용자 동의 여부, 정보 존재 여부에 따라서 nil 혹은 empty string("")일 수 있습니다.
             각 항목에 대한 사용자의 동의 여부는 [PIDMemberProfile isNeedsAgreement:]를 통해 확인하실 수 있습니다.
 */
@interface PIDMemberProfile : NSObject

/// @brief 회원 UUID
@property(nonatomic, strong, readonly) NSString *idNo;
/// @brief 이메일 아이디(이메일 아이디에 값이 없으면 모바일 아이디만 존재)
@property(nonatomic, strong, readonly, nullable) NSString *email;
/// @brief 마스킹된 이메일 아이디
@property(nonatomic, strong, readonly, nullable) NSString *maskedEmail;
/// @brief 모바일 아이디
@property(nonatomic, strong, readonly, nullable) NSString *mobile;
/// @brief 마스킹된 모바일 아이디
@property(nonatomic, strong, readonly, nullable) NSString *maskedMobile;
/// @brief 이름
@property(nonatomic, strong, readonly, nullable) NSString *name;
/// @brief 성별
@property(nonatomic, strong, readonly, nullable) NSString *genderCode;
/// @brief 생일
@property(nonatomic, strong, readonly, nullable) NSString *birthday;
/// @brief 연령대 (0, 10, 20, 30, ....)
@property(nonatomic, strong, readonly, nullable) NSString *ageGroup;

/// @brief 제휴를 통해 권한이 부여된 특정 서비스에서 사용되는 데이터. extraData의 값에는 null([NSNull null])이 포함될 수 있습니다.
@property(nonatomic, strong, readonly, nullable) NSDictionary *extraData;

/**
 각 항목에 대한 사용자 동의 필요 여부
 
 @example BOOL isEmailNeedsAgreement = [memberProfile isNeedsAgreement:PIDMemberEmail];
 
 @param key 사용자 동의 필요 여부를 확인할 항목(PIDMember{항목이름})
 @return 각 항목에 대한 사용자 동의 필요 여부. return 값이 YES면 해당하는 정보를 확인할 수 없습니다. 동의를 받은 후에 확인이 가능합니다.
 */
- (BOOL)isNeedsAgreement:(NSString *)key;

@end

NS_ASSUME_NONNULL_END
