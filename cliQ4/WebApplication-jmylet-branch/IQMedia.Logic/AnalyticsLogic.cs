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

        public AnalyticsDataModel GetHourSummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, bool loadEverything = true, string GroupByHeader = "")
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            AnalyticsDataModel dataModel = analyticsDA.GetHourSummaryData(clientGUID, searchRequestXml, subMediaType, loadEverything, GroupByHeader);
            return dataModel;
        }

        public AnalyticsDataModel GetCampaignHourSummaryData(string searchRequestXml, string subMediaType, bool loadEverything = true, string GroupByHeader = "")
        {
            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetHourSummaryDataForCampaign(searchRequestXml, subMediaType, loadEverything, GroupByHeader);
        }

        public AnalyticsDataModel GetCampaignDaySummaryData(string searchRequestXml, string subMediaType, bool loadEverything = true)
        {
            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetDaySummaryDataForCampaign(searchRequestXml, subMediaType, loadEverything);
        }

        public AnalyticsDataModel GetDaySummaryData(Guid clientGUID, string searchRequestXml, string subMediaType, bool loadEverything = true)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            AnalyticsDataModel dataModel = analyticsDA.GetDaySummaryData(clientGUID, searchRequestXml, subMediaType, loadEverything);
            return dataModel;
        }

        public AnalyticsDataModel GetMonthSummaryData(Guid clientGUID, string searchRequestXml, string subMediaType)
        {
            AnalyticsDA analyticsDA = new AnalyticsDA();

            AnalyticsDataModel dataModel = analyticsDA.GetMonthSummaryData(clientGUID, searchRequestXml, subMediaType);
            return dataModel;
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

        public AnalyticsDataModel GetDaySummaryDataForCampaign(List<string> campaignIDs, string subMediaType)
        {
            string campaignIDXml = null;
            if (campaignIDs != null && campaignIDs.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in campaignIDs
                    select new XElement(
                        "item",
                        new XAttribute("id", i)
                    )
                ));
                campaignIDXml = doc.ToString();
            }

            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetDaySummaryDataForCampaign(campaignIDXml, subMediaType);
        }

        public AnalyticsDataModel GetHourSummaryDataForCampaign(List<string> campaignIDs, string subMediaType)
        {
            string campaignIDXml = null;
            if (campaignIDs != null && campaignIDs.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in campaignIDs
                    select new XElement(
                        "item",
                        new XAttribute("id", i)
                    )
                ));
                campaignIDXml = doc.ToString();
            }

            AnalyticsDA analyticsDA = (AnalyticsDA)DataAccessFactory.GetDataAccess(DataAccessType.Analytics);
            return analyticsDA.GetHourSummaryDataForCampaign(campaignIDXml, subMediaType);
        }

        public string GetFusionUsaDmaMap(AnalyticsGraphRequest graphRequest, List<AnalyticsMapSummaryModel> dmaMentionMapList)
        {
            try
            {
                FusionMapOutput fusionMapOutput = new FusionMapOutput();
                List<string> colors = new List<string> { "A0D6FC", "83C9FC", "5DBBFE", "3FAEFD", "0395FE" };

                // set all map display properties
                FusionMap fusionMap = new FusionMap();
                fusionMap.animation = "0";
                fusionMap.showbevel = "1";
                fusionMap.usehovercolor = "1";
                fusionMap.canvasbordercolor = "FFFFFF";
                fusionMap.bordercolor = "B7B7B7";
                fusionMap.showlegend = "1";
                fusionMap.showshadow = "0";
                fusionMap.legendposition = "BOTTOM";
                fusionMap.legendborderalpha = 1;
                fusionMap.legendbordercolor = "ffffff";
                fusionMap.legendallowdrag = "0";
                fusionMap.legendshadow = "1";
                fusionMap.connectorcolor = "000000";
                fusionMap.fillalpha = "80";
                fusionMap.hovercolor = "CCCCCC";
                fusionMap.showEntityToolTip = "1";
                fusionMap.showToolTip = "0";

                // set legend color ranges 
                FusionMapColorRange fusionMapColorRange = new FusionMapColorRange();
                fusionMapColorRange.color = new List<FusionMapColor>();

                long minValue = 1000000;
                long maxValue = 0;

                // set map data 
                List<FusionMapData> lstFusionMapData = new List<FusionMapData>();
                foreach (KeyValuePair<string, short> keyval in IQDmaToFusionIDMapModel.IQDmaToFusionIDMap)
                {
                    FusionMapData fusionMapData = new FusionMapData();
                    long mention = dmaMentionMapList.Where(w => w.DMAName == keyval.Key).Sum(s => s.NumberOfHits);

                    fusionMapData.id = keyval.Value.ToString();
                    fusionMapData.value = mention.ToString();
                    fusionMapData.tooltext = "DMA Area : " + keyval.Key + "{br}Mention:" + mention.ToString("N0");
                    fusionMapData.showEntityToolTip = "1";
                    if (string.Compare("Honolulu", keyval.Key, true) == 0 || string.Compare("Anchorage", keyval.Key, true) == 0 || string.Compare("Juneau", keyval.Key, true) == 0 || string.Compare("Fairbanks", keyval.Key, true) == 0)
                    {
                        fusionMapData.showlabel = "1";
                    }
                    else
                    {
                        fusionMapData.showlabel = "0";
                    }

                    if (mention > maxValue)
                    {
                        maxValue = mention;
                    }

                    if (mention < minValue)
                    {
                        minValue = mention;
                    }

                    lstFusionMapData.Add(fusionMapData);
                }

                long colorStep = (maxValue - minValue) / 5;
                if (colorStep > 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        FusionMapColor fusionMapColor = new FusionMapColor();
                        if (i == 0)
                        {
                            fusionMapColor.minvalue = (minValue).ToString();
                            fusionMapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                        }
                        else if (i == 4)
                        {
                            fusionMapColor.minvalue = (colorStep * i).ToString();
                            fusionMapColor.maxvalue = maxValue.ToString();
                        }
                        else
                        {
                            fusionMapColor.minvalue = (colorStep * i).ToString();
                            fusionMapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                        }

                        fusionMapColor.code = colors[i];
                        fusionMapColor.displayvalue = fusionMapColor.maxvalue;
                        fusionMapColorRange.color.Add(fusionMapColor);
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

                    FusionMapColor fusionMapColor = new FusionMapColor();
                    fusionMapColor.minvalue = minValue.ToString();
                    fusionMapColor.maxvalue = maxValue.ToString();
                    fusionMapColor.code = "C3EBFD";
                    fusionMapColor.displayvalue = fusionMapColor.maxvalue;
                    fusionMapColorRange.color.Add(fusionMapColor);
                }

                fusionMapOutput.map = fusionMap;
                fusionMapOutput.colorrange = fusionMapColorRange;
                fusionMapOutput.data = lstFusionMapData;

                return CommonFunctions.SearializeJson(fusionMapOutput);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public string GetFusionCanadaProvinceMap(AnalyticsGraphRequest graphRequest, List<AnalyticsMapSummaryModel> provinceMentionMapList)
        {
            try
            {
                FusionMapOutput fusionMapOutput = new FusionMapOutput();
                List<string> colors = new List<string> { "A0D6FC", "83C9FC", "5DBBFE", "3FAEFD", "0395FE" };

                // set all map display properties
                FusionMap fusionMap = new FusionMap();
                fusionMap.animation = "0";
                fusionMap.showbevel = "1";
                fusionMap.usehovercolor = "1";
                fusionMap.canvasbordercolor = "FFFFFF";
                fusionMap.bordercolor = "B7B7B7";
                fusionMap.showlegend = "1";
                fusionMap.showshadow = "0";
                fusionMap.legendposition = "BOTTOM";
                fusionMap.legendborderalpha = 1;
                fusionMap.legendbordercolor = "ffffff";
                fusionMap.legendallowdrag = "0";
                fusionMap.legendshadow = "1";
                fusionMap.connectorcolor = "000000";
                fusionMap.fillalpha = "80";
                fusionMap.hovercolor = "CCCCCC";
                fusionMap.showEntityToolTip = "1";
                fusionMap.showToolTip = "0";

                // set legend color ranges 
                FusionMapColorRange fusionMapColorRange = new FusionMapColorRange();
                fusionMapColorRange.color = new List<FusionMapColor>();

                long minValue = 1000000;
                long maxValue = 0;

                // set map data 
                List<FusionMapData> lstFusionMapData = new List<FusionMapData>();
                foreach (KeyValuePair<string, string> keyval in IQProvinceToFusionIDMapModel.IQProvinceToFusionIDMap)
                {
                    FusionMapData fusionMapData = new FusionMapData();
                    long mention = provinceMentionMapList.Where(w => w.DMAName == keyval.Key).Sum(s => s.NumberOfHits);

                    fusionMapData.id = keyval.Value.ToString();
                    fusionMapData.value = mention.ToString();
                    fusionMapData.tooltext = "Province : " + keyval.Key + "{br}Mention:" + mention.ToString("N0");
                    fusionMapData.showEntityToolTip = "1";
                    fusionMapData.showlabel = "0";

                    if (mention > maxValue)
                    {
                        maxValue = mention;
                    }

                    if (mention < minValue)
                    {
                        minValue = mention;
                    }

                    lstFusionMapData.Add(fusionMapData);
                }

                long colorStep = (maxValue - minValue) / 5;
                if (colorStep > 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        FusionMapColor fusionMapColor = new FusionMapColor();
                        if (i == 0)
                        {
                            fusionMapColor.minvalue = (minValue).ToString();
                            fusionMapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                        }
                        else if (i == 4)
                        {
                            fusionMapColor.minvalue = (colorStep * i).ToString();
                            fusionMapColor.maxvalue = maxValue.ToString();
                        }
                        else
                        {
                            fusionMapColor.minvalue = (colorStep * i).ToString();
                            fusionMapColor.maxvalue = ((colorStep * (i + 1)) - 1).ToString();
                        }

                        fusionMapColor.code = colors[i];
                        fusionMapColor.displayvalue = fusionMapColor.maxvalue;
                        fusionMapColorRange.color.Add(fusionMapColor);
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

                    FusionMapColor fusionMapColor = new FusionMapColor();
                    fusionMapColor.minvalue = minValue.ToString();
                    fusionMapColor.maxvalue = maxValue.ToString();
                    fusionMapColor.code = "C3EBFD";
                    fusionMapColor.displayvalue = fusionMapColor.maxvalue;
                    fusionMapColorRange.color.Add(fusionMapColor);
                }

                fusionMapOutput.map = fusionMap;
                fusionMapOutput.colorrange = fusionMapColorRange;
                fusionMapOutput.data = lstFusionMapData;

                return CommonFunctions.SearializeJson(fusionMapOutput);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public string GetHighChartSeriesForMediaValue(List<AnalyticsSummaryModel> lstSummaryModel, AnalyticsGraphRequest graphRequest)
        {
            try
            {
                List<DateTime> distinctDate = new List<DateTime>();
                List<string> xAxisValues = GetChartXAxisValues(graphRequest, out distinctDate);

                // The chart will never be displayed, but it has to be created properly in order for it's data to be added to the main chart
                HighLineChartOutput mediaValueChart = new HighLineChartOutput();

                mediaValueChart.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                mediaValueChart.hChart = new HChart() {
                    height = 100,
                    width = 120,
                    type = "spline"
                };
                mediaValueChart.tooltip = new Tooltip() {
                    formatter = graphRequest.GraphType == "comparison" ? "FormatComparisonTooltip" : "" // Set here for reference, must also be set in JS
                };
                mediaValueChart.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                Dictionary<long, string> dictAgentNames = lstSummaryModel.Select(s => new { 
                    s.SearchRequestID, 
                    s.Query_Name 
                }).Distinct().ToDictionary(
                    t => t.SearchRequestID, 
                    t => t.Query_Name
                );
                List<Series> lstSeries = new List<Series>();


                foreach (AnalyticsAgentRequest agentRequest in graphRequest.AgentList)
                {
                    // set series name
                    Series series = new Series();
                    series.name = dictAgentNames[agentRequest.ID] + " (Ad Value)";
                    series.data = new List<HighChartDatum>();

                    if (graphRequest.GraphType == "comparison")
                    {
                        distinctDate = new List<DateTime>();
                        TimeSpan agentTS = agentRequest.DateTo.Subtract(agentRequest.DateFrom);

                        switch (graphRequest.DateInterval)
                        {
                            case "hour":
                                for (int i = 0; i <= agentTS.TotalHours; i++)
                                {
                                    distinctDate.Add(agentRequest.DateFrom.AddHours(i));
                                }
                                break;
                            case "day":
                                for (int i = 0; i <= agentTS.Days; i++)
                                {
                                    distinctDate.Add(agentRequest.DateFrom.AddDays(i));
                                }
                                break;
                            case "month":
                                var months = ((agentRequest.DateTo.Year - agentRequest.DateFrom.Year) * 12) + agentRequest.DateTo.Month - agentRequest.DateFrom.Month;
                                for (int i = 0; i <= months; i++)
                                {
                                    distinctDate.Add(agentRequest.DateFrom.AddMonths(i));
                                }
                                break;
                        }
                    }

                    // set list of data for series 
                    foreach (var rDate in distinctDate)
                    {
                        decimal sumValue = 0;
                        switch (graphRequest.DateInterval)
                        {
                            case "hour":
                                sumValue = lstSummaryModel.Where(d => 
                                    d.SummaryDateTime.Date.Equals(rDate.Date) && 
                                    d.SummaryDateTime.Hour.Equals(rDate.Hour) && 
                                    d.SearchRequestID == agentRequest.ID
                                ).Sum(s => s.IQMediaValue);
                                break;
                            case "day":
                                sumValue = lstSummaryModel.Where(d => 
                                    d.SummaryDateTime.Date.Equals(rDate.Date) &&
                                    d.SearchRequestID == agentRequest.ID
                                ).Sum(s => s.IQMediaValue);
                                break;
                            case "month":
                                sumValue = lstSummaryModel.Where(d => 
                                    d.SummaryDateTime.Month == rDate.Month && 
                                    d.SummaryDateTime.Year == rDate.Year
                                    && d.SearchRequestID == agentRequest.ID
                                ).Sum(s => s.IQMediaValue);
                                break;
                        }

                        HighChartDatum highChartDatum = new Model.HighChartDatum();
                        highChartDatum.y = sumValue;
                        highChartDatum.Value = agentRequest.ID.ToString();
                        highChartDatum.Date = rDate.ToShortDateString();
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

                mediaValueChart.series = lstSeries;

                return CommonFunctions.SearializeJson(mediaValueChart);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public string GetHighChartSeriesForViews(List<AnalyticsSummaryModel> lstSummaryModel, AnalyticsGraphRequest graphRequest)
        {
            try
            {
                List<DateTime> distinctDate = new List<DateTime>();
                List<string> xAxisValues = GetChartXAxisValues(graphRequest, out distinctDate);

                // The chart will never be displayed, but it has to be created properly in order for it's data to be added to the main chart
                HighLineChartOutput viewsChart = new HighLineChartOutput();

                viewsChart.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(xAxisValues.Count()) / 7)),
                    categories = xAxisValues,
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                viewsChart.hChart = new HChart() {
                    height = 100,
                    width = 120,
                    type = "spline"
                };
                viewsChart.tooltip = new Tooltip() {
                    formatter = graphRequest.GraphType == "comparison" ? "FormatComparisonTooltip" : "" // Set here for reference, must also be set in JS
                };
                viewsChart.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                Dictionary<long, string> dictAgentNames = lstSummaryModel.Select(s => new { s.SearchRequestID, s.Query_Name }).Distinct().ToDictionary(t => t.SearchRequestID, t => t.Query_Name);
                List<Series> lstSeries = new List<Series>();

                foreach (AnalyticsAgentRequest agentRequest in graphRequest.AgentList)
                {
                    // set series name
                    Series series = new Series();
                    series.name = dictAgentNames[agentRequest.ID] + " (Audience)";
                    series.data = new List<HighChartDatum>();

                    if (graphRequest.GraphType == "comparison")
                    {
                        distinctDate = new List<DateTime>();
                        TimeSpan agentTS = agentRequest.DateTo.Subtract(agentRequest.DateFrom);

                        switch (graphRequest.DateInterval)
                        {
                            case "hour":
                                for (int i = 0; i <= agentTS.TotalHours; i++)
                                {
                                    distinctDate.Add(agentRequest.DateFrom.AddHours(i));
                                }
                                break;
                            case "day":
                                for (int i = 0; i <= agentTS.Days; i++)
                                {
                                    distinctDate.Add(agentRequest.DateFrom.AddDays(i));
                                }
                                break;
                            case "month":
                                var months = ((agentRequest.DateTo.Year - agentRequest.DateFrom.Year) * 12) + agentRequest.DateTo.Month - agentRequest.DateFrom.Month;
                                for (int i = 0; i <= months; i++)
                                {
                                    distinctDate.Add(agentRequest.DateFrom.AddMonths(i));
                                }
                                break;
                        }
                    }

                    // set list of data for series 
                    foreach (var rDate in distinctDate)
                    {
                        long sumValue = 0;
                        switch (graphRequest.DateInterval)
                        {
                            case "hour":
                                sumValue = lstSummaryModel.Where(d =>
                                    d.SummaryDateTime.Date.Equals(rDate.Date) &&
                                    d.SummaryDateTime.Hour.Equals(rDate.Hour) &&
                                    d.SearchRequestID == agentRequest.ID
                                ).Sum(s => s.Audience);
                                break;
                            case "day":
                                sumValue = lstSummaryModel.Where(d => 
                                    d.SummaryDateTime.Date.Equals(rDate.Date) &&
                                    d.SearchRequestID == agentRequest.ID
                                ).Sum(s => s.Audience);
                                break;
                            case "month":
                                sumValue = lstSummaryModel.Where(d => 
                                    d.SummaryDateTime.Month == rDate.Month && 
                                    d.SummaryDateTime.Year == rDate.Year &&
                                    d.SearchRequestID == agentRequest.ID
                                ).Sum(s => s.Audience);
                                break;
                        }

                        HighChartDatum highChartDatum = new Model.HighChartDatum();
                        highChartDatum.y = sumValue;
                        highChartDatum.Value = agentRequest.ID.ToString();
                        highChartDatum.Date = rDate.ToShortDateString();
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

                viewsChart.series = lstSeries;

                return CommonFunctions.SearializeJson(viewsChart);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public string GetHighChartSeriesForGoogle(List<GoogleSummaryModel> lstGoogleSummaryModel, DateTime fromDate, DateTime toDate, string interval, AnalyticsGraphRequest graphRequest)
        {
            try
            {
                var distinctDate = new List<DateTime>();
                switch (interval)
                {
                    case "hour":
                        for (var dt = fromDate; dt <= toDate; dt = dt.AddHours(1))
                        {
                            distinctDate.Add(dt);
                        }
                        break;
                    case "day":
                        for (var dt = fromDate; dt <= toDate; dt = dt.AddDays(1))
                        {
                            distinctDate.Add(dt);
                        }
                        break;
                    case "month":
                        for (var dt = fromDate; dt <= toDate; dt = dt.AddMonths(1))
                        {
                            distinctDate.Add(dt);
                        }
                        break;
                }

                // The chart will never be displayed, but it has to be created properly in order for it's data to be added to the main chart
                HighLineChartOutput googleChart = new HighLineChartOutput();

                googleChart.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(distinctDate.Count()) / 7)),
                    categories = distinctDate.Select(a => a.ToShortDateString()).ToList(),
                    labels = new labels()
                    {
                        enabled = false
                    }
                };

                googleChart.hChart = new HChart() {
                    height = 100,
                    width = 120,
                    type = "spline"
                };
                googleChart.tooltip = new Tooltip() {
                    formatter = graphRequest.GraphType == "comparison" ? "FormatComparisonTooltip" : "" // Set here for reference, must also be set in JS
                };
                googleChart.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = false,
                            lineWidth = 0
                        }
                    }
                };

                List<Series> lstSeries = new List<Series>();
                List<string> dataTypes = lstGoogleSummaryModel.Select(s => s.DataType).Distinct().ToList();

                // Create a series for each distinct type of data in the list
                foreach (string dataType in dataTypes)
                {
                    Series series = new Series();
                    series.name = dataType;
                    series.data = new List<HighChartDatum>();

                    foreach (DateTime date in distinctDate)
                    {
                        long sumValue = 0;
                        switch (interval)
                        {
                            case "hour":
                                sumValue = lstGoogleSummaryModel.Where(d =>
                                    d.DataType == dataType &&
                                    d.DayDate.Date.Equals(date.Date) &&
                                    d.DayDate.Hour.Equals(date.Hour)
                                ).Sum(s => s.NoOfDocs);
                                break;
                            case "day":
                                sumValue = lstGoogleSummaryModel.Where(d => 
                                    d.DataType == dataType && 
                                    d.DayDate.Equals(date)
                                ).Sum(s => s.NoOfDocs);
                                break;
                            case "month":
                                sumValue = lstGoogleSummaryModel.Where(d => 
                                    d.DataType == dataType && 
                                    d.DayDate.Month == date.Month && 
                                    d.DayDate.Year == date.Year
                                ).Sum(s => s.NoOfDocs);
                                break;
                        }

                        HighChartDatum highChartDatum = new Model.HighChartDatum();
                        highChartDatum.y = sumValue;
                        highChartDatum.Date = date.ToShortDateString();
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

                googleChart.series = lstSeries;

                return CommonFunctions.SearializeJson(googleChart);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        #region Utility

        private List<string> GetChartXAxisValues(AnalyticsGraphRequest graphRequest, out List<DateTime> dateRange)
        {
            List<string> xAxisValues = new List<string>();
            dateRange = new List<DateTime>();

            Dictionary<long, TimeSpan> timeSpanList = graphRequest.AgentList.Select(agent => new
            {
                agent.ID,
                timeSpan = agent.DateTo.Subtract(agent.DateFrom)
            }).ToDictionary(
                    ag => ag.ID,
                    ag => ag.timeSpan
                );
            KeyValuePair<long, TimeSpan> maxSpan = timeSpanList.First(span => span.Value == timeSpanList.Values.Max());
            AnalyticsAgentRequest maxSpanAgent = graphRequest.AgentList.Where(agent => agent.ID == maxSpan.Key).First();

            switch (graphRequest.DateInterval)
            {
                case "hour":
                    for (int i = 0; i <= maxSpan.Value.TotalHours; i++)
                    {
                        dateRange.Add(maxSpanAgent.DateFrom.AddHours(i));
                    }

                    if (graphRequest.GraphType != "comparison")
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
                        dateRange.Add(maxSpanAgent.DateFrom.AddDays(i));
                    }

                    if (graphRequest.GraphType != "comparison")
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
                    var months = ((maxSpanAgent.DateTo.Year - maxSpanAgent.DateFrom.Year) * 12) + maxSpanAgent.DateTo.Month - maxSpanAgent.DateFrom.Month;

                    for (int i = 0; i <= months; i++)
                    {
                        dateRange.Add(maxSpanAgent.DateFrom.AddMonths(i));
                    }

                    if (graphRequest.GraphType != "comparison")
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

            filters.isEarned = false;
            filters.isSeenEarned = false;
            filters.isHeardEarned = false;

            filters.isSeenPaid = false;
            filters.isHeardPaid = false;

            filters.isRead = false;

            if (PESHTypes != null && PESHTypes.Count > 0)
            {
                // Read is an unique filter. Within the Seen/Heard/Read group, if it is the only one selected then OnAir results should be excluded. But if Seen or Heard is also selected then OnAir results should be included. 
                // This is accomplished by filtering out the SeenPaid, SeenEarned, HeardPaid, and HeardEarned values when necessary, since those are 
                bool isRead = PESHTypes.Contains("Read");
                bool isSeen = PESHTypes.Contains("Seen");
                bool isHeard = PESHTypes.Contains("Heard");
                bool isPaid = PESHTypes.Contains("Paid");
                bool isEarned = PESHTypes.Contains("Earned");

                filters.isRead = isRead;
                filters.isEarned = isEarned && !isSeen && !isHeard;

                if (!(isEarned && isRead && !isSeen && !isHeard))
                {
                    filters.isSeenEarned = (isSeen && isEarned) || (isSeen && !isPaid) || (isEarned && !isHeard);
                    filters.isHeardEarned = (isHeard && isEarned) || (isHeard && !isPaid) || (isEarned && !isSeen);
                }

                if (!(isPaid && isRead && !isSeen && !isHeard))
                {
                    filters.isSeenPaid = (isSeen && isPaid) || (isSeen && !isEarned) || (isPaid && !isHeard);
                    filters.isHeardPaid = (isHeard && isPaid) || (isHeard && !isEarned) || (isPaid && !isSeen);
                }
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
                }

                if (PESHTypes == null || PESHTypes.Count == 0)
                {
                    returnSum = summaries.Sum(s => s.Number_Of_Hits);
                }
                else
                {
                    // Read is a subset of Earned, so if Read is selected then Earned doesn't matter.
                    if (peshFilters.isRead)
                    {
                        returnSum += summaries.Sum(s => s.Number_Of_Hits - s.SeenEarned - s.SeenPaid - s.HeardEarned - s.HeardPaid);
                    }
                    else if (peshFilters.isEarned)
                    {
                        // Earned includes all media types.
                        returnSum += summaries.Where(w => w.SubMediaType != "TV").Sum(s => s.Number_Of_Hits);
                    }

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
                }
            }

            return returnSum;
        }

        public List<Series> GetAllSeriesForSummaries(List<AnalyticsSummaryModel> selectedSummaries, SecondaryTabID tab, DateTime startDate, DateTime endDate, string interval)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Log4NetLogger.Debug(string.Format("GetAllSeriesForSummaries"));
                Log4NetLogger.Debug(string.Format("selectedSummaries.Count: {0}", selectedSummaries.Count));
                Log4NetLogger.Debug(string.Format("tab: {0}", tab.ToString()));
                Log4NetLogger.Debug(string.Format("date range: {0} - {1}", startDate, endDate));
                Log4NetLogger.Debug(string.Format("interval: {0}", interval));

                List<DateTime> dateRange = new List<DateTime>();
                TimeSpan span = endDate.Subtract(startDate);
                switch(interval)
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
                    case "month":
                        break;
                }
                List<Series> allSeries = new List<Series>();
                // group selected summaries based on tab and create series for each group
                if (tab == SecondaryTabID.OverTime)
                {
                    foreach(var group in selectedSummaries.GroupBy(gb => gb.SearchRequestID))
                    {
                        Series agentSeries = new Series();
                        agentSeries.name = group.First().Query_Name;
                        agentSeries.data = new List<HighChartDatum>();
                        foreach (var date in dateRange)
                        {
                            var summariesForDate = group.Where(g => g.SummaryDateTime.Equals(date));
                            HighChartDatum hcd = new HighChartDatum();
                            hcd.Date = date.ToShortDateString();
                            hcd.SearchName = group.First().Query_Name;
                            hcd.Value = group.First().SearchRequestID.ToString();
                            hcd.y = summariesForDate.Any() ? summariesForDate.Sum(s => s.Number_Of_Hits) : 0;

                            agentSeries.data.Add(hcd);
                        }
                        allSeries.Add(agentSeries);
                    }
                }
                else if (tab == SecondaryTabID.Sources)
                {
                    foreach (var group in selectedSummaries.GroupBy(gb => gb.SubMediaType))
                    {
                        Series smtSeries = new Series();
                        smtSeries.name = group.First().SMTDisplayName;
                        smtSeries.data = new List<HighChartDatum>();
                        foreach (var date in dateRange)
                        {
                            var summariesForDate = group.Where(g => g.SummaryDateTime.Equals(date));
                            HighChartDatum hcd = new HighChartDatum();
                            hcd.Date = date.ToShortDateString();
                            hcd.SearchName = group.First().SMTDisplayName;
                            hcd.Value = group.First().SubMediaType;
                            hcd.y = summariesForDate.Any() ? summariesForDate.Sum(s => s.Number_Of_Hits) : 0;

                            smtSeries.data.Add(hcd);
                        }
                        allSeries.Add(smtSeries);
                    }
                }
                else if (tab == SecondaryTabID.Market)
                {
                    foreach (var group in selectedSummaries.GroupBy(gb => gb.MarketID))
                    {
                        Series marketSeries = new Series();
                        marketSeries.name = group.First().Market;
                        marketSeries.data = new List<HighChartDatum>();
                        foreach (var date in dateRange)
                        {
                            var summariesForDate = group.Where(g => g.SummaryDateTime.Equals(date));
                            HighChartDatum hcd = new HighChartDatum();
                            hcd.Date = date.ToShortDateString();
                            hcd.SearchName = group.First().Market;
                            hcd.Value = group.First().MarketID.ToString();
                            hcd.y = summariesForDate.Any() ? summariesForDate.Sum(s => s.Number_Of_Hits) : 0;

                            marketSeries.data.Add(hcd);
                        }
                        allSeries.Add(marketSeries);
                    }
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetAllSeriesForSummaries: {0} ms", sw.ElapsedMilliseconds));
                Log4NetLogger.Debug(string.Format("allSeries.count: {0}", allSeries.Count));
                return allSeries;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new List<Series>();
            }
        }

        #endregion

        #region ChartCreation

        /// <summary>
        /// Creates a column chart object
        /// </summary>
        /// <param name="graphRequest"></param>
        /// <param name="agentSummaryList"></param>
        /// <param name="PESHTypes"></param>
        /// <param name="sourceGroups"></param>
        /// <param name="subMediaTypesList"></param>
        /// <returns></returns>
        /// <remarks>
        /// Currently assumes that date range for each agent is the same
        /// </remarks>
        public string GetColumnOrBarChart(AnalyticsGraphRequest graphRequest, List<AnalyticsSummaryModel> agentSummaryList, List<string> PESHTypes, List<string> sourceGroups, List<IQ_MediaTypeModel> lstSubMediaTypes)
        {
            try
            {
                //Log4NetLogger.Debug("GETCOLUMNORBARCHART");
                List<Series> seriesList = new List<Series>();

                HighColumnChartModel chartOutput = new HighColumnChartModel() {
                    chart = new HChart() {
                        type = graphRequest.chartType.ToString().ToLower(),
                        //height = 325,
                        //width = 950
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
                            text = "Occurrences",
                            rotation = graphRequest.chartType == ChartType.Bar ? 0 : 270
                        },
                        min = 0
                    },
                    tooltip = new Tooltip() {
                        valueSuffix = string.Empty
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
                    }
                };

                // Determine which slices of data should be displayed based on which filters the user has enabled
                AnalyticsPESHFilters peshFilters = GetPESHFilters(PESHTypes, sourceGroups);

                Dictionary<long, string> dictSeries;
                if (graphRequest.PageType == "campaign")
                {
                    dictSeries = agentSummaryList.Select(s => new {
                        s.CampaignID,
                        s.CampaignName
                    }).Distinct().ToDictionary(
                        t => t.CampaignID,
                        t => t.CampaignName
                    );
                }
                else
                {
                    dictSeries = agentSummaryList.Select(s => new {
                        s.SearchRequestID,
                        s.Query_Name
                    }).Distinct().ToDictionary(
                        t => t.SearchRequestID,
                        t => t.Query_Name
                    );
                }

                switch(graphRequest.Tab)
                {
                    case SecondaryTabID.OverTime:
                        if (graphRequest.AgentList.Count > 0)
                        {
                            // Create a series for each agent
                            foreach (AnalyticsAgentRequest agentRequest in graphRequest.AgentList)
                            {
                                IEnumerable<AnalyticsSummaryModel> summaries;
                                if (graphRequest.PageType == "campaign")
                                {
                                    summaries = agentSummaryList.Where(summary => summary.CampaignID == agentRequest.ID);
                                }
                                else
                                {
                                    summaries = agentSummaryList.Where(summary => summary.SearchRequestID == agentRequest.ID);
                                }

                                string agentName = dictSeries[agentRequest.ID];

                                Series series = new Series();
                                series.data = new List<HighChartDatum>();
                                series.name = agentName;

                                HighChartDatum hcDatum = new HighChartDatum();
                                hcDatum.y = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, summaries);
                                hcDatum.SearchTerm = agentName;
                                hcDatum.Value = agentRequest.ID.ToString();
                                hcDatum.Type = "Media";
                                series.data.Add(hcDatum);

                                seriesList.Add(series);
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    case SecondaryTabID.Demographic:
                        if (graphRequest.AgentList.Count > 0)
                        {
                            chartOutput.yAxis.title.text = "Audience";

                            switch (graphRequest.SubTab)
                            {
                                case "gender":
                                    // Create a series for male + female
                                    Series maleSeries = new Series() {
                                        data = new List<HighChartDatum>(),
                                        name = "male"
                                    };

                                    HighChartDatum maleDatum = new HighChartDatum() {
                                        y = agentSummaryList.Sum(summary => summary.MaleAudience),
                                        SearchTerm = "",
                                        Value = "male",
                                        Type = "Media"
                                    };

                                    maleSeries.data.Add(maleDatum);
                                    seriesList.Add(maleSeries);

                                    Series femaleSeries = new Series() {
                                        data = new List<HighChartDatum>(),
                                        name = "female"
                                    };

                                    HighChartDatum femaleDatum = new HighChartDatum() {
                                        y = agentSummaryList.Sum(summary => summary.FemaleAudience),
                                        SearchTerm = "",
                                        Value = "female",
                                        Type = "Media"
                                    };

                                    femaleSeries.data.Add(femaleDatum);
                                    seriesList.Add(femaleSeries);
                                    break;
                                case "age":
                                    Series s18_24 = new Series() {
                                        data = new List<HighChartDatum>(),
                                        name = "18-24"
                                    };
                                    s18_24.data.Add(new HighChartDatum() {
                                        y = agentSummaryList.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24),
                                        Value = "18-24"
                                    });
                                    seriesList.Add(s18_24);

                                    Series s25_34 = new Series() {
                                        data = new List<HighChartDatum>(),
                                        name = "25-34"
                                    };
                                    s25_34.data.Add(new HighChartDatum() {
                                        y = agentSummaryList.Sum(s => s.AM25_34 + s.AF25_34),
                                        Value = "25-34"
                                    });
                                    seriesList.Add(s25_34);

                                    Series s35_49 = new Series() {
                                        data = new List<HighChartDatum>(),
                                        name = "35-49"
                                    };
                                    s35_49.data.Add(new HighChartDatum() {
                                        y = agentSummaryList.Sum(s => s.AM35_49 + s.AF35_49),
                                        Value = "35-49"
                                    });
                                    seriesList.Add(s35_49);

                                    Series s50_54 = new Series() {
                                        data = new List<HighChartDatum>(),
                                        name = "50-54"
                                    };
                                    s50_54.data.Add(new HighChartDatum() {
                                        y = agentSummaryList.Sum(s => s.AM50_54 + s.AF50_54),
                                        Value = "50-54"
                                    });
                                    seriesList.Add(s50_54);

                                    Series s55_64 = new Series() {
                                        data = new List<HighChartDatum>(),
                                        name = "55-64"
                                    };
                                    s55_64.data.Add(new HighChartDatum() {
                                        y = agentSummaryList.Sum(s => s.AM55_64 + s.AF55_64),
                                        Value = "55-64"
                                    });
                                    seriesList.Add(s55_64);

                                    Series s65_Plus = new Series() {
                                        data = new List<HighChartDatum>(),
                                        name = "65+"
                                    };
                                    s65_Plus.data.Add(new HighChartDatum() {
                                        y = agentSummaryList.Sum(s => s.AM65_Plus + s.AF65_Plus),
                                        Value = "65+"
                                    });
                                    seriesList.Add(s65_Plus);
                                    break;
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    case SecondaryTabID.Daytime:
                        // One series per day of the week
                        List<DayOfWeek> daysUnordered = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();
                        var days = daysUnordered.OrderBy(d => d == DayOfWeek.Sunday).ThenBy(d => d).ToList();

                        foreach (DayOfWeek dayOfWeek in days)
                        {
                            Series series = new Series();
                            series.name = dayOfWeek.ToString();
                            series.data = new List<HighChartDatum>() {
                                new HighChartDatum() {
                                    y = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, agentSummaryList.Where(w => w.SummaryDateTime.DayOfWeek == dayOfWeek)),
                                    Value = dayOfWeek.ToString()
                                }
                            };
                            seriesList.Add(series);
                        }

                        break;
                    case SecondaryTabID.Daypart:
                        var lstSort = agentSummaryList.Select(x => x.DayPartDisplay).Distinct();
                        //Log4NetLogger.Debug(string.Format("lstSort.Count: {0}", lstSort.Count()));
                        foreach (string sort in lstSort)
                        {
                            var ag = agentSummaryList.Find(f => f.DayPartDisplay == sort);
                            //Log4NetLogger.Debug(string.Format("sort is {0} part of agent {1} at {2}", sort, ag.SearchRequestID, ag.SummaryDateTime));
                            Series series = new Series();
                            series.name = sort;
                            series.data = new List<HighChartDatum>() {
                                new HighChartDatum() {
                                    y = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, agentSummaryList.Where(w => w.DayPartDisplay == sort)),
                                    Value = ag.DayPartID
                                }
                            };
                            seriesList.Add(series);
                        }

                        break;
                    case SecondaryTabID.Sources:
                        if (graphRequest.AgentList.Count > 0)
                        {
                            // Create a series for each sub media type
                            var smtGroups = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.SubMediaType)).GroupBy(gb => gb.SubMediaType).OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits));
                            foreach (var series in smtGroups)
                            {
                                Series smtSeries = new Series() {
                                    name = lstSubMediaTypes.Where(smt => smt.SubMediaType == series.First().SubMediaType).First().DisplayName,
                                    data = new List<HighChartDatum>()
                                };

                                long smtSum = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, series);

                                HighChartDatum hcDatum = new HighChartDatum();
                                hcDatum.y = smtSum;
                                hcDatum.Value = series.First().SubMediaType;
                                smtSeries.data.Add(hcDatum);

                                seriesList.Add(smtSeries);
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    case SecondaryTabID.Market:
                        //Log4NetLogger.Debug(string.Format("SECONDARY TAB MARKET"));
                        if (graphRequest.AgentList.Count > 0)
                        {
                            //Log4NetLogger.Debug(string.Format("graphRequest.AgentList.Count: {0}", graphRequest.AgentList.Count));
                            // Create a series for each market
                            var listMarkets = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.Market)).Select(asl => asl.Market).Distinct();
                            var dictDMAs = GetAllDMAs();
                            var marketGroups = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.Market)).GroupBy(gb => gb.Market).OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits));
                            var count = 0;
                            foreach (var series in marketGroups)
                            {
                                //Log4NetLogger.Debug(string.Format("series {0} Count: {1}", series.First().Market, series.Count()));
                                if (count < 15)
                                {
                                    Series marketSeries = new Series() {
                                        name = series.First().Market,
                                        data = new List<HighChartDatum>()
                                    };

                                    long marketSum = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, series);
                                    var marketDMA = dictDMAs.Where(dma => dma.Value == series.First().Market);
                                    string marketID = marketDMA.Any() ? marketDMA.First().Key : "";

                                    HighChartDatum hcDatum = new HighChartDatum() {
                                        y = marketSum,
                                        Value = marketID
                                    };
                                    marketSeries.data.Add(hcDatum);

                                    seriesList.Add(marketSeries);
                                    count += 1;
                                }
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    default:
                        break;
                }
                chartOutput.series = seriesList;

                return CommonFunctions.SearializeJson(chartOutput);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
            }
            return string.Empty;
        }

        public string GetPieChart(AnalyticsGraphRequest graphRequest, List<AnalyticsSummaryModel> agentSummaryList, List<string> PESHTypes, List<string> sourceGroups, List<IQ_MediaTypeModel> lstSubMediaTypes)
        {
            try
            {
                //Log4NetLogger.Debug("Logic.GetPieChart");
                //Log4NetLogger.Debug(string.Format("summaryList.count: {0}", agentSummaryList.Count));
                //Log4NetLogger.Debug(string.Format("agentsRequestedCount: {0}", graphRequest.AgentList.Count));
                HighPieChartModel pieChart = new HighPieChartModel();
                string chartName = "Occurrences";

                pieChart.chart = new PChart() {
                    //height = 400,
                    //width = 950
                };

                pieChart.title = new PTitle() {
                    text = chartName,
                    style = new HStyle {
                        color = "null",
                        fontFamily = "Open Sans",
                        fontSize = "null",
                        fontWeight = "null"
                    }
                };

                pieChart.plotOptions = new PPlotOptions() {
                    pie = new Pie() {
                        allowPointSelect = true,
                        cursor = "pointer",
                        showInLegend = true,
                        innerSize = "60%",
                        dataLabels = new DataLabels() {
                            enabled = false
                        }
                    }
                };

                pieChart.legend = new Legend() {
                    align = "center",
                    width = 500,
                    borderWidth = "0",
                    enabled = false
                };

                // Create new single series, multiple pie series will result in donut chart
                List<PSeries> pSeriesList = new List<PSeries>();
                PSeries pSeries = new PSeries() {
                    type = "pie",
                    name = chartName
                };

                // List to contain each agent and their associated value, will be data of pSeries, each object is a slice
                List<Object> sliceList = new List<object>();

                // Determine which slices of data should be displayed based on which filters the user has enabled
                AnalyticsPESHFilters peshFilters = GetPESHFilters(PESHTypes, sourceGroups);

                Dictionary<long, string> dictSeries;
                if (graphRequest.PageType == "campaign")
                {
                    dictSeries = agentSummaryList.Select(s => new {
                        s.CampaignID,
                        s.CampaignName
                    }).Distinct().ToDictionary(
                        t => t.CampaignID,
                        t => t.CampaignName
                    );
                }
                else
                {
                    dictSeries = agentSummaryList.Select(s => new {
                        s.SearchRequestID,
                        s.Query_Name
                    }).Distinct().ToDictionary(
                        t => t.SearchRequestID,
                        t => t.Query_Name
                    );
                }

                switch (graphRequest.Tab)
                {
                    case SecondaryTabID.OverTime:   // Each agent is a slice
                        // Create data slice for each agent
                        foreach (AnalyticsAgentRequest agentRequest in graphRequest.AgentList)
                        {
                            // Get all summaries for agent then returnSum together number of docs
                            IEnumerable<AnalyticsSummaryModel> summaries;
                            if (graphRequest.PageType == "campaign")
                            {
                                summaries = agentSummaryList.Where(summary => summary.CampaignID == agentRequest.ID);
                            }
                            else
                            {
                                summaries = agentSummaryList.Where(summary => summary.SearchRequestID == agentRequest.ID);
                            }
                            long agentSum = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, summaries);
                            string agentName = dictSeries[agentRequest.ID];

                            // Add agent slice to list of slices
                            sliceList.Add(new object[] {
                                agentName,
                                Convert.ToInt64(agentSum)
                            });
                        }
                        break;
                    case SecondaryTabID.Demographic:    // Slice are by gender or age depending on subTab
                        switch (graphRequest.SubTab)
                        {
                            case "gender":  // A slice for each gender
                                long maleSum = agentSummaryList.Sum(s => s.MaleAudience);
                                long femaleSum = agentSummaryList.Sum(s => s.FemaleAudience);

                                sliceList.Add(new object[] {
                                    "male",
                                    maleSum
                                });
                                sliceList.Add(new object[] {
                                    "female",
                                    femaleSum
                                });

                                break;
                            case "age": // A slice for each age group
                                long sum18_24 = agentSummaryList.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24);
                                long sum25_34 = agentSummaryList.Sum(s => s.AM25_34 + s.AF25_34);
                                long sum35_49 = agentSummaryList.Sum(s => s.AM35_49 + s.AF35_49);
                                long sum50_54 = agentSummaryList.Sum(s => s.AM50_54 + s.AF50_54);
                                long sum55_64 = agentSummaryList.Sum(s => s.AM55_64 + s.AF55_64);
                                long sum65_Plus = agentSummaryList.Sum(s => s.AM65_Plus + s.AF65_Plus);

                                sliceList.Add(new object[] {
                                    "18-24",
                                    sum18_24
                                });
                                sliceList.Add(new object[] {
                                    "25-34",
                                    sum25_34
                                });
                                sliceList.Add(new object[] {
                                    "35-49",
                                    sum35_49
                                });
                                sliceList.Add(new object[] {
                                    "50-54",
                                    sum50_54
                                });
                                sliceList.Add(new object[] {
                                    "55-64",
                                    sum55_64
                                });
                                sliceList.Add(new object[] {
                                    "65+",
                                    sum65_Plus
                                });

                                break;
                        }
                        break;
                    case SecondaryTabID.Daytime:
                        // One series per day of the week
                        List<DayOfWeek> daysUnordered = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();
                        var days = daysUnordered.OrderBy(d => d == DayOfWeek.Sunday).ThenBy(d => d).ToList();

                        foreach (DayOfWeek dayOfWeek in days)
                        {
                            sliceList.Add(new object[] {
                                dayOfWeek.ToString(),
                                GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, agentSummaryList.Where(w => w.SummaryDateTime.DayOfWeek == dayOfWeek))
                            });
                        }

                        break;
                    case SecondaryTabID.Daypart:
                        // One series per day of the week
                        var lstSort = agentSummaryList.Select(x => x.DayPartDisplay).Distinct();

                        foreach (string sort in lstSort)
                        {
                            sliceList.Add(new object[] {
                                sort,
                                GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, agentSummaryList.Where(w => w.DayPartDisplay == sort))
                            });
                            
                        }

                        break;
                    case SecondaryTabID.Sources:    // Each sub media type is a slice
                        var smtGroups = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.SubMediaType)).GroupBy(gb => gb.SubMediaType).OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits));
                        foreach (var series in smtGroups)
                        {
                            long smtSum = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, series);

                            sliceList.Add(new object[] {
                                lstSubMediaTypes.Where(smt => smt.SubMediaType == series.First().SubMediaType).First().DisplayName,
                                smtSum
                            });
                        }
                        break;
                    case SecondaryTabID.Market:
                        // Create a series for each market
                        var listMarkets = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.Market)).Select(asl => asl.Market).Distinct();
                        var dictDMAs = GetAllDMAs();
                        var marketGroups = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.Market)).GroupBy(gb => gb.Market).OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits));
                        var count = 0;
                        foreach (var series in marketGroups)
                        {
                            if (count < 15)
                            {
                                sliceList.Add(new object[] {
                                    series.First().Market,
                                    GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, series)
                                });
                                count += 1;
                            }
                        }
                        break;
                }

                // Add list of points to pSeries then add pSeries to list of series
                pSeries.data = sliceList;
                pSeriesList.Add(pSeries);
                pieChart.series = pSeriesList;

                pieChart.tooltip = new PTooltip() {
                    pointFormat = "<span style=\"color:{point.color}\">\u25CF </span>{point.name}: <b>{point.y}/{series.total:,.0f} = {point.percentage:.2f}%</b> of total"
                };

                return CommonFunctions.SearializeJson(pieChart);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
            }
            return string.Empty;
        }

        public string GetDaytimeHeatMap(List<AnalyticsSummaryModel> agentSummaryList, List<string> PESHTypes, List<string> sourceGroups, List<IQ_MediaTypeModel> lstSubMediaTypes)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("-GetDaytimeHeatMap"));
                //Log4NetLogger.Debug(string.Format("--agentSummaryList.Count: {0}", agentSummaryList.Count));
                
                HighHeatMapModel chartOutput = new HighHeatMapModel();
                List<DayOfWeek> daysUnordered = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();
                var days = daysUnordered.OrderBy(d => d == DayOfWeek.Sunday).ThenBy(d => d).Reverse().ToList();
                List<int> hours = Enumerable.Range(0, 24).ToList();
                string chartName = "Occurrences";

                chartOutput.hChart = new HChart() {
                    type = "heatmap",
                };

                chartOutput.title = new Title() {
                    text = chartName
                };

                chartOutput.yAxis = new HeatMapYAxis()
                {
                    title = null,
                    categories = days.Select(s => s.ToString()).ToList()
                };

                chartOutput.xAxis = new XAxis() {
                    categories = hours.Select(s => (s > 12 ? s - 12 : (s == 0 ? 12 : s)).ToString("D2") + ":00 " + (s >= 12 ? "PM" : "AM")).ToList(),
                    labels = new labels() { rotation = 315 },
                    tickWidth = 1,
                    tickmarkPlacement = "between"
                };

                chartOutput.legend = new HeatMapLegend() {
                    align = "right",
                    layout = "vertical",
                    verticalAlign = "top",
                    y = 25,
                    symbolHeight = 300
                };

                chartOutput.tooltip = new Tooltip() {
                    formatter = "FormatDaytimeTooltip" // Set here for reference, must also be set in JS
                };

                chartOutput.colorAxis = new ColorAxis()
                {
                    min = 0,
                    minColor = "#ffffff",
                    maxColor = "#598ea2"
                };

                HeatMapSeries series = new HeatMapSeries();
                series.name = chartName;
                series.data = new List<HeatMapDatum>();

                // Determine which slices of data should be displayed based on which filters the user has enabled
                AnalyticsPESHFilters peshFilters = GetPESHFilters(PESHTypes, sourceGroups);

                foreach (int hour in hours)
                {
                    foreach (DayOfWeek day in days)
                    {
                        List<AnalyticsSummaryModel> summaries = agentSummaryList == null ? new List<AnalyticsSummaryModel>() : agentSummaryList.Where(w => w.SummaryDateTime.Hour == hour && w.SummaryDateTime.DayOfWeek == day).ToList();

                        HeatMapDatum datum = new HeatMapDatum();
                        datum.x = hours.IndexOf(hour);
                        datum.y = days.IndexOf(day);
                        datum.value = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, summaries);
                        datum.borderWidth = 1;
                        datum.borderColor = "#cccccc";
                        series.data.Add(datum); 
                    }
                }

                chartOutput.series = new List<HeatMapSeries>() { series };
                return CommonFunctions.SearializeJson(chartOutput);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
            }
            return string.Empty;
        }

        public string GetDaypartHeatMap(List<AnalyticsSummaryModel> agentSummaryList, List<string> PESHTypes, List<string> sourceGroups, List<IQ_MediaTypeModel> lstSubMediaTypes)
        {
            try
            {
                HighHeatMapModel chartOutput = new HighHeatMapModel();
                List<DayOfWeek> daysUnordered = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();
                var days = daysUnordered.OrderBy(d => d == DayOfWeek.Sunday).ThenBy(d => d).Reverse().ToList();
                List<int> hours = Enumerable.Range(0, 24).ToList();
                string chartName = "Occurrences";

                chartOutput.hChart = new HChart()
                {
                    type = "heatmap",
                };

                chartOutput.title = new Title()
                {
                    text = chartName
                };

                var axisLabels = new List<string>();
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
                chartOutput.yAxis = new HeatMapYAxis()
                {
                    title = null,
                    categories = axisLabels
                };

                chartOutput.xAxis = new XAxis()
                {
                    categories = hours.Select(s => (s > 12 ? s - 12 : (s == 0 ? 12 : s)).ToString("D2") + ":00 " + (s >= 12 ? "PM" : "AM")).ToList(),
                    labels = new labels() { rotation = 315 },
                    tickWidth = 1,
                    tickmarkPlacement = "between"
                };

                chartOutput.legend = new HeatMapLegend()
                {
                    align = "right",
                    layout = "vertical",
                    verticalAlign = "top",
                    y = 25,
                    symbolHeight = 300
                };

                chartOutput.tooltip = new Tooltip()
                {
                    formatter = "FormatDaypartTooltip" // Set here for reference, must also be set in JS
                };

                chartOutput.colorAxis = new ColorAxis()
                {
                    min = 0,
                    minColor = "#ffffff",
                    maxColor = "#598ea2"
                };

                HeatMapSeries series = new HeatMapSeries();
                series.name = chartName;
                series.data = new List<HeatMapDatum>();
                series.dataLabels = new HeatMapDataLabels()
                {
                    enabled = true,
                    color = "#636f72",
                    formatter = "FormatDaypartDataLabel", // Set here for reference, must also be set in JS
                    style = new HeatStyle(){
                        fontSize = "12px",
                        fontWeight = "normal",
                        textShadow = "0 0 10px white"
                    }
                };

                // Determine which slices of data should be displayed based on which filters the user has enabled
                AnalyticsPESHFilters peshFilters = GetPESHFilters(PESHTypes, sourceGroups);

                //Get Day Part Data
                List<DayPartDataItem> dayPartData = GetDayPartData("A");

                foreach (int hour in hours)
                {
                    int count = 0;
                    var datums = new List<HeatMapDatum>();

                    foreach (DayOfWeek day in days)
                    {
                        if (count == 2) count++;

                        List<AnalyticsSummaryModel> summaries = agentSummaryList == null ? new List<AnalyticsSummaryModel>() : agentSummaryList.Where(w => w.SummaryDateTime.Hour == hour && w.SummaryDateTime.DayOfWeek == day).ToList();
                        List<DayPartDataItem> dayPartItem = dayPartData == null ? new List<DayPartDataItem>() : dayPartData.Where(x => x.DayOfWeek == day && hour == x.HourOfDay).ToList();

                        HeatMapDatum datum = new HeatMapDatum();
                        datum.x = hours.IndexOf(hour);
                        datum.y = count;
                        datum.value = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, summaries);
                        datum.name =  (dayPartItem != null && dayPartItem.Count() > 0) ? dayPartItem.First().DayPartName : "";
                        datum.code =  (dayPartItem != null && dayPartItem.Count() > 0) ? dayPartItem.First().DayPartCode : "";
                        datums.Add(datum);
                        count++;
                    }

                    datums.Insert(2,new HeatMapDatum()
                    {
                        x = hours.IndexOf(hour),
                        y = 2,
                        value = 0,
                        name = "",
                        code = ""
                    });

                    series.data.AddRange(datums);
                }

                chartOutput.series = new List<HeatMapSeries>() { series };
                return CommonFunctions.SearializeJson(chartOutput);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
            }
            return string.Empty;
        }

        public string GetGrowthChart(AnalyticsGraphRequest graphRequest, List<AnalyticsSummaryModel> agentSummaryList, List<string> PESHTypes, List<string> sourceGroups, List<IQ_MediaTypeModel> lstSubMediaTypes, string xAxisLabel)
        {
            try
            {
                HighLineChartOutput chartOutput = new HighLineChartOutput();
                List<DateTime> dateRange = new List<DateTime>();
                List<string> xAxisValues = new List<string>();
                List<Series> seriesList = new List<Series>();
                Dictionary<long, TimeSpan> timeSpanList = graphRequest.AgentList.Select(agent => new {
                    agent.ID,
                    timeSpan = agent.DateTo.Subtract(agent.DateFrom)
                }).ToDictionary(
                    ag => ag.ID,
                    ag => ag.timeSpan
                );

                // Get max timespan and agent ID associated with it
                KeyValuePair<long, TimeSpan> maxSpan = timeSpanList.First(span => span.Value == timeSpanList.Values.Max());
                //Log4NetLogger.Debug(string.Format("MaxSpan Value: {0} Key: {1}", maxSpan.Value, maxSpan.Key));

                // Get agent with max timespan - need for the start date
                AnalyticsAgentRequest maxSpanAgent = graphRequest.AgentList.Where(agent => agent.ID == maxSpan.Key).First();

                switch (graphRequest.DateInterval)
                {
                    case "hour":
                        // All agents have same start/end dates
                        for (int i = 0; i <= maxSpan.Value.TotalHours; i++)
                        {
                            dateRange.Add(maxSpanAgent.DateFrom.AddHours(i));
                        }

                        if (graphRequest.GraphType != "comparison")
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

                        //Log4NetLogger.Debug(string.Format("TimeSpan total Hours: {0}", maxSpan.Value.TotalHours));
                        //Log4NetLogger.Debug(string.Format("dateRange Start: {0} End: {1}", dateRange[0], dateRange[dateRange.Count - 1]));
                        break;
                    case "day":
                        for (int i = 0; i <= maxSpan.Value.Days; i++)
                        {
                            dateRange.Add(maxSpanAgent.DateFrom.AddDays(i));
                        }

                        if (graphRequest.GraphType != "comparison")
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
                        var months = ((maxSpanAgent.DateTo.Year - maxSpanAgent.DateFrom.Year) * 12) + maxSpanAgent.DateTo.Month - maxSpanAgent.DateFrom.Month;
                        var startDate = new DateTime(maxSpanAgent.DateFrom.Year, maxSpanAgent.DateFrom.Month, 1);
                        for (int i = 0; i <= months; i++)
                        {
                            //Log4NetLogger.Debug(string.Format("adding date: {0}", date.ToShortDateString()));
                            dateRange.Add(startDate.AddMonths(i));
                        }
                        if (graphRequest.GraphType != "comparison")
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

                // Single line medium chart, without applying any medium filter
                chartOutput.title = new Title() {
                    text = string.Empty,
                    x = -20
                };
                chartOutput.subtitle = new Subtitle() {
                    text = string.Empty,
                    x = -20
                };

                // If filtering on hit type, hit count will be displayed instead of document count. Label the y-axis to indicate this.
                chartOutput.yAxis = new List<YAxis>() {
                    new YAxis() {
                        title =  new Title2() {
                            text = "% Growth"
                        }
                    }
                };

                // To show date labels vertically on x-axis, apply rotation on label = 270
                // Maximum number of x-axis labels set with tickInterval, currently 45
                // tickWidth = 2 will skip showing alternative labels in x-axis
                chartOutput.xAxis = new XAxis() {
                    // Determines width of intervals, wants graph dividing in 7 equal sections - min for that is 14 dates
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(xAxisValues.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = xAxisValues,    // All x-axis values
                    labels = new labels() {
                        staggerLines = graphRequest.DateInterval == "hour" ? 2 : 0 
                    },
                    title = new Title2() {
                        text = xAxisLabel,
                        rotation = 0
                    }
                };

                // Show default tooltip format x/y values
                chartOutput.tooltip = new Tooltip() {
                    formatter = graphRequest.GraphType == "comparison" ? "FormatComparisonTooltip" : "", // Set here for reference, must also be set in JS
                    valueSuffix = "%"
                };

                if (graphRequest.GraphType != "comparison")
                {
                    chartOutput.tooltip.pointFormat = "<span style=\"color:{series.color}\">\u25CF </span>{series.name}: <b>{point.y:.2f}%</b>";
                }

                // Show legend in center, with borderWidth = 0
                chartOutput.legend = new Legend() {
                    align = "right",
                    borderWidth = "0",
                    enabled = true,
                    layout = "vertical",
                    verticalAlign = "top",
                    x = -90
                };

                // Set chart height, if not it will default to 100%
                chartOutput.hChart = new HChart() {
                    //height = 350,
                    type = "spline",
                    marginRight = 0
                };

                // Set plot options and click event for series points (will again be assigned in JS)
                chartOutput.plotOption = new PlotOptions() {
                    spline = new PlotSeries() {
                        marker = new PlotMarker() {
                            enabled = true
                        }
                    },
                    series = new PlotSeries() {
                        cursor = "pointer",
                        point = new PlotPoint() {
                            events = new PlotEvents() {
                                click = ""
                            }
                        }
                    }
                };

                // Determine which slices of data should be displayed based on which filters the user has enabled
                AnalyticsPESHFilters peshFilters = GetPESHFilters(PESHTypes, sourceGroups);

                Dictionary<long, string> dictSeries;
                if (graphRequest.PageType == "campaign")
                {
                    dictSeries = agentSummaryList.Select(s => new {
                        s.CampaignID,
                        s.CampaignName
                    }).Distinct().ToDictionary(
                        t => t.CampaignID,
                        t => t.CampaignName
                    );
                }
                else
                {
                    dictSeries = agentSummaryList.Select(s => new {
                        s.SearchRequestID,
                        s.Query_Name
                    }).Distinct().ToDictionary(
                        t => t.SearchRequestID,
                        t => t.Query_Name
                    );
                }

                // Create plot
                switch(graphRequest.Tab)
                {
                    case SecondaryTabID.OverTime:   // Each agent is a series
                        //Log4NetLogger.Debug("OverTime LineChart Requested");
                        if (graphRequest.AgentList.Count > 0)
                        {
                            // Create a series for each agent
                            foreach (AnalyticsAgentRequest agentRequest in graphRequest.AgentList)
                            {
                                IEnumerable<AnalyticsSummaryModel> summaries;
                                if (graphRequest.PageType == "campaign")
                                {
                                    summaries = agentSummaryList.Where(summary => summary.CampaignID == agentRequest.ID);
                                }
                                else
                                {
                                    summaries = agentSummaryList.Where(summary => summary.SearchRequestID == agentRequest.ID);
                                }

                                string agentName = dictSeries[agentRequest.ID];

                                Series series = new Series();
                                series.data = new List<HighChartDatum>();
                                series.name = agentName;

                                if (graphRequest.GraphType == "comparison")
                                {
                                    dateRange = new List<DateTime>();
                                    TimeSpan agentTS = agentRequest.DateTo.Subtract(agentRequest.DateFrom);

                                    switch (graphRequest.DateInterval)
                                    {
                                        case "hour":
                                            for (int i = 0; i <= agentTS.TotalHours; i++)
                                            {
                                                dateRange.Add(agentRequest.DateFrom.AddHours(i));
                                            }
                                            break;
                                        case "day":
                                            for (int i = 0; i <= agentTS.Days; i++)
                                            {
                                                dateRange.Add(agentRequest.DateFrom.AddDays(i));
                                            }
                                            break;
                                        case "month":
                                            var months = ((agentRequest.DateTo.Year - agentRequest.DateFrom.Year) * 12) + agentRequest.DateTo.Month - agentRequest.DateFrom.Month;
                                            for (int i = 0; i <= months; i++)
                                            {
                                                dateRange.Add(agentRequest.DateFrom.AddMonths(i));
                                            }
                                            break;
                                    }
                                }

                                long prevDaySum = 0;

                                // For each date in date range create points for agent in series
                                foreach (var date in dateRange)
                                {
                                    var summariesForDate = summaries.Where(summary => summary.SummaryDateTime.Equals(date));
                                    // Get day returnSum for all hits in summaries for date, needed if multiple media types
                                    long daySum = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, summariesForDate);
                                    decimal pctChange = 0;
                                    if (prevDaySum != 0)
                                    {
                                        pctChange = ((daySum - prevDaySum) / (decimal)prevDaySum) * 100;
                                    }
                                    prevDaySum = daySum;

                                    HighChartDatum hcDatum = new HighChartDatum();
                                    hcDatum.y = pctChange;
                                    hcDatum.SearchTerm = agentName;
                                    hcDatum.Value = agentRequest.ID.ToString();
                                    hcDatum.Type = "Media";
                                    hcDatum.Date = date.ToShortDateString();
                                    series.data.Add(hcDatum);
                                }
                                seriesList.Add(series);
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    case SecondaryTabID.Sources:    //Each source/submedia type is a series
                        if (graphRequest.AgentList.Count > 0)
                        {
                            // Get a list of each requested agent's date range, since in comparison mode they won't be the same
                            Dictionary<long, List<DateTime>> dictAgentDateRanges = new Dictionary<long, List<DateTime>>();
                            List<DateTime> lstDateRange = new List<DateTime>();
                            int maxPeriods = 0;
                            foreach (AnalyticsAgentRequest agentRequest in graphRequest.AgentList)
                            {
                                switch (graphRequest.DateInterval)
                                {
                                    case "hour":
                                        lstDateRange = Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(agentRequest.DateTo.Subtract(agentRequest.DateFrom).TotalHours))).Select(offset => agentRequest.DateFrom.AddHours(offset)).ToList();
                                        maxPeriods = Convert.ToInt32(maxSpan.Value.TotalHours);
                                        break;
                                    case "day":
                                        lstDateRange = Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(agentRequest.DateTo.Subtract(agentRequest.DateFrom).TotalDays))).Select(offset => agentRequest.DateFrom.AddDays(offset)).ToList();
                                        maxPeriods = maxSpan.Value.Days;
                                        break;
                                    case "month":
                                        var startDate = new DateTime(agentRequest.DateFrom.Year, agentRequest.DateFrom.Month, 1);
                                        var months = ((agentRequest.DateTo.Year - agentRequest.DateFrom.Year) * 12) + agentRequest.DateTo.Month - agentRequest.DateFrom.Month;
                                        lstDateRange = Enumerable.Range(0, months).Select(offset => startDate.AddMonths(offset)).ToList();
                                        maxPeriods = ((maxSpanAgent.DateTo.Year - maxSpanAgent.DateFrom.Year) * 12) + maxSpanAgent.DateTo.Month - maxSpanAgent.DateFrom.Month;
                                        break;
                                }

                                dictAgentDateRanges.Add(agentRequest.ID, lstDateRange);
                            }

                            var smtGroups = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.SubMediaType)).GroupBy(gb => gb.SubMediaType).OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits));
                            foreach (var series in smtGroups)
                            {
                                Series smtSeries = new Series() {
                                    name = lstSubMediaTypes.Where(smt => smt.SubMediaType == series.First().SubMediaType).First().DisplayName,
                                    data = new List<HighChartDatum>()
                                };

                                long prevDaySum = 0;

                                for (int i = 0; i <= maxPeriods; i++)
                                {
                                    var summariesForDate = new List<AnalyticsSummaryModel>();
                                    DateTime datumDT = new DateTime();

                                    foreach (KeyValuePair<long, List<DateTime>> agentDateRange in dictAgentDateRanges)
                                    {
                                        if (i < agentDateRange.Value.Count)
                                        {
                                            if (graphRequest.PageType == "campaign")
                                            {
                                                summariesForDate.AddRange(
                                                    series.Where(summary =>
                                                        summary.CampaignID == agentDateRange.Key &&
                                                        summary.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                    )
                                                );
                                            }
                                            else
                                            {
                                                summariesForDate.AddRange(
                                                    series.Where(summary =>
                                                        summary.SearchRequestID == agentDateRange.Key &&
                                                        summary.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                    )
                                                );
                                            }
                                            datumDT = agentDateRange.Value[i];
                                        }
                                    }

                                    long daySum = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, summariesForDate);
                                    decimal pctChange = 0;
                                    if (prevDaySum != 0)
                                    {
                                        pctChange = ((daySum - prevDaySum) / (decimal)prevDaySum) * 100;
                                    }
                                    prevDaySum = daySum;

                                    HighChartDatum hcDatum = new HighChartDatum() {
                                        y = pctChange,
                                        Date = datumDT.ToShortDateString(),
                                        Value = series.First().SubMediaType
                                    };
                                    smtSeries.data.Add(hcDatum);
                                }

                                seriesList.Add(smtSeries);
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    case SecondaryTabID.Market:
                        if (graphRequest.AgentList.Count > 0)
                        {
                            // Get a list of each requested agent's date range, since in comparison mode they won't be the same
                            Dictionary<long, List<DateTime>> dictAgentDateRanges = new Dictionary<long, List<DateTime>>();
                            List<DateTime> lstDateRange = new List<DateTime>();
                            int maxPeriods = 0;
                            foreach (AnalyticsAgentRequest agentRequest in graphRequest.AgentList)
                            {
                                switch (graphRequest.DateInterval)
                                {
                                    case "hour":
                                        lstDateRange = Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(agentRequest.DateTo.Subtract(agentRequest.DateFrom).TotalHours))).Select(offset => agentRequest.DateFrom.AddHours(offset)).ToList();
                                        maxPeriods = Convert.ToInt32(maxSpan.Value.TotalHours);
                                        break;
                                    case "day":
                                        lstDateRange = Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(agentRequest.DateTo.Subtract(agentRequest.DateFrom).TotalDays))).Select(offset => agentRequest.DateFrom.AddDays(offset)).ToList();
                                        maxPeriods = maxSpan.Value.Days;
                                        break;
                                    case "month":
                                        var startDate = new DateTime(agentRequest.DateFrom.Year, agentRequest.DateFrom.Month, 1);
                                        var months = ((agentRequest.DateTo.Year - agentRequest.DateFrom.Year) * 12) + agentRequest.DateTo.Month - agentRequest.DateFrom.Month;
                                        lstDateRange = Enumerable.Range(0, months).Select(offset => startDate.AddMonths(offset)).ToList();
                                        maxPeriods = ((maxSpanAgent.DateTo.Year - maxSpanAgent.DateFrom.Year) * 12) + maxSpanAgent.DateTo.Month - maxSpanAgent.DateFrom.Month;
                                        break;
                                }

                                dictAgentDateRanges.Add(agentRequest.ID, lstDateRange);
                            }

                            // Create a series for each market
                            var listMarkets = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.Market)).Select(asl => asl.Market).Distinct();
                            var dictDMAs = GetAllDMAs();
                            var marketGroups = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.Market)).GroupBy(gb => gb.Market).OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits));
                            var count = 0;
                            foreach (var series in marketGroups)
                            {
                                if (count < 15)
                                {
                                    Series marketSeries = new Series() {
                                        name = series.First().Market,
                                        data = new List<HighChartDatum>()
                                    };

                                    long prevDaySum = 0;
                                    long marketSum = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, series);
                                    var marketDMA = dictDMAs.Where(dma => dma.Value == series.First().Market);
                                    string marketID = marketDMA.Any() ? marketDMA.First().Key : "";

                                    for (int i = 0; i <= maxPeriods; i++)
                                    {
                                        var summariesForDate = new List<AnalyticsSummaryModel>();
                                        DateTime datumDT = new DateTime();

                                        foreach (KeyValuePair<long, List<DateTime>> agentDateRange in dictAgentDateRanges)
                                        {
                                            if (i < agentDateRange.Value.Count)
                                            {
                                                if (graphRequest.PageType == "campaign")
                                                {
                                                    var ms = series.Where(summ =>
                                                        summ.CampaignID == agentDateRange.Key &&
                                                        summ.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                    );
                                                    summariesForDate.AddRange(ms);
                                                }
                                                else
                                                {
                                                    var ms = series.Where(summ =>
                                                        summ.SearchRequestID == agentDateRange.Key &&
                                                        summ.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                    );
                                                    summariesForDate.AddRange(ms);
                                                }

                                                datumDT = agentDateRange.Value[i];
                                            }
                                        }

                                        long daySum = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, summariesForDate);
                                        decimal pctChange = 0;

                                        if (prevDaySum != 0)
                                        {
                                            pctChange = ((daySum - prevDaySum) / (decimal)prevDaySum) * 100;
                                        }
                                        prevDaySum = daySum;

                                        HighChartDatum hcDatum = new HighChartDatum() {
                                            y = pctChange,
                                            Date = datumDT.ToShortDateString(),
                                            Value = marketID.ToString()
                                        };
                                        marketSeries.data.Add(hcDatum);
                                    }
                                    seriesList.Add(marketSeries);
                                    count += 1;
                                }
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    default:
                        break;
                }

                chartOutput.series = seriesList;

                return CommonFunctions.SearializeJson(chartOutput);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
            }
            return string.Empty;
        }

        public string GetLineChart(AnalyticsGraphRequest graphRequest, List<AnalyticsSummaryModel> agentSummaryList, List<string> PESHTypes, List<string> sourceGroups, List<IQ_MediaTypeModel> lstSubMediaTypes, string xAxisLabel)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("GetLineChart"));
                //Log4NetLogger.Debug(string.Format("dateInterval: {0}", graphRequest.DateInterval));
                //Log4NetLogger.Debug(string.Format("graphType: {0}", graphRequest.GraphType));
                HighLineChartOutput chartOutput = new HighLineChartOutput();
                List<DateTime> dateRange = new List<DateTime>();    // Used to get summaries for the dates in list
                List<string> xAxisValues = new List<string>();      // Values shown on xAxis will be different than date range
                List<Series> seriesList = new List<Series>();
                Dictionary<long, TimeSpan> timeSpanList = graphRequest.AgentList.Select(agent => new {
                        agent.ID,
                        timeSpan = agent.DateTo.Subtract(agent.DateFrom)
                    }).ToDictionary(
                        ag => ag.ID,
                        ag => ag.timeSpan
                    );

                // Get max timespan and agent ID associated with it
                KeyValuePair<long, TimeSpan> maxSpan = timeSpanList.First(span => span.Value == timeSpanList.Values.Max());
                //Log4NetLogger.Debug(string.Format("MaxSpan Value: {0} Key: {1}", maxSpan.Value, maxSpan.Key));

                // Get agent with max timespan - need for the start date
                AnalyticsAgentRequest maxSpanAgent = graphRequest.AgentList.Where(agent => agent.ID == maxSpan.Key).First();
                //Log4NetLogger.Debug(string.Format("maxSpanAgent {0} date from {1}", maxSpanAgent.ID, maxSpanAgent.DateFrom.Date));
                switch (graphRequest.DateInterval)
                {
                    case "hour":
                        // All agents have same start/end dates
                        for (int i = 0; i <= maxSpan.Value.TotalHours; i++)
                        {
                            dateRange.Add(maxSpanAgent.DateFrom.AddHours(i));
                        }

                        if (graphRequest.GraphType != "comparison")
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

                        //Log4NetLogger.Debug(string.Format("TimeSpan total Hours: {0}", maxSpan.Value.TotalHours));
                        //Log4NetLogger.Debug(string.Format("dateRange Start: {0} End: {1}", dateRange[0], dateRange[dateRange.Count - 1]));
                        break;
                    case "day":
                        for (int i = 0; i <= maxSpan.Value.Days; i++)
                        {
                            // Do not want to include hour when only needing to compare based on day, fixes issue of different time zones not displaying line charts
                            dateRange.Add(maxSpanAgent.DateFrom.AddDays(i));
                        }

                        if (graphRequest.GraphType != "comparison")
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
                        var months = ((maxSpanAgent.DateTo.Year - maxSpanAgent.DateFrom.Year) * 12) + maxSpanAgent.DateTo.Month - maxSpanAgent.DateFrom.Month;
                        var startDate = new DateTime(maxSpanAgent.DateFrom.Year, maxSpanAgent.DateFrom.Month, 1);
                        for (int i = 0; i <= months; i++)
                        {
                            //Log4NetLogger.Debug(string.Format("adding date: {0}", date.ToShortDateString()));
                            dateRange.Add(startDate.AddMonths(i));
                        }
                        if (graphRequest.GraphType != "comparison")
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

                // Single line medium chart, without applying any medium filter
                chartOutput.title = new Title() {
                    text = string.Empty,
                    x = -20
                };
                chartOutput.subtitle = new Subtitle() {
                    text = string.Empty,
                    x = -20
                };

                // If filtering on hit type, hit count will be displayed instead of document count. Label the y-axis to indicate this.
                chartOutput.yAxis = new List<YAxis>() {
                    new YAxis() {
                        title =  new Title2() {
                            text = "Occurrences"
                        }
                    }
                };

                // To show date labels vertically on x-axis, apply rotation on label = 270
                // Maximum number of x-axis labels set with tickInterval, currently 45
                // tickWidth = 2 will skip showing alternative labels in x-axis
                chartOutput.xAxis = new XAxis() {
                    // Determines width of intervals, wants graph dividing in 7 equal sections - min for that is 14 dates
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(xAxisValues.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = xAxisValues,    // All x-axis values
                    labels = new labels() {
                        staggerLines = graphRequest.DateInterval == "hour" ? 2 : 0 
                    },
                    title = new Title2() {
                        text = xAxisLabel,
                        rotation = 0
                    }
                };

                // Show default tooltip format x/y values
                chartOutput.tooltip = new Tooltip() {
                    formatter = graphRequest.GraphType == "comparison" ? "FormatComparisonTooltip" : "" // Set here for reference, must also be set in JS
                };

                // Show legend in top right, with borderWidth = 0, will only display overlay series in legend
                chartOutput.legend = new Legend() {
                    align = "right",
                    borderWidth = "0",
                    enabled = true,
                    verticalAlign = "top",
                    layout = "vertical",
                    x = -90
                };

                // Set chart height, if not it will default to 100%
                chartOutput.hChart = new HChart() {
                    //height = 350,
                    type = "spline",
                    marginRight = 0
                };

                // Set plot options and click event for series points (will again be assigned in JS)
                chartOutput.plotOption = new PlotOptions() {
                    spline = new PlotSeries() {
                        marker = new PlotMarker() {
                            enabled = true
                        }
                    },
                    series = new PlotSeries() {
                        cursor = "pointer",
                        point = new PlotPoint() {
                            events = new PlotEvents() {
                                click = ""
                            }
                        }
                    }
                };

                // Determine which slices of data should be displayed based on which filters the user has enabled
                AnalyticsPESHFilters peshFilters = GetPESHFilters(PESHTypes, sourceGroups);

                Dictionary<long, string> dictSeries;
                if (graphRequest.PageType == "campaign")
                {
                    dictSeries = agentSummaryList.Select(s => new {
                        s.CampaignID,
                        s.CampaignName
                    }).Distinct().ToDictionary(
                        t => t.CampaignID,
                        t => t.CampaignName
                    );
                }
                else
                {
                    dictSeries = agentSummaryList.Select(s => new {
                        s.SearchRequestID,
                        s.Query_Name
                    }).Distinct().ToDictionary(
                        t => t.SearchRequestID,
                        t => t.Query_Name
                    );
                }

                // Create plot
                switch (graphRequest.Tab)
                {
                    case SecondaryTabID.OverTime:   // Each agent is a series
                        //Log4NetLogger.Debug("OverTime LineChart Requested");
                        if (graphRequest.AgentList.Count > 0)
                        {
                            // Create a series for each agent
                            foreach (AnalyticsAgentRequest agentRequest in graphRequest.AgentList)
                            {
                                IEnumerable<AnalyticsSummaryModel> summaries;
                                if (graphRequest.PageType == "campaign")
                                {
                                    summaries = agentSummaryList.Where(summary => summary.CampaignID == agentRequest.ID);
                                }
                                else
                                {
                                    summaries = agentSummaryList.Where(summary => summary.SearchRequestID == agentRequest.ID);
                                }

                                string agentName = dictSeries[agentRequest.ID];

                                Series series = new Series();
                                series.data = new List<HighChartDatum>();
                                series.name = agentName;

                                if (graphRequest.GraphType == "comparison")
                                {
                                    dateRange = new List<DateTime>();
                                    TimeSpan agentTS = agentRequest.DateTo.Subtract(agentRequest.DateFrom);

                                    switch (graphRequest.DateInterval)
                                    {
                                        case "hour":
                                            for (int i = 0; i <= agentTS.TotalHours; i++)
                                            {
                                                dateRange.Add(agentRequest.DateFrom.AddHours(i));
                                            }
                                            break;
                                        case "day":
                                            for (int i = 0; i <= agentTS.Days; i++)
                                            {
                                                dateRange.Add(agentRequest.DateFrom.AddDays(i));
                                            }
                                            break;
                                        case "month":
                                            var months = ((agentRequest.DateTo.Year - agentRequest.DateFrom.Year) * 12) + agentRequest.DateTo.Month - agentRequest.DateFrom.Month;
                                            for (int i = 0; i <= months; i++)
                                            {
                                                dateRange.Add(agentRequest.DateFrom.AddMonths(i));
                                            }
                                            break;
                                    }
                                }

                                // For each date in date range create points for agent in series
                                foreach (var date in dateRange)
                                {
                                    var summariesForDate = summaries.Where(summary => summary.SummaryDateTime.Equals(date));
                                    // Get day returnSum for all hits in summaries for date, needed if multiple media types
                                    //Log4NetLogger.Debug(string.Format("--LineChart date {0} from dateRange with {1} summaries", date, summariesForDate.Count()));

                                    HighChartDatum hcDatum = new HighChartDatum();
                                    hcDatum.y = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, summariesForDate);
                                    hcDatum.SearchTerm = agentName;
                                    hcDatum.Value = agentRequest.ID.ToString();
                                    hcDatum.Type = "Media";
                                    hcDatum.Date = date.ToShortDateString();
                                    series.data.Add(hcDatum);
                                }
                                seriesList.Add(series);
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    case SecondaryTabID.Demographic:    // Each gender/ageRange is a series
                        if (graphRequest.AgentList.Count > 0)
                        {
                            chartOutput.yAxis[0].title.text = "Audience";

                            // Get a list of each requested agent's date range, since in comparison mode they won't be the same
                            Dictionary<long, List<DateTime>> dictAgentDateRanges = new Dictionary<long, List<DateTime>>();
                            List<DateTime> lstDateRange = new List<DateTime>();
                            int maxPeriods = 0;
                            foreach (AnalyticsAgentRequest agentRequest in graphRequest.AgentList)
                            {
                                switch (graphRequest.DateInterval)
                                {
                                    case "hour":
                                        lstDateRange = Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(agentRequest.DateTo.Subtract(agentRequest.DateFrom).TotalHours))).Select(offset => agentRequest.DateFrom.AddHours(offset)).ToList();
                                        maxPeriods = Convert.ToInt32(maxSpan.Value.TotalHours);
                                        break;
                                    case "day":
                                        lstDateRange = Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(agentRequest.DateTo.Subtract(agentRequest.DateFrom).TotalDays))).Select(offset => agentRequest.DateFrom.AddDays(offset)).ToList();
                                        maxPeriods = maxSpan.Value.Days;
                                        break;
                                    case "month":
                                        var startDate = new DateTime(agentRequest.DateFrom.Year, agentRequest.DateFrom.Month, 1);
                                        var months = ((agentRequest.DateTo.Year - agentRequest.DateFrom.Year) * 12) + agentRequest.DateTo.Month - agentRequest.DateFrom.Month;
                                        lstDateRange = Enumerable.Range(0, months).Select(offset => startDate.AddMonths(offset)).ToList();
                                        maxPeriods = ((maxSpanAgent.DateTo.Year - maxSpanAgent.DateFrom.Year) * 12) + maxSpanAgent.DateTo.Month - maxSpanAgent.DateFrom.Month;
                                        break;
                                }

                                dictAgentDateRanges.Add(agentRequest.ID, lstDateRange);
                            }

                            switch (graphRequest.SubTab)
                            {
                                case "gender":
                                    // Create a series for male + female
                                    Series maleSeries = new Series();
                                    maleSeries.data = new List<HighChartDatum>();
                                    maleSeries.name = "male";

                                    Series femaleSeries = new Series();
                                    femaleSeries.data = new List<HighChartDatum>();
                                    femaleSeries.name = "female";

                                    // For each date in date range create points for returnSum audience by gender in series
                                    // All agents should be combined for each date
                                    for (int i = 0; i <= maxPeriods; i++)
                                    {
                                        var summariesForDate = new List<AnalyticsSummaryModel>();
                                        DateTime datumDT = new DateTime();

                                        foreach (KeyValuePair<long, List<DateTime>> agentDateRange in dictAgentDateRanges)
                                        {
                                            if (i < agentDateRange.Value.Count)
                                            {
                                                if (graphRequest.PageType == "campaign")
                                                {
                                                    summariesForDate.AddRange(
                                                        agentSummaryList.Where(summary =>
                                                            summary.CampaignID == agentDateRange.Key &&
                                                            summary.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                        )
                                                    );
                                                }
                                                else
                                                {
                                                    // Add to summariesForDate list summaries where agentDateRange key  == summary SRID and on datetime
                                                    summariesForDate.AddRange(
                                                        agentSummaryList.Where(summary => 
                                                            summary.SearchRequestID == agentDateRange.Key && 
                                                            summary.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                        )
                                                    );
                                                }
                                                datumDT = agentDateRange.Value[i];
                                            }
                                        }
                                        long? maleSum = summariesForDate.Sum(summary => summary.MaleAudience);
                                        long? femaleSum = summariesForDate.Sum(summary => summary.FemaleAudience);

                                        HighChartDatum maleDatum = new HighChartDatum();
                                        maleDatum.y = maleSum == null ? 0 : maleSum;
                                        maleDatum.SearchTerm = "";
                                        maleDatum.Value = "male";
                                        maleDatum.Type = "Media";
                                        maleDatum.Date = datumDT.ToShortDateString();
                                        maleSeries.data.Add(maleDatum);

                                        HighChartDatum femaleDatum = new HighChartDatum();
                                        femaleDatum.y = femaleSum == null ? 0 : femaleSum;
                                        femaleDatum.SearchTerm = "";
                                        femaleDatum.Value = "female";
                                        femaleDatum.Type = "Media";
                                        femaleDatum.Date = datumDT.ToShortDateString();
                                        femaleSeries.data.Add(femaleDatum);
                                    }
                                    seriesList.Add(maleSeries);
                                    seriesList.Add(femaleSeries);
                                    break;
                                case "age":
                                    Series s18_24 = new Series();
                                    s18_24.data = new List<HighChartDatum>();
                                    s18_24.name = "18-24";

                                    Series s25_34 = new Series();
                                    s25_34.data = new List<HighChartDatum>();
                                    s25_34.name = "25-34";

                                    Series s35_49 = new Series();
                                    s35_49.data = new List<HighChartDatum>();
                                    s35_49.name = "35-49";

                                    Series s50_54 = new Series();
                                    s50_54.data = new List<HighChartDatum>();
                                    s50_54.name = "50-54";

                                    Series s55_64 = new Series();
                                    s55_64.data = new List<HighChartDatum>();
                                    s55_64.name = "55-64";

                                    Series s65_Plus = new Series();
                                    s65_Plus.data = new List<HighChartDatum>();
                                    s65_Plus.name = "65+";

                                    for (int i = 0; i <= maxPeriods; i++)
                                    {
                                        var summariesForDate = new List<AnalyticsSummaryModel>();
                                        DateTime datumDT = new DateTime();
                                        foreach (KeyValuePair<long, List<DateTime>> agentDateRange in dictAgentDateRanges)
                                        {
                                            if (i < agentDateRange.Value.Count)
                                            {
                                                if (graphRequest.PageType == "campaign")
                                                {
                                                    summariesForDate.AddRange(
                                                        agentSummaryList.Where(summary =>
                                                            summary.CampaignID == agentDateRange.Key &&
                                                            summary.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                        )
                                                    );
                                                }
                                                else
                                                {
                                                    summariesForDate.AddRange(
                                                        agentSummaryList.Where(summary =>
                                                            summary.SearchRequestID == agentDateRange.Key &&
                                                            summary.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                        )
                                                    );
                                                }
                                                summariesForDate.AddRange(agentSummaryList.Where(summary => summary.SearchRequestID == agentDateRange.Key && summary.SummaryDateTime.Equals(agentDateRange.Value[i])));
                                                datumDT = agentDateRange.Value[i];
                                            }
                                        }

                                        s18_24.data.Add(new HighChartDatum() {
                                            y = summariesForDate.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24),
                                            Date = datumDT.ToShortDateString(),
                                            Value = "18-24"
                                        });
                                        s25_34.data.Add(new HighChartDatum() {
                                            y = summariesForDate.Sum(s => s.AM25_34 + s.AF25_34),
                                            Date = datumDT.ToShortDateString(),
                                            Value = "25-34"
                                        });
                                        s35_49.data.Add(new HighChartDatum() {
                                            y = summariesForDate.Sum(s => s.AM35_49 + s.AF35_49),
                                            Date = datumDT.ToShortDateString(),
                                            Value = "35-49"
                                        });
                                        s50_54.data.Add(new HighChartDatum() {
                                            y = summariesForDate.Sum(s => s.AM50_54 + s.AF50_54),
                                            Date = datumDT.ToShortDateString(),
                                            Value = "50-54"
                                        });
                                        s55_64.data.Add(new HighChartDatum() {
                                            y = summariesForDate.Sum(s => s.AM55_64 + s.AF55_64),
                                            Date = datumDT.ToShortDateString(),
                                            Value = "55-64"
                                        });
                                        s65_Plus.data.Add(new HighChartDatum() {
                                            y = summariesForDate.Sum(s => s.AM65_Plus + s.AF65_Plus),
                                            Date = datumDT.ToShortDateString(),
                                            Value = "65+"
                                        });
                                    }

                                    seriesList.Add(s18_24);
                                    seriesList.Add(s25_34);
                                    seriesList.Add(s35_49);
                                    seriesList.Add(s50_54);
                                    seriesList.Add(s55_64);
                                    seriesList.Add(s65_Plus);
                                    break;
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    case SecondaryTabID.Sources:    //Each source/submedia type is a series
                        if (graphRequest.AgentList.Count > 0)
                        {
                            // Get a list of each requested agent's date range, since in comparison mode they won't be the same
                            Dictionary<long, List<DateTime>> dictAgentDateRanges = new Dictionary<long, List<DateTime>>();
                            List<DateTime> lstDateRange = new List<DateTime>();
                            int maxPeriods = 0;
                            foreach (AnalyticsAgentRequest agentRequest in graphRequest.AgentList)
                            {
                                switch (graphRequest.DateInterval)
                                {
                                    case "hour":
                                        lstDateRange = Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(agentRequest.DateTo.Subtract(agentRequest.DateFrom).TotalHours))).Select(offset => agentRequest.DateFrom.AddHours(offset)).ToList();
                                        maxPeriods = Convert.ToInt32(maxSpan.Value.TotalHours);
                                        break;
                                    case "day":
                                        lstDateRange = Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(agentRequest.DateTo.Subtract(agentRequest.DateFrom).TotalDays))).Select(offset => agentRequest.DateFrom.AddDays(offset)).ToList();
                                        maxPeriods = maxSpan.Value.Days;
                                        break;
                                    case "month":
                                        var startDate = new DateTime(agentRequest.DateFrom.Year, agentRequest.DateFrom.Month, 1);
                                        var months = ((agentRequest.DateTo.Year - agentRequest.DateFrom.Year) * 12) + agentRequest.DateTo.Month - agentRequest.DateFrom.Month;
                                        lstDateRange = Enumerable.Range(0, months).Select(offset => startDate.AddMonths(offset)).ToList();
                                        maxPeriods = ((maxSpanAgent.DateTo.Year - maxSpanAgent.DateFrom.Year) * 12) + maxSpanAgent.DateTo.Month - maxSpanAgent.DateFrom.Month;
                                        break;
                                }

                                dictAgentDateRanges.Add(agentRequest.ID, lstDateRange);
                            }

                            var smtGroups = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.SubMediaType)).GroupBy(gb => gb.SubMediaType).OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits));
                            foreach (var series in smtGroups)
                            {
                                //Log4NetLogger.Debug(string.Format("series.name: {0}", series.First().SMTDisplayName));
                                Series smtSeries = new Series() {
                                    name = lstSubMediaTypes.Where(smt => smt.SubMediaType == series.First().SubMediaType).First().DisplayName,
                                    data = new List<HighChartDatum>()
                                };

                                for (int i = 0; i <= maxPeriods; i++)
                                {
                                    var summariesForDate = new List<AnalyticsSummaryModel>();
                                    DateTime datumDT = new DateTime();

                                    foreach (KeyValuePair<long, List<DateTime>> agentDateRange in dictAgentDateRanges)
                                    {
                                        if (i < agentDateRange.Value.Count)
                                        {
                                            if (graphRequest.PageType == "campaign")
                                            {
                                                summariesForDate.AddRange(
                                                    series.Where(summary =>
                                                        summary.CampaignID == agentDateRange.Key &&
                                                        summary.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                    )
                                                );
                                            }
                                            else
                                            {
                                                summariesForDate.AddRange(
                                                    series.Where(summary =>
                                                        summary.SearchRequestID == agentDateRange.Key &&
                                                        summary.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                    )
                                                );
                                            }
                                            datumDT = agentDateRange.Value[i];
                                        }
                                    }

                                    HighChartDatum hcDatum = new HighChartDatum() {
                                        y = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, summariesForDate),
                                        Date = datumDT.ToShortDateString(),
                                        Value = series.First().SubMediaType
                                    };
                                    smtSeries.data.Add(hcDatum);
                                }

                                seriesList.Add(smtSeries);
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    case SecondaryTabID.Market:     // Each market is a series
                        if (graphRequest.AgentList.Count > 0)
                        {
                            // Get a list of each requested agent's date range, since in comparison mode they won't be the same
                            Dictionary<long, List<DateTime>> dictAgentDateRanges = new Dictionary<long, List<DateTime>>();
                            List<DateTime> lstDateRange = new List<DateTime>();
                            int maxPeriods = 0;
                            foreach (AnalyticsAgentRequest agentRequest in graphRequest.AgentList)
                            {
                                switch (graphRequest.DateInterval)
                                {
                                    case "hour":
                                        lstDateRange = Enumerable.Range(0, 
                                            Convert.ToInt32(Math.Ceiling(agentRequest.DateTo.Subtract(agentRequest.DateFrom).TotalHours))).Select(offset => agentRequest.DateFrom.AddHours(offset)).ToList();
                                        maxPeriods = Convert.ToInt32(maxSpan.Value.TotalHours);
                                        break;
                                    case "day":
                                        lstDateRange = Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(agentRequest.DateTo.Subtract(agentRequest.DateFrom).TotalDays))).Select(offset => agentRequest.DateFrom.AddDays(offset)).ToList();
                                        maxPeriods = maxSpan.Value.Days;
                                        break;
                                    case "month":
                                        var startDate = new DateTime(agentRequest.DateFrom.Year, agentRequest.DateFrom.Month, 1);
                                        var months = ((agentRequest.DateTo.Year - agentRequest.DateFrom.Year) * 12) + agentRequest.DateTo.Month - agentRequest.DateFrom.Month;
                                        lstDateRange = Enumerable.Range(0, months).Select(offset => startDate.AddMonths(offset)).ToList();
                                        maxPeriods = ((maxSpanAgent.DateTo.Year - maxSpanAgent.DateFrom.Year) * 12) + maxSpanAgent.DateTo.Month - maxSpanAgent.DateFrom.Month;
                                        break;
                                }

                                dictAgentDateRanges.Add(agentRequest.ID, lstDateRange);
                            }

                            // Create a series for each market
                            var listMarkets = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.Market)).Select(asl => asl.Market).Distinct();
                            var dictDMAs = GetAllDMAs();
                            var marketGroups = agentSummaryList.Where(asl => !string.IsNullOrEmpty(asl.Market)).GroupBy(gb => gb.Market).OrderByDescending(ob => ob.Sum(s => s.Number_Of_Hits));

                            var count = 0;
                            foreach (var series in marketGroups)
                            {
                                if (count < 15)
                                {
                                    Series marketSeries = new Series() {
                                        name = series.First().Market,
                                        data = new List<HighChartDatum>()
                                    };

                                    long marketSum = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, series);
                                    var marketDMA = dictDMAs.Where(dma => dma.Value == series.First().Market);
                                    string marketID = marketDMA.Any() ? marketDMA.First().Key : "";

                                    for (int i = 0; i <= maxPeriods; i++)
                                    {
                                        var summariesForDate = new List<AnalyticsSummaryModel>();
                                        DateTime datumDT = new DateTime();

                                        foreach (KeyValuePair<long, List<DateTime>> agentDateRange in dictAgentDateRanges)
                                        {
                                            if (i < agentDateRange.Value.Count)
                                            {
                                                if (graphRequest.PageType == "campaign")
                                                {
                                                    var ms = series.Where(summ =>
                                                        summ.CampaignID == agentDateRange.Key &&
                                                        summ.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                    );
                                                    summariesForDate.AddRange(ms);
                                                }
                                                else
                                                {
                                                    var ms = series.Where(summ =>
                                                        summ.SearchRequestID == agentDateRange.Key &&
                                                        summ.SummaryDateTime.Equals(agentDateRange.Value[i])
                                                    );
                                                    summariesForDate.AddRange(ms);
                                                }
                                                datumDT = agentDateRange.Value[i];
                                            }
                                        }

                                        HighChartDatum hcDatum = new HighChartDatum() {
                                            y = GetSumsFromSummaries(PESHTypes, lstSubMediaTypes, peshFilters, summariesForDate),
                                            Date = datumDT.ToShortDateString(),
                                            Value = marketID.ToString()
                                        };
                                        marketSeries.data.Add(hcDatum);
                                    }

                                    seriesList.Add(marketSeries);
                                    count += 1;
                                }
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    default:
                        break;
                }

                chartOutput.series = seriesList;

                return CommonFunctions.SearializeJson(chartOutput);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
            }
            return string.Empty;
        }

        public Dictionary<string, object> GetChart(AnalyticsGraphRequest graphRequest, AnalyticsSecondaryTable table, List<AnalyticsSummaryModel> selectedSummaries, Dictionary<string, string> allGroups)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("GetChart"));
                Stopwatch sw = new Stopwatch();
                sw.Start();

                List<AnalyticsGrouping> groupings = new List<AnalyticsGrouping>();
                if (graphRequest.Tab != SecondaryTabID.Demographic)
                {
                    PropertyInfo groupByPI = typeof(AnalyticsSummaryModel).GetProperty(table.GroupBy);
                    PropertyInfo gropuByDisplayPI = typeof(AnalyticsSummaryModel).GetProperty(table.GroupByDisplay);

                    // Remove all summaries without a value for groupByPI - if left in they cause problems
                    selectedSummaries = selectedSummaries.Where(w => groupByPI.GetValue(w, null) != null).ToList();

                    foreach (var group in allGroups)
                    {
                        var groupSummaries = selectedSummaries.Where(w => groupByPI.GetValue(w, null).ToString() == group.Key).ToList();
                        groupings.Add(new AnalyticsGrouping() {
                            ID = group.Key,
                            Name = group.Value,
                            Summaries = groupSummaries
                        });
                    }
                }
                else
                {
                    groupings.Add(new AnalyticsGrouping() {
                        ID = "demographic",
                        Name = "demographic",
                        Summaries = selectedSummaries
                    });
                }

                Dictionary<string, object> seriesAndChart = new Dictionary<string, object>();
                switch (graphRequest.HCChartType)
                {
                    case HCChartTypes.bar:
                    case HCChartTypes.column:
                        seriesAndChart = CreateBarOrColumnChart(groupings, graphRequest);
                        break;
                    case HCChartTypes.spline:
                        seriesAndChart = CreateLineChart(groupings, graphRequest);
                        break;
                    case HCChartTypes.pie:
                        seriesAndChart = CreatePieChart(groupings, graphRequest);
                        break;
                    case HCChartTypes.heatmap:
                        seriesAndChart = CreateHeatMap(groupings, graphRequest);
                        break;
                }

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetChart: {0} ms", sw.ElapsedMilliseconds));

                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        private Dictionary<string, object> CreateBarOrColumnChart(List<AnalyticsGrouping> groupings, AnalyticsGraphRequest request)
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
                            text = "Occurrences",
                            rotation = request.HCChartType == HCChartTypes.bar ? 0 : 270
                        },
                        min = 0
                    },
                    tooltip = new Tooltip() {
                        valueSuffix = string.Empty
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
                        allSummaries.AddRange(e.Summaries);
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
                            y = group.Summaries.Any() ? group.Summaries.Sum(s => s.Number_Of_Hits) : 0,
                            SearchTerm = group.Name,
                            Value = group.ID
                        };

                        groupSeries.data.Add(hcDatum);
                        seriesList.Add(groupSeries);
                        if (count < 5)
                        {
                            barOrColumn.series.Add(groupSeries);
                        }
                        count += 1;
                    }
                }

                Dictionary<string, object> seriesAndChart = new Dictionary<string, object>();
                seriesAndChart.Add("series", seriesList);
                seriesAndChart.Add("chart", CommonFunctions.SearializeJson(barOrColumn));

                sw.Stop();
                //Log4NetLogger.Debug(string.Format("GetNewBarChart: {0} ms", sw.ElapsedMilliseconds));

                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        private Dictionary<string, object> CreateLineChart(List<AnalyticsGrouping> groupings, AnalyticsGraphRequest request)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Log4NetLogger.Debug("CreateLineChart");
                List<DateTime> distinctDates = new List<DateTime>();
                List<string> xAxisValues = GetChartXAxisValues(request, out distinctDates);
                List<Series> seriesList = new List<Series>();

                string chartTitle;
                if (request.chartType == ChartType.Growth)
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
                            rotation = 0
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
                        formatter = request.GraphType == "comparison" ? "FormatComparisonTooltip" : ""
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

                if (request.chartType == ChartType.Growth)
                {
                    lineChart.tooltip.pointFormat = "<span style=\"color:{series.color}\">\u25CF </span>{series.name}: <b>{point.y:.2f}%</b>";
                }

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

                        long malePrevDaySum = 0;
                        long femalePrevDaySum = 0;

                        foreach (var date in distinctDates)
                        {
                            var summariesForDate = allSummaries.Where(w => w.SummaryDateTime.Equals(date));
                            long maleDaySum = summariesForDate.Any() ? summariesForDate.Sum(s => s.MaleAudience) : 0;
                            long femaleDaySum = summariesForDate.Any() ? summariesForDate.Sum(s => s.FemaleAudience) : 0;
                            decimal malePctChange = 0;
                            decimal femalePctChange = 0;

                            if (malePrevDaySum != 0)
                            {
                                malePctChange = ((maleDaySum - malePrevDaySum) / (decimal)malePrevDaySum) * 100;
                            }
                            malePrevDaySum = maleDaySum;
                            if (femalePrevDaySum != 0)
                            {
                                femalePctChange = ((femaleDaySum - femalePrevDaySum) / (decimal)femalePrevDaySum) * 100;
                            }
                            femalePrevDaySum = femaleDaySum;

                            HighChartDatum maleDatum = new HighChartDatum() {
                                Date = date.ToShortDateString(),
                                Value = "male",
                                SearchName = "male",
                                y = request.chartType == ChartType.Growth ? malePctChange : maleDaySum
                            };
                            maleSeries.data.Add(maleDatum);
                            HighChartDatum femaleDatum = new HighChartDatum() {
                                Date = date.ToShortDateString(),
                                Value = "female",
                                SearchName = "female",
                                y = request.chartType == ChartType.Growth ? femalePctChange : femaleDaySum
                            };
                            femaleSeries.data.Add(femaleDatum);
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

                        foreach (var date in distinctDates)
                        {
                            var summariesForDate = allSummaries.Where(w => w.SummaryDateTime.Equals(date));
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
                                y = request.chartType == ChartType.Growth ? s18_24PctChange : s18_24Sum
                            };
                            s18_24.data.Add(s18_24HCD);

                            HighChartDatum s25_34HCD = new HighChartDatum() {
                                Date = date.ToShortDateString(),
                                Value = s25_34.name,
                                SearchName = s25_34.name,
                                y = request.chartType == ChartType.Growth ? s25_34PctChange : s25_34Sum
                            };
                            s25_34.data.Add(s25_34HCD);

                            HighChartDatum s35_49HCD = new HighChartDatum() {
                                Date = date.ToShortDateString(),
                                Value = s35_49.name,
                                SearchName = s35_49.name,
                                y = request.chartType == ChartType.Growth ? s35_49PctChange : s35_49Sum
                            };
                            s35_49.data.Add(s35_49HCD);

                            HighChartDatum s50_54HCD = new HighChartDatum() {
                                Date = date.ToShortDateString(),
                                Value = s50_54.name,
                                SearchName = s50_54.name,
                                y = request.chartType == ChartType.Growth ? s50_54PctChange : s50_54Sum
                            };
                            s50_54.data.Add(s50_54HCD);

                            HighChartDatum s55_64HCD = new HighChartDatum() {
                                Date = date.ToShortDateString(),
                                Value = s55_64.name,
                                SearchName = s55_64.name,
                                y = request.chartType == ChartType.Growth ? s55_64PctChange : s55_64Sum
                            };
                            s55_64.data.Add(s55_64HCD);

                            HighChartDatum s65_PlusHCD = new HighChartDatum() {
                                Date = date.ToShortDateString(),
                                Value = s65_Plus.name,
                                SearchName = s65_Plus.name,
                                y = request.chartType == ChartType.Growth ? s65_PlusPctChange : s65_PlusSum
                            };
                            s65_Plus.data.Add(s65_PlusHCD);
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
                            name = group.Name
                        };

                        long prevDaySum = 0;

                        foreach (var date in distinctDates)
                        {
                            var summariesForDate = group.Summaries.Where(w => w.SummaryDateTime.Equals(date));
                            long daySum = summariesForDate.Any() ? summariesForDate.Sum(s => s.Number_Of_Hits) : 0;
                            decimal pctChange = 0;
                            if (prevDaySum != 0)
                            {
                                pctChange = ((daySum - prevDaySum) / (decimal)prevDaySum) * 100;
                            }
                            prevDaySum = daySum;

                            HighChartDatum hcd = new HighChartDatum() {
                                Date = date.ToShortDateString(),
                                Type = "Media",
                                Value = group.ID,
                                SearchName = group.Name,
                                y = request.chartType == ChartType.Growth ? pctChange : daySum
                            };

                            groupSeries.data.Add(hcd);
                        }

                        seriesList.Add(groupSeries);
                        if (count < 5)
                        {
                            lineChart.series.Add(groupSeries);
                        }
                        count += 1;
                    }
                }

                Dictionary<string, object> seriesAndChart = new Dictionary<string,object>();
                seriesAndChart.Add("series", seriesList);
                seriesAndChart.Add("chart", CommonFunctions.SearializeJson(lineChart));
                sw.Stop();
                Log4NetLogger.Debug(string.Format("CreateLineChart: {0} ms", sw.ElapsedMilliseconds));
                return seriesAndChart;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new Dictionary<string, object>();
            }
        }

        private Dictionary<string, object> CreatePieChart(List<AnalyticsGrouping> groupings, AnalyticsGraphRequest request)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Log4NetLogger.Debug("CreatePieChart");
                Log4NetLogger.Debug(string.Format("groupings.Count: {0}", groupings.Count));
                // Series list holds all series
                List<object> seriesList = new List<object>();
                List<object> sliceList = new List<object>();

                HighPieChartModel pieChart = new HighPieChartModel() {
                    chart = new PChart(),
                    title = new PTitle() {
                        text = "Occurrences",
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
                    tooltip = new PTooltip() {
                        pointFormat = "<span style=\"color:{point.color}\">\u25CF </span>{point.name}: <b>{point.y}/{series.total:,.0f} = {point.percentage:.2f}%</b> of total"
                    },
                    series = new List<PSeries>()
                };

                if (request.Tab == SecondaryTabID.Demographic)
                {
                    List<AnalyticsSummaryModel> allSummaries = new List<AnalyticsSummaryModel>();
                    groupings.ForEach(e => {
                        allSummaries.AddRange(e.Summaries);
                    });

                    if (request.SubTab == "gender")
                    {
                        var maleSeries = new object[] {
                            "male",
                            Convert.ToInt64(allSummaries.Sum(s => s.MaleAudience))
                        };
                        seriesList.Add(maleSeries);
                        sliceList.Add(maleSeries);

                        var femaleSeries = new object[] {
                            "female",
                            Convert.ToInt64(allSummaries.Sum(s => s.FemaleAudience))
                        };
                        seriesList.Add(femaleSeries);
                        sliceList.Add(femaleSeries);
                    }
                    else
                    {
                        var s18_24 = new object[] {
                            "18-24",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM18_20 + s.AM21_24 + s.AF18_20 + s.AF21_24))
                        };
                        seriesList.Add(s18_24);
                        sliceList.Add(s18_24);

                        var s25_34 = new object[] {
                            "25-34",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM25_34 + s.AF25_34))
                        };
                        seriesList.Add(s25_34);
                        sliceList.Add(s25_34);

                        var s35_49 = new object[] {
                            "35-49",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM35_49 + s.AF35_49))
                        };
                        seriesList.Add(s35_49);
                        sliceList.Add(s35_49);

                        var s50_54 = new object[] {
                            "50-54",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM50_54 + s.AF50_54))
                        };
                        seriesList.Add(s50_54);
                        sliceList.Add(s50_54);

                        var s55_64 = new object[] {
                            "55-64",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM55_64 + s.AF55_64))
                        };
                        seriesList.Add(s55_64);
                        sliceList.Add(s55_64);

                        var s65_Plus = new object[] {
                            "65+",
                            Convert.ToInt64(allSummaries.Sum(s => s.AM65_Plus + s.AF65_Plus))
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
                        seriesList.Add(new object[]{
                            group.Name,
                            Convert.ToInt64(group.Summaries.Sum(s => s.Number_Of_Hits))
                        });

                        if (count < 5)
                        {
                            sliceList.Add(new object[]{
                                group.Name,
                                Convert.ToInt64(group.Summaries.Sum(s => s.Number_Of_Hits))
                            });
                        }
                        count += 1;
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

        private Dictionary<string, object> CreateHeatMap(List<AnalyticsGrouping> groupings, AnalyticsGraphRequest request)
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
                            value = group.Summaries.Sum(s => s.Number_Of_Hits),
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
                            var dayPart = dayPartData.Any() ? dayPartData.First(dp => dp.DayOfWeek.Equals(day) && dp.HourOfDay.Equals(hour)) : new DayPartDataItem();
                            HeatMapDatum hcd = new HeatMapDatum() {
                                value = summsForPart.Any() ? summsForPart.Sum(s => s.Number_Of_Hits) : 0,
                                borderColor = "#cccccc",
                                borderWidth = 1,
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

        #endregion

    }
}

