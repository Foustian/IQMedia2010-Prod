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
        List<string> lstTopTen = new List<string>();
        Dictionary<string, string> dctTopTen = new Dictionary<string, string>();
        #endregion

        public ActionResult Index(string type)
        {
            try
            {
                sessionInfo = ActiveUserMgr.GetActiveUser();
                //Log4NetLogger.Debug(string.Format("Controller Index - Client GUID: {0}", sessionInfo.ClientGUID));
                //Log4NetLogger.Debug(string.Format("sessionInfo.gmt: {0}", sessionInfo.gmt));
                IQAgentLogic iQAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                AnalyticsLogic analyticsLogic = new AnalyticsLogic();

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

                analyticsTempData.ClientAnalyticsCampaigns = GetCampaignsByGUID(sessionInfo.ClientGUID);
                analyticsTempData.ActiveElements = analyticsLogic.GetActiveElements();

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
        public ContentResult GetMainTable(AnalyticsRequest graphRequest)
        {
            try
            {
                //Log4NetLogger.Debug("GetMainTable");
                var sw = new Stopwatch();
                sw.Start();

                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();
                AnalyticsLogic analyticsLogic = new AnalyticsLogic();

                List<AnalyticsSecondaryTable> tables = new List<AnalyticsSecondaryTable>();
                // Summaries for MSTable - summaries from all agents during time period
                AnalyticsDataModel mstData = new AnalyticsDataModel();
                bool useCampaignIDs = graphRequest.IsFilter && graphRequest.RequestIDs != null;

                // If no previous request
                if (analyticsTempData.PrevRequest == null)
                {
                    tables = analyticsLogic.GetSecondaryTables(graphRequest.Tab, graphRequest.PageType);
                    analyticsTempData.PrevSecondaryTables = tables;
                }
                else
                {
                    // Check if current request would return same tables as previous request
                    if (analyticsTempData.PrevRequest.PageType == graphRequest.PageType && analyticsTempData.PrevRequest.Tab == graphRequest.Tab)
                    {
                        tables = analyticsTempData.PrevSecondaryTables;
                    }
                    else
                    {
                        tables = analyticsLogic.GetSecondaryTables(graphRequest.Tab, graphRequest.PageType);
                        analyticsTempData.PrevSecondaryTables = tables;
                    }
                }

                // Get main, will be table with tab display of agent or campaign after getting secondary tables, never have both tables
                AnalyticsSecondaryTable msTable = tables.First(tbl => tbl.TabDisplay.Equals("agent") || tbl.TabDisplay.Equals("campaign"));
                PropertyInfo msGroupBy = typeof(AnalyticsSummaryModel).GetProperty(msTable.GroupBy);
                PropertyInfo msGroupByDisplay = typeof(AnalyticsSummaryModel).GetProperty(msTable.GroupByDisplay);

                if (graphRequest.PageType == "campaign")
                {
                    graphRequest.Campaigns = analyticsTempData.ClientAnalyticsCampaigns;

                    if (analyticsTempData.PrevRequest != null && analyticsTempData.PrevRequest.PageType == "campaign" && graphRequest.RequestIDs != null)
                    {
                        bool sameSMT = analyticsTempData.PrevRequest.SubMediaType == graphRequest.SubMediaType;
                        bool sameDateInterval = analyticsTempData.PrevRequest.DateInterval == graphRequest.DateInterval;
                        bool sameIsFilter = analyticsTempData.PrevRequest.IsFilter == graphRequest.IsFilter;
                        if (analyticsTempData.PrevRequest.RequestIDs == null)
                        {
                            analyticsTempData.PrevRequest.RequestIDs = new List<long>();
                        }
                        bool sameRequestIDs = analyticsTempData.PrevRequest.RequestIDs.SequenceEqual(graphRequest.RequestIDs);

                        //Log4NetLogger.Debug(string.Format("sameSMT: {0}, sameDateInterval: {1}, sameIsFilter: {2}, sameRequestIDs: {3}", sameSMT, sameDateInterval, sameIsFilter, sameRequestIDs));
                        if (sameDateInterval && sameIsFilter && sameRequestIDs && sameSMT)
                        {
                            mstData = analyticsTempData.PrevCampaignData;
                        }
                        else
                        {
                            mstData = GetCampaignSummaries(graphRequest, useCampaignIDs);
                            SetCampaignDates(mstData, graphRequest);
                            analyticsTempData.PrevCampaignData = mstData;
                        }
                    }
                    else
                    {
                        mstData = GetCampaignSummaries(graphRequest, useCampaignIDs);
                        SetCampaignDates(mstData, graphRequest);
                        analyticsTempData.PrevCampaignData = mstData;
                    }
                }
                else
                {
                    ConvertDatesFromInterval(graphRequest);

                    // If filtering to specific agents then only want those agents in MST Data
                    if (graphRequest.IsFilter && graphRequest.RequestIDs != null && graphRequest.RequestIDs.Count > 0)
                    {
                        mstData = GetAgentSummaries(sessionInfo.ClientGUID, graphRequest);
                        mstData.DmaMentionMapList = mstData.DmaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                        mstData.CanadaMentionMapList = mstData.CanadaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                    }
                    else
                    {
                        mstData = GetAllAgentSummaries(sessionInfo.ClientGUID, Convert.ToDateTime(graphRequest.DateFrom), Convert.ToDateTime(graphRequest.DateTo), graphRequest.DateInterval, graphRequest.SubMediaType);
                    }
                }

                // If selected agents null - get top 5 agents or campaigns - will happen on page load - used for checking MST rows
                if (graphRequest.RequestIDs == null)
                {
                    graphRequest.RequestIDs = new List<Int64>();
                    var summaryGroups = graphRequest.PageType == "campaign" ? mstData.SummaryDataList.GroupBy(gb => gb.CampaignID) : mstData.SummaryDataList.GroupBy(gb => gb.SearchRequestID);
                    var count = 0;
                    foreach (var summGroup in summaryGroups.OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits)))
                    {
                        // Limit is arbitrary
                        if (count < 5)
                        {
                            graphRequest.RequestIDs.Add(graphRequest.PageType == "campaign" ? summGroup.First().CampaignID : summGroup.First().SearchRequestID);
                        }
                        count += 1;
                    }
                }

                HtmlTable msTableHTML = new HtmlTable();
                msTableHTML.Attributes["class"] = "table clearBorders font12Pt";
                msTableHTML.Attributes["id"] = string.Format("tbl_{0}", msTable.TabDisplay);

                msTableHTML.Rows.Add(CreateSecondaryTableHeader(msTable, (graphRequest.Tab == SecondaryTabID.Networks || graphRequest.Tab == SecondaryTabID.Shows) ? true : false));

                BuildMainTableRows(msTable, mstData.SummaryDataList, graphRequest.RequestIDs, (graphRequest.Tab == SecondaryTabID.Networks || graphRequest.Tab == SecondaryTabID.Shows) ? true : false).ForEach(e => {
                    msTableHTML.Rows.Add(e);
                });

                StringWriter msTableStrWriter = new StringWriter();
                msTableHTML.RenderControl(new System.Web.UI.HtmlTextWriter(msTableStrWriter));

                dynamic jsonResult = new ExpandoObject();
                jsonResult.MSTable = msTableStrWriter.ToString();
                jsonResult.MSTableTab = msTable.TabDisplay;

                // If not on OT tab there will be a second table, the TSST
                if (tables.Count > 1)
                {
                    // If not on OT tab, maps are disabled so no need to get/set agent or canada map lists
                    AnalyticsSecondaryTable tssTable = tables.First(tbl => string.Compare(tbl.TabDisplay, graphRequest.Tab.ToString(), true) == 0);
                    // Summaries for TSSTable - summaries from selected agents
                    AnalyticsDataModel tsstData = new AnalyticsDataModel();

                    if (graphRequest.PageType == "campaign")
                    {
                        tsstData.SummaryDataList = mstData.SummaryDataList.Where(w => graphRequest.RequestIDs.Contains(w.CampaignID)).ToList();
                    }
                    else
                    {
                        tsstData.SummaryDataList = mstData.SummaryDataList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                    }

                    tsstData.DmaMentionMapList = mstData.DmaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                    tsstData.CanadaMentionMapList = mstData.CanadaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();

                    HtmlTable tssTableHTML = new HtmlTable();
                    tssTableHTML.Attributes["class"] = "table clearBorders font12Pt";
                    tssTableHTML.Attributes["id"] = string.Format("tbl_{0}", tssTable.TabDisplay);
                    tssTableHTML.Rows.Add(CreateSecondaryTableHeader(tssTable, (graphRequest.Tab == SecondaryTabID.Networks || graphRequest.Tab == SecondaryTabID.Shows) ? true : false));

                    if (graphRequest.Tab == SecondaryTabID.Demographic)
                    {
                        BuildDemographicTableRows(tssTable, tsstData.SummaryDataList).ForEach(e => {
                            tssTableHTML.Rows.Add(e);
                        });
                    }
                    else if (graphRequest.Tab == SecondaryTabID.Networks || graphRequest.Tab == SecondaryTabID.Shows)
                    {
                        BuildNetworkShowTabSpecificTableRows(tssTable, tsstData.SummaryDataList,true).ForEach(e =>
                        {
                            tssTableHTML.Rows.Add(e);
                        });
                    }
                    else
                    {
                        BuildTabSpecificTableRows(tssTable, tsstData.SummaryDataList).ForEach(e =>
                        {
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
                        // Demo chart is unique, in that it does not use same grouping paradigm as other tabs do
                        tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, tsstData, new Dictionary<string, string>(), analyticsTempData.SubMediaTypes);
                    }
                    else
                    {
                        tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, tsstData, ((graphRequest.Tab == SecondaryTabID.Networks || graphRequest.Tab == SecondaryTabID.Shows) ? dctTopTen : analyticsTempData.Groups[tssTable.TabDisplay]), analyticsTempData.SubMediaTypes);
                    }

                    jsonResult.TSSTSeries = tsstChart["series"];
                    jsonResult.chartJSON = tsstChart["chart"];
                }
                else
                {
                    var otChart = analyticsLogic.GetChart(graphRequest, msTable, mstData, ((graphRequest.Tab == SecondaryTabID.Networks || graphRequest.Tab == SecondaryTabID.Shows) ? dctTopTen : analyticsTempData.Groups[msTable.TabDisplay]), analyticsTempData.SubMediaTypes);
                    jsonResult.chartJSON = otChart["chart"];
                    jsonResult.TSSTable = string.Empty;
                    jsonResult.TSSTableTab = string.Empty;
                }

                analyticsTempData.PrevRequest = graphRequest;
                jsonResult.isSuccess = true;
                jsonResult.isLRAccess = sessionInfo.isv5LRAccess;
                jsonResult.isAdsAccess = sessionInfo.Isv5AdsAccess;

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetMainTable: {0} ms", sw.ElapsedMilliseconds));

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
        public ContentResult GetTabSpecificTable(AnalyticsRequest graphRequest)
        {
            // When method called, it is assumed that MainTable is already on page - no need to build/return main table for this method
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug("GetTabSpecificTable");
                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();

                AnalyticsLogic analyticsLogic = new AnalyticsLogic();

                List<AnalyticsSecondaryTable> tables = new List<AnalyticsSecondaryTable>();

                // If no previous request - shouldn't happen if this method called
                if (analyticsTempData.PrevRequest == null)
                {
                    tables = analyticsLogic.GetSecondaryTables(graphRequest.Tab, graphRequest.PageType);
                    analyticsTempData.PrevSecondaryTables = tables;
                }
                else
                {
                    // Check if current request would return same tables as previous request
                    if (analyticsTempData.PrevRequest.PageType == graphRequest.PageType && analyticsTempData.PrevRequest.Tab == graphRequest.Tab)
                    {
                        tables = analyticsTempData.PrevSecondaryTables;
                    }
                    else
                    {
                        tables = analyticsLogic.GetSecondaryTables(graphRequest.Tab, graphRequest.PageType);
                        analyticsTempData.PrevSecondaryTables = tables;
                    }
                }

                AnalyticsSecondaryTable tssTable = tables.First(tbl => string.Compare(tbl.TabDisplay, graphRequest.Tab.ToString(), true) == 0);
                AnalyticsDataModel tsstData = new AnalyticsDataModel();
                List<AnalyticsSummaryModel> selectedSummaries = new List<AnalyticsSummaryModel>();

                if (graphRequest.PageType == "campaign")
                {
                    graphRequest.Campaigns = analyticsTempData.ClientAnalyticsCampaigns;
                    if (graphRequest.RequestIDs != null && graphRequest.RequestIDs.Count > 0)
                    {
                        bool sameSMT = analyticsTempData.PrevRequest.SubMediaType == graphRequest.SubMediaType;
                        bool sameDateInterval = analyticsTempData.PrevRequest.DateInterval == graphRequest.DateInterval;
                        bool sameIsFilter = analyticsTempData.PrevRequest.IsFilter == graphRequest.IsFilter;
                        if (analyticsTempData.PrevRequest.RequestIDs == null)
                        {
                            analyticsTempData.PrevRequest.RequestIDs = new List<long>();
                        }
                        bool sameRequestIDs = analyticsTempData.PrevRequest.RequestIDs.SequenceEqual(graphRequest.RequestIDs);
                        //Log4NetLogger.Debug(string.Format("sameSMT: {0}, sameDateInterval: {1}, sameIsFilter: {2}, sameRequestIDs: {3}", sameSMT, sameDateInterval, sameIsFilter, sameRequestIDs));
                        // If previous request same as current use data stored in temp
                        if (analyticsTempData.PrevRequest.PageType == "campaign" && sameDateInterval && sameIsFilter && sameRequestIDs && sameSMT)
                        {
                            tsstData = analyticsTempData.PrevCampaignData;
                        }
                        else
                        {
                            tsstData = GetCampaignSummaries(graphRequest, true);
                            SetCampaignDates(tsstData, graphRequest);
                            analyticsTempData.PrevCampaignData = tsstData;
                        }
                    }
                }
                else
                {
                    ConvertDatesFromInterval(graphRequest);
                    tsstData = GetAgentSummaries(sessionInfo.ClientGUID, graphRequest);
                }

                HtmlTable tableHTML = new HtmlTable();
                tableHTML.Attributes["class"] = "table clearBorders font12Pt";
                tableHTML.Attributes["id"] = string.Format("tbl_{0}", tssTable.TabDisplay);
                tableHTML.Rows.Add(CreateSecondaryTableHeader(tssTable));
                List<HtmlTableRow> detailRows = new List<HtmlTableRow>();

                if (graphRequest.Tab != SecondaryTabID.Demographic)
                {
                    BuildTabSpecificTableRows(tssTable, tsstData.SummaryDataList).ForEach(e => {
                        tableHTML.Rows.Add(e);
                    });
                }
                else
                {
                    BuildDemographicTableRows(tssTable, tsstData.SummaryDataList).ForEach(e => {
                        tableHTML.Rows.Add(e);
                    });
                }

                StringWriter strWriter = new StringWriter();
                tableHTML.RenderControl(new System.Web.UI.HtmlTextWriter(strWriter));

                analyticsTempData.PrevRequest = graphRequest;

                dynamic jsonResult = new ExpandoObject();
                jsonResult.isSuccess = true;
                jsonResult.isLRAccess = sessionInfo.isv5LRAccess;
                jsonResult.isAdsAccess = sessionInfo.Isv5AdsAccess;
                jsonResult.TSSTable = strWriter.ToString();
                jsonResult.TSSTableTab = tssTable.TabDisplay;

                Dictionary<string, object> tsstChart = new Dictionary<string, object>();
                if (graphRequest.RequestIDs != null && graphRequest.RequestIDs.Count > 0)
                {
                    if (graphRequest.Tab == SecondaryTabID.Demographic)
                    {
                        // Demo chart is unique, in that it does not use same grouping paradigm as other tabs do
                        tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, tsstData, new Dictionary<string, string>(), analyticsTempData.SubMediaTypes);
                    }
                    else
                    {
                        tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, tsstData, analyticsTempData.Groups[tssTable.TabDisplay], analyticsTempData.SubMediaTypes);
                    }
                    jsonResult.chartJSON = tsstChart["chart"];
                    jsonResult.TSSTSeries = tsstChart["series"];
                }
                else
                {
                    jsonResult.chartJSON = "";
                    jsonResult.TSSTSeries = "";
                }

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
        public ContentResult GetNetworkShowTabTable(AnalyticsRequest graphRequest)
        {
            // When method called, it is assumed that MainTable is already on page - no need to build/return main table for this method
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();
                //Log4NetLogger.Debug(string.Format("GetNetworkShowTabTable: {0} ms", sw.ElapsedMilliseconds));

                AnalyticsLogic analyticsLogic = new AnalyticsLogic();
                List<AnalyticsSecondaryTable> tables = analyticsLogic.GetSecondaryTables(graphRequest.Tab, graphRequest.PageType);

                //Network/Show tab data
                AnalyticsSecondaryTable tssTable = tables.First(tbl => tbl.TabDisplay == graphRequest.Tab.ToString().ToLower());
                AnalyticsDataModel tsstData = new AnalyticsDataModel();
                tsstData = GetNetworkShowSummaries(sessionInfo.ClientGUID, graphRequest);

                //Main tab data
                AnalyticsSecondaryTable msTable = tables.First(tbl => tbl.TabDisplay.Equals("agent"));
                AnalyticsDataModel mstData = new AnalyticsDataModel();
                ConvertDatesFromInterval(graphRequest);
                // If filtering to specific agents then only want those agents in MST Data
                if (graphRequest.IsFilter && graphRequest.RequestIDs != null && graphRequest.RequestIDs.Count > 0)
                {
                    mstData = GetNetworkShowSummaries(sessionInfo.ClientGUID, graphRequest);
                    mstData.DmaMentionMapList = mstData.DmaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                    mstData.CanadaMentionMapList = mstData.CanadaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                }
                else
                {
                    mstData = GetAllAgentSummaries(sessionInfo.ClientGUID, Convert.ToDateTime(graphRequest.DateFrom), Convert.ToDateTime(graphRequest.DateTo), graphRequest.DateInterval, graphRequest.SubMediaType);
                }

                //MainTab HTML
                HtmlTable msTableHTML = new HtmlTable();
                msTableHTML.Attributes["class"] = "table clearBorders font12Pt";
                msTableHTML.Attributes["id"] = string.Format("tbl_{0}", msTable.TabDisplay);
                msTableHTML.Rows.Add(CreateSecondaryTableHeader(msTable, true));
                BuildMainTableRows(msTable, mstData.SummaryDataList, graphRequest.RequestIDs, (graphRequest.Tab == SecondaryTabID.Networks || graphRequest.Tab == SecondaryTabID.Shows) ? true : false).ForEach(e =>
                {
                    msTableHTML.Rows.Add(e);
                });
                StringWriter msTableStrWriter = new StringWriter();
                msTableHTML.RenderControl(new System.Web.UI.HtmlTextWriter(msTableStrWriter));


                //Network/Show HTML
                HtmlTable tableHTML = new HtmlTable();
                tableHTML.Attributes["class"] = "table clearBorders font12Pt";
                tableHTML.Attributes["id"] = string.Format("tbl_{0}", tssTable.TabDisplay);
                tableHTML.Rows.Add(CreateSecondaryTableHeader(tssTable, true));
                BuildNetworkShowTabSpecificTableRows(tssTable, tsstData.SummaryDataList, true).ForEach(e =>
                {
                    tableHTML.Rows.Add(e);
                });
                StringWriter strWriter = new StringWriter();
                tableHTML.RenderControl(new System.Web.UI.HtmlTextWriter(strWriter));


                dynamic jsonResult = new ExpandoObject();
                jsonResult.MSTable = msTableStrWriter.ToString();
                jsonResult.MSTableTab = msTable.TabDisplay;
                jsonResult.isSuccess = true;
                jsonResult.isLRAccess = sessionInfo.isv5LRAccess;
                jsonResult.isAdsAccess = sessionInfo.Isv5AdsAccess;
                jsonResult.TSSTable = strWriter.ToString();
                jsonResult.TSSTableTab = tssTable.TabDisplay;

                Dictionary<string, object> tsstChart = new Dictionary<string, object>();
                tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, tsstData, dctTopTen, analyticsTempData.SubMediaTypes);
                jsonResult.chartJSON = tsstChart["chart"];
                jsonResult.TSSTSeries = tsstChart["series"];

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetNetworkShowTabTable: {0} ms", sw.ElapsedMilliseconds));
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
        public ContentResult GetChart(AnalyticsRequest graphRequest)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug(string.Format("GetChart"));

                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);

                List<AnalyticsSecondaryTable> tables = new List<AnalyticsSecondaryTable>();

                // If no previous request - shouldnt happen if this method called
                if (analyticsTempData.PrevRequest == null)
                {
                    analyticsTempData.PrevRequest = graphRequest;
                    tables = analyticsLogic.GetSecondaryTables(graphRequest.Tab, graphRequest.PageType);
                    analyticsTempData.PrevSecondaryTables = tables;
                }
                else
                {
                    // Check if current request would return same tables as previous request
                    if (analyticsTempData.PrevRequest.PageType == graphRequest.PageType && analyticsTempData.PrevRequest.Tab == graphRequest.Tab)
                    {
                        tables = analyticsTempData.PrevSecondaryTables;
                    }
                    else
                    {
                        tables = analyticsLogic.GetSecondaryTables(graphRequest.Tab, graphRequest.PageType);
                        analyticsTempData.PrevSecondaryTables = tables;
                    }
                }

                AnalyticsDataModel dataModel = new AnalyticsDataModel();

                AnalyticsSecondaryTable tssTable;
                if (tables.Count == 1)  // If only one table returned chart is for OverTime
                {
                    tssTable = tables.ElementAt(0);
                }
                else
                {
                    tssTable = tables.First(tbl => string.Compare(tbl.TabDisplay, graphRequest.Tab.ToString(), true) == 0);
                }

                // Selected agents are only relevant for creating TSSTable and checking MSTable rows previously selected
                if (graphRequest.PageType == "campaign")
                {
                    graphRequest.Campaigns = analyticsTempData.ClientAnalyticsCampaigns;
                    if (graphRequest.RequestIDs != null && analyticsTempData.PrevRequest != null)
                    {
                        bool sameSMT = analyticsTempData.PrevRequest.SubMediaType == graphRequest.SubMediaType;
                        bool sameDateInterval = analyticsTempData.PrevRequest.DateInterval == graphRequest.DateInterval;
                        bool sameIsFilter = analyticsTempData.PrevRequest.IsFilter == graphRequest.IsFilter;
                        if (analyticsTempData.PrevRequest.RequestIDs == null)
                        {
                            analyticsTempData.PrevRequest.RequestIDs = new List<long>();
                        }
                        bool sameRequestIDs = analyticsTempData.PrevRequest.RequestIDs.SequenceEqual(graphRequest.RequestIDs);

                        if (analyticsTempData.PrevRequest.PageType == "campaign" && sameDateInterval && sameIsFilter && sameRequestIDs && sameSMT)
                        {
                            dataModel = analyticsTempData.PrevCampaignData;
                        }
                        else
                        {
                            dataModel = GetCampaignSummaries(graphRequest, true);
                            dataModel.DmaMentionMapList = dataModel.DmaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                            dataModel.CanadaMentionMapList = dataModel.CanadaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();

                            SetCampaignDates(dataModel, graphRequest);
                            analyticsTempData.PrevCampaignData = dataModel;
                        }
                    }
                    else
                    {
                        dataModel = GetCampaignSummaries(graphRequest, true);
                        dataModel.DmaMentionMapList = dataModel.DmaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                        dataModel.CanadaMentionMapList = dataModel.CanadaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();

                        SetCampaignDates(dataModel, graphRequest);
                        analyticsTempData.PrevCampaignData = dataModel;
                    }
                }
                else if (graphRequest.Tab == SecondaryTabID.Networks || graphRequest.Tab == SecondaryTabID.Shows)
                {
                    ConvertDatesFromInterval(graphRequest);
                    dataModel = GetNetworkShowSummaries(sessionInfo.ClientGUID, graphRequest);
                    dataModel.DmaMentionMapList = dataModel.DmaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                    dataModel.CanadaMentionMapList = dataModel.CanadaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                }
                else
                {
                    ConvertDatesFromInterval(graphRequest);
                    dataModel = GetAgentSummaries(sessionInfo.ClientGUID, graphRequest);
                    dataModel.DmaMentionMapList = dataModel.DmaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                    dataModel.CanadaMentionMapList = dataModel.CanadaMentionMapList.Where(w => graphRequest.RequestIDs.Contains(w.SearchRequestID)).ToList();
                }

                foreach (AnalyticsSummaryModel summary in dataModel.SummaryDataList)
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
                if (graphRequest.RequestIDs != null && graphRequest.RequestIDs.Count > 0)
                {
                    if (graphRequest.Tab == SecondaryTabID.Demographic)
                    {
                        tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, dataModel, new Dictionary<string, string>(), analyticsTempData.SubMediaTypes);
                    }
                    else
                    {
                        tsstChart = analyticsLogic.GetChart(graphRequest, tssTable, dataModel, (graphRequest.Tab == SecondaryTabID.Networks || graphRequest.Tab == SecondaryTabID.Shows) ? dctTopTen : analyticsTempData.Groups[tssTable.TabDisplay], analyticsTempData.SubMediaTypes);
                    }
                }
                else
                {
                    tsstChart.Add("series", "");
                    tsstChart.Add("chart", "");
                }

                analyticsTempData.PrevRequest = graphRequest;

                dynamic jsonResult = new ExpandoObject();
                jsonResult.isSuccess = true;
                jsonResult.TSSTSeries = tsstChart["series"];
                jsonResult.chartJSON = tsstChart["chart"];
                jsonResult.isLRAccess = sessionInfo.isv5LRAccess;
                jsonResult.isAdsAccess = sessionInfo.Isv5AdsAccess;

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetChart: {0} ms", sw.ElapsedMilliseconds));
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
        public ContentResult GetOverlay(AnalyticsRequest graphRequest, int overlayType)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug(string.Format("GetOverlayData"));
                string overlayData = string.Empty;

                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();
                AnalyticsLogic analyticsLogic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel overlayDataModel = new AnalyticsDataModel();

                if (graphRequest.RequestIDs != null && graphRequest.RequestIDs.Count > 0)
                {
                    if (graphRequest.PageType == "campaign")
                    {
                        graphRequest.Campaigns = analyticsTempData.ClientAnalyticsCampaigns;
                        bool sameSMT = analyticsTempData.PrevRequest.SubMediaType == graphRequest.SubMediaType;
                        bool sameDateInterval = analyticsTempData.PrevRequest.DateInterval == graphRequest.DateInterval;
                        bool sameIsFilter = analyticsTempData.PrevRequest.IsFilter == graphRequest.IsFilter;
                        if (analyticsTempData.PrevRequest.RequestIDs == null)
                        {
                            analyticsTempData.PrevRequest.RequestIDs = new List<long>();
                        }
                        bool sameRequestIDs = analyticsTempData.PrevRequest.RequestIDs.SequenceEqual(graphRequest.RequestIDs);

                        if (analyticsTempData.PrevRequest.PageType == "campaign" && sameDateInterval && sameIsFilter && sameRequestIDs && sameSMT)
                        {
                            overlayDataModel = analyticsTempData.PrevCampaignData;
                        }
                        else
                        {
                            overlayDataModel = GetCampaignSummaries(graphRequest, true);
                            SetCampaignDates(overlayDataModel, graphRequest);
                            analyticsTempData.PrevCampaignData = overlayDataModel;
                        }
                    }
                    else
                    {
                        overlayDataModel = GetAgentSummaries(sessionInfo.ClientGUID, graphRequest);
                    }

                    foreach (AnalyticsSummaryModel summary in overlayDataModel.SummaryDataList)
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

                    List<AnalyticsGrouping> groupings = new List<AnalyticsGrouping>();

                    // Only make groupings for agents requested
                    foreach (var id in graphRequest.RequestIDs)
                    {
                        var groupSummaries = overlayDataModel.SummaryDataList.Where(w => graphRequest.PageType.Equals("amplification") ? w.SearchRequestID.Equals(id) : w.CampaignID.Equals(id)).ToList();
                        groupings.Add(new AnalyticsGrouping() {
                            ID = id.ToString(),
                            Name = graphRequest.PageType.Equals("amplification") ? groupSummaries.First().Query_Name : groupSummaries.First().CampaignName,
                            Summaries = groupSummaries
                        });
                    }

                    if (overlayType == 1)
                    {
                        GoogleLogic googleLogic = (GoogleLogic)LogicFactory.GetLogic(LogicType.Google);
                        List<GoogleSummaryModel> googleData = new List<GoogleSummaryModel>();
                        DateTime fromDate = Convert.ToDateTime(graphRequest.DateFrom);
                        DateTime toDate = Convert.ToDateTime(graphRequest.DateTo);
                        switch (graphRequest.DateInterval)
                        {
                            case "day":
                                googleData = googleLogic.GetGoogleDataByDay(sessionInfo.ClientGUID, fromDate, toDate);
                                break;
                            case "hour":
                                googleData = googleLogic.GetGoogleDataByHour(sessionInfo.ClientGUID, fromDate, toDate);
                                break;
                            case "month":
                                googleData = googleLogic.GetGoogleDataByMonth(sessionInfo.ClientGUID, fromDate, toDate);
                                break;
                        }

                        overlayData = analyticsLogic.CreateGoogleOverlay(googleData, graphRequest);
                    }
                    else
                    {
                        overlayData = analyticsLogic.CreateOverlay(groupings, graphRequest, overlayType);
                    }
                }

                analyticsTempData.PrevRequest = graphRequest;

                dynamic jsonResult = new ExpandoObject();
                jsonResult.isSuccess = true;
                jsonResult.overlayData = overlayData;
                jsonResult.isLRAccess = sessionInfo.isv5LRAccess;
                jsonResult.isAdsAccess = sessionInfo.Isv5AdsAccess;

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetOverlayData: {0} ms", sw.ElapsedMilliseconds));
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
        public JsonResult OpenIFrame()
        {
            return Json(new { isSuccess = true });
        }

        [HttpPost]
        public ContentResult GetActiveElements()
        {
            try
            {
                analyticsTempData = GetTempData();
                List<AnalyticsActiveElement> activeElements = analyticsTempData.ActiveElements;

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
            finally
            {
                TempData.Keep("AnalyticsTempData");
            }
        }

        #endregion

        #region Utility

        private void SetCampaignDates(AnalyticsDataModel data, AnalyticsRequest graphRequest)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("SetCampaignDates"));
                foreach (var summary in data.SummaryDataList)
                {
                    var campaign = graphRequest.Campaigns.First(c => c.CampaignID.Equals(summary.CampaignID));
                    var campaignOffset = summary.SummaryDateTime.Subtract(campaign.StartDate);
                    var months = ((summary.SummaryDateTime.Year - campaign.StartDate.Year) * 12) + summary.SummaryDateTime.Month - campaign.StartDate.Month;
                    switch(graphRequest.DateInterval)
                    {
                        case "hour":
                            summary.CampaignOffset = Convert.ToInt64(campaignOffset.TotalHours);
                            break;
                        case "day":
                            summary.CampaignOffset = Convert.ToInt64(campaignOffset.TotalDays);
                            break;
                        case "month":
                            summary.CampaignOffset = months;
                            break;
                    }
                }
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
        }

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

        private void ConvertDatesFromInterval(AnalyticsRequest graphRequest)
        {
            try
            {
                graphRequest.DateFrom = ConvertStartDateFromInterval(Convert.ToDateTime(graphRequest.DateFrom), graphRequest.DateInterval).ToString();
                graphRequest.DateTo = ConvertEndDateFromInterval(Convert.ToDateTime(graphRequest.DateTo), graphRequest.DateInterval).ToString();
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

        /// <summary>
        /// Gets all active agent summaries with summary data between dates
        /// </summary>
        /// <param name="clientGUID"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="interval"></param>
        /// <param name="subMediaType"></param>
        /// <returns></returns>
        private AnalyticsDataModel GetAllAgentSummaries(Guid clientGUID, DateTime dateFrom, DateTime dateTo, string interval, string subMediaType, SecondaryTabID tab = SecondaryTabID.OverTime)
        {
            try
            {
                sessionInfo = ActiveUserMgr.GetActiveUser();

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

                if (tab == SecondaryTabID.Networks || tab == SecondaryTabID.Shows)
                {
                    dataModel = logic.GetNetworkShowSummaryData(clientGUID, requestXml, subMediaType, sessionInfo.gmt, sessionInfo.dst, lstTopTen, tab, interval);
                }
                else
                {
                    if (interval == "hour")
                    {
                        dataModel = logic.GetHourSummaryData(clientGUID, requestXml, subMediaType, sessionInfo.gmt, sessionInfo.dst);
                        // Convert summary dates to client's local time
                        dataModel.SummaryDataList = CommonFunctions.GetGMTandDSTTime(dataModel.SummaryDataList, CommonFunctions.ResultType.Analytics);
                    }
                    else if (interval == "month")
                    {
                        dataModel = logic.GetMonthSummaryData(clientGUID, requestXml, subMediaType, sessionInfo.gmt, sessionInfo.dst);

                        // Line charts expect summary dates to be the 1st of their month
                        dataModel.SummaryDataList.ForEach(e =>
                        {
                            e.SummaryDateTime = new DateTime(e.SummaryDateTime.Year, e.SummaryDateTime.Month, 1);
                        });
                    }
                    else
                    {
                        dataModel = logic.GetDaySummaryData(clientGUID, requestXml, subMediaType, sessionInfo.gmt, sessionInfo.dst);
                    }
                }

                return dataModel;
            }
            catch (Exception exc)
            {
                CommonFunctions.WriteException(exc);
            }
            return new AnalyticsDataModel();
        }

        private AnalyticsDataModel GetAgentSummaries(Guid clientGUID, AnalyticsRequest request)
        {
            try
            {
                sessionInfo = ActiveUserMgr.GetActiveUser();

                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel dataModel;
                string requestXml = null;

                if (request.RequestIDs != null && request.RequestIDs.Count > 0)
                {
                    XDocument xDoc = new XDocument(new XElement(
                        "list",
                        from i in request.RequestIDs
                        select new XElement(
                            "item",
                            new XAttribute("id", i),
                            new XAttribute("fromDate", request.DateFrom),
                            new XAttribute("toDate", request.DateTo),
                            new XAttribute("fromDateGMT", CommonFunctions.GetGMTandDSTTime(Convert.ToDateTime(request.DateFrom)).ToString()),
                            new XAttribute("toDateGMT", CommonFunctions.GetGMTandDSTTime(Convert.ToDateTime(request.DateTo)).ToString())
                        )
                    ));
                    requestXml = xDoc.ToString();
                }

                switch(request.DateInterval)
                {
                    case "hour":
                        dataModel = logic.GetHourSummaryData(clientGUID, requestXml, request.SubMediaType, sessionInfo.gmt, sessionInfo.dst);
                        // Convert summary dates to client's local time
                        dataModel.SummaryDataList = CommonFunctions.GetGMTandDSTTime(dataModel.SummaryDataList, CommonFunctions.ResultType.Analytics);
                        break;
                    case "day":
                        dataModel = logic.GetDaySummaryData(clientGUID, requestXml, request.SubMediaType, sessionInfo.gmt, sessionInfo.dst);
                        break;
                    case "month":
                        dataModel = logic.GetMonthSummaryData(clientGUID, requestXml, request.SubMediaType, sessionInfo.gmt, sessionInfo.dst);

                        // Line charts expect summary dates to be the 1st of their month
                        dataModel.SummaryDataList.ForEach(e =>
                        {
                            e.SummaryDateTime = new DateTime(e.SummaryDateTime.Year, e.SummaryDateTime.Month, 1);
                        });
                        break;
                    default:
                        dataModel = logic.GetDaySummaryData(clientGUID, requestXml, request.SubMediaType, sessionInfo.gmt, sessionInfo.dst);
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

        private AnalyticsDataModel GetNetworkShowSummaries(Guid clientGUID, AnalyticsRequest request)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel dataModel;
                string requestXml = null;

                if (request.RequestIDs != null && request.RequestIDs.Count > 0)
                {
                    XDocument xDoc = new XDocument(new XElement(
                        "list",
                        from i in request.RequestIDs
                        select new XElement(
                            "item",
                            new XAttribute("id", i),
                            new XAttribute("fromDate", request.DateFrom),
                            new XAttribute("toDate", request.DateTo),
                            new XAttribute("fromDateGMT", CommonFunctions.GetGMTandDSTTime(Convert.ToDateTime(request.DateFrom)).ToString()),
                            new XAttribute("toDateGMT", CommonFunctions.GetGMTandDSTTime(Convert.ToDateTime(request.DateTo)).ToString())
                        )
                    ));
                    requestXml = xDoc.ToString();
                }

                lstTopTen = logic.GetTopTenData(clientGUID, requestXml, request.Tab).Where(x => x != null && x.Trim() != "").ToList();
                dctTopTen = new Dictionary<string, string>();
                var count = 0;
                foreach (var item in lstTopTen)
                {
                    dctTopTen.Add("TopTen" + count++, item);
                }

                dataModel = logic.GetNetworkShowSummaryData(clientGUID, requestXml, request.SubMediaType, sessionInfo.gmt, sessionInfo.dst, lstTopTen, request.Tab, request.DateInterval);

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

        private AnalyticsDataModel GetCampaignSummaries(AnalyticsRequest request, bool useSelectedIDs)
        {
            try
            {
                sessionInfo = ActiveUserMgr.GetActiveUser();

                analyticsTempData = GetTempData();
                // List should contain IDs of all campaigns for this client, unless filtering to just requested campaigns
                List<Int64> listCampaignIDs = useSelectedIDs ? request.RequestIDs : analyticsTempData.Groups["campaign"].Select(s => Convert.ToInt64(s.Key)).ToList();

                AnalyticsLogic logic = (AnalyticsLogic)LogicFactory.GetLogic(LogicType.Analytics);
                AnalyticsDataModel dataModel;
                string requestXml = null;

                if (listCampaignIDs != null && listCampaignIDs.Count > 0)
                {
                    XDocument xDoc = new XDocument(new XElement(
                        "list",
                        from i in listCampaignIDs
                        select new XElement(
                            "item",
                            new XAttribute("id", i)
                        )
                    ));
                    requestXml = xDoc.ToString();
                }

                switch (request.DateInterval)
                {
                    case "hour":
                        dataModel = logic.GetCampaignHourSummaryData(requestXml, request.SubMediaType, sessionInfo.gmt, sessionInfo.dst, false);
                        // Convert summary dates to client's local time
                        dataModel.SummaryDataList = CommonFunctions.GetGMTandDSTTime(dataModel.SummaryDataList, CommonFunctions.ResultType.Analytics);
                        break;
                    case "day":
                        dataModel = logic.GetCampaignDaySummaryData(requestXml, request.SubMediaType, sessionInfo.gmt, sessionInfo.dst, false);
                        break;
                    case "month":
                        // Currently no method to get campaign month summaries
                        dataModel = new AnalyticsDataModel();
                        break;
                    default:
                        dataModel = logic.GetCampaignDaySummaryData(requestXml, request.SubMediaType, sessionInfo.gmt, sessionInfo.dst, false);
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
                string agent0 = dictParameters["agent0"].ToString();
                string agent1 = dictParameters["agent1"].ToString();
                string source0 = dictParameters["source0"].ToString();
                string source1 = dictParameters["source1"].ToString();

                string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
                //string tempPDFPath = string.Format("C:\\Logs\\Download\\Analytics\\PDF\\{0}_{1}.pdf", sessionInfo.CustomerGUID, timeStamp);
                //string tempHTMLPath = string.Format("C:\\Logs\\Download\\Analytics\\HTML\\{0}_{1}.html", sessionInfo.CustomerGUID, timeStamp);
                string tempPDFPath = string.Format("{0}Download\\Analytics\\PDF\\{1}_{2}.pdf", ConfigurationManager.AppSettings["TempHTML-PDFPath"], sessionInfo.CustomerGUID, timeStamp);
                bool isFileGenerated = false;
                string html = GetHTMLWithCSS(HTML, agent0, agent1, source0, source1);

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

        private string GetHTMLWithCSS(string HTML, string agent0, string agent1, string source0, string source1)
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
                cssData.Append("body{background:none;}\n");

                HTML = string.Format("<html><head><style type=\"text/css\">{0}</style></head><body><img src=\"../../" + ConfigurationManager.AppSettings["IQMediaEmailLogo"] + "\" alt='IQMedia Logo'/>{1}</body></html>", cssData, HTML);

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

        #region TableCreation

        private List<HtmlTableRow> BuildMainTableRows(AnalyticsSecondaryTable table, List<AnalyticsSummaryModel> summaries, List<Int64> selectedIDs, bool hideLRAds = false)
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

                PropertyInfo groupByPI = typeof(AnalyticsSummaryModel).GetProperty(table.GroupBy);
                PropertyInfo groupByDisplayPI = typeof(AnalyticsSummaryModel).GetProperty(table.GroupByDisplay);

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

                    // Check the CB if the ID of this group is in the list of selected IDs
                    cbControl.Checked = selectedIDs.Any(sa => Convert.ToInt64(groupByPI.GetValue(group.First(), null)) == sa);

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
                    if (sessionInfo.isv5LRAccess && sessionInfo.Isv5AdsAccess && !hideLRAds)
                    {
                        columnHeader = table.ColumnHeadersAdsLR;
                    }
                    else if (sessionInfo.Isv5AdsAccess && !hideLRAds)
                    {
                        columnHeader = table.ColumnHeadersAds;
                    }
                    else if (sessionInfo.isv5LRAccess && !hideLRAds)
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
                                if (sessionInfo.isv5LRAccess && !hideLRAds)
                                {
                                    tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.SeenEarned + s.SeenPaid));
                                    tc.ID = string.Format("SEEN_{0}", groupByPI.GetValue(group.First(), null));
                                }
                                break;
                            case "heard":
                                if (sessionInfo.isv5LRAccess && !hideLRAds)
                                {
                                    tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.HeardEarned + s.HeardPaid));
                                    tc.ID = string.Format("HEARD_{0}", groupByPI.GetValue(group.First(), null));
                                }
                                break;
                            case "read":
                                if (sessionInfo.isv5LRAccess && !hideLRAds)
                                {
                                    Int64 sumRead = group.Sum(s => s.Number_Of_Hits - (s.SeenEarned + s.SeenPaid + s.HeardEarned + s.HeardPaid));
                                    tc.InnerText = string.Format("{0:N0}", sumRead < 0 ? 0 : sumRead);
                                    tc.ID = string.Format("READ_{0}", groupByPI.GetValue(group.First(), null));
                                }
                                break;
                            case "paid":
                                if (sessionInfo.Isv5AdsAccess && !hideLRAds)
                                {
                                    tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.HeardPaid + s.SeenPaid));
                                    tc.ID = string.Format("PAID_{0}", groupByPI.GetValue(group.First(), null));
                                }
                                break;
                            case "earned":
                                if (sessionInfo.Isv5AdsAccess && !hideLRAds)
                                {
                                    tc.InnerText = string.Format("{0:N0}", group.Sum(s => s.HeardEarned + s.SeenEarned));
                                    tc.ID = string.Format("EARNED_{0}", groupByPI.GetValue(group.First(), null));
                                }
                                break;
                            case "on air time":
                                // If we don't have seen/heard values, calculate on air time off the total number of hits
                                long onAir = !hideLRAds ? group.Sum(s => ((s.HeardEarned + s.HeardPaid) * 8 + (s.SeenEarned + s.SeenPaid))) : group.Sum(s => s.Number_Of_Hits * 8);
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
                //Log4NetLogger.Debug("BuildDemographicTableRows");
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

                    if (ar.AgeRange != "Total")
                    {
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
                    }

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
                Dictionary<string, string> allGroups = new Dictionary<string, string>();

                allGroups = analyticsTempData.Groups[table.TabDisplay];

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
                                    //tc.ID = string.Format("OCCURRENCES_{0}", group.ID);
                                    tc.Attributes.Add("OnlineCount", group.Summaries.Where(w => analyticsTempData.OnlineSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits).ToString());
                                    tc.Attributes.Add("PrintCount", group.Summaries.Where(w => analyticsTempData.PrintSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits).ToString());
                                    break;
                                case "seen":
                                    if (sessionInfo.isv5LRAccess)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.SeenEarned + s.SeenPaid));
                                        //tc.ID = string.Format("SEEN_{0}", group.ID);
                                    }
                                    break;
                                case "heard":
                                    if (sessionInfo.isv5LRAccess)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.HeardEarned + s.HeardPaid));
                                        //tc.ID = string.Format("HEARD_{0}", group.ID);
                                    }
                                    break;
                                case "read":
                                    if (sessionInfo.isv5LRAccess)
                                    {
                                        Int64 sumRead = group.Summaries.Sum(s => s.Number_Of_Hits - (s.SeenEarned + s.SeenPaid + s.HeardEarned + s.HeardPaid));
                                        tc.InnerText = string.Format("{0:N0}", sumRead < 0 ? 0 : sumRead);
                                        //tc.ID = string.Format("READ_{0}", group.ID);
                                    }
                                    break;
                                case "paid":
                                    if (sessionInfo.Isv5AdsAccess)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.HeardPaid + s.SeenPaid));
                                        //tc.ID = string.Format("PAID_{0}", group.ID);
                                    }
                                    break;
                                case "earned":
                                    if (sessionInfo.Isv5AdsAccess)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.HeardEarned + s.SeenEarned));
                                        //tc.ID = string.Format("EARNED_{0}", group.ID);
                                    }
                                    break;
                                case "on air time":
                                    long onAir = group.Summaries.Sum(s => ((s.HeardEarned + s.HeardPaid) * 8 + (s.SeenEarned + s.SeenPaid)));
                                    var sumOnAir = group.Summaries.Where(w => analyticsTempData.OnAirSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits);
                                    tc.InnerText = string.Format("{0:00}:{1:00}:{2:00}", onAir / 3600, (onAir / 60) % 60, onAir % 60);
                                    //tc.ID = string.Format("ONAIRTIME_{0}", group.ID);
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

        private List<HtmlTableRow> BuildNetworkShowTabSpecificTableRows(AnalyticsSecondaryTable table, List<AnalyticsSummaryModel> selectedSummaries, bool hideLRAds = false)
        {
            try
            {
                sessionInfo = ActiveUserMgr.GetActiveUser();
                analyticsTempData = GetTempData();
                //Log4NetLogger.Debug(string.Format("BuildTabSpecificTableRows"));
                Dictionary<string, string> allGroups = new Dictionary<string, string>();

                allGroups = dctTopTen;

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
                    var groupSummaries = selectedSummaries.Where(w => groupByPI.GetValue(w, null).ToString() == grp.Value).ToList();
                    groupings.Add(new AnalyticsGrouping()
                    {
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

                        HtmlTableRow tr = new HtmlTableRow()
                        {
                            ID = string.Format("{0}TR_{1}", table.GroupByHeader, group.ID)
                        };
                        tr.Attributes.Add("class", "secondaryDetailRow");

                        HtmlTableCell tc = new HtmlTableCell();
                        tc.Attributes.Add("class", "secondaryDetailCBCol");

                        HtmlInputCheckBox cbControl = new HtmlInputCheckBox()
                        {
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
                        tr.Cells.Add(new HtmlTableCell()
                        {
                            InnerText = string.Format("{0}", group.Name),
                            ID = string.Format("{0}TD_{1}", table.GroupByHeader, group.ID)
                        });

                        List<string> columnHeader = new List<string>();
                        if (sessionInfo.isv5LRAccess && sessionInfo.Isv5AdsAccess && !hideLRAds)
                        {
                            columnHeader = table.ColumnHeadersAdsLR;
                        }
                        else if (sessionInfo.Isv5AdsAccess && !hideLRAds)
                        {
                            columnHeader = table.ColumnHeadersAds;
                        }
                        else if (sessionInfo.isv5LRAccess && !hideLRAds)
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
                                    //tc.ID = string.Format("OCCURRENCES_{0}", group.ID);
                                    tc.Attributes.Add("OnlineCount", group.Summaries.Where(w => analyticsTempData.OnlineSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits).ToString());
                                    tc.Attributes.Add("PrintCount", group.Summaries.Where(w => analyticsTempData.PrintSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits).ToString());
                                    break;
                                case "seen":
                                    if (sessionInfo.isv5LRAccess && !hideLRAds)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.SeenEarned + s.SeenPaid));
                                        //tc.ID = string.Format("SEEN_{0}", group.ID);
                                    }
                                    break;
                                case "heard":
                                    if (sessionInfo.isv5LRAccess && !hideLRAds)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.HeardEarned + s.HeardPaid));
                                        //tc.ID = string.Format("HEARD_{0}", group.ID);
                                    }
                                    break;
                                case "read":
                                    if (sessionInfo.isv5LRAccess && !hideLRAds)
                                    {
                                        Int64 sumRead = group.Summaries.Sum(s => s.Number_Of_Hits - (s.SeenEarned + s.SeenPaid + s.HeardEarned + s.HeardPaid));
                                        tc.InnerText = string.Format("{0:N0}", sumRead < 0 ? 0 : sumRead);
                                        //tc.ID = string.Format("READ_{0}", group.ID);
                                    }
                                    break;
                                case "paid":
                                    if (sessionInfo.Isv5AdsAccess && !hideLRAds)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.HeardPaid + s.SeenPaid));
                                        //tc.ID = string.Format("PAID_{0}", group.ID);
                                    }
                                    break;
                                case "earned":
                                    if (sessionInfo.Isv5AdsAccess && !hideLRAds)
                                    {
                                        tc.InnerText = string.Format("{0:N0}", group.Summaries.Sum(s => s.HeardEarned + s.SeenEarned));
                                        //tc.ID = string.Format("EARNED_{0}", group.ID);
                                    }
                                    break;
                                case "on air time":
                                    // If we don't have seen/heard values, calculate on air time off the total number of hits
                                    long onAir = !hideLRAds ? group.Summaries.Sum(s => ((s.HeardEarned + s.HeardPaid) * 8 + (s.SeenEarned + s.SeenPaid))) : group.Summaries.Sum(s => s.Number_Of_Hits * 8);
                                    var sumOnAir = group.Summaries.Where(w => analyticsTempData.OnAirSubMediaTypes.Contains(w.SubMediaType)).Sum(s => s.Number_Of_Hits);
                                    tc.InnerText = string.Format("{0:00}:{1:00}:{2:00}", onAir / 3600, (onAir / 60) % 60, onAir % 60);
                                    //tc.ID = string.Format("ONAIRTIME_{0}", group.ID);
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

                                    HtmlTable sentimentTbl = new HtmlTable()
                                    {
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

                                    HtmlGenericControl negSpan = new HtmlGenericControl("span")
                                    {
                                        InnerHtml = string.Format("{0:N0}&nbsp;", negSentiment)
                                    };
                                    negSpan.Attributes.Add("class", "sentimentNegSpan");
                                    negSpan.Attributes.Add("style", string.Format("width:{0}px;", negSentWidth));

                                    sentimentNegTD.Controls.Add(negSpan);

                                    HtmlTableCell sentimentPosTD = new HtmlTableCell();
                                    sentimentPosTD.Attributes.Add("class", "sentimentNegPosTD");

                                    HtmlGenericControl posSpan = new HtmlGenericControl("span")
                                    {
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

        private HtmlTableRow CreateSecondaryTableHeader(AnalyticsSecondaryTable secondaryTable, bool hideLRAds = false)
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
                if (sessionInfo.isv5LRAccess && sessionInfo.Isv5AdsAccess && !hideLRAds)
                {
                    columnHeader = secondaryTable.ColumnHeadersAdsLR;
                }
                else if (sessionInfo.Isv5AdsAccess && !hideLRAds)
                {
                    columnHeader = secondaryTable.ColumnHeadersAds;
                }
                else if (sessionInfo.isv5LRAccess && !hideLRAds)
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

        #endregion
    }
}
