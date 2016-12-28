﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using IQMedia.TVEyes.Common.Util;
using System.Xml.Linq;

namespace IQMedia.Service.TVEyes.RequestDownload
{
    static class RequestDownloadController
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (Environment.CommandLine.ToLower().Contains("-debug"))
            {
                Logger.Info("Starting Service in Debug...");
                using (var debugService = new RequestDownload())
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
                ServicesToRun = new ServiceBase[] { new RequestDownload() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
