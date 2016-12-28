using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using IQMediaGroup.Common.Util;
using IQMedia.Web.Common;
using System.Security.Authentication;
using IQMediaGroup.Logic;
using IQMediaGroup.Domain;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetVideoCategoryData : ICommand
    {
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            CLogger.Debug("GetCategoryData Request Started.");

            string callback = string.Empty;
            VideoCategoryDataOutput output = new VideoCategoryDataOutput();

            try
            {
                FormsAuthenticationTicket formAuthTicket = null;

                if (System.Web.HttpContext.Current.Request.Cookies[".IQAUTH"] != null)
                {
                    formAuthTicket = System.Web.Security.FormsAuthentication.Decrypt(Convert.ToString(System.Web.HttpContext.Current.Request.Cookies[".IQAUTH"].Value));

                    CLogger.Debug("Cookie :" + Convert.ToString(System.Web.HttpContext.Current.Request.Cookies[".IQAUTH"].Value));

                    string[] roles = formAuthTicket.UserData.Split(new char[] { '|' });

                    System.Security.Principal.IIdentity id = new System.Web.Security.FormsIdentity(formAuthTicket);

                    System.Security.Principal.IPrincipal principal = new System.Security.Principal.GenericPrincipal(id, roles);

                    HttpContext.Current.User = principal;

                    CLogger.Debug("User:" + HttpContext.Current.User.Identity.Name);
                }

                if (!Authentication.IsAuthenticated || (formAuthTicket == null || formAuthTicket.Expired))
                {
                    throw new AuthenticationException();
                }
                else
                {
                    var clientGUID = Authentication.CurrentUser.ClientGuid.Value;

                    callback = HttpRequest["callback"];

                    CategoriesServiceLogic categoriesSvcLgc = (CategoriesServiceLogic)LogicFactory.GetLogic(LogicType.CategoryService);

                    Dictionary<string, Guid> categories = categoriesSvcLgc.GetCategoriesByClientGUID(clientGUID);

                    output.Category = categories;
                    output.Status = 0;
                    output.Message = "Success";
                }

            }
            catch (AuthenticationException ex)
            {
                CLogger.Error(ex.Message);

                output.Status = -1;
                output.Message = "Authentication Failed";
            }
            catch (Exception ex) 
            {
                CLogger.Error(ex);

                output.Status = -2;
                output.Message = "Failed";
            }

            var jsonResult =  Serializer.Searialize(output);

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/json";
            CLogger.Debug("GetCategoryData Request Ended");

            HttpResponse.Output.Write(jsonResult);
        }
    }
}
