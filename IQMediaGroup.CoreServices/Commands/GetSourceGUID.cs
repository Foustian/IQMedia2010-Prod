using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.CoreServices.Logic;
using System.Configuration;

namespace IQMediaGroup.CoreServices.Commands
{
    public class GetSourceGUID : ICommand
    {

        public string _sourceID { get; private set; }

        public GetSourceGUID(object sourceID)
        {
            _sourceID = (sourceID is NullParameter) ? null : (string)sourceID;
        }
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            Log4NetLogger.Info("Get Source GUID service started.");
            string xmlResult = string.Empty;
            var sourceGuidOutput = new SourceGuidOutput();
            try
            {
                if (string.IsNullOrEmpty(_sourceID))
                    throw new ArgumentException("invalid or missing sourceID");

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Logger.LogInfo("Get Source GUID Input sourceID: " + _sourceID);
                }

                var sourceLogic = (SourceLogic)LogicFactory.GetLogic(LogicType.Source);
                sourceGuidOutput = sourceLogic.GetSourceGUIDBySourceID(_sourceID);
                
            }
            catch (ArgumentException ex)
            {
                sourceGuidOutput.status = 1;
                sourceGuidOutput.message = ex.Message;

            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error : " + ex.Message + " stack : " + ex.StackTrace);
                sourceGuidOutput.message = "An error occurred, Please try again.";
                sourceGuidOutput.status = 1;
            }

            xmlResult = Serializers.Serializer.SerializeToXml(sourceGuidOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("Get Source GUID Output " + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Log4NetLogger.Info("Get Source GUID Request Ended.");
            HttpResponse.Output.Write(xmlResult);
            
        }

    }
}