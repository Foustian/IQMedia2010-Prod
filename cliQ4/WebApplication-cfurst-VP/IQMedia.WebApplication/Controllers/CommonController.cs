﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Model;
using System.Configuration;
using IQMedia.Web.Logic.Base;
using System.Dynamic;
using IQMedia.WebApplication.Models;
using IQMedia.Shared.Utility;
using System.Globalization;
using IQMedia.WebApplication.Config;
using System.Net;
using System.IO;
using IQCommon.Model;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class CommonController : Controller
    {
        //
        // GET: /Common/

        #region Public Member

        ActiveUser sessionInformation = null;

        #endregion

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult LoadPlayerByGuidnSearchTerm(Guid p_ItemGuid, string p_SearchTerm, string p_Title120)
        {
            try
            {
                int? offset = 0;
                bool hasCaption = false;
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                string captionString = string.Empty;
                List<int> SearchTermList = new List<int>(); // needed for TAds 
                string highlightString = UtilityLogic.GetRawMediaCaption(p_SearchTerm, new Guid(Convert.ToString(p_ItemGuid)), out offset, out captionString, IQMedia.WebApplication.Utility.CommonFunctions.GeneratePMGUrl(IQMedia.WebApplication.Utility.CommonFunctions.PMGUrlType.TV.ToString(), null, null), out SearchTermList, p_Title120);
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

                string rawMediaObject = UtilityLogic.RenderRawMediaPlayer(string.Empty,
                                                    Convert.ToString(p_ItemGuid),
                                                    "true",
                                                    "false",
                    Convert.ToString(sessionInformation.ClientGUID),
                    // sessionInformation.ClientGUID,
                                                    "false",
                     Convert.ToString(sessionInformation.CustomerGUID),
                    //"B92B5C68-FA30-478F-9A20-B6F754C1F89C",
                                                    ConfigurationManager.AppSettings["ServicesBaseURL"],
                                                    offset,
                                                    sessionInformation.IsClientPlayerLogoActive,
                                                    sessionInformation.ClientPlayerLogoImage, Request.Browser.Type);


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
                    forceCategorySelection = forceCategorySelection,
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
        }

        public JsonResult LoadBasicPlayerByGuidnSearchTerm(Guid p_ItemGuid, string p_SearchTerm, string p_Title120, bool p_IsOptiQ = false, string p_KeyValues = null, bool p_AutoPlayback = true, bool p_ARSZ=false)
        {
            try
            {
                int? offset = 0;
                bool hasCaption = false;
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                string captionString = string.Empty;
                List<int> lstSearchTermHits = new List<int>();
                string highlightString = UtilityLogic.GetRawMediaCaption(p_SearchTerm, new Guid(Convert.ToString(p_ItemGuid)), out offset, out captionString, IQMedia.WebApplication.Utility.CommonFunctions.GeneratePMGUrl(IQMedia.WebApplication.Utility.CommonFunctions.PMGUrlType.TV.ToString(), null, null), out lstSearchTermHits, p_Title120, p_IsOptiQ);
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

                string rawMediaObject = UtilityLogic.RenderBasicRawMediaPlayer(string.Empty,
                                                    Convert.ToString(p_ItemGuid),
                                                    "true",
                                                    "false",
                    Convert.ToString(sessionInformation.ClientGUID),
                    // sessionInformation.ClientGUID,
                                                    "false",
                     Convert.ToString(sessionInformation.CustomerGUID),
                    //"B92B5C68-FA30-478F-9A20-B6F754C1F89C",
                                                    ConfigurationManager.AppSettings["ServicesBaseURL"],
                                                    offset,
                                                    sessionInformation.IsClientPlayerLogoActive,
                                                    sessionInformation.ClientPlayerLogoImage, Request.Browser.Type, p_KeyValues, p_AutoPlayback, p_AutoResize:p_ARSZ);


                if (!string.IsNullOrWhiteSpace(captionString) || !string.IsNullOrWhiteSpace(highlightString))
                {
                    hasCaption = true;
                }

                return Json(new
                {
                    SearchTermHits = lstSearchTermHits,
                    rawMediaObjectHTML = rawMediaObject,
                    HighlightHTML = highlightString,
                    CaptionHTML = captionString,
                    hasCaptionString = hasCaption,
                    offset = offset,
                    forceCategorySelection = forceCategorySelection,
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
        }

        [HttpPost]
        public JsonResult GetDMAsByZipCode(List<string> zipCodes)
        {
            try
            {
                SSPLogic SSPLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                Dictionary<string, object> dictResults = SSPLogic.GetDMAsByZipCode(zipCodes);

                List<string> dmas = new List<string>();
                if (dictResults["DMAs"] != null)
                {
                    dmas = ((List<IQ_Zip_Code>)dictResults["DMAs"]).Select(zc => zc.ZipCode + ":" + zc.IQ_DMA_Name).ToList();
                }

                string invalidZipCodeMsg = string.Empty;
                if (dictResults["InvalidZipCodes"] != null)
                {
                    List<int> invalidZipCodes = (List<int>)dictResults["InvalidZipCodes"];
                    invalidZipCodeMsg = string.Join(", ", invalidZipCodes);
                }

                return Json(new
                {
                    dmas = dmas,
                    invalidZipCodeMsg = invalidZipCodeMsg,
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
        }

        public static Dictionary<string, object> GetDashboardOverviewResults(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, Dictionary<long, string> p_SearchRequests, IQAgent_DashBoardPrevSummaryModel p_PrevIQAgentSummary, List<ThirdPartyDataTypeModel> p_ThirdPartyDataTypes, ActiveUser p_User)
        {
            try
            {
                List<string> availMediaTypes = p_User.MediaTypes.Where(w => w.TypeLevel == 1 && !w.IsArchiveOnly).Select(s => s.MediaType).ToList();
                List<IQ_MediaTypeModel> lstMediaTypes = p_User.MediaTypes.Where(w => availMediaTypes.Contains(w.MediaType)).ToList();

                SummaryReportMulti linechart = LineChart(listOfSummaryReportData, p_FromDate, p_ToDate, p_SearchType, p_SearchRequests, p_PrevIQAgentSummary != null ? p_PrevIQAgentSummary.ListOfIQAgentPrevSummary : null, p_ThirdPartyDataTypes, p_User);
                string piechart = PieChart(listOfSummaryReportData, p_FromDate, p_ToDate, lstMediaTypes);

                DashboardOverviewResults dashboardOverviewResults = new DashboardOverviewResults();
                
                dashboardOverviewResults.SumAudienceRecord = linechart.AudienceRecordsSum;
                dashboardOverviewResults.PrevSumAudienceRecord = linechart.AudiencePrevRecordsSum;
                dashboardOverviewResults.SumIQMediaValueRecord = linechart.IQMediaValueRecordsSum;
                dashboardOverviewResults.PrevSumIQMediaValueRecord = linechart.IQMediaValuePrevRecordsSum;
                
                dashboardOverviewResults.TotNumOfHits = linechart.TotalNumOfHits;
                dashboardOverviewResults.IsprevSummaryEnoughData = p_PrevIQAgentSummary != null ? p_PrevIQAgentSummary.IsEnoughData : false;

                dashboardOverviewResults.ReportMediumList = linechart.SummaryReportMedium.OrderBy(mt=>mt.MediaTypeModel.SortOrder).Select(s => new ReportMedium() { MediaType = s.MediaTypeModel.MediaType, DisplayName= s.MediaTypeModel.DisplayName, PrevRecordsSum = s.PrevRecordsSum, Records = s.Records, RecordsSum = s.RecordsSum }).ToList();

                dynamic jsonResult = new ExpandoObject();

                jsonResult.jsonMediaRecord = linechart.MediaRecords;
                jsonResult.jsonSubMediaRecord = linechart.SubMediaRecords;
                jsonResult.jsonPieChartSubMedia = piechart;               
                jsonResult.jsonAudienceRecord = linechart.AudienceRecords;
                jsonResult.jsonIQMediaValueRecords = linechart.IQMediaValueRecords;
                jsonResult.CategoryDescription = "Overview";
                jsonResult.fromDate = p_FromDate.ToString();
                jsonResult.toDate = p_ToDate.ToString();
                jsonResult.ReportMediumList = dashboardOverviewResults.ReportMediumList;
                jsonResult.IsprevSummaryEnoughData = p_PrevIQAgentSummary != null ? p_PrevIQAgentSummary.IsEnoughData : false;

                NumberFormatInfo numInfo = new NumberFormatInfo();
                numInfo.NumberGroupSeparator = String.Empty; // Format the number without comma separators
                jsonResult.totalHits = Decimal.Parse(linechart.TotalNumOfHits).ToString("N0", numInfo);

                Dictionary<string, object> dictResults = new Dictionary<string, object>();
                dictResults["OverviewResults"] = dashboardOverviewResults;
                dictResults["JsonResult"] = jsonResult;

                return dictResults;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Dictionary<string, object> GetDashboardMediumResults(IQ_MediaTypeModel p_MediaType, IQAgent_DashBoardModel iQAgent_DashBoardModel, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, List<string> p_SearchRequestIDs, bool useDMAMap, ActiveUser p_SessionInformation)
        {
            try
            {
                /*
                 Can have dynamic list, with type instead of fix variables.
                 */ 
                DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);                

                string noOfDocs = string.Empty;
                string noOfHits = string.Empty;
                string noOfMinofAiring = string.Empty;
                string noOfAd = string.Empty;
                string noOfView = string.Empty;

                string hitsTitle = string.Empty;
                string airingTitle = string.Empty;
                string adTitle = string.Empty;
                string viewTitle = string.Empty;

                Int64 noOfHitsCount = 0;
                Int64 totalAirSeconds = 0;
                Int64 noOfViewsCount = 0;
                Decimal noOfMinsOfAiringCount = 0;
                Decimal noOfAdCount = 0;

                string dmaMapChart = string.Empty;
                string canadaMapChart = string.Empty;

                Int64 negativeSentiment = 0;
                Int64 positiveSentiment = 0;
                string sentiMentChart = string.Empty;

                string sentimentTitle = string.Empty;

                foreach (DataChart dchart in p_MediaType.DashboardData.ChartTypes)
                {
                    switch (dchart.DataChartType)
                    {
                        case DataChartTypes.Hits:
                            hitsTitle = dchart.Title;
                            break;
                        case DataChartTypes.Airing:
                            airingTitle = dchart.Title;
                            break;
                        case DataChartTypes.Ad:
                            adTitle = dchart.Title;
                            break;
                        case DataChartTypes.Audience:
                            viewTitle = dchart.Title;
                            break;                        
                        case DataChartTypes.Sentiment:
                            sentimentTitle = dchart.Title;
                            break;
                        default:
                            break;
                    }                    
                }

                if (p_SearchType == 1)
                {
                    noOfDocs = dashboardLogic.GetHighChartForDocs(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, p_MediaType, p_SearchRequestIDs, out totalAirSeconds);

                    foreach (DataChart dchart in p_MediaType.DashboardData.ChartTypes)
                    {
                        switch (dchart.DataChartType)
                        {
                            case DataChartTypes.Hits:
                                noOfHits = dashboardLogic.GetHighChartForHits(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfHitsCount);
                                break;
                            case DataChartTypes.Airing:
                                noOfMinofAiring = dashboardLogic.GetHighChartForMinutesOfAiring(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfMinsOfAiringCount);
                                break;
                            case DataChartTypes.Ad:
                                if (CommonFunctions.CheckNielsenCompeteAccess(p_MediaType.UseMediaValue, p_MediaType.RequireNielsenAccess, p_SessionInformation.IsNielsenData, p_MediaType.RequireCompeteAccess, p_SessionInformation.IsCompeteData))
                                {
                                    noOfAd = dashboardLogic.GetHighChartForAd(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfAdCount);
                                }
                                break;
                            case DataChartTypes.Audience:
                                if (CommonFunctions.CheckNielsenCompeteAccess(p_MediaType.UseAudience, p_MediaType.RequireNielsenAccess, p_SessionInformation.IsNielsenData, p_MediaType.RequireCompeteAccess, p_SessionInformation.IsCompeteData))
                                {
                                    noOfView = dashboardLogic.GetHighChartForViews(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfViewsCount);
                                }
                                break;
                            case DataChartTypes.USADMAMap:
                                if (useDMAMap)
                                {
                                    dmaMapChart = dashboardLogic.GetFusionUsaDmaMap(iQAgent_DashBoardModel.DmaMentionMapList); 
                                }
                                break;
                            case DataChartTypes.CANADAProvinceMap:
                                if (useDMAMap)
                                {
                                    canadaMapChart = dashboardLogic.GetFusionCanadaProvinceMap(iQAgent_DashBoardModel.CanadaMentionMapList);
                                }
                                break;
                            case DataChartTypes.Sentiment:
                                sentiMentChart = dashboardLogic.GetHighChartForSentiment(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out positiveSentiment, out negativeSentiment);
                                break;
                        }
                    }
                    /*
                    if (p_Medium != CommonFunctions.DashBoardMediumType.MS.ToString())
                    {
                        if (p_Medium != CommonFunctions.DashBoardMediumType.TW.ToString() && p_Medium != CommonFunctions.DashBoardMediumType.Radio.ToString() && (p_Medium != CommonFunctions.DashBoardMediumType.PM.ToString() || sessionInformation.Isv4PQ))
                        {
                            noOfHits = dashboardLogic.GetHighChartForHits(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfHitsCount);
                        }
                        noOfMinofAiring = dashboardLogic.GetHighChartForMinutesOfAiring(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfMinsOfAiringCount);

                        if ((p_Medium == CommonFunctions.DashBoardMediumType.TV.ToString() && sessionInformation.IsNielsenData) || ((p_Medium == CommonFunctions.DashBoardMediumType.Blog.ToString() || p_Medium == CommonFunctions.DashBoardMediumType.NM.ToString()) && sessionInformation.IsCompeteData))
                        {
                            noOfAd = dashboardLogic.GetHighChartForAd(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfAdCount);
                        }
                        if (p_Medium != CommonFunctions.DashBoardMediumType.SocialMedia.ToString() && p_Medium != CommonFunctions.DashBoardMediumType.Forum.ToString() && p_Medium != CommonFunctions.DashBoardMediumType.Radio.ToString())
                        {
                            if ((p_Medium == CommonFunctions.DashBoardMediumType.TV.ToString() && sessionInformation.IsNielsenData) || ((p_Medium == CommonFunctions.DashBoardMediumType.Blog.ToString() || p_Medium == CommonFunctions.DashBoardMediumType.NM.ToString()) && sessionInformation.IsCompeteData) || p_Medium == CommonFunctions.DashBoardMediumType.TW.ToString() || (p_Medium == CommonFunctions.DashBoardMediumType.PM.ToString() && sessionInformation.Isv4BLPM))
                            {
                                noOfView = dashboardLogic.GetHighChartForViews(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, p_Medium, out noOfViewsCount);
                            }
                        }
                    }*/
                }
                else if (p_SearchType == 0)
                {
                    noOfDocs = dashboardLogic.GetHighChartForDocsHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, p_MediaType, p_SessionInformation.gmt, p_SessionInformation.dst, p_SearchRequestIDs, out totalAirSeconds);

                    foreach (DataChart dchart in p_MediaType.DashboardData.ChartTypes)
                    {
                        switch (dchart.DataChartType)
                        {
                            case DataChartTypes.Hits:
                                noOfHits = dashboardLogic.GetHighChartForHitsHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfHitsCount, p_SessionInformation.gmt, p_SessionInformation.dst);
                                break;
                            case DataChartTypes.Airing:
                                noOfMinofAiring = dashboardLogic.GetHighChartForMinutesOfAiringHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfMinsOfAiringCount, p_SessionInformation.gmt, p_SessionInformation.dst);
                                break;
                            case DataChartTypes.Ad:
                                if (CommonFunctions.CheckNielsenCompeteAccess(p_MediaType.UseMediaValue, p_MediaType.RequireNielsenAccess, p_SessionInformation.IsNielsenData, p_MediaType.RequireCompeteAccess, p_SessionInformation.IsCompeteData))
                                {
                                    noOfAd = dashboardLogic.GetHighChartForAdHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfAdCount, p_SessionInformation.gmt, p_SessionInformation.dst);
                                }
                                break;
                            case DataChartTypes.Audience:
                                if (CommonFunctions.CheckNielsenCompeteAccess(p_MediaType.UseAudience, p_MediaType.RequireNielsenAccess, p_SessionInformation.IsNielsenData, p_MediaType.RequireCompeteAccess, p_SessionInformation.IsCompeteData))
                                {
                                    noOfView = dashboardLogic.GetHighChartForViewsHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfViewsCount, p_SessionInformation.gmt, p_SessionInformation.dst);
                                }
                                break;
                            case DataChartTypes.USADMAMap:
                                if (useDMAMap)
                                {
                                    dmaMapChart = dashboardLogic.GetFusionUsaDmaMap(iQAgent_DashBoardModel.DmaMentionMapList);
                                }
                                break;
                            case DataChartTypes.CANADAProvinceMap:
                                if (useDMAMap)
                                {
                                    canadaMapChart = dashboardLogic.GetFusionCanadaProvinceMap(iQAgent_DashBoardModel.CanadaMentionMapList);
                                }
                                break;
                            case DataChartTypes.Sentiment:
                                sentiMentChart = dashboardLogic.GetHighChartForSentimentHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out positiveSentiment, out negativeSentiment, p_SessionInformation.gmt, p_SessionInformation.dst);
                                break;
                        }
                    }
                        
                    /*
                    if (p_Medium != CommonFunctions.DashBoardMediumType.MS.ToString())
                    {
                        if (p_Medium != CommonFunctions.DashBoardMediumType.TW.ToString() && p_Medium != CommonFunctions.DashBoardMediumType.Radio.ToString() && (p_Medium != CommonFunctions.DashBoardMediumType.PM.ToString() || sessionInformation.Isv4PQ))
                        {
                            noOfHits = dashboardLogic.GetHighChartForHitsHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfHitsCount, sessionInformation.gmt, sessionInformation.dst);
                        }
                        noOfMinofAiring = dashboardLogic.GetHighChartForMinutesOfAiringHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfMinsOfAiringCount, sessionInformation.gmt, sessionInformation.dst);

                        if ((p_Medium == CommonFunctions.DashBoardMediumType.TV.ToString() && sessionInformation.IsNielsenData) || ((p_Medium == CommonFunctions.DashBoardMediumType.Blog.ToString() || p_Medium == CommonFunctions.DashBoardMediumType.NM.ToString()) && sessionInformation.IsCompeteData))
                        {
                            noOfAd = dashboardLogic.GetHighChartForAdHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfAdCount, sessionInformation.gmt, sessionInformation.dst);
                        }
                        if (p_Medium != CommonFunctions.DashBoardMediumType.SocialMedia.ToString() && p_Medium != CommonFunctions.DashBoardMediumType.Forum.ToString() && p_Medium != CommonFunctions.DashBoardMediumType.Radio.ToString())
                        {
                            if ((p_Medium == CommonFunctions.DashBoardMediumType.TV.ToString() && sessionInformation.IsNielsenData) || ((p_Medium == CommonFunctions.DashBoardMediumType.Blog.ToString() || p_Medium == CommonFunctions.DashBoardMediumType.NM.ToString()) && sessionInformation.IsCompeteData) || p_Medium == CommonFunctions.DashBoardMediumType.TW.ToString() || (p_Medium == CommonFunctions.DashBoardMediumType.PM.ToString() && sessionInformation.Isv4BLPM))
                            {
                                noOfView = dashboardLogic.GetHighChartForViewsHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, p_Medium, out noOfViewsCount, sessionInformation.gmt, sessionInformation.dst);
                            }
                        }
                    }*/
                }
                else if (p_SearchType == 2)
                {

                }
                else if (p_SearchType == 3)
                {
                    noOfDocs = dashboardLogic.GetHighChartForDocsMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, p_MediaType, p_SearchRequestIDs, out totalAirSeconds);

                    foreach (DataChart dchart in p_MediaType.DashboardData.ChartTypes)
                    {
                        switch (dchart.DataChartType)
                        {
                            case DataChartTypes.Hits:
                                noOfHits = dashboardLogic.GetHighChartForHitsMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfHitsCount);
                                break;
                            case DataChartTypes.Airing:
                                noOfMinofAiring = dashboardLogic.GetHighChartForMinutesOfAiringMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfMinsOfAiringCount);
                                break;
                            case DataChartTypes.Ad:
                                if (CommonFunctions.CheckNielsenCompeteAccess(p_MediaType.UseMediaValue, p_MediaType.RequireNielsenAccess, p_SessionInformation.IsNielsenData, p_MediaType.RequireCompeteAccess, p_SessionInformation.IsCompeteData))
                                {
                                    noOfAd = dashboardLogic.GetHighChartForAdMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfAdCount);
                                }
                                break;
                            case DataChartTypes.Audience:
                                if (CommonFunctions.CheckNielsenCompeteAccess(p_MediaType.UseAudience, p_MediaType.RequireNielsenAccess, p_SessionInformation.IsNielsenData, p_MediaType.RequireCompeteAccess, p_SessionInformation.IsCompeteData))
                                {
                                    noOfView = dashboardLogic.GetHighChartForViewsMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfViewsCount);
                                }
                                break;
                            case DataChartTypes.USADMAMap:
                                if (useDMAMap)
                                {
                                    dmaMapChart = dashboardLogic.GetFusionUsaDmaMap(iQAgent_DashBoardModel.DmaMentionMapList);
                                }
                                break;
                            case DataChartTypes.CANADAProvinceMap:
                                if (useDMAMap)
                                {
                                    canadaMapChart = dashboardLogic.GetFusionCanadaProvinceMap(iQAgent_DashBoardModel.CanadaMentionMapList);
                                }
                                break;
                            case DataChartTypes.Sentiment:
                                sentiMentChart = dashboardLogic.GetHighChartForSentimentMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out positiveSentiment, out negativeSentiment);
                                break;
                        }
                    }
                    /*
                    if (p_Medium != CommonFunctions.DashBoardMediumType.MS.ToString())
                    {
                        if (p_Medium != CommonFunctions.DashBoardMediumType.TW.ToString() && p_Medium != CommonFunctions.DashBoardMediumType.Radio.ToString() && (p_Medium != CommonFunctions.DashBoardMediumType.PM.ToString() || sessionInformation.Isv4PQ))
                        {
                            noOfHits = dashboardLogic.GetHighChartForHitsMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfHitsCount);
                        }
                        noOfMinofAiring = dashboardLogic.GetHighChartForMinutesOfAiringMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfMinsOfAiringCount);

                        if ((p_Medium == CommonFunctions.DashBoardMediumType.TV.ToString() && sessionInformation.IsNielsenData) || ((p_Medium == CommonFunctions.DashBoardMediumType.Blog.ToString() || p_Medium == CommonFunctions.DashBoardMediumType.NM.ToString()) && sessionInformation.IsCompeteData))
                        {
                            noOfAd = dashboardLogic.GetHighChartForAdMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out noOfAdCount);
                        }
                        if (p_Medium != CommonFunctions.DashBoardMediumType.SocialMedia.ToString() && p_Medium != CommonFunctions.DashBoardMediumType.Forum.ToString() && p_Medium != CommonFunctions.DashBoardMediumType.Radio.ToString())
                        {
                            if ((p_Medium == CommonFunctions.DashBoardMediumType.TV.ToString() && sessionInformation.IsNielsenData) || ((p_Medium == CommonFunctions.DashBoardMediumType.Blog.ToString() || p_Medium == CommonFunctions.DashBoardMediumType.NM.ToString()) && sessionInformation.IsCompeteData) || p_Medium == CommonFunctions.DashBoardMediumType.TW.ToString() || (p_Medium == CommonFunctions.DashBoardMediumType.PM.ToString() && sessionInformation.Isv4BLPM))
                            {
                                noOfView = dashboardLogic.GetHighChartForViewsMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, p_Medium, out noOfViewsCount);
                            }
                        }
                    }
                     */ 
                }

                /*
                if (useDMAMap && (p_Medium == CommonFunctions.DashBoardMediumType.TV.ToString() || p_Medium == CommonFunctions.DashBoardMediumType.NM.ToString()))
                {
                    dmaMapChart = dashboardLogic.GetFusionUsaDmaMap(iQAgent_DashBoardModel.DmaMentionMapList);
                    canadaMapChart = dashboardLogic.GetFusionCanadaProvinceMap(iQAgent_DashBoardModel.CanadaMentionMapList);
                }
                 */ 

                /*
                if (p_Medium != CommonFunctions.DashBoardMediumType.Radio.ToString() && (p_Medium != CommonFunctions.DashBoardMediumType.PM.ToString() || sessionInformation.Isv4PQ) && p_Medium != CommonFunctions.DashBoardMediumType.MS.ToString())
                {
                    if (p_SearchType == 1)
                    {
                        sentiMentChart = dashboardLogic.GetHighChartForSentiment(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out positiveSentiment, out negativeSentiment);
                    }
                    else if (p_SearchType == 0)
                    {
                        sentiMentChart = dashboardLogic.GetHighChartForSentimentHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out positiveSentiment, out negativeSentiment, sessionInformation.gmt, sessionInformation.dst);
                    }
                    else if (p_SearchType == 3)
                    {
                        sentiMentChart = dashboardLogic.GetHighChartForSentimentMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, out positiveSentiment, out negativeSentiment);
                    }
                }
                */

                DashboardMediaResults dashboardMediaResults = new DashboardMediaResults();

                dashboardMediaResults.SumNegativeSentiment = negativeSentiment;
                dashboardMediaResults.SumPositiveSentiment = positiveSentiment;
                dashboardMediaResults.SumAirSeconds = totalAirSeconds;
                dashboardMediaResults.SumHits = noOfHitsCount;
                dashboardMediaResults.SumAudience = noOfViewsCount;
                dashboardMediaResults.SumIQMediaValue = noOfAdCount;

                dashboardMediaResults.SentimentTitle = sentimentTitle;
                dashboardMediaResults.AirTitle = airingTitle;
                dashboardMediaResults.HitsTitle = hitsTitle;
                dashboardMediaResults.AudienceTitle = viewTitle;
                dashboardMediaResults.MediaValueTitle = adTitle;

                if (iQAgent_DashBoardModel.PrevIQAgentSummary != null && iQAgent_DashBoardModel.PrevIQAgentSummary.ListOfIQAgentPrevSummary != null && iQAgent_DashBoardModel.PrevIQAgentSummary.ListOfIQAgentPrevSummary.Count > 0)
                {
                    dashboardMediaResults.IsprevSummaryEnoughData = iQAgent_DashBoardModel.PrevIQAgentSummary.IsEnoughData;
                    dashboardMediaResults.PrevSumNegativeSentiment = iQAgent_DashBoardModel.PrevIQAgentSummary.ListOfIQAgentPrevSummary[0].NegativeSentiment;
                    dashboardMediaResults.PrevSumPositiveSentiment = iQAgent_DashBoardModel.PrevIQAgentSummary.ListOfIQAgentPrevSummary[0].PositiveSentiment;
                    dashboardMediaResults.PrevSumAirSeconds = iQAgent_DashBoardModel.PrevIQAgentSummary.ListOfIQAgentPrevSummary[0].TotalAirSeconds;
                    dashboardMediaResults.PrevSumHits = iQAgent_DashBoardModel.PrevIQAgentSummary.ListOfIQAgentPrevSummary[0].NoOfHits;
                    dashboardMediaResults.PrevSumAudience = iQAgent_DashBoardModel.PrevIQAgentSummary.ListOfIQAgentPrevSummary[0].Audience;
                    dashboardMediaResults.PrevSumIQMediaValue = iQAgent_DashBoardModel.PrevIQAgentSummary.ListOfIQAgentPrevSummary[0].IQMediaValue;
                }
                else
                {
                    dashboardMediaResults.IsprevSummaryEnoughData = false;
                }


                dynamic jsonResult = new ExpandoObject();

                jsonResult.noOfDocsJson = noOfDocs;
                jsonResult.noOfHitsJson = noOfHits;
                jsonResult.noOfMinOfAiringJson = noOfMinofAiring;
                jsonResult.noOfAdJson = noOfAd;
                jsonResult.noOfViewJson = noOfView;
                jsonResult.CategoryDescription = p_MediaType.DisplayName;
                jsonResult.fromDate = p_FromDate.ToString();
                jsonResult.toDate = p_ToDate.ToString();
                jsonResult.sentimentChart = sentiMentChart;
                jsonResult.dmaMapJson = dmaMapChart;
                jsonResult.canadaMapJson = canadaMapChart;

                Dictionary<string, object> dictResults = new Dictionary<string, object>();
                dictResults["MediaResults"] = dashboardMediaResults;
                dictResults["JsonResult"] = jsonResult;

                return dictResults;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Dictionary<string, object> GetDashboardAdhocResults(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, int? chartWidth, bool isUGCEnabled, ActiveUser p_User)
        {
            try
            {
                // Remove the MS media type if viewing Feeds data
                List<IQ_MediaTypeModel> lstMediaTypes = p_User.MediaTypes;
                if (!isUGCEnabled)
                {
                    List<string> availMediaTypes = p_User.MediaTypes.Where(w => w.TypeLevel == 1 && !w.IsArchiveOnly).Select(s => s.MediaType).ToList();
                    lstMediaTypes = p_User.MediaTypes.Where(w => availMediaTypes.Contains(w.MediaType)).ToList();
                }

                SummaryReportMulti linechart = LineChartForAdhocSummary(listOfSummaryReportData, p_FromDate, p_ToDate, p_SearchType, chartWidth, p_User, lstMediaTypes);
                string piechart = PieChart(listOfSummaryReportData, p_FromDate, p_ToDate, lstMediaTypes);

                DashboardOverviewResults dashboardOverviewResults = new DashboardOverviewResults();
                
                dashboardOverviewResults.SumAudienceRecord = linechart.AudienceRecordsSum;
                dashboardOverviewResults.PrevSumAudienceRecord = linechart.AudiencePrevRecordsSum;
                dashboardOverviewResults.SumIQMediaValueRecord = linechart.IQMediaValueRecordsSum;
                dashboardOverviewResults.PrevSumIQMediaValueRecord = linechart.IQMediaValuePrevRecordsSum;
                
                dashboardOverviewResults.TotNumOfHits = linechart.TotalNumOfHits;
                dashboardOverviewResults.IsprevSummaryEnoughData = false;

                dashboardOverviewResults.ReportMediumList = linechart.SummaryReportMedium.OrderBy(mt => mt.MediaTypeModel.SortOrder).Select(s => new ReportMedium() { MediaType = s.MediaTypeModel.MediaType, DisplayName = s.MediaTypeModel.DisplayName, PrevRecordsSum = s.PrevRecordsSum, Records = s.Records, RecordsSum = s.RecordsSum }).ToList();

                dynamic jsonResult = new ExpandoObject();

                jsonResult.jsonMediaRecord = linechart.MediaRecords;
                jsonResult.jsonSubMediaRecord = linechart.SubMediaRecords;
                jsonResult.jsonPieChartSubMedia = piechart;
                
                jsonResult.jsonAudienceRecord = linechart.AudienceRecords;
                jsonResult.jsonIQMediaValueRecords = linechart.IQMediaValueRecords;
                jsonResult.CategoryDescription = "Overview";
                jsonResult.fromDate = p_FromDate.ToString();
                jsonResult.toDate = p_ToDate.ToString();

                jsonResult.ReportMediumList = dashboardOverviewResults.ReportMediumList;

                NumberFormatInfo numInfo = new NumberFormatInfo();
                numInfo.NumberGroupSeparator = String.Empty; // Format the number without comma separators
                jsonResult.totalHits = Decimal.Parse(linechart.TotalNumOfHits).ToString("N0", numInfo);

                Dictionary<string, object> dictResults = new Dictionary<string, object>();
                dictResults["OverviewResults"] = dashboardOverviewResults;
                dictResults["JsonResult"] = jsonResult;

                return dictResults;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static SummaryReportMulti LineChart(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, Dictionary<long, string> p_SearchRequests, List<IQAgent_ComparisionValues> p_ListOfIQAgent_ComparisionValues, List<ThirdPartyDataTypeModel> p_ThirdPartyDataTypes, ActiveUser p_User)
        {
            List<string> availMediaTypes = p_User.MediaTypes.Where(w => w.TypeLevel == 1 && !w.IsArchiveOnly).Select(s => s.MediaType).ToList();
            List<IQ_MediaTypeModel> lstMediaTypes = p_User.MediaTypes.Where(w => availMediaTypes.Contains(w.MediaType)).ToList();

            DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
            SummaryReportMulti lstSummaryReportMulti = null;
            if (p_SearchType == 0)
            {
                lstSummaryReportMulti = dashboardLogic.HighChartsLineChartHour(listOfSummaryReportData, p_FromDate, p_ToDate, null, p_User.gmt, p_User.dst, p_SearchRequests, p_ListOfIQAgent_ComparisionValues, p_ThirdPartyDataTypes, p_User.IsNielsenData, p_User.IsCompeteData, p_User.IsThirdPartyData, lstMediaTypes);
            }
            else if (p_SearchType == 1)
            {
                lstSummaryReportMulti = dashboardLogic.HighChartsLineChart(listOfSummaryReportData, p_FromDate, p_ToDate, null, p_SearchRequests, p_ListOfIQAgent_ComparisionValues, p_ThirdPartyDataTypes, p_User.IsNielsenData, p_User.IsCompeteData, p_User.IsThirdPartyData, lstMediaTypes);
            }
            else if (p_SearchType == 3)
            {
                lstSummaryReportMulti = dashboardLogic.HighChartsLineChartMonth(listOfSummaryReportData, p_FromDate, p_ToDate, p_SearchRequests, p_ListOfIQAgent_ComparisionValues, p_ThirdPartyDataTypes, p_User.IsNielsenData, p_User.IsCompeteData, p_User.IsThirdPartyData, lstMediaTypes);
            }

            return lstSummaryReportMulti;
        }

        public static SummaryReportMulti LineChartForAdhocSummary(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, short p_SearchType, int? chartWidth, ActiveUser p_User, List<IQ_MediaTypeModel> p_MediaTypes)
        {
            DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
            SummaryReportMulti lstSummaryReportMulti = null;
            if (p_SearchType == 0)
            {
                lstSummaryReportMulti = dashboardLogic.HighChartsLineChartHour(listOfSummaryReportData, p_FromDate, p_ToDate, chartWidth, p_User.gmt, p_User.dst, null, null, null, p_User.IsNielsenData, p_User.IsCompeteData, false, p_MediaTypes);
            }
            else if (p_SearchType == 1)
            {
                lstSummaryReportMulti = dashboardLogic.HighChartsLineChart(listOfSummaryReportData, p_FromDate, p_ToDate, chartWidth, null, null, null, p_User.IsNielsenData, p_User.IsCompeteData, false, p_MediaTypes);
            }

            return lstSummaryReportMulti;
        }

        public static string PieChart(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, List<IQ_MediaTypeModel> p_MediaTypeList)
        {
            DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
            string pieChart = dashboardLogic.HighChartPieChart(listOfSummaryReportData, p_FromDate, p_ToDate, p_MediaTypeList);
            return pieChart;
        }

        [HttpPost]
        public JsonResult LoadClipPlayer(string ClipID, Int16 HCC = 0,bool p_ARSZ=false, bool p_ASZ=false, Int16 p_AP=1)
        {
            try
            {
                sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                string ServiceBaseURL = Convert.ToString(ConfigurationManager.AppSettings["ServicesBaseURL"]);
                bool IsPlayFromLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);
                bool useCustomerEmailAsDefault = false;

                if (sessionInformation != null)
                {
                    ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                    IQClient_CustomSettingsModel customSettings = clientLogic.GetClientCustomSettings(sessionInformation.ClientGUID.ToString());
                    useCustomerEmailAsDefault = customSettings.UseCustomerEmailDefault.Value;
                }

                // Generate Clip Player Object
                var autoPlayback = true;

                if (p_AP==0)
                {
                    autoPlayback = false;
                }

                string ClipPlayer = UtilityLogic.RenderClipPlayer(ClipID, ServiceBaseURL, IsPlayFromLocal, sessionInformation == null ? "" : Convert.ToString(sessionInformation.ClientGUID), Request.Browser.Type, HideCC: HCC, p_AutoResize:p_ARSZ, p_AutoSize:p_ASZ,p_AutoPlayback:autoPlayback);
                //string ClipPlayer = UtilityLogic.RenderBasicRawMediaPlayer(string.Empty, ClipID, "true", "false", ClientGUID, "false", Convert.ToString(sessionInformation.CustomerGUID), ServiceBaseURL, null, sessionInformation.IsClientPlayerLogoActive.Value, PlayerLogo, Request.Browser.Type);

                // Get Closed Caption from ArchiveClip table
                IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                string ClosedCaption = string.Empty;

                Guid tempClipID;
                if (Guid.TryParse(ClipID, out tempClipID))
                {
                    IQArchive_ArchiveClipModel clip = iQArchieveLogic.GetArchiveClipByClipID(ClipID);

                    if (clip != null)
                    {
                        ClosedCaption = Server.HtmlDecode(clip.ClosedCaption);
                    }
                    SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                    Boolean IsSharing = sspLogic.GetSharing(ClipID, Request.Cookies[".IQAUTH"]);
                    Boolean IsEmailSharing = sspLogic.GetEmailSharing(ClipID, Request.Cookies[".IQAUTH"]);

                    var json = new
                    {
                        isSuccess = true,
                        clipHTML = ClipPlayer,
                        closedCaption = ClosedCaption,
                        isSharing = IsSharing,
                        isEmailSharing = IsEmailSharing,
                        email = !useCustomerEmailAsDefault ? ConfigurationManager.AppSettings["Sender"] : sessionInformation.Email,
                        isFromEmailEditable = sessionInformation == null || useCustomerEmailAsDefault, // Only make the From field read-only if the user is logged in, so that we can use the UseCustomerEmailDefault custom setting to be sure they want the donotreply address
                        clientGuid = sessionInformation == null ? new Guid() : sessionInformation.ClientGUID,
                        title = clip.ClipTitle
                    };

                    return Json(json);
                }
                else
                {
                    Log4NetLogger.Warning("Invalid clip guid found: " + ClipID);
                    return Json(new
                    {
                        isSuccess = false,
                        errorMessage = ConfigSettings.Settings.ErrorOccurred
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
            finally { TempData.Keep(); }
        }

        [HttpPost]
        public JsonResult BindCategoryDropDown()
        {
            try
            {
                if (IQMedia.WebApplication.Utility.ActiveUserMgr.CheckAuthentication())
                {
                    sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();
                    CustomCategoryLogic customCategoryLogic = (CustomCategoryLogic)LogicFactory.GetLogic(LogicType.Category);
                    IEnumerable<CustomCategoryModel> customCategoryModelList = customCategoryLogic.GetCustomCategory(sessionInformation.ClientGUID);

                    return Json(new
                    {
                        customCategory = customCategoryModelList,
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

        #region Cross domain request for IE 9 or below

        [HttpGet]
        public ActionResult GetVideoCategoryData()
        {
            return Content(CommonFunctions.DoHttpGetRequest(ConfigurationManager.AppSettings["UrlGetVideoCategoryData"], authCookie: GetAuthCookie()),"application/json; charset=utf-8");
        }

        [HttpPost]
        public ActionResult GetVideoNielsenData()
        {
            var data = "";

            using (var sr = new StreamReader(Request.InputStream))
            {
                data = sr.ReadToEnd();
            }

            return Content(CommonFunctions.DoHttpPostRequest(ConfigurationManager.AppSettings["UrlGetNielSenData"], data, GetAuthCookie()), "application/json; charset=utf-8");
        }

        [HttpPost]
        public ActionResult CreateClip()
        {
            var data = "";

            using (var sr = new StreamReader(Request.InputStream))
            {
                data = sr.ReadToEnd();
            }

            return Content(CommonFunctions.DoHttpPostRequest(ConfigurationManager.AppSettings["UrlCreateMediaClip"], data, GetAuthCookie()),"application/json; charset=utf-8");
        }

        [HttpGet]
        public ActionResult GenerateClipThumbnail(Guid fID, int Offset)
        {
            return Content(CommonFunctions.DoHttpGetRequest(ConfigurationManager.AppSettings["UrlGenerateMediaClipThumbnail"] + fID + "&Offset=" + Offset, authCookie: GetAuthCookie()), "application/json; charset=utf-8");
        }

        public ActionResult ExportClip(Guid fID)
        {
            return Content(CommonFunctions.DoHttpGetRequest(ConfigurationManager.AppSettings["UrlExportMediaClip"] + fID, authCookie: GetAuthCookie()), "application/json; charset=utf-8");
        }

        public ActionResult ExportIOSClip(Guid fID)
        {
            return Content(CommonFunctions.DoHttpGetRequest(ConfigurationManager.AppSettings["UrlExportIOSMediaClip"] + fID, authCookie: GetAuthCookie()), "application/json; charset=utf-8");
        }

        public ActionResult CreateClipTimeSync()
        {
            var data = "";

            using (var sr = new StreamReader(Request.InputStream))
            {
                data = sr.ReadToEnd();
            }

            return Content(CommonFunctions.DoHttpPostRequest(ConfigurationManager.AppSettings["UrlClipTimeSync"], data, GetAuthCookie()), "application/json; charset=utf-8");
        }

        public ActionResult SendMail()
        {
            var data = "";

            using (var sr = new StreamReader(Request.InputStream))
            {
                data = sr.ReadToEnd();
            }

            return Content(CommonFunctions.DoHttpPostRequest(ConfigurationManager.AppSettings["UrlSendEmail"], data, GetAuthCookie()), "application/json; charset=utf-8");
        }

        private Cookie GetAuthCookie()
        {
            var authCookie = Request.Cookies[".IQAUTH"];
            return authCookie != null ? new System.Net.Cookie(authCookie.Name, authCookie.Value, "/", ".iqmediacorp.com") : null;
        }

        #endregion
    }
}
