using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;
using System.Configuration;
using IQMediaGroup.CoreServices.Logic;
using IQMediaGroup.CoreServices.Serializers;

namespace IQMediaGroup.CoreServices.Commands
{
    public class GetOneHourIngestParam : ICommand
    {

        public string _stationid { get; private set; }

        public GetOneHourIngestParam(object stationid)
        {
            _stationid = (stationid is NullParameter) ? null : (string)stationid;
        }

        public void Execute(HttpRequest request, HttpResponse response)
        {
            string xmlResult = string.Empty;
            var oneHourIngestParamOutput = new OneHourIngestParamOutput();
            Logger.LogInfo("Get OneHourIngestParam Request Started");
            try
            {
                if (string.IsNullOrWhiteSpace(_stationid))
                    throw new ArgumentException("Invalid or missing station ID");

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Logger.LogInfo("Get OneHourIngestParam Input stationid: " + _stationid);
                }

                var iQIngestionLogic = (IQIngestionLogic)LogicFactory.GetLogic(LogicType.IQIngestion);
                oneHourIngestParamOutput = iQIngestionLogic.GetOneHourIngestParam(_stationid);

            }
            catch (ArgumentException ex)
            {
                oneHourIngestParamOutput.status = 1;
                oneHourIngestParamOutput.message = ex.Message;

            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error : " + ex.Message + " stack : " + ex.StackTrace);
                oneHourIngestParamOutput.status = 1;
                oneHourIngestParamOutput.message = "An error occurred, please try again.";
            }

            xmlResult = Serializer.SerializeToXml(oneHourIngestParamOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("Get OneHourIngestParam Output " + xmlResult);
            }

            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/xml";
            Logger.LogInfo("Get OneHourIngestParam Request Ended");
            response.Output.Write(xmlResult);
        }
    }
}