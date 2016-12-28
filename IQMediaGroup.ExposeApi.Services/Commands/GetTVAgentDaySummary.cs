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
    public class GetTVAgentDaySummary : ICommand
    {
        public String _Format { get; private set; }

        public GetTVAgentDaySummary(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var tvAgentDaySummaryOutput = new TVAgentDaySummaryOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetTVAgentDaySummary Request Started");

            try
            {
                TVAgentDaySummaryInput tvAgentDaySummaryInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<TVAgentDaySummaryInput>(p_HttpRequest, _Format);

                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4TV, CustomerGUID) && authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4Dashboard, CustomerGUID))
                {
                    ValidationLogic _ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);
                    string errorMessage = string.Empty;
                    if (!_ValidationLogic.ValidateTVAgentDaySummaryDuration(tvAgentDaySummaryInput, out errorMessage))
                    {
                        //throw new CustomException("Invalid FromDate and ToDate. Maxmium " + System.Configuration.ConfigurationManager.AppSettings["MaxDaySummaryDuration"] + " days duration is allowed.");
                        throw new CustomException(errorMessage);
                    }

                    IQAgentLogic _IQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                    tvAgentDaySummaryOutput.DaySummaryList = _IQAgentLogic.GetTVAgentDailySummary(tvAgentDaySummaryInput, Authentication.CurrentUser.ClientGuid.Value);
                    if (tvAgentDaySummaryOutput.DaySummaryList != null && tvAgentDaySummaryOutput.DaySummaryList.Count() > 0)
                    {
                        tvAgentDaySummaryOutput.Status = 0;
                        tvAgentDaySummaryOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                    }
                    else
                    {
                        tvAgentDaySummaryOutput.Status = 1;
                        tvAgentDaySummaryOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
                    }
                }
                else
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AccessDeniedMessage);
                }

            }
            catch (AuthenticationException ex)
            {
                tvAgentDaySummaryOutput.Status = -3;
                tvAgentDaySummaryOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                tvAgentDaySummaryOutput.Status = -2;
                tvAgentDaySummaryOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                tvAgentDaySummaryOutput.Status = -1;
                tvAgentDaySummaryOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetTVAgentDaySummary Request Started");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, tvAgentDaySummaryOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }

        #endregion
    }
}