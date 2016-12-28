using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using IQMediaGroup.Common.Util;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.CoreServices.Logic;
using IQMediaGroup.CoreServices.Serializers;

namespace IQMediaGroup.CoreServices.Commands
{
    public class GetFiveMinIngestParam : ICommand
    {
        public string _stationid { get; private set; }

        public GetFiveMinIngestParam(object stationid)
        {
            _stationid = (stationid is NullParameter) ? null : (string)stationid;
        }

        public void Execute(HttpRequest request, HttpResponse response)
        {
            string xmlResult = string.Empty;
            var fiveMinIngestParamOutput = new FiveMinIngestParamOutput();
            Logger.LogInfo("Get FiveMinIngestParam Request Started");
            try
            {
                if (string.IsNullOrWhiteSpace(_stationid))
                    throw new ArgumentException("Invalid or missing station ID");

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Logger.LogInfo("Get FiveMinIngestParam Input stationid: " + _stationid);
                }

                var iQIngestionLogic = (IQIngestionLogic)LogicFactory.GetLogic(LogicType.IQIngestion);
                fiveMinIngestParamOutput = iQIngestionLogic.GetFiveMinIngestParam(_stationid);

            }
            catch (ArgumentException ex)
            {
                fiveMinIngestParamOutput.status = 1;
                fiveMinIngestParamOutput.message = ex.Message;

            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error : " + ex.Message + " stack : " + ex.StackTrace);
                fiveMinIngestParamOutput.status = 1;
                fiveMinIngestParamOutput.message = "An error occurred, please try again.";
            }

            xmlResult = Serializer.SerializeToXml(fiveMinIngestParamOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("Get FiveMinIngestParam Output " + xmlResult);
            }

            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/xml";
            Logger.LogInfo("Get FiveMinIngestParam Request Ended");
            response.Output.Write(xmlResult);
        }
    }
}