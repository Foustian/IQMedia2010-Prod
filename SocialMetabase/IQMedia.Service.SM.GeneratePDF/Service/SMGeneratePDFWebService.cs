using IQMedia.Common.Util;

namespace IQMedia.Service.SM.GeneratePDF.Service
{
    public class SMGeneratePDFWebService : ISMGeneratePDFWebService
    {
        public void WakeupService()
        {
            //Forcefully tell generatepdf to run.
            Logger.Info("GeneratePDF kicked off by WCF Service.");
            GeneratePDFService.Instance.EnqueueTasks();
        }
    }
}
