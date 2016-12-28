using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMedia.Model;

namespace IQMedia.WebApplication.Models
{
    public class DiscoveryResult
    {
        public bool isSuccess { get; set; }
        public bool hasMoreResults { get; set; }
        public string searchedTerm { get; set; }
        public string availableData { get; set; }

        public long searchTermTotalRecords { get; set; }
        public long? searchTermShownRecords { get; set; }
        public int searchTermAvailableRecords { get; set; }
        public int displayPageSize { get; set; }

        public bool isAnyDataAvailable { get; set; }
        public short searchedIndex { get; set; }
        public string HTML { get; set; }
    }

    public class LibraryResult
    {        
        public bool isSuccess { get; set; }
        public bool hasMoreResults { get; set; }        
        public string HTML { get; set; }
        public string totalRecords { get; set; }
        public string currentRecords { get; set; }
        public string errorMessage { get; set; }
        public IQArchive_FilterModel filter { get; set; }
        public IQ_ReportSettingsModel reportSettings { get; set; }
        public long? reportImageId { get; set; }
        public string totalRecordsDisplay { get; set; }
        public string currentRecordsDisplay { get; set; }
        public string archiveIDs { get; set; }
        public string title { get; set; }
        public bool reportHasCustomSort { get; set; }
    }

    public class FeedsResult
    {
        public string html { get; set; }
        public bool hasMoreResults { get; set; }
        public FeedsFilterModel filter { get; set; }
        public string totalRecords { get; set; }
        public string totalRecordsDisplay { get; set; }
        public string currentRecords { get; set; }
        public bool isSuccess { get; set; }
        public string errorMessage { get; set; }
        public string currentRecordsDisplay { get; set; }
        public bool isAllDeleted { get; set; }
        public List<string> listofTVResultID { get; set; }
        public bool isValidResponse { get; set; }
        public bool isReadLimitExceeded { get; set; }
    }

    public class DashboardOverviewResults
    {
        public long SumTVRecord { get; set; }
        public long PrevSumTVRecord { get; set; }
        public long SumNMRecord { get; set; }
        public long PrevSumNMRecord { get; set; }
        public long SumTWRecord { get; set; }
        public long PrevSumTWRecord { get; set; }
        public long SumForumRecord { get; set; }
        public long PrevSumForumRecord { get; set; }
        public long SumSocialMRecord { get; set; }
        public long PrevSumSocialMRecord { get; set; }
        public long SumBlogRecord { get; set; }
        public long PrevSumBlogRecord { get; set; }
        public long SumAudienceRecord { get; set; }
        public long PrevSumAudienceRecord { get; set; }
        public decimal SumIQMediaValueRecord { get; set; }
        public decimal PrevSumIQMediaValueRecord { get; set; }
        public long SumPMRecord { get; set; }
        public long PrevSumPMRecord { get; set; }
        public long SumTMRecord { get; set; }
        public long PrevSumTMRecord { get; set; }
        public long SumMSRecord { get; set; }
        public string TotNumOfHits { get; set; }
        public bool IsprevSummaryEnoughData { get; set; }
    }

    public class DashboardMediaResults
    {
        public long SumNegativeSentiment { get; set; }
        public long PrevSumNegativeSentiment { get; set; }

        public long SumPositiveSentiment { get; set; }
        public long PrevSumPositiveSentiment { get; set; }

        public long SumHits { get; set; }
        public long PrevSumHits { get; set; }

        public long SumAudience { get; set; }
        public long PrevSumAudience { get; set; }

        public decimal SumIQMediaValue { get; set; }
        public decimal PrevSumIQMediaValue { get; set; }

        public long SumAirSeconds { get; set; }
        public long PrevSumAirSeconds { get; set; }

        public bool IsprevSummaryEnoughData { get; set; }

        public bool ShowCanadaMap { get; set; }
    }
}