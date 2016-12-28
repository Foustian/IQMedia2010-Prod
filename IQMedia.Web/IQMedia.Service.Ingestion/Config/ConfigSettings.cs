using System.Configuration;
using IQMedia.Service.Ingestion.Config.Sections;

namespace IQMedia.Service.Ingestion.Config
{
    public sealed class ConfigSettings
    {
        private const string INGESTION_SETTINGS = "IngestionSettings";
        private const string INGESTION_PATHS = "IngestionPaths";

        /// <summary>
        /// The Singleton instance of the IngestionSettings ConfigSection
        /// </summary>
        public static IngestionSettings Settings
        {
            get { return ConfigurationManager.GetSection(INGESTION_SETTINGS) as IngestionSettings; }
        }

        /// <summary>
        /// The Singleton instance of the IngestionPaths ConfigSection
        /// </summary>
        public static IngestionPaths Paths
        {
            get { return ConfigurationManager.GetSection(INGESTION_PATHS) as IngestionPaths; }
        }
    }
}
