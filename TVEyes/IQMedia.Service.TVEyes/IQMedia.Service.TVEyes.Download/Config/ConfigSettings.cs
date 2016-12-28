using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using IQMedia.Service.TVEyes.Download.Config.Sections;

namespace IQMedia.Service.TVEyes.Download.Config
{
    public sealed class ConfigSettings
    {
        private const string DOWNLOAD_SETTINGS = "DownloadSettings";

        /// <summary>
        /// The Singleton instance of the ExportSettings ConfigSection
        /// </summary>
        public static DownloadSettings Settings
        {
            get { return ConfigurationManager.GetSection(DOWNLOAD_SETTINGS) as DownloadSettings; }
        }

    }
}
