using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Service.TVEyes.RequestDownload.Config.Sections
{
    public class RequestDownloadSettings
    {
        public int PollInterval { get; set; }
        public int WorkerThreads { get; set; }
        public string WCFServicePort { get; set; }
        public string DownloadRequestURL { get; set; }
        public string ProxyServerURL { get; set; }
        public Int64 ParentID { get; set; }
    }
}
