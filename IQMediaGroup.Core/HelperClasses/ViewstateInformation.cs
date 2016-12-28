using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
using PMGSearch;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    public class ViewstateInformation
    {
        /// <summary>
        /// Contains the Sort Expression
        /// </summary>
        public string SortExpression { get; set; }

        /// <summary>
        /// Contains the Sort Direction
        /// </summary>
        public string SortDirection { get; set; }

        /// <summary>
        /// Represents Total no of RawMedia PMGSearchDLL
        /// </summary>
        public int RawMediaTotalHitsCount { get; set; }

        /// <summary>
        /// Represents Current Page
        /// </summary>
        public int CurrentRawMediaPage { get; set; }

        /// <summary>
        /// Represents ListOfRawMedia from response
        /// </summary>
        public List<RawMedia> _IQAdvanceResponseListOfRawMedia { get; set; }

        public string MailMessage { get; set; }

        /// <summary>
        /// Represents Current DB Page
        /// </summary>
        public int CurrentIQAgentPage { get; set; }

        /// <summary>
        /// Represents List of objects of IQAgentResults
        /// </summary>
        public List<IQAgentResults> _ListOfIQAgentResultsFromDB { get; set; }

        /// <summary>
        /// Represents Total IQAgentResults Count
        /// </summary>
        public int TotalIQAgentResultsCount { get; set; }

        /// <summary>
        /// Represents ListOfRadioStations
        /// </summary>
        public List<IQ_STATION> ListOfRadioStations { get; set; }

        public Int64 TotalRadioStationsCountDB { get; set; }

        public List<RadioStation> ListOfRadioStation { get; set; }

        public bool IsSortDirecitonAsc { get; set; }

        public List<ArchiveClip> _ListOfArchiveClipSearch { get; set; }

        public int? TotalRecordsCountArchiveClip { get; set; }

        public string _ClipSearchTerm { get; set; }

        public int? _CurrentArchiveClipSearchPage { get; set; }

        public List<CustomCategory> ListOfCustomCategory { get; set; }

        public int? TotalRecordsCountUGCRawMedia { get; set; }

        public int? _CurrentUGCRawMediaPage { get; set; }

        public List<Customer> ListOfUGCCustomer { get; set; }

        public string ClipSortExpression { get; set; }

        public int? _CurrentClipPage { get; set; }

        public int? TotalRecordsCountClip { get; set; }

        public bool IsClipSortDirecitonAsc { get; set; }

        public bool? IsmyIQSearchActive { get; set; }

        public bool? IsIQCustomSearchActive { get; set; }

        public string IFrameMicroSiteSearchText { get; set; }

        public string FtpUrl { get; set; }

        public string FtpFilePath { get; set; }

        public MasterStatSkedProg MasterStatSkedProgClientSettings { get; set; }

        public Boolean IsAllDmaAllowed { get; set; }

        public Boolean IsAllStationAllowed { get; set; }

        public Boolean IsAllClassAllowed { get; set; }

        public List<ChartZoomHistory> listchartZoomHistory { get; set; }

        public List<OnlineNewsChartZoomHistory> listOnlineNewsChartZoomHistory { get; set; }

        public List<SocialMediaChartZoomHistory> listSocialMediaChartZoomHistory { get; set; }

        public List<ChartZoomHistory> listTwitterChartZoomHistory { get; set; }

        public List<NB_PublicationCategory> listPublicationCategory { get; set; }

        /// <summary>
        /// Contains the Online News Sort Expression
        /// </summary>
        public string SortExpressionOnlineNews { get; set; }

        public string SortExpressionSocialMedia { get; set; }

        public string SortExpressionTwitter { get; set; }

        public string SortExpressionRadio { get; set; }

        public bool IsTwitterSortDirecitonAsc { get; set; }
        public bool IsRadioSortDirecitonAsc { get; set; }
        /// <summary>
        /// Contains the Online News Sort Direction
        /// </summary>
        public string SortDirectionOnlineNews { get; set; }
        public string SortDirectionSocialMedia { get; set; }

        public bool IsOnlineNewsSortDirecitonAsc { get; set; }
        public bool IsSocialMediaSortDirecitonAsc { get; set; }

        public object searchRequestTV { get; set; }

        public object searchRequestOnlineNews { get; set; }

        public object searchRequestSM { get; set; }

        public object searchRequestTwitter { get; set; }

        public object searchRequestRadio { get; set; }

        public object TweeterResult { get; set; }

        public bool IsNielSenData { get; set; }

        public bool IsCompeteData { get; set; }

        public string ArticleUrl { get; set; }

        public DateTime Harvest_Time { get; set; }

        public string ArticleSortExpression { get; set; }

        public bool IsArticleSortDirecitonAsc { get; set; }

        public int? TotalRecordsCountArticle { get; set; }

        public string SocialArticleSortExpression { get; set; }

        public bool IsSocialArticleSortDirecitonAsc { get; set; }

        public int? TotalRecordsCountSocialArticle { get; set; }

        public string PrintMediaSortExpression { get; set; }

        public bool IsPrintMediaSortDirectionAsc { get; set; }

        public int? TotalRecordsCountPrintMedia { get; set; }

        public string TVReportSortExpression { get; set; }

        public string NewsReportSortExpression { get; set; }

        public string SocialMediaReportSortExpression { get; set; }

        public bool IsTVReportSortDirecitonAsc { get; set; }

        public bool IsNewsReportSortDirecitonAsc { get; set; }

        public bool IsSocialMediaReportSortDirecitonAsc { get; set; }


        public MyIQSearchParams _MyIQSearchParams { get; set; }

        public MyIQReportParams _MyIQReportParams { get; set; }

        public List<SavedSearch> ListSavedSearch { get; set; }

        public SavedSearch ListEditSavedSearch { get; set; }

        public int? SavedSearchSelectedIndex { get; set; }

        public int? SavedSearchSelectedPage { get; set; }

        public SavedSearch LoadedSavedSearch { get; set; }

        public Int64 TotalRecordsCountSavedSearch { get; set; }

        public int? CurrentPageSavedSearch { get; set; }

        public string MyIQTVReportResult { get; set; }

        public string MyIQNewsReportResult { get; set; }

        public string MyIQSMReportResult { get; set; }

        public string MyIQTwitterResult { get; set; }

        public string MyIQTwitterReportResult { get; set; }

        // Twitter

        public bool IsTweetSortDirecitonAsc { get; set; }
        public object searchRequestTweet { get; set; }
        public string SortExpressionTweet { get; set; }
        public int? TotalRecordsCountTweets { get; set; }

        public bool IsIQAgentTVResultShow { get; set; }

        public bool IsIQAgentNMResultShow { get; set; }

        public bool IsIQAgentSMResultShow { get; set; }

        public bool IsIQAgentTwitterResultShow { get; set; }

        public int TVReportNoOfRecordsDisplayInEmail { get; set; }

        public int NMReportNoOfRecordsDisplayInEmail { get; set; }

        public int SMReportNoOfRecordsDisplayInEmail { get; set; }

        public int TWReportNoOfRecordsDisplayInEmail { get; set; }

        public float? TVLowThreshold { get; set; }

        public float? NMLowThreshold { get; set; }

        public float? SMLowThreshold { get; set; }

        public float? TwitterLowThreshold { get; set; }

        public float? TVHighThreshold { get; set; }

        public float? NMHighThreshold { get; set; }

        public float? SMHighThreshold { get; set; }

        public float? TwitterHighThreshold { get; set; }

        public bool IsMicrositeDownload { get; set; }
    }
}



