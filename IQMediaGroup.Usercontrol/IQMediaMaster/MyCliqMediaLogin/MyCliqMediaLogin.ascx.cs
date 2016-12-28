using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using System.Configuration;
using IQMediaGroup.Core.Enumeration;
using System.Web.Caching;
using IQMediaGroup.Usercontrol.Base;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.MyCliqMediaLogin
{
    public partial class MyCliqMediaLogin : BaseControl
    {

        string _ErrorMessage = "Wrong credentials.";
        string _SuccessMessage = "Your password has been sent to your email.";
        string _InvalidEmail = "Invalid Email Address";
        string _InvalidLogin = "Login can not be completed,please try again.";
        public event EventHandler LoggedIn;
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        public bool IsCustomSession { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                if (Page.IsValid)
                {
                    List<Customer> _ListOfCustomerInformation = new List<Customer>();
                    IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                    ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();

                    _ListOfCustomerInformation = _ICustomerController.CheckAuthentication(txtUserName.Text, txtPassword.Text);
                    if (_ListOfCustomerInformation.Count == 0)
                    {
                        lblError.Visible = true;
                        lblError.Text = _ErrorMessage;
                    }
                    else
                    {
                        int _Service = 0;
                        bool _IsLogin = false;

                        if (ConfigurationManager.AppSettings["ServicesBaseURL"] != null && int.TryParse(ConfigurationManager.AppSettings["ServicesBaseURL"], out _Service))
                        {
                            if (_Service == 1 || _Service == 2)
                            {
                                var result = IQMedia.Web.Common.Authentication.Login(txtUserName.Text, txtPassword.Text);

                                if (result == IQMedia.Web.Common.Authentication.LoginStatus.Success)
                                {
                                    _IsLogin = true;

                                    if (HttpContext.Current.Response.Cookies[".IQAUTH"] != null)
                                    {
                                        HttpContext.Current.Response.Cookies[".IQAUTH"].Domain = ".mycliqmedia.com";
                                    }
                                }
                            }
                        }

                        if (_IsLogin == true)
                        {
                            /* if (chkRememberME.Checked == true)
                             {
                                 HttpCookie _HttpCookie = new HttpCookie(CommonConstants.CookieUserName, txtUserName.Text);
                                 Response.Cookies.Add(_HttpCookie);
                             }*/

                            ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                            List<CustomerRoles> _ListOfCustomerRoles = new List<CustomerRoles>();
                            _ListOfCustomerRoles = _ICustomerRoleController.GetCustomerRoleAccess(_ListOfCustomerInformation[0].CustomerKey);
                            List<CustomerClientRoleAccess> _ListOfCustomerClientRoleAccess = new List<CustomerClientRoleAccess>();
                            _ListOfCustomerClientRoleAccess = _ICustomerRoleController.GetCustomerClientRoleAccess(_ListOfCustomerInformation[0].CustomerKey);
                            _SessionInformation.IsLogIn = true;
                            _SessionInformation.CustomerClientRoles = _ListOfCustomerClientRoleAccess;


                            _SessionInformation.ClientID = _ListOfCustomerInformation[0].ClientID;
                            _SessionInformation.CustomerKey = _ListOfCustomerInformation[0].CustomerKey;
                            _SessionInformation.ClientName = _ListOfCustomerInformation[0].ClientName;
                            _SessionInformation.ClientGUID = _ListOfCustomerInformation[0].ClientGUID;
                            _SessionInformation.FirstName = _ListOfCustomerInformation[0].FirstName;
                            _SessionInformation.Email = _ListOfCustomerInformation[0].Email;
                            _SessionInformation.CustomeHeaderImage = _ListOfCustomerInformation[0].CustomHeaderImage;
                            _SessionInformation.ISCustomeHeader = _ListOfCustomerInformation[0].IsCustomHeader;
                            _SessionInformation.CustomerGUID = _ListOfCustomerInformation[0].CustomerGUID;
                            _SessionInformation.MultiLogin = CommonFunctions.GetBoolValue(Convert.ToString(_ListOfCustomerInformation[0].MultiLogin));
                            _SessionInformation.IsClientPlayerLogoActive = CommonFunctions.GetBoolValue(Convert.ToString(_ListOfCustomerInformation[0].IsClientPlayerLogoActive));
                            _SessionInformation.ClientPlayerLogoImage = _ListOfCustomerInformation[0].ClientPlayerLogoImage;
                            IQMediaGroup.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);

                            CurrentUsers _CurrentUsers = new CurrentUsers();

                            _CurrentUsers.CustomerKey = _SessionInformation.CustomerKey;
                            _CurrentUsers.SessionID = HttpContext.Current.Session.SessionID;
                            _CurrentUsers.EndTime = (DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout));
                            _CurrentUsers.IsActive = true;
                            _CurrentUsers.MultiLogin = _SessionInformation.MultiLogin;

                            List<CurrentUsers> _ListOfCurrentUsers = null;
                            bool _IsActiveUser = false;

                            if (_CurrentUsers.MultiLogin == false || _CurrentUsers.MultiLogin == null)
                            {
                                if (HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers) == null)
                                {
                                    _ListOfCurrentUsers = new List<CurrentUsers>();
                                }
                                else
                                {
                                    _ListOfCurrentUsers = (List<CurrentUsers>)HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers);

                                    foreach (CurrentUsers _CurrentUsersInList in _ListOfCurrentUsers)
                                    {
                                        if (_CurrentUsersInList.CustomerKey == _SessionInformation.CustomerKey && _CurrentUsersInList.SessionID != HttpContext.Current.Session.SessionID)
                                        {
                                            _CurrentUsersInList.IsActive = false;
                                        }
                                        else if (_CurrentUsersInList.CustomerKey == _SessionInformation.CustomerKey && _CurrentUsersInList.SessionID == HttpContext.Current.Session.SessionID && _CurrentUsersInList.IsActive != false)
                                        {
                                            _IsActiveUser = true;
                                        }
                                    }
                                }

                                if (_IsActiveUser == false)
                                {
                                    _ListOfCurrentUsers.Add(_CurrentUsers);
                                }

                                HttpContext.Current.Cache.Insert(CommonConstants.CurrentUsers, _ListOfCurrentUsers, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                            }


                            List<Role> _ListOfRole = new List<Role>();
                            Role _Role = new Role();
                            int _CustomerID = _ListOfCustomerInformation[0].CustomerKey;
                            IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                            _ListOfRole = _IRoleController.GetRoleName(_CustomerID);
                            if (_ListOfRole.Count > 0)
                            {
                                _SessionInformation._ListOfRoleName = _ListOfRole;

                                //_ListOfRole
                                foreach (Role _RoleName in _ListOfRole)
                                {
                                    if (_RoleName.RoleName == RolesName.myIQAccess.ToString())
                                    {
                                        _SessionInformation.IsmyIQ = true;
                                    }
                                    else if (_RoleName.RoleName == RolesName.IQBasic.ToString())
                                    {
                                        _SessionInformation.IsiQBasic = true;
                                    }
                                    else if (_RoleName.RoleName == RolesName.AdvancedSearchAccess.ToString())
                                    {
                                        _SessionInformation.IsiQAdvance = true;
                                    }
                                    else if (_RoleName.RoleName == RolesName.IQAgentWebsiteAccess.ToString())
                                    {
                                        _SessionInformation.IsiQAgent = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.DownloadClips.ToString().ToLower())
                                    {
                                        _SessionInformation.IsDownloadClips = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.IQCustomAccess.ToString().ToLower())
                                    {
                                        _SessionInformation.IsiQCustom = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.UGCDownload.ToString().ToLower())
                                    {
                                        _SessionInformation.IsUGCDownload = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.UGCUploadEdit.ToString().ToLower())
                                    {
                                        _SessionInformation.IsUGCUploadEdit = true;
                                    }
                                }

                                if (IsCustomSession == false)
                                {
                                    IQMediaGroup.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);

                                    if (string.IsNullOrEmpty(_ListOfCustomerInformation[0].DefaultPage))
                                    {
                                        if (_SessionInformation.IsiQBasic == true)
                                        {
                                            Response.Redirect("~/ClipandRawMedia", false);
                                        }
                                        else if (_SessionInformation.IsmyIQ == true)
                                        {
                                            Response.Redirect("~/MyClips", false);
                                        }
                                        else if (_SessionInformation.IsiQAdvance == true)
                                        {
                                            Response.Redirect("~/IQAdvance", false);
                                        }
                                        else if (_SessionInformation.IsiQAgent == true)
                                        {
                                            Response.Redirect("~/IQAgent", false);
                                        }
                                        else if (_SessionInformation.IsiQCustom)
                                        {
                                            Response.Redirect("~/IQCustom", false);
                                        }
                                        else
                                        {
                                            Response.Redirect("~/NoRole/", false);
                                        }
                                    }
                                    else
                                    {

                                        if (_ListOfCustomerInformation[0].DefaultPage.ToLower() == Pages.IQBasic.ToString().ToLower())
                                        {
                                            Response.Redirect("~/ClipandRawMedia", false);
                                        }
                                        else if (_ListOfCustomerInformation[0].DefaultPage.ToLower() == Pages.myIQ.ToString().ToLower())
                                        {
                                            Response.Redirect("~/MyClips", false);
                                        }
                                        else if (_ListOfCustomerInformation[0].DefaultPage.ToLower() == Pages.IQCustom.ToString().ToLower())
                                        {
                                            Response.Redirect("~/IQCustom", false);
                                        }
                                        else if (_ListOfCustomerInformation[0].DefaultPage.ToLower() == Pages.IQAdvance.ToString().ToLower())
                                        {
                                            Response.Redirect("~/IQAdvance", false);
                                        }
                                        else if (_ListOfCustomerInformation[0].DefaultPage.ToLower() == Pages.IQAgent.ToString().ToLower())
                                        {
                                            Response.Redirect("~/IQAgent", false);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LoggedIn != null)
                                    {
                                        this.LoggedIn(this, new EventArgs());
                                    }
                                }

                            }
                            else
                            {
                                if (IsCustomSession == false)
                                {
                                    Response.Redirect("~/NoRole/", false);
                                }
                                else
                                {
                                    if (LoggedIn != null)
                                    {
                                        this.LoggedIn(this, new EventArgs());
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = _InvalidLogin;
                        }
                    }
                }
                else
                {
                    //if (pplEmail.IsValid == false)
                    //{
                    //    txtUserName.Text = "";
                    //}
                    //if (ppvForgotPassword.IsValid == false)
                    //{
                    //    txtPassword.Text = "";
                    //}
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

      
    }
}