using System.Configuration;
using IQMediaGroup.ExposeApi.Logic.Config.Sections;

namespace IQMediaGroup.ExposeApi.Logic.Config
{
    public sealed class ConfigSettings
    {
        private const string MESSAGE_SETTINGS = "MessageSettings";


        public static MessageSettings MessageSettings
        {
            get { return ConfigurationManager.GetSection(MESSAGE_SETTINGS) as MessageSettings; }
        }
    }
}