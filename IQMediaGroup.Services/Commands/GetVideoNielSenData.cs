using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Domain;
using IQMedia.Web.Common;
using IQMediaGroup.Common.Util;
using System.IO;
using System.Configuration;
using IQMediaGroup.Logic;
using System.Security.Authentication;
using IQMediaGroup.Services.Serializers;
using System.Web.Security;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetVideoNielSenData : ICommand
    {
        public Guid? _ID { get; private set; }
        public bool _IsRawMedia { get; private set; }
        public int? _IQ_Start_Point { get; private set; }
        public string _IQ_Dma_Num { get; private set; }

        public GetVideoNielSenData(object ID, object Type, object SP, object Num)
        {
            _ID = (ID is NullParameter) ? null : (Guid?)ID;
            _IsRawMedia = (Type is NullParameter) ? false : (bool)Type;
            _IQ_Start_Point = (SP is NullParameter) ? null: (int?)SP;
            _IQ_Dma_Num = (Num is NullParameter) ? null : (string)Num;
        }

        #region ICommand Members

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _JSONResult = string.Empty;
            string _JSONRequest = string.Empty;
            string _callback = "";

            VideoNielSenDataOutput _VideoNielSenDataOutput = new VideoNielSenDataOutput();
            try
            {
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

                    throw new AuthenticationException();
                }
                else
                {
                    var clientGUID = Authentication.CurrentUser.ClientGuid.Value;

                    _callback = HttpRequest["callback"];

                    CLogger.Debug("GetVideoNielSenData Request Started");

                    VideoNielSenDataInput _VideoNielSenDataInput = new VideoNielSenDataInput();

                    if (!_ID.HasValue)
                    {
                        throw new CustomException("Error during parsing input");
                    }

                    if (string.IsNullOrWhiteSpace(_IQ_Dma_Num))
                    {
                        throw new CustomException("Error during parsing input");
                    }

                    _VideoNielSenDataInput.Guid = _ID.Value;
                    _VideoNielSenDataInput.IQ_Dma_Num = _IQ_Dma_Num;
                    _VideoNielSenDataInput.IQ_Start_Point = _IQ_Start_Point;
                    _VideoNielSenDataInput.ClientGuid = clientGUID;
                    _VideoNielSenDataInput.IsRawMedia = _IsRawMedia;

                    ValidationLogic ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);

                    bool IsValidate = ValidationLogic.ValidateNielSenInput(_VideoNielSenDataInput);

                    if (IsValidate)
                    {
                        NielSenDataLogic _NielSenDataLogic = (NielSenDataLogic)LogicFactory.GetLogic(LogicType.NielSen);

                        bool? HasAccess = _NielSenDataLogic.CheckClientNielSenDataAccess(clientGUID);
                        if (HasAccess.HasValue && HasAccess.Value == true)
                        {                            
                            _VideoNielSenDataOutput = _NielSenDataLogic.GetNielSenData(_VideoNielSenDataInput);
                        }
                        else
                        {
                            _VideoNielSenDataOutput.Status = 1;
                            CLogger.Debug("Access denied.");
                        }
                    }
                    else
                    {
                        _VideoNielSenDataOutput.Status = 1;
                        CLogger.Debug("Invalid Input");
                    }
                }
            }
            catch (CustomException ex)
            {
                _VideoNielSenDataOutput.Status = 1;
                CLogger.Debug(ex.Message);
            }
            catch (AuthenticationException)
            {
                _VideoNielSenDataOutput.Status = 1;
                CLogger.Debug("User is not Authenticated.");

            }
            catch (Exception _Exception)
            {
                _VideoNielSenDataOutput.Status = 1;
                CLogger.Error("Error : " + _Exception.ToString() + " stack : " + _Exception.StackTrace);
            }

            _JSONResult = _callback + "([" + Serializer.Searialize(_VideoNielSenDataOutput) + "])";
            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug(_JSONResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/json";
            CLogger.Debug("GetVideoNielSenData Request Ended");

            HttpResponse.Output.Write(_JSONResult);
        }

        #endregion
    }
}