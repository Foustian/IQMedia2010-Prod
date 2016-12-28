using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.IO;
using System.Configuration;
using IQMediaGroup.CoreServices.Logic;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.CoreServices.Serializers;

namespace IQMediaGroup.CoreServices.Commands
{
    public class InsertErrorLogRecord : ICommand
    {

        #region ICommand Members

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string inputXML = string.Empty;
            string ingestionLogOutput = string.Empty;
            Logger.LogInfo("Insert IQLog_Ingestion Request Started");
            IQLog_IngestionOutput iQLog_IngestionOutput = new IQLog_IngestionOutput();
            try
            {
                StreamReader _StreamReader = new StreamReader(HttpRequest.InputStream);
                IQLog_IngestionInput iQLog_IngestionInput = new IQLog_IngestionInput();
                try
                {
                    inputXML = _StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        Logger.LogInfo("IQLog_Ingestion Input : " + inputXML);
                    }
                    iQLog_IngestionInput = (IQLog_IngestionInput)Serializer.DeserialiazeXml(inputXML, iQLog_IngestionInput);
                }
                catch (Exception ex)
                {
                    throw new CustomException("Error during parsing IQLog_Ingestion input");
                }

                ValidationLogic _ValidationLogic = new ValidationLogic();

                bool IsValidate = true;
                if (!string.IsNullOrWhiteSpace(inputXML.Trim()))
                {
                    IsValidate = _ValidationLogic.ValidateIngestionLogInput(iQLog_IngestionInput);
                }
                else
                {
                    IsValidate = false;
                }

                if (!IsValidate)
                {
                    iQLog_IngestionOutput.Message = "Invalid Input";
                    iQLog_IngestionOutput.Status = 1;
                    ingestionLogOutput = Serializer.SerializeToXml(iQLog_IngestionOutput);
                }
                else
                {

                    IQIngestionLogic iQIngestionLogic = new IQIngestionLogic();
                    Int64 Result = iQIngestionLogic.InsertIQLog_Ingestion(iQLog_IngestionInput);

                    if (Result > 0)
                    {
                        iQLog_IngestionOutput.Message = "ErrorLogRecord Inserted Successfully";
                        iQLog_IngestionOutput.Status = 0;
                        ingestionLogOutput = Serializer.SerializeToXml(iQLog_IngestionOutput);
                    }
                    else
                    {

                        iQLog_IngestionOutput.Message = "No ErrorLogRecord Inserted";
                        iQLog_IngestionOutput.Status = 1;
                        ingestionLogOutput = Serializer.SerializeToXml(iQLog_IngestionOutput);
                    }
                }
            }
            catch (CustomException _CustomException)
            {
                iQLog_IngestionOutput.Message = _CustomException.Message;
                iQLog_IngestionOutput.Status = 1;
                ingestionLogOutput = Serializer.SerializeToXml(iQLog_IngestionOutput);
            }
            catch (Exception _ex)
            {
                Logger.LogInfo("Error : " + _ex.Message + " stack : " + _ex.StackTrace);
                iQLog_IngestionOutput.Message = "An error occurred, please try again." + _ex.Message + " inner Exception=" + _ex.InnerException;
                iQLog_IngestionOutput.Status = 1;
                ingestionLogOutput = Serializer.SerializeToXml(iQLog_IngestionOutput);

            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("IQLog_Ingestion Output : " + ingestionLogOutput);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Logger.LogInfo("Insert IQLog_Ingestion Request Ended");
            HttpResponse.Output.Write(ingestionLogOutput);
        }

        #endregion


    }
}