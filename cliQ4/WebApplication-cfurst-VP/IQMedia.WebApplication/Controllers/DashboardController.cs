using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using HtmlAgilityPack;
using IQMedia.Model;
using IQMedia.Shared.Utility;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.WebApplication.Config;
using IQMedia.WebApplication.Models;
using Newtonsoft.Json;
using HiQPdf;
using IQCommon.Model;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        #region Public Property

        ActiveUser sessionInformation = null;
        string PATH_DashboardBroadCastPartialView = "~/Views/Dashboard/_Media.cshtml";
        string PATH_DashboardOverviewPartialView = "~/Views/Dashboard/_Overview.cshtml";
        string PATH_DashboardTopBroadcastDMA = "~/Views/Dashboard/_TopBroadcastDMA.cshtml";
        string PATH_DashboardTopBroadcastStation = "~/Views/Dashboard/_TopBroadcastStation.cshtml";
        string PATH_DashboardTopOnlineNewsDMA = "~/Views/Dashboard/_TopOnlineNewsDMA.cshtml";
        string PATH_DashboardTopOnlineNewsSites = "~/Views/Dashboard/_TopOnlineNewsSites.cshtml";
        string PATH_DashboardTopBroadcastCountries = "~/Views/Dashboard/_TopBroadcastCountries.cshtml";

        #endregion

        public ActionResult Index()
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                List<IQAgent_SearchRequestModel> lstIQAgent_SearchRequestModel = iQAgentLogic.SelectIQAgentSearchRequestByClientGuid(sessionInformation.ClientGUID.ToString());
                TempData["Agents"] = lstIQAgent_SearchRequestModel;

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                IQClient_CustomSettingsModel customSettings = clientLogic.GetClientCustomSettings(sessionInformation.ClientGUID.ToString());
                TempData["CustomSettings"] = customSettings;

                ThirdPartyLogic thirdPartyLogic = (ThirdPartyLogic)LogicFactory.GetLogic(LogicType.ThirdParty);
                List<ThirdPartyDataTypeModel> lstThirdPartyDataTypeModel = thirdPartyLogic.GetThirdPartyDataTypesWithCustomerSelection(sessionInformation.CustomerGUID);
                TempData["ThirdPartyDataTypes"] = lstThirdPartyDataTypeModel;

                Dictionary<string, object> dictViewModel = new Dictionary<string, object>();
                dictViewModel.Add("Agents", lstIQAgent_SearchRequestModel);
                dictViewModel.Add("ThirdPartyDataTypes", lstThirdPartyDataTypeModel);
                dictViewModel.Add("HasThirdPartyDataAccess", sessionInformation.IsThirdPartyData);
                dictViewModel.Add("UseCustomerEmailAsDefault", customSettings.UseCustomerEmailDefault.Value);
                dictViewModel.Add("DefaultEmailSender", customSettings.UseCustomerEmailDefault.Value ? sessionInformation.Email : ConfigurationManager.AppSettings["Sender"]);

                ViewBag.MaxEmailAddresses = System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"];
                ViewBag.IsSuccess = true;
                return View(dictViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.IsSuccess = false;
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(exception);
                return View();
            }
            finally
            {
                TempData.Keep();
            }
        }
        
        /*
        [HttpPost]
        public ContentResult SummaryReportResults(DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_SearchRequestIDs, Int16 p_SearchType)
        {


            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                DateTime FromDate;
                DateTime ToDate;

                if (p_FromDate == null)
                {
                    FromDate = DateTime.Today.AddDays(-30);
                    ToDate = DateTime.Today;
                }
                else
                {
                    FromDate = Convert.ToDateTime(p_FromDate);
                    ToDate = Convert.ToDateTime(p_ToDate);
                }

                IQAgent_DashBoardPrevSummaryModel PrevIQAgentSummary = null;
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                List<SummaryReportModel> listOfSummaryReportData = GetSummaryReportData(sessionInformation.ClientGUID, FromDate, ToDate, p_SearchType, p_SearchRequestIDs, p_ThirdPartyDataTypeIDs, out PrevIQAgentSummary);

                return GetJsonSummaryWise(listOfSummaryReportData, FromDate, ToDate, p_SearchType, p_SearchRequestIDs, PrevIQAgentSummary);
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);

                return Content(IQMedia.WebApplication.Utility.CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
        }*/

        /*
        private List<string> MultiLinechart(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate)
        {
            DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
            List<string> lstMultiLineChart = dashboardLogic.MultiLinechart(listOfSummaryReportData, p_FromDate, p_ToDate);
            return lstMultiLineChart;
        }*/

        [HttpPost]
        public ContentResult GetAdhocSummaryData(string mediaIDs, string source, DateTime? fromDate, DateTime? toDate, List<string> searchRequestID, List<string> mediumTypes, string keyword, short? sentiment, short? prominenceValue, bool? isProminenceAudience, bool? isOnlyParents, bool? isRead, long? sinceID)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                List<IQAgent_DaySummaryModel> listOfSummaryReportData = null;
                bool isValidResponse = true;
                if (!isOnlyParents.HasValue && !sinceID.HasValue)
                {
                    string mediaIDXml = null;
                    if (mediaIDs != null)
                    {
                        XDocument doc = new XDocument(new XElement("list", from i in mediaIDs.Split(',') select new XElement("item", new XAttribute("id", i))));
                        mediaIDXml = doc.ToString();
                    }

                    DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                    listOfSummaryReportData = dashboardLogic.GetAdhocSummaryData(mediaIDXml, source, null, sessionInformation.ClientGUID, sessionInformation.MediaTypes).ListOfIQAgentSummary;
                }
                else
                {
                    if (fromDate.HasValue && toDate.HasValue)
                    {
                        fromDate = Utility.CommonFunctions.GetGMTandDSTTime(fromDate);
                        toDate = Utility.CommonFunctions.GetGMTandDSTTime(toDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
                    }
                    else
                    {
                        fromDate = Utility.CommonFunctions.GetGMTandDSTTime(DateTime.Now.Date.AddMonths(-3));
                        toDate = Utility.CommonFunctions.GetGMTandDSTTime(DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
                    }   

                    List<string> excludeMediumTypes = new List<string>();
                    if (mediumTypes == null || mediumTypes.Count == 0)
                    {
                        foreach (var mt in sessionInformation.MediaTypes.Where(m=>m.TypeLevel == 2 && !m.HasAccess))
                        {
                            excludeMediumTypes.Add(mt.SubMediaType);
                        }
                    }
                    
                    IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                    string pmgUrl = Utility.CommonFunctions.GeneratePMGUrl(Utility.CommonFunctions.PMGUrlType.FE.ToString(), null, null);

                    List<Task> lstTask = new List<Task>();
                    var tokenSource = new CancellationTokenSource();
                    var token = tokenSource.Token;
                    Dictionary<string, object> dictResults = new Dictionary<string, object>();
                    dictResults.Add("IsValid", false);
                    
                    lstTask.Add(Task<Dictionary<string, object>>.Factory.StartNew((object obj) =>
                        iQAgentLogic.SearchDashboardResults(sessionInformation.CustomerKey, sessionInformation.ClientGUID, (mediaIDs != null ? mediaIDs.Split(',').ToList() : null), fromDate, toDate, searchRequestID, mediumTypes, excludeMediumTypes, keyword, sentiment, prominenceValue, isProminenceAudience.Value, 
                                                                isOnlyParents.Value, isRead, sinceID, pmgUrl, token, dictResults),
                            dictResults));

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
                        dictResults = ((Task<Dictionary<string, object>>)tsk).Result;
                    }

                    if ((bool)dictResults["IsValid"])
                    {
                        listOfSummaryReportData = (List<IQAgent_DaySummaryModel>)dictResults["Results"];

                        // Set MediaType

                        foreach (var sm in listOfSummaryReportData)
                        {
                            sm.MediaType = sessionInformation.MediaTypes.Where(mt => string.Compare(mt.SubMediaType, sm.SubMediaType, true) == 0 && mt.HasAccess).Single().MediaType;
                        }
                    }
                    else
                    {
                        isValidResponse = false;
                    }
                }

                if (isValidResponse)
                {
                    DateTime fDate;
                    DateTime tDate;
                    Int16 searchType;
                    bool isUGCEnabled = false;                    

                    // Only display UGC data if coming from Library/Reports
                    if (source.ToLower() == "library" || source.ToLower() == "report")
                    {
                        isUGCEnabled = sessionInformation.Isv4UGCAccess;
                    }

                    fDate = listOfSummaryReportData.Min(x => x.DayDate);
                    tDate = listOfSummaryReportData.Max(x => x.DayDate);

                    if ((tDate - fDate).Days > 2)
                    {
                        searchType = 1;
                    }
                    else
                    {
                        searchType = 0;

                        // The line chart doesn't render correctly when only displaying a single data point
                        if (fDate == tDate)
                        {
                            tDate = tDate.AddHours(1);
                        }
                    }

                    List<SummaryReportModel> lstSummaryReport = listOfSummaryReportData.Select(s => new SummaryReportModel()
                    {
                        Audience = s.Audience,
                        GMT_DateTime = s.DayDate,
                        IQMediaValue = s.IQMediaValue,
                        MediaType = s.MediaType,
                        SubMediaType = s.SubMediaType,
                        Number_Docs = s.NoOfDocs,
                        Number_Of_Hits = s.NoOfHits,
                        SearchRequestID = s.SearchRequestID,
                        Query_Name = s.Query_Name,
                        DefaultMediaType = sessionInformation.MediaTypes.Where(mt => string.Compare(mt.MediaType, s.MediaType, true) == 0).Count() > 0 ? true : false

                    }).ToList();                    

                    return GetJsonForAdhocSummary(lstSummaryReport, fDate, tDate, searchType, sessionInformation, isUGCEnabled);
                }
                else
                {
                    dynamic json = new ExpandoObject();
                    json.isSuccess = false;
                    json.errorMessage = "Data not available. Please try again.";

                    return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8); 
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);

                dynamic json = new ExpandoObject();
                json.isSuccess = false;
                json.errorMessage = ConfigSettings.Settings.ErrorOccurred;

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);           
            }
        }

        [HttpPost]
        public JsonResult SaveThirdPartyDataTypeSelections(List<string> dataTypeIDs)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                ThirdPartyLogic thirdPartyLogic = (ThirdPartyLogic)LogicFactory.GetLogic(LogicType.ThirdParty);
                int success = thirdPartyLogic.SaveThirdPartyDataTypeSelections(sessionInformation.CustomerGUID, dataTypeIDs);

                return Json(new
                {
                    isSuccess = success >= 0
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
        }

        #region SummaryReport
        public List<SummaryReportModel> GetSummaryReportData(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, List<string> p_SearchRequestIDs, List<string> p_ThirdPartyDataTypeIDs, out IQAgent_DashBoardPrevSummaryModel iQAgent_DashBoardPrevSummaryModel, List<IQ_MediaTypeModel> p_MediaTypeList)
        {
            try
            {
                string searchRequestXml = null;
                iQAgent_DashBoardPrevSummaryModel = new IQAgent_DashBoardPrevSummaryModel();
                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    XDocument doc = new XDocument(new XElement("list", from i in p_SearchRequestIDs select new XElement("item", new XAttribute("id", i))));
                    searchRequestXml = doc.ToString();
                }

                DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                List<SummaryReportModel> lstSummaryReport = null;
                if (p_SearchType == 0)
                {
                    IQAgent_DashBoardModel iQAgent_DashBoardModel = dashboardLogic.GetHourSummaryDataHourWise(p_ClientGUID, p_FromDate, p_ToDate, null, searchRequestXml, p_MediaTypeList);
                    iQAgent_DashBoardPrevSummaryModel = iQAgent_DashBoardModel.PrevIQAgentSummary;
                    lstSummaryReport = iQAgent_DashBoardModel.ListOfIQAgentSummary.Select(s => new SummaryReportModel()
                    {
                        Audience = s.Audience,
                        GMT_DateTime = s.DayDate,
                        IQMediaValue = s.IQMediaValue,
                        MediaType = s.MediaType,
                        SubMediaType = s.SubMediaType,
                        Number_Docs = s.NoOfDocs,
                        Number_Of_Hits = s.NoOfHits,
                        SearchRequestID = s.SearchRequestID,
                        Query_Name = s.Query_Name,
                        DefaultMediaType = p_MediaTypeList.Where(mt => string.Compare(mt.MediaType, s.MediaType,true) == 0).Count() > 0

                    }).ToList();
                }
                else if (p_SearchType == 1)
                {
                    IQAgent_DashBoardModel iQAgent_DashBoardModel = dashboardLogic.GetDaySummaryDataDayWise(p_ClientGUID, p_FromDate, p_ToDate, null, searchRequestXml, p_MediaTypeList);
                    iQAgent_DashBoardPrevSummaryModel = iQAgent_DashBoardModel.PrevIQAgentSummary;
                    lstSummaryReport = iQAgent_DashBoardModel.ListOfIQAgentSummary.Select(s => new SummaryReportModel()
                    {
                        Audience = s.Audience,
                        GMT_DateTime = s.DayDate,
                        IQMediaValue = s.IQMediaValue,
                        MediaType = s.MediaType,
                        SubMediaType = s.SubMediaType,
                        Number_Docs = s.NoOfDocs,
                        Number_Of_Hits = s.NoOfHits,
                        SearchRequestID = s.SearchRequestID,
                        Query_Name = s.Query_Name,
                        DefaultMediaType = p_MediaTypeList.Where(mt => string.Compare(mt.MediaType, s.MediaType, true) == 0).Count() > 0

                    }).ToList();

                }
                else if (p_SearchType == 3)
                {
                    IQAgent_DashBoardModel iQAgent_DashBoardModel = dashboardLogic.GetDaySummaryDataMonthWise(p_ClientGUID, p_FromDate, p_ToDate, null, searchRequestXml, p_MediaTypeList);
                    iQAgent_DashBoardPrevSummaryModel = iQAgent_DashBoardModel.PrevIQAgentSummary;
                    lstSummaryReport = iQAgent_DashBoardModel.ListOfIQAgentSummary.Select(s => new SummaryReportModel()
                    {
                        Audience = s.Audience,
                        GMT_DateTime = s.DayDate,
                        IQMediaValue = s.IQMediaValue,
                        MediaType = s.MediaType,
                        SubMediaType = s.SubMediaType,
                        Number_Docs = s.NoOfDocs,
                        Number_Of_Hits = s.NoOfHits,
                        SearchRequestID = s.SearchRequestID,
                        Query_Name = s.Query_Name,
                        DefaultMediaType = p_MediaTypeList.Where(mt => string.Compare(mt.MediaType, s.MediaType, true) == 0).Count() > 0

                    }).ToList();
                }

                if (p_ThirdPartyDataTypeIDs != null && p_ThirdPartyDataTypeIDs.Count > 0)
                {
                    ThirdPartyLogic thirdPartyLogic = (ThirdPartyLogic)LogicFactory.GetLogic(LogicType.ThirdParty);
                    List<ThirdPartyDataTypeModel> lstDataTypeModels = (List<ThirdPartyDataTypeModel>)TempData["ThirdPartyDataTypes"];
                    foreach (ThirdPartyDataTypeModel dataTypeModel in lstDataTypeModels.Where(w => p_ThirdPartyDataTypeIDs.Contains(w.ID.ToString())))
                    {
                        if (p_SearchType != 0 || dataTypeModel.UseHourData)
                        {
                            List<SummaryReportModel> lstThirdPartySummaries = thirdPartyLogic.GetThirdPartySummaryData(p_ClientGUID, dataTypeModel, p_FromDate, p_ToDate, p_SearchType, p_SearchRequestIDs);
                            if (lstThirdPartySummaries != null && lstThirdPartySummaries.Count > 0)
                            {
                                lstSummaryReport.AddRange(lstThirdPartySummaries);
                            }
                            else
                            {
                                // If no data exists for the given time range, add a dummy record so that a series is still created
                                lstSummaryReport.Add(new SummaryReportModel()
                                {
                                    GMT_DateTime = DateTime.Now,
                                    Number_Docs = 0,
                                    ThirdPartyDataTypeID = dataTypeModel.ID
                                });
                            }
                        }
                    }
                }

                return lstSummaryReport;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep();
            }
        }
        #endregion

        #region BroadCast

        [HttpPost]
        public ContentResult GetMediumData(DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_SearchRequestIDs, string p_Medium, Int16 p_SearchType, bool p_IsDefaultLoad, List<string> p_ThirdPartyDataTypeIDs)
        {
            /*
             * 0- Hourly
             * 1 - Day
             * 2 - Week
             * 3 - Month
             */
            try
            {

                DateTime FromDate;
                DateTime ToDate;

                if (p_FromDate == null || p_IsDefaultLoad)
                {
                    FromDate = DateTime.Today.AddDays(-30);
                    ToDate = DateTime.Today;
                }
                else
                {
                    FromDate = Convert.ToDateTime(p_FromDate);
                    ToDate = Convert.ToDateTime(p_ToDate);
                }

                if (p_SearchType == 0)
                {
                    if (p_IsDefaultLoad)
                    {
                        ToDate = DateTime.Today.AddHours(23).AddMinutes(59);
                        FromDate = DateTime.Today.AddDays(-1);
                    }
                    else
                    {
                        ToDate = ToDate.Date.AddHours(23).AddMinutes(59);
                    }
                }
                else if (p_SearchType == 3)
                {
                    FromDate = new DateTime(FromDate.Year, FromDate.Month, 1);
                    ToDate = new DateTime(ToDate.Year, ToDate.Month, DateTime.DaysInMonth(ToDate.Year, ToDate.Month));
                }

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                if (p_Medium.ToLower() == IQMedia.Shared.Utility.CommonFunctions.DashBoardMediumType.Overview.ToString().ToLower())
                {
                    IQAgent_DashBoardPrevSummaryModel PrevIQAgentSummary = null;
                    List<SummaryReportModel> listOfSummaryReportData = GetSummaryReportData(sessionInformation.ClientGUID, FromDate, ToDate, p_SearchType, p_SearchRequestIDs, p_ThirdPartyDataTypeIDs, out PrevIQAgentSummary, sessionInformation.MediaTypes);

                    return GetJsonSummaryWise(listOfSummaryReportData, FromDate, ToDate, p_SearchType, p_SearchRequestIDs, PrevIQAgentSummary,sessionInformation);
                }
                else
                {
                    IQAgent_DashBoardModel iQAgent_DashBoardModel = GetDaySummaryMediumWise(sessionInformation.ClientGUID, FromDate, ToDate, p_Medium, p_SearchType, p_SearchRequestIDs, sessionInformation.MediaTypes);

                    var mediaType = sessionInformation.MediaTypes.Where(mt => mt.TypeLevel == 1 && string.Compare(p_Medium, mt.MediaType, true) == 0).Single();

                    if (mediaType.HasAccess)
                    {
                        return GetJsonMediumWise(mediaType, iQAgent_DashBoardModel, FromDate, ToDate, p_SearchType, p_SearchRequestIDs, sessionInformation); 
                    }
                    else
                    {
                        throw new CustomException("User doesn't have requested media type \""+ mediaType.MediaType+"\" access.");
                    }
                }
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);

                return Content(IQMedia.WebApplication.Utility.CommonFunctions.GetSuccessFalseJson(),"application/json", Encoding.UTF8);                
            }
        }

        #endregion

        #region Download PDF

        [HttpPost]
        public JsonResult GenerateDashboardPDF()
        {
            try
            {
                Request.InputStream.Position = 0;
                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                string HTML = dictParams["p_HTML"].ToString();
                string fromDate = dictParams["p_FromDate"].ToString();
                string toDate = dictParams["p_ToDate"].ToString();
                string searchRequests = dictParams["p_SearchRequests"].ToString();

                string html = GetHTMLWithCSSIncluded(HTML, fromDate, toDate, false, searchRequests);

                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                string TempPDFPath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Dashboard\\PDF\\" + sessionInformation.CustomerGUID + "_" + DateTimeStamp + ".pdf";

                bool IsFileGenerated = false;

                HtmlToPdf htmlToPdfConverter = new HtmlToPdf();
                htmlToPdfConverter.SerialNumber = ConfigurationManager.AppSettings["HiQPdfSerialKey"];
                htmlToPdfConverter.Document.Margins = new PdfMargins(20);
                htmlToPdfConverter.BrowserWidth = 1000;
                htmlToPdfConverter.ConvertHtmlToFile(html, String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority), TempPDFPath);

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
        public JsonResult SendDashBoardEmail()
        {
            try
            {
                Request.InputStream.Position = 0;

                Dictionary<string, object> dictParams;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                string HTML = dictParams["p_HTML"].ToString();
                string fromDate = dictParams["p_FromDate"].ToString();
                string toDate = dictParams["p_ToDate"].ToString();
                string fromEmail = dictParams["p_FromEmail"].ToString();
                string toEmail = dictParams["p_ToEmail"].ToString();
                string bccEmail = dictParams["p_BCCEmail"].ToString();
                string subject = dictParams["p_Subject"].ToString();
                string userBody = dictParams["p_UserBody"].ToString();
                string searchRequests = dictParams["p_SearchRequests"].ToString();

                if (toEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"]) &&
                        (String.IsNullOrEmpty(bccEmail) || bccEmail.Split(new char[] { ';' }).Count() <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxDefaultEmailAddress"])))
                {
                    int EmailSendCount = 0;
                    string[] bccEmails = !String.IsNullOrWhiteSpace(bccEmail) ? bccEmail.Split(new char[] { ';' }) : new string[0];

                    string html = GetHTMLWithCSSIncluded(HTML, fromDate, toDate, true, searchRequests);

                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                    string DateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    string TempImagePath = ConfigurationManager.AppSettings["TempHTML-PDFPath"] + "Download\\Dashboard\\PDF\\" + sessionInformation.CustomerGUID + "_" + DateTimeStamp + ".jpg";

                    HtmlToImage htmlToImageConverter = new HtmlToImage();
                    htmlToImageConverter.SerialNumber = ConfigurationManager.AppSettings["HiQPdfSerialKey"];
                    htmlToImageConverter.BrowserWidth = 1000;
                    Image img = htmlToImageConverter.ConvertHtmlToImage(html, String.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority))[0];

                    img.Save(TempImagePath);

                    string attachmentId = Path.GetFileName(TempImagePath);
                    userBody = userBody + "<br/>" + "<img src=\"cid:" + attachmentId + "\" alt='Dashboard'/>";

                    StreamReader strmEmailPolicy = new StreamReader(Server.MapPath("~/content/EmailPolicy.txt"));
                    string emailPolicy = strmEmailPolicy.ReadToEnd();
                    strmEmailPolicy.Close();
                    strmEmailPolicy.Dispose();
                    userBody = userBody + emailPolicy;

                    string[] alternetViewsName = new string[1];
                    alternetViewsName[0] = TempImagePath;

                    if (!string.IsNullOrEmpty(toEmail))
                    {
                        foreach (string id in toEmail.Split(new char[] { ';' }))
                        {
                            // send email code

                            if (IQMedia.Shared.Utility.CommonFunctions.SendMail(id, string.Empty, bccEmails, fromEmail, subject, userBody, true, null, alternetViewsName))
                            {
                                EmailSendCount++;
                            }
                        }
                    }

                    img.Dispose();
                    if (System.IO.File.Exists(TempImagePath))
                    {
                        System.IO.File.Delete(TempImagePath);
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

        #region Compare Province

        public JsonResult CompareProvince(DateTime p_FromDate, DateTime p_ToDate, List<string> p_SearchRequests, List<DashboardDMAChartSelectionModel> p_Provinces, Int16 p_SearchType, string p_Medium)
        {
            try
            {
                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                string searchRequestXml = null;
                string provinceXml = null;

                if (p_SearchRequests != null && p_SearchRequests.Count > 0)
                {
                    XDocument doc = new XDocument(new XElement("list", from i in p_SearchRequests select new XElement("item", new XAttribute("id", i))));
                    searchRequestXml = doc.ToString();
                }

                if (p_Provinces != null && p_Provinces.Count > 0)
                {
                    p_Provinces = IQProvinceToFusionIDMapModel.IQProvinceToFusionIDMap.Join(p_Provinces, a => a.Value.ToString(), b => b.id, (a, b) => new DashboardDMAChartSelectionModel { id = a.Key, clickColor = b.clickColor }).ToList();
                    XDocument doc = new XDocument(new XElement("list", from i in p_Provinces select new XElement("item", new XAttribute("province", i.id))));
                    provinceXml = doc.ToString();
                }

                string noOfDocs = "";
                string noOfHits = "";
                string noOfMinofAiring = "";
                string noOfView = "";

                DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = null;
                if (p_SearchType == 0)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetProvinceSummaryDataHourWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, provinceXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces, sessionInformation.gmt, sessionInformation.dst);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces, sessionInformation.gmt, sessionInformation.dst);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces, sessionInformation.gmt, sessionInformation.dst);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces, sessionInformation.gmt, sessionInformation.dst);
                    }
                }
                else if (p_SearchType == 1)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetProvinceSummaryDataDayWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, provinceXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    }
                }
                else if (p_SearchType == 3)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetProvinceSummaryDataMonthWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, provinceXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Provinces);
                    }
                }

                var json = new
                {
                    noOfDocsJson = noOfDocs,
                    noOfHitsJson = noOfHits,
                    noOfMinOfAiringJson = noOfMinofAiring,
                    noOfViewJson = noOfView,
                    isSuccess = true
                };
                return Json(json);
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

        #endregion

        #region Compare DMA

        public JsonResult CompareDma(DateTime p_FromDate, DateTime p_ToDate, List<string> p_SearchRequests, List<DashboardDMAChartSelectionModel> p_Dmas, Int16 p_SearchType, string p_Medium)
        {
            try
            {

                sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

                string searchRequestXml = null;
                string dmaXml = null;

                if (p_SearchRequests != null && p_SearchRequests.Count > 0)
                {
                    XDocument doc = new XDocument(new XElement("list", from i in p_SearchRequests select new XElement("item", new XAttribute("id", i))));
                    searchRequestXml = doc.ToString();
                }

                if (p_Dmas != null && p_Dmas.Count > 0)
                {
                    p_Dmas = IQDmaToFusionIDMapModel.IQDmaToFusionIDMap.Join(p_Dmas, a => a.Value.ToString(), b => b.id, (a, b) => new DashboardDMAChartSelectionModel { id = a.Key, clickColor = b.clickColor }).ToList();
                    XDocument doc = new XDocument(new XElement("list", from i in p_Dmas select new XElement("item", new XAttribute("dma", i.id))));
                    dmaXml = doc.ToString();
                }

                string noOfDocs = "";
                string noOfHits = "";
                string noOfMinofAiring = "";
                string noOfView = "";

                DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = null;
                if (p_SearchType == 0)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetDmaSummaryDataHourWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, dmaXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas, sessionInformation.gmt, sessionInformation.dst);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas, sessionInformation.gmt, sessionInformation.dst);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas, sessionInformation.gmt, sessionInformation.dst);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDmaHourly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas, sessionInformation.gmt, sessionInformation.dst);
                    }

                }
                else if (p_SearchType == 1)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetDmaSummaryDataDayWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, dmaXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDma(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    }

                }
                else if (p_SearchType == 3)
                {
                    lstIQAgent_DaySummaryModel = dashboardLogic.GetDmaSummaryDataMonthWise(sessionInformation.ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, dmaXml);
                    noOfDocs = dashboardLogic.GetHighChartsForDocsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    noOfHits = dashboardLogic.GetHighChartsForHitsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    noOfView = dashboardLogic.GetHighChartsForViewsForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);

                    if (p_Medium == CommonFunctions.CategoryType.TV.ToString())
                    {
                        noOfMinofAiring = dashboardLogic.GetHighChartsForMinutesOfAiringForDmaMonthly(lstIQAgent_DaySummaryModel, p_FromDate, p_ToDate, p_Dmas);
                    }
                }

                var json = new
                {
                    noOfDocsJson = noOfDocs,
                    noOfHitsJson = noOfHits,
                    noOfMinOfAiringJson = noOfMinofAiring,
                    noOfViewJson = noOfView,
                    isSuccess = true
                };
                return Json(json);
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

        #endregion

        #region Utility

        public IQAgent_DashBoardModel GetDaySummaryMediumWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, Int16 p_SearchType, List<string> p_SearchRequestIDs, List<IQ_MediaTypeModel> p_MediaTypeList)
        {
            try
            {
                string searchRequestXml = null;

                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    XDocument doc = new XDocument(new XElement("list", from i in p_SearchRequestIDs select new XElement("item", new XAttribute("id", i))));
                    searchRequestXml = doc.ToString();
                }

                DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                IQAgent_DashBoardModel iQAgent_DashBoardModel = null;
                if (p_SearchType == 0)
                {
                    iQAgent_DashBoardModel = dashboardLogic.GetHourSummaryDataHourWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, p_MediaTypeList);
                }
                else if (p_SearchType == 1)
                {
                    iQAgent_DashBoardModel = dashboardLogic.GetDaySummaryDataDayWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, p_MediaTypeList);
                }
                else if (p_SearchType == 3)
                {
                    iQAgent_DashBoardModel = dashboardLogic.GetDaySummaryDataMonthWise(p_ClientGUID, p_FromDate, p_ToDate, p_Medium, searchRequestXml, p_MediaTypeList);
                }

                return iQAgent_DashBoardModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetMediumHTML()
        {
            try
            {
                //if (medium.ToLower() == MediumType.TV.ToString().ToLower())
                //{
                return RenderPartialToString(PATH_DashboardBroadCastPartialView, new object());
                //}
            }
            catch (Exception)
            {

                throw;
            }

            return string.Empty;
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

        public ContentResult GetJsonMediumWise(IQ_MediaTypeModel p_MediaType, IQAgent_DashBoardModel iQAgent_DashBoardModel, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, List<string> p_SearchRequestIDs,ActiveUser p_ActiveUser)
        {
            try
            {
                Dictionary<string, object> dictResults = CommonController.GetDashboardMediumResults(p_MediaType, iQAgent_DashBoardModel, p_FromDate, p_ToDate, p_SearchType, p_SearchRequestIDs, true,p_ActiveUser);
                DashboardMediaResults dashboardMediaResults = (DashboardMediaResults)dictResults["MediaResults"];
                dynamic jsonResult = (ExpandoObject)dictResults["JsonResult"];

                var TopDmasHTML = string.Empty;
                var TopStationsHTML = string.Empty;
                var TopOnlineNewsDmasHTML = string.Empty;
                var TopOnlineNewsSitesHTML = string.Empty;
                var TopPrintPublicationsHTML = string.Empty;
                var topPrintAuthorsHTML = string.Empty;
                var topCountriesHTML = string.Empty;

                Func<DataList, List<DashboardTopResultsModel>, string> RenderDataList = (dl,data) =>
                {
                    var templateHTML = "";

                    Dictionary<string, object> dictTopResults = new Dictionary<string, object>();
                    dictTopResults.Add("Results", data);
                    dictTopResults.Add("Medium", p_MediaType);
                    dictTopResults.Add("TitleGrid", dl.Title);
                    dictTopResults.Add("TitleColumn", dl.TitleColumn);
                    dictTopResults.Add("DataType", dl.DataType);
                    dictTopResults.Add("CompeteAccess", p_ActiveUser.IsCompeteData);
                    dictTopResults.Add("NielsenAccess", p_ActiveUser.IsNielsenData);                    

                    switch (dl.TemplateType)
                    {
                        case TemplateTypes.TVDMA:
                            templateHTML = RenderPartialToString(PATH_DashboardTopBroadcastDMA, dictTopResults);
                            break;
                        case TemplateTypes.TVStation:
                            templateHTML = RenderPartialToString(PATH_DashboardTopBroadcastStation, dictTopResults);
                            break;
                        case TemplateTypes.TVCountry:
                            templateHTML = RenderPartialToString(PATH_DashboardTopBroadcastCountries, dictTopResults);
                            break;
                        case TemplateTypes.NMDMA:
                            templateHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsDMA, dictTopResults);
                            break;
                        case TemplateTypes.Common:                            
                            templateHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dictTopResults);
                            break;
                        default:
                            break;
                    }

                    return templateHTML;
                };

                List<Tuple<string, string>> dataLists = new List<Tuple<string, string>>();
                var listHtml = "";

                foreach (DataList dl in p_MediaType.DashboardData.DataLists)
                {
                    switch (dl.ListType)
                    {
                        case ListTypes.Country:
                            listHtml = RenderDataList(dl, iQAgent_DashBoardModel.ListOfTopCountryBroadCast);                            
                            break;
                        case ListTypes.DMA:
                            listHtml = RenderDataList(dl, iQAgent_DashBoardModel.ListOfTopDMABroadCast);
                            break;
                        case ListTypes.Station:
                            listHtml = RenderDataList(dl, iQAgent_DashBoardModel.ListOfTopStationBroadCast);
                            break;
                        default:
                            break;
                    }

                    dataLists.Add(Tuple.Create(dl.TemplateType.ToString(), listHtml));
                }

                /*
                if (p_MediaType == CommonFunctions.CategoryType.TV.ToString())
                {
                    TopDmasHTML = RenderPartialToString(PATH_DashboardTopBroadcastDMA, iQAgent_DashBoardModel.ListOfTopDMABroadCast);
                    TopStationsHTML = RenderPartialToString(PATH_DashboardTopBroadcastStation, iQAgent_DashBoardModel.ListOfTopStationBroadCast);
                    topCountriesHTML = RenderPartialToString(PATH_DashboardTopBroadcastCountries, iQAgent_DashBoardModel.ListOfTopCountryBroadCast);
                }
                else if (p_MediaType != CommonFunctions.CategoryType.Radio.ToString() && p_MediaType != CommonFunctions.CategoryType.PM.ToString())
                {
                    if (p_MediaType == CommonFunctions.CategoryType.NM.ToString())
                    {
                        TopOnlineNewsDmasHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsDMA, iQAgent_DashBoardModel.ListOfTopDMABroadCast);
                    }
                    Dictionary<string, object> dicTopSites = new Dictionary<string, object>();

                    dicTopSites.Add("Results", iQAgent_DashBoardModel.ListOfTopStationBroadCast);
                    dicTopSites.Add("Medium", p_MediaType);
                    TopOnlineNewsSitesHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dicTopSites);
                }
                else if (p_MediaType == IQMedia.Shared.Utility.CommonFunctions.CategoryType.PM.ToString() && sessionInformation.Isv4PQ)
                {
                    Dictionary<string, object> dictTopPubs = new Dictionary<string, object>();
                    dictTopPubs.Add("Results", iQAgent_DashBoardModel.ListOfTopStationBroadCast);
                    dictTopPubs.Add("Medium", p_MediaType);
                    dictTopPubs.Add("TitleGrid", "Publications");
                    dictTopPubs.Add("TitleColumn", "Publication");
                    dictTopPubs.Add("DataType", "pub");
                    TopPrintPublicationsHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dictTopPubs);

                    Dictionary<string, object> dictTopAuthors = new Dictionary<string, object>();
                    dictTopAuthors.Add("Results", iQAgent_DashBoardModel.ListOfTopDMABroadCast);
                    dictTopAuthors.Add("Medium", p_MediaType);
                    dictTopAuthors.Add("TitleGrid", "Authors");
                    dictTopAuthors.Add("TitleColumn", "Author");
                    dictTopAuthors.Add("DataType", "author");
                    topPrintAuthorsHTML = RenderPartialToString(PATH_DashboardTopOnlineNewsSites, dictTopAuthors);
                }
                */
                // Determine if the client has Canadian TV. If not, hide Canadian heat map.
                IQClient_CustomSettingsModel customSettings = (IQClient_CustomSettingsModel)TempData["CustomSettings"];
                List<string> tvCountries = customSettings.IQTVCountry.Split(',').ToList();
                List<string> tvRegions = customSettings.IQTVRegion.Split(',').ToList();
                dashboardMediaResults.ShowCanadaMap = (p_MediaType.DashboardData.UseCanadaMap && (!p_MediaType.DashboardData.CheckCanadaSettings || (tvCountries.Contains("2") && tvRegions.Contains("2"))));

                jsonResult.HTML = RenderPartialToString(PATH_DashboardBroadCastPartialView, dashboardMediaResults);
                /*jsonResult.p_TopDmasHTML = TopDmasHTML;
                jsonResult.p_TopStationsHTML = TopStationsHTML;
                jsonResult.p_TopOnlineNewsDmasHTML = TopOnlineNewsDmasHTML;
                jsonResult.p_TopOnlineNewsSitesHTML = TopOnlineNewsSitesHTML;
                jsonResult.p_TopPrintPublicationsHTML = TopPrintPublicationsHTML;
                jsonResult.p_TopPrintAuthorsHTML = topPrintAuthorsHTML;
                jsonResult.p_TopCountriesHTML = topCountriesHTML;*/
                jsonResult.DataLists = dataLists;
                jsonResult.isSuccess = true;

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep();
            }
        }

        private ContentResult GetJsonSummaryWise(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, List<string> p_SearchRequestIDs, IQAgent_DashBoardPrevSummaryModel p_PrevIQAgentSummary, ActiveUser p_User)
        {
            try
            {
                List<IQAgent_SearchRequestModel> lstAgents = (List<IQAgent_SearchRequestModel>)TempData["Agents"];
                Dictionary<long, string> dictSelectedAgents = new Dictionary<long, string>();

                if (p_SearchRequestIDs != null && p_SearchRequestIDs.Count > 0)
                {
                    dictSelectedAgents = lstAgents.Where(w => p_SearchRequestIDs.Contains(w.ID.ToString()))
                                                .Select(s => new { s.ID, s.QueryName })
                                                .ToDictionary(t => t.ID, t => t.QueryName);
                }

                Dictionary<string, object> dictResults = CommonController.GetDashboardOverviewResults(listOfSummaryReportData, p_FromDate, p_ToDate, p_SearchType, dictSelectedAgents, p_PrevIQAgentSummary, (List<ThirdPartyDataTypeModel>)TempData["ThirdPartyDataTypes"], p_User);
                DashboardOverviewResults dashboardOverviewResults = (DashboardOverviewResults)dictResults["OverviewResults"];
                dynamic jsonResult = (ExpandoObject)dictResults["JsonResult"];

                jsonResult.HTML = RenderPartialToString(PATH_DashboardOverviewPartialView, dashboardOverviewResults);
                jsonResult.isSuccess = true;

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                TempData.Keep();
            }
        }

        private ContentResult GetJsonForAdhocSummary(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, short p_SearchType, ActiveUser p_User, bool isUGCEnabled = false)
        {
            try
            {
                Dictionary<string, object> dictResults = CommonController.GetDashboardAdhocResults(listOfSummaryReportData, p_FromDate, p_ToDate, p_SearchType, 850, isUGCEnabled,p_User);
                DashboardOverviewResults dashboardOverviewResults = (DashboardOverviewResults)dictResults["OverviewResults"];
                dynamic jsonResult = (ExpandoObject)dictResults["JsonResult"];

                jsonResult.HTML = RenderPartialToString(PATH_DashboardOverviewPartialView, dashboardOverviewResults);
                jsonResult.isSuccess = true;

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetHTMLWithCSSIncluded(string p_HTML, string p_FromDate, string p_ToDate, bool p_IsEmail, string p_SearchRequests)
        {
            StringBuilder cssData = new StringBuilder();

            StreamReader strmReader = new StreamReader(Server.MapPath("~/css/Dashboard.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();


            strmReader = new StreamReader(Server.MapPath("~/css/Feed.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();

            strmReader = new StreamReader(Server.MapPath("~/css/bootstrap.css"));
            cssData.Append(strmReader.ReadToEnd());
            strmReader.Close();
            strmReader.Dispose();

            cssData.Append(" li.liDmaMap{width:70%;float:left;}   li.liDmaChart{width:30%;float:left;}   \n");
            cssData.Append(" .divSentimentNeg div{overflow:visible;} \n .divSentimentPos div{overflow:visible;} \n .borderBottom{border-bottom:none;} \n body{background:none;}\n");



            p_HTML = "<html><head><style type=\"text/css\">" + Convert.ToString(cssData) + "</style></head>" + "<body>" + "<img src=\"../../" + ConfigurationManager.AppSettings["IQMediaEmailLogo"] + "\" alt='IQMedia Logo'/>" + p_HTML + "</body></html>";

            HtmlDocument doc = new HtmlDocument();
            doc.Load(new StringReader(p_HTML));
            doc.OptionOutputOriginalCase = true;

            if (p_IsEmail)
            {
                doc.DocumentNode.SelectSingleNode("//body").SetAttributeValue("style", "width:1000px;");
            }

            HtmlNode dpFromDiv = doc.DocumentNode.SelectSingleNode("//input[@id='dpFrom']");
            var newdpFromNodeHTML = p_FromDate + "  - ";
            var newdpFromNode = HtmlNode.CreateNode(newdpFromNodeHTML);
            dpFromDiv.ParentNode.ReplaceChild(newdpFromNode, dpFromDiv);

            HtmlNode dpToDiv = doc.DocumentNode.SelectSingleNode("//input[@id='dpTo']");
            var newdpToNodeHTML = p_ToDate;
            var newdpToNode = HtmlNode.CreateNode(newdpToNodeHTML);
            dpToDiv.ParentNode.ReplaceChild(newdpToNode, dpToDiv);

            /* HtmlNode dpSearchRequestDiv = doc.DocumentNode.SelectSingleNode("//div[@id='ddlSearchRequest_chosen']");
             var newdpSearchRequestNodeHTML = "<div style=\"margin=top:5px\">" + p_SearchRequests + "</div>";
             var newdpSearchRequestNode = HtmlNode.CreateNode(newdpSearchRequestNodeHTML);
             dpSearchRequestDiv.ParentNode.ReplaceChild(newdpSearchRequestNode, dpSearchRequestDiv);

             HtmlNode dpSearchRequestButtonDiv = doc.DocumentNode.SelectSingleNode("//input[@id='btnGetDataOnSearchRequest']");
             var newdpSearchRequestButtonNodeHTML = string.Empty;
             var newdpSearchRequestButtonNode = HtmlNode.CreateNode(newdpSearchRequestButtonNodeHTML);
             dpSearchRequestButtonDiv.ParentNode.ReplaceChild(newdpSearchRequestButtonNode, dpSearchRequestButtonDiv);*/

            HtmlNode dpDurationDiv = doc.DocumentNode.SelectSingleNode("//div[@id='divDuration']");
            var newdpDurationNodeHTML = string.Empty;
            var newdpDurationNode = HtmlNode.CreateNode(newdpDurationNodeHTML);
            dpDurationDiv.ParentNode.ReplaceChild(newdpDurationNode, dpDurationDiv);


            HtmlNode dpDashboardUtilityDiv = doc.DocumentNode.SelectSingleNode("//div[@id='divDashboardUtility']");
            var newdpDashboardUtilityNodeHTML = string.Empty;
            if (dpDashboardUtilityDiv != null)
            {
                var newdpDashboardUtilityNode = HtmlNode.CreateNode(newdpDashboardUtilityNodeHTML);
                dpDashboardUtilityDiv.ParentNode.ReplaceChild(newdpDashboardUtilityNode, dpDashboardUtilityDiv);
            }

            HtmlNode dpDashboardMapCompareDiv = doc.DocumentNode.SelectSingleNode("//div[@class='divcompareItalic']");
            var newdpDashboardMapCompareNodeHTML = string.Empty;
            if (dpDashboardMapCompareDiv != null)
            {
                var newdpDashboardMapCompareNode = HtmlNode.CreateNode(newdpDashboardUtilityNodeHTML);
                dpDashboardMapCompareDiv.ParentNode.ReplaceChild(newdpDashboardMapCompareNode, dpDashboardMapCompareDiv);
            }

            HtmlNode dpMapTooltipCompareDiv = doc.DocumentNode.SelectSingleNode("//div[@id='mapToolTip']");
            var newMapTooltipNodeHTML = string.Empty;
            if (dpMapTooltipCompareDiv != null)
            {
                var newMapTooltipNode = HtmlNode.CreateNode(newMapTooltipNodeHTML);
                dpMapTooltipCompareDiv.ParentNode.ReplaceChild(newMapTooltipNode, dpMapTooltipCompareDiv);
            }



            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//img[@class='ui-datepicker-trigger']"))
            {
                var newNodeHTML = string.Empty;
                var newNode = HtmlNode.CreateNode(newNodeHTML);
                link.ParentNode.ReplaceChild(newNode, link);
            }

            return doc.DocumentNode.OuterHtml;

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
}
