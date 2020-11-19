#pragma once

#define SMART_DL_API
#import <Foundation/Foundation.h>

extern "C" {
	SMART_DL_API char* GetCountryCode();
	SMART_DL_API char* GetOsVersion();
	SMART_DL_API char* GetLanguage();
	SMART_DL_API char* GetLocale();
	SMART_DL_API char* GetMobileCountry();
	SMART_DL_API char* GetMobileCarrier();

    SMART_DL_API char* GetAvailableFreeSpace(char* path, unsigned long& availableFreeSpace);
}
