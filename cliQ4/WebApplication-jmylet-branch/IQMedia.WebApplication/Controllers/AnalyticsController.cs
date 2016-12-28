using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using HiQPdf;
using HtmlAgilityPack;
using IQCommon.Model;
using IQMedia.Model;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using IQMedia.WebApplication.Models.TempData;
using IQMedia.WebApplication.Utility;
using Newtonsoft.Json;
using Log4NetLogger = IQMedia.Shared.Utility.Log4NetLogger;

namespace IQMedia.WebApplication.Controllers
{
    [CheckAuthentication()]
    public class AnalyticsController : Controller
    {
        #region Public Property
        ActiveUser sessionInfo = null;
        AnalyticsTempData analyticsTempData;
        string PATH_AnalyticsPartialPrimary = "~/Views/Analytics/_AnalyticsPrimary.cshtml";
        string PATH_AnalyticsPartialSecondary = "~/Views/Analytics/_AnalyticsSecondary.cshtml";
        string PATH_AnalyticsAmpPartialFilter = "~/Views/Analytics/Amplification/_AmplificationFilter.cshtml";
        string PATH_AnalyticsCampPartialFilter = "~/Views/Analytics/Campaign/_CampaignFilter.cshtml";
        #endregion

        public ActionResult Index(string type)
        {
            try
            {
                sessionInfo = ActiveUserMgr.GetActiveUser();
                //Log4NetLogger.Debug(string.Format("Controller Index - Client GUID: {0}", sessionInfo.ClientGUID));
                //Log4NetLogger.Debug(string.Format("sessionInfo.gmt: {0}", sessionInfo.gmt));
                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);

                SetTempData(null);
                analyticsTempData = new AnalyticsTempData();
                analyticsTempData.CampaignModelList = iQAgentLogic.SelectIQAgentCampaignsByClientGuid(sessionInfo.ClientGUID);
                analyticsTempData.ClientAgents = GetAgentsByGUID(sessionInfo.ClientGUID).Select(agent => new {
                    agent.ID,
                    agent.QueryName
                }).ToDictionary(
                    ag => ag.ID,
                    ag => ag.QueryName
                );

                analyticsTempData.Groups = new Dictionary<string, Dictionary<string, string>>();
                analyticsTempData.Groups.Add("sources", GetAllSMTs());
                analyticsTempData.Groups.Add("market", GetAllDMAs());
                analyticsTempData.Groups.Add("daytime", GetAllDayTimes());
                analyticsTempData.Groups.Add("daypart", GetAllDayParts());
                analyticsTempData.Groups.Add("agent", GetAllAgentGroups());
                analyticsTempData.Groups.Add("campaign", GetAllCampaignGroups());

                analyticsTempData.SubMediaTypes = sessionInfo.MediaTypes == null ? new List<IQ_MediaTypeModel>() : sessionInfo.MediaTypes.Where(w => w.TypeLevel == 2).ToList();
                analyticsTempData.OnlineSubMediaTypes = sessionInfo.MediaTypes == null ? new List<string>() : sessionInfo.MediaTypes.Where(w => w.AnalyticsDataType.Equals("Online")).Select(s => s.SubMediaType).ToList();
                analyticsTempData.PrintSubMediaTypes = sessionInfo.MediaTypes == null ? new List<string>() : sessionInfo.MediaTypes.Where(w => w.AnalyticsDataType.Equals("Print")).Select(s => s.SubMediaType).ToList();
                analyticsTempData.OnAirSubMediaTypes = sessionInfo.MediaTypes == null ? new List<string>() : sessionInfo.MediaTypes.Where(w => w.AnalyticsDataType.Equals("OnAir")).Select(s => s.SubMediaType).ToList();

                SetTempData(analyticsTempData);

                Dictionary<string, object> dictModel = new Dictionary<string, object>();

                dictModel.Add("Primary", RenderPartialToString(PATH_AnalyticsPartialPrimary, null));
                dictModel.Add("Secondary", RenderPartialToString(PATH_AnalyticsPartialSecondary, null));
                switch (type)
                {
                    case "amplification":
                        dictModel.Add("Filter", RenderPartialToString(PATH_AnalyticsAmpPartialFilter, analyticsTempData.ClientAgents));
                        break;
                    case "campaign":
                        dictModel.Add("Filter", RenderPartialToString(PATH_AnalyticsCampPartialFilter, analyticsTempData.CampaignModelList));
                        break;
                }
                dictModel.Add("MasterMediaTypes", sessionInfo.MediaTypes);

                ViewBag.IsSuccess = true;
                return View("Index", dictModel);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                ViewBag.IsSuccess = false;
            }
            return View();
        }

        #region AjaxRequest

        [HttpPost]
        public ContentResult MediaJsonChart(AnalyticsGraphRequest graphRequest, string subMediaType, List<string> PESHTypes, List<string> sourceGroups, int chartType, string pageType, bool? isNewSearch)
        {
            try
            {
                sessionInfo = ActiveUserMgr.GetActiveUser();
                //Log4NetLogger.Debug(string.Format("MediaJSONChart"));
                //Stopwatch sw = new Stopwatch();
                //sw.Start();

                analyticsTempData = GetTempData();
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);

                string xAxisLabel = String.Empty;
                string chartHTML = String.Empty;
                long sumHits = 0;
                long sumSeen = 0;
                long sumHeard = 0;
                long sumPaid = 0;
                long sumEarned = 0;
                long sumOnAir = 0;
                long sumOnline = 0;
                long sumPrint = 0;
                long sumRead = 0;
                bool isDaytimeRequest = graphRequest.Tab == SecondaryTabID.Daytime;
                bool isDaypartRequest = graphRequest.Tab == SecondaryTabID.Daypart;

                AnalyticsDataModel dataModel = null;
                List<AnalyticsSummaryModel> requestedAgentSummariesList = null;
                List<AnalyticsMapSummaryModel> requestedAgentDmaMapList = new List<AnalyticsMapSummaryModel>();
                List<AnalyticsMapSummaryModel> requestedAgentCanadaMapList = new List<AnalyticsMapSummaryModel>();

                if (graphRequest.AgentList != null && graphRequest.AgentList.Count > 0)
                {
                    List<long> requestedAgentIDs = graphRequest.AgentList.Select(s => s.ID).ToList();

                    //graphRequest.AgentList.ForEach(s => {
                    //    Log4NetLogger.Debug(string.Format("agent {0} start {1} end {2} localStart {3} localEnd {4}", s.ID, s.DateFrom, s.DateTo, s.DateFrom.ToLocalTime(), s.DateTo.ToLocalTime()));
                    //});

                    if (pageType == "amplification")
                    {
                        xAxisLabel = graphRequest.GraphType == "comparison" ? "Day" : "Date";
                    }
                    else if (pageType == "campaign")
                    {
                        xAxisLabel = "Campaign Day";
                    }

                    ConvertDatesFromInterval(graphRequest.AgentList, graphRequest.DateInterval);

                    // Query the database only if the filter criteria was changed or TempData has not yet been set. Otherwise use TempData.
                    if ((bool)isNewSearch || (isDaytimeRequest && analyticsTempData.DaytimeDataModel == null) || (!isDaytimeRequest && analyticsTempData.AnalyticsDataModel == null) || (isDaypartRequest && analyticsTempData.DaypartDataModel == null) || (!isDaypartRequest && analyticsTempData.AnalyticsDataModel == null))
                    {
                        if (pageType == "amplification")
                        {
                            // On page load, or switching between tabs, get new data from database
                            switch (graphRequest.DateInterval)
                            {
                                case "hour":
                                    dataModel = GetAgentHourSummaries(sessionInfo.ClientGUID, graphRequest.AgentList, subMediaType);
                                    break;
                                case "day":
                                    dataModel = GetAgentDaySummaries(sessionInfo.ClientGUID, graphRequest.AgentList, subMediaType);
                                    break;
                                case "month":
                                    dataModel = GetAgentMonthSummaries(sessionInfo.ClientGUID, graphRequest.AgentList, subMediaType);
                                    //Log4NetLogger.Debug(string.Format("dataModel.summaries.count: {0}", dataModel.SummaryDataList.Count));
                                    //foreach (var summary in dataModel.SummaryDataList)
                                    //{
                                    //    Log4NetLogger.Debug(string.Format("agent {0} market {1}", summary.SearchRequestID, summary.Market));
                                    //}
                                    break;
                            }
                        }
                        else if (pageType == "campaign")
                        {
                            if (isDaytimeRequest || isDaypartRequest)
                            {
                                // Always get hour data for Daytime/Daypart chart
                                dataModel = analyticsLogic.GetHourSummaryDataForCampaign(
                                    graphRequest.AgentList.Select(s => s.ID.ToString()).ToList(), 
                                    subMediaType
                                );

                                // Convert summary dates to client's local time
                                dataModel.SummaryDataList = CommonFunctions.GetGMTandDSTTime(dataModel.SummaryDataList, CommonFunctions.ResultType.Analytics);
                            }
                            else
                            {
                                dataModel = analyticsLogic.GetDaySummaryDataForCampaign(graphRequest.AgentList.Select(s => s.ID.ToString()).ToList(), subMediaType);
                            }
                        }

                        if (isDaytimeRequest)
                        {
                            analyticsTempData.DaytimeDataModel = dataModel;
                        }
                        else if (isDaypartRequest)
                        {
                            analyticsTempData.DaypartDataModel = dataModel;
                        }
                        else
                        {
                            analyticsTempData.AnalyticsDataModel = dataModel;
                        }
                        SetTempData(analyticsTempData);
                    }
                    else
                    {
                        if (isDaytimeRequest)
                        {
                            dataModel = analyticsTempData.DaytimeDataModel;
                        }
                        else if (isDaypartRequest)
                        {
                            dataModel = analyticsTempData.DaypartDataModel;
                        }
                        else
                        {
                            dataModel = analyticsTempData.AnalyticsDataModel;
                        }
                    }

                    if (dataModel != null)
                    {
                        if (dataModel.SummaryDataList != null && dataModel.SummaryDataList.Count > 0)
                        {
                            if (pageType == "campaign")
                            {
                                requestedAgentSummariesList = dataModel.SummaryDataList.Where(w => requestedAgentIDs.Contains(w.CampaignID)).ToList();
                            }
                            else
                            {
                                // Filter the retrieved data to just the agents that were requested
                                requestedAgentSummariesList = dataModel.SummaryDataList.Where(w => requestedAgentIDs.Contains(w.SearchRequestID)).ToList();
                                //Log4NetLogger.Debug(string.Format("requestAgentSummariesList.count: {0}", requestedAgentSummariesList.Count));
                            }

                            sumHits = requestedAgentSummariesList.Sum(s => s.Number_Of_Hits);
                            sumSeen = requestedAgentSummariesList.Sum(s => s.SeenEarned + s.SeenPaid);
                            sumHeard = requestedAgentSummariesList.Sum(s => s.HeardEarned + s.HeardPaid);
                            sumPaid = requestedAgentSummariesList.Sum(s => s.SeenPaid + s.HeardPaid);
                            sumEarned = sumHits - sumPaid; // Earned includes all media types, not just TV
                            sumOnAir = requestedAgentSummariesList.Where(w => analyticsTempData.OnAirSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits);
                            sumOnline = requestedAgentSummariesList.Where(w => analyticsTempData.OnlineSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits);
                            sumPrint = requestedAgentSummariesList.Where(w => analyticsTempData.PrintSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits);
                            sumRead = sumHits - (sumSeen + sumHeard);

                            // The JS doesn't have access to each campaign's start/end dates, so set them here
                            if (pageType == "campaign")
                            {
                                graphRequest.AgentList.ForEach(s =>
                                {
                                    IQAgent_CampaignModel campaign = analyticsTempData.CampaignModelList.First(f => f.ID == s.ID);
                                    s.DateFrom = campaign.StartDateTime;
                                    s.DateTo = campaign.EndDateTime;
                                });
                            }
                        }
                        if (dataModel.DmaMentionMapList != null && dataModel.DmaMentionMapList.Count > 0)
                        {
                            requestedAgentDmaMapList = dataModel.DmaMentionMapList.Where(w => requestedAgentIDs.Contains(w.SearchRequestID)).ToList();
                        }
                        if (dataModel.CanadaMentionMapList != null && dataModel.CanadaMentionMapList.Count > 0)
                        {
                            requestedAgentCanadaMapList = dataModel.CanadaMentionMapList.Where(w => requestedAgentIDs.Contains(w.SearchRequestID)).ToList();
                        }
                    }

                    switch (chartType)
                    {
                        case 1: // Multi-Line Chart
                            graphRequest.chartType = ChartType.Line;
                            if (requestedAgentSummariesList != null)
                            {
                                chartHTML = analyticsLogic.GetLineChart(graphRequest, requestedAgentSummariesList, PESHTypes, sourceGroups, analyticsTempData.SubMediaTypes, xAxisLabel);
                            }
                            break;
                        case 2: // US Map
                            chartHTML = analyticsLogic.GetFusionUsaDmaMap(graphRequest, requestedAgentDmaMapList);
                            break;
                        case 3: // Canada Map
                            chartHTML = analyticsLogic.GetFusionCanadaProvinceMap(graphRequest, requestedAgentCanadaMapList);
                            break;
                        case 4: // Pie Chart
                            graphRequest.chartType = ChartType.Pie;
                            if (requestedAgentSummariesList != null)
                            {
                                chartHTML = analyticsLogic.GetPieChart(graphRequest, requestedAgentSummariesList, PESHTypes, sourceGroups, analyticsTempData.SubMediaTypes);
                            }
                            break;
                        case 5: // Bar Chart
                            graphRequest.chartType = ChartType.Bar;
                            if (requestedAgentSummariesList != null)
                            {
                                chartHTML = analyticsLogic.GetColumnOrBarChart(graphRequest, requestedAgentSummariesList, PESHTypes, sourceGroups, analyticsTempData.SubMediaTypes);
                            }
                            break;
                        case 6: // Column Chart
                            graphRequest.chartType = ChartType.Column;
                            if (requestedAgentSummariesList != null)
                            {
                                chartHTML = analyticsLogic.GetColumnOrBarChart(graphRequest, requestedAgentSummariesList, PESHTypes, sourceGroups, analyticsTempData.SubMediaTypes);
                            }
                            break;
                        case 7: // Daytime Heat Map
                            graphRequest.chartType = ChartType.Daytime;
                            chartHTML = analyticsLogic.GetDaytimeHeatMap(requestedAgentSummariesList, PESHTypes, sourceGroups, analyticsTempData.SubMediaTypes);
                            break;
                        case 8: // Growth Chart
                            graphRequest.chartType = ChartType.Growth;
                            if (requestedAgentSummariesList != null)
                            {
                                chartHTML = analyticsLogic.GetGrowthChart(graphRequest, requestedAgentSummariesList, PESHTypes, sourceGroups, analyticsTempData.SubMediaTypes, xAxisLabel); 
                            }
                            break;
                        case 9: // Daypart Heat Map
                            graphRequest.chartType = ChartType.Daypart;
                            chartHTML = analyticsLogic.GetDaypartHeatMap(requestedAgentSummariesList, PESHTypes, sourceGroups, analyticsTempData.SubMediaTypes);
                            break;
                    }
                }

                dynamic json = new ExpandoObject();
                json.jsonMediaRecord = chartHTML;
                json.hitsTotal = string.Format("{0:N0}", sumHits);
                json.seenTotal = string.Format("{0:N0}", sumSeen);
                json.heardTotal = string.Format("{0:N0}", sumHeard);
                json.paidTotal = string.Format("{0:N0}", sumPaid);
                json.earnedTotal = string.Format("{0:N0}", sumEarned);
                json.onAirTotal = string.Format("{0:N0}", sumOnAir);
                json.onlineTotal = string.Format("{0:N0}", sumOnline);
                json.printTotal = string.Format("{0:N0}", sumPrint);
                json.readTotal = string.Format("{0:N0}", sumRead);
                json.isLRAccess = sessionInfo.isv5LRAccess;
                json.isAdsAccess = sessionInfo.Isv5AdsAccess;
                json.isSuccess = true;

                //sw.Stop();
                //Log4NetLogger.Debug(string.Format("MediaJSONChart: {0} ms", sw.ElapsedMilliseconds));

                return Content(JsonConvert.SerializeObject(json), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return Content(CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("AnalyticsTempData");
            }
        }

        [HttpPost]
        public JsonResult GetOverlayData(int overlayType, AnalyticsGraphRequest graphRequest, string pageType, DateTime? fromDate, DateTime? toDate, List<string> PESHTypes, List<string> sourceGroups)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("GetOverlayData"));
                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();
                string overlayData = String.Empty;
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                var lstSubMediaTypes = sessionInfo.MediaTypes == null ? new List<IQCommon.Model.IQ_MediaTypeModel>() : sessionInfo.MediaTypes.Where(w => w.TypeLevel == 2).ToList();

                if (graphRequest.AgentList != null && graphRequest.AgentList.Count > 0)
                {
                    // Ensure that any modifications done to the summary data for campaign mode don't affect the source TempData list
                    List<AnalyticsSummaryModel> lstSummaryData = analyticsTempData.AnalyticsDataModel.SummaryDataList;

                    if (pageType == "campaign")
                    {
                        graphRequest.AgentList.ForEach(s =>
                        {
                            IQAgent_CampaignModel campaign = analyticsTempData.CampaignModelList.First(f => f.ID == s.ID);
                            s.DateFrom = campaign.StartDateTime;
                            s.DateTo = campaign.EndDateTime;
                        });
                        lstSummaryData = analyticsTempData.AnalyticsDataModel.SummaryDataList.Where(s => graphRequest.AgentList.Select(x => x.ID).Contains(s.CampaignID)).ToList();
                        lstSummaryData.ForEach(s => 
                        {
                            s.SearchRequestID = s.CampaignID;
                            s.Query_Name = s.CampaignName;
                        });
                        PESHTypes = new List<string>();
                        sourceGroups = new List<string>();
                    }

                    switch (overlayType)
                    {
                        case 1: // Google
                            GoogleLogic googleLogic = (GoogleLogic)LogicFactory.GetLogic(LogicType.Google);
                            List<GoogleSummaryModel> googleData = new List<GoogleSummaryModel>();
                            switch (graphRequest.DateInterval)
                            {
                                case "day":
                                    googleData = googleLogic.GetGoogleDataByDay(sessionInfo.ClientGUID, fromDate.Value, toDate.Value);
                                    break;
                                case "hour":
                                    googleData = googleLogic.GetGoogleDataByHour(sessionInfo.ClientGUID, fromDate.Value, toDate.Value);
                                    break;
                                case "month":
                                    googleData = googleLogic.GetGoogleDataByMonth(sessionInfo.ClientGUID, fromDate.Value, toDate.Value);
                                    break;
                            }
                            overlayData = analyticsLogic.GetHighChartSeriesForGoogle(googleData, fromDate.Value, toDate.Value, graphRequest.DateInterval, graphRequest);
                            break;
                        case 2: // Audience
                            overlayData = analyticsLogic.GetHighChartSeriesForViews(lstSummaryData, graphRequest);
                            break;
                        case 3: // Media Value
                            overlayData = analyticsLogic.GetHighChartSeriesForMediaValue(lstSummaryData, graphRequest);
                            break;
                    }
                }

                return Json(new
                {
                    overlayData = overlayData,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false
                });
            }
            finally
            {
                TempData.Keep("AnalyticsTempData");
            }
        }

        [HttpPost]
        public JsonResult OpenIFrame()
        {
            return Json(new { isSuccess = true });
        }

        #endregion

        #region BroadCast

        public AnalyticsDataModel GetAgentHourSummaries(Guid clientGUID, List<AnalyticsAgentRequest> agentsList, string subMediaType, string GroupByHeader = "")
        {
            //Log4NetLogger.Debug(string.Format("GetAgentHourSummaries"));
            try
            {
                //Log4NetLogger.Debug(string.Format("GetAgentHourSummaries"));
                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel dataModel;

                // Make 

                // Set GMT time from local time
                agentsList.ForEach(e => {
                    e.DateFromGMT = Utility.CommonFunctions.GetGMTandDSTTime(e.DateFrom).Value;
                    e.DateToGMT = Utility.CommonFunctions.GetGMTandDSTTime(e.DateTo).Value;
                });

                string requestXml = null;
                if (agentsList != null && agentsList.Count > 0)
                {
                    XDocument doc = new XDocument(new XElement(
                        "list",
                        from i in agentsList
                        select new XElement(
                            "item",
                            new XAttribute("id", i.ID),
                            new XAttribute("fromDate", i.DateFrom.ToString()),
                            new XAttribute("toDate", i.DateTo.ToString()),
                            new XAttribute("fromDateGMT", i.DateFromGMT.ToString()),
                            new XAttribute("toDateGMT", i.DateToGMT.ToString())
                        )
                    ));
                    requestXml = doc.ToString();
                }

                dataModel = logic.GetHourSummaryData(clientGUID, requestXml, subMediaType, false, GroupByHeader);
                //Log4NetLogger.Debug(string.Format("{0} summaries returned", dataModel.SummaryDataList.Count));
                // Convert summary dates to client's local time
                dataModel.SummaryDataList = CommonFunctions.GetGMTandDSTTime(dataModel.SummaryDataList, CommonFunctions.ResultType.Analytics);

                return dataModel;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new AnalyticsDataModel();
        }

        public AnalyticsDataModel GetCampaignHourSummaryData(List<string> campaignIDs, string subMediaType, string GroupByHeader = "")
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("GetCampaignHourSummaryData"));
                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel dataModel;

                string requestXML = null;
                if (campaignIDs != null && campaignIDs.Count > 0)
                {
                    XDocument xdoc = new XDocument(new XElement(
                        "list",
                        from i in campaignIDs
                        select new XElement(
                            "item",
                            new XAttribute("id", i)
                        )
                    ));
                    requestXML = xdoc.ToString();
                }

                dataModel = logic.GetCampaignHourSummaryData(requestXML, subMediaType, false, GroupByHeader);

                // Convert summary dates to client's local time
                dataModel.SummaryDataList = CommonFunctions.GetGMTandDSTTime(dataModel.SummaryDataList, CommonFunctions.ResultType.Analytics);

                return dataModel;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new AnalyticsDataModel();
        }

        public AnalyticsDataModel GetCampaignDaySummaryData(List<string> campaignIDs, string subMediaType)
        {
            try
            {
                //Log4NetLogger.Debug("GetCampaignDaySummaryData");
                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel dataModel;

                string requestXML = null;
                if (campaignIDs != null && campaignIDs.Count > 0)
                {
                    XDocument xdoc = new XDocument(new XElement(
                        "list",
                        from i in campaignIDs
                        select new XElement(
                            "item",
                            new XAttribute("id", i)
                        )
                    ));
                    requestXML = xdoc.ToString();
                }

                dataModel = logic.GetCampaignDaySummaryData(requestXML, subMediaType, false);

                return dataModel;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new AnalyticsDataModel();
        }

        public AnalyticsDataModel GetAgentDaySummaries(Guid clientGUID, List<AnalyticsAgentRequest> lstAgents, string subMediaType)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("GetAgentDaySummaries"));
                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel dataModel;

                if (lstAgents != null && lstAgents.Count > 0)
                {
                    lstAgents.ForEach(e =>
                    {
                        e.DateFromGMT = CommonFunctions.GetGMTandDSTTime(e.DateFrom).Value;
                        e.DateToGMT = CommonFunctions.GetGMTandDSTTime(e.DateTo.Date.AddHours(23).AddMinutes(59).AddSeconds(59)).Value;
                    });
                }

                string requestXml = null;
                if (lstAgents != null && lstAgents.Count > 0)
                {
                    XDocument doc = new XDocument(new XElement(
                        "list",
                        from i in lstAgents
                        select new XElement(
                            "item",
                            new XAttribute("id", i.ID),
                            new XAttribute("fromDate", i.DateFrom.ToString()),
                            new XAttribute("toDate", i.DateTo.ToString()),
                            new XAttribute("fromDateGMT", i.DateFromGMT.ToString()),
                            new XAttribute("toDateGMT", i.DateToGMT.ToString())
                        )
                    ));
                    requestXml = doc.ToString();
                }

                dataModel = logic.GetDaySummaryData(clientGUID, requestXml, subMediaType);
                return dataModel;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new AnalyticsDataModel();
        }

        private AnalyticsDataModel GetAgentMonthSummaries(Guid clientGUID, List<AnalyticsAgentRequest> lstAgents, string subMediaType)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("GetAgentMonthSummaries"));

                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel dataModel;

                // Set GMT time from local time
                lstAgents.ForEach(e =>
                {
                    e.DateFromGMT = CommonFunctions.GetGMTandDSTTime(e.DateFrom).Value;
                    e.DateToGMT = CommonFunctions.GetGMTandDSTTime(e.DateTo.Date.AddHours(23).AddMinutes(59).AddSeconds(59)).Value;
                });

                string requestXml = null;
                if (lstAgents != null && lstAgents.Count > 0)
                {
                    XDocument doc = new XDocument(new XElement(
                        "list",
                        from i in lstAgents
                        select new XElement(
                            "item",
                            new XAttribute("id", i.ID),
                            new XAttribute("fromDate", i.DateFrom.ToString()),
                            new XAttribute("toDate", i.DateTo.ToString()),
                            new XAttribute("fromDateGMT", i.DateFromGMT.ToString()),
                            new XAttribute("toDateGMT", i.DateToGMT.ToString())
                        )
                    ));
                    requestXml = doc.ToString();
                }

                dataModel = logic.GetMonthSummaryData(clientGUID, requestXml, subMediaType);

                // Set dates for summaries to be first of the month they occur in
                dataModel.SummaryDataList.ForEach(e =>
                {
                    e.SummaryDateTime = new DateTime(e.SummaryDateTime.Year, e.SummaryDateTime.Month, 1);
                });

                return dataModel;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new AnalyticsDataModel();
        }

        /// <summary>
        /// Gets all active agent summaries with summary data between dates
        /// </summary>
        /// <param name="clientGUID"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="interval"></param>
        /// <param name="subMediaType"></param>
        /// <returns></returns>
        private AnalyticsDataModel GetAllAgentSummaries(Guid clientGUID, DateTime dateFrom, DateTime dateTo, string interval, string subMediaType)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("GetAllAgentSummaries"));
                Stopwatch sw = new Stopwatch();
                sw.Start();

                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel dataModel;
                List<IQAgent_SearchRequestModel> listAllAgents = new List<IQAgent_SearchRequestModel>();
                List<IQAgent_SearchRequestModel> listAllAgentsSaved = new List<IQAgent_SearchRequestModel>();

                if (Session["ListAllAgentsSaved"] != null)
                {
                    listAllAgentsSaved = (List<IQAgent_SearchRequestModel>)Session["ListAllAgentsSaved"];
                }
                listAllAgents = (listAllAgentsSaved == null || !listAllAgentsSaved.Any()) ? GetAgentsByGUID(clientGUID) : listAllAgentsSaved;
                Session["ListAllAgentsSaved"] = listAllAgents;

                string requestXml = null;
                if (listAllAgents != null && listAllAgents.Count > 0)
                {
                    XDocument xDoc = new XDocument(new XElement(
                        "list",
                        from i in listAllAgents
                        select new XElement(
                            "item",
                            new XAttribute("id", i.ID),
                            new XAttribute("fromDate", dateFrom),
                            new XAttribute("toDate", dateTo),
                            new XAttribute("fromDateGMT", CommonFunctions.GetGMTandDSTTime(dateFrom).ToString()),
                            new XAttribute("toDateGMT", CommonFunctions.GetGMTandDSTTime(dateTo).ToString())
                        )
                    ));
                    requestXml = xDoc.ToString();
                }

                if (interval == "hour")
                {
                    dataModel = logic.GetHourSummaryData(clientGUID, requestXml, subMediaType);
                }
                else if (interval == "month")
                {
                    dataModel = logic.GetMonthSummaryData(clientGUID, requestXml, subMediaType);
                }
                else
                {
                    dataModel = logic.GetDaySummaryData(clientGUID, requestXml, subMediaType, false);
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetAllAgentSummaries: {0} ms elapsed", sw.ElapsedMilliseconds));
                return dataModel;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new AnalyticsDataModel();
        }

        private AnalyticsDataModel GetAgentSummaries(Guid clientGUID, List<AnalyticsAgentRequest> agents, string subMediaType, string interval)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel dataModel;
                string requestXml = null;

                if (agents != null && agents.Count > 0)
                {
                    XDocument xDoc = new XDocument(new XElement(
                        "list",
                        from i in agents
                        select new XElement(
                            "item",
                            new XAttribute("id", i.ID),
                            new XAttribute("fromDate", i.DateFrom),
                            new XAttribute("toDate", i.DateTo),
                            new XAttribute("fromDateGMT", CommonFunctions.GetGMTandDSTTime(i.DateFrom).ToString()),
                            new XAttribute("toDateGMT", CommonFunctions.GetGMTandDSTTime(i.DateTo).ToString())
                        )
                    ));
                    requestXml = xDoc.ToString();
                }

                switch(interval)
                {
                    case "hour":
                        dataModel = logic.GetHourSummaryData(clientGUID, requestXml, subMediaType);
                        break;
                    case "day":
                        dataModel = logic.GetDaySummaryData(clientGUID, requestXml, subMediaType, false);
                        break;
                    case "month":
                        dataModel = logic.GetMonthSummaryData(clientGUID, requestXml, subMediaType);
                        break;
                    default:
                        dataModel = logic.GetDaySummaryData(clientGUID, requestXml, subMediaType);
                        break;
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetAgentSummaries: {0} ms", sw.ElapsedMilliseconds));
                return dataModel;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new AnalyticsDataModel();
        }

        private AnalyticsDataModel GetCampaignSummaries(List<string> campaignIDs, string subMediaType, string interval)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel dataModel;
                string requestXml = null;

                if (campaignIDs != null && campaignIDs.Count > 0)
                {
                    XDocument xDoc = new XDocument(new XElement(
                        "list",
                        from i in campaignIDs
                        select new XElement(
                            "item",
                            new XAttribute("id", i)
                        )
                    ));
                    requestXml = xDoc.ToString();
                }

                switch (interval)
                {
                    case "hour":
                        dataModel = logic.GetCampaignHourSummaryData(requestXml, subMediaType, false);
                        break;
                    case "day":
                        dataModel = logic.GetCampaignDaySummaryData(requestXml, subMediaType, false);
                        break;
                    case "month":
                        // Currently no method to get campaign month summaries
                        dataModel = new AnalyticsDataModel();
                        break;
                    default:
                        dataModel = logic.GetCampaignDaySummaryData(requestXml, subMediaType, false);
                        break;
                }

                return dataModel;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new AnalyticsDataModel();
        }

        private Dictionary<string, string> GetAllDMAs()
        {
            try
            {
                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                return logic.GetAllDMAs();
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new Dictionary<string, string>();
        }

        private Dictionary<string, string> GetAllSMTs()
        {
            try
            {
                Dictionary<string, string> subMediaTypes = new Dictionary<string, string>();
                if (sessionInfo.MediaTypes != null)
                {
                    subMediaTypes = sessionInfo.MediaTypes.Where(w => w.TypeLevel == 2).Select(s => new {
                        s.SubMediaType,
                        s.DisplayName
                    }).ToDictionary(
                        smt => smt.SubMediaType,
                        smt => smt.DisplayName
                    );
                }
                return subMediaTypes;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new Dictionary<string, string>();
        }

        private Dictionary<string, string> GetAllDayTimes()
        {
            try
            {
                Dictionary<string, string> dayTimes = new Dictionary<string, string>();
                List<DayOfWeek> days = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();
                List<int> hours = Enumerable.Range(0, 23).ToList();
                foreach (var day in days)
                {
                    foreach (var hour in hours)
                    {
                        if (hour == 0)
                        {
                            dayTimes.Add(string.Format("{0}_{1}", day, hour), string.Format("{0} 12:00 AM", day));
                        }
                        else if (hour < 12)
                        {
                            dayTimes.Add(string.Format("{0}_{1}", day, hour), string.Format("{0} {1}:00 AM", day, hour));
                        }
                        else if (hour == 12)
                        {
                            dayTimes.Add(string.Format("{0}_{1}", day, hour), string.Format("{0} 12:00 PM", day));
                        }
                        else
                        {
                            dayTimes.Add(string.Format("{0}_{1}", day, hour), string.Format("{0} {1}:00 PM", day, hour - 12));
                        }
                    }
                }

                return dayTimes;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new Dictionary<string, string>();
        }

        private Dictionary<string, string> GetAllDayParts()
        {
            try
            {
                Dictionary<string, string> dayParts = new Dictionary<string, string>();
                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                List<DayPartDataItem> dayPartData = logic.GetDayPartData("A");

                foreach (var part in dayPartData.GroupBy(gb => gb.DayPartCode).ToList())
                {
                    dayParts.Add(part.First().DayPartCode, part.First().DayPartName);
                }

                return dayParts;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new Dictionary<string, string>();
        }

        private Dictionary<string, string> GetAllAgentGroups()
        {
            try
            {
                Dictionary<string, string> agentGroups = GetAgentsByGUID(sessionInfo.ClientGUID).Select(s => new {
                    s.ID,
                    s.QueryName
                }).Distinct().ToDictionary(
                    t => t.ID.ToString(),
                    t => t.QueryName
                );

                return agentGroups;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new Dictionary<string, string>();
        }

        private Dictionary<string, string> GetAllCampaignGroups()
        {
            try
            {
                Dictionary<string, string> campGroups = GetCampaignsByGUID(sessionInfo.ClientGUID).Select(s => new {
                    s.CampaignID,
                    s.CampaignName
                }).Distinct().ToDictionary(
                    t => t.CampaignID.ToString(),
                    t => t.CampaignName
                );

                return campGroups;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new Dictionary<string, string>();
        }

        #endregion

        #region Utility

        public string RenderPartialToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public List<IQAgent_SearchRequestModel> GetAgentsByGUID(Guid clientGUID)
        {
            //Log4NetLogger.Debug(string.Format("GetAgentsByGUID: {0}", clientGUID));
            IQAgentLogic agentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
            List<IQAgent_SearchRequestModel> agents = agentLogic.SelectIQAgentSearchRequestByClientGuid(clientGUID.ToString());

            return agents;
        }

        public List<IQAgent_SearchRequestModel> GetAgentsByID(Guid clientGUID, List<long> agentIDs)
        {
            IQAgentLogic agentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
            List<IQAgent_SearchRequestModel> agentList = new List<IQAgent_SearchRequestModel>();

            foreach(long ID in agentIDs)
            {
                agentList.Add(agentLogic.SelectIQAgentSearchRequestByID(clientGUID.ToString(), ID));
            }

            return agentList;
        }

        private List<AnalyticsCampaign> GetCampaignsByGUID(Guid clientGUID)
        {
            try
            {
                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                return logic.GetCampaigns(clientGUID);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new List<AnalyticsCampaign>();
        }

        public AnalyticsTempData GetTempData()
        {
            analyticsTempData = TempData["AnalyticsTempData"] != null ? (AnalyticsTempData)TempData["AnalyticsTempData"] : new AnalyticsTempData();
            return analyticsTempData;
        }

        public void SetTempData(AnalyticsTempData p_AnalyticsTempData)
        {
            TempData["AnalyticsTempData"] = p_AnalyticsTempData;
            TempData.Keep("AnalyticsTempData");
        }

        private void ConvertDatesFromInterval(List<AnalyticsAgentRequest> agentList, string dateInterval)
        {
            try
            {
                agentList.ForEach(e => {
                    e.DateFrom = ConvertStartDateFromInterval(e.DateFrom, dateInterval);
                    e.DateTo = ConvertEndDateFromInterval(e.DateTo, dateInterval);
                });

                agentList.ForEach(e =>
                {
                    e.DateFromGMT = CommonFunctions.GetGMTandDSTTime(e.DateFrom).Value;
                    e.DateToGMT = CommonFunctions.GetGMTandDSTTime(e.DateTo).Value;
                });
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
        }

        private DateTime ConvertStartDateFromInterval(DateTime startDate, string interval)
        {
            try
            {
                switch(interval)
                {
                    case "hour":
                        break;
                    case "day":
                        break;
                    case "month":
                        // Convert start date to first of month if date interval is month
                        startDate = new DateTime(startDate.Year, startDate.Month, 1);
                        break;
                }

                return startDate;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new DateTime();
        }

        private DateTime ConvertEndDateFromInterval(DateTime endDate, string interval)
        {
            try
            {
                switch(interval)
                {
                    case "hour":
                        // End date is today will need to set end date to 3 hours before now (to allow for processing) in client local time
                        if (endDate.Date.Equals(DateTime.Now.Date))
                        {
                            endDate = CommonFunctions.GetLocalTime(new DateTime(
                                endDate.Year,
                                endDate.Month,
                                endDate.Day,
                                DateTime.UtcNow.Hour,
                                0,
                                0
                            ).AddHours(-3)).Value;
                        }
                        else
                        {
                            // End date will set time to be end of day (11:59:59 PM)
                            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                        break;
                    case "day":
                        // If endDate time not set to end of day, will miss last day
                        if (endDate.Hour != 23)
                        {
                            endDate = new DateTime(
                                endDate.Year,
                                endDate.Month,
                                endDate.Day,
                                23,
                                59,
                                59
                            );
                        }
                        break;
                    case "month":
                        // End date will be last day of the month
                        endDate = new DateTime(
                            endDate.Year,
                            endDate.Month,
                            DateTime.DaysInMonth(endDate.Year, endDate.Month)
                        );
                        break;
                }

                return endDate;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new DateTime();
        }

        #endregion

        #region Secondary

        /// <summary>
        /// Receives ajax requests from analytics page to get a specific set of tables
        /// </summary>
        /// <param name="tab">The specific tab to get tables for (e.g. OverTime, Demographic, Sources, etc.)</param>
        /// <param name="selectedAgents">Specific agents to be selected</param>
        /// <param name="dateFrom">Date to start including data on</param>
        /// <param name="dateTo">Date to stop including data on</param>
        /// <param name="pageType">What analytics page to request for (e.g. Amplification, Campaign)</param>
        /// <param name="interval">The length of summary grouping (e.g. a summary every month, week, day, hour, etc.)</param>
        /// <returns>JSON string containing isSuccess boolean, array of html table representations, and an html string of tabs to switch tables on</returns>
        [HttpPost]
        public ContentResult GetSecondaryTables(string tab, List<AnalyticsAgentRequest> selectedAgents, DateTime? dateFrom, DateTime? dateTo, string pageType, string interval, string subMediaType)
        {
            try
            {
                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();

                //Log4NetLogger.Debug(string.Format("GetSecondaryTables"));
                //Log4NetLogger.Debug(string.Format("-tab: {0}", tab));
                //Log4NetLogger.Debug(string.Format("-dateFrom: {0}", dateFrom));
                //Log4NetLogger.Debug(string.Format("-dateTo: {0}", dateTo));
                //Log4NetLogger.Debug(string.Format("-pageType: {0}", pageType));
                //Log4NetLogger.Debug(string.Format("-interval: {0}", interval));

                Stopwatch sw = new Stopwatch();
                sw.Start();

                if (selectedAgents == null)
                {
                    selectedAgents = new List<AnalyticsAgentRequest>();
                }

                //Log4NetLogger.Debug(string.Format("-selectedAgents.Count: {0}", selectedAgents.Count));

                SecondaryTabID secondaryTab;
                try
                {
                    secondaryTab = (SecondaryTabID)Enum.Parse(typeof(SecondaryTabID), tab);
                }
                catch (Exception parseException)
                {
                    Log4NetLogger.Warning(parseException);
                    secondaryTab = SecondaryTabID.OverTime;
                }

                //Log4NetLogger.Debug(string.Format("GetSecondaryTables- secondaryTab: {0}", secondaryTab));
                AnalyticsLogic analyticsLogic = new AnalyticsLogic();
                List<AnalyticsSecondaryTable> secondaryTables = analyticsLogic.GetSecondaryTables(secondaryTab, pageType);

                ConvertDatesFromInterval(selectedAgents, interval);

                dynamic jsonResult = new ExpandoObject();
                jsonResult.isSuccess = true;
                jsonResult.tables = CreateSecondaryTables(secondaryTables, selectedAgents, selectedAgents.Any() ? selectedAgents.Select(x => x.DateFrom).Min() : dateFrom, selectedAgents.Any() ? selectedAgents.Select(x => x.DateTo).Max() : dateTo, secondaryTab, pageType, interval, subMediaType);
                jsonResult.tableTabs = CreateTableTabs(secondaryTables, secondaryTab, pageType);

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetSecondaryTables: {0} ms", sw.ElapsedMilliseconds));

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return Content(CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("AnalyticsTempData");
            }
        }

        private List<string> CreateSecondaryTables(List<AnalyticsSecondaryTable> listSecondaryTables, List<AnalyticsAgentRequest> listSelectedAgents, DateTime? dateFrom, DateTime? dateTo, SecondaryTabID tab, string pageType, string interval, string subMediaType)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("CreateSecondaryTables"));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug(string.Format("--listSelectedAgents.Count: {0}", listSelectedAgents.Count));
                // Each string in list is a table
                List<string> listTableStrings = new List<string>();

                for (int i = 0; i < listSecondaryTables.Count; i++)
                {
                    AnalyticsSecondaryTable table = listSecondaryTables[i];
                    //Log4NetLogger.Debug(string.Format("table.TabDisplay: {0}", table.TabDisplay));

                    HtmlTable tbl = new HtmlTable();
                    tbl.Attributes["class"] = "table clearBorders font12Pt";
                    tbl.Attributes["id"] = string.Format("tbl_{0}", table.TabDisplay);

                    // Show related secondary table initially - tab display and tab will match or OverTime == agent/campaign secondary
                    // If not on overtime tab - hide agent/campaign table
                    if (tab != SecondaryTabID.OverTime && (table.TabDisplay == "agent" || table.TabDisplay == "campaign"))
                    {
                        tbl.Attributes["style"] = "display:none;";
                    }

                    tbl.Rows.Add(CreateSecondaryTableHeader(table));
                    List<HtmlTableRow> detailRows = new List<HtmlTableRow>();

                    if (table.TabDisplay != "demographic")
                    {
                        detailRows = CreateSecondaryTableRows(table, listSelectedAgents, dateFrom, dateTo, interval, subMediaType);
                        //Log4NetLogger.Debug(string.Format("detailRows.Count: {0}", detailRows.Count));
                    }
                    else
                    {
                        detailRows = CreateSecondaryDemoTableRows(table, listSelectedAgents, dateFrom, dateTo, interval, subMediaType);
                    }

                    foreach (var dr in detailRows)
                    {
                        tbl.Rows.Add(dr);
                    }

                    StringWriter strWriter = new StringWriter();
                    tbl.RenderControl(new System.Web.UI.HtmlTextWriter(strWriter));

                    listTableStrings.Add(strWriter.ToString());
                }
                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateSecondaryTables: {0} ms", sw.ElapsedMilliseconds));
                return listTableStrings;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new List<string>();
        }

        private HtmlTableRow CreateSecondaryTableHeader(AnalyticsSecondaryTable secondaryTable)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("CreateSecondaryTableHeader"));
                Stopwatch sw = new Stopwatch();
                sw.Start();

                HtmlTableRow headerRow = new HtmlTableRow();
                headerRow.Attributes.Add("class", "secondaryHeaderRow");

                HtmlTableCell tc = new HtmlTableCell();
                tc.Attributes.Add("class", "secondaryHeaderCol");
                headerRow.Cells.Add(tc);

                tc = new HtmlTableCell() {
                    InnerText = secondaryTable.GroupByHeader
                };
                tc.Attributes.Add("class", "secondaryHeaderCol");
                tc.Attributes.Add("style", "width:280px;");
                headerRow.Cells.Add(tc);

                List<string> columnHeader = new List<string>();
                if (sessionInfo.isv5LRAccess && sessionInfo.Isv5AdsAccess)
                {
                    columnHeader = secondaryTable.ColumnHeadersAdsLR;
                }
                else if (sessionInfo.Isv5AdsAccess)
                {
                    columnHeader = secondaryTable.ColumnHeadersAds;
                }
                else if (sessionInfo.isv5LRAccess)
                {
                    columnHeader = secondaryTable.ColumnHeadersLR;
                }
                else
                {
                    columnHeader = secondaryTable.ColumnHeaders;
                }

                foreach (string colHead in columnHeader)
                {
                    tc = new HtmlTableCell() {
                        InnerText = colHead
                    };
                    tc.Attributes.Add("class", "secondaryHeaderCol");
                    if (colHead == "sentiment")
                    {
                        tc.Attributes.Add("style", "text-align:center;");
                    }
                    else if (colHead == "agent")
                    {
                        tc.Attributes.Add("style", "text-align:left");
                    }
                    else
                    {
                        tc.Attributes.Add("style", "text-align:right;");
                    }

                    if (colHead == "male" || colHead == "female")
                    {
                        tc.InnerText = string.Format("{0} ", colHead);
                        // Add cbs to header
                        HtmlInputCheckBox cbControl = new HtmlInputCheckBox() {
                            ID = string.Format("demographicCB_{0}", colHead),
                            Checked = true
                        };
                        cbControl.Attributes.Add("onclick", string.Format("ToggleSeries('{0}')", colHead));
                        cbControl.Attributes.Add("class", "coloredCB");

                        HtmlGenericControl lblControl = new HtmlGenericControl("label");
                        lblControl.ID = string.Format("demographicLbl_{0}", colHead);
                        lblControl.Attributes.Add("for", string.Format("demographicCB_{0}", colHead));
                        lblControl.Attributes.Add("style", "display:inline;text-align:center;");

                        HtmlGenericControl divControl = new HtmlGenericControl("div");
                        divControl.Attributes.Add("class", "coloredCBdiv");

                        HtmlGenericControl spnControl = new HtmlGenericControl("span");
                        spnControl.Attributes.Add("title", "select to remove from graph");
                        spnControl.Attributes.Add("class", "coloredCBspan");

                        divControl.Controls.Add(spnControl);
                        lblControl.Controls.Add(divControl);

                        tc.Controls.Add(cbControl);
                        tc.Controls.Add(lblControl);
                    }

                    headerRow.Cells.Add(tc);
                }
                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateSecondaryTableHeader: {0} ms", sw.ElapsedMilliseconds));
                return headerRow;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new HtmlTableRow();
        }

        private List<HtmlTableRow> CreateSecondaryDemoTableRows(AnalyticsSecondaryTable table, List<AnalyticsAgentRequest> listSelectedAgents, DateTime? dateFrom, DateTime? dateTo, string interval, string subMediaType)
        {
            try
            {
                sessionInfo = ActiveUserMgr.GetActiveUser();
                //Log4NetLogger.Debug(string.Format("CreateSecondaryDemoTableRows"));
                Stopwatch sw = new Stopwatch();

                AnalyticsDataModel dataModel;

                if (table.PageType == "campaign")
                {
                    List<string> listCampaignIDs = GetCampaignsByGUID(sessionInfo.ClientGUID).Select(c => c.CampaignID.ToString()).ToList();
                    dataModel = GetCampaignDaySummaryData(listCampaignIDs, "");
                }
                else
                {
                    dataModel = GetAllAgentSummaries(sessionInfo.ClientGUID, (DateTime)dateFrom, (DateTime)dateTo, interval, subMediaType);
                    //Log4NetLogger.Debug(string.Format("--dataModel.SummaryDataList.Count: {0}", dataModel.SummaryDataList.Count));
                }

                if (listSelectedAgents != null)
                {
                    // Only want demo data from agents selected
                    dataModel.SummaryDataList = dataModel.SummaryDataList.Where(summ => 
                        listSelectedAgents.Exists(x => x.ID == (table.PageType == "campaign" ? summ.CampaignID : summ.SearchRequestID))).ToList();
                    //Log4NetLogger.Debug(string.Format("dataModel.SummaryDataList.Count: {0}", dataModel.SummaryDataList.Count));
                }
                List<HtmlTableRow> listRows = new List<HtmlTableRow>();
                List<AnalyticsAgeRange> ageRanges = new List<AnalyticsAgeRange>();
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "18-24",
                    MaleAudience = dataModel.SummaryDataList.Sum(s => s.AM18_20 + s.AM21_24),
                    FemaleAudience = dataModel.SummaryDataList.Sum(s => s.AF18_20 + s.AF21_24),
                    TotalAudience = dataModel.SummaryDataList.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24)
                });
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "25-34",
                    MaleAudience = dataModel.SummaryDataList.Sum(s => s.AM25_34),
                    FemaleAudience = dataModel.SummaryDataList.Sum(s => s.AF25_34),
                    TotalAudience = dataModel.SummaryDataList.Sum(s => s.AM25_34 + s.AF25_34)
                });
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "35-49",
                    MaleAudience = dataModel.SummaryDataList.Sum(s => s.AM35_49),
                    FemaleAudience = dataModel.SummaryDataList.Sum(s => s.AF35_49),
                    TotalAudience = dataModel.SummaryDataList.Sum(s => s.AM35_49 + s.AF35_49)
                });
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "50-54",
                    MaleAudience = dataModel.SummaryDataList.Sum(s => s.AM50_54),
                    FemaleAudience = dataModel.SummaryDataList.Sum(s => s.AF50_54),
                    TotalAudience = dataModel.SummaryDataList.Sum(s => s.AM50_54 + s.AF50_54)
                });
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "55-64",
                    MaleAudience = dataModel.SummaryDataList.Sum(s => s.AM55_64),
                    FemaleAudience = dataModel.SummaryDataList.Sum(s => s.AF55_64),
                    TotalAudience = dataModel.SummaryDataList.Sum(s => s.AM55_64 + s.AF55_64)
                });
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "65+",
                    MaleAudience = dataModel.SummaryDataList.Sum(s => s.AM65_Plus),
                    FemaleAudience = dataModel.SummaryDataList.Sum(s => s.AF65_Plus),
                    TotalAudience = dataModel.SummaryDataList.Sum(s => s.AM65_Plus + s.AF65_Plus)
                });

                foreach (AnalyticsAgeRange ar in ageRanges)
                {
                    HtmlTableRow tr = new HtmlTableRow() {
                        ID = string.Format("demographicTR_{0}", ar.AgeRange)
                    };
                    tr.Attributes.Add("class", "secondaryDetailRow");

                    HtmlTableCell tc = new HtmlTableCell();
                    tc.Attributes.Add("class", "secondaryDetailCBCol");

                    HtmlInputCheckBox cbControl = new HtmlInputCheckBox() {
                        ID = string.Format("demographicCB_{0}", ar.AgeRange),
                        Checked = true
                    };

                    cbControl.Attributes.Add("onclick", string.Format("ToggleSeries('{0}')", ar.AgeRange));
                    cbControl.Attributes.Add("class", "coloredCB");

                    HtmlGenericControl lblControl = new HtmlGenericControl("label");
                    lblControl.ID = string.Format("demographicLbl_{0}", ar.AgeRange);
                    lblControl.Attributes.Add("for", string.Format("demographicCB_{0}", ar.AgeRange));
                    lblControl.Attributes.Add("style", "display:none;");

                    HtmlGenericControl divControl = new HtmlGenericControl("div");
                    divControl.Attributes.Add("class", "coloredCBdiv");

                    HtmlGenericControl spnControl = new HtmlGenericControl("span");
                    spnControl.Attributes.Add("title", "select to remove from graph");
                    spnControl.Attributes.Add("class", "coloredCBspan");

                    divControl.Controls.Add(spnControl);
                    lblControl.Controls.Add(divControl);

                    tc.Controls.Add(cbControl);
                    tc.Controls.Add(lblControl);

                    tr.Cells.Add(tc);

                    // Age Range col
                    tc = new HtmlTableCell() {
                        ID = string.Format("demographicTD_{0}", ar.AgeRange),
                        InnerText = ar.AgeRange
                    };
                    tr.Cells.Add(tc);

                    // Male col
                    tc = new HtmlTableCell() {
                        InnerText = string.Format("{0:N0}", ar.MaleAudience)
                    };
                    tc.Attributes.Add("style", "text-align:right;");
                    tr.Cells.Add(tc);

                    // Female col
                    tc = new HtmlTableCell() {
                        InnerText = string.Format("{0:N0}", ar.FemaleAudience)
                    };
                    tc.Attributes.Add("style", "text-align:right;");
                    tr.Cells.Add(tc);

                    // Total col
                    tc = new HtmlTableCell() {
                        InnerText = string.Format("{0:N0}", ar.TotalAudience)
                    };
                    tc.Attributes.Add("style", "text-align:right;");
                    tr.Cells.Add(tc);

                    listRows.Add(tr);
                }
                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateSecondaryDemoTableRows: {0} ms", sw.ElapsedMilliseconds));

                return listRows;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new List<HtmlTableRow>();
        }

        private List<HtmlTableRow> CreateSecondaryTableRows(AnalyticsSecondaryTable table, List<AnalyticsAgentRequest> listSelectedAgents, DateTime? dateFrom, DateTime? dateTo, string interval, string subMediaType)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("CreateSecondaryTableRows"));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug(string.Format("-table.DisplayTab: {0}", table.TabDisplay));
                //Log4NetLogger.Debug(string.Format("-listSelectedAgents.Count: {0}", listSelectedAgents.Count));
                sessionInfo = ActiveUserMgr.GetActiveUser();
                var lstSubMediaTypes = sessionInfo.MediaTypes == null ? new List<IQCommon.Model.IQ_MediaTypeModel>() : sessionInfo.MediaTypes.Where(w => w.TypeLevel == 2).ToList();
                List<string> onlineSubMediaTypes = sessionInfo.MediaTypes == null ? new List<string>() : sessionInfo.MediaTypes.Where(w => w.AnalyticsDataType.Equals("Online")).Select(s => s.SubMediaType).ToList();
                List<string> printSubMediaTypes = sessionInfo.MediaTypes == null ? new List<string>() : sessionInfo.MediaTypes.Where(w => w.AnalyticsDataType.Equals("Print")).Select(s => s.SubMediaType).ToList();
                List<string> onAirSubMediaTypes = sessionInfo.MediaTypes == null ? new List<string>() : sessionInfo.MediaTypes.Where(w => w.AnalyticsDataType.Equals("OnAir")).Select(s => s.SubMediaType).ToList();
                //Log4NetLogger.Debug(string.Format("sessionInfo.MediaTypes == null: {0}", sessionInfo.MediaTypes == null));
                //Log4NetLogger.Debug(string.Format("lstSubMediaTypes.Count: {0}", lstSubMediaTypes.Count));
                Dictionary<string, string> dictDMAs = GetAllDMAs();
                AnalyticsDataModel dataModel;

                if (table.PageType == "campaign")
                {
                    List<string> listCampaignIDs = GetCampaignsByGUID(sessionInfo.ClientGUID).Select(c => c.CampaignID.ToString()).ToList();

                    if (table.GroupByHeader == "daytime" || table.GroupByHeader == "daypart")
                    {
                        dataModel = GetCampaignHourSummaryData(listCampaignIDs, "", table.GroupByHeader);
                    }
                    else
                    {
                        dataModel = GetCampaignDaySummaryData(listCampaignIDs, "");
                    }
                }
                else
                {
                    if (table.GroupByHeader == "daytime" || table.GroupByHeader == "daypart")
                    {
                        dataModel = GetAgentHourSummaries(sessionInfo.ClientGUID, listSelectedAgents, "", table.GroupByHeader);
                    }
                    else
                    {
                        dataModel = GetAllAgentSummaries(sessionInfo.ClientGUID, (DateTime)dateFrom, (DateTime)dateTo, interval, subMediaType);
                    }
                }

                foreach (AnalyticsSummaryModel summary in dataModel.SummaryDataList)
                {
                    if (!string.IsNullOrEmpty(summary.SubMediaType))
                    {
                        //Log4NetLogger.Debug(string.Format("summary.SubMediaType: {0}", summary.SubMediaType));
                        //Log4NetLogger.Debug(string.Format("lstSubMediaTypes.count: {0}", lstSubMediaTypes.Count));
                        var listForSubMediaType = lstSubMediaTypes.Where(sm => sm.SubMediaType == summary.SubMediaType);
                        //Log4NetLogger.Debug(string.Format("SubMediaType: {0}.DisplayName: {1}", summary.SubMediaType, listForSubMediaType.First().DisplayName));
                        summary.SMTDisplayName = listForSubMediaType.Any() ? listForSubMediaType.First().DisplayName : "";
                    }
                    if (!string.IsNullOrWhiteSpace(summary.Market))
                    {
                        //Log4NetLogger.Debug(string.Format("summary.Market: {0}", summary.Market));
                        var marketID = dictDMAs.Where(dma => dma.Value == summary.Market);
                        summary.MarketID = marketID.Any() ? Convert.ToInt64(marketID.First().Key) : -1;
                        //Log4NetLogger.Debug(string.Format("summary.Market{1}: {0}", summary.Market, summary.MarketID));
                    }
                }

                List<HtmlTableRow> listRows = new List<HtmlTableRow>();
                AnalyticsSummaryModel summModel = new AnalyticsSummaryModel();
                PropertyInfo groupByPI = summModel.GetType().GetProperty(table.GroupBy);
                PropertyInfo groupByDisplayPI = summModel.GetType().GetProperty(table.GroupByDisplay);

                var groups = (dataModel != null && dataModel.SummaryDataList != null && dataModel.SummaryDataList.Any()) ? dataModel.SummaryDataList.GroupBy(g => groupByPI.GetValue(g, null)).ToList() : new List<IGrouping<object, AnalyticsSummaryModel>>();

                int count = 0;
                // For each grouping of summaries, create a table row
                foreach (var group in (groups.Any() ? groups.OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits)).ToList() : groups))
                {
                    if (group.Count() > 0)
                    {
                        var gbValue = groupByPI.GetValue(group.First(), null);

                        if (table.GroupByHeader == "market" && count >= 15)
                        {
                            // Only ever want top 15 markets - adding many more series causes highcharts to render chart extremely slowly
                            return listRows;
                        }
                        // Do not want any summaries that don't have value for group by
                        else if (gbValue != null)
                        {
                            //Log4NetLogger.Debug(string.Format("groupByPI.GetValue: {0}", groupByPI.GetValue(group.First(), null)));
                            count += 1;

                            HtmlTableRow tr = new HtmlTableRow() {
                                ID = string.Format("{0}TR_{1}", table.GroupByHeader, groupByPI.GetValue(group.First(), null))
                            };
                            tr.Attributes.Add("class", "secondaryDetailRow");

                            HtmlTableCell tc = new HtmlTableCell();
                            tc.Attributes.Add("class", "secondaryDetailCBCol");

                            HtmlInputCheckBox cbControl = new HtmlInputCheckBox() {
                                ID = string.Format("{0}CB_{1}", table.GroupByHeader, groupByPI.GetValue(group.First(), null))
                            };

                            // If on agent/campaign/market secondary table and no agents specifically selected, check top 5
                            if (table.GroupByHeader == "agent" || table.GroupByHeader == "campaign")
                            {
                                if (listSelectedAgents != null)
                                {
                                    if (listSelectedAgents.Count == 0)
                                    {
                                        if (count <= 5)
                                        {
                                            cbControl.Checked = true;
                                        }
                                    }
                                    else
                                    {
                                        // Agents/Campaigns specifically requested
                                        cbControl.Checked = listSelectedAgents.Any(sa => sa.ID == (table.PageType == "campaign" ? group.First().CampaignID : group.First().SearchRequestID));
                                    }
                                }
                            }
                            // Not on agent/campaign secondary
                            else
                            {
                                if (table.GroupByHeader == "market")
                                {
                                    if (count <= 15)
                                    {
                                        cbControl.Checked = true;
                                    }
                                }
                                else
                                {
                                    cbControl.Checked = true;
                                }
                            }

                            cbControl.Attributes.Add("onclick", string.Format("ToggleSeries('{0}')", groupByPI.GetValue(group.First(), null)));
                            cbControl.Attributes.Add("class", "coloredCB");

                            HtmlGenericControl lblControl = new HtmlGenericControl("label");
                            lblControl.Attributes.Add("for", string.Format("{0}CB_{1}", table.GroupByHeader, groupByPI.GetValue(group.First(), null)));

                            HtmlGenericControl divControl = new HtmlGenericControl("div");
                            divControl.Attributes.Add("class", "coloredCBdiv");

                            HtmlGenericControl spnControl = new HtmlGenericControl("span");
                            spnControl.Attributes.Add("title", "select to add to graph");
                            spnControl.Attributes.Add("class", "coloredCBspan");

                            divControl.Controls.Add(spnControl);
                            lblControl.Controls.Add(divControl);

                            tc.Controls.Add(cbControl);
                            tc.Controls.Add(lblControl);

                            tr.Cells.Add(tc);

                            // Group by col
                            tr.Cells.Add(new HtmlTableCell() {
                                InnerText = string.Format("{0}", groupByDisplayPI.GetValue(group.First(), null)),
                                ID = string.Format("{0}TD_{1}", table.GroupByHeader, groupByPI.GetValue(group.First(), null))
                            });

                            List<string> columnHeader = new List<string>();
                            if (sessionInfo.isv5LRAccess && sessionInfo.Isv5AdsAccess)
                            {
                                columnHeader = table.ColumnHeadersAdsLR;
                            }
                            else if (sessionInfo.Isv5AdsAccess)
                            {
                                columnHeader = table.ColumnHeadersAds;
                            }
                            else if (sessionInfo.isv5LRAccess)
                            {
                                columnHeader = table.ColumnHeadersLR;
                            }
                            else
                            {
                                columnHeader = table.ColumnHeaders;
                            }

                            foreach (string column in columnHeader)
                            {
                                tc = new HtmlTableCell();
                                tc.Attributes.Add("style", "text-align:right;");

                                switch (column)
                                {
                                    case "agent":
                                        tc.InnerText = string.Format("{0}", group.First().Query_Name);
                                        tc.Attributes.Clear();
                                        break;
                                    case "occurrences":
                                        tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.Number_Of_Hits));
                                        tc.ID = string.Format("OCCURRENCES_{0}", groupByPI.GetValue(group.First(), null));
                                        tc.Attributes.Add("OnlineCount", group.Where(w => onlineSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits).ToString());
                                        tc.Attributes.Add("PrintCount", group.Where(w => printSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits).ToString());
                                        break;
                                    case "seen":
                                        if (sessionInfo.isv5LRAccess)
                                        {
                                            tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.SeenEarned + s.SeenPaid));
                                            tc.ID = string.Format("SEEN_{0}", groupByPI.GetValue(group.First(), null));
                                        }
                                        break;
                                    case "heard":
                                        if (sessionInfo.isv5LRAccess)
                                        {
                                            tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.HeardEarned + s.HeardPaid));
                                            tc.ID = string.Format("HEARD_{0}", groupByPI.GetValue(group.First(), null));
                                        }
                                        break;
                                    case "read":
                                        if (sessionInfo.isv5LRAccess)
                                        {
                                            Int64 sumRead = group.Sum(s => s.Number_Of_Hits - (s.SeenEarned + s.SeenPaid + s.HeardEarned + s.HeardPaid));
                                            tc.InnerText = string.Format("{0:N0}", sumRead < 0 ? 0 : sumRead);
                                            tc.ID = string.Format("READ_{0}", groupByPI.GetValue(group.First(), null));
                                        }
                                        break;
                                    case "paid":
                                        if (sessionInfo.Isv5AdsAccess)
                                        {
                                            tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.HeardPaid + s.SeenPaid));
                                            tc.ID = string.Format("PAID_{0}", groupByPI.GetValue(group.First(), null));
                                        }
                                        break;
                                    case "earned":
                                        if (sessionInfo.Isv5AdsAccess)
                                        {
                                            tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.HeardEarned + s.SeenEarned));
                                            tc.ID = string.Format("EARNED_{0}", groupByPI.GetValue(group.First(), null));
                                        }
                                        break;
                                    case "on air time":
                                        long onAir = group.Sum(s => ((s.HeardEarned + s.HeardPaid) * 8 + (s.SeenEarned + s.SeenPaid)));
                                        var sumOnAir = group.Where(w => onAirSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits);
                                        tc.InnerText = string.Format("{0:00}:{1:00}:{2:00}", onAir / 3600, (onAir / 60) % 60, onAir % 60);
                                        tc.ID = string.Format("ONAIRTIME_{0}", groupByPI.GetValue(group.First(), null));
                                        tc.Attributes.Add("PESHvalue", sumOnAir.ToString());
                                        break;
                                    case "audience":
                                        tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.Audience));
                                        break;
                                    case "ad value":
                                        tc.InnerText = string.Format("{0:C2}", group.Sum(s => s.IQMediaValue));
                                        break;
                                    case "sentiment":
                                        tc.Attributes.Add("class", "sentimentTDContainer");

                                        divControl = new HtmlGenericControl("div");
                                        divControl.Attributes.Add("class", "sentimentDivContainer");
                                        divControl.Attributes.Add("align", "center");

                                        HtmlTable sentimentTbl = new HtmlTable() {
                                            CellPadding = 0,
                                            CellSpacing = 0
                                        };
                                        sentimentTbl.Attributes.Add("style", "height:100%;");

                                        HtmlTableRow sentimentTR = new HtmlTableRow();
                                        sentimentTR.Attributes.Add("class", "sentimentTRContainer");

                                        long negSentiment = group.Sum(s => s.NegativeSentiment);
                                        long posSentiment = group.Sum(s => s.PositiveSentiment);
                                        long negSentWidth = 0;
                                        long posSentWidth = 0;

                                        if (negSentiment > 0)
                                        {
                                            negSentWidth = (group.Sum(s => s.NegativeSentiment).ToString().Length * 8) + 5;
                                        }
                                        if (posSentiment > 0)
                                        {
                                            posSentWidth = (group.Sum(s => s.PositiveSentiment).ToString().Length * 8) + 5;
                                        }

                                        if (negSentWidth > 51)
                                        {
                                            negSentWidth = 51;
                                        }
                                        if (posSentWidth > 51)
                                        {
                                            posSentWidth = 51;
                                        }

                                        HtmlTableCell sentimentNegTD = new HtmlTableCell();
                                        sentimentNegTD.Attributes.Add("class", "sentimentNegPosTD");

                                        HtmlGenericControl negSpan = new HtmlGenericControl("span") {
                                            InnerHtml = string.Format("{0:N0}&nbsp;", negSentiment)
                                        };
                                        negSpan.Attributes.Add("class", "sentimentNegSpan");
                                        negSpan.Attributes.Add("style", string.Format("width:{0}px;", negSentWidth));

                                        sentimentNegTD.Controls.Add(negSpan);

                                        HtmlTableCell sentimentPosTD = new HtmlTableCell();
                                        sentimentPosTD.Attributes.Add("class", "sentimentNegPosTD");

                                        HtmlGenericControl posSpan = new HtmlGenericControl("span") {
                                            InnerHtml = string.Format("&nbsp;{0:N0}", posSentiment)
                                        };
                                        posSpan.Attributes.Add("class", "sentimentPosSpan");
                                        posSpan.Attributes.Add("style", string.Format("width:{0}px;", posSentWidth));

                                        sentimentPosTD.Controls.Add(posSpan);

                                        sentimentTR.Controls.Add(sentimentNegTD);
                                        sentimentTR.Controls.Add(sentimentPosTD);

                                        sentimentTbl.Controls.Add(sentimentTR);

                                        divControl.Controls.Add(sentimentTbl);
                                        tc.Controls.Add(divControl);
                                        break;
                                    default:
                                        break;
                                }

                                tr.Cells.Add(tc);
                            }

                            listRows.Add(tr);
                        }
                    }
                }
                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateSecondaryTableRows: {0} ms", sw.ElapsedMilliseconds));

                return listRows;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new List<HtmlTableRow>();
        }

        private string CreateTableTabs(List<AnalyticsSecondaryTable> listSecondaryTables, SecondaryTabID tab, string pageType)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("CreateTableTabs"));
                Stopwatch sw = new Stopwatch();
                sw.Start();

                string tabs = string.Empty;
                for (int i = 0; i < listSecondaryTables.Count; i++)
                {
                    var table = listSecondaryTables[i];

                    var li = new TagBuilder("li");
                    li.MergeAttribute("id", string.Format("li{0}{1}Detail", table.TabDisplay[0].ToString().ToUpper(), table.TabDisplay.Substring(1)));
                    // Capitalize first letter of group by header for id
                    li.MergeAttribute("name", "liSecondaryTab");

                    if (tab != SecondaryTabID.OverTime && table.TabDisplay != "agent" && table.TabDisplay != "campaign")
                    {
                        li.MergeAttribute("class", "active");
                    }
                    else if (tab == SecondaryTabID.OverTime && (table.TabDisplay == "agent" || table.TabDisplay == "campaign"))
                    {
                        li.MergeAttribute("class", "active");
                    }

                    var aTag = new TagBuilder("a");
                    aTag.MergeAttribute("onclick", string.Format("SwitchSecondaryTable('{0}');", table.TabDisplay));
                    aTag.InnerHtml = table.TabDisplay;
                    aTag.ToString(TagRenderMode.EndTag);

                    li.InnerHtml += aTag.ToString();
                    li.ToString(TagRenderMode.EndTag);

                    tabs += li.ToString();
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateTableTabs: {0} ms", sw.ElapsedMilliseconds));

                return tabs;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return string.Empty;
        }

        #endregion

        #region PDF

        [HttpPost]
        public JsonResult GeneratePDF()
        {
            try
            {
                sessionInfo = ActiveUserMgr.GetActiveUser();
                Request.InputStream.Position = 0;
                Dictionary<string, object> dictParameters;

                using (var sr = new StreamReader(Request.InputStream))
                {
                    dictParameters = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(Request.InputStream).ReadToEnd());
                }

                string HTML = dictParameters["HTML"].ToString();
                string dateFrom0 = dictParameters["dateFrom0"].ToString();
                string dateFrom1 = dictParameters["dateFrom1"].ToString();
                string dateTo0 = dictParameters["dateTo0"].ToString();
                string dateTo1 = dictParameters["dateTo1"].ToString();
                string agent0 = dictParameters["agent0"].ToString();
                string agent1 = dictParameters["agent1"].ToString();
                string source0 = dictParameters["source0"].ToString();
                string source1 = dictParameters["source1"].ToString();

                string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
                //string tempPDFPath = string.Format("C:\\Logs\\Download\\Analytics\\PDF\\{0}_{1}.pdf", sessionInfo.CustomerGUID, timeStamp);
                //string tempHTMLPath = string.Format("C:\\Logs\\Download\\Analytics\\HTML\\{0}_{1}.html", sessionInfo.CustomerGUID, timeStamp);
                string tempPDFPath = string.Format("{0}Download\\Analytics\\PDF\\{1}_{2}.pdf", ConfigurationManager.AppSettings["TempHTML-PDFPath"], sessionInfo.CustomerGUID, timeStamp);
                bool isFileGenerated = false;
                string html = GetHTMLWithCSS(HTML, dateFrom0, dateFrom1, dateTo0, dateTo1, agent0, agent1, source0, source1);

                //using (FileStream fs = System.IO.File.Create(tempHTMLPath))
                //{
                //    Byte[] info = new UTF8Encoding(true).GetBytes(html);
                //    fs.Write(info, 0, info.Length);
                //}

                HtmlToPdf HtPConverter = new HtmlToPdf();
                HtPConverter.SerialNumber = ConfigurationManager.AppSettings["HiQPdfSerialKey"];
                HtPConverter.Document.Margins = new PdfMargins(20);
                HtPConverter.BrowserWidth = 1000;
                HtPConverter.ConvertHtmlToFile(html, string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority), tempPDFPath);

                if (System.IO.File.Exists(tempPDFPath))
                {
                    isFileGenerated = true;
                    Session["PDFFile"] = tempPDFPath;
                }

                var json = new {
                    isSuccess = isFileGenerated
                };

                return Json(json);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);

                var json = new {
                    isSuccess = false,
                    errorMessage = Config.ConfigSettings.Settings.ErrorOccurred
                };
                return Json(json);
            }
        }

        [HttpGet]
        public ActionResult DownloadPDF()
        {
            try
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
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return Content(Config.ConfigSettings.Settings.FileNotAvailable);
        }

        private string GetHTMLWithCSS(string HTML, string dateFrom0, string dateFrom1, string dateTo0, string dateTo1, string agent0, string agent1, string source0, string source1)
        {
            try
            {
                StringBuilder cssData = new StringBuilder();

                StreamReader cssStreamReader = new StreamReader(Server.MapPath("~/css/analytics.css"));
                cssData.Append(cssStreamReader.ReadToEnd());
                cssStreamReader.Close();
                cssStreamReader.Dispose();

                cssStreamReader = new StreamReader(Server.MapPath("~/css/Feed.css"));
                cssData.Append(cssStreamReader.ReadToEnd());
                cssStreamReader.Close();
                cssStreamReader.Dispose();

                cssStreamReader = new StreamReader(Server.MapPath("~/css/bootstrap.css"));
                cssData.Append(cssStreamReader.ReadToEnd());
                cssStreamReader.Close();
                cssStreamReader.Dispose();

                // Remove global gray background from PDF
                cssData.Append("body{gackground:none;}\n");

                HTML = string.Format("<html><head><style type=\"text/css\">{0}</style></head><body>{1}</body></html>", cssData, HTML);

                HtmlDocument doc = new HtmlDocument();
                doc.Load(new StringReader(HTML));
                doc.OptionOutputOriginalCase = true;

                //// Get node of primary chart div
                //HtmlNode primaryChartDiv = doc.DocumentNode.SelectSingleNode("//div[@id='divPrimaryChart']");
                //primaryChartDiv.SetAttributeValue("style", "width:100%;height:400px;");

                // Do not want date range or date interval drops hovering over filter - not present in campaigns
                HtmlNode dateRangeDrop = doc.DocumentNode.SelectSingleNode("//div[@id='dateRangeDD']");
                if (dateRangeDrop != null)
                {
                    dateRangeDrop.ParentNode.ReplaceChild(HtmlNode.CreateNode(string.Empty), dateRangeDrop);
                }

                // Interval drop not present in campaigns
                HtmlNode dateIntervalDrop = doc.DocumentNode.SelectSingleNode("//div[@id='dateIntervalDrop']");
                if (dateIntervalDrop != null)
                {
                    dateIntervalDrop.ParentNode.ReplaceChild(HtmlNode.CreateNode(string.Empty), dateIntervalDrop);
                }

                // Get node of div that holds both agent filters
                HtmlNode filterDiv = doc.DocumentNode.SelectSingleNode("//div[@id='divFilters']");
                var newFilterDivHTML = string.Empty;
                if (filterDiv != null)
                {
                    // If main div holding both agent filters is hidden then replace with empty string to exclude from PDF
                    if (filterDiv.GetAttributeValue("class", "").Contains("hidden"))
                    {
                        filterDiv.ParentNode.ReplaceChild(HtmlNode.CreateNode(string.Empty), filterDiv);
                    }
                    else
                    {
                        HtmlNode agentNode0 = doc.DocumentNode.SelectSingleNode("//select[@id='ddAgentFilter0']");
                        agentNode0.ParentNode.ReplaceChild(HtmlNode.CreateNode(agent0 + "  - "), agentNode0);

                        HtmlNode sourceNode0 = doc.DocumentNode.SelectSingleNode("//select[@id='ddSourceFilter0']");
                        sourceNode0.ParentNode.ReplaceChild(HtmlNode.CreateNode(source0 + "  - "), sourceNode0);

                        HtmlNode dpFromNode0 = doc.DocumentNode.SelectSingleNode("//input[@id='dpDateFromFilter0']");
                        dpFromNode0.ParentNode.ReplaceChild(HtmlNode.CreateNode(dateFrom0), dpFromNode0);

                        HtmlNode dpToNode0 = doc.DocumentNode.SelectSingleNode("//input[@id='dpDateToFilter0']");
                        dpToNode0.ParentNode.ReplaceChild(HtmlNode.CreateNode(dateTo0), dpToNode0);

                        // Get node of div that holds second agent filter - no reason to get first agent filter as it should always be included if the main filter div is
                        HtmlNode filterDiv1 = doc.DocumentNode.SelectSingleNode("//div[@id='divAgentFilter1']");
                        if (filterDiv1 != null)
                        {
                            // If the second agent filter div is hidden then replace it with an empty string to exclude from PDF
                            if (filterDiv1.GetAttributeValue("class", "").Contains("hidden"))
                            {
                                filterDiv1.ParentNode.ReplaceChild(HtmlNode.CreateNode(string.Empty), filterDiv1);
                            }
                            else
                            {
                                HtmlNode agentNode1 = doc.DocumentNode.SelectSingleNode("//select[@id='ddAgentFilter1']");
                                agentNode1.ParentNode.ReplaceChild(HtmlNode.CreateNode(agent1 + "  - "), agentNode1);

                                HtmlNode sourceNode1 = doc.DocumentNode.SelectSingleNode("//select[@id='ddSourceFilter1']");
                                sourceNode1.ParentNode.ReplaceChild(HtmlNode.CreateNode(source1 + "  - "), sourceNode1);

                                HtmlNode dpFromNode1 = doc.DocumentNode.SelectSingleNode("//input[@id='dpDateFromFilter1']");
                                dpFromNode1.ParentNode.ReplaceChild(HtmlNode.CreateNode(dateFrom1), dpFromNode1);

                                HtmlNode dpToNode1 = doc.DocumentNode.SelectSingleNode("//input[@id='dpDateToFilter1']");
                                dpToNode1.ParentNode.ReplaceChild(HtmlNode.CreateNode(dateTo1), dpToNode1);
                            }
                        }
                    }
                }

                return doc.DocumentNode.OuterHtml;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return string.Empty;
        }

        #endregion

        #region Rework

        [HttpPost]
        public ContentResult GetMainTable(AnalyticsGraphRequest graphRequest, DateTime? dateFrom, DateTime? dateTo)
        {
            try
            {
                Log4NetLogger.Debug("GetMainTable");
                var sw = new Stopwatch();
                sw.Start();

                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();
                AnalyticsLogic analyticsLogic = new AnalyticsLogic();

                List<AnalyticsSecondaryTable> tables = analyticsLogic.GetSecondaryTables(graphRequest.Tab, graphRequest.PageType);

                // Get main, will be table with tab display of agent or campaign after getting secondary tables, never have both tables
                AnalyticsSecondaryTable msTable = tables.First(tbl => tbl.TabDisplay.Equals("agent") || tbl.TabDisplay.Equals("campaign"));
                PropertyInfo msGroupBy = typeof(AnalyticsSummaryModel).GetProperty(msTable.GroupBy);
                PropertyInfo msGroupByDisplay = typeof(AnalyticsSummaryModel).GetProperty(msTable.GroupByDisplay);

                // Summaries for TSSTable - summaries from selected agents
                List<AnalyticsSummaryModel> selectedSummaries = new List<AnalyticsSummaryModel>();
                // List should contain IDs of all campaigns for this client
                List<string> listCampaignIDs = analyticsTempData.Groups["campaign"].Select(s => s.Key).ToList();
                // Summaries for MSTable - summaries from all agents during time period
                List<AnalyticsSummaryModel> mstSummaries = new List<AnalyticsSummaryModel>();

                if (graphRequest.PageType == "campaign")
                {
                    mstSummaries = GetCampaignSummaries(listCampaignIDs, "", graphRequest.DateInterval).SummaryDataList;
                }
                else
                {
                    mstSummaries = GetAllAgentSummaries(sessionInfo.ClientGUID, (DateTime)dateFrom, (DateTime)dateTo, graphRequest.DateInterval, "").SummaryDataList;
                }

                // If selected agents null - get top 5 agents or campaigns - will happen on page load
                if (graphRequest.AgentList == null)
                {
                    graphRequest.AgentList = new List<AnalyticsAgentRequest>();
                    var summaryGroups = graphRequest.PageType == "campaign" ? mstSummaries.GroupBy(gb => gb.CampaignID) : mstSummaries.GroupBy(gb => gb.SearchRequestID);
                    var count = 0;
                    foreach (var summGroup in summaryGroups.OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits)))
                    {
                        // Limit is arbitrary
                        if (count < 5)
                        {
                            graphRequest.AgentList.Add(new AnalyticsAgentRequest() {
                                ID = graphRequest.PageType == "campaign" ? summGroup.First().CampaignID : summGroup.First().SearchRequestID,
                                DateFrom = (DateTime)dateFrom,
                                DateTo = (DateTime)dateTo,
                                DateFromGMT = (DateTime)CommonFunctions.GetGMTandDSTTime(dateFrom),
                                DateToGMT = (DateTime)CommonFunctions.GetGMTandDSTTime(dateTo)
                            });
                        }
                        count += 1;
                    }
                }

                HtmlTable msTableHTML = new HtmlTable();
                msTableHTML.Attributes["class"] = "table clearBorders font12Pt";
                msTableHTML.Attributes["id"] = string.Format("tbl_{0}", msTable.TabDisplay);

                msTableHTML.Rows.Add(CreateSecondaryTableHeader(msTable));
                BuildMainTableRows(msTable, mstSummaries, graphRequest.AgentList).ForEach(e => {
                    msTableHTML.Rows.Add(e);
                });

                StringWriter msTableStrWriter = new StringWriter();
                msTableHTML.RenderControl(new System.Web.UI.HtmlTextWriter(msTableStrWriter));

                dynamic jsonResult = new ExpandoObject();
                jsonResult.MSTable = msTableStrWriter.ToString();
                jsonResult.MSTableTab = msTable.TabDisplay;

                // If on OT tab there will not be a TSST
                if (tables.Count > 1)
                {
                    AnalyticsSecondaryTable tssTable = tables.First(tbl => string.Compare(tbl.TabDisplay, graphRequest.Tab.ToString(), true) == 0);
                    List<string> selectedIDs = graphRequest.AgentList.Select(sa => sa.ID.ToString()).ToList();

                    // Selected agents are only relevant for creating TSSTable and checking MSTable rows previously selected
                    if (graphRequest.PageType == "campaign")
                    {
                        selectedSummaries = GetCampaignSummaries(selectedIDs, "", graphRequest.DateInterval).SummaryDataList;
                    }
                    else
                    {
                        selectedSummaries = GetAgentSummaries(sessionInfo.ClientGUID, graphRequest.AgentList, "", graphRequest.DateInterval).SummaryDataList;
                    }

                    HtmlTable tssTableHTML = new HtmlTable();
                    tssTableHTML.Attributes["class"] = "table clearBorders font12Pt";
                    tssTableHTML.Attributes["id"] = string.Format("tbl_{0}", tssTable.TabDisplay);
                    tssTableHTML.Rows.Add(CreateSecondaryTableHeader(tssTable));

                    if (graphRequest.Tab != SecondaryTabID.Demographic)
                    {
                        BuildTabSpecificTableRows(tssTable, selectedSummaries).ForEach(e => {
                            tssTableHTML.Rows.Add(e);
                        });
                    }
                    else
                    {
                        BuildDemographicTableRows(tssTable, selectedSummaries).ForEach(e => {
                            tssTableHTML.Rows.Add(e);
                        });
                    }

                    StringWriter tssTableStrWriter = new StringWriter();
                    tssTableHTML.RenderControl(new System.Web.UI.HtmlTextWriter(tssTableStrWriter));

                    jsonResult.TSSTable = tssTableStrWriter.ToString();
                    jsonResult.TSSTableTab = tssTable.TabDisplay;

                    Dictionary<string, object> tsstChart = new Dictionary<string, object>();

                    if (graphRequest.Tab == SecondaryTabID.Demographic)
                    {
                        tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, selectedSummaries, new Dictionary<string, string>());
                    }
                    else
                    {
                        tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, selectedSummaries, analyticsTempData.Groups[tssTable.TabDisplay]);
                    }

                    jsonResult.TSSTSeries = tsstChart["series"];
                    jsonResult.chartJSON = tsstChart["chart"];
                }
                else
                {
                    var otChart = analyticsLogic.GetChart(graphRequest, msTable, mstSummaries, analyticsTempData.Groups[msTable.TabDisplay]);
                    jsonResult.MSTSeries = otChart["series"];
                    jsonResult.chartJSON = otChart["chart"];
                    jsonResult.TSSTable = string.Empty;
                    jsonResult.TSSTableTab = string.Empty;
                }

                jsonResult.isSuccess = true;

                sw.Stop();
                Log4NetLogger.Debug(string.Format("GetMainTable: {0} ms", sw.ElapsedMilliseconds));

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return Content(CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("AnalyticsTempData");
            }
        }

        [HttpPost]
        public ContentResult GetTabSpecificTable(AnalyticsGraphRequest graphRequest, DateTime? dateFrom, DateTime? dateTo)
        {
            // When method called, it is assumed that MainTable is already on page - no need to build/return main table for this method
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();

                AnalyticsLogic analyticsLogic = new AnalyticsLogic();
                List<AnalyticsSecondaryTable> tables = analyticsLogic.GetSecondaryTables(graphRequest.Tab, graphRequest.PageType);
                AnalyticsSecondaryTable tssTable = tables.First(tbl => tbl.TabDisplay == graphRequest.Tab.ToString().ToLower());
                List<AnalyticsSummaryModel> selectedSummaries = new List<AnalyticsSummaryModel>();
                List<string> selectedIDs = graphRequest.AgentList.Select(sa => sa.ID.ToString()).ToList();

                if (graphRequest.PageType == "campaign")
                {
                    selectedSummaries = GetCampaignSummaries(selectedIDs, "", graphRequest.DateInterval).SummaryDataList;
                }
                else
                {
                    selectedSummaries = GetAgentSummaries(sessionInfo.ClientGUID, graphRequest.AgentList, "", graphRequest.DateInterval).SummaryDataList;
                }

                HtmlTable tableHTML = new HtmlTable();
                tableHTML.Rows.Add(CreateSecondaryTableHeader(tssTable));
                List<HtmlTableRow> detailRows = new List<HtmlTableRow>();

                if (graphRequest.Tab != SecondaryTabID.Demographic)
                {
                    BuildTabSpecificTableRows(tssTable, selectedSummaries).ForEach(e => {
                        tableHTML.Rows.Add(e);
                    });
                }
                else
                {
                    BuildDemographicTableRows(tssTable, selectedSummaries).ForEach(e => {
                        tableHTML.Rows.Add(e);
                    });
                }

                StringWriter strWriter = new StringWriter();
                tableHTML.RenderControl(new System.Web.UI.HtmlTextWriter(strWriter));

                dynamic jsonResult = new ExpandoObject();
                jsonResult.isSuccess = true;
                jsonResult.TSSTable = strWriter.ToString();
                jsonResult.TSSTableTab = tssTable.TabDisplay;

                Dictionary<string, object> tsstChart = new Dictionary<string, object>();
                if (graphRequest.Tab == SecondaryTabID.Demographic)
                {
                    tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, selectedSummaries, new Dictionary<string, string>());
                }
                else
                {
                    tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, selectedSummaries, analyticsTempData.Groups[tssTable.TabDisplay]);
                }
                jsonResult.chartJSON = tsstChart["chart"];
                jsonResult.TSSTSeries = tsstChart["series"];

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetTabSpecificTable: {0} ms", sw.ElapsedMilliseconds));
                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return Content(CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("AnalyticsTempData");
            }
        }

        [HttpPost]
        public ContentResult GetChart(AnalyticsGraphRequest graphRequest, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug(string.Format("GetChart"));

                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();
                AnalyticsLogic analyticsLogic = new AnalyticsLogic();

                List<AnalyticsSecondaryTable> tables = analyticsLogic.GetSecondaryTables(graphRequest.Tab, graphRequest.PageType);

                List<AnalyticsSummaryModel> selectedSummaries = new List<AnalyticsSummaryModel>();
                // Summaries for MSTable - summaries from all agents during time period
                List<AnalyticsSummaryModel> mstSummaries = new List<AnalyticsSummaryModel>();
                // List should contain IDs of all campaigns for this client
                List<string> listCampaignIDs = GetCampaignsByGUID(sessionInfo.ClientGUID).Select(c => c.CampaignID.ToString()).ToList();

                if (graphRequest.PageType == "campaign")
                {
                    mstSummaries = GetCampaignSummaries(listCampaignIDs, "", graphRequest.DateInterval).SummaryDataList;
                }
                else
                {
                    mstSummaries = GetAllAgentSummaries(sessionInfo.ClientGUID, (DateTime)dateFrom, (DateTime)dateTo, graphRequest.DateInterval, "").SummaryDataList;
                }

                // If selected agents null - get top 5 agents or campaigns - will happen on page load
                if (graphRequest.AgentList == null)
                {
                    graphRequest.AgentList = new List<AnalyticsAgentRequest>();
                    var summaryGroups = graphRequest.PageType == "campaign" ? mstSummaries.GroupBy(gb => gb.CampaignID) : mstSummaries.GroupBy(gb => gb.SearchRequestID);
                    var count = 0;
                    foreach (var summGroup in summaryGroups.OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits)))
                    {
                        // Limit is arbitrary
                        if (count < 5)
                        {
                            graphRequest.AgentList.Add(new AnalyticsAgentRequest() {
                                ID = graphRequest.PageType == "campaign" ? summGroup.First().CampaignID : summGroup.First().SearchRequestID,
                                DateFrom = (DateTime)dateFrom,
                                DateTo = (DateTime)dateTo,
                                DateFromGMT = (DateTime)CommonFunctions.GetGMTandDSTTime(dateFrom),
                                DateToGMT = (DateTime)CommonFunctions.GetGMTandDSTTime(dateTo)
                            });
                        }
                        count += 1;
                    }
                }

                AnalyticsSecondaryTable tssTable;
                if (tables.Count == 1)  // If only one table returned chart is for OverTime
                {
                    tssTable = tables.ElementAt(0);
                }
                else
                {
                    tssTable = tables.First(tbl => string.Compare(tbl.TabDisplay, graphRequest.Tab.ToString(), true) == 0);
                }

                List<string> selectedIDs = graphRequest.AgentList.Select(sa => sa.ID.ToString()).ToList();

                // Selected agents are only relevant for creating TSSTable and checking MSTable rows previously selected
                if (graphRequest.PageType == "campaign")
                {
                    selectedSummaries = GetCampaignSummaries(selectedIDs, "", graphRequest.DateInterval).SummaryDataList;
                }
                else
                {
                    selectedSummaries = GetAgentSummaries(sessionInfo.ClientGUID, graphRequest.AgentList, "", graphRequest.DateInterval).SummaryDataList;
                }

                foreach (AnalyticsSummaryModel summary in selectedSummaries)
                {
                    if (!string.IsNullOrEmpty(summary.SubMediaType))
                    {
                        var subMediaType = analyticsTempData.SubMediaTypes.Where(sm => sm.SubMediaType == summary.SubMediaType);
                        //Log4NetLogger.Debug(string.Format("summary.SubMediaType: {0}", summary.SubMediaType));
                        summary.SMTDisplayName = subMediaType.Any() ? subMediaType.First().DisplayName : "";
                    }
                    if (!string.IsNullOrWhiteSpace(summary.Market))
                    {
                        var marketID = analyticsTempData.Groups["market"].Where(dma => dma.Value == summary.Market);
                        summary.MarketID = marketID.Any() ? Convert.ToInt64(marketID.First().Key) : -1;
                    }
                }

                Dictionary<string, object> tsstChart = new Dictionary<string, object>();
                if (graphRequest.Tab == SecondaryTabID.Demographic)
                {
                    tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, selectedSummaries, new Dictionary<string, string>());
                }
                else
                {
                    tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, selectedSummaries, analyticsTempData.Groups[tssTable.TabDisplay]);
                }

                dynamic jsonResult = new ExpandoObject();
                jsonResult.isSuccess = true;
                jsonResult.TSSTSeries = tsstChart["series"];
                jsonResult.chartJSON = tsstChart["chart"];

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetChart: {0}", sw.ElapsedMilliseconds));
                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return Content(CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
            finally
            {
                TempData.Keep("AnalyticsTempData");
            }
        }

        private List<HtmlTableRow> BuildMainTableRows(AnalyticsSecondaryTable table, List<AnalyticsSummaryModel> summaries, List<AnalyticsAgentRequest> selectedAgents)
        {
            try
            {
                // Assuming selected agents never null
                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();

                foreach (AnalyticsSummaryModel summary in summaries)
                {
                    if (!string.IsNullOrEmpty(summary.SubMediaType))
                    {
                        var subMediaType = analyticsTempData.SubMediaTypes.Where(sm => sm.SubMediaType == summary.SubMediaType);
                        summary.SMTDisplayName = subMediaType.Any() ? subMediaType.First().DisplayName : "";
                    }
                    if (!string.IsNullOrEmpty(summary.Market))
                    {
                        var marketID = analyticsTempData.Groups["market"].Where(dma => dma.Value == summary.Market);
                        summary.MarketID = marketID.Any() ? Convert.ToInt64(marketID.First().Key) : -1;
                    }
                }

                AnalyticsSummaryModel summModel = new AnalyticsSummaryModel();
                PropertyInfo groupByPI = summModel.GetType().GetProperty(table.GroupBy);
                PropertyInfo groupByDisplayPI = summModel.GetType().GetProperty(table.GroupByDisplay);

                var groups = summaries.GroupBy(g => groupByPI.GetValue(g, null));

                List<HtmlTableRow> tableRows = new List<HtmlTableRow>();
                foreach (var group in groups.OrderByDescending(g => g.Sum(s => s.Number_Of_Hits)).ToList())
                {
                    // Create row for each group
                    HtmlTableRow tr = new HtmlTableRow() {
                        ID = string.Format("{0}TR_{1}", table.GroupByHeader, groupByPI.GetValue(group.First(), null))
                    };
                    tr.Attributes.Add("class", "secondaryDetailRow");

                    HtmlTableCell tc = new HtmlTableCell();
                    tc.Attributes.Add("class", "secondaryDetailCBCol");

                    HtmlInputCheckBox cbControl = new HtmlInputCheckBox() {
                        ID = string.Format("{0}CB_{1}", table.GroupByHeader, groupByPI.GetValue(group.First(), null))
                    };

                    cbControl.Checked = selectedAgents.Any(sa => groupByPI.GetValue(group.First(), null).ToString() == sa.ID.ToString());

                    cbControl.Attributes.Add("onclick", string.Format("ToggleSeries('{0}')", groupByPI.GetValue(group.First(), null)));
                    cbControl.Attributes.Add("class", "coloredCB");

                    HtmlGenericControl lblControl = new HtmlGenericControl("label");
                    lblControl.Attributes.Add("for", string.Format("{0}CB_{1}", table.GroupByHeader, groupByPI.GetValue(group.First(), null)));

                    HtmlGenericControl divControl = new HtmlGenericControl("div");
                    divControl.Attributes.Add("class", "coloredCBdiv");

                    HtmlGenericControl spnControl = new HtmlGenericControl("span");
                    spnControl.Attributes.Add("title", "select to add to graph");
                    spnControl.Attributes.Add("class", "coloredCBspan");

                    divControl.Controls.Add(spnControl);
                    lblControl.Controls.Add(divControl);

                    tc.Controls.Add(cbControl);
                    tc.Controls.Add(lblControl);

                    tr.Cells.Add(tc);

                    // Group by col
                    tr.Cells.Add(new HtmlTableCell() {
                        InnerText = string.Format("{0}", groupByDisplayPI.GetValue(group.First(), null)),
                        ID = string.Format("{0}TD_{1}", table.GroupByHeader, groupByPI.GetValue(group.First(), null))
                    });

                    List<string> columnHeader = new List<string>();
                    if (sessionInfo.isv5LRAccess && sessionInfo.Isv5AdsAccess)
                    {
                        columnHeader = table.ColumnHeadersAdsLR;
                    }
                    else if (sessionInfo.Isv5AdsAccess)
                    {
                        columnHeader = table.ColumnHeadersAds;
                    }
                    else if (sessionInfo.isv5LRAccess)
                    {
                        columnHeader = table.ColumnHeadersLR;
                    }
                    else
                    {
                        columnHeader = table.ColumnHeaders;
                    }

                    foreach (string column in columnHeader)
                    {
                        tc = new HtmlTableCell();
                        tc.Attributes.Add("style", "text-align:right;");

                        switch (column)
                        {
                            case "agent":
                                tc.InnerText = string.Format("{0}", group.First().Query_Name);
                                tc.Attributes.Clear();
                                break;
                            case "occurrences":
                                tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.Number_Of_Hits));
                                tc.ID = string.Format("OCCURRENCES_{0}", groupByPI.GetValue(group.First(), null));
                                tc.Attributes.Add("OnlineCount", group.Where(w => analyticsTempData.OnlineSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits).ToString());
                                tc.Attributes.Add("PrintCount", group.Where(w => analyticsTempData.PrintSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits).ToString());
                                break;
                            case "seen":
                                if (sessionInfo.isv5LRAccess)
                                {
                                    tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.SeenEarned + s.SeenPaid));
                                    tc.ID = string.Format("SEEN_{0}", groupByPI.GetValue(group.First(), null));
                                }
                                break;
                            case "heard":
                                if (sessionInfo.isv5LRAccess)
                                {
                                    tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.HeardEarned + s.HeardPaid));
                                    tc.ID = string.Format("HEARD_{0}", groupByPI.GetValue(group.First(), null));
                                }
                                break;
                            case "read":
                                if (sessionInfo.isv5LRAccess)
                                {
                                    Int64 sumRead = group.Sum(s => s.Number_Of_Hits - (s.SeenEarned + s.SeenPaid + s.HeardEarned + s.HeardPaid));
                                    tc.InnerText = string.Format("{0:N0}", sumRead < 0 ? 0 : sumRead);
                                    tc.ID = string.Format("READ_{0}", groupByPI.GetValue(group.First(), null));
                                }
                                break;
                            case "paid":
                                if (sessionInfo.Isv5AdsAccess)
                                {
                                    tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.HeardPaid + s.SeenPaid));
                                    tc.ID = string.Format("PAID_{0}", groupByPI.GetValue(group.First(), null));
                                }
                                break;
                            case "earned":
                                if (sessionInfo.Isv5AdsAccess)
                                {
                                    tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.HeardEarned + s.SeenEarned));
                                    tc.ID = string.Format("EARNED_{0}", groupByPI.GetValue(group.First(), null));
                                }
                                break;
                            case "on air time":
                                long onAir = group.Sum(s => ((s.HeardEarned + s.HeardPaid) * 8 + (s.SeenEarned + s.SeenPaid)));
                                var sumOnAir = group.Where(w => analyticsTempData.OnAirSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits);
                                tc.InnerText = string.Format("{0:00}:{1:00}:{2:00}", onAir / 3600, (onAir / 60) % 60, onAir % 60);
                                tc.ID = string.Format("ONAIRTIME_{0}", groupByPI.GetValue(group.First(), null));
                                tc.Attributes.Add("PESHvalue", sumOnAir.ToString());
                                break;
                            case "audience":
                                tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.Audience));
                                break;
                            case "ad value":
                                tc.InnerText = string.Format("{0:C2}", group.Sum(s => s.IQMediaValue));
                                break;
                            case "sentiment":
                                tc.Attributes.Add("class", "sentimentTDContainer");

                                divControl = new HtmlGenericControl("div");
                                divControl.Attributes.Add("class", "sentimentDivContainer");
                                divControl.Attributes.Add("align", "center");

                                HtmlTable sentimentTbl = new HtmlTable() {
                                    CellPadding = 0,
                                    CellSpacing = 0
                                };
                                sentimentTbl.Attributes.Add("style", "height:100%;");

                                HtmlTableRow sentimentTR = new HtmlTableRow();
                                sentimentTR.Attributes.Add("class", "sentimentTRContainer");

                                long negSentiment = group.Sum(s => s.NegativeSentiment);
                                long posSentiment = group.Sum(s => s.PositiveSentiment);
                                long negSentWidth = 0;
                                long posSentWidth = 0;

                                if (negSentiment > 0)
                                {
                                    negSentWidth = (group.Sum(s => s.NegativeSentiment).ToString().Length * 8) + 5;
                                }
                                if (posSentiment > 0)
                                {
                                    posSentWidth = (group.Sum(s => s.PositiveSentiment).ToString().Length * 8) + 5;
                                }

                                if (negSentWidth > 51)
                                {
                                    negSentWidth = 51;
                                }
                                if (posSentWidth > 51)
                                {
                                    posSentWidth = 51;
                                }

                                HtmlTableCell sentimentNegTD = new HtmlTableCell();
                                sentimentNegTD.Attributes.Add("class", "sentimentNegPosTD");

                                HtmlGenericControl negSpan = new HtmlGenericControl("span") {
                                    InnerHtml = string.Format("{0:N0}&nbsp;", negSentiment)
                                };
                                negSpan.Attributes.Add("class", "sentimentNegSpan");
                                negSpan.Attributes.Add("style", string.Format("width:{0}px;", negSentWidth));

                                sentimentNegTD.Controls.Add(negSpan);

                                HtmlTableCell sentimentPosTD = new HtmlTableCell();
                                sentimentPosTD.Attributes.Add("class", "sentimentNegPosTD");

                                HtmlGenericControl posSpan = new HtmlGenericControl("span") {
                                    InnerHtml = string.Format("&nbsp;{0:N0}", posSentiment)
                                };
                                posSpan.Attributes.Add("class", "sentimentPosSpan");
                                posSpan.Attributes.Add("style", string.Format("width:{0}px;", posSentWidth));

                                sentimentPosTD.Controls.Add(posSpan);

                                sentimentTR.Controls.Add(sentimentNegTD);
                                sentimentTR.Controls.Add(sentimentPosTD);

                                sentimentTbl.Controls.Add(sentimentTR);

                                divControl.Controls.Add(sentimentTbl);
                                tc.Controls.Add(divControl);
                                break;
                            default:
                                break;
                        }

                        tr.Cells.Add(tc);
                    }

                    tableRows.Add(tr);
                }

                return tableRows;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return new List<HtmlTableRow>();
            }
            finally
            {
                TempData.Keep("AnalyticsTempData");
            }
        }

        private List<HtmlTableRow> BuildDemographicTableRows(AnalyticsSecondaryTable table, List<AnalyticsSummaryModel> selectedSummaries)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Log4NetLogger.Debug("BuildDemographicTableRows");
                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();

                List<HtmlTableRow> demoRows = new List<HtmlTableRow>();
                List<AnalyticsAgeRange> ageRanges = new List<AnalyticsAgeRange>();

                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "18-24",
                    MaleAudience = selectedSummaries.Sum(s => s.AM18_20 + s.AM21_24),
                    FemaleAudience = selectedSummaries.Sum(s => s.AF18_20 + s.AF21_24),
                    TotalAudience = selectedSummaries.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24)
                });
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "25-34",
                    MaleAudience = selectedSummaries.Sum(s => s.AM25_34),
                    FemaleAudience = selectedSummaries.Sum(s => s.AF25_34),
                    TotalAudience = selectedSummaries.Sum(s => s.AM25_34 + s.AF25_34)
                });
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "35-49",
                    MaleAudience = selectedSummaries.Sum(s => s.AM35_49),
                    FemaleAudience = selectedSummaries.Sum(s => s.AF35_49),
                    TotalAudience = selectedSummaries.Sum(s => s.AM35_49 + s.AF35_49)
                });
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "50-54",
                    MaleAudience = selectedSummaries.Sum(s => s.AM50_54),
                    FemaleAudience = selectedSummaries.Sum(s => s.AF50_54),
                    TotalAudience = selectedSummaries.Sum(s => s.AM50_54 + s.AF50_54)
                });
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "55-64",
                    MaleAudience = selectedSummaries.Sum(s => s.AM55_64),
                    FemaleAudience = selectedSummaries.Sum(s => s.AF55_64),
                    TotalAudience = selectedSummaries.Sum(s => s.AM55_64 + s.AF55_64)
                });
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "65+",
                    MaleAudience = selectedSummaries.Sum(s => s.AM65_Plus),
                    FemaleAudience = selectedSummaries.Sum(s => s.AF65_Plus),
                    TotalAudience = selectedSummaries.Sum(s => s.AM65_Plus + s.AF65_Plus)
                });
                ageRanges.Add(new AnalyticsAgeRange() {
                    AgeRange = "Total",
                    MaleAudience = selectedSummaries.Sum(s => s.MaleAudience),
                    FemaleAudience = selectedSummaries.Sum(s => s.FemaleAudience),
                    TotalAudience = selectedSummaries.Sum(s => s.MaleAudience + s.FemaleAudience)
                });

                foreach (AnalyticsAgeRange ar in ageRanges)
                {
                    HtmlTableRow tr = new HtmlTableRow() {
                        ID = string.Format("demographicTR_{0}", ar.AgeRange)
                    };
                    tr.Attributes.Add("class", "secondaryDetailRow");

                    HtmlTableCell tc = new HtmlTableCell();
                    tc.Attributes.Add("class", "secondaryDetailCBCol");

                    HtmlInputCheckBox cbControl = new HtmlInputCheckBox() {
                        ID = string.Format("demographicCB_{0}", ar.AgeRange),
                        Checked = true
                    };

                    cbControl.Attributes.Add("onclick", string.Format("ToggleSeries('{0}')", ar.AgeRange));
                    cbControl.Attributes.Add("class", "coloredCB");

                    HtmlGenericControl lblControl = new HtmlGenericControl("label");
                    lblControl.ID = string.Format("demographicLbl_{0}", ar.AgeRange);
                    lblControl.Attributes.Add("for", string.Format("demographicCB_{0}", ar.AgeRange));
                    lblControl.Attributes.Add("style", "display:none;");

                    HtmlGenericControl divControl = new HtmlGenericControl("div");
                    divControl.Attributes.Add("class", "coloredCBdiv");

                    HtmlGenericControl spnControl = new HtmlGenericControl("span");
                    spnControl.Attributes.Add("title", "select to remove from graph");
                    spnControl.Attributes.Add("class", "coloredCBspan");

                    divControl.Controls.Add(spnControl);
                    lblControl.Controls.Add(divControl);

                    tc.Controls.Add(cbControl);
                    tc.Controls.Add(lblControl);

                    tr.Cells.Add(tc);

                    // Age Range col
                    tc = new HtmlTableCell() {
                        ID = string.Format("demographicTD_{0}", ar.AgeRange),
                        InnerText = ar.AgeRange
                    };
                    tr.Cells.Add(tc);

                    // Male col
                    tc = new HtmlTableCell() {
                        InnerText = string.Format("{0:N0}", ar.MaleAudience)
                    };
                    tc.Attributes.Add("style", "text-align:right;");
                    tr.Cells.Add(tc);

                    // Female col
                    tc = new HtmlTableCell() {
                        InnerText = string.Format("{0:N0}", ar.FemaleAudience)
                    };
                    tc.Attributes.Add("style", "text-align:right;");
                    tr.Cells.Add(tc);

                    // Total col
                    tc = new HtmlTableCell() {
                        InnerText = string.Format("{0:N0}", ar.TotalAudience)
                    };
                    tc.Attributes.Add("style", "text-align:right;");
                    tr.Cells.Add(tc);

                    demoRows.Add(tr);
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("BuildDemographicTableRows: {0} ms", sw.ElapsedMilliseconds));

                return demoRows;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            finally
            {
                TempData.Keep("AnalyticsTempData");
            }
            return new List<HtmlTableRow>();
        }

        private List<HtmlTableRow> BuildTabSpecificTableRows(AnalyticsSecondaryTable table, List<AnalyticsSummaryModel> selectedSummaries)
        {
            try
            {
                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();
                //Log4NetLogger.Debug(string.Format("BuildTabSpecificTableRows"));
                //Log4NetLogger.Debug(string.Format("table.tabDisplay: {0}", table.TabDisplay));
                Dictionary<string, string> allGroups = new Dictionary<string, string>();

                allGroups = analyticsTempData.Groups[table.TabDisplay];

                //Log4NetLogger.Debug(string.Format("{0} groupings in allGroups", allGroups.Count));
                //Log4NetLogger.Debug("   Group Key | Group Value ");
                //Log4NetLogger.Debug("-------------+-------------");

                //foreach (var grp in allGroups)
                //{
                //    Log4NetLogger.Debug(string.Format(" {0,11} | {1}", grp.Key, grp.Value));
                //}

                foreach (AnalyticsSummaryModel summary in selectedSummaries)
                {
                    if (!string.IsNullOrEmpty(summary.SubMediaType))
                    {
                        var subMediaType = analyticsTempData.SubMediaTypes.Where(sm => sm.SubMediaType == summary.SubMediaType);
                        //Log4NetLogger.Debug(string.Format("summary.SubMediaType: {0}", summary.SubMediaType));
                        summary.SMTDisplayName = subMediaType.Any() ? subMediaType.First().DisplayName : "";
                    }
                    if (!string.IsNullOrWhiteSpace(summary.Market))
                    {
                        var marketID = analyticsTempData.Groups["market"].Where(dma => dma.Value == summary.Market);
                        summary.MarketID = marketID.Any() ? Convert.ToInt64(marketID.First().Key) : -1;
                    }
                }

                AnalyticsSummaryModel summModel = new AnalyticsSummaryModel();
                PropertyInfo groupByPI = summModel.GetType().GetProperty(table.GroupBy);
                PropertyInfo groupByDisplayPI = summModel.GetType().GetProperty(table.GroupByDisplay);
                // Remove all summaries which have null values for property trying to group around
                selectedSummaries = selectedSummaries.Where(w => groupByPI.GetValue(w, null) != null).ToList();

                List<AnalyticsGrouping> groupings = new List<AnalyticsGrouping>();
                foreach (var grp in allGroups)
                {
                    var groupSummaries = selectedSummaries.Where(w => groupByPI.GetValue(w, null).ToString() == grp.Key).ToList();
                    groupings.Add(new AnalyticsGrouping() {
                        ID = grp.Key,
                        Name = grp.Value,
                        Summaries = groupSummaries
                    });
                }

                List<HtmlTableRow> tableRows = new List<HtmlTableRow>();
                int count = 0;

                // For each grouping of summaries, create a table row
                foreach (var group in groupings.OrderByDescending(g => g.Summaries.Sum(s => s.Number_Of_Hits)))
                {
                    // Do not want any summaries that don't have value for group by
                    if (group.ID != null)
                    {
                        count += 1;

                        HtmlTableRow tr = new HtmlTableRow() {
                            ID = string.Format("{0}TR_{1}", table.GroupByHeader, group.ID)
                        };
                        tr.Attributes.Add("class", "secondaryDetailRow");

                        HtmlTableCell tc = new HtmlTableCell();
                        tc.Attributes.Add("class", "secondaryDetailCBCol");

                        HtmlInputCheckBox cbControl = new HtmlInputCheckBox() {
                            ID = string.Format("{0}CB_{1}", table.GroupByHeader, group.ID)
                        };

                        if (count <= 10)
                        {
                            cbControl.Checked = true;
                        }

                        cbControl.Attributes.Add("onclick", string.Format("ToggleSeries('{0}')", group.ID));
                        cbControl.Attributes.Add("class", "coloredCB");

                        HtmlGenericControl lblControl = new HtmlGenericControl("label");
                        lblControl.Attributes.Add("for", string.Format("{0}CB_{1}", table.GroupByHeader, group.ID));

                        HtmlGenericControl divControl = new HtmlGenericControl("div");
                        divControl.Attributes.Add("class", "coloredCBdiv");

                        HtmlGenericControl spnControl = new HtmlGenericControl("span");
                        spnControl.Attributes.Add("title", "select to add to graph");
                        spnControl.Attributes.Add("class", "coloredCBspan");

                        divControl.Controls.Add(spnControl);
                        lblControl.Controls.Add(divControl);

                        tc.Controls.Add(cbControl);
                        tc.Controls.Add(lblControl);

                        tr.Cells.Add(tc);

                        // Group by col
                        tr.Cells.Add(new HtmlTableCell() {
                            InnerText = string.Format("{0}", group.Name),
                            ID = string.Format("{0}TD_{1}", table.GroupByHeader, group.ID)
                        });

                        List<string> columnHeader = new List<string>();
                        if (sessionInfo.isv5LRAccess && sessionInfo.Isv5AdsAccess)
                        {
                            columnHeader = table.ColumnHeadersAdsLR;
                        }
                        else if (sessionInfo.Isv5AdsAccess)
                        {
                            columnHeader = table.ColumnHeadersAds;
                        }
                        else if (sessionInfo.isv5LRAccess)
                        {
                            columnHeader = table.ColumnHeadersLR;
                        }
                        else
                        {
                            columnHeader = table.ColumnHeaders;
                        }

                        foreach (string column in columnHeader)
                        {
                            tc = new HtmlTableCell();
                            tc.Attributes.Add("style", "text-align:right;");

                            switch (column)
                            {
                                case "agent":
                                    tc.InnerText = string.Format("{0}", group.Summaries.First().Query_Name);
                                    tc.Attributes.Clear();
                                    break;
                                case "occurrences":
                                    tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.Number_Of_Hits));
                                    tc.ID = string.Format("OCCURRENCES_{0}", group.ID);
                                    tc.Attributes.Add("OnlineCount", group.Summaries.Where(w => analyticsTempData.OnlineSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits).ToString());
                                    tc.Attributes.Add("PrintCount", group.Summaries.Where(w => analyticsTempData.PrintSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits).ToString());
                                    break;
                                case "seen":
                                    if (sessionInfo.isv5LRAccess)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.SeenEarned + s.SeenPaid));
                                        tc.ID = string.Format("SEEN_{0}", group.ID);
                                    }
                                    break;
                                case "heard":
                                    if (sessionInfo.isv5LRAccess)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.HeardEarned + s.HeardPaid));
                                        tc.ID = string.Format("HEARD_{0}", group.ID);
                                    }
                                    break;
                                case "read":
                                    if (sessionInfo.isv5LRAccess)
                                    {
                                        Int64 sumRead = group.Summaries.Sum(s => s.Number_Of_Hits - (s.SeenEarned + s.SeenPaid + s.HeardEarned + s.HeardPaid));
                                        tc.InnerText = string.Format("{0:N0}", sumRead < 0 ? 0 : sumRead);
                                        tc.ID = string.Format("READ_{0}", group.ID);
                                    }
                                    break;
                                case "paid":
                                    if (sessionInfo.Isv5AdsAccess)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.HeardPaid + s.SeenPaid));
                                        tc.ID = string.Format("PAID_{0}", group.ID);
                                    }
                                    break;
                                case "earned":
                                    if (sessionInfo.Isv5AdsAccess)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.HeardEarned + s.SeenEarned));
                                        tc.ID = string.Format("EARNED_{0}", group.ID);
                                    }
                                    break;
                                case "on air time":
                                    long onAir = group.Summaries.Sum(s => ((s.HeardEarned + s.HeardPaid) * 8 + (s.SeenEarned + s.SeenPaid)));
                                    var sumOnAir = group.Summaries.Where(w => analyticsTempData.OnAirSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits);
                                    tc.InnerText = string.Format("{0:00}:{1:00}:{2:00}", onAir / 3600, (onAir / 60) % 60, onAir % 60);
                                    tc.ID = string.Format("ONAIRTIME_{0}", group.ID);
                                    tc.Attributes.Add("PESHvalue", sumOnAir.ToString());
                                    break;
                                case "audience":
                                    tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.Audience));
                                    break;
                                case "ad value":
                                    tc.InnerText = string.Format("{0:C2}", group.Summaries.Sum(s => s.IQMediaValue));
                                    break;
                                case "sentiment":
                                    tc.Attributes.Add("class", "sentimentTDContainer");

                                    divControl = new HtmlGenericControl("div");
                                    divControl.Attributes.Add("class", "sentimentDivContainer");
                                    divControl.Attributes.Add("align", "center");

                                    HtmlTable sentimentTbl = new HtmlTable() {
                                        CellPadding = 0,
                                        CellSpacing = 0
                                    };
                                    sentimentTbl.Attributes.Add("style", "height:100%;");

                                    HtmlTableRow sentimentTR = new HtmlTableRow();
                                    sentimentTR.Attributes.Add("class", "sentimentTRContainer");

                                    long negSentiment = group.Summaries.Sum(s => s.NegativeSentiment);
                                    long posSentiment = group.Summaries.Sum(s => s.PositiveSentiment);
                                    long negSentWidth = 0;
                                    long posSentWidth = 0;

                                    if (negSentiment > 0)
                                    {
                                        negSentWidth = (group.Summaries.Sum(s => s.NegativeSentiment).ToString().Length * 8) + 5;
                                    }
                                    if (posSentiment > 0)
                                    {
                                        posSentWidth = (group.Summaries.Sum(s => s.PositiveSentiment).ToString().Length * 8) + 5;
                                    }

                                    if (negSentWidth > 51)
                                    {
                                        negSentWidth = 51;
                                    }
                                    if (posSentWidth > 51)
                                    {
                                        posSentWidth = 51;
                                    }

                                    HtmlTableCell sentimentNegTD = new HtmlTableCell();
                                    sentimentNegTD.Attributes.Add("class", "sentimentNegPosTD");

                                    HtmlGenericControl negSpan = new HtmlGenericControl("span") {
                                        InnerHtml = string.Format("{0:N0}&nbsp;", negSentiment)
                                    };
                                    negSpan.Attributes.Add("class", "sentimentNegSpan");
                                    negSpan.Attributes.Add("style", string.Format("width:{0}px;", negSentWidth));

                                    sentimentNegTD.Controls.Add(negSpan);

                                    HtmlTableCell sentimentPosTD = new HtmlTableCell();
                                    sentimentPosTD.Attributes.Add("class", "sentimentNegPosTD");

                                    HtmlGenericControl posSpan = new HtmlGenericControl("span") {
                                        InnerHtml = string.Format("&nbsp;{0:N0}", posSentiment)
                                    };
                                    posSpan.Attributes.Add("class", "sentimentPosSpan");
                                    posSpan.Attributes.Add("style", string.Format("width:{0}px;", posSentWidth));

                                    sentimentPosTD.Controls.Add(posSpan);

                                    sentimentTR.Controls.Add(sentimentNegTD);
                                    sentimentTR.Controls.Add(sentimentPosTD);

                                    sentimentTbl.Controls.Add(sentimentTR);

                                    divControl.Controls.Add(sentimentTbl);
                                    tc.Controls.Add(divControl);
                                    break;
                                default:
                                    break;
                            }

                            tr.Cells.Add(tc);
                        }

                        tableRows.Add(tr);
                    }
                }

                return tableRows;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            finally
            {
                TempData.Keep("AnalyticsTempData");
            }
            return new List<HtmlTableRow>();
        }

        private List<Series> GetAllSeries(List<AnalyticsSummaryModel> agentSummaries, AnalyticsSecondaryTable table, DateTime startDate, DateTime endDate, string interval, int chartType)
        {
            try
            {
                /* Chart Types */
                // 1. Line
                // 2. US
                // 3. Canada
                // 4. Pie
                // 5. Bar
                // 6. Column
                // 7. daytime heat map
                // 8. growth
                // 9. daypart heat map

                /* Series/Points type */
                // 1. multi/multi
                // 2. multi/single
                // 3. single/multi

                /* Series/Points type - chart types */
                // 1 - 1, 8
                // 2 - 5, 6
                // 3 - 4, 7, 9

                // Line/Growth charts only charts where points in series correspond to dates

                //Log4NetLogger.Debug(string.Format("table.TabDisplay: {0}", table.TabDisplay));
                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();
                Dictionary<string, string> allGroups = new Dictionary<string, string>();
                allGroups = analyticsTempData.Groups[table.TabDisplay];

                foreach (AnalyticsSummaryModel summary in agentSummaries)
                {
                    if (!string.IsNullOrEmpty(summary.SubMediaType))
                    {
                        var subMediaType = analyticsTempData.SubMediaTypes.Where(sm => sm.SubMediaType == summary.SubMediaType);
                        //Log4NetLogger.Debug(string.Format("summary.SubMediaType: {0}", summary.SubMediaType));
                        summary.SMTDisplayName = subMediaType.Any() ? subMediaType.First().DisplayName : "";
                    }
                    if (!string.IsNullOrWhiteSpace(summary.Market))
                    {
                        var marketID = analyticsTempData.Groups["market"].Where(dma => dma.Value == summary.Market);
                        summary.MarketID = marketID.Any() ? Convert.ToInt64(marketID.First().Key) : -1;
                    }
                }

                List<DateTime> dateRange = new List<DateTime>();
                TimeSpan span = endDate.Subtract(startDate);
                switch (interval)
                {
                    case "hour":
                        for (int i = 0; i <= span.TotalHours; i++)
                        {
                            dateRange.Add(startDate.AddHours(i));
                        }
                        break;
                    case "day":
                        for (int i = 0; i <= span.TotalDays; i++)
                        {
                            dateRange.Add(startDate.AddDays(i));
                        }
                        break;
                }
                List<Series> allSeries = new List<Series>();

                AnalyticsSummaryModel summModel = new AnalyticsSummaryModel();
                PropertyInfo groupByPI = summModel.GetType().GetProperty(table.GroupBy);
                Log4NetLogger.Debug(string.Format("groupByPI.name: {0}", groupByPI.Name));
                PropertyInfo groupByDisplayPI = summModel.GetType().GetProperty(table.GroupByDisplay);
                Log4NetLogger.Debug(string.Format("groupByDisplayPI.name: {0}", groupByDisplayPI.Name));
                // Remove all summaries which have null values for property trying to group around
                var selectedSummaries = agentSummaries.Where(w => groupByPI.GetValue(w, null) != null).ToList();

                List<AnalyticsGrouping> groupings = new List<AnalyticsGrouping>();
                foreach (var grp in allGroups)
                {
                    var groupSummaries = selectedSummaries.Where(w => groupByPI.GetValue(w, null).ToString() == grp.Key).ToList();
                    groupings.Add(new AnalyticsGrouping() {
                        ID = grp.Key,
                        Name = grp.Value,
                        Summaries = groupSummaries
                    });
                }

                foreach (var group in groupings.OrderByDescending(g => g.Summaries.Sum(s => s.Number_Of_Hits)))
                {
                    // Line/Growth
                    if (chartType == 1 || chartType == 8)
                    {
                        Series newSeries = new Series() {
                            name = group.Name,
                            data = new List<HighChartDatum>()
                        };
                        foreach (var date in dateRange)
                        {
                            var summsForDate = group.Summaries.Where(summ => summ.SummaryDateTime.Equals(date));
                            HighChartDatum hcd = new HighChartDatum() {
                                Date = date.ToShortDateString(),
                                SearchName = group.Name,
                                Value = group.ID,
                                y = summsForDate.Any() ? summsForDate.Sum(s => s.Number_Of_Hits) : 0
                            };

                            newSeries.data.Add(hcd);
                        }
                        allSeries.Add(newSeries);
                    }
                }

                return allSeries;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return new List<Series>();
            }
        }

        #endregion

        [HttpPost]
        public ContentResult GetActiveElements()
        {
            try
            {
                AnalyticsLogic analyticsLogic = new AnalyticsLogic();
                List<AnalyticsActiveElement> activeElements = analyticsLogic.GetActiveElements();

                dynamic jsonResult = new ExpandoObject();
                if (activeElements.Any())
                {
                    jsonResult.isSuccess = true;
                    jsonResult.activeElements = activeElements;
                }
                else
                {
                    jsonResult.isSuccess = false;
                }

                return Content(JsonConvert.SerializeObject(jsonResult), "application/json", Encoding.UTF8);
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
                return Content(CommonFunctions.GetSuccessFalseJson(), "application/json", Encoding.UTF8);
            }
        }
    }
}
