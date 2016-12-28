using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using IQMedia.Web.Logic;
using IQMedia.Model;
using IQMedia.WebApplication.Utility;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class InfoAlertController : Controller
    {
        //
        // GET: /InfoAlert/

        public ActionResult Index()
        {
            try
            {
                if (ConfigurationManager.AppSettings["UseInfoAlert"] == "true")
                {
                    ViewBag.IsSuccess = true;
                    if (ConfigurationManager.AppSettings["IsMaintenance"] == "true")
                    {
                        return RedirectToAction("Index", "UnderMaintenance");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return HttpNotFound();
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
        public ActionResult Continue()
        {
            SessionInformation sessionInformation = CommonFunctions.GetSessionInformation();
            return RedirectToDefaultPage(sessionInformation.DefaultPage);
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
            SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
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
    }
}
