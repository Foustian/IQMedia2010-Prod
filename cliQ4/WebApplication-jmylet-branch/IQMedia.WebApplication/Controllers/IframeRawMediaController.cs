using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using System.Configuration;
using IQMedia.WebApplication.Utility;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class IframeRawMediaController : Controller
    {
        //
        // GET: /IframeRawMedia/

        public ActionResult Index()
        {
            try
            {
                int? offset = null;
                string searchTerm = string.Empty;
                Guid rawMediaID;
                string highlightContent = string.Empty;
                string captionContent = string.Empty;
                Dictionary<string, string> rawMediaModel = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(Request.QueryString["RawMediaID"]) && Guid.TryParse(Request.QueryString["RawMediaID"].Trim(), out rawMediaID))
                {
                    if (Request.QueryString["CC"] == null || Convert.ToBoolean(Request.QueryString["CC"]) != false)
                    {
                        if (Request.QueryString["Offset"] != null)
                        {
                            offset = Convert.ToInt32(Request.QueryString["Offset"]);
                        }

                        //if (!string.IsNullOrWhiteSpace(Request.QueryString["SearchTerm"]))
                        //{
                        searchTerm = !string.IsNullOrEmpty(Request.QueryString["SearchTerm"]) ? Request.QueryString["SearchTerm"].Trim() : string.Empty;

                        int? captionOffset;
                        List<int> SearchTermList = new List<int>(); // needed for TAds 
                        highlightContent = UtilityLogic.GetRawMediaCaption(searchTerm, rawMediaID, out captionOffset, out captionContent, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.TV.ToString(), null, null), out SearchTermList);

                        if (captionOffset != null && captionOffset.Value - Convert.ToInt32(ConfigurationManager.AppSettings["PlayerDefaultOffset"]) >= 0)
                        {
                            captionOffset = captionOffset.Value - Convert.ToInt32(ConfigurationManager.AppSettings["PlayerDefaultOffset"]);
                        }
                        else
                        {
                            captionOffset = 0;
                        }

                        if (offset == null)
                        {
                            offset = captionOffset;
                        }
                        //}
                    }

                    IQMedia.Model.ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                    string rawMediaObject = UtilityLogic.RenderRawMediaPlayer(string.Empty,
                                                   Convert.ToString(rawMediaID),
                                                   "true",
                                                   "false",
                                                    Convert.ToString(sessionInformation.ClientGUID),
                                                   "false",
                                                    Convert.ToString(sessionInformation.CustomerGUID),
                                                   ConfigurationManager.AppSettings["ServicesBaseURL"],
                                                   offset,
                                                   sessionInformation.IsClientPlayerLogoActive,
                                                   sessionInformation.ClientPlayerLogoImage, Request.Browser.Type);

                    rawMediaModel.Add("RawMediaPlayer", rawMediaObject);
                    rawMediaModel.Add("HighlightCaption", highlightContent);
                    rawMediaModel.Add("ClosedCaption", captionContent);
                }
                ViewBag.IsSuccess = true;
                return View(rawMediaModel);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                ViewBag.IsSuccess = false;

                return View();
            }            

        }

    }
}
