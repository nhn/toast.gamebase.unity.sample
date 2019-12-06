/*
 *
 * Copyright © 2016년 NHN Entertainment Corp. All rights reserved.
 *
 */

#import <UIKit/UIKit.h>


@interface UIViewController (PIDAuthAdditions)

/// @brief 호출하는 view controller의 네비게이션바 속성에 맞게 화면을 띄워줍니다.
- (UIViewController *)PIDPresentViewController:(UIViewController *)viewController animated:(BOOL)flag;

/// @brief 가장 상단에 present된 view가 있을 경우 그 위에, 아닐 경우 root view controller로 세팅하여 넘겨진 view controller를 호출합니다.
+ (UIViewController *)PIDPresentOnAppsTopmostViewcontroller:(UIViewController *)viewController animated:(BOOL)flag;

/// @brief 파라미터로 넘겨져온 view controller를 root view controller로 세팅합니다.
+ (UIViewController *)PIDPresentAsRootViewController:(UIViewController *)viewController;

/// @brief 파라미터로 넘겨져 온 view를 정렬여부에 따라서 위치를 조정합니다.
- (void)PIDAuthAdditionsAlignViewByCenter:(UIView *)view horizontal:(BOOL)horizontal vertical:(BOOL)vertical;

/// @brief 해당 메서드를 호출하는 view controller만 dismiss 시킵니다.
- (void)PIDDismiss:(BOOL)animated;

/// @brief 호출하는 view controller를 포함하여 모든 child view controller들을 dismiss 시킵니다.
- (void)PIDDismissAllFromThisViewController;

/// @brief 호출하는 view controller를 제외한 모든 child view controller를 dismiss 시킵니다.
- (void)PIDDismissAllChildViewController:(BOOL)animated;

/// @brief 모든 child view controller들을 닫고 새로운 view controller를 띄웁니다.
- (void)PIDSwitchViewController:(UIViewController *)toViewController animated:(BOOL)animated;

/// @brief 현재 view controller가 최상단에 위치한 view controller인지 확인합니다.
- (BOOL)PIDIsTopMostViewController;

/// @brief 현재 최상단에 위치한 view controller를 호출합니다.
+ (UIViewController *)PIDTopmostViewcontroller;

/// @brief 현재 root view controller를 호출합니다.
+ (UIViewController *)PIDRootViewController;

/// @brief view controller를 transparent 속성으로 띄워줍니다.
- (void)presentTransparentModalViewController:(id)viewController animated:(BOOL)animated completion:(void (^)(void))completion;

@end
