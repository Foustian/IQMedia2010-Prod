using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Common.Util;

namespace IQMedia.Service.Media.News.GeneratePDF.Service
{
    public class NewsGeneratePDFWebService : INewsGeneratePDFWebService
    {
        public void WakeupService()
        {
            //Forcefully tell generatepdf to run.
            Logger.Info("GeneratePDF kicked off by WCF Service.");
             //GeneratePDFService.Instance.EnqueueTasks();
        }
    }
}
