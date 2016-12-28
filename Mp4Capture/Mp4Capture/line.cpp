/////////////////////////////////////////////////////////////////////////////
//
// line.cpp - CC line callback object
//
#include "stdafx.h"
#include <string.h>
#include <direct.h>
#include "resource.h"
#include "line.h"



#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


int iscloded=0;

/////////////////////////////////////////////////////////////////////////////
//
CLine::CLine(FILE *c_pF) //: CDialog(CLine::IDD, pParent)
{
	m_CappF =c_pF;
	LogMsg("CCLine Constucted");
  m_pICCLineInterp = NULL;
  m_ulDeviceId= 0;
  IsWriteToFile =0;
  IsInitated=0;
  m_ulFlags     = 0;
  m_iLines      = LINELINES;
  m_iLineChars  = LINECHARS;
  m_iLineDim    = LINEDIM;
  m_ulChannelId = 0;
  Alloc();
  
  //{{AFX_DATA_INIT(CLine)
    // NOTE: the ClassWizard will add member initialization here
  //}}AFX_DATA_INIT
}

void CLine::DoDataExchange(CDataExchange* pDX)
{
  //CDialog::DoDataExchange(pDX);
  //{{AFX_DATA_MAP(CLine)
    // NOTE: the ClassWizard will add DDX and DDV calls here
  //}}AFX_DATA_MAP
}

//BEGIN_MESSAGE_MAP(CLine, CDialog)
//{{AFX_MSG_MAP(CLine)
  //ON_MESSAGE(WM_CALLBACK, OnCallback)
  //ON_WM_DESTROY()
  //ON_WM_CLOSE()
//}}AFX_MSG_MAP
//END_MESSAGE_MAP()






/////////////////////////////////////////////////////////////////////////////
//
void CLine::Init(IReferenceClock* pClock,REFERENCE_TIME lCapStartTime,char *m_uPacTitle,UINT uButton, ULONG ulFlags, wchar_t* stationid)
{ 
 LogMsg("CCLine Init");
	pacTitle =	  m_uPacTitle;
  m_lCapStartTime =lCapStartTime;
  m_pClock =		pClock;
  StartTime=0;
  EndTime=0;
  // after first video capture starts , we have to open CC channel , 
  // on further captures, as CC Channel is already open
  // we just intilize few variables and create new text file
  // create and open the CC file in append mode.
	if (SetTitle(pacTitle, stationid))
    {
		// open the CCChannel
		if (Open() == S_OK)
		{
			IsInitated =1;
			return;
		}
		return;
    } 
}

void CLine::ReInit(IReferenceClock* pClock,REFERENCE_TIME lCapStartTime,char *m_uPacTitle, wchar_t* stationid)
{ 
	// now CC Channel is already open , so we just need to reset below variable on next capture. 
	LogMsg("CCLine ReInit");
  pacTitle =	  m_uPacTitle;
  m_lCapStartTime =lCapStartTime;
  m_pClock =		pClock;
  StartTime=0;
  EndTime=0;
  
	if (SetTitle(pacTitle, stationid))
    {
		LogMsg("SetTitle Succeed");
		return;
    } 
}


/////////////////////////////////////////////////////////////////////////////
//
// Allocate and initialize the CC text buffer:
//
BOOL CLine::Alloc()
{

 LogMsg("CCLine Alloc");
  m_pc = new char[(m_iLines * m_iLineDim) + 1];
  if (m_pc == NULL)
    return FALSE;

  for (int iV = 0; iV < m_iLines; iV++)
  {
    char *pc2 = &m_pc[iV * m_iLineDim];
    for (int iH = 0; iH < m_iLineDim - 2; iH++)
      pc2[iH] = ' ';
    pc2[m_iLineDim - 2] = '\r';
    pc2[m_iLineDim - 1] = '\n';
  }
  m_pcTime = &m_pc    [(m_iLines - 1) * m_iLineDim];
  m_pcLine = &m_pcTime[TIMEDIM];
  m_pcTime[m_iLineDim] = '\0';

  remainingCC = NULL;
  return TRUE;
}


/////////////////////////////////////////////////////////////////////////////
//
// Set the title of the window and open the save text file:
//
BOOL CLine::SetTitle(char* pcTitle, wchar_t* stationid)
{
  
  LogMsg("CCLine SetTitle");
  errno_t err;
  if((err  = fopen_s( &m_pF, pcTitle, "a+" )) !=0)
	return FALSE;

  if(m_pF != NULL)
  {
	  // write to file 
	  fprintf(m_pF,"<?xml version=\"1.0\" encoding=\"utf-8\"?><tt xml:lang=\"%s\" xmlns=\"http://www.w3.org/2006/10/ttaf1\">\n","EN");
	  fprintf(m_pF,"<head>\n");
	  fprintf(m_pF,"<metadata xmlns:ttm=\"http://www.w3.org/2006/10/ttaf1#metadata\">\n");
	  fprintf(m_pF,"<ttm:title>%ls</ttm:title>\n",stationid); // todo: put the time/date in here
	  fprintf(m_pF,"</metadata>\n");
	  fprintf(m_pF,"</head>\n");
	  fprintf(m_pF,"<body region=\"subtitleArea\">\n");
	  fprintf(m_pF,"<div>\n");
	  fflush(m_pF);

	   //check if any chars are peding to write from prev session and write them to current file.
	  if(remainingCC !=NULL)
	  {
		  LogMsg("going to write remaining CC");
		  // Find the last nonspace on the line:
		  int iEnd;
		  for (iEnd = m_iLineChars - 1; (iEnd >= 0) && (remainingCC[iEnd] == ' '); iEnd--);
		  fprintf(m_pF,"<p xml:id=\"s%d\" begin=\"%ds\" end=\"%ds\">",0,0,0);
		  for (int i = 0; i <= iEnd; i++)
			  putc(remainingCC[i], m_pF);
		  
		  fprintf(m_pF,"</p>");
		  putc('\n', m_pF);
		  fflush(m_pF);
		  remainingCC = NULL;
		  LogMsg("remaining CC written");
	  }
	  IsWriteToFile =1;
	  
  }

  return (m_pF != NULL);
}

/////////////////////////////////////////////////////////////////////////////
//
// Close CC text file
//
void CLine::Term()
{
  LogMsg("CCLine Term");
  // we will now store char. in varibale and write them in next file.
  IsWriteToFile =0;
  fprintf(m_pF,"</div>\n");
  fprintf(m_pF,"</body>\n");
  fprintf(m_pF,"</tt>\n");

  if (m_pF)
  {
    // Write any residual part-line to file:
    iscloded = fclose(m_pF);
	char *msg = new char[100];
	sprintf_s(msg,100,"fclose return : %d",iscloded);
	LogMsg(msg);
	m_pF =NULL;
  }
  
}


/////////////////////////////////////////////////////////////////////////////
//
// typedef void (CALLBACK* CC_CALLBACK)(CCLINE_DATA_S* pCCLineData);
//
// this method will automatically callback after we receive CC char text from video
void CALLBACK CCLineCallback(CCLINE_DATA_S* pCCLineData)
{
	TRY
    {
		CLine* pLine = (CLine*)pCCLineData->pContext;
		pLine->CCCallback(pCCLineData);
		
	}
    CATCH(CException,ex)
	{
	  // catch
	}
	END_CATCH
}


/////////////////////////////////////////////////////////////////////////////
//
//  Open channel to CCLineInterp.ax:
//
//  typedef struct
//  {
//    ULONG       ulDeviceId;     // the device owning the channel
//    ULONG       ulFlags;        // CCCHANNEL_ bits, see below
//    CC_CALLBACK pCallback;      // client's callback function
//    void*       pContext;       // context pointer, or null, supplied by client
//    ULONG       ulChannelId;    // channel id - returned on open, supplied on close
//  }
//  CCCHANNEL_CTL_S;
//
//  #define CCCHANNEL_TEXTMODE    0x0001  // 1 = text mode
//  #define CCCHANNEL_CHANNEL2    0x0002  // 1 = cc2/cc4/text2/text4
//  #define CCCHANNEL_FIELD2      0x0004  // 1 = field 2 - required for close as well as open
//  #define CCCHANNEL_XDS         0x0008  // 1 = XDS stream - lower 3 bits must be 100
//  #define CCCHANNEL_RAW         0x0010  // 1 = raw stream
//  #define CCCHANNEL_ASCIIXLAT   0x0020  // 1 = translate special chars to nearest asciis
//  #define CCCHANNEL_OPEN        0x0040  // 1 = open a channel, 0 = close channel 'ulChannelId'
//  #define CCCHANNEL_NULLS       0x0001  // raw mode: 1 = return all valid double null pairs as 0x0000
//  #define CCCHANNEL_NO_CARRIER  0x0002  // raw mode: 1 = return 0x8080 for no carrier/indecipherable
//
HRESULT CLine::Open()
{
  LogMsg("CCLine Open");
  try
  {
	  if (m_pICCLineInterp == NULL)
	  {
		return E_FAIL;
	  }
	  CCCHANNEL_CTL_S CCChannelCtl;
	  CCChannelCtl.ulDeviceId = m_ulDeviceId;
	  CCChannelCtl.ulFlags    = m_ulFlags | CCCHANNEL_OPEN | CCCHANNEL_ASCIIXLAT;

	  // here we set callback method for CCline
	  CCChannelCtl.pCallback  = ::CCLineCallback;
	  CCChannelCtl.pContext   = this;

	  HRESULT hr = m_pICCLineInterp->CCChannelCtl(&CCChannelCtl);
	  if (hr == 0)
		m_ulChannelId = CCChannelCtl.ulChannelId;

	  LogMsg("CCLine Open Ended");

	  return hr;
  }
  catch(EXCEPINFO e)
  {
	  char *msg = new char[1000];
	  sprintf_s(msg,1000 ,"error:%s",e.bstrDescription);
	  LogMsg(msg);
	  return S_FALSE;
  }
}

/////////////////////////////////////////////////////////////////////////////
//
// Close channel to CCLineInterp.ax:
//
HRESULT CLine::Close()
{
  try
  {
	  if ((m_pICCLineInterp == NULL) || (m_ulChannelId == 0))
		return S_OK;

	  CCCHANNEL_CTL_S CCChannelCtl;
	  CCChannelCtl.ulDeviceId   = m_ulDeviceId;
	  CCChannelCtl.ulFlags      = m_ulFlags;  // must provide field bit to close
	  CCChannelCtl.ulChannelId  = m_ulChannelId;

	  return m_pICCLineInterp->CCChannelCtl(&CCChannelCtl);
  }
  catch(EXCEPINFO e)
  {
	  char *msg = new char[1000];
	  sprintf_s(msg,1000 ,"error:%s",e.bstrDescription);
	  LogMsg(msg);
	  return S_FALSE;
  }
}

/////////////////////////////////////////////////////////////////////////////
//
//  Callback from the driver, part 1:
//  If your callback updates screen text, separate it into two routines, and
//  have the first routine send a message to the second routine containing 
//  the text window interaction.  Use PostMessage(), not SendMessage(),
//  to prevent possible freezeups.
//
//  typedef struct
//  {
//    void*     pContext;     // context pointer, or null, from client when stream opened
//    char*     pcLine;       // pointer to the channel's line data (or XDS block)
//    ULONG     ulStreamId;   // stream id set when stream opened
//    ULONG     ulFlags;      // info about the stream, see CCLINEDATA____ below
//    ULONGLONG ullMsecs;     // most recent timestamp
//    ULONG     ulLineN;      // sequential number of the line returned
//    ULONG     ulReserved;   // currently, 0
//  }
//  CCLINE_DATA_S;
//  
//  #define CCLINE_MODE_MASK  0x000C
//  #define CCLINE_TEXTMODE   0x0000
//  #define CCLINE_PAINTON    0x0004
//  #define CCLINE_ROLLUP     0x0008
//  #define CCLINE_POPON      0x000C
//  #define CCLINE_NEWLINE    0x0010
//  #define CCLINE_POPPED     0x0020
//
void CLine::CCCallback(CCLINE_DATA_S* pCCLineData)
{
	TRY
    {
		//after cal back method called, we'll check if file is open to write? (checking IsWriteToFile vairable)
		// and only write if it is true. 
		if(IsWriteToFile)
		{
			REFERENCE_TIME pCurrTime;
			// get current time of reference clock
			m_pClock->GetTime(&pCurrTime);

			// m_lCapStartTime  this time we saved, on while we start capture
			// by substracting m_lCapStartTime from current time,
			// we can get relative time after capture started (in 1 hour duration). 
			EndTime = pCurrTime - m_lCapStartTime;

			if(!StartTime)
				StartTime = pCurrTime - m_lCapStartTime;
		}
		// if it is flag for new line , then write whole line in CC text file.
		if (pCCLineData->usFlags & CCLINE_NEWLINE)
		{
			SetLine();
			StartTime = NULL;  
		}
		strncpy(m_pcLine,pCCLineData->pcLine, 32);
	}
    CATCH(CException,ex)
	{
	  char *msg  = new char[301];
	  TCHAR   szCause[255];
	  ex->GetErrorMessage(szCause,255);
	  sprintf_s(msg,1000 ,"error:%ls",szCause);
	  LogMsg(msg);
	}
	END_CATCH

}

// this method will write CC line text fo file 
void CLine::SetLine()
{
	TRY
	{
		int iEnd, i;
		if(StartTime == NULL)
			StartTime = EndTime;

		// Find the last nonspace on the line:
		for (iEnd = m_iLineChars - 1; (iEnd >= 0) && (m_pcLine[iEnd] == ' '); iEnd--);

		if(IsWriteToFile)
		{
			// Write the line to file:
			fprintf(m_pF,"<p xml:id=\"s%d\" begin=\"%ds\" end=\"%ds\">",(int)(StartTime /    10000000),(int)(StartTime /    10000000),(int)(EndTime /    10000000));
			for (i = 0; i <= iEnd; i++)
				putc(m_pcLine[i], m_pF);
			fprintf(m_pF,"</p>");
			putc('\n', m_pF);
			fflush(m_pF);
		}
		else
		{
			// if file is closed , then we'll save this text in one variable, 
			// and then writes it to next file after next video capture starts
			LogMsg("Writing to Variable");
			remainingCC = new char[m_iLineChars];
			for (i = 0; i <= iEnd; i++)
			{
				remainingCC[i] = m_pcLine[i];
			}
		}
	}
	CATCH(CException,ex)
	{
	  /*char *msg  = new char[301];
	  TCHAR   szCause[255];
	  ex->GetErrorMessage(szCause,255);
	  sprintf(msg ,"error:%ls",szCause);
	  LogMsg(msg);*/
	}
	END_CATCH    
}

void CLine::LogMsg(char* szBuffer)
{
    SYSTEMTIME st1;
	GetSystemTime(&st1);
	fprintf(m_CappF,"[%d-%2d-%2d %2d:%2d:%2d,%d] - %s\n",st1.wYear,st1.wMonth,st1.wDay,st1.wHour,st1.wMinute,st1.wSecond,st1.wMilliseconds, szBuffer);
	fflush(m_CappF);
	
}



