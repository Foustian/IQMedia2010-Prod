using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.Configuration;
using IQMediaGroup.ExposeApi.Services.Serializers;
using IQMediaGroup.ExposeApi.Logic;
using IQMediaGroup.ExposeApi.Domain;
using System.IO;
using IQMedia.Web.Common;
using System.Security.Authentication;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class GetMediaPlayerURL : ICommand
    {
        public String _Format { get; private set; }

        public GetMediaPlayerURL(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var mediaPlayerURLOutput = new MediaPlayerURLOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetMediaPlayerURL Request Started");
            try
            {
                var mediaPlayerURLInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<MediaPlayerURLInput>(p_HttpRequest, _Format);
                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4TV, CustomerGUID))
                {
                    var validationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);
                    string errorMessage = string.Empty;
                    bool IsValidate = validationLogic.ValidateMediaPlayerURLInput(mediaPlayerURLInput, out errorMessage);

                    if (IsValidate)
                    {
                        var rawMediaLogic = (RawMediaLogic)LogicFactory.GetLogic(LogicType.RawMedia);
                        if (mediaPlayerURLInput.SeqID == null)
                        {
                            Guid? recordFileGUID = rawMediaLogic.GetRecordFileGUIDByStationIDANDDatetime(mediaPlayerURLInput.StationID, Convert.ToDateTime(mediaPlayerURLInput.DateTime),mediaPlayerURLInput.SearchOnGMTDateTime);

                            if (recordFileGUID != null)
                            {
                                mediaPlayerURLOutput.URL = ConfigurationManager.AppSettings["IframeURL"] + "?RawMediaID=" + recordFileGUID;
                            }
                        }
                        else
                        {
                            Int64? SeqID = rawMediaLogic.IQAgentMediaResultsVerifyIDByClientGUID(Convert.ToInt64(mediaPlayerURLInput.SeqID), Authentication.CurrentUser.ClientGuid.Value);

                            if (SeqID > 0)
                            {
                                mediaPlayerURLOutput.URL = ConfigurationManager.AppSettings["IframeURL"] + "?SeqID=" + SeqID;
                            }
                        }
                        if (mediaPlayerURLOutput.URL != null)
                        {
                            mediaPlayerURLOutput.Status = 0;
                            mediaPlayerURLOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                        }
                        else
                        {
                            mediaPlayerURLOutput.Status = 1;
                            mediaPlayerURLOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
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
                mediaPlayerURLOutput.Status = -3;
                mediaPlayerURLOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                mediaPlayerURLOutput.Status = -2;
                mediaPlayerURLOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                mediaPlayerURLOutput.Status = -1;
                mediaPlayerURLOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetMediaPlayerURL Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, mediaPlayerURLOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }
    }
}