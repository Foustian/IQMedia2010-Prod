using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Sentiment.Config.Sections;

namespace Sentiment.Config
{
    public sealed class ConfigSettings
    {
        private const string SENTIMENT_SETTINGS = "SentimentSettings";
        

        /// <summary>
        /// The Singleton instance of the ExportSettings ConfigSection
        /// </summary>
        public static SentimentSettings Settings
        {
            get { return ConfigurationManager.GetSection(SENTIMENT_SETTINGS) as SentimentSettings; }
        }

        
    }
}
