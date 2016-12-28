using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using PMGSearch;
using System.Configuration;
using IQMedia.Model;
using System.Threading.Tasks;
using System.Xml.Linq;
using IQMedia.Shared.Utility;
using IQMedia.Web.Logic.Base;
using System.Threading;
using System.Text;
using System.Collections;
using System.IO;
using IQMedia.WebApplication.Models;
using IQMedia.WebApplication.Config;
using IQMedia.WebApplication.Models.TempData;
using System.Diagnostics;
using HtmlAgilityPack;
using System.Drawing;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HiQPdf;
using System.Net;
using System.Xml;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class DiscoveryController : Controller
    {
        //
        // GET: /Discovery/

        #region Public Member

        ActiveUser sessionInformation = null;
        DiscoveryTempData discoveryTempData = null;
        string PATH_DiscoveryPartialView = "~/Views/Discovery/_Result.cshtml";
        string PATH_DiscoverySavedSearchPartialView = "~/Views/Discovery/_SavedSearch.cshtml";
        string PATH_DiscoveryTopResultsPartialView = "~/Views/Discovery/_TopResult.cshtml";
        string PATH_DiscoveryTopicsPartialView = "~/Views/Discovery/_Topics.cshtml";
        List<DiscoverySearchResponse> lstTVDiscoveryResponse = null;
        List<DiscoverySearchResponse> lstNMDiscoveryResponse = null;
        List<DiscoverySearchResponse> lstSMDiscoveryResponse = null;
        List<DiscoverySearchResponse> lstSMResponseFeedClass = null;
        List<DiscoverySearchResponse> lstPQDiscoveryResponse = null;
        List<DiscoverySearchResponse> lstMainDiscoverySearchResponse = null;
        List<String> lstTVMarket = null;
        object lockObject = new object();
        #endregion

        public ActionResult Index()
        {
            try
            {
                // Clear out temp data used by other pages
                if (TempData.ContainsKey("AnalyticsTempData")) { TempData["AnalyticsTempData"] = null; }
                if (TempData.ContainsKey("FeedsTempData")) { TempData["FeedsTempData"] = null; }
                if (TempData.ContainsKey("TimeShiftTempData")) { TempData["TimeShiftTempData"] = null; }
                if (TempData.ContainsKey("TAdsTempData")) { TempData["TAdsTempData"] = null; }
                if (TempData.ContainsKey("SetupTempData")) { TempData["SetupTempData"] = null; }
                if (TempData.ContainsKey("GlobalAdminTempData")) { TempData["GlobalAdminTempData"] = null; }

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                SetTempData(null);

                DiscoveryResultRecordTrack discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                List<DiscoveryResultRecordTrack> lst = new List<DiscoveryResultRecordTrack>();
                lst.Add(discoveryResultRecordTrack);
                discoveryTempData = new DiscoveryTempData();
                discoveryTempData.lstDiscoveryResultRecordTrack = lst;
                discoveryTempData.SavedSearchPage = null;
                discoveryTempData.ActiveSearch = null;
                discoveryTempData.MaxDiscoveryReportLimit = null;

                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
                DiscoveryAdvanceSearch_DropDown discoveryAdvanceSearch_DropDown = discoveryLogic.GetSSPDataWithStationByClientGUID(sessionInformation.ClientGUID.ToString());

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                IQClient_CustomSettingsModel customSettings = clientLogic.GetClientCustomSettings(sessionInformation.ClientGUID.ToString());
                discoveryTempData.DefaultDiscoveryPageSize = customSettings.DefaultDiscoveryPageSize;

                IQClient_CustomSettingsModel maxDiscoveryLimit = UtilityLogic.GetDiscoveryReportAndExportLimit(sessionInformation.ClientGUID);
                discoveryTempData.MaxDiscoveryReportLimit = maxDiscoveryLimit.v4MaxDiscoveryReportItems;
                discoveryTempData.MaxDiscoveryExportLimit = maxDiscoveryLimit.v4MaxDiscoveryExportItems;
                discoveryTempData.MaxDiscoveryHistory = maxDiscoveryLimit.v4MaxDiscoveryHistory;

                IQClient_CustomImageLogic iQClient_CustomImageLogic = (IQClient_CustomImageLogic)LogicFactory.GetLogic(LogicType.IQClient_CustomImage);
                List<IQClient_CustomImageModel> lstIQClient_CustomImageModel = iQClient_CustomImageLogic.GetAllIQClient_CustomImageByClientGuid(sessionInformation.ClientGUID);

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

                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                List<IQFeeds_ReportModel> lstIQFeeds_ReportModel = reportLogic.SelectFeedsReport(sessionInformation.ClientGUID);

                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                List<IQAgent_SearchRequestModel> lstIQAgentSearchRequest = iqAgentLogic.SelectActiveIQAgentSearchRequestByClientGuid(Convert.ToString(sessionInformation.ClientGUID));

                List<IQAgent_SearchRequestModel> lstIQAgentAllSearchRequest = iqAgentLogic.SelectIQAgentSearchRequestByClientGuid(Convert.ToString(sessionInformation.ClientGUID), true);

                var manualClipDuration = clientLogic.GetClientManualClipDurationSettings(sessionInformation.ClientGUID);
                Int16 rawMediaPauseSecs = clientLogic.GetClientRawMediaPauseSecs(sessionInformation.ClientGUID);

                Dictionary<string, object> dictFinalResult = new Dictionary<string, object>();
                dictFinalResult.Add("ManualClipDuration", manualClipDuration);
                dictFinalResult.Add("RawMediaPauseSecs", rawMediaPauseSecs);
                dictFinalResult.Add("FeedsReports", lstIQFeeds_ReportModel);
                dictFinalResult.Add("SearchRequests", lstIQAgentSearchRequest);
                dictFinalResult.Add("SearchRequestsAll", lstIQAgentAllSearchRequest);
                dictFinalResult.Add("AdvaceSearchDropDowns", discoveryAdvanceSearch_DropDown);
                dictFinalResult.Add("ReportImages", lstIQClient_CustomImageModel);
                dictFinalResult.Add("ReportFolders", orderedFolderList);
                dictFinalResult.Add("UseCustomerEmailAsDefault", customSettings.UseCustomerEmailDefault.Value);
                dictFinalResult.Add("DefaultEmailSender", customSettings.UseCustomerEmailDefault.Value ? sessionInformation.Email : ConfigurationManager.AppSettings["Sender"]);

                FillTempData(sessionInformation.ClientGUID, discoveryAdvanceSearch_DropDown, customSettings, discoveryTempData);
                SetTempData(discoveryTempData);

                ViewBag.MaxEmailAddresses = System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"];
                ViewBag.IsSuccess = true;
                return View(dictFinalResult);
            }
            catch (Exception exception)
            {
                ViewBag.IsSuccess = false;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return View();
            }
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

        #region Ajax Request
        [HttpPost]
        public ContentResult MediaJsonChart(string[] searchTerm, string[] searchName, string[] searchID, DateTime? fromDate, DateTime? toDate, string medium, bool isDefaultLoad, DiscoveryAdvanceSearchModel[] advanceSearches, string[] advanceSearchIDs, bool useAdvancedSearchDefault)
        {

            try
            {
                MediaChartJsonResponse mediaChartJsonResponse = new MediaChartJsonResponse();
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                if (!isDefaultLoad)
                {
                    List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = new List<DiscoveryResultRecordTrack>();
                    for (int i = 0; i < searchTerm.Length; i ++ )
                    {
                        DiscoveryResultRecordTrack discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                        discoveryResultRecordTrack.SearchTerm = searchTerm[i].ToString();
                        discoveryResultRecordTrack.SearchName = searchName[i].ToString();

                        discoveryResultRecordTrack.IsNMValid = true;
                        discoveryResultRecordTrack.IsSMValid = true;
                        discoveryResultRecordTrack.IsTVValid = true;
                        discoveryResultRecordTrack.IsPQValid = true;

                        discoveryResultRecordTrack.TVRecordTotal = null;
                        discoveryResultRecordTrack.NMRecordTotal = null;
                        discoveryResultRecordTrack.SMRecordTotal = null;
                        discoveryResultRecordTrack.PQRecordTotal = null;

                        lstDiscoveryResultRecordTrack.Add(discoveryResultRecordTrack);
                    }

                    if (fromDate.HasValue && toDate.HasValue)
                    {
                        fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                        toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                        toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                    }
                    else
                    {
                        fromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                        toDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59).AddSeconds(59);

                        fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);
                        toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                    }

                    Shared.Utility.Log4NetLogger.Debug(Session.SessionID + "Discovery From Date : " + fromDate.Value.ToString());
                    Shared.Utility.Log4NetLogger.Debug(Session.SessionID + "Discovery To Date : " + toDate.Value.ToString());

                    discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                    discoveryTempData.lstDiscoveryResultRecordTrack = lstDiscoveryResultRecordTrack;
                    SetTempData(discoveryTempData);
                    //SetSessionData(lstDiscoveryResultRecordTrack);

                    mediaChartJsonResponse = GetChartData(searchTerm, searchName, searchID, fromDate, toDate, medium, advanceSearches, advanceSearchIDs, useAdvancedSearchDefault, sessionInformation.ClientGUID, true, sessionInformation.gmt, sessionInformation.dst);
                    UpdateFromRecordID(lstMainDiscoverySearchResponse);

                    mediaChartJsonResponse.DataAvailableList = GetChartMessage(mediaChartJsonResponse.DataAvailableList, medium);
                }
                else
                {
                    discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                    discoveryTempData.ActiveSearch = null;
                    SetTempData(discoveryTempData);

                    mediaChartJsonResponse.IsSuccess = true;
                    mediaChartJsonResponse.IsSearchTermValid = true;
                }

                dynamic json = new ExpandoObject();
                json.columnChartJson = mediaChartJsonResponse.ColumnChartData;
                json.lineChartJson = mediaChartJsonResponse.LineChartData;
                json.lineChartMediumJson = mediaChartJsonResponse.LineChartMediumData;
                json.pieChartMediumJson = mediaChartJsonResponse.PieChartMediumData;
                json.pieChartSearchTermJson = mediaChartJsonResponse.PieChartSearchTermData != null ? mediaChartJsonResponse.PieChartSearchTermData["JsonResult"] : null;
                json.pieChartSearchTermTotals = mediaChartJsonResponse.PieChartSearchTermData != null ? mediaChartJsonResponse.PieChartSearchTermData["TotalRecords"] : null;
                //notAvailableDataChart = mediaChartJsonResponse.DataNotAvailableList;
                json.availableDataChart = mediaChartJsonResponse.DataAvailableList;
                json.discoveryDateFilter = mediaChartJsonResponse.DateFilter;
                json.discoveryMediumFilter = mediaChartJsonResponse.MediumFilter;
                json.discoveryTVMarketFilter = mediaChartJsonResponse.TVMarket;
                json.isSearchTermValid = mediaChartJsonResponse.IsSearchTermValid;
                json.chartTotal = lstMainDiscoverySearchResponse != null ? string.Format("{0:N0}", lstMainDiscoverySearchResponse.Sum(s => s.TotalResult)) : null;
                json.isSuccess = mediaChartJsonResponse.IsSuccess;

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8); 
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);

                return Content(IQMedia.WebApplication.Utility.CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);                         
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        public ContentResult MediaJsonResults(string[] searchTermArray, string[] searchNameArray, string[] searchIDArray, Int16 searchTermIndex, DateTime? fromDate, DateTime? toDate, string medium, bool IsAsc, bool IsToggle, bool IsTabChange, int? PageSize, DiscoveryAdvanceSearchModel[] advanceSearches, string[] advanceSearchIDs, bool useAdvancedSearchDefault)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                string searchTerm = searchTermArray[searchTermIndex];
                string searchName = searchNameArray[searchTermIndex];
                string searchID = searchIDArray[searchTermIndex];

                if (!IsToggle)
                {
                    List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = new List<DiscoveryResultRecordTrack>();
                    for (int i = 0; i < searchTermArray.Length; i++ )
                    {
                        DiscoveryResultRecordTrack discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                        discoveryResultRecordTrack.SearchTerm = searchTermArray[i].ToString();
                        discoveryResultRecordTrack.SearchName = searchNameArray[i].ToString();
                        discoveryResultRecordTrack.IsNMValid = true;
                        discoveryResultRecordTrack.IsSMValid = true;
                        discoveryResultRecordTrack.IsTVValid = true;
                        discoveryResultRecordTrack.IsPQValid = true;

                        discoveryResultRecordTrack.NMRecordTotal = null;
                        discoveryResultRecordTrack.SMRecordTotal = null;
                        discoveryResultRecordTrack.TVRecordTotal = null;
                        discoveryResultRecordTrack.PQRecordTotal = null;

                        lstDiscoveryResultRecordTrack.Add(discoveryResultRecordTrack);
                    }

                    discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                    discoveryTempData.lstDiscoveryResultRecordTrack = lstDiscoveryResultRecordTrack;
                    SetTempData(discoveryTempData);
                }

                if (fromDate.HasValue && toDate.HasValue)
                {
                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                    toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }
                else
                {
                    fromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    toDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59).AddSeconds(59);

                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }

                Shared.Utility.Log4NetLogger.Debug(Session.SessionID + "Discovery From Date : " + fromDate.Value.ToString());
                Shared.Utility.Log4NetLogger.Debug(Session.SessionID + "Discovery To Date : " + toDate.Value.ToString());

                discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                // Get 4 times the requested number of records and store them on the client, so that a server call doesn't need to be made to obtain more results
                if (PageSize.HasValue)
                {
                    discoveryTempData.CurrentPageSize = PageSize.Value * 4;
                }
                else
                {
                    discoveryTempData.CurrentPageSize = discoveryTempData.DefaultDiscoveryPageSize * 4;
                }

                if (discoveryTempData.CurrentPageSize > Convert.ToInt32(ConfigurationManager.AppSettings["MaxDiscoveryItemLimit"]))
                {
                    discoveryTempData.CurrentPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDiscoveryItemLimit"]);
                }

                Int64? shownRecords = 0;
                Int32 discoveryDisplayPageSize = 0;
                Int64? searchTermWiseTotalRecords = 0;
                string dataAvailableList = string.Empty;
                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;


                if (!IsToggle)
                {
                    lstTask.Add(Task<MediaChartJsonResponse>.Factory.StartNew((object obj) => GetChartData(searchTermArray, searchNameArray, searchIDArray, fromDate, toDate, medium, advanceSearches, advanceSearchIDs, useAdvancedSearchDefault, sessionInformation.ClientGUID, !IsToggle, sessionInformation.gmt, sessionInformation.dst), new MediaChartJsonResponse() { SearchTerm = searchTerm }));
                }

                DiscoveryAdvanceSearchModel advancedSearch = null;
                string advancedSearchID = useAdvancedSearchDefault ? "-2" : searchID;
                if (advanceSearches != null && advanceSearchIDs != null)
                {
                    for (int x = 0; x < advanceSearches.Length; x++)
                    {
                        if (advanceSearchIDs[x] == advancedSearchID)
                        {
                            advancedSearch = advanceSearches[x];
                            break;
                        }
                    }
                }
                if (advancedSearch == null) advancedSearch = new DiscoveryAdvanceSearchModel();

                lstTask.Add(Task<List<DiscoveryMediaResult>>.Factory.StartNew((object obj) => SearchDiscoveryResult(searchTerm, fromDate, toDate, medium, IsAsc, advancedSearch, false, sessionInformation.ClientGUID, out shownRecords, out searchTermWiseTotalRecords, out  dataAvailableList), new DiscoveryMediaResult() { SearchTerm = searchTerm, MediumType = CommonFunctions.CategoryType.TV }));

                try
                {
                    Task.WaitAll(lstTask.ToArray(), token);//, Convert.ToInt32(ConfigurationManager.AppSettings["MaxRequestDuration"]));
                    tokenSource.Cancel();

                }
                catch (AggregateException ex)
                {
                }
                catch (Exception ex)
                {

                }

                StringBuilder strngNotAvailableData = new StringBuilder();
                List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();
                MediaChartJsonResponse mediaChartJsonResponse = new MediaChartJsonResponse();
                foreach (var tsk in lstTask)
                {
                    if (tsk.IsCompleted)
                    {

                        //if (tsk.Status != TaskStatus.Running)
                        //{
                        try
                        {
                            if (tsk.AsyncState.GetType() == typeof(MediaChartJsonResponse))
                            {
                                mediaChartJsonResponse = ((Task<MediaChartJsonResponse>)tsk).Result;
                            }
                            else
                            {
                                lstDiscoveryMediaResult.AddRange(((Task<List<DiscoveryMediaResult>>)tsk).Result);
                            }

                        }
                        catch (Exception)
                        {
                        }
                        //}

                    }
                }

                /*lstDiscoveryMediaResult = lstDiscoveryMediaResult.OrderByDescending(o => o.Date).ToList();
                lstDiscoveryMediaResult = lstDiscoveryMediaResult.Take(Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryPageSize"])).ToList();*/


                List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrackTemp = lstDiscoveryResultRecordTrackTemp = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData(sessionInformation.ClientGUID)).lstDiscoveryResultRecordTrack;
                if (!IsToggle)
                {
                    UpdateFromRecordID(lstMainDiscoverySearchResponse);
                    mediaChartJsonResponse.DataAvailableList = GetChartMessage(mediaChartJsonResponse.DataAvailableList, medium);
                }


                lstDiscoveryMediaResult = UpdateRecordTracking(lstDiscoveryMediaResult, searchTerm, discoveryTempData.CurrentPageSize, IsAsc);
                lstDiscoveryMediaResult.ToList().ForEach(x => x.timeDifference = WebApplication.Utility.CommonFunctions.GetTimeDifference(x.Date));
                lstDiscoveryMediaResult = IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(lstDiscoveryMediaResult, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Discovery);

                bool anyDataAvailable = false;
                dataAvailableList = GetResultMessage(searchTerm, dataAvailableList, medium, out anyDataAvailable);

                discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                searchTermWiseTotalRecords = lstDiscoveryResultRecordTrackTemp.Where(w => w.SearchTerm.Equals(searchTerm)).FirstOrDefault().TotalRecords;


                //shownRecords = lstDiscoveryResultRecordTrackTemp.Where(w => w.SearchTerm.Equals(searchTerm)).Sum(s => s.TVRecordShownNum + s.NMRecordShownNum + s.SMRecordShownNum);
                if (PageSize.HasValue)
                {
                    shownRecords = lstDiscoveryMediaResult.Take(PageSize.Value).Count();
                    discoveryDisplayPageSize = PageSize.Value;
                }
                else
                {
                    shownRecords = lstDiscoveryMediaResult.Take(discoveryTempData.DefaultDiscoveryPageSize).Count();
                    discoveryDisplayPageSize = discoveryTempData.DefaultDiscoveryPageSize;
                }
                searchTermWiseTotalRecords = searchTermWiseTotalRecords == null ? 0 : searchTermWiseTotalRecords;
                shownRecords = shownRecords == null ? 0 : shownRecords;

                dynamic json = new ExpandoObject();
                json.columnChartJson = mediaChartJsonResponse.ColumnChartData;
                json.lineChartJson = mediaChartJsonResponse.LineChartData;
                json.lineChartMediumJson = mediaChartJsonResponse.LineChartMediumData;
                json.pieChartMediumJson = mediaChartJsonResponse.PieChartMediumData;
                json.pieChartSearchTermJson = mediaChartJsonResponse.PieChartSearchTermData != null ? mediaChartJsonResponse.PieChartSearchTermData["JsonResult"] : null;
                json.pieChartSearchTermTotals = mediaChartJsonResponse.PieChartSearchTermData != null ? mediaChartJsonResponse.PieChartSearchTermData["TotalRecords"] : null;

                json.availableDataChart = mediaChartJsonResponse.DataAvailableList;
                json.discoveryDateFilter = mediaChartJsonResponse.DateFilter;
                json.discoveryMediumFilter = mediaChartJsonResponse.MediumFilter;
                json.discoveryTVMarketFilter = mediaChartJsonResponse.TVMarket;
                json.isSearchTermValid = mediaChartJsonResponse.IsSearchTermValid;

                json.hasMoreResults = HasMoreResults((Int64)searchTermWiseTotalRecords, (Int64)shownRecords);

                json.searchTermTotalRecords = searchTermWiseTotalRecords.Value;
                json.searchTermShownRecords = shownRecords;
                json.searchTermAvailableRecords = lstDiscoveryMediaResult.Count();
                json.displayPageSize = discoveryDisplayPageSize;
                json.displayPageSizeOptions = ConfigurationManager.AppSettings["DiscoveryPageSizeOptions"];

                json.chartTotal = lstMainDiscoverySearchResponse != null ? string.Format("{0:N0}", lstMainDiscoverySearchResponse.Sum(s => s.TotalResult)) : null;

                json.searchedIndex = searchTermIndex;
                json.availableDataResult = dataAvailableList;
                json.HTML = lstDiscoveryMediaResult != null ? RenderPartialToString(PATH_DiscoveryPartialView, lstDiscoveryMediaResult) : "";
                json.isAnyDataAvailable = anyDataAvailable;

                json.isSuccess = IsToggle ? true : mediaChartJsonResponse.IsSuccess;

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = new
                {
                    isSuccess = false
                };

                return Content(Convert.ToString(json), "application/json");
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        #region Show More Result

        [HttpPost]
        public ContentResult MoreResult(string[] searchTermArray, Int16 searchTermIndex, DateTime? fromDate, DateTime? toDate, string medium, bool IsAsc, int? PageSize, DiscoveryAdvanceSearchModel advanceSearch)
        {
            try
            {
                Int64? shownRecords = 0;
                Int32 discoveryDisplayPageSize = 0;
                Int64? searchTermWiseTotalRecords = 0;
                string searchTerm = searchTermArray[searchTermIndex];
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                //string dataNotAvailableList = string.Empty;
                string dataAvailableList = string.Empty;

                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;

                if (fromDate.HasValue && toDate.HasValue)
                {
                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                    toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }
                else
                {
                    fromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    toDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59).AddSeconds(59);

                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }

                Shared.Utility.Log4NetLogger.Debug(Session.SessionID + "Discovery From Date : " + fromDate.Value.ToString());
                Shared.Utility.Log4NetLogger.Debug(Session.SessionID + "Discovery To Date : " + toDate.Value.ToString());

                discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                // Get 4 times the requested number of results and store them on the client, so that a server call doesn't need to be made for each More Results call
                if (PageSize.HasValue)
                {
                    discoveryTempData.CurrentPageSize += PageSize.Value * 4;
                }
                else
                {
                    discoveryTempData.CurrentPageSize += discoveryTempData.DefaultDiscoveryPageSize * 4;
                }

                if (discoveryTempData.CurrentPageSize > Convert.ToInt32(ConfigurationManager.AppSettings["MaxDiscoveryItemLimit"]))
                {
                    discoveryTempData.CurrentPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDiscoveryItemLimit"]);
                }

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                lstTask.Add(Task<List<DiscoveryMediaResult>>.Factory.StartNew((object obj) => SearchDiscoveryResult(searchTerm, fromDate, toDate, medium, IsAsc, advanceSearch, true, sessionInformation.ClientGUID, out shownRecords, out searchTermWiseTotalRecords, out  dataAvailableList), new DiscoveryMediaResult() { SearchTerm = searchTerm, MediumType = CommonFunctions.CategoryType.TV }));
                //lstTask.Add(Task<MediaChartJsonResponse>.Factory.StartNew((object obj) => GetChartData(searchTermArray, date, medium, tvMarket, false), new MediaChartJsonResponse() { SearchTerm = searchTerm }));

                try
                {
                    Task.WaitAll(lstTask.ToArray(), token);
                    tokenSource.Cancel();
                }
                catch (AggregateException ex)
                {
                }
                catch (Exception ex)
                {

                }

                StringBuilder strngNotAvailableData = new StringBuilder();
                List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();
                MediaChartJsonResponse mediaChartJsonResponse = new MediaChartJsonResponse();

                foreach (var tsk in lstTask)
                {

                    if (tsk.IsCompleted)
                    {
                        try
                        {
                            lstDiscoveryMediaResult.AddRange(((Task<List<DiscoveryMediaResult>>)tsk).Result);
                        }
                        catch (Exception)
                        {
                            mediaChartJsonResponse = ((Task<MediaChartJsonResponse>)tsk).Result;
                        }
                    }
                }

                /*lstDiscoveryMediaResult = lstDiscoveryMediaResult.OrderByDescending(o => o.Date).ToList();
                lstDiscoveryMediaResult = lstDiscoveryMediaResult.Take(Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryPageSize"])).ToList();*/
                lstDiscoveryMediaResult.ToList().ForEach(x => x.timeDifference = WebApplication.Utility.CommonFunctions.GetTimeDifference(x.Date));
                lstDiscoveryMediaResult = UpdateRecordTracking(lstDiscoveryMediaResult, searchTerm, discoveryTempData.CurrentPageSize, IsAsc);
                lstDiscoveryMediaResult = IQMedia.WebApplication.Utility.CommonFunctions.GetGMTandDSTTime(lstDiscoveryMediaResult, IQMedia.WebApplication.Utility.CommonFunctions.ResultType.Discovery);

                List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrackTemp = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData(sessionInformation.ClientGUID)).lstDiscoveryResultRecordTrack;

                bool anyDataAvailable = false;
                dataAvailableList = GetResultMessage(searchTerm, dataAvailableList, medium, out anyDataAvailable);

                searchTermWiseTotalRecords = (Int64)lstDiscoveryResultRecordTrackTemp.Where(w => w.SearchTerm.Equals(searchTerm)).FirstOrDefault().TotalRecords;
                //shownRecords = lstDiscoveryResultRecordTrackTemp.Where(w => w.SearchTerm.Equals(searchTerm)).Sum(s => s.TVRecordShownNum + s.NMRecordShownNum + s.SMRecordShownNum);
                if (PageSize.HasValue)
                {
                    shownRecords = lstDiscoveryMediaResult.Take(PageSize.Value).Count();
                    discoveryDisplayPageSize = PageSize.Value;
                }
                else
                {
                    shownRecords = lstDiscoveryMediaResult.Take(discoveryTempData.DefaultDiscoveryPageSize).Count();
                    discoveryDisplayPageSize = discoveryTempData.DefaultDiscoveryPageSize;
                }

                searchTermWiseTotalRecords = searchTermWiseTotalRecords == null ? 0 : searchTermWiseTotalRecords;
                shownRecords = shownRecords == null ? 0 : shownRecords;

                DiscoveryResult json = new DiscoveryResult()
                {
                    isSuccess = true,
                    hasMoreResults = HasMoreResults((Int64)searchTermWiseTotalRecords, (Int64)shownRecords),
                    searchedTerm = searchTerm,
                    availableData = dataAvailableList,
                    /*searchTermTotalRecords = string.Format("{0:N0}", searchTermWiseTotalRecords.Value),
                    searchTermShownRecords = string.Format("{0:N0}", shownRecords.Value),*/

                    searchTermTotalRecords = searchTermWiseTotalRecords.Value,
                    searchTermShownRecords = shownRecords,
                    searchTermAvailableRecords = lstDiscoveryMediaResult.Count(),
                    displayPageSize = discoveryDisplayPageSize,

                    isAnyDataAvailable = anyDataAvailable,
                    searchedIndex = searchTermIndex,
                    HTML = lstDiscoveryMediaResult != null ? RenderPartialToString(PATH_DiscoveryPartialView, lstDiscoveryMediaResult) : ""
                };


                var res = Content(CommonFunctions.SearializeJson(json), "application/json", Encoding.UTF8);


                return res;
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = Json(new
                {
                    isSuccess = false
                });

                return Content(Convert.ToString(json), "application/json");
            }

            return Content("");
        }
        #endregion

        [HttpPost]
        public JsonResult GetProQuestResultByID(string articleID)
        {
            try
            {
                PQLogic pqLogic = (PQLogic)LogicFactory.GetLogic(LogicType.PQ);
                IQAgent_PQResultsModel iQAgent_PQResultsModel = pqLogic.SearchProQuestByArticleID(articleID, WebApplication.Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.PQ.ToString(), null, null));

                if (iQAgent_PQResultsModel != null)
                {
                    return Json(new
                    {
                        title = iQAgent_PQResultsModel.Title,
                        publication = iQAgent_PQResultsModel.Publication,
                        authors = iQAgent_PQResultsModel.Authors != null && iQAgent_PQResultsModel.Authors.Count > 0 ? String.Join(", ", iQAgent_PQResultsModel.Authors) : String.Empty,
                        content = iQAgent_PQResultsModel.ContentHTML,
                        mediaDate = iQAgent_PQResultsModel.MediaDate.Value.ToShortDateString(),
                        copyright = iQAgent_PQResultsModel.Copyright,
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
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
        }

        #region Insert
        [HttpPost]
        public JsonResult SaveArticle(string articleID, string searchTem, string categoryGuid, string mediaType, string keywords, string description)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                string result = string.Empty;
                string resultMessage = string.Empty;
                bool showPopup = false;

                if (mediaType == CommonFunctions.CategoryType.NM.ToString())
                {
                    string Event = "Insert Discovery";
                    NMLogic nmLogic = (NMLogic)LogicFactory.GetLogic(LogicType.NM);
                    IQAgent_NewsResultsModel iQAgent_NewsResultsModel = nmLogic.SearchNewsByArticleID(articleID, WebApplication.Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), null, null), searchTem, discoveryTempData.iQClient_ThresholdValueModel);
                    if (iQAgent_NewsResultsModel.IQLicense == 3)
                    {
                        // LexisNexis articles need to know if they were opened from Library
                        iQAgent_NewsResultsModel.ArticleUri += "&source=library";
                    }
                    result = nmLogic.InsertArchiveNM(iQAgent_NewsResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, new Guid(categoryGuid), Event, keywords, description);
                }
                else if (mediaType == CommonFunctions.CategoryType.SocialMedia.ToString() ||
                        mediaType == CommonFunctions.CategoryType.Blog.ToString() ||
                            mediaType == CommonFunctions.CategoryType.Forum.ToString())
                {
                    SMLogic smLogic = (SMLogic)LogicFactory.GetLogic(LogicType.SM);
                    IQAgent_SMResultsModel iQAgent_SMResultsModel = smLogic.SearchSocialMediaByArticleID(articleID, WebApplication.Utility.CommonFunctions.GeneratePMGUrl(WebApplication.Utility.CommonFunctions.PMGUrlType.MO.ToString(), null, null), searchTem, discoveryTempData.iQClient_ThresholdValueModel);
                    iQAgent_SMResultsModel.SourceCategory = mediaType;
                    result = smLogic.InsertArchiveSM(iQAgent_SMResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, new Guid(categoryGuid), keywords, description);
                }
                else if (mediaType == CommonFunctions.CategoryType.PQ.ToString())
                {
                    PQLogic pqLogic = (PQLogic)LogicFactory.GetLogic(LogicType.PQ);
                    IQAgent_PQResultsModel iQAgent_PQResultsModel = pqLogic.SearchProQuestByArticleID(articleID, WebApplication.Utility.CommonFunctions.GeneratePMGUrl(WebApplication.Utility.CommonFunctions.PMGUrlType.PQ.ToString(), null, null), searchTem, discoveryTempData.iQClient_ThresholdValueModel);
                    result = pqLogic.InsertArchivePQ(iQAgent_PQResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, new Guid(categoryGuid), keywords, description);
                }

                if (string.IsNullOrWhiteSpace(result))
                {
                    resultMessage = ConfigSettings.Settings.ArticleNotSaved;// "Article not saved.";
                    showPopup = false;
                }
                else if (!string.IsNullOrWhiteSpace(result) && Convert.ToInt32(result) <= 0)
                {
                    if (Convert.ToInt32(result) == -1)
                    {
                        resultMessage = ConfigSettings.Settings.ArticleAlreadySaved; //"Article is already saved.";
                        showPopup = false;
                    }
                    else
                    {
                        resultMessage = ConfigSettings.Settings.ErrorOccurred;// "An error occur, please try again.";
                        showPopup = true;
                    }

                }

                else if (Convert.ToInt32(result) == 0)
                {
                    resultMessage = ConfigSettings.Settings.ErrorOccurred; //"An error occur, please try again.";
                    showPopup = true;
                }
                else
                {
                    resultMessage = ConfigSettings.Settings.ArticleSaved; //"Article Saved Successfully";
                    showPopup = false;
                }


                return Json(new
                {
                    message = resultMessage,
                    needToShowPopup = showPopup,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = true
                });
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }

            return Json(new object());
        }
        #endregion

        #region Saved Search

        [HttpPost]
        public JsonResult SaveSearch(string title, string[] searchTerm, string[] searchID, string medium, List<DiscoveryAdvanceSearchModel> advanceSearchList, List<string> advanceSearchIDList)//, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket)
        {
            try
            {
                Discovery_SavedSearchModel discovery_SavedSearch = new Discovery_SavedSearchModel();
                discovery_SavedSearch.Title = title;
                discovery_SavedSearch.SearchTermArray = searchTerm;
                discovery_SavedSearch.SearchIDArray = searchID;
                discovery_SavedSearch.AdvanceSearchSettingsList = advanceSearchList;
                discovery_SavedSearch.AdvanceSearchSettingIDsList = advanceSearchIDList;
                discovery_SavedSearch.Medium = medium;
                /*discovery_SavedSearch.FromDate = fromDate;
                discovery_SavedSearch.ToDate = toDate;
                discovery_SavedSearch.Medium = medium;
                discovery_SavedSearch.TVMarket = tvMarket;*/
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                discovery_SavedSearch.CustomerGuid = sessionInformation.CustomerGUID;
                discovery_SavedSearch.ClientGuid = sessionInformation.ClientGUID;
                IQDiscovery_SavedSearchLogic iQDiscovery_SavedSearchLogic = (IQDiscovery_SavedSearchLogic)LogicFactory.GetLogic(LogicType.SavedSearch);
                string result = iQDiscovery_SavedSearchLogic.InsertDiscoverySavedSearch(discovery_SavedSearch);
                string resultMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(result))
                {
                    resultMessage = ConfigSettings.Settings.ErrorOccurred; // "Some error occured, try again later";
                }
                else if (Convert.ToInt64(result) == -2)
                {
                    resultMessage = ConfigSettings.Settings.SearchWithSameNameExists; //"Search with same title already exists";
                }
                else if (Convert.ToInt64(result) > 0)
                {
                    resultMessage = ConfigSettings.Settings.SearchSaved;// "Search saved successfully.";
                    discovery_SavedSearch.ID = Convert.ToInt32(result);
                    discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                    discoveryTempData.ActiveSearch = discovery_SavedSearch;
                }
                return Json(new
                {
                    message = resultMessage,

                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        [HttpPost]
        public JsonResult UpdateSavedSearch(Int32 p_ID, string[] p_SearchTerm, string[] p_SearchID, string medium, List<DiscoveryAdvanceSearchModel> advanceSearchList, List<string> advanceSearchIDList)
        {
            try
            {
                Discovery_SavedSearchModel discovery_SavedSearch = new Discovery_SavedSearchModel();
                discovery_SavedSearch.ID = p_ID;
                discovery_SavedSearch.SearchTermArray = p_SearchTerm;
                discovery_SavedSearch.SearchIDArray = p_SearchID;
                discovery_SavedSearch.Medium = medium;
                discovery_SavedSearch.AdvanceSearchSettingsList = advanceSearchList;
                discovery_SavedSearch.AdvanceSearchSettingIDsList = advanceSearchIDList;
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                discovery_SavedSearch.CustomerGuid = sessionInformation.CustomerGUID;
                discovery_SavedSearch.ClientGuid = sessionInformation.ClientGUID;
                IQDiscovery_SavedSearchLogic iQDiscovery_SavedSearchLogic = (IQDiscovery_SavedSearchLogic)LogicFactory.GetLogic(LogicType.SavedSearch);
                string result = iQDiscovery_SavedSearchLogic.UpdateDiscoverySavedSearch(discovery_SavedSearch);
                string resultMessage = string.Empty;

                if (!string.IsNullOrWhiteSpace(result) && Convert.ToInt64(result) > 0)
                {
                    resultMessage = ConfigSettings.Settings.SearchUpdated;
                    discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                    discoveryTempData.ActiveSearch = discovery_SavedSearch;
                }
                else
                {
                    resultMessage = ConfigSettings.Settings.ErrorOccurred;
                }
                return Json(new
                {
                    message = resultMessage,

                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        [HttpPost]
        public JsonResult GetSaveSearch(bool isNext, bool isInitialize)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                Int32? currentPagenumber = 0;
                Int64 totalRecords = 0;
                discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                currentPagenumber = (Int32?)discoveryTempData.SavedSearchPage;// TempData["SavedSearchPage"];

                if (isInitialize)
                {
                    currentPagenumber = 0;
                }
                else
                {
                    if (isNext)
                    {
                        currentPagenumber = currentPagenumber + 1;
                    }
                    else
                    {
                        currentPagenumber = currentPagenumber - 1;
                    }
                }
                Discovery_SavedSearchModel discovery_SavedSearchModel = new Discovery_SavedSearchModel();
                discovery_SavedSearchModel = (Discovery_SavedSearchModel)discoveryTempData.ActiveSearch;

                Int32? activeSearchID = null;
                if (discovery_SavedSearchModel != null)
                {
                    activeSearchID = discovery_SavedSearchModel.ID;
                }

                IQDiscovery_SavedSearchLogic iQDiscovery_SavedSearchLogic = (IQDiscovery_SavedSearchLogic)LogicFactory.GetLogic(LogicType.SavedSearch);
                List<Discovery_SavedSearchModel> lstDiscovery_SavedSearchModel = iQDiscovery_SavedSearchLogic.SelectDiscoverySavedSearch(currentPagenumber, Convert.ToInt32(ConfigurationManager.AppSettings["DiscoverySavedSearchPageSize"]), activeSearchID, sessionInformation.CustomerGUID, out totalRecords);

                Int64 startIndex = (Int64)((currentPagenumber * Convert.ToInt32(ConfigurationManager.AppSettings["DiscoverySavedSearchPageSize"])) + 1);
                Int64 endIndex = startIndex + Convert.ToInt32(ConfigurationManager.AppSettings["DiscoverySavedSearchPageSize"]) - 1;
                if (endIndex > totalRecords)
                {
                    endIndex = totalRecords;
                }
                string recordDetail = startIndex + " - " + endIndex + " of " + totalRecords;

                discoveryTempData.SavedSearchPage = currentPagenumber;
                SetTempData(discoveryTempData);


                return Json(new
                {
                    isSuccess = true,
                    HTML = RenderPartialToString(PATH_DiscoverySavedSearchPartialView, lstDiscovery_SavedSearchModel),
                    HasMoreResult = HasMoreResults(totalRecords, Convert.ToInt64((currentPagenumber + 1) * Convert.ToInt64(ConfigurationManager.AppSettings["DiscoverySavedSearchPageSize"]))),
                    isPreviousAvailable = currentPagenumber > 0 ? true : false,
                    saveSearchRecordDetail = recordDetail
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        [HttpPost]
        public JsonResult LoadSavedSearch(Int64 p_ID)
        {
            try
            {
                Int64 totalRecords = 0;
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                IQDiscovery_SavedSearchLogic iQDiscovery_SavedSearchLogic = (IQDiscovery_SavedSearchLogic)LogicFactory.GetLogic(LogicType.SavedSearch);
                Discovery_SavedSearchModel discovery_SavedSearchModel = iQDiscovery_SavedSearchLogic.SelectDiscoverySavedSearchByID(p_ID, sessionInformation.CustomerGUID);
                //discovery_SavedSearchModel.IsCurrent = true;

                discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                discoveryTempData.ActiveSearch = discovery_SavedSearchModel;
                int currentPagenumber = discoveryTempData.SavedSearchPage.HasValue ? discoveryTempData.SavedSearchPage.Value : 0;
                //TempData["ActiveSearch"] = discovery_SavedSearchModel;

                List<Discovery_SavedSearchModel> lstDiscovery_SavedSearchModel = iQDiscovery_SavedSearchLogic.SelectDiscoverySavedSearch(currentPagenumber, Convert.ToInt32(ConfigurationManager.AppSettings["DiscoverySavedSearchPageSize"]), (int?)p_ID, sessionInformation.CustomerGUID, out totalRecords);//(int?)p_ID,
                // lstDiscovery_SavedSearchModel.Insert(0, discovery_SavedSearchModel);
                SetTempData(discoveryTempData);
                Int64 startIndex = (Int64)((currentPagenumber * Convert.ToInt32(ConfigurationManager.AppSettings["DiscoverySavedSearchPageSize"])) + 1);
                Int64 endIndex = startIndex + Convert.ToInt32(ConfigurationManager.AppSettings["DiscoverySavedSearchPageSize"]) - 1;
                if (endIndex > totalRecords)
                {
                    endIndex = totalRecords;
                }
                string recordDetail = startIndex + " - " + endIndex + " of " + totalRecords;

                //TempData.Keep();
                return Json(new
                {
                    discovery_SavedSearch = discovery_SavedSearchModel,
                    HTML = RenderPartialToString(PATH_DiscoverySavedSearchPartialView, lstDiscovery_SavedSearchModel),
                    HasMoreResult = HasMoreResults(totalRecords, Convert.ToInt64((Convert.ToInt32(discoveryTempData.SavedSearchPage) + 1) * Convert.ToInt64(ConfigurationManager.AppSettings["DiscoverySavedSearchPageSize"]))),
                    isPreviousAvailable = Convert.ToInt32(discoveryTempData.SavedSearchPage) > 0 ? true : false,
                    saveSearchRecordDetail = recordDetail,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        [HttpPost]
        public JsonResult DeleteDiscoverySavedSearchByID(Int64 p_ID)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                IQDiscovery_SavedSearchLogic iQDiscovery_SavedSearchLogic = (IQDiscovery_SavedSearchLogic)LogicFactory.GetLogic(LogicType.SavedSearch);
                string result = iQDiscovery_SavedSearchLogic.DeleteDiscoverySavedSearchByID(p_ID, sessionInformation.CustomerGUID);

                string returnMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(result))
                {
                    returnMessage = ConfigSettings.Settings.ErrorOccurred;// "Some error occured, try again later";
                }
                else if (Convert.ToInt64(result) > 0)
                {
                    returnMessage = ConfigSettings.Settings.RecordDeleted;// "Record deleted successfully";
                }
                else if (Convert.ToInt64(result) <= 0)
                {
                    returnMessage = ConfigSettings.Settings.RecordNotDeleted; //"Record not deleted";
                }

                return Json(new
                {
                    message = returnMessage,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }

        }


        [HttpPost]
        public JsonResult GetAgentAdvancedSearch(long agentID)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                IQAgent_SearchRequestModel objIQAgentSearchRequest = iqAgentLogic.SelectIQAgentSearchRequestByID(sessionInformation.ClientGUID.ToString(), agentID);

                IQMedia.Model.IQAgentXML.SearchRequest _AgentAdvSearch = new Model.IQAgentXML.SearchRequest();
                if (!string.IsNullOrWhiteSpace(objIQAgentSearchRequest.SearchTerm))
                {
                    _AgentAdvSearch = IQMedia.Shared.Utility.CommonFunctions.DeserialiazeXml(objIQAgentSearchRequest.SearchTerm, _AgentAdvSearch) as IQMedia.Model.IQAgentXML.SearchRequest;
                    _AgentAdvSearch.TVSpecified = (_AgentAdvSearch.TV != null);
                    _AgentAdvSearch.NewsSpecified = (_AgentAdvSearch.News != null);
                    _AgentAdvSearch.SocialMediaSpecified = (_AgentAdvSearch.SocialMedia != null);
                    _AgentAdvSearch.FacebookSpecified = (_AgentAdvSearch.Facebook != null);
                    _AgentAdvSearch.TMSpecified = (_AgentAdvSearch.TM != null);
                    _AgentAdvSearch.PMSpecified = (_AgentAdvSearch.PM != null);
                    _AgentAdvSearch.PQSpecified = (_AgentAdvSearch.PQ != null);
                }

                return Json(new
                {
                    isSuccess = true,
                    agentID = objIQAgentSearchRequest.ID,
                    queryName = objIQAgentSearchRequest.QueryName,
                    agentAdvancedSearch = _AgentAdvSearch
                });
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return Json(new
                {
                    isSuccess = false,
                    error = Config.ConfigSettings.Settings.ErrorOccurred
                });
            }
        }
        #endregion

        #region Download PDF

        [HttpPost]
        public JsonResult GenerateDiscoveryPDF(string p_HTML, string p_FromDate, string p_ToDate, string[] p_SearchTerm)
        {
            try
            {
                string reportHTML = GetHTMLWithCSSIncluded(p_HTML, p_FromDate, p_ToDate, false, p_SearchTerm)[0];

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                string TempPDFPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Discovery\\PDF\\" + sessionInformation.CustomerGUID + "_" + DateTimeStamp + ".pdf"; ;
                
                HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
                htmlToPdfConverter.SerialNumber = ConfigurationManager.AppSettings["HiQPdfSerialKey"];
                htmlToPdfConverter.Document.Margins = new PdfMargins(20);
                htmlToPdfConverter.BrowserWidth = 1000;
                htmlToPdfConverter.ConvertHtmlToFile(reportHTML, String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority), TempPDFPath);

                bool IsFileGenerated = false;

                if (System.IO.File.Exists(TempPDFPath))
                {
                    IsFileGenerated = true;
                    Session["PDFFile"] = TempPDFPath;
                }

                var json = new
                {
                    isSuccess = IsFileGenerated
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
        }

        [HttpPost]
        public JsonResult SendEmail(string p_HTML, string p_FromDate, string p_ToDate, string p_FromEmail, string p_ToEmail, string p_BCCEmail, string p_Subject, string p_UserBody, string[] p_SearchTerm)
        {
            try
            {
                if (p_ToEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"]) &&
                        (String.IsNullOrEmpty(p_BCCEmail) || p_BCCEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"])))
                {
                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                    int EmailSendCount = 0;
                    string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string TempImagePath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Discovery\\PDF\\" + sessionInformation.CustomerGUID + "_" + DateTimeStamp + "_{0}.jpg";
                    string[] bccEmails = !String.IsNullOrWhiteSpace(p_BCCEmail) ? p_BCCEmail.Split(new char[] { ';' }) : new string[0];

                    List<string> lstOfImageHtml = GetHTMLWithCSSIncluded(p_HTML, p_FromDate, p_ToDate, true, p_SearchTerm);
                    string[] alternetViewsName = new string[lstOfImageHtml.Count];

                    for (int i = 0; i < lstOfImageHtml.Count; i++)
                    {
                        string imagePath = String.Format(TempImagePath, i);
                        string imageHtml = lstOfImageHtml[i];          

                        HtmlToImage htmlToImageConverter = new HtmlToImage();
                        htmlToImageConverter.SerialNumber = ConfigurationManager.AppSettings["HiQPdfSerialKey"];
                        htmlToImageConverter.BrowserWidth = 1000;
                        Image img = htmlToImageConverter.ConvertHtmlToImage(imageHtml, String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority))[0];

                        img.Save(imagePath);

                        string attachmentId = Path.GetFileName(imagePath);
                        string iqMediaLogo = Server.MapPath("~/" + ConfigurationManager.AppSettings["IQMediaEmailLogo"]);
                        string iqMediaLogoID = Path.GetFileName(iqMediaLogo);
                        p_UserBody = p_UserBody + "<br/>" + "<img src=\"cid:" + attachmentId + "\" alt='Discovery'/>";

                        alternetViewsName[i] = imagePath;
                        img.Dispose();
                    }

                    StreamReader strmEmailPolicy = new StreamReader(Server.MapPath("~/content/EmailPolicy.txt"));
                    string emailPolicy = strmEmailPolicy.ReadToEnd();
                    strmEmailPolicy.Close();
                    strmEmailPolicy.Dispose();
                    p_UserBody = p_UserBody + emailPolicy;

                    if (!string.IsNullOrEmpty(p_ToEmail))
                    {
                        foreach (string id in p_ToEmail.Split(new char[] { ';' }))
                        {
                            // send email code

                            if (IQMedia.Shared.Utility.CommonFunctions.SendMail(id, string.Empty, bccEmails, p_FromEmail, p_Subject, p_UserBody, true, null, alternetViewsName))
                            {
                                EmailSendCount++;
                            }
                        }
                    }

                    for (int i = 0; i < lstOfImageHtml.Count; i++)
                    {
                        string imagePath = String.Format(TempImagePath, i);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    var json = new
                    {
                        isSuccess = true,
                        emailSendCount = EmailSendCount + bccEmails.Count()
                    };
                    return Json(json);
                }
                else
                {
                    var json = new
                    {
                        isSuccess = false,
                        errorMessage = Config.ConfigSettings.Settings.MaxEmailAdressLimitExceeds.Replace("@@MaxLimit@@", System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"])
                    };
                    return Json(json);
                }

            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }

        }

        [HttpGet]
        public ActionResult DownloadPDFFile()
        {
            if (Session["PDFFile"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["PDFFile"])))
            {
                string PDFFile = Convert.ToString(Session["PDFFile"]);

                if (System.IO.File.Exists(PDFFile))
                {
                    Session.Remove("PDFFile");
                    return File(PDFFile, "application/pdf", Path.GetFileName(PDFFile));
                }
            }
            return Content(ConfigSettings.Settings.FileNotAvailable);
        }

        #endregion

        #region Export CSV
        public JsonResult ExportCSV(List<MediaIDClass> p_MediaID, bool p_SelectAll, Int16 searchTermIndex, string[] searchTermArray, DateTime? fromDate, DateTime? toDate, string medium, DiscoveryAdvanceSearchModel advanceSearch)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);

                discoveryTempData = GetTempData(sessionInformation.ClientGUID);

                Shared.Utility.Log4NetLogger.Info("Discovery Report Limit from Tempdata : " + discoveryTempData.MaxDiscoveryExportLimit);

                if (!discoveryTempData.MaxDiscoveryExportLimit.HasValue)
                {
                    Shared.Utility.Log4NetLogger.Info("Discovery Report Limit is null so fetch from DB again");


                    IQClient_CustomSettingsModel clientSettings = UtilityLogic.GetDiscoveryReportAndExportLimit(sessionInformation.ClientGUID);
                    discoveryTempData.MaxDiscoveryExportLimit = clientSettings.v4MaxDiscoveryExportItems;

                    Shared.Utility.Log4NetLogger.Info("Discovery Report Limit fetched from DB is : " + discoveryTempData.MaxDiscoveryExportLimit);

                    SetTempData(discoveryTempData);
                }

                IQService_DiscoveryLogic iQService_DiscoveryLogic = (IQService_DiscoveryLogic)LogicFactory.GetLogic(LogicType.IQService_Discovery);

                if (!p_SelectAll)
                {
                    List<MediaIDClass> lstMediaID = p_MediaID.Take(discoveryTempData.MaxDiscoveryExportLimit.Value).ToList();
                    XDocument xdoc = new XDocument(new XElement("MediaIDList", new XElement("SearchTerm", searchTermArray[searchTermIndex])));
                    xdoc.Root.Add(new XElement("TV", lstMediaID.Where(w => string.Compare(w.MediaType, "TV", true) == 0).Select(s => new XElement("ID", s.MediaID))));
                    xdoc.Root.Add(new XElement("NM", lstMediaID.Where(w => string.Compare(w.MediaType, "NM", true) == 0).Select(s => new XElement("ID", s.MediaID))));
                    xdoc.Root.Add(new XElement("SM", lstMediaID.Where(w => string.Compare(w.MediaType, "SM", true) == 0).Select(s => new XElement("ID", s.MediaID))));
                    xdoc.Root.Add(new XElement("PQ", lstMediaID.Where(w => string.Compare(w.MediaType, "PQ", true) == 0).Select(s => new XElement("ID", s.MediaID))));

                    string res = iQService_DiscoveryLogic.InsertExportDiscovery(sessionInformation.CustomerGUID, p_SelectAll, null, xdoc.ToString());

                    return Json(new
                    {
                        itemCount = lstMediaID.Count(),
                        isSuccess = !string.IsNullOrEmpty(res) && Convert.ToInt32(res) > 0 ? true : false
                    });
                }
                else
                {
                    if (fromDate.HasValue && toDate.HasValue)
                    {
                        fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                        toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                        toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                    }

                    XElement advanceSearchSettings = null;
                    if (advanceSearch != null)
                    {
                        if (advanceSearch.TVSettings != null
                            || advanceSearch.NewsSettings != null
                            || advanceSearch.SociaMediaSettings != null
                            || advanceSearch.ProQuestSettings != null)
                        {
                            advanceSearchSettings = XElement.Parse(Shared.Utility.CommonFunctions.SerializeToXml(advanceSearch));
                        }
                    }

                    XDocument xdoc = new XDocument(new XElement("SearchCriteria"));
                    xdoc.Root.Add(new XElement("SearchTerm", searchTermArray[searchTermIndex]));
                    xdoc.Root.Add(new XElement("FromDate", fromDate));
                    xdoc.Root.Add(new XElement("ToDate", toDate));
                    xdoc.Root.Add(new XElement("MediaType", medium));
                    if (advanceSearchSettings != null)
                    {
                        xdoc.Root.Add(advanceSearchSettings);
                    }
                    string res = iQService_DiscoveryLogic.InsertExportDiscovery(sessionInformation.CustomerGUID, p_SelectAll, xdoc.ToString(), null);

                    return Json(new
                    {
                        isSuccess = !string.IsNullOrEmpty(res) && Convert.ToInt32(res) > 0 ? true : false
                    });
                }

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
                TempData.Keep("DiscoveryTempData");
            }
        }

        [HttpPost]
        public JsonResult GetTopics(string index, string searchTerm, string searchName, DateTime? fromDate, DateTime? toDate, string medium, DiscoveryAdvanceSearchModel advanceSearch)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                if (medium == "Social Media")
                {
                    medium = "SocialMedia";
                }

                if (fromDate.HasValue && toDate.HasValue)
                {
                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);

                    toDate = toDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }
                else
                {
                    fromDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    toDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59).AddSeconds(59);

                    fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);
                    toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate);
                }
                
                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;

                //variables used to set the medium for the discovery topics drilldown popup
                string topicPopupMedium = "";
                string topicPopupMediumName = "";

                DiscoveryLogic discLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);

                if ((sessionInformation.Isv4NM && String.IsNullOrEmpty(medium)) || medium == CommonFunctions.CategoryType.NM.ToString())
                {
                    topicPopupMedium = CommonFunctions.CategoryType.NM.ToString();
                    topicPopupMediumName = CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.NM);

                    string pmgSearchUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), fromDate, toDate);
                    DiscoveryTopicsResponse dtrNM = new DiscoveryTopicsResponse() { IsValid = false };
                    lstTask.Add(Task<DiscoveryTopicsResponse>.Factory.StartNew((object obj) => discLogic.GetNewsTopics(searchTerm, fromDate.Value, toDate.Value, pmgSearchUrl, advanceSearch.NewsSettings, token, dtrNM), dtrNM));
                }
                else if ((sessionInformation.Isv4TV && String.IsNullOrEmpty(medium)) || medium == CommonFunctions.CategoryType.TV.ToString())
                {
                    topicPopupMedium = CommonFunctions.CategoryType.TV.ToString();
                    topicPopupMediumName = CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.TV);

                    string pmgSearchUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.TV.ToString(), fromDate, toDate);
                    DiscoveryTopicsResponse dtrTV = new DiscoveryTopicsResponse() { IsValid = false };
                    lstTask.Add(Task<DiscoveryTopicsResponse>.Factory.StartNew((object obj) => discLogic.GetTVTopics(searchTerm, fromDate.Value, toDate.Value, pmgSearchUrl, advanceSearch.TVSettings, token, dtrTV), dtrTV));

               }
                else if ((sessionInformation.Isv4SM && String.IsNullOrEmpty(medium)) || medium == CommonFunctions.CategoryType.SocialMedia.ToString() || medium == CommonFunctions.CategoryType.Forum.ToString() || medium == CommonFunctions.CategoryType.Blog.ToString())
                {
                    //use social media by default
                    if (medium == CommonFunctions.CategoryType.Forum.ToString()) topicPopupMediumName = CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.Forum);
                    else if (medium == CommonFunctions.CategoryType.Blog.ToString()) topicPopupMediumName = CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.Blog);
                    else topicPopupMediumName = CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.SocialMedia);
                    topicPopupMedium = topicPopupMediumName;

                    string pmgSearchUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), fromDate, toDate);
                    DiscoveryTopicsResponse dtrSM = new DiscoveryTopicsResponse() { IsValid = false };
                    lstTask.Add(Task<DiscoveryTopicsResponse>.Factory.StartNew((object obj) => discLogic.GetSocialMediaTopics(searchTerm, medium, fromDate.Value, toDate.Value, pmgSearchUrl, advanceSearch.SociaMediaSettings, token, dtrSM), dtrSM));
                }
                else if ((sessionInformation.Isv4PQ && String.IsNullOrEmpty(medium)) || medium == CommonFunctions.CategoryType.PQ.ToString())
                {
                    topicPopupMedium = CommonFunctions.CategoryType.PQ.ToString();
                    topicPopupMediumName = CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.PQ);

                    string pmgSearchUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.PQ.ToString(), fromDate, toDate);
                    DiscoveryTopicsResponse dtrPQ = new DiscoveryTopicsResponse() { IsValid = false };
                    lstTask.Add(Task<DiscoveryTopicsResponse>.Factory.StartNew((object obj) => discLogic.GetProQuestTopics(searchTerm, fromDate.Value, toDate.Value, pmgSearchUrl, advanceSearch.ProQuestSettings, token, dtrPQ), dtrPQ));
                } 
                
                try
                {
                    Task.WaitAll(lstTask.ToArray(), token);
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

                List<Topics> lstTopics = new List<Topics>();
                foreach (var tsk in lstTask)
                {
                    if (((DiscoveryTopicsResponse)tsk.AsyncState).IsValid)
                    {
                        lstTopics.AddRange(((Task<DiscoveryTopicsResponse>)tsk).Result.ListTopics);
                    }
                }

                Dictionary<string, object> dictModel = new Dictionary<string, object>();
                Dictionary<string, string> mediaTypes = new Dictionary<string, string>();

                // Get list of media types the user can select from
                if (String.IsNullOrEmpty(medium))
                {
                    if (sessionInformation.Isv4NM) { mediaTypes.Add(CommonFunctions.MediaType.NM.ToString(), CommonFunctions.GetEnumDescription(CommonFunctions.MediaType.NM)); }
                    if (sessionInformation.Isv4TV) { mediaTypes.Add(CommonFunctions.MediaType.TV.ToString(), CommonFunctions.GetEnumDescription(CommonFunctions.MediaType.TV)); }
                    if (sessionInformation.Isv4SM) 
                    {
                        mediaTypes.Add(CommonFunctions.CategoryType.SocialMedia.ToString(), CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.SocialMedia));
                        mediaTypes.Add(CommonFunctions.CategoryType.Forum.ToString(), CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.Forum));
                        mediaTypes.Add(CommonFunctions.CategoryType.Blog.ToString(), CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.Blog)); 
                    }
                    if (sessionInformation.Isv4PQ) { mediaTypes.Add(CommonFunctions.MediaType.PQ.ToString(), CommonFunctions.GetEnumDescription(CommonFunctions.MediaType.PQ)); }

                }
                else
                {
                    mediaTypes.Add(medium, CommonFunctions.GetEnumDescription((CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), medium)));
                }

                dictModel.Add("MediaTypes", mediaTypes);
                dictModel.Add("Index", index);

                string jsonResult = String.Empty;
                if (lstTopics.Count > 0)
                {
                    lstTopics = lstTopics.OrderByDescending(s => s.Frequency).Take(10).ToList();
                    HighColumnChartModel barChart = discLogic.HighChartsTopicsBarChart(lstTopics, searchTerm, searchName, topicPopupMedium, topicPopupMediumName);
                    jsonResult = CommonFunctions.SearializeJson(barChart);
                }

                var json = new
                {
                    isSuccess = true
                    ,HTML = RenderPartialToString(PATH_DiscoveryTopicsPartialView, dictModel)
                    ,chartHTML = jsonResult
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
        }        

        #endregion

        #region Add to Feeds

        public JsonResult AddToFeeds()
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                Request.InputStream.Position = 0;

                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                string[] searchTermArray = ((JArray)dictParams["searchTermArray"]).Select(x => x.ToString()).ToArray();
                short searchTermIndex = Convert.ToInt16(dictParams["searchTermIndex"]);
                long p_SearchRequestID = Convert.ToInt64(dictParams["p_SearchRequestID"]);
                JArray p_MediaID = (JArray)dictParams["p_MediaID"];
                List<MediaIDClass> mediaIDs = new List<MediaIDClass>();

                foreach (var mediaID in p_MediaID.Children())
                {
                    MediaIDClass mediaIDClass = new MediaIDClass();

                    var itemProperties = mediaID.Children<JProperty>();
                    mediaIDClass.MediaID = Convert.ToString(itemProperties.FirstOrDefault(x => x.Name == "MediaID").Value);
                    mediaIDClass.MediaType = Convert.ToString(itemProperties.FirstOrDefault(x => x.Name == "MediaType").Value);
                    mediaIDs.Add(mediaIDClass);
                }

                XDocument xdoc = new XDocument(new XElement("MediaIDList", new XElement("SearchTerm", searchTermArray[searchTermIndex])));
                xdoc.Root.Add(new XElement("TV", mediaIDs.Where(w => string.Compare(w.MediaType, "TV", true) == 0).Select(s => new XElement("ID", s.MediaID))));
                xdoc.Root.Add(new XElement("NM", mediaIDs.Where(w => string.Compare(w.MediaType, "NM", true) == 0).Select(s => new XElement("ID", s.MediaID))));
                xdoc.Root.Add(new XElement("SM", mediaIDs.Where(w => string.Compare(w.MediaType, "SM", true) == 0).Select(s => new XElement("ID", s.MediaID))));
                xdoc.Root.Add(new XElement("PQ", mediaIDs.Where(w => string.Compare(w.MediaType, "PQ", true) == 0).Select(s => new XElement("ID", s.MediaID))));

                IQDiscovery_ToFeedsLogic iQDiscovery_ToFeedsLogic = (IQDiscovery_ToFeedsLogic)LogicFactory.GetLogic(LogicType.IQDiscovery_ToFeeds);
                string res = iQDiscovery_ToFeedsLogic.InsertIQDiscovery_ToFeeds(sessionInformation.ClientGUID, sessionInformation.CustomerGUID, xdoc.ToString(), p_SearchRequestID, null);
                if (!string.IsNullOrEmpty(res) && Convert.ToInt32(res) > 0)
                {
                    return Json(new
                    {
                        isSuccess = true,
                        message = p_MediaID.Count()
                    });
                }
                else
                {
                    return Json(new
                    {
                        isSuccess = false,
                    });
                }

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
                TempData.Keep("DiscoveryTempData");
            }

        }

        #endregion

        #endregion

        #region Methods


        #region PMG Search

        #region Chart

        public bool SearchMedia(string[] searchTerm, string[] searchName, string[] searchID, DateTime? fromdate, DateTime? toDate, string medium, DiscoveryAdvanceSearchModel[] advanceSearches, string[] advanceSearchIDs, bool useAdvancedSearchDefault, bool IsInsertFromRecordID, Guid p_ClientGUID, out string availableData)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                availableData = string.Empty;
                //notAvailableData = string.Empty;
                /*SearchTV(searchTerm, date, medium, tvMarket);
                SearchSocialMedia(searchTerm, date, medium, tvMarket);*/

                List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData(p_ClientGUID)).lstDiscoveryResultRecordTrack;// TempData["DiscoveryResultRecordTrack"];
                discoveryTempData = GetTempData(p_ClientGUID);

                string TVFromRecordID = null;
                string NMFromRecordID = null;
                string SMFromRecordID = null;

                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;

                Int16 searchTermCount = 0;
                Dictionary<String, object> dictSSPData = new Dictionary<string, object>();
                if (!discoveryTempData.IsAllDmaAllowed ||
                        !discoveryTempData.IsAllClassAllowed ||
                        !discoveryTempData.IsAllStationAllowed)
                {
                    dictSSPData.Add("IQ_Dma", discoveryTempData.DmaList);
                    dictSSPData.Add("IQ_Station", discoveryTempData.StationList);
                    dictSSPData.Add("Station_Affil", discoveryTempData.AffiliateList);
                    dictSSPData.Add("IQ_Class", discoveryTempData.ClassList);
                    //dictSSPData = GetSSPDataOld(p_ClientGUID);
                }
                else
                {
                    dictSSPData.Add("IQ_Dma", new List<IQ_Dma>());
                    dictSSPData.Add("IQ_Class", new List<IQ_Class>());
                    dictSSPData.Add("IQ_Station", new List<IQ_Station>());
                    dictSSPData.Add("Station_Affil", new List<Station_Affil>());
                }

                dictSSPData.Add("IQ_Region", discoveryTempData.RegionList ?? new List<IQ_Region>());
                dictSSPData.Add("IQ_Country", discoveryTempData.CountryList ?? new List<IQ_Country>());

                for (var i = 0; i < searchTerm.Length; i++)
                {
                    searchTermCount++;
                    if (searchTermCount <= 5)
                    {
                        var term = searchTerm[i].ToString();
                        var termName = searchName[i].ToString();
                        var termID = searchID[i].ToString();

                        DiscoveryAdvanceSearchModel advanceSearch = null;
                        string advancedSearchID = useAdvancedSearchDefault ? "-2" : termID;
                        if (advanceSearches != null && advanceSearchIDs != null)
                        {
                            for (int x = 0; x < advanceSearches.Length; x++)
                            {
                                if (advanceSearchIDs[x].ToString() == advancedSearchID)
                                {
                                    advanceSearch = advanceSearches[x];
                                    break;
                                }
                            }
                        }
                        if (advanceSearch == null) advanceSearch = new DiscoveryAdvanceSearchModel();

                        DiscoveryResultRecordTrack discoveryResultRecordTrack = (DiscoveryResultRecordTrack)(lstDiscoveryResultRecordTrack != null ?
                            lstDiscoveryResultRecordTrack.Where(w => w.SearchTerm.Equals(term)).SingleOrDefault() : null);

                        /*if (discoveryResultRecordTrack != null)
                        {
                            TVFromRecordID = discoveryResultRecordTrack.TVFromRecordID;
                            NMFromRecordID = discoveryResultRecordTrack.NMFromRecordID;
                            SMFromRecordID = discoveryResultRecordTrack.SMFromRecordID;
                        }*/

                        //TV Task
                        if (sessionInformation.Isv4TV && (discoveryResultRecordTrack.IsTVValid) && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.TV.ToString()))
                        {
                            bool isAllDmaAllowed = discoveryTempData.IsAllDmaAllowed;
                            bool isAllClassAllowed = discoveryTempData.IsAllClassAllowed;
                            bool isAllStationAllowed = discoveryTempData.IsAllStationAllowed;
                            List<int> listTVRegion = new List<int>(discoveryTempData.IQTVRegion);

                            DiscoverySearchResponse dsrTV = new DiscoverySearchResponse() { SearchTerm = term, SearchName = termName, MediumType = CommonFunctions.CategoryType.TV.ToString(), IsValid = false };

                            lstTask.Add(Task<DiscoverySearchResponse>.Factory.StartNew((object obj) => SearchTV(term, termName, fromdate, toDate, medium,
                                                                                    isAllDmaAllowed, (List<IQ_Dma>)dictSSPData["IQ_Dma"],
                                                                                    isAllClassAllowed, (List<IQ_Class>)dictSSPData["IQ_Class"],
                                                                                    isAllStationAllowed, (List<IQ_Station>)dictSSPData["IQ_Station"], 
                                                                                    (List<Station_Affil>)dictSSPData["Station_Affil"], 
                                                                                    (List<IQ_Region>)dictSSPData["IQ_Region"], 
                                                                                    (List<IQ_Country>)dictSSPData["IQ_Country"], 
                                                                                    listTVRegion, advanceSearch.TVSettings, token, dsrTV), dsrTV));
                        }
                        
                        // News Task
                        if (sessionInformation.Isv4NM && (discoveryResultRecordTrack.IsNMValid) && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.NM.ToString()))
                        {
                            List<short> lstIQLicense = new List<short>(discoveryTempData.lstIQLicense);

                            DiscoverySearchResponse dsrNews = new DiscoverySearchResponse() { SearchTerm = term, SearchName = termName, MediumType = CommonFunctions.CategoryType.NM.ToString(), IsValid = false };

                            lstTask.Add(Task<DiscoverySearchResponse>.Factory.StartNew((object obj) => SearchNews(term, termName, fromdate, toDate, medium, NMFromRecordID, lstIQLicense, advanceSearch.NewsSettings, token, dsrNews), dsrNews));
                        }

                        // SM Task
                        if (sessionInformation.Isv4SM && (discoveryResultRecordTrack.IsSMValid) &&
                            (string.IsNullOrWhiteSpace(medium) || medium == "Social Media" ||
                                medium == CommonFunctions.CategoryType.Blog.ToString() ||
                            medium == CommonFunctions.CategoryType.Forum.ToString()))
                        {
                            DiscoverySearchResponse dsrSM = new DiscoverySearchResponse() { SearchTerm = term, SearchName = termName, MediumType = CommonFunctions.CategoryType.SocialMedia.ToString(), IsValid = false };

                            lstTask.Add(Task<SocialMediaFacet>.Factory.StartNew((object obj) => SearchSocialMedia(term, termName, fromdate, toDate, medium, SMFromRecordID, advanceSearch.SociaMediaSettings, token, dsrSM), dsrSM));
                        }

                        // PQ Task
                        if (sessionInformation.Isv4PQ && (discoveryResultRecordTrack.IsPQValid) && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.PQ.ToString()))
                        {
                            DiscoverySearchResponse dsrPQ = new DiscoverySearchResponse() { SearchTerm = term, SearchName = termName, MediumType = CommonFunctions.CategoryType.PQ.ToString(), IsValid = false };

                            lstTask.Add(Task<DiscoverySearchResponse>.Factory.StartNew((object obj) => SearchProQuest(term, termName, fromdate, toDate, medium, null /*TODO: Figure out FromRecordID*/, advanceSearch.ProQuestSettings, token, dsrPQ), dsrPQ));
                        }
                    
                    }

                }

                //Task[] searchTasks = (Task[])lstTask.ToArray();
                try
                {

                    Task.WaitAll(lstTask.ToArray(), Convert.ToInt32(ConfigurationManager.AppSettings["MaxRequestDuration"]), token);
                    tokenSource.Cancel();

                }
                catch (AggregateException ex)
                {
                    foreach (var item in ex.InnerExceptions)
                    {
                        if (item is System.Security.Authentication.AuthenticationException)
                        {
                            Log4NetLogger.Error("Manual Exception from AggregateException");
                        }
                    }
                    Log4NetLogger.Error("AggregateException " + ex.ToString());
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Error("Exception " + ex.ToString());
                }

                StringBuilder strngNotAvailableData = new StringBuilder();
                StringBuilder strngAvailableData = new StringBuilder();


                //strnAvailableData.Append(" " + CommonFunctions.GetEnumDescription(((DiscoveryMediaResult)tsk.AsyncState).MediumType) + " on search " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + ", ");


                foreach (var tsk in lstTask)
                {
                    if (((DiscoverySearchResponse)tsk.AsyncState).IsValid)
                    {
                        DiscoverySearchResponse discoverySearchResponse = null;
                        if (((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.SocialMedia.ToString()
                            || ((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Blog.ToString()
                            || ((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Forum.ToString())
                        {
                            discoverySearchResponse = ((Task<SocialMediaFacet>)tsk).Result.DateData;
                            discoverySearchResponse.IsValid = true;
                            if (!string.IsNullOrEmpty(discoverySearchResponse.SearchTermParent)) discoverySearchResponse.SearchTerm = discoverySearchResponse.SearchTermParent;

                            lstSMResponseFeedClass.Add(((Task<SocialMediaFacet>)tsk).Result.FeedClassData);
                            lstMainDiscoverySearchResponse.Add(discoverySearchResponse);

                            if (((Task<SocialMediaFacet>)tsk).Result.DateData.ListRecordData.Count > 0)
                            {
                                if (string.IsNullOrWhiteSpace(medium))
                                {
                                    strngAvailableData.Append(" Social Media on search " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + ", ");
                                    strngAvailableData.Append(" Blog on search " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + ", ");
                                    strngAvailableData.Append(" Forum on search " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + ", ");
                                }
                                else
                                {
                                    strngAvailableData.Append(" " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(medium.Replace(" ", string.Empty))) + " on search " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " ,");
                                }
                            }

                        }
                        else
                        {
                            discoverySearchResponse = ((Task<DiscoverySearchResponse>)tsk).Result;
                            discoverySearchResponse.IsValid = true;
                            if (!string.IsNullOrEmpty(discoverySearchResponse.SearchTermParent)) discoverySearchResponse.SearchTerm = discoverySearchResponse.SearchTermParent;

                            lstMainDiscoverySearchResponse.Add(discoverySearchResponse);
                            if (((Task<DiscoverySearchResponse>)tsk).Result.ListRecordData.Count > 0)
                            {
                                strngAvailableData.Append(" " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(((DiscoverySearchResponse)tsk.AsyncState).MediumType.Replace(" ", string.Empty))) + " on search " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " ,");
                            }
                        }
                    }
                    else
                    {
                        DiscoverySearchResponse discoverySearchResponse = null;
                        if (((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.SocialMedia.ToString()
                            || ((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Blog.ToString()
                            || ((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Forum.ToString())
                        {
                            discoverySearchResponse = new DiscoverySearchResponse();
                            discoverySearchResponse.ListRecordData = new List<RecordData>();
                            discoverySearchResponse.SearchTerm = ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm;
                            discoverySearchResponse.SearchName = ((DiscoverySearchResponse)tsk.AsyncState).SearchName;
                            discoverySearchResponse.IsValid = false;
                            discoverySearchResponse.ListTopResults = new List<TopResults>();
                            discoverySearchResponse.MediumType = ((DiscoverySearchResponse)tsk.AsyncState).MediumType;
                            discoverySearchResponse.ListRecordData = new List<RecordData>();

                            lstSMResponseFeedClass.Add(discoverySearchResponse);

                            if (string.IsNullOrWhiteSpace(medium))
                            {
                                strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + "Social Media" + ", ");
                                strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + "Blog" + ", ");
                                strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + "Forum" + ", ");
                            }
                            else
                            {
                                strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(medium.Replace(" ", string.Empty))) + ", ");
                            }
                        }
                        else
                        {
                            discoverySearchResponse = new DiscoverySearchResponse();
                            discoverySearchResponse.ListRecordData = new List<RecordData>();
                            discoverySearchResponse.SearchTerm = ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm;
                            discoverySearchResponse.SearchName = ((DiscoverySearchResponse)tsk.AsyncState).SearchName;
                            discoverySearchResponse.MediumType = ((DiscoverySearchResponse)tsk.AsyncState).MediumType;
                            discoverySearchResponse.IsValid = false;
                            discoverySearchResponse.ListTopResults = new List<TopResults>();
                            strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(((DiscoverySearchResponse)tsk.AsyncState).MediumType.Replace(" ", string.Empty))) + ", ");
                        }
                        if (!string.IsNullOrEmpty(discoverySearchResponse.SearchTermParent)) discoverySearchResponse.SearchTerm = discoverySearchResponse.SearchTermParent;
                        lstMainDiscoverySearchResponse.Add(discoverySearchResponse);
                    }


                    #region Commented
                    /*if (tsk.IsCompleted)
                    {

                        if (((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.SocialMedia.ToString()
                            || ((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Blog.ToString()
                            || ((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Forum.ToString())
                        {

                            DiscoverySearchResponse discoverySearchResponse = null;
                            if (!tsk.IsFaulted)
                            {
                                discoverySearchResponse = ((Task<SocialMediaFacet>)tsk).Result.DateData;
                                discoverySearchResponse.IsValid = true;

                                lstSMResponseFeedClass.Add(((Task<SocialMediaFacet>)tsk).Result.FeedClassData);
                                //lstMainDiscoverySearchResponse.Add(discoverySearchResponse);
                            }
                            else
                            {
                                discoverySearchResponse = new DiscoverySearchResponse();
                                discoverySearchResponse.ListRecordData = new List<RecordData>();
                                discoverySearchResponse.SearchTerm = ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm;
                                discoverySearchResponse.IsValid = false;
                                discoverySearchResponse.MediumType = ((DiscoverySearchResponse)tsk.AsyncState).MediumType;
                                discoverySearchResponse.ListRecordData = new List<RecordData>();

                                lstSMResponseFeedClass.Add(discoverySearchResponse);

                                if (string.IsNullOrWhiteSpace(medium))
                                {
                                    strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + "Social Media" + ", ");
                                    strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + "Blog" + ", ");
                                    strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + "Forum" + ", ");
                                }
                                else
                                {
                                    strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(medium.Replace(" ", string.Empty))) + ", ");
                                }
                            }
                            lstMainDiscoverySearchResponse.Add(discoverySearchResponse);

                        }
                        else
                        {
                            DiscoverySearchResponse discoverySearchResponse = null;
                            if (!tsk.IsFaulted)
                            {
                                discoverySearchResponse = ((Task<DiscoverySearchResponse>)tsk).Result;
                                discoverySearchResponse.IsValid = true;
                            }
                            else
                            {
                                discoverySearchResponse = new DiscoverySearchResponse();
                                discoverySearchResponse.ListRecordData = new List<RecordData>();
                                discoverySearchResponse.SearchTerm = ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm;
                                discoverySearchResponse.MediumType = ((DiscoverySearchResponse)tsk.AsyncState).MediumType;
                                discoverySearchResponse.IsValid = false;
                                strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(((DiscoverySearchResponse)tsk.AsyncState).MediumType.Replace(" ", string.Empty))) + ", ");
                            }

                            lstMainDiscoverySearchResponse.Add(discoverySearchResponse);
                            //lstMainDiscoverySearchResponse.Add(((Task<DiscoverySearchResponse>)tsk).Result);
                        }

                    }
                    else
                    {

                        if (((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.SocialMedia.ToString()
                       || ((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Blog.ToString()
                       || ((DiscoverySearchResponse)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.Forum.ToString())
                        {
                            DiscoverySearchResponse discoverySearchResponse = ((Task<SocialMediaFacet>)tsk).Result.DateData;
                            discoverySearchResponse.IsValid = false;
                            discoverySearchResponse.SearchTerm = ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm;
                            discoverySearchResponse.MediumType = ((DiscoverySearchResponse)tsk.AsyncState).MediumType;
                            lstMainDiscoverySearchResponse.Add(discoverySearchResponse);
                        }
                        else
                        {
                            DiscoverySearchResponse discoverySearchResponse = ((Task<DiscoverySearchResponse>)tsk).Result;
                            discoverySearchResponse.IsValid = false;
                            discoverySearchResponse.SearchTerm = ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm;
                            discoverySearchResponse.MediumType = ((DiscoverySearchResponse)tsk.AsyncState).MediumType;
                            lstMainDiscoverySearchResponse.Add(discoverySearchResponse);
                        }
                        strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(((DiscoverySearchResponse)tsk.AsyncState).MediumType.Replace(" ", string.Empty))) + ", ");

                    }*/
                    #endregion
                }

                //notAvailableData = Convert.ToString(strngNotAvailableData);
                availableData = Convert.ToString(strngAvailableData);
                sw.Stop();
                Log4NetLogger.Info(string.Format("time taken to fetch chart data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));

                return false;
            }
            catch (Exception ex)
            {
                availableData = "";
                UtilityLogic.WriteException(ex, null, " || ClientGUID: " + p_ClientGUID);
                return true;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        private Task<DiscoverySearchResponse> CheckForException(Task<DiscoverySearchResponse> task)
        {
            if (task.Exception != null)
            {
                Log4NetLogger.Error("CheckForException : " + task.Exception.ToString());
            }
            return task;
        }


        public DiscoverySearchResponse SearchTV(string srcTerm, string srcTermName, DateTime? fromdate, DateTime? toDate, string medium,
                                              bool IsAllDmaAllowed, List<IQ_Dma> listDma,
                                                bool IsAllClassAllowed, List<IQ_Class> listClass,
                                                bool IsAllStationAllowed, List<IQ_Station> listStation, List<Station_Affil> listAffiliate, List<IQ_Region> listRegion, List<IQ_Country> listCountry, List<int> IQTVRegion, TVAdvanceSearchSettings tvSearchSettings, CancellationToken token, DiscoverySearchResponse dsrTV)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.TV.ToString(), fromdate, toDate);
                DiscoverySearchResponse discoverySearchResponseTV = discoveryLogic.SearchTV(srcTerm, srcTermName, fromdate, toDate, medium, IsAllDmaAllowed, listDma, IsAllClassAllowed, listClass, IsAllStationAllowed, listStation, listAffiliate, listRegion, listCountry, out lstTVMarket, pmgUrl, IQTVRegion, tvSearchSettings);

                if (!token.IsCancellationRequested)
                {
                    dsrTV.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                sw.Stop();

                Log4NetLogger.Info(string.Format("time taken to fetch TV Chart data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));

                return discoverySearchResponseTV;
            }
            catch (Exception ex)
            {
                dsrTV.IsValid = false;
                //throw ex;
                /*if (!token.IsCancellationRequested)
                {
                    Log4NetLogger.Error("SearchTV - IsCancellationRequested false");
                    throw;
                }
                else
                {
                    Log4NetLogger.Error("SearchTV - IsCancellationRequested true");
                }*/
            }
            return new DiscoverySearchResponse();
        }

        public DiscoverySearchResponse SearchNews(string srcTerm, string srcTermName, DateTime? fromdate, DateTime? toDate, string medium, string p_fromRecordID, List<short> lstIQLicense, NewsAdvanceSearchSettings newsSearchSettings, CancellationToken token, DiscoverySearchResponse dsrNews)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), fromdate, toDate);
                DiscoverySearchResponse discoverySearchResponseNews = discoveryLogic.SearchNews(srcTerm, srcTermName, fromdate, toDate, medium, p_fromRecordID, pmgUrl, lstIQLicense, newsSearchSettings);

                if (!token.IsCancellationRequested)
                {
                    dsrNews.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                sw.Stop();

                Log4NetLogger.Info(string.Format("time taken to fetch News Chart data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));

                return discoverySearchResponseNews;
            }
            catch (Exception ex)
            {
                dsrNews.IsValid = false;
                //throw ex;
                /*if (token.IsCancellationRequested)
                {
                    Log4NetLogger.Error("SearchNews - IsCancellationRequested false");
                    token.ThrowIfCancellationRequested();
                }
                else
                {
                    Log4NetLogger.Error("SearchNews - IsCancellationRequested true");
                    throw ex;
                }*/

            }
            return new DiscoverySearchResponse();

        }

        public SocialMediaFacet SearchSocialMedia(string srcTerm, string srcTermName, DateTime? fromdate, DateTime? toDate, string medium, string p_fromRecordID, SociaMediaAdvanceSearchSettings socialMediaSearchSettings, CancellationToken token, DiscoverySearchResponse dsrSM)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), fromdate, toDate);
                SocialMediaFacet socialMediaFacet = discoveryLogic.SearchSocialMedia(srcTerm, srcTermName, fromdate, toDate, medium, p_fromRecordID, pmgUrl, socialMediaSearchSettings);

                if (!token.IsCancellationRequested)
                {
                    dsrSM.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                sw.Stop();

                Log4NetLogger.Info(string.Format("time taken to fetch SM Chart data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));

                return socialMediaFacet;
            }
            catch (Exception ex)
            {
                dsrSM.IsValid = false;
                //throw ex;
                /*if (!token.IsCancellationRequested)
                {
                    Log4NetLogger.Error("SearchSocialMedia - IsCancellationRequested false");
                    throw;
                }
                else
                {
                    Log4NetLogger.Error("SearchSocialMedia - IsCancellationRequested true");
                }*/
            }
            return new SocialMediaFacet();
        }

        public DiscoverySearchResponse SearchProQuest(string srcTerm, string srcTermName, DateTime? fromdate, DateTime? toDate, string medium, string p_fromRecordID, ProQuestAdvanceSearchSettings proQuestSearchSettings, CancellationToken token, DiscoverySearchResponse dsrPQ)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.PQ.ToString(), fromdate, toDate);
                DiscoverySearchResponse discoverySearchResponse = discoveryLogic.SearchProQuest(srcTerm, srcTermName, fromdate, toDate, medium, p_fromRecordID, pmgUrl, proQuestSearchSettings);

                if (!token.IsCancellationRequested)
                {
                    dsrPQ.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                sw.Stop();

                Log4NetLogger.Info(string.Format("time taken to fetch ProQuest Chart data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));

                return discoverySearchResponse;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Info(ex.Message + " :: " + ex.StackTrace);
                dsrPQ.IsValid = false;
            }
            return new DiscoverySearchResponse();
        }

        #endregion

        #region Result

        public List<DiscoveryMediaResult> SearchTVResult(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, bool isAsc,
                                                                bool IsAllDmaAllowed, List<IQ_Dma> listDma,
                                                            bool IsAllClassAllowed, List<IQ_Class> listClass,
                                                            bool IsAllStationAllowed, List<IQ_Station> listStation, List<Station_Affil> listAffiliate, List<IQ_Region> listRegion, List<IQ_Country> listCountry, Int32 p_PageSize, List<int> IQTVRegion, IQClient_ThresholdValueModel thresholdValueModel, TVAdvanceSearchSettings tvSearchSettings, CancellationToken token, DiscoveryMediaResult dmrTV)//Int64 startRecordID,
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.TV.ToString(), fromdate, toDate);
                List<DiscoveryMediaResult> lstDiscoveryMediaResult = discoveryLogic.SearchTVResult(sessionInformation.CustomerKey, srcTerm, fromdate, toDate, medium, isAsc, sessionInformation.ClientGUID,
                                                                                IsAllDmaAllowed, listDma,
                                                                                IsAllClassAllowed, listClass,
                                                                                IsAllStationAllowed, listStation, listAffiliate, listRegion, listCountry, thresholdValueModel, p_PageSize, out lstTVMarket, pmgUrl, IQTVRegion, tvSearchSettings);// startRecordID,


                if (!token.IsCancellationRequested)
                {
                    dmrTV.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                if (lstDiscoveryMediaResult.Count <= 0)
                {
                    DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                    discoveryMediaResult.IsValid = true;
                    discoveryMediaResult.SearchTerm = srcTerm;
                    discoveryMediaResult.MediumType = CommonFunctions.CategoryType.TV;
                    discoveryMediaResult.IncludeInResult = false;
                    lstDiscoveryMediaResult.Add(discoveryMediaResult);
                }
                sw.Stop();

                Log4NetLogger.Info(string.Format("time taken to fetch TV Results data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));

                return lstDiscoveryMediaResult;
            }
            catch (Exception)
            {
                dmrTV.IsValid = false;
                /*if (!token.IsCancellationRequested)
                {
                    Log4NetLogger.Error("SearchTVResult - IsCancellationRequested false");
                    throw;
                }
                else
                {
                    Log4NetLogger.Error("SearchTVResult - IsCancellationRequested true");
                }*/
            }
            DiscoveryMediaResult discoveryMediaResultFail = new DiscoveryMediaResult();
            discoveryMediaResultFail.IsValid = false;
            discoveryMediaResultFail.SearchTerm = srcTerm;
            discoveryMediaResultFail.MediumType = CommonFunctions.CategoryType.TV;
            discoveryMediaResultFail.IncludeInResult = false;

            List<DiscoveryMediaResult> lstDiscoveryMediaResultFail = new List<DiscoveryMediaResult>();
            lstDiscoveryMediaResultFail.Add(discoveryMediaResultFail);
            return lstDiscoveryMediaResultFail;
        }

        public List<DiscoveryMediaResult> SearchNewsResult(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, bool isAsc, Int32 p_PageSize, List<Int16> lstIQLicense, IQClient_ThresholdValueModel thresholdValueModel, NewsAdvanceSearchSettings newsSearchSettings, CancellationToken token, DiscoveryMediaResult dmrNews)//Int64 startRecordID, string fromRecordID,
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), fromdate, toDate);
                List<DiscoveryMediaResult> lstDiscoveryMediaResult = discoveryLogic.SearchNewsResult(sessionInformation.CustomerKey, srcTerm, fromdate, toDate, medium, isAsc, sessionInformation.ClientGUID, thresholdValueModel, p_PageSize, pmgUrl, lstIQLicense, newsSearchSettings);//startRecordID, fromRecordID,

                if (!token.IsCancellationRequested)
                {
                    dmrNews.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                if (lstDiscoveryMediaResult.Count <= 0)
                {
                    DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                    discoveryMediaResult.IsValid = true;
                    discoveryMediaResult.SearchTerm = srcTerm;
                    discoveryMediaResult.MediumType = CommonFunctions.CategoryType.NM;
                    discoveryMediaResult.IncludeInResult = false;
                    lstDiscoveryMediaResult.Add(discoveryMediaResult);
                }

                sw.Stop();

                Log4NetLogger.Info(string.Format("time taken to fetch TV Results data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));

                return lstDiscoveryMediaResult;
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                dmrNews.IsValid = false;
                /* if (!token.IsCancellationRequested)
                 {
                     Log4NetLogger.Error("SearchNewsResult - IsCancellationRequested false");
                     throw;
                 }
                 else
                 {
                     Log4NetLogger.Error("SearchNewsResult - IsCancellationRequested true");
                 }*/
            }
            DiscoveryMediaResult discoveryMediaResultFail = new DiscoveryMediaResult();
            discoveryMediaResultFail.IsValid = false;
            discoveryMediaResultFail.SearchTerm = srcTerm;
            discoveryMediaResultFail.MediumType = CommonFunctions.CategoryType.NM;
            discoveryMediaResultFail.IncludeInResult = false;

            List<DiscoveryMediaResult> lstDiscoveryMediaResultFail = new List<DiscoveryMediaResult>();
            lstDiscoveryMediaResultFail.Add(discoveryMediaResultFail);

            return lstDiscoveryMediaResultFail;
        }

        public List<DiscoveryMediaResult> SearchSocialMediaResult(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, bool isAsc, Int32 p_PageSize, IQClient_ThresholdValueModel thresholdValueModel, SociaMediaAdvanceSearchSettings socialMediaSearchSettings, CancellationToken token, DiscoveryMediaResult dmrSM)//Int64 startRecordID, string fromRecordID,
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), fromdate, toDate);
                List<DiscoveryMediaResult> lstDiscoveryMediaResult = discoveryLogic.SearchSocialMediaResult(sessionInformation.CustomerKey, srcTerm, fromdate, toDate, medium, isAsc, sessionInformation.ClientGUID, thresholdValueModel, p_PageSize, pmgUrl, socialMediaSearchSettings);//startRecordID, fromRecordID

                if (!token.IsCancellationRequested)
                {
                    dmrSM.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                if (lstDiscoveryMediaResult.Count <= 0)
                {
                    DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                    discoveryMediaResult.IsValid = true;
                    discoveryMediaResult.SearchTerm = srcTerm;
                    discoveryMediaResult.MediumType = CommonFunctions.CategoryType.SocialMedia;
                    discoveryMediaResult.IncludeInResult = false;
                    lstDiscoveryMediaResult.Add(discoveryMediaResult);


                }

                sw.Stop();

                Log4NetLogger.Info(string.Format("time taken to fetch TV Results data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));
                return lstDiscoveryMediaResult;
            }
            catch (Exception)
            {
                dmrSM.IsValid = false;
                /*if (!token.IsCancellationRequested)
                {
                    Log4NetLogger.Error("SearchSocialMediaResult - IsCancellationRequested false");
                    throw;
                }
                else
                {
                    Log4NetLogger.Error("SearchSocialMediaResult - IsCancellationRequested true");
                }*/
            }

            DiscoveryMediaResult discoveryMediaResultFail = new DiscoveryMediaResult();
            discoveryMediaResultFail.IsValid = false;
            discoveryMediaResultFail.SearchTerm = srcTerm;
            discoveryMediaResultFail.MediumType = CommonFunctions.CategoryType.SocialMedia;
            discoveryMediaResultFail.IncludeInResult = false;

            List<DiscoveryMediaResult> lstDiscoveryMediaResultFail = new List<DiscoveryMediaResult>();
            lstDiscoveryMediaResultFail.Add(discoveryMediaResultFail);

            return lstDiscoveryMediaResultFail;
        }

        public List<DiscoveryMediaResult> SearchProQuestResult(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, bool isAsc, Int32 p_PageSize, IQClient_ThresholdValueModel thresholdValueModel, ProQuestAdvanceSearchSettings proQuestSearchSettings, CancellationToken token, DiscoveryMediaResult dmrPQ)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
                string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.PQ.ToString(), fromdate, toDate);
                List<DiscoveryMediaResult> lstDiscoveryMediaResult = discoveryLogic.SearchProQuestResult(sessionInformation.CustomerKey, srcTerm, fromdate, toDate, medium, isAsc, thresholdValueModel, p_PageSize, pmgUrl, proQuestSearchSettings);

                if (!token.IsCancellationRequested)
                {
                    dmrPQ.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }

                if (lstDiscoveryMediaResult.Count <= 0)
                {
                    DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                    discoveryMediaResult.IsValid = true;
                    discoveryMediaResult.SearchTerm = srcTerm;
                    discoveryMediaResult.MediumType = CommonFunctions.CategoryType.PQ;
                    discoveryMediaResult.IncludeInResult = false;
                    lstDiscoveryMediaResult.Add(discoveryMediaResult);
                }

                sw.Stop();

                Log4NetLogger.Info(string.Format("time taken to fetch ProQuest Results data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));
                return lstDiscoveryMediaResult;
            }
            catch (Exception)
            {
                dmrPQ.IsValid = false;
            }

            DiscoveryMediaResult discoveryMediaResultFail = new DiscoveryMediaResult();
            discoveryMediaResultFail.IsValid = false;
            discoveryMediaResultFail.SearchTerm = srcTerm;
            discoveryMediaResultFail.MediumType = CommonFunctions.CategoryType.PQ;
            discoveryMediaResultFail.IncludeInResult = false;

            List<DiscoveryMediaResult> lstDiscoveryMediaResultFail = new List<DiscoveryMediaResult>();
            lstDiscoveryMediaResultFail.Add(discoveryMediaResultFail);

            return lstDiscoveryMediaResultFail;
        }

        #endregion

        #endregion

        #region Chart Binding

        public string ColumnChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            try
            {
                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
                string jsonResult = discoveryLogic.HighChartsColumnChart(lstDiscoverySearchResponse);
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        public string LineChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse, Boolean isHourData, decimal clientGmtOffset, decimal clientDstOffset)
        {

            DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
            string jsonLineChartResult = discoveryLogic.HighChartsLineChart(lstDiscoverySearchResponse, isHourData, clientGmtOffset, clientDstOffset);
            return jsonLineChartResult;
        }

        public List<PieChartResponse> LineChartByMedium(List<DiscoverySearchResponse> lstDiscoverySearchResponse, string[] searchTerm, string medium, bool isHourData, decimal clientGmtOffset, decimal clientDstOffset)
        {
            DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
            List<PieChartResponse> lstPieChartResponse = discoveryLogic.HighChartsLineChartByMedium(lstDiscoverySearchResponse, lstSMResponseFeedClass, searchTerm, medium, isHourData, clientGmtOffset, clientDstOffset,
                                                                                                        sessionInformation.Isv4TV, sessionInformation.Isv4NM, sessionInformation.Isv4SM, sessionInformation.Isv4PQ);
            return lstPieChartResponse;
        }

        public Dictionary<string, object> PieChartBySearchTerm(List<DiscoverySearchResponse> lstDiscoverySearchResponse, string[] searchTerm, string[] searchName, string medium)
        {

            DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
            Dictionary<string, object> dictPieChartResponse = discoveryLogic.HighChartsPieChartBySearchTerm(lstDiscoverySearchResponse, lstSMResponseFeedClass, searchTerm, searchName, medium,
                                                                    sessionInformation.Isv4TV, sessionInformation.Isv4NM, sessionInformation.Isv4SM, sessionInformation.Isv4PQ);
            return dictPieChartResponse;
        }

        public List<PieChartResponse> PieChartByMedium(List<DiscoverySearchResponse> lstDiscoverySearchResponse, string[] searchTerm, string[] searchName, string medium)
        {

            DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
            List<PieChartResponse> lstPieChartReponse = discoveryLogic.HighChartsPieChartByMedium(lstDiscoverySearchResponse, lstSMResponseFeedClass, searchTerm, searchName, medium,
                                                            sessionInformation.Isv4TV, sessionInformation.Isv4NM,
                                                             sessionInformation.Isv4SM, sessionInformation.Isv4PQ);
            return lstPieChartReponse;
        }

        public IEnumerable GetDateFilter(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
            return discoveryLogic.GetDateFilter(lstDiscoverySearchResponse);
        }

        public IEnumerable GetMediumFilter(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
            return discoveryLogic.GetMediumFilter(lstDiscoverySearchResponse, lstSMResponseFeedClass);
        }
        #endregion

        #region Validation

        public bool SearchTermValidation(string[] searchTerm)
        {
            try
            {

                var searchTermCount = searchTerm.GroupBy(g => g).Select(s => new { count = s.Count() }).Where(w => w.count > 1);
                if (searchTermCount != null && searchTermCount.Count() > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("error while validating search term", ex);
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
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

        private bool HasMoreResults(Int64 TotalRecords, Int64 shownRecords)
        {
            if (shownRecords < TotalRecords && shownRecords < Convert.ToInt32(ConfigurationManager.AppSettings["MaxDiscoveryItemLimit"]))
                return true;
            else
                return false;
        }

        public string GetAvailableDataString(string dataString)
        {
            /*dataString = string.IsNullOrWhiteSpace(dataString) ? dataString : "Wow, we are searching on lot of data! We returned results for " + dataString
                                                + " Would you like to continue your search  ";*/

            dataString = string.IsNullOrWhiteSpace(dataString) ? dataString : ConfigSettings.Settings.DiscoveryMessage;

            return dataString;
        }

        public string GetNoDataDataString(string dataString)
        {
            dataString = ConfigSettings.Settings.DiscoveryNoDataAvailable;
            return dataString;
        }

        public string GetResultMessage(string sTerm, string dataAvailableList, string medium, out bool anyDataAvailable)
        {
            // Check if Result Data contains any Valid Request and Set its message accordingly
            anyDataAvailable = false;
            List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData(sessionInformation.ClientGUID)).lstDiscoveryResultRecordTrack;
            DiscoveryResultRecordTrack drrt = lstDiscoveryResultRecordTrack.Where(w => String.Compare(w.SearchTerm, sTerm, true) == 0).FirstOrDefault();
            bool isAnyDataAvailable = false;
            bool isAllDataAvailable = true;
            if (drrt != null)
            {
                if (string.IsNullOrWhiteSpace(medium))
                {
                    if (drrt.IsTVValid && drrt.IsNMValid && drrt.IsSMValid && drrt.IsPQValid)
                    {
                        isAnyDataAvailable = true;
                    }
                    else
                    {
                        isAllDataAvailable = false;
                        if ((sessionInformation.Isv4TV && drrt.IsTVValid) || (sessionInformation.Isv4NM && drrt.IsNMValid)
                            || (sessionInformation.Isv4SM && drrt.IsSMValid) || (sessionInformation.Isv4PQ && drrt.IsPQValid))
                        {
                            isAnyDataAvailable = true;
                        }
                    }
                }
                else
                {
                    if (medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        if (drrt.IsTVValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }

                    else if (medium == CommonFunctions.CategoryType.NM.ToString())
                    {
                        if (drrt.IsNMValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }
                    else if (medium == "Social Media" || medium == CommonFunctions.CategoryType.Blog.ToString() || medium == CommonFunctions.CategoryType.Forum.ToString())
                    {
                        if (drrt.IsSMValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }
                    else if (medium == CommonFunctions.CategoryType.PQ.ToString())
                    {
                        if (drrt.IsPQValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }
                }
            }

            if (isAllDataAvailable)
            {
                anyDataAvailable = true;
                return string.Empty;
            }
            else
            {
                if (isAnyDataAvailable)
                {
                    anyDataAvailable = true;
                    return GetAvailableDataString(dataAvailableList);
                }
                else
                {
                    anyDataAvailable = false;
                    return GetNoDataDataString(dataAvailableList);
                }
            }
        }

        public string GetChartMessage(string dataAvailableList, string medium)
        {
            List<DiscoveryResultRecordTrack> lstrecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData(sessionInformation.ClientGUID)).lstDiscoveryResultRecordTrack;
            bool isAllDataAvailable = true;
            bool isAnyDataAvailable = false;
            foreach (DiscoveryResultRecordTrack drrt in lstrecordTrack)
            {
                if (string.IsNullOrWhiteSpace(medium))
                {
                    if (drrt.IsTVValid && drrt.IsNMValid && drrt.IsSMValid && drrt.IsPQValid)
                    {
                        isAnyDataAvailable = true;
                    }
                    else
                    {
                        isAllDataAvailable = false;
                        if ((sessionInformation.Isv4TV && drrt.IsTVValid) || (sessionInformation.Isv4NM && drrt.IsNMValid)
                           || (sessionInformation.Isv4SM && drrt.IsSMValid) || (sessionInformation.Isv4PQ && drrt.IsPQValid))
                        {
                            isAnyDataAvailable = true;
                        }
                    }
                }
                else
                {
                    if (medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        if (drrt.IsTVValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }

                    else if (medium == CommonFunctions.CategoryType.NM.ToString())
                    {
                        if (drrt.IsNMValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }
                    else if (medium == "Social Media" ||
                                medium == CommonFunctions.CategoryType.Blog.ToString() ||
                            medium == CommonFunctions.CategoryType.Forum.ToString())
                    {
                        if (drrt.IsSMValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }
                    else if (medium == CommonFunctions.CategoryType.PQ.ToString())
                    {
                        if (drrt.IsPQValid)
                        {
                            isAnyDataAvailable = true;
                        }
                        else
                        {
                            isAllDataAvailable = false;
                        }
                    }
                }
            }
            if (isAllDataAvailable)
            {
                return string.Empty;
            }
            else
            {
                if (isAnyDataAvailable)
                {
                    return GetAvailableDataString(dataAvailableList);
                }
                else
                {
                    return GetNoDataDataString(dataAvailableList);
                }
            }
        }

        private List<string> GetHTMLWithCSSIncluded(string p_HTML, string p_FromDate, string p_ToDate, bool p_IsEmail, string[] p_SearchTerm)
        {
            StringBuilder cssData = new StringBuilder();



            StreamReader strmReader = new StreamReader(Server.MapPath("~/css/Feed.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();

            strmReader = new StreamReader(Server.MapPath("~/css/bootstrap.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();

            cssData.Append(" .divtopres {width: 50%;} \n .pieChartChks {margin-top: 59px;} \n body {background:none;}");


            p_HTML = "<html><head><style type=\"text/css\">" + Convert.ToString(cssData) + "</style></head>" + "<body>" + "<img src=\"../../" + ConfigurationManager.AppSettings["IQMediaEmailLogo"] + "\" alt='IQMedia Logo'/>" + p_HTML + "</body></html>";
            
            HtmlDocument doc = new HtmlDocument();
            doc.Load(new StringReader(p_HTML));
            doc.OptionOutputOriginalCase = true;

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//svg"))
            {
                if (link.ParentNode != null && link.ParentNode.Name == "div" && link.ParentNode.Attributes["style"] != null && !link.ParentNode.Attributes["style"].Value.Contains("float"))
                {
                    link.ParentNode.Attributes["style"].Value = link.ParentNode.Attributes["style"].Value + ";float:left";
                }
            }

            if (p_IsEmail)
            {
                doc.DocumentNode.SelectSingleNode("//body").SetAttributeValue("style", "width:1000px;");
            }

            StringBuilder finalHTML = new StringBuilder();
            string divColumnChart = string.Empty;
            if (doc.DocumentNode.SelectSingleNode("//div[@id='divColumnChart']") != null)
            {
                divColumnChart = doc.DocumentNode.SelectSingleNode("//div[@id='divColumnChart']").InnerHtml;
            }

            string divLineChart = string.Empty;
            if (doc.DocumentNode.SelectSingleNode("//div[@id='divLineChart']") != null)
            {
                divLineChart = doc.DocumentNode.SelectSingleNode("//div[@id='divLineChart']").InnerHtml;
            }

            string divPieChartSearchTerm = string.Empty;
            if (p_SearchTerm.Length > 1 && doc.DocumentNode.SelectSingleNode("//div[@id='divPieChartSearchTerm']") != null)
            {
                divPieChartSearchTerm = doc.DocumentNode.SelectSingleNode("//div[@id='divPieChartSearchTerm']").InnerHtml;
            }

            List<string> lstPieChart = new List<string>();
            for (int i = 0; i < p_SearchTerm.Length; i++)
            {
                if (p_IsEmail)
                {
                    finalHTML = new StringBuilder();
                }

                finalHTML.Append("<div style=\"float:left;clear:both;width:100%;\">");
                if (!p_IsEmail && i > 0)
                {
                    finalHTML.Append("<div class=\"pagebreak\" >&nbsp;</div>");
                }
                else
                {
                    finalHTML.Append("<div class=\"clear\"><br/><br/></div>");
                }
                finalHTML.Append("<div class=\"searchTermData\">Search Term : " + p_SearchTerm[i] + "</div>");
                finalHTML.Append(divColumnChart);
                finalHTML.Append(divLineChart);
                finalHTML.Append(divPieChartSearchTerm);
                finalHTML.Append("</div>");
                finalHTML.Append("<div style=\"float:left;clear:both;width:100%;\">");
                if (doc.DocumentNode.SelectSingleNode("//div[@id='divPieChart_Child_" + i + "']") != null)
                {
                    // Tabs don't render properly, so replace them with a label for whichever tab is displayed
                    HtmlNode divTopResultsHeader = doc.DocumentNode.SelectSingleNode("//div[@id='divTopResultsHeader_" + i + "']");
                    HtmlNode divTopResultsData = doc.DocumentNode.SelectSingleNode("//div[@id='divTopResultsData_" + i + "']");
                    if (divTopResultsData.Attributes.Contains("class") && divTopResultsData.Attributes["class"].Value.Contains("displayNone"))
                    {
                        divTopResultsHeader.InnerHtml = "<b>Most Relevant Topics</b>";
                    }
                    else
                    {
                        divTopResultsHeader.InnerHtml = "<b>Most Relevant Coverage</b>";
                    }

                    finalHTML.Append(doc.DocumentNode.SelectSingleNode("//div[@id='divPieChart_Child_" + i + "']").InnerHtml);
                }
                finalHTML.Append("</div>");

                if (p_IsEmail)
                {
                    string logoImage = i == 0 ? "<img src=\"../../" + ConfigurationManager.AppSettings["IQMediaEmailLogo"] + "\" alt='IQMedia Logo'/><br/>" : String.Empty;
                    string finalHTMLData = "<html><head><style type=\"text/css\">.pagebreak {display: block;clear: both; page-break-after: always;} .searchTermData {font-weight:bold} " + Convert.ToString(cssData) + "</style></head>" + "<body>" + logoImage + Convert.ToString(finalHTML) + "</body></html>";

                    lstPieChart.Add(finalHTMLData);
                }
            }

            if (!p_IsEmail)
            {
                string finalHTMLData = "<html><head><style type=\"text/css\">.pagebreak {display: block;clear: both; page-break-after: always;} .searchTermData {font-weight:bold} " + Convert.ToString(cssData) + "</style></head>" + "<body>" + "<img src=\"../../" + ConfigurationManager.AppSettings["IQMediaEmailLogo"] + "\" alt='IQMedia Logo'/><br/>" + Convert.ToString(finalHTML) + "</body></html>";
                lstPieChart.Add(finalHTMLData);
            }

            return lstPieChart;
        }

        #endregion

        #region SSP
        public Dictionary<string, object> GetSSPDataOld(Guid p_ClientGUID)
        {
            try
            {
                bool isAllDmaAllowed = false;
                bool isAllClassAllowed = false;
                bool isAllStationAllowed = false;

                discoveryTempData = GetTempData(p_ClientGUID);

                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                Dictionary<string, object> dictSSP = sspLogic.GetSSPDataWithStationByClientGUIDOld(p_ClientGUID, out isAllDmaAllowed, out isAllClassAllowed, out isAllStationAllowed, discoveryTempData.IQTVRegion);
                discoveryTempData.IsAllDmaAllowed = isAllDmaAllowed;
                discoveryTempData.IsAllClassAllowed = isAllClassAllowed;
                discoveryTempData.IsAllStationAllowed = isAllStationAllowed;
                List<IQ_Region> IQTVRegionList = (List<IQ_Region>)dictSSP["IQ_Region"];
                List<IQ_Country> IQTVCountryList = (List<IQ_Country>)dictSSP["IQ_Country"];
                discoveryTempData.IQTVRegion = IQTVRegionList.Select(r => r.Num).ToList();
                discoveryTempData.IQTVCountry = IQTVCountryList.Select(c => c.Num).ToList();

                SetTempData(discoveryTempData);
                //TempData.Keep();

                return dictSSP;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }
        #endregion

        #region UpdateRecordTracking

        protected List<DiscoveryMediaResult> UpdateRecordTracking(List<DiscoveryMediaResult> lstDiscoveryMediaResult, string sTerm, Int32 p_PageSize, bool isAsc)
        {


            try
            {


                Log4NetLogger.Info("Update Record Tracking Start");
                Log4NetLogger.Info("Total Discovery Results : " + (lstDiscoveryMediaResult == null ? "null" : lstDiscoveryMediaResult.Count.ToString()));

                List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData(sessionInformation.ClientGUID)).lstDiscoveryResultRecordTrack;

                //string[] distinctSearchTerm = lstDiscoveryResultRecordTrack.Select(s => s.SearchTerm).Distinct().ToArray();

                bool IsTVValid = true;
                bool IsNMValid = true;
                bool IsSMValid = true;
                bool IsPQValid = true;

                //foreach (string sTerm in distinctSearchTerm)
                //{
                DiscoveryResultRecordTrack discoveryResultRecordTrack = lstDiscoveryResultRecordTrack.Where(w => w.SearchTerm.Equals(sTerm)).FirstOrDefault();

                if (discoveryResultRecordTrack != null)
                {

                    Log4NetLogger.Info("Fetched current discovery track from tempdata by search term succeed.");

                    if (lstMainDiscoverySearchResponse != null)
                    {

                        Log4NetLogger.Info("update total records for each medium type");

                        discoveryResultRecordTrack.SearchTerm = sTerm;

                        if (lstMainDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TV.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault() != null)
                        {

                            discoveryResultRecordTrack.TVRecordTotal = lstMainDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TV.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().TotalResult;

                        }

                        if (lstMainDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault() != null)
                        {

                            discoveryResultRecordTrack.NMRecordTotal = lstMainDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().TotalResult;


                            //discoveryResultRecordTrack.NMFromRecordID = lstMainDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().FromRecordID;

                        }

                        if (lstMainDiscoverySearchResponse.Where(w => (w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString())
                                                                                        || w.MediumType.Equals(CommonFunctions.CategoryType.Blog.ToString())
                                                                                        || w.MediumType.Equals(CommonFunctions.CategoryType.Forum.ToString())) && w.SearchTerm.Equals(sTerm)).FirstOrDefault() != null)
                        {

                            discoveryResultRecordTrack.SMRecordTotal = lstMainDiscoverySearchResponse.Where(w => (w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString())
                                                                                        || w.MediumType.Equals(CommonFunctions.CategoryType.Blog.ToString())
                                                                                        || w.MediumType.Equals(CommonFunctions.CategoryType.Forum.ToString())) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().TotalResult;



                            /* discoveryResultRecordTrack.SMFromRecordID = lstMainDiscoverySearchResponse.Where(w => (w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString())
                                                                                         || w.MediumType.Equals(CommonFunctions.CategoryType.Blog.ToString())
                                                                                         || w.MediumType.Equals(CommonFunctions.CategoryType.Forum.ToString()))
                                                                                         && (w.SearchTerm.Equals(sTerm)))
                                                                                         .FirstOrDefault().FromRecordID;*/


                        }

                        if (lstMainDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.PQ.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault() != null)
                        {
                            discoveryResultRecordTrack.PQRecordTotal = lstMainDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.PQ.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().TotalResult;
                        }


                        Log4NetLogger.Info("update total records of search term");
                        discoveryResultRecordTrack.TotalRecords = lstMainDiscoverySearchResponse.Where(w => w.SearchTerm.Equals(sTerm)).Sum(s => s.TotalResult);

                        IsTVValid = (!discoveryResultRecordTrack.IsTVValid ? false : true);
                        IsNMValid = (!discoveryResultRecordTrack.IsNMValid ? false : true);
                        IsSMValid = (!discoveryResultRecordTrack.IsSMValid ? false : true);
                        IsPQValid = (!discoveryResultRecordTrack.IsPQValid ? false : true);

                        /*IsTVValid = discoveryResultRecordTrack.TVRecordTotal > 0 ? true : false;
                        IsNMValid = discoveryResultRecordTrack.NMRecordTotal > 0 ? true : false;
                        IsSMValid = discoveryResultRecordTrack.SMRecordTotal > 0 ? true : false;*/
                    }
                }
                else
                {
                    Log4NetLogger.Info("fetching of discovery track from tempdata by search term failed.");
                    discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                    discoveryResultRecordTrack.SearchTerm = sTerm;
                    lstDiscoveryResultRecordTrack.Add(discoveryResultRecordTrack);


                }

                if (lstDiscoveryMediaResult != null)
                {

                    Log4NetLogger.Info("update shown records for each medium type");
                    
                    /* 
                     * NOT CURRENTLY USED - MUST BE REWRITTEN BEFORE IT IS FUNCTIONAL
                     * 
                    List<DiscoveryMediaResult> lstFinal = new List<DiscoveryMediaResult>();
                    if (isAsc)
                    {
                        lstFinal = lstDiscoveryMediaResult.OrderBy(o => o.Date).Where(w => w.IsValid && w.IncludeInResult).Take(Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryPageSize"])).ToList();
                    }
                    else
                    {
                        lstFinal = lstDiscoveryMediaResult.OrderByDescending(o => o.Date).Where(w => w.IsValid && w.IncludeInResult).Take(Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryPageSize"])).ToList();
                    }

                    discoveryResultRecordTrack.TVRecordShownNum += lstFinal.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.TV.ToString()) && w.IsValid && w.IncludeInResult).Count();



                    discoveryResultRecordTrack.NMRecordShownNum += lstFinal.Where(w => String.Compare(w.SearchTerm, sTerm, true) == 0 && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.NM.ToString()) && w.IsValid && w.IncludeInResult).Count();


                    discoveryResultRecordTrack.SMRecordShownNum += lstFinal.Where(w => w.SearchTerm.Equals(sTerm) && (Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.SocialMedia.ToString()) ||
                                                                              Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Blog.ToString()) ||
                                                                               Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Forum.ToString()))
                                                                                  && w.IsValid && w.IncludeInResult).Count();

                    discoveryResultRecordTrack.PQRecordShownNum += lstFinal.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.PQ.ToString()) && w.IsValid && w.IncludeInResult).Count();
                    */

                    #region Commented Total Record Minus Logic
                    /*if (discoveryResultRecordTrack.IsTVValid && !IsTVValid)
                    {

                        discoveryResultRecordTrack.TotalRecords -=
                            (discoveryResultRecordTrack.TVRecordTotal == null ? 0 : discoveryResultRecordTrack.TVRecordTotal)
                        - discoveryResultRecordTrack.TVRecordShownNum;

                    }

                    if (discoveryResultRecordTrack.IsNMValid && !IsNMValid)
                    {

                        discoveryResultRecordTrack.TotalRecords -=
                            (discoveryResultRecordTrack.NMRecordTotal == null ? 0 : discoveryResultRecordTrack.NMRecordTotal)
                        - discoveryResultRecordTrack.NMRecordShownNum;

                    }

                    if (discoveryResultRecordTrack.IsSMValid && !IsSMValid)
                    {

                        discoveryResultRecordTrack.TotalRecords -=
                            (discoveryResultRecordTrack.SMRecordTotal == null ? 0 : discoveryResultRecordTrack.SMRecordTotal)
                        - discoveryResultRecordTrack.SMRecordShownNum;

                    }*/

                    #endregion
                }

                if (!sessionInformation.Isv4TV)
                {
                    Log4NetLogger.Info("TV Discovery Found ? =" + (lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.TV.ToString())).FirstOrDefault() != null ? "true" : "false"));
                }

                if (!sessionInformation.Isv4NM)
                {
                    Log4NetLogger.Info("NM Discovery Found ? =" + (lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.NM.ToString())).FirstOrDefault() != null ? "true" : "false"));
                }

                if (!sessionInformation.Isv4SM)
                {
                    Log4NetLogger.Info("SM Discovery Found ? =" + (lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && (Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.SocialMedia.ToString()) ||
                                                                              Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Blog.ToString()) ||
                                                                               Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Forum.ToString()))).FirstOrDefault() != null ? "true" : "false"));
                }

                if (!sessionInformation.Isv4PQ)
                {
                    Log4NetLogger.Info("PQ Discovery Found ? =" + (lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.PQ.ToString())).FirstOrDefault() != null ? "true" : "false"));
                }


                Log4NetLogger.Info("set isvalid for each medium type");
                if (!sessionInformation.Isv4TV)
                {
                    discoveryResultRecordTrack.IsTVValid = true;
                }
                else if (discoveryResultRecordTrack.IsTVValid && lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.TV.ToString())).FirstOrDefault() != null)
                {
                    discoveryResultRecordTrack.IsTVValid = //true;
                    lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.TV.ToString())).FirstOrDefault().IsValid;
                }
                else
                {
                    discoveryResultRecordTrack.IsTVValid = false;
                }


                if (!sessionInformation.Isv4NM)
                {
                    discoveryResultRecordTrack.IsNMValid = true;
                }
                else if (discoveryResultRecordTrack.IsNMValid && lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.NM.ToString())).FirstOrDefault() != null)
                {
                    discoveryResultRecordTrack.IsNMValid = //true;
                    lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.NM.ToString())).FirstOrDefault().IsValid;
                }
                else
                {
                    discoveryResultRecordTrack.IsNMValid = false;
                }


                if (!sessionInformation.Isv4SM)
                {
                    discoveryResultRecordTrack.IsSMValid = true;
                }
                else if (discoveryResultRecordTrack.IsSMValid && lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && (Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.SocialMedia.ToString()) ||
                                                                          Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Blog.ToString()) ||
                                                                           Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Forum.ToString()))).FirstOrDefault() != null)
                {
                    discoveryResultRecordTrack.IsSMValid = //true;
                    lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && (Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.SocialMedia.ToString()) ||
                                                                          Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Blog.ToString()) ||
                                                                           Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Forum.ToString()))).FirstOrDefault().IsValid;
                }
                else
                {
                    discoveryResultRecordTrack.IsSMValid = false;
                }

                if (!sessionInformation.Isv4PQ)
                {
                    discoveryResultRecordTrack.IsPQValid = true;
                }
                else if (discoveryResultRecordTrack.IsPQValid && lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.PQ.ToString())).FirstOrDefault() != null)
                {
                    discoveryResultRecordTrack.IsPQValid = lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.PQ.ToString())).FirstOrDefault().IsValid;
                }
                else
                {
                    discoveryResultRecordTrack.IsPQValid = false;
                }

                discoveryTempData = GetTempData(sessionInformation.ClientGUID);
                discoveryTempData.lstDiscoveryResultRecordTrack = lstDiscoveryResultRecordTrack;
                SetTempData(discoveryTempData);

                Log4NetLogger.Error("Update Record Tracking End");
                if (isAsc)
                {
                    return lstDiscoveryMediaResult.OrderBy(o => o.Date).Where(w => w.IsValid && w.IncludeInResult).Take(p_PageSize).ToList(); ;
                }
                else
                {
                    return lstDiscoveryMediaResult.OrderByDescending(o => o.Date).Where(w => w.IsValid && w.IncludeInResult).Take(p_PageSize).ToList(); ;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("Update Record Tracking Error Occured", ex);
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        #endregion

        #region

        public MediaChartJsonResponse GetChartData(string[] searchTerm, string[] searchName, string[] searchID, DateTime? fromDate, DateTime? toDate, string medium, DiscoveryAdvanceSearchModel[] advanceSearches, string[] advanceSearchIDs, bool useAdvancedSearchDefault, Guid p_ClientGUID, bool IsInsertFromRecordID, decimal clientGmtOffset, decimal clientDstOffset)
        {
            MediaChartJsonResponse mediaChartJsonResponse = new MediaChartJsonResponse();
            try
            {
                if (SearchTermValidation(searchTerm))
                {
                    lstTVDiscoveryResponse = new List<DiscoverySearchResponse>();
                    lstNMDiscoveryResponse = new List<DiscoverySearchResponse>();
                    lstSMDiscoveryResponse = new List<DiscoverySearchResponse>();
                    lstSMResponseFeedClass = new List<DiscoverySearchResponse>();
                    lstPQDiscoveryResponse = new List<DiscoverySearchResponse>();

                    lstMainDiscoverySearchResponse = new List<DiscoverySearchResponse>();
                    //string dataNotAvailableList = string.Empty;
                    string dataAvailableList = string.Empty;

                    bool hasError = SearchMedia(searchTerm, searchName, searchID, fromDate, toDate, medium, advanceSearches, advanceSearchIDs, useAdvancedSearchDefault, IsInsertFromRecordID, p_ClientGUID, out dataAvailableList);

                    //dataNotAvailableList = string.IsNullOrWhiteSpace(dataNotAvailableList) ? dataNotAvailableList : "Data not available : " + dataNotAvailableList;

                    if (!hasError)
                    {
                        var columnChartData = ColumnChart(lstMainDiscoverySearchResponse);

                        bool isHourData = false;

                        /* if ((toDate.Value - fromDate.Value).TotalDays <= 1)
                         {
                             isHourData = true;
                         }*/

                        TimeSpan dateDiff = (TimeSpan)(toDate.Value - fromDate.Value);

                        if (dateDiff.Days <= 1)
                        {
                            isHourData = true;
                        }

                        Log4NetLogger.Debug("IsHourData: " + isHourData.ToString());

                        var lineChartData = LineChart(lstMainDiscoverySearchResponse, isHourData, clientGmtOffset, clientDstOffset);
                        var lineChartMediumData = LineChartByMedium(lstMainDiscoverySearchResponse, searchTerm, medium, isHourData, clientGmtOffset, clientDstOffset);

                        var pieChartSearchTermData = PieChartBySearchTerm(lstMainDiscoverySearchResponse, searchTerm, searchName, medium);
                        var pieChartMediumData = PieChartByMedium(lstMainDiscoverySearchResponse, searchTerm, searchName, medium);
                        var dateFilter = GetDateFilter(lstMainDiscoverySearchResponse);
                        var mediumFilter = GetMediumFilter(lstMainDiscoverySearchResponse);

                        pieChartMediumData = GetTopResult((List<PieChartResponse>)pieChartMediumData);
                        mediaChartJsonResponse.ColumnChartData = columnChartData;
                        mediaChartJsonResponse.LineChartData = lineChartData;
                        mediaChartJsonResponse.LineChartMediumData = lineChartMediumData;
                        mediaChartJsonResponse.PieChartMediumData = pieChartMediumData;
                        mediaChartJsonResponse.PieChartSearchTermData = pieChartSearchTermData;
                        //mediaChartJsonResponse.DataNotAvailableList = dataNotAvailableList;
                        mediaChartJsonResponse.DataAvailableList = dataAvailableList;
                        mediaChartJsonResponse.DateFilter = dateFilter;
                        mediaChartJsonResponse.MediumFilter = mediumFilter;
                        if (lstTVMarket != null)
                        {
                            mediaChartJsonResponse.TVMarket = lstTVMarket.Select(s => s).Distinct().ToList();
                        }
                        mediaChartJsonResponse.IsSearchTermValid = true;
                        mediaChartJsonResponse.IsSuccess = true;
                    }
                    else
                    {
                        mediaChartJsonResponse.IsSuccess = false;
                    }
                }
                else
                {
                    mediaChartJsonResponse.IsSuccess = true;
                    mediaChartJsonResponse.IsSearchTermValid = false;
                }

            }
            catch (Exception ex)
            {
                UtilityLogic.WriteException(ex, null, " || ClientGUID: " + p_ClientGUID);
                mediaChartJsonResponse.IsSuccess = false;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
            return mediaChartJsonResponse;
        }

        public List<DiscoveryMediaResult> SearchDiscoveryResult(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, bool isAsc, DiscoveryAdvanceSearchModel advanceSearch, bool isMoreResult, Guid p_ClientGUID, out Int64? shownRecords, out Int64? searchTermWiseTotalRecords, out string availableData)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //notAvailableData = string.Empty;
                discoveryTempData = GetTempData(p_ClientGUID);

                availableData = string.Empty;
                shownRecords = 0;
                searchTermWiseTotalRecords = 0;
                Int64 tvStartRecordID = 0;
                Int64 nmStartRecordID = 0;
                Int64 smStartRecordID = 0;
                Int64 pqStartRecordID = 0;

                string tvFromRecordID = string.Empty;
                string nmFromRecordID = string.Empty;
                string smFromRecordID = string.Empty;
                string pqFromRecordID = string.Empty;

                List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData(p_ClientGUID)).lstDiscoveryResultRecordTrack;// TempData["DiscoveryResultRecordTrack"];

                DiscoveryResultRecordTrack discoveryResultRecordTrack = lstDiscoveryResultRecordTrack.Where(w => w.SearchTerm.Equals(searchTerm)).FirstOrDefault();

                tvStartRecordID = discoveryResultRecordTrack.TVRecordShownNum;
                nmStartRecordID = discoveryResultRecordTrack.NMRecordShownNum;
                smStartRecordID = discoveryResultRecordTrack.SMRecordShownNum;
                pqStartRecordID = discoveryResultRecordTrack.PQRecordShownNum;

                //tvFromRecordID = discoveryResultRecordTrack.TVFromRecordID;
                nmFromRecordID = discoveryResultRecordTrack.NMFromRecordID;
                smFromRecordID = discoveryResultRecordTrack.SMFromRecordID;
                pqFromRecordID = discoveryResultRecordTrack.PQFromRecordID;

                DiscoveryResultRecordTrack discoveryResultRecordTrackCount = new DiscoveryResultRecordTrack();

                Dictionary<String, object> dictSSPData = new Dictionary<string, object>();
                if (!discoveryTempData.IsAllDmaAllowed || !discoveryTempData.IsAllClassAllowed || !discoveryTempData.IsAllStationAllowed)
                {
                    dictSSPData.Add("IQ_Dma", discoveryTempData.DmaList);
                    dictSSPData.Add("IQ_Station", discoveryTempData.StationList);
                    dictSSPData.Add("Station_Affil", discoveryTempData.AffiliateList);
                    dictSSPData.Add("IQ_Class", discoveryTempData.ClassList);
                    //dictSSPData = GetSSPData(p_ClientGUID);
                }
                else
                {
                    dictSSPData.Add("IQ_Dma", new List<IQ_Dma>());
                    dictSSPData.Add("IQ_Class", new List<IQ_Class>());
                    dictSSPData.Add("IQ_Station", new List<IQ_Station>());
                    dictSSPData.Add("Station_Affil", new List<Station_Affil>());
                }


                dictSSPData.Add("IQ_Region", discoveryTempData.RegionList ?? new List<IQ_Region>());
                dictSSPData.Add("IQ_Country", discoveryTempData.CountryList ?? new List<IQ_Country>());

                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                int currentPageSize = discoveryTempData.CurrentPageSize;
                IQClient_ThresholdValueModel thresholdValueModel = discoveryTempData.iQClient_ThresholdValueModel.Copy();

                //TV Task
                if (sessionInformation.Isv4TV && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.TV.ToString())) //(discoveryResultRecordTrack.IsTVValid) && 
                {
                    bool isAllDmaAllowed = discoveryTempData.IsAllDmaAllowed;
                    bool isAllClassAllowed = discoveryTempData.IsAllClassAllowed;
                    bool isAllStationAllowed = discoveryTempData.IsAllStationAllowed;
                    List<int> listTVRegion = new List<int>(discoveryTempData.IQTVRegion);

                    DiscoveryMediaResult dmrTV = new DiscoveryMediaResult() { SearchTerm = searchTerm, MediumType = CommonFunctions.CategoryType.TV, IsValid = false };

                    lstTask.Add(Task<List<DiscoveryMediaResult>>.Factory.StartNew((object obj) => SearchTVResult(searchTerm, fromDate, toDate, medium, isAsc,
                                                                                                    isAllDmaAllowed, (List<IQ_Dma>)dictSSPData["IQ_Dma"],
                                                                                    isAllClassAllowed, (List<IQ_Class>)dictSSPData["IQ_Class"],
                                                                                    isAllStationAllowed, (List<IQ_Station>)dictSSPData["IQ_Station"], (List<Station_Affil>)dictSSPData["Station_Affil"], (List<IQ_Region>)dictSSPData["IQ_Region"], (List<IQ_Country>)dictSSPData["IQ_Country"], currentPageSize, listTVRegion, thresholdValueModel, advanceSearch.TVSettings, token, dmrTV), dmrTV));//tvStartRecordID,
                }

                // News Task
                if (sessionInformation.Isv4NM && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.NM.ToString())) //&& (discoveryResultRecordTrack.IsNMValid)
                {
                    List<short> lstIQLicense = new List<short>(discoveryTempData.lstIQLicense);

                    DiscoveryMediaResult dmrNews = new DiscoveryMediaResult() { SearchTerm = searchTerm, MediumType = CommonFunctions.CategoryType.NM, IsValid = false };
                    lstTask.Add(Task<List<DiscoveryMediaResult>>.Factory.StartNew((object obj) => SearchNewsResult(searchTerm, fromDate, toDate, medium, isAsc, currentPageSize, lstIQLicense, thresholdValueModel, advanceSearch.NewsSettings, token, dmrNews), dmrNews));//nmStartRecordID, nmFromRecordID,
                }

                // SM Task
                if (sessionInformation.Isv4SM && (string.IsNullOrWhiteSpace(medium) || medium == "Social Media" ||
                        medium == CommonFunctions.CategoryType.Blog.ToString() ||
                    medium == CommonFunctions.CategoryType.Forum.ToString())) // && (discoveryResultRecordTrack.IsSMValid)
                {
                    DiscoveryMediaResult dmrSM = new DiscoveryMediaResult() { SearchTerm = searchTerm, MediumType = CommonFunctions.CategoryType.SocialMedia, IsValid = false };
                    lstTask.Add(Task<List<DiscoveryMediaResult>>.Factory.StartNew((object obj) => SearchSocialMediaResult(searchTerm, fromDate, toDate, medium, isAsc, currentPageSize, thresholdValueModel, advanceSearch.SociaMediaSettings, token, dmrSM), dmrSM));//smStartRecordID, smFromRecordID,
                }

                // ProQuest Task
                if (sessionInformation.Isv4PQ && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.PQ.ToString()))
                {
                    DiscoveryMediaResult dmrPQ = new DiscoveryMediaResult() { SearchTerm = searchTerm, MediumType = CommonFunctions.CategoryType.PQ, IsValid = false };
                    lstTask.Add(Task<List<DiscoveryMediaResult>>.Factory.StartNew((object obj) => SearchProQuestResult(searchTerm, fromDate, toDate, medium, isAsc, currentPageSize, thresholdValueModel, advanceSearch.ProQuestSettings, token, dmrPQ), dmrPQ));
                }

                List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();

                try
                {
                    Task.WaitAll(lstTask.ToArray(), Convert.ToInt32(ConfigurationManager.AppSettings["MaxRequestDuration"]), token);
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

                StringBuilder strngNotAvailableData = new StringBuilder();
                StringBuilder strnAvailableData = new StringBuilder();
                //bool isDataAvaialable = false;

                foreach (var tsk in lstTask)
                {
                    if (((DiscoveryMediaResult)tsk.AsyncState).IsValid)
                    {
                        //isDataAvaialable = true;
                        if (((DiscoveryMediaResult)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.SocialMedia)
                        {
                            if (((Task<List<DiscoveryMediaResult>>)tsk).Result.Count > 0)
                            {
                                if (string.IsNullOrWhiteSpace(medium))
                                {
                                    strnAvailableData.Append(" Social Media on search " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + ", ");
                                    strnAvailableData.Append(" Blog on search " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + ", ");
                                    strnAvailableData.Append(" Forum on search " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + ", ");
                                }
                                else
                                {
                                    strnAvailableData.Append(" " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(medium.Replace(" ", string.Empty))) + " on search " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + ", ");
                                }
                            }
                        }
                        else
                        {
                            if (((Task<List<DiscoveryMediaResult>>)tsk).Result.Count > 0)
                            {
                                strnAvailableData.Append(" " + CommonFunctions.GetEnumDescription(((DiscoveryMediaResult)tsk.AsyncState).MediumType) + " on search " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + ", ");
                            }
                        }

                        lstDiscoveryMediaResult.AddRange(((Task<List<DiscoveryMediaResult>>)tsk).Result);
                    }
                    else
                    {
                        /*if (tsk.Status == TaskStatus.Running)
                        {*/
                        DiscoveryMediaResult discoveryMediaResult = (DiscoveryMediaResult)tsk.AsyncState;
                        DiscoveryMediaResult discoveryMediaResultFail = new DiscoveryMediaResult();
                        discoveryMediaResultFail.IsValid = false;


                        discoveryMediaResultFail.SearchTerm = discoveryMediaResult.SearchTerm;
                        discoveryMediaResultFail.SearchName = discoveryMediaResult.SearchName;
                        discoveryMediaResultFail.MediumType = discoveryMediaResult.MediumType;// CommonFunctions.CategoryType.TV;
                        discoveryMediaResultFail.IncludeInResult = false;
                        lstDiscoveryMediaResult.Add(discoveryMediaResultFail);
                        /*}
                        else
                        {

                        }*/

                        //lstDiscoveryMediaResult.AddRange(((Task<List<DiscoveryMediaResult>>)tsk).Result);
                        /* if (((DiscoveryMediaResult)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.SocialMedia)
                         {
                             if (string.IsNullOrWhiteSpace(medium))
                             {
                                 strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + "Social Media" + ", ");
                                 strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + "Blog" + ", ");
                                 strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + "Forum" + ", ");
                             }
                             else
                             {
                                 strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(medium.Replace(" ", string.Empty))) + ", ");
                             }
                         }
                         else
                         {
                             strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(((DiscoveryMediaResult)tsk.AsyncState).MediumType) + ", ");
                         }*/
                    }

                    #region Commented
                    /*if (tsk.IsCompleted)
                    {
                        if (!tsk.IsFaulted)
                        {
                            lstDiscoveryMediaResult.AddRange(((Task<List<DiscoveryMediaResult>>)tsk).Result);
                        }
                        else
                        {
                            if (((DiscoveryMediaResult)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.SocialMedia)
                            {
                                if (string.IsNullOrWhiteSpace(medium))
                                {
                                    strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + "Social Media" + ", ");
                                    strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + "Blog" + ", ");
                                    strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + "Forum" + ", ");
                                }
                                else
                                {
                                    strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(medium.Replace(" ", string.Empty))) + ", ");
                                }
                            }
                            else
                            {
                                strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(((DiscoveryMediaResult)tsk.AsyncState).MediumType) + ", ");
                            }

                        }
                    }
                    else
                    {
                        try
                        {
                            if (((DiscoveryMediaResult)tsk.AsyncState).MediumType == CommonFunctions.CategoryType.SocialMedia)
                            {
                                if (string.IsNullOrWhiteSpace(medium))
                                {
                                    strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + "Social Media" + ", ");
                                    strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + "Blog" + ", ");
                                    strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + "Forum" + ", ");
                                }
                                else
                                {
                                    strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(medium.Replace(" ", string.Empty))) + ", ");
                                }
                            }
                            else
                            {
                                strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(((DiscoveryMediaResult)tsk.AsyncState).MediumType) + ", ");
                                //strngNotAvailableData.Append(" " + ((DiscoveryMediaResult)tsk.AsyncState).SearchTerm + " for " + ((DiscoveryMediaResult)tsk.AsyncState).MediumType + ", ");
                            }
                        }
                        catch (Exception)
                        {
                            // throw;
                        }
                    }*/
                    #endregion
                }

                availableData = Convert.ToString(strnAvailableData);
                //availableData = GetAvailableDataString(availableData);

                //if (!isDataAvaialable)
                //{
                //    availableData = GetNoDataDataString(availableData);
                //}
                /*notAvailableData = Convert.ToString(strngNotAvailableData);
                notAvailableData = string.IsNullOrWhiteSpace(notAvailableData) ? notAvailableData : "Data not available : " + notAvailableData;*/


                //if (isAsc)
                //{
                //    lstDiscoveryMediaResult = lstDiscoveryMediaResult.OrderBy(o => o.MediumType == CommonFunctions.CategoryType.TV ? o.LocalDateTime : o.Date).ToList();
                //}
                //else
                //{
                //    lstDiscoveryMediaResult = lstDiscoveryMediaResult.OrderByDescending(o => o.MediumType == CommonFunctions.CategoryType.TV ? o.LocalDateTime : o.Date).ToList();
                //}

                sw.Stop();

                Log4NetLogger.Info(string.Format("time taken to fetch results data {0}min {1}sec {2}mlsec ", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds));

                return lstDiscoveryMediaResult;
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }
        #endregion


        #region From Record ID and Paging Track

        public void UpdateFromRecordID(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData(sessionInformation.ClientGUID)).lstDiscoveryResultRecordTrack;// TempData["DiscoveryResultRecordTrack"];
            string[] distinctSearchTerm = lstDiscoverySearchResponse.Select(s => s.SearchTerm.Trim()).Distinct().ToArray();

            foreach (string sTerm in distinctSearchTerm)
            {
                DiscoveryResultRecordTrack discoveryResultRecordTrack = lstDiscoveryResultRecordTrack.Where(w => w.SearchTerm.Equals(sTerm)).FirstOrDefault();
                discoveryResultRecordTrack.SearchTerm = sTerm;

                if (lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TV.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault() != null)
                {
                    discoveryResultRecordTrack.TVRecordTotal = lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TV.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().TotalResult;
                    discoveryResultRecordTrack.IsTVValid = //discoveryResultRecordTrack.TVRecordTotal > 0 ? true : false;
                    lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TV.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().IsValid;
                }
                else
                {
                    if (sessionInformation.Isv4TV)
                    {
                        discoveryResultRecordTrack.IsTVValid = false;
                    }
                }

                if (lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault() != null)
                {
                    discoveryResultRecordTrack.NMRecordTotal = lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().TotalResult;
                    discoveryResultRecordTrack.IsNMValid = //discoveryResultRecordTrack.NMRecordTotal > 0 ? true : false;
                    lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().IsValid;
                    //discoveryResultRecordTrack.NMFromRecordID = lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).Select(s => s.FromRecordID).FirstOrDefault();
                }
                else
                {
                    if (sessionInformation.Isv4NM)
                    {
                        discoveryResultRecordTrack.IsNMValid = false;
                    }
                }

                if (lstDiscoverySearchResponse.Where(w => (w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Blog.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Forum.ToString())) && w.SearchTerm.Equals(sTerm)).FirstOrDefault() != null)
                {
                    discoveryResultRecordTrack.SMRecordTotal = lstDiscoverySearchResponse.Where(w => (w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Blog.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Forum.ToString())) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().TotalResult;

                    discoveryResultRecordTrack.IsSMValid = //discoveryResultRecordTrack.SMRecordTotal > 0 ? true : false;
                    lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().IsValid;
                    /*discoveryResultRecordTrack.SMFromRecordID = lstDiscoverySearchResponse.Where(w => (w.MediumType.Equals(CommonFunctions.CategoryType.SocialMedia.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Blog.ToString())
                                                                                || w.MediumType.Equals(CommonFunctions.CategoryType.Forum.ToString()))
                                                                                && (w.SearchTerm.Equals(sTerm)))
                                                                                .Select(s => s.FromRecordID).FirstOrDefault();*/
                }
                else
                {
                    if (sessionInformation.Isv4SM)
                    {
                        discoveryResultRecordTrack.IsSMValid = false;
                    }
                }

                if (lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.PQ.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault() != null)
                {
                    discoveryResultRecordTrack.PQRecordTotal = lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.PQ.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().TotalResult;
                    discoveryResultRecordTrack.IsPQValid = lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.PQ.ToString()) && w.SearchTerm.Equals(sTerm)).FirstOrDefault().IsValid;
                }
                else
                {
                    if (sessionInformation.Isv4PQ)
                    {
                        discoveryResultRecordTrack.IsPQValid = false;
                    }
                }

                discoveryResultRecordTrack.TotalRecords = lstDiscoverySearchResponse.Where(w => w.SearchTerm.Equals(sTerm)).Sum(s => s.TotalResult);

                #region Logger
                /*Log4NetLogger.Debug("into UpdateFromRecordID start");
                Log4NetLogger.Debug("Total Reocords :" + discoveryResultRecordTrack.TotalRecords);
                Log4NetLogger.Debug("NMFromRecordID : " + discoveryResultRecordTrack.NMFromRecordID);
                Log4NetLogger.Debug("SMFromRecordID : " + discoveryResultRecordTrack.SMFromRecordID);
                Log4NetLogger.Debug("into UpdateFromRecordID end");*/
                #endregion
            }

            discoveryTempData = GetTempData(sessionInformation.ClientGUID);
            discoveryTempData.lstDiscoveryResultRecordTrack = lstDiscoveryResultRecordTrack;
            SetTempData(discoveryTempData);
        }
        #endregion

        #endregion

        #region TempData

        private object GetSessionData()
        {

            if (Session["DiscoveryResultRecordTrack"] == null)
            {

                DiscoveryResultRecordTrack discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                List<DiscoveryResultRecordTrack> lst = new List<DiscoveryResultRecordTrack>();
                lst.Add(discoveryResultRecordTrack);
                Session["DiscoveryResultRecordTrack"] = lst;
            }

            return Session["DiscoveryResultRecordTrack"];

        }
        private void SetSessionData(List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack)
        {
            Session["DiscoveryResultRecordTrack"] = lstDiscoveryResultRecordTrack;
        }
        #endregion

        #region Top Results

        public List<PieChartResponse> GetTopResult(List<PieChartResponse> lstPieChartResponse)
        {
            try
            {
                List<String> distinctSearchTerm = lstMainDiscoverySearchResponse.Select(s => s.SearchTerm).Distinct().ToList();
                foreach (string sTerm in distinctSearchTerm)
                {
                    if (lstMainDiscoverySearchResponse.Where(w => string.Compare(w.SearchTerm, sTerm, true) == 0).SelectMany(s => s.ListTopResults) != null)
                    {
                        lstPieChartResponse.Where(w => string.Compare(w.SearchTerm, sTerm, true) == 0).FirstOrDefault().TopResultHtml =
                            RenderPartialToString(PATH_DiscoveryTopResultsPartialView, lstMainDiscoverySearchResponse.Where(w => string.Compare(w.SearchTerm, sTerm, true) == 0).SelectMany(s => s.ListTopResults).ToList());
                    }
                }

                return lstPieChartResponse;
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error("Error GetTopResult:  " + ex.ToString());
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        #endregion

        #region Utility

        public DiscoveryTempData GetTempData(Guid clientGuid)
        {
            if (TempData["DiscoveryTempData"] == null)
            {
                DiscoveryResultRecordTrack discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                List<DiscoveryResultRecordTrack> lst = new List<DiscoveryResultRecordTrack>();
                lst.Add(discoveryResultRecordTrack);
                discoveryTempData = new DiscoveryTempData();
                discoveryTempData.lstDiscoveryResultRecordTrack = lst;

                DiscoveryLogic discoveryLogic = (DiscoveryLogic)LogicFactory.GetLogic(LogicType.Discovery);
                DiscoveryAdvanceSearch_DropDown discoveryAdvanceSearch_DropDown = discoveryLogic.GetSSPDataWithStationByClientGUID(clientGuid.ToString());

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                IQClient_CustomSettingsModel customSettings = clientLogic.GetClientCustomSettings(sessionInformation.ClientGUID.ToString());

                FillTempData(clientGuid, discoveryAdvanceSearch_DropDown, customSettings, discoveryTempData);
            }
            else
            {
                discoveryTempData = (DiscoveryTempData)TempData["DiscoveryTempData"];
            }

            return discoveryTempData;
        }

        public void SetTempData(DiscoveryTempData p_DiscoveryTempData)
        {
            TempData["DiscoveryTempData"] = p_DiscoveryTempData;
            TempData.Keep("DiscoveryTempData");
        }

        public void FillTempData(Guid clientGuid, DiscoveryAdvanceSearch_DropDown discoveryAdvanceSearch_DropDown, IQClient_CustomSettingsModel customSettings, DiscoveryTempData discoveryTempData)
        {
            ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
            discoveryTempData.lstIQLicense = clientLogic.GetClientLicenseSettings(clientGuid);

            IQClient_ThresholdValueModel iQClient_ThresholdValueModel = GetClientThresholdValue(clientGuid);
            discoveryTempData.iQClient_ThresholdValueModel = iQClient_ThresholdValueModel;

            IQClient_CustomSettingsModel maxDiscoveryLimit = UtilityLogic.GetDiscoveryReportAndExportLimit(clientGuid);
            discoveryTempData.MaxDiscoveryReportLimit = maxDiscoveryLimit.v4MaxDiscoveryReportItems;
            discoveryTempData.MaxDiscoveryExportLimit = maxDiscoveryLimit.v4MaxDiscoveryExportItems;
            discoveryTempData.MaxDiscoveryHistory = maxDiscoveryLimit.v4MaxDiscoveryHistory;

            discoveryTempData.DefaultDiscoveryPageSize = customSettings.DefaultDiscoveryPageSize;
            discoveryTempData.IsAllDmaAllowed = discoveryAdvanceSearch_DropDown.IsAllDmaAllowed;
            discoveryTempData.IsAllClassAllowed = discoveryAdvanceSearch_DropDown.IsAllClassAllowed;
            discoveryTempData.IsAllStationAllowed = discoveryAdvanceSearch_DropDown.IsAllStationAllowed;

            discoveryTempData.IQTVCountry = discoveryAdvanceSearch_DropDown.TV_CountryList.Select(c => c.Num).ToList();
            discoveryTempData.IQTVRegion = discoveryAdvanceSearch_DropDown.TV_RegionList.Select(r => r.Num).ToList();
            discoveryTempData.DmaList = discoveryAdvanceSearch_DropDown.TV_DMAList;
            discoveryTempData.AffiliateList = discoveryAdvanceSearch_DropDown.TV_AffiliateList;
            discoveryTempData.StationList = discoveryAdvanceSearch_DropDown.TV_StationList;
            discoveryTempData.ClassList = discoveryAdvanceSearch_DropDown.TV_ClassList;
            discoveryTempData.RegionList = discoveryAdvanceSearch_DropDown.TV_RegionList;
            discoveryTempData.CountryList = discoveryAdvanceSearch_DropDown.TV_CountryList;
        }

        #endregion

        #region Report

        public JsonResult GetDiscoveryReportLimit()
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                discoveryTempData = GetTempData(sessionInformation.ClientGUID);

                return Json(new
                {
                    MaxDiscoveryReportItems = discoveryTempData.MaxDiscoveryReportLimit,
                    MaxDiscoveryExportItems = discoveryTempData.MaxDiscoveryExportLimit,
                    MaxDiscoveryHistory = discoveryTempData.MaxDiscoveryHistory,
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
                TempData.Keep("DiscoveryTempData");
            }
        }

        [HttpPost]
        public JsonResult SelectDiscoveryReport()
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                List<IQDiscovery_ReportModel> lstIQDiscovery_ReportModel = reportLogic.SelectDiscoveryReport(sessionInformation.ClientGUID);

                return Json(new
                {
                    reportList = lstIQDiscovery_ReportModel,
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
                TempData.Keep("DiscoveryTempData");
            }

        }

        [HttpPost]
        public JsonResult Insert_DiscoveryReport()
        {
            try
            {
                Request.InputStream.Position = 0;

                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                string p_Title = Convert.ToString(dictParams["p_Title"]);
                string p_Keywords = Convert.ToString(dictParams["p_Keywords"]);
                string p_Description = Convert.ToString(dictParams["p_Description"]);
                Guid p_CategoryGuid = new Guid(Convert.ToString(dictParams["p_CategoryGuid"]));
                long reportImageTemp;
                long? p_ReportImage = Int64.TryParse(Convert.ToString(dictParams["p_ReportImage"]), out reportImageTemp) ? reportImageTemp : (long?)null;
                long p_FolderID = Convert.ToInt64(dictParams["p_FolderID"]);
                JArray p_MediaID = (JArray)dictParams["p_MediaID"];
                List<MediaIDClass> mediaIDs = null;

                if (p_MediaID.Count > 0)
                {
                    mediaIDs = new List<MediaIDClass>();
                    foreach (var mediaID in p_MediaID.Children())
                    {
                        MediaIDClass mediaIDClass = new MediaIDClass();

                        var itemProperties = mediaID.Children<JProperty>();
                        mediaIDClass.MediaID = Convert.ToString(itemProperties.FirstOrDefault(x => x.Name == "MediaID").Value);
                        mediaIDClass.MediaType = Convert.ToString(itemProperties.FirstOrDefault(x => x.Name == "MediaType").Value);
                        mediaIDClass.SearchTerm = Convert.ToString(itemProperties.FirstOrDefault(x => x.Name == "SearchTerm").Value);
                        mediaIDs.Add(mediaIDClass);
                    }
                }

                Log4NetLogger.Info("Create Report Input Params are :");
                Log4NetLogger.Info("MediaID :" + (mediaIDs != null ? string.Join(",", mediaIDs.Select(a => a.MediaID)) : string.Empty));
                Log4NetLogger.Info("p_Title :" + p_Title);
                Log4NetLogger.Info("p_Keywords :" + p_Keywords);
                Log4NetLogger.Info("p_Description :" + p_Description);
                Log4NetLogger.Info("p_CategoryGuid :" + p_CategoryGuid);
                Log4NetLogger.Info("p_ReportImage :" + p_ReportImage);
                Log4NetLogger.Info("p_ReportImage :" + p_FolderID);

                string resultMessage = string.Empty;
                bool showPopup = false;
                if (mediaIDs != null && mediaIDs.Count > 0)
                {
                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                    IQDiscovery_ReportModel iQDiscovery_ReportModel = new IQDiscovery_ReportModel();
                    iQDiscovery_ReportModel.Title = p_Title.Trim();
                    iQDiscovery_ReportModel.Keywords = p_Keywords.Trim();
                    iQDiscovery_ReportModel.Description = p_Description.Trim();
                    iQDiscovery_ReportModel.CategoryGuid = p_CategoryGuid;
                    iQDiscovery_ReportModel.FolderID = p_FolderID;
                    iQDiscovery_ReportModel.CustomerGuid = sessionInformation.CustomerGUID;
                    iQDiscovery_ReportModel.ClientGuid = sessionInformation.ClientGUID;
                    iQDiscovery_ReportModel.ReportImageID = p_ReportImage;

                    Log4NetLogger.Info("set report object for insert");

                    discoveryTempData = GetTempData(sessionInformation.ClientGUID);

                    List<MediaIDClass> lstmediaIDClass = new List<MediaIDClass>();

                    foreach (MediaIDClass record in mediaIDs)
                    {

                        if (string.Compare(record.MediaType, "TV", true) == 0 || lstmediaIDClass.FirstOrDefault(a => a.MediaID.Equals(record.MediaID)) == null)
                        {
                            MediaIDClass mediaIDClass = new MediaIDClass();
                            mediaIDClass.MediaID = record.MediaID;
                            mediaIDClass.MediaType = record.MediaType;
                            mediaIDClass.SearchTerm = record.SearchTerm;
                            lstmediaIDClass.Add(mediaIDClass);
                        }
                    }

                    lstmediaIDClass = lstmediaIDClass.Take(discoveryTempData.MaxDiscoveryReportLimit.Value).ToList();

                    Log4NetLogger.Info("take items upto allowed limit only limit = " + discoveryTempData.MaxDiscoveryReportLimit.Value);

                    Log4NetLogger.Info("going to create mediaid xml");

                    XDocument xdoc = new XDocument(new XElement("MediaIds", new XElement("TV", lstmediaIDClass.Where(w => string.Compare(w.MediaType, "TV", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm))))));
                    xdoc.Root.Add(new XElement("NM", lstmediaIDClass.Where(w => string.Compare(w.MediaType, "NM", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm)))));
                    xdoc.Root.Add(new XElement("SM", lstmediaIDClass.Where(w => string.Compare(w.MediaType, "SM", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm)))));
                    xdoc.Root.Add(new XElement("PQ", lstmediaIDClass.Where(w => string.Compare(w.MediaType, "PQ", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm)))));
                    iQDiscovery_ReportModel.MediaID = xdoc;

                    Log4NetLogger.Info("going to insert report");

                    ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                    string result = reportLogic.InsertDiscoveryReport(iQDiscovery_ReportModel);

                    Log4NetLogger.Info("insert report sp executed");

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        Log4NetLogger.Info("report not saved");

                        resultMessage = ConfigSettings.Settings.ReportNotSaved;// "Report not saved.";
                        showPopup = false;
                    }
                    else if (!string.IsNullOrWhiteSpace(result) && Convert.ToInt32(result) < 0)
                    {
                        Log4NetLogger.Info("some db error occured");

                        resultMessage = ConfigSettings.Settings.ErrorOccurred;// "An error occur, please try again.";
                        showPopup = true;
                    }

                    else if (Convert.ToInt32(result) == 0)
                    {
                        Log4NetLogger.Info("report with same name exist");

                        resultMessage = ConfigSettings.Settings.ReportWithSameNameExists; //"Report with same name already exists.";
                        showPopup = true;
                    }
                    else
                    {
                        Log4NetLogger.Info("report saved");

                        resultMessage = ConfigSettings.Settings.ReportSaved;// "Report Saved Successfully";
                        showPopup = false;
                    }
                }
                else
                {
                    Log4NetLogger.Info("no items selected");

                    resultMessage = ConfigSettings.Settings.SelectOneMoreItemMessage;// "Report Saved Successfully";
                    showPopup = false;
                }

                Log4NetLogger.Info("return response");

                Log4NetLogger.Info("Discovery Create Report End");

                return Json(new
                {
                    message = resultMessage,
                    needToShowPopup = showPopup,
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
                TempData.Keep("DiscoveryTempData");
            }
            return Json(new object());
        }

        [HttpPost]
        public JsonResult AddToDiscoveryReport()
        {
            Request.InputStream.Position = 0;

            Dictionary<string, object> dictParams;

            using (var sr = new StreamReader(Request.InputStream))
            {
                dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
            }

            long p_ReportID = Convert.ToInt64(dictParams["p_ReportID"]);
            JArray p_RecordList = (JArray)dictParams["p_RecordList"];

            try
            {
                Log4NetLogger.Info("Start Add to Report Action for Report ID : " + p_ReportID);

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                string mediaID = reportLogic.SelectDiscoveryMediaIDByID(sessionInformation.ClientGUID, p_ReportID);
                discoveryTempData = GetTempData(sessionInformation.ClientGUID);

                Log4NetLogger.Info("Existing Report XML for Report ID : " + p_ReportID + " === " + mediaID);

                XDocument xDoc = XDocument.Parse(mediaID);

                List<MediaIDClass> lstmediaIDClass = new List<MediaIDClass>();

                List<string> lstMediaIDs = xDoc.Descendants("MediaIds").Descendants("ID").Select(s => s.Value).ToList();

                foreach (var record in p_RecordList.Children())
                {
                    MediaIDClass mediaIDClass = new MediaIDClass();

                    var itemProperties = record.Children<JProperty>();
                    mediaIDClass.MediaID = Convert.ToString(itemProperties.FirstOrDefault(x => x.Name == "MediaID").Value);
                    mediaIDClass.MediaType = Convert.ToString(itemProperties.FirstOrDefault(x => x.Name == "MediaType").Value);
                    mediaIDClass.SearchTerm = Convert.ToString(itemProperties.FirstOrDefault(x => x.Name == "SearchTerm").Value);

                    if (!lstMediaIDs.Contains(mediaIDClass.MediaID) && (string.Compare(mediaIDClass.MediaType, "TV", true) == 0 || lstmediaIDClass.FirstOrDefault(a => a.MediaID.Equals(mediaIDClass.MediaID)) == null))
                    {
                        lstmediaIDClass.Add(mediaIDClass);
                    }
                }

                if ((xDoc.Root.Descendants("ID").Count() + lstmediaIDClass.Count) > discoveryTempData.MaxDiscoveryReportLimit)
                {
                    lstmediaIDClass = lstmediaIDClass.Take(Convert.ToInt32(discoveryTempData.MaxDiscoveryReportLimit - xDoc.Root.Descendants("ID").Count())).ToList();
                }

                if (lstmediaIDClass.Count > 0)
                {

                    xDoc.Root.Descendants("TV").FirstOrDefault().Add(lstmediaIDClass.Where(w => string.Compare(w.MediaType, "TV", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm))));
                    xDoc.Root.Descendants("NM").FirstOrDefault().Add(lstmediaIDClass.Where(w => string.Compare(w.MediaType, "NM", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm))));
                    xDoc.Root.Descendants("SM").FirstOrDefault().Add(lstmediaIDClass.Where(w => string.Compare(w.MediaType, "SM", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm))));
                    xDoc.Root.Descendants("PQ").FirstOrDefault().Add(lstmediaIDClass.Where(w => string.Compare(w.MediaType, "PQ", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm))));

                    Log4NetLogger.Info("Updating Report XML for Report ID : " + p_ReportID + " === " + xDoc.ToString());

                    reportLogic.IQReportDiscovery_Update(xDoc.ToString(), p_ReportID, sessionInformation.ClientGUID, sessionInformation.CustomerGUID);
                }

                Log4NetLogger.Info("End Add to Report Action for Report ID : " + p_ReportID);

                return Json(new
                {
                    message = lstmediaIDClass.Count(),
                    isSuccess = true
                });

            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                Log4NetLogger.Info("End Add to Report Action with exception for Report ID : " + p_ReportID);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
            return Json(new object());
        }

        [HttpPost]
        public JsonResult AddToDiscoveryLibrary()
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                discoveryTempData = GetTempData(sessionInformation.ClientGUID);

                List<MediaIDClass> lstmediaIDClass = new List<MediaIDClass>();
                Request.InputStream.Position = 0;

                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                string p_Keywords = Convert.ToString(dictParams["p_Keywords"]);
                string p_Description = Convert.ToString(dictParams["p_Description"]);
                Guid p_CategoryGuid = new Guid(Convert.ToString(dictParams["p_CategoryGuid"]));
                JArray p_MediaID = (JArray)dictParams["p_MediaID"];

                foreach (var mediaID in p_MediaID.Children())
                {
                    MediaIDClass mediaIDClass = new MediaIDClass();

                    var itemProperties = mediaID.Children<JProperty>();
                    mediaIDClass.MediaID = Convert.ToString(itemProperties.FirstOrDefault(x => x.Name == "MediaID").Value);
                    mediaIDClass.MediaType = Convert.ToString(itemProperties.FirstOrDefault(x => x.Name == "MediaType").Value);
                    mediaIDClass.SearchTerm = Convert.ToString(itemProperties.FirstOrDefault(x => x.Name == "SearchTerm").Value);

                    if (string.Compare(mediaIDClass.MediaType, "TV", true) == 0 || lstmediaIDClass.FirstOrDefault(a => a.MediaID.Equals(mediaIDClass.MediaID)) == null)
                    {
                        lstmediaIDClass.Add(mediaIDClass);
                    }
                }

                lstmediaIDClass = lstmediaIDClass.Take(discoveryTempData.MaxDiscoveryReportLimit.Value).ToList();

                XDocument xdoc = new XDocument(new XElement("MediaIds", new XElement("TV", lstmediaIDClass.Where(w => string.Compare(w.MediaType, "TV", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm))))));
                xdoc.Root.Add(new XElement("NM", lstmediaIDClass.Where(w => string.Compare(w.MediaType, "NM", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm)))));
                xdoc.Root.Add(new XElement("SM", lstmediaIDClass.Where(w => string.Compare(w.MediaType, "SM", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm)))));
                xdoc.Root.Add(new XElement("PQ", lstmediaIDClass.Where(w => string.Compare(w.MediaType, "PQ", true) == 0).Select(s => new XElement("Media", new XElement("ID", s.MediaID), new XElement("SearchTerm", s.SearchTerm)))));

                IQDiscovery_ReportModel iQDiscovery_ReportModel = new IQDiscovery_ReportModel();
                iQDiscovery_ReportModel.Keywords = p_Keywords.Trim();
                iQDiscovery_ReportModel.Description = p_Description.Trim();
                iQDiscovery_ReportModel.CategoryGuid = p_CategoryGuid;
                iQDiscovery_ReportModel.CustomerGuid = sessionInformation.CustomerGUID;
                iQDiscovery_ReportModel.ClientGuid = sessionInformation.ClientGUID;
                iQDiscovery_ReportModel.MediaID = xdoc;

                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                reportLogic.InsertDiscoveryLibrary(iQDiscovery_ReportModel);

                return Json(new
                {
                    message = lstmediaIDClass.Count(),
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
                TempData.Keep("DiscoveryTempData");
            }
        }



        #endregion

        #region Threshold

        protected IQClient_ThresholdValueModel GetClientThresholdValue(Guid p_ClientGuid)
        {
            try
            {
                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                IQClient_ThresholdValueModel iQClient_ThresholdValueModel = clientLogic.GetClientThresholdValue(p_ClientGuid);
                return iQClient_ThresholdValueModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region iFrame

        [HttpPost]
        public JsonResult OpenIFrame()
        {

            return Json(new
            {
                isSuccess = true
            });
        }

        #endregion
    }

    public class MediaIDClass
    {
        public string MediaID { get; set; }
        public string MediaType { get; set; }
        public string SearchTerm { get; set; }
    }


}
