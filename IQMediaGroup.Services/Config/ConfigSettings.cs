using IQMediaGroup.Services.Config.Sections.Mappings;
using IQMediaGroup.Services.Config.Sections;
using System.Configuration;

namespace IQMediaGroup.Services.Config
{
    public sealed class ConfigSettings
    {
        private const string MAPPING_SECTION = "Mappings";
        private const string SOLRURL_SETTINGS = "SolrUrlSettings";

        /// <summary>
        /// The Singleton instance of the Mappings Configuration Section
        /// </summary>
        public static Mappings Mappings
        {
            get { return ConfigurationManager.GetSection(MAPPING_SECTION) as Mappings; }
        }

        public static SolrUrlSettings SolrSettings
        {
            get { return ConfigurationManager.GetSection(SOLRURL_SETTINGS) as SolrUrlSettings; }
        }
    }
}