//------------------------------------------------------------------------------
// File: AMCap.cpp
//
// Desc: Audio/Video Capture sample for DirectShow
//
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

#include "stdafx.h"
#include "Mp4Capture.h"


#ifdef _DEBUG
  #define new DEBUG_NEW
  #undef THIS_FILE
  static char THIS_FILE[] = __FILE__;
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
	int periodicity;
	wchar_t *currentoutput;
	wchar_t *wcurrentoutput;
	int bitrate;
	int enconding_type;
	int zerolatency;
	int framerate;
	int widthratio;
	int heightratio;
	int audiobitrate;
	int videowidth;
	int videoheight;
	wchar_t* logfilelocation;
	

}capinfo;


/////////////////////////////////////////////////////////////////////////////
//
// Mp4Capture
//
//BEGIN_MESSAGE_MAP(Mp4Capture, CWinApp)
//    //{{AFX_MSG_MAP(CCCApp)
//	// NOTE - the ClassWizard will add and remove mapping macros here.
//	//    DO NOT EDIT what you see in these blocks of generated code!
//    //}}AFX_MSG
//    //ON_COMMAND(ID_HELP, AmCap::OnHelp)
//END_MESSAGE_MAP()


Mp4Capture::Mp4Capture()
{
    // TODO: add construction code here,
    // Place all significant initialization in InitInstance
}



/////////////////////////////////////////////////////////////////////////////
//
// The one and only CCCApp object
//
Mp4Capture theApp;

/////////////////////////////////////////////////////////////////////////////
//
// Method to Read the Config Parameters and Assign to capinfo object
//
void Mp4Capture::readConfig(BSTR url) 
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
	
	// Get a pointer to the root
	IXMLDOMElement *iRootElm;
	iXMLDoc->get_documentElement(&iRootElm);

	// Read the CaptureChannel Node
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

				// get child nodes by tagname 
				IXMLDOMNode *videoname;		
				hr = node->selectSingleNode(L"./name",&videoname); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *audioname;		
				hr = node->selectSingleNode(L"./audioname",&audioname); 
				IXMLDOMNode *templocation;		
				hr = node->selectSingleNode(L"./templocation",&templocation); 
				IXMLDOMNode *destination;		
				hr = node->selectSingleNode(L"./destination",&destination); 
				IXMLDOMNode *channelid;		
				hr = node->selectSingleNode(L"./channelid",&channelid); 
				IXMLDOMNode *schedule;		
				hr = node->selectSingleNode(L"./schedule",&schedule); 
				IXMLDOMNode *periodicity;		
				hr = node->selectSingleNode(L"./periodicity",&periodicity); 
				IXMLDOMNode *overlapnode;		
				hr = node->selectSingleNode(L"./overlap",&overlapnode); 
				IXMLDOMNode *formatnode;		
				hr = node->selectSingleNode(L"./format",&formatnode); 
				IXMLDOMNode *bitratenode;		
				hr = node->selectSingleNode(L"./bitrate",&bitratenode); 
				IXMLDOMNode *enconding_typenode;		
				hr = node->selectSingleNode(L"./enconding_type",&enconding_typenode); 
				IXMLDOMNode *zerolatencynode;		
				hr = node->selectSingleNode(L"./zerolatency",&zerolatencynode); 
				IXMLDOMNode *frameratenode;		
				hr = node->selectSingleNode(L"./framerate",&frameratenode); 
				IXMLDOMNode *audiobitratenode;		
				hr = node->selectSingleNode(L"./audidbitrate",&audiobitratenode); 
				IXMLDOMNode *aspectrationode;		
				hr = node->selectSingleNode(L"./aspectratio",&aspectrationode); 
				IXMLDOMNode *videowidthnode;		
				hr = node->selectSingleNode(L"./videowidth",&videowidthnode); 
				IXMLDOMNode *videoheightnode;		
				hr = node->selectSingleNode(L"./videoheight",&videoheightnode); 
				IXMLDOMNode *logfilelocationnode;		
				hr = node->selectSingleNode(L"./logfilelocation",&logfilelocationnode); 
				
				
				

				// get the text for each of the childrem and away we go
				BSTR wnamestr,wanamestr,wlocstr,wdeststr,wchnstr,wperiod,wlatency,wbitrate,wenconding_type,wframerate,waspectratio,waudiobitrate,wvideowidth,wvideoheight,wloglocationstr;
				char *namestr,*anamestr,*period,*zerolatency,*bitrate,*enconding_type,*framerate,*aspectratio,*audiobitrate,*videowidth,*videoheight;
				size_t convertedChars;

				// read logfile location for directory path where logfile will be created
				if(logfilelocationnode !=0)
				{
					logfilelocationnode->get_text(&wloglocationstr);
					capinfo.logfilelocation =  (wchar_t*) wloglocationstr;
					
					// if directoryt does not exist then create the directory
					if (GetFileAttributes(capinfo.logfilelocation) == INVALID_FILE_ATTRIBUTES) {
						CreateDirectory(capinfo.logfilelocation,NULL);
					}
				}
				else
				{
					exit(0);
				}

				// read video index (name tag of config) indicates opsrey device channel to make capture program for 
				if(videoname != 0)
				{
					videoname->get_text(&wnamestr); // ~devnum
					namestr = (char*)calloc(wcslen(wnamestr+1),sizeof(char));
					wcstombs_s(&convertedChars, namestr, wcslen(wnamestr) + 1, wnamestr, _TRUNCATE);
					capinfo.devnum =(int)atol(namestr);

					// we will init capture object here , bcoz we use device index to create logfile 
					capInstance = new Capture(capinfo.devnum,capinfo.logfilelocation);
				}
				else
				{
					exit(0);
				}
				
				// read audio index (audioname tag of config) indicates opsrey audio device channel 
				if(audioname != 0)
				{
					audioname->get_text(&wanamestr); // ~devnum
					anamestr = (char*)calloc(wcslen(wanamestr+1),sizeof(char));
					wcstombs_s(&convertedChars, anamestr, wcslen(wanamestr) + 1, wanamestr, _TRUNCATE);
					capinfo.adevnum =(int)atol(anamestr);
					
				}
				else
				{
					capInstance->LogMsg("missing audioname in app.config");
					exit(0);
				}

				// periodicity tag of config indicates the time period (in minutes) the capture duration
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

				// templocation tag indicates the directory path where our capture files will be processed. and later moved.
				if(templocation !=0)
				{
					templocation->get_text(&wlocstr);
					capinfo.templocation = (wchar_t*) wlocstr;
					if (GetFileAttributes(capinfo.templocation) == INVALID_FILE_ATTRIBUTES) {
						capInstance->LogMsg("directory for templocation does not exist physically.");
						exit(0);
					}

				}
				else
				{
					capInstance->LogMsg("missing templocation in app.config");
					exit(0);
				}
				
				// destinationlocation tag indicates the directory path where our capture files will be moved after all processing done
				if(destination !=0)
				{
					destination->get_text(&wdeststr);
					capinfo.destination =  (wchar_t*) wdeststr;
					if (GetFileAttributes(capinfo.destination) == INVALID_FILE_ATTRIBUTES) {
						capInstance->LogMsg("directory for destination location does not exist physically.");
						exit(0);
					}
				}
				else
				{
					capInstance->LogMsg("missing destination in app.config");
					exit(0);
				}

				// stationid tag just used for file naming of capture program.
				if(channelid !=0)
				{
					channelid->get_text(&wchnstr);
					capinfo.stationid = (wchar_t*) wchnstr;
				}
				else
				{
					capInstance->LogMsg("missing channelid in app.config");
					exit(0);
				}
				
				// videowidth node used for setting width for output capture file. if provided otherwise use default 
				if(videowidthnode != 0)
				{
					videowidthnode->get_text(&wvideowidth);
					videowidth = (char*)calloc(wcslen(wvideowidth+1),sizeof(char));
					wcstombs_s(&convertedChars, videowidth, wcslen(wvideowidth) + 1, wvideowidth, _TRUNCATE);
					capinfo.videowidth = (int) atol(videowidth);
				}

				// videoheight node used for setting height for output capture file. if provided otherwise use default 
				if(videoheightnode != 0)
				{
					videoheightnode->get_text(&wvideoheight);
					videoheight = (char*)calloc(wcslen(wvideoheight+1),sizeof(char));
					wcstombs_s(&convertedChars, videoheight, wcslen(wvideoheight) + 1, wvideoheight, _TRUNCATE);
					capinfo.videoheight = (int) atol(videoheight);
				}

				// bitrate node used for setting bitrate for output file (setting will be applied to x264vfw codec filter)
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
				

				//enconding_typenode node used for encoding_type for output file (setting will be applied to x264vfw codec filter). more information is in app.config
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
				
				// zerolatency is setting for x264vfw codec filter (0 : False , 1 : True)
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

				// framerate node is used for set framerate for output file. (will use 30 default if not provided)
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

				// aspect ratio for output file (x264vfw settings)
				if(aspectrationode !=0)
				{
					aspectrationode->get_text(&waspectratio);
					aspectratio = (char*)calloc(wcslen(waspectratio+1),sizeof(char));
					wcstombs_s(&convertedChars, aspectratio, wcslen(waspectratio) + 1, waspectratio, _TRUNCATE);
					char* next_token = 0;
					char* pch = strtok_s(aspectratio,":",&next_token);
					if(pch != NULL)
					{
						capinfo.widthratio = (int)atol(pch);
						char* pch1 = strtok_s(NULL,":",&next_token);
						if(pch1 != NULL)
						{
							capinfo.heightratio = (int)atol(pch1);
						}
					}
				}
				else
				{
					capinfo.widthratio = 2;
					capinfo.heightratio = 1;
				}
				
				// bitrate for audio setting output file (this settings is applied to Monogram AAC Encoder filter)
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
}

// this is start up point for the application
BOOL Mp4Capture::InitInstance()
{
  

    /* Call initialization procedure */
     CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);
	 

	// get the directory location for application and read app.config file on application directory location
	int drive = _getdrive();
	size_t n;
	char loc[1024],loc1[8192];
	wchar_t loc2[8192];
	_getdcwd(drive,loc,sizeof(loc));
	sprintf_s(loc1,8192, "file://%s/app.config",loc);
	mbstowcs_s(&n,loc2,strlen(loc1) + 1,loc1,_TRUNCATE);
	readConfig(loc2);

	TRY
	{
		// we have read all the config params , now we will assign them to our capture object to use them with capture class methods. 
		capInstance->adevnum = capinfo.adevnum;
		capInstance->destination = capinfo.destination;
		capInstance->devnum = capinfo.devnum;

		capInstance->periodicity = capinfo.periodicity;

		capInstance->stationid = capinfo.stationid;
		capInstance->templocation = capinfo.templocation;
		capInstance->FrameRate = capinfo.framerate;
		capInstance->audiobitrate = capinfo.audiobitrate;
		capInstance->Videowidth = capinfo.videowidth;
		capInstance->Videoheight = capinfo.videoheight;

		// we need to make check that our periodicity should be divisible to 60 (e.g. 2, 3, 5 , 10 etc. ). value like 3.5 , 7 , 9 not allowed
		if(!(60 % capinfo.periodicity  == 0))
		{
			capInstance->LogMsg("Invalid periodicity , periodicity should be divisible to 60");
			exit(0);
		}


		/// this we have used for preview , currently we dont need any window.
		//capInstance->ShowWindow(SW_SHOW);
		//capInstance->UpdateWindow();
	

		// We need to set settings for x264vfw filter programtically 
		// (we have included x264vfwConfig.h header file for all its required parameters)
		
		// this will set the config object for x264vfw settings
		CONFIG *X264Config = new CONFIG();
		X264Config->b_zerolatency =capinfo.zerolatency;
		X264Config->i_encoding_type =capinfo.enconding_type;
		X264Config->i_passbitrate = capinfo.bitrate;
		X264Config->i_pass =1;
		X264Config->i_sar_width = capinfo.widthratio;
		X264Config->i_sar_height = capinfo.heightratio;
		
		// now we'll save above config settings for x264vfw codec , by settings values in registery 
		capInstance->x264vfw_config_reg_save(X264Config);
		
		// add video , audio devices , here we also get x264vfw filter 
		capInstance->GetCaptureDevices();
		
		// now we'll initialize Graph objects 
		// (Graph Builder Object, Filter Graph Manager Object, and other filters needed for our mp4capture graph)
		capInstance->InitCapFilters();
		
		RunInstance();

		return TRUE;
		
	}
	CATCH(CException,ex)
	{
	  char *msg  = new char[301];
	  TCHAR   szCause[255];
	  ex->GetErrorMessage(szCause,255);
	  sprintf_s(msg ,255,"error:%ls",szCause);
	  capInstance->LogMsg(msg);

	  return FALSE;
	}
	END_CATCH  

	
}

VOID Mp4Capture::RunInstance()
{
	TRY
	{
		while(1)
		{
			// we have intialized all filter objects for graph in InitCapFilters() method
			// not we'll add all this filters in graph and connect them.
			capInstance->BuildMp4CaptureGraph();
			
			// now, our graph ready to run , 
			// StartCapture will start capturing video and their closed captions
			capInstance->StartCapture();

			
			HANDLE hTimer = NULL;
			LARGE_INTEGER liDueTime;
			liDueTime.QuadPart=-100000000LL;
			liDueTime.QuadPart=-10000000;
			
			// Create a waitable timer.
			hTimer = CreateWaitableTimer(NULL, FALSE, capInstance->stationid);
		
			// if our waiting time periodicity is 60 minutes, and for e.g current time is 1:20 PM , 
			// then first video will be of 40 minutes only (from 1:20 PM to 2:00 PM)
			// we'll set timer duration like this , below is code for that. 
			SYSTEMTIME st1;
			GetSystemTime(&st1);
			LONGLONG curmsseconds = (st1.wMinute)*60*1000 + (st1.wSecond)*1000 + st1.wMilliseconds;
			LONGLONG period = (capInstance->periodicity*60)*1000;
			LONGLONG remainingDuration = (curmsseconds % period);
			LONGLONG elaspedDuration = (period - remainingDuration);

			
			// I really want to just sleep for the period , wake up, an tell it to start
			liDueTime.QuadPart=-(elaspedDuration*10000);

			// set waitable timer to duration set above.
			if(!SetWaitableTimer(hTimer, &liDueTime, 0, (PTIMERAPCROUTINE)TimerProc, this, 0))
			{
				capInstance->LogMsg("can not set waitable timer");
			}

			// this will wait until duration is elapsed.
			if(WaitForSingleObject(hTimer,(DWORD)elaspedDuration) != WAIT_OBJECT_0) {	
				capInstance->LogMsg("waitable timer waiting failed");
			}
			
			// after time competed , we'll stop capture
			capInstance->StopCapture();
			
			// remove all filters from graph
			capInstance->FreeCapFilters();

			wchar_t *sourceFile = new wchar_t[8912];
			wchar_t *destFile = new wchar_t[8912];

			wchar_t *sourceCCFile = new wchar_t[8912];
			wchar_t *destCCFile = new wchar_t[8912];

			swprintf_s(sourceFile,8912,L"%s\\%s.mp4",capInstance->templocation,capInstance->wcurrentoutput);
			swprintf_s(destFile,8912,L"%s\\%s.mp4",capInstance->destination,capInstance->wcurrentoutput);

			swprintf_s(sourceCCFile,8912,L"%s\\%s.txt",capInstance->templocation,capInstance->wcurrentoutput);
			swprintf_s(destCCFile,8912,L"%s\\%s.txt",capInstance->destination,capInstance->wcurrentoutput);

			// now we'll move .mp4 file to destination location
			if(!MoveFile(sourceFile,destFile))
			{
				char *msg  = new char[1000];
				sprintf_s(msg ,1000,"Error on Moving Capture File %ls\n",sourceFile);
				capInstance->LogMsg(msg);
			}

			// now we'll move cc text file to destination location
			if(!MoveFile(sourceCCFile,destCCFile))
			{
			    char *msg  = new char[1000];
				sprintf_s(msg ,1000,"Error on Moving Capture CC File %ls\n",sourceCCFile);
				capInstance->LogMsg(msg);
			}
		}
	}
	CATCH(CException,ex)
	{
	  char *msg  = new char[301];
	  TCHAR   szCause[255];
	  ex->GetErrorMessage(szCause,255);
	  sprintf_s(msg ,255,"error:%ls",szCause);
	  capInstance->LogMsg(msg);
	}
	END_CATCH 
	
	
}

VOID CALLBACK TimerProc(LPVOID lpArg, DWORD dwTimerLowValue, DWORD dwTimerHighValue)
{
}





