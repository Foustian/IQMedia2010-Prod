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

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication]
    public class Discovery2Controller : Controller
    {
        //
        // GET: /Discovery2/

        #region Public Member

        ActiveUser sessionInformation = null;
        DiscoveryTempData discoveryTempData = null;
        string PATH_DiscoveryPartialView = "~/Views/Shared/_DiscoveryResult.cshtml";
        string PATH_DiscoverySavedSearchPartialView = "~/Views/Shared/_DiscoverySavedSearch.cshtml";
        string PATH_DiscoveryTopResultsPartialView = "~/Views/Shared/_DiscoveryTopResult.cshtml";
        List<DiscoverySearchResponse> lstTVDiscoveryResponse = null;
        List<DiscoverySearchResponse> lstNMDiscoveryResponse = null;
        List<DiscoverySearchResponse> lstSMDiscoveryResponse = null;
        List<DiscoverySearchResponse> lstSMResponseFeedClass = null;
        List<DiscoverySearchResponse> lstMainDiscoverySearchResponse = null;
        List<String> lstTVMarket = null;
        object lockObject = new object();

        #endregion

        public ActionResult Index()
        {
            //SetSessionData(null);
            SetTempData(null);
            discoveryTempData = GetTempData();
            discoveryTempData.SavedSearchPage = null;
            discoveryTempData.ActiveSearch = null;
            discoveryTempData.MaxDiscoveryReportLimit = null;
            /*TempData["SavedSearchPage"] = null;
            TempData["ActiveSearch"] = null;*/

            sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
            GetSSPData(sessionInformation.ClientGUID);
            IQClient_ThresholdValueModel iQClient_ThresholdValueModel = GetClientThresholdValue(sessionInformation.ClientGUID);
            discoveryTempData.iQClient_ThresholdValueModel = iQClient_ThresholdValueModel;
            SetTempData(discoveryTempData);
            //TempData["DiscoveryResultRecordTrack"] = null;
            return View();
        }

        #region Ajax Request
        [HttpPost]
        public JsonResult MediaJsonChart(string[] searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, bool isDefaultLoad)
        {
            try
            {
                MediaChartJsonResponse mediaChartJsonResponse = new MediaChartJsonResponse();
                if (!isDefaultLoad)
                {
                    List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = new List<DiscoveryResultRecordTrack>();
                    foreach (string str in searchTerm)
                    {
                        DiscoveryResultRecordTrack discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                        discoveryResultRecordTrack.SearchTerm = str;

                        discoveryResultRecordTrack.IsNMValid = true;
                        discoveryResultRecordTrack.IsSMValid = true;
                        discoveryResultRecordTrack.IsTVValid = true;

                        discoveryResultRecordTrack.TVRecordTotal = null;
                        discoveryResultRecordTrack.NMRecordTotal = null;
                        discoveryResultRecordTrack.SMRecordTotal = null;

                        lstDiscoveryResultRecordTrack.Add(discoveryResultRecordTrack);
                    }

                    discoveryTempData = GetTempData();
                    discoveryTempData.lstDiscoveryResultRecordTrack = lstDiscoveryResultRecordTrack;
                    SetTempData(discoveryTempData);
                    //SetSessionData(lstDiscoveryResultRecordTrack);
                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                    mediaChartJsonResponse = GetChartData(searchTerm, fromDate, toDate, medium, tvMarket, sessionInformation.ClientGUID, true);
                    UpdateFromRecordID(lstMainDiscoverySearchResponse);

                    mediaChartJsonResponse.DataAvailableList = GetChartMessage(mediaChartJsonResponse.DataAvailableList, medium);
                }
                else
                {
                    discoveryTempData = GetTempData();
                    discoveryTempData.ActiveSearch = null;
                    SetTempData(discoveryTempData);

                    mediaChartJsonResponse.IsSuccess = true;
                    mediaChartJsonResponse.IsSearchTermValid = true;
                }

                return Json(new
                {
                    columnChartJson = mediaChartJsonResponse.ColumnChartData,
                    lineChartJson = mediaChartJsonResponse.LineChartData,
                    pieChartJson = mediaChartJsonResponse.PieChartMediumData,
                    //notAvailableDataChart = mediaChartJsonResponse.DataNotAvailableList,
                    availableDataChart = mediaChartJsonResponse.DataAvailableList,
                    discoveryDateFilter = mediaChartJsonResponse.DateFilter,
                    discoveryMediumFilter = mediaChartJsonResponse.MediumFilter,
                    discoveryTVMarketFilter = mediaChartJsonResponse.TVMarket,
                    isSearchTermValid = mediaChartJsonResponse.IsSearchTermValid,
                    chartTotal = lstMainDiscoverySearchResponse != null ? string.Format("{0:N0}", lstMainDiscoverySearchResponse.Sum(s => s.TotalResult)) : null,
                    isSuccess = mediaChartJsonResponse.IsSuccess
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

        public JsonResult MediaJsonResults(string[] searchTermArray, Int16 searchTermIndex, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, bool IsToggle, bool IsTabChange)
        {
            try
            {
                string searchTerm = searchTermArray[searchTermIndex];
                if (!IsToggle)
                {
                    List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = new List<DiscoveryResultRecordTrack>();
                    foreach (string str in searchTermArray)
                    {
                        DiscoveryResultRecordTrack discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                        discoveryResultRecordTrack.SearchTerm = str;
                        discoveryResultRecordTrack.IsNMValid = true;
                        discoveryResultRecordTrack.IsSMValid = true;
                        discoveryResultRecordTrack.IsTVValid = true;

                        discoveryResultRecordTrack.NMRecordTotal = null;
                        discoveryResultRecordTrack.SMRecordTotal = null;
                        discoveryResultRecordTrack.TVRecordTotal = null;

                        lstDiscoveryResultRecordTrack.Add(discoveryResultRecordTrack);
                    }

                    discoveryTempData = GetTempData();
                    discoveryTempData.lstDiscoveryResultRecordTrack = lstDiscoveryResultRecordTrack;
                    SetTempData(discoveryTempData);
                }


                discoveryTempData = GetTempData();
                discoveryTempData.CurrentPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryPageSize"]);

                Int64? shownRecords = 0;
                Int64? searchTermWiseTotalRecords = 0;
                string dataAvailableList = string.Empty;
                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();


                if (!IsToggle)
                {
                    lstTask.Add(Task<MediaChartJsonResponse>.Factory.StartNew((object obj) => GetChartData(searchTermArray, fromDate, toDate, medium, tvMarket, sessionInformation.ClientGUID, !IsToggle), new MediaChartJsonResponse() { SearchTerm = searchTerm }));
                }
                lstTask.Add(Task<List<DiscoveryMediaResult>>.Factory.StartNew((object obj) => SearchDiscoveryResult(searchTerm, fromDate, toDate, medium, tvMarket, false, sessionInformation.ClientGUID, out shownRecords, out searchTermWiseTotalRecords, out  dataAvailableList), new DiscoveryMediaResult() { SearchTerm = searchTerm, MediumType = CommonFunctions.CategoryType.TV }));

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

                lstDiscoveryMediaResult = GetGMTandDSTTime(lstDiscoveryMediaResult);

                List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrackTemp = lstDiscoveryResultRecordTrackTemp = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData()).lstDiscoveryResultRecordTrack;
                if (!IsToggle)
                {
                    UpdateFromRecordID(lstMainDiscoverySearchResponse);
                    mediaChartJsonResponse.DataAvailableList = GetChartMessage(mediaChartJsonResponse.DataAvailableList, medium);
                }

                lstDiscoveryMediaResult = UpdateRecordTracking(lstDiscoveryMediaResult, searchTerm, discoveryTempData.CurrentPageSize);

                bool anyDataAvailable = false;
                dataAvailableList = GetResultMessage(searchTerm, dataAvailableList, medium, out anyDataAvailable);

                discoveryTempData = GetTempData();
                searchTermWiseTotalRecords = lstDiscoveryResultRecordTrackTemp.Where(w => w.SearchTerm.Equals(searchTerm)).FirstOrDefault().TotalRecords;


                //shownRecords = lstDiscoveryResultRecordTrackTemp.Where(w => w.SearchTerm.Equals(searchTerm)).Sum(s => s.TVRecordShownNum + s.NMRecordShownNum + s.SMRecordShownNum);
                shownRecords = lstDiscoveryMediaResult.Take(Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryDisplayPageSize"])).Count();
                searchTermWiseTotalRecords = searchTermWiseTotalRecords == null ? 0 : searchTermWiseTotalRecords;
                shownRecords = shownRecords == null ? 0 : shownRecords;

                return Json(new
                {
                    columnChartJson = mediaChartJsonResponse.ColumnChartData,
                    lineChartJson = mediaChartJsonResponse.LineChartData,
                    pieChartJson = mediaChartJsonResponse.PieChartMediumData,

                    availableDataChart = mediaChartJsonResponse.DataAvailableList,
                    discoveryDateFilter = mediaChartJsonResponse.DateFilter,
                    discoveryMediumFilter = mediaChartJsonResponse.MediumFilter,
                    discoveryTVMarketFilter = mediaChartJsonResponse.TVMarket,
                    isSearchTermValid = mediaChartJsonResponse.IsSearchTermValid,

                    hasMoreResults = HasMoreResults((Int64)searchTermWiseTotalRecords, (Int64)shownRecords),
                    /*searchTermTotalRecords = string.Format("{0:N0}", searchTermWiseTotalRecords.Value),
                    searchTermShownRecords = string.Format("{0:N0}", shownRecords),*/

                    searchTermTotalRecords = searchTermWiseTotalRecords.Value,
                    searchTermShownRecords = shownRecords,
                    searchTermAvailableRecords = lstDiscoveryMediaResult.Count(),
                    displayPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryDisplayPageSize"]),

                    chartTotal = lstMainDiscoverySearchResponse != null ? string.Format("{0:N0}", lstMainDiscoverySearchResponse.Sum(s => s.TotalResult)) : null,

                    searchedIndex = searchTermIndex,
                    //notAvailableDataResult = dataNotAvailableList,
                    availableDataResult = dataAvailableList,
                    HTML = lstDiscoveryMediaResult != null ? RenderPartialToString(PATH_DiscoveryPartialView, lstDiscoveryMediaResult) : "",
                    isAnyDataAvailable = anyDataAvailable,

                    isSuccess = IsToggle ? true : mediaChartJsonResponse.IsSuccess
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

        #region Show More Result

        [HttpPost]
        public JsonResult MoreResult(string[] searchTermArray, Int16 searchTermIndex, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket)
        {
            try
            {
                Int64? shownRecords = 0;
                Int64? searchTermWiseTotalRecords = 0;
                string searchTerm = searchTermArray[searchTermIndex];
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                //string dataNotAvailableList = string.Empty;
                string dataAvailableList = string.Empty;

                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;

                discoveryTempData = GetTempData();
                discoveryTempData.CurrentPageSize += Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryPageSize"]);
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                lstTask.Add(Task<List<DiscoveryMediaResult>>.Factory.StartNew((object obj) => SearchDiscoveryResult(searchTerm, fromDate, toDate, medium, tvMarket, true, sessionInformation.ClientGUID, out shownRecords, out searchTermWiseTotalRecords, out  dataAvailableList), new DiscoveryMediaResult() { SearchTerm = searchTerm, MediumType = CommonFunctions.CategoryType.TV }));
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

                lstDiscoveryMediaResult = GetGMTandDSTTime(lstDiscoveryMediaResult);
                lstDiscoveryMediaResult = UpdateRecordTracking(lstDiscoveryMediaResult, searchTerm, discoveryTempData.CurrentPageSize);

                List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrackTemp = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData()).lstDiscoveryResultRecordTrack;

                bool anyDataAvailable = false;
                dataAvailableList = GetResultMessage(searchTerm, dataAvailableList, medium, out anyDataAvailable);

                searchTermWiseTotalRecords = (Int64)lstDiscoveryResultRecordTrackTemp.Where(w => w.SearchTerm.Equals(searchTerm)).FirstOrDefault().TotalRecords;
                //shownRecords = lstDiscoveryResultRecordTrackTemp.Where(w => w.SearchTerm.Equals(searchTerm)).Sum(s => s.TVRecordShownNum + s.NMRecordShownNum + s.SMRecordShownNum);

                shownRecords = lstDiscoveryMediaResult.Take(Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryDisplayPageSize"])).Count();

                searchTermWiseTotalRecords = searchTermWiseTotalRecords == null ? 0 : searchTermWiseTotalRecords;
                shownRecords = shownRecords == null ? 0 : shownRecords;

                var json = new
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
                    displayPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryDisplayPageSize"]),

                    isAnyDataAvailable = anyDataAvailable,
                    searchedIndex = searchTermIndex,
                    HTML = lstDiscoveryMediaResult != null ? RenderPartialToString(PATH_DiscoveryPartialView, lstDiscoveryMediaResult) : ""
                };
                return Json(json);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }

            return Json(new object());
        }
        #endregion


        #region Insert
        [HttpPost]
        public JsonResult SaveArticle(string articleID, string categoryGuid, string mediaType, string keywords, string description)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                string result = string.Empty;
                string resultMessage = string.Empty;
                bool showPopup = false;

                if (mediaType == CommonFunctions.CategoryType.NM.ToString())
                {
                    NMLogic nmLogic = (NMLogic)LogicFactory.GetLogic(LogicType.NM);
                    IQAgent_NewsResultsModel iQAgent_NewsResultsModel = nmLogic.SearchNewsByArticleID(articleID, WebApplication.Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.MO.ToString(), null,null));
                    result = nmLogic.InsertArchiveNM(iQAgent_NewsResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, new Guid(categoryGuid), string.Empty, keywords, description);
                }
                else if (mediaType == CommonFunctions.CategoryType.SocialMedia.ToString() ||
                        mediaType == CommonFunctions.CategoryType.Blog.ToString() ||
                            mediaType == CommonFunctions.CategoryType.Forum.ToString())
                {
                    SMLogic smLogic = (SMLogic)LogicFactory.GetLogic(LogicType.SM);
                    IQAgent_SMResultsModel iQAgent_SMResultsModel = smLogic.SearchSocialMediaByArticleID(articleID, WebApplication.Utility.CommonFunctions.GeneratePMGUrl(WebApplication.Utility.CommonFunctions.PMGUrlType.MO.ToString(), null, null));
                    iQAgent_SMResultsModel.SourceCategory = mediaType;
                    result = smLogic.InsertArchiveSM(iQAgent_SMResultsModel, sessionInformation.CustomerGUID, sessionInformation.ClientGUID, new Guid(categoryGuid), keywords, description);
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

            return Json(new object());
        }
        #endregion

        #region Saved Search

        [HttpPost]
        public JsonResult SaveSearch(string title, string[] searchTerm)//, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket)
        {
            try
            {
                Discovery_SavedSearchModel discovery_SavedSearch = new Discovery_SavedSearchModel();
                discovery_SavedSearch.Title = title;
                discovery_SavedSearch.SearchTermArray = searchTerm;
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
                    discoveryTempData = GetTempData();
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
        public JsonResult UpdateSavedSearch(Int32 p_ID, string[] p_SearchTerm)
        {
            try
            {
                Discovery_SavedSearchModel discovery_SavedSearch = new Discovery_SavedSearchModel();
                discovery_SavedSearch.ID = p_ID;
                discovery_SavedSearch.SearchTermArray = p_SearchTerm;
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                discovery_SavedSearch.CustomerGuid = sessionInformation.CustomerGUID;
                discovery_SavedSearch.ClientGuid = sessionInformation.ClientGUID;
                IQDiscovery_SavedSearchLogic iQDiscovery_SavedSearchLogic = (IQDiscovery_SavedSearchLogic)LogicFactory.GetLogic(LogicType.SavedSearch);
                string result = iQDiscovery_SavedSearchLogic.UpdateDiscoverySavedSearch(discovery_SavedSearch);
                string resultMessage = string.Empty;

                if (!string.IsNullOrWhiteSpace(result) && Convert.ToInt64(result) > 0)
                {
                    resultMessage = ConfigSettings.Settings.SearchUpdated;
                    discoveryTempData = GetTempData();
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
                Int32? currentPagenumber = 0;
                Int64 totalRecords = 0;
                discoveryTempData = GetTempData();
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
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
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

                discoveryTempData = GetTempData();
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
        #endregion

        #region Download PDF

        [HttpPost]
        public JsonResult GenerateDiscoveryPDF(string p_HTML, string p_FromDate, string p_ToDate, string[] p_SearchTerm)
        {
            try
            {
                string TempHTMLPath = GetHTMLFilePathWithCSSIncluded(p_HTML, p_FromDate, p_ToDate, false, p_SearchTerm);


                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");


                string TempPDFPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + sessionInformation.CustomerGUID + "_" + DateTimeStamp + ".pdf";

                bool IsFileGenerated = false;

                Utility.CommonFunctions.RunProcess(ConfigurationManager.AppSettings["WKHtmlToPDFPath"], TempHTMLPath + " " + TempPDFPath);

                if (System.IO.File.Exists(TempPDFPath))
                {
                    IsFileGenerated = true;
                    Session["PDFFile"] = TempPDFPath;

                    if (System.IO.File.Exists(TempHTMLPath))
                    {
                        System.IO.File.Delete(TempHTMLPath);
                    }
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
                    errorMessage = _ex.Message
                };
                return Json(json);
            }
        }

        [HttpPost]
        public JsonResult SendEmail(string p_HTML, string p_FromDate, string p_ToDate, string p_FromEmail, string p_ToEmail, string p_Subject, string p_UserBody, string[] p_SearchTerm)
        {
            try
            {
                //bool IsFileGenerated = false;
                int EmailSendCount = 0;

                string TempHTMLPath = GetHTMLFilePathWithCSSIncluded(p_HTML, p_FromDate, p_ToDate, true, p_SearchTerm);

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");


                string TempImagePath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + sessionInformation.CustomerGUID + "_" + DateTimeStamp + ".jpg";

                bool IsFileGenerated = false;

                var startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                //NOTE: there seems to be an issue with the redirect 
                startInfo.RedirectStandardError = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = ConfigurationManager.AppSettings["WKHtmlToImagePath"];
                startInfo.Arguments = TempHTMLPath + " " + TempImagePath;


                Shared.Utility.Log4NetLogger.Debug(startInfo.FileName + " " + startInfo.Arguments);
                using (var exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    exeProcess.Close();
                    //NOTE: Can't do this; see error above about redirect 
                    //var res = exeProcess.StandardError.ReadToEnd(); 
                    //Shared.Utility.Log4NetLogger.Debug(res); 
                }

                Image img = Image.FromFile(TempImagePath);
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                var bytes = ms.ToArray();
                ms.Close();
                ms.Dispose();


                string base64 = Convert.ToBase64String(bytes);
                string attachmentId = Path.GetFileName(TempImagePath);
                string iqMediaLogo = Server.MapPath("~/" + ConfigurationManager.AppSettings["IQMediaEmailLogo"]);
                string iqMediaLogoID = Path.GetFileName(iqMediaLogo);
                p_UserBody = p_UserBody + "<br/>" + "<img src=\"cid:" + attachmentId + "\" alt='Discovery'/>";

                StreamReader strmEmailPolicy = new StreamReader(Server.MapPath("~/content/EmailPolicy.txt"));
                string emailPolicy = strmEmailPolicy.ReadToEnd();
                strmEmailPolicy.Close();
                strmEmailPolicy.Dispose();
                p_UserBody = p_UserBody + emailPolicy;




                string[] alternetViewsName = new string[1];
                alternetViewsName[0] = TempImagePath;
                //alternetViewsName[1] = iqMediaLogo;



                if (!string.IsNullOrEmpty(p_ToEmail))
                {
                    foreach (string id in p_ToEmail.Split(new char[] { ';' }))
                    {
                        // send email code

                        if (IQMedia.Shared.Utility.CommonFunctions.SendMail(id, string.Empty, null, p_FromEmail, p_Subject, p_UserBody, true, null, alternetViewsName))
                        {
                            EmailSendCount++;
                        }
                    }
                }

                img.Dispose();
                if (System.IO.File.Exists(TempImagePath))
                {
                    System.IO.File.Delete(TempImagePath);
                    IsFileGenerated = true;
                    if (System.IO.File.Exists(TempHTMLPath))
                    {
                        System.IO.File.Delete(TempHTMLPath);
                    }
                }

                var json = new
                {
                    isSuccess = true,
                    emailSendCount = EmailSendCount
                };
                return Json(json);

            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                var json = new
                {
                    isSuccess = false,
                    errorMessage = ex.Message
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

        #endregion

        #region Methods


        #region PMG Search

        #region Chart

        public void SearchMedia(string[] searchTerm, DateTime? fromdate, DateTime? toDate, string medium, string tvMarket, bool IsInsertFromRecordID, Guid p_ClientGUID, out string availableData)
        {
            try
            {
                availableData = string.Empty;
                //notAvailableData = string.Empty;
                /*SearchTV(searchTerm, date, medium, tvMarket);
                SearchSocialMedia(searchTerm, date, medium, tvMarket);*/

                List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData()).lstDiscoveryResultRecordTrack;// TempData["DiscoveryResultRecordTrack"];
                discoveryTempData = GetTempData();

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
                    dictSSPData = GetSSPData(p_ClientGUID);
                }

                foreach (string sTerm in searchTerm)
                {

                    searchTermCount++;
                    if (searchTermCount <= 5)
                    {
                        var term = sTerm;
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
                            DiscoverySearchResponse dsrTV = new DiscoverySearchResponse() { SearchTerm = term, MediumType = CommonFunctions.CategoryType.TV.ToString(), IsValid = false };

                            lstTask.Add(Task<DiscoverySearchResponse>.Factory.StartNew((object obj) => SearchTV(term, fromdate, toDate, medium, tvMarket,
                                                                                    discoveryTempData.IsAllDmaAllowed, (List<IQ_Dma>)dictSSPData["IQ_Dma"],
                                                                                    discoveryTempData.IsAllClassAllowed, (List<IQ_Class>)dictSSPData["IQ_Class"],
                                                                                    discoveryTempData.IsAllStationAllowed, (List<Station_Affil>)dictSSPData["Station_Affil"], token, dsrTV), dsrTV));
                        }

                        // News Task
                        if (sessionInformation.Isv4NM && (discoveryResultRecordTrack.IsNMValid) && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.NM.ToString()))
                        {
                            DiscoverySearchResponse dsrNews = new DiscoverySearchResponse() { SearchTerm = term, MediumType = CommonFunctions.CategoryType.NM.ToString(), IsValid = false };

                            lstTask.Add(Task<DiscoverySearchResponse>.Factory.StartNew((object obj) => SearchNews(term, fromdate, toDate, medium, tvMarket, NMFromRecordID, token, dsrNews), dsrNews));
                        }

                        // SM Task
                        if (sessionInformation.Isv4SM && (discoveryResultRecordTrack.IsSMValid) &&
                            (string.IsNullOrWhiteSpace(medium) || medium == "Social Media" ||
                                medium == CommonFunctions.CategoryType.Blog.ToString() ||
                            medium == CommonFunctions.CategoryType.Forum.ToString()))
                        {

                            DiscoverySearchResponse dsrSM = new DiscoverySearchResponse() { SearchTerm = term, MediumType = CommonFunctions.CategoryType.SocialMedia.ToString(), IsValid = false };

                            lstTask.Add(Task<SocialMediaFacet>.Factory.StartNew((object obj) => SearchSocialMedia(term, fromdate, toDate, medium, tvMarket, SMFromRecordID, token, dsrSM), dsrSM));
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
                            discoverySearchResponse.MediumType = ((DiscoverySearchResponse)tsk.AsyncState).MediumType;
                            discoverySearchResponse.IsValid = false;
                            discoverySearchResponse.ListTopResults = new List<TopResults>();
                            strngNotAvailableData.Append(" " + ((DiscoverySearchResponse)tsk.AsyncState).SearchTerm + " for " + CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(((DiscoverySearchResponse)tsk.AsyncState).MediumType.Replace(" ", string.Empty))) + ", ");
                        }
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

        private Task<DiscoverySearchResponse> CheckForException(Task<DiscoverySearchResponse> task)
        {
            if (task.Exception != null)
            {
                Log4NetLogger.Error("CheckForException : " + task.Exception.ToString());
            }
            return task;
        }


        public DiscoverySearchResponse SearchTV(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, string tvMarket,
                                              bool IsAllDmaAllowed, List<IQ_Dma> listDma,
                                                bool IsAllClassAllowed, List<IQ_Class> listClass,
                                                bool IsAllStationAllowed, List<Station_Affil> listStation, CancellationToken token, DiscoverySearchResponse dsrTV)
        {
            try
            {

                Discovery2Logic discoveryLogic = (Discovery2Logic)LogicFactory.GetLogic(LogicType.Discovery2);
                DiscoverySearchResponse discoverySearchResponseTV = discoveryLogic.SearchTV(srcTerm, fromdate, toDate, medium, tvMarket, IsAllDmaAllowed, listDma, IsAllClassAllowed, listClass, IsAllStationAllowed, listStation, out lstTVMarket);

                if (!token.IsCancellationRequested)
                {
                    dsrTV.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }
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

        public DiscoverySearchResponse SearchNews(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, string tvMarket, string p_fromRecordID, CancellationToken token, DiscoverySearchResponse dsrNews)
        {
            try
            {

                Discovery2Logic discoveryLogic = (Discovery2Logic)LogicFactory.GetLogic(LogicType.Discovery2);
                DiscoverySearchResponse discoverySearchResponseNews = discoveryLogic.SearchNews(srcTerm, fromdate, toDate, medium, tvMarket, p_fromRecordID);

                if (!token.IsCancellationRequested)
                {
                    dsrNews.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }
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

        public SocialMediaFacet SearchSocialMedia(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, string tvMarket, string p_fromRecordID, CancellationToken token, DiscoverySearchResponse dsrSM)
        {
            try
            {

                Discovery2Logic discoveryLogic = (Discovery2Logic)LogicFactory.GetLogic(LogicType.Discovery2);
                SocialMediaFacet socialMediaFacet = discoveryLogic.SearchSocialMedia(srcTerm, fromdate, toDate, medium, tvMarket, p_fromRecordID);

                if (!token.IsCancellationRequested)
                {
                    dsrSM.IsValid = true;
                }
                else
                {
                    throw new Exception();
                }
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

        #endregion

        #region Result

        public List<DiscoveryMediaResult> SearchTVResult(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, string tvMarket,
                                                                bool IsAllDmaAllowed, List<IQ_Dma> listDma,
                                                            bool IsAllClassAllowed, List<IQ_Class> listClass,
                                                            bool IsAllStationAllowed, List<Station_Affil> listStation, Int32 p_PageSize, CancellationToken token, DiscoveryMediaResult dmrTV)//Int64 startRecordID,
        {
            try
            {

                Discovery2Logic discoveryLogic = (Discovery2Logic)LogicFactory.GetLogic(LogicType.Discovery2);
                List<DiscoveryMediaResult> lstDiscoveryMediaResult = discoveryLogic.SearchTVResult(srcTerm, fromdate, toDate, medium, tvMarket, sessionInformation.ClientGUID,
                                                                                IsAllDmaAllowed, listDma,
                                                                                IsAllClassAllowed, listClass,
                                                                                IsAllStationAllowed, listStation, discoveryTempData.iQClient_ThresholdValueModel, p_PageSize, out lstTVMarket);// startRecordID,


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

        public List<DiscoveryMediaResult> SearchNewsResult(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, Int32 p_PageSize, CancellationToken token, DiscoveryMediaResult dmrNews)//Int64 startRecordID, string fromRecordID,
        {
            try
            {
                Discovery2Logic discoveryLogic = (Discovery2Logic)LogicFactory.GetLogic(LogicType.Discovery2);
                List<DiscoveryMediaResult> lstDiscoveryMediaResult = discoveryLogic.SearchNewsResult(srcTerm, fromdate, toDate, medium, sessionInformation.ClientGUID, discoveryTempData.iQClient_ThresholdValueModel, p_PageSize);//startRecordID, fromRecordID,

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

                return lstDiscoveryMediaResult;
            }
            catch (Exception)
            {
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

        public List<DiscoveryMediaResult> SearchSocialMediaResult(string srcTerm, DateTime? fromdate, DateTime? toDate, string medium, Int32 p_PageSize, CancellationToken token, DiscoveryMediaResult dmrSM)//Int64 startRecordID, string fromRecordID,
        {
            try
            {

                Discovery2Logic discoveryLogic = (Discovery2Logic)LogicFactory.GetLogic(LogicType.Discovery2);
                List<DiscoveryMediaResult> lstDiscoveryMediaResult = discoveryLogic.SearchSocialMediaResult(srcTerm, fromdate, toDate, medium, sessionInformation.ClientGUID, discoveryTempData.iQClient_ThresholdValueModel, p_PageSize);//startRecordID, fromRecordID

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

        #endregion

        #endregion

        #region Chart Binding

        public string ColumnChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            try
            {
                Discovery2Logic discoveryLogic = (Discovery2Logic)LogicFactory.GetLogic(LogicType.Discovery2);
                string jsonResult = discoveryLogic.ColumnChart(lstDiscoverySearchResponse);
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

        public string LineChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse, Boolean isHourData)
        {

            Discovery2Logic discoveryLogic = (Discovery2Logic)LogicFactory.GetLogic(LogicType.Discovery2);
            string jsonLineChartResult = discoveryLogic.LineChart(lstDiscoverySearchResponse, isHourData);
            return jsonLineChartResult;
        }

        public List<PieChartResponse> PieChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse, string[] searchTerm, string medium)
        {

            Discovery2Logic discoveryLogic = (Discovery2Logic)LogicFactory.GetLogic(LogicType.Discovery2);
            List<PieChartResponse> lstPieChartReponse = discoveryLogic.PieChart(lstDiscoverySearchResponse, lstSMResponseFeedClass, searchTerm, medium,
                                                            sessionInformation.Isv4TV, sessionInformation.Isv4NM,
                                                             sessionInformation.Isv4SM);
            return lstPieChartReponse;
        }

        public IEnumerable GetDateFilter(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            Discovery2Logic discoveryLogic = (Discovery2Logic)LogicFactory.GetLogic(LogicType.Discovery2);
            return discoveryLogic.GetDateFilter(lstDiscoverySearchResponse);
        }

        public IEnumerable GetMediumFilter(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            Discovery2Logic discoveryLogic = (Discovery2Logic)LogicFactory.GetLogic(LogicType.Discovery2);
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
            if (shownRecords < TotalRecords)
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
            List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData()).lstDiscoveryResultRecordTrack;
            DiscoveryResultRecordTrack drrt = lstDiscoveryResultRecordTrack.Where(w => String.Compare(w.SearchTerm, sTerm, true) == 0).FirstOrDefault();
            bool isAnyDataAvailable = false;
            bool isAllDataAvailable = true;
            if (drrt != null)
            {
                if (string.IsNullOrWhiteSpace(medium))
                {
                    if (drrt.IsTVValid && drrt.IsNMValid && drrt.IsSMValid)
                    {
                        isAnyDataAvailable = true;
                    }
                    else
                    {
                        isAllDataAvailable = false;
                        if ((sessionInformation.Isv4TV && drrt.IsTVValid) || (sessionInformation.Isv4NM && drrt.IsNMValid) || (sessionInformation.Isv4SM && drrt.IsSMValid))
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
            List<DiscoveryResultRecordTrack> lstrecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData()).lstDiscoveryResultRecordTrack;
            bool isAllDataAvailable = true;
            bool isAnyDataAvailable = false;
            foreach (DiscoveryResultRecordTrack drrt in lstrecordTrack)
            {
                if (string.IsNullOrWhiteSpace(medium))
                {
                    if (drrt.IsTVValid && drrt.IsNMValid && drrt.IsSMValid)
                    {
                        isAnyDataAvailable = true;
                    }
                    else
                    {
                        isAllDataAvailable = false;
                        if ((sessionInformation.Isv4TV && drrt.IsTVValid) || (sessionInformation.Isv4NM && drrt.IsNMValid) || (sessionInformation.Isv4SM && drrt.IsSMValid))
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

        private string GetHTMLFilePathWithCSSIncluded(string p_HTML, string p_FromDate, string p_ToDate, bool p_IsEmail, string[] p_SearchTerm)
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


            p_HTML = "<html><head><style type=\"text/css\">" + Convert.ToString(cssData) + "</style></head>" + "<body>" + "<img src=\"" + Server.MapPath("~/" + ConfigurationManager.AppSettings["IQMediaEmailLogo"]) + "\" alt='IQMedia Logo'/>" + p_HTML + "</body></html>";

            sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
            string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            string TempHTMLPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + sessionInformation.CustomerGUID + "_" + DateTimeStamp + ".html";

            using (FileStream fs = new FileStream(TempHTMLPath, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.Write(p_HTML);
                }
            }


            HtmlDocument doc = new HtmlDocument();
            doc.Load(new StringReader(p_HTML));
            doc.OptionOutputOriginalCase = true;


            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//img"))
            {
                /*if (!string.IsNullOrEmpty(link.Attributes["alt"].Value))
                {*/
                try
                {
                    var newNodeHTML = "<img src=\"" + (Server.MapPath("~") + new Uri("http://qav4.iqmediacorp.com" + link.Attributes["src"].Value.Replace("..", string.Empty)).LocalPath).Replace("/", "\\") + "\" alt=''/>";
                    var newNode = HtmlNode.CreateNode(newNodeHTML);
                    link.ParentNode.ReplaceChild(newNode, link);
                }
                catch (Exception)
                {

                }

                //}
            }


            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//svg"))
            {
                var bytes = Encoding.UTF8.GetBytes(link.OuterHtml);
                string base64 = Convert.ToBase64String(bytes);
                var newNodeHTML = "<img src=\"data:image/svg+xml;base64," + base64 + "\" alt='test'/>";
                var newNode = HtmlNode.CreateNode(newNodeHTML);
                if (link != null)
                    link.ParentNode.ReplaceChild(newNode, link);
            }

            if (p_IsEmail)
            {
                doc.DocumentNode.SelectSingleNode("//body").SetAttributeValue("style", "width:1000px;");
            }

            StringBuilder finalHTML = new StringBuilder(); ;
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

            List<string> lstPieChart = new List<string>();
            for (int i = 0; i < p_SearchTerm.Length; i++)
            {
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
                if (doc.DocumentNode.SelectSingleNode("//div[@id='divPieChart_Child_" + i + "']") != null)
                {
                    finalHTML.Append(doc.DocumentNode.SelectSingleNode("//div[@id='divPieChart_Child_" + i + "']").InnerHtml);
                }

            }

            string finalHTMLData = "<html><head><style type=\"text/css\">.pagebreak {display: block;clear: both; page-break-after: always;} .searchTermData {font-weight:bold} " + Convert.ToString(cssData) + "</style></head>" + "<body>" + "<img src=\"" + Server.MapPath("~/" + ConfigurationManager.AppSettings["IQMediaEmailLogo"]) + "\" alt='IQMedia Logo'/><br/>" + Convert.ToString(finalHTML) + "</body></html>";
            HtmlDocument docFinal = new HtmlDocument();
            docFinal.Load(new StringReader(finalHTMLData));

            if (System.IO.File.Exists(TempHTMLPath))
            {
                System.IO.File.Delete(TempHTMLPath);
                docFinal.Save(TempHTMLPath);
            }
            return TempHTMLPath;

        }

        private List<DiscoveryMediaResult> GetGMTandDSTTime(List<DiscoveryMediaResult> listDiscoveryMediaResult)
        {
            sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
            listDiscoveryMediaResult.ForEach(s =>
            {
                if (s.MediumType != IQMedia.Shared.Utility.CommonFunctions.CategoryType.TV && s.Date.HasValue)
                {
                    if (DateTime.Now.IsDaylightSavingTime())
                    {

                        s.Date = s.Date.Value.AddHours((Convert.ToDouble(sessionInformation.gmt)) + Convert.ToDouble(sessionInformation.dst));
                    }
                    else
                    {
                        s.Date = s.Date.Value.AddHours((Convert.ToDouble(sessionInformation.gmt)));
                    }
                }

            });

            return listDiscoveryMediaResult;
        }

        #endregion

        #region SSP
        public Dictionary<string, object> GetSSPData(Guid p_ClientGUID)
        {
            try
            {
                bool isAllDmaAllowed = false;
                bool isAllClassAllowed = false;
                bool isAllStationAllowed = false;

                discoveryTempData = GetTempData();

                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                Dictionary<string, object> dictSSP = sspLogic.GetSSPDataByClientGUID(p_ClientGUID, out isAllStationAllowed, out isAllClassAllowed, out isAllStationAllowed, discoveryTempData.IQTVRegion);
                discoveryTempData.IsAllDmaAllowed = isAllDmaAllowed;
                discoveryTempData.IsAllClassAllowed = isAllClassAllowed;
                discoveryTempData.IsAllStationAllowed = isAllStationAllowed;
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

        protected List<DiscoveryMediaResult> UpdateRecordTracking(List<DiscoveryMediaResult> lstDiscoveryMediaResult, string sTerm, Int32 p_PageSize)
        {


            try
            {
                List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData()).lstDiscoveryResultRecordTrack;

                //string[] distinctSearchTerm = lstDiscoveryResultRecordTrack.Select(s => s.SearchTerm).Distinct().ToArray();

                bool IsTVValid = true;
                bool IsNMValid = true;
                bool IsSMValid = true;

                //foreach (string sTerm in distinctSearchTerm)
                //{

                DiscoveryResultRecordTrack discoveryResultRecordTrack = lstDiscoveryResultRecordTrack.Where(w => w.SearchTerm.Equals(sTerm)).FirstOrDefault();

                if (lstMainDiscoverySearchResponse != null)
                {

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


                    discoveryResultRecordTrack.TotalRecords = lstMainDiscoverySearchResponse.Where(w => w.SearchTerm.Equals(sTerm)).Sum(s => s.TotalResult);

                    IsTVValid = (!discoveryResultRecordTrack.IsTVValid ? false : true);
                    IsNMValid = (!discoveryResultRecordTrack.IsNMValid ? false : true);
                    IsSMValid = (!discoveryResultRecordTrack.IsSMValid ? false : true);

                    /*IsTVValid = discoveryResultRecordTrack.TVRecordTotal > 0 ? true : false;
                    IsNMValid = discoveryResultRecordTrack.NMRecordTotal > 0 ? true : false;
                    IsSMValid = discoveryResultRecordTrack.SMRecordTotal > 0 ? true : false;*/
                }

                if (lstDiscoveryMediaResult != null)
                {

                    #region Commented Previous Logic
                    /* if (IsTVValid)
                     {
                         IsTVValid = lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.TV.ToString()) && w.IsValid).Count() > 0 ? true : false;
                     }

                     if (IsNMValid)
                     {

                         IsNMValid = lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.NM.ToString()) && w.IsValid).Count() > 0 ? true : false;

                     }

                     if (IsSMValid)
                     {

                         IsSMValid = lstDiscoveryMediaResult.Where(w => w.SearchTerm.Equals(sTerm) && (Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.SocialMedia.ToString()) ||
                                                                           Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Blog.ToString()) ||
                                                                            Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Forum.ToString())) && w.IsValid).Count() > 0 ? true : false;

                     }*/

                    #endregion

                    List<DiscoveryMediaResult> lstFinal = lstDiscoveryMediaResult.OrderByDescending(o => o.Date).Where(w => w.IsValid).Take(Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryPageSize"])).ToList();
                    discoveryResultRecordTrack.TVRecordShownNum += lstFinal.Where(w => w.SearchTerm.Equals(sTerm) && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.TV.ToString()) && w.IsValid && w.IncludeInResult).Count();



                    discoveryResultRecordTrack.NMRecordShownNum += lstFinal.Where(w => String.Compare(w.SearchTerm, sTerm, true) == 0 && Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.NM.ToString()) && w.IsValid && w.IncludeInResult).Count();


                    discoveryResultRecordTrack.SMRecordShownNum += lstFinal.Where(w => w.SearchTerm.Equals(sTerm) && (Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.SocialMedia.ToString()) ||
                                                                              Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Blog.ToString()) ||
                                                                               Convert.ToString(w.MediumType).Equals(CommonFunctions.CategoryType.Forum.ToString()))
                                                                                  && w.IsValid && w.IncludeInResult).Count();


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
                    discoveryResultRecordTrack.IsTVValid = true;
                }
                else if (discoveryResultRecordTrack.IsTVValid)
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
                else if (discoveryResultRecordTrack.IsNMValid)
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
                else if (discoveryResultRecordTrack.IsSMValid)
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

                discoveryTempData = GetTempData();
                discoveryTempData.lstDiscoveryResultRecordTrack = lstDiscoveryResultRecordTrack;
                SetTempData(discoveryTempData);
                return lstDiscoveryMediaResult.OrderByDescending(o => o.Date).Where(w => w.IsValid && w.IncludeInResult).Take(p_PageSize).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        #endregion

        #region

        public MediaChartJsonResponse GetChartData(string[] searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, Guid p_ClientGUID, bool IsInsertFromRecordID)
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


                    lstMainDiscoverySearchResponse = new List<DiscoverySearchResponse>();
                    //string dataNotAvailableList = string.Empty;
                    string dataAvailableList = string.Empty;

                    SearchMedia(searchTerm, fromDate, toDate, medium, tvMarket, IsInsertFromRecordID, p_ClientGUID, out dataAvailableList);

                    //dataNotAvailableList = string.IsNullOrWhiteSpace(dataNotAvailableList) ? dataNotAvailableList : "Data not available : " + dataNotAvailableList;

                    var columnChartData = ColumnChart(lstMainDiscoverySearchResponse);

                    bool isHourData = false;

                    if ((toDate.Value - fromDate.Value).TotalDays <= 1)
                    {
                        isHourData = true;
                    }

                    var lineChartData = LineChart(lstMainDiscoverySearchResponse, isHourData);

                    var pieChartData = PieChart(lstMainDiscoverySearchResponse, searchTerm, medium);
                    var dateFilter = GetDateFilter(lstMainDiscoverySearchResponse);
                    var mediumFilter = GetMediumFilter(lstMainDiscoverySearchResponse);

                    pieChartData = GetTopResult((List<PieChartResponse>)pieChartData);
                    mediaChartJsonResponse.ColumnChartData = columnChartData;
                    mediaChartJsonResponse.LineChartData = lineChartData;
                    mediaChartJsonResponse.PieChartMediumData = pieChartData;
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
                    mediaChartJsonResponse.IsSuccess = true;
                    mediaChartJsonResponse.IsSearchTermValid = false;
                }

            }
            catch (Exception ex)
            {
                mediaChartJsonResponse.IsSuccess = false;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
            return mediaChartJsonResponse;
        }

        public List<DiscoveryMediaResult> SearchDiscoveryResult(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, bool isMoreResult, Guid p_ClientGUID, out Int64? shownRecords, out Int64? searchTermWiseTotalRecords, out string availableData)
        {
            try
            {

                //notAvailableData = string.Empty;
                discoveryTempData = GetTempData();

                availableData = string.Empty;
                shownRecords = 0;
                searchTermWiseTotalRecords = 0;
                Int64 tvStartRecordID = 0;
                Int64 nmStartRecordID = 0;
                Int64 smStartRecordID = 0;

                string tvFromRecordID = string.Empty;
                string nmFromRecordID = string.Empty;
                string smFromRecordID = string.Empty;


                List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData()).lstDiscoveryResultRecordTrack;// TempData["DiscoveryResultRecordTrack"];

                DiscoveryResultRecordTrack discoveryResultRecordTrack = lstDiscoveryResultRecordTrack.Where(w => w.SearchTerm.Equals(searchTerm)).FirstOrDefault();


                tvStartRecordID = discoveryResultRecordTrack.TVRecordShownNum;
                nmStartRecordID = discoveryResultRecordTrack.NMRecordShownNum;
                smStartRecordID = discoveryResultRecordTrack.SMRecordShownNum;

                //tvFromRecordID = discoveryResultRecordTrack.TVFromRecordID;
                nmFromRecordID = discoveryResultRecordTrack.NMFromRecordID;
                smFromRecordID = discoveryResultRecordTrack.SMFromRecordID;


                DiscoveryResultRecordTrack discoveryResultRecordTrackCount = new DiscoveryResultRecordTrack();

                Dictionary<String, object> dictSSPData = new Dictionary<string, object>();
                if (!discoveryTempData.IsAllDmaAllowed ||
                        !discoveryTempData.IsAllClassAllowed ||
                        !discoveryTempData.IsAllStationAllowed)
                {
                    dictSSPData = GetSSPData(p_ClientGUID);
                }

                List<Task> lstTask = new List<Task>();
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;

                //TV Task
                if (sessionInformation.Isv4TV && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.TV.ToString())) //(discoveryResultRecordTrack.IsTVValid) && 
                {

                    DiscoveryMediaResult dmrTV = new DiscoveryMediaResult() { SearchTerm = searchTerm, MediumType = CommonFunctions.CategoryType.TV, IsValid = false };

                    lstTask.Add(Task<List<DiscoveryMediaResult>>.Factory.StartNew((object obj) => SearchTVResult(searchTerm, fromDate, toDate, medium, tvMarket,
                                                                                                    discoveryTempData.IsAllDmaAllowed, (List<IQ_Dma>)dictSSPData["IQ_Dma"],
                                                                                    discoveryTempData.IsAllClassAllowed, (List<IQ_Class>)dictSSPData["IQ_Class"],
                                                                                    discoveryTempData.IsAllStationAllowed, (List<Station_Affil>)dictSSPData["Station_Affil"], discoveryTempData.CurrentPageSize, token, dmrTV), dmrTV));//tvStartRecordID,
                }

                // News Task
                if (sessionInformation.Isv4NM && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.NM.ToString())) //&& (discoveryResultRecordTrack.IsNMValid)
                {
                    DiscoveryMediaResult dmrNews = new DiscoveryMediaResult() { SearchTerm = searchTerm, MediumType = CommonFunctions.CategoryType.NM, IsValid = false };
                    lstTask.Add(Task<List<DiscoveryMediaResult>>.Factory.StartNew((object obj) => SearchNewsResult(searchTerm, fromDate, toDate, medium, discoveryTempData.CurrentPageSize, token, dmrNews), dmrNews));//nmStartRecordID, nmFromRecordID,
                }

                // SM Task
                if (sessionInformation.Isv4SM && (string.IsNullOrWhiteSpace(medium) || medium == "Social Media" ||
                        medium == CommonFunctions.CategoryType.Blog.ToString() ||
                    medium == CommonFunctions.CategoryType.Forum.ToString())) // && (discoveryResultRecordTrack.IsSMValid)
                {
                    DiscoveryMediaResult dmrSM = new DiscoveryMediaResult() { SearchTerm = searchTerm, MediumType = CommonFunctions.CategoryType.SocialMedia, IsValid = false };
                    lstTask.Add(Task<List<DiscoveryMediaResult>>.Factory.StartNew((object obj) => SearchSocialMediaResult(searchTerm, fromDate, toDate, medium, discoveryTempData.CurrentPageSize, token, dmrSM), dmrSM));//smStartRecordID, smFromRecordID,
                }

                List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();

                try
                {
                    Task.WaitAll(lstTask.ToArray(), Convert.ToInt32(ConfigurationManager.AppSettings["MaxRequestDuration"]), token);
                    tokenSource.Cancel();

                }
                catch (AggregateException ex)
                {
                }
                catch (Exception ex)
                {

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



                lstDiscoveryMediaResult = lstDiscoveryMediaResult.OrderByDescending(o => o.Date).ToList();
                return lstDiscoveryMediaResult;
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


        #region From Record ID and Paging Track

        public void UpdateFromRecordID(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            List<DiscoveryResultRecordTrack> lstDiscoveryResultRecordTrack = (List<DiscoveryResultRecordTrack>)((DiscoveryTempData)GetTempData()).lstDiscoveryResultRecordTrack;// TempData["DiscoveryResultRecordTrack"];
            string[] distinctSearchTerm = lstDiscoverySearchResponse.Select(s => s.SearchTerm).Distinct().ToArray();


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

                discoveryResultRecordTrack.TotalRecords = lstDiscoverySearchResponse.Where(w => w.SearchTerm.Equals(sTerm)).Sum(s => s.TotalResult);

                #region Logger
                /*Log4NetLogger.Debug("into UpdateFromRecordID start");
                Log4NetLogger.Debug("Total Reocords :" + discoveryResultRecordTrack.TotalRecords);
                Log4NetLogger.Debug("NMFromRecordID : " + discoveryResultRecordTrack.NMFromRecordID);
                Log4NetLogger.Debug("SMFromRecordID : " + discoveryResultRecordTrack.SMFromRecordID);
                Log4NetLogger.Debug("into UpdateFromRecordID end");*/
                #endregion
            }

            discoveryTempData = GetTempData();
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
                throw;
            }
            finally
            {
                TempData.Keep("DiscoveryTempData");
            }
        }

        #endregion

        #region Utility

        public DiscoveryTempData GetTempData()
        {
            if (TempData["DiscoveryTempData"] == null)
            {
                DiscoveryResultRecordTrack discoveryResultRecordTrack = new DiscoveryResultRecordTrack();
                List<DiscoveryResultRecordTrack> lst = new List<DiscoveryResultRecordTrack>();
                lst.Add(discoveryResultRecordTrack);
                discoveryTempData = new DiscoveryTempData();
                discoveryTempData.lstDiscoveryResultRecordTrack = lst;
                //Session["DiscoveryResultRecordTrack"] = lst;
            }
            else
            {
                discoveryTempData = (DiscoveryTempData)TempData["DiscoveryTempData"];
            }
            //discoveryTempData = TempData["DiscoveryTempData"] != null ? (DiscoveryTempData)TempData["DiscoveryTempData"] : new DiscoveryTempData();

            return discoveryTempData;
        }

        public void SetTempData(DiscoveryTempData p_DiscoveryTempData)
        {
            TempData["DiscoveryTempData"] = p_DiscoveryTempData;
            TempData.Keep("DiscoveryTempData");
        }

        #endregion

        #region Report

        public JsonResult GetDiscoveryReportLimit()
        {
            try
            {
                if (IQMedia.WebApplication.Utility.ActiveUserMgr.CheckAuthentication())
                {
                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                    IQClient_CustomSettingsModel maxDiscoveryReportLimit = UtilityLogic.GetDiscoveryReportAndExportLimit(sessionInformation.ClientGUID);

                    discoveryTempData = GetTempData();
                    discoveryTempData.MaxDiscoveryReportLimit = Convert.ToInt32(maxDiscoveryReportLimit.v4MaxDiscoveryReportItems);
                    SetTempData(discoveryTempData);
                    //TempData.Keep();

                    return Json(new
                    {
                        MaxDiscoveryReportItems = maxDiscoveryReportLimit.v4MaxDiscoveryReportItems,
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
                TempData.Keep("DiscoveryTempData");
            }
            return Json(new object());
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
        public JsonResult Insert_DiscoveryReport(string p_Title, string p_Keywords, string p_Description, Guid p_CategoryGuid, List<MediaIDClass> p_MediaID)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                IQDiscovery_ReportModel iQDiscovery_ReportModel = new IQDiscovery_ReportModel();
                iQDiscovery_ReportModel.Title = p_Title.Trim();
                iQDiscovery_ReportModel.Keywords = p_Keywords.Trim();
                iQDiscovery_ReportModel.Description = p_Description.Trim();
                iQDiscovery_ReportModel.CategoryGuid = p_CategoryGuid;
                iQDiscovery_ReportModel.CustomerGuid = sessionInformation.CustomerGUID;
                iQDiscovery_ReportModel.ClientGuid = sessionInformation.ClientGUID;

                discoveryTempData = GetTempData();
                p_MediaID = p_MediaID.Take(discoveryTempData.MaxDiscoveryReportLimit.Value).ToList();
                XDocument xdoc = new XDocument(new XElement("MediaIds", new XElement("TV", p_MediaID.Where(w => string.Compare(w.MediaType, "TV", true) == 0).Select(s => new XElement("ID", s.MediaID)))));
                xdoc.Root.Add(new XElement("NM", p_MediaID.Where(w => string.Compare(w.MediaType, "NM", true) == 0).Select(s => new XElement("ID", s.MediaID))));
                xdoc.Root.Add(new XElement("SM", p_MediaID.Where(w => string.Compare(w.MediaType, "SM", true) == 0).Select(s => new XElement("ID", s.MediaID))));
                /*xdoc.Root.Add(new XElement("Forum", p_MediaID.Where(w => string.Compare(w.MediaType, "Forum", true) == 0).Select(s => new XElement("ID", s.MediaID))));
                xdoc.Root.Add(new XElement("Blog", p_MediaID.Where(w => string.Compare(w.MediaType, "Blog", true) == 0).Select(s => new XElement("ID", s.MediaID))));*/
                iQDiscovery_ReportModel.MediaID = xdoc;

                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                string result = reportLogic.InsertDiscoveryReport(iQDiscovery_ReportModel);

                string resultMessage = string.Empty;
                bool showPopup = false;

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
        public JsonResult AddToDiscoveryReport(Int64 p_ReportID, string[] p_RecordList)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                ReportLogic reportLogic = (ReportLogic)LogicFactory.GetLogic(LogicType.Report);
                string mediaID = reportLogic.SelectDiscoveryMediaIDByID(sessionInformation.ClientGUID, p_ReportID);
                discoveryTempData = GetTempData();

                XDocument xDoc = XDocument.Parse(mediaID);

                List<MediaIDClass> lstmediaIDClass = new List<MediaIDClass>();

                List<string> lstMediaIDs = xDoc.Descendants("MediaIds").Descendants("ID").Select(s => s.Value).ToList();



                foreach (String record in p_RecordList)
                {
                    string[] mediaData = record.Split(',');

                    if (!lstMediaIDs.Contains(mediaData[0]))
                    {
                        MediaIDClass mediaIDClass = new MediaIDClass();
                        mediaIDClass.MediaID = mediaData[0];
                        mediaIDClass.MediaType = mediaData[1];
                        lstmediaIDClass.Add(mediaIDClass);
                    }
                }

                if ((xDoc.Root.Descendants("ID").Count() + lstmediaIDClass.Count) > discoveryTempData.MaxDiscoveryReportLimit)
                {
                    lstmediaIDClass = lstmediaIDClass.Take(Convert.ToInt32(discoveryTempData.MaxDiscoveryReportLimit - xDoc.Root.Descendants("ID").Count())).ToList();
                }

                if (lstmediaIDClass.Count > 0)
                {

                    xDoc.Root.Descendants("TV").FirstOrDefault().Add(lstmediaIDClass.Where(w => string.Compare(w.MediaType, "TV", true) == 0).Select(s => new XElement("ID", s.MediaID)));
                    xDoc.Root.Descendants("NM").FirstOrDefault().Add(lstmediaIDClass.Where(w => string.Compare(w.MediaType, "NM", true) == 0).Select(s => new XElement("ID", s.MediaID)));
                    xDoc.Root.Descendants("SM").FirstOrDefault().Add(lstmediaIDClass.Where(w => string.Compare(w.MediaType, "SM", true) == 0).Select(s => new XElement("ID", s.MediaID)));

                    reportLogic.IQReportDiscovery_Update(xDoc.ToString(), p_ReportID, sessionInformation.ClientGUID, sessionInformation.CustomerGUID);

                }
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
            return Json(new object());
        }

        [HttpPost]
        public JsonResult AddToDiscoveryLibrary(string p_Keywords, string p_Description, Guid p_CategoryGuid, List<MediaIDClass> p_MediaID)
        {
            try
            {
                discoveryTempData = GetTempData();
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                p_MediaID = p_MediaID.Take(discoveryTempData.MaxDiscoveryReportLimit.Value).ToList();

                XDocument xdoc = new XDocument(new XElement("MediaIds", new XElement("TV", p_MediaID.Where(w => string.Compare(w.MediaType, "TV", true) == 0).Select(s => new XElement("ID", s.MediaID)))));
                xdoc.Root.Add(new XElement("NM", p_MediaID.Where(w => string.Compare(w.MediaType, "NM", true) == 0).Select(s => new XElement("ID", s.MediaID))));
                xdoc.Root.Add(new XElement("SM", p_MediaID.Where(w => string.Compare(w.MediaType, "SM", true) == 0).Select(s => new XElement("ID", s.MediaID))));

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
                    message = p_MediaID.Select(w => string.Compare(w.MediaType, "TV", true) == 0 || string.Compare(w.MediaType, "NM", true) == 0 || string.Compare(w.MediaType, "SM", true) == 0).Count(),
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
    }

   /* public class SearchTermClass
    {
        public string SearchTerm { get; set; }
        public bool ResultShown { get; set; }
        public bool IsCurrentTab { get; set; }
        
    }

    public class MediaIDClass
    {
        public string MediaID { get; set; }
        public string MediaType { get; set; }
    }*/


}
