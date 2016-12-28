using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Common.Util;

namespace IQMedia.Service.NM.GeneratePDF.Service
{
    public class GeneratePDFWebService : IGeneratePDFWebService
    {
        public void WakeupService()
        {
            //Forcefully tell generatepdf to run.
            Logger.Info("GeneratePDF kicked off by WCF Service.");
             GeneratePDFService.Instance.EnqueueTasks();
        }
    }
}
