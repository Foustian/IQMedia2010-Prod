using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.ExposeApi.Domain;
using System.IO;
using IQMediaGroup.ExposeApi.Services.Serializers;
using IQMediaGroup.ExposeApi.Logic;
using IQMedia.Web.Common;
using System.Security.Authentication;
using System.Configuration;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class GetRadioRawMedia : ICommand
    {
        #region ICommand Members

        public String _Format { get; private set; }

        public GetRadioRawMedia(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var radioRawMediaOutputObj = new RadioMediaOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - Get Radio Raw Media Request Started");

            try
            {
                var radioMediaInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<RadioMediaInput>(p_HttpRequest, _Format);
                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4Radio, CustomerGUID))
                {
                    ValidationLogic ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);
                    string errorMessage = string.Empty;
                    bool IsValidate = ValidationLogic.ValidateRadioRawMediaInput(radioMediaInput, out errorMessage);

                    if (IsValidate == true)
                    {
                        RadioStationLogic RadioStationLogic = (RadioStationLogic)LogicFactory.GetLogic(LogicType.RadioStation);

                        radioRawMediaOutputObj = RadioStationLogic.GetRadioRawMedia(radioMediaInput);
                        if (radioRawMediaOutputObj != null && radioRawMediaOutputObj.RadioMediaList != null && radioRawMediaOutputObj.RadioMediaList.Count() > 0)
                        {
                            radioRawMediaOutputObj.Status = 0;
                            radioRawMediaOutputObj.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                        }
                        else
                        {
                            radioRawMediaOutputObj.Status = 1;
                            radioRawMediaOutputObj.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
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
                radioRawMediaOutputObj.Status = -3;
                radioRawMediaOutputObj.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                radioRawMediaOutputObj.Status = -2;
                radioRawMediaOutputObj.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                radioRawMediaOutputObj.Status = -1;
                radioRawMediaOutputObj.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - Get Radio Raw Media Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, radioRawMediaOutputObj, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }

        #endregion
    }
}