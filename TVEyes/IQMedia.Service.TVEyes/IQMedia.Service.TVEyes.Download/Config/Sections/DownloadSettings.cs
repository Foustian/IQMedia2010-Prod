using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Service.TVEyes.Download.Config.Sections
{
    public class DownloadSettings
    {
        public int PollInterval { get; set; }
        public int WorkerThreads { get; set; }
        public string WCFServicePort { get; set; }
        public string WorkingDirectory { get; set; }
        public string FfmpegParametersMp3 { get; set; }
        public string ProcessDirectory { get; set; }
        public string FFMpegPath { get; set; }
    }
}
