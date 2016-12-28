using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.ExposeApi.Logic;
using IQMediaGroup.ExposeApi.Services.Serializers;
using IQMedia.Web.Common;
using System.Security.Authentication;
using IQMediaGroup.ExposeApi.Domain;
using System.IO;
using System.Configuration;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class GetRadioStation : ICommand
    {
        public String _Format { get; private set; }

        public GetRadioStation(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var radioStationDataOutput = new RadioStationDataOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetRadioStation Request Started");

            try
            {
                var radioStationDataInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<RadioStationDataInput>(p_HttpRequest, _Format);

                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4Radio, CustomerGUID))
                {
                    RadioStationLogic RadioStationLogic = (RadioStationLogic)LogicFactory.GetLogic(LogicType.RadioStation);

                    radioStationDataOutput.RadioStationList = RadioStationLogic.GetRadioStation();
                    if (radioStationDataOutput.RadioStationList != null && radioStationDataOutput.RadioStationList.Count() > 0)
                    {
                        radioStationDataOutput.Status = 0;
                        radioStationDataOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                    }
                    else
                    {
                        radioStationDataOutput.Status = 1;
                        radioStationDataOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
                    }
                }
                else
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AccessDeniedMessage);
                }
            }
            catch (AuthenticationException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                radioStationDataOutput.Status = -3;
                radioStationDataOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                radioStationDataOutput.Status = -2;
                radioStationDataOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                radioStationDataOutput.Status = -1;
                radioStationDataOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetRadioStation Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, radioStationDataOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());

        #endregion
        }
    }
}