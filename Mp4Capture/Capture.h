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

class Capture 
{
// Construction
public:
  Capture(int deviceno);

// Implementation
public:
	virtual ~Capture();
	void AddDevicesToMenu();
	BOOL MakeBuilder();
	BOOL MakeGraph();
	void RemoveDownstream(IBaseFilter *pf);
	void ChooseDevices(IMoniker *pmVideo, IMoniker *pmAudio);
	void TearDownGraph(void);
	BOOL BuildMp4CaptureGrapth();
	void LogMsg(char* szBuffer);
	BOOL InitCapFilters();
	void FreeCapFilters();
	BOOL StopCapture();
	BOOL StartCapture();
	void UpdateStatus(BOOL fAllStats);
	BOOL setCaptureInfo();
	void InitializeCC();
	void SetFormatSize();
	void x264vfw_config_reg_save(CONFIG *config);
	BOOL GetCaptureFiltersAndSource();
	BOOL MakeGraphInstances();
	
public:
	WCHAR wszCaptureFile[9000];
	char* wszCCFile;
    ISampleCaptureGraphBuilder *pBuilder;
	CLine  *m_aLine;
    IVideoWindow *pVW;
    IMediaEventEx *pME;
    IAMDroppedFrames *pDF;
    IAMVideoCompression *pVC;
    IAMVfwCaptureDialogs *pDlg;
    IAMStreamConfig *pASC;      // for audio cap
    IAMStreamConfig *pVSC;      // for video cap
    IBaseFilter *pRender;
    IBaseFilter *pVCap, *pACap;
    IGraphBuilder *pFg;
    IFileSinkFilter *pSink;
    BOOL fCaptureGraphBuilt;
    BOOL fPreviewGraphBuilt;
    BOOL fCapturing;
    BOOL fPreviewing;
    IBaseFilter *pVcompFilter;
	IMoniker *rgpmVideoMenu[10];
	IMoniker *rgpmAudioMenu[10];
    BOOL fCapAudio;
    BOOL fCapCC;
    BOOL fCCAvail;
    BOOL fCapAudioIsRelevant;
    IMoniker *pmVideo;
    IMoniker *pmAudio;
    BOOL fWantPreview;
    long lCapStopTime;
    WCHAR wachFriendlyName[120];
	WCHAR wachAudioFriendlyName[256];
    BOOL fUseTimeLimit;
    BOOL fUseFrameRate;
    DWORD dwTimeLimit;
    long lDroppedBase;
    long lNotBase;
    BOOL fPreviewFaked;
    LONG NumberOfVideoInputs;
    int iNumVCapDevices;
	int iNumACapDevices;
	IBaseFilter *pAVI;
	OspreyCaptureDevice    *pOCD;
	CONFIG reg_curr;
	IMediaFilter *pMediaFilter;
	IBaseFilter *g_mp4mux;
	IBaseFilter *g_ACMWrapper;
	IBaseFilter *g_MonoAACEndcoder;


	int devnum;
	int adevnum;
	wchar_t *templocation;
	wchar_t *destination;
	wchar_t *stationid;
	int overlap;
	int periodicity;
	wchar_t* schedule;
	char *currentoutput;
	wchar_t *wcurrentoutput;
	int sleeptime;
	int zerolatency;
	int enconding_type;
	int birate;
	int audiobitrate;
	double FrameRate;
	int widthratio;
	int heightratio;
	int Videowidth;
	int Videoheight;

	FILE* c_pF;
	



};