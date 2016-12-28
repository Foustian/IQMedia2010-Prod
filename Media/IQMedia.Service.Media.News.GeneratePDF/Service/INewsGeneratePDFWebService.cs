using System.ServiceModel;

namespace IQMedia.Service.Media.News.GeneratePDF.Service
{
    [ServiceContract]
    public interface INewsGeneratePDFWebService
    {
        [OperationContract]
        void WakeupService();
    }
}
