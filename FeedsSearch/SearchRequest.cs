using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeedsSearch
{
    public class SearchRequest
    {
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }int _PageSize;

        /// <summary>
        /// Used for paging. The number of records currently displayed.
        /// </summary>
        public Int64? FromRecordID
        {
            get { return _FromRecordID; }
            set { _FromRecordID = value; }
        }Int64? _FromRecordID = null;

        /// <summary>
        /// Flag indicating if this is first search being run after page load. Determines if the SinceID query will be run.
        /// </summary>
        public bool IsInitialSearch
        {
            get { return _IsInitialSearch; }
            set { _IsInitialSearch = value; }
        }bool _IsInitialSearch = false;

        /// <summary>
        ///  Used to limit the results to records with IDs at or below this. Ensures that the total record set to search upon is locked at initial page load.
        /// </summary>
        public Int64? SinceID
        {
            get { return _SinceID; }
            set { _SinceID = value; }
        }Int64? _SinceID = null;


        /// <summary>
        /// To get records having SinceID greater than or equal to this.
        /// </summary>
        public Int64? SinceIDAsc { get; set; }

        /// <summary>
        /// Determines if deleted records should be returned.
        /// </summary>
        public bool IncludeDeleted { get; set; }

        /// <summary>
        ///  The Guid of the user's client.
        /// </summary>
        public Guid? ClientGUID
        {
            get { return _ClientGUID; }
            set { _ClientGUID = value; }
        }Guid? _ClientGUID = null;

        /// <summary>
        /// The ID of the parent record when searching for child results.
        /// </summary>
        public Int64? ParentID
        {
            get { return _ParentID; }
            set { _ParentID = value; }
        }Int64? _ParentID = null;

        /// <summary>
        /// Text to search for in the title and highlighting text.
        /// </summary>
        public string Keyword
        {
            get { return _Keyword; }
            set { _Keyword = value; }
        }string _Keyword = null;

        /// <summary>
        /// Start date (inclusive) of the search in GMT time.
        /// </summary>
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; }
        }DateTime? _FromDate = null;

        /// <summary>
        /// End date (inclusive) of the search in GMT time.
        /// </summary>
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; }
        }DateTime? _ToDate = null;

        /// <summary>
        /// Used for faceting. The start date of the search in the client's local time.
        /// </summary>
        public DateTime? FromDateLocal
        {
            get { return _FromDateLocal; }
            set { _FromDateLocal = value; }
        }DateTime? _FromDateLocal = null;

        /// <summary>
        /// Used for faceting. The end date of the search in the client's local time.
        /// </summary>
        public DateTime? ToDateLocal
        {
            get { return _ToDateLocal; }
            set { _ToDateLocal = value; }
        }DateTime? _ToDateLocal = null;   

        /// <summary>
        /// List of IQAgents to search on.
        /// </summary>
        public List<string> SearchRequestIDs
        {
            get { return _SearchRequestIDs; }
            set { _SearchRequestIDs = value; }
        }List<string> _SearchRequestIDs = null;

        /// <summary>
        /// Used to exclude IQAgents that have been queued for deletion, but not yet processed in solr.
        /// </summary>
        public List<string> ExcludeSearchRequestIDs
        {
            get { return _ExcludeSearchRequestIDs; }
            set { _ExcludeSearchRequestIDs = value; }
        }List<string> _ExcludeSearchRequestIDs = null;

        /// <summary>
        /// Media Categories to search on.
        /// </summary>
        public List<string> MediaCategories
        {
            get { return _MediaCategories; }
            set { _MediaCategories = value; }
        }List<string> _MediaCategories = null;

        /// <summary>
        /// Used to exclude media categories that the user doesn't have acccess to.
        /// </summary>
        public List<string> ExcludeMediaCategories
        {
            get { return _ExcludeMediaCategories; }
            set { _ExcludeMediaCategories = value; }
        }List<string> _ExcludeMediaCategories = null;

        /// <summary>
        /// Flag indicating what type of sentiment to search on. -1: Negative, 0: Neutral, 1: Positive
        /// </summary>
        public short? SentimentFlag
        {
            get { return _SentimentFlag; }
            set { _SentimentFlag = value; }
        }short? _SentimentFlag = null;
        
        /// <summary>
        /// Name of the DMA to search on.  
        /// </summary>
        public string Dma
        {
            get { return _Dma; }
            set { _Dma = value; }
        }string _Dma = null;

        /// <summary>
        /// Number of the DMA to search on.
        /// </summary>
        public List<string> DmaIDs
        {
            get { return _DmaIDs; }
            set { _DmaIDs = value; }
        }List<string> _DmaIDs = null;

        /// <summary>
        /// Name of the TV station to search on.
        /// </summary>
        public string Station
        {
            get { return _Station; }
            set { _Station = value; }
        }string _Station = null;

        /// <summary>
        /// The outlet to search on.
        /// </summary>
        public string Outlet
        {
            get { return _Outlet; }
            set { _Outlet = value; }
        }string _Outlet = null;

        /// <summary>
        /// Twitter handle to search on. Checks against the PreferredName field.
        /// </summary>
        public string TwitterHandle
        {
            get { return _TwitterHandle; }
            set { _TwitterHandle = value; }
        }string _TwitterHandle = null;

        /// <summary>
        /// The publication to search on.
        /// </summary>
        public string Publication { get; set; }

        /// <summary>
        /// The author to search on.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Flag indicating whether to search for only read/unread records.
        /// </summary>
        public bool? IsRead { get; set; }

        /// <summary>
        /// Used to filter results. The list of records that have been marked as read/unread, depending on the selected filter value, but not yet processed in solr.
        /// </summary>
        public List<string> IsReadIncludeIDs { get; set; }

        /// <summary>
        /// Flag indicating whether to search for only heard records.
        /// </summary>
        public bool IsHeardFilter { get; set; }

        /// <summary>
        /// Flag indicating whether to search for only seen records.
        /// </summary>
        public bool isSeenFilter { get; set; }

        /// <summary>
        /// Flag indicating whether to search for only paid records.
        /// </summary>
        public bool isPaidFilter { get; set; }

        /// <summary>
        /// Flag indicating whether to search for only earned records.
        /// </summary>
        public bool isEarnedFilter { get; set; }

        /// <summary>
        /// Flag indicating whether to search for only In Program records
        /// </summary>
        public bool isInProgramFilter { get; set; }

        /// <summary>
        /// Flag indicating whether to search for only earned records.
        /// </summary>
        public bool usePESHFilters { get; set; }

        /// <summary>
        /// Used for faceting. The list of IDs that have been marked as read but not yet processed in solr.
        /// </summary>
        public List<string> QueuedAsReadIDs { get; set; }

        /// <summary>
        /// Used for faceting. The list of IDs that have been marked as unread but not yet processed in solr.
        /// </summary>
        public List<string> QueuedAsUnreadIDs { get; set; }

        /// <summary>
        /// Minimum percentile of prominence rank to search on.
        /// </summary>
        public short? IQProminence
        {
            get { return _IQProminence; }
            set { _IQProminence = value; }
        }short? _IQProminence = null;

        /// <summary>
        /// Flag indicating whether to rank results by prominence multiplier (false) or audience-adjusted value (true).  
        /// </summary>
        public bool IsProminenceAudience
        {
            get { return _IsProminenceAudience; }
            set { _IsProminenceAudience = value; }
        }bool _IsProminenceAudience = false;

        /// <summary>
        /// A list of specific IDs to search for.
        /// </summary>
        public List<string> MediaIDs
        {
            get { return _MediaIDs; }
            set { _MediaIDs = value; }
        }List<string> _MediaIDs = null;

        /// <summary>
        /// Used to exclude records that have been queued for deletion or marked as read/unread, but not yet processed in solr.
        /// </summary>
        public List<string> ExcludeIDs
        {
            get { return _ExcludeIDs; }
            set { _ExcludeIDs = value; }
        }List<string> _ExcludeIDs = null;

        /// <summary>
        /// Type of sort to be applied to the results.
        /// </summary>
        public SortType SortType
        {
            get { return _SortType; }
            set { _SortType = value; }
        }SortType _SortType = SortType.DATE;

        /// <summary>
        /// Flag indicating whether to sort ascending or descending. Only applies when SortType is DATE.
        /// </summary>
        public bool IsSortAsc
        {
            get { return _IsSortAsc; }
            set { _IsSortAsc = value; }
        }bool _IsSortAsc = false;

        /// <summary>
        /// Flag indicating if child records will be returned.
        /// </summary>
        public bool IsOnlyParents
        {
            get { return _IsOnlyParents; }
            set { _IsOnlyParents = value; }
        }bool _IsOnlyParents = true;

        /// <summary>
        /// Flag indicating if logging is enabled. If not set, IsFeedsLogging option in web.config is checked.
        /// </summary>
        public bool IsLogging
        {
            get { return _IsLogging; }
            set { _IsLogging = value; }
        }bool _IsLogging = false;

        /// <summary>
        /// The path of the log file. If not set, FeedsLogFileLocation option in web.config is used.
        /// </summary>
        public string LogFileLocation
        {
            get { return _LogFileLocation; }
            set { _LogFileLocation = value; }
        }string _LogFileLocation;

        /// <summary>
        /// Flag indicating if faceting is enabled. 
        /// </summary>
        public bool IsFaceting
        {
            get { return _IsFaceting; }
            set { _IsFaceting = value; }
        }bool _IsFaceting = false;

        /// <summary>
        /// The format of the response. Only applies if faceting is enabled.
        /// </summary>
        public ResponseType? ResponseType
        {
            get { return _ResponseType; }
            set { _ResponseType = value; }
        }ResponseType? _ResponseType = null;

        /// <summary>
        /// Sets the "fl" solr property.
        /// </summary>
        public string FieldList
        {
            get { return _FieldList; }
            set { _FieldList = value; }
        }string _FieldList = null;

        /// <summary>
        /// Flag indicating if the ProminenceMediaValue column should be used instead of the MediaValue column for calculations.
        /// </summary>
        public bool UseProminenceMediaValue
        {
            get { return _UseProminenceMediaValue; }
            set { _UseProminenceMediaValue = value; }
        }bool _UseProminenceMediaValue = false;

        /// <summary>
        /// Flag indicating if the MediaIDs property should search against both the main ID field and the parent ID field, or just the main field.
        /// If searching for the children of a single parent, the ParentID property should be used.
        /// </summary>
        public bool SearchOnParentID
        {
            get { return _SearchOnParentID; }
            set { _SearchOnParentID = value; }
        }bool _SearchOnParentID = false;

        public string ShowTitle
        {
            get { return _showTitle; }
            set { _showTitle = value; }
        }
        string _showTitle = null;

        public List<int> DayOfWeek
        {
            get { return _dayOfWeek; }
            set { _dayOfWeek = value; }
        }
        List<int> _dayOfWeek = null;

        public List<int> TimeOfDay
        {
            get { return _timeOfDay; }
            set { _timeOfDay = value; }
        }
        List<int> _timeOfDay = null;

        public bool? useGMT
        {
            get { return _useGMT; }
            set { _useGMT = value; }
        }
        bool? _useGMT = null;

        public string stationAffil
        {
            get { return _stationAffil; }
            set { _stationAffil = value; }
        }
        string _stationAffil = null;

        public string demographic
        {
            get { return _demographic; }
            set { _demographic = value; }
        }
        string _demographic = null;

        public SearchRequest DeepCopy()
        {
            SearchRequest newRequest = (SearchRequest)this.MemberwiseClone();
            newRequest.SearchRequestIDs = SearchRequestIDs == null ? null : new List<string>(SearchRequestIDs);
            newRequest.ExcludeSearchRequestIDs = ExcludeSearchRequestIDs == null ? null : new List<string>(ExcludeSearchRequestIDs);
            newRequest.MediaCategories = MediaCategories == null ? null : new List<string>(MediaCategories);
            newRequest.ExcludeMediaCategories = ExcludeMediaCategories == null ? null : new List<string>(ExcludeMediaCategories);
            newRequest.IsReadIncludeIDs = IsReadIncludeIDs == null ? null : new List<string>(IsReadIncludeIDs);
            newRequest.QueuedAsReadIDs = QueuedAsReadIDs == null ? null : new List<string>(QueuedAsReadIDs);
            newRequest.QueuedAsUnreadIDs = QueuedAsUnreadIDs == null ? null : new List<string>(QueuedAsUnreadIDs);
            newRequest.MediaIDs = MediaIDs == null ? null : new List<string>(MediaIDs);
            newRequest.ExcludeIDs = ExcludeIDs == null ? null : new List<string>(ExcludeIDs);

            return newRequest;
        }
    }

    public enum ResponseType
    {
        XML,
        json
    }

    public enum SortType
    {
        DATE,
        ARTICLE_WEIGHT,
        OUTLET_WEIGHT,
        IQSEQID
    }
}
