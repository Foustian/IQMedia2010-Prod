using IQMedia.Service.SM.GeneratePDF.Config.Sections;
using System.Configuration;

namespace IQMedia.Service.SM.GeneratePDF.Config
{
    public sealed class ConfigSettings
    {
        private const string GeneratePDF_SETTINGS = "GeneratePDFSettings";

        /// <summary>
        /// The Singleton instance of the GeneratePDFSettings ConfigSection
        /// </summary>
        public static GeneratePDFSettings Settings
        {
            get { return ConfigurationManager.GetSection(GeneratePDF_SETTINGS) as GeneratePDFSettings; }
        }
    }
}
