using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.ExposeApi.Domain;
using IQMediaGroup.Common.Util;
using System.IO;
using System.Configuration;
using IQMediaGroup.ExposeApi.Services.Serializers;
using IQMediaGroup.ExposeApi.Logic;
using System.Security.Authentication;
using IQMedia.Web.Common;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class DeleteTVAgent : ICommand
    {
        public String _Format { get; private set; }

        public DeleteTVAgent(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var deleteIQAgentOutput = new DeleteTVAgentOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - DeleteTVAgent Request Started");

            try
            {
                DeleteTVAgentInput deleteTVAgentInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<DeleteTVAgentInput>(p_HttpRequest, _Format);
                if (deleteTVAgentInput.SRID > 0)
                {
                    Guid CustomerGUID = Authentication.CurrentUser.Guid;
                    AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                    if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4IQAgentSetup, CustomerGUID))
                    {
                        IQAgentLogic _IQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                        var rowsAffected = _IQAgentLogic.DeleteTVAgents(deleteTVAgentInput, Authentication.CurrentUser.ClientGuid.Value, CustomerGUID);
                        if (rowsAffected > 0)
                        {
                            deleteIQAgentOutput.Status = 0;
                            deleteIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                        }
                        else if(rowsAffected==-2)
                        {
                            deleteIQAgentOutput.Status = 1;
                            deleteIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AgentAlreadyDeleted;
                        }
                        else
                        {
                            throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.InvalidTVAgentRequestMessage);
                        }
                    }
                    else
                    {
                        throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AccessDeniedMessage);
                    }
                }
                else
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SearchRequestIDMissingMessage);
                }
            }
            catch (AuthenticationException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                deleteIQAgentOutput.Status = -3;
                deleteIQAgentOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                deleteIQAgentOutput.Status = -2;
                deleteIQAgentOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                deleteIQAgentOutput.Status = -1;
                deleteIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - DeleteTVAgent Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, deleteIQAgentOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }
        #endregion
    }
}