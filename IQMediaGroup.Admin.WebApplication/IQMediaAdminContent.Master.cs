using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Admin.Core.Enumeration;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.ServiceProcess;
using System.Configuration;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Usercontrol.Base;

namespace IQMediaGroup.Admin.WebApplication
{
    public partial class IQMediaAdminContent : System.Web.UI.MasterPage
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        #region Page Events

        public int SetSMTimeOut
        {
            get
            {
                return ScriptManager1.AsyncPostBackTimeout;
            }
            set
            {
                ScriptManager1.AsyncPostBackTimeout = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UserAgent.IndexOf("AppleWebKit") > 0)
            {
                Request.Browser.Adapters.Clear();
            }

            

            SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
            try
            {
                //    if (!IsPostBack)
                //    {
                //        Session.Timeout = 20;
                //    }                

                if (_SessionInformation == null || _SessionInformation.IsAdminLogin != true)
                {
                    //_SessionInformation = CommonFunctions.GetSessionInformation()
                    //_SessionInformation.ErrorMessage = CommonConstants.SessionTimeOutMsg;
                    //CommonFunctions.SetSessionInformation(_SessionInformation);
                    Response.Redirect("~/Login/");
                }



                if (_SessionInformation.IsIQMediaAdmin == null || _SessionInformation.IsIQMediaAdmin != true)
                {
                    _SessionInformation.IsAdminLogin = false;
                    Response.Redirect("~/Login/");
                }

                //    if (HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers) != null)
                //    {
                //        List<CurrentUsers> _ListOfCurrentUsers = (List<CurrentUsers>)HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers);

                //        //CurrentUsers _ExistingUsers = _ListOfCurrentUsers.Find(delegate(CurrentUsers _User) { return _User.SessionID == HttpContext.Current.Session.SessionID && _User.IsActive == false; });

                //        CurrentUsers _ExistingUsers = _ListOfCurrentUsers.Find(delegate(CurrentUsers _User) { return _User.SessionID == HttpContext.Current.Session.SessionID && _User.CustomerKey == _SessionInformation.CustomerKey; });

                //        if (_ExistingUsers != null && _ExistingUsers.IsActive == false)
                //        {
                //            //Session.Abandon();
                //            //CommonConstants.IsLogout = true;
                //            Response.Redirect(CommonConstants.HomePage);
                //        }
                //    }

                //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //    Response.Cache.SetAllowResponseInBrowserHistory(false);
                //    Response.Cache.SetNoStore();
                //    Response.Cache.SetExpires(DateTime.MinValue);

                //hlogo.HRef = "~/" + "Home/";

                /* if (_SessionInformation.IsPlayRawMedia == true)
                 {
                     _SessionInformation.IsPlayRawMedia = false;

                     CommonFunctions.SetSessionInformation(_SessionInformation);
                     ServiceController _ArchiveClipService = new ServiceController(CommonConstants.MyClipsWinService);

                     if (_ArchiveClipService != null)
                     {
                         if (_ArchiveClipService.Status != ServiceControllerStatus.Stopped)
                         {                            
                             _ArchiveClipService.Stop();
                             _ArchiveClipService.WaitForStatus(ServiceControllerStatus.Stopped);
                             _ArchiveClipService.Start();
                         }
                         else
                         {
                             _ArchiveClipService.Start();
                         }
                     }

                     //ServiceController[] _ServiceControllerArray;
                     //_ServiceControllerArray = ServiceController.GetServices();

                     //var _varOfServiceControllerArrayByAsc =
                     //        from _ServiceController in _ServiceControllerArray
                     //        orderby _ServiceController.ServiceName
                     //        select _ServiceController;

                     //List<ServiceController> _ServiceControllerArraylist = new List<ServiceController>(_varOfServiceControllerArrayByAsc);

                     //foreach (ServiceController _ServiceController in _ServiceControllerArraylist)
                     //{
                     //    if (_ServiceController.ServiceName == CommonConstants.MyClipsWinService)
                     //    {
                     //        if (_ServiceController.Status != ServiceControllerStatus.Stopped)
                     //        {
                     //            _ServiceController.Stop();
                     //        }

                     //        _ServiceController.Start();
                     //    }
                     //}
                 }*/

                if (!IsPostBack)
                {
                    BuildMenu();
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetAllowResponseInBrowserHistory(false);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.MinValue);
              
            }
            catch (System.Threading.ThreadAbortException _ThreadAbortException)
            {

            }
            catch (Exception _Exception)
            {

                BaseControl _BaseControl = new BaseControl();
                _BaseControl.WriteException(_Exception);
            }

        }

        private void BuildMenu()
        {
            SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

            long CustomerID = _SessionInformation.AdminUserKey;
            ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
            List<Customer> _ListOfCustomer = _ICustomerController.GetCustomerByCustomerIDRoleName(CustomerID, "IQAgentAdminAccess");

            /*MenuItem itemContentManagement = new MenuItem("Content Management");
            itemContentManagement.Selectable = false;
            itemContentManagement.ChildItems.Add(new MenuItem("Career", "", "", "~/CareerContent/"));
            itemContentManagement.ChildItems.Add(new MenuItem("Contact Us", "", "", "~/ContactUsContent/"));
            itemContentManagement.ChildItems.Add(new MenuItem("About Us", "", "", "~/AboutUsContent/"));
            itemContentManagement.ChildItems.Add(new MenuItem("Product", "", "", "~/ProductContent/"));
            Menu1.Items.Add(itemContentManagement);*/

            MenuItem itemRegistration = new MenuItem("Registration");
            itemRegistration.Selectable = false;
            itemRegistration.ChildItems.Add(new MenuItem("Client Registration", "", "", "~/ClientRegistration/"));
            itemRegistration.ChildItems.Add(new MenuItem("Customer Registration", "", "", "~/CustomerRegistration/"));
            Menu1.Items.Add(itemRegistration);


            //MenuItem itemRoleMapping = new MenuItem("Role Mapping");
            //itemRoleMapping.Selectable = false;
            //itemRoleMapping.ChildItems.Add(new MenuItem("IQ Role", "", "", "~/IQRole/"));
            //itemRoleMapping.ChildItems.Add(new MenuItem("Customer Role Mapping", "", "", "~/CustomerRole/"));
            //itemRoleMapping.ChildItems.Add(new MenuItem("Client Role Mapping", "", "", "~/ClientRole/"));
            //Menu1.Items.Add(itemRoleMapping);

            //MenuItem itemExport = new MenuItem("Export");
            //itemExport.Selectable = false;
            //itemExport.ChildItems.Add(new MenuItem("Clip Export", "", "", "~/ClipExport/"));
            //Menu1.Items.Add(itemExport);

            if (_ListOfCustomer.Count > 0)
            {
                MenuItem itemIQAgent = new MenuItem("IQAgent");
                itemIQAgent.Selectable = false;
                //itemIQAgent.ChildItems.Add(new MenuItem("Agent Setup", "", "", "~/IQAgentSetup/"));
                itemIQAgent.ChildItems.Add(new MenuItem("Agent Queries", "", "", "~/IQAgentConsole/"));
                Menu1.Items.Add(itemIQAgent);
            }

            MenuItem itemCND = new MenuItem("CDN");
            itemCND.Selectable = false;
            itemCND.ChildItems.Add(new MenuItem("CDN Upload Client", "", "", "~/CDNUploadClient/"));
            Menu1.Items.Add(itemCND);

            MenuItem itemClientSettings = new MenuItem("Settings");
            itemClientSettings.Selectable = false;
            itemClientSettings.ChildItems.Add(new MenuItem("Client Search Settings", string.Empty, string.Empty, "~/ClientSearchSettings/"));
            
            Menu1.Items.Add(itemClientSettings);

        }

        #endregion

        #region Button Events
        protected void ImgbtnContectUs_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/" + "Contact/");
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + "Home/");
        }

        protected void lnkProducts_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + "Product/");
        }

        protected void lnkAboutUs_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + "AboutUs/");
        }

        protected void lnkCareer_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + "Careers/");
        }

        protected void lnkContactUs_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + "ContactUs/");
        }

        protected void SiteMapPath1_ItemDataBound(object sender, SiteMapNodeItemEventArgs e)
        {
            e.Item.SiteMapNode.Url = e.Item.SiteMapNode.Url.Replace("Default.aspx", string.Empty);
        }

        //protected void Unnamed1_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/" + "Home/");
        //}        

        #endregion
    }
}
