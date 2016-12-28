#include "stdafx.h"
#include "Capture.h"
#include "status.h"
#include <comdef.h>
#include <dvdmedia.h>
#include "MonoGramAACConfig.h"
DWORD g_dwGraphRegister=0;
/* Global DLL critical section */
CRITICAL_SECTION x264vfw_CS;
IReferenceClock *pClock;
REFERENCE_TIME lCapStartTime;

/////////////////////////////////////////////////////////////////////////////
//
// Capture
//
Capture::Capture(int deviceno)
{
	  // do we want audio?
	char *LogFile = new char[500];
	m_aLine =NULL;
	SYSTEMTIME st;
	GetSystemTime(&st);
	sprintf(LogFile,"C:\\Logs\\LogDevice%d_%d%.2d%.2d.txt",deviceno,st.wYear,st.wMonth,st.wDay);
	c_pF = fopen(LogFile, "a+");

	LogMsg("Capture Application Initialized");

    // do we want preview?
	fCapAudio = TRUE;
    
    // get the frame rate from win.ini before making the graph
    
	memset(&this->pBuilder,0,sizeof(pBuilder));
	fCapturing = FALSE;
	fCaptureGraphBuilt = FALSE;

	Videowidth = 400;
	Videoheight =300;
	int ul_reason_for_call=1;
	#ifdef PTW32_STATIC_LIB
    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
            InitializeCriticalSection(&x264vfw_CS);
            pthread_win32_process_attach_np();
            pthread_win32_thread_attach_np();
            break;

        case DLL_THREAD_ATTACH:
            pthread_win32_thread_attach_np();
            break;

        case DLL_THREAD_DETACH:
            pthread_win32_thread_detach_np();
            break;

        case DLL_PROCESS_DETACH:
            pthread_win32_thread_detach_np();
            pthread_win32_process_detach_np();
            DeleteCriticalSection(&x264vfw_CS);
            break;
    }
#else
    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
            InitializeCriticalSection(&x264vfw_CS);
            break;

        case DLL_PROCESS_DETACH:
            DeleteCriticalSection(&x264vfw_CS);
            break;
    }
#endif
}

Capture::~Capture()
{
	TearDownGraph();
	FreeCapFilters();
	int i;
  for(i=0;i<iNumVCapDevices;i++)
  {
    if (rgpmVideoMenu[i])
      rgpmVideoMenu[i]->Release();
    rgpmVideoMenu[i] = NULL;
  }
  for(i=0;i<iNumACapDevices;i++)
  {
    if (rgpmAudioMenu[i])
      rgpmAudioMenu[i]->Release();
    rgpmAudioMenu[i] = NULL;
  }
  LogMsg("Capture Application Stopped");
  c_pF = NULL;
}


const CLSID CLSID_GDCLMp4Mux = { 0x5FD85181, 0xE542, 0x4E52, { 0x8D, 0x9D, 0x5D, 0x61, 0x3C, 0x30, 0x13, 0x1B }};
const CLSID CLSID_CCLineInterp = {0xae6a37f1, 0x62ef, 0x4140,{0x82, 0x92, 0xc0, 0x71, 0x85, 0x67, 0x96, 0xb5}};
#define LINELINES   24
#define TIMEDIM     15  // "00:00:00:000:  " = 15
#define LINECHARS   32
#define LINEDIM     (TIMEDIM + LINECHARS + 2)
#define IDC_CC1                         1002
   


//------------------------------------------------------------------------------
// Macros
//------------------------------------------------------------------------------
#define ABS(x) (((x) > 0) ? (x) : -(x))

// An application can advertise the existence of its filter graph
// by registering the graph with a global Running Object Table (ROT).
// The GraphEdit application can detect and remotely view the running
// filter graph, allowing you to 'spy' on the graph with GraphEdit.
//
// To enable registration in this sample, define REGISTER_FILTERGRAPH.
//
#ifdef  DEBUG
#define REGISTER_FILTERGRAPH
#endif

void IMonRelease(IMoniker *&pm)
{
    if(pm)
    {
        pm->Release();
        pm = 0;
    }
}

// Make a graph builder object we can use for capture graph building
//
BOOL Capture::MakeBuilder()
{
    // we have one already
    
	//LogMsg("Start of Create GraphBuilder");

	if(pBuilder)
        return TRUE;

    pBuilder = new ISampleCaptureGraphBuilder();
    if( NULL == pBuilder )
    {
        return FALSE;
		LogMsg("Failed to Create GraphBulder ");
    }

    return TRUE;
}

// Make a graph object we can use for capture graph building
//
BOOL Capture::MakeGraph()
{
    
	//LogMsg("Start of Create FilterGraph Object");

    HRESULT hr = CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC,
                                  IID_IGraphBuilder, (LPVOID *)&pFg);

    if(hr == NOERROR) 
	{
		return TRUE;
	}
	else
	{
		LogMsg("Failed to Create FilterGraph Object");
		return FALSE;
	}

	
}

// put all installed video and audio devices in the menus
//
void Capture::AddDevicesToMenu()
{
    
	LogMsg("Get Devices For Capture Application");

    iNumVCapDevices = 0;
    UINT    uIndex = 0;

    HRESULT hr;
    BOOL bCheck = FALSE;

    // enumerate all video capture devices
    ICreateDevEnum *pCreateDevEnum=0;
    hr = CoCreateInstance(CLSID_SystemDeviceEnum, NULL, CLSCTX_INPROC_SERVER,
                          IID_ICreateDevEnum, (void**)&pCreateDevEnum);
    if(hr != NOERROR)
    {
        LogMsg("Error Creating Device Enumerator");
        return;
    }

    IEnumMoniker *pEm=0;
    hr = pCreateDevEnum->CreateClassEnumerator(CLSID_VideoInputDeviceCategory, &pEm, 0);
	
    if(hr != NOERROR)
    {
        LogMsg("Sorry, you have no video capture hardware.\r\n\r\nVideo capture will not function properly.");
        goto EnumAudio;
    }

    pEm->Reset();
    ULONG cFetched;
    IMoniker *pM;

    while(hr = pEm->Next(1, &pM, &cFetched), hr==S_OK)
    {
        IPropertyBag *pBag=0;

        hr = pM->BindToStorage(0, 0, IID_IPropertyBag, (void **)&pBag);
        if(SUCCEEDED(hr))
        {
            VARIANT var;
            var.vt = VT_BSTR;
            hr = pBag->Read(L"FriendlyName", &var, NULL);
            if(hr == NOERROR)
            {

                rgpmVideoMenu[uIndex] = pM;
                pM->AddRef();
            }
            pBag->Release();
        }

        pM->Release();
        uIndex++;
    }
    pEm->Release();



	/* added by meghana for init compression filter */

	
	IEnumMoniker *pEnum = NULL;
	IMoniker *pMoniker = NULL;

	hr = pCreateDevEnum->CreateClassEnumerator(
				CLSID_VideoCompressorCategory, &pEnum, 0);
	if (hr == S_OK)  // S_FALSE means nothing in this category.
	{
		while (S_OK == pEnum->Next(1, &pMoniker, NULL))
		{
			IPropertyBag *pPropBag = NULL;
			pMoniker->BindToStorage(0, 0, IID_IPropertyBag, 
				(void **)&pPropBag);
			VARIANT var;
			VariantInit(&var);
			hr = pPropBag->Read(L"FriendlyName", &var, 0);
			if (SUCCEEDED(hr))
			{
				char nameBuf[256];

				BOOL bUsedDef = TRUE;
				WideCharToMultiByte(
				  CP_ACP,
				  WC_COMPOSITECHECK | WC_DEFAULTCHAR,
				  var.bstrVal,
				  -1,
				  nameBuf,
				  255,
				  "x",
				  &bUsedDef
				);
				// add to the list of devices if it has an AVStream driver
				// devices with VFW drivers are NOT added
				// Note that I am only going to add the capture device if it matches the slected dvice number

				if (strstr(nameBuf,"x264vfw - H.264/MPEG-4 AVC codec") != NULL)
				{
					hr = pMoniker->BindToObject(NULL, NULL, IID_IBaseFilter, 
									(void**)&pVcompFilter);
					if (FAILED(hr))
					{
						LogMsg("Cannot init video compressor filter");
					}
				}				
			}   
			VariantClear(&var); 
			pPropBag->Release();
			pMoniker->Release();
		}
	}
	pEnum->Release();
    iNumVCapDevices = uIndex;
EnumAudio:

    // enumerate all audio capture devices
    uIndex = 0;
    bCheck = FALSE;

    ASSERT(pCreateDevEnum != NULL);

    //hr = pCreateDevEnum->CreateClassEnumerator(CLSID_AudioInputDeviceCategory, &pEm, 0);
	hr = pCreateDevEnum->CreateClassEnumerator(AM_KSCATEGORY_CAPTURE, &pEm, 0);
    pCreateDevEnum->Release();
    if(hr != NOERROR)
	{
		LogMsg("Sorry, you have no audio capture hardware");
		return;
	}
    pEm->Reset();

    while(hr = pEm->Next(1, &pM, &cFetched), hr==S_OK)
    {
        IPropertyBag *pBag;
        hr = pM->BindToStorage(0, 0, IID_IPropertyBag, (void **)&pBag);
        if(SUCCEEDED(hr))
        {
            VARIANT var;
            var.vt = VT_BSTR;
            hr = pBag->Read(L"FriendlyName", &var, NULL);

        
        // add to the list of devices if it has an AVStream driver
        // devices with VFW drivers are NOT added


            if(hr == NOERROR)
            {
				char nameBuf[256];

				BOOL bUsedDef = TRUE;
				WideCharToMultiByte(
				  CP_ACP,
				  WC_COMPOSITECHECK | WC_DEFAULTCHAR,
				  var.bstrVal,
				  -1,
				  nameBuf,
				  255,
				  "x",
				  &bUsedDef
				);
				// add to the list of devices if it has an AVStream driver
				// devices with VFW drivers are NOT added
				// Note that I am only going to add the capture device if it matches the slected dvice number


				if (!strncmp(nameBuf,"Osprey-440 Audio",16) && strstr(nameBuf,"Unbalanced") != NULL)
				{
					rgpmAudioMenu[uIndex] = pM;
					pM->AddRef();

					uIndex++;
				}
				SysFreeString(var.bstrVal);


				

            }
            pBag->Release();
        }
        pM->Release();
    }
    pEm->Release();

}

// Tear down everything downstream of a given filter
void Capture::RemoveDownstream(IBaseFilter *pf)
{
    IPin *pP=0, *pTo=0;
    ULONG u;
    IEnumPins *pins = NULL;
    PIN_INFO pininfo;

    if (!pf)
        return;

    HRESULT hr = pf->EnumPins(&pins);
    pins->Reset();

    while(hr == NOERROR)
    {
        hr = pins->Next(1, &pP, &u);
        if(hr == S_OK && pP)
        {
            pP->ConnectedTo(&pTo);
            if(pTo)
            {
                hr = pTo->QueryPinInfo(&pininfo);
                if(hr == NOERROR)
                {
                    if(pininfo.dir == PINDIR_INPUT)
                    {
                        RemoveDownstream(pininfo.pFilter);
                        pFg->Disconnect(pTo);
                        pFg->Disconnect(pP);
                        pFg->RemoveFilter(pininfo.pFilter);
                    }
                    pininfo.pFilter->Release();
                }
                pTo->Release();
            }
            pP->Release();
        }
    }

    if(pins)
        pins->Release();
}

// Tear down everything downstream of the capture filters, so we can build
// a different capture graph.  Notice that we never destroy the capture filters
// and WDM filters upstream of them, because then all the capture settings
// we've set would be lost.
//
void Capture::TearDownGraph()
{

	LogMsg("Start Of TearDownGraph");
    
//    SAFE_RELEASE(pRender);
    SAFE_RELEASE(pME);
//    SAFE_RELEASE(pDF);

    if(pVW)
    {
        // stop drawing in our window, or we may get wierd repaint effects
//        pVW->put_Owner(NULL);
 //       pVW->put_Visible(OAFALSE);
 //       pVW->Release();
        pVW = NULL;
    }

    // destroy the graph downstream of our capture filters
    if(pVCap)
        RemoveDownstream(pVCap);
    if(pACap)
        RemoveDownstream(pACap);
    if(pVCap)
        pBuilder->ReleaseFilters();

    // potential debug output - what the graph looks like
    // if (pFg) DumpGraph(pFg, 1);

#ifdef REGISTER_FILTERGRAPH
    // Remove filter graph from the running object table
    if(g_dwGraphRegister)
    {
        RemoveGraphFromRot(g_dwGraphRegister);
        g_dwGraphRegister = 0;
    }
#endif

    fCaptureGraphBuilt = FALSE;
    
    fPreviewFaked = FALSE;
}

// create the capture filters of the graph.  We need to keep them loaded from
// the beginning, so we can set parameters on them and have them remembered
//
BOOL Capture::InitCapFilters()
{
	LogMsg("Start Of InitCapFilters");

    HRESULT hr=S_OK;
    BOOL f;

    fCCAvail = FALSE;  // assume no closed captioning support

    f = MakeBuilder();
    if(!f)
    {
        LogMsg("Cannot instantiate graph builder");
        return FALSE;
    }
    //
    // make a filtergraph, give it to the graph builder and put the video
    // capture filter in the graph
    //

    f = MakeGraph();
    if(!f)
    {
        LogMsg("Cannot instantiate filtergraph");
        return FALSE;
    }

    hr = pBuilder->SetFiltergraph(pFg);
    if(hr != NOERROR)
    {
        LogMsg("Cannot give graph to builder");
        return FALSE;
    }

	MakeGraphInstances();

    return TRUE;
}

// method to set the height and width of the video
void Capture::SetFormatSize()
{

	
  AM_MEDIA_TYPE *pmt;
  // default capture format
  if (pVSC && pVSC->GetFormat(&pmt) == S_OK)
  {
    // set the height and width
    BITMAPINFOHEADER* pBMIH = NULL;

    if (pmt->formattype == FORMAT_VideoInfo) 
    {
      VIDEOINFOHEADER* pVIH = (VIDEOINFOHEADER*)(pmt->pbFormat);
      pVIH->AvgTimePerFrame = (LONGLONG)(10000000 / FrameRate);
      pBMIH = &pVIH->bmiHeader;
    }

    else
    if (pmt->formattype == FORMAT_VideoInfo2) 
    {
      VIDEOINFOHEADER2* pVIH2 = (VIDEOINFOHEADER2*)(pmt->pbFormat);
      pVIH2->AvgTimePerFrame  = (LONGLONG)(10000000 / FrameRate);
      pBMIH = &pVIH2->bmiHeader;
    }

	if(pBMIH && Videowidth >0 && Videoheight>0)
	{
      pBMIH->biWidth = Videowidth;
      if (ABS(pBMIH->biHeight) < 0)
		pBMIH->biHeight = -Videoheight;
      else
		pBMIH->biHeight =  Videoheight;
    
	}
	pVSC->SetFormat(pmt);
    DeleteMediaType(pmt);
  }
}


// epoch variance
void TimetToFileTime( time_t t, LPFILETIME pft )
{
    LONGLONG ll = Int32x32To64(t, 10000000) + 116444736000000000;
    pft->dwLowDateTime = (DWORD) ll;
    pft->dwHighDateTime = (DWORD)(ll >>32);
}

// all done with the capture filters and the graph builder
//
void Capture::FreeCapFilters()
{

	LogMsg("Start Of FreeCapFilters");

	pFg->RemoveFilter(pVCap);
	pFg->RemoveFilter(pVcompFilter);
	pFg->RemoveFilter(g_mp4mux);

	pFg->RemoveFilter(pACap);
	pFg->RemoveFilter(g_ACMWrapper);
	pFg->RemoveFilter(g_MonoAACEndcoder);


	pFg->RemoveFilter(pAVI);
	
	
	

    /*SAFE_RELEASE(pFg);
    if( pBuilder )
    {
        delete pBuilder;
        pBuilder = NULL;
    }
    SAFE_RELEASE(pVCap);
    SAFE_RELEASE(pACap);
	SAFE_RELEASE(pAVI);
    SAFE_RELEASE(pASC);
    SAFE_RELEASE(pVSC);
    SAFE_RELEASE(pVC);
    SAFE_RELEASE(pDlg);

	if(pMediaFilter)
	{
		pMediaFilter->Release();
		pMediaFilter=NULL;
	}

	if(pClock)
	{
		pClock->Release();
		pClock=NULL;
	}*/
}

BOOL Capture::GetCaptureFiltersAndSource()
{
	// Add the video capture filter to the graph with its friendly name
    HRESULT hr = pFg->AddFilter(pVCap, wachFriendlyName);
    if(hr != NOERROR)
    {
        LogMsg("Error %x: Cannot add vidcap to filtergraph");
		return FALSE;
        //goto InitCapFiltersFail;
    }

    // Calling FindInterface below will result in building the upstream
    // section of the capture graph (any WDM TVTuners or Crossbars we might
    // need).

    // we use this interface to get the name of the driver
    // Don't worry if it doesn't work:  This interface may not be available
    // until the pin is connected, or it may not be available at all.
    // (eg: interface may not be available for some DV capture)
    hr = pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE,
                                      &MEDIATYPE_Interleaved, pVCap,
                                      IID_IAMVideoCompression, (void **)&pVC);
    if(hr != S_OK)
    {
        hr = pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE,
                                          &MEDIATYPE_Video, pVCap,
                                          IID_IAMVideoCompression, (void **)&pVC);
    }

    // !!! What if this interface isn't supported?
    // we use this interface to set the frame rate and get the capture size
    hr = pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE,
                                      &MEDIATYPE_Interleaved,
                                      pVCap, IID_IAMStreamConfig, (void **)&pVSC);

    if(hr != NOERROR)
    {
        hr = pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE,
                                          &MEDIATYPE_Video, pVCap,
                                          IID_IAMStreamConfig, (void **)&pVSC);


        if(hr != NOERROR)
        {
            // this means we can't set frame rate (non-DV only)
            LogMsg("Error %x: Cannot find VCapture:IAMStreamConfig");
        }
    }


	// We'll need this in the graph to get audio property pages
        hr = pFg->AddFilter(pACap, wachAudioFriendlyName);
        if(hr != NOERROR)
        {
            LogMsg("Error Cannot add audio capture filter to filtergraph");
			return FALSE;
            //goto InitCapFiltersFail;
        }
    // Calling FindInterface below will result in building the upstream
    // section of the capture graph (any WDM TVAudio's or Crossbars we might
    // need).


    // !!! What if this interface isn't supported?
    // we use this interface to set the captured wave format
    hr = pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE, &MEDIATYPE_Audio, pACap,
                                      IID_IAMStreamConfig, (void **)&pASC);

	long lStandard;
	if (pOCD)
	  {
		pOCD->getStandard(&lStandard);
		SetFormatSize();
		if(Videoheight > 0 && Videowidth > 0)
		{
			PIN_ID m_Pin0;
			m_Pin0.ucInstance = 0;
			m_Pin0.ucType     = PIN_TYPE_PAIRED;
			hr = pOCD->setDefaultSize(m_Pin0,Videoheight,Videowidth);
			//pOCD->setSquareAspect(0);
		}
	  }

    if(hr != NOERROR)
    {
        LogMsg("Cannot find ACapture:IAMStreamConfig");
    }
	return TRUE;
}

BOOL Capture::MakeGraphInstances()
{
	// Create instance of mp4mux

	IMoniker *pmVideo1 = 0, *pmAudio1 = 0;
	pmVideo1 = rgpmVideoMenu[devnum];
	pmAudio1 = rgpmAudioMenu[adevnum];
	ChooseDevices(pmVideo1,pmAudio1);

	HRESULT hr;

	g_mp4mux=NULL;
	hr = CoCreateInstance(CLSID_GDCLMp4Mux, NULL, CLSCTX_INPROC_SERVER,
							IID_IBaseFilter, ( void **) &g_mp4mux);

	if (FAILED(hr)){
		LogMsg("Error : Cannot create MJpeg Decommpression Filter");
		return FALSE;
	}

	IBaseFilter *dsWriter;
	hr =(CoCreateInstance(CLSID_FileWriter, NULL, CLSCTX_INPROC, IID_IBaseFilter, (void **)&dsWriter));
	if (FAILED(hr)){
		LogMsg("Error : Cannot create FileWriter Filter");
		return FALSE;
	}
	pAVI = dsWriter; 


	g_ACMWrapper=NULL;
	hr =(CoCreateInstance(CLSID_ACMWrapper, NULL, CLSCTX_INPROC, IID_IBaseFilter, (void **)&g_ACMWrapper));

	if(FAILED(hr))
	{
		LogMsg("Error : Cannot create filter for ACM Wrapper");
		return FALSE;
	}

	g_MonoAACEndcoder =NULL;
	hr =(CoCreateInstance(CLSID_MonogramAACEncoder, NULL, CLSCTX_INPROC, IID_IBaseFilter, (void **)&g_MonoAACEndcoder));	
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot create instance for Monogram AAC Encoder");
		return FALSE;
	}

	IMonogramAACEncoder *mmc_encoder=NULL;
	hr = g_MonoAACEndcoder->QueryInterface(IID_IMonogramAACEncoder,(void **)&mmc_encoder);

	if(FAILED(hr))
	{
		LogMsg("Error : Cannot Get Monogram AAC Encoder Interface");
	}
	else
	{

		AACConfig *audioconfig = new AACConfig();
		audioconfig->version		= AAC_VERSION_MPEG4;
		audioconfig->object_type	= AAC_OBJECT_LOW;
		audioconfig->output_type	= AAC_OUTPUT_RAW;
		audioconfig->bitrate		= audiobitrate*1000;
		
		hr =  mmc_encoder->SetConfig(audioconfig);

		if(FAILED(hr))
		{
			char *msg = new char[1000];
			sprintf(msg,"Error %d : Cannot Set Configuration for the MMC AAC Encoder",hr);
			LogMsg(msg);
		}
	}
}

BOOL Capture::BuildMp4CaptureGrapth()
{
	
	LogMsg("Start Mp4 Capture Graph Creation");
	
    HRESULT hr;

    AM_MEDIA_TYPE *pmt=0;

    // No rebuilding while we're running
    if(fCapturing)
        return FALSE;

	GetCaptureFiltersAndSource();

	// We don't have the necessary capture filters
    if(pVCap == NULL)
	{
		LogMsg("Error : No Video Filter Exist");
        return FALSE;
	}
    if(pACap == NULL)
	{
		LogMsg("Error : No Audio Filter Exist");
        return FALSE;
	}  

	IPin *pOut = NULL;
	hr = FindUnconnectedPin(pVCap,PINDIR_OUTPUT,&pOut);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot find unconnected output pin for source");
		return FALSE; 
	}

	
	// add x codec filter to graph
	 hr = pFg->AddFilter(pVcompFilter,L"Xcode Filter");
	 if(FAILED(hr))
	{
		char *msg = new char[300];
		sprintf(msg,"Error : %d Cannot add x codec filter to graph",hr);
		LogMsg(msg);	
		return FALSE; 
	}

	// connect mpeg decompressor to x codec filter
	hr = ConnectFilters(pFg,pOut,pVcompFilter);
	if(FAILED(hr))
	{
		char *msg = new char[300];
		sprintf(msg,"Error : %d Cannot connect  source and x codec filter",hr);
		LogMsg(msg);	
		return FALSE; 
	}

	// add mp4mux to graph
	pFg->AddFilter(g_mp4mux, L"GDCL Mux Filter");


	/* commented by meghana */
	IPin *pXcodecOut = NULL;
	hr = FindUnconnectedPin(pVcompFilter,PINDIR_OUTPUT,&pXcodecOut);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot find unconnected output pin for xcodec");
		return FALSE; 
	}

	// connect mpeg decompressor to source
	hr = ConnectFilters(pFg,pXcodecOut,g_mp4mux);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot connect xcodec and GDCL mux");
		return FALSE; 
	}

	IPin *pAOut = NULL;
	hr = FindUnconnectedPin(pACap,PINDIR_OUTPUT,&pAOut);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot find unconnected output pin for audio pin");
		return FALSE;
	}

	hr= pFg->AddFilter(g_ACMWrapper,L"ACM Wrapper");
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot add ACM Wrapper");
		return FALSE;
	}

	hr = ConnectFilters(pFg,pAOut,g_ACMWrapper);
	if(FAILED(hr))
	{
		_com_error err(hr);
		LPCTSTR msg = err.ErrorMessage();
		LogMsg("Error : Cannot connect Audio Pin and ACM Wrapper");
		return FALSE;
	}

	IPin *pACMWrapperOut = NULL;
	hr = FindUnconnectedPin(g_ACMWrapper,PINDIR_OUTPUT,&pACMWrapperOut);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot find unconnected output pin on ACM Wrapper");
		return FALSE;
	}

	hr = pFg->AddFilter(g_MonoAACEndcoder,L"Monogram AAC Encoder");
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot create filter for Monogram AAC Encoder");
		return FALSE;
	}

	hr = ConnectFilters(pFg,pACMWrapperOut,g_MonoAACEndcoder);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot connect ACM Wrapper and Monogram AAC Encoder");
		return FALSE;
			
	}

	IPin *pMonoAACEncoderrOut = NULL;
	hr = FindUnconnectedPin(g_MonoAACEndcoder,PINDIR_OUTPUT,&pMonoAACEncoderrOut);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot find unconnected output pin on ACM Wrapper");
		return FALSE;		
	}

	hr = ConnectFilters(pFg,pMonoAACEncoderrOut,g_mp4mux);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot connect Monogram AAC Encoder and GDCL Mux");
		return FALSE;
			
	}

	SYSTEMTIME st;
	FILETIME ft;
	time_t localtime = 0;
	time(&localtime);
	
	TimetToFileTime(localtime,&ft);
	FileTimeToSystemTime(&ft,&st);
	
	wcurrentoutput = (wchar_t*)calloc(8192,sizeof(wchar_t));
	currentoutput = (char*)calloc(8192,sizeof(char));

	swprintf_s(wcurrentoutput,8192,L"%s_%.2d%.2d%.2d_%.2d%.2d",stationid,st.wYear,st.wMonth,st.wDay,st.wHour,st.wMinute);
	sprintf(currentoutput,"%ls_%d%.2d%.2d_%.2d%.2d",stationid,st.wYear,st.wMonth,st.wDay,st.wHour,st.wMinute);
	
	wszCCFile = (char*)calloc(9000,sizeof(char));
		
	//wchar_t ccfile[9000],capfile[9000];
	sprintf(wszCCFile,"%ls\\%s.txt",templocation,currentoutput);
	swprintf_s(wszCaptureFile,9000,L"%s\\%s.mp4",templocation,wcurrentoutput); // tODO : need the wchar representation as well II guess

	hr =(pFg->AddFilter(pAVI, L"File Writer"));
	IFileSinkFilter2  *dsFileName;
	hr =(pAVI->QueryInterface(IID_IFileSinkFilter2, (void **)&dsFileName));
	hr =(dsFileName->SetMode(AM_FILE_OVERWRITE));
	hr =(dsFileName->SetFileName(wszCaptureFile,NULL));


	IPin *pMuxOut = NULL;
	hr = FindUnconnectedPin(g_mp4mux,PINDIR_OUTPUT,&pMuxOut);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot find unconnected output pin on ACM Wrapper");
		return FALSE;
	}

	hr = ConnectFilters(pFg,pMuxOut,pAVI);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot connect GDCL Mux and file writer");
		return FALSE;
			
	}

    // now ask the filtergraph to tell us when something is completed or aborted
    // (EC_COMPLETE, EC_USERABORT, EC_ERRORABORT).  This is how we will find out
    // if the disk gets full while capturing
    hr = pFg->QueryInterface(IID_IMediaEventEx, (void **)&pME);
    
    // potential debug output - what the graph looks like
    // DumpGraph(pFg, 1);

    // Add our graph to the running object table, which will allow
    // the GraphEdit application to "spy" on our graph
		#ifdef REGISTER_FILTERGRAPH
			hr = AddGraphToRot(pFg, &g_dwGraphRegister);
			if(FAILED(hr))
			{
				LogMsg("Failed to register filter graph with ROT!");
				g_dwGraphRegister = 0;
			}
		#endif

    // All done.
    fCaptureGraphBuilt = TRUE;
    return TRUE;

}

BOOL Capture::setCaptureInfo() {
	HRESULT hr;
	// //make sure the graph is built and stopped and filters are built etc.  This is to prevent API abuse
	//if(!pBuilder || fCapturing || pVCap == NULL) 
	//	return FALSE;
	
	LogMsg("Start Of SetCaptureInfo");

	SYSTEMTIME st;
	FILETIME ft;
	time_t localtime = 0;
	time(&localtime);
	TimetToFileTime(localtime,&ft);
	FileTimeToSystemTime(&ft,&st);
	
	wcurrentoutput = (wchar_t*)calloc(8192,sizeof(wchar_t));
	currentoutput = (char*)calloc(8192,sizeof(char));

	swprintf_s(wcurrentoutput,8192,L"%s_%.2d%.2d%.2d_%.2d%.2d",stationid,st.wYear,st.wMonth,st.wDay,st.wHour,st.wMinute);
	sprintf(currentoutput,"%ls_%d%.2d%.2d_%.2d%.2d",stationid,st.wYear,st.wMonth,st.wDay,st.wHour,st.wMinute);
	
	wszCCFile = (char*)calloc(9000,sizeof(char));
		
	//wchar_t ccfile[9000],capfile[9000];

	sprintf(wszCCFile,"%ls\\%s.txt",templocation,currentoutput);
	swprintf_s(wszCaptureFile,9000,L"%s\\%s.mp4",templocation,wcurrentoutput); 

	
	// reset the output name used
	{
		// render source output to mux
		
		// Assume that pAsfWriter is a valid IBaseFilter pointer.
		IServiceProvider *pProvider = NULL;
		
		IFileSinkFilter2  *dsFileName;
		hr =(pAVI->QueryInterface(IID_IFileSinkFilter2, (void **)&dsFileName));
		if(0) hr = pAVI->QueryInterface(
			IID_IServiceProvider, 
			(void**)&pProvider
			);

		if (SUCCEEDED(hr)) {
			//RETURN_FAIL((dsFileName->SetMode(AM_FILE_OVERWRITE)));
			hr = dsFileName->SetFileName(wszCaptureFile,NULL);

		}
	}
	return hr;
}

void Capture::x264vfw_config_reg_save(CONFIG *config)
{
    HKEY    hKey;
    DWORD   dwDisposition;
    int     i;

	LogMsg("Start of VFW Configuration");

	if (RegCreateKeyExA(X264VFW_REG_KEY, X264VFW_REG_PARENT "\\" X264VFW_REG_CHILD, 0, X264VFW_REG_CLASS, REG_OPTION_NON_VOLATILE, KEY_WRITE, 0, &hKey, &dwDisposition) != ERROR_SUCCESS)
        return;

    EnterCriticalSection(&x264vfw_CS);
    
	memcpy(&reg, config, sizeof(CONFIG));
	
	if(reg_curr.b_zerolatency == reg.b_zerolatency && reg_curr.i_encoding_type == reg.i_encoding_type && reg_curr.i_passbitrate == reg.i_passbitrate)
	{
		LogMsg("Configuration are same no need to update");
		LeaveCriticalSection(&x264vfw_CS);
		RegCloseKey(hKey);
		return;
	}

    GordianKnotWorkaround(reg.i_encoding_type);

    /* Save all named params */
	/// commented as of now we do not required to set the fields for vfw settings

    /*for (i = 0; i < sizeof(reg_named_str_table) / sizeof(reg_named_str_t); i++)
    {
        const char *temp = *reg_named_str_table[i].config_int >= 0 && *reg_named_str_table[i].config_int < reg_named_str_table[i].list_count
                           ? reg_named_str_table[i].list[*reg_named_str_table[i].config_int].value
                           : "";
		RegSetValueExA(hKey, reg_named_str_table[i].reg_value, 0, REG_SZ, (LPBYTE)temp, strlen(temp) + 1);
    }*/

    /* Save all integers */
    for (i = 0; i < sizeof(reg_int_table) / sizeof(reg_int_t); i++)
        RegSetValueExA(hKey, reg_int_table[i].reg_value, 0, REG_DWORD, (LPBYTE)reg_int_table[i].config_int, sizeof(int));

    /* Save all strings */

	/// commented as of now we do not required to set the fields for vfw settings

   /* for (i = 0; i < sizeof(reg_str_table) / sizeof(reg_str_t); i++)
        RegSetValueExA(hKey, reg_str_table[i].reg_value, 0, REG_SZ, (LPBYTE)reg_str_table[i].config_str, strlen(reg_str_table[i].config_str) + 1);
*/
    LeaveCriticalSection(&x264vfw_CS);
    RegCloseKey(hKey);
	memcpy(&reg_curr, config, sizeof(CONFIG));
}


// Check the devices we're currently using and make filters for them
//
void Capture::ChooseDevices(IMoniker *pmVideo1, IMoniker *pmAudio1)
{

	LogMsg("Start of Choose Devices using Config Params");


	HRESULT hr;
	

    // they chose a new device. rebuild the graphs
    /*if(pmVideo != pmVideo1 || pmAudio != pmAudio1)
    {*/
        if(pmVideo1)
        {
            pmVideo1->AddRef();
        }
        if(pmAudio1)
        {
            pmAudio1->AddRef();
        }
		

        //IMonRelease(pmVideo);
        //IMonRelease(pmAudio);
        pmVideo = pmVideo1;
        pmAudio = pmAudio1;

        
        

		//
    // First, we need a Video Capture filter, and some interfaces
    //
    pVCap = NULL;

    if(pmVideo != 0)
    {
        IPropertyBag *pBag;
        wachFriendlyName[0] = 0;

        hr = pmVideo->BindToStorage(0, 0, IID_IPropertyBag, (void **)&pBag);
        if(SUCCEEDED(hr))
        {
            VARIANT var;
            var.vt = VT_BSTR;

            hr = pBag->Read(L"FriendlyName", &var, NULL);
            if(hr == NOERROR)
            {
                hr = StringCchCopyW(wachFriendlyName, sizeof(wachFriendlyName) / sizeof(wachFriendlyName[0]), var.bstrVal);
                SysFreeString(var.bstrVal);
            }

            pBag->Release();
        }

        hr = pmVideo->BindToObject(0, 0, IID_IBaseFilter, (void**)&pVCap);
    }

    if(pVCap == NULL)
    {
        LogMsg("Error %x: Cannot create video capture filter");
        goto InitCapFiltersFail;
    }

   
    fCapAudioIsRelevant = TRUE;

    // we use this interface to bring up the 3 dialogs
    // NOTE:  Only the VfW capture filter supports this.  This app only brings
    // up dialogs for legacy VfW capture drivers, since only those have dialogs
    hr = pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE,
                                      &MEDIATYPE_Video, pVCap,
                                      IID_IAMVfwCaptureDialogs, (void **)&pDlg);

    
	// Get interfaces to get/set video properties
	  pOCD = new OspreyCaptureDevice(pVCap);

    
    //
    // We want an audio capture filter and some interfaces
    //

    if(pmAudio == 0)
    {
        // there are no audio capture devices. We'll only allow video capture
		//LogMsg("pmAudio = 0");
        fCapAudio = FALSE;
        goto SkipAudio;
    }
    pACap = NULL;

	
	 
    hr = pmAudio->BindToObject(0, 0, IID_IBaseFilter, (void**)&pACap);


    if(pACap == NULL)
    {
        // there are no audio capture devices. We'll only allow video capture
        fCapAudio = FALSE;
        LogMsg("Error : Cannot create audio capture filter");
        goto SkipAudio;
    }

    //
    // put the audio capture filter in the graph
    //
    {
        
        IPropertyBag *pBag;
        wachAudioFriendlyName[0] = 0;

        // Read the friendly name of the filter to assist with remote graph
        // viewing through GraphEdit
        hr = pmAudio->BindToStorage(0, 0, IID_IPropertyBag, (void **)&pBag);
        if(SUCCEEDED(hr))
        {
            VARIANT var;
            var.vt = VT_BSTR;

            hr = pBag->Read(L"FriendlyName", &var, NULL);
            if(hr == NOERROR)
            {
                hr = StringCchCopyW(wachAudioFriendlyName, 256, var.bstrVal);
				 //LogMsg("Filter Name:");
				
                SysFreeString(var.bstrVal);
            }

            pBag->Release();
        }
    }


SkipAudio:

    // Can this filter do closed captioning?
    IPin *pPin;
    hr = pBuilder->FindPin(pVCap, PINDIR_OUTPUT, &PIN_CATEGORY_VBI,
                                NULL, FALSE, 0, &pPin);
    if(hr != S_OK)
        hr = pBuilder->FindPin(pVCap, PINDIR_OUTPUT, &PIN_CATEGORY_CC,
                                    NULL, FALSE, 0, &pPin);
    if(hr == S_OK)
    {
        pPin->Release();
        fCCAvail = TRUE;
    }
    
	return;
    // Since the GetInfo method failed (or the interface did not exist),
    // display the device's friendly name.
    //statusUpdateStatus(ghwndStatus, wachFriendlyName);

	InitCapFiltersFail:
    FreeCapFilters();
}

void Capture::InitializeCC()
{
	if(m_aLine == NULL)
	{
		LogMsg("Start Of InitializeCC");
		m_aLine = new CLine(c_pF);

		// set Cline remaining Char String to write on first node of file.
		IBaseFilter* pCCLineInterp=NULL;
		  HRESULT hr = CoCreateInstance(
			CLSID_CCLineInterp,
			NULL,
			CLSCTX_INPROC_SERVER,
			IID_IBaseFilter,
			(void**)&pCCLineInterp
		  );
		  //LogMsg("initCC ");
		  if (hr == 0)
		  {
			hr = pCCLineInterp->QueryInterface(IID_ICCLineInterp, (void**)&m_aLine->m_pICCLineInterp);
			if(m_aLine->m_pICCLineInterp == NULL)
			{
				LogMsg("m_aLine->m_pICCLineInterp Is Null");
			}
			if (hr == 0)
			{
			  pCCLineInterp->Release();

			  CCDEVICE_CTL_S CCDeviceCtl;
			  CCDeviceCtl.ulFlags       = CCDEVICE_OPEN;
			  CCDeviceCtl.pOspreyDevice = pVCap;

			  hr = m_aLine->m_pICCLineInterp->CCDeviceCtl(&CCDeviceCtl);
			  if (hr == 0)
			  {
				   m_aLine->m_ulDeviceId = CCDeviceCtl.ulDeviceId;
			  }
			  else
			  {
				  LogMsg("Error : Can not Get CCDeviceControl for CCLineInterp");
			  }
			}
			else
			{
				LogMsg("Error : Can not Create Interface on CCLineInterp");
			}
		  }
		  else
		  {
			char *msg = new char[1000];
			sprintf(msg,"%x Can not Init CCLineInterp",hr);
			LogMsg(msg);
		  }
	}
}

// start the capture graph
//
BOOL Capture::StartCapture()
{    

	TRY
	{
		LogMsg("Start Of StartCapture");

		// prepare to run the graph
		IMediaControl *pMC = NULL;
		HRESULT hr = pFg->QueryInterface(IID_IMediaControl, (void **)&pMC);
		if(FAILED(hr))
		{
			LogMsg("Error : Cannot get IMediaControl");
			return FALSE;
		}
	
		InitializeCC();


		/* code for preview with capture*/

		//hr = pBuilder->RenderStream(&PIN_CATEGORY_PREVIEW,
	 //                                        &MEDIATYPE_Interleaved, pVCap, NULL, NULL);
	 //   if(hr != S_OK)
	 //   {
	 //       // maybe it's DV?
	 //       hr = pBuilder->RenderStream(&PIN_CATEGORY_PREVIEW,
	 //                                           &MEDIATYPE_Video, pVCap, NULL, NULL);
	 //       if(hr == VFW_S_NOPREVIEWPIN)
	 //       {
	 //           // preview was faked up for us using the (only) capture pin
	 //           fPreviewFaked = TRUE;
	 //       }
	 //       else if(hr != S_OK)
	 //       {
		//		LogMsg("This graph cannot preview!");
	 //           fPreviewGraphBuilt = FALSE;
	 //       }
	 //   }


	   REFERENCE_TIME stop = MAXLONGLONG;
			// turn the capture pin on now!
   
		hr = pBuilder->ControlStream(&PIN_CATEGORY_CAPTURE, NULL,
			NULL, NULL, &stop, 0, 0);
	
		hr = pMC->Run();

		if(FAILED(hr))
		{
			// stop parts that started
			pMC->Stop();
			pMC->Release();
			char *msg = new char[500];
			sprintf(msg,"Error %d : Cannot run graph",hr);
			LogMsg(msg);
			return FALSE;
		}
		else
		{
			LogMsg("Graph Started ");
		}
   
		pMC->Release();
	
		fCapturing=TRUE;


		hr = CoCreateInstance(CLSID_SystemClock,NULL,CLSCTX_INPROC_SERVER,IID_IReferenceClock,(void**)&pClock); 
		//hr = pACap->QueryInterface(IID_IReferenceClock,(void **)&pClock);
		if(pClock != NULL)
		{
			  pMediaFilter = 0;
			  hr = pFg->QueryInterface(IID_IMediaFilter, (void**)&pMediaFilter);
			  hr = pMediaFilter->SetSyncSource(pClock);
		  
			  hr = pClock->GetTime(&lCapStartTime);
			  char *msg = new char[1000];
			  sprintf(msg,"Start Reference Time Is : %d Seconds",(int)(lCapStartTime/10000000));
			  LogMsg(msg);
		  
		}
		else
		{
			char *msg = new char[1000];
			sprintf(msg,"%x : Unable to Init IReferenceClock",hr);
			LogMsg(msg);
		}
	
		// check if is there's any unwritten chars pending which are not written in prev. file
		if(m_aLine->IsInitated)
			m_aLine->ReInit(pClock,lCapStartTime,wszCCFile,stationid);
		else
			m_aLine->Init(pClock,lCapStartTime,wszCCFile,IDC_CC1,0,stationid);
		return TRUE;
	}
	CATCH(CException,ex)
	{
	  char *msg  = new char[301];
	  TCHAR   szCause[255];
	  ex->GetErrorMessage(szCause,255);
	  sprintf(msg ,"error:%ls",szCause);
	  LogMsg(msg);
	}
	END_CATCH  
}

// stop the capture graph
//
BOOL Capture::StopCapture()
{
	TRY
	{
		LogMsg("Start of StopCapture");

		// way ahead of you
		if(!fCapturing)
		{
			return FALSE;
		}

		IMediaControl *pMC = NULL;
		HRESULT hr = pFg->QueryInterface(IID_IMediaControl, (void **)&pMC);

	
		if(SUCCEEDED(hr))
		{
			hr = pMC->Stop();
			pMC->Release();
		}

	

		if(FAILED(hr))
		{
			LogMsg("Error : Cannot stop graph");
			return FALSE;
		}
		else
		{
			LogMsg("Graph Capture Stopped");
		}

		m_aLine->Term();

		if(pClock != NULL)
		{
			  REFERENCE_TIME ptime;
			  pClock->GetTime(&ptime);
			  char *msg = new char[1000];
			  sprintf(msg,"Stop Reference Time Is : %d Seconds",(int)(ptime/10000000));
			  LogMsg(msg);
		}
	
		fCapturing = FALSE;
		return TRUE;
	}
	CATCH(CException,ex)
	{
	  char *msg  = new char[301];
	  TCHAR   szCause[255];
	  ex->GetErrorMessage(szCause,255);
	  sprintf(msg ,"error:%ls",szCause);
	  LogMsg(msg);
	  return FALSE;
	}
	END_CATCH  
}

/*----------------------------------------------------------------------------*\
|   LogMsg - Opens a Message box with a error message in it.  The user can     |
|            select the OK button to continue                                  |
\*----------------------------------------------------------------------------*/
void Capture::LogMsg(char* szBuffer)
{
    SYSTEMTIME st1;
	GetSystemTime(&st1);
	fprintf(c_pF,"[%d-%2d-%2d %2d:%2d:%2d,%d] - %s\n",st1.wYear,st1.wMonth,st1.wDay,st1.wHour,st1.wMinute,st1.wSecond,st1.wMilliseconds, szBuffer);
	fflush(c_pF);
	
}

//// how many captured/dropped so far
////
//void Capture::UpdateStatus(BOOL fAllStats)
//{
//
//	LogMsg("Update Status Called");
//
//    HRESULT hr;
//    LONG lDropped, lNot=0, lAvgFrameSize;
//    TCHAR tach[160];
//
//    // we use this interface to get the number of captured and dropped frames
//    // NOTE:  We cannot query for this interface earlier, as it may not be
//    // available until the pin is connected
//    if(!pDF)
//    {
//        hr = pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE,
//                                          &MEDIATYPE_Interleaved, pVCap,
//                                          IID_IAMDroppedFrames, (void **)&pDF);
//        if(hr != S_OK)
//            hr = pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE,
//                                              &MEDIATYPE_Video, pVCap,
//                                              IID_IAMDroppedFrames, (void **)&pDF);
//    }
//
//    // this filter can't tell us dropped frame info.
//    if(!pDF)
//    {
//        //statusUpdateStatus(ghwndStatus, TEXT("Filter cannot report capture information"));
//		LogMsg("Filter cannot report capture information");
//        return;
//    }
//
//    hr = pDF->GetNumDropped(&lDropped);
//    if(hr == S_OK)
//        hr = pDF->GetNumNotDropped(&lNot);
//    if(hr != S_OK)
//        return;
//
//    lDropped -= lDroppedBase;
//    lNot -= lNotBase;
//
//    if(!fAllStats)
//    {
//        LONG lTime = timeGetTime() - lCapStartTime;
//        hr = StringCchPrintf(tach, 160, TEXT("Captured %d frames (%d dropped) %d.%dsec\0"), lNot,
//                 lDropped, lTime / 1000,
//                 lTime / 100 - lTime / 1000 * 10);
//		//LogMsg("Captured %d frames (%d dropped) %d.%dsec\0"),lNot,lDropped, lTime / 1000,lTime / 100 - lTime / 1000 * 10);
//        //statusUpdateStatus(ghwndStatus, tach);
//        return;
//    }
//
//    // we want all possible stats, including capture time and actual acheived
//    // frame rate and data rate (as opposed to what we tried to get).  These
//    // numbers are an indication that though we dropped frames just now, if we
//    // chose a data rate and frame rate equal to the numbers I'm about to
//    // print, we probably wouldn't drop any frames.
//
//    // average size of frame captured
//    hr = pDF->GetAverageFrameSize(&lAvgFrameSize);
//    if(hr != S_OK)
//        return;
//
//    // how long capture lasted
//    LONG lDurMS = lCapStopTime - lCapStartTime;
//    double flFrame;     // acheived frame rate
//    LONG lData;         // acheived data rate
//
//    if(lDurMS > 0)
//    {
//        flFrame = (double)(LONGLONG)lNot * 1000. /
//            (double)(LONGLONG)lDurMS;
//        lData = (LONG)(LONGLONG)(lNot / (double)(LONGLONG)lDurMS *
//            1000. * (double)(LONGLONG)lAvgFrameSize);
//    }
//    else
//    {
//        flFrame = 0.;
//        lData = 0;
//    }
//
//    hr = StringCchPrintf(tach, 160, TEXT("Captured %d frames in %d.%d sec (%d dropped): %d.%d fps %d.%d Meg/sec\0"),
//             lNot, lDurMS / 1000, lDurMS / 100 - lDurMS / 1000 * 10,
//             lDropped, (int)flFrame,
//             (int)(flFrame * 10.) - (int)flFrame * 10,
//             lData / 1000000,
//             lData / 1000 - (lData / 1000000 * 1000));
//    //statusUpdateStatus(ghwndStatus, tach);
//	//LogMsg("Captured %d frames in %d.%d sec (%d dropped): %d.%d fps %d.%d Meg/sec\0"),lNot, lDurMS / 1000, lDurMS / 100 - lDurMS / 1000 * 10,
//     //        lDropped, (int)flFrame,
//      //       (int)(flFrame * 10.) - (int)flFrame * 10,
//       //      lData / 1000000,
//       //      lData / 1000 - (lData / 1000000 * 1000));
//}