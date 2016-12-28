using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Model;
using IQMedia.Web.Logic.Base;
using System.Configuration;
using System.Net;
using System.IO;
using IQMedia.Web.Common;
using IQMedia.WebApplication.Utility;
using IQMedia.WebApplication.Config;
using IQMedia.Common.Util;
using System.Security.Authentication;
using System.Web.SessionState;
using System.Security.Principal;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class SignInController : Controller
    {
        //
        // GET: /SignIn/

        public ActionResult Index()
        {
            try
            {                

                ViewBag.IsSuccess = true;
                if (ConfigurationManager.AppSettings["IsMaintenance"] == "true")
                {
                    return RedirectToAction("Index", "UnderMaintenance");
                }
                else
                {
                    bool isUserAlreadyAuthenticated = IQMedia.WebApplication.Utility.CommonFunctions.CheckAuthentication();
                    if (isUserAlreadyAuthenticated)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        if (Authentication.IsAuthenticated)
                        {
                            Authentication.Logout();
                            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                        }

                        Session.Timeout = 1;
                        
                        return View();
                    }
                }
            }
            catch (Exception exception)
            {
                UtilityLogic.WriteException(exception);
                ViewBag.IsSuccess = false;
                return View();
            }
        }

        [HttpPost]
        public ActionResult SignIn(CustomerModel customer)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                    CustomerModel customerValue = customerLogic.CheckAuthentication(customer.LoginID.Trim(), customer.Password.Trim());
                    if (customerValue != null && !string.IsNullOrWhiteSpace(customerValue.FirstName))
                    {
                        if (customerValue.AuthorizedVersion.HasValue && (customerValue.AuthorizedVersion == 0 || customerValue.AuthorizedVersion == 4))
                        {

                            int _Service = 0;
                            bool _IsLogin = false;

                            if (ConfigurationManager.AppSettings["ServicesBaseURL"] != null && int.TryParse(ConfigurationManager.AppSettings["ServicesBaseURL"], out _Service))
                            {
                                //if (_Service == 1 || _Service == 2)
                                //{
                                var result = IQMedia.Web.Common.Authentication.Login(customer.LoginID.Trim(), customer.Password.Trim());

                                if (result == IQMedia.Web.Common.Authentication.LoginStatus.Success)
                                {
                                    _IsLogin = true;
                                    // HttpCookie cookie = new HttpCookie(".IQAUTH");
                                    //cookie.Domain = ".iqmediacorp.com";

                                    //this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);


                                }
                                else
                                {
                                    Logger.Info("login failed for User :" + customer.LoginID.Trim());
                                }
                                //}
                            }

                            if (_IsLogin)
                            {


                                SessionInformation sessionInformation = new SessionInformation();
                                sessionInformation.ClientGUID = customerValue.ClientGUID;
                                sessionInformation.ClientID = customerValue.ClientID;
                                sessionInformation.ClientName = customerValue.ClientName;
                                sessionInformation.ClientPlayerLogoImage = customerValue.ClientPlayerLogoImage;
                                sessionInformation.CustomerKey = customerValue.CustomerKey;
                                sessionInformation.Email = customerValue.Email;
                                sessionInformation.LoginID = customerValue.LoginID.Trim();
                                sessionInformation.FirstName = customerValue.FirstName;
                                sessionInformation.LastName = customerValue.LastName;
                                sessionInformation.IsClientPlayerLogoActive = customerValue.IsClientPlayerLogoActive;
                                sessionInformation.IsLogIn = true;
                                sessionInformation.IsUgcAutoClip = customerValue.IsUGCAutoClip;
                                sessionInformation.MultiLogin = customerValue.MultiLogin;
                                sessionInformation.CustomerGUID = customerValue.CustomerGUID;
                                sessionInformation.AuthorizedVersion = customerValue.AuthorizedVersion;
                                sessionInformation.DefaultPage = customerValue.DefaultPage;
                                sessionInformation.TimeZone = customerValue.TimeZone;
                                sessionInformation.dst = customerValue.dst;
                                sessionInformation.gmt = customerValue.gmt;
                                sessionInformation.MCID = customerValue.MCID;
                                sessionInformation.MasterCustomerID = customerValue.MasterCustomerID == null ? customerValue.CustomerKey : customerValue.MasterCustomerID.Value;

                                // Customer Roles Information

                                sessionInformation.Isv4FeedsAccess = customerValue.Isv4FeedsAccess;
                                sessionInformation.Isv4DiscoveryAccess = customerValue.Isv4DiscoveryAccess;
                                sessionInformation.Isv4LibraryAccess = customerValue.Isv4LibraryAccess;
                                sessionInformation.Isv4TimeshiftAccess = customerValue.Isv4TimeshiftAccess;
                                sessionInformation.Isv4TAdsAccess = customerValue.Isv4TAdsAccess;
                                sessionInformation.Isv4DashboardAccess = customerValue.Isv4DashboardAccess;
                                sessionInformation.Isv4LibraryDashboardAccess = customerValue.Isv4LibraryDashboardAccess;
                                sessionInformation.Isv4TimeshiftRadioAccess = customerValue.Isv4TimeshiftRadioAccess;
                                sessionInformation.Isv4SetupAccess = customerValue.Isv4SetupAccess;
                                sessionInformation.isv4OptiqAccess = customerValue.isv4OptiqAccess;
                                sessionInformation.IsGlobalAdminAccess = customerValue.IsGlobalAdminAccess;
                                sessionInformation.Isv4UGCAccess = customerValue.Isv4UGCAccess;
                                sessionInformation.Isv4IQAgentAccess = customerValue.Isv4IQAgentAccess;
                                sessionInformation.Isv4UGCDownload = customerValue.IsUGCDownload;
                                sessionInformation.Isv4UGCUploadEdit = customerValue.IsUGCUploadEdit;
                                sessionInformation.Isv4TV = customerValue.Isv4TV;
                                sessionInformation.Isv4NM = customerValue.Isv4NM;
                                sessionInformation.Isv4SM = customerValue.Isv4SM;
                                sessionInformation.Isv4TW = customerValue.Isv4TW;
                                sessionInformation.Isv4TM = customerValue.Isv4TM;
                                sessionInformation.Isv4CustomImage = customerValue.Isv4CustomImage;
                                sessionInformation.IsCompeteData = customerValue.IsCompeteData;
                                sessionInformation.IsNielsenData = customerValue.IsNielsenData;
                                sessionInformation.Isv4BLPM = customerValue.Isv4BLPM;
                                sessionInformation.IsNewsRights = customerValue.IsNewsRights;
                                sessionInformation.Isv4CustomSettings = customerValue.Isv4CustomSettings;
                                sessionInformation.Isv4DiscoveryLiteAccess = customerValue.Isv4DiscoveryLiteAccess;
                                sessionInformation.IsfliQAdmin = customerValue.IsfliQAdmin;
                                sessionInformation.Isv4PQ = customerValue.Isv4PQ;
                                sessionInformation.IsMediaRoomContributor = customerValue.IsMediaRoomContributor;
                                sessionInformation.IsMediaRoomEditor = customerValue.IsMediaRoomEditor;
                                sessionInformation.Isv4Google = customerValue.Isv4Google;
                                sessionInformation.IsTimeshiftFacet = customerValue.IsTimeshiftFacet;
                                sessionInformation.IsShareTV = customerValue.IsShareTV;

                                Session["SessionInformation"] = sessionInformation;
                                //return RedirectToAction("Index", "Feeds");

                                if (ConfigurationManager.AppSettings["UseInfoAlert"] == "true")
                                {
                                    RedirectToAction("Index", "InfoAlert");
                                }
                                else
                                {
                                    RedirectToDefaultPage(sessionInformation.DefaultPage);
                                }
                            }
                        }
                        else if (customerValue.AuthorizedVersion.HasValue && customerValue.AuthorizedVersion.Value == 3)
                        {
                            Response.Redirect(ConfigurationManager.AppSettings["v3WebsiteUrl"]);
                        }
                        else
                        {
                            Logger.Info("User do not have access to website ,  user email :" + customer.LoginID.Trim());
                            return RedirectToAction("Unauthorized", "Error");
                        }
                    }
                    else
                    {
                        Logger.Info("username and password  do not match for :" + customer.LoginID.Trim());
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Logger.Fatal("An error occured while login", ex);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]        
        [ValidateAntiForgeryToken]
        public ActionResult Index(CustomerModel customer)
        {
            System.Web.Configuration.SessionStateSection sessionStateSection = (System.Web.Configuration.SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");

            var cookieName = sessionStateSection.CookieName;
            var sessionCookieVal = Request.Cookies[cookieName].Value;

            if (!Session.IsNewSession)
            {
                Session.Timeout = 1;
                ViewBag.IsSuccess = false;
                ViewBag.Message = ConfigSettings.Settings.ErrorOccurred;
                ViewBag.Status = -3;                

                var msg = "Session Cookie is not empty " + sessionCookieVal;

                Logger.Fatal(msg);
                UtilityLogic.WriteException(new Exception(msg));

                return View();
            }
            else
            {
                ViewBag.Message = string.Empty;
                ViewBag.IsSuccess = true;
                ViewBag.Status = 0;

                if (ModelState.IsValid)
                {
                    try
                    {
                        customer.LoginID = customer.LoginID.Trim();
                        customer.Password = customer.Password.Trim();

                        CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                        bool isAuthenticated = customerLogic.CheckAuthentication(customer.LoginID, customer.Password, Convert.ToInt32(ConfigurationManager.AppSettings["MaxPasswordAttempts"]));

                        if (isAuthenticated)
                        {
                            CustomerModel customerValue = customerLogic.GetCustomerDetailsByLoginID(customer.LoginID);

                            if (customerValue != null && !string.IsNullOrWhiteSpace(customerValue.FirstName))
                            {
                                if (customerValue.AuthorizedVersion.HasValue && (customerValue.AuthorizedVersion == 0 || customerValue.AuthorizedVersion == 4))
                                {

                                    int _Service = 0;
                                    bool _IsLogin = false;

                                    // bool _AllowUserToLogin = true;                           

                                    /*if (_AllowUserToLogin)
                                    {*/
                                    if (ConfigurationManager.AppSettings["ServicesBaseURL"] != null && int.TryParse(ConfigurationManager.AppSettings["ServicesBaseURL"], out _Service))
                                    {
                                        //if (_Service == 1 || _Service == 2)
                                        //{
                                        var result = IQMedia.Web.Common.Authentication.Login(customer.LoginID, customer.Password);

                                        if (result == IQMedia.Web.Common.Authentication.LoginStatus.Success)
                                        {
                                            _IsLogin = true;
                                        }
                                        else
                                        {
                                            Logger.Info("login failed for user :" + customer.LoginID.Trim());
                                        }
                                        //}
                                    }

                                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["SessionDebug"]))
                                    {
                                        Logger.Debug(" IP: " + HttpContext.Request.Headers["X-ClientIP"] + " - " + HttpContext.Request.UserHostAddress + " SessionID: " + Session.SessionID + " Url: " + HttpContext.Request.Url.ToString() + "Req Cookies: " + HttpContext.Request.Headers["Cookie"] + "Res Cookies" + HttpContext.Response.Headers["Set-Cookie"] + " SignIn");
                                    }

                                    if (_IsLogin)
                                    {
                                        List<CustomerRoleModel> customerRoles = customerLogic.GetCustomerRoles(customerValue.CustomerGUID);
                                        IQMedia.WebApplication.Utility.CommonFunctions.FillCustomerRoles(customerValue, customerRoles);

                                        SessionInformation sessionInformation = new SessionInformation();
                                        sessionInformation.ClientGUID = customerValue.ClientGUID;
                                        sessionInformation.ClientID = customerValue.ClientID;
                                        sessionInformation.ClientName = customerValue.ClientName;
                                        sessionInformation.ClientPlayerLogoImage = customerValue.ClientPlayerLogoImage;
                                        sessionInformation.CustomerKey = customerValue.CustomerKey;
                                        sessionInformation.Email = customerValue.Email;
                                        sessionInformation.LoginID = customerValue.LoginID.Trim();
                                        sessionInformation.FirstName = customerValue.FirstName;
                                        sessionInformation.LastName = customerValue.LastName;
                                        sessionInformation.IsClientPlayerLogoActive = customerValue.IsClientPlayerLogoActive;
                                        sessionInformation.IsLogIn = true;
                                        sessionInformation.IsUgcAutoClip = customerValue.IsUGCAutoClip;
                                        sessionInformation.MultiLogin = customerValue.MultiLogin;
                                        sessionInformation.CustomerGUID = customerValue.CustomerGUID;
                                        sessionInformation.AuthorizedVersion = customerValue.AuthorizedVersion;
                                        sessionInformation.Isv4Group = customerValue.Isv4Group;
                                        sessionInformation.DefaultPage = customerValue.DefaultPage;
                                        sessionInformation.TimeZone = customerValue.TimeZone;
                                        sessionInformation.dst = customerValue.dst;
                                        sessionInformation.gmt = customerValue.gmt;
                                        sessionInformation.MCID = customerValue.MCID;
                                        sessionInformation.MasterCustomerID = customerValue.MasterCustomerID == null ? customerValue.CustomerKey : customerValue.MasterCustomerID.Value;

                                        // Customer Roles Information

                                        sessionInformation.Isv4FeedsAccess = customerValue.Isv4FeedsAccess;
                                        sessionInformation.Isv4DiscoveryAccess = customerValue.Isv4DiscoveryAccess;
                                        sessionInformation.Isv4LibraryAccess = customerValue.Isv4LibraryAccess;
                                        sessionInformation.Isv4TimeshiftAccess = customerValue.Isv4TimeshiftAccess;
                                        sessionInformation.Isv4TAdsAccess = customerValue.Isv4TAdsAccess;
                                        sessionInformation.Isv4DashboardAccess = customerValue.Isv4DashboardAccess;
                                        sessionInformation.Isv4LibraryDashboardAccess = customerValue.Isv4LibraryDashboardAccess;
                                        sessionInformation.Isv4TimeshiftRadioAccess = customerValue.Isv4TimeshiftRadioAccess;
                                        sessionInformation.Isv4SetupAccess = customerValue.Isv4SetupAccess;
                                        sessionInformation.isv4OptiqAccess = customerValue.isv4OptiqAccess;
                                        sessionInformation.IsGlobalAdminAccess = customerValue.IsGlobalAdminAccess;
                                        sessionInformation.Isv4UGCAccess = customerValue.Isv4UGCAccess;
                                        sessionInformation.Isv4UGCDownload = customerValue.IsUGCDownload;
                                        sessionInformation.Isv4UGCUploadEdit = customerValue.IsUGCUploadEdit;
                                        sessionInformation.Isv4IQAgentAccess = customerValue.Isv4IQAgentAccess;
                                        sessionInformation.Isv4TV = customerValue.Isv4TV;
                                        sessionInformation.Isv4NM = customerValue.Isv4NM;
                                        sessionInformation.Isv4SM = customerValue.Isv4SM;
                                        sessionInformation.Isv4TW = customerValue.Isv4TW;
                                        sessionInformation.Isv4TM = customerValue.Isv4TM;
                                        sessionInformation.Isv4CustomImage = customerValue.Isv4CustomImage;
                                        sessionInformation.IsCompeteData = customerValue.IsCompeteData;
                                        sessionInformation.IsNielsenData = customerValue.IsNielsenData;
                                        sessionInformation.Isv4BLPM = customerValue.Isv4BLPM;
                                        sessionInformation.IsNewsRights = customerValue.IsNewsRights;
                                        sessionInformation.Isv4CustomSettings = customerValue.Isv4CustomSettings;
                                        sessionInformation.Isv4DiscoveryLiteAccess = customerValue.Isv4DiscoveryLiteAccess;
                                        sessionInformation.IsfliQAdmin = customerValue.IsfliQAdmin;
                                        sessionInformation.Isv4PQ = customerValue.Isv4PQ;
                                        sessionInformation.IsMediaRoomContributor = customerValue.IsMediaRoomContributor;
                                        sessionInformation.IsMediaRoomEditor = customerValue.IsMediaRoomEditor;
                                        sessionInformation.Isv4Google = customerValue.Isv4Google;
                                        sessionInformation.IsTimeshiftFacet = customerValue.IsTimeshiftFacet;
                                        sessionInformation.IsShareTV = customerValue.IsShareTV;

                                        Session["SessionInformation"] = sessionInformation;


                                        IQMedia.WebApplication.Utility.CommonFunctions.AddUserIntoCache(customerValue.LoginID.Trim(), customerValue.MultiLogin.Value);

                                        if (ConfigurationManager.AppSettings["UseInfoAlert"] == "true")
                                        {
                                            return RedirectToAction("Index", "InfoAlert");
                                        }
                                        else
                                        {
                                            return RedirectToDefaultPage(sessionInformation.DefaultPage);// RedirectToAction("Index", "Dashboard");
                                        }
                                    }
                                    /*}
                                    else
                                    {
                                        ViewBag.Message = ConfigSettings.Settings.AlreadyLoggedIn;// "You are already Logged In";
                                        return View();
                                    }*/

                                }
                                else
                                {
                                    Logger.Info("User do not have access to website ,  user email :" + customer.LoginID.Trim());
                                    return RedirectToAction("Unauthorized", "Error");
                                }
                            }
                        }
                        else
                        {
                            Logger.Info("username and password  do not match for :" + customer.LoginID.Trim());
                        }
                        
                        ViewBag.Message = ConfigSettings.Settings.CredentialNotCorrect; //"Oops! the email and/or password is not correct.";
                        return View();
                    }
                    catch (AuthenticationException ex)
                    {
                        Logger.Error(ex);
                        UtilityLogic.WriteException(ex);
                        ViewBag.IsSuccess = false;
                        ViewBag.Message = ex.Message;
                        ViewBag.Status = -1;
                        return View();
                    }
                    catch (Exception ex)
                    {
                        Logger.Fatal("An error occured while login", ex);
                        UtilityLogic.WriteException(ex);
                        ViewBag.IsSuccess = false;
                        ViewBag.Message = ConfigSettings.Settings.ErrorOccurred;
                        ViewBag.Status = -3;
                        return View();
                    }
                    //return RedirectToAction("Index", "Home");


                }
                else
                {
                    return View();
                }
            }

        }

        [HttpPost]
        public JsonResult SignInModel(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                    CustomerModel customerValue = customerLogic.CheckAuthentication(customer.LoginID.Trim(), customer.Password.Trim());
                    if (customerValue != null && !string.IsNullOrWhiteSpace(customerValue.FirstName))
                    {
                        if (customerValue.AuthorizedVersion.HasValue && (customerValue.AuthorizedVersion == 0 || customerValue.AuthorizedVersion == 4))
                        {
                            int _Service = 0;
                            bool _IsLogin = false;

                            if (ConfigurationManager.AppSettings["ServicesBaseURL"] != null && int.TryParse(ConfigurationManager.AppSettings["ServicesBaseURL"], out _Service))
                            {
                                //if (_Service == 1 || _Service == 2)
                                //{
                                var result = IQMedia.Web.Common.Authentication.Login(customer.LoginID.Trim(), customer.Password.Trim());

                                if (result == IQMedia.Web.Common.Authentication.LoginStatus.Success)
                                {
                                    _IsLogin = true;
                                    // HttpCookie cookie = new HttpCookie(".IQAUTH");
                                    //cookie.Domain = ".iqmediacorp.com";

                                    //this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);


                                }
                                else
                                {
                                    Logger.Info("login failed for user :" + customer.LoginID.Trim());
                                }
                                //}
                            }

                            if (_IsLogin)
                            {
                                SessionInformation sessionInformation = new SessionInformation();
                                sessionInformation.ClientGUID = customerValue.ClientGUID;
                                sessionInformation.ClientID = customerValue.ClientID;
                                sessionInformation.ClientName = customerValue.ClientName;
                                sessionInformation.ClientPlayerLogoImage = customerValue.ClientPlayerLogoImage;
                                sessionInformation.CustomerKey = customerValue.CustomerKey;
                                sessionInformation.Email = customerValue.Email;
                                sessionInformation.LoginID = customerValue.LoginID.Trim();
                                sessionInformation.FirstName = customerValue.FirstName;
                                sessionInformation.LastName = customerValue.LastName;
                                sessionInformation.IsClientPlayerLogoActive = customerValue.IsClientPlayerLogoActive;
                                sessionInformation.IsLogIn = true;
                                sessionInformation.IsUgcAutoClip = customerValue.IsUGCAutoClip;
                                sessionInformation.Isv4UGCDownload = customerValue.IsUGCDownload;
                                sessionInformation.Isv4UGCUploadEdit = customerValue.IsUGCUploadEdit;
                                sessionInformation.MultiLogin = customerValue.MultiLogin;
                                sessionInformation.CustomerGUID = customerValue.CustomerGUID;
                                sessionInformation.AuthorizedVersion = customerValue.AuthorizedVersion;
                                sessionInformation.MCID = customerValue.MCID;
                                sessionInformation.MasterCustomerID = customerValue.MasterCustomerID == null ? customerValue.CustomerKey : customerValue.MasterCustomerID.Value;
                                sessionInformation.IsfliQAdmin = customerValue.IsfliQAdmin;

                                Session["SessionInformation"] = sessionInformation;

                                /*bool isAuthorizedVersion = false;
                                if (sessionInformation.AuthorizedVersion.HasValue && (sessionInformation.AuthorizedVersion == 0 || sessionInformation.AuthorizedVersion == 4))
                                {
                                    isAuthorizedVersion = true;
                                }*/
                                return Json(new
                                        {
                                            isSuccess = true,
                                            isAuthorizedVersionValid = true
                                        });
                            }
                        }
                        else
                        {
                            Logger.Info("User do not have access to website ,  user email :" + customer.LoginID.Trim());
                            return Json(new
                            {
                                isSuccess = true,
                                isAuthorizedVersionValid = false
                            });
                        }
                        //return RedirectToAction("Index", "Feeds");
                    }
                    else
                    {
                        Logger.Info("username and password  do not match for :" + customer.LoginID.Trim());
                    }
                    return Json(new
                    {
                        isSuccess = false
                    });
                }
                catch (Exception ex)
                {
                    Logger.Fatal("An error occured while login", ex);
                    return Json(new
                    {
                        isSuccess = false
                    });
                }

            }
            else
            {
                return Json(new
                {
                    isSuccess = false
                });
                //return RedirectToAction("Index", "Home");
            }

        }        

        #region Custom Methods

        public void SetSessionInformation(CustomerModel customerValue)
        {

            SessionInformation sessionInformation = new SessionInformation();
            sessionInformation.ClientGUID = customerValue.ClientGUID;
            sessionInformation.ClientID = customerValue.ClientID;
            sessionInformation.ClientName = customerValue.ClientName;
            sessionInformation.ClientPlayerLogoImage = customerValue.ClientPlayerLogoImage;
            sessionInformation.CustomerKey = customerValue.CustomerKey;
            sessionInformation.Email = customerValue.Email;
            sessionInformation.LoginID = customerValue.LoginID.Trim();
            sessionInformation.FirstName = customerValue.FirstName;
            sessionInformation.LastName = customerValue.LastName;
            sessionInformation.IsClientPlayerLogoActive = customerValue.IsClientPlayerLogoActive;
            sessionInformation.IsLogIn = true;
            sessionInformation.IsUgcAutoClip = customerValue.IsUGCAutoClip;
            sessionInformation.Isv4UGCDownload = customerValue.IsUGCDownload;
            sessionInformation.Isv4UGCUploadEdit = customerValue.IsUGCUploadEdit;
            sessionInformation.MultiLogin = customerValue.MultiLogin;
            sessionInformation.CustomerGUID = customerValue.CustomerGUID;
            sessionInformation.AuthorizedVersion = customerValue.AuthorizedVersion;
            sessionInformation.IsfliQAdmin = customerValue.IsfliQAdmin;

            Session["SessionInformation"] = sessionInformation;
        }        

        public ActionResult RedirectToDefaultPage(string p_DefaultPage)
        {
            if (Enum.IsDefined(typeof(IQMedia.Shared.Utility.CommonFunctions.DefaultPage), p_DefaultPage))
            {
                SessionInformation _SessionInformation = IQMedia.WebApplication.Utility.CommonFunctions.GetSessionInformation();
                switch ((IQMedia.Shared.Utility.CommonFunctions.DefaultPage)Enum.Parse(typeof(IQMedia.Shared.Utility.CommonFunctions.DefaultPage), p_DefaultPage))
                {
                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Dashboard:
                        if (_SessionInformation.Isv4DashboardAccess)
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Feeds:
                        if (_SessionInformation.Isv4FeedsAccess)
                        {
                            return RedirectToAction("Index", "Feeds");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Discovery:
                        if (_SessionInformation.Isv4DiscoveryAccess)
                        {
                            return RedirectToAction("Index", "Discovery");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }
                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4DiscoveryLite:
                        if (_SessionInformation.Isv4DiscoveryLiteAccess)
                        {
                            return RedirectToAction("Index", "DiscoveryLite");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }
                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Timeshift:
                        if (_SessionInformation.Isv4TimeshiftAccess)
                        {
                            return RedirectToAction("Index", "Timeshift");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4TAds:
                        if (_SessionInformation.Isv4TAdsAccess)
                        {
                            return RedirectToAction("Index", "TAds");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Optiq:
                        if (_SessionInformation.isv4OptiqAccess)
                        {
                            return RedirectToAction("Index", "ImagiQ");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Library:
                        if (_SessionInformation.Isv4LibraryAccess)
                        {
                            return RedirectToAction("Index", "Library");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4Setup:
                        if (_SessionInformation.Isv4SetupAccess)
                        {
                            return RedirectToAction("Index", "Setup");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    case IQMedia.Shared.Utility.CommonFunctions.DefaultPage.v4GlobalAdmin:
                        if (_SessionInformation.ClientGUID.Equals(new Guid("7722a116-c3bc-40ae-8070-8c59ee9e3d2a")) && _SessionInformation.IsGlobalAdminAccess)
                        {
                            return RedirectToAction("Index", "GlobalAdmin");
                        }
                        else
                        {
                            return GetDefaultPageByAccessRights();
                        }

                    default:
                        return GetDefaultPageByAccessRights();

                }
            }
            else
            {
                return GetDefaultPageByAccessRights();
            }
        }

        public ActionResult GetDefaultPageByAccessRights()
        {
            SessionInformation _SessionInformation = IQMedia.WebApplication.Utility.CommonFunctions.GetSessionInformation();
            if (_SessionInformation.Isv4DashboardAccess)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else if (_SessionInformation.Isv4FeedsAccess)
            {
                return RedirectToAction("Index", "Feeds");
            }
            else if (_SessionInformation.Isv4DiscoveryAccess)
            {
                return RedirectToAction("Index", "Discovery");
            }
            else if (_SessionInformation.Isv4DiscoveryLiteAccess)
            {
                return RedirectToAction("Index", "DiscoveryLite");
            }
            else if (_SessionInformation.Isv4TimeshiftAccess)
            {
                return RedirectToAction("Index", "Timeshift");
            }
            else if (_SessionInformation.Isv4TAdsAccess)
            {
                return RedirectToAction("Index", "TAds");
            }
            else if (_SessionInformation.isv4OptiqAccess)
            {
                return RedirectToAction("Index", "ImagiQ");
            }
            else if (_SessionInformation.Isv4LibraryAccess)
            {
                return RedirectToAction("Index", "Library");
            }
            else if (_SessionInformation.Isv4SetupAccess)
            {
                return RedirectToAction("Index", "Setup");
            }
            else if (_SessionInformation.ClientGUID.Equals(new Guid("7722a116-c3bc-40ae-8070-8c59ee9e3d2a")) && _SessionInformation.IsGlobalAdminAccess)
            {
                return RedirectToAction("Index", "GlobalAdmin");
            }
            else if (_SessionInformation.MCID.HasValue && _SessionInformation.Isv4Group)
            {
                return RedirectToAction("Index", "Group");
            }
            else
            {
                return RedirectToAction("Unauthorized", "Error");
            }


        }

        #endregion

    }
}
