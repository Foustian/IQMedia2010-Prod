using System.ServiceModel;

namespace IQMedia.Service.SM.GeneratePDF.Service
{
    [ServiceContract]
    public interface ISMGeneratePDFWebService
    {
        [OperationContract]
        void WakeupService();
    }
}
