using System.Collections.Generic;

namespace IQMedia.Service.Ingestion.Config.Sections
{
    public class IngestionPaths
    {
        /// <summary>
        /// List of directories specified in the App.config
        /// for the Ingestion Service to poll and process files
        /// </summary>
        /// <value>The directories.</value>
        public List<Directory> Directories { get; set; }
    }
}
