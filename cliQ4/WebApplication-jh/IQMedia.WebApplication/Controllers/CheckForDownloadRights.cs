﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Model;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;

namespace IQMedia.WebApplication.Controllers
{
    public class CheckForDownloadRights : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                SessionInformation sessionInformation = IQMedia.WebApplication.Utility.CommonFunctions.GetSessionInformation();

                var result = new JsonResult();
                if (!GetDownloadRoleByCustomerGuid())
                {
                    filterContext.Result = new RedirectResult("~/Error/Unauthorized");
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean GetDownloadRoleByCustomerGuid()
        {
            try
            {
                SessionInformation sessionInformation = IQMedia.WebApplication.Utility.CommonFunctions.GetSessionInformation();
                CustomerLogic customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);

                return customerLogic.GetDownloadRoleByCustomerGuid(sessionInformation.CustomerGUID);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}