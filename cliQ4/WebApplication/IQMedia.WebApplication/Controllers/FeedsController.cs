using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Model;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using System.Xml.Linq;
using System.Configuration;
using System.Text;
using System.Threading;
using IQMedia.WebApplication.Config;
using IQMedia.WebApplication.Models.TempData;
using System.IO;
using IQMedia.WebApplication.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CommonFunctions = IQMedia.WebApplication.Utility.CommonFunctions;
using Newtonsoft.Json.Linq;
using IQMedia.WebApplication.Utility;




namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class FeedsController : Controller
    {
        #region Public Property
        ActiveUser sessionInformation = null;
        FeedsTempData feedsTempData = null;
        #endregion

        /*#region Constructor

        public FeedsController()
        {
            feedsTempData = GetTempData();
        }
        #endregion*/

        string PATH_FeedResultsPartialView = "~/Views/Feeds/_Results.cshtml";
        string PATH_FeedChildResultsPartialView = "~/Views/Feeds/_ChildResults.cshtml";

        #region Action

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                // Clear out temp data used by other pages
                if (TempData.ContainsKey("AnalyticsTempData") && string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["isDD"]))
                {
                    TempData["AnalyticsTempData"] = null;
                }
                if (TempData.ContainsKey("DiscoveryTempData")) { TempData["DiscoveryTempData"] = null; }
                if (TempData.ContainsKey("TimeShiftTempData")) { TempData["TimeShiftTempData"] = null; }
                if (TempData.ContainsKey("TAdsTempData")) { TempData["TAdsTempData"] = null; }
                if (TempData.ContainsKey("SetupTempData")) { TempData["SetupTempData"] = null; }
                if (TempData.ContainsKey("GlobalAdminTempData")) { TempData["GlobalAdminTempData"] = null; } 

                sessionInformation = ActiveUserMgr.GetActiveUser();

                SetTempData(null);
                feedsTempData = new FeedsTempData();
                feedsTempData.FromRecordID = 0;
                feedsTempData.FromRecordIDDisplay = 0;
                feedsTempData.MaxFeedsReportLimit = null;
                feedsTempData.SinceID = null;
                feedsTempData.TotalResults = null;
                feedsTempData.TotalResultsDisplay = null;
                feedsTempData.ChildIDs = new Dictionary<string, List<string>>();
                SetTempData(feedsTempData);

                DateTime? fromDT = null;
                DateTime? toDT = null;
                string dma = null;
                string station = null;
                string competeurl = null;
                List<string> iqdmaids = new List<string>();
                string handle = null;
                string publication = null;
                string author = null;
                bool? isAudienceSort = null;
                bool? isHeard = null;
                bool? isSeen = null;
                bool? isPaid = null;
                bool? isEarned = null;
                bool? usePESHFilters = null;

                // Components added for Analytics drilldown
                string showTitle = null;
                int? dayOfWeek = null;
                int? timeOfDay = null;
                bool isHour = false;
                bool isMonth = false;

                IQMedia.Shared.Utility.Log4NetLogger.Debug("Query String Date : " + HttpContext.Request.QueryString["date"]);
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["date"]))// != null)
                {
                    IQMedia.Shared.Utility.Log4NetLogger.Debug("Inside IF condition of Date");
                    fromDT = Convert.ToDateTime(HttpContext.Request.QueryString["date"]);
                    toDT = Convert.ToDateTime(HttpContext.Request.QueryString["date"]);
                }
                else if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["fromDate"]) && !string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["toDate"]))
                {
                    fromDT = Convert.ToDateTime(HttpContext.Request.QueryString["fromDate"]);
                    toDT = Convert.ToDateTime(HttpContext.Request.QueryString["toDate"]);
                }
                else
                {
                    fromDT = DateTime.Now.Date.AddMonths(-1);
                    toDT = DateTime.Now.Date;
                }

                // If trying to get all records from a month
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["isMonth"]))
                {
                    isMonth = Convert.ToBoolean(HttpContext.Request.QueryString["isMonth"].ToString());

                    //Shared.Utility.Log4NetLogger.Debug("Feeds Controller isMonth != null");
                    DateTime startDate = Convert.ToDateTime(HttpContext.Request.QueryString["date"]);
                    if (startDate.Month.Equals(DateTime.Now.Month) && startDate.Year.Equals(DateTime.Now.Year))
                    {
                        // If trying to get current month, need to set end to be today -3 hours to allow for processing time
                        toDT = CommonFunctions.GetLocalTime(new DateTime(
                            startDate.Year,
                            startDate.Month,
                            DateTime.Now.Day,
                            DateTime.UtcNow.Hour,
                            0,
                            0
                        ).AddHours(-3)).Value;
                    }
                    else
                    {
                        // Set end date to be the last day of the month
                        toDT = new DateTime(
                            startDate.Year,
                            startDate.Month,
                            DateTime.DaysInMonth(startDate.Year, startDate.Month),
                            23,
                            59,
                            59
                        );
                    }
                    //Shared.Utility.Log4NetLogger.Debug(string.Format("dates {0} to {1}", fromDT.Value, toDT.Value));
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["isHour"]))
                {
                    isHour = Convert.ToBoolean(HttpContext.Request.QueryString["isHour"].ToString());
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["showTitle"]))
                {
                    showTitle = HttpContext.Request.QueryString["showTitle"].ToString();
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["dayOfWeek"]))
                {
                    dayOfWeek = Convert.ToInt32(HttpContext.Request.QueryString["dayOfWeek"].ToString());
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["timeOfDay"]))
                {
                    timeOfDay = Convert.ToInt32(HttpContext.Request.QueryString["timeOfDay"].ToString());
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["dma"]))
                {
                    dma = HttpContext.Request.QueryString["dma"].ToString();
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["station"]))
                {
                    station = HttpContext.Request.QueryString["station"].ToString();
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["competeurl"]))
                {
                    competeurl = HttpContext.Request.QueryString["competeurl"].ToString();
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["iqdmaid"]))
                {
                    iqdmaids.Add(HttpContext.Request.QueryString["iqdmaid"].ToString());
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["handle"]))
                {
                    handle = HttpContext.Request.QueryString["handle"].ToString();
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["publication"]))
                {
                    publication = "\"" + HttpContext.Request.QueryString["publication"].ToString() + "\"";
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["author"]))
                {
                    author = "\"" + HttpContext.Request.QueryString["author"].ToString() + "\"";
                }

                IQMedia.Shared.Utility.Log4NetLogger.Debug("Query String Date : " + HttpContext.Request.QueryString["medium"]);
                List<string> medium = new List<string>();
                List<string> mediumDesc = new List<string>();
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["medium"]))
                {
                    IQMedia.Shared.Utility.Log4NetLogger.Debug("Inside IF condition of Medium");
                    medium = (List<string>)Newtonsoft.Json.JsonConvert.DeserializeObject(Convert.ToString(HttpContext.Request.QueryString["medium"]), medium.GetType());

                    // If displaying all Print Media, include BLPM and ProQuest if the user has access
                    if (medium.IndexOf("PA") >= 0)
                    {
                        medium.Remove("PA");
                        if (sessionInformation.Isv4BLPM)
                        {
                            medium.Add(Shared.Utility.CommonFunctions.CategoryType.PM.ToString());
                        }
                        if (sessionInformation.Isv4PQ)
                        {
                            medium.Add(Shared.Utility.CommonFunctions.CategoryType.PQ.ToString());
                        }
                    }

                    mediumDesc = medium.Select(s => Shared.Utility.CommonFunctions.GetEnumDescription((Enum)(Enum.Parse(typeof(Shared.Utility.CommonFunctions.CategoryType), s)))).ToList();
                }

                IQMedia.Shared.Utility.Log4NetLogger.Debug("Query String Search Request : " + HttpContext.Request.QueryString["searchrequest"]);
                List<string> searchrequest = new List<string>();
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["searchrequest"]))
                {
                    searchrequest = (List<string>)Newtonsoft.Json.JsonConvert.DeserializeObject(Convert.ToString(HttpContext.Request.QueryString["searchrequest"]), searchrequest.GetType());
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["medium"]) || !string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["date"]))
                {
                    // If drilling down from Dashboard, sort by Outlet Weight by default
                    isAudienceSort = true;
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["heard"]))
                {
                    isHeard = Convert.ToBoolean(HttpContext.Request.QueryString["heard"].ToString());
                    usePESHFilters = true;
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["seen"]))
                {
                    isSeen = Convert.ToBoolean(HttpContext.Request.QueryString["seen"].ToString());
                    usePESHFilters = true;
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["paid"]))
                {
                    isPaid = Convert.ToBoolean(HttpContext.Request.QueryString["paid"].ToString());
                    usePESHFilters = true;
                }

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["earned"]))
                {
                    isEarned = Convert.ToBoolean(HttpContext.Request.QueryString["earned"].ToString());
                    usePESHFilters = true;
                }

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                IQClient_CustomSettingsModel clientSettings = clientLogic.GetClientCustomSettings(sessionInformation.ClientGUID.ToString());
                feedsTempData.DefaultFeedsPageSize = clientSettings.DefaultFeedsPageSize;
                feedsTempData.UseProminence = clientSettings.UseProminence;
                feedsTempData.UseProminenceMediaValue = clientSettings.UseProminenceMediaValue;

                IQClient_CustomSettingsModel clientSettingsExport = clientLogic.GetClientFeedsExportSettings(sessionInformation.ClientGUID);
                feedsTempData.MaxFeedsExportCSVLimit = clientSettingsExport.v4MaxFeedsExportItems;
                feedsTempData.RawMediaExpiration = clientSettingsExport.IQRawMediaExpiration;

                IQClient_CustomImageLogic iQClient_CustomImageLogic = (IQClient_CustomImageLogic)LogicFactory.GetLogic(LogicType.IQClient_CustomImage);
                List<IQClient_CustomImageModel> lstIQClient_CustomImageModel = iQClient_CustomImageLogic.GetAllIQClient_CustomImageByClientGuid(sessionInformation.ClientGUID);

                bool? defaultToUnread = clientSettings.DefaultFeedsShowUnread.HasValue && clientSettings.DefaultFeedsShowUnread.Value ? false : (bool?)null;
                FeedsSearchResponse fsr = SearchMediaResults(medium, fromDT, toDT, searchrequest, null, null, dma, station, competeurl, iqdmaids, handle, publication, author, null, false, false, isAudienceSort, null, 1, true, defaultToUnread, isHeard, isSeen, isPaid, isEarned, usePESHFilters, showTitle, dayOfWeek, timeOfDay, isHour, isMonth);
        
                Dictionary<string, object> dictFinalResult = new Dictionary<string, object>();
                List<IQAgent_MediaResultsModel> listIQAgent_MediaResultsModel = new List<IQAgent_MediaResultsModel>();
                if (fsr.IsValid)
                {
                    listIQAgent_MediaResultsModel = fsr.MediaResults;

                    int childCount = 0;
                    if (fsr.ChildCounts != null)
                    {
                        // Get the number of children associated to only the displayed parent items
                        childCount = fsr.ChildCounts.Where(s =>
                                            listIQAgent_MediaResultsModel.Select(m => m.ID)
                                                                        .Contains(Convert.ToInt64(s.Key))
                                        ).Sum(s => Convert.ToInt32(s.Value));
                        feedsTempData.ChildCounts = fsr.ChildCounts;
                    }

                    feedsTempData.FromRecordID = listIQAgent_MediaResultsModel.Count();
                    feedsTempData.FromRecordIDDisplay = listIQAgent_MediaResultsModel.Count() + childCount;

                    listIQAgent_MediaResultsModel.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDateTime)); 
                    listIQAgent_MediaResultsModel = CommonFunctions.GetGMTandDSTTime(listIQAgent_MediaResultsModel, CommonFunctions.ResultType.Feeds);
                }

                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                List<IQAgent_SearchRequestModel> lstIQAgent_SearchRequestModel = iqAgentLogic.SelectNewsAndSocialMediSearchRequestByClientGuid(sessionInformation.ClientGUID.ToString());

                IQReport_FolderLogic iQReport_FolderLogic = (IQReport_FolderLogic)LogicFactory.GetLogic(LogicType.IQReport_Folder);
                List<IQReport_FolderModel> lstIQReport_FolderModel = iQReport_FolderLogic.GetReportFolderByClientGuid(sessionInformation.ClientGUID);
                orderedFolderList = new List<IQReport_FolderModel>();
                string id = null;
                try
                {
                    id = lstIQReport_FolderModel.Where(x => x.parent == null).Select(x => x.id).First();
                    GetChildrenOrderedFolders(lstIQReport_FolderModel, id);
                }
                catch (Exception ex)
                {
                    id = null;
                    orderedFolderList = new List<IQReport_FolderModel>();
                }

                var manualClipDuration = clientLogic.GetClientManualClipDurationSettings(sessionInformation.ClientGUID);
                Int16 rawMediaPauseSecs = clientLogic.GetClientRawMediaPauseSecs(sessionInformation.ClientGUID);               

                dictFinalResult.Add("IsValidResponse", fsr.IsValid);
                dictFinalResult.Add("ManualClipDuration", manualClipDuration);
                dictFinalResult.Add("RawMediaPauseSecs", rawMediaPauseSecs);
                dictFinalResult.Add("FeedFilter", fsr.Filter);
                dictFinalResult.Add("ReportImages", lstIQClient_CustomImageModel);
                dictFinalResult.Add("SearchRequests", lstIQAgent_SearchRequestModel);
                dictFinalResult.Add("FromDate", fromDT.Value.ToShortDateString());
                dictFinalResult.Add("ToDate", toDT.Value.ToShortDateString());
                dictFinalResult.Add("HTML", RenderPartialToString(PATH_FeedResultsPartialView, listIQAgent_MediaResultsModel));
                dictFinalResult.Add("Medium", medium);
                dictFinalResult.Add("MediumDescription", mediumDesc);
                dictFinalResult.Add("ReportFolders", orderedFolderList);
                dictFinalResult.Add("DefaultToUnread", clientSettings.DefaultFeedsShowUnread.HasValue && clientSettings.DefaultFeedsShowUnread.Value ? 1 : 0);

                ViewBag.IsSuccess = true;
                SetTempData(feedsTempData);
                return View("Index", dictFinalResult);
            }
            catch (Exception exception)
            {
                ViewBag.IsSuccess = false;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
            return View();
        }

        List<IQReport_FolderModel> orderedFolderList = new List<IQReport_FolderModel>();
        public void GetChildrenOrderedFolders(List<IQReport_FolderModel> list, string id)
        {                                     
            foreach (var item in list.Where(x => x.id == id))
            {
                orderedFolderList.Add(item);
                foreach (var subItem in list.Where(x => x.parent == item.id))
                {
                    GetChildrenOrderedFolders(list, subItem.id);
                }
            }

        }


        [HttpPost]
        public ContentResult GetFilterData(DateTime? fromDate, DateTime? toDate, List<string> searchRequestID, List<string> mediumTypes, string keyword, short? sentiment, string Dma, string Station, string CompeteUrl,
                                                List<string> _DmaIDs, string Handle, string publication, string author, short? prominenceValue, bool isProminenceAudience, bool? isRead, bool? isHeardFilter, bool? isSeenFilter, 
                                                bool? isPaidFilter, bool? isEarnedFilter, bool? usePESHFilters, string showTitle, int? dayOfWeek, int? timeOfDay)
       {
            try
            {
                // Load it here for use in the task
                sessionInformation = ActiveUserMgr.GetActiveUser();

                DateTime? fromDateLocal = fromDate;
                DateTime? toDateLocal = null;
                if (fromDate.HasValue && toDate.HasValue)
                {
                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                    toDateLocal = toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }
                else
                {
                    fromDateLocal = DateTime.Now.Date.AddMonths(-3);
                    toDateLocal = toDate = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDateLocal);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }

                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                string currentUrl = Request.ServerVariables["HTTP_HOST"];
                FeedsSearchResponse fsr = new FeedsSearchResponse() { IsValid = false };

                lstTask.Add(Task<FeedsSearchResponse>.Factory.StartNew((object obj) =>
                    GetFilterData_Task(mediumTypes,
                                    fromDate,
                                    fromDateLocal,
                                    toDate,
                                    toDateLocal,
                                    searchRequestID,
                                    keyword,
                                    sentiment,
                                    Dma,
                                    Station,
                                    CompeteUrl,
                                    _DmaIDs,
                                    Handle,
                                    publication,
                                    author,
                                    prominenceValue,
                                    isProminenceAudience,
                                    isRead,
                                    isHeardFilter,
                                    isSeenFilter,
                                    isPaidFilter,
                                    isEarnedFilter,
                                    usePESHFilters,
                                    showTitle,
                                    dayOfWeek,
                                    timeOfDay,
                                    token,
                                    fsr),
                    fsr));

                try
                {
                    Task.WaitAll(lstTask.ToArray(), Convert.ToInt32(ConfigurationManager.AppSettings["MaxFeedsRequestDuration"]), token);
                    tokenSource.Cancel();
                }
                catch (AggregateException ex)
                {
                    IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                }
                catch (Exception ex)
                {
                    IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                }

                foreach (var tsk in lstTask)
                {
                    fsr = ((Task<FeedsSearchResponse>)tsk).Result;
                }

                FeedsResult json;
                if (fsr.IsValid)
                {
                    json = new FeedsResult()
                    {
                        filter = fsr.Filter,
                        isSuccess = true
                    };
                }
                else
                {
                    json = new FeedsResult()
                    {
                        isSuccess = false,
                        errorMessage = ConfigSettings.Settings.ErrorOccurred
                    };
                }

                return Content(Shared.Utility.CommonFunctions.SearializeJson(json), "application/json");
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                FeedsResult json = new FeedsResult()
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };

                return Content(Shared.Utility.CommonFunctions.SearializeJson(json), "application/json");
            }
        }

        [HttpPost]
        public ContentResult _MediaJsonResults(DateTime? fromDate, DateTime? toDate, List<string> searchRequestID, List<string> mediumTypes, string keyword, short? sentiment, bool isAsc, int? pageSize, string Dma, string Station, string CompeteUrl,
                                                List<string> _DmaIDs, string Handle, string publication, string author, bool? isRead, short? prominenceValue, bool isProminenceAudience, bool? isAudienceSort, int numPages, bool isHeardFilter, bool? isSeenFilter, bool? isPaidFilter, bool? isEarnedFilter, bool? usePESHFilters, string showTitle, int? dayOfWeek, int? timeOfDay, bool isHour, bool isMonth)
        {
            try
            {
                keyword = !string.IsNullOrWhiteSpace(keyword) ? keyword : null;

                feedsTempData = GetTempData();
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                string mediumHTML = string.Empty;
                feedsTempData.FromRecordID = 0;
                feedsTempData.FromRecordIDDisplay = 0;
                feedsTempData.ChildIDs = new Dictionary<string, List<string>>();

                FeedsSearchResponse fsr = SearchMediaResults(mediumTypes, fromDate, toDate, searchRequestID, keyword, sentiment, Dma, Station, CompeteUrl, _DmaIDs, Handle, publication, author, prominenceValue, isProminenceAudience, isAsc, isAudienceSort, pageSize, numPages, true, isRead, isHeardFilter, isSeenFilter, isPaidFilter, isEarnedFilter, usePESHFilters, showTitle, dayOfWeek, timeOfDay, isHour, isMonth);
                List<IQAgent_MediaResultsModel> listIQAgent_MediaResultsModel = new List<IQAgent_MediaResultsModel>();
                FeedsFilterModel filter = new FeedsFilterModel();
                bool hasMoreResults = false;
                if (fsr.IsValid)
                {
                    listIQAgent_MediaResultsModel = fsr.MediaResults;
                    filter = fsr.Filter;

                    int childCount = 0;
                    if (fsr.ChildCounts != null)
                    {
                        // Get the number of children associated to only the displayed parent items
                        childCount = fsr.ChildCounts.Where(s =>
                                            listIQAgent_MediaResultsModel.Select(m => m.ID)
                                                                        .Contains(Convert.ToInt64(s.Key))
                                        ).Sum(s => Convert.ToInt32(s.Value));
                        feedsTempData.ChildCounts = fsr.ChildCounts;
                    }

                    feedsTempData.FromRecordID = listIQAgent_MediaResultsModel.Count();
                    feedsTempData.FromRecordIDDisplay = listIQAgent_MediaResultsModel.Count() + childCount;

                    SetTempData(feedsTempData);

                    hasMoreResults = CheckForMoreResultAvailble();

                    listIQAgent_MediaResultsModel.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDateTime)); 
                    listIQAgent_MediaResultsModel = CommonFunctions.GetGMTandDSTTime(listIQAgent_MediaResultsModel, CommonFunctions.ResultType.Feeds);
                }

                FeedsResult json = new FeedsResult()
                {
                    html = RenderPartialToString(PATH_FeedResultsPartialView, listIQAgent_MediaResultsModel),
                    hasMoreResults = hasMoreResults,
                    filter = filter,
                    totalRecords = string.Format("{0:n0}", feedsTempData.TotalResults),
                    totalRecordsDisplay = string.Format("{0:n0}", feedsTempData.TotalResultsDisplay),
                    currentRecords = string.Format("{0:n0}", feedsTempData.FromRecordID),
                    currentRecordsDisplay = string.Format("{0:n0}", feedsTempData.FromRecordIDDisplay),
                    isSuccess = true,
                    isValidResponse = fsr.IsValid,
                    isReadLimitExceeded = fsr.IsReadLimitExceeded
                };

                return Content(Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                FeedsResult json = new FeedsResult()
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };

                return Content(Shared.Utility.CommonFunctions.SearializeJson(json), "application/json");
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        [HttpPost]
        public ContentResult _DeleteMediaResults(List<string> selectedRecords, bool isAsc, bool? isAudienceSort, bool isSelectAll)
        {
            try
            {
                bool hasDuplicates = selectedRecords.Distinct().Count() != selectedRecords.Count();
                bool insertError = hasDuplicates;
                Shared.Utility.Log4NetLogger.Debug("Feeds DeleteMediaResults - Start - Has Duplicates: " + hasDuplicates);

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                feedsTempData = GetTempData();
                Dictionary<string, object> dictResults = new Dictionary<string, object>();
                int recordCount;

                List<IQAgent_MediaResultsModel> mediaRecords = selectedRecords.Select(s => new IQAgent_MediaResultsModel()
                                                                                            {
                                                                                                ID = Convert.ToInt64(s.Substring(0, s.IndexOf(":"))),
                                                                                                MediaType = s.Substring(s.IndexOf(":") + 1)
                                                                                            }).ToList();
                List<string> mediaIDs = mediaRecords.Select(s => s.ID.ToString()).ToList();
                recordCount = mediaIDs.Count;

                hasDuplicates = mediaIDs.Distinct().Count() != mediaIDs.Count();
                insertError = insertError || hasDuplicates;
                Shared.Utility.Log4NetLogger.Debug("Feeds DeleteMediaResults - Conversion to MediaIDs - Has Duplicates: " + hasDuplicates);

                // If a user deletes a parent, need to make sure that the children are queued for deletion as well.

                // If the parent has already been expanded on the page, get the list of child IDs from temp data.
                // This has to come before SearchAllChildResults, otherwise we won't know which parents have already been expanded.
                List<string> expandedParents = mediaIDs.Where(s => feedsTempData.ChildIDs.ContainsKey(s)).ToList();
                foreach (string parentID in expandedParents)
                {
                    // Don't duplicate any children that were manually selected.
                    mediaIDs.AddRange(feedsTempData.ChildIDs[parentID].Where(s => !mediaIDs.Contains(s)).ToList());

                    hasDuplicates = mediaIDs.Distinct().Count() != mediaIDs.Count();
                    insertError = insertError || hasDuplicates;
                    Shared.Utility.Log4NetLogger.Debug("Feeds DeleteMediaResults - After Checking Parent " + parentID + " - Has Duplicates: " + hasDuplicates);
                }

                // If the parent hasn't been expanded then query solr to get the child IDs 
                dictResults = SearchAllChildResults(mediaRecords, mediaIDs, isAsc, isAudienceSort, null, true, true);

                if (dictResults.ContainsKey("MediaIDs"))
                {
                    mediaIDs = (List<string>)dictResults["MediaIDs"];

                    hasDuplicates = mediaIDs.Distinct().Count() != mediaIDs.Count();
                    insertError = insertError || hasDuplicates;
                    Shared.Utility.Log4NetLogger.Debug("Feeds DeleteMediaResults - After Searching Children - Has Duplicates: " + hasDuplicates);
                }

                if (insertError)
                {
                    CommonFunctions.WriteException(new Exception("Duplicate IDs found when deleting Feeds results."));
                }

                // Use the count of actual selected records, unless Select All With Dupes was checked
                if (isSelectAll)
                {
                    recordCount = mediaIDs.Count;
                }

                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                int result = iQAgentLogic.QueueMediaResultsForDelete(sessionInformation.ClientGUID, sessionInformation.CustomerGUID, mediaIDs);
                
                var json = new
                {
                    isSuccess = result == 1,
                    recordCount = recordCount,
                    errorMsg = result == 1 ? null : "Some error occurred. The records were not queued for deletion."
                };

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                var json = new
                {
                    isSuccess = false,
                    errorMsg = Config.ConfigSettings.Settings.ErrorOccurred
                };

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        [HttpPost]
        public ContentResult _MoreMediaJsonResults(DateTime? fromDate, DateTime? toDate, List<string> searchRequestID, List<string> mediumTypes, string keyword, short? sentiment, bool isAsc, int? pageSize, string Dma, string Station, string CompeteUrl, List<string> _DmaIDs, string Handle,
                                                    string publication, string author, bool? isRead, short? prominenceValue, bool isProminenceAudience, bool? isAudienceSort, bool? isHeardFilter, bool? isSeenFilter, bool? isPaidFilter, bool? isEarnedFilter, bool? usePESHFilters, string showTitle, int? dayOfWeek, int? timeOfDay, bool isHour, bool isMonth)
        {
            try
            {
                keyword = !string.IsNullOrWhiteSpace(keyword) ? keyword : null;

                  FeedsSearchResponse fsr = SearchMediaResults(mediumTypes, fromDate, toDate, searchRequestID, keyword, sentiment, Dma, Station, CompeteUrl, _DmaIDs, Handle, publication, author, prominenceValue, isProminenceAudience, isAsc, isAudienceSort, pageSize, 1, false, isRead, isHeardFilter, isSeenFilter, isPaidFilter, isEarnedFilter, usePESHFilters, showTitle, dayOfWeek, timeOfDay, isHour, isMonth);
                List<IQAgent_MediaResultsModel> listIQAgent_MediaResultsModel = new List<IQAgent_MediaResultsModel>();
                bool hasMoreResults = false;
                if (fsr.IsValid)
                {
                    listIQAgent_MediaResultsModel = fsr.MediaResults;

                    feedsTempData = GetTempData();

                    // Get the number of children associated to the newly obtained parent items
                    int childCount = feedsTempData.ChildCounts.Where(s =>
                                                                        listIQAgent_MediaResultsModel.Select(m => m.ID)
                                                                                                    .Contains(Convert.ToInt64(s.Key))
                                                                    ).Sum(s => Convert.ToInt32(s.Value));

                    feedsTempData.FromRecordID = feedsTempData.FromRecordID + listIQAgent_MediaResultsModel.Count();
                    feedsTempData.FromRecordIDDisplay = feedsTempData.FromRecordIDDisplay + listIQAgent_MediaResultsModel.Count() + childCount;
                    SetTempData(feedsTempData);

                    hasMoreResults = CheckForMoreResultAvailble();

                    listIQAgent_MediaResultsModel.ForEach(x => x.timeDifference = CommonFunctions.GetTimeDifference(x.MediaDateTime)); 
                    listIQAgent_MediaResultsModel = CommonFunctions.GetGMTandDSTTime(listIQAgent_MediaResultsModel, CommonFunctions.ResultType.Feeds);
                }

                FeedsResult json = new FeedsResult()
                {
                    html = RenderPartialToString(PATH_FeedResultsPartialView, listIQAgent_MediaResultsModel),
                    hasMoreResults = hasMoreResults,
                    totalRecords = string.Format("{0:n0}", feedsTempData.TotalResults),
                    totalRecordsDisplay = string.Format("{0:n0}", feedsTempData.TotalResultsDisplay),
                    currentRecords = string.Format("{0:n0}", feedsTempData.FromRecordID),
                    currentRecordsDisplay = string.Format("{0:n0}", feedsTempData.FromRecordIDDisplay),
                    isSuccess = true,
                    isValidResponse = fsr.IsValid,
                    isReadLimitExceeded = fsr.IsReadLimitExceeded
                };

                var res = Content(Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);

                return res;
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                FeedsResult json = new FeedsResult()
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };

                return Content(Convert.ToString(json), "application/json");
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        [HttpPost]
        public ContentResult _GetChildResults(long parentID, string mediaType, bool isAsc, bool? isAudienceSort, bool? isRead)
        {
            try
            {
                feedsTempData = GetTempData();

                FeedsChildSearchResponse fcsr = SearchChildResults(parentID, mediaType, isAsc, isAudienceSort, isRead);
                
                IQAgent_MediaResultsModel parent = new IQAgent_MediaResultsModel();
                if (fcsr.IsValid)
                {
                    parent = fcsr.MediaResult;
                    parent.timeDifference = CommonFunctions.GetTimeDifference(parent.MediaDateTime);
                    List<string> childIDs;

                    if (mediaType == "NM")
                    {
                        parent = ((List<IQAgent_MediaResultsModel>)CommonFunctions.GetGMTandDSTTime(new List<IQAgent_MediaResultsModel>() { parent }, CommonFunctions.ResultType.Feeds)).First();
                        IQAgent_NewsResultsModel iQAgent_NewsResultsModel = (IQAgent_NewsResultsModel)parent.MediaData;
                        iQAgent_NewsResultsModel.ChildResults = CommonFunctions.GetGMTandDSTTime(iQAgent_NewsResultsModel.ChildResults, CommonFunctions.ResultType.Feeds);
                        childIDs = iQAgent_NewsResultsModel.ChildResults.Select(s => s.ID.ToString()).ToList();
                    }
                    else
                    {
                        IQAgent_TVResultsModel iQAgent_TVResultsModel = (IQAgent_TVResultsModel)parent.MediaData;
                        childIDs = iQAgent_TVResultsModel.ChildResults.Select(s => s.ID.ToString()).ToList();

                        // Account for TV records where the current parent is actually a child
                        if (parent.ID != fcsr.OrigParentID)
                        {
                            childIDs.Remove(fcsr.OrigParentID.ToString());
                            childIDs.Add(parent.ID.ToString());
                        }
                    }

                    // Track which parent records have been expanded for when an action is performed
                    feedsTempData.ChildIDs.Add(fcsr.OrigParentID.ToString(), childIDs);
                    SetTempData(feedsTempData);
                }

                FeedsResult json = new FeedsResult()
                {
                    html = RenderPartialToString(PATH_FeedChildResultsPartialView, parent),
                    isSuccess = true,
                    isValidResponse = fcsr.IsValid
                };

                var res = Content(Shared.Utility.CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);
                return res;
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                FeedsResult json = new FeedsResult()
                {
                    isSuccess = false
                };

                return Content(Convert.ToString(json), "application/json");
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        [HttpPost]
        public JsonResult LoadPlayer(Int64 iqagentTVResultID)
        {
            try
            {

                int? offset = 0;
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                if (sessionInformation != null && !string.IsNullOrWhiteSpace(sessionInformation.FirstName))
                {

                    IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                    Guid rlVideoGUID = new Guid();
                    string searchTerm = string.Empty;
                    string iqCCKey = string.Empty;

                    iQAgentLogic.GetSearchTermByIQAgentTVResultID(sessionInformation.ClientGUID, iqagentTVResultID, out rlVideoGUID, out searchTerm, out iqCCKey);

                    string captionString = string.Empty;
                    List<int> SearchTermList = new List<int>(); // needed for TAds 
                    string highlightString = UtilityLogic.GetRawMediaCaption(searchTerm, rlVideoGUID, out  offset, out captionString, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.TV.ToString(), null, null), out SearchTermList);
                    bool forceCategorySelection = UtilityLogic.GetForceCategorySelection(sessionInformation.ClientGUID);

                    if (offset != null)
                    {
                        if (offset.Value - Convert.ToInt32(ConfigurationManager.AppSettings["PlayerDefaultOffset"]) >= 0)
                        {
                            offset = offset.Value - Convert.ToInt32(ConfigurationManager.AppSettings["PlayerDefaultOffset"]);
                        }
                        else
                        {
                            offset = 0;
                        }
                    }
                    string rawMediaObject = string.Empty;

                    if ((Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("ipod")) || (Request.UserAgent.ToLower().Contains("android") && Utility.CommonFunctions.CheckVersion()))
                    {

                    }
                    else
                    {
                        rawMediaObject = UtilityLogic.RenderBasicRawMediaPlayer(string.Empty,
                                                            Convert.ToString(rlVideoGUID),
                                                            "true",
                                                            "false",
                                                            Convert.ToString(sessionInformation.ClientGUID),
                                                            "false",
                                                            Convert.ToString(sessionInformation.CustomerGUID),
                                                            ConfigurationManager.AppSettings["ServicesBaseURL"],
                                                            offset,
                                                            sessionInformation.IsClientPlayerLogoActive,
                                                            sessionInformation.ClientPlayerLogoImage, Request.Browser.Type,p_AutoResize:true);
                    }

                    bool hasCaption = false;
                    if (!string.IsNullOrWhiteSpace(captionString) || !string.IsNullOrWhiteSpace(highlightString))
                    {
                        hasCaption = true;
                    }

                    return Json(new
                    {
                        rawMediaObjectHTML = rawMediaObject,
                        HighlightHTML = highlightString,
                        CaptionHTML = captionString,
                        hasCaptionString = hasCaption,
                        videoGuid = rlVideoGUID,
                        iqCCKey = iqCCKey,
                        offset = offset,
                        forceCategorySelection = forceCategorySelection,
                        SearchTermHits = SearchTermList,
                        isSuccess = true

                    });
                }
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }

            return Json(new object());
        }

        [HttpPost]
        public JsonResult ExportCSV(List<string> p_RecordList, bool p_SelectAll, bool p_SelectAllParent, DateTime? fromDate, DateTime? toDate, List<string> searchRequestID, List<string> mediumTypes, string keyword, short? sentiment, bool isAsc, string Dma, string Station, string CompeteUrl, List<string> _DmaIDs, string Handle,
                                        string publication, string author, bool? isRead, bool? isHeardFilter, bool? isSeenFilter, bool? isPaidFilter, bool? isEarnedFilter, bool? usePESHFilters, short? prominenceValue, bool isProminenceAudience, bool? isAudienceSort, string showTitle, int? dayOfWeek, int? timeOfDay)
        {
            try
            {
                Shared.Utility.Log4NetLogger.Info("Export CSV Started");

                List<string> mediaIDs = null;
                if (p_RecordList != null && p_RecordList.Count > 0)
                {
                    mediaIDs = p_RecordList.Select(s => s.Substring(0, s.IndexOf(":"))).ToList();
                }

                if (!String.IsNullOrEmpty(publication))
                {
                    publication = "\"" + publication + "\"";
                }
                if (!String.IsNullOrEmpty(author))
                {
                    author = "\"" + author + "\"";
                }

                  Shared.Utility.Log4NetLogger.Info("Export CSV Input Params are :");
                Shared.Utility.Log4NetLogger.Info("MediaID :" + (mediaIDs != null ? string.Join(",", mediaIDs) : string.Empty));
                Shared.Utility.Log4NetLogger.Info("p_SelectAll :" + p_SelectAll);
                Shared.Utility.Log4NetLogger.Info("p_SelectAllParent :" + p_SelectAllParent);
                Shared.Utility.Log4NetLogger.Info("fromDate :" + fromDate);
                Shared.Utility.Log4NetLogger.Info("toDate :" + toDate);
                Shared.Utility.Log4NetLogger.Info("searchRequestID :" + (searchRequestID != null ? string.Join(",", searchRequestID) : string.Empty));
                Shared.Utility.Log4NetLogger.Info("mediumTypes :" + (mediumTypes != null ? string.Join(",", mediumTypes) : string.Empty));
                Shared.Utility.Log4NetLogger.Info("keyword :" + keyword);
                Shared.Utility.Log4NetLogger.Info("sentiment :" + sentiment);
                Shared.Utility.Log4NetLogger.Info("Dma :" + Dma);
                Shared.Utility.Log4NetLogger.Info("Station :" + Station);
                Shared.Utility.Log4NetLogger.Info("CompeteUrl :" + CompeteUrl);
                Shared.Utility.Log4NetLogger.Info("Handle :" + Handle);
                Shared.Utility.Log4NetLogger.Info("_DmaIDs :" + _DmaIDs);
                Shared.Utility.Log4NetLogger.Info("Publication :" + publication);
                Shared.Utility.Log4NetLogger.Info("Author :" + author);
                Shared.Utility.Log4NetLogger.Info("prominenceValue :" + prominenceValue);
                Shared.Utility.Log4NetLogger.Info("isProminenceAudience :" + isProminenceAudience);
                Shared.Utility.Log4NetLogger.Info("isAudienceSort :" + isAudienceSort);
                Shared.Utility.Log4NetLogger.Info("showTitle : " + showTitle);
                Shared.Utility.Log4NetLogger.Info("dayOfWeek : " + dayOfWeek);
                Shared.Utility.Log4NetLogger.Info("timeOfDay : " + timeOfDay);

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                Dictionary<string, object> dicresult = new Dictionary<string, object>();
                List<IQAgent_MediaResultsModel> listIQAgent_MediaResultsModel = new List<IQAgent_MediaResultsModel>();
                feedsTempData = GetTempData();

                Shared.Utility.Log4NetLogger.Info("Feeds Report Limit from Tempdata : " + feedsTempData.MaxFeedsExportCSVLimit);

                if (!feedsTempData.MaxFeedsExportCSVLimit.HasValue)
                {
                    Shared.Utility.Log4NetLogger.Info("Feeds Report Limit is null so fetch from DB again");

                    ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                    IQClient_CustomSettingsModel clientSettings = clientLogic.GetClientFeedsExportSettings(sessionInformation.ClientGUID);
                    feedsTempData.MaxFeedsExportCSVLimit = clientSettings.v4MaxFeedsExportItems;

                    Shared.Utility.Log4NetLogger.Info("Feeds Report Limit fetched from DB is : " + feedsTempData.MaxFeedsExportCSVLimit);

                    SetTempData(feedsTempData);
                }

                Shared.Utility.Log4NetLogger.Info("fetch feeds data from DB");

                FeedsSearchResponse fsr = null;
                bool IsFileGenerated = false;
                if (!p_SelectAll && !p_SelectAllParent)
                {
                    List<string> lstMediaID = mediaIDs.Take(feedsTempData.MaxFeedsExportCSVLimit.Value).ToList();
                    fsr = SearchMediaResultsByID(lstMediaID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false, null, lstMediaID.Count, false, null, null, null, null, null, null, null, null, null);
                }
                else
                {
                    fsr = SearchMediaResultsByID(null, mediumTypes, fromDate, toDate, searchRequestID, keyword, sentiment, Dma, Station, CompeteUrl, _DmaIDs, Handle, publication, author, prominenceValue, isProminenceAudience, isAsc, isAudienceSort, feedsTempData.MaxFeedsExportCSVLimit.Value, p_SelectAllParent, isRead, 
                                                    isHeardFilter, isSeenFilter, isPaidFilter, isEarnedFilter, usePESHFilters, showTitle, dayOfWeek, timeOfDay);
                }

                if (fsr.IsValid)
                {
                    listIQAgent_MediaResultsModel = CommonFunctions.GetGMTandDSTTime(fsr.MediaResults, CommonFunctions.ResultType.Feeds);

                    Shared.Utility.Log4NetLogger.Info("generate CSV string for fetched Feeds data");

                    string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    string TempCSVPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Feeds\\CSV\\" + sessionInformation.CustomerGUID + "_" + DateTimeStamp + "_Feeds.csv";
                    string strCSVData = GetCSVData(listIQAgent_MediaResultsModel);


                    Session["DownloadFeedCSVFileName"] = Path.GetFileName("iQMedia_Feeds_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".csv");

                    using (FileStream fs = new FileStream(TempCSVPath, FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            w.Write(strCSVData);
                        }
                    }

                    if (System.IO.File.Exists(TempCSVPath))
                    {
                        IsFileGenerated = true;
                        Session["FeedsCSVFile"] = TempCSVPath;
                    }

                    Shared.Utility.Log4NetLogger.Info("is file created and Exist? :" + IsFileGenerated);
                    Shared.Utility.Log4NetLogger.Info("file download path  :" + TempCSVPath);

                    Shared.Utility.Log4NetLogger.Info("return isallowdownload = " + IsFileGenerated);
                }

                var json = new
                {
                    isSuccess = IsFileGenerated,
                    errorMessage = "Data not available. Please try again."
                };
                return Json(json);
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        [HttpGet]
        public ActionResult DownloadCSVFile()
        {
            if (Session["FeedsCSVFile"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["FeedsCSVFile"])) && Session["DownloadFeedCSVFileName"] != null)
            {
                string CSVFile = Convert.ToString(Session["FeedsCSVFile"]);
                string DownloadFileName = Convert.ToString(Session["DownloadFeedCSVFileName"]);

                if (System.IO.File.Exists(CSVFile))
                {
                    Session.Remove("FeedsCSVFile");
                    return File(CSVFile, "application/csv", DownloadFileName);
                }
            }
            return Content(ConfigSettings.Settings.FileNotAvailable);
        }

        public JsonResult ExcludeDomains(List<string> p_MediaID, List<string> p_SearchRequestIds)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                Int64 result = 0;

                result = iQAgentLogic.ExcludeDomainsBySearchRequest(sessionInformation.ClientGUID, p_MediaID, p_SearchRequestIds);
                if (result > 0)
                {
                    return Json(new
                    {
                        isSuccess = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false
                    });
                }
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        [HttpPost]
        public JsonResult GetSinceID()
        {
            try
            {
                feedsTempData = GetTempData();
                return Json(new
                {
                    isSuccess = true,
                    sinceID = feedsTempData.SinceID
                });
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                return Json(new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        [HttpPost]
        public JsonResult GetProQuestResultByID(string mediaID)
        {
            try
            {
                FeedsSearchResponse fsr = SearchMediaResultsByID(new List<string>() { mediaID }, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false, null, 1, false, null, null, null, null, null, null, null, null, null);

                if (fsr.IsValid && fsr.MediaResults.Count > 0)
                {
                    IQAgent_MediaResultsModel mediaResult = fsr.MediaResults[0];
                    IQAgent_PQResultsModel pqResult = (IQAgent_PQResultsModel)mediaResult.MediaData;
                    return Json(new
                    {
                        title = pqResult.Title,
                        publication = pqResult.Publication,
                        authors = pqResult.Authors != null && pqResult.Authors.Count > 0 ? String.Join(", ", pqResult.Authors) : String.Empty,
                        content = pqResult.ContentHTML,
                        mediaDate = mediaResult.MediaDateTime.ToShortDateString(),
                        copyright = pqResult.Copyright,
                        isSuccess = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        errorMessage = "Data not available. Please try again."
                    });
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                return Json(new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                });
            }
        }

        [HttpPost]
        public ContentResult UpdateIsRead(List<string> mediaIDs, bool isRead, bool isSelectAll)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                feedsTempData = GetTempData();
                Dictionary<string, object> dictResults;
                Dictionary<string, string> childHTML = null;
                int recordCount = mediaIDs.Count;

                List<IQAgent_MediaResultsModel> mediaRecords = mediaIDs.Select(s => new IQAgent_MediaResultsModel()
                {
                    ID = Convert.ToInt64(s)
                }).ToList();

                // In order to keep counts correct, update child records even if they aren't selected

                // If the parent has already been expanded on the page, get the list of child IDs from temp data.
                // This has to come before SearchAllChildResults, otherwise we won't know which parents have already been expanded.
                List<string> expandedParents = mediaIDs.Where(s => feedsTempData.ChildIDs.ContainsKey(s)).ToList();
                foreach (string parentID in expandedParents)
                {
                    // Don't duplicate any children that were manually selected.
                    mediaIDs.AddRange(feedsTempData.ChildIDs[parentID].Except(mediaIDs).ToList());
                }

                // If the parent hasn't been expanded then query solr to get the child IDs 
                dictResults = SearchAllChildResults(mediaRecords, mediaIDs, true, null, null, true);

                if (dictResults.ContainsKey("MediaIDs"))
                {
                    mediaIDs = (List<string>)dictResults["MediaIDs"];
                }
                if (dictResults.ContainsKey("ChildHTML"))
                {
                    childHTML = (Dictionary<string, string>)dictResults["ChildHTML"];
                }

                // Use the count of actual selected records, unless Select All With Dupes was checked
                if (isSelectAll)
                {
                    recordCount = mediaIDs.Count;
                }

                int retVal = IQCommon.CommonFunctions.UpdateIsRead(sessionInformation.ClientGUID, mediaIDs, isRead);

                var json = new
                {
                    isSuccess = retVal == 1,
                    recordCount = recordCount,
                    childHTML = childHTML,
                    errorMsg = retVal == 1 ? null : "Some error occurred. The records were not updated."
                };

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        #region INSERT ACTION

        [HttpPost]
        public JsonResult Insert_ArchiveData(ArchiveCommonModel archiveCommonModel)
        {
            IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

            try
            {
                if (ActiveUserMgr.CheckAuthentication())
                {
                    sessionInformation = ActiveUserMgr.GetActiveUser();
                    ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                    IQClient_CustomSettingsModel clientSettings = clientLogic.GetClientCustomSettings(sessionInformation.ClientGUID.ToString());
                    string result = string.Empty;

                    bool isMissingArticle = false;
                    IQAgent_MediaResultsModel objIQAgent_MediaResultsModel = iqAgentLogic.GetIQAgent_MediaResultByID(archiveCommonModel.MediaResultID, sessionInformation.ClientGUID, out isMissingArticle);
                    if (objIQAgent_MediaResultsModel != null && !string.IsNullOrEmpty(objIQAgent_MediaResultsModel.ArticleID))
                    {
                        string mediaType = objIQAgent_MediaResultsModel.MediaType;

                        if (mediaType == IQMedia.Shared.Utility.CommonFunctions.MediaType.NM.ToString())
                        {
                            string Event = "Insert Feeds";
                            NMLogic nmLogic = (NMLogic)LogicFactory.GetLogic(LogicType.NM);
                            if (isMissingArticle)
                            {
                                IQAgent_NewsResultsModel iQAgent_NewsResultsModel = objIQAgent_MediaResultsModel.MediaData as IQAgent_NewsResultsModel;
                                result = nmLogic.InsertArchiveNM(iQAgent_NewsResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, archiveCommonModel.CategoryGuid, Event, archiveCommonModel.Keywords, archiveCommonModel.Description, archiveCommonModel.MediaResultID, clientSettings.UseProminenceMediaValue == true);
                            }
                            else
                            {
                                IQAgent_NewsResultsModel iQAgent_NewsResultsModel = nmLogic.SearchNewsByArticleID(objIQAgent_MediaResultsModel.ArticleID, CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), null, null/*objIQAgent_MediaResultsModel.MediaDateTime, objIQAgent_MediaResultsModel.MediaDateTime*/));
                                iQAgent_NewsResultsModel.PositiveSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.PositiveSentiment);
                                iQAgent_NewsResultsModel.NegativeSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.NegativeSentiment);
                                iQAgent_NewsResultsModel.IQProminenceMultiplier = (objIQAgent_MediaResultsModel.MediaData as IQAgent_NewsResultsModel).IQProminenceMultiplier;
                                if (iQAgent_NewsResultsModel.IQLicense == 3)
                                {
                                    // LexisNexis articles need to know if they were opened from Library
                                    iQAgent_NewsResultsModel.ArticleUri += "&source=library";
                                }
                                result = nmLogic.InsertArchiveNM(iQAgent_NewsResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, archiveCommonModel.CategoryGuid, Event, archiveCommonModel.Keywords, archiveCommonModel.Description, archiveCommonModel.MediaResultID, clientSettings.UseProminenceMediaValue == true);
                            }
                        }
                        else if (mediaType == IQMedia.Shared.Utility.CommonFunctions.MediaType.SM.ToString())
                        {
                            SMLogic smLogic = (SMLogic)LogicFactory.GetLogic(LogicType.SM);
                            IQAgent_SMResultsModel iQAgent_SMResultsModel = objIQAgent_MediaResultsModel.MediaData as IQAgent_SMResultsModel;

                            if (isMissingArticle)
                            {
                                result = smLogic.InsertArchiveSM(iQAgent_SMResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, archiveCommonModel.CategoryGuid, archiveCommonModel.Keywords, archiveCommonModel.Description, archiveCommonModel.MediaResultID, clientSettings.UseProminenceMediaValue == true);
                            }
                            else if (iQAgent_SMResultsModel.SourceCategory == "FB" || iQAgent_SMResultsModel.SourceCategory == "IG") // Facebook/Instagram content isn't stored in the Discovery solr engines. 
                            {
                                // There is no highlighting text, so HighlightedSMOutput just stores the full content. Put that into the HighlightingText field so that it gets inserted into the archive table as content.
                                if (iQAgent_SMResultsModel.HighlightedSMOutput != null && iQAgent_SMResultsModel.HighlightedSMOutput.Highlights != null)
                                {
                                    iQAgent_SMResultsModel.HighlightingText = iQAgent_SMResultsModel.HighlightedSMOutput.Highlights[0];
                                }
                                result = smLogic.InsertArchiveSM(iQAgent_SMResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, archiveCommonModel.CategoryGuid, archiveCommonModel.Keywords, archiveCommonModel.Description, archiveCommonModel.MediaResultID, clientSettings.UseProminenceMediaValue == true);
                            }
                            else
                            {
                                iQAgent_SMResultsModel = smLogic.SearchSocialMediaByArticleID(objIQAgent_MediaResultsModel.ArticleID, WebApplication.Utility.CommonFunctions.GeneratePMGUrl(WebApplication.Utility.CommonFunctions.PMGUrlType.MO.ToString(), objIQAgent_MediaResultsModel.MediaDateTime, objIQAgent_MediaResultsModel.MediaDateTime));
                                iQAgent_SMResultsModel.PositiveSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.PositiveSentiment);
                                iQAgent_SMResultsModel.NegativeSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.NegativeSentiment);
                                iQAgent_SMResultsModel.IQProminenceMultiplier = (objIQAgent_MediaResultsModel.MediaData as IQAgent_SMResultsModel).IQProminenceMultiplier;
                                result = smLogic.InsertArchiveSM(iQAgent_SMResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, archiveCommonModel.CategoryGuid, archiveCommonModel.Keywords, archiveCommonModel.Description, archiveCommonModel.MediaResultID, clientSettings.UseProminenceMediaValue == true);
                            }
                        }
                        else if (mediaType == IQMedia.Shared.Utility.CommonFunctions.MediaType.TW.ToString())
                        {
                            TWLogic twLogic = (TWLogic)LogicFactory.GetLogic(LogicType.TW);
                            IQAgent_TwitterResultsModel iQAgent_TwitterResultsModel = twLogic.SearchTwitterByTweetID(objIQAgent_MediaResultsModel.ArticleID, WebApplication.Utility.CommonFunctions.GeneratePMGUrl(WebApplication.Utility.CommonFunctions.PMGUrlType.TW.ToString(), objIQAgent_MediaResultsModel.MediaDateTime, objIQAgent_MediaResultsModel.MediaDateTime));
                            iQAgent_TwitterResultsModel.PositiveSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.PositiveSentiment);
                            iQAgent_TwitterResultsModel.NegativeSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.NegativeSentiment);
                            result = twLogic.InsertArchiveTW(iQAgent_TwitterResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, archiveCommonModel.CategoryGuid, archiveCommonModel.Keywords, archiveCommonModel.Description, archiveCommonModel.MediaResultID);
                        }
                        else if (mediaType == IQMedia.Shared.Utility.CommonFunctions.MediaType.PQ.ToString())
                        {
                            PQLogic pqLogic = (PQLogic)LogicFactory.GetLogic(LogicType.PQ);
                            IQAgent_PQResultsModel iQAgent_PQResultsModel = pqLogic.SearchProQuestByArticleID(objIQAgent_MediaResultsModel.ArticleID, Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.PQ.ToString(), objIQAgent_MediaResultsModel.MediaDateTime, objIQAgent_MediaResultsModel.MediaDateTime));
                            iQAgent_PQResultsModel.PositiveSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.PositiveSentiment);
                            iQAgent_PQResultsModel.NegativeSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.NegativeSentiment);
                            result = pqLogic.InsertArchivePQ(iQAgent_PQResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, archiveCommonModel.CategoryGuid, archiveCommonModel.Keywords, archiveCommonModel.Description, archiveCommonModel.MediaResultID);
                        }
                        else if (mediaType == IQMedia.Shared.Utility.CommonFunctions.MediaType.TM.ToString())
                        {
                            TVEyesLogic tvEyesLogic = (TVEyesLogic)LogicFactory.GetLogic(LogicType.TVEyes);
                            IQAgent_TVEyesResultsModel iQAgent_TVEyesResultsModel = tvEyesLogic.SearchTVEyesByMediaID(archiveCommonModel.MediaResultID, Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.FE.ToString(), objIQAgent_MediaResultsModel.MediaDateTime, objIQAgent_MediaResultsModel.MediaDateTime));
                            result = tvEyesLogic.InsertArchiveTVEyes(archiveCommonModel.MediaResultID, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, archiveCommonModel.CategoryGuid, iQAgent_TVEyesResultsModel, archiveCommonModel.Keywords, archiveCommonModel.Description);
                        }
                    }

                    string resultMessage = string.Empty;
                    bool isSaved = false;

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        resultMessage = ConfigSettings.Settings.ArticleNotSaved;// "Article not saved.";
                    }
                    else if (!string.IsNullOrWhiteSpace(result) && Convert.ToInt32(result) <= 0)
                    {
                        if (Convert.ToInt32(result) == -1)
                        {
                            isSaved = true;
                            resultMessage = ConfigSettings.Settings.ArticleAlreadySaved; //"Article is already saved.";
                        }
                        else
                        {
                            resultMessage = ConfigSettings.Settings.ErrorOccurred;// "An error occur, please try again.";
                        }

                    }

                    else if (Convert.ToInt32(result) == 0)
                    {
                        resultMessage = ConfigSettings.Settings.ErrorOccurred;// "An error occur, please try again.";
                    }
                    else
                    {
                        isSaved = true;
                        resultMessage = ConfigSettings.Settings.ArticleSaved;// "Article Saved Successfully";
                    }

                    // If the article was successfully saved or was already in Library, mark it and all of its children as read.
                    // If it is a child record, mark it's parent and all other children as read.
                    if (isSaved)
                    {
                        UpdateIsRead(new List<string> { archiveCommonModel.ParentID.ToString() }, true, false);
                    }

                    return Json(new
                    {
                        message = resultMessage,
                        isSuccess = true
                    });
                }
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
            return Json(new object());
        }

        [HttpPost]
        public ContentResult Insert_FeedReport()
        {
            try
            {
                Request.InputStream.Position = 0;

                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    string input = new StreamReader(Request.InputStream).ReadToEnd();
                    Shared.Utility.Log4NetLogger.Info(input);
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(input);
                }

                string p_Title = dictParams["p_Title"].ToString();
                string p_Keywords = dictParams["p_Keywords"].ToString();
                string p_Description = dictParams["p_Description"].ToString();
                Guid p_CategoryGuid = Guid.Parse(dictParams["p_CategoryGuid"].ToString());
                List<string> p_SelectedRecords = ((JArray)dictParams["p_SelectedRecords"]).ToObject<List<string>>();
                long p_FolderID = Int64.Parse(dictParams["p_FolderID"].ToString());
                bool p_IsAsc = Boolean.Parse(dictParams["p_IsAsc"].ToString());
                bool p_IsSelectAllChecked = Boolean.Parse(dictParams["p_IsSelectAllChecked"].ToString());
                bool p_IsSelectAll = Boolean.Parse(dictParams["p_IsSelectAll"].ToString());
                bool? p_IsAudienceSort = dictParams["p_IsAudienceSort"] == null ? (bool?)null : Boolean.Parse(dictParams["p_IsAudienceSort"].ToString());
                bool p_IsProminenceSearch = Boolean.Parse(dictParams["p_IsProminenceSearch"].ToString());
                bool? p_IsRead = dictParams["p_IsRead"] == null ? (bool?)null : Boolean.Parse(dictParams["p_IsRead"].ToString());
                long tempReportImage;
                long? p_ReportImage = null;
                if (Int64.TryParse(dictParams["p_ReportImage"].ToString(), out tempReportImage))
                {
                    p_ReportImage = tempReportImage;
                }

                string resultMessage = string.Empty;
                bool showPopup = false;
                Dictionary<string, object> dictResults = new Dictionary<string, object>();
                Dictionary<string, string> childHTML = new Dictionary<string, string>();

                List<IQAgent_MediaResultsModel> mediaRecords = p_SelectedRecords.Select(s => new IQAgent_MediaResultsModel()
                                                                                            {
                                                                                                ID = Convert.ToInt64(s.Substring(0, s.IndexOf(":"))),
                                                                                                MediaType = s.Substring(s.IndexOf(":") + 1)
                                                                                            }).ToList();
                List<string> mediaIDs = mediaRecords.Select(s => s.ID.ToString()).ToList();

                // Since child records don't exist in the results until the parent is expanded, the Select All functionality can miss them.
                // So if it is selected, get the children now and add them to the results so that they are available for the next action.
                // Not necessary if filtering on prominence, since rollup is disabled.
                if (p_IsSelectAllChecked && !p_IsProminenceSearch)
                {
                    dictResults = SearchAllChildResults(mediaRecords, mediaIDs, p_IsAsc, p_IsAudienceSort, p_IsRead, p_IsSelectAll);
                }

                if (dictResults.ContainsKey("MediaIDs"))
                {
                    mediaIDs = (List<string>)dictResults["MediaIDs"];
                }
                if (dictResults.ContainsKey("ChildHTML"))
                {
                    childHTML = (Dictionary<string, string>)dictResults["ChildHTML"];
                }

                if (mediaIDs != null && mediaIDs.Count() > 0)
                {
                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                    feedsTempData = GetTempData();
                    IQFeeds_ReportModel iQFeeds_ReportModel = new IQFeeds_ReportModel();
                    iQFeeds_ReportModel.Title = p_Title.Trim();
                    iQFeeds_ReportModel.Keywords = p_Keywords.Trim();
                    iQFeeds_ReportModel.Description = p_Description.Trim();
                    iQFeeds_ReportModel.CategoryGuid = p_CategoryGuid;
                    iQFeeds_ReportModel.CustomerGuid = sessionInformation.CustomerGUID;
                    iQFeeds_ReportModel.ClientGuid = sessionInformation.ClientGUID;
                    iQFeeds_ReportModel.FolderID = p_FolderID;
                    iQFeeds_ReportModel.ReportImageID = p_ReportImage;

                    if (!feedsTempData.MaxFeedsReportLimit.HasValue)
                    {
                        string maxFeedsReportLimit = UtilityLogic.GetFeedsReportLimit(sessionInformation.ClientGUID);
                        feedsTempData.MaxFeedsReportLimit = Convert.ToInt32(maxFeedsReportLimit);
                        SetTempData(feedsTempData);
                    }

                    List<string> lstMediaID = mediaIDs.Take(feedsTempData.MaxFeedsReportLimit.Value).ToList();
                    XDocument xdoc = new XDocument(new XElement("MediaIds", lstMediaID.Select(s => new XElement("ID", s))));
                    iQFeeds_ReportModel.MediaID = xdoc;

                    ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                    string result = reportLogic.InsertFeedsReport(iQFeeds_ReportModel);

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        resultMessage = ConfigSettings.Settings.ReportNotSaved;// "Report not saved.";
                        showPopup = false;
                    }
                    else if (!string.IsNullOrWhiteSpace(result) && Convert.ToInt32(result) < 0)
                    {
                        resultMessage = ConfigSettings.Settings.ErrorOccurred;// "An error occur, please try again.";
                        showPopup = true;
                    }

                    else if (Convert.ToInt32(result) == 0)
                    {
                        resultMessage = ConfigSettings.Settings.ReportWithSameNameExists; //"Report with same name already exists.";
                        showPopup = true;
                    }
                    else
                    {
                        resultMessage = ConfigSettings.Settings.ReportSaved;// "Report Saved Successfully";
                        showPopup = false;
                    }
                }
                else
                {
                    resultMessage = ConfigSettings.Settings.SelectOneMoreItemMessage;
                    showPopup = false;
                }

                var json = new
                {
                    message = resultMessage,
                    needToShowPopup = showPopup,
                    childHTML = childHTML,
                    isSuccess = true
                };

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                var json = new
                {
                    isSuccess = false
                };

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        public JsonResult InsertMissingArticle(IQAgent_MissingArticlesModel p_IQAgent_MissingArticlesModel)
        {
            try
            {

                if (Shared.Utility.CommonFunctions.Validate_url(p_IQAgent_MissingArticlesModel.Url))
                {

                    IQMedia.Shared.Utility.CommonFunctions.MissingArticleTypes Category = (IQMedia.Shared.Utility.CommonFunctions.MissingArticleTypes)Enum.Parse(typeof(IQMedia.Shared.Utility.CommonFunctions.MissingArticleTypes), p_IQAgent_MissingArticlesModel.Category);
                    if (Category != Shared.Utility.CommonFunctions.MissingArticleTypes.NM && Category != Shared.Utility.CommonFunctions.MissingArticleTypes.SocialMedia)
                    {
                        p_IQAgent_MissingArticlesModel.Category = Shared.Utility.CommonFunctions.GetEnumDescription(Category);
                    }
                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                    // Convert article date from client's local time to GMT
                    DateTime harvestTime = p_IQAgent_MissingArticlesModel.harvest_time;
                    if (harvestTime.IsDaylightSavingTime())
                    {
                        p_IQAgent_MissingArticlesModel.harvest_time = harvestTime.AddHours(-1 * ((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst)));
                    }
                    else
                    {
                        p_IQAgent_MissingArticlesModel.harvest_time = harvestTime.AddHours(-1 * (Convert.ToDouble(sessionInformation.gmt)));
                    }

                    IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                    string result = iQAgentLogic.InsertMissingArticle(p_IQAgent_MissingArticlesModel, sessionInformation.ClientGUID, sessionInformation.CustomerGUID);

                    if (!string.IsNullOrEmpty(result) && Convert.ToInt32(result) > 0)
                    {
                        return Json(new
                        {
                            isSuccess = true,
                            msg = Config.ConfigSettings.Settings.MissingArticleSaved

                        });
                    }
                    else if (!string.IsNullOrEmpty(result) && Convert.ToInt32(result) == -1)
                    {
                        return Json(new
                        {
                            isSuccess = false,
                            msg = Config.ConfigSettings.Settings.ArticleAlreadySaved
                        });
                    }
                    else if (!string.IsNullOrEmpty(result) && Convert.ToInt32(result) == -2)
                    {
                        return Json(new
                        {
                            isSuccess = false,
                            msg = Config.ConfigSettings.Settings.InvalidSearchRequestForArticle
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = false,
                            msg = Config.ConfigSettings.Settings.ArticleNotSaved
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                        msg = "Invalid Url"
                    });
                }
            }
            catch (Exception exception)
            {

                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    msg = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        #endregion

        #region Utility Action

        public JsonResult GetFeedsReportLimit()
        {
            try
            {
                if (ActiveUserMgr.CheckAuthentication())
                {
                    sessionInformation = ActiveUserMgr.GetActiveUser();
                    string maxFeedsReportLimit = UtilityLogic.GetFeedsReportLimit(sessionInformation.ClientGUID);

                    feedsTempData = GetTempData();

                    feedsTempData.MaxFeedsReportLimit = Convert.ToInt32(maxFeedsReportLimit);
                    SetTempData(feedsTempData);
                    //TempData.Keep();

                    return Json(new
                    {
                        MaxFeedsReportItems = maxFeedsReportLimit,
                        isSuccess = true
                    });

                }
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
            return Json(new object());
        }

        [HttpPost]
        public JsonResult SelectFeedsReport()
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();

                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                List<IQFeeds_ReportModel> lstIQFeeds_ReportModel = reportLogic.SelectFeedsReport(sessionInformation.ClientGUID);

                return Json(new
                {
                    reportList = lstIQFeeds_ReportModel,
                    isSuccess = true
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }

        }

        [HttpPost]
        public ContentResult AddToFeedsReport()
        {
            string mediaID = string.Empty;
            XDocument xDoc = new XDocument();

            Request.InputStream.Position = 0;

            Dictionary<string, object> dictParams;

            using (var sr = new StreamReader(Request.InputStream))
            {
                dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
            }

            long p_ReportID = Int64.Parse(dictParams["p_ReportID"].ToString());

            try
            {
                List<string> p_RecordList = ((JArray)dictParams["p_RecordList"]).ToObject<List<string>>();
                bool p_IsAsc = Boolean.Parse(dictParams["p_IsAsc"].ToString());
                bool p_IsSelectAllChecked = Boolean.Parse(dictParams["p_IsSelectAllChecked"].ToString());
                bool p_IsSelectAll = Boolean.Parse(dictParams["p_IsSelectAll"].ToString());
                bool? p_IsAudienceSort = dictParams["p_IsAudienceSort"] == null ? (bool?)null : Boolean.Parse(dictParams["p_IsAudienceSort"].ToString());
                bool p_IsProminenceSearch = Boolean.Parse(dictParams["p_IsProminenceSearch"].ToString());
                bool? p_IsRead = dictParams["p_IsRead"] == null ? (bool?)null : Boolean.Parse(dictParams["p_IsRead"].ToString());

                Shared.Utility.Log4NetLogger.Info("Start Add to Report Action for Report ID : " + p_ReportID);

                sessionInformation = ActiveUserMgr.GetActiveUser();
                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                mediaID = reportLogic.SelectMediaIDByID(sessionInformation.ClientGUID, p_ReportID);

                Shared.Utility.Log4NetLogger.Info("Existing Report XML for Report ID : " + p_ReportID + " === " + mediaID);
                Shared.Utility.Log4NetLogger.Info("Getting child records for Report ID : " + p_ReportID);

                Dictionary<string, object> dictResults = new Dictionary<string, object>();
                Dictionary<string, string> childHTML = new Dictionary<string, string>();

                List<IQAgent_MediaResultsModel> mediaRecords = p_RecordList.Select(s => new IQAgent_MediaResultsModel()
                                                                                            {
                                                                                                ID = Convert.ToInt64(s.Substring(0, s.IndexOf(":"))),
                                                                                                MediaType = s.Substring(s.IndexOf(":") + 1)
                                                                                            }).ToList();
                List<string> mediaIDs = mediaRecords.Select(s => s.ID.ToString()).ToList();

                // Since child records don't exist in the results until the parent is expanded, the Select All functionality can miss them.
                // So if it is selected, get the children now and add them to the results so that they are available for the next action.
                // Not necessary if filtering on prominence, since rollup is disabled.
                if (p_IsSelectAllChecked && !p_IsProminenceSearch)
                {
                    dictResults = SearchAllChildResults(mediaRecords, mediaIDs, p_IsAsc, p_IsAudienceSort, p_IsRead, p_IsSelectAll);
                }

                if (dictResults.ContainsKey("MediaIDs"))
                {
                    mediaIDs = (List<string>)dictResults["MediaIDs"];
                }
                if (dictResults.ContainsKey("ChildHTML"))
                {
                    childHTML = (Dictionary<string, string>)dictResults["ChildHTML"];
                }

                feedsTempData = GetTempData();
                if (!feedsTempData.MaxFeedsReportLimit.HasValue)
                {
                    string maxFeedsReportLimit = UtilityLogic.GetFeedsReportLimit(sessionInformation.ClientGUID);
                    feedsTempData.MaxFeedsReportLimit = Convert.ToInt32(maxFeedsReportLimit);
                    SetTempData(feedsTempData);
                }

                xDoc = XDocument.Parse(mediaID);
                List<string> addToReportList = new List<String>();

                List<string> lstMediaIDs = xDoc.Descendants("MediaIds").Descendants("ID").Select(s => s.Value).ToList();


                foreach (String record in mediaIDs)
                {
                    if (!lstMediaIDs.Contains(record))
                    {
                        addToReportList.Add(record);
                    }
                }

                if ((xDoc.Root.Descendants("ID").Count() + addToReportList.Count) > feedsTempData.MaxFeedsReportLimit)
                {
                    addToReportList = addToReportList.Take(Convert.ToInt32(feedsTempData.MaxFeedsReportLimit - xDoc.Root.Descendants("ID").Count())).ToList();
                }

                if (addToReportList.Count > 0)
                {
                    xDoc.Root.Add(addToReportList.Select(s => new XElement("ID", s)));

                    Shared.Utility.Log4NetLogger.Info("Updating Report XML for Report ID : " + p_ReportID + " === " + xDoc.ToString());

                    reportLogic.IQReportFeeds_Update(xDoc.ToString(), p_ReportID, sessionInformation.ClientGUID, sessionInformation.CustomerGUID);
                }

                Shared.Utility.Log4NetLogger.Info("End Add to Report Action for Report ID : " + p_ReportID);

                var json = new
                {
                    message = addToReportList.Count(),
                    childHTML = childHTML,
                    isSuccess = true
                };

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            catch (Exception exception)
            {
                Shared.Utility.Log4NetLogger.Info("End Add to Report Action with exception for Report ID : " + p_ReportID);
                string info = "Error occured on Report : " + p_ReportID + " input xml :" + mediaID + " xml to update : " + xDoc.ToString();
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception, info);
                var json = new
                {
                    isSuccess = false
                };

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }

        }

        [HttpPost]
        public ContentResult AddToFeedsLibrary()
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser(); 
                
                Request.InputStream.Position = 0;

                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                string p_Keywords = dictParams["p_Keywords"].ToString();
                string p_Description = dictParams["p_Description"].ToString();
                Guid p_CategoryGuid = Guid.Parse(dictParams["p_CategoryGuid"].ToString());
                Guid? p_SubCategory1Guid = dictParams["p_SubCategory1Guid"] == null || dictParams["p_SubCategory1Guid"].ToString() == "0" ? (Guid?)null : Guid.Parse(dictParams["p_SubCategory1Guid"].ToString());
                Guid? p_SubCategory2Guid = dictParams["p_SubCategory2Guid"] == null || dictParams["p_SubCategory2Guid"].ToString() == "0" ? (Guid?)null : Guid.Parse(dictParams["p_SubCategory2Guid"].ToString());
                Guid? p_SubCategory3Guid = dictParams["p_SubCategory3Guid"] == null || dictParams["p_SubCategory3Guid"].ToString() == "0" ? (Guid?)null : Guid.Parse(dictParams["p_SubCategory3Guid"].ToString());
                List<string> p_RecordList = ((JArray)dictParams["p_RecordList"]).ToObject<List<string>>();
                bool p_IsAsc = Boolean.Parse(dictParams["p_IsAsc"].ToString());
                bool p_IsSelectAllChecked = Boolean.Parse(dictParams["p_IsSelectAllChecked"].ToString());
                bool p_IsSelectAll = Boolean.Parse(dictParams["p_IsSelectAll"].ToString());
                bool? p_IsAudienceSort = dictParams["p_IsAudienceSort"] == null ? (bool?)null : Boolean.Parse(dictParams["p_IsAudienceSort"].ToString());
                bool p_IsProminenceSearch = Boolean.Parse(dictParams["p_IsProminenceSearch"].ToString());
                bool? p_IsRead = dictParams["p_IsRead"] == null ? (bool?)null : Boolean.Parse(dictParams["p_IsRead"].ToString());

                Dictionary<string, object> dictResults = new Dictionary<string, object>();
                Dictionary<string, string> childHTML = new Dictionary<string, string>();

                List<IQAgent_MediaResultsModel> mediaRecords = p_RecordList.Select(s => new IQAgent_MediaResultsModel()
                                                                                            {
                                                                                                ID = Convert.ToInt64(s.Substring(0, s.IndexOf(":"))),
                                                                                                MediaType = s.Substring(s.IndexOf(":") + 1)
                                                                                            }).ToList();
                List<string> addToLibraryList = mediaRecords.Select(s => s.ID.ToString()).ToList();

                // Since child records don't exist in the results until the parent is expanded, the Select All functionality can miss them.
                // So if it is selected, get the children now and add them to the results so that they are available for the next action.
                // Not necessary if filtering on prominence, since rollup is disabled.
                if (p_IsSelectAllChecked && !p_IsProminenceSearch)
                {
                    dictResults = SearchAllChildResults(mediaRecords, addToLibraryList, p_IsAsc, p_IsAudienceSort, p_IsRead, p_IsSelectAll);
                }

                if (dictResults.ContainsKey("MediaIDs"))
                {
                    addToLibraryList = (List<string>)dictResults["MediaIDs"];
                }
                if (dictResults.ContainsKey("ChildHTML"))
                {
                    childHTML = (Dictionary<string, string>)dictResults["ChildHTML"];
                }

                feedsTempData = GetTempData();
                if (!feedsTempData.MaxFeedsReportLimit.HasValue)
                {
                    string maxFeedsReportLimit = UtilityLogic.GetFeedsReportLimit(sessionInformation.ClientGUID);
                    feedsTempData.MaxFeedsReportLimit = Convert.ToInt32(maxFeedsReportLimit);
                    SetTempData(feedsTempData);
                }

                if (addToLibraryList.Count > feedsTempData.MaxFeedsReportLimit)
                {
                    addToLibraryList = addToLibraryList.Take(feedsTempData.MaxFeedsReportLimit.Value).ToList();
                }

                if (addToLibraryList.Count > 0)
                {

                    XDocument xdoc = new XDocument(new XElement("MediaIds", addToLibraryList.Select(s => new XElement("ID", s))));

                    IQFeeds_ReportModel iQFeeds_ReportModel = new IQFeeds_ReportModel();
                    iQFeeds_ReportModel.Keywords = p_Keywords.Trim();
                    iQFeeds_ReportModel.Description = p_Description.Trim();
                    iQFeeds_ReportModel.CategoryGuid = p_CategoryGuid;
                    iQFeeds_ReportModel.SubCategory1Guid = p_SubCategory1Guid;
                    iQFeeds_ReportModel.SubCategory2Guid = p_SubCategory2Guid;
                    iQFeeds_ReportModel.SubCategory3Guid = p_SubCategory3Guid;
                    iQFeeds_ReportModel.CustomerGuid = sessionInformation.CustomerGUID;
                    iQFeeds_ReportModel.ClientGuid = sessionInformation.ClientGUID;
                    iQFeeds_ReportModel.MediaID = xdoc;

                    ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                    reportLogic.InsertFeedsLibrary(iQFeeds_ReportModel);
                }

                var json = new
                {
                    message = addToLibraryList.Count(),
                    childHTML = childHTML,
                    isSuccess = true
                };

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                var json = new
                {
                    isSuccess = false
                };

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }
        #endregion

        #endregion

        #region Custom Methods

        private bool CheckForMoreResultAvailble()
        {
            try
            {
                bool hasMoreResults = true;
                feedsTempData = GetTempData();
                if (feedsTempData.FromRecordID >= feedsTempData.TotalResults)
                {
                    hasMoreResults = false;
                }

                return hasMoreResults;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        private string GetCSVData(List<IQAgent_MediaResultsModel> lstIQAgent_MediaResultsModel)
        {
            string NotApplicable = "";
            string CompeteValue = "(c)";
            string DQ = "\"";
            
            StringBuilder sb = new StringBuilder();

            sessionInformation = ActiveUserMgr.GetActiveUser();
            ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
            IQClient_CustomSettingsModel clientSettings = clientLogic.GetClientCustomSettings(sessionInformation.ClientGUID.ToString());
            feedsTempData = GetTempData();

            if (lstIQAgent_MediaResultsModel != null)
            {
                bool hasFacebook = lstIQAgent_MediaResultsModel.Where(w => w.CategoryType == "FB" || w.CategoryType == "IG").Count() > 0;

                // Build media item header
                sb.Append("Media Date time,Time Zone, Agent,Source,Title,Outlet,DMA,URL" + (sessionInformation.IsNielsenData || sessionInformation.IsCompeteData ? ",Audience,Audience Source, Media Value ($)" : string.Empty) + (sessionInformation.IsNielsenData ? ",National Audience,National Media Value ($)" : string.Empty) + ",Twitter Followers,Twitter Following,Twitter Klout Score," + (hasFacebook ? "Likes,Comments,Shares," : string.Empty) + "Positive Sentiment, Negative Sentiment,Number of Hits,Text");
                sb.Append(Environment.NewLine);

                foreach (IQAgent_MediaResultsModel item in lstIQAgent_MediaResultsModel)
                {
                    IQMedia.Shared.Utility.CommonFunctions.CategoryType category = (IQMedia.Shared.Utility.CommonFunctions.CategoryType)Enum.Parse(typeof(IQMedia.Shared.Utility.CommonFunctions.CategoryType), Convert.ToString(item.CategoryType));

                    // Append Media Date

                    switch (category)
                    {
                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.TV:
                            IQAgent_TVResultsModel tvModel = item.MediaData as IQAgent_TVResultsModel;
                            if (tvModel.LocalDateTime != null)
                            {
                                sb.Append(tvModel.LocalDateTime.ToString());
                            }
                            else
                            {
                                sb.Append(string.Empty);
                            }
                            break;
                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.Radio:
                            IQAgent_TVEyesResultsModel tvEyesModel = item.MediaData as IQAgent_TVEyesResultsModel;
                            if (tvEyesModel.LocalDateTime != null)
                            {
                                sb.Append(tvEyesModel.LocalDateTime.ToString());
                            }
                            else
                            {
                                sb.Append(string.Empty);
                            }
                            break;
                        default:
                            if (item.MediaDateTime != null)
                            {
                                sb.Append(item.MediaDateTime.ToString());
                            }
                            else
                            {
                                sb.Append(string.Empty);
                            }
                            break;
                    }

                    sb.Append(",");

                    string HighlighedText = string.Empty;
                    switch (category)
                    {
                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.TV:

                            IQAgent_TVResultsModel tvModel = item.MediaData as IQAgent_TVResultsModel;

                            if (tvModel.highlightedCCOutput != null && tvModel.highlightedCCOutput.CC != null)
                            {
                                HighlighedText = string.Join(" ", tvModel.highlightedCCOutput.CC.Select(c => c.Text));
                            }

                            HighlighedText = HighlighedText.Replace("&lt;", "<").Replace("&gt;", ">");

                            if (HighlighedText.Length > 255)
                            {
                                int IndexAfter255Char = HighlighedText.Substring(255).IndexOfAny(new[] { ' ', '\t', '\n', '\r' });
                                HighlighedText = IndexAfter255Char == -1 ? HighlighedText.Substring(0, HighlighedText.Length) : HighlighedText.Substring(0, 255 + IndexAfter255Char) + "...";
                            }
                            HighlighedText = HighlighedText.Replace("\"", "\"\"");

                            // Append TimeZone
                            sb.Append(tvModel.TimeZone);
                            sb.Append(",");


                            // Agent Name
                            sb.Append(DQ + item.SearchAgentName.Replace("\"", "\"\"") + DQ);
                            sb.Append(",");


                            // Append Source
                            sb.Append(IQMedia.Shared.Utility.CommonFunctions.GetEnumDescription(IQMedia.Shared.Utility.CommonFunctions.CategoryType.TV));
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + tvModel.Title120.Replace("\"", "\"\"") + DQ);
                            sb.Append(",");

                            // Append Station Call Sign
                            sb.Append(DQ + tvModel.Station_Call_Sign + DQ);
                            sb.Append(",");

                            // Append DMA

                            sb.Append(DQ + tvModel.Market + DQ);
                            sb.Append(",");
                            
                            // Append URL
                            // Get the encrypted url key. If the key already exists, the expiration date will be updated.
                            bool isRetry = true;
                            int numTries = 0;
                            string response;
                            XDocument xDoc = null;
                            while (isRetry && numTries < 5)
                            {
                                // If a database connection couldn't be established by the web service, keep trying up to 5 times.
                                response = Shared.Utility.CommonFunctions.DoHttpGetRequest(
                                    String.Format(ConfigurationManager.AppSettings["UrlGetIQAgentIframeUrl"],
                                        DateTime.Now.AddDays(feedsTempData.RawMediaExpiration.Value).ToShortDateString(), tvModel.ID, String.Empty),
                                    Request.UserAgent);

                                xDoc = XDocument.Parse(response);
                                XElement messageNode = xDoc.Descendants("Message").FirstOrDefault();

                                isRetry = messageNode != null && messageNode.Value == "The connection was not closed.";
                                numTries++;
                            }

                            XElement keyNode = xDoc.Descendants("IQAgentFrameURL").FirstOrDefault();
                            if (keyNode != null)
                            {
                                sb.Append(string.Format("=HYPERLINK(" + DQ + ConfigurationManager.AppSettings["IQAgentReportRawMediaPlayerUrl"] + DQ + ")", keyNode.Value.Replace(",", "%2c")));
                            }
                            sb.Append(",");

                            if (tvModel.Nielsen_Audience > 0 && tvModel.IQAdShareValue > 0 && sessionInformation.IsNielsenData)
                            {
                                // Audience
                                sb.Append(tvModel.Nielsen_Audience.HasValue ? tvModel.Nielsen_Audience.Value.ToString() : string.Empty);
                                sb.Append(",");

                                // Audience Source
                                sb.Append(",");

                                // iqMediaValue
                                sb.Append(clientSettings.UseProminenceMediaValue == true ? (tvModel.IQAdShareValue * tvModel.IQProminenceMultiplier) : tvModel.IQAdShareValue);
                                sb.Append(",");
                            }
                            else if (sessionInformation.IsCompeteData)
                            {
                                // Audience
                                sb.Append(string.Empty);
                                sb.Append(",");

                                // Audience Source
                                sb.Append(",");

                                // iqMediaValue
                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (sessionInformation.IsNielsenData)
                            {
                                sb.Append(tvModel.National_Nielsen_Audience);
                                sb.Append(",");

                                sb.Append(tvModel.National_IQAdShareValue);
                                sb.Append(",");
                            }

                            // Followers Count
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Friends Count
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Klout Score
                            sb.Append(string.Empty);
                            sb.Append(",");

                            if (hasFacebook)
                            {
                                sb.Append(",,,");
                            }

                            //positive sentiment
                            sb.Append(item.PositiveSentiment.ToString());
                            sb.Append(",");
                            //negative sentiment
                            sb.Append(item.NegativeSentiment.ToString());
                            sb.Append(",");

                            //number of hits
                            sb.Append(item.NumberOfHits.ToString());
                            sb.Append(",");

                            // Text
                            sb.Append(DQ + HighlighedText + DQ);
                            sb.Append(",");

                            break;

                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.TW:

                            IQAgent_TwitterResultsModel twitterModel = item.MediaData as IQAgent_TwitterResultsModel;

                            if (twitterModel.HighlightedOutput != null && twitterModel.HighlightedOutput.Highlights != null)
                            {
                                HighlighedText = twitterModel.HighlightedOutput.Highlights;
                                HighlighedText = HighlighedText.Replace("\r\n", " ");
                            }
                            HighlighedText = HighlighedText.Replace("\"", "\"\"");

                            // Append TimeZone
                            sb.Append(sessionInformation.TimeZone);
                            sb.Append(",");

                            // Agent Name
                            sb.Append(DQ + item.SearchAgentName.Replace("\"", "\"\"") + DQ);
                            sb.Append(",");

                            // Append Source
                            sb.Append(IQMedia.Shared.Utility.CommonFunctions.GetEnumDescription(IQMedia.Shared.Utility.CommonFunctions.CategoryType.TW));
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + string.Empty + DQ);
                            sb.Append(",");

                            // Append Publication
                            sb.Append(DQ + twitterModel.Actor_DisplayName + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Append URL
                            if (!String.IsNullOrEmpty(twitterModel.Actor_Link) && !String.IsNullOrEmpty(twitterModel.TweetID))
                            {
                                sb.Append(String.Format("=HYPERLINK(\"{0}\")", twitterModel.Actor_Link.Replace(",", "%2c") + "/status/" + twitterModel.TweetID));
                            }
                            sb.Append(",");

                            if (sessionInformation.IsCompeteData || sessionInformation.IsNielsenData)
                            {
                                // Append Audience
                                sb.Append(string.Empty);
                                sb.Append(",");

                                // Audience Source
                                sb.Append(",");

                                // Append Media Value
                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            if (sessionInformation.IsNielsenData)
                            {
                                sb.Append(string.Empty);
                                sb.Append(",");

                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            // Followers Count
                            sb.Append(twitterModel.Actor_FollowersCount);
                            sb.Append(",");

                            // Friends Count
                            sb.Append(twitterModel.Actor_FriendsCount);
                            sb.Append(",");

                            // Klout Score
                            sb.Append(twitterModel.KlOutScore);
                            sb.Append(",");

                            if (hasFacebook)
                            {
                                sb.Append(",,,");
                            }

                            //positive sentiment
                            sb.Append(item.PositiveSentiment.ToString());
                            sb.Append(",");
                            //negative sentiment
                            sb.Append(item.NegativeSentiment.ToString());
                            sb.Append(",");

                            //number of hits
                            sb.Append(item.NumberOfHits.ToString());
                            sb.Append(",");

                            // Text
                            sb.Append(DQ + HighlighedText + DQ);

                            break;

                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.NM:

                            IQAgent_NewsResultsModel newsModel = item.MediaData as IQAgent_NewsResultsModel;

                            if (newsModel.HighlightedNewsOutput != null && newsModel.HighlightedNewsOutput.Highlights != null)
                            {
                                HighlighedText = string.Join(" ", newsModel.HighlightedNewsOutput.Highlights.Select(c => c));
                            }

                            if (HighlighedText.Length > 255)
                            {
                                int IndexAfter255Char = HighlighedText.Substring(255).IndexOfAny(new[] { ' ', '\t', '\n', '\r' });
                                HighlighedText = IndexAfter255Char == -1 ? HighlighedText.Substring(0, HighlighedText.Length) : HighlighedText.Substring(0, 255 + IndexAfter255Char) + "...";
                            }
                            HighlighedText = HighlighedText.Replace("\"", "\"\"");

                            // Append TimeZone
                            sb.Append(sessionInformation.TimeZone);
                            sb.Append(",");

                            // Agent Name
                            sb.Append(DQ + item.SearchAgentName.Replace("\"", "\"\"") + DQ);
                            sb.Append(",");

                            // Append Source
                            if (newsModel.IQLicense == 3)
                            {
                                sb.Append("LexisNexis(R)");
                            }
                            else
                            {
                                sb.Append(IQMedia.Shared.Utility.CommonFunctions.GetEnumDescription(IQMedia.Shared.Utility.CommonFunctions.CategoryType.NM));
                            }
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + HttpUtility.HtmlDecode(newsModel.Title.Replace("\"", "\"\"")) + DQ);
                            sb.Append(",");

                            // Append Publication
                            sb.Append(DQ + newsModel.Publication + DQ);
                            sb.Append(",");

                            // Append DMA
                            if (newsModel.Market.ToLower() == "unknown" || string.IsNullOrEmpty(newsModel.Market))
                            {
                                sb.Append("Global");
                                sb.Append(",");
                            }
                            else
                            {
                                sb.Append(newsModel.Market);
                                sb.Append(",");
                            }
                            // Append URL
                            string nmUrl = newsModel.IQLicense > 0 ? "http://" + Request.ServerVariables["HTTP_HOST"] + Url.Action("Index", "Article", new
                            {
                                au = IQMedia.Shared.Utility.CommonFunctions.EncryptLicenseStringAES(sessionInformation.CustomerKey + "¶Feeds CSV¶" + newsModel.Url + "&u1=cliq40&u2=" + sessionInformation.ClientID + "¶" + newsModel.IQLicense)
                            }) : newsModel.Url;

                            if (!String.IsNullOrEmpty(nmUrl))
                            {
                                sb.Append(String.Format("=HYPERLINK(\"{0}\")", nmUrl.Replace(",", "%2c")));
                            }
                            sb.Append(",");


                            if (Decimal.Compare(Convert.ToDecimal(newsModel.IQAdShareValue), -1M) != 0 && newsModel.Compete_Audience != -1 && sessionInformation.IsCompeteData)
                            {
                                // Audience
                                if (newsModel.Compete_Audience.HasValue)
                                {
                                    sb.Append(newsModel.Compete_Audience.Value.ToString());
                                }
                                else
                                {
                                    sb.Append(NotApplicable);
                                }
                                sb.Append(",");

                                if (!string.IsNullOrWhiteSpace(newsModel.Compete_Result) && newsModel.Compete_Result.ToUpper() == "A")
                                {
                                    sb.Append(CompeteValue);
                                }
                                sb.Append(",");

                                // iQ Media value
                                if (newsModel.IQAdShareValue.HasValue)
                                {
                                    sb.Append(clientSettings.UseProminenceMediaValue == true ? (newsModel.IQAdShareValue * newsModel.IQProminenceMultiplier) : newsModel.IQAdShareValue);
                                }
                                else
                                {
                                    sb.Append(NotApplicable);
                                }
                                sb.Append(",");
                            }
                            else if (sessionInformation.IsNielsenData)
                            {
                                sb.Append(",");
                                sb.Append(",");
                                sb.Append(",");
                            }

                            if (sessionInformation.IsNielsenData)
                            {
                                sb.Append(string.Empty);
                                sb.Append(",");

                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            // Twitter Followers
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Twitter Following
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Klout Score
                            sb.Append(string.Empty);
                            sb.Append(",");

                            if (hasFacebook)
                            {
                                sb.Append(",,,");
                            }

                            //positive sentiment
                            sb.Append(item.PositiveSentiment.ToString());
                            sb.Append(",");

                            //negative sentiment
                            sb.Append(item.NegativeSentiment.ToString());
                            sb.Append(",");

                            //number of hits
                            sb.Append(item.NumberOfHits.ToString());
                            sb.Append(",");

                            // Text
                            sb.Append(DQ + HighlighedText + DQ);
                           
                            break;

                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.SocialMedia:
                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.Forum:
                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.Blog:
                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.FB:
                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.IG:
                            IQAgent_SMResultsModel socialModel = item.MediaData as IQAgent_SMResultsModel;

                            if (socialModel.HighlightedSMOutput != null && socialModel.HighlightedSMOutput.Highlights != null)
                            {
                                HighlighedText = string.Join(" ", socialModel.HighlightedSMOutput.Highlights.Select(c => c));
                            }

                            if (HighlighedText.Length > 255)
                            {
                                int IndexAfter255Char = HighlighedText.Substring(255).IndexOfAny(new[] { ' ', '\t', '\n', '\r' });
                                HighlighedText = IndexAfter255Char == -1 ? HighlighedText.Substring(0, HighlighedText.Length) : HighlighedText.Substring(0, 255 + IndexAfter255Char) + "...";
                            }
                            HighlighedText = HighlighedText.Replace("\"", "\"\"");

                            // Append TimeZone
                            sb.Append(sessionInformation.TimeZone);
                            sb.Append(",");

                            // Agent Name
                            sb.Append(DQ + item.SearchAgentName.Replace("\"", "\"\"") + DQ);
                            sb.Append(",");

                            // Append Source
                            sb.Append(IQMedia.Shared.Utility.CommonFunctions.GetEnumDescription(category));
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + HttpUtility.HtmlDecode(socialModel.Description.Replace("\"", "\"\"")) + DQ);
                            sb.Append(",");

                            // Append Publication
                            sb.Append(DQ + socialModel.HomeLink + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Append URL
                            if (!String.IsNullOrEmpty(socialModel.Link))
                            {
                                sb.Append(String.Format("=HYPERLINK(\"{0}\")", socialModel.Link.Replace(",", "%2c")));
                            }
                            sb.Append(",");

                            if (category == Shared.Utility.CommonFunctions.CategoryType.Blog && Decimal.Compare(Convert.ToDecimal(socialModel.IQAdShareValue), -1M) != 0 && socialModel.Compete_Audience != -1 && sessionInformation.IsCompeteData)
                            {
                                // Audience
                                if (socialModel.Compete_Audience.HasValue)
                                {
                                    sb.Append(socialModel.Compete_Audience.Value.ToString());
                                }
                                else
                                {
                                    sb.Append(NotApplicable);
                                }
                                sb.Append(",");

                                if (!string.IsNullOrWhiteSpace(socialModel.Compete_Result) && socialModel.Compete_Result.ToUpper() == "A")
                                {
                                    sb.Append(CompeteValue);
                                }
                                sb.Append(",");

                                // iQ Media value
                                if (socialModel.IQAdShareValue.HasValue)
                                {
                                    sb.Append(clientSettings.UseProminenceMediaValue == true ? (socialModel.IQAdShareValue * socialModel.IQProminenceMultiplier) : socialModel.IQAdShareValue);
                                }
                                else
                                {
                                    sb.Append(NotApplicable);
                                }
                                sb.Append(",");
                            }
                            else if (sessionInformation.IsNielsenData)
                            {
                                sb.Append(",");
                                sb.Append(",");
                                sb.Append(",");
                            }

                            if (sessionInformation.IsNielsenData)
                            {
                                sb.Append(string.Empty);
                                sb.Append(",");

                                sb.Append(string.Empty);
                                sb.Append(",");
                            }


                            // Twitter Followers
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Twitter Following
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Klout Score
                            sb.Append(string.Empty);
                            sb.Append(",");

                            if (hasFacebook)
                            {
                                if (socialModel.ArticleStats != null)
                                {
                                    sb.Append(socialModel.ArticleStats.Likes.ToString());
                                    sb.Append(",");

                                    sb.Append(socialModel.ArticleStats.Comments.ToString());
                                    sb.Append(",");

                                    sb.Append(socialModel.ArticleStats.Shares.ToString());
                                    sb.Append(",");
                                }
                                else
                                {
                                    sb.Append(",,,");
                                }
                            }

                            //positive sentiment
                            sb.Append(item.PositiveSentiment.ToString());
                            sb.Append(",");

                            //negative sentiment
                            short negsent = item.NegativeSentiment;
                            sb.Append(Convert.ToString(negsent));
                            sb.Append(",");

                            //number of hits
                            sb.Append(item.NumberOfHits.ToString());
                            sb.Append(",");

                            // Text
                            sb.Append(DQ + HighlighedText + DQ);                         

                            break;

                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.Radio:

                            IQAgent_TVEyesResultsModel tvEyesModel = item.MediaData as IQAgent_TVEyesResultsModel;

                            HighlighedText = tvEyesModel.HighlightingText.Replace("&lt;", "<").Replace("&gt;", ">");

                            if (HighlighedText.Length > 255)
                            {
                                int IndexAfter255Char = HighlighedText.Substring(255).IndexOfAny(new[] { ' ', '\t', '\n', '\r' });
                                HighlighedText = IndexAfter255Char == -1 ? HighlighedText.Substring(0, HighlighedText.Length) : HighlighedText.Substring(0, 255 + IndexAfter255Char) + "...";
                            }
                            HighlighedText = HighlighedText.Replace("\"", "\"\"");

                            // Append TimeZone
                            sb.Append(sessionInformation.TimeZone);
                            sb.Append(",");

                            // Agent Name
                            sb.Append(DQ + item.SearchAgentName.Replace("\"", "\"\"") + DQ);
                            sb.Append(",");

                            // Append Source
                            sb.Append(IQMedia.Shared.Utility.CommonFunctions.GetEnumDescription(IQMedia.Shared.Utility.CommonFunctions.CategoryType.Radio));
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + tvEyesModel.Title.Replace("\"", "\"\"") + DQ);
                            sb.Append(",");

                            // Append Station Call Sign
                            sb.Append(DQ + tvEyesModel.StationID + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(DQ + tvEyesModel.Market + DQ);
                            sb.Append(",");

                            // Append URL
                            string radioUrl = ConfigurationManager.AppSettings["RadioRawPlayerURL"] + Url.Encode(IQMedia.Shared.Utility.CommonFunctions.GenerateRandomString() + Shared.Utility.CommonFunctions.EncryptStringAES(item.ID.ToString(), Shared.Utility.CommonFunctions.AesKeyFeedsRadioPlayer, Shared.Utility.CommonFunctions.AesIVFeedsRadioPlayer));
                            sb.Append(String.Format("=HYPERLINK(\"{0}\")", radioUrl.Replace(",", "%2c")));
                            sb.Append(",");

                            if (sessionInformation.IsCompeteData || sessionInformation.IsNielsenData)
                            {
                                // Audience
                                sb.Append(string.Empty);
                                sb.Append(",");

                                // Audience Source
                                sb.Append(",");

                                // iqMediaValue
                                sb.Append(string.Empty);
                                //sb.Append(tvModel.IQAdShareValue.HasValue ? (!string.IsNullOrWhiteSpace(tvModel.Nielsen_Result) ? "(" + tvModel.Nielsen_Result.ToUpper() + ")" : string.Empty) : string.Empty);
                                sb.Append(",");
                            }

                            if (sessionInformation.IsNielsenData)
                            {
                                sb.Append(string.Empty);
                                sb.Append(",");

                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            // Folllowers Count
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Friends Count
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Circulation
                            sb.Append(string.Empty);
                            sb.Append(",");

                            if (hasFacebook)
                            {
                                sb.Append(",,,");
                            }

                            //positive sentiment
                            sb.Append(item.PositiveSentiment.ToString());
                            sb.Append(",");

                            //negative sentiment
                            sb.Append(item.NegativeSentiment.ToString());
                            sb.Append(",");

                            //number of hits
                            sb.Append(item.NumberOfHits.ToString());
                            sb.Append(",");

                            // Text
                            sb.Append(DQ + HighlighedText + DQ);
                           
                            break;

                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.PM:

                            IQAgent_BLPMResultsModel blpmModel = item.MediaData as IQAgent_BLPMResultsModel;
                            string PMBasePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IQArchieve_PMBaseUrl"]);

                            HighlighedText = !string.IsNullOrEmpty(blpmModel.HighlightingText) ? blpmModel.HighlightingText.Replace("&lt;", "<").Replace("&gt;", ">") : string.Empty;

                            if (HighlighedText.Length > 255)
                            {
                                int IndexAfter255Char = HighlighedText.Substring(255).IndexOfAny(new[] { ' ', '\t', '\n', '\r' });
                                HighlighedText = IndexAfter255Char == -1 ? HighlighedText.Substring(0, HighlighedText.Length) : HighlighedText.Substring(0, 255 + IndexAfter255Char) + "...";
                            }
                            HighlighedText = HighlighedText.Replace("\"", "\"\"");

                            // Append TimeZone
                            sb.Append(sessionInformation.TimeZone);
                            sb.Append(",");

                            // Agent Name
                            sb.Append(DQ + item.SearchAgentName.Replace("\"", "\"\"") + DQ);
                            sb.Append(",");

                            // Append Source
                            sb.Append(IQMedia.Shared.Utility.CommonFunctions.GetEnumDescription(IQMedia.Shared.Utility.CommonFunctions.CategoryType.PM));
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + blpmModel.Title.Replace("\"", "\"\"") + DQ);
                            sb.Append(",");

                            // Append Station Call Sign
                            sb.Append(DQ + blpmModel.Pub_Name + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Append URL
                            if (!String.IsNullOrEmpty(blpmModel.FileLocation))
                            {
                                sb.Append(String.Format("=HYPERLINK(\"{0}\")", PMBasePath + blpmModel.FileLocation.Replace(@"\", @"/").Replace(",", "%2c")));
                            }
                            sb.Append(",");

                            if (sessionInformation.IsCompeteData || sessionInformation.IsNielsenData)
                            {
                                // Audience
                                sb.Append(blpmModel.Circulation);
                                sb.Append(",");

                                // Audience Source
                                sb.Append(",");

                                // iqMediaValue
                                sb.Append(string.Empty);
                                //sb.Append(tvModel.IQAdShareValue.HasValue ? (!string.IsNullOrWhiteSpace(tvModel.Nielsen_Result) ? "(" + tvModel.Nielsen_Result.ToUpper() + ")" : string.Empty) : string.Empty);
                                sb.Append(",");
                            }

                            if (sessionInformation.IsNielsenData)
                            {
                                sb.Append(string.Empty);
                                sb.Append(",");

                                sb.Append(string.Empty);
                                sb.Append(",");
                            }

                            // Folllowers Count
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Friends Count
                            sb.Append(string.Empty);
                            sb.Append(",");

                            // Klout Score
                            sb.Append(string.Empty);
                            sb.Append(",");

                            if (hasFacebook)
                            {
                                sb.Append(",,,");
                            }

                           //positive sentiment
                            sb.Append(item.PositiveSentiment.ToString());
                            sb.Append(",");

                            //negative sentiment
                            sb.Append(item.NegativeSentiment.ToString());
                            sb.Append(",");

                           //number of hits
                            sb.Append(item.NumberOfHits.ToString());
                            sb.Append(",");

                            // Text
                            sb.Append(DQ + HighlighedText + DQ);

                            break;

                        case IQMedia.Shared.Utility.CommonFunctions.CategoryType.PQ:

                            IQAgent_PQResultsModel pqModel = item.MediaData as IQAgent_PQResultsModel;

                            if (pqModel.HighlightedPQOutput != null && pqModel.HighlightedPQOutput.Highlights != null)
                            {
                                HighlighedText = string.Join(" ", pqModel.HighlightedPQOutput.Highlights.Select(c => c));
                            }

                            HighlighedText = HighlighedText.Replace("&lt;", "<").Replace("&gt;", ">");

                            if (HighlighedText.Length > 255)
                            {
                                int IndexAfter255Char = HighlighedText.Substring(255).IndexOfAny(new[] { ' ', '\t', '\n', '\r' });
                                HighlighedText = IndexAfter255Char == -1 ? HighlighedText.Substring(0, HighlighedText.Length) : HighlighedText.Substring(0, 255 + IndexAfter255Char) + "...";
                            }
                            HighlighedText = HighlighedText.Replace("\"", "\"\"");

                            // Append TimeZone
                            sb.Append(sessionInformation.TimeZone);
                            sb.Append(",");

                            // Agent Name
                            sb.Append(DQ + item.SearchAgentName.Replace("\"", "\"\"") + DQ);
                            sb.Append(",");

                            // Append Source
                            sb.Append(IQMedia.Shared.Utility.CommonFunctions.GetEnumDescription(IQMedia.Shared.Utility.CommonFunctions.CategoryType.PQ));
                            sb.Append(",");

                            // Append Title
                            sb.Append(DQ + pqModel.Title.Replace("\"", "\"\"") + DQ);
                            sb.Append(",");

                            // Append Publication
                            sb.Append(DQ + pqModel.Publication + DQ);
                            sb.Append(",");

                            // Append DMA
                            sb.Append(",");

                            // Append URL
                            sb.Append("=HYPERLINK(" + DQ + string.Format(ConfigurationManager.AppSettings["ProQuestURL"], "feeds", item.ID) + DQ + ")");
                            sb.Append(",");

                            if (sessionInformation.IsNielsenData || sessionInformation.IsCompeteData)
                            {
                                // Audience
                                sb.Append(",");

                                // Audience Source
                                sb.Append(",");

                                // iqMediaValue
                                sb.Append(",");
                            }

                            // National Audience/Media Value
                            if (sessionInformation.IsNielsenData)
                            {
                                sb.Append(",");
                                sb.Append(",");
                            }

                            // Folllowers Count
                            sb.Append(",");

                            // Friends Count
                            sb.Append(",");

                            // Klout Score
                            sb.Append(",");

                            if (hasFacebook)
                            {
                                sb.Append(",,,");
                            }

                            //positive sentiment
                            sb.Append(item.PositiveSentiment.ToString());
                            sb.Append(",");
                            //negative sentiment
                            sb.Append(item.NegativeSentiment.ToString());
                            sb.Append(",");

                           //number of hits
                            sb.Append(item.NumberOfHits.ToString());
                            sb.Append(",");

                            // Text
                            sb.Append(DQ + HighlighedText + DQ);
                           
                            break;
                        default:
                            sb.Append(",");
                            sb.Append(",");
                            break;
                    }

                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }



        #endregion

        #region Utility

        public string RenderPartialToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public FeedsTempData GetTempData()
        {
            feedsTempData = TempData["FeedsTempData"] != null ? (FeedsTempData)TempData["FeedsTempData"] : new FeedsTempData();
            return feedsTempData;
        }

        public void SetTempData(FeedsTempData p_FeedsTempData)
        {
            TempData["FeedsTempData"] = p_FeedsTempData;
            TempData.Keep("FeedsTempData");
        }

        #endregion


        #region Solr

          public FeedsSearchResponse SearchMediaResults(List<string> mediaTypes, DateTime? fromDate, DateTime? toDate, List<string> searchRequestID, string keyword, short? sentiment, string dma, string station, string competeUrl, List<string> iQDmaIDs, string handle,
                                                        string publication, string author, short? prominenceValue, bool isProminenceAudience, bool isAsc, bool? isAudienceSort, int? pageSize, int numPages, bool isFacetingEnabled, bool? isRead, bool? isHeardFilter, bool? isSeenFilter, bool? isPaidFilter, bool? isEarnedFilter, bool? usePESHFilters, string showTitle, int? dayOfWeek, int? timeOfDay, bool isHour, bool isMonth)
        {       
          try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                feedsTempData = GetTempData();

                DateTime? fromDateLocal = fromDate;
                DateTime? toDateLocal = null;
                if (fromDate.HasValue && toDate.HasValue)
                {
                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);
                    if (!isHour && !isMonth)
                    {
                        toDateLocal = toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    }
                    else
                    {
                        toDateLocal = toDate;
                    }
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }
                else
                {
                    fromDateLocal = DateTime.Now.Date.AddMonths(-3);
                    toDateLocal = toDate = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDateLocal);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }

                int feedsPageSize;
                if (pageSize.HasValue)
                {
                    feedsPageSize = pageSize.Value;
                }
                else
                {
                    feedsPageSize = feedsTempData.DefaultFeedsPageSize;
                }
                feedsPageSize = feedsPageSize * numPages;

                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                string currentUrl = Request.ServerVariables["HTTP_HOST"];
                FeedsSearchResponse fsr = new FeedsSearchResponse() { IsValid = false };
                

                lstTask.Add(Task<FeedsSearchResponse>.Factory.StartNew((object obj) =>
                    SearchMediaResults_Task(null,
                                    mediaTypes,
                                    fromDate,
                                    fromDateLocal,
                                    toDate,
                                    toDateLocal,
                                    searchRequestID,
                                    keyword,
                                    sentiment,
                                    dma,
                                    station,
                                    competeUrl,
                                    iQDmaIDs,
                                    handle,
                                    publication,
                                    author,
                                    prominenceValue,
                                    isProminenceAudience,
                                    isAsc,
                                    isAudienceSort,
                                    feedsPageSize,
                                    GetTempData().FromRecordID,
                                    isFacetingEnabled,
                                    !prominenceValue.HasValue && (iQDmaIDs == null || iQDmaIDs.Count() <= 0), // If filtering on prominence or markets, parent/child rollup is disabled
                                    isRead,
                                    isHeardFilter,
                                    isSeenFilter,
                                    isPaidFilter,
                                    isEarnedFilter,
                                    usePESHFilters,
                                    currentUrl,
                                    showTitle,
                                    dayOfWeek,
                                    timeOfDay,
                                    token,
                                    fsr),
                    fsr));

                try
                {
                    Task.WaitAll(lstTask.ToArray(), Convert.ToInt32(ConfigurationManager.AppSettings["MaxFeedsRequestDuration"]), token);
                    tokenSource.Cancel();
                }
                catch (AggregateException ex)
                {
                    IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                }
                catch (Exception ex)
                {
                    IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                }

                foreach (var tsk in lstTask)
                {
                    if (!((FeedsSearchResponse)tsk.AsyncState).IsValid)
                    {
                        feedsTempData.TotalResults = 0;
                        feedsTempData.TotalResultsDisplay = 0;
                        SetTempData(feedsTempData);
                    }
                    fsr = ((Task<FeedsSearchResponse>)tsk).Result;
                }

                return fsr;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        public FeedsSearchResponse SearchMediaResultsByID(List<string> mediaIDs, List<string> mediaTypes, DateTime? fromDate, DateTime? toDate, List<string> searchRequestID, string keyword, short? sentiment, string dma, string station, string competeUrl, List<string> iQDmaIDs,
                                                            string handle, string publication, string author, short? prominenceValue, bool isProminenceAudience, bool isAsc, bool? isAudienceSort, int pageSize, bool isOnlyParents, bool? isRead, bool? isHeardFilter, bool? isSeenFilter, bool? isPaidFilter, bool? isEarnedFilter, bool? usePESHFilters, string showTitle, int? dayOfWeek, int? timeOfDay)
        {
            try
            {
                // Set the variable here so that it can be used in SearchMediaResults_Task
                sessionInformation = ActiveUserMgr.GetActiveUser();

                if (mediaIDs == null)
                {
                    if (fromDate.HasValue && toDate.HasValue)
                    {
                        fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);
                        toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59));
                    }
                    else
                    {
                        fromDate = Utility.CommonFunctions.GetGMTandDSTTime(DateTime.Now.Date.AddMonths(-3));
                        toDate = Utility.CommonFunctions.GetGMTandDSTTime(DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
                    }
                }

                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                string currentUrl = Request.ServerVariables["HTTP_HOST"];
                FeedsSearchResponse fsrTotal = new FeedsSearchResponse() { IsValid = true };

                int tempPageSize = 500;
                for (int i = 0; i < pageSize; i += tempPageSize)
                {
                    FeedsSearchResponse fsr = new FeedsSearchResponse() { IsValid = false };
                    int fromRecordID = i; // Can't use for loop variable directly in StartNew call, since it will use the last loop value instead of the current loop value.

                    lstTask.Add(Task<FeedsSearchResponse>.Factory.StartNew((object obj) =>
                        SearchMediaResults_Task(mediaIDs,
                                        mediaTypes,
                                        fromDate,
                                        null,
                                        toDate,
                                        null,
                                        searchRequestID,
                                        keyword,
                                        sentiment,
                                        dma,
                                        station,
                                        competeUrl,
                                        iQDmaIDs,
                                        handle,
                                        publication,
                                        author,
                                        prominenceValue,
                                        isProminenceAudience,
                                        isAsc,
                                        isAudienceSort,
                                        tempPageSize,
                                        fromRecordID,
                                        false,
                                        isOnlyParents,
                                        isRead,
                                        isHeardFilter, 
                                        isSeenFilter, 
                                        isPaidFilter, 
                                        isEarnedFilter, 
                                        usePESHFilters,
                                        currentUrl,
                                        showTitle,
                                        dayOfWeek,
                                        timeOfDay,
                                        token,
                                        fsr),
                        fsr));
                }

                try
                {
                    Task.WaitAll(lstTask.ToArray(), Convert.ToInt32(ConfigurationManager.AppSettings["MaxFeedsRequestDuration"]), token);
                    tokenSource.Cancel();
                }
                catch (AggregateException ex)
                {
                    IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                }
                catch (Exception ex)
                {
                    IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                }

                fsrTotal.MediaResults = new List<IQAgent_MediaResultsModel>();
                foreach (var tsk in lstTask)
                {
                    FeedsSearchResponse fsrResult = ((Task<FeedsSearchResponse>)tsk).Result;

                    fsrTotal.IsValid = fsrTotal.IsValid && fsrResult.IsValid;
                    if (fsrResult.IsValid)
                    {
                        fsrTotal.MediaResults.AddRange(fsrResult.MediaResults);
                    }
                }

                // The solr queries are run in parallel, so the results may be returned out of order
                if (fsrTotal.IsValid && fsrTotal.MediaResults.Count > 0)
                {
                    if (isAudienceSort.HasValue)
                    {
                        if (isAudienceSort.Value)
                        {
                            Shared.Utility.Log4NetLogger.Info("Prominence Sort");
                            fsrTotal.MediaResults = fsrTotal.MediaResults.OrderByDescending(x => x.IQProminence).ThenByDescending(x => x.MediaDateTime).ToList();
                        }
                        else
                        {
                            Shared.Utility.Log4NetLogger.Info("Prominence Multiplier Sort");
                            fsrTotal.MediaResults = fsrTotal.MediaResults.OrderByDescending(x => x.IQProminenceMultiplier).ThenByDescending(x => x.MediaDateTime).ToList();
                        }
                    }
                    else if (isAsc)
                    {
                        Shared.Utility.Log4NetLogger.Info("Date Ascending Sort");
                        fsrTotal.MediaResults = fsrTotal.MediaResults.OrderBy(x => x.MediaDateTime).ToList();
                    }
                    else
                    {
                        Shared.Utility.Log4NetLogger.Info("Date Descending Sort");
                        fsrTotal.MediaResults = fsrTotal.MediaResults.OrderByDescending(x => x.MediaDateTime).ToList();
                    }
                }

                return fsrTotal;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        public FeedsSearchResponse SearchMediaResults_Task(List<string> mediaIDs, List<string> mediaTypes, DateTime? fromDate, DateTime? fromDateLocal, DateTime? toDate, DateTime? toDateLocal, List<string> searchRequestID,
                                                              string keyword, short? sentiment, string dma, string station, string competeUrl, List<string> iQDmaIDs, string handle, string publication, string author, short? prominenceValue,
                                                            bool isProminenceAudience, bool isAsc, bool? isAudienceSort, int pageSize, long? fromRecordID, bool isFacetingEnabled, bool isOnlyParents, bool? isRead, bool? isHeardFilter, bool? isSeenFilter, bool? isPaidFilter, bool? isEarnedFilter, bool? usePESHFilters, string currentUrl, string showTitle, int? dayOfWeek, int? timeOfDay, 
                                                          CancellationToken token, FeedsSearchResponse fsr)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Int64? totalResults = 0;
                Int64? totalResultsDisplay = 0;
                Int64? sinceID = 0;
                bool isReadLimitExceeded = false;

                feedsTempData = GetTempData();
                sinceID = feedsTempData.SinceID;

                List<string> excludeMediaTypes = new List<string>();
                if (mediaTypes == null || mediaTypes.Count == 0)
                {
                    if (!sessionInformation.Isv4TV) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.TV.ToString());
                    if (!sessionInformation.Isv4NM) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.NM.ToString());
                    if (!sessionInformation.Isv4SM)
                    {
                        excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.SocialMedia.ToString());
                        excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.Blog.ToString());
                        excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.Forum.ToString());
                        excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.FB.ToString());
                        excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.IG.ToString());
                    }
                    if (!sessionInformation.Isv4TW) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.TW.ToString());
                    if (!sessionInformation.Isv4TM) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.Radio.ToString());
                    if (!sessionInformation.Isv4BLPM) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.PM.ToString());
                    if (!sessionInformation.Isv4PQ) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.PQ.ToString());
                }

                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.FE.ToString(), null, null);
                Dictionary<string, object> dictResults = iqAgentLogic.SearchMediaResults(sessionInformation.CustomerKey,
                                                                                sessionInformation.ClientGUID,
                                                                                mediaIDs,
                                                                                mediaTypes,
                                                                                excludeMediaTypes,
                                                                                fromDate,
                                                                                fromDateLocal,
                                                                                toDate,
                                                                                toDateLocal,
                                                                                searchRequestID,
                                                                                keyword,
                                                                                sentiment,
                                                                                dma,
                                                                                station,
                                                                                competeUrl,
                                                                                iQDmaIDs,
                                                                                handle,
                                                                                publication,
                                                                                author,
                                                                                prominenceValue,
                                                                                isProminenceAudience,
                                                                                isAsc,
                                                                                isAudienceSort,
                                                                                pageSize,
                                                                                fromRecordID,
                                                                                ref sinceID,
                                                                                isFacetingEnabled,
                                                                                isOnlyParents,
                                                                                isRead,
                                                                                isHeardFilter, 
                                                                                isSeenFilter, 
                                                                                isPaidFilter, 
                                                                                isEarnedFilter, 
                                                                                usePESHFilters,
                                                                                pmgUrl,
                                                                                currentUrl,
                                                                                showTitle,
                                                                                dayOfWeek,
                                                                                timeOfDay,
                                                                                feedsTempData.FromRecordID.HasValue && feedsTempData.FromRecordID.Value > 0 ? feedsTempData.ChildCounts : null,
                                                                                out totalResults,
                                                                                out totalResultsDisplay,
                                                                                out isReadLimitExceeded);

                if (!token.IsCancellationRequested)
                {
                    fsr.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                fsr.IsReadLimitExceeded = isReadLimitExceeded;

                if (dictResults.ContainsKey("Result"))
                {
                    fsr.MediaResults = (List<IQAgent_MediaResultsModel>)dictResults["Result"];
                }
                if (dictResults.ContainsKey("Filter"))
                {
                    fsr.Filter = (FeedsFilterModel)dictResults["Filter"];
                }
                if (dictResults.ContainsKey("ChildCounts"))
                {
                    fsr.ChildCounts = (Dictionary<string, string>)dictResults["ChildCounts"];
                }

                // This data only needs to be set on queries that require faceting
                if (isFacetingEnabled)
                {
                    feedsTempData.TotalResults = totalResults;
                    feedsTempData.TotalResultsDisplay = totalResultsDisplay;
                    feedsTempData.SinceID = sinceID;
                    SetTempData(feedsTempData);
                }

                sw.Stop();
                IQMedia.Shared.Utility.Log4NetLogger.Info(string.Format("time taken to fetch Feeds results data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error("SearchMediaResults_Task Error: " + ex);
                fsr.IsValid = false;
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
            return fsr;
        }

        public FeedsChildSearchResponse SearchChildResults(long parentID, string mediaType, bool isAsc, bool? isAudienceSort, bool? isRead)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                feedsTempData = GetTempData();

                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                string currentUrl = Request.ServerVariables["HTTP_HOST"];
                FeedsChildSearchResponse fcsr = new FeedsChildSearchResponse() { IsValid = false };

                lstTask.Add(Task<FeedsChildSearchResponse>.Factory.StartNew((object obj) =>
                    SearchChildResults_Task(sessionInformation.CustomerKey, sessionInformation.ClientGUID, parentID, mediaType, isAsc, isAudienceSort, isRead, feedsTempData.SinceID.Value, currentUrl, token, fcsr),
                    fcsr));

                try
                {
                    Task.WaitAll(lstTask.ToArray(), Convert.ToInt32(ConfigurationManager.AppSettings["MaxFeedsRequestDuration"]), token);
                    tokenSource.Cancel();
                }
                catch (AggregateException ex)
                {
                    IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                }
                catch (Exception ex)
                {
                    IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                }

                foreach (var tsk in lstTask)
                {
                    fcsr = ((Task<FeedsChildSearchResponse>)tsk).Result;
                }

                return fcsr;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        public Dictionary<string, object> SearchAllChildResults(List<IQAgent_MediaResultsModel> lstMediaResults, List<string> mediaIDs, bool isAsc, bool? isAudienceSort, bool? isRead, bool isSelectAll, bool isDelete = false)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();
                feedsTempData = GetTempData();

                Dictionary<string, string> childHTML = new Dictionary<string, string>();
                Dictionary<string, object> dictResults = new Dictionary<string, object>();
                bool isValid = true;

                if (feedsTempData.ChildCounts != null && feedsTempData.ChildCounts.Count > 0)
                {
                    // Get the list of selected records that have children and haven't yet been expanded
                    List<IQAgent_MediaResultsModel> unexpandedRecords = lstMediaResults.Where(s =>
                                                                            feedsTempData.ChildCounts.ContainsKey(s.ID.ToString()) &&
                                                                            !feedsTempData.ChildIDs.ContainsKey(s.ID.ToString())
                                                                        ).ToList();

                    List<Task> lstTask = new List<Task>();
                    var tokenSource = new CancellationTokenSource();
                    var token = tokenSource.Token;
                    string currentUrl = Request.ServerVariables["HTTP_HOST"];

                    foreach (IQAgent_MediaResultsModel parent in unexpandedRecords)
                    {
                        FeedsChildSearchResponse fcsr = new FeedsChildSearchResponse() { IsValid = false };

                        // Can't use loop variable directly in StartNew call, since it will use the last iteration's instance rather than the current one
                        long parentID = parent.ID;
                        string mediaType = parent.MediaType;

                        lstTask.Add(Task<FeedsChildSearchResponse>.Factory.StartNew((object obj) =>
                            SearchChildResults_Task(sessionInformation.CustomerKey, sessionInformation.ClientGUID, parentID, mediaType, isAsc, isAudienceSort, isRead, feedsTempData.SinceID.Value, currentUrl, token, fcsr),
                            fcsr));
                    }

                    try
                    {
                        Task.WaitAll(lstTask.ToArray(), Convert.ToInt32(ConfigurationManager.AppSettings["MaxFeedsRequestDuration"]), token);
                        tokenSource.Cancel();
                    }
                    catch (AggregateException ex)
                    {
                        IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                    }
                    catch (Exception ex)
                    {
                        IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                    }

                    List<FeedsChildSearchResponse> fcsrList = new List<FeedsChildSearchResponse>();
                    foreach (var tsk in lstTask)
                    {
                        FeedsChildSearchResponse fcsr = ((Task<FeedsChildSearchResponse>)tsk).Result;
                        fcsrList.Add(fcsr);
                    }

                    foreach (FeedsChildSearchResponse fcsr in fcsrList)
                    {
                        isValid = isValid && fcsr.IsValid;

                        if (fcsr.IsValid)
                        {
                            List<string> childIDs;
                            if (fcsr.MediaResult.MediaType == Shared.Utility.CommonFunctions.CategoryType.TV.ToString())
                            {
                                childIDs = ((IQAgent_TVResultsModel)fcsr.MediaResult.MediaData).ChildResults.Select(s => s.ID.ToString()).ToList();
                            }
                            else
                            {
                                childIDs = ((IQAgent_NewsResultsModel)fcsr.MediaResult.MediaData).ChildResults.Select(s => s.ID.ToString()).ToList();
                            }

                            // NM parent items will always be correct. TV records could have a new parent, so remove the old ID and add the new one. 
                            // To account for cutoff due to selection size limits, add the new ID at the same index as the old one.
                            if (fcsr.MediaResult.MediaType == Shared.Utility.CommonFunctions.CategoryType.TV.ToString())
                            {
                                if (!mediaIDs.Contains(fcsr.MediaResult.ID.ToString()))
                                {
                                    mediaIDs.Insert(mediaIDs.IndexOf(fcsr.OrigParentID.ToString()), fcsr.MediaResult.ID.ToString());
                                }
                                mediaIDs.Remove(fcsr.OrigParentID.ToString());
                            }

                            // If selecting parents and children, add the child IDs. Make sure not to add duplicate IDs, which can happen if filtering on prominence.
                            if (isSelectAll)
                            {
                                mediaIDs.InsertRange(mediaIDs.IndexOf(fcsr.MediaResult.ID.ToString()) + 1, childIDs.Where(s => !mediaIDs.Contains(s)).ToList());
                            }

                            // If not deleting, track the child IDs for future actions, and render their html so the parents can be expanded on the page
                            if (!isDelete)
                            {
                                // Account for TV records where the current parent may actually be a child
                                if (fcsr.OrigParentID != fcsr.MediaResult.ID)
                                {
                                    childIDs.Remove(fcsr.OrigParentID.ToString());
                                    childIDs.Add(fcsr.MediaResult.ID.ToString());
                                }
                                feedsTempData.ChildIDs.Add(fcsr.OrigParentID.ToString(), childIDs);

                                childHTML.Add(fcsr.OrigParentID.ToString(), RenderPartialToString(PATH_FeedChildResultsPartialView, fcsr.MediaResult));
                            }
                        }
                    }

                    SetTempData(feedsTempData);

                    dictResults.Add("ChildHTML", childHTML);
                    dictResults.Add("MediaIDs", mediaIDs);
                    dictResults.Add("IsValid", isValid);
                }

                return dictResults;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
        }

        public FeedsChildSearchResponse SearchChildResults_Task(int customerKey, Guid clientGUID, long parentID, string mediaType, bool isAsc, bool? isAudienceSort, bool? isRead, long sinceID, string currentUrl, CancellationToken token, FeedsChildSearchResponse fcsr)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.FE.ToString(), null, null);
                IQAgent_MediaResultsModel parent = iqAgentLogic.SearchChildResults(customerKey, clientGUID, parentID, mediaType, isAsc, isAudienceSort, isRead, sinceID, pmgUrl, currentUrl);

                if (!token.IsCancellationRequested)
                {
                    fcsr.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                fcsr.OrigParentID = parentID;
                fcsr.MediaResult = parent;

                sw.Stop();
                IQMedia.Shared.Utility.Log4NetLogger.Info(string.Format("time taken to fetch Feeds child results data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                fcsr.IsValid = false;
            }
            return fcsr;
        }

        public FeedsSearchResponse GetFilterData_Task(List<string> mediaTypes, DateTime? fromDate, DateTime? fromDateLocal, DateTime? toDate, DateTime? toDateLocal, List<string> searchRequestID,
                                                              string keyword, short? sentiment, string dma, string station, string competeUrl, List<string> iQDmaIDs, string handle, string publication, string author, short? prominenceValue,
                                                            bool isProminenceAudience, bool? isRead, bool? isHeardFilter, bool? isSeenFilter, bool? isPaidFilter, bool? isEarnedFilter, bool? usePESHFilters, string showTitle, int? dayOfWeek, int? timeOfDay,
                                                          CancellationToken token, FeedsSearchResponse fsr)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Int64? sinceID = 0;

                feedsTempData = GetTempData();
                sinceID = feedsTempData.SinceID;

                List<string> excludeMediaTypes = new List<string>();
                if (mediaTypes == null || mediaTypes.Count == 0)
                {
                    if (!sessionInformation.Isv4TV) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.TV.ToString());
                    if (!sessionInformation.Isv4NM) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.NM.ToString());
                    if (!sessionInformation.Isv4SM)
                    {
                        excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.SocialMedia.ToString());
                        excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.Blog.ToString());
                        excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.Forum.ToString());
                        excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.FB.ToString());
                        excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.IG.ToString());
                    }
                    if (!sessionInformation.Isv4TW) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.TW.ToString());
                    if (!sessionInformation.Isv4TM) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.Radio.ToString());
                    if (!sessionInformation.Isv4BLPM) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.PM.ToString());
                    if (!sessionInformation.Isv4PQ) excludeMediaTypes.Add(Shared.Utility.CommonFunctions.CategoryType.PQ.ToString());
                }

                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.FE.ToString(), null, null);
                FeedsFilterModel filterModel = iqAgentLogic.GetFilterData(sessionInformation.ClientGUID,
                                                                            mediaTypes,
                                                                            excludeMediaTypes,
                                                                            fromDate,
                                                                            fromDateLocal,
                                                                            toDate,
                                                                            toDateLocal,
                                                                            searchRequestID,
                                                                            keyword,
                                                                            sentiment,
                                                                            dma,
                                                                            station,
                                                                            competeUrl,
                                                                            iQDmaIDs,
                                                                            handle,
                                                                            publication,
                                                                            author,
                                                                            prominenceValue,
                                                                            isProminenceAudience,
                                                                            isRead,
                                                                            isHeardFilter,
                                                                            isSeenFilter,
                                                                            isPaidFilter,
                                                                            isEarnedFilter,
                                                                            usePESHFilters,
                                                                            sinceID,
                                                                            pmgUrl,
                                                                            showTitle,
                                                                            dayOfWeek,
                                                                            timeOfDay);

                if (!token.IsCancellationRequested)
                {
                    fsr.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }
                Shared.Utility.Log4NetLogger.Debug(string.Format("filter is {0}", fsr.Filter));
                fsr.Filter = filterModel;

                sw.Stop();
                IQMedia.Shared.Utility.Log4NetLogger.Info(string.Format("time taken to fetch Feeds filter results data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Info("FeedsController: " + ex.Message + " || " + ex.StackTrace);
                fsr.IsValid = false;
            }
            finally
            {
                TempData.Keep("FeedsTempData");
            }
            return fsr;
        }

        #endregion
    }

}
