
namespace IQMedia.Service.SM.GeneratePDF.Config.Sections
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
