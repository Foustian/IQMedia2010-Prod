using System;
using System.Configuration;
using System.IO;

namespace IQMediaGroup.Common.Util
{
    public static class Logger
    {
        public static void LogInfo(string LogMessage)
        {
            try
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLOGWrite"]) == true)
                {
                    string path = ConfigurationManager.AppSettings["LOGAdvancedSearchServicesFileLocation"] + "LOG_" + DateTime.Today.ToString("MMddyyyy") + ".csv";
                    if (!File.Exists(path))
                    {
                        File.Create(path).Close();
                        
                    }
                    using (StreamWriter w = File.AppendText(path))
                    {
                        w.WriteLine(DateTime.Now.ToString() + " , [INFO] ,\"" + LogMessage + "\"");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}