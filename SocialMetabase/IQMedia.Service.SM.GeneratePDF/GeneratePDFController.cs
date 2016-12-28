﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Common.Util;
using System.ServiceProcess;

namespace IQMedia.Service.SM.GeneratePDF
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
