﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using FeedsSearch;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;
using IQMedia.Shared.Utility;
using IQMedia.Web.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class IQAgentLogic : IQMedia.Web.Logic.Base.ILogic
    {

        public void GetSearchTermByIQAgentTVResultID(Guid clientGuid, Int64 iqagentTVResultID, out Guid rlVideoGUID, out string searchTerm)
        {
            rlVideoGUID = new Guid();
            searchTerm = string.Empty;
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            iQAgentDA.GetSearchTermByIQAgentTVResultID(clientGuid, iqagentTVResultID, out rlVideoGUID, out searchTerm);

        }

        public IQAgent_MediaResultsModel GetIQAgent_MediaResultByID(long ID, Guid ClientGuid, out bool p_IsMissingArticle)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            IQAgent_MediaResultsModel objIQAgent_MediaResultsModel = iQAgentDA.GetIQAgent_MediaResultByID(ID, ClientGuid, out p_IsMissingArticle);
            return objIQAgent_MediaResultsModel;
        }

        public IQAgentReport GetIQAgent_MediaResultReportByReportGuid(string ReportGuid, Int64? SearchRequestID, string MediaType, int IQAgentReportMaxRecordDisplay, bool IsSourceEmail)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            IQAgentReport objIQAgentReport = iQAgentDA.GetIQAgent_MediaResultReportByReportGuid(ReportGuid, SearchRequestID, MediaType, IQAgentReportMaxRecordDisplay, IsSourceEmail);
            
            // remove TV Records having IQAgentURL is null

            objIQAgentReport.Results.ForEach(res => res.MediaResults.Where(mr => mr.MediaType == "TV").ToList().ForEach(delegate(IQAgent_MediaResultsModel mres)
                                                                                                                    {
                                                                                                                        if (string.IsNullOrEmpty(((IQAgent_TVResultsModel)mres.MediaData).IQAgentResultUrl))
                                                                                                                        {
                                                                                                                            res.MediaResults.Remove(mres);
                                                                                                                        }
                                                                                                                    }
                                                                                                                   ));

            // PQ - Encrypt ID

            objIQAgentReport.Results.ForEach(res => res.MediaResults.Where(mr => mr.MediaType == "PQ").ToList().ForEach(delegate(IQAgent_MediaResultsModel mres)
            {
                string key = "0B358AB55C5D059DFFDD7028AD9985EB";
                string autoGenIV = "";
                string encID = CommonFunctions.EncryptStringAES(key, Convert.ToString(mres.ID), out autoGenIV, true);
                string tempIV = "";
                string encIV = CommonFunctions.EncryptStringAES("6A26F02B6D9EB6DD68F85A012BD8322B", autoGenIV, out tempIV, false, "C6DBC2575C2652B01B3F80D27225058D");
                string finalString = encID.Substring(0, 16) + encIV.Substring(16, 16) + encID.Length + encID.Substring(16) + encIV.Substring(0, 16) + encIV.Substring(32);

                var pqModel = (IQAgent_PQResultsModel)mres.MediaData;
                pqModel.EncryptedID = finalString;
            }));
            
            return objIQAgentReport;
        }

        public IQAgentReport_WithoutAuthentication GetIQAgent_MediaResultReportByReportGuid(string ReportGuid, int IQAgentReportMaxRecordDisplay, bool IsSourceEmail)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.GetIQAgent_MediaResultReportByReportGuid(ReportGuid, IQAgentReportMaxRecordDisplay, IsSourceEmail);
        }

        public IQAgentReport_RawMediaPlayer GetIQAgentReport_DetailsToPlayRawMedia(string IQAgentResultUrl, out int Offset)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.GetIQAgentReport_DetailsToPlayRawMedia(IQAgentResultUrl, out Offset);
        }

        public List<IQAgent_SearchRequestModel> SelectIQAgentSearchRequestByClientGuid(string ClientGuid, bool includeDeleted = false)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.SelectIQAgentSearchRequestByClientGuid(ClientGuid, includeDeleted);
        }

        public List<IQAgent_SearchRequestModel> SelectActiveIQAgentSearchRequestByClientGuid(string ClientGuid)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.SelectIQAgentSearchRequestByClientGuid(ClientGuid, false).Where(sr=>sr.IsActive==1).ToList();
        }

        public IQAgent_SearchRequestModel SelectIQAgentSearchRequestByID(string ClientGuid, long ID)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.SelectIQAgentSearchRequestByID(ClientGuid, ID);
        }

        public IQAgentSearchRequest_DropDown SelectAllDropdown(string ClientGuid)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.SelectAllDropdown(ClientGuid);
        }

        public string InsertIQAgentSearchRequest(string ClientGuid, string QueryName, string SearchXML)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.InsertIQAgentSearchRequest(ClientGuid, QueryName, SearchXML);
        }

        public string UpdateIQAgentSearchRequest(string ClientGuid, long IQAgentSearchRequestID, string QueryName, string SearchXML)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.UpdateIQAgentSearchRequest(ClientGuid, IQAgentSearchRequestID, QueryName, SearchXML);
        }

        public Int64 DeleteIQAgentSearchRequest(Int64 ID, Guid ClientGuid)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.DeleteIQAgentSearchRequest(ID, ClientGuid);
        }

        public Int64 RequestDeleteIQAgentSearchRequest(Int64 ID, Guid ClientGuid, Guid CustomerGuid)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.RequestDeleteIQAgentSearchRequest(ID, ClientGuid, CustomerGuid);
        }

        public Int64 ExcludeDomainsBySearchRequest(Guid p_ClientGUID, List<string> p_MediaXml, List<string> p_SearchRequestXml)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);

            string strAgentList = null;
            if (p_SearchRequestXml != null && p_SearchRequestXml.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in p_SearchRequestXml
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strAgentList = xdoc.ToString();
            }

            string strArticleList = null;
            if (p_MediaXml != null && p_MediaXml.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in p_MediaXml
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strArticleList = xdoc.ToString();
            }

            return iQAgentDA.ExcludeDomainsBySearchRequest(p_ClientGUID, strArticleList, strAgentList);
        }

        public string InsertMissingArticle(IQAgent_MissingArticlesModel p_IQAgent_MissingArticlesModel, Guid p_ClientGuid, Guid p_CustomerGuid)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.InsertMissingArticle(p_IQAgent_MissingArticlesModel, p_ClientGuid, p_CustomerGuid);
        }

        public List<IQAgent_SearchRequestModel> SelectNewsAndSocialMediSearchRequestByClientGuid(string ClientGuid)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.SelectNewsAndSocialMediSearchRequestByClientGuid(ClientGuid);
        }

        public string GetTVEyesLocationMediaID(Int64 p_ID)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.GetTVEyesLocationMediaID(p_ID);
        }


        #region Solr

        public Dictionary<string, object> SearchMediaResults(int customerKey, Guid? clientGUID, List<string> mediaIDs, List<string> mediaCategories, List<string> excludeMediaCategories, DateTime? fromDate, DateTime? fromDateLocal, DateTime? toDate, DateTime? toDateLocal, List<string> searchRequestID, 
                                                                string keyword, short? sentiment, string dma, string station, string competeUrl, int? iQDmaID, string handle, string publication, string author, short? prominenceValue, bool isProminenceAudience, bool isAsc, 
                                                                bool? isAudienceSort, int pageSize, long? fromRecordID, ref long? sinceID, bool isFacetingEnabled, bool isOnlyParents, bool? isRead, string pmgSearchUrl, string currentUrl, 
                                                                Dictionary<string, string> dictChildCounts, out long? totalResults, out long? totalResultsDisplay, out bool isReadLimitExceeded)
        {
            Dictionary<string, object> dictResults = new Dictionary<string, object>();
            try
            {
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchRequest searchRequest = new SearchRequest();

                searchRequest.ClientGUID = clientGUID;
                searchRequest.MediaIDs = mediaIDs;
                searchRequest.Keyword = keyword;
                searchRequest.SearchRequestIDs = searchRequestID;
                searchRequest.MediaCategories = mediaCategories;
                searchRequest.ExcludeMediaCategories = excludeMediaCategories;
                searchRequest.IQProminence = prominenceValue;
                searchRequest.IsProminenceAudience = isProminenceAudience;
                searchRequest.SentimentFlag = sentiment;
                searchRequest.Dma = dma;
                searchRequest.DmaID = iQDmaID;
                searchRequest.Station = station;
                searchRequest.Outlet = competeUrl;
                searchRequest.TwitterHandle = handle;
                searchRequest.Publication = publication;
                searchRequest.Author = author;
                searchRequest.FromDate = fromDate;
                searchRequest.FromDateLocal = fromDateLocal;
                searchRequest.ToDate = toDate;
                searchRequest.ToDateLocal = toDateLocal;
                searchRequest.IsFaceting = isFacetingEnabled;
                searchRequest.ResponseType = ResponseType.XML;
                searchRequest.IsOnlyParents = isOnlyParents;
                searchRequest.IsRead = isRead;

                isReadLimitExceeded = false;
                if (clientGUID.HasValue)
                {
                    isReadLimitExceeded = CheckForQueuedSolrUpdates(searchRequest, clientGUID.Value, isRead);
                }

                searchRequest.PageSize = pageSize;
                searchRequest.FromRecordID = fromRecordID;
                searchRequest.SinceID = sinceID;
                searchRequest.IsInitialSearch = !sinceID.HasValue || sinceID.Value == 0;

                if (isAudienceSort.HasValue)
                {
                    searchRequest.SortType = isAudienceSort.Value ? SortType.OUTLET_WEIGHT : SortType.ARTICLE_WEIGHT;
                }
                else
                {
                    searchRequest.SortType = SortType.DATE;
                    searchRequest.IsSortAsc = isAsc;
                }

                Dictionary<string, SearchResult> dictSearchResults = searchEngine.Search(searchRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                SearchResult searchResult = dictSearchResults["Results"];

                #region Insert Search Log
                string _Response = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(searchResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Response = _XmlDocument.InnerXml;
                            _Response = _Response.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Response = null;
                }

                UtilityLogic.InsertFeedsSearchLog(customerKey, 
                                    mediaIDs != null ? string.Join(",", mediaIDs) : String.Empty, 
                                    mediaCategories != null ? string.Join(",", mediaCategories) : string.Empty,
                                    excludeMediaCategories != null ? string.Join(",", excludeMediaCategories) : String.Empty, 
                                    fromDate, 
                                    toDate, 
                                    searchRequestID != null ? string.Join(",", searchRequestID) : String.Empty, 
                                    keyword, 
                                    sentiment, 
                                    dma, 
                                    station, 
                                    competeUrl, 
                                    iQDmaID, 
                                    handle, 
                                    publication,
                                    author,
                                    prominenceValue, 
                                    isProminenceAudience, 
                                    isAsc, 
                                    isAudienceSort, 
                                    pageSize, 
                                    fromRecordID, 
                                    sinceID, 
                                    isOnlyParents, 
                                    isRead,
                                    IQMedia.Shared.Utility.CommonFunctions.SearchType.Feeds.ToString(), 
                                    _Response);
                #endregion

                totalResults = searchResult.TotalDisplayedHitCount;
                totalResultsDisplay = searchResult.TotalHitCount;
                sinceID = searchResult.SinceID;

                FeedsFilterModel filterModel = FillMediaResultsFilter(dictSearchResults);
                dictResults.Add("Filter", filterModel);
                
                // If not filtering on prominence (in which case rollup is disabled) and the dictionary doesn't already exist (when displaying more records), 
                // create a dictionary of the child counts for every returned parent
                if (dictSearchResults.ContainsKey("ParentFacet") && !prominenceValue.HasValue && dictChildCounts == null)
                {
                    SearchResult parentFacet = dictSearchResults["ParentFacet"];
                    XDocument xDoc = XDocument.Parse(parentFacet.ResponseXml);

                    dictChildCounts = xDoc.Descendants("lst")
                                        .Where(d => d.Attribute("name") != null && d.Attribute("name").Value == "parentid")
                                        .First()
                                        .Descendants()
                                        .Where(d => d.Attribute("name") != null && d.Attribute("name").Value != "0") // Ignore parentID 0
                                        .ToDictionary(d => (string)d.Attribute("name").Value, d => (string)d.Value); // Dictionary<ParentID, Count>
                    dictResults.Add("ChildCounts", dictChildCounts);
                }

                List<IQAgent_MediaResultsModel> lstIQAgent_MediaResultsModel = new List<IQAgent_MediaResultsModel>();
                if (searchResult.Hits != null)
                {
                    lstIQAgent_MediaResultsModel = FillMediaResults(searchResult.Hits, dictChildCounts, currentUrl);
                }

                dictResults.Add("Result", lstIQAgent_MediaResultsModel);

                return dictResults;
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error("IQAgentLogic: " + ex.Message + " || " + ex.StackTrace);
                throw;
            }
        }

        public IQAgent_MediaResultsModel SearchChildResults(int customerKey, Guid clientGUID, long parentID, string mediaType, bool isAsc, bool? isAudienceSort, bool? isRead, long sinceID, string pmgSearchUrl, string currentUrl)
        {
            try
            {
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchRequest searchRequest = new SearchRequest();

                searchRequest.ParentID = parentID;
                searchRequest.IsOnlyParents = false;
                searchRequest.SinceID = sinceID;
                if (!String.IsNullOrEmpty(mediaType))
                {
                    searchRequest.MediaCategories = new List<string>() { mediaType };
                }

                CheckForQueuedSolrUpdates(searchRequest, clientGUID, isRead);

                if (isAudienceSort.HasValue)
                {
                    searchRequest.SortType = isAudienceSort.Value ? SortType.OUTLET_WEIGHT : SortType.ARTICLE_WEIGHT;
                }
                else
                {
                    searchRequest.SortType = SortType.DATE;
                    searchRequest.IsSortAsc = isAsc;
                }

                Dictionary<string, SearchResult> dictSearchResults = searchEngine.Search(searchRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                SearchResult searchResult = dictSearchResults["Results"];

                #region Insert Search Log
                string _Response = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(searchResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Response = _XmlDocument.InnerXml;
                            _Response = _Response.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Response = null;
                }

                UtilityLogic.InsertFeedsSearchLog(customerKey,
                                    parentID.ToString(),
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    false,
                                    isAsc,
                                    isAudienceSort,
                                    0,
                                    null,
                                    null,
                                    false,
                                    isRead,
                                    IQMedia.Shared.Utility.CommonFunctions.SearchType.Feeds_Children.ToString(),
                                    _Response);
                #endregion

                IQAgent_MediaResultsModel parent = null;
                if (searchResult.Hits != null)
                {
                    List<IQAgent_MediaResultsModel> lstIQAgent_MediaResultsModel = new List<IQAgent_MediaResultsModel>();
                    lstIQAgent_MediaResultsModel = FillMediaResults(searchResult.Hits, null, currentUrl);

                    parent = lstIQAgent_MediaResultsModel[0];
                    List<IQAgent_MediaResultsModel> children = lstIQAgent_MediaResultsModel.GetRange(1, lstIQAgent_MediaResultsModel.Count - 1);
                    if (parent.MediaType == CommonFunctions.MediaType.TV.ToString())
                    {
                        IQAgent_TVResultsModel tvParent = parent.MediaData as IQAgent_TVResultsModel;

                        // Move national rollup data from the actual parent to the top DMA result
                        IQAgent_MediaResultsModel actualParent = lstIQAgent_MediaResultsModel.Where(s => (s.MediaData as IQAgent_TVResultsModel)._ParentID == 0).First();
                        if (actualParent.ID != parent.ID)
                        {
                            IQAgent_TVResultsModel tvActual = actualParent.MediaData as IQAgent_TVResultsModel;

                            tvParent.National_IQAdShareValue = tvActual.National_IQAdShareValue;
                            tvParent.National_Nielsen_Audience = tvActual.National_Nielsen_Audience;
                            tvParent.National_Nielsen_Result = tvActual.National_Nielsen_Result;
                        }

                        // Solr returns the records sorted by DMA and local time, so resort by the correct field if needed
                        if (isAudienceSort.HasValue)
                        {
                            if (isAudienceSort.Value)
                            {
                                tvParent.ChildResults = children.OrderByDescending(a => a.IQProminence).ThenByDescending(a => (a.MediaData as IQAgent_TVResultsModel).LocalDateTime).ToList();
                            }
                            else
                            {
                                tvParent.ChildResults = children.OrderByDescending(a => a.IQProminenceMultiplier).ThenByDescending(a => (a.MediaData as IQAgent_TVResultsModel).LocalDateTime).ToList();
                            }
                        }
                        else
                        {
                            tvParent.ChildResults = children;
                        }

                        parent.MediaData = tvParent;
                    }
                    else
                    {
                        (parent.MediaData as IQAgent_NewsResultsModel).ChildResults = children;
                    }
                }

                return parent;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<string, object> SearchDashboardResults(int customerKey, Guid clientGUID, List<string> mediaIDs, DateTime? fromDate, DateTime? toDate, List<string> searchRequestID, List<string> mediaCategories, List<string> excludeMediaCategories, string keyword, short? sentiment, short? prominenceValue, 
                                                                    bool isProminenceAudience, bool isOnlyParents, bool? isRead, long? sinceID, string pmgSearchUrl, CancellationToken token, Dictionary<string, object> dictResults)
        {
            try
            {
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchRequest searchRequest = new SearchRequest();

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                IQClient_CustomSettingsModel clientSettings = clientLogic.GetClientCustomSettings(clientGUID.ToString());
                bool UseProminenceMediaValue = clientSettings.UseProminenceMediaValue == true;

                searchRequest.UseProminenceMediaValue = UseProminenceMediaValue;
                if (mediaIDs != null && mediaIDs.Count > 0)
                {
                    searchRequest.MediaIDs = mediaIDs;
                }
                else
                {
                    searchRequest.ClientGUID = clientGUID;
                    searchRequest.Keyword = keyword;
                    searchRequest.SearchRequestIDs = searchRequestID;
                    searchRequest.MediaCategories = mediaCategories;
                    searchRequest.ExcludeMediaCategories = excludeMediaCategories;
                    searchRequest.IQProminence = prominenceValue;
                    searchRequest.IsProminenceAudience = isProminenceAudience;
                    searchRequest.SentimentFlag = sentiment;
                    searchRequest.FromDate = fromDate;
                    searchRequest.ToDate = toDate;
                    searchRequest.IsOnlyParents = isOnlyParents && !prominenceValue.HasValue;
                    searchRequest.SinceID = sinceID;
                    searchRequest.IsRead = isRead;
                    
                    CheckForQueuedSolrUpdates(searchRequest, clientGUID, isRead);
                }

                SearchResult res = searchEngine.SearchForDashboard(searchRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));

                #region Insert Search Log
                string _Response = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(res.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Response = _XmlDocument.InnerXml;
                            _Response = _Response.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Response = null;
                }

                UtilityLogic.InsertFeedsSearchLog(customerKey,
                                    mediaIDs != null ? string.Join(",", mediaIDs) : String.Empty,
                                    mediaCategories != null ? string.Join(",", mediaCategories) : string.Empty,
                                    excludeMediaCategories != null ? string.Join(",", excludeMediaCategories) : String.Empty, 
                                    fromDate,
                                    toDate,
                                    searchRequestID != null ? string.Join(",", searchRequestID) : String.Empty,
                                    keyword,
                                    sentiment,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    prominenceValue,
                                    isProminenceAudience,
                                    false,
                                    null,
                                    0,
                                    null,
                                    sinceID,
                                    isOnlyParents,
                                    isRead,
                                    IQMedia.Shared.Utility.CommonFunctions.SearchType.Feeds_Dashboard.ToString(),
                                    _Response);
                #endregion
                
                if (!token.IsCancellationRequested)
                {
                    dictResults["IsValid"] = true;
                }
                else
                {
                    Log4NetLogger.Error("NOT VALID: manually thrown");
                    throw new Exception();
                }

                List<IQAgent_DaySummaryModel> lstIQAgentSummary = new List<IQAgent_DaySummaryModel>();
                XDocument xDoc = XDocument.Parse(res.ResponseXml);
                
                // response/lst[@name='facet_counts']/lst[@name='facet_pivots']/arr/lst/arr/lst
                List<XElement> elements = xDoc.Descendants("lst").Where(e => e.Parent.Attribute("name") != null && e.Parent.Attribute("name").Value == "pivot").ToList();
                foreach (XElement element in elements)
                {
                    IQAgent_DaySummaryModel summaryModel = new IQAgent_DaySummaryModel();
                    summaryModel.SubMediaType = element.Parent.ElementsBeforeSelf().ToList()[1].Value;
                    if (summaryModel.SubMediaType == CommonFunctions.CategoryType.SocialMedia.ToString() || summaryModel.SubMediaType == CommonFunctions.CategoryType.Blog.ToString() || summaryModel.SubMediaType == CommonFunctions.CategoryType.Forum.ToString() || summaryModel.SubMediaType == CommonFunctions.CategoryType.FB.ToString())
                    {
                        summaryModel.MediaType = CommonFunctions.MediaType.SM.ToString();
                    }
                    else if (summaryModel.SubMediaType == CommonFunctions.CategoryType.Radio.ToString())
                    {
                        summaryModel.MediaType = CommonFunctions.MediaType.TM.ToString();
                    }

                    summaryModel.DayDate = Convert.ToDateTime(element.Descendants("date").First().Value).ToUniversalTime();
                    summaryModel.NoOfDocs = Convert.ToInt64(element.Descendants("int").First().Value);
                    summaryModel.Audience = Int64.Parse(element.Descendants("lst")
                                                            .Where(e => e.Attribute("name").Value == "audience")
                                                            .First()
                                                            .Descendants("double")
                                                            .Where(e => e.Attribute("name").Value == "sum")
                                                            .First()
                                                            .Value, NumberStyles.Any);
                    summaryModel.NoOfHits = Int64.Parse(element.Descendants("lst")
                                                            .Where(e => e.Attribute("name").Value == "numberofhits")
                                                            .First()
                                                            .Descendants("double")
                                                            .Where(e => e.Attribute("name").Value == "sum")
                                                            .First()
                                                            .Value, NumberStyles.Any);
                    summaryModel.IQMediaValue = Decimal.Parse(element.Descendants("lst")
                                                            .Where(e => e.Attribute("name").Value == (UseProminenceMediaValue ? "prominencemediavalue" : "mediavalue"))
                                                            .First()
                                                            .Descendants("double")
                                                            .Where(e => e.Attribute("name").Value == "sum")
                                                            .First()
                                                            .Value, NumberStyles.Any);

                    lstIQAgentSummary.Add(summaryModel);
                }

                dictResults.Add("Results", lstIQAgentSummary);
            }
            catch (Exception ex)
            {
                dictResults["IsValid"] = false;
            }
            return dictResults;
        }

        public FeedsFilterModel GetFilterData(Guid? clientGUID, List<string> mediaCategories, List<string> excludeMediaCategories, DateTime? fromDate, DateTime? fromDateLocal, DateTime? toDate, DateTime? toDateLocal, List<string> searchRequestID, string keyword,
                                                short? sentiment, string dma, string station, string competeUrl, int? iQDmaID, string handle, string publication, string author, short? prominenceValue, bool isProminenceAudience, long? sinceID, string pmgSearchUrl)
        {
            try
            {
                System.Uri PMGSearchRequestUrl = new Uri(pmgSearchUrl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchRequest searchRequest = new SearchRequest();

                searchRequest.ClientGUID = clientGUID;
                searchRequest.Keyword = keyword;
                searchRequest.SearchRequestIDs = searchRequestID;
                searchRequest.MediaCategories = mediaCategories;
                searchRequest.ExcludeMediaCategories = excludeMediaCategories;
                searchRequest.IQProminence = prominenceValue;
                searchRequest.IsProminenceAudience = isProminenceAudience;
                searchRequest.SentimentFlag = sentiment;
                searchRequest.Dma = dma;
                searchRequest.DmaID = iQDmaID;
                searchRequest.Station = station;
                searchRequest.Outlet = competeUrl;
                searchRequest.TwitterHandle = handle;
                searchRequest.Publication = publication;
                searchRequest.Author = author;
                searchRequest.FromDate = fromDate;
                searchRequest.FromDateLocal = fromDateLocal;
                searchRequest.ToDate = toDate;
                searchRequest.ToDateLocal = toDateLocal;
                searchRequest.ResponseType = ResponseType.XML;
                searchRequest.SinceID = sinceID;

                if (clientGUID.HasValue)
                {
                    CheckForQueuedSolrUpdates(searchRequest, clientGUID.Value, null);
                }

                Dictionary<string, SearchResult> dictFilterResults = searchEngine.SearchFacets(searchRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                return FillMediaResultsFilter(dictFilterResults);
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error("IQAgentLogic: " + ex.Message + " || " + ex.StackTrace);
                throw;
            }
        }

        public int QueueMediaResultsForDelete(Guid clientGuid, Guid customerGuid, List<string> mediaIDs)
        {
            string mediaIDXml = null;
            if (mediaIDs != null && mediaIDs.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("add", 
                                                from ele in mediaIDs
                                                select new XElement("doc", 
                                                                new XElement("field", new XAttribute("name", "iqseqid"), ele)
                                                     )));
                mediaIDXml = xdoc.ToString();
            }
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.QueueMediaResultsForDelete(clientGuid, customerGuid, mediaIDXml);
        }

        private List<IQAgent_MediaResultsModel> FillMediaResults(List<Hit> hits, Dictionary<string, string> childCounts, string currentUrl)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            List<IQAgent_MediaResultsModel> lstIQAgent_MediaResultsModel = new List<IQAgent_MediaResultsModel>();

            foreach (Hit hit in hits)
            {
                Uri aPublisherUri;
                IQAgent_MediaResultsModel mediaResultModel = new IQAgent_MediaResultsModel();

                mediaResultModel.ID = hit.ID;
                mediaResultModel.MediaType = hit.MediaType;
                mediaResultModel.CategoryType = hit.MediaCategory;
                mediaResultModel.PositiveSentiment = hit.PositiveSentiment;
                mediaResultModel.NegativeSentiment = hit.NegativeSentiment;
                mediaResultModel.IQProminence = hit.IQProminence;
                mediaResultModel.IQProminenceMultiplier = hit.IQProminenceMultiplier;
                mediaResultModel.MediaDateTime = hit.MediaDate;
                mediaResultModel.ArticleID = hit.ArticleID;
                mediaResultModel.HasChildren = childCounts != null && childCounts.ContainsKey(hit.ID.ToString()); // childCounts will only contain counts that are greater than 0, so simply check if the item ID is present

                CommonFunctions.MediaType mediaType = (CommonFunctions.MediaType)Enum.Parse(typeof(CommonFunctions.MediaType), hit.MediaType);
                switch (mediaType)
                {
                    case CommonFunctions.MediaType.TV:
                        IQAgent_TVResultsModel tvResultModel = new IQAgent_TVResultsModel();
                        mediaResultModel.MediaData = tvResultModel;

                        tvResultModel.ID = hit.MediaID;
                        tvResultModel._ParentID = hit.ParentID;
                        tvResultModel.Nielsen_Audience = hit.Audience;
                        tvResultModel.IQAdShareValue = hit.MediaValue;
                        tvResultModel.Nielsen_Result = hit.AudienceType;
                        tvResultModel.National_Nielsen_Audience = hit.NationalAudience > 0 ? hit.NationalAudience : (long?)null;
                        tvResultModel.National_IQAdShareValue = hit.NationalMediaValue > 0 ? hit.NationalMediaValue : (decimal?)null;
                        tvResultModel.National_Nielsen_Result = hit.NationalAudienceType;
                        tvResultModel.Date = hit.MediaDate;
                        tvResultModel.LocalDateTime = hit.LocalDate;
                        tvResultModel.TimeZone = hit.TimeZone;
                        tvResultModel.RL_Station = hit.StationID;
                        tvResultModel.StationLogo = "http://" + currentUrl + "/StationLogoImages/" + hit.StationID + ".jpg";
                        tvResultModel.Station_Call_Sign = hit.Outlet;
                        tvResultModel.Title120 = hit.Title;
                        tvResultModel.Market = hit.Market;
                        tvResultModel.IQProminence = hit.IQProminence;
                        tvResultModel.IQProminenceMultiplier = hit.IQProminenceMultiplier;
                        tvResultModel.RL_VideoGUID = hit.VideoGUID;
                        tvResultModel.RawMediaThumbUrl = !String.IsNullOrEmpty(hit.ThumbnailUrl) ? hit.ThumbnailUrl.Replace(@"\", "/") : String.Empty;
                        tvResultModel.higlightedCC = hit.HighlightingText;
                        if (!String.IsNullOrEmpty(hit.HighlightingText))
                        {
                            HighlightedCCOutput highlightedCCOutput = new HighlightedCCOutput();
                            tvResultModel.highlightedCCOutput = (HighlightedCCOutput)CommonFunctions.DeserialiazeXml(hit.HighlightingText, highlightedCCOutput);
                        }
                        break;
                    case CommonFunctions.MediaType.NM:
                        IQAgent_NewsResultsModel nmResultModel = new IQAgent_NewsResultsModel();
                        mediaResultModel.MediaData = nmResultModel;

                        nmResultModel.ID = hit.MediaID;
                        nmResultModel.ArticleID = hit.ArticleID;
                        nmResultModel._ParentID = hit.ParentID;
                        nmResultModel.Harvest_Time = hit.MediaDate;
                        nmResultModel.IQLicense = hit.IQLicense;
                        nmResultModel.Url = hit.Url;
                        nmResultModel.Title = hit.Title;
                        nmResultModel.Compete_Audience = hit.Audience;
                        nmResultModel.IQAdShareValue = hit.MediaValue;
                        nmResultModel.Compete_Result = hit.AudienceType;
                        nmResultModel.IQProminence = hit.IQProminence;
                        nmResultModel.IQProminenceMultiplier = hit.IQProminenceMultiplier;
                        nmResultModel.Publication = Uri.TryCreate(hit.Publication, UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : hit.Publication;
                        nmResultModel.HighlightingText = hit.HighlightingText;
                        if (!string.IsNullOrWhiteSpace(hit.HighlightingText))
                        {
                            HighlightedNewsOutput highlightedNewsOutput = new HighlightedNewsOutput();
                            nmResultModel.HighlightedNewsOutput = (HighlightedNewsOutput)CommonFunctions.DeserialiazeXml(hit.HighlightingText, highlightedNewsOutput);
                        }
                        break;
                    case CommonFunctions.MediaType.SM:
                        IQAgent_SMResultsModel smResultModel = new IQAgent_SMResultsModel();
                        mediaResultModel.MediaData = smResultModel;

                        smResultModel.ID = hit.MediaID;
                        smResultModel.ArticleID = hit.ArticleID;
                        smResultModel.ItemHarvestDate = hit.MediaDate;
                        smResultModel.HomeLink = Uri.TryCreate(hit.Publication, UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : hit.Publication;
                        smResultModel.Link = hit.Url;
                        smResultModel.Description = hit.Title;
                        smResultModel.IQAdShareValue = hit.MediaValue;
                        smResultModel.Compete_Audience = hit.Audience;
                        smResultModel.Compete_Result = hit.AudienceType;
                        smResultModel.ThumbUrl = hit.ThumbnailUrl;
                        smResultModel.IQProminence = hit.IQProminence;
                        smResultModel.IQProminenceMultiplier = hit.IQProminenceMultiplier;
                        if (!String.IsNullOrWhiteSpace(hit.ArticleStats))
                        {
                            ArticleStatsModel statsModel = new ArticleStatsModel();
                            smResultModel.ArticleStats = (ArticleStatsModel)CommonFunctions.DeserialiazeXml(hit.ArticleStats, statsModel);
                        }
                        smResultModel.HighlightingText = hit.HighlightingText;
                        if (!string.IsNullOrWhiteSpace(hit.HighlightingText))
                        {
                            HighlightedSMOutput highlightedSMOutput = new HighlightedSMOutput();
                            smResultModel.HighlightedSMOutput = (HighlightedSMOutput)CommonFunctions.DeserialiazeXml(hit.HighlightingText, highlightedSMOutput);
                        }
                        break;
                    case CommonFunctions.MediaType.TW:
                        IQAgent_TwitterResultsModel twResultModel = new IQAgent_TwitterResultsModel();
                        mediaResultModel.MediaData = twResultModel;

                        twResultModel.ID = hit.MediaID;
                        twResultModel.TweetID = hit.ArticleID;
                        twResultModel.Tweet_DateTime = hit.MediaDate;
                        twResultModel.Actor_Link = hit.Url;
                        twResultModel.Actor_DisplayName = hit.Title;
                        twResultModel.Actor_PreferredName = hit.ActorPreferredName;
                        twResultModel.Actor_Image = hit.ThumbnailUrl;
                        twResultModel.KlOutScore = Convert.ToInt64(hit.MediaValue);
                        twResultModel.Actor_FollowersCount = hit.Audience;
                        twResultModel.Actor_FriendsCount = hit.ActorFriendsCount;
                        twResultModel.IQProminence = hit.IQProminence;
                        twResultModel.IQProminenceMultiplier = hit.IQProminenceMultiplier;
                        twResultModel.Summary = hit.HighlightingText;
                        if (!string.IsNullOrWhiteSpace(hit.HighlightingText))
                        {
                            HighlightedTWOutput highlightedTWOutput = new HighlightedTWOutput();
                            twResultModel.HighlightedOutput = (HighlightedTWOutput)CommonFunctions.DeserialiazeXml(hit.HighlightingText, highlightedTWOutput);
                        }
                        break;
                    case CommonFunctions.MediaType.TM:
                        IQAgent_TVEyesResultsModel tvEyesResultModel = new IQAgent_TVEyesResultsModel();
                        mediaResultModel.MediaData = tvEyesResultModel;

                        tvEyesResultModel.ID = hit.MediaID;
                        tvEyesResultModel.UTCDateTime = hit.MediaDate;
                        tvEyesResultModel.LocalDateTime = hit.LocalDate;
                        tvEyesResultModel.TimeZone = hit.TimeZone;
                        tvEyesResultModel.StationID = hit.StationID;
                        tvEyesResultModel.Title = hit.Title;
                        tvEyesResultModel.Market = hit.Market;
                        tvEyesResultModel.TranscriptUrl = hit.Url;
                        tvEyesResultModel.DMARank = hit.DmaID;
                        tvEyesResultModel.HighlightingText = hit.HighlightingText;
                        break;
                    case CommonFunctions.MediaType.PM:
                        IQAgent_BLPMResultsModel pmResultModel = new IQAgent_BLPMResultsModel();
                        mediaResultModel.MediaData = pmResultModel;

                        pmResultModel.ID = hit.MediaID;
                        pmResultModel.FileLocation = hit.FileLocation;
                        pmResultModel.Title = hit.Title;
                        pmResultModel.Pub_Name = hit.Publication;
                        pmResultModel.Circulation = hit.Audience;
                        pmResultModel.HighlightingText = iQAgentDA.GetHilightedText_PM(hit.HighlightingText);
                        break;
                    case CommonFunctions.MediaType.PQ:
                        IQAgent_PQResultsModel pqResultModel = new IQAgent_PQResultsModel();
                        mediaResultModel.MediaData = pqResultModel;

                        pqResultModel.ID = hit.MediaID;
                        pqResultModel.ArticleID = hit.ArticleID;
                        pqResultModel.Title = hit.Title;
                        pqResultModel.Publication = hit.Publication;
                        pqResultModel.Authors = hit.Authors;
                        pqResultModel.ContentHTML = hit.Content;
                        pqResultModel.Copyright = hit.Copyright;
                        pqResultModel.IQProminence = hit.IQProminence;
                        pqResultModel.IQProminenceMultiplier = hit.IQProminenceMultiplier;
                        if (!string.IsNullOrWhiteSpace(hit.HighlightingText))
                        {
                            HighlightedPQOutput highlightedPQOutput = new HighlightedPQOutput();
                            pqResultModel.HighlightedPQOutput = (HighlightedPQOutput)CommonFunctions.DeserialiazeXml(hit.HighlightingText, highlightedPQOutput);
                        }
                        break;
                }

                lstIQAgent_MediaResultsModel.Add(mediaResultModel);
            }

            return lstIQAgent_MediaResultsModel;
        }

        private FeedsFilterModel FillMediaResultsFilter(Dictionary<string, SearchResult> dictSearchResults)
        {
            SearchResult facetResult;
            XmlDocument xmlDoc;
            FeedsFilterModel filter = new FeedsFilterModel();
            bool hasFacets = false;

            // For dates, search requests, and categories, Solr filters out facets that have a count of 0, so if a node exists it should be included
            foreach (KeyValuePair<string, SearchResult> kvResults in dictSearchResults)
            {
                facetResult = kvResults.Value;
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(facetResult.ResponseXml);

                switch (kvResults.Key)
                {
                    case "MediaCategoryFacet":
                        XmlNode mediaCategoryNode = xmlDoc.SelectSingleNode("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='mediacategory']");
                        if (mediaCategoryNode != null)
                        {
                            filter.FilterListCategory = new List<Category>();
                            foreach (XmlNode node in mediaCategoryNode.ChildNodes)
                            {
                                Category category = new Category();
                                category.Key = CommonFunctions.GetEnumDescription((Enum)(Enum.Parse(typeof(CommonFunctions.CategoryType), node.Attributes["name"].Value)));
                                category.Value = node.Attributes["name"].Value;
                                category.Count = Convert.ToInt32(node.InnerText);
                                filter.FilterListCategory.Add(category);
                            }
                        }

                        hasFacets = true;
                        break;
                    case "SentimentFacet":
                        XmlNode posSentimentNode = xmlDoc.SelectSingleNode("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='positivesentiment']");
                        filter.PositiveSentiment = 0;
                        if (posSentimentNode != null)
                        {
                            foreach (XmlNode node in posSentimentNode.ChildNodes)
                            {
                                if (node.Attributes["name"].Value != "0")
                                {
                                    filter.PositiveSentiment += Convert.ToInt32(node.InnerText);
                                }
                            }
                        }

                        XmlNode negSentimentNode = xmlDoc.SelectSingleNode("/response/lst[@name='facet_counts']/lst[@name='facet_fields']/lst[@name='negativesentiment']");
                        filter.NegativeSentiment = 0;
                        if (negSentimentNode != null)
                        {
                            foreach (XmlNode node in negSentimentNode.ChildNodes)
                            {
                                if (node.Attributes["name"].Value != "0")
                                {
                                    filter.NegativeSentiment += Convert.ToInt32(node.InnerText);
                                }
                            }
                        }

                        XmlNode nullSentimentNode = xmlDoc.SelectSingleNode("/response/lst[@name='facet_counts']/lst[@name='facet_queries']/int");
                        filter.NullSentiment = nullSentimentNode != null ? Convert.ToInt32(nullSentimentNode.InnerText) : 0;

                        hasFacets = true;
                        break;
                    case "AgentFacet":
                        XmlNodeList searchRequestNodes = xmlDoc.SelectNodes("/response/lst[@name='facet_counts']/lst[@name='facet_pivot']/arr/lst");
                        if (searchRequestNodes != null && searchRequestNodes.Count > 0)
                        {
                            filter.ListOfSearchRequestFilter = new List<SearchRequestFilter>();
                            foreach (XmlNode node in searchRequestNodes)
                            {
                                SearchRequestFilter searchRequestFilter = new SearchRequestFilter();
                                searchRequestFilter.ID = Convert.ToInt64(node.SelectSingleNode("long[@name='value']").InnerText);
                                searchRequestFilter.Count = Convert.ToInt32(node.SelectSingleNode("int[@name='count']").InnerText);
                                searchRequestFilter.QueryName = node.SelectSingleNode("arr/lst/str[@name='value']").InnerText;
                                filter.ListOfSearchRequestFilter.Add(searchRequestFilter);
                            }
                            filter.ListOfSearchRequestFilter = filter.ListOfSearchRequestFilter.OrderBy(o => o.QueryName).ToList();
                        }

                        hasFacets = true;
                        break;
                    case "DateFacet":
                        XmlNode mediaDateNode = xmlDoc.SelectSingleNode("/response/lst[@name='facet_counts']/lst[@name='facet_ranges']/lst/lst");
                        if (mediaDateNode != null)
                        {
                            filter.FilterMediaDate = new List<string>();
                            foreach (XmlNode node in mediaDateNode.ChildNodes)
                            {
                                filter.FilterMediaDate.Add(Convert.ToDateTime(node.Attributes["name"].Value).ToUniversalTime().ToString("MM/dd/yyyy"));
                            }
                        }

                        hasFacets = true;
                        break;
                    case "IsReadFacet":
                        XmlNode isReadNode = xmlDoc.SelectSingleNode("/response/lst[@name='facet_counts']/lst[@name='facet_queries']");
                        if (isReadNode != null)
                        {
                            filter.Read = 0;
                            filter.Unread = 0;
                            foreach (XmlNode node in isReadNode.ChildNodes)
                            {
                                // The only way to differentiate between the two nodes is a "-" at the start of the Unread node
                                if (node.Attributes["name"].Value.StartsWith("-"))
                                {
                                    filter.Unread = Convert.ToInt64(node.InnerText);
                                }
                                else
                                {
                                    filter.Read = Convert.ToInt64(node.InnerText);
                                }
                            }
                        }

                        hasFacets = true;
                        break;
                }
            }

            return hasFacets ? filter : null;
        }

        private bool CheckForQueuedSolrUpdates(SearchRequest searchRequest, Guid clientGuid, bool? isRead)
        {
            Dictionary<string, List<string>> dictExclude = UtilityLogic.GetQueuedDeleteMediaResults(clientGuid);
            if (dictExclude.ContainsKey("ExcludeIDs"))
            {
                searchRequest.ExcludeIDs = dictExclude["ExcludeIDs"];
            }
            if (dictExclude.ContainsKey("ExcludeSearchRequestIDs"))
            {
                searchRequest.ExcludeSearchRequestIDs = dictExclude["ExcludeSearchRequestIDs"];
            }

            Dictionary<string, bool> dictIsRead = GetQueuedIsRead(clientGuid);
            List<string> readIDs = dictIsRead.Where(s => s.Value).Select(s => s.Key).ToList();
            List<string> unreadIDs = dictIsRead.Where(s => !s.Value).Select(s => s.Key).ToList();
            bool isReadLimitExceeded = readIDs.Count > 1000 || unreadIDs.Count > 1000;

            // Solr has trouble handling more than 1000 queued records at once, so if the limit is exceeded, ignore them and inform the user
            if (!isReadLimitExceeded)
            {
                // Set faceting lists
                if (readIDs != null && readIDs.Count > 0)
                {
                    searchRequest.QueuedAsReadIDs = readIDs;
                }
                if (unreadIDs != null && unreadIDs.Count > 0)
                {
                    searchRequest.QueuedAsUnreadIDs = unreadIDs;
                }

                if (isRead.HasValue)
                {
                    List<string> includeIDs = dictIsRead.Where(s => s.Value == isRead.Value).Select(s => s.Key).ToList();
                    List<string> excludeIDs = dictIsRead.Where(s => s.Value != isRead.Value).Select(s => s.Key).ToList();

                    // Set filtering lists
                    if (includeIDs != null && includeIDs.Count > 0)
                    {
                        // Manually include any records that match the criteria, but haven't been updated in solr. Remove records that are queued for deletion.
                        if (searchRequest.ExcludeIDs != null)
                        {
                            searchRequest.IsReadIncludeIDs = includeIDs.Except(searchRequest.ExcludeIDs).ToList();
                        }
                        else
                        {
                            searchRequest.IsReadIncludeIDs = includeIDs;
                        }
                    }
                    if (excludeIDs != null && excludeIDs.Count > 0)
                    {
                        // Manually exclude any records that don't match the criteria, but haven't been updated in solr.
                        if (searchRequest.ExcludeIDs != null)
                        {
                            searchRequest.ExcludeIDs = searchRequest.ExcludeIDs.Union(excludeIDs).ToList();
                        }
                        else
                        {
                            searchRequest.ExcludeIDs = excludeIDs;
                        }
                    }
                }
            }

            return isReadLimitExceeded;
        }

        #endregion


        public Int16 SuspendAgentSearchRequest(long p_ID, Guid p_ClientGUID,Guid p_CustomerGUID)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.SuspendAgentSearchRequest(p_ID, p_ClientGUID,p_CustomerGUID);
        }

        public short ResumeSuspendedAgent(long p_ID, Guid p_ClientGUID, Guid p_CustomerGUID)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.ResumeSuspendedAgent(p_ID, p_ClientGUID, p_CustomerGUID);
        }

        public int UpdateIsRead(Guid clientGuid, List<string> mediaIDs, bool isRead)
        {
            string strMediaIDs = null;
            if (mediaIDs != null && mediaIDs.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in mediaIDs
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strMediaIDs = xdoc.ToString();
            }

            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.UpdateIsRead(clientGuid, strMediaIDs, isRead);
        }

        public Dictionary<string, bool> GetQueuedIsRead(Guid clientGuid)
        {
            IQAgentDA iQAgentDA = (IQAgentDA)DataAccessFactory.GetDataAccess(DataAccessType.IQAgent);
            return iQAgentDA.GetQueuedIsRead(clientGuid);
        }
    }


}