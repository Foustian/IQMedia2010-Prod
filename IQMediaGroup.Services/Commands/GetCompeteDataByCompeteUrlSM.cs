using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Domain;
using IQMediaGroup.Common.Util;
using System.IO;
using System.Configuration;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetCompeteDataByCompeteUrlSM : ICommand
    {
        public String _Format { get; private set; }

        public GetCompeteDataByCompeteUrlSM(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _Input = string.Empty;
            string _Output = string.Empty;
            var _CompeteUrlOutput = new CompeteUrlOutput();

            CLogger.Debug("Get SM CompeteDataByURL  Request Started");

            try
            {
                StreamReader StreamReader = new StreamReader(HttpRequest.InputStream);
                CompeteUrlInput _CompeteUrlInput = new CompeteUrlInput();
                try
                {
                    _Input = StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        CLogger.Debug(_Input);
                    }

                    if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
                    {
                        _CompeteUrlInput = (CompeteUrlInput)Serializer.DeserialiazeXml(_Input, _CompeteUrlInput);
                    }
                    else
                    {
                        _CompeteUrlInput = (CompeteUrlInput)Serializer.Deserialize(_Input, _CompeteUrlInput.GetType());
                    }
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                if (!_CompeteUrlInput.ClientGuid.HasValue)
                    throw new CustomException("Invalid input");

                CompeteDataLogic _CompeteDataLogic = (CompeteDataLogic)LogicFactory.GetLogic(LogicType.Compete);
                
                _CompeteUrlOutput = _CompeteDataLogic.GetCompeteByCompeteURL(_CompeteUrlInput, CommonConstants.MediaType.SM);
            }
            catch (CustomException _Exception)
            {
                _CompeteUrlOutput.Message = _Exception.Message;
                _CompeteUrlOutput.Status = 1;
                CLogger.Debug(_Exception.Message);
            }
            catch (Exception _Exception)
            {
                _CompeteUrlOutput.Status = 1;
                _CompeteUrlOutput.Message = "An error occured, please try again!!";
                CLogger.Error("Error : " + _Exception.Message + " stack : " + _Exception.StackTrace);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                _Output = Serializer.SerializeToXmlWithoutNameSpace(_CompeteUrlOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                _Output = Serializer.JsonSearialize(_CompeteUrlOutput);
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug("Final result: " + _Output);
            }
            CLogger.Debug("Get SM CompeteDataByURL  Request Ended");
            HttpResponse.Output.Write(_Output);
        }
    }
}