using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Model;
using IQMedia.Web.Logic;
using PMGSearch;
using System.Configuration;
using System.IO;
using System.Text;
using IQMedia.Web.Logic.Base;
using System.Globalization;
using IQMedia.WebApplication.Utility;
using IQMedia.WebApplication.Models.TempData;
using IQMedia.WebApplication.Config;
using System.Xml.Linq;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class TAdsController : Controller
    {
        #region Public Property
        SessionInformation sessionInformation = null;
        TAdsTempData tAdsTempData = null;
        string PATH_DiscoveryPartialView = "~/Views/TAds/_Result.cshtml";
        bool hasMoreResultPage = false;
        bool hasPreviousResultPage = false;
        string recordNumberDesc = string.Empty;
        #endregion

        public ActionResult Index()
        {
            try
            {
                SetTempData(null);
                tAdsTempData = GetTempData();
                tAdsTempData.SinceID = null;
                tAdsTempData.TotalResults = null;
                tAdsTempData.CurrentPage = null;
                tAdsTempData.p_IsAllClassAllowed = false;
                tAdsTempData.p_IsAllClassAllowed = false;
                tAdsTempData.p_IsAllStationAllowed = false;

                sessionInformation = IQMedia.WebApplication.Utility.CommonFunctions.GetSessionInformation();

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                var manualClipDuration = clientLogic.GetClientManualClipDurationSettings(sessionInformation.ClientGUID);
                Int16 rawMediaPauseSecs = clientLogic.GetClientRawMediaPauseSecs(sessionInformation.ClientGUID);

                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                bool p_IsAllDmaAllowed;
                bool p_IsAllClassAllowed;
                bool p_IsAllStationAllowed;
                Dictionary<string, object> dicSSP = sspLogic.GetSSPDataByClientGUID(sessionInformation.ClientGUID, out p_IsAllDmaAllowed, out p_IsAllClassAllowed, out p_IsAllStationAllowed, tAdsTempData.IQTVRegion, true);

                tAdsTempData.p_IsAllDmaAllowed = p_IsAllDmaAllowed;
                tAdsTempData.p_IsAllClassAllowed = p_IsAllClassAllowed;
                tAdsTempData.p_IsAllStationAllowed = p_IsAllStationAllowed;
                List<IQ_Region> IQTVRegionList = (List<IQ_Region>)dicSSP["IQ_Region"];
                List<IQ_Country> IQTVCountryList = (List<IQ_Country>)dicSSP["IQ_Country"];
                tAdsTempData.IQTVRegion = IQTVRegionList.Select(r => r.Num).ToList();
                tAdsTempData.IQTVCountry = IQTVCountryList.Select(c => c.Num).ToList();

                SetTempData(tAdsTempData);

                TVLogic tvlogic = new TVLogic();
                tAdsTempData.IQTVStations = tvlogic.GetTAdsStations();

                string strHtml = TAdsDefaultResults("", "", null, null, null, null, null, "", null, null, false);
                dicSSP.Add("DefaultHTML", strHtml);
                ViewBag.IsSuccess = true;
                ViewBag.ManualClipDuration = manualClipDuration;
                ViewBag.RawMediaPauseSecs = rawMediaPauseSecs;
                return View(dicSSP);
            }
            catch (Exception exception)
            {
                UtilityLogic.WriteException(exception);
                ViewBag.IsSuccess = false;
            }
            finally
            {
                TempData.Keep("TAdsTempData");
            }
            return View();
        }
        public string TAdsDefaultResults(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.CommonFunctions.GetSessionInformation();
                int ResultCount = 0;
                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TAdsPageSize"]);

                tAdsTempData = GetTempData();
                tAdsTempData.PageNumber = 0;

                List<IQAgent_TVResultsModel> tvResult = new List<IQAgent_TVResultsModel>();
                bool isallDmaAllowed = tAdsTempData.p_IsAllDmaAllowed;
                bool isallClassAllowed = tAdsTempData.p_IsAllClassAllowed;
                bool isallStationAllowed = tAdsTempData.p_IsAllStationAllowed;

                //do not get results from before 12/14/15
                if (p_FromDate == null) p_ToDate = DateTime.Now;
                else if (p_FromDate < new DateTime(2015, 12, 14)) p_ToDate = new DateTime(2015, 12, 14);
                if (p_FromDate == null || p_FromDate < new DateTime(2015, 12, 14)) p_FromDate = new DateTime(2015, 12, 14);

                TVLogic tvlogic = new TVLogic();
                if (p_IQStationID == null || p_IQStationID.Count == 0) p_IQStationID = tvlogic.GetTAdsStations();

                tvResult = tvlogic.TAdsSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, 0, isallDmaAllowed, isallClassAllowed, isallStationAllowed, string.Empty, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.TV.ToString(), p_FromDate, p_ToDate), tAdsTempData.IQTVRegion, tAdsTempData.IQTVCountry, p_Region, p_Country);
                tAdsTempData.ResultCount = ResultCount;
                hasMoreResultPage = false;
                hasPreviousResultPage = false;

                if (tvResult != null && ResultCount > 0)
                {
                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;
                    if (totalHit > (tAdsTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;

                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((tAdsTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                }
                if (tAdsTempData.PageNumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                tAdsTempData.HasMoreResultPage = hasMoreResultPage;
                tAdsTempData.HasPreviousResultPage = hasPreviousResultPage;
                tAdsTempData.RecordNumber = recordNumberDesc;

                SetTempData(tAdsTempData);
                string htmlResult = RenderPartialToString(PATH_DiscoveryPartialView, tvResult);

                return htmlResult;

            }
            catch (IQMedia.Shared.Utility.CustomException ex)
            {
                UtilityLogic.WriteException(ex, ex.Message);
                return ConfigSettings.Settings.ErrorOccurred;
            }
            catch (Exception ex)
            {
                UtilityLogic.WriteException(ex);
                return "";
            }
            finally
            {
                TempData.Keep("TAdsTempData");
            }
        }
        [HttpPost]
        public JsonResult TAdsSearchResults(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, string p_SortColumn, bool p_IsDefaultLoad)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.CommonFunctions.GetSessionInformation();
                int ResultCount = 0;
                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TAdsPageSize"]);


                tAdsTempData = GetTempData();
                tAdsTempData.PageNumber = 0;

                List<IQAgent_TVResultsModel> tvResult = new List<IQAgent_TVResultsModel>();
                bool isallDmaAllowed = tAdsTempData.p_IsAllDmaAllowed;
                bool isallClassAllowed = tAdsTempData.p_IsAllClassAllowed;
                bool isallStationAllowed = tAdsTempData.p_IsAllStationAllowed;

                Dictionary<string, object> filters = null;

                string strDmaXml = null;
                if (p_Dma != null && p_Dma.Count > 0)
                {
                    XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in p_Dma
                                             select new XElement("dma", new XAttribute("name", ele))
                                                     ));
                    strDmaXml = xdoc.ToString();
                }

                string strStationXml = null;
                if (p_Station != null && p_Station.Count > 0)
                {
                    XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in p_Station
                                             select new XElement("station", new XAttribute("name", ele))
                                                     ));
                    strStationXml = xdoc.ToString();
                }

                string strStationIDXml = null;
                TVLogic tvlogic = new TVLogic();
                var TAdsIQStations = tvlogic.GetTAdsStations();
                if (p_IQStationID == null || p_IQStationID.Count == 0)
                {
                    p_IQStationID = TAdsIQStations;
                }
                else
                {
                    p_IQStationID = p_IQStationID.Where(x => TAdsIQStations.Contains(x)).ToList();
                    if (p_IQStationID == null || p_IQStationID.Count == 0) p_IQStationID = new List<string>();
                }
                if (p_IQStationID != null && p_IQStationID.Count > 0)
                {
                    XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in p_IQStationID
                                             select new XElement("stationid", new XAttribute("id", ele))
                                                     ));
                    strStationIDXml = xdoc.ToString();
                }
                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                filters = sspLogic.GetSSPDataByClientGUIDAndFilter(sessionInformation.ClientGUID, strDmaXml, strStationXml, strStationIDXml, p_Region, p_Country, tAdsTempData.IQTVRegion);

                tAdsTempData.SelectedDma = p_Dma;
                tAdsTempData.SelectedStation = p_Station;

                //do not get results from before 12/14/15
                if (p_FromDate == null) p_ToDate = DateTime.Now;
                else if (p_FromDate < new DateTime(2015, 12, 14)) p_ToDate = new DateTime(2015, 12, 14);
                if (p_FromDate == null || p_FromDate < new DateTime(2015, 12, 14)) p_FromDate = new DateTime(2015, 12, 14);

                tvResult = tvlogic.TAdsSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, 0, isallDmaAllowed, isallClassAllowed, isallStationAllowed, p_SortColumn, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.TV.ToString(), p_FromDate, p_ToDate), tAdsTempData.IQTVRegion, tAdsTempData.IQTVCountry, p_Region, p_Country);
                tAdsTempData.ResultCount = ResultCount;
                hasMoreResultPage = false;
                hasPreviousResultPage = false;

                if (tvResult != null && ResultCount > 0)
                {
                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;

                    if (totalHit > (tAdsTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;
                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((tAdsTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                }
                if (tAdsTempData.PageNumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                tAdsTempData.HasMoreResultPage = hasMoreResultPage;
                tAdsTempData.HasPreviousResultPage = hasPreviousResultPage;
                tAdsTempData.RecordNumber = recordNumberDesc;

                SetTempData(tAdsTempData);
                string htmlResult = RenderPartialToString(PATH_DiscoveryPartialView, tvResult);

                return Json(new
                {
                    HTML = htmlResult != null ? htmlResult : "",
                    hasMoreResult = hasMoreResultPage,
                    filters = filters,
                    hasPreviouResult = hasPreviousResultPage,
                    recordNumber = recordNumberDesc,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {

                UtilityLogic.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("TAdsTempData");
            }
        }

        [HttpPost]
        public JsonResult TAdsSearchResultsPaging(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, bool p_IsNext, string p_SortColumn)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.CommonFunctions.GetSessionInformation();
                tAdsTempData = GetTempData();

                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TAdsPageSize"]);
                hasMoreResultPage = false;
                hasPreviousResultPage = false;
                int ResultCount = 0;
                Int32 pagenumber = 0;

                if (p_IsNext)
                {
                    if (tAdsTempData.HasMoreResultPage)
                    {
                        if (tAdsTempData.PageNumber != null)
                        {
                            pagenumber = tAdsTempData.PageNumber + 1;
                            tAdsTempData.PageNumber = tAdsTempData.PageNumber + 1;
                        }
                        else
                        {
                            tAdsTempData.PageNumber = 1;
                            pagenumber = 1;
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = false
                        });
                    }
                }
                else
                {
                    if (tAdsTempData.HasPreviousResultPage)
                    {
                        if (tAdsTempData.PageNumber != null)
                        {
                            pagenumber = tAdsTempData.PageNumber - 1;
                            tAdsTempData.PageNumber = tAdsTempData.PageNumber - 1;
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            isSuccess = false
                        });
                    }

                }

                List<IQAgent_TVResultsModel> tvResult = new List<IQAgent_TVResultsModel>();
                bool isallDmaAllowed = tAdsTempData.p_IsAllDmaAllowed;
                bool isallClassAllowed = tAdsTempData.p_IsAllClassAllowed;
                bool isallStationAllowed = tAdsTempData.p_IsAllStationAllowed;

                //do not get results from before 12/14/15
                if (p_FromDate == null || p_FromDate < new DateTime(2015, 12, 14)) p_FromDate = new DateTime(2015, 12, 14);
                if (p_FromDate == null) p_ToDate = DateTime.Now;
                else if (p_FromDate < new DateTime(2015, 12, 14)) p_ToDate = new DateTime(2015, 12, 14);

                TVLogic tvlogic = new TVLogic();
                var TAdsIQStations = tvlogic.GetTAdsStations();
                if (p_IQStationID == null || p_IQStationID.Count == 0) p_IQStationID = TAdsIQStations;
                else
                {
                    p_IQStationID = p_IQStationID.Where(x => TAdsIQStations.Contains(x)).ToList();
                    if (p_IQStationID == null || p_IQStationID.Count == 0) p_IQStationID = new List<string>();
                }
                tvResult = tvlogic.TAdsSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, pagenumber, isallDmaAllowed, isallClassAllowed, isallStationAllowed, p_SortColumn, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.TV.ToString(), p_FromDate, p_ToDate), tAdsTempData.IQTVRegion, tAdsTempData.IQTVCountry, p_Region, p_Country);
                if (tvResult != null && ResultCount > 0)
                {
                    double totalHit = Convert.ToDouble(ResultCount) / PageSize;
                    if (totalHit > (tAdsTempData.PageNumber + 1))
                    {
                        hasMoreResultPage = true;
                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ((tAdsTempData.PageNumber + 1) * PageSize).ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                    else
                    {
                        hasMoreResultPage = false;
                        recordNumberDesc = (((tAdsTempData.PageNumber + 1) * PageSize) - PageSize + 1).ToString("N0") + " - " + ResultCount.ToString("N0") + " of " + ResultCount.ToString("N0");
                    }
                }
                if (pagenumber > 0)
                {
                    hasPreviousResultPage = true;
                }
                else
                {
                    hasPreviousResultPage = false;
                }
                tAdsTempData.HasMoreResultPage = hasMoreResultPage;
                tAdsTempData.HasPreviousResultPage = hasPreviousResultPage;
                SetTempData(tAdsTempData);

                string htmlResult = RenderPartialToString(PATH_DiscoveryPartialView, tvResult);

                return Json(new
                {
                    HTML = htmlResult != null ? htmlResult : "",
                    hasMoreResult = hasMoreResultPage,
                    hasPreviouResult = hasPreviousResultPage,
                    recordNumber = recordNumberDesc,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {

                UtilityLogic.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("TAdsTempData");
            }
        }
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

        [HttpPost]
        public JsonResult GetChart(string IQ_CC_KEY)
        {
            try
            {
                sessionInformation = CommonFunctions.GetSessionInformation();

                TVLogic tvlogic = new TVLogic();
                List<IQTAdsResultModel> lstAds_Results = tvlogic.GetTadsResultByIQCCKey(IQ_CC_KEY);
                string tAdsResult_string = tvlogic.TAdsHighLineChart(lstAds_Results);

                bool HasTAdsResults = lstAds_Results != null && lstAds_Results.Count > 0;


                IQTimeSync_DataLogic iQTimeSync_DataLogic = (IQTimeSync_DataLogic)LogicFactory.GetLogic(LogicType.IQTimeSync_Data);
                List<IQTimeSync_DataModel> lstiQTimeSync_DataModel = iQTimeSync_DataLogic.GetTimeSyncDataByIQCCKeyAndCustomerGuid(IQ_CC_KEY, sessionInformation.CustomerGUID);

                bool IsTimeSync = true;
                if (lstiQTimeSync_DataModel != null && lstiQTimeSync_DataModel.Count > 0)
                {
                    foreach (IQTimeSync_DataModel item in lstiQTimeSync_DataModel)
                    {
                        item.Data = iQTimeSync_DataLogic.TimeSyncHighLineChart(item.Data, item.GraphStructure);
                    }
                }
                else
                {
                    IsTimeSync = false;
                }

                return Json(new
                {
                    tAdsResultsJson = tAdsResult_string,
                    hasTAdsResults = HasTAdsResults,
                    lineChartJson = lstiQTimeSync_DataModel,
                    isTimeSync = IsTimeSync,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                UtilityLogic.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("TAdsTempData");
            }
        }

        private bool HasMoreResults(Int64 TotalRecords, Int64 shownRecords)
        {
            if (shownRecords < TotalRecords)
                return true;
            else
                return false;
        }


        #region Utility
        public TAdsTempData GetTempData()
        {
            tAdsTempData = TempData["TAdsTempData"] != null ? (TAdsTempData)TempData["TAdsTempData"] : new TAdsTempData();
            return tAdsTempData;
        }
        public void SetTempData(TAdsTempData p_TAdsTempData)
        {
            TempData["TAdsTempData"] = p_TAdsTempData;
            TempData.Keep("TAdsTempData");
        }
        #endregion

    }
}
