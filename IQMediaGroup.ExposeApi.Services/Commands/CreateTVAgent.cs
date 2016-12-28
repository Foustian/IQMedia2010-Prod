using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.ExposeApi.Domain;
using IQMediaGroup.Common.Util;
using System.Security.Authentication;
using IQMediaGroup.ExposeApi.Logic;
using System.IO;
using System.Configuration;
using IQMediaGroup.ExposeApi.Services.Serializers;
using IQMedia.Web.Common;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class CreateTVAgent : ICommand
    {
        public String _Format { get; private set; }

        public CreateTVAgent(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var createIQAgentOutput = new CreateTVAgentOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - CreateTVAgent Request Started");

            try
            {
                CreateTVAgentInput createTVAgentInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<CreateTVAgentInput>(p_HttpRequest, _Format);
                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4IQAgentSetup, CustomerGUID))
                {
                    IQAgentLogic _IQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                    ValidationLogic _ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);
                    string errorMessage = string.Empty;
                    bool IsValidate = _ValidationLogic.ValidateTVAgentInput(Authentication.CurrentUser.ClientGuid.Value, out errorMessage, createTVAgentInput, null);
                    if (!IsValidate)
                    {
                        throw new CustomException(errorMessage);
                    }

                    string searchRequest = IQMediaGroup.ExposeApi.Services.Serializers.Serializer.SerializeToXml(createTVAgentInput.SearchRequest);
                    string searchTerm = _IQAgentLogic.ChangeNodeName(searchRequest);
                    var dicTVAgent = _IQAgentLogic.CreateTVAgents(createTVAgentInput, searchTerm, Authentication.CurrentUser.ClientGuid.Value);

                    if (dicTVAgent["IQAgentSearchRequestID"] != null && Convert.ToInt32(dicTVAgent["IQAgentSearchRequestID"]) > 0)
                    {
                        createIQAgentOutput.Status = 0;
                        createIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                        createIQAgentOutput.SRID = Convert.ToInt32(dicTVAgent["IQAgentSearchRequestID"]);
                    }
                    else if (dicTVAgent["IQAgentSearchRequestID"] == null && dicTVAgent["SearchRequestCount"] != null && dicTVAgent["AllowedIQAgent"] != null && (Convert.ToInt32(dicTVAgent["SearchRequestCount"]) < Convert.ToInt32(dicTVAgent["AllowedIQAgent"])))
                    {
                        throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.QueryNameAlreadyExistsMessage);
                    }
                    else
                    {
                        throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AgentQuotaExceededMessage);
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
                createIQAgentOutput.Status = -3;
                createIQAgentOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                createIQAgentOutput.Status = -2;
                createIQAgentOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                createIQAgentOutput.Status = -1;
                createIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - CreateTVAgent Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, createIQAgentOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }
        #endregion
    }
}