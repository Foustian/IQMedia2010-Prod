using System;
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

        public static Dictionary<string, object> GetDashboardOverviewResults(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, List<string> p_SearchRequestIDs, IQAgent_DashBoardPrevSummaryModel p_PrevIQAgentSummary)
        {
            try
            {
                ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                SummaryReportMulti linechart = LineChart(listOfSummaryReportData, p_FromDate, p_ToDate, p_SearchType, p_SearchRequestIDs, p_PrevIQAgentSummary != null ? p_PrevIQAgentSummary.ListOfIQAgentPrevSummary : null);
                string piechart = PieChart(listOfSummaryReportData, p_FromDate, p_ToDate, sessionInformation.Isv4TM, sessionInformation.Isv4NM, sessionInformation.Isv4SM, sessionInformation.Isv4TW, sessionInformation.Isv4TV, sessionInformation.Isv4BLPM, false, sessionInformation.Isv4PQ, sessionInformation.IsNielsenData, sessionInformation.IsCompeteData);

                DashboardOverviewResults dashboardOverviewResults = new DashboardOverviewResults();

                dashboardOverviewResults.SumTVRecord = linechart.TVRecordsSum;
                dashboardOverviewResults.PrevSumTVRecord = linechart.TVPrevRecordsSum;
                dashboardOverviewResults.SumNMRecord = linechart.NMRecordsSum;
                dashboardOverviewResults.PrevSumNMRecord = linechart.NMPrevRecordsSum;
                dashboardOverviewResults.SumTWRecord = linechart.TWRecordsSum;
                dashboardOverviewResults.PrevSumTWRecord = linechart.TWPrevRecordsSum;
                dashboardOverviewResults.SumForumRecord = linechart.ForumRecordsSum;
                dashboardOverviewResults.PrevSumForumRecord = linechart.ForumPrevRecordsSum;
                dashboardOverviewResults.SumSocialMRecord = linechart.SocialMediaRecordsSum;
                dashboardOverviewResults.PrevSumSocialMRecord = linechart.SocialMediaPrevRecordsSum;
                dashboardOverviewResults.SumBlogRecord = linechart.BlogRecordsSum;
                dashboardOverviewResults.PrevSumBlogRecord = linechart.BlogPrevRecordsSum;
                dashboardOverviewResults.SumAudienceRecord = linechart.AudienceRecordsSum;
                dashboardOverviewResults.PrevSumAudienceRecord = linechart.AudiencePrevRecordsSum;
                dashboardOverviewResults.SumIQMediaValueRecord = linechart.IQMediaValueRecordsSum;
                dashboardOverviewResults.PrevSumIQMediaValueRecord = linechart.IQMediaValuePrevRecordsSum;
                dashboardOverviewResults.SumPMRecord = linechart.PMRecordsSum;
                dashboardOverviewResults.PrevSumPMRecord = linechart.PMPrevRecordsSum;
                dashboardOverviewResults.SumTMRecord = linechart.TMRecordsSum;
                dashboardOverviewResults.PrevSumTMRecord = linechart.TMPrevRecordsSum;
                dashboardOverviewResults.SumMSRecord = linechart.MSRecordsSum;
                dashboardOverviewResults.TotNumOfHits = linechart.TotalNumOfHits;
                dashboardOverviewResults.IsprevSummaryEnoughData = p_PrevIQAgentSummary != null ? p_PrevIQAgentSummary.IsEnoughData : false;

                dynamic jsonResult = new ExpandoObject();

                jsonResult.jsonMediaRecord = linechart.MediaRecords;
                jsonResult.jsonSubMediaRecord = linechart.SubMediaRecords;
                jsonResult.jsonPieChartSubMedia = piechart;
                jsonResult.jsonTVRecord = linechart.TVRecords;
                jsonResult.jsonNMRecord = linechart.NMRecords;
                jsonResult.jsonTWRecord = linechart.TWRecords;
                jsonResult.jsonForumRecord = linechart.ForumRecords;
                jsonResult.jsonSocialMRecord = linechart.SocialMediaRecords;
                jsonResult.jsonBlogRecord = linechart.BlogRecords;
                jsonResult.jsonAudienceRecord = linechart.AudienceRecords;
                jsonResult.jsonIQMediaValueRecords = linechart.IQMediaValueRecords;
                jsonResult.CategoryDescription = "Overview";
                jsonResult.fromDate = p_FromDate.ToString();
                jsonResult.toDate = p_ToDate.ToString();
                jsonResult.jsonPMRecord = linechart.PMRecords;
                jsonResult.jsonTMRecord = linechart.TMRecords;
                jsonResult.jsonMSRecord = linechart.MSRecords;

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

        public static Dictionary<string, object> GetDashboardMediumResults(string p_Medium, IQAgent_DashBoardModel iQAgent_DashBoardModel, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, List<string> p_SearchRequestIDs, bool useDMAMap)
        {
            try
            {
                DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
                ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                string noOfDocs = string.Empty;
                string noOfHits = string.Empty;
                string noOfMinofAiring = string.Empty;
                string noOfAd = string.Empty;
                string noOfView = string.Empty;

                Int64 noOfHitsCount = 0;
                Int64 totalAirSeconds = 0;
                Int64 noOfViewsCount = 0;
                Decimal noOfMinsOfAiringCount = 0;
                Decimal noOfAdCount = 0;

                if (p_SearchType == 1)
                {
                    noOfDocs = dashboardLogic.GetHighChartForDocs(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.DashBoardMediumType>(p_Medium.Replace(" ", string.Empty))), p_SearchRequestIDs, out totalAirSeconds);

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
                    }
                }
                else if (p_SearchType == 0)
                {
                    noOfDocs = dashboardLogic.GetHighChartForDocsHourly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.DashBoardMediumType>(p_Medium.Replace(" ", string.Empty))), sessionInformation.gmt, sessionInformation.dst, p_SearchRequestIDs, out totalAirSeconds);

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
                    }
                }
                else if (p_SearchType == 2)
                {

                }
                else if (p_SearchType == 3)
                {
                    noOfDocs = dashboardLogic.GetHighChartForDocsMonthly(iQAgent_DashBoardModel.ListOfIQAgentSummary, p_FromDate, p_ToDate, CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.DashBoardMediumType>(p_Medium.Replace(" ", string.Empty))), p_SearchRequestIDs, out totalAirSeconds);

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
                }

                string dmaMapChart = string.Empty;
                string canadaMapChart = string.Empty;
                if (useDMAMap && (p_Medium == CommonFunctions.DashBoardMediumType.TV.ToString() || p_Medium == CommonFunctions.DashBoardMediumType.NM.ToString()))
                {
                    dmaMapChart = dashboardLogic.GetFusionUsaDmaMap(iQAgent_DashBoardModel.DmaMentionMapList);
                    canadaMapChart = dashboardLogic.GetFusionCanadaProvinceMap(iQAgent_DashBoardModel.CanadaMentionMapList);
                }

                Int64 negativeSentiment = 0;
                Int64 positiveSentiment = 0;
                string sentiMentChart = string.Empty;
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

                DashboardMediaResults dashboardMediaResults = new DashboardMediaResults();

                dashboardMediaResults.SumNegativeSentiment = negativeSentiment;
                dashboardMediaResults.SumPositiveSentiment = positiveSentiment;
                dashboardMediaResults.SumAirSeconds = totalAirSeconds;
                dashboardMediaResults.SumHits = noOfHitsCount;
                dashboardMediaResults.SumAudience = noOfViewsCount;
                dashboardMediaResults.SumIQMediaValue = noOfAdCount;

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
                jsonResult.CategoryDescription = CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.DashBoardMediumType>(p_Medium.Replace(" ", string.Empty)));
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

        public static Dictionary<string, object> GetDashboardAdhocResults(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, int? chartWidth, bool isUGCEnabled)
        {
            try
            {
                ActiveUser sessionInformation = IQMedia.WebApplication.Utility.ActiveUserMgr.GetActiveUser();

                SummaryReportMulti linechart = LineChartForAdhocSummary(listOfSummaryReportData, p_FromDate, p_ToDate, p_SearchType, chartWidth, isUGCEnabled);
                string piechart = PieChart(listOfSummaryReportData, p_FromDate, p_ToDate, sessionInformation.Isv4TM, sessionInformation.Isv4NM, sessionInformation.Isv4SM, sessionInformation.Isv4TW, sessionInformation.Isv4TV, sessionInformation.Isv4BLPM, isUGCEnabled, sessionInformation.Isv4PQ, sessionInformation.IsNielsenData, sessionInformation.IsCompeteData);

                DashboardOverviewResults dashboardOverviewResults = new DashboardOverviewResults();

                dashboardOverviewResults.SumTVRecord = linechart.TVRecordsSum;
                dashboardOverviewResults.PrevSumTVRecord = linechart.TVPrevRecordsSum;
                dashboardOverviewResults.SumNMRecord = linechart.NMRecordsSum;
                dashboardOverviewResults.PrevSumNMRecord = linechart.NMPrevRecordsSum;
                dashboardOverviewResults.SumTWRecord = linechart.TWRecordsSum;
                dashboardOverviewResults.PrevSumTWRecord = linechart.TWPrevRecordsSum;
                dashboardOverviewResults.SumForumRecord = linechart.ForumRecordsSum;
                dashboardOverviewResults.PrevSumForumRecord = linechart.ForumPrevRecordsSum;
                dashboardOverviewResults.SumSocialMRecord = linechart.SocialMediaRecordsSum;
                dashboardOverviewResults.PrevSumSocialMRecord = linechart.SocialMediaPrevRecordsSum;
                dashboardOverviewResults.SumBlogRecord = linechart.BlogRecordsSum;
                dashboardOverviewResults.PrevSumBlogRecord = linechart.BlogPrevRecordsSum;
                dashboardOverviewResults.SumAudienceRecord = linechart.AudienceRecordsSum;
                dashboardOverviewResults.PrevSumAudienceRecord = linechart.AudiencePrevRecordsSum;
                dashboardOverviewResults.SumIQMediaValueRecord = linechart.IQMediaValueRecordsSum;
                dashboardOverviewResults.PrevSumIQMediaValueRecord = linechart.IQMediaValuePrevRecordsSum;
                dashboardOverviewResults.SumPMRecord = linechart.PMRecordsSum;
                dashboardOverviewResults.PrevSumPMRecord = linechart.PMPrevRecordsSum;
                dashboardOverviewResults.SumTMRecord = linechart.TMRecordsSum;
                dashboardOverviewResults.PrevSumTMRecord = linechart.TMPrevRecordsSum;
                dashboardOverviewResults.SumMSRecord = linechart.MSRecordsSum;
                dashboardOverviewResults.TotNumOfHits = linechart.TotalNumOfHits;
                dashboardOverviewResults.IsprevSummaryEnoughData = false;

                dynamic jsonResult = new ExpandoObject();

                jsonResult.jsonMediaRecord = linechart.MediaRecords;
                jsonResult.jsonSubMediaRecord = linechart.SubMediaRecords;
                jsonResult.jsonPieChartSubMedia = piechart;
                jsonResult.jsonTVRecord = linechart.TVRecords;
                jsonResult.jsonNMRecord = linechart.NMRecords;
                jsonResult.jsonTWRecord = linechart.TWRecords;
                jsonResult.jsonForumRecord = linechart.ForumRecords;
                jsonResult.jsonSocialMRecord = linechart.SocialMediaRecords;
                jsonResult.jsonBlogRecord = linechart.BlogRecords;
                jsonResult.jsonAudienceRecord = linechart.AudienceRecords;
                jsonResult.jsonIQMediaValueRecords = linechart.IQMediaValueRecords;
                jsonResult.CategoryDescription = "Overview";
                jsonResult.fromDate = p_FromDate.ToString();
                jsonResult.toDate = p_ToDate.ToString();
                jsonResult.jsonPMRecord = linechart.PMRecords;
                jsonResult.jsonTMRecord = linechart.TMRecords;
                jsonResult.jsonMSRecord = linechart.MSRecords;

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

        public static SummaryReportMulti LineChart(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, Int16 p_SearchType, List<string> p_SearchRequestIDs, List<IQAgent_ComparisionValues> p_ListOfIQAgent_ComparisionValues)
        {
            ActiveUser sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
            DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
            SummaryReportMulti lstSummaryReportMulti = null;
            if (p_SearchType == 0)
            {
                 lstSummaryReportMulti = dashboardLogic.HighChartsLineChartHour(listOfSummaryReportData, p_FromDate, p_ToDate, null, sessionInformation.Isv4TM, sessionInformation.gmt, sessionInformation.dst, p_SearchRequestIDs, p_ListOfIQAgent_ComparisionValues, sessionInformation.Isv4NM, sessionInformation.Isv4SM, sessionInformation.Isv4TW, sessionInformation.Isv4TV, sessionInformation.Isv4BLPM, false, sessionInformation.Isv4PQ, sessionInformation.IsNielsenData, sessionInformation.IsCompeteData, sessionInformation.Isv4Google);
            }
            else if (p_SearchType == 1)
            {
                lstSummaryReportMulti = dashboardLogic.HighChartsLineChart(listOfSummaryReportData, p_FromDate, p_ToDate, null, sessionInformation.Isv4TM, p_SearchRequestIDs, p_ListOfIQAgent_ComparisionValues, sessionInformation.Isv4NM, sessionInformation.Isv4SM, sessionInformation.Isv4TW, sessionInformation.Isv4TV, sessionInformation.Isv4BLPM, false, sessionInformation.Isv4PQ, sessionInformation.IsNielsenData, sessionInformation.IsCompeteData, sessionInformation.Isv4Google);
            }
            else if (p_SearchType == 3)
            {
                lstSummaryReportMulti = dashboardLogic.HighChartsLineChartMonth(listOfSummaryReportData, p_FromDate, p_ToDate, sessionInformation.Isv4TM, p_SearchRequestIDs, p_ListOfIQAgent_ComparisionValues, sessionInformation.Isv4NM, sessionInformation.Isv4SM, sessionInformation.Isv4TW, sessionInformation.Isv4TV, sessionInformation.Isv4BLPM, false, sessionInformation.Isv4PQ, sessionInformation.IsNielsenData, sessionInformation.IsCompeteData, sessionInformation.Isv4Google);
            }

            return lstSummaryReportMulti;
        }

        public static SummaryReportMulti LineChartForAdhocSummary(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, short p_SearchType, int? chartWidth, bool isUGCEnabled = false)
        {
            ActiveUser sessionInformation = Utility.ActiveUserMgr.GetActiveUser();
            DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
            SummaryReportMulti lstSummaryReportMulti = null;
            if (p_SearchType == 0)
            {
                lstSummaryReportMulti = dashboardLogic.HighChartsLineChartHour(listOfSummaryReportData, p_FromDate, p_ToDate, chartWidth, sessionInformation.Isv4TM, sessionInformation.gmt, sessionInformation.dst, null, null, sessionInformation.Isv4NM, sessionInformation.Isv4SM, sessionInformation.Isv4TW, sessionInformation.Isv4TV, sessionInformation.Isv4BLPM, isUGCEnabled, sessionInformation.Isv4PQ, sessionInformation.IsNielsenData, sessionInformation.IsCompeteData, false);
            }
            else if (p_SearchType == 1)
            {
                lstSummaryReportMulti = dashboardLogic.HighChartsLineChart(listOfSummaryReportData, p_FromDate, p_ToDate, chartWidth, sessionInformation.Isv4TM, null, null, sessionInformation.Isv4NM, sessionInformation.Isv4SM, sessionInformation.Isv4TW, sessionInformation.Isv4TV, sessionInformation.Isv4BLPM, isUGCEnabled, sessionInformation.Isv4PQ, sessionInformation.IsNielsenData, sessionInformation.IsCompeteData, false);
            }

            return lstSummaryReportMulti;
        }

        public static string PieChart(List<SummaryReportModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, bool p_Isv4TMAccess, bool p_Isv4NMAccess, bool p_Isv4SMAccess, bool p_Isv4TWAccess, bool p_Isv4TVAccess, bool p_Isv4BLPMAccess, bool p_Isv4UGCAccess, bool p_Isv4PQAccess, bool p_NielsenAccess, bool p_CompeteDataAccess)
        {
            DashboardLogic dashboardLogic = (DashboardLogic)LogicFactory.GetLogic(LogicType.Dashboard);
            string pieChart = dashboardLogic.HighChartPieChart(listOfSummaryReportData, p_FromDate, p_ToDate, p_Isv4TMAccess, p_Isv4NMAccess, p_Isv4SMAccess, p_Isv4TWAccess, p_Isv4TVAccess, p_Isv4BLPMAccess, p_Isv4UGCAccess, p_Isv4PQAccess, p_NielsenAccess, p_CompeteDataAccess);
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
                    email = sessionInformation == null ? "" : sessionInformation.Email,
                    clientGuid = sessionInformation == null ? new Guid() : sessionInformation.ClientGUID,
                    title = clip.ClipTitle
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
