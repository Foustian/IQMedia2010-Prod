using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;
using IQMediaGroup.CoreServices.Logic;
using System.Configuration;

namespace IQMediaGroup.CoreServices.Commands
{
    public class GetActiveRootPathIDs : ICommand
    {
        public string _ipaddr { get; private set; }

        public GetActiveRootPathIDs(object ipaddr)
        {
            _ipaddr = (ipaddr is NullParameter) ? null : (string)ipaddr;

        }
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            Log4NetLogger.Info("Get Active RootPaths Request Started.");
            string xmlResult = string.Empty;
            var activeRootPathOutput = new ActiveRootPathOutput();
            try
            {
                if (string.IsNullOrEmpty(_ipaddr))
                    throw new ArgumentException("invalid or missing ipAddress");

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Logger.LogInfo("Get Active RootPaths Input ipAddress: " + _ipaddr);
                }

                var rootPathLogic = (RootPathLogic)LogicFactory.GetLogic(LogicType.RootPath);
                activeRootPathOutput = rootPathLogic.GetActiveRootPath(_ipaddr);
                
            }
            catch (ArgumentException ex)
            {
                activeRootPathOutput.status = 1;
                activeRootPathOutput.message = ex.Message;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error : " + ex.Message + " stack : " + ex.StackTrace);
                activeRootPathOutput.message = "An error occurred, Please try again.";
                activeRootPathOutput.status = 1;
            }
            
            xmlResult = Serializers.Serializer.SerializeToXml(activeRootPathOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("Get Active RootPath Output :" + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Log4NetLogger.Info("Get Active RootPath IDs Request Ended.");
            HttpResponse.Output.Write(xmlResult);
            
        }
    }
}