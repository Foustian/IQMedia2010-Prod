using System.ServiceModel;

namespace IQMedia.Service.Media.Social.GeneratePDF.Service
{
    [ServiceContract]
    public interface ISocialGeneratePDFWebService
    {
        [OperationContract]
        void WakeupService();
    }
}
