using System.Xml.Serialization;

namespace IQMedia.Service.Ingestion.Config.Sections
{
    public class Directory
    {
        /// <summary>
        /// The SourcePath of the files to be ingested 
        /// specified in the App.config
        /// </summary>
        [XmlAttribute]
        public string SourcePath { get; set; }

        /// <summary>
        /// If marked true, all sub-directories will be polled
        /// and processed as well.
        /// </summary>
        [XmlAttribute]
        public string IncludeSubDirectories { get; set; }

        /// <summary>
        /// If specified, filters the allowed file-types for the
        /// ingestion service to process.
        /// </summary>
        [XmlAttribute]
        public string TypeFilters { get; set; }
    }
}
