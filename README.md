# Gamebase Sample App for Unity

Gamebase SDK를 쉽게 적용할 수 있도록 SDK 개발자가 추천하는 흐름을 Sample App으로 직접 구현했습니다.

## Sample App Configuration

Sample App을 에디터에서 실행할 때는 Intro 씬을 열어 플레이를 진행합니다.

* Assets/Sample/Scene/Intro.scene

### Scene Configuration

#### Intro

#### Login
* Login
* Register Push
* Smart Downloader
    * StartDownload 

#### Main
* Store Popup
    * Purchase
* Settings Popup
    * Logout
    * Withdraw
    * PushSetting
    * Add Mapping
    * Add Mapping Forcibly
    * Remove Mapping
    * TransferAccount
* GameInfo Popup
* Leaderboard Popup
    * Get single user info
    * Get multiple user info by range
* Webview
* Smart Downloader
    * StartDownload

#### Ingame

* TAA
    * LevelUp
* Leaderboard
    * Set single user score

### Build Settings

Gamebase Setting Tool 2.0.0이 적용되었습니다.

* [Gamebase Guide - Using the Setting Tool](https://docs.toast.com/ko/Game/Gamebase/ko/unity-started/#using-the-setting-tool)

#### Android

1. Unity 버전 별로 gradle 설정이 달라지기 때문에 직접 생성해야 합니다.
    * Unity 2019.3 미만
        * Player Settings > Publishing Settings에서 Custom Gradle Template을 활성화하여 mainTemplate.gradle 파일을 생성
    * Unity 2019.3 이상
        * Player Settings > Publishing Settings에서 Custom Gradle Template을 활성화하여 mainTemplate.gradle 파일을 생성        
        * Player Settings > Publishing Settings에서 Custom Gradle Properties Template을 활성화하여 gradleTemplate.properties 파일을 생성
2. gradle이 생성되었다면 EDM4U의 Resolve를 진행합니다.
    * 상단 메뉴 > Assets > External Dependency Manager > Android Resolver > Force Resolve를 선택

## TOAST Console Settings

Sample App의 모든 기능을 사용하기 위해서 TOAST에 신규 프로젝트를 등록하고 설정해야 합니다.

### App

앱, 클라이언트 상태를 설정합니다.

* [Gamebase Guide - App](http://docs.toast.com/ko/Game/Gamebase/ko/oper-app/)

### IdP Settings

인증 정보를 추가합니다.

* [Gamebase Guide - Authentication Information](http://docs.toast.com/ko/Game/Gamebase/ko/oper-app/#authentication-information)

### Purchase(IAP) Settings

Purchase(IAP)를 설정합니다.

* [Gamebase Guide - Purchase(IAP)](http://docs.toast.com/ko/Game/Gamebase/ko/oper-purchase/)

### Push Settings

Push를 설정합니다.

* [Gamebase Guide - Push](http://docs.toast.com/ko/Game/Gamebase/ko/oper-push/)

### Operaction

앱 운영 시 사용합니다. 공지, 점검 등을 등록할 수 있습니다.

* [Gamebase Guide - Operation](http://docs.toast.com/ko/Game/Gamebase/ko/oper-operation/)


## Unity Settings

### GamebaseUnitySDKSettings

GamebaseUnitySDKSettings 컴포넌트의 Inspector를 설정합니다.

* [Gamebase Guide - Unity Inspector Settings](http://docs.toast.com/ko/Game/Gamebase/ko/unity-initialization/#inspector-settings)

### AndroidManifest
AndroidManifest.xml 파일에서 아래 목록에 있는 값들을 설정합니다.

* APP_PACKAGE_NAME

### FCM
google-services-json.xml 파일에서 아래 목록에 있는 값들을 설정합니다.

* DEFAULT_WEB_CLIENT_ID
* GCM_DEFAULTSENDERID
* FIREBASE_DATABASE_URL
* GOOGLE_APP_ID
* GOOGLE_API_KEY
* GOOGLE_STORAGE_BUCKET

설정방법은 아래 링크를 참조해 주세요.

* [Gamebase Guide - Google Services Settings (Firebase only)](http://docs.toast.com/ko/Game/Gamebase/ko/aos-push/#google-services-settings-firebase-only)
