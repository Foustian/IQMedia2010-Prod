using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.ExposeApi.Domain;
using IQMediaGroup.ExposeApi.Services.Serializers;
using System.IO;
using IQMediaGroup.ExposeApi.Logic;
using IQMedia.Web.Common;
using System.Security.Authentication;
using System.Net;
using IQMediaGroup.Common.Util;
using System.Configuration;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class GetRawMedia : ICommand
    {
        public String _Format { get; private set; }

        public GetRawMedia(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            RawMediaOutput rawMediaOutput = new RawMediaOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetRawMedia Request Started");

            try
            {
                var rawMediaInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<RawMediaInput>(p_HttpRequest, _Format);
                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4TV, CustomerGUID))
                {
                    ValidationLogic ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);
                    string errorMessage = string.Empty;
                    bool IsValidate = ValidationLogic.ValidateRawMediaInput(rawMediaInput, out errorMessage);

                    if (IsValidate == true)
                    {
                        rawMediaInput.ClientGUID = Authentication.CurrentUser.ClientGuid.Value;
                        rawMediaInput.CustomerID = Authentication.CurrentUser.CustomerID;

                        RawMediaLogic rawMediaLogic = (RawMediaLogic)LogicFactory.GetLogic(LogicType.RawMedia);

                        rawMediaOutput = rawMediaLogic.GetPMGRawMedia(rawMediaInput, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
                        if (rawMediaOutput != null && rawMediaOutput.RawMediaList != null && rawMediaOutput.RawMediaList.Count() > 0)
                        {
                            rawMediaOutput.Status = 0;
                            rawMediaOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                        }
                        else
                        {
                            rawMediaOutput.Status = 1;
                            rawMediaOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
                        }
                    }
                    else
                    {
                        throw new CustomException(errorMessage);
                    }
                }
                else
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AccessDeniedMessage);
                }
            }
            catch (AuthenticationException ex)
            {
                rawMediaOutput.Status = -3;
                rawMediaOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                rawMediaOutput.Status = -2;
                rawMediaOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                rawMediaOutput.Status = -1;
                rawMediaOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetRawMedia Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, rawMediaOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }

        #endregion
    }
}
