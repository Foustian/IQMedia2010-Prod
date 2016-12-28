using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Model;
using IQMedia.Web.Common;
using IQMedia.Web.Logic.Base;
using IQMedia.WebApplication.Utility;

namespace IQMedia.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home_New/

        public ActionResult Index()
        {
            try
            {
                IQMedia.Shared.Utility.Log4NetLogger.Debug("Start");

                IQMedia.WebApplication.Utility.CommonFunctions.CheckAuthentication();
            }
            catch (Exception exception)
            {
                ViewBag.IsSuccess = false;
                UtilityLogic.WriteException(exception);
            }

            return View();
        }

    }
}
