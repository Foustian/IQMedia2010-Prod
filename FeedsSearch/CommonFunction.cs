using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.ComponentModel;

namespace FeedsSearch
{
    class CommonFunction
    {
        // TODO: Add new web.config settings
        public static void LogInfo(string Message, bool IsFeedsLogging = false, string FeedsLogFileLocation = "", bool OverrideConfig = false)
        {
            LogMessage(Message, "[INFO]", IsFeedsLogging, FeedsLogFileLocation, OverrideConfig);
        }

        public static void LogError(string Message, bool IsFeedsLogging = false, string FeedsLogFileLocation = "", bool OverrideConfig = false)
        {
            LogMessage(Message, "[ERROR]", IsFeedsLogging, FeedsLogFileLocation, OverrideConfig);
        }

        private static void LogMessage(string LogMessage, string MessageType, bool IsFeedsLogging, string FeedsLogFileLocation, bool OverrideConfig)
        {
            try
            {
                if ((ConfigurationManager.AppSettings["IsFeedsLogging"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["IsFeedsLogging"], System.Globalization.CultureInfo.CurrentCulture) == true) || OverrideConfig)
                {
                    string path = ConfigurationManager.AppSettings["FeedsLogFileLocation"] + "LOG_" + DateTime.Today.ToString("MMddyyyy", System.Globalization.CultureInfo.CurrentCulture) + ".csv";

                    if (!File.Exists(path))
                    {
                        File.Create(path).Close();
                    }
                    using (StreamWriter w = File.AppendText(path))
                    {
                        w.WriteLine(DateTime.Now.ToString() + " , " + MessageType + " ,\"" + LogMessage + "\"");
                    }
                }
                else if (ConfigurationManager.AppSettings["IsFeedsLogging"] == null && IsFeedsLogging == true && !string.IsNullOrEmpty(FeedsLogFileLocation))
                {
                    string path = FeedsLogFileLocation + "LOG_" + DateTime.Today.ToString("MMddyyyy", System.Globalization.CultureInfo.CurrentCulture) + ".csv";

                    if (!File.Exists(path))
                    {
                        File.Create(path).Close();
                    }
                    using (StreamWriter w = File.AppendText(path))
                    {
                        w.WriteLine(DateTime.Now.ToString() + " , " + MessageType + " ,\"" + LogMessage + "\"");
                    }
                }
            }
            catch (Exception)
            {
            }
        }



        public enum CategoryType
        {
            [Description("Social Media")]
            SocialMedia,
            [Description("Online News")]
            NM,
            [Description("Print Media")]
            PM,
            [Description("TV")]
            TV,
            [Description("Twitter")]
            TW,
            [Description("Forum")]
            Forum,
            [Description("Blog")]
            Blog,
            [Description("Radio")]
            Radio
        }
    }
}
