using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Authentication;
using IQMediaGroup.Common.Util;
using System.Web.Security;
using IQMedia.Web.Common;
using IQMediaGroup.Domain;
using System.Configuration;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetEmailSharing : ICommand
    {
        public String _Format { get; private set; }

        public GetEmailSharing(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            var emailSharingOutput = new EmailSharingOutput();
            string _Output = string.Empty;
            CLogger.Debug("GetEmailSharing Request Started");

            try
            {
                FormsAuthenticationTicket formAuthTicket = null;
                if (System.Web.HttpContext.Current.Request.Cookies[".IQAUTH"] != null)
                {
                    formAuthTicket = System.Web.Security.FormsAuthentication.Decrypt(Convert.ToString(System.Web.HttpContext.Current.Request.Cookies[".IQAUTH"].Value));

                    string[] roles = formAuthTicket.UserData.Split(new char[] { '|' });

                    System.Security.Principal.IIdentity id = new System.Web.Security.FormsIdentity(formAuthTicket);

                    System.Security.Principal.IPrincipal principal = new System.Security.Principal.GenericPrincipal(id, roles);

                    HttpContext.Current.User = principal;
                }

                if (!Authentication.IsAuthenticated || (formAuthTicket == null || formAuthTicket.Expired))
                {
                    emailSharingOutput.Status = 0;
                    emailSharingOutput.Message = "Success";
                    emailSharingOutput.IsEmailSharing = false;
                }
                else
                {
                    emailSharingOutput.Status = 0;
                    emailSharingOutput.Message = "Success";
                    emailSharingOutput.IsEmailSharing = true;
                }
            }
            catch (AuthenticationException ex)
            {
                emailSharingOutput.Status = 1;
                emailSharingOutput.Message = "User not authenticated";
                emailSharingOutput.IsEmailSharing = false;
            }
            catch (ArgumentException exception)
            {
                emailSharingOutput.Status = 1;
                emailSharingOutput.Message = exception.Message;
                emailSharingOutput.IsEmailSharing = false;
            }

            catch (Exception ex)
            {
                CLogger.Error("Error : " + ex.Message + " stack : " + ex.StackTrace);
                emailSharingOutput.Status = 1;
                emailSharingOutput.IsEmailSharing = false;
                emailSharingOutput.Message = "An error occured, please try again!!";
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                _Output = Serializer.SerializeToXmlWithoutNameSpace(emailSharingOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                _Output = Serializer.Searialize(emailSharingOutput);
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug("Final result: " + _Output);
            }
            CLogger.Debug("GetEmailSharing Request Ended");
            HttpResponse.Output.Write(_Output);
        }
    }
}