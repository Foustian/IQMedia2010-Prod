using System.ServiceModel;

namespace IQMedia.Service.NM.GeneratePDF.Service
{
    [ServiceContract]
    public interface IGeneratePDFWebService
    {
        [OperationContract]
        void WakeupService();
    }
}
