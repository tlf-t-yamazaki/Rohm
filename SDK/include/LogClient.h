// LogClient.h
//

#include <tchar.h>

#ifdef LOGCLIENT_EXPORTS
#define LOGCLIENT_API __declspec(dllexport) __stdcall
#else
#define LOGCLIENT_API __declspec(dllimport) __stdcall
#endif

#ifdef __cplusplus
extern "C" {
#endif


// LogType
#define NUM_LOGTYPES	100

#define LOGTYPE_TRACE	0	// function/method trace
#define LOGTYPE_LOG		1	// general log

// LogLevel
typedef enum _LOGLEVEL {
	LOGLEVEL_NONE = 0,
	LOGLEVEL_ERROR,
	LOGLEVEL_WARNING,
	LOGLEVEL_INFORMATION,
	LOGLEVEL_TRACE,
	LOGLEVEL_DETAILS,
	
	LOGLEVEL_DEBUG = 10,
} LOGLEVEL;


void LOGCLIENT_API Log_Initialize(LPCTSTR sComponent);

void LOGCLIENT_API Log_Uninitialize();

// header formatter
typedef LONG (*LOG_HEADER_FORMATTER)(LPTSTR sBuffer, LONG lBufferLength,
					  LPCTSTR sComputerName, LPCTSTR sComponent,
					  LONG lLogType, LONG lLogLevel,
					  LONG lNesting, LPCTSTR sFunction);

void LOGCLIENT_API Log_SetHeaderFormatter(LOG_HEADER_FORMATTER headerFormatter);

void LOGCLIENT_API Log_UnsetHeaderFormatter(void);

// logging functions

// log simple string message
void LOGCLIENT_API Log_String(LONG lLogType, LONG lLogLevel, LPCTSTR sMessage);

// log simple string message with file contents
void LOGCLIENT_API Log_File(LONG lLogType, LONG lLogLevel, LPCTSTR sMessage,
									   LPCTSTR sFileNameTemplate,
									   PVOID pFileContents,
									   DWORD dwFileContentsLength);

// log message with format
void LOGCLIENT_API Log_Message(LONG lLogType, LONG lLogLevel, LPCTSTR sFormat, ...);

void LOGCLIENT_API Log_MessageV(LONG lLogType, LONG lLogLevel, LPCTSTR sFormat, va_list args);

// log message with format with file contents
void LOGCLIENT_API Log_MessageWithFile(LONG lLogType, LONG lLogLevel,
									   LPCTSTR sFileNameTemplate,
									   PVOID pFileContents,
									   DWORD dwFileContentsLength,
									   LPCTSTR sFormat, ...);

// log error, warning, and information
void LOGCLIENT_API Log_Error(LONG lLogType, LPCTSTR sFormat, ...);
void LOGCLIENT_API Log_Warning(LONG lLogType, LPCTSTR sFormat, ...);
void LOGCLIENT_API Log_Information(LONG lLogType, LPCTSTR sFormat, ...);


// trace
void LOGCLIENT_API Log_TraceEntry(LPCTSTR sFunctionName);
void LOGCLIENT_API Log_TraceExit(void);
void LOGCLIENT_API Log_Trace(LPCTSTR sFormat, ...);


// trace utility class
// Usage sample:
//     void Function(...)
//     {
//         CTrace _trace_(_T("Function"));
//         .....
//     }
class CTrace
{
public:
	CTrace(LPCTSTR sFunctionName)
	{
		Log_TraceEntry(sFunctionName);
	}
	~CTrace(void)
	{
		Log_TraceExit();
	}
};


#ifdef __cplusplus
}
#endif
