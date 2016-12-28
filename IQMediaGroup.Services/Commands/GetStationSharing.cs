using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Domain;
using IQMediaGroup.Services.Serializers;
using System.Configuration;
using IQMediaGroup.Logic;
using IQMedia.Web.Common;
using System.Security.Authentication;
using System.Web.Security;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetStationSharing : ICommand
    {
        public Guid? _ClipID { get; private set; }
        public String _Format { get; private set; }

        public GetStationSharing(object ClipID, object Format)
        {
            _ClipID = (ClipID is NullParameter) ? null : (Guid?)ClipID;
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _Input = string.Empty;
            string _Output = string.Empty;
            var stationSharingOutput = new StationSharingOutput();
            CLogger.Debug("GetStationSharing Request Started");

            try
            {
                

                if (_ClipID == null)
                {
                    throw new ArgumentException("Missing or Invalid Clip ID");
                }

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    CLogger.Debug("{\"ClipID\":\"" + _ClipID + "\"}");
                }

                FormsAuthenticationTicket formAuthTicket = null;
                if (System.Web.HttpContext.Current.Request.Cookies[".IQAUTH"] != null)
                {
                    formAuthTicket = System.Web.Security.FormsAuthentication.Decrypt(Convert.ToString(System.Web.HttpContext.Current.Request.Cookies[".IQAUTH"].Value));

                    CLogger.Debug("Cookie :" + Convert.ToString(System.Web.HttpContext.Current.Request.Cookies[".IQAUTH"].Value));

                    string[] roles = formAuthTicket.UserData.Split(new char[] { '|' });

                    System.Security.Principal.IIdentity id = new System.Web.Security.FormsIdentity(formAuthTicket);

                    System.Security.Principal.IPrincipal principal = new System.Security.Principal.GenericPrincipal(id, roles);

                    HttpContext.Current.User = principal;

                    CLogger.Debug("User:" + HttpContext.Current.User.Identity.Name);
                }

                if (!Authentication.IsAuthenticated || (formAuthTicket == null || formAuthTicket.Expired))
                {
                    CLogger.Debug("user not authenticated");
                    stationSharingOutput.Status = 0;
                    stationSharingOutput.Message = "Success";
                    stationSharingOutput.IsSharing = false;
                }
                else
                {
                

                    Guid clientGuid = Authentication.CurrentUser.ClientGuid.Value;
                    Guid customerGuid = Authentication.CurrentUser.Guid;

                    CLogger.Debug("GetStationSharing into if condition of customerGUID");
                    var stationLogic = (StationLogic)LogicFactory.GetLogic(LogicType.Station);

                    CLogger.Info("Client Guid : " + clientGuid + " Customer Guid :" + customerGuid);
                    
                    Boolean result = stationLogic.SelectStationSharingByClipIDNClientGUID(_ClipID, clientGuid, customerGuid);
                    stationSharingOutput.Status = 0;
                    stationSharingOutput.Message = "Success";
                    stationSharingOutput.IsSharing = result;
                }
            }
            catch (AuthenticationException ex)
            {
                CLogger.Error(ex);

                stationSharingOutput.Status = 1;
                stationSharingOutput.Message = "User not authenticated";
            }
            catch (ArgumentException exception)
            {
                CLogger.Error(exception);

                stationSharingOutput.Status = 1;
                stationSharingOutput.Message = exception.Message;
            }

            catch (Exception ex)
            {
                CLogger.Error(ex);
                stationSharingOutput.Status = 1;
                stationSharingOutput.Message = "An error occured, please try again!!";
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                _Output = Serializer.SerializeToXmlWithoutNameSpace(stationSharingOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                _Output = Serializer.Searialize(stationSharingOutput);
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug("Final result: " + _Output);
            }
            CLogger.Debug("GetStationSharing Request Ended");
            HttpResponse.Output.Write(_Output);
        }
    }
}