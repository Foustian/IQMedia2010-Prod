using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.SessionState;
using IQMediaGroup.Core.HelperClasses;
using System.Configuration;
using IQMediaGroup.Core.Enumeration;
using System.IO;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;

namespace IQMediaGroup.WebApplication
{
    public class Global : System.Web.HttpApplication
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        protected void Application_Start(object sender, EventArgs e)
        {
            //Uncomment one of the following lines to select a processor

            //UploadManager.Instance.BufferSize = 1024;
            //UploadManager.Instance.ProcessorType = typeof(FileSystemProcessor);
            ////UploadManager.Instance.ProcessorType = typeof(SQLProcessor);
            //UploadManager.Instance.ProcessorInit += new FileProcessorInitEventHandler(Processor_Init);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {


        }

        protected void Application_PostAcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                //if (Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState)
                //{
                //    if (IQMedia.Web.Common.Authentication.IsAuthenticated)
                //    {
                //        //HttpContext.Current.Request.Url
                //        if (!HttpContext.Current.Request.Url.ToString().ToLower().EndsWith("norole/default.aspx"))
                //        //if (!Convert.ToString(HttpContext.Current.Request.Url).Contains("NoRole"))
                //        {
                //            IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                //            if (_SessionInformation != null && !_SessionInformation.IsLogIn)
                //            {
                //                var currentUser = IQMedia.Web.Common.Authentication.CurrentUser;
                //                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                //                List<Customer> listofCustomer = _ICustomerController.GetCustomerByCustomerGUIDForAuthentication(currentUser.Guid);

                //                if (listofCustomer != null && listofCustomer[0].AuthorizedVersion != null
                //                        && (listofCustomer[0].AuthorizedVersion == 0 || listofCustomer[0].AuthorizedVersion == 3))
                //                {
                //                    List<Role> _ListOfRole = new List<Role>();
                //                    Role _Role = new Role();
                //                    int _CustomerID = listofCustomer[0].CustomerKey;
                //                    IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                //                    _ListOfRole = _IRoleController.GetRoleName(_CustomerID);

                //                    if (listofCustomer != null && listofCustomer.Count > 0)
                //                    {
                //                        bool hasRole = CommonFunctions.SetSessionInformationByFormAuthentication(listofCustomer[0], _ListOfRole);
                //                        if (!hasRole)
                //                        {
                //                            Response.Redirect("~/NoRole/", false);
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    Response.Redirect("~/NoRole/", false);
                //                }

                //            }
                //            else
                //            {
                //                if (_SessionInformation != null && _SessionInformation.AuthorizedVersion != null
                //                          && !(_SessionInformation.AuthorizedVersion == 0 || _SessionInformation.AuthorizedVersion == 3))
                //                {
                //                    Response.Redirect("~/NoRole/", false);
                //                }

                //            }
                //        }
                //    }
                //    else
                //    {
                //        CommonFunctions.SetSessionInformation(null);
                //        Session.Abandon();

                //    }
                //}
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.Page _Page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
                CommonFunctions.RemoveCookies(_Page);
            }
            catch (Exception _Exception)
            {
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }

        protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Initialises the file processor.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Arguments</param>
        //void Processor_Init(object sender, FileProcessorInitEventArgs args)
        //{
        //    if (args.Processor is FileSystemProcessor)
        //    {
        //        FileSystemProcessor processor;

        //        processor = args.Processor as FileSystemProcessor;

        //        // Set up the download path here - default to the root of the web application
        //        //string ClipUploadLocation = Server.MapPath("~/UGC-Content/");

        //        //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[CommonConstants.ConfigUGCFileLoadLocation]) && Directory.Exists(ConfigurationManager.AppSettings[CommonConstants.ConfigUGCFileLoadLocation]))
        //        //{
        //        //    ClipUploadLocation = ConfigurationManager.AppSettings[CommonConstants.ConfigUGCFileLoadLocation];
        //        //}

        //        //fuMedia.SaveAs(ClipUploadLocation + fuMedia.FileName.Substring(0, fuMedia.FileName.LastIndexOf(".")) + "_" + DateTime.Now.ToString("MMddyyyy_hhmmss") + ReturnExtension(fuMedia.PostedFile.ContentType));


        //        processor.OutputPath = ConfigurationManager.AppSettings[CommonConstants.ConfigUGCFileLoadLocation];
        //    }
        //}
    }
}