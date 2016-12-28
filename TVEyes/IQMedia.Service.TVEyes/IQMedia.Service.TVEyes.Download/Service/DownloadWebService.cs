using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.TVEyes.Common.Util;

namespace IQMedia.Service.TVEyes.Download.Service
{
    public class DownloadWebService : IDownload
    {
        public void WakeupService()
        {
            //Forcefully tell export to run.
            Logger.Info("Download kicked off by WCF Service.");
            Download.Instance.EnqueueTasks();
        }
    }
}
