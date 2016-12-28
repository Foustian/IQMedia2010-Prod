using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Domain;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Logic;
using System.IO;
using System.Configuration;
using IQMedia.Web.Common;
using System.Security.Authentication;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetNielSenData : ICommand
    {
        #region ICommand Members

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _JSONResult = string.Empty;
            string _JSONRequest = string.Empty;
            NielSenDataOutput _NielSenDataOutput = new NielSenDataOutput();
            try
            {
                CLogger.Debug("GetNielSenData Request Started");

                if (!Authentication.IsAuthenticated)
                {
                    throw new AuthenticationException();
                }

                NielSenDataInput _NielSenInput = new NielSenDataInput();

                StreamReader StreamReader = new StreamReader(HttpRequest.InputStream);

                try
                {
                    _JSONRequest = StreamReader.ReadToEnd();
                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        CLogger.Debug(_JSONRequest);
                    }
                    _NielSenInput = (NielSenDataInput)Serializer.Deserialize(_JSONRequest, _NielSenInput.GetType());
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                ValidationLogic ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);

                bool IsValidate = ValidationLogic.ValidateNielSenInput(_NielSenInput);

                if (IsValidate)
                {
                    NielSenDataLogic _NielSenDataLogic = (NielSenDataLogic)LogicFactory.GetLogic(LogicType.NielSen);

                    bool? HasAccess=false;

                    if (_NielSenInput.ClientGuid.HasValue && _NielSenInput.ClientGuid.Value!=new Guid())
                    {
                        HasAccess = _NielSenDataLogic.CheckClientNielSenDataAccess(_NielSenInput.ClientGuid.Value); 
                    }
                    else if(_NielSenInput.IsRawMedia.HasValue && !_NielSenInput.IsRawMedia.Value)
                    {
                        var clientLgc = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                        var clientGUID = clientLgc.GetClientGUIDByClipGUID(_NielSenInput.Guid.Value);

                        if (clientGUID!=null && clientGUID.HasValue)
                        {
                            HasAccess = _NielSenDataLogic.CheckClientNielSenDataAccess(clientGUID.Value);     
                        }
                    }

                    if (HasAccess.HasValue && HasAccess.Value == true)
                    {
                        _NielSenDataOutput = _NielSenDataLogic.GetNielSenData(_NielSenInput);
                    }
                    else
                    {
                        _NielSenDataOutput.Status = 1;
                        CLogger.Debug("Client Has No Access For NielSenData");
                    }
                }
                else
                {
                    _NielSenDataOutput.Status = 1;
                    CLogger.Debug("Invalid Input");
                }
            }
            catch (CustomException ex)
            {
                _NielSenDataOutput.Status = 1;
                CLogger.Debug(ex.Message);
            }
            catch (AuthenticationException)
            {
                _NielSenDataOutput.Status = 1;
                CLogger.Debug("User is not Authenticated.");

            }
            catch (Exception _Exception)
            {
                _NielSenDataOutput.Status = 1;
                CLogger.Error("Error : " + _Exception.ToString() + " stack : " + _Exception.StackTrace);
            }

            _JSONResult = Serializer.Searialize(_NielSenDataOutput);
            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug(_JSONResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/json";
            CLogger.Debug("GetNielSenData Request Ended");

            HttpResponse.Output.Write(_JSONResult);
        }

        #endregion
    }
}