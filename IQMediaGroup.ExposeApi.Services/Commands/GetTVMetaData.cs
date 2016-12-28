using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.ExposeApi.Domain;
using IQMediaGroup.ExposeApi.Logic;
using IQMediaGroup.ExposeApi.Services.Serializers;
using IQMedia.Web.Common;
using System.Security.Authentication;
using System.IO;
using System.Configuration;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class GetTVMetaData : ICommand
    {

        public String _Format { get; private set; }

        public GetTVMetaData(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }
        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var statskedprogData = new StatskedprogData();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetTVMetaData Request Started");

            try
            {
                TVMetaDataInput SessionIDInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<TVMetaDataInput>(p_HttpRequest, _Format);

                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4TV, CustomerGUID))
                {
                    StatskedprogLogic _StatskedprogLogic = (StatskedprogLogic)LogicFactory.GetLogic(LogicType.StatskedprogData);

                    statskedprogData = _StatskedprogLogic.GetStatskedprogData(Authentication.CurrentUser.ClientGuid.Value);
                    if (statskedprogData != null)
                    {
                        statskedprogData.Status = 0;
                        statskedprogData.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                    }
                    else
                    {
                        statskedprogData.Status = 1;
                        statskedprogData.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
                    }
                }
                else
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AccessDeniedMessage);
                }
            }
            catch (AuthenticationException ex)
            {
                statskedprogData.Status = -3;
                statskedprogData.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                statskedprogData.Status = -2;
                statskedprogData.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                statskedprogData.Status = -1;
                statskedprogData.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetTVMetaData Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, statskedprogData, _Format, null, true, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }

        #endregion
    }
}