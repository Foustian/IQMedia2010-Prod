using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using IQMediaGroup.Common.IOS.Util;

namespace IQMediaGroup.Services.IOS.ExportClip
{
    static class ExportClipController
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (Environment.CommandLine.ToLower().Contains("-debug"))
            {
                Log4NetLogger.Info("Starting Service in Debug...");
                using (var debugService = new ExportService())
                {
                    debugService.Run();
                    Log4NetLogger.Info("Service started. Press 'Enter' to exit.");
                    Console.ReadLine();
                    debugService.Quit();
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new ExportService() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
