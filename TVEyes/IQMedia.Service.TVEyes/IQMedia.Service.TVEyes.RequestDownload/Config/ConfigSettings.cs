using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using IQMedia.Service.TVEyes.RequestDownload.Config.Sections;

namespace IQMedia.Service.TVEyes.RequestDownload.Config
{
    public sealed class ConfigSettings
    {
        private const string REQUESTDOWNLOAD_SETTINGS = "RequestDownloadSettings";

        /// <summary>
        /// The Singleton instance of the ExportSettings ConfigSection
        /// </summary>
        public static RequestDownloadSettings Settings
        {
            get { return ConfigurationManager.GetSection(REQUESTDOWNLOAD_SETTINGS) as RequestDownloadSettings; }
        }

    }
}
