using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.ExposeApi.Domain;
using IQMediaGroup.ExposeApi.Logic;
using IQMedia.Web.Common;
using IQMediaGroup.Common.Util;
using System.Security.Authentication;
using IQMediaGroup.ExposeApi.Services.Serializers;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class GetTVAgents : ICommand
    {
        public String _Format { get; private set; }

        public GetTVAgents(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var tvAgentsOutput = new TVAgentsOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetTVAgents Request Started");

            try
            {
                TVAgentsInput tvAgentsInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<TVAgentsInput>(p_HttpRequest, _Format);
                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4IQAgentSetup, CustomerGUID))
                {
                    IQAgentLogic _IQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                    string SRIDList = null;
                    if (tvAgentsInput.SRIDList != null && tvAgentsInput.SRIDList.Count() > 0)
                    {
                        SRIDList = IQMediaGroup.ExposeApi.Services.Serializers.Serializer.SerializeToXml(tvAgentsInput);
                    }
                    tvAgentsOutput.TVAgentList = _IQAgentLogic.GetIQAgents(Authentication.CurrentUser.ClientGuid.Value, SRIDList);
                    foreach (var tvAgent in tvAgentsOutput.TVAgentList)
                    {
                        string searchTerm = _IQAgentLogic.ChangeXmlNodeName(tvAgent.SearchTerm);
                        tvAgent.SearchRequest = new SearchRequestAgent();
                        tvAgent.SearchRequest = (SearchRequestAgent)Serializer.DeserialiazeXml(searchTerm, tvAgent.SearchRequest);
                        tvAgent.SearchRequest.AgentName = tvAgent.AgentName;
                    }
                    if (tvAgentsOutput.TVAgentList != null && tvAgentsOutput.TVAgentList.Count() > 0)
                    {
                        tvAgentsOutput.Status = 0;
                        tvAgentsOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                    }
                    else
                    {
                        tvAgentsOutput.Status = 1;
                        tvAgentsOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
                    }
                }
                else
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AccessDeniedMessage);
                }
            }
            catch (AuthenticationException ex)
            {
                tvAgentsOutput.Status = -3;
                tvAgentsOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                tvAgentsOutput.Status = -2;
                tvAgentsOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                tvAgentsOutput.Status = -1;
                tvAgentsOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetTVAgents Request Started");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, tvAgentsOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }

        #endregion
    }
}