using IQMedia.Common.Util;

namespace IQMedia.Service.Media.Social.GeneratePDF.Service
{
    public class SocialGeneratePDFWebService : ISocialGeneratePDFWebService
    {
        public void WakeupService()
        {
            //Forcefully tell generatepdf to run.
            Logger.Info("GeneratePDF kicked off by WCF Service.");
            //GeneratePDFService.Instance.EnqueueTasks();
        }
    }
}
