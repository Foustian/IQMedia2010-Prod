/////////////////////////////////////////////////////////////////////////////
//
// line.h - CC line callback object
//
#if !defined(AFX_LINE1_H__05F69B21_87C2_11D3_876D_006008A98EB7__INCLUDED_)
     #define AFX_LINE1_H__05F69B21_87C2_11D3_876D_006008A98EB7__INCLUDED_

#define IDD_CC_LINE                     129

#if _MSC_VER > 1000
  #pragma once
#endif // _MSC_VER > 1000

#include "o200avsCom.h"
#include "ICCLineInterp.h"
#include <afxwin.h>



#define WM_CALLBACK (WM_USER + 0x100)

#define LINELINES   24
#define TIMEDIM     15  // "00:00:00:000:  " = 15
#define LINECHARS   32
#define LINEDIM     (TIMEDIM + LINECHARS + 2)



/////////////////////////////////////////////////////////////////////////////
//
class CLine 
{
// Construction
public:
  CLine(FILE *c_pF);   // standard constructor

    enum { IDD = IDD_CC_LINE };
    protected:
	virtual void    DoDataExchange(CDataExchange* pDX);    


protected:
  
public:
	void    Init      (IReferenceClock* pClock,REFERENCE_TIME lCapStartTime,char *m_uPacTitle,UINT uButton, ULONG ulFlags, wchar_t* stationid);
  BOOL    Alloc     ();
  BOOL    SetTitle  (char* pcTitle, wchar_t* stationid);
  void    Term      ();
  HRESULT Open      ();
  HRESULT Close     ();
  void    CCCallback(CCLINE_DATA_S* pCCLineData);
  void CLine::ReInit(IReferenceClock* pClock,REFERENCE_TIME lCapStartTime,char *m_uPacTitle, wchar_t* stationid);
  void	  SetLine();
  void LogMsg(char* szBuffer);

  //CCCDlg* m_pDlg;
  ICCLineInterp*  m_pICCLineInterp;
  ULONG           m_ulDeviceId;
  UINT    m_uButton;
  UINT    m_uEditBox;
  ULONG   m_ulFlags;
  int     m_iLines;
  int     m_iLineChars;
  int     m_iLineDim;
  ULONG   m_ulChannelId;  // from CCLineInterp.ax
  char*   m_pc;           // the text buffer
  char*   m_pcTime;       // current time header - at last line of text buffer
  char*   m_pcLine;       // current line buffer - on last line of text buffer
  REFERENCE_TIME StartTime;
  REFERENCE_TIME EndTime;
  char*   pacTitle; 
  FILE* m_pF;
  FILE* m_CappF;
  IReferenceClock *m_pClock;
  REFERENCE_TIME m_lCapStartTime;
  BOOL IsWriteToFile;
  char *remainingCC;
  int IsInitated;
};


//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_LINE1_H__05F69B21_87C2_11D3_876D_006008A98EB7__INCLUDED_)
