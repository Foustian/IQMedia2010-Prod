using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Domain;
using System.IO;
using System.Configuration;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetCompeteDataSM : ICommand
    {
        public String _Format { get; private set; }

        public GetCompeteDataSM(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string input = string.Empty;
            string output = string.Empty;
            var competeOutput = new CompeteOutput();

            CLogger.Debug("Get CompeteData SM Request Started");

            try
            {
                StreamReader StreamReader = new StreamReader(HttpRequest.InputStream);
                CompeteInput competeInput = new CompeteInput();
                try
                {
                    input = StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        CLogger.Debug(input);
                    }

                    if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
                    {
                        competeInput = (CompeteInput)Serializer.DeserialiazeXml(input, competeInput);
                    }
                    else
                    {
                        competeInput = (CompeteInput)Serializer.Deserialize(input, competeInput.GetType());
                    }
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                if (!competeInput.ClientGuid.HasValue)
                    throw new CustomException("Invalid input");

                CompeteDataLogic competeDataLogic = (CompeteDataLogic)LogicFactory.GetLogic(LogicType.Compete);

                /*bool? HasAccess = competeDataLogic.CheckClientCompeteDataAccess(competeInput.ClientGuid.Value);
                if (HasAccess.HasValue && HasAccess.Value == true)
                {*/
                competeOutput = competeDataLogic.GetCompeteByListSM(competeInput, BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.MO.ToString(), (!string.IsNullOrEmpty(competeInput.FromDate) ? (DateTime?)Convert.ToDateTime(competeInput.FromDate) : null), (!string.IsNullOrEmpty(competeInput.ToDate) ? (DateTime?)Convert.ToDateTime(competeInput.ToDate) : null)));
                /*}
                else
                {
                    competeOutput.Status = 1;
                    competeOutput.Message = "Client has no access for Compete Data.";
                    CLogger.Debug("Client has no access for Compete Data.");
                }*/
            }
            catch (CustomException ex)
            {
                competeOutput.Message = ex.Message;
                competeOutput.Status = 1;
                CLogger.Error(ex.Message);
            }
            catch (Exception ex)
            {
                competeOutput.Status = 1;
                competeOutput.Message = "An error occured, please try again!!";
                CLogger.Error("Error : " + ex.ToString() + " stack : " + ex.StackTrace);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                output = Serializer.SerializeToXmlWithoutNameSpace(competeOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                output = Serializer.Searialize(competeOutput);
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug("Final result: " + output);
            }
            CLogger.Debug("Get CompeteData SM Request Ended");
            HttpResponse.Output.Write(output);
        }
    }
}