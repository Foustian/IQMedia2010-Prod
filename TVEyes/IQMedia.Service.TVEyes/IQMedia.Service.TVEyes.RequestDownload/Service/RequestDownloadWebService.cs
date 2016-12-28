using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.TVEyes.Common.Util;

namespace IQMedia.Service.TVEyes.RequestDownload.Service
{
    public class RequestDownloadWebService : IRequestDownload
    {
        public void WakeupService()
        {
            //Forcefully tell export to run.
            Logger.Info("DiscoveryReportGenerate kicked off by WCF Service.");
            RequestDownload.Instance.EnqueueTasks();
        }
    }
}
