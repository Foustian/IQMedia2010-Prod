#include "stdafx.h"
#include <dbt.h>
#include <mmreg.h>
#include <msacm.h>
#include <fcntl.h>
#include <io.h>
#include <stdio.h>
#include <commdlg.h>
#include <strsafe.h>
#include "stdafx.h"
//#include "amcap.h"
#include "dshowutil.h"
#include <direct.h>
#include <time.h>
#include "SampleCGB.h"
#include <initguid.h>
#include "ICCLineInterp.h"
#include "line.h"
#include "OspreyCaptureDevice.h"
#include "x264vfwConfig.h"


#define RETURN_FAIL(Y) \
{\
	HRESULT hr; \
	if(!SUCCEEDED(hr=(Y))) \
{\
	return hr;\
}\
}

#define	WM_GRAPH_NOTIFY		(WM_APP + 1)

class Capture 
{
// Construction
public:
  Capture(int deviceno,wchar_t* logdirlocation);
  virtual ~Capture();
  

protected:
// Implementation
public:
	
	void GetCaptureDevices();
	BOOL MakeBuilder();
	BOOL MakeGraph();
	void RemoveDownstream(IBaseFilter *pf);
	void ChooseDevices(IMoniker *pmVideo, IMoniker *pmAudio);
	void TearDownGraph(void);
	BOOL BuildMp4CaptureGraph();
	void LogMsg(char* szBuffer);
	BOOL InitCapFilters();
	void FreeCapFilters();
	BOOL StopCapture();
	BOOL StartCapture();
	void InitializeCC();
	void SetFormatSize();
	void x264vfw_config_reg_save(CONFIG *config);
	BOOL GetCaptureFiltersAndSource();
	BOOL MakeGraphInstances();
	//BOOL BuildPreviewGraph();
	
	
public:
	WCHAR wszCaptureFile[9000];
	char* wszCCFile;
    ISampleCaptureGraphBuilder *pBuilder;
	CLine  *m_aLine;
    
    IAMStreamConfig *pVSC;      // for video cap
	
    IBaseFilter *pRender;
    IBaseFilter *pVCap, *pACap;
    IGraphBuilder *pFg;
    BOOL fCaptureGraphBuilt;
    BOOL fCapturing;
    IBaseFilter *pVcompFilter;
	IMoniker *rgpmVideoMenu[10];
	IMoniker *rgpmAudioMenu[10];
    BOOL fCapAudio;
    WCHAR wachFriendlyName[120];
	WCHAR wachAudioFriendlyName[256];
    FILE* c_pF;
    
    int iNumVCapDevices;
	int iNumACapDevices;
	IBaseFilter *pAVI;
	OspreyCaptureDevice    *pOCD;
	CONFIG reg_curr;
	IMediaFilter *pMediaFilter;
	IBaseFilter *g_mp4mux;
	IBaseFilter *g_ACMWrapper;
	IBaseFilter *g_MonoAACEndcoder;
	
	IReferenceClock *pClock;
	REFERENCE_TIME lCapStartTime;
	
	// variables for preview
	//BOOL fPreviewFaked;
	//BOOL fPreviewGraphBuilt;
	//IAMStreamConfig *pVPSC;      // for video cap
	//IVideoWindow *pVW;
	//IMediaEventEx *pME;

	// for config settings 
	int devnum;
	int adevnum;
	wchar_t *templocation;
	wchar_t *destination;
	wchar_t *stationid;
	int periodicity;
	char *currentoutput;
	wchar_t *wcurrentoutput;

	int audiobitrate;
	double FrameRate;
	int Videowidth;
	int Videoheight;	
};