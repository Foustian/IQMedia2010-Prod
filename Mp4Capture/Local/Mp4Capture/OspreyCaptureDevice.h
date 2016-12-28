// OspreyCaptureDevice.h : definition of the OspreyCaptureDevice class
//

#if !defined (_OSPREYCAPTUREDEVICE_H_)
#define _OSPREYCAPTUREDEVICE_H_

//#include <objbase.h>
#include <streams.h>
#include <ks.h>
#include <ksproxy.h>
#include <ksmedia.h>
#include "o200avsCom.h"
 
#define DEFAULT_CUSTOM   9999

typedef struct
{
  long lMin;
  long lMax;
  long lStep;
  long lDefault;
  long lFlags;
}
  PROCAMP_RANGE,
*PPROCAMP_RANGE;


// class to represent an Osprey Capture device
// has methods to get and set the capture attributes
class OspreyCaptureDevice
{
// Construction
public:
  OspreyCaptureDevice(IMoniker * pM);
  OspreyCaptureDevice(IBaseFilter * base);

// Destructor
  ~OspreyCaptureDevice();

// Operations to get/set capture attributes
public:
  BOOL getSource(long * source);
  BOOL setSource(long source);
  BOOL getNumInputs(ULONG * numInputs);
  BOOL getNumInputsOfType(ULONG type, ULONG * numInputs);
  BOOL getInputIndex(ULONG * index);
  BOOL setInputIndex(ULONG index);
  BOOL getInputName(ULONG index, CString * csName);
  BOOL getCropRect(PIN_ID pID, CRect * rect);
  BOOL setCropRect(PIN_ID pID, CRect rect);
  BOOL getCropEnabled(PIN_ID pID, BOOL * on);
  BOOL setCropEnabled(PIN_ID pID, BOOL on);
  BOOL getCropSettings(PIN_ID pID, CRect &cropRect, BOOL * cropEnabled);
  BOOL setCropSettings(PIN_ID pID, CRect cropRect, BOOL cropEnabled);
  BOOL OspreyCaptureDevice::getGranularity(
    ULONG ulColorFmt,
    int*  piGranV,
    int*  piGranH,
    int*  piMinV,
    int*  piMinH
  );
  BOOL getBrightness(long * brightness);
  BOOL setBrightness(long brightness);
  BOOL getSaturation(long * saturation);
  BOOL setSaturation(long saturation);
  BOOL getContrast(long * contrast);
  BOOL setContrast(long contrast);
  BOOL getHue(long * hue);
  BOOL setHue(long hue);
  BOOL getVideoSettings(long * brightness, long * saturation, long * contrast, long * hue);
  BOOL setVideoSettings(long brightness, long saturation, long contrast, long hue);
  BOOL setVideoDefaults();
  BOOL getBrightnessRange(PPROCAMP_RANGE pRange);
  BOOL getContrastRange  (PPROCAMP_RANGE pRange);
  BOOL getSaturationRange(PPROCAMP_RANGE pRange);
  BOOL getHueRange       (PPROCAMP_RANGE pRange);
  BOOL getStandard(long *plStandard);
  BOOL setStandard(long   lStandard);
  BOOL getTransparentEnabled(PIN_ID pID, BOOL * enabled);
  BOOL setTransparentEnabled(PIN_ID pID, BOOL enabled);
  BOOL getKeyColorEnabled(PIN_ID pID, BOOL * enabled);
  BOOL setKeyColorEnabled(PIN_ID pID, BOOL enabled);
  BOOL getKeyColor(PIN_ID pID, PULONG pulRed, PULONG pulGreen, PULONG pulBlue);
  BOOL setKeyColor(PIN_ID pID,  ULONG  ulRed,  ULONG  ulGreen,  ULONG  ulBlue);
  BOOL getOutputSize(PIN_ID pID, int* piWidth, int* piHeight);
  BOOL setOutputSize(PIN_ID pID, int   iWidth, int   iHeight);
  BOOL getSize(PIN_ID pID, ULONG *sizeMode, ULONG *sizeAuto, ULONG *defaultX, ULONG *defaultY, ULONG *customX, ULONG *customY);
  BOOL setSize(PIN_ID pID, ULONG sizeMode, ULONG sizeAuto, ULONG defaultX, ULONG defaultY, ULONG customX, ULONG customY);
  BOOL getRefSize(PIN_ID pID,int * width, int * height);
  BOOL setDefaultSize(PIN_ID pID,long height, long width);
  BOOL getSquareAspect(long * square);
  BOOL setSquareAspect(long square);
  BOOL getLogoFileName(PIN_ID pID, CString &fName);
  BOOL setLogoFileName(PIN_ID pID, CString  fName);
  BOOL getLogoEnabled(PIN_ID pID, BOOL * enabledFlag);
  BOOL setLogoEnabled(PIN_ID pID, BOOL enabledFlag);
  BOOL getLogoPosition(PIN_ID pID, ULONG * x,ULONG * y);
  BOOL setLogoPosition(PIN_ID pID, ULONG x, ULONG y);
  BOOL getLogoSize(PIN_ID pID, ULONG * width, ULONG * height);
  BOOL setLogoSize(PIN_ID pID, ULONG width, ULONG height);
  BOOL setLogoVidSize(PIN_ID pID, ULONG width, ULONG height);
  BOOL setLogoSizeScale(PIN_ID pID, int scale);
  BOOL getLogoSettings(PIN_ID pID,WCHAR * pawFilename, CRect * logoRect, BOOL * logoEnabled, ULONG * keyColor, BOOL * keyColorEnabled, BOOL * transparent);
  BOOL setLogoSettings(PIN_ID pID,WCHAR * pawFilename, CRect logoRect, BOOL logoEnabled, ULONG keyColor, BOOL keyColorEnabled, BOOL transparent);
  BOOL getDeviceName(WCHAR * devName);
  BOOL getInstanceNumber(ULONG * iNumber);
  BOOL getSerialNumber(ULONG * serialNumber);
  BOOL getDeviceType(ULONG * dType);
  BOOL getBusNumber(ULONG * busNumber);
  BOOL getSlotNumber(ULONG * slotNumber);
  BOOL getFieldOrder(BOOL * evenOdd);
  BOOL setFieldOrder(BOOL evenOdd);
  BOOL getSWDeinterlace(BOOL * enabled);
  BOOL setSWDeinterlace(BOOL enabled);
  IBaseFilter* getBaseFilter() { return baseFilter; };

// Members
private:
    IBaseFilter           * baseFilter;
    IAMVideoProcAmp       * pVPA;
    IAMAnalogVideoDecoder * pAVD;
    IOspreyCrossbar       * pXBar;
    IOspreyCrop           * pCrop;
    IOspreyRefSize        * pSize;
    IOspreyLogo           * pLogo;
    IOspreyDevice         * pDevice;

    void getInterfaces();
};

#endif    // !defined(_OSPREYCAPTUREDEVICE_H_)



