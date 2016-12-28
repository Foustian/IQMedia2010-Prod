using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Domain;
using IQMediaGroup.Common.Util;
using System.Configuration;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetIQAgentIframeURL : ICommand
    {

        public Guid? _RawMediaID { get; private set; }
        public DateTime? _Date { get; private set; }
        public Int64? _IQAgentResultID { get; private set; }

        public GetIQAgentIframeURL(object RawMediaID, object Date, object IQAgentResultID)
        {
            _RawMediaID = (RawMediaID is NullParameter) ? null : (Guid?)RawMediaID;
            _Date = (Date is NullParameter) ? null : (DateTime?)Date;
            _IQAgentResultID = (IQAgentResultID is NullParameter) ? null : (Int64?)IQAgentResultID;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string xmlResult = string.Empty;
            CLogger.Debug("Get IQAgent IFrame URL Started");
            try
            {

                if (_RawMediaID == null && _IQAgentResultID == null)
                {
                    throw new ArgumentException("Invalid or missing RawMediaID or IQAgentResultID");
                }
                if (_Date == null)
                {
                    throw new ArgumentException("Invalid or missing Date");
                }

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    CLogger.Debug("{\"RawMediaID\":\"" + _RawMediaID + "\",\"Date\":\"" + _Date + "\"}");
                }

                var encryptLogic = (IQAgentIframeLogic)LogicFactory.GetLogic(LogicType.IQAgentFrame);

                //jsonResult = encryptLogic.GetIQAgentIFrameLink(_RawMediaID, _Date);
                var iQAgentIframeOutput = encryptLogic.InsertIQAgentIframe(_RawMediaID, _Date, _IQAgentResultID, "TV");

                xmlResult = Serializer.SerializeToXml(iQAgentIframeOutput);
                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    CLogger.Debug("Final result: " + xmlResult);
                }


            }
            catch (CustomException _CustomException)
            {
                var iQAgentIframeOutput = new IQAgentIframeOutput();
                iQAgentIframeOutput.Status = 1;
                iQAgentIframeOutput.Message = _CustomException.Message;
                xmlResult = Serializer.SerializeToXml(iQAgentIframeOutput);
            }
            catch (Exception ex)
            {
                var iQAgentIframeOutput = new IQAgentIframeOutput();
                iQAgentIframeOutput.Status = 1;
                iQAgentIframeOutput.Message = ex.Message;
                xmlResult = Serializer.SerializeToXml(iQAgentIframeOutput);
                CLogger.Error("Error : " + ex.Message + "\n Inner Exception" + ex.InnerException + "\n stack : " + ex.StackTrace);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug(xmlResult);
            }
            CLogger.Debug("Get IQAgent IFrame URL Ended");
            HttpResponse.Output.Write(xmlResult);

        }
    }
}