using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using IQMediaGroup.Common.Util;
using IQMediaGroup.ExposeApi.Services.Serializers;
using IQMediaGroup.ExposeApi.Domain;
using System.Web.Security;
using IQMedia.Web.Common;
using System.Security.Authentication;
using IQMediaGroup.ExposeApi.Logic;
using Newtonsoft.Json;

namespace IQMediaGroup.ExposeApi.Services.Util
{
    public class CommonFunctions
    {
        public static T InitializeRequest<T>(HttpRequest p_httpRequest, string p_Format,bool p_HasCookie=false)
        {
            try
            {
                string request = string.Empty;

                using (StreamReader StreamReader = new StreamReader(p_httpRequest.InputStream))
                {
                    request = StreamReader.ReadToEnd();
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogRequestResponse"]) == true)
                {
                    Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - " + request);
                }

                var inputObject = Activator.CreateInstance<T>();

                try
                {
                    if (!string.IsNullOrWhiteSpace(p_Format) && string.Compare(p_Format, CommonConstants.formatType.xml.ToString(), true) == 0)
                    {
                        inputObject = (T)Serializer.DeserialiazeXmlDC<T>(request, inputObject);
                    }
                    else
                    {
                        inputObject = (T)Serializer.Deserialize(request, inputObject.GetType());
                    }

                    if (!p_HasCookie)
                    {
                        FormsAuthenticationTicket formAuthTicket = System.Web.Security.FormsAuthentication.Decrypt(Convert.ToString(inputObject.GetType().GetProperty("SessionID").GetValue(inputObject, null)));

                        string[] roles = formAuthTicket.UserData.Split(new char[] { '|' });

                        System.Security.Principal.IIdentity id = new System.Web.Security.FormsIdentity(formAuthTicket);

                        System.Security.Principal.IPrincipal principal = new System.Security.Principal.GenericPrincipal(id, roles);

                        HttpContext.Current.User = principal;

                        if (!Authentication.IsAuthenticated || (formAuthTicket == null || formAuthTicket.Expired))
                        {
                            throw new AuthenticationException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AuthencticationFailedMessage);
                        } 
                    }
                    else
                    {
                        if (!Authentication.IsAuthenticated)
                        {
                            throw new AuthenticationException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AuthencticationFailedMessage);
                        } 
                    }

                    AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                    if (!authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4API, Authentication.CurrentUser.Guid))
                    {
                        throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AccessDeniedMessage);
                    }
                }
                catch (AuthenticationException)
                {
                    throw;
                }
                catch (CustomException e)
                {
                    throw e;
                }
                catch (Exception)
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.InputParsingErrorMessage);
                }

                return inputObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ReturnResponse(HttpResponse HttpResponse, dynamic p_outputObject, string p_Format, IEnumerable<Type> types = null, bool p_IsCustomizedSerializer = false, string p_USeqID = null, NullValueHandling p_NullValH=NullValueHandling.Include)
        {
            string outputResult = string.Empty;

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            try
            {
                if (!string.IsNullOrWhiteSpace(p_Format) && string.Compare(p_Format, CommonConstants.formatType.xml.ToString(), true) == 0)
                {
                    if (p_IsCustomizedSerializer)
                    {
                        outputResult = Serializer.SerializeXmlDC(p_outputObject);
                    }
                    else
                    {
                        outputResult = Serializer.SerializeToXmlWithoutNameSpace(p_outputObject);
                    }
                    HttpResponse.ContentType = "application/xml";
                }
                else
                {
                    outputResult = Serializer.Searialize(p_outputObject,p_NullValH);

                    HttpResponse.ContentType = "application/json";
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogRequestResponse"]) == true)
                {
                    Log4NetLogger.Debug(p_USeqID + " - " + outputResult);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(p_USeqID, ex);
            }

            HttpResponse.Output.Write(outputResult);
        }

        public static string GetUniqueSeqIDForLog()
        { 
            return Convert.ToString(System.Web.HttpContext.Current.Items["USeqID"]);
        }       

    }
}