using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class AnalyticsAgent
    {
        public Int64 ID { get; set; }
        public string QueryName { get; set; }
        public Guid ClientGUID { get; set; }
        public string SearchTerm { get; set; }
        public DateTime DayDate { get; set; }
        public string MediaType { get; set; }
        public Int64 NoOfDocs { get; set; }
        public Int64 NoOfHits { get; set; }
        public Int64 Audience { get; set; }
        public Decimal IQMediaValue { get; set; }
        public Int64 PositiveSentiment { get; set; }
        public Int64 NegativeSentiment { get; set; }
    }

    /// <summary>
    /// Model used to send a request for an agent by an ID, a date range, and a media type
    /// Need string versions of dates because on passing datetime objects from page to server, dates will automatically be converted to EST from client TZ upon de-serialization, so need string without associated time zone information to ensure data is returned for day/month/etc. summaries as those summaries are tied to 12 AM of that day/month/etc. and when server converted from client TZ to EST, unless client TZ was EST, the time would be offset from 12 by however many hours away from EST that the client TZ was
    /// </summary>
    public class AnalyticsAgentRequest
    {
        public Int64 ID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateFromGMT { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime DateToGMT { get; set; }
        public string MediaType { get; set; }
    }

    public class AnalyticsGraphRequest
    {
        public SecondaryTabID Tab { get; set; }
        public string SubTab { get; set; }
        public string GraphType { get; set; }
        public string DateInterval { get; set; }
        public List<AnalyticsAgentRequest> AgentList { get; set; }
        public ChartType chartType { get; set; }
        public string PageType { get; set; }
        public HCChartTypes HCChartType { get; set; }
    }

    public class AnalyticsSecondaryModel
    {
        public List<string> Headers { get; set; }

        public List<List<AnalyticsSecondaryDetailModel>> secondaryDetailList { get; set; }
    }

    public class AnalyticsSecondaryDetailModel
    {
        public SpecialCase SpecialCaseOption { get; set; }

        public Alignment AlignmentOption { get; set; }

        public string ContentOne { get; set; }

        public string ContentTwo { get; set; }

        public string Link { get; set; }
    }

    public class AnalyticsPESHFilters
    {
        public bool isFiltering { get; set; }
        public bool isOnAir { get; set; }
        public bool isOnline { get; set; }
        public bool isPrint { get; set; }
        public bool isSeenEarned { get; set; }
        public bool isSeenPaid { get; set; }
        public bool isHeardEarned { get; set; }
        public bool isHeardPaid { get; set; }
        public bool isEarned { get; set; }
        public bool isRead { get; set; }
    }

    public class AnalyticsSecondaryTable
    {
        public string GroupByHeader { get; set; }
        public List<string> ColumnHeaders { get; set; }
        public List<string> ColumnHeadersAds { get; set; }
        public List<string> ColumnHeadersLR { get; set; }
        public List<string> ColumnHeadersAdsLR { get; set; }
        public string GroupBy { get; set; }
        public string GroupByDisplay { get; set; }
        public string TabDisplay { get; set; }
        public string PageType { get; set; }
    }

    public class AnalyticsAgeRange
    {
        public string AgeRange { get; set; }
        public Int64 MaleAudience { get; set; }
        public Int64 FemaleAudience { get; set; }
        public Int64 TotalAudience { get; set; }
    }

    /// <summary>
    /// Used to organize summaries into groups and associate these summaries with a name and ID
    /// </summary>
    public class AnalyticsGrouping
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public List<AnalyticsSummaryModel> Summaries { get; set; }
    }

    [Serializable]
    public class AnalyticsCampaign
    {
        public Int64 CampaignID { get; set; }
        public Int64 SearchRequestID { get; set; }
        public string CampaignName { get; set; }
        public string QueryName { get; set; }
        public string QueryVersion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AnalyticsSummaryModel CampaignSummary { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Int64 IsActive { get; set; }
    }

    public class AnalyticsActiveElement
    {
        public string ActivePage { get; set; }
        public string ElementSelector { get; set; }
        public List<string> ActiveTabs { get; set; }
        public bool IsActiveWithPESH { get; set; }
        public bool IsActiveWithMaps { get; set; }
        public bool IsActiveWithLineCharts { get; set; }
        public bool IsActiveWithOtherCharts { get; set; }
        public List<string> HiddenTabs { get; set; }
    }

    public enum ChartType
    {
        Bar,
        Column,
        Line,
        Pie,
        Daytime,
        Growth,
        Daypart
    }

    public enum SecondaryTabID
    {
        OverTime,
        Demographic,
        Daytime,
        Sources,
        Daypart,
        Market
    }

    public enum SpecialCase
    {
        None,
        Checkbox,
        Sentiment
    }

    public enum Alignment
    {
        Left,
        Center,
        Right
    }

    public enum AnalyticsGraphType
    {
        line = 1,
        us = 2,
        canada = 3,
        pie = 4,
        bar = 5,
        column = 6,
        heat = 7,
        growth = 8
    }

    public enum HCChartTypes
    {
        area,
        arearange,
        areaspline,
        areasplinerange,
        bar,
        boxplot,
        bubble,
        column,
        columnrange,
        errorbar,
        funnel,
        gauge,
        heatmap,
        line,
        pie,
        polygon,
        pyramid,
        scatter,
        solidgauge,
        spline,
        treemap,
        waterfall
    }
}
