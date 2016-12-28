using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.Configuration;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Domain;
using System.IO;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetIQLicenseUrl : ICommand
    {
        public String _Format { get; private set; }

        public GetIQLicenseUrl(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _Input = string.Empty;
            string _Output = string.Empty;
            var _IQLicenseUrlOutput = new IQLicenseUrlOutput();
            CLogger.Debug("Get Nielsen Request Started");
            try
            {
                StreamReader StreamReader = new StreamReader(HttpRequest.InputStream);
                IQLicenseUrlInput _IQLicenseUrlInput = new IQLicenseUrlInput();
                try
                {
                    _Input = StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        CLogger.Debug(_Input);
                    }

                    if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
                    {
                        _IQLicenseUrlInput = (IQLicenseUrlInput)Serializer.DeserialiazeXml(_Input, _IQLicenseUrlInput);
                    }
                    else
                    {
                        _IQLicenseUrlInput = (IQLicenseUrlInput)Serializer.Deserialize(_Input, _IQLicenseUrlInput.GetType());
                    }
                }
                catch (Exception ex)
                {
                    throw new CustomException("Error during parsing input");
                }

                LicenseLogic _LicenseLogic = (LicenseLogic)LogicFactory.GetLogic(LogicType.License);
                
                _IQLicenseUrlOutput = _LicenseLogic.GetLicenseUrl(_IQLicenseUrlInput);

            }
            catch (CustomException _Exception)
            {
                _IQLicenseUrlOutput.Message = _Exception.Message;
                _IQLicenseUrlOutput.Status = 1;
                CLogger.Debug(_Exception.Message);
            }
            catch (Exception _Exception)
            {
                _IQLicenseUrlOutput.Status = 1;
                _IQLicenseUrlOutput.Message = "An error occured, please try again!!";
                CLogger.Error("Error : " + _Exception.Message + " stack : " + _Exception.StackTrace);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                _Output = Serializer.SerializeToXmlWithoutNameSpace(_IQLicenseUrlOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                _Output = Serializer.Searialize(_IQLicenseUrlOutput);
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