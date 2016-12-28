using System;
using System.ServiceModel;

namespace IQMediaGroup.Services.IOS.ExportClip.Service
{
    [ServiceContract]
    public interface IIOSExportWebService
    {
      
        [OperationContract]
        void WakeupService();
    }
}
