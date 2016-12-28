using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Domain;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Services.Serializers;
using System.IO;
using System.Configuration;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetNielsen : ICommand
    {
        public String _Format { get; private set; }

        public GetNielsen(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _Input = string.Empty;
            string _Output = string.Empty;
            var _NielsenOutput = new NielsenOutput();
            CLogger.Debug("Get Nielsen Request Started");
            try
            {
                StreamReader StreamReader = new StreamReader(HttpRequest.InputStream);
                NielsenInput _NielsenInput = new NielsenInput();
                try
                {
                    _Input = StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        CLogger.Debug(_Input);
                    }

                    if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
                    {
                        _NielsenInput = (NielsenInput)Serializer.DeserialiazeXml(_Input, _NielsenInput);
                    }
                    else
                    {
                        _NielsenInput = (NielsenInput)Serializer.Deserialize(_Input, _NielsenInput.GetType());
                    }
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                if (!_NielsenInput.ClientGuid.HasValue)
                    throw new CustomException("Invalid input");

                NielSenDataLogic _NielSenDataLogic = (NielSenDataLogic)LogicFactory.GetLogic(LogicType.NielSen);

               /* bool? HasAccess = _NielSenDataLogic.CheckClientNielSenDataAccess(_NielsenInput.ClientGuid.Value);
                if (HasAccess.HasValue && HasAccess.Value == true)
                {*/
                    _NielsenOutput = _NielSenDataLogic.GetNielsenByList(_NielsenInput);
                /*}
                else
                {
                    _NielsenOutput.Status = 1;
                    CLogger.Debug("Client has no access for Nielsen");
                }*/
            }
            catch (CustomException _Exception)
            {
                _NielsenOutput.Message = _Exception.Message;
                _NielsenOutput.Status = 1;
                CLogger.Debug(_Exception.Message);
            }
            catch (Exception _Exception)
            {
                _NielsenOutput.Status = 1;
                _NielsenOutput.Message = "An error occured, please try again!!";
                CLogger.Error("Error : " + _Exception.Message + " stack : " + _Exception.StackTrace);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                _Output = Serializer.SerializeToXmlWithoutNameSpace(_NielsenOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                _Output = Serializer.Searialize(_NielsenOutput);
            }


            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug("Final result: " + _Output);
            }
            CLogger.Debug("Get Nielsen Request Ended");
            HttpResponse.Output.Write(_Output);
            

        }
    }
}