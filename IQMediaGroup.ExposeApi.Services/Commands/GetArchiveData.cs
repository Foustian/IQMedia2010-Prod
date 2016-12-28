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
    public class GetArchiveData : ICommand
    {
        public String _Format { get; private set; }

        public GetArchiveData(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            ArchiveOutput archiveOutput = new ArchiveOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetArchiveData Request Started");

            try
            {
                var archiveinput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<ArchiveInput>(p_HttpRequest, _Format);

                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4Library, CustomerGUID) && authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4TV, CustomerGUID))
                {
                    ValidationLogic ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);
                    string errorMessage = string.Empty;
                    bool IsValidate = ValidationLogic.ValidateArchiveInput(archiveinput, out errorMessage);

                    if (IsValidate == true)
                    {
                        Guid ClientGUID = Authentication.CurrentUser.ClientGuid.Value;

                        ArchiveLogic archiveLogic = (ArchiveLogic)LogicFactory.GetLogic(LogicType.Archive);

                        archiveOutput = archiveLogic.GetArchiveData(archiveinput, ClientGUID, CustomerGUID, Authentication.CurrentUser.CustomerID,Authentication.CurrentUser.ClientID,IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
                        if (archiveOutput != null && archiveOutput.ArchiveMediaList != null && archiveOutput.ArchiveMediaList.Count() > 0)
                        {
                            archiveOutput.Status = 0;
                            archiveOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                        }
                        else
                        {
                            archiveOutput.Status = 1;
                            archiveOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
                        }
                    }
                    else
                    {
                        throw new CustomException(errorMessage);
                    }
                }
                else
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AccessDeniedMessage);
                }

            }
            catch (AuthenticationException ex)
            {
                archiveOutput.Status = -3;
                archiveOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                archiveOutput.Status = -2;
                archiveOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                archiveOutput.Status = -1;
                archiveOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetArchiveData Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, archiveOutput, _Format, null, true, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }
    }
}