using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using IQCommon.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;
using IQMedia.Shared.Utility;
//using AnalyticsSearch;

namespace IQMedia.Web.Logic
{
    public class AnalyticsLogic: IQMedia.Web.Logic.Base.ILogic
    {
        #region DataAccess

        public List<AnalyticsSecondaryTable> GetSecondaryTables(SecondaryTabID tab, string pageType)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            List<AnalyticsSecondaryTable> listSecondaryTables = analyticsDA.GetSecondaryTables(tab, pageType);

            return listSecondaryTables;
        }

        public List<AnalyticsActiveElement> GetActiveElements()
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            List<AnalyticsActiveElement> listActiveElements = analyticsDA.GetActiveElements();

            return listActiveElements;
        }

        public AnalyticsDataModel GetHourSummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool inDefaultRange, bool loadEverything = true)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            AnalyticsDataModel dataModel = analyticsDA.GetHourSummaryData(clientGUID, searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment, inDefaultRange, loadEverything);
            return dataModel;
        }

        public AnalyticsDataModel GetDaySummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool inDefaultRange, bool loadEverything = true)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            AnalyticsDataModel dataModel = analyticsDA.GetDaySummaryData(clientGUID, searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment, loadEverything, inDefaultRange);
            return dataModel;
        }

        public AnalyticsDataModel GetMonthSummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            AnalyticsDataModel dataModel = analyticsDA.GetMonthSummaryData(clientGUID, searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment);
            return dataModel;
        }

        public AnalyticsDataModel GetCampaignHourSummaryData(string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool loadEverything = true, string GroupByHeader = "")
        {
            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetHourSummaryDataForCampaign(searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment, loadEverything, GroupByHeader);
        }

        public AnalyticsDataModel GetCampaignDaySummaryData(string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, bool loadEverything = true)
        {
            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetDaySummaryDataForCampaign(searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment, loadEverything);
        }

        public AnalyticsDataModel GetNetworkShowSummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, decimal gmtAdjustment, decimal dstAdjustment, List<string> lstTopTen, SecondaryTabID tab, string dateInterval)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            AnalyticsDataModel dataModel = analyticsDA.GetNetworkShowSummaryData(clientGUID, searchRequestXml, subMediaType, gmtAdjustment, dstAdjustment, lstTopTen, tab, dateInterval);
            return dataModel;
        }

        public List<string> GetTopTenData(Guid clientGUID, string searchRequestXml, SecondaryTabID tab)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            List<string> topTen = analyticsDA.GetTopTenData(clientGUID, searchRequestXml, tab);
            return topTen;
        }

        public List<AnalyticsCampaign> GetCampaigns(Guid clientGUID)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            List<AnalyticsCampaign> lstCampaigns = analyticsDA.GetCampaigns(clientGUID);

            return lstCampaigns;
        }

        public AnalyticsCampaign GetCampaignByID(Int64 campaignID)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            return analyticsDA.GetCampaignByID(campaignID);
        }

        public DateTime EditCampaign(Int64 campaignID, string campaignName, Int64? agentSRID, DateTime? startDate, DateTime? endDate, DateTime? startDateGMT, DateTime? endDateGMT)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            DateTime modifiedDate = analyticsDA.EditCampaign(campaignID, campaignName, agentSRID, startDate, endDate, startDateGMT, endDateGMT);
            return modifiedDate;
        }

        public Int64 CreateCampaign(string campaignName, int agentSRID, DateTime startDate, DateTime endDate, DateTime startDateGMT, DateTime endDateGMT)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            return analyticsDA.CreateCampaign(campaignName, agentSRID, startDate, endDate, startDateGMT, endDateGMT);
        }

        public void DeleteCampaign(Int64 campaignID)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();
            analyticsDA.DeleteCampaign(campaignID);
        }

        public Dictionary<string, string> GetAllDMAs()
        {
            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetAllDMAs();
        }

        public List<DayPartDataItem> GetDayPartData(string AffiliateCode)
        {
            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetDayPartData(AffiliateCode);
        }

        #endregion

        #region Utility

        private List<string> GetChartXAxisValues(AnalyticsRequest graphRequest, out List<DateTime> dateRange)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //Log4NetLogger.Debug("GetChartXAxisValues");
            Boolean isCampaign = graphRequest.PageType == "campaign";

            List<string> xAxisValues = new List<string>();
            dateRange = new List<DateTime>();
            DateTime startDate;
            DateTime endDate;
            KeyValuePair<long, TimeSpan> maxSpan;

            if (isCampaign)
            {
                // Only include campaigns requested, should always have requested campaigns or a specific request for a blank chart
                var requestedCampaigns = graphRequest.Campaigns.Where(w => graphRequest.RequestIDs.Contains(w.CampaignID)).ToList();

                Dictionary<long, TimeSpan> timeSpanList = requestedCampaigns.Select(campaign => new {
                    campaign.CampaignID,
                    timeSpan = campaign.EndDate.Subtract(campaign.StartDate)
                }).ToDictionary(
                    c => c.CampaignID,
                    c => c.timeSpan
                );

                maxSpan = timeSpanList.First(span => span.Value == timeSpanList.Values.Max());
                startDate = graphRequest.Campaigns.First(campaign => campaign.CampaignID == maxSpan.Key).StartDate;
                endDate = graphRequest.Campaigns.First(campaign => campaign.CampaignID == maxSpan.Key).EndDate;
            }
            else
            {
                // If on amplification, max span is taken from overall date range specified in request
                maxSpan = new KeyValuePair<long, TimeSpan>(-1, Convert.ToDateTime(graphRequest.DateTo).Subtract(Convert.ToDateTime(graphRequest.DateFrom)));
                startDate = Convert.ToDateTime(graphRequest.DateFrom);
                endDate = Convert.ToDateTime(graphRequest.DateTo);
            }

            switch (graphRequest.DateInterval)
            {
                case "hour":
                    for (int i = 0; i <= maxSpan.Value.TotalHours; i++)
                    {
                        dateRange.Add(startDate.AddHours(i));
                    }

                    if (!isCampaign)
                    {
                        xAxisValues = dateRange.Select(s => s.ToString()).ToList();
                    }
                    else
                    {
                        for (int i = 0; i < dateRange.Count; i++)
                        {
                            xAxisValues.Add(i.ToString());
                        }
                    }
                    break;
                case "day":
                    for (int i = 0; i <= maxSpan.Value.Days; i++)
                    {
                        dateRange.Add(startDate.AddDays(i));
                    }

                    if (!isCampaign)
                    {
                        xAxisValues = dateRange.Select(date => date.ToShortDateString()).ToList();
                    }
                    else
                    {
                        for (int i = 0; i < dateRange.Count; i++)
                        {
                            xAxisValues.Add(i.ToString());
                        }
                    }
                    break;
                case "month":
                    // Get number of months between start and end
                    var months = ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;

                    for (int i = 0; i <= months; i++)
                    {
                        dateRange.Add(startDate.AddMonths(i));
                    }

                    if (!isCampaign)
                    {
                        xAxisValues = dateRange.Select(date => date.ToShortDateString()).ToList();
                    }
                    else
                    {
                        for (int i = 0; i <= months; i++)
                        {
                            xAxisValues.Add(i.ToString());
                        }
                    }
                    break;
            }

            sw.Stop();
            //Log4NetLogger.Debug(string.Format("GetChartXAxisValues: {0} ms", sw.ElapsedMilliseconds));
            return xAxisValues;
        }

        private AnalyticsPESHFilters GetPESHFilters(List<string> PESHTypes, List<string> sourceGroups)
        {
            // Earned + Paid = All
            // Seen + Heard + Read = All

            AnalyticsPESHFilters filters = new AnalyticsPESHFilters();
            filters.isFiltering = (PESHTypes != null && PESHTypes.Count > 0) || (sourceGroups != null && sourceGroups.Count > 0);
            filters.isOnAir = false;
            filters.isOnline = false;
            filters.isPrint = false;

            filters.isSeenEarned = false;
            filters.isHeardEarned = false;

            filters.isSeenPaid = false;
            filters.isHeardPaid = false;

            if (PESHTypes != null && PESHTypes.Count > 0)
            {
                // Read is a unique filter. Within the Seen/Heard/Read group, if it is the only one selected then OnAir results should be excluded. 
                // But if Seen or Heard is also selected then OnAir results should be included. 
                // This is accomplished by filtering out the SeenPaid, SeenEarned, HeardPaid, and HeardEarned values when necessary, since those are 
                bool isRead = PESHTypes.Contains("Read");
                bool isSeen = PESHTypes.Contains("Seen");
                bool isHeard = PESHTypes.Contains("Heard");
                bool isPaid = PESHTypes.Contains("Paid");
                bool isEarned = PESHTypes.Contains("Earned");

                filters.isReadEarned = (isRead && isEarned) || (isRead && !isPaid) || (isEarned && !isHeard && !isSeen);

                // Seen Earned must be isSeen or earned and not heard
                filters.isSeenEarned = (isSeen && isEarned) || (isSeen && !isPaid) || (isEarned && !isHeard && !isRead);

                // heard earned must be isHeard or earned and not seen
                filters.isHeardEarned = (isHeard && isEarned) || (isHeard && !isPaid) || (isEarned && !isSeen && !isRead);

                // seen paid must be isSeen or paid and not heard
                filters.isSeenPaid = (isSeen && isPaid) || (isSeen && !isEarned) || (isPaid && !isHeard && !isRead);

                // heard paid must be isHeard or paid and not seen
                filters.isHeardPaid = (isHeard && isPaid) || (isHeard && !isEarned) || (isPaid && !isSeen && !isRead);

                /*
                filters.isRead = isRead;
                // isEarned if only picked earned PESH type - if isEarned & notRead & notSeen & notHeard => isEarned & isSeenEarned
                filters.isEarned = isEarned && !isSeen && !isHeard; // E = E & NOT S & NOT H -> NOT EARNED IF SEEN OR HEARD

                // Only fails when isEarned & isRead & notSeen & notHeard
                if (!(isEarned && isRead && !isSeen && !isHeard)) // => (NOT E & NOT R) OR (S OR H)
                {
                    // Seen Earned must be isSeen or earned and not heard
                    filters.isSeenEarned = (isSeen && isEarned) || (isSeen && !isPaid) || (isEarned && !isHeard);
                    // heard earned must be isHeard or earned and not seen
                    filters.isHeardEarned = (isHeard && isEarned) || (isHeard && !isPaid) || (isEarned && !isSeen);
                }

                if (!(isPaid && isRead && !isSeen && !isHeard)) // => (NOT P & NOT R) OR (S 0R H)
                {
                    // seen paid must be isSeen or paid and not heard
                    filters.isSeenPaid = (isSeen && isPaid) || (isSeen && !isEarned) || (isPaid && !isHeard);
                    // heard paid must be isHeard or paid and not seen
                    filters.isHeardPaid = (isHeard && isPaid) || (isHeard && !isEarned) || (isPaid && !isSeen);
                }*/
            }
            if (sourceGroups != null && sourceGroups.Count > 0)
            {
                filters.isOnAir = sourceGroups.Contains("OnAir");
                filters.isOnline = sourceGroups.Contains("Online");
                filters.isPrint = sourceGroups.Contains("Print");
            }

            return filters;
        }

        private long GetSumsFromSummaries(List<string> PESHTypes, List<IQ_MediaTypeModel> lstSubMediaTypes, AnalyticsPESHFilters peshFilters, IEnumerable<AnalyticsSummaryModel> summaries)
        {
            List<string> onAirSubMediaTypes = lstSubMediaTypes.Where(w => w.AnalyticsDataType.Equals("OnAir")).Select(s => s.SubMediaType).ToList();
            List<string> onlineSubMediaTypes = lstSubMediaTypes.Where(w => w.AnalyticsDataType.Equals("Online")).Select(s => s.SubMediaType).ToList();
            List<string> printSubMediaTypes = lstSubMediaTypes.Where(w => w.AnalyticsDataType.Equals("Print")).Select(s => s.SubMediaType).ToList();

            long returnSum = 0;

            // Neither a PESH type nor a source group was selected
            if (!peshFilters.isFiltering)
            {
                returnSum = summaries.Sum(summary => summary.Number_Of_Hits);
            }
            else
            {
                if (peshFilters.isOnAir || peshFilters.isOnline || peshFilters.isPrint)
                {
                    var partialSummaries = new List<AnalyticsSummaryModel>();
                    if (peshFilters.isOnAir)
                    {
                        partialSummaries.AddRange(summaries.Where(w => onAirSubMediaTypes.Contains(w.SubMediaType)));
                    }
                    if (peshFilters.isOnline)
                    {
                        partialSummaries.AddRange(summaries.Where(w => onlineSubMediaTypes.Contains(w.SubMediaType)));
                    }
                    if (peshFilters.isPrint)
                    {
                        partialSummaries.AddRange(summaries.Where(w => printSubMediaTypes.Contains(w.SubMediaType)));
                    }

                    summaries = partialSummaries;
                    Log4NetLogger.Debug(string.Format("partialSummaries.Count: {0}", summaries.Count()));
                }

                // If filtering to source groups but NOT to PESH types
                if (PESHTypes == null || PESHTypes.Count == 0)
                {
                    returnSum = summaries.Sum(s => s.Number_Of_Hits);
                }
                /*if (!peshFilters.isEarned )
                {
                    Log4NetLogger.Debug(string.Format("!isEarned"));
                }
                // Filtering to source groups and PESH types
                else
                {
                    // Read is a subset of Earned, so if Read is selected then Earned doesn't matter.
                    if (peshFilters.isRead)
                    {
                        returnSum += summaries.Sum(s => s.Number_Of_Hits - s.SeenEarned - s.SeenPaid - s.HeardEarned - s.HeardPaid);
                    }
                    else if (peshFilters.isEarned)
                    {
                        // Earned includes all media types. - all types but not TV?
                        returnSum += summaries.Where(w => w.SubMediaType != "TV").Sum(s => s.Number_Of_Hits);
                    }*/

                    if (peshFilters.isSeenEarned)
                    {
                        returnSum += summaries.Sum(s => s.SeenEarned);
                    }
                    if (peshFilters.isSeenPaid)
                    {
                        returnSum += summaries.Sum(s => s.SeenPaid);
                    }
                    if (peshFilters.isHeardEarned)
                    {
                        returnSum += summaries.Sum(s => s.HeardEarned);
                    }
                    if (peshFilters.isHeardPaid)
                    {
                        returnSum += summaries.Sum(s => s.HeardPaid);
                    }
                    if (peshFilters.isReadEarned)
                    {
                        returnSum += summaries.Sum(s => s.ReadEarned);
                    } 
                /*}*/
            }

            return returnSum;
        }

        private List<AnalyticsSummaryModel> GetSummariesForSources(List<IQ_MediaTypeModel> subMediaTypes, AnalyticsPESHFilters peshFilters, IEnumerable<AnalyticsSummaryModel> summaries)
        {
            try
            {
                List<string> onAirSubMediaTypes = subMediaTypes.Where(w => w.AnalyticsDataType.Equals("OnAir")).Select(s => s.SubMediaType).ToList();
                List<string> onlineSubMediaTypes = subMediaTypes.Where(w => w.AnalyticsDataType.Equals("Online")).Select(s => s.SubMediaType).ToList();
                List<string> printSubMediaTypes = subMediaTypes.Where(w => w.AnalyticsDataType.Equals("Print")).Select(s => s.SubMediaType).ToList();
                var partialSummaries = new List<AnalyticsSummaryModel>();
                if (peshFilters.isFiltering)
                {
                    if (peshFilters.isOnAir || peshFilters.isOnline || peshFilters.isPrint)
                    {
                        if (peshFilters.isOnAir)
                        {
                            partialSummaries.AddRange(summaries.Where(w => onAirSubMediaTypes.Contains(w.SubMediaType)));
                        }
                        if (peshFilters.isOnline)
                        {
                            partialSummaries.AddRange(summaries.Where(w => onlineSubMediaTypes.Contains(w.SubMediaType)));
                        }
                        if (peshFilters.isPrint)
                        {
                            partialSummaries.AddRange(summaries.Where(w => printSubMediaTypes.Contains(w.SubMediaType)));
                        }
                    }
                }
                else
                {
                    partialSummaries.AddRange(summaries);
                }

                return partialSummaries;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new List<AnalyticsSummaryModel>();
            }
        }

        #endregion

        #region ChartCreation

        public Dictionary<string, object> GetChart(AnalyticsRequest graphRequest, AnalyticsSecondaryTable table, AnalyticsDataModel analyticsData, List<AnalyticsGrouping> groupings, List<IQ_MediaTypeModel> subMediaTypes)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Dictionary<string, object> seriesAndChart = new Dictionary<string, object>();
                if (groupings.Count > 0)
                {
                    //Log4NetLogger.Debug(string.Format("Logic.GetChart"));
                    //Log4NetLogger.Debug(string.Format("graphParams - HCChartType: {0}, chartType: {1}", graphRequest.HCChartType, graphRequest.chartType));

                    AnalyticsPESHFilters PESHFilters = GetPESHFilters(graphRequest.PESHTypes, graphRequest.SourceGroups);
                    switch (graphRequest.HCChartType)
                    {
                        case HCChartTypes.bar:
                        case HCChartTypes.column:
                            seriesAndChart = CreateBarOrColumnChart(groupings, graphRequest, subMediaTypes, PESHFilters);
                            break;
                        case HCChartTypes.spline:
                            seriesAndChart = CreateLineChart(groupings, graphRequest, subMediaTypes, PESHFilters);
                            break;
                        case HCChartTypes.pie:
                            seriesAndChart = CreatePieChart(groupings, graphRequest, subMediaTypes, PESHFilters);
                            break;
                        case HCChartTypes.heatmap:
                            seriesAndChart = CreateHeatMap(groupings, graphRequest, subMediaTypes, PESHFilters);
                            break;
                        case HCChartTypes.fusionMap:
                            seriesAndChart.Add("chart", CreateFusionMap(graphRequest, analyticsData));
                            seriesAndChart.Add("series", null);
                            break;
                    }
                }
                else
                {
                    seriesAndChart.Add("chart", null);
                    seriesAndChart.Add("series", null);
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("Logic.GetChart: {0} ms", sw.ElapsedMilliseconds));
                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        private Dictionary<string, object> CreateBarOrColumnChart(List<AnalyticsGrouping> groupings, AnalyticsRequest request, List<IQ_MediaTypeModel> subMediaTypes, AnalyticsPESHFilters PESHFilters)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug("CreateBarOrColumnChart");
                List<Series> seriesList = new List<Series>();

                HighColumnChartModel barOrColumn = new HighColumnChartModel() {
                    chart = new HChart() {
                        type = request.HCChartType.ToString()
                    },
                    title = new Title() {
                        text = string.Empty,
                        x = -20
                    },
                    subtitle = new Subtitle() {
                        text = string.Empty,
                        x = -20
                    },
                    legend = new Legend() {
                        align = "center",
                        borderWidth = "0",
                        width = 950,
                        enabled = false
                    },
                    xAxis = new XAxis() {
                        categories = new List<string>(),
                        tickWidth = 2,
                        tickInterval = 0,
                        tickmarkPlacement = "off",
                        labels = new labels() {
                            enabled = false
                        }
                    },
                    yAxis = new YAxis() {
                        title = new Title2() {
                            text = request.Tab == SecondaryTabID.Demographic ? "Audience" : "Occurrences",
                            rotation = request.HCChartType == HCChartTypes.bar ? 0 : 270
                        },
                        min = 0
                    },
                    tooltip = new Tooltip() {
                        formatter = "FormatBarColumnTooltip"
                    },
                    plotOptions = new PlotOptions() {
                        column = new Column() {
                            borderWidth = 0,
                            pointPadding = 0.2
                        },
                        series = new PlotSeries() {
                            cursor = "pointer",
                            point = new PlotPoint() {
                                events = new PlotEvents() {
                                    click = ""
                                }
                            }
                        }
                    },
                    series = new List<Series>()
                };

                if (request.Tab == SecondaryTabID.Demographic)
                {
                    List<AnalyticsSummaryModel> allSummaries = new List<AnalyticsSummaryModel>();
                    groupings.ForEach(e => {
                        allSummaries.AddRange(GetSummariesForSources(subMediaTypes, PESHFilters, e.Summaries));
                    });

                    if (request.SubTab == "gender")
                    {
                        Series maleSeries = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "male"
                        };

                        HighChartDatum maleDatum = new HighChartDatum() {
                            y = allSummaries.Sum(s => s.MaleAudience),
                            Value = "male"
                        };

                        maleSeries.data.Add(maleDatum);
                        seriesList.Add(maleSeries);
                        barOrColumn.series.Add(maleSeries);

                        Series femaleSeries = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "female"
                        };

                        HighChartDatum femaleDatum = new HighChartDatum() {
                            y = allSummaries.Sum(s => s.FemaleAudience),
                            Value = "female"
                        };

                        femaleSeries.data.Add(femaleDatum);
                        seriesList.Add(femaleSeries);
                        barOrColumn.series.Add(femaleSeries);
                    }
                    else
                    {
                        List<AnalyticsAgeRange> ageRanges = new List<AnalyticsAgeRange>();

                        ageRanges.Add(new AnalyticsAgeRange() {
                            AgeRange = "18-24",
                            MaleAudience = allSummaries.Sum(s => s.AM18_20 + s.AM21_24),
                            FemaleAudience = allSummaries.Sum(s => s.AF18_20 + s.AF21_24),
                            TotalAudience = allSummaries.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24)
                        });
                        ageRanges.Add(new AnalyticsAgeRange() {
                            AgeRange = "25-34",
                            MaleAudience = allSummaries.Sum(s => s.AM25_34),
                            FemaleAudience = allSummaries.Sum(s => s.AF25_34),
                            TotalAudience = allSummaries.Sum(s => s.AM25_34 + s.AF25_34)
                        });
                        ageRanges.Add(new AnalyticsAgeRange() {
                            AgeRange = "35-49",
                            MaleAudience = allSummaries.Sum(s => s.AM35_49),
                            FemaleAudience = allSummaries.Sum(s => s.AF35_49),
                            TotalAudience = allSummaries.Sum(s => s.AM35_49 + s.AF35_49)
                        });
                        ageRanges.Add(new AnalyticsAgeRange() {
                            AgeRange = "50-54",
                            MaleAudience = allSummaries.Sum(s => s.AM50_54),
                            FemaleAudience = allSummaries.Sum(s => s.AF50_54),
                            TotalAudience = allSummaries.Sum(s => s.AM50_54 + s.AF50_54)
                        });
                        ageRanges.Add(new AnalyticsAgeRange() {
                            AgeRange = "55-64",
                            MaleAudience = allSummaries.Sum(s => s.AM55_64),
                            FemaleAudience = allSummaries.Sum(s => s.AF55_64),
                            TotalAudience = allSummaries.Sum(s => s.AM55_64 + s.AF55_64)
                        });
                        ageRanges.Add(new AnalyticsAgeRange() {
                            AgeRange = "65+",
                            MaleAudience = allSummaries.Sum(s => s.AM65_Plus),
                            FemaleAudience = allSummaries.Sum(s => s.AF65_Plus),
                            TotalAudience = allSummaries.Sum(s => s.AM65_Plus + s.AF65_Plus)
                        });

                        foreach (var ar in ageRanges)
                        {
                            Series arSeries = new Series() {
                                data = new List<HighChartDatum>(),
                                name = ar.AgeRange
                            };

                            HighChartDatum arDatum = new HighChartDatum() {
                                Value = ar.AgeRange,
                                y = ar.TotalAudience
                            };

                            arSeries.data.Add(arDatum);
                            seriesList.Add(arSeries);
                            barOrColumn.series.Add(arSeries);
                        }
                    }
                }
                else
                {
                    int count = 0;
                    foreach (var group in groupings.OrderByDescending(g => g.Summaries.Sum(s => s.Number_Of_Hits)))
                    {
                        Series groupSeries = new Series() {
                            data = new List<HighChartDatum>(),
                            name = group.Name
                        };

                        HighChartDatum hcDatum = new HighChartDatum() {
                            y = group.Summaries.Any() ? GetSumsFromSummaries(request.PESHTypes, subMediaTypes, PESHFilters, group.Summaries) : 0,
                            SearchTerm = group.Name,
                            Value = group.ID
                        };

                        groupSeries.data.Add(hcDatum);
                        seriesList.Add(groupSeries);

                        if (request.Tab == SecondaryTabID.OverTime)
                        {
                            if (request.RequestIDs.Any(id => id.ToString().Equals(group.ID)))
                            {
                                barOrColumn.series.Add(groupSeries);
                            }
                        }
                        else
                        {
                            if (count < 10)
                            {
                                barOrColumn.series.Add(groupSeries);
                            }
                            count += 1;
                        }
                    }
                }

                Dictionary<string, object> seriesAndChart = new Dictionary<string, object>();
                seriesAndChart.Add("series", seriesList);
                seriesAndChart.Add("chart", CommonFunctions.SearializeJson(barOrColumn));

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateBarOrColumnChart: {0} ms", sw.ElapsedMilliseconds));

                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        private Dictionary<string, object> CreateLineChart(List<AnalyticsGrouping> groupings, AnalyticsRequest request, List<IQ_MediaTypeModel> subMediaTypes, AnalyticsPESHFilters PESHFilters)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Log4NetLogger.Debug("CreateLineChart");
                List<DateTime> distinctDates = new List<DateTime>();
                List<string> xAxisValues = GetChartXAxisValues(request, out distinctDates);
                List<Series> seriesList = new List<Series>();

                string chartTitle;
                if (request.ChartType == ChartType.Growth)
                {
                    chartTitle = "% Growth";
                }
                else
                {
                    if (request.Tab == SecondaryTabID.Demographic)
                    {
                        chartTitle = "Audience";
                    }
                    else
                    {
                        chartTitle = "Occurrences";
                    }
                }

                HighLineChartOutput lineChart = new HighLineChartOutput() {
                    hChart = new HChart() {
                        type = "spline"
                    },
                    title = new Title() {
                        text = string.Empty,
                        x = -20
                    },
                    subtitle = new Subtitle() {
                        text = string.Empty,
                        x = -20
                    },
                    legend = new Legend() {
                        align = "right",
                        borderWidth = "0",
                        enabled = true,
                        verticalAlign = "top",
                        layout = "vertical",
                        x = -90
                    },
                    xAxis = new XAxis() {
                        tickmarkPlacement = "off",
                        tickWidth = 2,
                        labels = new labels() {
                            staggerLines = request.DateInterval == "hour" ? 2 : 0
                        },
                        title = new Title2() {
                            rotation = 0,
                            text = request.PageType == "campaign" ? "Campaign Day" : "Date"
                        },
                        categories = xAxisValues,
                        tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(xAxisValues.Count()) / 7))
                    },
                    yAxis = new List<YAxis>() {
                        new YAxis() {
                            title = new Title2() {
                                text = chartTitle
                            }
                        }
                    },
                    tooltip = new Tooltip() {
                        formatter = "FormatSplineTooltip"
                    },
                    plotOption = new PlotOptions() {
                        series = new PlotSeries() {
                            cursor = "pointer",
                            point = new PlotPoint() {
                                events = new PlotEvents() {
                                    click = ""
                                }
                            }
                        },
                        spline = new PlotSeries() {
                            marker = new PlotMarker() {
                                enabled = true
                            }
                        }
                    },
                    series = new List<Series>()
                };

                if (request.Tab == SecondaryTabID.Demographic)
                {
                    List<AnalyticsSummaryModel> allSummaries = new List<AnalyticsSummaryModel>();
                    groupings.ForEach(e => {
                        allSummaries.AddRange(e.Summaries);
                    });

                    if (request.SubTab == "gender")
                    {
                        Series maleSeries = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "male"
                        };
                        Series femaleSeries = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "female"
                        };

                        long malePrevSum = 0;
                        long femalePrevSum = 0;

                        if (request.PageType == "campaign")
                        {
                            foreach (var step in xAxisValues)
                            {
                                var summariesForOffset = allSummaries.Where(w => w.CampaignOffset.Equals(Convert.ToInt64(step))).ToList();
                                summariesForOffset = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForOffset);
                                long maleSum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.MaleAudience) : 0;
                                long femaleSum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.FemaleAudience) : 0;
                                decimal malePctChange = 0;
                                decimal femalePctChange = 0;

                                if (malePrevSum != 0)
                                {
                                    malePctChange = ((maleSum - malePrevSum) / (decimal)malePrevSum) * 100;
                                }
                                malePrevSum = maleSum;
                                if (femalePrevSum != 0)
                                {
                                    femalePctChange = ((femaleSum - femalePrevSum) / (decimal)femalePrevSum) * 100;
                                }
                                femalePrevSum = femaleSum;

                                HighChartDatum maleDatum = new HighChartDatum() {
                                    Value = "male",
                                    SearchName = "male",
                                    y = request.ChartType == ChartType.Growth ? malePctChange : maleSum
                                };
                                maleSeries.data.Add(maleDatum);
                                HighChartDatum femaleDatum = new HighChartDatum() {
                                    Value = "female",
                                    SearchName = "female",
                                    y = request.ChartType == ChartType.Growth ? femalePctChange : femaleSum
                                };
                                femaleSeries.data.Add(femaleDatum);
                            }
                        }
                        else
                        {
                            foreach (var date in distinctDates)
                            {
                                var summariesForDate = allSummaries.Where(w => w.SummaryDateTime.Equals(date));
                                summariesForDate = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForDate);
                                long maleSum = summariesForDate.Any() ? summariesForDate.Sum(s => s.MaleAudience) : 0;
                                long femaleSum = summariesForDate.Any() ? summariesForDate.Sum(s => s.FemaleAudience) : 0;
                                decimal malePctChange = 0;
                                decimal femalePctChange = 0;

                                if (malePrevSum != 0)
                                {
                                    malePctChange = ((maleSum - malePrevSum) / (decimal)malePrevSum) * 100;
                                }
                                malePrevSum = maleSum;
                                if (femalePrevSum != 0)
                                {
                                    femalePctChange = ((femaleSum - femalePrevSum) / (decimal)femalePrevSum) * 100;
                                }
                                femalePrevSum = femaleSum;

                                HighChartDatum maleDatum = new HighChartDatum() {
                                    Date = date.ToShortDateString(),
                                    Value = "male",
                                    SearchName = "male",
                                    y = request.ChartType == ChartType.Growth ? malePctChange : maleSum
                                };
                                maleSeries.data.Add(maleDatum);
                                HighChartDatum femaleDatum = new HighChartDatum() {
                                    Date = date.ToShortDateString(),
                                    Value = "female",
                                    SearchName = "female",
                                    y = request.ChartType == ChartType.Growth ? femalePctChange : femaleSum
                                };
                                femaleSeries.data.Add(femaleDatum);
                            }
                        }

                        seriesList.Add(maleSeries);
                        seriesList.Add(femaleSeries);
                        lineChart.series.Add(maleSeries);
                        lineChart.series.Add(femaleSeries);
                    }
                    else
                    {
                        Series s18_24 = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "18-24"
                        };
                        long s18_24PrevSum = 0;

                        Series s25_34 = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "25-34"
                        };
                        long s25_34PrevSum = 0;

                        Series s35_49 = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "35-49"
                        };
                        long s35_49PrevSum = 0;

                        Series s50_54 = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "50-54"
                        };
                        long s50_54PrevSum = 0;

                        Series s55_64 = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "55-64"
                        };
                        long s55_64PrevSum = 0;

                        Series s65_Plus = new Series() {
                            data = new List<HighChartDatum>(),
                            name = "65+"
                        };
                        long s65_PlusPrevSum = 0;

                        if (request.PageType == "campaign")
                        {
                            foreach (var step in xAxisValues)
                            {
                                var summariesForOffset = allSummaries.Where(w => w.CampaignOffset.Equals(Convert.ToInt64(step))).ToList();
                                summariesForOffset = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForOffset);
                                long s18_24Sum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24) : 0;
                                decimal s18_24PctChange = 0;
                                long s25_34Sum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM25_34 + s.AF25_34) : 0;
                                decimal s25_34PctChange = 0;
                                long s35_49Sum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM35_49 + s.AF35_49) : 0;
                                decimal s35_49PctChange = 0;
                                long s50_54Sum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM50_54 + s.AF50_54) : 0;
                                decimal s50_54PctChange = 0;
                                long s55_64Sum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM55_64 + s.AF55_64) : 0;
                                decimal s55_64PctChange = 0;
                                long s65_PlusSum = summariesForOffset.Any() ? summariesForOffset.Sum(s => s.AM65_Plus + s.AF65_Plus) : 0;
                                decimal s65_PlusPctChange = 0;

                                if (s18_24PrevSum != 0)
                                {
                                    s18_24PctChange = ((s18_24Sum - s18_24PrevSum) / (decimal)s18_24PrevSum) * 100;
                                }
                                s18_24PrevSum = s18_24Sum;

                                if (s25_34PrevSum != 0)
                                {
                                    s25_34PctChange = ((s25_34Sum - s25_34PrevSum) / (decimal)s25_34PrevSum) * 100;
                                }
                                s25_34PrevSum = s25_34Sum;

                                if (s35_49PrevSum != 0)
                                {
                                    s35_49PctChange = ((s35_49Sum - s35_49PrevSum) / (decimal)s35_49PrevSum) * 100;
                                }
                                s35_49PrevSum = s35_49Sum;

                                if (s50_54PrevSum != 0)
                                {
                                    s50_54PctChange = ((s50_54Sum - s50_54PrevSum) / (decimal)s50_54PrevSum) * 100;
                                }
                                s50_54PrevSum = s50_54Sum;

                                if (s55_64PrevSum != 0)
                                {
                                    s55_64PctChange = ((s55_64Sum - s55_64PrevSum) / (decimal)s55_64PrevSum) * 100;
                                }
                                s55_64PrevSum = s55_64Sum;

                                if (s65_PlusPrevSum != 0)
                                {
                                    s65_PlusPctChange = ((s65_PlusSum - s65_PlusPrevSum) / (decimal)s65_PlusPrevSum) * 100;
                                }
                                s65_PlusPrevSum = s65_PlusSum;

                                HighChartDatum s18_24HCD = new HighChartDatum() {
                                    Value = s18_24.name,
                                    SearchName = s18_24.name,
                                    y = request.ChartType == ChartType.Growth ? s18_24PctChange : s18_24Sum
                                };
                                s18_24.data.Add(s18_24HCD);

                                HighChartDatum s25_34HCD = new HighChartDatum() {
                                    Value = s25_34.name,
                                    SearchName = s25_34.name,
                                    y = request.ChartType == ChartType.Growth ? s25_34PctChange : s25_34Sum
                                };
                                s25_34.data.Add(s25_34HCD);

                                HighChartDatum s35_49HCD = new HighChartDatum() {
                                    Value = s35_49.name,
                                    SearchName = s35_49.name,
                                    y = request.ChartType == ChartType.Growth ? s35_49PctChange : s35_49Sum
                                };
                                s35_49.data.Add(s35_49HCD);

                                HighChartDatum s50_54HCD = new HighChartDatum() {
                                    Value = s50_54.name,
                                    SearchName = s50_54.name,
                                    y = request.ChartType == ChartType.Growth ? s50_54PctChange : s50_54Sum
                                };
                                s50_54.data.Add(s50_54HCD);

                                HighChartDatum s55_64HCD = new HighChartDatum() {
                                    Value = s55_64.name,
                                    SearchName = s55_64.name,
                                    y = request.ChartType == ChartType.Growth ? s55_64PctChange : s55_64Sum
                                };
                                s55_64.data.Add(s55_64HCD);

                                HighChartDatum s65_PlusHCD = new HighChartDatum() {
                                    Value = s65_Plus.name,
                                    SearchName = s65_Plus.name,
                                    y = request.ChartType == ChartType.Growth ? s65_PlusPctChange : s65_PlusSum
                                };
                                s65_Plus.data.Add(s65_PlusHCD);
                            }
                        }
                        else
                        {
                            foreach (var date in distinctDates)
                            {
                                var summariesForDate = allSummaries.Where(w => w.SummaryDateTime.Equals(date));
                                summariesForDate = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForDate);
                                long s18_24Sum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24) : 0;
                                decimal s18_24PctChange = 0;
                                long s25_34Sum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM25_34 + s.AF25_34) : 0;
                                decimal s25_34PctChange = 0;
                                long s35_49Sum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM35_49 + s.AF35_49) : 0;
                                decimal s35_49PctChange = 0;
                                long s50_54Sum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM50_54 + s.AF50_54) : 0;
                                decimal s50_54PctChange = 0;
                                long s55_64Sum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM55_64 + s.AF55_64) : 0;
                                decimal s55_64PctChange = 0;
                                long s65_PlusSum = summariesForDate.Any() ? summariesForDate.Sum(s => s.AM65_Plus + s.AF65_Plus) : 0;
                                decimal s65_PlusPctChange = 0;

                                if (s18_24PrevSum != 0)
                                {
                                    s18_24PctChange = ((s18_24Sum - s18_24PrevSum) / (decimal)s18_24PrevSum) * 100;
                                }
                                s18_24PrevSum = s18_24Sum;

                                if (s25_34PrevSum != 0)
                                {
                                    s25_34PctChange = ((s25_34Sum - s25_34PrevSum) / (decimal)s25_34PrevSum) * 100;
                                }
                                s25_34PrevSum = s25_34Sum;

                                if (s35_49PrevSum != 0)
                                {
                                    s35_49PctChange = ((s35_49Sum - s35_49PrevSum) / (decimal)s35_49PrevSum) * 100;
                                }
                                s35_49PrevSum = s35_49Sum;

                                if (s50_54PrevSum != 0)
                                {
                                    s50_54PctChange = ((s50_54Sum - s50_54PrevSum) / (decimal)s50_54PrevSum) * 100;
                                }
                                s50_54PrevSum = s50_54Sum;

                                if (s55_64PrevSum != 0)
                                {
                                    s55_64PctChange = ((s55_64Sum - s55_64PrevSum) / (decimal)s55_64PrevSum) * 100;
                                }
                                s55_64PrevSum = s55_64Sum;

                                if (s65_PlusPrevSum != 0)
                                {
                                    s65_PlusPctChange = ((s65_PlusSum - s65_PlusPrevSum) / (decimal)s65_PlusPrevSum) * 100;
                                }
                                s65_PlusPrevSum = s65_PlusSum;

                                HighChartDatum s18_24HCD = new HighChartDatum() {
                                    Date = date.ToShortDateString(),
                                    Value = s18_24.name,
                                    SearchName = s18_24.name,
                                    y = request.ChartType == ChartType.Growth ? s18_24PctChange : s18_24Sum
                                };
                                s18_24.data.Add(s18_24HCD);

                                HighChartDatum s25_34HCD = new HighChartDatum() {
                                    Date = date.ToShortDateString(),
                                    Value = s25_34.name,
                                    SearchName = s25_34.name,
                                    y = request.ChartType == ChartType.Growth ? s25_34PctChange : s25_34Sum
                                };
                                s25_34.data.Add(s25_34HCD);

                                HighChartDatum s35_49HCD = new HighChartDatum() {
                                    Date = date.ToShortDateString(),
                                    Value = s35_49.name,
                                    SearchName = s35_49.name,
                                    y = request.ChartType == ChartType.Growth ? s35_49PctChange : s35_49Sum
                                };
                                s35_49.data.Add(s35_49HCD);

                                HighChartDatum s50_54HCD = new HighChartDatum() {
                                    Date = date.ToShortDateString(),
                                    Value = s50_54.name,
                                    SearchName = s50_54.name,
                                    y = request.ChartType == ChartType.Growth ? s50_54PctChange : s50_54Sum
                                };
                                s50_54.data.Add(s50_54HCD);

                                HighChartDatum s55_64HCD = new HighChartDatum() {
                                    Date = date.ToShortDateString(),
                                    Value = s55_64.name,
                                    SearchName = s55_64.name,
                                    y = request.ChartType == ChartType.Growth ? s55_64PctChange : s55_64Sum
                                };
                                s55_64.data.Add(s55_64HCD);

                                HighChartDatum s65_PlusHCD = new HighChartDatum() {
                                    Date = date.ToShortDateString(),
                                    Value = s65_Plus.name,
                                    SearchName = s65_Plus.name,
                                    y = request.ChartType == ChartType.Growth ? s65_PlusPctChange : s65_PlusSum
                                };
                                s65_Plus.data.Add(s65_PlusHCD);
                            }
                        }

                        seriesList.Add(s18_24);
                        lineChart.series.Add(s18_24);

                        seriesList.Add(s25_34);
                        lineChart.series.Add(s25_34);

                        seriesList.Add(s35_49);
                        lineChart.series.Add(s35_49);

                        seriesList.Add(s50_54);
                        lineChart.series.Add(s50_54);

                        seriesList.Add(s55_64);
                        lineChart.series.Add(s55_64);

                        seriesList.Add(s65_Plus);
                        lineChart.series.Add(s65_Plus);
                    }
                }
                else
                {
                    int count = 0;
                    foreach (var group in groupings.OrderByDescending(ob => ob.Summaries.Sum(s => s.Number_Of_Hits)))
                    {
                        Series groupSeries = new Series() {
                            data = new List<HighChartDatum>(),
                            name = group.Name,
                            tooltip = new Tooltip() {
                                formatter = ""
                            }
                        };

                        long prevSum = 0;

                        if (request.PageType == "campaign")
                        {
                            DateTime campaignStart = new DateTime();
                            DateTime campaignEnd = new DateTime();
                            if (request.Tab == SecondaryTabID.OverTime)
                            {
                                campaignStart = request.Campaigns.First(c => group.ID.Equals(c.CampaignID.ToString())).StartDate;
                                campaignEnd = request.Campaigns.First(c => group.ID.Equals(c.CampaignID.ToString())).EndDate;
                            }
                            foreach (var step in xAxisValues)
                            {
                                var summariesForOffset = group.Summaries.Where(w => w.CampaignOffset.Equals(Convert.ToInt64(step))).ToList();
                                decimal? offsetSum = summariesForOffset.Any() ? GetSumsFromSummaries(request.PESHTypes, subMediaTypes, PESHFilters, summariesForOffset) : 0;
                                decimal pctChange = 0;
                                if (prevSum != 0)
                                {
                                    pctChange = (((long)offsetSum - prevSum) / (decimal)prevSum) * 100;
                                }
                                prevSum = (long)offsetSum;
                                var offset = Convert.ToInt32(step);

                                var dateString = string.Empty;
                                if (request.Tab == SecondaryTabID.OverTime)
                                {
                                    switch(request.DateInterval)
                                    {
                                        case "hour":
                                            dateString = campaignStart.AddHours(offset).ToShortDateString();
                                            if (campaignStart.AddHours(offset).CompareTo(campaignEnd) > 0)
                                            {
                                                offsetSum = null;
                                            }
                                            break;
                                        case "day":
                                            dateString = campaignStart.AddDays(offset).ToShortDateString();
                                            if (campaignStart.AddDays(offset).CompareTo(campaignEnd) > 0)
                                            {
                                                offsetSum = null;
                                            }
                                            break;
                                        case "month":
                                            dateString = campaignStart.AddMonths(offset).ToShortDateString();
                                            if (campaignStart.AddMonths(offset).CompareTo(campaignEnd) > 0)
                                            {
                                                offsetSum = null;
                                            }
                                            break;
                                    }
                                }

                                // Date is only applicable to add into tooltip when on OverTime tab
                                HighChartDatum hcd = new HighChartDatum() {
                                    Date = dateString,
                                    Value = group.ID,
                                    SearchName = group.Name,
                                    SearchTerm = group.Name,
                                    y = request.ChartType == ChartType.Growth ? pctChange : offsetSum
                                };

                                groupSeries.data.Add(hcd);
                            }
                        }
                        else
                        {
                            foreach (var date in distinctDates)
                            {
                                var summariesForDate = group.Summaries.Where(w => w.SummaryDateTime.Equals(date));

                                long daySum = summariesForDate.Any() ? GetSumsFromSummaries(request.PESHTypes, subMediaTypes, PESHFilters, summariesForDate) : 0;
                                decimal pctChange = 0;
                                if (prevSum != 0)
                                {
                                    pctChange = ((daySum - prevSum) / (decimal)prevSum) * 100;
                                }
                                prevSum = daySum;

                                HighChartDatum hcd = new HighChartDatum() {
                                    Date = date.ToShortDateString(),
                                    Type = "Media",
                                    Value = group.ID,
                                    SearchName = group.Name,
                                    SearchTerm = group.Name,
                                    y = request.ChartType == ChartType.Growth ? pctChange : daySum
                                };

                                groupSeries.data.Add(hcd);
                            }
                        }

                        seriesList.Add(groupSeries);
                        if (request.Tab == SecondaryTabID.OverTime)
                        {
                            // Add only series from agentList
                            if (request.RequestIDs.Any(ID => ID.ToString().Equals(group.ID)))
                            {
                                lineChart.series.Add(groupSeries);
                            }
                        }
                        else
                        {
                            if (count < 10)
                            {
                                lineChart.series.Add(groupSeries);
                            }
                            count += 1;
                        }
                    }
                }

                Dictionary<string, object> seriesAndChart = new Dictionary<string,object>();
                seriesAndChart.Add("series", seriesList);
                seriesAndChart.Add("chart", CommonFunctions.SearializeJson(lineChart));
                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateLineChart: {0} ms", sw.ElapsedMilliseconds));
                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        private Dictionary<string, object> CreatePieChart(List<AnalyticsGrouping> groupings, AnalyticsRequest request, List<IQ_MediaTypeModel> subMediaTypes, AnalyticsPESHFilters PESHFilters)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Log4NetLogger.Debug("CreatePieChart");
                //Log4NetLogger.Debug(string.Format("groupings.Count: {0}", groupings.Count));
                // Series list holds all series
                List<object> seriesList = new List<object>();
                List<object> sliceList = new List<object>();

                HighPieChartModel pieChart = new HighPieChartModel() {
                    chart = new PChart(),
                    title = new PTitle() {
                        text =  request.Tab == SecondaryTabID.Demographic ? "Audience" : "Occurrences",
                        style = new HStyle() {
                            color = "null",
                            fontFamily = "Open Sans",
                            fontSize = "null",
                            fontWeight = "null"
                        }
                    },
                    plotOptions = new PPlotOptions() {
                        pie = new Pie() {
                            allowPointSelect = true,
                            cursor = "pointer",
                            showInLegend = true,
                            innerSize = "60%",
                            dataLabels = new DataLabels() {
                                enabled = false
                            }
                        }
                    },
                    legend = new Legend() {
                        align = "center",
                        borderWidth = "0",
                        width = 500,
                        enabled = false
                    },
                    tooltip = new PTooltip(),
                    series = new List<PSeries>()
                };

                if (request.Tab == SecondaryTabID.Demographic)
                {
                    List<AnalyticsSummaryModel> allSummaries = new List<AnalyticsSummaryModel>();
                    groupings.ForEach(e => {
                        allSummaries.AddRange(GetSummariesForSources(subMediaTypes, PESHFilters, e.Summaries));
                    });

                    if (request.SubTab == "gender")
                    {
                        var maleSeries = new object[] {
                            "male",
                            Convert.ToInt64(allSummaries.Sum(s => s.MaleAudience)),
                            "male"
                        };
                        seriesList.Add(maleSeries);
                        sliceList.Add(maleSeries);

                        var femaleSeries = new object[] {
                            "female",
                            Convert.ToInt64(allSummaries.Sum(s => s.FemaleAudience)),
                            "female"
                        };
                        seriesList.Add(femaleSeries);
                        sliceList.Add(femaleSeries);
                    }
                    else
                    {
                        var s18_24 = new object[] {
                            "18-24",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24)),
                            "18-24"
                        };
                        seriesList.Add(s18_24);
                        sliceList.Add(s18_24);

                        var s25_34 = new object[] {
                            "25-34",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM25_34 + s.AF25_34)),
                            "25-34"
                        };
                        seriesList.Add(s25_34);
                        sliceList.Add(s25_34);

                        var s35_49 = new object[] {
                            "35-49",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM35_49 + s.AF35_49)),
                            "35-49"
                        };
                        seriesList.Add(s35_49);
                        sliceList.Add(s35_49);

                        var s50_54 = new object[] {
                            "50-54",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM50_54 + s.AF50_54)),
                            "50-54"
                        };
                        seriesList.Add(s50_54);
                        sliceList.Add(s50_54);

                        var s55_64 = new object[] {
                            "55-64",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM55_64 + s.AF55_64)),
                            "55-64"
                        };
                        seriesList.Add(s55_64);
                        sliceList.Add(s55_64);

                        var s65_Plus = new object[] {
                            "65+",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM65_Plus + s.AF65_Plus)),
                            "65+"
                        };
                        seriesList.Add(s65_Plus);
                        sliceList.Add(s65_Plus);
                    }
                }
                else
                {
                    int count = 0;
                    foreach (var group in groupings.OrderByDescending(ob => ob.Summaries.Sum(s => s.Number_Of_Hits)))
                    {
                        var slice = new object[] {
                            group.Name,
                            group.Summaries.Any() ? GetSumsFromSummaries(request.PESHTypes, subMediaTypes, PESHFilters, group.Summaries) : 0,
                            group.ID
                        };

                        seriesList.Add(slice);

                        if (request.Tab == SecondaryTabID.OverTime)
                        {
                            if (request.RequestIDs.Any(id => id.ToString().Equals(group.ID)))
                            {
                                sliceList.Add(slice);
                            }
                        }
                        else
                        {
                            if (count < 10)
                            {
                                sliceList.Add(slice);
                            }
                            count += 1;
                        }
                    }
                }

                pieChart.series.Add(new PSeries() {
                    type = "pie",
                    name = request.Tab == SecondaryTabID.Demographic ? "Audience" : "Occurrences",
                    data = sliceList
                });

                Dictionary<string, object> seriesAndChart = new Dictionary<string, object>();
                seriesAndChart.Add("series", seriesList);
                seriesAndChart.Add("chart", CommonFunctions.SearializeJson(pieChart));

                sw.Stop();
                Log4NetLogger.Debug(string.Format("CreatePieChart: {0} ms", sw.ElapsedMilliseconds));

                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        private Dictionary<string, object> CreateHeatMap(List<AnalyticsGrouping> groupings, AnalyticsRequest request, List<IQ_MediaTypeModel> subMediaTypes, AnalyticsPESHFilters PESHFilters)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Log4NetLogger.Debug("CreateHeatMap");

                List<DayOfWeek> daysUnordered = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();
                var days = daysUnordered.OrderBy(d => d == DayOfWeek.Sunday).ThenBy(d => d).Reverse().ToList();
                List<int> hours = Enumerable.Range(0, 24).ToList();
                List<string> axisLabels = new List<string>();
                for (int x = 0; x <= 7; x++)
                {
                    if (x < 2)
                    {
                        axisLabels.Add(days[x].ToString());
                    }
                    else if (x == 2)
                    {
                        axisLabels.Add("");
                    }
                    else
                    {
                        axisLabels.Add(days[x-1].ToString());
                    }
                }

                HighHeatMapModel heatMap = new HighHeatMapModel() {
                    hChart = new HChart() {
                        type = "heatmap"
                    },
                    title = new Title() {
                        text = "Occurrences"
                    },
                    legend = new HeatMapLegend() {
                        align = "right",
                        verticalAlign = "top",
                        layout = "vertical",
                        y = 25,
                        symbolHeight = 300
                    },
                    xAxis = new XAxis() {
                        categories = hours.Select(s => (s > 12 ? s - 12 : (s == 0 ? 12 : s)).ToString("D2") + ":00 " + (s >= 12 ? "PM" : "AM")).ToList(),
                        tickmarkPlacement = "between",
                        tickWidth = 1,
                        labels = new labels() {
                            rotation = 315
                        }
                    },
                    yAxis = new HeatMapYAxis() {
                        title = new Title2() {
                            text = null
                        },
                        categories = request.Tab == SecondaryTabID.Daytime ? days.Select(s => s.ToString()).ToList() : axisLabels
                    },
                    tooltip = new Tooltip() {
                        formatter = request.Tab == SecondaryTabID.Daytime ? "FormatDaytimeTooltip" : "FormatDaypartTooltip"
                    },
                    colorAxis = new ColorAxis() {
                        min = 0,
                        minColor = "#ffffff",
                        maxColor = "#598ea2"
                    }
                };

                HeatMapSeries series = new HeatMapSeries() {
                    name = "Occurrences",
                    data = new List<HeatMapDatum>()
                };

                if (request.Tab == SecondaryTabID.Daytime)
                {
                    foreach (var group in groupings.OrderByDescending(ob => ob.Summaries.Sum(s => s.Number_Of_Hits)))
                    {
                        // Daytime will have both day and hour in its ID
                        var yIndex = days.FindIndex(i => group.ID.Split('_').ElementAt(0).Equals(i.ToString()));
                        var xIndex = hours.FindIndex(i => group.ID.Split('_').ElementAt(1).Equals(i.ToString()));

                        HeatMapDatum hcd = new HeatMapDatum() {
                            value = group.Summaries.Any() ? GetSumsFromSummaries(request.PESHTypes, subMediaTypes, PESHFilters, group.Summaries) : 0,
                            borderColor = "#cccccc",
                            borderWidth = 1,
                            x = xIndex,
                            y = yIndex,
                            name = group.Name,
                            code = group.ID
                        };

                        series.data.Add(hcd);
                    }
                }
                else
                {
                    series.dataLabels = new HeatMapDataLabels() {
                        enabled = true,
                        color = "#636F72",
                        formatter = "FormatDaypartDataLabel",
                        style = new HeatStyle() {
                            fontSize = "12px",
                            fontWeight = "normal",
                            textShadow = "0 0 10px white"
                        }
                    };

                    //Get Day Part Data
                    List<DayPartDataItem> dayPartData = GetDayPartData("A");

                    List<AnalyticsSummaryModel> allSummaries = new List<AnalyticsSummaryModel>();
                    groupings.ForEach(e => {
                        allSummaries.AddRange(e.Summaries);
                    });

                    foreach (var hour in hours)
                    {
                        int count = 0;

                        foreach (var day in days)
                        {
                            if (count == 2)
                            {
                                count += 1;
                            }

                            var summsForPart = allSummaries.Where(w => w.SummaryDateTime.DayOfWeek.Equals(day) && w.SummaryDateTime.Hour.Equals(hour));
                            DayPartDataItem dayPart = dayPartData.Any() ? dayPartData.First(dp => dp.DayOfWeek.Equals(day) && dp.HourOfDay.Equals(hour)) : new DayPartDataItem();

                            HeatMapDatum hcd = new HeatMapDatum() {
                                value = summsForPart.Any() ? GetSumsFromSummaries(request.PESHTypes, subMediaTypes, PESHFilters, summsForPart) : 0,
                                x = hours.IndexOf(hour),
                                y = count,
                                name = dayPart.DayPartName,
                                code = dayPart.DayPartCode
                            };

                            series.data.Add(hcd);
                            count += 1;
                        }

                        series.data.Insert(2, new HeatMapDatum() {
                            x = hours.IndexOf(hour),
                            y = 2,
                            value = 0,
                            name = string.Empty,
                            code = string.Empty
                        });
                    }
                }

                //Log4NetLogger.Debug(string.Format(" {0,-12} | {1,12} ", "ID", "Sum"));
                //Log4NetLogger.Debug(string.Format("--------------+--------------"));
                // Heat maps do not have toggle-able series so no need to limit # of "series"


                heatMap.series = new List<HeatMapSeries>() { series };
                Dictionary<string, object> seriesAndChart = new Dictionary<string, object>();
                seriesAndChart.Add("series", series.data);
                seriesAndChart.Add("chart", CommonFunctions.SearializeJson(heatMap));

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateHeatMap: {0} ms", sw.ElapsedMilliseconds));

                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        public string CreateGoogleOverlay(List<GoogleSummaryModel> googleSummaries, AnalyticsRequest request)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug("CreateGoogleOverlay");

                List<DateTime> distinctDates = new List<DateTime>();
                List<string> xAxisValues = GetChartXAxisValues(request, out distinctDates);

                HighLineChartOutput chartOutput = new HighLineChartOutput() {
                    xAxis = new XAxis() {
                        tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDates.Count()) / 7)),
                        categories = xAxisValues,
                        labels = new labels() {
                            enabled = false
                        }
                    },
                    hChart = new HChart() {
                        height = 100,
                        width = 120,
                        type = "spline"
                    },
                    tooltip = new Tooltip() {
                        formatter = ""
                    },
                    plotOption = new PlotOptions() {
                        spline = new PlotSeries() {
                            marker = new PlotMarker() {
                                enabled = false,
                                lineWidth = 0
                            }
                        }
                    },
                    series = new List<Series>()
                };

                List<string> dataTypes = googleSummaries.Select(s => s.DataType).Distinct().ToList();

                foreach (var dataType in dataTypes)
                {
                    Series dataTypeSeries = new Series() {
                        name = dataType,
                        data = new List<HighChartDatum>()
                    };

                    decimal prevDaySum = 0;
                    foreach (var date in distinctDates)
                    {
                        var summariesForDate = googleSummaries.Where(w => w.DataType.Equals(dataType) && w.DayDate.Equals(date));
                        decimal daySum = summariesForDate.Any() ? summariesForDate.Sum(s => s.NoOfDocs) : 0;
                        decimal pctChange = 0;

                        if (prevDaySum != 0)
                        {
                            pctChange = ((daySum - prevDaySum) / prevDaySum) * 100;
                        }
                        prevDaySum = daySum;

                        HighChartDatum hcd = new HighChartDatum() {
                            Date = date.ToShortDateString(),
                            SearchName = dataType,
                            y = request.ChartType == ChartType.Growth ? pctChange : daySum
                        };

                        dataTypeSeries.data.Add(hcd);
                    }

                    chartOutput.series.Add(dataTypeSeries);
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateGoogleOverlay: {0}", sw.ElapsedMilliseconds));

                return CommonFunctions.SearializeJson(chartOutput);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return string.Empty;
            }
        }

        public string CreateOverlay(List<AnalyticsGrouping> groupings, AnalyticsRequest request, int overlayType, List<IQ_MediaTypeModel> subMediaTypes)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug(string.Format("CreateOverlay"));

                List<DateTime> distinctDates = new List<DateTime>();
                List<string> xAxisValues = GetChartXAxisValues(request, out distinctDates);
                AnalyticsPESHFilters PESHFilters = GetPESHFilters(request.PESHTypes, request.SourceGroups);

                HighLineChartOutput chartOutput = new HighLineChartOutput() {
                    xAxis = new XAxis() {
                        tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDates.Count()) / 7)),
                        categories = xAxisValues,
                        labels = new labels() {
                            enabled = false
                        }
                    },
                    hChart = new HChart() {
                        height = 100,
                        width = 120,
                        type = "spline"
                    },
                    tooltip = new Tooltip() {
                        formatter = "FormatSplineTooltip"
                    },
                    plotOption = new PlotOptions() {
                        spline = new PlotSeries() {
                            marker = new PlotMarker() {
                                enabled = false,
                                lineWidth = 0
                            }
                        }
                    },
                    series = new List<Series>()
                };

                foreach (var group in groupings.OrderByDescending(ob => ob.Summaries.Sum(s => s.Number_Of_Hits)))
                {
                    Series groupSeries = new Series() {
                        name = group.Name + (overlayType == 2 ? " (Audience)" : " (Ad Value)"),
                        data = new List<HighChartDatum>()
                    };

                    decimal prevSum = 0;

                    if (request.PageType == "campaign")
                    {
                        DateTime campaignStart = new DateTime();
                        DateTime campaignEnd = new DateTime();
                        if (request.Tab == SecondaryTabID.OverTime)
                        {
                            campaignStart = request.Campaigns.First(c => group.ID.Equals(c.CampaignID.ToString())).StartDate;
                            campaignEnd = request.Campaigns.First(c => group.ID.Equals(c.CampaignID.ToString())).EndDate;
                        }
                        foreach (var step in xAxisValues)
                        {
                            var summariesForOffset = group.Summaries.Where(w => w.CampaignOffset.Equals(Convert.ToInt64(step))).ToList();
                            if (overlayType == 2)
                            {
                                summariesForOffset = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForOffset);
                            }
                            decimal? offsetSum = summariesForOffset.Any() ? (overlayType == 2 ? summariesForOffset.Sum(s => s.Audience) : summariesForOffset.Sum(s => s.IQMediaValue)) : 0;

                            decimal pctChange = 0;
                            if (prevSum != 0)
                            {
                                pctChange = (((long)offsetSum - prevSum) / (decimal)prevSum) * 100;
                            }
                            prevSum = (long)offsetSum;
                            var offset = Convert.ToInt32(step);

                            var dateString = string.Empty;
                            if (request.Tab == SecondaryTabID.OverTime)
                            {
                                switch(request.DateInterval)
                                {
                                    case "hour":
                                        dateString = campaignStart.AddHours(offset).ToShortDateString();
                                        if (campaignStart.AddHours(offset).CompareTo(campaignEnd) > 0)
                                        {
                                            offsetSum = null;
                                        }
                                        break;
                                    case "day":
                                        dateString = campaignStart.AddDays(offset).ToShortDateString();
                                        if (campaignStart.AddDays(offset).CompareTo(campaignEnd) > 0)
                                        {
                                            offsetSum = null;
                                        }
                                        break;
                                    case "month":
                                        dateString = campaignStart.AddMonths(offset).ToShortDateString();
                                        if (campaignStart.AddMonths(offset).CompareTo(campaignEnd) > 0)
                                        {
                                            offsetSum = null;
                                        }
                                        break;
                                }
                            }

                            HighChartDatum hcd = new HighChartDatum() {
                                Date = dateString,
                                Value = group.ID,
                                SearchName = group.Name,
                                y = offsetSum
                            };

                            groupSeries.data.Add(hcd);
                        }
                    }
                    else
                    {
                        foreach (var date in distinctDates)
                        {
                            var summariesForDate = group.Summaries.Where(w => w.SummaryDateTime.Equals(date));
                            if (overlayType == 2)
                            {
                                summariesForDate = GetSummariesForSources(subMediaTypes, PESHFilters, summariesForDate);
                            }
                            decimal daySum = summariesForDate.Any() ? (overlayType == 2 ? summariesForDate.Sum(s => s.Audience) : summariesForDate.Sum(s => s.IQMediaValue)) : 0;
                            decimal pctChange = 0;

                            if (prevSum != 0)
                            {
                                pctChange = ((daySum - prevSum) / prevSum) * 100;
                            }
                            prevSum = daySum;

                            HighChartDatum hcd = new HighChartDatum() {
                                Date = date.ToShortDateString(),
                                Value = group.ID,
                                SearchName = group.Name,
                                //y = request.ChartType == ChartType.Growth ? pctChange : daySum    // Keep for if want overlay on growth to show growth
                                y = daySum
                            };

                            groupSeries.data.Add(hcd);
                        }
                    }

                    chartOutput.series.Add(groupSeries);
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateOverlay: {0}", sw.ElapsedMilliseconds));

                return CommonFunctions.SearializeJson(chartOutput);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return string.Empty;
            }
        }

        public string CreateFusionMap(AnalyticsRequest request, AnalyticsDataModel analyticsData)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //Log4NetLogger.Debug(string.Format("CreateFusionMap"));

                List<string> colors = new List<string>() {
                    "A0D6FC",
                    "83C9FC",
                    "5DBBFE",
                    "3FAEFD",
                    "0395FE"
                };

                FusionMapOutput fusionMap = new FusionMapOutput() {
                    map = new FusionMap() {
                        animation = "0",
                        showbevel = "1",
                        usehovercolor = "1",
                        canvasbordercolor = "FFFFFF",
                        bordercolor = "B7B7B7",
                        showlegend = "1",
                        showshadow = "0",
                        legendposition = "BOTTOM",
                        legendborderalpha = 1,
                        legendbordercolor = "FFFFFF",
                        legendallowdrag = "0",
                        legendshadow = "1",
                        connectorcolor = "000000",
                        fillalpha = "80",
                        hovercolor = "CCCCCC",
                        showEntityToolTip = "1",
                        showToolTip = "0"
                    }
                };

                // Set legend color ranges
                FusionMapColorRange mapColorRange = new FusionMapColorRange() {
                    color = new List<FusionMapColor>()
                };

                long minValue = 1000000;
                long maxValue = 0;

                // Set map data
                List<FusionMapData> mapData = new List<FusionMapData>();
                if (request.ChartType == ChartType.US)
                {
                    foreach (var keyVal in IQDmaToFusionIDMapModel.IQDmaToFusionIDMap)
                    {
                        long mention = analyticsData.DmaMentionMapList.Where(w => w.DMAName == keyVal.Key).Sum(s => s.NumberOfHits);

                        FusionMapData mapDatum = new FusionMapData() {
                            id = keyVal.Value.ToString(),
                            value = mention.ToString(),
                            tooltext = string.Format("DMA Area : {0}{1}Mention: {2:N0}", keyVal.Key, "{br}", mention),
                            showEntityToolTip = "1"
                        };

                        if (string.Compare("Honolulu", keyVal.Key, true) == 0 || string.Compare("Anchorage", keyVal.Key, true) == 0 || string.Compare("Juneau", keyVal.Key, true) == 0 || string.Compare("Fairbanks", keyVal.Key, true) == 0)
                        {
                            mapDatum.showlabel = "1";
                        }
                        else
                        {
                            mapDatum.showlabel = "0";
                        }

                        if (mention > maxValue)
                        {
                            maxValue = mention;
                        }

                        if (mention < minValue)
                        {
                            minValue = mention;
                        }

                        mapData.Add(mapDatum);
                    }
                }
                else
                {
                    foreach (var keyVal in IQProvinceToFusionIDMapModel.IQProvinceToFusionIDMap)
                    {
                        long mention = analyticsData.CanadaMentionMapList.Where(w => w.DMAName == keyVal.Key).Sum(s => s.NumberOfHits);
                        FusionMapData mapDatum = new FusionMapData() {
                            id = keyVal.Value,
                            value = mention.ToString(),
                            tooltext = string.Format("Province : {0}{1}Mention: {2:N0}", keyVal.Key, "{br}", mention),
                            showEntityToolTip = "1",
                            showlabel = "0"
                        };

                        if (mention > maxValue)
                        {
                            maxValue = mention;
                        }

                        if (mention < minValue)
                        {
                            minValue = mention;
                        }

                        mapData.Add(mapDatum);
                    }
                }

                long colorStep = (maxValue - minValue) / 5;
                if (colorStep > 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        FusionMapColor mapColor = new FusionMapColor();
                        if (i == 0)
                        {
                            mapColor.minvalue = minValue.ToString();
                            mapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                        }
                        else if (i == 4)
                        {
                            mapColor.minvalue = (colorStep * i).ToString();
                            mapColor.maxvalue = maxValue.ToString();
                        }
                        else
                        {
                            mapColor.minvalue = (colorStep * i).ToString();
                            mapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                        }

                        mapColor.code = colors[i];
                        mapColor.displayvalue = mapColor.maxvalue;
                        mapColorRange.color.Add(mapColor);
                    }
                }
                else
                {
                    if (maxValue == 0)
                    {
                        maxValue = 1;
                    }

                    if (minValue == maxValue)
                    {
                        minValue = minValue - 1;
                    }

                    FusionMapColor mapColor = new FusionMapColor() {
                        minvalue = minValue.ToString(),
                        maxvalue = maxValue.ToString(),
                        code = "C3EBFD",
                        displayvalue = maxValue.ToString()
                    };
                    mapColorRange.color.Add(mapColor);
                }

                fusionMap.colorrange = mapColorRange;
                fusionMap.data = mapData;

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("CreateFusionMap: {0} ms", sw.ElapsedMilliseconds));
                return CommonFunctions.SearializeJson(fusionMap);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return string.Empty;
            }
        }

        #endregion

        #region Solr

        //public List<FacetResponse> Search(List<string> SRIDs, DateTime dateFrom, DateTime dateTo)
        //{
        //    try
        //    {
        //        System.Uri searchRequestUrl = new Uri("http://10.100.1.41:8080/solr/cfe-2016-1/select?");
        //        SearchEngine searchEngine = new SearchEngine(searchRequestUrl);
        //        SearchRequest request = new SearchRequest() {
        //            SearchRequestIDs = SRIDs,
        //            FromDate = dateFrom,
        //            ToDate = dateTo
        //        };

        //        //Dictionary<string, string> searchResult = searchEngine.Search(request);

        //        return searchEngine.Search(request);
        //    }
        //    catch (Exception exc)
        //    {
        //        Log4NetLogger.Error(exc);
        //        return new List<FacetResponse>();
        //    }
        //}

        #endregion

    }
}

