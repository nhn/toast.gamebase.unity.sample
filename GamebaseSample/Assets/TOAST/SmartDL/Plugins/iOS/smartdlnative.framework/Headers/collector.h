#pragma once

#define SMART_DL_API
#import <Foundation/Foundation.h>

extern "C" {
	enum SystemInfo
	{
		CountryCode = 0,
		OsVersion,
		Language,
		Locale,
		Ip
	};

	SMART_DL_API char* GetCountryCode();
	SMART_DL_API char* GetOsVersion();
	SMART_DL_API char* GetLanguage();
	SMART_DL_API char* GetLocale();
	SMART_DL_API char* GetMobileCountry();
	SMART_DL_API char* GetMobileCarrier();

	SMART_DL_API unsigned long GetAvailableFreeSpace(char* path);
	SMART_DL_API bool CanDownloadToDisk(char* path, int64_t totalWillDownloadBytes);
}
