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
    public class UnSuspendTVAgent : ICommand
    {
        public String _Format { get; private set; }

        public UnSuspendTVAgent(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var unSuspendIQAgentOutput = new UnSuspendTVAgentOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - UnSuspendTVAgent Request Started");

            try
            {
                UnSuspendTVAgentInput unSuspendTVAgentInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<UnSuspendTVAgentInput>(p_HttpRequest, _Format);

                if (unSuspendTVAgentInput.SRID > 0)
                {
                    Guid CustomerGUID = Authentication.CurrentUser.Guid;
                    AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                    if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4IQAgentSetup, CustomerGUID))
                    {
                        IQAgentLogic _IQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                        short? previousIsActive = null;

                        var rowsAffected = _IQAgentLogic.UnSuspendTVAgent(unSuspendTVAgentInput, Authentication.CurrentUser.ClientGuid.Value, CustomerGUID,out previousIsActive);

                        if (previousIsActive==2 && rowsAffected==1)
                        {
                            unSuspendIQAgentOutput.Status = 0;
                            unSuspendIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                        }
                        else if(previousIsActive==1)
                        {
                            unSuspendIQAgentOutput.Status = 1;
                            unSuspendIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AgentAlreadyActive;                            
                        }
                        else if(previousIsActive==0)
                        {
                            unSuspendIQAgentOutput.Status = 2;
                            unSuspendIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AgentDeletedForUnSuspend;                                                        
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
                unSuspendIQAgentOutput.Status = -3;
                unSuspendIQAgentOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                unSuspendIQAgentOutput.Status = -2;
                unSuspendIQAgentOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                unSuspendIQAgentOutput.Status = -1;
                unSuspendIQAgentOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - DeleteTVAgent Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, unSuspendIQAgentOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }
        #endregion
    }
}