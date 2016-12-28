using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.ExposeApi.Domain;
using IQMediaGroup.ExposeApi.Logic;
using IQMediaGroup.Common.Util;
using IQMedia.Web.Common;
using System.Security.Authentication;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class GetTVAgentHourSummary : ICommand
    {
        public String _Format { get; private set; }

        public GetTVAgentHourSummary(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var tvAgentHourSummaryOutput = new TVAgentHourSummaryOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetTVAgentHourSummary Request Started");

            try
            {
                TVAgentHourSummaryInput tvAgentHourSummaryInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<TVAgentHourSummaryInput>(p_HttpRequest, _Format);
                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4TV, CustomerGUID) && authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4Dashboard, CustomerGUID))
                {
                    ValidationLogic _ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);
                    string errorMessage = string.Empty;
                    if (!_ValidationLogic.ValidateTVAgentHourSummaryDuration(tvAgentHourSummaryInput, out errorMessage))
                    {
                        //throw new CustomException("Invalid FromDateTime and ToDateTime. Maxmium " + System.Configuration.ConfigurationManager.AppSettings["MaxHourSummaryDuration"] + " hours duration is allowed.");
                        throw new CustomException(errorMessage);
                    }

                    IQAgentLogic _IQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                    tvAgentHourSummaryOutput.HourSummaryList = _IQAgentLogic.GetTVAgentHourSummary(tvAgentHourSummaryInput, Authentication.CurrentUser.ClientGuid.Value);
                    if (tvAgentHourSummaryOutput.HourSummaryList != null && tvAgentHourSummaryOutput.HourSummaryList.Count() > 0)
                    {
                        tvAgentHourSummaryOutput.Status = 0;
                        tvAgentHourSummaryOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                    }
                    else
                    {
                        tvAgentHourSummaryOutput.Status = 1;
                        tvAgentHourSummaryOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
                    }
                }
                else
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AccessDeniedMessage);
                }

            }
            catch (AuthenticationException ex)
            {
                tvAgentHourSummaryOutput.Status = -3;
                tvAgentHourSummaryOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                tvAgentHourSummaryOutput.Status = -2;
                tvAgentHourSummaryOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                tvAgentHourSummaryOutput.Status = -1;
                tvAgentHourSummaryOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetTVAgentHourSummary Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, tvAgentHourSummaryOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }

        #endregion
    }
}