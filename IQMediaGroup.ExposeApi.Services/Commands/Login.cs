using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Authentication;
using IQMediaGroup.ExposeApi.Domain;
using IQMediaGroup.ExposeApi.Logic;
using IQMediaGroup.ExposeApi.Services.Serializers;
using IQMedia.Web.Common;
using System.IO;
using System.Configuration;
using IQMediaGroup.Common.Util;
namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class Login : ICommand
    {
        public String _Format { get; private set; }

        public Login(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            string _JSONResult = string.Empty;
            string _JSONRequest = string.Empty;
            LoginOutput ResponseMessage = new LoginOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - Login Request Started");

            try
            {

                LoginInput loginInput = new LoginInput();

                StreamReader StreamReader = new StreamReader(p_HttpRequest.InputStream);

                System.Web.Security.FormsAuthenticationTicket FormsAuthenticationTicket = null;

                try
                {
                    _JSONRequest = StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - " + _JSONRequest);
                    }

                    if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
                    {
                        loginInput = (LoginInput)Serializer.DeserialiazeXml(_JSONRequest, loginInput);
                    }
                    else
                    {
                        loginInput = (LoginInput)Serializer.Deserialize(_JSONRequest, loginInput.GetType());
                    }

                    if (!string.IsNullOrEmpty(loginInput.SessionID))
                    {
                        FormsAuthenticationTicket = System.Web.Security.FormsAuthentication.Decrypt(loginInput.SessionID);
                    }
                }
                catch (Exception)
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.InputParsingErrorMessage);
                }

                if (FormsAuthenticationTicket == null || FormsAuthenticationTicket.Expired == true)
                {
                    AuthenticationLogic AuthenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);

                    if (!string.IsNullOrEmpty(loginInput.UserID) && !string.IsNullOrEmpty(loginInput.Password))
                    {

                        if (AuthenticationLogic.AuthenticateCustomer(loginInput.UserID, loginInput.Password, Convert.ToInt32(ConfigurationManager.AppSettings["MaxPasswordAttempts"])))
                        {
                            Authentication.LoginStatus LoginStatus = Authentication.Login(loginInput.UserID, loginInput.Password);

                            if (LoginStatus == Authentication.LoginStatus.Success)
                            {
                                ResponseMessage.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AuthencticationSuccessfullyMessage;
                                ResponseMessage.Status = 0;
                            }
                            else
                            {
                                ResponseMessage.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AuthencticationFailedMessage;
                                ResponseMessage.Status = -3;
                            }
                        }
                        else
                        {
                            ResponseMessage.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AuthencticationFailedMessage;
                            ResponseMessage.Status = -3;
                        }
                    }
                    else
                    {
                        throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.InvalidInputMessage);
                    }
                }
                else
                {
                    ResponseMessage.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.UserAlreadyLoggedInMessage;
                    ResponseMessage.Status = 1;
                }
            }
            catch (CustomException ex)
            {
                ResponseMessage.Message = ex.Message;
                ResponseMessage.Status = -2;
            }
            catch (AuthenticationException ex)
            {
                ResponseMessage.Message = ex.Message;
                ResponseMessage.Status = -3;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                ResponseMessage.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
                ResponseMessage.Status = -1;
            }

            if (HttpContext.Current.Response.Cookies.Count > 0 && HttpContext.Current.Response.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName] != null)
            {
                ResponseMessage.SessionID = HttpContext.Current.Response.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName].Value;

                HttpContext.Current.Response.Cookies.Remove(System.Web.Security.FormsAuthentication.FormsCookieName);
            }

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                p_HttpResponse.ContentType = "application/xml";
                _JSONResult = Serializer.SerializeToXmlWithoutNameSpace(ResponseMessage);
            }
            else
            {
                p_HttpResponse.ContentType = "application/json";
                _JSONResult = Serializer.Searialize(ResponseMessage);
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - " + _JSONResult);
            }

            p_HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - Login Request Ended");

            p_HttpResponse.Output.Write(_JSONResult);
        }
    }
}