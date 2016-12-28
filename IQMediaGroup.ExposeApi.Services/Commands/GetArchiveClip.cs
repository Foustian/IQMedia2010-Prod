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
    public class GetArchiveClip : ICommand
    {
        public String _Format { get; private set; }

        public GetArchiveClip(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            ArchiveClipOutput clipOutput = new ArchiveClipOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetArchiveClip Request Started");

            try
            {
                var clipInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<ArchiveClipInput>(p_HttpRequest, _Format,true);

                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4Library, CustomerGUID) && authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4TV, CustomerGUID))
                {
                    if (clipInput.ClipGUID!=Guid.Empty)
                    {
                        Guid clientGUID = Authentication.CurrentUser.ClientGuid.Value;

                        ArchiveLogic archiveLogic = (ArchiveLogic)LogicFactory.GetLogic(LogicType.Archive);

                        clipOutput = archiveLogic.GetArchiveClipByClipGUID(clipInput.ClipGUID, clientGUID);
                        if (clipOutput.Clip!=null)
                        {
                            clipOutput.Status = 0;
                            clipOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                        }
                        else
                        {
                            clipOutput.Status = 1;
                            clipOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
                        }
                    }
                    else
                    {
                        throw new CustomException("Invalid Clip");
                    }
                }
                else
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AccessDeniedMessage);
                }
            }
            catch (AuthenticationException ex)
            {
                clipOutput.Status = -3;
                clipOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                clipOutput.Status = -2;
                clipOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                clipOutput.Status = -1;
                clipOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetArchiveClip Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, clipOutput, _Format, null, true, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }
    }
}