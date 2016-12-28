using System.Configuration;
using IQMedia.Services.SMS.Config.Sections.Mappings;
using IQMedia.Services.SMS.Config.Sections;

namespace IQMedia.Services.SMS.Config
{
    public sealed class ConfigSettings
    {
        private const string MAPPING_SECTION = "Mappings";
        private const string APPSETTINGS_SECTION = "appSettings";

        /// <summary>
        /// The Singleton instance of the Mappings Configuration Section
        /// </summary>
        public static Mappings Mappings
        {
            get { return ConfigurationManager.GetSection(MAPPING_SECTION) as Mappings; }
        }

        public static AppSettings AppSettings
        { get { return ConfigurationManager.GetSection(APPSETTINGS_SECTION) as AppSettings; } }
    }
}