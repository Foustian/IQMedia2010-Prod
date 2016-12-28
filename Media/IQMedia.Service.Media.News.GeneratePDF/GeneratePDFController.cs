using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using IQMedia.Common.Util;

namespace IQMedia.Service.Media.News.GeneratePDF
{
    static class GeneratePDFController
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (Environment.CommandLine.ToLower().Contains("-debug"))
            {
                Logger.Info("Starting Service in Debug...");
                using (var debugService = new GeneratePDFService())
                {
                    debugService.Run();
                    Logger.Info("Service started. Press 'Enter' to exit.");
                    Console.ReadLine();
                    debugService.Quit();
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new GeneratePDFService() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
