using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Service.NM.GeneratePDF.Config.Sections
{
    public class GeneratePDFSettings
    {
        public int PollInterval { get; set; }
        public int WorkerThreads { get; set; }
        public string WCFServicePort { get; set; }

        public string WorkingDirectory { get; set; }

        public string wkhtmltopdfPath { get; set; }        
    }
}
