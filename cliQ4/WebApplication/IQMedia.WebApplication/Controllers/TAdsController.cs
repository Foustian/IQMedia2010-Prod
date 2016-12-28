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
using System.Dynamic;
using Newtonsoft.Json;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class TAdsController : Controller
    {
        #region Public Property
        ActiveUser sessionInformation = null;
        TAdsTempData tAdsTempData = null;
        string PATH_TAdsPartialView = "~/Views/TAds/_Result.cshtml";
        bool hasMoreResultPage = false;
        bool hasPreviousResultPage = false;
        string recordNumberDesc = string.Empty;
        #endregion

        public ActionResult Index()
        {
            try
            {
                // Clear out temp data used by other pages
                if (TempData.ContainsKey("AnalyticsTempData")) { TempData["AnalyticsTempData"] = null; }
                if (TempData.ContainsKey("FeedsTempData")) { TempData["FeedsTempData"] = null; }
                if (TempData.ContainsKey("DiscoveryTempData")) { TempData["DiscoveryTempData"] = null; }
                if (TempData.ContainsKey("TimeShiftTempData")) { TempData["TimeShiftTempData"] = null; }
                if (TempData.ContainsKey("SetupTempData")) { TempData["SetupTempData"] = null; }
                if (TempData.ContainsKey("GlobalAdminTempData")) { TempData["GlobalAdminTempData"] = null; } 

                SetTempData(null);
                tAdsTempData = GetTempData();
                tAdsTempData.SinceID = null;
                tAdsTempData.TotalResults = null;
                tAdsTempData.CurrentPage = null;
                tAdsTempData.p_IsAllClassAllowed = false;
                tAdsTempData.p_IsAllClassAllowed = false;
                tAdsTempData.p_IsAllStationAllowed = false;

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                var manualClipDuration = clientLogic.GetClientManualClipDurationSettings(sessionInformation.ClientGUID);
                Int16 rawMediaPauseSecs = clientLogic.GetClientRawMediaPauseSecs(sessionInformation.ClientGUID);
                IQClient_CustomSettingsModel customSettingsModel = clientLogic.GetClientCustomSettings(sessionInformation.ClientGUID.ToString());

                if (customSettingsModel.visibleLRIndustries != null)
                {
                    tAdsTempData.VisibleLRIndustries = new List<string>();
                    tAdsTempData.VisibleLRBrands = new List<string>();

                    foreach (IQ_Industry ind in customSettingsModel.visibleLRIndustries)
                    {
                        tAdsTempData.VisibleLRIndustries.Add(ind.ID);
                    }
                    tAdsTempData.VisibleLRBrands = customSettingsModel.visibleLRBrands;
                }
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

                //don't get any results before2016
                List<Dictionary<string, string>> logoHits;
                List<Dictionary<string, string>> adHits;
                string strHtml = TAdsDefaultResults("", "", new DateTime(2016,1,1), DateTime.Now, null, null, null, "", null, null, false, out logoHits, out adHits);
                dicSSP.Add("DefaultHTML", strHtml);

                System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                dicSSP.Add("DefaultLogo", oSerializer.Serialize(logoHits));
                dicSSP.Add("DefaultAds", oSerializer.Serialize(adHits));

                ViewBag.IsSuccess = true;
                ViewBag.ManualClipDuration = manualClipDuration;
                ViewBag.RawMediaPauseSecs = rawMediaPauseSecs;
                return View(dicSSP);
            }
            catch (Exception exception)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                ViewBag.IsSuccess = false;
            }
            finally
            {
                TempData.Keep("TAdsTempData");
            }
            return View();
        }
        public string TAdsDefaultResults(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, out List<Dictionary<string, string>> logoHits, out List<Dictionary<string, string>> adHits)
        {
            logoHits = new List<Dictionary<string, string>>();
            adHits = new List<Dictionary<string, string>>();

            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                int ResultCount = 0;
                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TAdsPageSize"]);

                tAdsTempData = GetTempData();
                tAdsTempData.PageNumber = 0;

                List<IQAgent_TVFullResultsModel> tvResult = new List<IQAgent_TVFullResultsModel>();
                bool isallDmaAllowed = tAdsTempData.p_IsAllDmaAllowed;
                bool isallClassAllowed = tAdsTempData.p_IsAllClassAllowed;
                bool isallStationAllowed = tAdsTempData.p_IsAllStationAllowed;

                TVLogic tvlogic = new TVLogic();
                if (p_IQStationID == null || p_IQStationID.Count == 0) p_IQStationID = tvlogic.GetTAdsStations();             

                var dicTVResult = tvlogic.TAdsSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, 0, isallDmaAllowed, isallClassAllowed, isallStationAllowed, string.Empty, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.MT.ToString(), p_FromDate, p_ToDate), tAdsTempData.IQTVRegion, tAdsTempData.IQTVCountry, p_Region, p_Country, null, null, tAdsTempData.VisibleLRIndustries);
                tvResult = (List<IQAgent_TVFullResultsModel>)dicTVResult["result"];
                

                tAdsTempData.ResultCount = ResultCount;
                hasMoreResultPage = false;
                hasPreviousResultPage = false;

                if (tvResult != null && ResultCount > 0)
                {
                    foreach (var result in tvResult)
                    {
                        logoHits.AddRange(result.Logos.Distinct().Select(x => new Dictionary<string, string> { { result.IQ_CC_Key, x } }).ToList());
                        adHits.AddRange(result.Ads.Distinct().Select(x => new Dictionary<string, string> { { result.IQ_CC_Key, x } }).ToList());
                    }

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
                string htmlResult = RenderPartialToString(PATH_TAdsPartialView, tvResult);

                return htmlResult;

            }
            catch (IQMedia.Shared.Utility.CustomException ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex, ex.Message);
                return ConfigSettings.Settings.ErrorOccurred;
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return "";
            }
            finally
            {
                TempData.Keep("TAdsTempData");
            }
        }
        
        [HttpPost]
        public JsonResult TAdsSearchResults(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, string p_SortColumn, bool p_IsDefaultLoad, List<string> p_SearchLogo, List<string> p_Brand, List<string> p_Industry, List<string> p_Company, string p_PaidEarned)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                int ResultCount = 0;
                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TAdsPageSize"]);


                tAdsTempData = GetTempData();
                tAdsTempData.PageNumber = 0;

                //check if industries is restricted
                if ((p_Industry == null || p_Industry.Count <= 0) && tAdsTempData.VisibleLRIndustries != null) 
                {
                    p_Industry = tAdsTempData.VisibleLRIndustries;
                }

                List<IQAgent_TVFullResultsModel> tvResult = new List<IQAgent_TVFullResultsModel>();
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

                var dicTVResult = tvlogic.TAdsSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, 0, isallDmaAllowed, isallClassAllowed, isallStationAllowed, p_SortColumn, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.MT.ToString(), p_FromDate, p_ToDate), tAdsTempData.IQTVRegion, tAdsTempData.IQTVCountry, p_Region, p_Country, p_SearchLogo, p_Brand, p_Industry, p_Company, p_PaidEarned, true);
                tvResult = (List<IQAgent_TVFullResultsModel>)dicTVResult["result"];

                tAdsTempData.ResultCount = ResultCount;
                hasMoreResultPage = false;
                hasPreviousResultPage = false;

                var logoHits = new List<Dictionary<string, string>>();
                var adHits = new List<Dictionary<string, string>>();
                if (tvResult != null && ResultCount > 0)
                {
                    var logos = new List<long>();
                    var brands = new List<long>();
                    var companies = new List<long>();
                    var industries = new List<long>();

                    foreach (var result in tvResult)
                    {
                        logoHits.AddRange(result.Logos.Distinct().Select(x => new Dictionary<string, string>{{result.IQ_CC_Key, x}}).ToList());
                        adHits.AddRange(result.Ads.Distinct().Select(x => new Dictionary<string, string>{{result.IQ_CC_Key, x}}).ToList());
                    }


                    if (tAdsTempData.VisibleLRIndustries != null && tAdsTempData.VisibleLRIndustries.Count > 0) 
                    { 
                    List<string> iqTotalBrands = ((IEnumerable<string>)dicTVResult["brands"]).ToList();
                    List<string> iqTotalIndustries = ((IEnumerable<string>)dicTVResult["industries"]).ToList();
                    List<string> iqBrands = new List<string>();
                    List<string> iqIndustries= new List<string>();
                        foreach(string b in tAdsTempData.VisibleLRBrands)
                        {
                        if(iqTotalBrands.Contains(b))
                        {
                            iqBrands.Add(b);
                        }                       
                        }
                        foreach (string i in tAdsTempData.VisibleLRIndustries)
                        {
                            if(iqTotalIndustries.Contains(i))
                            {
                                iqIndustries.Add(i);
                            }
                        }
                        filters.Add("IQ_Brand", iqBrands);
                        filters.Add("IQ_Industry", iqIndustries);
                    }
                    else
                    {
                        filters.Add("IQ_Logo", ((IEnumerable<string>)dicTVResult["logos"]).ToList());
                        filters.Add("IQ_Brand", ((IEnumerable<string>)dicTVResult["brands"]).ToList());
                        filters.Add("IQ_Company", ((IEnumerable<string>)dicTVResult["companies"]).ToList());
                        filters.Add("IQ_Industry", ((IEnumerable<string>)dicTVResult["industries"]).ToList());
                    }
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
                string htmlResult = RenderPartialToString(PATH_TAdsPartialView, tvResult);

                return Json(new
                {
                    HTML = htmlResult != null ? htmlResult : "",
                    hasMoreResult = hasMoreResultPage,
                    filters = filters,
                    hasPreviouResult = hasPreviousResultPage,
                    recordNumber = recordNumberDesc,
                    logoHits = logoHits,
                    adHits = adHits,
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
                TempData.Keep("TAdsTempData");
            }
        }

        [HttpPost]
        public JsonResult GetFilters()
        {
            try
            {
                TVLogic logic = new TVLogic();
                var result = logic.GetFilters();
                tAdsTempData = GetTempData();

                List<IQ_Logo> logoList = new List<IQ_Logo>();
                List<IQ_Brand> brandList = new List<IQ_Brand>();
                List<IQ_Industry> industryList = new List<IQ_Industry>();
                List<IQ_Company> companyList = new List<IQ_Company>();

                if (result["IQ_Logo"] != null) 
                {
                    logoList = (List<IQ_Logo>)result["IQ_Logo"];
                    Session["FullSearchLogoList"] = (List<IQ_Logo>)result["IQ_Logo"];
                }
                if (result["IQ_Brand"] != null) 
                {
                    brandList = (List<IQ_Brand>)result["IQ_Brand"];
                    Session["FullBrandList"] = (List<IQ_Brand>)result["IQ_Brand"];
                }
                if (result["IQ_Industry"] != null) industryList = (List<IQ_Industry>)result["IQ_Industry"];
                if (result["IQ_Company"] != null) companyList = (List<IQ_Company>)result["IQ_Company"];               

                return Json(new
                {
                    logoList = logoList,
                    brandList = brandList,
                    companyList = companyList,
                    industryList = industryList,
                    visibleLRIndustries = tAdsTempData.VisibleLRIndustries,
                    visibleLRBrands = tAdsTempData.VisibleLRBrands,
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
                TempData.Keep("TAdsTempData");
            }
        }

        public IQAgent_TVFullResultsModel GetTadsResultByIQCCKey(string iqcckey, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                TVLogic logic = new TVLogic();
                var result = logic.GetTadsResultByIQCCKey(iqcckey, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.MT.ToString(), fromDate, toDate));

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult TAdsSearchResultsPaging(string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_IQStationID, string p_Class, int? p_Region, int? p_Country, bool p_IsAsc, bool p_IsNext, string p_SortColumn, List<string> p_SearchLogo, List<string> p_Brand, List<string> p_Industry, List<string> p_Company, string p_PaidEarned)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
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

                List<IQAgent_TVFullResultsModel> tvResult = new List<IQAgent_TVFullResultsModel>();
                bool isallDmaAllowed = tAdsTempData.p_IsAllDmaAllowed;
                bool isallClassAllowed = tAdsTempData.p_IsAllClassAllowed;
                bool isallStationAllowed = tAdsTempData.p_IsAllStationAllowed;

                TVLogic tvlogic = new TVLogic();
                var TAdsIQStations = tvlogic.GetTAdsStations();
                if (p_IQStationID == null || p_IQStationID.Count == 0) p_IQStationID = TAdsIQStations;
                else
                {
                    p_IQStationID = p_IQStationID.Where(x => TAdsIQStations.Contains(x)).ToList();
                    if (p_IQStationID == null || p_IQStationID.Count == 0) p_IQStationID = new List<string>();
                }

                // If not filtering by Industry, check if the client is restricted to a subset of industries
                if (p_Industry == null && tAdsTempData.VisibleLRIndustries != null)
                {
                    p_Industry = tAdsTempData.VisibleLRIndustries;
                }

                var dicTVResult = tvlogic.TAdsSearchResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, p_SearchTerm, p_Title, p_FromDate, p_ToDate, p_Dma, p_Station, p_IQStationID, p_Class, p_IsAsc, pagenumber, isallDmaAllowed, isallClassAllowed, isallStationAllowed, p_SortColumn, ref ResultCount, CommonFunctions.GeneratePMGUrl(CommonFunctions.PMGUrlType.MT.ToString(), p_FromDate, p_ToDate), tAdsTempData.IQTVRegion, tAdsTempData.IQTVCountry, p_Region, p_Country, p_SearchLogo, p_Brand, p_Industry, p_Company, p_PaidEarned);
                tvResult = (List<IQAgent_TVFullResultsModel>)dicTVResult["result"];
                
                var logoHits = new List<Dictionary<string, string>>();
                var adHits = new List<Dictionary<string, string>>();
                if (tvResult != null && ResultCount > 0)
                {
                    foreach (var result in tvResult)
                    {
                        logoHits.AddRange(result.Logos.Distinct().Select(x => new Dictionary<string, string> { { result.IQ_CC_Key, x } }).ToList());
                        adHits.AddRange(result.Ads.Distinct().Select(x => new Dictionary<string, string> { { result.IQ_CC_Key, x } }).ToList());
                    }

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

                string htmlResult = RenderPartialToString(PATH_TAdsPartialView, tvResult);

                return Json(new
                {
                    HTML = htmlResult != null ? htmlResult : "",
                    hasMoreResult = hasMoreResultPage,
                    hasPreviouResult = hasPreviousResultPage,
                    recordNumber = recordNumberDesc,
                    logoHits = logoHits,
                    adHits = adHits,
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
        public ContentResult GetChart(string IQ_CC_KEY, Guid RAW_MEDIA_GUID, List<int> lstSearchTermHits, List<string> lstLogoHitStrings, List<string> lstAdHitStrings, bool sortByHitStart, bool feedsDrillDown = false)
        {
            try
            {
                sessionInformation = ActiveUserMgr.GetActiveUser();

                if (feedsDrillDown && (sessionInformation.isv5LRAccess || sessionInformation.Isv5AdsAccess))
                {
                    var result = GetTadsResultByIQCCKey(IQ_CC_KEY, new DateTime(2016,1,1), DateTime.Today);
                    if (result != null)
                    {
                        if (sessionInformation.isv5LRAccess) lstLogoHitStrings = result.Logos;
                        if (sessionInformation.Isv5AdsAccess) lstAdHitStrings = result.Ads;
                    }
                }


                TVLogic tvlogic = new TVLogic();
                var lstAdHits = ParseAdHitStrings(lstAdHitStrings);
                var lstLogoHits = ParseLogoHitStrings(lstLogoHitStrings);

                List<string> yAxisCompanies = new List<string>();
                string tAdsResult_string = tvlogic.TAdsHighLineChart(IQ_CC_KEY, lstAdHits, lstSearchTermHits, lstLogoHits, sortByHitStart, out yAxisCompanies);

                bool HasResults = (lstAdHits != null && lstAdHits.Count > 0) || (lstSearchTermHits != null && lstSearchTermHits.Count > 0) || (lstLogoHits != null && lstLogoHits.Count > 0);

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

                dynamic jsonResult = new ExpandoObject();
                jsonResult.yAxisCompanies = yAxisCompanies;
                jsonResult.tAdsResultsJson = tAdsResult_string;
                jsonResult.hasTAdsResults = HasResults;
                jsonResult.lineChartJson = lstiQTimeSync_DataModel;
                jsonResult.isTimeSync = IsTimeSync;
                jsonResult.isSuccess = true;

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                
                return Content(IQMedia.WebApplication.Utility.CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("TAdsTempData");
            }
        }
        public List<IQTAdsHit> ParseAdHitStrings(List<string> lstAdHitStrings)
        {
            var returnAds = new List<IQTAdsHit>();
            int startHour = 0;
            bool firstRun = true;

            if (lstAdHitStrings != null)
            {
                foreach (var ad in lstAdHitStrings)
                {
                    if (ad.IndexOf("TO") > 0)
                    {
                        string part1 = ad.Substring(1, ad.IndexOf("TO") - 2);
                        string part2 = ad.Substring(ad.IndexOf("TO") + 3);
                        part2 = part2.Substring(0, part2.Length - 1);

                        DateTime rangeStart = String.IsNullOrEmpty(part1.Trim()) ? DateTime.MinValue : DateTime.Parse(part1.Trim());
                        DateTime rangeEnd = String.IsNullOrEmpty(part2.Trim()) ? DateTime.MinValue : DateTime.Parse(part2.Trim());

                        if(firstRun) 
                        {
                            firstRun = false;
                            startHour = rangeStart.Hour;
                        }

                        if (rangeStart > DateTime.MinValue && rangeEnd > DateTime.MinValue)
                        {
                            var range = new IQTAdsHit();
                            range.startOffset = ((rangeStart.Hour - startHour) * 60 * 60) + (rangeStart.Minute * 60) + rangeStart.Second;
                            if (rangeEnd.Hour == 0 && rangeStart.Hour != 0) //if ad block ends on exactly 0.0.0, convert it back to the 24th hour so that it can be subtracted properly
                            {
                                range.endOffset = ((24 - startHour) * 60 * 60) + (rangeEnd.Minute * 60) + rangeEnd.Second;
                            }
                            else
                            {
                                range.endOffset = ((rangeEnd.Hour - startHour) * 60 * 60) + (rangeEnd.Minute * 60) + rangeEnd.Second;                              
                            }
                            returnAds.Add(range);
                        }
                    }
                }
            }
            return returnAds;
        }
        public List<ImagiQLogoModel> ParseLogoHitStrings(List<string> lstLogoHitStrings)
        {
            var returnLogos = new List<ImagiQLogoModel>();
            tAdsTempData = GetTempData();
            if (Session["FullSearchLogoList"] == null && Session["FullBrandList"] == null)
            {
                GetFilters();
            }

            if (lstLogoHitStrings != null)
            {
                foreach (var logo in lstLogoHitStrings)
                {
                    var brandStart = logo.IndexOf("brand:");
                    var companyStart = logo.IndexOf("company:");
                    var industryStart = logo.IndexOf("industry:");
                    var offsetStart = logo.IndexOf("offset:");
                    var dateStart = logo.IndexOf("date:");

                    if (brandStart > 0 && companyStart > 0 && industryStart > 0 && industryStart > 0 && offsetStart > 0 && dateStart > 0)
                    {
                        brandStart += 6;
                        companyStart += 8;
                        industryStart += 9;
                        offsetStart += 7;
                        dateStart += 5;

                        var logoID = logo.Substring(0, logo.IndexOf("brand:") - 1);
                        var brandID = logo.Substring(brandStart, logo.IndexOf("company:") - 1 - brandStart);
                        var companyID = logo.Substring(companyStart, logo.IndexOf("industry:") - 1 - companyStart);
                        var industry = logo.Substring(industryStart, logo.IndexOf("offset:") - 1 - industryStart);
                        var offset = logo.Substring(offsetStart, logo.IndexOf("date:") - 1 - offsetStart);
                        var date = logo.Substring(dateStart);

                        var _FullBrandList = Session["FullBrandList"] != null ? (List<IQ_Brand>)Session["FullBrandList"] : new List<IQ_Brand>();
                        var _FullSearchLogoList = Session["FullSearchLogoList"] != null ? (List<IQ_Logo>)Session["FullSearchLogoList"] : new List<IQ_Logo>();
                        var fullbrand = _FullBrandList.Where(e => e.ID == brandID.Trim());
                        var fulllogo = _FullSearchLogoList.Where(e => e.ID == logoID.Trim());

                        if (fullbrand != null && fullbrand.Any() && fulllogo != null && fulllogo.Any() && offset.Trim().Length > 0)
                        {
                            //if industries has no restrictions OR this logo's industry matches the ristricted list of industries
                            if (tAdsTempData.VisibleLRIndustries == null || tAdsTempData.VisibleLRIndustries.Count == 0 || (tAdsTempData.VisibleLRIndustries.Contains(industry)))
                            {
                                var logoItem = new ImagiQLogoModel();

                                //Logo
                                logoItem.ID = Int64.Parse(logoID);
                                logoItem.HitLogoPath = fulllogo.First().URL;

                                //Brand 
                                logoItem.CompanyName = fullbrand.First().Name;
                                logoItem.ThumbnailPath = fullbrand.First().URL;

                                //Offset
                                logoItem.Offset = Int32.Parse(offset.Trim());

                                returnLogos.Add(logoItem);
                            }
                        }
                    }
                }

                if (returnLogos.Any()) returnLogos = returnLogos.OrderBy(x => x.CompanyName).ThenBy(x => x.Offset).ThenByDescending(x => x.ID).ToList();
            }
            return returnLogos;
        }

        private bool HasMoreResults(Int64 TotalRecords, Int64 shownRecords)
        {
            if (shownRecords < TotalRecords)
                return true;
            else
                return false;
        }

        [HttpPost]
        public JsonResult GetRawData(string iqcckey)
        {
            try
            {
                TVLogic logic = new TVLogic();
                var result = logic.GetRawData(iqcckey);

                if (result != null && result.Count == 2)
                {
                    Session["xmlFileLoc"] = result.First();
                    Session["tgzFileLoc"] = result.Last();

                    return Json(new
                    {
                        isSuccess = true
                    });
                }

                return Json(new
                {
                    isSuccess = false
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
                TempData.Keep("TAdsTempData");
            }
        }

        [HttpGet]
        public ActionResult GetRawDataXML()
        {
            if (Session["xmlFileLoc"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["xmlFileLoc"])))
            {
                string fileLoc = Convert.ToString(Session["xmlFileLoc"]);

                if (System.IO.File.Exists(fileLoc))
                {
                    Session.Remove("xmlFileLoc");
                    return File(fileLoc, "text/xml", Path.GetFileName(fileLoc));
                }
            }

            return Content(ConfigSettings.Settings.FileNotAvailable);
        }

        [HttpGet]
        public ActionResult GetRawDataTGZ()
        {
            if (Session["tgzFileLoc"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["tgzFileLoc"])))
            {
                string fileLoc = Convert.ToString(Session["tgzFileLoc"]);

                if (System.IO.File.Exists(fileLoc))
                {
                    Session.Remove("tgzFileLoc");
                    return File(fileLoc, "application/x-compressed", Path.GetFileName(fileLoc));
                }
            }

            return Content(ConfigSettings.Settings.FileNotAvailable);
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
