using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Service.Media.News.GeneratePDF.Config.Sections;
using System.Configuration;

namespace IQMedia.Service.Media.News.GeneratePDF.Config
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
