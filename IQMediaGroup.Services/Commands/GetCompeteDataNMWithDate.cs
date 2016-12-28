using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Domain;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetCompeteDataNMWithDate : ICommand
    {
        public String _Format { get; private set; }

        public GetCompeteDataNMWithDate(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _Input = string.Empty;
            string _Output = string.Empty;
            var _CompeteOutput = new CompeteOutput();
            CLogger.Debug("Get CompeteData NM Request Started");



            try
            {
                StreamReader StreamReader = new StreamReader(HttpRequest.InputStream);
                CompeteInput _CompeteInput = new CompeteInput();
                try
                {
                    _Input = StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        CLogger.Debug(_Input);
                    }

                    if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
                    {
                        _CompeteInput = (CompeteInput)Serializer.DeserialiazeXml(_Input, _CompeteInput);
                    }
                    else
                    {
                        _CompeteInput = (CompeteInput)Serializer.Deserialize(_Input, _CompeteInput.GetType());
                    }
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                if (!_CompeteInput.ClientGuid.HasValue)
                    throw new CustomException("Invalid input");

                CompeteDataLogic _CompeteDataLogic = (CompeteDataLogic)LogicFactory.GetLogic(LogicType.Compete);

               /* bool? HasAccess = _CompeteDataLogic.CheckClientCompeteDataAccess(_CompeteInput.ClientGuid.Value);
                if (HasAccess.HasValue && HasAccess.Value == true)
                {*/
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                FromDate = !string.IsNullOrEmpty(_CompeteInput.FromDate) ? (DateTime?)Convert.ToDateTime(_CompeteInput.FromDate) : null;
                ToDate = !string.IsNullOrEmpty(_CompeteInput.ToDate) ? (DateTime?)Convert.ToDateTime(_CompeteInput.ToDate) : null;
                string _PmgUrl = BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.MO.ToString(), FromDate, ToDate);
                _CompeteOutput = _CompeteDataLogic.GetCompeteByListNM(_CompeteInput, _PmgUrl);
                /*}
                else
                {
                    _CompeteOutput.Status = 1;
                    CLogger.Debug("Client has no access for Compete Data");
                }*/
            }
            catch (CustomException _Exception)
            {
                _CompeteOutput.Message = _Exception.Message;
                _CompeteOutput.Status = 1;
                CLogger.Debug(_Exception.Message);
            }
            catch (Exception _Exception)
            {
                _CompeteOutput.Status = 1;
                _CompeteOutput.Message = "An error occured, please try again!!";
                CLogger.Error("Error : " + _Exception.Message + " stack : " + _Exception.StackTrace);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                _Output = Serializer.SerializeToXmlWithoutNameSpace(_CompeteOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                _Output = Serializer.Searialize(_CompeteOutput);
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug("Final result: " + _Output);
            }
            CLogger.Debug("Get CompeteData NM Request Ended");
            HttpResponse.Output.Write(_Output);
            

        }
    }
}