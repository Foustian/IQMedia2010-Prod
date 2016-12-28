using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace AnalyticsSearch
{
    class CommonFunctions
    {
        public static void LogError(string msg)
        {
            LogMessage(msg, MessageType.ERROR);
        }

        public static void LogInfo(string msg)
        {
            LogMessage(msg, MessageType.INFO);
        }

        public static void LogDebug(string msg)
        {
            LogMessage(msg, MessageType.DEBUG);
        }

        private static void LogMessage(string msg, MessageType msgType)
        {
            try
            {
                string timeStamp = DateTime.Now.ToString("yyyyMMdd");
                string path = string.Format("C:\\Logs\\Download\\Analytics\\Search\\LOG_{0}.csv", timeStamp);

                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " , " + msgType + ",\"" + msg + "\"");
                }
            }
            catch (Exception exc)
            {

            }
        }

        public enum MessageType
        {
            INFO,
            ERROR,
            DEBUG
        }
    }
}
