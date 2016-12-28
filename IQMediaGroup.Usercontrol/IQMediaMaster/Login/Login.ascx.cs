using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Web.Caching;
using System.Net;
using System.Web.UI.HtmlControls;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.Login
{
    public partial class Login : BaseControl
    {
        string _ErrorMessage = "User Name And\\Or Password is wrong.";
        string _SuccessMessage = "Your password has been sent to your email.";
        string _InvalidEmail = "Invalid Email Address";
        string _InvalidLogin = "Login can not be completed,please try again.";
        private string MessageSubject = "IQMedia Password Reminder";
        private string EmailFolder = "EmailTemplate";
        private string EmailFile = "PasswordReminder.htm";

        public event EventHandler LoggedIn;

        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        public bool IsCustomSession { get; set; }

        #region Page Events
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Page.SetFocus(txtUserName);
                /*lblMsg.Text = string.Empty;*/


                if (Request.Cookies.Get(CommonConstants.CookieUserName) != null && !string.IsNullOrEmpty(Request.Cookies.Get(CommonConstants.CookieUserName).Value))
                {
                    txtUserName.Text = Request.Cookies[CommonConstants.CookieUserName].Value;
                }

                // btnSubmit.Attributes.Add("onclick", "javascript:alert('hi');  $('input[type=submit]').attr('disabled', 'disabled'); return true;");

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
        #endregion

        #region Button Events

        protected void btnSubmit_Click(object sender, EventArgs e)
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
                                        HttpContext.Current.Response.Cookies[".IQAUTH"].Domain = ".iqmediacorp.com";
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

                            if (_ListOfCustomerInformation != null && _ListOfCustomerInformation[0].AuthorizedVersion != null
                                        && (_ListOfCustomerInformation[0].AuthorizedVersion == 4))
                            {
                                Response.Redirect(ConfigurationManager.AppSettings["v4SiteURL"], false);
                                return;
                            }
                            ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                            List<CustomerRoles> _ListOfCustomerRoles = new List<CustomerRoles>();
                            _ListOfCustomerRoles = _ICustomerRoleController.GetCustomerRoleAccess(_ListOfCustomerInformation[0].CustomerKey);
                            _SessionInformation.IsLogIn = true;
                            _SessionInformation.IsUgcAutoClip = false;
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
                            //_SessionInformation.AuthorizedVersion = _ListOfCustomerInformation[0].AuthorizedVersion;

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
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.UGCAutoClip.ToString().ToLower())
                                    {
                                        _SessionInformation.IsUgcAutoClip = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremium.ToString().ToLower())
                                    {
                                        _SessionInformation.IsiQPremium = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.MyIQnew.ToString().ToLower())
                                    {
                                        _SessionInformation.IsMyIQnew = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumSM.ToString().ToLower())
                                    {
                                        _SessionInformation.IsiQPremiumSM = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumNM.ToString().ToLower())
                                    {
                                        _SessionInformation.IsiQPremiumNM = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.myiQSM.ToString().ToLower())
                                    {
                                        _SessionInformation.IsmyiQSM = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.myiQNM.ToString().ToLower())
                                    {
                                        _SessionInformation.IsmyiQNM = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.myiQPM.ToString().ToLower())
                                    {
                                        _SessionInformation.IsmyiQPM = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQAgentReport.ToString().ToLower())
                                    {
                                        _SessionInformation.IsiQAgentReport = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.myiQReport.ToString().ToLower())
                                    {
                                        _SessionInformation.IsMyiQReport = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.MyIQTwitter.ToString().ToLower())
                                    {
                                        _SessionInformation.IsMyIQTwitter = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumTwitter.ToString().ToLower())
                                    {
                                        _SessionInformation.IsiQPremiumTwitter = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumAgent.ToString().ToLower())
                                    {
                                        _SessionInformation.IsiQPremiumAgent = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumRadio.ToString().ToLower())
                                    {
                                        _SessionInformation.IsiQPremiumRadio = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumSentiment.ToString().ToLower())
                                    {
                                        _SessionInformation.IsiQPremiumSentiment = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.CompeteData.ToString().ToLower())
                                    {
                                        _SessionInformation.IsCompeteData = true;
                                    }
                                    else if (_RoleName.RoleName.Trim().ToLower() == RolesName.NielsenData.ToString().ToLower())
                                    {
                                        _SessionInformation.IsNielSenData = true;
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
                                        else if (_SessionInformation.IsiQPremium)
                                        {
                                            Response.Redirect("~/IQPremium", false);
                                        }
                                        else if (_SessionInformation.IsMyIQnew)
                                        {
                                            Response.Redirect("~/MyIQ", false);
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
                                        else if (_ListOfCustomerInformation[0].DefaultPage.ToLower() == Pages.iQPremium.ToString().ToLower())
                                        {
                                            Response.Redirect("~/IQPremium", false);
                                        }
                                        else if (_ListOfCustomerInformation[0].DefaultPage.ToLower() == Pages.MyIQnew.ToString().ToLower())
                                        {
                                            Response.Redirect("~/MyIQ", false);
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
                    if (pplEmail.IsValid == false)
                    {
                        txtUserName.Text = "";
                    }
                    if (ppvForgotPassword.IsValid == false)
                    {
                        txtPassword.Text = "";
                    }
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            try
            {

                string _Result = string.Empty;
                if (Page.IsValid)
                {
                    List<Customer> _ListOfCustomerInformation = new List<Customer>();
                    ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                    _ListOfCustomerInformation = _ICustomerController.ForgotPassword(txtUserName.Text);


                    if (_ListOfCustomerInformation.Count > 0)
                    {


                        //Code of Mail Sending to admin
                        string _EmailtoCustomer = string.Empty;
                        SmtpClient _SmtpClient = new SmtpClient();
                        MailMessage _msgCsutomer = new MailMessage();
                        StreamReader _StreamReader;
                        _EmailtoCustomer = txtUserName.Text;
                        string strEmail_Body = _EmailtoCustomer;
                        _msgCsutomer.To.Add(_EmailtoCustomer);
                        _msgCsutomer.Subject = MessageSubject;
                        _StreamReader = File.OpenText(Server.MapPath("~/" + EmailFolder + "/" + EmailFile));
                        strEmail_Body = _StreamReader.ReadToEnd();
                        _StreamReader.Close();
                        strEmail_Body = strEmail_Body.Replace("#FirstName#", Convert.ToString(_ListOfCustomerInformation[0].FirstName));
                        strEmail_Body = strEmail_Body.Replace("#Password#", Convert.ToString(_ListOfCustomerInformation[0].Pwd));
                        _msgCsutomer.IsBodyHtml = true;
                        _msgCsutomer.Body = strEmail_Body;
                        _SmtpClient.Host = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServer"));
                        _SmtpClient.Send(_msgCsutomer);

                        //End Code of Mail Sending

                        lblError.Visible = true;
                        lblError.Text = _SuccessMessage;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = _InvalidEmail;
                    }
                }

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnSessionTimeOut_Click(object sender, EventArgs e)
        {
            SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
            _SessionInformation = null;
            Session.Abandon();

        }

        #endregion
    }
}