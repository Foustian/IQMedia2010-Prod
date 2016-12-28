using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.ExposeApi.Domain;
using IQMediaGroup.Common.Util;
using IQMediaGroup.ExposeApi.Logic;
using System.Configuration;
using IQMedia.Web.Common;
using System.Security.Authentication;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class GetAgentResults : ICommand
    {
        public String _Format { get; private set; }

        public GetAgentResults(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var agentResultsOutput = new AgentResultsOutput();

            try
            {
                Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - Get Agent Results Request Started");

                AgentResultsInput agentResultsInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<AgentResultsInput>(p_HttpRequest, _Format);

                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);

                Dictionary<string, bool> roles = null;

                if (authenticationLogic.CheckFeedsAccess(CustomerGUID, out roles))
                {
                    agentResultsInput.Rows = agentResultsInput.Rows.HasValue ? agentResultsInput.Rows : 10;

                    if (agentResultsInput.Rows > Convert.ToInt32(ConfigurationManager.AppSettings["MaxTVResultsPageSize"]) || agentResultsInput.Rows <= 0)
                    {
                        string errorMessage = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.RowsLimitExceedMessage.Replace("{limit}", ConfigurationManager.AppSettings["MaxTVResultsPageSize"]);
                        throw new CustomException(errorMessage);
                    }

                    if (!string.IsNullOrEmpty(agentResultsInput.MediaType) && (string.IsNullOrEmpty(agentResultsInput.SubMediaTypeEnum) || !roles[CommonConstants.SubMediaCategoryRoles[((CommonConstants.SubMediaCategory)CommonConstants.SubMediaCategory.Parse(typeof(CommonConstants.SubMediaCategory),agentResultsInput.SubMediaTypeEnum))].ToString()]))
                    {
                        throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.InvalidMediaTypeORNoRight);
                    }

                    IQAgentLogic _IQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                    agentResultsOutput = _IQAgentLogic.GetAgentResults(agentResultsInput, Authentication.CurrentUser.ClientGuid.Value, Authentication.CurrentUser.Guid, roles, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());

                    if (agentResultsOutput != null && agentResultsOutput.AgentResultList != null && agentResultsOutput.AgentResultList.Count() > 0)
                    {
                        agentResultsOutput.Status = 0;
                        agentResultsOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                    }
                    else
                    {
                        agentResultsOutput.Status = 1;
                        agentResultsOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
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
                agentResultsOutput.Status = -3;
                agentResultsOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                agentResultsOutput.Status = -2;
                agentResultsOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                agentResultsOutput.Status = -1;
                agentResultsOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - Get Agent Results Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, agentResultsOutput, _Format, null, true, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(),Newtonsoft.Json.NullValueHandling.Ignore);
        }
    }
}