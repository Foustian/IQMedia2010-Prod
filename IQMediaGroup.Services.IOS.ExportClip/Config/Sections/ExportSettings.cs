using System.Collections.Generic;

namespace IQMediaGroup.Services.IOS.ExportClip.Config.Sections
{
    public class ExportSettings
    {

        public int PollInterval { get; set; }
        public int WorkerThreads { get; set; }
        public string WCFServicePort { get; set; }

        public string FtpHost { get; set; }
        public string FtpUsername { get; set; }
        public string FtpPassword { get; set; }
        public string FtpRootPath { get; set; }
        public bool FtpUsePassive { get; set; }
        public string FFMpegPath { get; set; }
        public string WorkingDirectory { get; set; }
    }
}

