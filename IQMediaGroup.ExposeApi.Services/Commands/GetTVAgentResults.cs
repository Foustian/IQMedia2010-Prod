using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.ExposeApi.Domain;
using IQMediaGroup.ExposeApi.Logic;
using System.Security.Authentication;
using IQMedia.Web.Common;
using IQMediaGroup.Common.Util;
using System.Configuration;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class GetTVAgentResults : ICommand
    {
        public String _Format { get; private set; }

        public GetTVAgentResults(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var tvIQAgentResultsOutput = new TVAgentResultsOutput();

            try
            {
                Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - Get TV Agent Results Request Started");

                TVAgentResultsInput tvIQAgentResultsInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<TVAgentResultsInput>(p_HttpRequest, _Format);

                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4TV, CustomerGUID) && authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4Feeds, CustomerGUID))
                {
                    tvIQAgentResultsInput.Rows = tvIQAgentResultsInput.Rows.HasValue ? tvIQAgentResultsInput.Rows : 10;

                    if (tvIQAgentResultsInput.Rows > Convert.ToInt32(ConfigurationManager.AppSettings["MaxTVResultsPageSize"]) || tvIQAgentResultsInput.Rows <= 0)
                    {
                        string errorMessage = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.RowsLimitExceedMessage.Replace("{limit}", ConfigurationManager.AppSettings["MaxTVResultsPageSize"]);
                        throw new CustomException(errorMessage);
                    }

                    IQAgentLogic _IQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                    tvIQAgentResultsOutput = _IQAgentLogic.GetTVAgentResults(tvIQAgentResultsInput, Authentication.CurrentUser.ClientGuid.Value, Authentication.CurrentUser.Guid, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());

                    if (tvIQAgentResultsOutput != null && tvIQAgentResultsOutput.TVResultList != null && tvIQAgentResultsOutput.TVResultList.Count() > 0)
                    {
                        tvIQAgentResultsOutput.Status = 0;
                        tvIQAgentResultsOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                    }
                    else
                    {
                        tvIQAgentResultsOutput.Status = 1;
                        tvIQAgentResultsOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
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
                tvIQAgentResultsOutput.Status = -3;
                tvIQAgentResultsOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                tvIQAgentResultsOutput.Status = -2;
                tvIQAgentResultsOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                tvIQAgentResultsOutput.Status = -1;
                tvIQAgentResultsOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - Get TV Agent Results Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, tvIQAgentResultsOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }

        #endregion

    }
}