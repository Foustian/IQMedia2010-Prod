using System.Configuration;
using IQMediaGroup.Services.IOS.ExportClip.Config.Sections;

namespace IQMediaGroup.Services.IOS.ExportClip.Config
{
    public sealed class ConfigSettings
    {
        private const string EXPORT_SETTINGS = "ExportSettings";
        //private const string FFmpeg_Settings = "FFmpegSettings";

        /// <summary>
        /// The Singleton instance of the ExportSettings ConfigSection
        /// </summary>
        public static ExportSettings Settings
        {
            get { return ConfigurationManager.GetSection(EXPORT_SETTINGS) as ExportSettings; }
        }

        //public static FFmpegSettings FFmpegSettings
        //{
        //    get { return ConfigurationManager.GetSection(FFmpeg_Settings) as FFmpegSettings; }
        //}
    }
}
