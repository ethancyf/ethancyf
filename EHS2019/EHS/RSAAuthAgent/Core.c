
#ifdef WIN32
#include <windows.h>
#define sleep(x)    Sleep(x*1000)
#define ENABLE_ECHO_INPUT 0x0004
#define ENABLE_LINE_INPUT 0x0002
#endif

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#ifndef WIN32
#include <termio.h>
#include <fcntl.h>
#endif

// in NT this allows using the aceclnt.lib file for linking
#define USE_ACE_AGENT_API_PROTOTYPES
#include "acexport.h"

#ifdef BLD_OFFLINEAUTH
#include "da_svc_api.h"
#endif

#ifdef WIN32
    HANDLE hStdIn;
    DWORD OrigConsoleMode, ConsoleMode;
#endif

int auth(char* strConfig, char* strUserID, char* strPasscode, char** strStackTrace)
{
    int         acmRet, cTryCount = 0;
    SD_BOOL     bAuthenticated = SD_FALSE;
    SDI_HANDLE  SdiHandle = SDI_HANDLE_NONE;
    
#ifdef BLD_OFFLINEAUTH
    acmRet = SD_InitEx(&SdiHandle,1,RSA_DA_AGENT_LOCAL);
#else
	if (AceInitializeEx(strConfig, NULL, 0) == SD_FALSE) {
		sprintf(*strStackTrace, "AceInitializeEx failed. Config Path: %s", strConfig);		
		return 9;
	}

	acmRet = SD_Init(&SdiHandle);
#endif

    if (acmRet != ACM_OK) {
		strcpy(*strStackTrace, "SD_Init failed");
        return 9;
    }

	acmRet = SD_Lock(SdiHandle, strUserID);

	if (acmRet != ACM_OK) {
		strcpy(*strStackTrace, "SD_Lock failed");
        return 9;
    }

	acmRet = SD_Check(SdiHandle, strPasscode, strUserID);
    
    SD_Close(SdiHandle);

#ifdef WIN32
	CloseHandle(hStdIn);
#endif

    return acmRet;
}

void clearRSA() {
	AceDisableNodeSecretCache();
	AceShutdown(NULL);	
}