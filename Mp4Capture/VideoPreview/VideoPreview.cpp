// WinRunGraphFromFile.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "VideoPreview.h"
#include "dshowutil.h"
#include <direct.h>

#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name

// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);
void LogMsg(char* szBuffer);
void readConfig(BSTR url);

struct previewconfig
{
	wchar_t* graphlocation;
	wchar_t* logfilelocation;
	wchar_t* windowtitle;

}previewinfo;

int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

 	// TODO: Place code here.
	MSG msg;
	HACCEL hAccelTable;

	// Initialize global strings
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_WINRUNGRAPHFROMFILE, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance (hInstance, nCmdShow))
	{
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_WINRUNGRAPHFROMFILE));

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int) msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
//  COMMENTS:
//
//    This function and its usage are only necessary if you want this code
//    to be compatible with Win32 systems prior to the 'RegisterClassEx'
//    function that was added to Windows 95. It is important to call this function
//    so that the application will get 'well formed' small icons associated
//    with it.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_WINRUNGRAPHFROMFILE));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_WINRUNGRAPHFROMFILE);
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}


HANDLE gDoneEvent;
VOID CALLBACK TimerRoutine(PVOID lpParam, BOOLEAN TimerOrWaitFired)
{
    
	
    if(TimerOrWaitFired)
    {
        //printf("The wait timed out.\n");
    }
    else
    {
        //printf("The wait event was signaled.\n");
    }
    SetEvent(gDoneEvent);
	//RunCapture();

	
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	HWND hWnd;

	 hInst = hInstance; // Store instance handle in our global variable

	 CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);

	__try
	{
		int drive = _getdrive();
		size_t n;
		char loc[1024],loc1[8192];
		wchar_t loc2[8192];
		_getdcwd(drive,loc,sizeof(loc));
		sprintf_s(loc1,8192, "file://%s/app.config",loc);
		mbstowcs_s(&n,loc2,strlen(loc1) + 1,loc1,_TRUNCATE);
		readConfig(loc2);


		hWnd = CreateWindow(szWindowClass, previewinfo.windowtitle, WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, 0, 640, 480, NULL, NULL, hInstance, NULL);
   
		

		if (!hWnd)
		{
			return FALSE;
		}

		ShowWindow(hWnd, nCmdShow);
		UpdateWindow(hWnd);
	

		IGraphBuilder *pFg;
		IMediaControl *pControl = NULL;
		IMediaEvent *pEvent = NULL;
		IVideoWindow *pVW=NULL; 

		HRESULT hr = CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC,
										IID_IGraphBuilder, (LPVOID *)&pFg);

		if(hr == NOERROR) 
		{
			hr = LoadGraphFile(pFg,previewinfo.graphlocation);

			hr = pFg->QueryInterface(IID_IVideoWindow, reinterpret_cast <void **> (&pVW));
			pVW->put_Owner((OAHWND)hWnd);    // We own the window now
			pVW->put_WindowStyle(WS_CHILD|WS_CLIPSIBLINGS);    // you are now a child
			RECT rect;
			GetWindowRect(hWnd,&rect);
			pVW->SetWindowPosition(0, 0,rect.right - rect.left,rect.bottom - rect.top);

			pVW->put_Visible(OATRUE);
			pVW->SetWindowForeground(OATRUE);

			hr = pFg->QueryInterface(IID_IMediaControl, reinterpret_cast <void **> (&pControl));
			if (FAILED (hr))
			{
				LogMsg("Media control couldn't be retrieved");
				MessageBox(hWnd,L"Media control couldn't be retrieved",L"Error",NULL);
				return FALSE;
			}
  
			hr = pFg->QueryInterface(IID_IMediaEvent, reinterpret_cast <void **> (&pEvent));
			if (FAILED (hr))
			{
				LogMsg("Media event couldn't be retrieved");
				MessageBox(hWnd,L"Media event couldn't be retrieved",L"Error",NULL);
				return FALSE;
			}
		
			hr = pControl->Run();
			if (FAILED (hr))
			{
				LogMsg("Graph couldn't be started");
				MessageBox(hWnd,L"Graph couldn't be started",L"Error",NULL);
				return FALSE;
			}
			
		}
		else
		{
			return FALSE;
		}
	}
	__except(EXCEPTION_EXECUTE_HANDLER) // Here is exception filter expression
	{  
		// Here is exception handler
		LogMsg("exception occures");
		// Terminate program
		ExitProcess(1);
		return FALSE;
	}
   return TRUE;
}

void readConfig(BSTR url) 
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
	iRootElm->getElementsByTagName(L"PreviewChannel",&children);
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
				hr = node->selectSingleNode(L"./graphlocation",&name); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *logfilelocationnode;		
				hr = node->selectSingleNode(L"./logfilelocation",&logfilelocationnode); // uses xsl.  should expose getbytagname higher up
				IXMLDOMNode *windowtitlenode;		
				hr = node->selectSingleNode(L"./windowtitle",&windowtitlenode); // uses xsl.  should expose getbytagname higher up
				
				// get the text for each of the childrem and away we go
				BSTR wnamestr,wloglocationstr,wwindowtitle;

				if(logfilelocationnode !=0)
				{
					logfilelocationnode->get_text(&wloglocationstr);
					previewinfo.logfilelocation =  (wchar_t*) wloglocationstr;
					if (GetFileAttributes(previewinfo.logfilelocation) == INVALID_FILE_ATTRIBUTES) {
						CreateDirectory(previewinfo.logfilelocation,NULL);
					}
					char *LogFile = new char[500];
					SYSTEMTIME st;
					GetSystemTime(&st);
					sprintf_s(LogFile,500,"%ls\\Preview_Log_%d%.2d%.2d.txt",previewinfo.logfilelocation,st.wYear,st.wMonth,st.wDay);
					errno_t err = fopen_s(&c_pF,LogFile, "a+");

				}
				else
				{
					char *LogFile = new char[500];
					SYSTEMTIME st;
					GetSystemTime(&st);
					sprintf_s(LogFile,500,"Preview_Log_%d%.2d%.2d.txt",st.wYear,st.wMonth,st.wDay);
					errno_t err = fopen_s(&c_pF,LogFile, "a+");
				}

				if(name != 0)
				{
					name->get_text(&wnamestr); // ~devnum
					previewinfo.graphlocation =(wchar_t*)wnamestr;
					if (GetFileAttributes(previewinfo.graphlocation) == INVALID_FILE_ATTRIBUTES) 
					{
						LogMsg("Graph file doesn't exit at config file location.");
						MessageBox(GetActiveWindow(),L"Graph file doesn't exit at config file location.",L"Error",NULL);
						exit(0);
					}
				}
				else
				{
					exit(0);
				}			


				if(windowtitlenode != 0)
				{
					windowtitlenode->get_text(&wwindowtitle); // ~devnum
					previewinfo.windowtitle =(wchar_t*)wwindowtitle;
				}
				else
				{
					previewinfo.windowtitle = szTitle;
				}	
			}
		}
	}
}

/*----------------------------------------------------------------------------*\
|   LogMsg - Opens a Message box with a error message in it.  The user can     |
|            select the OK button to continue                                  |
\*----------------------------------------------------------------------------*/
void LogMsg(char* szBuffer)
{
    SYSTEMTIME st1;
	GetSystemTime(&st1);
	fprintf(c_pF,"[%d-%2d-%2d %2d:%2d:%2d,%d] - %s\n",st1.wYear,st1.wMonth,st1.wDay,st1.wHour,st1.wMinute,st1.wSecond,st1.wMilliseconds, szBuffer);
	fflush(c_pF);
}


    // callback a function based on the type

	// Thanks to the magic of CComPtr, we never need call
	// Release() -- that gets done automatically.

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;

	switch (message)
	{
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		// TODO: Add any drawing code here...
		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}