//------------------------------------------------------------------------------
// File: AMCap.h
//
// Desc: DirectShow sample code - audio/video capture.
//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------


// Macros
#ifndef SAFE_RELEASE
#define SAFE_RELEASE(x) { if (x) x->Release(); x = NULL; }
#endif


#if !defined(AFX_CCAPP2_H__05F69B17_87C2_11D3_876D_006008A98EB7__INCLUDED_)
     #define AFX_CCAPP2_H__05F69B17_87C2_11D3_876D_006008A98EB7__INCLUDED_

#if _MSC_VER > 1000
  #pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
  #error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"
#include <time.h>
#include "Capture.h"

/////////////////////////////////////////////////////////////////////////////
//
// CCApp:
// See CCApp.cpp for the implementation of this class
//
class Mp4Capture : CWinApp
{

public:
  Mp4Capture();

// Overrides
  // ClassWizard generated virtual function overrides
  //{{AFX_VIRTUAL(CCCApp)
      public:
      virtual BOOL InitInstance();
	  VOID RunInstance();
	  void readConfig(BSTR url);
	 // VOID CALLBACK TimerProc(LPVOID lpArg, DWORD dwTimerLowValue, DWORD dwTimerHighValue);
  //}}AFX_VIRTUAL

// Implementation
public :
	HANDLE gDoneEvent;
	Capture *capInstance;

  //{{AFX_MSG(CCCApp)
      // NOTE - the ClassWizard will add and remove member functions here.
      //    DO NOT EDIT what you see in these blocks of generated code !
  //}}AFX_MSG
  //DECLARE_MESSAGE_MAP()

};


/////////////////////////////////////////////////////////////////////////////
//
//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.
//
#endif // !defined(AFX_CCAPP2_H__05F69B17_87C2_11D3_876D_006008A98EB7__INCLUDED_)