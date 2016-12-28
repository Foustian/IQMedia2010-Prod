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
    public class SuspendTVAgent : ICommand
    {
        public String _Format { get; private set; }

        public SuspendTVAgent(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var suspendIQAgentOutput = new SuspendTVAgentOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - SuspendTVAgent Request Started");

            try
            {
                SuspendTVAgentInput suspendTVAgentInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<SuspendTVAgentInput>(p_HttpRequest, _Format);

                if (suspendTVAgentInput.SRID > 0)
                {
                    Guid CustomerGUID = Authentication.CurrentUser.Guid;
                    AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                    if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4IQAgentSetup, CustomerGUID))
                    {
                        IQAgentLogic _IQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                        short? previousIsActive = null;

                        var rowsAffected = _IQAgentLogic.SuspendTVAgent(suspendTVAgentInput, Authentication.CurrentUser.ClientGuid.Value, CustomerGUID,out previousIsActive);

                        if (previousIsActive==1 && rowsAffected==1)
                        {
                            suspendIQAgentOutput.Status = 0;
                            suspendIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                        }
                        else if(previousIsActive==2)
                        {
                            suspendIQAgentOutput.Status = 1;
                            suspendIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AgentAlreadySuspended;
                        }
                        else if (previousIsActive==0)
                        {
                            suspendIQAgentOutput.Status = 2;
                            suspendIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AgentDeletedForSuspend;
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
                suspendIQAgentOutput.Status = -3;
                suspendIQAgentOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                suspendIQAgentOutput.Status = -2;
                suspendIQAgentOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                suspendIQAgentOutput.Status = -1;
                suspendIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - DeleteTVAgent Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, suspendIQAgentOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }
        #endregion
    }
}