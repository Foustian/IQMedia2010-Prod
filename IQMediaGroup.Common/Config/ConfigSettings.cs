using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using IQMediaGroup.Common.Config.Sections;

namespace IQMediaGroup.Common.Config
{
    public sealed class ConfigSettings
    {
        private const string MESSAGE_SECTION = "MessagesSection";

        /// <summary>
        /// The Singleton instance of the Mappings Configuration Section
        /// </summary>
        public static MessagesSection MessagesSection
        {
            get { return ConfigurationManager.GetSection(MESSAGE_SECTION) as MessagesSection; }
        }

    }
}