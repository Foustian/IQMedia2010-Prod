#include "stdafx.h"
#include "Capture.h"
#include "status.h"
#include <comdef.h>
#include <dvdmedia.h>
#include "MonoGramAACConfig.h"
DWORD g_dwGraphRegister=0;
/* Global DLL critical section */
CRITICAL_SECTION x264vfw_CS;


/////////////////////////////////////////////////////////////////////////////
//
// Capture
//
Capture::Capture(int deviceno,wchar_t* logdirlocation)
{
	// lets first create / open log file.
	char *LogFile = new char[500];
	m_aLine =NULL;
	SYSTEMTIME st;
	GetSystemTime(&st);
	sprintf_s(LogFile,500,"%ls\\LogDevice%d_%d%.2d%.2d.txt",logdirlocation,deviceno,st.wYear,st.wMonth,st.wDay);
	errno_t err = fopen_s(&c_pF,LogFile, "a+");

	LogMsg("Capture Application Initialized");

	
	/* this was added for preview parent window*/
	//Create(NULL,L"MP4Capture");
	//SetWindowPos(&wndBottom,100,100,640,480,SWP_SHOWWINDOW);
	
	fCapAudio = TRUE;
	memset(&this->pBuilder,0,sizeof(pBuilder));
	fCapturing = FALSE;
	fCaptureGraphBuilt = FALSE;
	
	// needed for preview
	//fPreviewGraphBuilt = FALSE;
	
	// this code we needed for giving rights to enter critical section 
	//as we need to edit registry entry for x264vfw settings
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


// this all re the GUIDS for external filters that we added (CCLineInterPreter and GDCL Mux Filter)
const CLSID CLSID_GDCLMp4Mux = { 0x5FD85181, 0xE542, 0x4E52, { 0x8D, 0x9D, 0x5D, 0x61, 0x3C, 0x30, 0x13, 0x1B }};
const CLSID CLSID_CCLineInterp = {0xae6a37f1, 0x62ef, 0x4140,{0x82, 0x92, 0xc0, 0x71, 0x85, 0x67, 0x96, 0xb5}};

// we will use this to get CC in line.cpp
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
    // make ICaptureGraphBuilder object
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
    
	// make graph object
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

// put all installed video and audio devices in the array
//
void Capture::GetCaptureDevices()
{
    
	LogMsg("Get Devices For Capture Application");

    iNumVCapDevices = 0;
    UINT    uIndex = 0;

    HRESULT hr;
    BOOL bCheck = FALSE;

    // enumerate all video capture devices
	// create object for device enumurator to get all devices
    ICreateDevEnum *pCreateDevEnum=0;
    hr = CoCreateInstance(CLSID_SystemDeviceEnum, NULL, CLSCTX_INPROC_SERVER,
                          IID_ICreateDevEnum, (void**)&pCreateDevEnum);
    if(hr != NOERROR)
    {
        LogMsg("Error Creating Device Enumerator");
        return;
    }

	// this will get IMoniker reference for all Video Input Devices (using CLSID_VideoInputDeviceCategory)
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

	// loop trough all the video capture devices using moniker and all monikers pointer to array,
	// later we use them for making our Video Capture Source Filter (by index defined in config)
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
				// we will add them to array  , so we can use it later
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

	// as x264vfw can not be created directly using CLSID like other filters.
	// we can create it using monikers 
	// as x264 is compresson filter we can get it from 'Video Compression' category
	// we will get all compression filters and loop them using monikers and just create x264vfw filter
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
				
				if (strstr(nameBuf,"x264vfw - H.264/MPEG-4 AVC codec") != NULL)
				{
					// okey, we get x264vfw filter ,
					// we have to create filter for it here only , 
					// later after if we want to create from moniker , it does not work.. strange!!!! 
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

    // the another strange thing about osprey audio devices is , we do not get from audio devices
	// we has to loop all streaming devices (which include both , all audio and video devices)
	// and then add only audio devices in array.
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

				// we have fetched name of streaming device in one variable, 
				// we have to check that if they are Opsrey Audio device only
				// and then only we'll add them in array.
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
    
	//SAFE_RELEASE(pRender);	
	//SAFE_RELEASE(pME);
	//SAFE_RELEASE(pDF);


	/// this is added  for preview
  //  if(pVW)
   // {
        // stop drawing in our window, or we may get wierd repaint effects
//        pVW->put_Owner(NULL);
 //       pVW->put_Visible(OAFALSE);
 //       pVW->Release();
//        pVW = NULL;
 //   }

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
    
	// needed for preview
    // fPreviewFaked = FALSE;
}

// create the capture filters of the graph.  We need to keep them loaded from the beginning, 
// and then we only add and remove filters from graph as needed . we'll not create release graph. 
// als we can set parameters on them and have them remembered
BOOL Capture::InitCapFilters()
{
	LogMsg("Start Of InitCapFilters");

    HRESULT hr=S_OK;
    BOOL f;

    
	// make graph builder object
    f = MakeBuilder();
    if(!f)
    {
        LogMsg("Cannot instantiate graph builder");
        return FALSE;
    }
    
    // make a filtergraph object
    f = MakeGraph();
    if(!f)
    {
        LogMsg("Cannot instantiate filtergraph");
        return FALSE;
    }

	// set graph object to our builder object
    hr = pBuilder->SetFiltergraph(pFg);
    if(hr != NOERROR)
    {
        LogMsg("Cannot give graph to builder");
        return FALSE;
    }

	// intailly we'll create instace for all filters (just one  time for appliction)
	// later when creating filter graph , we'll add that filters to graph and connect from one-another. 
	// so here , we create instaces for all filters.
	MakeGraphInstances();

    return TRUE;
}

// method to set the height and width and framerate for the video
void Capture::SetFormatSize()
{

  /* this was added for preview framrate*/
  // needed for preview
 /* AM_MEDIA_TYPE *pmtPreview;

  if(pVPSC && pVPSC->GetFormat(&pmtPreview) == S_OK)
  {
	  char* msg = new char[300];
	  LogMsg("going to set framte for Preview Pin");
	  BITMAPINFOHEADER* pBMIH = NULL;

	  if (pmtPreview->formattype == FORMAT_VideoInfo) 
      {
		  VIDEOINFOHEADER* pVIH = (VIDEOINFOHEADER*)(pmtPreview->pbFormat);
		  pVIH->AvgTimePerFrame = (LONGLONG)(10000000 / 7.00);
		  pBMIH = &pVIH->bmiHeader;
		  sprintf_s(msg,300,"Preview AvgTimePerFrame is %ld",pVIH->AvgTimePerFrame);
		  LogMsg(msg);
	  }
	  else if (pmtPreview->formattype == FORMAT_VideoInfo2) 
      {
		  VIDEOINFOHEADER2* pVIH2 = (VIDEOINFOHEADER2*)(pmtPreview->pbFormat);
		  pVIH2->AvgTimePerFrame  = (LONGLONG)(10000000 / 7.00);
		  pBMIH = &pVIH2->bmiHeader;
		  sprintf_s(msg,300,"Preview AvgTimePerFrame is %ld",pVIH2->AvgTimePerFrame);
		  LogMsg(msg);
      }
	  HRESULT hr = pVSC->SetFormat(pmtPreview);
	  if(FAILED(hr))
	  {
		  sprintf_s(msg,300,"Error : %d can not save format",hr);
		  LogMsg(msg);
	  }
	  DeleteMediaType(pmtPreview);
  }*/

  // default capture format

	AM_MEDIA_TYPE *pmt;

  // we'll set framerate for our video device here. using IAMStreamConfig* object
  if (pVSC && pVSC->GetFormat(&pmt) == S_OK)
  {
	char* msg = new char[300];
	sprintf_s(msg,300,"going to set framerate : %f",FrameRate);
	LogMsg(msg);
    // set the height and width
    BITMAPINFOHEADER* pBMIH = NULL;

    if (pmt->formattype == FORMAT_VideoInfo) 
    {
      VIDEOINFOHEADER* pVIH = (VIDEOINFOHEADER*)(pmt->pbFormat);
      pVIH->AvgTimePerFrame = (LONGLONG)(10000000 / FrameRate);
      pBMIH = &pVIH->bmiHeader;
	  sprintf_s(msg,300,"AvgTimePerFrame is %ld",pVIH->AvgTimePerFrame);
	  LogMsg(msg);
    }

    else
    if (pmt->formattype == FORMAT_VideoInfo2) 
    {
      VIDEOINFOHEADER2* pVIH2 = (VIDEOINFOHEADER2*)(pmt->pbFormat);
      pVIH2->AvgTimePerFrame  = (LONGLONG)(10000000 / FrameRate);
      pBMIH = &pVIH2->bmiHeader;
	  sprintf_s(msg,300,"AvgTimePerFrame is %ld",pVIH2->AvgTimePerFrame);
	  LogMsg(msg);
    }
	
	// we have get videoheight and videowidth from config, 
	// if both specified then we set height and width for our output mp4 file
	if(pBMIH && Videowidth >0 && Videoheight>0)
	{
      pBMIH->biWidth = Videowidth;
      if (ABS(pBMIH->biHeight) < 0)
		pBMIH->biHeight = -Videoheight;
      else
		pBMIH->biHeight =  Videoheight;
    
	}
	HRESULT hr = pVSC->SetFormat(pmt);
	if(FAILED(hr))
	{
		sprintf_s(msg,300,"Error : %d can not save format",hr);
		LogMsg(msg);
	}
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
	// after stopping capture , 
	// we now remove all the filters from graph
	// in next loop we again add them to graph to and again start capture.
	LogMsg("Start Of FreeCapFilters");

	pFg->RemoveFilter(pVCap);
	pFg->RemoveFilter(pVcompFilter);
	pFg->RemoveFilter(g_mp4mux);

	pFg->RemoveFilter(pACap);
	pFg->RemoveFilter(g_ACMWrapper);
	pFg->RemoveFilter(g_MonoAACEndcoder);
	pFg->RemoveFilter(pAVI);
	
	
	/* this was added for preview remove filters */
	// needed for preview
	//IBaseFilter *pSmartTee=NULL;
	//HRESULT hr = pFg->FindFilterByName(L"Smart Tee",&pSmartTee);
	//if(SUCCEEDED(hr) && pSmartTee != NULL)
	//{
	//	pFg->RemoveFilter(pSmartTee);
	//}


	//IBaseFilter *pVideoRender=NULL;
	//hr = pFg->FindFilterByName(L"Video Renderer",&pVideoRender);
	//if(SUCCEEDED(hr) && pVideoRender != NULL)
	//{
	//	pFg->RemoveFilter(pVideoRender);
	//}

	//IBaseFilter *AviDecompressor=NULL;
	//hr = pFg->FindFilterByName(L"AVI Decompressor",&AviDecompressor);
	//if(SUCCEEDED(hr) && AviDecompressor != NULL)
	//{
	//	pFg->RemoveFilter(AviDecompressor);
	//}
	//
	//if(pVW)
 //   {
 //       // stop drawing in our window, or we may get wierd repaint effects
	//	pVW->put_Owner(NULL);
	//	pVW->put_Visible(OAFALSE);
	//	pVW->Release();
 //       pVW = NULL;
 //   }


	pBuilder->ReleaseFilters();

    // potential debug output - what the graph looks like
    // if (gcap.pFg) DumpGraph(gcap.pFg, 1);

#ifdef REGISTER_FILTERGRAPH
    // Remove filter graph from the running object table
    if(g_dwGraphRegister)
    {
        RemoveGraphFromRot(g_dwGraphRegister);
        g_dwGraphRegister = 0;
    }
#endif

    fCaptureGraphBuilt = FALSE;
    
	// needed for preview
	//fPreviewGraphBuilt = FALSE;
    //fPreviewFaked = FALSE;

}

BOOL Capture::GetCaptureFiltersAndSource()
{
	// Add the video capture filter to the graph with its friendly name
    HRESULT hr = pFg->AddFilter(pVCap, wachFriendlyName);
    if(hr != NOERROR)
    {
        LogMsg("Error %x: Cannot add vidcap to filtergraph");
		return FALSE;
    }

    // Calling FindInterface below will result in building the upstream
    // section of the capture graph (any WDM TVTuners or Crossbars we might
    // need).
    
    // !!! What if this interface isn't supported?
    // we use this interface to set the frame rate and get the capture size
	// later we'll use this to set framerate and height and width of output file
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


	/* this was added for preview framrate*/
	// !!! What if this interface isn't supported?
    // we use this interface to set the frame rate and get the capture size
    //hr = pBuilder->FindInterface(&PIN_CATEGORY_PREVIEW,
    //                                  &MEDIATYPE_Interleaved,
				//					  pVCap, IID_IAMStreamConfig, (void **)&pVPSC);

    //if(hr != NOERROR)
    //{
    //    hr = pBuilder->FindInterface(&PIN_CATEGORY_PREVIEW,
    //                                      &MEDIATYPE_Video, pVCap,
    //                                      IID_IAMStreamConfig, (void **)&pVPSC);


    //    if(hr != NOERROR)
    //    {
    //        // this means we can't set frame rate (non-DV only)
    //        LogMsg("Error %x: Cannot find VPreview:IAMStreamConfig");
    //    }
    //}


	// We'll need this in the graph to get audio property pages


	// Add the audio capture filter to the graph with its friendly name

	hr= NOERROR;
	
	// add audio filter with its friendly name 
    hr = pFg->AddFilter(pACap, wachAudioFriendlyName);
    if(hr != NOERROR)
    {
        LogMsg("Error Cannot add audio capture filter to filtergraph");
		return FALSE;
        
    }
    
	
    long lStandard;
	if (pOCD)
	{
		pOCD->getStandard(&lStandard);
		
		// now set framerate for input video source , and height / width of output file.
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
	
	// we have all the audio video monikers in array .
	// we just need one filter as provided in config. 
	// we get that devices and add only them to graph
	IMoniker *pmVideo1 = 0, *pmAudio1 = 0;
	pmVideo1 = rgpmVideoMenu[devnum];
	pmAudio1 = rgpmAudioMenu[adevnum];
	
	// now we'll create filter instances for selected audio and video device from their monikers
	ChooseDevices(pmVideo1,pmAudio1);

	HRESULT hr;

	// Create instance of GDCL MPEG-4 Multiplexer filter.
	g_mp4mux=NULL;
	hr = CoCreateInstance(CLSID_GDCLMp4Mux, NULL, CLSCTX_INPROC_SERVER,
							IID_IBaseFilter, ( void **) &g_mp4mux);

	if (FAILED(hr)){
		LogMsg("Error : Cannot create GDCL Mux Filter");
		return FALSE;
	}

	// Create instance of file-writer
	IBaseFilter *dsWriter;
	hr =(CoCreateInstance(CLSID_FileWriter, NULL, CLSCTX_INPROC, IID_IBaseFilter, (void **)&dsWriter));
	if (FAILED(hr)){
		LogMsg("Error : Cannot create FileWriter Filter");
		return FALSE;
	}
	pAVI = dsWriter; 

	// Create instance of acm wrapper.
	g_ACMWrapper=NULL;
	hr =(CoCreateInstance(CLSID_ACMWrapper, NULL, CLSCTX_INPROC, IID_IBaseFilter, (void **)&g_ACMWrapper));

	if(FAILED(hr))
	{
		LogMsg("Error : Cannot create filter for ACM Wrapper");
		return FALSE;
	}

	// Create instance of monogram acc encoder.
	g_MonoAACEndcoder =NULL;
	hr =(CoCreateInstance(CLSID_MonogramAACEncoder, NULL, CLSCTX_INPROC, IID_IBaseFilter, (void **)&g_MonoAACEndcoder));	
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot create instance for Monogram AAC Encoder");
		return FALSE;
	}

	// we need to set bitrate for the audio device ,
	// we'll do that in Monogram AAC Encoder Filter
	// well , we have included its library in Linker section
	// and then just included header file 'MonoGramAACConfig.h' , so that we can use its interface
	// and can call its method to set configuration
	IMonogramAACEncoder *mmc_encoder=NULL;
	hr = g_MonoAACEndcoder->QueryInterface(IID_IMonogramAACEncoder,(void **)&mmc_encoder);

	if(FAILED(hr))
	{
		LogMsg("Error : Cannot Get Monogram AAC Encoder Interface");
		return FALSE;
	}
	else
	{
		// we have retrived the Interface on monogram aac encoder
		// so now we'll set the config values for it 
		AACConfig *audioconfig = new AACConfig();
		audioconfig->version		= AAC_VERSION_MPEG4;
		audioconfig->object_type	= AAC_OBJECT_LOW;
		audioconfig->output_type	= AAC_OUTPUT_RAW;
		audioconfig->bitrate		= audiobitrate*1000;
		
		// set configuration for audio and per config settings 
		hr =  mmc_encoder->SetConfig(audioconfig);

		if(FAILED(hr))
		{
			char *msg = new char[1000];
			sprintf_s(msg,1000,"Error %d : Cannot Set Configuration for the MMC AAC Encoder",hr);
			LogMsg(msg);
		}
	}
	return TRUE;
}

BOOL Capture::BuildMp4CaptureGraph()
{
	
	LogMsg("Start Mp4 Capture Graph Creation");
	
    HRESULT hr;

    AM_MEDIA_TYPE *pmt=0;

    // No rebuilding while we're running
    if(fCapturing)
        return FALSE;

	// first we need video and audio capture sources and their StramConfig objects
	GetCaptureFiltersAndSource();
	
	/* this was added for preview framrate*/
	//BuildPreviewGraph();

	// if We don't have the necessary capture filters then retrun
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

	// add x codec filter to graph
	 hr = pFg->AddFilter(pVcompFilter,L"Xcode Filter");
	 if(FAILED(hr))
	{
		char *msg = new char[300];
		sprintf_s(msg,300,"Error : %d Cannot add x codec filter to graph",hr);
		LogMsg(msg);	
		return FALSE; 
	}

	// find capture pin of video filter
	IPin *pOut = NULL;
	hr = FindUnconnectedPin(pVCap,PINDIR_OUTPUT,&pOut);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot find unconnected output pin for source");
		return FALSE; 
	}

	// connect it to capture pin of video filter to xcodec filter
	hr = ConnectFilters(pFg,pOut,pVcompFilter);
	if(FAILED(hr))
	{
		char *msg = new char[300];
		sprintf_s(msg,300,"Error : %d Cannot connect  source and x codec filter",hr);
		LogMsg(msg);	
		return FALSE; 
	}

	// add mp4mux to graph
	pFg->AddFilter(g_mp4mux, L"GDCL Mux Filter");


	// find output pin of xcodec filter 
	IPin *pXcodecOut = NULL;
	hr = FindUnconnectedPin(pVcompFilter,PINDIR_OUTPUT,&pXcodecOut);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot find unconnected output pin for xcodec");
		return FALSE; 
	}

	// connect connect x254vfw codec filter to mp4mux filter
	hr = ConnectFilters(pFg,pXcodecOut,g_mp4mux);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot connect xcodec and GDCL mux");
		return FALSE; 
	}

	// add audio filter to graph
	hr= pFg->AddFilter(g_ACMWrapper,L"ACM Wrapper");
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot add ACM Wrapper");
		return FALSE;
	}
	
	// find output pin of audio filter 
	IPin *pAOut = NULL;
	hr = FindUnconnectedPin(pACap,PINDIR_OUTPUT,&pAOut);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot find unconnected output pin for audio pin");
		return FALSE;
	}

	// connect audio output pin to acm wrapper.
	hr = ConnectFilters(pFg,pAOut,g_ACMWrapper);
	if(FAILED(hr))
	{
		_com_error err(hr);
		LPCTSTR msg = err.ErrorMessage();
		LogMsg("Error : Cannot connect Audio Pin and ACM Wrapper");
		return FALSE;
	}

	// add monogram AAC Encoder to graph
	hr = pFg->AddFilter(g_MonoAACEndcoder,L"Monogram AAC Encoder");
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot create filter for Monogram AAC Encoder");
		return FALSE;
	}

	// find output pin of acm wrapper and connect it to monogram AAC encoder
	IPin *pACMWrapperOut = NULL;
	hr = FindUnconnectedPin(g_ACMWrapper,PINDIR_OUTPUT,&pACMWrapperOut);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot find unconnected output pin on ACM Wrapper");
		return FALSE;
	}

	hr = ConnectFilters(pFg,pACMWrapperOut,g_MonoAACEndcoder);
	if(FAILED(hr))
	{
		LogMsg("Error : Cannot connect ACM Wrapper and Monogram AAC Encoder");
		return FALSE;
			
	}

	// find output pin of monogram AAC Encoder and connect it to GDCL Mux
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

	
	// set file names varibales for our CC file and mp4 file 
	// format will be like 
	//StationID_YearMonthDay_HourMinute.txt
	//and StationID_YearMonthDay_HourMinute.mp4
	SYSTEMTIME st;
	FILETIME ft;
	time_t localtime = 0;
	time(&localtime);
	
	TimetToFileTime(localtime,&ft);
	FileTimeToSystemTime(&ft,&st);
	
	wcurrentoutput = (wchar_t*)calloc(8192,sizeof(wchar_t));
	currentoutput = (char*)calloc(8192,sizeof(char));

	swprintf_s(wcurrentoutput,8192,L"%s_%.2d%.2d%.2d_%.2d%.2d",stationid,st.wYear,st.wMonth,st.wDay,st.wHour,st.wMinute);
	sprintf_s(currentoutput,8912,"%ls_%d%.2d%.2d_%.2d%.2d",stationid,st.wYear,st.wMonth,st.wDay,st.wHour,st.wMinute);
	
	wszCCFile = (char*)calloc(9000,sizeof(char));
	
	
	sprintf_s(wszCCFile,9000,"%ls\\%s.txt",templocation,currentoutput);
	swprintf_s(wszCaptureFile,9000,L"%s\\%s.mp4",templocation,wcurrentoutput); // tODO : need the wchar representation as well II guess

	// add file writer to graph and set its filename. 
	hr =(pFg->AddFilter(pAVI, L"File Writer"));
	IFileSinkFilter2  *dsFileName;
	hr =(pAVI->QueryInterface(IID_IFileSinkFilter2, (void **)&dsFileName));
	hr =(dsFileName->SetMode(AM_FILE_OVERWRITE));
	hr =(dsFileName->SetFileName(wszCaptureFile,NULL));

	// find outpin of mux and connect it to file-writer
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

//// build the preview graph!
////
//// !!! PLEASE NOTE !!!  Some new WDM devices have totally separate capture
//// and preview settings.  An application that wishes to preview and then
//// capture may have to set the preview pin format using IAMStreamConfig on the
//// preview pin, and then again on the capture pin to capture with that format.
//// In this sample app, there is a separate page to set the settings on the
//// capture pin and one for the preview pin.  To avoid the user
//// having to enter the same settings in 2 dialog boxes, an app can have its own
//// UI for choosing a format (the possible formats can be enumerated using
//// IAMStreamConfig) and then the app can programmatically call IAMStreamConfig
//// to set the format on both pins.
////
//BOOL Capture::BuildPreviewGraph()
//{
//
//    // we have one already
//    if(fPreviewGraphBuilt)
//        return TRUE;
//
//    // No rebuilding while we're running
//    if(fCapturing)
//        return FALSE;
//
//    // We don't have the necessary capture filters
//    if(pVCap == NULL)
//        return FALSE;
//    if(pACap == NULL && fCapAudio)
//        return FALSE;
//
//    
//
//    //
//    // Render the preview pin - even if there is not preview pin, the capture
//    // graph builder will use a smart tee filter and provide a preview.
//    //
//    // !!! what about latency/buffer issues?
//
//    // NOTE that we try to render the interleaved pin before the video pin, because
//    // if BOTH exist, it's a DV filter and the only way to get the audio is to use
//    // the interleaved pin.  Using the Video pin on a DV filter is only useful if
//    // you don't want the audio.
//
//    
//       HRESULT hr = pBuilder->RenderStream(&PIN_CATEGORY_PREVIEW,
//                                         &MEDIATYPE_Interleaved, pVCap, NULL, NULL);
//        if(hr == VFW_S_NOPREVIEWPIN)
//        {
//            // preview was faked up for us using the (only) capture pin
//            fPreviewFaked = TRUE;
//        }
//        else if(hr != S_OK)
//        {
//            // maybe it's DV?
//            hr = pBuilder->RenderStream(&PIN_CATEGORY_PREVIEW,
//                                             &MEDIATYPE_Video, pVCap, NULL, NULL);
//            if(hr == VFW_S_NOPREVIEWPIN)
//            {
//                // preview was faked up for us using the (only) capture pin
//                fPreviewFaked = TRUE;
//            }
//            else if(hr != S_OK)
//            {
//                LogMsg("This graph cannot preview!");
//                fPreviewGraphBuilt = FALSE;
//                return FALSE;
//            }
//        }
//
//        
//
//    //
//    // Get the preview window to be a child of our app's window
//    //
//
//    // This will find the IVideoWindow interface on the renderer.  It is
//    // important to ask the filtergraph for this interface... do NOT use
//    // ICaptureGraphBuilder2::FindInterface, because the filtergraph needs to
//    // know we own the window so it can give us display changed messages, etc.
//
//    hr = pFg->QueryInterface(IID_IVideoWindow, (void **)&pVW);
//    if(hr != NOERROR)
//    {
//        LogMsg("This graph cannot preview properly");
//    }
//    else
//    {
//        pVW->put_Owner((OAHWND)GetSafeHwnd());    // We own the window now
//        pVW->put_WindowStyle(WS_CHILD|WS_CLIPSIBLINGS);    // you are now a child
//        RECT rect;
//		GetClientRect(&rect);
//		pVW->SetWindowPosition(0, 0,rect.right - rect.left,rect.bottom - rect.top);
//
//		pVW->put_Visible(OATRUE);
//		pVW->SetWindowForeground(OATRUE);
//	}
//
//    // make sure we process events while we're previewing!
//    hr = pFg->QueryInterface(IID_IMediaEventEx, (void **)&pME);
//    if(hr == NOERROR)
//    {
//        hr= pME->SetNotifyWindow((OAHWND)GetSafeHwnd(),WM_GRAPH_NOTIFY, NULL);
//    }
//
//    // potential debug output - what the graph looks like
//    // DumpGraph(pFg, 1);
//
//    // Add our graph to the running object table, which will allow
//    // the GraphEdit application to "spy" on our graph
//#ifdef REGISTER_FILTERGRAPH
//    hr = AddGraphToRot(gcap.pFg, &g_dwGraphRegister);
//    if(FAILED(hr))
//    {
//        ErrMsg(TEXT("Failed to register filter graph with ROT!  hr=0x%x"), hr);
//        g_dwGraphRegister = 0;
//    }
//#endif
//
//    // All done.
//    fPreviewGraphBuilt = TRUE;
//    return TRUE;
//}



/// save settings for vfwconfig , this will edit settings in registery
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
void Capture::ChooseDevices(IMoniker *pmVideo, IMoniker *pmAudio)
{

	LogMsg("Start of Choose Devices using Config Params");
	HRESULT hr;
	
    if(pmVideo)
    {
        pmVideo->AddRef();
    }
    if(pmAudio)
    {
        pmAudio->AddRef();
    }
		
    // First, we need a Video Capture filter, and some interfaces
    // we'll create filter object from moniker and get name of device using propertybag for moniker
	// later w'll use that name while adding filter to graph with name as of this name
    pVCap = NULL;

    if(pmVideo != 0)
    {
        IPropertyBag *pBag;
        wachFriendlyName[0] = 0;

		// we will use IPropertyBag to fetch name of the Video Device
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

		// this will create filter object from moniker 
        hr = pmVideo->BindToObject(0, 0, IID_IBaseFilter, (void**)&pVCap);
    }

    if(pVCap == NULL)
    {
        LogMsg("Error %x: Cannot create video capture filter");
        return;
    }

    // Get interfaces to get/set video properties
	// get osprey crossbar device.
	pOCD = new OspreyCaptureDevice(pVCap);

    if(pmAudio == 0)
    {
        // there are no audio capture devices. We'll only allow video capture
		//LogMsg("pmAudio = 0");
        fCapAudio = FALSE;
        return;
    }
    pACap = NULL;

	
	// create filter object from moniker 
    hr = pmAudio->BindToObject(0, 0, IID_IBaseFilter, (void**)&pACap);


    if(pACap == NULL)
    {
        // there are no audio capture devices. We'll only allow video capture
        fCapAudio = FALSE;
        LogMsg("Error : Cannot create audio capture filter");
        return;
    }

    //
    // put the audio capture filter in the graph
	//get name of device using propertybag for moniker
	// later w'll use that name while adding filter to graph with name as of this name
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
}
// Initialize CCLineInterPreter object to get cc over video filter
void Capture::InitializeCC()
{
	
	// nice thing about CC is that we not need to stop it, 
	// so we just create it once for application
	// on next capture loop , we'll get that and we not need to create it again. 
	if(m_aLine == NULL)
	{
		LogMsg("Start Of InitializeCC");
		m_aLine = new CLine(c_pF);

		IBaseFilter* pCCLineInterp=NULL;
		  HRESULT hr = CoCreateInstance(
			CLSID_CCLineInterp,
			NULL,
			CLSCTX_INPROC_SERVER,
			IID_IBaseFilter,
			(void**)&pCCLineInterp
		  );
		  
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
			sprintf_s(msg,1000,"%x Can not Init CCLineInterp",hr);
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


		// to get CC from Video 
		// first we need to create filter object for CCLineInterPreter 
		// and we then make interface on CCLineInterpreter filter 
		// to set Video device for to get CC from and get device control on that video filter
		InitializeCC();
		
		// we do this for our checking purpose that graph we have created is proper or not.
		// save graph in root of application 
		hr = SaveGraphFile(pFg,L"graph.grf");
		

	   REFERENCE_TIME stop = MAXLONGLONG;
			
   // turn the capture pin on now!
		hr = pBuilder->ControlStream(&PIN_CATEGORY_CAPTURE, NULL,
			NULL, NULL, &stop, 0, 0);
	
		// capture started. 
		hr = pMC->Run();

		if(FAILED(hr))
		{
			// stop parts that started
			pMC->Stop();
			pMC->Release();
			char *msg = new char[500];
			sprintf_s(msg,500,"Error %d : Cannot run graph",hr);
			LogMsg(msg);
			return FALSE;
		}
		else
		{
			LogMsg("Graph Started ");
		}
   
		pMC->Release();
	
		fCapturing=TRUE;

		// each filter graph inbuilt make refernce clock that streamline its all capture 
		// but we need to override this clock for specific reason
		// as we need to add time in our CC File with text ,
		// we use this clock to find actual time after capture started. and use that time in CC file.
		hr = CoCreateInstance(CLSID_SystemClock,NULL,CLSCTX_INPROC_SERVER,IID_IReferenceClock,(void**)&pClock); 
		if(pClock != NULL)
		{
			  pMediaFilter = 0;
			  hr = pFg->QueryInterface(IID_IMediaFilter, (void**)&pMediaFilter);
			  hr = pMediaFilter->SetSyncSource(pClock);
		  
			  // we use this time to calculate relative time for Nth graph after it started running.
			  hr = pClock->GetTime(&lCapStartTime);
			  char *msg = new char[1000];
			  sprintf_s(msg,1000,"Start Reference Time Is : %d Seconds",(int)(lCapStartTime/10000000));
			  LogMsg(msg);
		  
		}
		else
		{
			char *msg = new char[1000];
			sprintf_s(msg,1000,"%x : Unable to Init IReferenceClock",hr);
			LogMsg(msg);
		}
		
		
		// lets init/ reint the CCLine now. 
		// this will create cc text file and 
		// and starts writing CC on text file , and get relative vieo from Reference Clock object
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
	  sprintf_s(msg ,255,"error:%ls",szCause);
	  LogMsg(msg);

	  return FALSE;
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

		// stop the capture	
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

		// well now CCline need to term
		// this just close the file ,  but do not stop to get CC from video.
		// until next loop starts , it will save chars to one varibale
		// and then after new capture starts , it start writing to file
		m_aLine->Term();

		if(pClock != NULL)
		{
			  REFERENCE_TIME ptime;
			  pClock->GetTime(&ptime);
			  char *msg = new char[1000];
			  sprintf_s(msg,1000,"Stop Reference Time Is : %d Seconds",(int)(ptime/10000000));
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
	  sprintf_s(msg ,255,"error:%ls",szCause);
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

