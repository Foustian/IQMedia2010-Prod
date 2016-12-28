#include "stdafx.h"

#include "OspreyCaptureDevice.h"

#include <initguid.h>

// include to pick up the string conversion macros
#include <AFXPRIV.H>

// {551C07EF-E0DA-411d-B712-BFCBE3DE9C64}
DEFINE_GUID(IID_IOspreyCrop,
  0x551c07ef, 0xe0da, 0x411d, 0xb7, 0x12, 0xbf, 0xcb, 0xe3, 0xde, 0x9c, 0x64);

// {9C317B97-3F9B-470a-BD6C-AAE08B93B279}
DEFINE_GUID(IID_IOspreyCrossbar, 
  0x9c317b97, 0x3f9b, 0x470a, 0xbd, 0x6c, 0xaa, 0xe0, 0x8b, 0x93, 0xb2, 0x79);

// {8C0F52A2-B65F-4864-897E-6A6748C270E9}
DEFINE_GUID(IID_IOspreyRefSize, 
  0x8C0F52A2, 0xB65F, 0x4864, 0x89, 0x7E, 0x6A, 0x67, 0x48, 0xC2, 0x70, 0xE9);

// {C8001E49-697A-4123-9E67-CEFACA031BDD}
DEFINE_GUID(IID_IOspreyLogo,
  0xc8001e49, 0x697a, 0x4123, 0x9e, 0x67, 0xce, 0xfa, 0xca, 0x3, 0x1b, 0xdd);

// {C30BF251-3B00-45f9-A35A-E4969E3EB685}
DEFINE_GUID(IID_IOspreyDevice,
  0xC30BF251, 0x3B00, 0x45f9, 0xA3, 0x5A, 0xE4, 0x96, 0x9E, 0x3E, 0xB6, 0x85);

// Constructor
// from IBaseFilter
OspreyCaptureDevice::OspreyCaptureDevice(IBaseFilter * base)
{
  baseFilter = base;
  base->AddRef();
  getInterfaces();
}

// from IMoniker
OspreyCaptureDevice::OspreyCaptureDevice(IMoniker * pM)
{
  baseFilter = NULL;
  HRESULT hr = pM->BindToObject(0, 0, IID_IBaseFilter, (void**)&baseFilter);
  getInterfaces();
}

void OspreyCaptureDevice::getInterfaces()
{
  if (!baseFilter)
    return;

  HRESULT hr = baseFilter->QueryInterface(IID_IAMVideoProcAmp,(void **)&pVPA);
  if (!SUCCEEDED(hr))
    pVPA = NULL;
  hr = baseFilter->QueryInterface(IID_IAMAnalogVideoDecoder,(void **)&pAVD);
  if (!SUCCEEDED(hr))
    pAVD = NULL;
  hr = baseFilter->QueryInterface(IID_IOspreyCrossbar,(void **)&pXBar);
  if (!SUCCEEDED(hr))
    pXBar = NULL;
  hr = baseFilter->QueryInterface(IID_IOspreyCrop,(void **)&pCrop);
  if (!SUCCEEDED(hr))
    pCrop = NULL;
  hr = baseFilter->QueryInterface(IID_IOspreyRefSize,(void **)&pSize);
  if (!SUCCEEDED(hr))
    pSize = NULL;
  hr = baseFilter->QueryInterface(IID_IOspreyLogo,(void **)&pLogo);
  if (!SUCCEEDED(hr))
    pLogo = NULL;
  hr = baseFilter->QueryInterface(IID_IOspreyDevice,(void **)&pDevice);
  if (!SUCCEEDED(hr))
    pDevice = NULL;
}

// Destructor
OspreyCaptureDevice::~OspreyCaptureDevice()
{
  if (pVPA)
    pVPA->Release();
  pVPA = NULL;

  if (pAVD)
    pAVD->Release();
  pAVD = NULL;

  if (pXBar)
    pXBar->Release();
  pXBar = NULL;

  if (pCrop)
    pCrop->Release();
  pCrop = NULL;

  if (pSize)
    pSize->Release();
  pSize = NULL;

  if (pLogo)
    pLogo->Release();
  pLogo = NULL;

  if (pDevice)
    pDevice->Release();
  pDevice = NULL;

  if (baseFilter)
    baseFilter->Release();
  baseFilter = NULL;
}

////////////////////////////////////
// Methods to get and set attributes
////////////////////////////////////

// method to get the number of video source inputs of any type
BOOL OspreyCaptureDevice::getNumInputs(ULONG * numInputs)
{
  // if there is no IOspreyCrossbar interface then there is nothing we can do
  if (!pXBar)
    return FALSE;

  // if we were passed a null pointer to put the answer in the there is nothing we can do
  if (!numInputs)
    return FALSE;

  HRESULT hr;
  KSPROPERTY_CROSSBAR_CAPS_S    KPS_Caps;
  hr = pXBar->GetCaps(&KPS_Caps);
  if (SUCCEEDED(hr))
  {
    *numInputs = KPS_Caps.NumberOfInputs;
    return TRUE;
  }

  return FALSE;
}


// method to get the number of video source inputs of the specified type
BOOL OspreyCaptureDevice::getNumInputsOfType(ULONG inputType, ULONG * numInputs)
{
  // if we were passed a null pointer to put the answer in the there is nothing we can do
  if (!numInputs)
    return FALSE;

  // get the number of inputs of any type
  ULONG totalInputs;
  if (!getNumInputs(&totalInputs))
    return FALSE;

  // go through the inputs and count the number of the specified type
  *numInputs = 0;
  HRESULT hr;
  KSPROPERTY_CROSSBAR_PININFO_S KPS_PinInfo;
  for(ULONG i = 0; i < totalInputs; i++)
  {
    KPS_PinInfo.Direction = KSPIN_DATAFLOW_IN;
    KPS_PinInfo.Index     = i;
    hr = pXBar->PinInfo(&KPS_PinInfo);
    if (!SUCCEEDED(hr))
      return FALSE;
    if (KPS_PinInfo.PinType == inputType)
      (*numInputs)++;
  }
  return TRUE;
}

// method to get the index of the current video source input
BOOL OspreyCaptureDevice::getInputIndex(ULONG * index)
{
  // if there is no IOspreyCrossbar interface then there is nothing we can do
  if (!pXBar)
    return FALSE;

  // if we were passed a null pointer to put the answer in the there is nothing we can do
  if (!index)
    return FALSE;

  HRESULT hr;
  KSPROPERTY_CROSSBAR_ROUTE_S   KPS_Route;
  // get the currently selected Crossbar route
  hr = pXBar->GetRoute(&KPS_Route);
  if (!SUCCEEDED(hr))
    return FALSE;

  // get information for the current input pin
  *index = KPS_Route.IndexInputPin;
  return TRUE;
}

// method to set the index of the video source input
BOOL OspreyCaptureDevice::setInputIndex(ULONG index)
{
  // if there is no IOspreyCrossbar interface then there is nothing we can do
  if (!pXBar)
    return FALSE;

  // validate the input index
  ULONG nInputs = 0;
  if ((index < 0) || ((!getNumInputs(&nInputs)) || (index >= nInputs)))
    return FALSE;

  HRESULT hr;
  KSPROPERTY_CROSSBAR_ROUTE_S   KPS_Route;
  // route the specified input pin to the output pin
  KPS_Route.IndexOutputPin  = 0;
  KPS_Route.IndexInputPin   = index;
  hr = pXBar->SetRoute(&KPS_Route);
  if (SUCCEEDED(hr))
    return TRUE;

  return FALSE;
}

// method to get the name of a video source input given the index
BOOL OspreyCaptureDevice::getInputName(ULONG index, CString * csName)
{
  // if there is no IOspreyCrossbar interface then there is nothing we can do
  if (!pXBar)
    return FALSE;

  // validate the input index
  ULONG nInputs = 0;
  if ((index < 0) || ((!getNumInputs(&nInputs)) || (index >= nInputs)))
    return FALSE;

  ULONG numComposite = 0;
  ULONG numSVideo    = 0;
  ULONG numSDI       = 0;
  ULONG numDV        = 0;
  ULONG numOfType    = 0;
  HRESULT hr;
  KSPROPERTY_CROSSBAR_PININFO_S KPS_PinInfo;
  for(ULONG i = 0; i <= index; i++)
  {
    KPS_PinInfo.Direction = KSPIN_DATAFLOW_IN;
    KPS_PinInfo.Index     = i;
    hr = pXBar->PinInfo(&KPS_PinInfo);
    if (!SUCCEEDED(hr))
      return FALSE;

    switch (KPS_PinInfo.PinType)
    {
      case PhysConn_Video_Composite:     
        numComposite++; 
        if (i == index)
        {
          getNumInputsOfType(PhysConn_Video_Composite,&numOfType);
          if (numOfType > 1)
            (*csName).Format(_T("Composite%lu"),numComposite);
          else
            (*csName).Format(_T("Composite"));
        }
        break;
      case PhysConn_Video_SVideo:  
        numSVideo++;
        if (i == index)
        {
          getNumInputsOfType(PhysConn_Video_SVideo,&numOfType);
          if (numOfType > 1)
            (*csName).Format(_T("SVideo%lu"),numSVideo);
          else
            (*csName).Format(_T("SVideo"));
        }
        break;
      case PhysConn_Video_SerialDigital:  
        numSDI++;
        if (i == index)
        {
          getNumInputsOfType(PhysConn_Video_SerialDigital,&numOfType);
          if (numOfType > 1)
            (*csName).Format(_T("SDI%lu"),numSDI);
          else
            (*csName).Format(_T("SDI"));
        }
        break;
      case PhysConn_Video_1394:  
        numDV++;
        if (i == index)
        {
          getNumInputsOfType(PhysConn_Video_1394,&numOfType);
          if (numOfType > 1)
            (*csName).Format(_T("DV%lu"),numDV);
          else
            (*csName).Format(_T("DV"));
        }
        break;
      default:
        (*csName).Format(_T("Unknown"));
        break;
    }
  }
  return TRUE;
}

// method to get the type of the video source input
// should be one of: PhysConn_Video_Composite or PhysConn_VideoSVideo
BOOL OspreyCaptureDevice::getSource(long * source)
{
  // if there is no IOspreyCrossbar interface then there is nothing we can do
  if (!pXBar)
    return FALSE;

  // if we were passed a null pointer to put the answer in the there is nothing we can do
  if (!source)
    return FALSE;

  HRESULT hr;
  KSPROPERTY_CROSSBAR_PININFO_S KPS_PinInfo;
  KSPROPERTY_CROSSBAR_ROUTE_S   KPS_Route;
  // get the currently selected Crossbar route
  hr = pXBar->GetRoute(&KPS_Route);
  if (!SUCCEEDED(hr))
    return FALSE;

  // get information for the current input pin
  KPS_PinInfo.Direction      = KSPIN_DATAFLOW_IN;
  KPS_PinInfo.Index          = KPS_Route.IndexInputPin;
  hr = pXBar->PinInfo(&KPS_PinInfo);
  if (SUCCEEDED(hr))
  {
    *source = KPS_PinInfo.PinType;
    return TRUE;
  }

  return FALSE;
}

// method to set the type of the video source input
// should be one of: PhysConn_Video_Composite or PhysConn_VideoSVideo
// if there is more than 1 input of the specified type then the 1st one found is used
BOOL OspreyCaptureDevice::setSource(long source)
{
  // if there is no IOspreyCrossbar interface then there is nothing we can do
  if (!pXBar)
    return FALSE;
  
  KSPROPERTY_CROSSBAR_PININFO_S KPS_PinInfo;
  KSPROPERTY_CROSSBAR_ROUTE_S   KPS_Route;
  KSPROPERTY_CROSSBAR_CAPS_S    KPS_Caps;
  ULONG i;

  // get the number of input pins on the Crossbar
  HRESULT hr = pXBar->GetCaps(&KPS_Caps);
  if (FAILED(hr))
    return FALSE;

  for(i=0;i<KPS_Caps.NumberOfInputs;i++)
  {
    // find the index of the desired input pin
    KPS_PinInfo.Direction      = KSPIN_DATAFLOW_IN;
    KPS_PinInfo.Index          = i;
    hr = pXBar->PinInfo(&KPS_PinInfo);
    if (FAILED(hr))
      return FALSE;

    if (KPS_PinInfo.PinType == (ULONG)source)
    {
      // route the input pin to the output pin
      KPS_Route.IndexOutputPin  = 0;
      KPS_Route.IndexInputPin   = i;
      hr = pXBar->SetRoute(&KPS_Route);
      if (SUCCEEDED(hr))
        return TRUE;
      else
        return FALSE;
    }
  }
  return FALSE;
}


// method to get the cropping rectangle
BOOL OspreyCaptureDevice::getCropRect(PIN_ID pID, CRect * rect)
{
  // if we do not have a cropping interface then there is nothing we can do
  if (!pCrop)
    return FALSE;

  // get the current crop settings
  KSPROPERTY_OSPREY_CROP_S KPS_Crop;
  KPS_Crop.PinId = pID;
  HRESULT hr = pCrop->Get(&KPS_Crop);
  if (SUCCEEDED(hr))
  {
    rect->top    = KPS_Crop.ulCropTop;
    rect->bottom = KPS_Crop.ulCropBottom;
    rect->left   = KPS_Crop.ulCropLeft;
    rect->right  = KPS_Crop.ulCropRight;
  }

  return SUCCEEDED(hr);
}

// method to set the cropping rectangle
BOOL OspreyCaptureDevice::setCropRect(PIN_ID pID, CRect rect)
{
  // if we do not have a cropping interface then there is nothing we can do
  if (!pCrop)
    return FALSE;

  // get the current crop settings
  KSPROPERTY_OSPREY_CROP_S KPS_Crop;
  KPS_Crop.PinId = pID;
  HRESULT hr = pCrop->Get(&KPS_Crop);
  if (SUCCEEDED(hr))
  {
    // set the new cropping rectangle
    KPS_Crop.ulCropTop    = rect.top;
    KPS_Crop.ulCropBottom = rect.bottom;
    KPS_Crop.ulCropLeft   = rect.left;
    KPS_Crop.ulCropRight  = rect.right;

    hr = pCrop->Set(&KPS_Crop);
  }

  return SUCCEEDED(hr);
}

// method to determine if cropping is turned on
BOOL OspreyCaptureDevice::getCropEnabled(PIN_ID pID, BOOL * on)
{
  // if we do not have a cropping interface then there is nothing we can do
  if (!pCrop)
    return FALSE;

  // get the current crop settings
  KSPROPERTY_OSPREY_CROP_S KPS_Crop;
  KPS_Crop.PinId = pID;
  HRESULT hr = pCrop->Get(&KPS_Crop);
  if (SUCCEEDED(hr))
  {
    *on = (KPS_Crop.ulCropEnable == 1);
  }

  return SUCCEEDED(hr);
}

// method to turn cropping on or off
BOOL OspreyCaptureDevice::setCropEnabled(PIN_ID pID, BOOL on)
{
  // if we do not have a cropping interface then there is nothing we can do
  if (!pCrop)
    return FALSE;

  // get the current crop settings
  KSPROPERTY_OSPREY_CROP_S KPS_Crop;
  KPS_Crop.PinId = pID;
  HRESULT hr = pCrop->Get(&KPS_Crop);
  if (SUCCEEDED(hr))
  {
    // change the cropping on setting to the specified value
    if (on)
      KPS_Crop.ulCropEnable = 1;
    else
      KPS_Crop.ulCropEnable = 0;
    hr = pCrop->Set(&KPS_Crop);
  }

  return SUCCEEDED(hr);
}

// method to get crop granularity of a color format; and min size
BOOL OspreyCaptureDevice::getGranularity(
  ULONG ulColorFmt,
  int*  piGranV,
  int*  piGranH,
  int*  piMinV,
  int*  piMinH
)
{
  // if we do not have a cropping interface then there is nothing we can do
  if (!pCrop)
    return FALSE;

  KSPROPERTY_OSPREY_CROP_ALIGNMENT_S CropAlignment;
  CropAlignment.ulColorFmt = ulColorFmt;
  HRESULT hr = pCrop->GetAlignment(&CropAlignment);
  if (SUCCEEDED(hr))
  {
    *piGranV = CropAlignment.ulGranV + 1; // the interface returns masks
    *piGranH = CropAlignment.ulGranH + 1;
    *piMinV  = CropAlignment.ulMinV;
    *piMinH  = CropAlignment.ulMinH;
  }

  return SUCCEEDED(hr);
}

// method to get the value of the brightness setting
BOOL OspreyCaptureDevice::getBrightness(long * brightness)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  long flags;
  HRESULT hr = pVPA->Get(VideoProcAmp_Brightness, brightness, &flags);
  if (SUCCEEDED(hr))
    return TRUE;

  return FALSE;
}

// method to set the value of the brightness setting
BOOL OspreyCaptureDevice::setBrightness(long brightness)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  long flags;
  long currentValue;
  HRESULT hr = pVPA->Get(VideoProcAmp_Brightness, &currentValue, &flags);
  if (SUCCEEDED(hr))
  {
    hr = pVPA->Set(VideoProcAmp_Brightness, brightness, flags);
    return TRUE;
  }

  return FALSE;
}

// method to get the value of the Contrast setting
BOOL OspreyCaptureDevice::getContrast(long * contrast)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  long flags;
  HRESULT hr = pVPA->Get(VideoProcAmp_Contrast, contrast, &flags);
  if (SUCCEEDED(hr))
    return TRUE;

  return FALSE;
}

// method to set the value of the Contrast setting
BOOL OspreyCaptureDevice::setContrast(long contrast)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  long flags;
  long currentValue;
  HRESULT hr = pVPA->Get(VideoProcAmp_Contrast, &currentValue, &flags);
  if (SUCCEEDED(hr))
  {
    hr = pVPA->Set(VideoProcAmp_Contrast, contrast, flags);
    return TRUE;
  }

  return FALSE;
}

// method to get the value of the Saturation setting
BOOL OspreyCaptureDevice::getSaturation(long * saturation)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  long flags;
  HRESULT hr = pVPA->Get(VideoProcAmp_Saturation, saturation, &flags);

  if (SUCCEEDED(hr))
    return TRUE;

  return FALSE;
}

// method to set the value of the Saturation setting
BOOL OspreyCaptureDevice::setSaturation(long saturation)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  long flags;
  long currentValue;
  HRESULT hr = pVPA->Get(VideoProcAmp_Saturation, &currentValue, &flags);
  if (SUCCEEDED(hr))
  {
    hr = pVPA->Set(VideoProcAmp_Saturation, saturation, flags);
    return TRUE;
  }

  return FALSE;
}

// method to get the value of the Hue setting
BOOL OspreyCaptureDevice::getHue(long * hue)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  long flags;
  HRESULT hr = pVPA->Get(VideoProcAmp_Hue, hue, &flags);

  if (SUCCEEDED(hr))
    return TRUE;

  return FALSE;
}

// method to set the value of the Hue setting
BOOL OspreyCaptureDevice::setHue(long hue)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  long flags;
  long currentValue;
  HRESULT hr = pVPA->Get(VideoProcAmp_Hue, &currentValue, &flags);
  if (SUCCEEDED(hr))
  {
    hr = pVPA->Set(VideoProcAmp_Hue, hue, flags);
    return TRUE;
  }

  return FALSE;
}

// method to get all the video settings 
BOOL OspreyCaptureDevice::getVideoSettings(long * brightness, long * saturation, long * contrast, long * hue)
{
  if (!getBrightness(brightness))
    return FALSE;

  if (!getSaturation(saturation))
    return FALSE;

  if (!getContrast(contrast))
    return FALSE;

  if (!getHue(hue))
    return FALSE;

  return TRUE;
}

// method to set all the video settings 
BOOL OspreyCaptureDevice::setVideoSettings(long brightness, long saturation, long contrast, long hue)
{
  if (!setBrightness(brightness))
    return FALSE;

  if (!setSaturation(saturation))
    return FALSE;

  if (!setContrast(contrast))
    return FALSE;

  if (!setHue(hue))
    return FALSE;

  return TRUE;
}

// method to set all procamp settings to defaults
BOOL OspreyCaptureDevice::setVideoDefaults()
{
  PROCAMP_RANGE Range;

  getBrightnessRange(&Range);  setBrightness(Range.lDefault);
  getContrastRange  (&Range);  setContrast  (Range.lDefault);
  getSaturationRange(&Range);  setSaturation(Range.lDefault);
  getHueRange       (&Range);  setHue       (Range.lDefault);

  return TRUE;
}

BOOL OspreyCaptureDevice::getBrightnessRange(PPROCAMP_RANGE pRange)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  HRESULT hr = pVPA->GetRange(
    VideoProcAmp_Brightness,
    &pRange->lMin,
    &pRange->lMax,
    &pRange->lStep,
    &pRange->lDefault,
    &pRange->lFlags
  );

  return SUCCEEDED(hr);
}

BOOL OspreyCaptureDevice::getContrastRange(PPROCAMP_RANGE pRange)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  HRESULT hr = pVPA->GetRange(
    VideoProcAmp_Contrast,
    &pRange->lMin,
    &pRange->lMax,
    &pRange->lStep,
    &pRange->lDefault,
    &pRange->lFlags
  );

  return SUCCEEDED(hr);
}

BOOL OspreyCaptureDevice::getSaturationRange(PPROCAMP_RANGE pRange)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  HRESULT hr = pVPA->GetRange(
    VideoProcAmp_Saturation,
    &pRange->lMin,
    &pRange->lMax,
    &pRange->lStep,
    &pRange->lDefault,
    &pRange->lFlags
  );

  return SUCCEEDED(hr);
}

BOOL OspreyCaptureDevice::getHueRange(PPROCAMP_RANGE pRange)
{
  // if we do not have an IAMVideoProcAmp interface then there is nothing we can do
  if (!pVPA)
    return FALSE;

  HRESULT hr = pVPA->GetRange(
    VideoProcAmp_Hue,
    &pRange->lMin,
    &pRange->lMax,
    &pRange->lStep,
    &pRange->lDefault,
    &pRange->lFlags
  );

  return SUCCEEDED(hr);
}

// method to get the video standard
BOOL OspreyCaptureDevice::getStandard(long* plStandard)
{
  // if no IAMAnalogVideoDecoder interface then there is nothing we can do
  if (!pAVD)
    return FALSE;

  HRESULT hr = pAVD->get_TVFormat(plStandard);

  return SUCCEEDED(hr);
}

// method to set the video standard
BOOL OspreyCaptureDevice::setStandard(long lStandard)
{
  // if no IAMAnalogVideoDecoder interface then there is nothing we can do
  if (!pAVD)
    return FALSE;

  HRESULT hr = pAVD->put_TVFormat(lStandard);

  return SUCCEEDED(hr);
}

// method to get the logo Key Color
BOOL OspreyCaptureDevice::getKeyColor(PIN_ID pID, PULONG pulRed, PULONG pulGreen, PULONG pulBlue)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the current logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    *pulRed   = (KPS_Logo.ulKeyColor >> 16) & 0xFF;
    *pulGreen = (KPS_Logo.ulKeyColor >>  8) & 0xFF;
    *pulBlue  = (KPS_Logo.ulKeyColor      ) & 0xFF;
  }

  return SUCCEEDED(hr);
}

// method to set the logo Key Color
BOOL OspreyCaptureDevice::setKeyColor(PIN_ID pID, ULONG ulRed, ULONG ulGreen, ULONG ulBlue)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the current logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    KPS_Logo.ulKeyColorCustom =
    KPS_Logo.ulKeyColor       = BGR(ulBlue, ulGreen, ulRed);

    hr = pLogo->Set(&KPS_Logo);
  }

  return SUCCEEDED(hr);
}

// method to determine if the logo Key Color is enabled/disabled
BOOL OspreyCaptureDevice::getKeyColorEnabled(PIN_ID pID, BOOL* enable)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the current logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    *enable = (KPS_Logo.ulFlags & LOGO_KEYCOLOR) != 0;
  }

  return SUCCEEDED(hr);
}

// method to enable/disable the logo Key Color
BOOL OspreyCaptureDevice::setKeyColorEnabled(PIN_ID pID, BOOL enable)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the current logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    if (enable)
      KPS_Logo.ulFlags |=  LOGO_KEYCOLOR;
    else
      KPS_Logo.ulFlags &= ~LOGO_KEYCOLOR;

    hr = pLogo->Set(&KPS_Logo);
  }

  return SUCCEEDED(hr);
}


// method to determine if the logo Transparentcy is enabled/disabled
BOOL OspreyCaptureDevice::getTransparentEnabled(PIN_ID pID, BOOL* enable)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the current logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    *enable = (KPS_Logo.ulFlags & LOGO_EMBOSSED) != 0;
  }

  return SUCCEEDED(hr);
}

// method to enable/disable the logo Transparentcy
BOOL OspreyCaptureDevice::setTransparentEnabled(PIN_ID pID, BOOL enable)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the current logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    if (enable)
      KPS_Logo.ulFlags |=  LOGO_EMBOSSED;
    else
      KPS_Logo.ulFlags &= ~LOGO_EMBOSSED;

    hr = pLogo->Set(&KPS_Logo);
  }

  return SUCCEEDED(hr);
}

// method to get the reference height and width
BOOL OspreyCaptureDevice::getRefSize(PIN_ID pID, int * width, int * height)
{
  // if no IOspreyCrop interface then there is nothing we can do
  if (!pCrop)
    return FALSE;

  // get the current size settings
  KSPROPERTY_OSPREY_CROP_S KPS_Crop;
  KPS_Crop.PinId = pID;
  HRESULT hr = pCrop->Get(&KPS_Crop);
  if (SUCCEEDED(hr))
  {
    *height = KPS_Crop.ulRefHt;
    *width  = KPS_Crop.ulRefWd;
  }

  return SUCCEEDED(hr);
}

// method to set a default height and width
BOOL OspreyCaptureDevice::getOutputSize(PIN_ID pID, int* piWidth, int* piHeight)
{
  // if no IOspreyCrop interface then there is nothing we can do
  if (!pCrop)
    return FALSE;

  // get the current size settings
  KSPROPERTY_OSPREY_CROP_S KPS_Crop;
  KPS_Crop.PinId = pID;
  HRESULT hr = pCrop->Get(&KPS_Crop);
  if (SUCCEEDED(hr))
  {
    // get the new size value
    *piHeight = KPS_Crop.ulDefaultHeight;
    *piWidth  = KPS_Crop.ulDefaultWidth ;
  }

  return SUCCEEDED(hr);
}

// method to set a default height and width
BOOL OspreyCaptureDevice::setOutputSize(PIN_ID pID, int iWidth, int iHeight)
{
  // if no IOspreyCrop interface then there is nothing we can do
  if (!pCrop)
    return FALSE;

  // get the current size settings
  KSPROPERTY_OSPREY_CROP_S KPS_Crop;
  KPS_Crop.PinId = pID;
  HRESULT hr = pCrop->Get(&KPS_Crop);
  if (SUCCEEDED(hr))
  {
    // set the new size value
    KPS_Crop.ulDefaultHeight  = iHeight;
    KPS_Crop.ulDefaultWidth   = iWidth;
    KPS_Crop.ulDefaultHeightX = iHeight;
    KPS_Crop.ulDefaultWidthX  = iWidth;

    hr = pCrop->Set(&KPS_Crop);
  }

  return SUCCEEDED(hr);
}

// method to set a default height and width
BOOL OspreyCaptureDevice::setDefaultSize(PIN_ID pID, long height, long width)
{
  // if no IOspreyCrop interface then there is nothing we can do
  if (!pCrop)
    return FALSE;

  // get the current size settings
  KSPROPERTY_OSPREY_CROP_S KPS_Crop;
  KPS_Crop.PinId = pID;
  HRESULT hr = pCrop->Get(&KPS_Crop);
  if (SUCCEEDED(hr))
  {
    // set the new size value
    KPS_Crop.ulAutoMode = 0;
    KPS_Crop.ulDefaultHeight  = height;
    KPS_Crop.ulDefaultWidth   = width;
    KPS_Crop.ulDefaultHeightX = height;
    KPS_Crop.ulDefaultWidthX  = width;

    hr = pCrop->Set(&KPS_Crop);
  }

  return SUCCEEDED(hr);
}

// method to get all the size settings for a pin
BOOL OspreyCaptureDevice::getSize(PIN_ID pID, ULONG * sizeMode, ULONG * sizeAuto, ULONG * defaultX, ULONG * defaultY, ULONG * customX, ULONG * customY)
{
  // if no IOspreyCrop interface then there is nothing we can do
  if (!pCrop)
    return FALSE;

  // get the current size settings
  KSPROPERTY_OSPREY_CROP_S KPS_Crop;
  KPS_Crop.PinId = pID;
  HRESULT hr = pCrop->Get(&KPS_Crop);
  if (SUCCEEDED(hr))
  {
    *sizeMode = KPS_Crop.ulAutoMode;
    *sizeAuto = KPS_Crop.ulAutoSize;
    *defaultX = KPS_Crop.ulDefaultHeight;
    *defaultY = KPS_Crop.ulDefaultWidth;
    *customX  = KPS_Crop.ulDefaultHeightX;
    *customY  = KPS_Crop.ulDefaultWidthX;
  }

  return SUCCEEDED(hr);
}

// method to set all the size settings for a pin
BOOL OspreyCaptureDevice::setSize(PIN_ID pID, ULONG sizeMode, ULONG sizeAuto, ULONG defaultX, ULONG defaultY, ULONG customX, ULONG customY)
{
  // if no IOspreyCrop interface then there is nothing we can do
  if (!pCrop)
    return FALSE;

  // get the current size settings
  KSPROPERTY_OSPREY_CROP_S KPS_Crop;
  KPS_Crop.PinId = pID;
  HRESULT hr = pCrop->Get(&KPS_Crop);
  if (SUCCEEDED(hr))
  {
    KPS_Crop.ulAutoMode       = sizeMode;
    KPS_Crop.ulAutoSize       = sizeAuto;
    KPS_Crop.ulDefaultHeight  = defaultX;
    KPS_Crop.ulDefaultWidth   = defaultY;
    KPS_Crop.ulDefaultHeightX = customX;
    KPS_Crop.ulDefaultWidthX  = customY;

    hr = pCrop->Set(&KPS_Crop);
  }

  return SUCCEEDED(hr);
}

// method to get the aspect ratio
// value set to 1 for square aspect ratio
// value set to 0 for CCIR-601 
BOOL OspreyCaptureDevice::getSquareAspect(long * square)
{
  // if no IOspreyRefSize interface then there is nothing we can do
  if (!pSize)
    return FALSE;

  // get the current size settings
  KSPROPERTY_OSPREY_REFSIZE_S KPS_Size;
  memset(&KPS_Size,0,sizeof(KPS_Size));
  HRESULT hr = pSize->Get(&KPS_Size);
  if (SUCCEEDED(hr))
  {
    *square = KPS_Size.ulCCIR;
    return TRUE;
  }

  return FALSE;
}


// method to set the aspect ratio
// value set to 1 for square aspect ratio
// value set to 0 for CCIR-601 
BOOL OspreyCaptureDevice::setSquareAspect(long square)
{
  // if no IOspreyRefSize interface then there is nothing we can do
  if (!pSize)
    return FALSE;

  // make sure the value is valid
  if ((square < 0) || (square > 1))
    return FALSE;

  // get the current size settings
  KSPROPERTY_OSPREY_REFSIZE_S KPS_Size;
  memset(&KPS_Size,0,sizeof(KPS_Size));
  HRESULT hr = pSize->Get(&KPS_Size);
  if (SUCCEEDED(hr))
  {
    KPS_Size.ulCCIR = square;
    HRESULT hr = pSize->Set(&KPS_Size);
    if (SUCCEEDED(hr))
    {
      return TRUE;
    }
  }

  return FALSE;
}


// method to get the logo file name
BOOL OspreyCaptureDevice::getLogoFileName(PIN_ID pID, CString &fName)
{
  WCHAR wFName[LOGO_MAXPATH];
  TCHAR * tfName;

  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  memset(wFName,0,sizeof(wFName));

  // get the current logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);

  if (SUCCEEDED(hr))
  {
    wcscpy_s(wFName,LOGO_MAXPATH,KPS_Logo.awFName);
    USES_CONVERSION;
    tfName = W2T(wFName);
    fName.Format(_T("%s"), tfName);
  }

  return SUCCEEDED(hr);
}


// method to set the logo file name
BOOL OspreyCaptureDevice::setLogoFileName(PIN_ID pID, CString fname)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // set the logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    #ifdef _UNICODE
      // CString is already using wide charactors if _UNICODE is defined
      wcscpy_s(KPS_Logo.awFName,(LPCTSTR)fname);
    #else
      // convert the CString to wide chars if _UNICODE is not defined
      MultiByteToWideChar(
        CP_ACP,
        MB_PRECOMPOSED,
        (LPCTSTR)fname,
        -1,KPS_Logo.awFName,
        LOGO_MAXPATH
      );
    #endif

    hr = pLogo->Set(&KPS_Logo);
  }

  return SUCCEEDED(hr);
}

// method to see if logo display is enabled
BOOL OspreyCaptureDevice::getLogoEnabled(PIN_ID pID, BOOL * enabledFlag)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    *enabledFlag = (KPS_Logo.ulFlags & LOGO_ENABLE) != 0;
  }

  return SUCCEEDED(hr);
}

// method to enable/disable logo display
BOOL OspreyCaptureDevice::setLogoEnabled(PIN_ID pID, BOOL  enabledFlag)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    if (enabledFlag)
      KPS_Logo.ulFlags |=  LOGO_ENABLE;
    else
      KPS_Logo.ulFlags &= ~LOGO_ENABLE;

    hr = pLogo->Set(&KPS_Logo);
  }

  return SUCCEEDED(hr);
}


// method to get the position of the logo
BOOL OspreyCaptureDevice::getLogoPosition(PIN_ID pID, ULONG * x, ULONG * y)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    *x = KPS_Logo.ulLeft;
    *y = KPS_Logo.ulTop;
  }

  return SUCCEEDED(hr);
}


// method to set the position of the logo
BOOL OspreyCaptureDevice::setLogoPosition(PIN_ID pID, ULONG x, ULONG y)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // set the position of the logo
  KSPROPERTY_OSPREY_LOGO_OP_S KPS_LogoOp;
  KPS_LogoOp.PinId  = pID;
  KPS_LogoOp.ulLeft = x;
  KPS_LogoOp.ulTop  = y;
  HRESULT hr = pLogo->SetPosition(&KPS_LogoOp);

  return SUCCEEDED(hr);
}


// method to get the size of the logo
BOOL OspreyCaptureDevice::getLogoSize(PIN_ID pID, ULONG * width, ULONG * height)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    *width  = KPS_Logo.ulWidth;
    *height = KPS_Logo.ulHeight;
  }

  return SUCCEEDED(hr);
}

// method to set the size of the video for 1X logo scaling
BOOL OspreyCaptureDevice::setLogoVidSize(PIN_ID pID, ULONG width, ULONG height)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the current logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    // set the size of the logo
    KPS_Logo.ulVideoWd = width;
    KPS_Logo.ulVideoHt = height;
    hr = pLogo->Set(&KPS_Logo);
  }

  return SUCCEEDED(hr);
}

// method to set the size of the logo
BOOL OspreyCaptureDevice::setLogoSize(PIN_ID pID, ULONG width, ULONG height)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the current logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    // set the size of the logo
    KPS_Logo.ulWidth  = width;
    KPS_Logo.ulHeight = height;
    hr = pLogo->Set(&KPS_Logo);
  }

  return SUCCEEDED(hr);
}


// method to set the size of the logo as a multiple of the .bmp file size
BOOL OspreyCaptureDevice::setLogoSizeScale(PIN_ID pID, int scale)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // make sure the scale is a reasonable number
  if ((scale < 1) || (scale > 10))
    return FALSE;

  // get the current logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    // set the size of the logo to the bmp size * scale
    KPS_Logo.ulWidth  = KPS_Logo.ulWidth1x  * scale;
    KPS_Logo.ulHeight = KPS_Logo.ulHeight1x * scale;
    hr = pLogo->Set(&KPS_Logo);
  }

  return SUCCEEDED(hr);
}

// method to get all of the logo settings at once
// logo file pointer MUST point to buffer large enough to hold 260 wide charactors
BOOL OspreyCaptureDevice::getLogoSettings(PIN_ID pID, WCHAR * pawLogoFile, CRect * logoRect, BOOL * logoEnabled, ULONG * keyColor, BOOL * keyColorEnabled, BOOL * transparent)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    wcscpy_s(pawLogoFile, LOGO_MAXPATH, KPS_Logo.awFName);
    logoRect->top    =  KPS_Logo.ulTop;
    logoRect->left   =  KPS_Logo.ulLeft;
    logoRect->bottom =  KPS_Logo.ulTop   + KPS_Logo.ulHeight;
    logoRect->right  =  KPS_Logo.ulLeft  + KPS_Logo.ulWidth;
    *keyColor        =  KPS_Logo.ulKeyColor;
    *logoEnabled     = ((KPS_Logo.ulFlags & LOGO_ENABLE)   != 0);
    *keyColorEnabled = ((KPS_Logo.ulFlags & LOGO_KEYCOLOR) != 0);
    *transparent     = ((KPS_Logo.ulFlags & LOGO_EMBOSSED) != 0);
  }

  return SUCCEEDED(hr);
}

// method to set all of the logo settings at once
BOOL OspreyCaptureDevice::setLogoSettings(PIN_ID pID, WCHAR* pawLogoFile, CRect logoRect, BOOL logoEnabled, ULONG keyColor, BOOL keyColorEnabled, BOOL transparent)
{
  // if no IOspreyLogo interface then there is nothing we can do
  if (!pLogo)
    return FALSE;

  // get the current logo settings
  KSPROPERTY_OSPREY_LOGO_S KPS_Logo;
  KPS_Logo.PinId = pID;
  HRESULT hr = pLogo->Get(&KPS_Logo);
  if (SUCCEEDED(hr))
  {
    // set the the logo settings
    wcscpy_s(KPS_Logo.awFName,LOGO_MAXPATH,pawLogoFile);
    KPS_Logo.ulTop            = logoRect.top;
    KPS_Logo.ulLeft           = logoRect.left;
    KPS_Logo.ulHeight         = logoRect.Height();
    KPS_Logo.ulWidth          = logoRect.Width();
    KPS_Logo.ulKeyColor       = keyColor;
    KPS_Logo.ulKeyColorCustom = keyColor;

    KPS_Logo.ulFlags &= ~(LOGO_ENABLE | LOGO_KEYCOLOR | LOGO_EMBOSSED);
    if (logoEnabled)
      KPS_Logo.ulFlags  |= LOGO_ENABLE;
    if (keyColorEnabled)
      KPS_Logo.ulFlags  |= LOGO_KEYCOLOR;
    if (transparent)
      KPS_Logo.ulFlags  |= LOGO_EMBOSSED;

    hr = pLogo->Set(&KPS_Logo);
  }

  return SUCCEEDED(hr);
}

// method to get the device name
// the input parameter must point to a buffer to hold the device name
// the buffer must be large enough to hold 32 WIDE Charactors i.e. 64 bytes
BOOL OspreyCaptureDevice::getDeviceName(WCHAR * devName)
{
  // if no IOspreyDevice interface then there is nothing we can do
  if (!pDevice)
    return FALSE;

  // get the device information
  KSPROPERTY_OSPREY_DVC_INFO_S KPS_DevInfo;
  memset(&KPS_DevInfo,0,sizeof(KPS_DevInfo));
  HRESULT hr = pDevice->GetDvcInfo(&KPS_DevInfo);
  if (SUCCEEDED(hr))
  {
    wcscpy_s(devName,32,KPS_DevInfo.awDeviceName);
    return TRUE;
  }

  return FALSE;
}

// method to get the device serial number
BOOL OspreyCaptureDevice::getSerialNumber(ULONG * num)
{
  // if no IOspreyDevice interface then there is nothing we can do
  if (!pDevice)
    return FALSE;

  // get the device information
  KSPROPERTY_OSPREY_DVC_INFO_S KPS_DevInfo;
  memset(&KPS_DevInfo,0,sizeof(KPS_DevInfo));
  HRESULT hr = pDevice->GetDvcInfo(&KPS_DevInfo);
  if (SUCCEEDED(hr))
  {
    *num = KPS_DevInfo.ulSerNum;
    return TRUE;
  }

  return FALSE;
}

// method to get the device instance number
// this is the device number in the device name
BOOL OspreyCaptureDevice::getInstanceNumber(ULONG * iNum)
{
  // if no IOspreyDevice interface then there is nothing we can do
  if (!pDevice)
    return FALSE;

  // get the device information
  KSPROPERTY_OSPREY_DVC_INFO_S KPS_DevInfo;
  memset(&KPS_DevInfo,0,sizeof(KPS_DevInfo));
  HRESULT hr = pDevice->GetDvcInfo(&KPS_DevInfo);
  if (SUCCEEDED(hr))
  {
    *iNum = KPS_DevInfo.ulDeviceId;
    return TRUE;
  }

  return FALSE;
}

// method to get the device Type number
BOOL OspreyCaptureDevice::getDeviceType(ULONG * devType)
{
  // if no IOspreyDevice interface then there is nothing we can do
  if (!pDevice)
    return FALSE;

  // get the device information
  KSPROPERTY_OSPREY_DVC_INFO_S KPS_DevInfo;
  memset(&KPS_DevInfo,0,sizeof(KPS_DevInfo));
  HRESULT hr = pDevice->GetDvcInfo(&KPS_DevInfo);
  if (SUCCEEDED(hr))
  {
    *devType = KPS_DevInfo.ulType;
    return TRUE;
  }

  return FALSE;
}

// method to get the device PCI Bus number
BOOL OspreyCaptureDevice::getBusNumber(ULONG * num)
{
  // if no IOspreyDevice interface then there is nothing we can do
  if (!pDevice)
    return FALSE;

  // get the device information
  KSPROPERTY_OSPREY_DVC_INFO_S KPS_DevInfo;
  memset(&KPS_DevInfo,0,sizeof(KPS_DevInfo));
  HRESULT hr = pDevice->GetDvcInfo(&KPS_DevInfo);
  if (SUCCEEDED(hr))
  {
    *num = KPS_DevInfo.ulBusNum;
    return TRUE;
  }

  return FALSE;
}

// method to get the device PCI Slot number
BOOL OspreyCaptureDevice::getSlotNumber(ULONG * num)
{
  // if no IOspreyDevice interface then there is nothing we can do
  if (!pDevice)
    return FALSE;

  // get the device information
  KSPROPERTY_OSPREY_DVC_INFO_S KPS_DevInfo;
  memset(&KPS_DevInfo,0,sizeof(KPS_DevInfo));
  HRESULT hr = pDevice->GetDvcInfo(&KPS_DevInfo);
  if (SUCCEEDED(hr))
  {
    *num = KPS_DevInfo.ulSlotNum;
    return TRUE;
  }

  return FALSE;
}

// method to get the field order
// BOOL variable is set to true if the field order is even-odd
BOOL OspreyCaptureDevice::getFieldOrder(BOOL* evenOdd)
{
  // if no IOspreyDevice interface then there is nothing we can do
  if (!pDevice)
    return FALSE;

  // get the device information
  KSPROPERTY_OSPREY_DEVICE_CONFIG_S KPS_DevConfig;
  memset(&KPS_DevConfig,0,sizeof(KPS_DevConfig));
  HRESULT hr = pDevice->GetConfig(&KPS_DevConfig);
  if (SUCCEEDED(hr))
  {
    // extract the value of the field order setting
    *evenOdd = (KPS_DevConfig.ulCfg & DVC_EVEN_ODD) != 0;
    return TRUE;
  }

  return FALSE;
}

// method to set the field order
// set BOOL variable true for even-odd field order
BOOL OspreyCaptureDevice::setFieldOrder(BOOL evenOdd)
{
  // if no IOspreyDevice interface then there is nothing we can do
  if (!pDevice)
    return FALSE;

  // get the device information
  KSPROPERTY_OSPREY_DEVICE_CONFIG_S KPS_DevConfig;
  memset(&KPS_DevConfig,0,sizeof(KPS_DevConfig));
  HRESULT hr = pDevice->GetConfig(&KPS_DevConfig);
  if (SUCCEEDED(hr))
  {
    // set the value of the field order setting
    if (evenOdd)
      KPS_DevConfig.ulCfg |=  DVC_EVEN_ODD;
    else
      KPS_DevConfig.ulCfg &= ~DVC_EVEN_ODD;
    return TRUE;
  }

  return FALSE;
}

// method to get the software deinterlace setting
BOOL OspreyCaptureDevice::getSWDeinterlace(BOOL* enable)
{
  // if no IOspreyDevice interface then there is nothing we can do
  if (!pDevice)
    return FALSE;

  // get the device information
  KSPROPERTY_OSPREY_DEVICE_CONFIG_S KPS_DevConfig;
  memset(&KPS_DevConfig, 0, sizeof(KPS_DevConfig));
  HRESULT hr = pDevice->GetConfig(&KPS_DevConfig);
  if (SUCCEEDED(hr))
  {
    // extract the value of the Software Deinterlace setting
    *enable = (KPS_DevConfig.ulCfg & DVC_SWDEILACE) != 0;
    return TRUE;
  }

  return FALSE;
}

// method to set the Software Deinterlace setting
BOOL OspreyCaptureDevice::setSWDeinterlace(BOOL enable)
{
  // if no IOspreyDevice interface then there is nothing we can do
  if (!pDevice)
    return FALSE;

  // get the device information
  KSPROPERTY_OSPREY_DEVICE_CONFIG_S KPS_DevConfig;
  memset(&KPS_DevConfig, 0, sizeof(KPS_DevConfig));
  HRESULT hr = pDevice->GetConfig(&KPS_DevConfig);
  if (SUCCEEDED(hr))
  {
    // set the value of the Software Deinterlace setting
    if (enable)
      KPS_DevConfig.ulCfg |=  DVC_SWDEILACE;
    else
      KPS_DevConfig.ulCfg &= ~DVC_SWDEILACE;
    return TRUE;
  }

  return FALSE;
}

