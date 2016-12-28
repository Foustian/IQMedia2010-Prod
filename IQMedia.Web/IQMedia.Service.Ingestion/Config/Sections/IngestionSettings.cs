namespace IQMedia.Service.Ingestion.Config.Sections
{
    /// <summary>
    /// The IngestionSettings ConfigSection in the App.Config
    /// See the config file for comments about setting these values.
    /// </summary>
    public class IngestionSettings
    {
        public int PollInterval { get; set; }
        public int WorkerThreads { get; set; }
        public int NumberOfFiles { get; set; }

        public string RecordingStatus_CC { get; set; }
        public string RecordingStatus_Audio { get; set; }
        public string RecordingStatus_Video { get; set; }

        public string SourcePattern { get; set; }
        public string DestinationPattern { get; set; }
    }
}
