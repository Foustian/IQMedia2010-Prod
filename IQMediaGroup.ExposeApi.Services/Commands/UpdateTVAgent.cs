using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.IO;
using IQMediaGroup.ExposeApi.Domain;
using System.Configuration;
using IQMediaGroup.ExposeApi.Services.Serializers;
using IQMediaGroup.ExposeApi.Logic;
using System.Security.Authentication;
using IQMedia.Web.Common;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class UpdateTVAgent : ICommand
    {
        public String _Format { get; private set; }

        public UpdateTVAgent(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var updateIQAgentOutput = new UpdateTVAgentOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - UpdateTVAgent Request Started");

            try
            {
                UpdateTVAgentInput updateTVAgentInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<UpdateTVAgentInput>(p_HttpRequest, _Format);
                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4IQAgentSetup, CustomerGUID))
                {
                    IQAgentLogic _IQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                    ValidationLogic _ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);
                    string errorMessage = string.Empty;
                    bool IsValidate = _ValidationLogic.ValidateTVAgentInput(Authentication.CurrentUser.ClientGuid.Value, out errorMessage, null, updateTVAgentInput);
                    if (!IsValidate)
                    {
                        throw new CustomException(errorMessage);
                    }
                    string searchRequest = IQMediaGroup.ExposeApi.Services.Serializers.Serializer.SerializeToXml(updateTVAgentInput.SearchRequest);
                    string searchTerm = _IQAgentLogic.ChangeNodeName(searchRequest);
                    var rowsAffected = _IQAgentLogic.UpdateTVAgents(updateTVAgentInput, searchTerm, Authentication.CurrentUser.ClientGuid.Value);
                    if (rowsAffected >= 0)
                    {
                        updateIQAgentOutput.Status = 0;
                        updateIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                    }
                    else if (rowsAffected == -2)
                    {
                        throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.QueryNameAlreadyExistsMessage);
                    }
                    else
                    {
                        throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage);
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
                updateIQAgentOutput.Status = -3;
                updateIQAgentOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                updateIQAgentOutput.Status = -2;
                updateIQAgentOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                updateIQAgentOutput.Status = -1;
                updateIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - UpdateTVAgent Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, updateIQAgentOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }
        #endregion
    }
}