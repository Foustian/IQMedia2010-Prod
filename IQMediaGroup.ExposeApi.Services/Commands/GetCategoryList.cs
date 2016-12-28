using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.ExposeApi.Domain;
using IQMediaGroup.ExposeApi.Logic;
using IQMedia.Web.Common;
using IQMediaGroup.Common.Util;
using System.Security.Authentication;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class GetCategoryList : ICommand
    {
        public String _Format { get; private set; }

        public GetCategoryList(object Format)
        {
            _Format = (Format is NullParameter) ? null : (String)Format;
        }

        #region ICommand Members

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var categoryOutput = new CategoryOutput();
            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetCategoryList Request Started");

            try
            {
                CategoryInput categoryInput = IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.InitializeRequest<CategoryInput>(p_HttpRequest, _Format);

                Guid CustomerGUID = Authentication.CurrentUser.Guid;
                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                if (authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4TV, CustomerGUID) && authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4Library, CustomerGUID))
                {
                    CategoryLogic categoryLogic = (CategoryLogic)LogicFactory.GetLogic(LogicType.Category);

                    categoryOutput.CategoryList = categoryLogic.GetCategoryList(Authentication.CurrentUser.ClientGuid.Value);
                    if (categoryOutput.CategoryList != null && categoryOutput.CategoryList.Count() > 0)
                    {
                        categoryOutput.Status = 0;
                        categoryOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.SuccessMessage;
                    }
                    else
                    {
                        categoryOutput.Status = 1;
                        categoryOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.NoResultsFoundMessage;
                    }
                }
                else
                {
                    throw new CustomException(IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.AccessDeniedMessage);
                }
            }
            catch (AuthenticationException ex)
            {
                categoryOutput.Status = -3;
                categoryOutput.Message = ex.Message;
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                categoryOutput.Status = -2;
                categoryOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
                categoryOutput.Status = -1;
                categoryOutput.Message = IQMediaGroup.ExposeApi.Logic.Config.ConfigSettings.MessageSettings.ErrorMessage;
            }

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetCategoryList Request Ended");
            IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.ReturnResponse(p_HttpResponse, categoryOutput, _Format, null, false, IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog());
        }

        #endregion
    }
}