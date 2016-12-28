//------------------------------------------------------------------------------
// File: AMCap.cpp
//
// Desc: Audio/Video Capture sample for DirectShow
//
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

#include "stdafx.h"
#include "amcap.h"


#ifdef _DEBUG
  #define new DEBUG_NEW
  #undef THIS_FILE
  static char THIS_FILE[] = __FILE__;
#endif

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

  VOID CALLBACK TimerProc(LPVOID lpArg, DWORD dwTimerLowValue, DWORD dwTimerHighValue);
  

//------------------------------------------------------------------------------
// Global data
//------------------------------------------------------------------------------

struct capconfig
{
	int devnum;
	int adevnum;
	wchar_t *templocation;
	wchar_t *destination;
	wchar_t *stationid;
	int overlap;
	int periodicity;
	wchar_t* schedule;
	wchar_t *currentoutput;
	wchar_t *wcurrentoutput;
	int sleeptime;
	int bitrate;
	int enconding_type;
	int zerolatency;
	int framerate;
	int widthratio;
	int heightratio;
	int audiobitrate;
	int videowidth;
	int videoheight;

}capinfo;


/////////////////////////////////////////////////////////////////////////////
//
// CCApp
//
BEGIN_MESSAGE_MAP(AmCap, CWinApp)
    //{{AFX_MSG_MAP(CCCApp)
	// NOTE - the ClassWizard will add and remove mapping macros here.
	//    DO NOT EDIT what you see in these blocks of generated code!
    //}}AFX_MSG
    ON_COMMAND(ID_HELP, AmCap::OnHelp)
END_MESSAGE_MAP()


AmCap::AmCap()
{
    // TODO: add construction code here,
    // Place all significant initialization in InitInstance
}



/////////////////////////////////////////////////////////////////////////////
//
// The one and only CCCApp object
//
AmCap theApp;

void AmCap::readConfig(BSTR url) 
{
	HRESULT hr;
	IXMLDOMDocument *iXMLDoc;
	hr = CoCreateInstance(CLSID_DOMDocument, NULL, CLSCTX_ALL, __uuidof( IXMLDOMDocument ), ( void ** ) &iXMLDoc);


	// Load the file. 
	VARIANT_BOOL bSuccess=false;
	// Can load it from a url/filename...
//	VARIANT v = CComVariant(url);

	VARIANT vt;
	VariantInit(&vt); // initialize the variant
	vt.vt = VT_BSTR; // set to BSTR string
	vt.bstrVal = SysAllocString(url);

	iXMLDoc->load(vt,&bSuccess);
//	iXMLDoc->load(url,&bSuccess);
	// or from a BSTR...
	//iXMLDoc->loadXML(CComBSTR(s),&bSuccess);

	// Get a pointer to the root
	IXMLDOMElement *iRootElm;
	iXMLDoc->get_documentElement(&iRootElm);

	IXMLDOMNodeList *children;
	iRootElm->getElementsByTagName(L"CaptureChannel",&children);
	long i;
	
	hr = children->get_length(&i);
	if(SUCCEEDED(hr)) {
		children->reset();
		while(i--) {
			//grab the info that I care about
			IXMLDOMNode *node;		
			hr = children->get_item(i,&node);
			if(SUCCEEDED(hr)) {
				IXMLDOMNode *name;		
				hr = node->selectSingleNode(L"./name",&name); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *audioname;		
				hr = node->selectSingleNode(L"./audioname",&audioname); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *templocation;		
				hr = node->selectSingleNode(L"./templocation",&templocation); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *destination;		
				hr = node->selectSingleNode(L"./destination",&destination); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *channelid;		
				hr = node->selectSingleNode(L"./channelid",&channelid); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *schedule;		
				hr = node->selectSingleNode(L"./schedule",&schedule); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *periodicity;		
				hr = node->selectSingleNode(L"./periodicity",&periodicity); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *overlapnode;		
				hr = node->selectSingleNode(L"./overlap",&overlapnode); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *formatnode;		
				hr = node->selectSingleNode(L"./format",&formatnode); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *bitratenode;		
				hr = node->selectSingleNode(L"./bitrate",&bitratenode); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *enconding_typenode;		
				hr = node->selectSingleNode(L"./enconding_type",&enconding_typenode); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *zerolatencynode;		
				hr = node->selectSingleNode(L"./zerolatency",&zerolatencynode); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *frameratenode;		
				hr = node->selectSingleNode(L"./framerate",&frameratenode); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *audiobitratenode;		
				hr = node->selectSingleNode(L"./audidbitrate",&audiobitratenode); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *aspectrationode;		
				hr = node->selectSingleNode(L"./aspectratio",&aspectrationode); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *videowidthnode;		
				hr = node->selectSingleNode(L"./videowidth",&videowidthnode); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *videoheightnode;		
				hr = node->selectSingleNode(L"./videoheight",&videoheightnode); // uses xsl.  should expose getbytagname higher up

				// get the text for each of the childrem and away we go
				BSTR wnamestr,wanamestr,wlocstr,wdeststr,wchnstr,wsched,wperiod,woverlap,wformat,wlatency,wbitrate,wenconding_type,wframerate,waspectratio,waudiobitrate,wvideowidth,wvideoheight;
				char *namestr,*anamestr,*period,*overlap,*format,*zerolatency,*bitrate,*enconding_type,*framerate,*aspectratio,*audiobitrate,*videowidth,*videoheight;
				size_t convertedChars;

				if(name != 0)
				{
					name->get_text(&wnamestr); // ~devnum
					namestr = (char*)calloc(wcslen(wnamestr+1),sizeof(char));
					wcstombs_s(&convertedChars, namestr, wcslen(wnamestr) + 1, wnamestr, _TRUNCATE);
					capinfo.devnum =(int)atol(namestr);
					capodd = new Capture(capinfo.devnum);
				}
				else
				{
					exit(0);
				}
				
				if(audioname != 0)
				{
					audioname->get_text(&wanamestr); // ~devnum
					anamestr = (char*)calloc(wcslen(wanamestr+1),sizeof(char));
					wcstombs_s(&convertedChars, anamestr, wcslen(wanamestr) + 1, wanamestr, _TRUNCATE);
					capinfo.adevnum =(int)atol(anamestr);
					
				}
				else
				{
					capodd->LogMsg("Invalid Value for audioname in app.config");
					exit(0);
				}

				if(periodicity != 0)
				{
					periodicity->get_text(&wperiod);
					period = (char*)calloc(wcslen(wperiod+1),sizeof(char));
					wcstombs_s(&convertedChars, period, wcslen(wperiod) + 1, wperiod, _TRUNCATE);
					capinfo.periodicity = (int)atol(period);
				}
				else
				{
					capinfo.periodicity = 60;
				}

				if(templocation !=0)
				{
					templocation->get_text(&wlocstr);
					capinfo.templocation = (wchar_t*) wlocstr;
				}
				else
				{
					capodd->LogMsg("Invalid Value for templocation in app.config");
					exit(0);
				}
				
				if(destination !=0)
				{
					destination->get_text(&wdeststr);
					capinfo.destination =  (wchar_t*) wdeststr;
				}
				else
				{
					capodd->LogMsg("Invalid Value for destination in app.config");
					exit(0);
				}

				if(channelid !=0)
				{
					channelid->get_text(&wchnstr);
					capinfo.stationid = (wchar_t*) wchnstr;
				}
				else
				{
					capodd->LogMsg("Invalid Value for channelid in app.config");
					exit(0);
				}
				
				if(videowidthnode != 0)
				{
					videowidthnode->get_text(&wvideowidth);
					videowidth = (char*)calloc(wcslen(wvideowidth+1),sizeof(char));
					wcstombs_s(&convertedChars, videowidth, wcslen(wvideowidth) + 1, wvideowidth, _TRUNCATE);
					capinfo.videowidth = (int) atol(videowidth);
				}

				if(videoheightnode != 0)
				{
					videoheightnode->get_text(&wvideoheight);
					videoheight = (char*)calloc(wcslen(wvideoheight+1),sizeof(char));
					wcstombs_s(&convertedChars, videoheight, wcslen(wvideoheight) + 1, wvideoheight, _TRUNCATE);
					capinfo.videoheight = (int) atol(videoheight);
				}


				if(bitratenode !=0)
				{
					bitratenode->get_text(&wbitrate);
					bitrate = (char*)calloc(wcslen(wbitrate+1),sizeof(char));
					wcstombs_s(&convertedChars, bitrate, wcslen(wbitrate) + 1, wbitrate, _TRUNCATE);
					capinfo.bitrate = (int)atol(bitrate);
				}
				else
				{
					capinfo.bitrate =285;
				}
				
				if(enconding_typenode != 0)
				{
					enconding_typenode->get_text(&wenconding_type);
					enconding_type = (char*)calloc(wcslen(wenconding_type+1),sizeof(char));
					wcstombs_s(&convertedChars, enconding_type, wcslen(wenconding_type) + 1, wenconding_type, _TRUNCATE);
					capinfo.enconding_type = (int)atol(enconding_type);
				}
				else
				{
					capinfo.enconding_type =3;
				}
				
				if(zerolatencynode != 0)
				{
					zerolatencynode->get_text(&wlatency);
					zerolatency = (char*)calloc(wcslen(wlatency+1),sizeof(char));
					wcstombs_s(&convertedChars, zerolatency, wcslen(wlatency) + 1, wlatency, _TRUNCATE);
					capinfo.zerolatency = (int)atol(zerolatency);
				}
				else
				{
					capinfo.zerolatency = 1;
				}


				if(frameratenode != 0)
				{
					frameratenode->get_text(&wframerate);
					framerate = (char*)calloc(wcslen(wframerate+1),sizeof(char));
					wcstombs_s(&convertedChars, framerate, wcslen(wframerate) + 1, wframerate, _TRUNCATE);
					capinfo.framerate = (int)atol(framerate);
				}
				else
				{
					capinfo.framerate = 30;
				}

				if(aspectrationode !=0)
				{
					aspectrationode->get_text(&waspectratio);
					aspectratio = (char*)calloc(wcslen(waspectratio+1),sizeof(char));
					wcstombs_s(&convertedChars, aspectratio, wcslen(waspectratio) + 1, waspectratio, _TRUNCATE);
					char* pch = strtok(aspectratio,":");
					if(pch != NULL)
					{
						capinfo.widthratio = (int)atol(pch);
						char* pch1 = strtok(NULL,":");
						if(pch1 != NULL)
						{
							capinfo.heightratio = (int)atol(pch1);
						}
					}
				}
				else
				{
					capinfo.widthratio = 32;
					capinfo.heightratio = 27;
				}
				
				if(audiobitratenode != 0)
				{
					audiobitratenode->get_text(&waudiobitrate);
					audiobitrate = (char*)calloc(wcslen(waudiobitrate+1),sizeof(char));
					wcstombs_s(&convertedChars, audiobitrate, wcslen(waudiobitrate) + 1, waudiobitrate, _TRUNCATE);
					capinfo.audiobitrate = (int) atol(audiobitrate);	
				}
				else
				{
					capinfo.audiobitrate =128;
				}
			}
		}
	}
    // callback a function based on the type

	// Thanks to the magic of CComPtr, we never need call
	// Release() -- that gets done automatically.
}

BOOL AmCap::InitInstance()
{
  

    /* Call initialization procedure */
     CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);
	 

	int drive = _getdrive();
	size_t n;
	char loc[1024],loc1[8192];
	wchar_t loc2[8192];
	_getdcwd(drive,loc,sizeof(loc));
	sprintf_s(loc1,8192, "file://%s/app.config",loc);
	mbstowcs_s(&n,loc2,strlen(loc1) + 1,loc1,_TRUNCATE);
	readConfig(loc2);

	try
	{
		
		capodd->adevnum = capinfo.adevnum;
		capodd->destination = capinfo.destination;
		capodd->devnum = capinfo.devnum;
		capodd->overlap =capinfo.overlap;
		capodd->periodicity = capinfo.periodicity;
		capodd->schedule = capinfo.schedule;
		capodd->sleeptime =capinfo.sleeptime;
		capodd->stationid = capinfo.stationid;
		capodd->templocation = capinfo.templocation;
		capodd->FrameRate = capinfo.framerate;
		capodd->widthratio = capinfo.widthratio;
		capodd->heightratio = capinfo.heightratio;
		capodd->audiobitrate = capinfo.audiobitrate;
		capodd->Videowidth = capinfo.videowidth;
		capodd->Videoheight = capinfo.videoheight;

		if(!(60 % capinfo.periodicity  == 0))
		{
			capodd->LogMsg("Invalid periodicity , periodicity should be divisible to 60");
			exit(0);
		}
	
		CONFIG *MyX264Config = new CONFIG();
		MyX264Config->b_zerolatency =capinfo.zerolatency;;
		MyX264Config->i_encoding_type =capinfo.enconding_type;
		MyX264Config->i_passbitrate = capinfo.bitrate;
		MyX264Config->i_pass =1;
		MyX264Config->i_sar_width = capinfo.widthratio;
		MyX264Config->i_sar_height = capinfo.heightratio;
		capodd->x264vfw_config_reg_save(MyX264Config);
		capodd->AddDevicesToMenu();
		capodd->InitCapFilters();
		
		RunInstance();

		return TRUE;
		
	}
	catch(EXCEPINFO e)
	{
		char *msg=new char[4000];
		sprintf(msg,"error:%s",e.bstrDescription);
		capodd->LogMsg(msg);
	}
}

VOID AmCap::RunInstance()
{
	try
	{
		while(1)
		{
			capodd->BuildMp4CaptureGrapth();
			capodd->StartCapture();
			
			time_t cutime;		
			HANDLE hTimer = NULL;
			LARGE_INTEGER liDueTime;
			liDueTime.QuadPart=-100000000LL;
			liDueTime.QuadPart=-10000000;
			// Create a waitable timer.
			hTimer = CreateWaitableTimer(NULL, FALSE, capodd->stationid);
		
			SYSTEMTIME st1;
			GetSystemTime(&st1);
		

			LONGLONG curseconds = (st1.wMinute)*60*1000 + (st1.wSecond)*1000 + st1.wMilliseconds;
			LONGLONG period = (capodd->periodicity*60)*1000;
			LONGLONG remainingDuration = (curseconds % period);
			LONGLONG elaspedDuration = (period - remainingDuration);
		

			// localtime is now the deired next time to wake up, so it is either the end or beginning of the period, so set the 
			// shared object information to be this correct value.  At the end of the hour I dont really care so much
			// this should be pretty damn close to the periodicity number.  I really want to just sleep for the period , wake up, an tell it to start
		
			liDueTime.QuadPart=-(elaspedDuration*10000);

			if (!SetWaitableTimer(hTimer, &liDueTime, 0, (PTIMERAPCROUTINE)TimerProc, this, 0))
			{
			}

			if (WaitForSingleObject(hTimer,elaspedDuration) != WAIT_OBJECT_0) {	
			}

			capodd->StopCapture();
			capodd->FreeCapFilters();

			wchar_t *sourceFile = new wchar_t[8912];
			wchar_t *destFile = new wchar_t[8912];

			wchar_t *sourceCCFile = new wchar_t[8912];
			wchar_t *destCCFile = new wchar_t[8912];

			swprintf_s(sourceFile,8912,L"%s\\%s.mp4",capodd->templocation,capodd->wcurrentoutput);
			swprintf_s(destFile,8912,L"%s\\%s.mp4",capodd->destination,capodd->wcurrentoutput);

			swprintf_s(sourceCCFile,8912,L"%s\\%s.txt",capodd->templocation,capodd->wcurrentoutput);
			swprintf_s(destCCFile,8912,L"%s\\%s.txt",capodd->destination,capodd->wcurrentoutput);

			if(!MoveFile(sourceFile,destFile))
			{
				fprintf(capodd->c_pF,"Error on Moving Capture File %ls",sourceFile);
			}
			if(!MoveFile(sourceCCFile,destCCFile))
			{
				fprintf(capodd->c_pF,"Error on Moving Capture CC File %ls",sourceCCFile);
			}
		}
	}
	catch(EXCEPINFO e)
	{
		char *msg=new char[4000];
		sprintf(msg,"error:%s",e.bstrDescription);
		capodd->LogMsg(msg);
	}
	
	
}

VOID CALLBACK TimerProc(LPVOID lpArg, DWORD dwTimerLowValue, DWORD dwTimerHighValue)
{
	//AmCap* thisobj = (AmCap*)lpArg;
	//thisobj->capodd->StopCapture();
	//thisobj->capodd->FreeCapFilters();
	//thisobj->RunInstance();
}





