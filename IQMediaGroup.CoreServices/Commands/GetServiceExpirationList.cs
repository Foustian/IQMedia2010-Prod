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
    public class GetServiceExpirationList : ICommand
    {
        public String _RPSiteID { get; private set; }
        public int? _NumRecord { get; private set; }

        public GetServiceExpirationList(object RPSiteID, object NumRecord)
        {
            _RPSiteID = (RPSiteID is NullParameter) ? null : Convert.ToString(RPSiteID);
            _NumRecord = (NumRecord is NullParameter) ? null : (int?)NumRecord;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string xmlResult = string.Empty;
            Logger.LogInfo("GetServiceExpirationList Request Started");
            var getServiceExpirationOutput = new GetServiceExpirationOutput();

            try
            {
                if (string.IsNullOrWhiteSpace(_RPSiteID))
                {
                    throw new ArgumentException("Invalid or missing RPSiteID");
                }
                if (_NumRecord == null)
                {
                    _NumRecord = 10;
                }
                bool isRemoteLocation = true;

                if (_RPSiteID == "ILG")
                {
                    isRemoteLocation = false;
                }

                var serviceExpirationLogic = (ServiceExpirationLogic)LogicFactory.GetLogic(LogicType.ServiceExpiration);
                var serviceExpirationList = serviceExpirationLogic.GetServiceExpirationList(_RPSiteID, _NumRecord, isRemoteLocation);

                getServiceExpirationOutput.status = 0;
                getServiceExpirationOutput.message = "Success";
                getServiceExpirationOutput.GUIDList = new ServiceExpirationData();
                ServiceExpirationData serviceExpirationData = new ServiceExpirationData();
                serviceExpirationData.listofMediaLocationOutput = serviceExpirationList;
                getServiceExpirationOutput.GUIDList = serviceExpirationData;

            }
            catch (ArgumentException ex)
            {
                getServiceExpirationOutput.status = 1;
                getServiceExpirationOutput.message = ex.Message;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error : " + ex.Message + " stack : " + ex.StackTrace);
                getServiceExpirationOutput.status = 1;
                getServiceExpirationOutput.message = "An error occurred, Please try again.";
            }

            xmlResult = Serializers.Serializer.SerializeToXml(getServiceExpirationOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("GetServiceExpirationList Output :" + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Log4NetLogger.Info("GetServiceExpirationList Request Ended.");
            HttpResponse.Output.Write(xmlResult);
        }
    }
}