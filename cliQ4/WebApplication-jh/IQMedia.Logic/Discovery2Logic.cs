using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using PMGSearch;
using System.Configuration;
using IQMedia.Shared.Utility;
using System.Threading;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using IQMedia.Logic.Base;
using IQMedia.Data;
using IQMedia.Web.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class Discovery2Logic : IQMedia.Web.Logic.Base.ILogic
    {
        #region Chart

        public DiscoverySearchResponse SearchTV(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket,
                                                bool IsAllDmaAllowed, List<IQ_Dma> listDma,
                                                bool IsAllClassAllowed, List<IQ_Class> listClass,
                                                bool IsAllStationAllowed, List<Station_Affil> listStation,
                                                out List<String> tvMarketList)
        {
            try
            {
                Boolean isError = false;
                tvMarketList = new List<String>();
                DiscoverySearchResponse discoverySearchResponse = new DiscoverySearchResponse();
                discoverySearchResponse.SearchTerm = searchTerm;
                discoverySearchResponse.MediumType = CommonFunctions.CategoryType.TV.ToString();
                //List<DiscoverySearchResponse> lstDiscoverySearchResponse = new List<DiscoverySearchResponse>();

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();

                //foreach (string sterm in searchTerm)
                //{



                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["PMGSearchUrl"].ToString());
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchRequest searchRequest = new SearchRequest();

                searchRequest.Terms = searchTerm;
                searchRequest.Facet = true;
                searchRequest.FacetRangeOther = "all";

                if (!string.IsNullOrWhiteSpace(medium))
                {
                    searchRequest.PageSize = 4;
                }
                else
                {
                    searchRequest.PageSize = 1;
                }

                searchRequest.SortFields = "date-";

                bool isDmaValid = false;
                isDmaValid = IsAllDmaAllowed;

                searchRequest.IQDmaName = new List<string>();
                List<String> lstDMAString = listDma.Select(s => s.Name).ToList();


                if (!string.IsNullOrWhiteSpace(tvMarket))
                {
                    if (IsAllDmaAllowed)
                    {
                        searchRequest.IQDmaName.Add(tvMarket);
                        isDmaValid = true;
                    }
                    else
                    {
                        if (lstDMAString.Contains(tvMarket))
                        {
                            isDmaValid = true;
                            searchRequest.IQDmaName.Add(tvMarket);
                        }
                        else
                        {
                            isDmaValid = false;
                        }
                    }
                }
                else
                {
                    if (!IsAllDmaAllowed)
                    {
                        isDmaValid = true;
                        searchRequest.IQDmaName = lstDMAString;
                    }
                }


                if (!isDmaValid)
                {
                    throw new Exception();
                }

                if (!IsAllClassAllowed)
                {
                    searchRequest.IQClassNum = listClass.Select(s => s.Num).ToList();
                }

                if (!IsAllStationAllowed)
                {
                    searchRequest.StationAffil = listStation.Select(s => s.Name).ToList();
                }

                /*if (!string.IsNullOrWhiteSpace(tvMarket))
                {
                    searchRequest.IQDmaName = new List<string>();
                    searchRequest.IQDmaName.Add(tvMarket);
                }*/

                if (fromDate == null || toDate == null)
                {
                    searchRequest.FacetRangeStarts = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchRequest.FacetRangeEnds = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchRequest.FacetRangeStarts = fromDate;
                    searchRequest.FacetRangeEnds = toDate.Value.AddHours(23).AddMinutes(59);
                }

                searchRequest.FacetRangeEnds = searchRequest.FacetRangeEnds.Value.ToUniversalTime();

                searchRequest.StartDate = searchRequest.FacetRangeStarts;
                searchRequest.EndDate = searchRequest.FacetRangeEnds;

                TimeSpan dateDiff = (TimeSpan)(searchRequest.FacetRangeEnds - searchRequest.FacetRangeStarts);
                if (dateDiff.Days <= 1)
                {
                    searchRequest.FacetRangeGap = RangeGap.HOUR;
                }
                else
                {
                    searchRequest.FacetRangeGap = RangeGap.DAY;
                }


                searchRequest.FacetRangeGapDuration = 1;
                searchRequest.FacetRange = "RL_Station_DateTime_DT";
                searchRequest.FacetField = "market";
                searchRequest.AffilForFacet = new Dictionary<Dictionary<string, string>, List<string>>();
                searchRequest.wt = ReponseType.json;

                SearchResult searchResult = searchEngine.SearchTVChart(searchRequest, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));

                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(searchResult.ResponseXml);

                string totalResult = Convert.ToString(jsonData["facet_counts"]["facet_ranges"]["RL_Station_DateTime_DT"]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                string[] facetData = totalResult.Split(',');

                discoverySearchResponse.ListRecordData = new List<RecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {

                    RecordData recorddata = new RecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);

                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<TopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    TopResults topResults = new TopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["title120"]).Replace("[", "").Replace("]", "");
                    topResults.Logo = "../images/MediaIcon/network-icon.png";//ConfigurationManager.AppSettings["StationLogo"] + Convert.ToString(jsonData["response"]["docs"][i]["stationid"]) + ".jpg";
                    topResults.Publisher = "<img src=\"" + ConfigurationManager.AppSettings["StationLogo"] + Convert.ToString(jsonData["response"]["docs"][i]["stationid"]) + ".jpg" + "\" alt=\"\" />";// ;
                    //Convert.ToString(jsonData["response"]["docs"][i]["market"]);

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                string totalResultmarket = Convert.ToString(jsonData["facet_counts"]["facet_fields"]["market"]).Replace("\r\n", string.Empty).Replace("[", "").Replace("]", "");
                //string[] facetDatamarket = totalResultmarket.Split(',');

                string[] facetDatamarket = Regex.Split(totalResultmarket, "\"(.*?)\"");

                List<String> tvMarketListtemp = new List<String>();

                for (int i = 1; i < facetDatamarket.Length; i = i + 2)
                {
                    if (Convert.ToInt64(facetDatamarket[i + 1].Trim().Replace(",", string.Empty)) > 0)
                    {
                        tvMarketListtemp.Add(facetDatamarket[i].Trim().Replace("\"", string.Empty).Replace("[", string.Empty).Trim());
                    }
                }

                tvMarketListtemp = tvMarketListtemp.Distinct().ToList();

                if (tvMarketListtemp.Contains("National"))
                {
                    tvMarketList.Add("National");
                }
                var allMarketData = (from item in tvMarketListtemp
                                     where item != "National"
                                     orderby item
                                     select item).ToList();
                tvMarketList.AddRange(allMarketData);
                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);

                return discoverySearchResponse;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public DiscoverySearchResponse SearchNews(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, string p_fromRecordID)
        {
            try
            {

                Boolean isError = false;
                //List<DiscoverySearchResponse> lstDiscoverySearchResponse = new List<DiscoverySearchResponse>();
                DiscoverySearchResponse discoverySearchResponse = new DiscoverySearchResponse();
                discoverySearchResponse.SearchTerm = searchTerm;
                discoverySearchResponse.MediumType = CommonFunctions.CategoryType.NM.ToString();

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();

                //foreach (string sterm in searchTerm)
                //{

                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["SolrNewsUrl"].ToString());
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchNewsRequest searchNewsRequest = new SearchNewsRequest();

                searchNewsRequest.SearchTerm = searchTerm;
                searchNewsRequest.Facet = true;
                searchNewsRequest.FacetRangeOther = "all";

                if (!string.IsNullOrWhiteSpace(medium))
                {
                    searchNewsRequest.PageSize = 4;
                }
                else
                {
                    searchNewsRequest.PageSize = 1;
                }

                searchNewsRequest.SortFields = "date-";

                if (!string.IsNullOrWhiteSpace(p_fromRecordID))
                {
                    searchNewsRequest.FromRecordID = Convert.ToString(p_fromRecordID);
                }

                if (fromDate == null || toDate == null)
                {
                    searchNewsRequest.FacetRangeStarts = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchNewsRequest.FacetRangeEnds = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchNewsRequest.FacetRangeStarts = fromDate;
                    searchNewsRequest.FacetRangeEnds = toDate.Value.AddHours(23).AddMinutes(59);
                }

                searchNewsRequest.FacetRangeEnds = searchNewsRequest.FacetRangeEnds.Value.ToUniversalTime();

                searchNewsRequest.StartDate = searchNewsRequest.FacetRangeStarts;
                searchNewsRequest.EndDate = searchNewsRequest.FacetRangeEnds;

                TimeSpan dateDiff = (TimeSpan)(searchNewsRequest.FacetRangeEnds - searchNewsRequest.FacetRangeStarts);
                if (dateDiff.Days <= 1)
                {
                    searchNewsRequest.FacetRangeGap = RangeGap.HOUR;
                }
                else
                {
                    searchNewsRequest.FacetRangeGap = RangeGap.DAY;
                }

                searchNewsRequest.FacetRangeGapDuration = 1;
                searchNewsRequest.FacetRange = "harvest_time_DT";
                searchNewsRequest.wt = ReponseType.json;

                string newsReponse = searchEngine.SearchNewsChart(searchNewsRequest, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(newsReponse);

                string totalResult = Convert.ToString(jsonData["facet_counts"]["facet_ranges"]["harvest_time_DT"]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                string[] facetData = totalResult.Split(',');

                string fromRecordID = string.Empty;
                try
                {
                    fromRecordID = Convert.ToString(jsonData["response"]["docs"][0]["id"]);
                }
                catch (Exception)
                {


                }

                if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    discoverySearchResponse.FromRecordID = fromRecordID;
                }

                discoverySearchResponse.ListRecordData = new List<RecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {

                    RecordData recorddata = new RecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);

                }


                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<TopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    TopResults topResults = new TopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["hltext_display"]);
                    topResults.Logo = "../images/MediaIcon/News.png";
                    Uri aPublisherUri;
                    topResults.Publisher = Uri.TryCreate(Convert.ToString(jsonData["response"]["docs"][i]["docurl"]).Replace("\n", ""), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(jsonData["response"]["docs"][i]["docurl"]).Replace("\n", "");

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                //lstDiscoverySearchResponse.Add(discoverySearchResponse);
                // }
                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);
                return discoverySearchResponse;


            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("Error in News : " + ex.ToString());
                throw;
            }

        }

        public SocialMediaFacet SearchSocialMedia(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, string p_fromRecordID)
        {
            try
            {
                Boolean isError = false;

                SocialMediaFacet socialMediaFacet = new SocialMediaFacet();

                DiscoverySearchResponse discoverySearchResponse = new DiscoverySearchResponse();
                discoverySearchResponse.SearchTerm = searchTerm;
                discoverySearchResponse.MediumType = CommonFunctions.CategoryType.SocialMedia.ToString();

                DiscoverySearchResponse discoverySearchResponseFeedClass = new DiscoverySearchResponse();
                discoverySearchResponseFeedClass.SearchTerm = searchTerm;
                discoverySearchResponseFeedClass.MediumType = CommonFunctions.CategoryType.SocialMedia.ToString();


                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();

                //foreach (string sterm in searchTerm)
                //{

                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["SolrSMUrl"].ToString());
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchSMRequest searchSMRequest = new SearchSMRequest();

                searchSMRequest.SearchTerm = searchTerm;
                searchSMRequest.Facet = true;
                searchSMRequest.FacetRangeOther = "all";

                if (!string.IsNullOrWhiteSpace(medium))
                {
                    searchSMRequest.PageSize = 4;
                }
                else
                {
                    searchSMRequest.PageSize = 1;
                }

                searchSMRequest.SortFields = "date-";
                if (!string.IsNullOrWhiteSpace(p_fromRecordID))
                {
                    searchSMRequest.FromRecordID = Convert.ToString(p_fromRecordID);
                }

                if (fromDate == null || toDate == null)
                {
                    searchSMRequest.FacetRangeStarts = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchSMRequest.FacetRangeEnds = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchSMRequest.FacetRangeStarts = fromDate;
                    searchSMRequest.FacetRangeEnds = toDate.Value.AddHours(23).AddMinutes(59);
                }

                searchSMRequest.FacetRangeEnds = searchSMRequest.FacetRangeEnds.Value.ToUniversalTime();

                searchSMRequest.StartDate = searchSMRequest.FacetRangeStarts;
                searchSMRequest.EndDate = searchSMRequest.FacetRangeEnds;

                TimeSpan dateDiff = (TimeSpan)(searchSMRequest.FacetRangeEnds - searchSMRequest.FacetRangeStarts);
                if (dateDiff.Days <= 1)
                {
                    searchSMRequest.FacetRangeGap = RangeGap.HOUR;
                }
                else
                {
                    searchSMRequest.FacetRangeGap = RangeGap.DAY;
                }

                searchSMRequest.FacetRangeGapDuration = 1;
                searchSMRequest.FacetRange = "itemHarvestDate_DT";
                searchSMRequest.FacetField = "feedClass";
                searchSMRequest.StartDate = searchSMRequest.FacetRangeStarts;
                searchSMRequest.EndDate = searchSMRequest.FacetRangeEnds;
                searchSMRequest.IsTaggingExcluded = true;
                if (!string.IsNullOrWhiteSpace(medium))
                {
                    searchSMRequest.SourceType = new List<string>();

                    if (medium == CommonFunctions.CategoryType.Blog.ToString())
                    {
                        searchSMRequest.SourceType.Add(CommonFunctions.CategoryType.Blog.ToString());
                        //searchSMRequest.SourceType.Add(medium);
                        //searchSMRequest.SourceType.Add("Comment");
                    }
                    else if (medium == CommonFunctions.CategoryType.Forum.ToString())
                    {
                        searchSMRequest.SourceType.Add(CommonFunctions.CategoryType.Forum.ToString());
                        //searchSMRequest.SourceType.Add(medium);
                        //searchSMRequest.SourceType.Add("Review");
                    }
                    else
                    {
                        searchSMRequest.SourceType.Add(CommonFunctions.CategoryType.SocialMedia.ToString());
                        /*searchSMRequest.ExcludedSourceType = new List<string>();
                        searchSMRequest.ExcludedSourceType.Add("Blog");
                        searchSMRequest.ExcludedSourceType.Add("Review");
                        searchSMRequest.ExcludedSourceType.Add("Forum");
                        searchSMRequest.ExcludedSourceType.Add("Comment");*/
                    }
                }

                searchSMRequest.wt = ReponseType.json;

                string smReponse = searchEngine.SearchSocialMediaChart(searchSMRequest, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(smReponse);

                string totalResult = Convert.ToString(jsonData["facet_counts"]["facet_ranges"]["itemHarvestDate_DT"]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                string[] facetData = totalResult.Split(',');

                discoverySearchResponse.ListRecordData = new List<RecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {

                    RecordData recorddata = new RecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);
                }


                string totalResultFeedClass = Convert.ToString(jsonData["facet_counts"]["facet_fields"]["feedClass"]).Replace("\r\n", string.Empty);
                string[] facetDataFeedClass = totalResultFeedClass.Split(',');

                string fromRecordID = string.Empty;
                try
                {
                    fromRecordID = Convert.ToString(jsonData["response"]["docs"][0]["seqId"]);
                }
                catch (Exception)
                {

                }

                if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    discoverySearchResponse.FromRecordID = fromRecordID;
                }

                discoverySearchResponseFeedClass.ListRecordData = new List<RecordData>();

                for (int i = 0; i < facetDataFeedClass.Length; i = i + 2)
                {
                    string feedClass = GetFeedClass(facetDataFeedClass[i].Trim().Replace("\"", string.Empty).Replace("[", string.Empty).Trim());
                    RecordData recorddata = new RecordData();
                    recorddata.FeedClass = feedClass;
                    recorddata.TotalRecord = Convert.ToString(facetDataFeedClass[i + 1].Trim().Replace("\"", string.Empty).Replace("]", string.Empty));
                    if (string.IsNullOrWhiteSpace(medium))
                    {
                        discoverySearchResponseFeedClass.ListRecordData.Add(recorddata);
                    }
                    else if (medium == feedClass)
                    {
                        discoverySearchResponseFeedClass.ListRecordData.Add(recorddata);
                    }
                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<TopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    TopResults topResults = new TopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["description"]);
                    topResults.Logo = "../images/MediaIcon/" + GetFeedClass(Convert.ToString(jsonData["response"]["docs"][i]["feedClass"])).Replace(" ", "-") + ".png";
                    Uri aPublisherUri;
                    topResults.Publisher = Uri.TryCreate(Convert.ToString(jsonData["response"]["docs"][i]["homeLink"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(jsonData["response"]["docs"][i]["homeLink"]);

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }

                //lstDiscoverySearchResponseFeedClass.Add(discoverySearchResponseFeedClass);

                //lstDiscoverySearchResponse.Add(discoverySearchResponse);
                //}
                socialMediaFacet.DateData = discoverySearchResponse;
                socialMediaFacet.FeedClassData = discoverySearchResponseFeedClass;
                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);
                return socialMediaFacet;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public DiscoverySearchResponse SearchTwitter(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, string p_fromRecordID)
        {
            try
            {
                Boolean isError = false;
                List<DiscoverySearchResponse> lstDiscoverySearchResponse = new List<DiscoverySearchResponse>();

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();

                //foreach (string sterm in searchTerm)
                // {
                DiscoverySearchResponse discoverySearchResponse = new DiscoverySearchResponse();
                discoverySearchResponse.SearchTerm = searchTerm;
                discoverySearchResponse.MediumType = CommonFunctions.CategoryType.TW.ToString();

                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["SolrTwitterUrl"].ToString());
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchTwitterRequest searchTwitterRequest = new SearchTwitterRequest();

                searchTwitterRequest.SearchTerm = searchTerm;
                searchTwitterRequest.Facet = true;
                searchTwitterRequest.FacetRangeOther = "all";

                if (!string.IsNullOrWhiteSpace(medium))
                {
                    searchTwitterRequest.PageSize = 4;
                }
                else
                {
                    searchTwitterRequest.PageSize = 1;
                }

                searchTwitterRequest.SortFields = "date-";

                if (!string.IsNullOrWhiteSpace(p_fromRecordID))
                {
                    searchTwitterRequest.FromRecordID = Convert.ToString(p_fromRecordID);
                }

                if (fromDate == null || toDate == null)
                {
                    searchTwitterRequest.FacetRangeStarts = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchTwitterRequest.FacetRangeEnds = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchTwitterRequest.FacetRangeStarts = fromDate;
                    searchTwitterRequest.FacetRangeEnds = toDate.Value.AddHours(23).AddMinutes(59);
                }


                searchTwitterRequest.FacetRangeEnds = searchTwitterRequest.FacetRangeEnds.Value.ToUniversalTime();

                searchTwitterRequest.StartDate = searchTwitterRequest.FacetRangeStarts;
                searchTwitterRequest.EndDate = searchTwitterRequest.FacetRangeEnds;

                TimeSpan dateDiff = (TimeSpan)(searchTwitterRequest.FacetRangeEnds - searchTwitterRequest.FacetRangeStarts);
                if (dateDiff.Days <= 1)
                {
                    searchTwitterRequest.FacetRangeGap = RangeGap.HOUR;
                }
                else
                {
                    searchTwitterRequest.FacetRangeGap = RangeGap.DAY;
                }

                searchTwitterRequest.FacetRangeGapDuration = 1;
                searchTwitterRequest.FacetRange = "tweet_posteddatetime";

                searchTwitterRequest.wt = ReponseType.json;

                searchTwitterRequest.IsDeleted = false;

                SearchTwitterResult searchTwitterResult = searchEngine.SearchTwitter(searchTwitterRequest, true, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(searchTwitterResult.ResponseXml);

                string totalResult = Convert.ToString(jsonData["facet_counts"]["facet_ranges"]["tweet_posteddatetime"]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                string[] facetData = totalResult.Split(',');

                string fromRecordID = string.Empty;
                try
                {
                    fromRecordID = Convert.ToString(jsonData["response"]["docs"][0]["iqseqid"]);
                }
                catch (Exception)
                {


                }

                if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    discoverySearchResponse.FromRecordID = fromRecordID;
                }

                discoverySearchResponse.ListRecordData = new List<RecordData>();
                for (int i = 0; i < facetData.Length; i = i + 2)
                {

                    RecordData recorddata = new RecordData();
                    recorddata.Date = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
                    recorddata.TotalRecord = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty));

                    discoverySearchResponse.ListRecordData.Add(recorddata);

                }

                // Get Top Results
                discoverySearchResponse.ListTopResults = new List<TopResults>();
                for (int i = 0; i < jsonData["response"]["docs"].Count(); i++)
                {
                    TopResults topResults = new TopResults();
                    topResults.Title = Convert.ToString(jsonData["response"]["docs"][i]["tweet_body"]);
                    topResults.Logo = "../images/MediaIcon/twitter.png";
                    topResults.Publisher = Convert.ToString(jsonData["response"]["docs"][i]["actor_displayname"]);

                    discoverySearchResponse.ListTopResults.Add(topResults);
                }


                //lstDiscoverySearchResponse.Add(discoverySearchResponse);
                // }
                discoverySearchResponse.TotalResult = Convert.ToInt64(jsonData["response"]["numFound"]);
                return discoverySearchResponse;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public string ColumnChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            try
            {
                var dataValue = (List<Series>)(from x in
                                                   (
                                                       from sr in lstDiscoverySearchResponse
                                                       from dc in sr.ListRecordData
                                                       select new { sr.SearchTerm, dc }
                                                   )
                                               group x by new { x.SearchTerm } into g
                                               select new Series()
                                       {
                                           name = g.Key.SearchTerm,
                                           data = GetSeriesList(g.Sum(g1 => Convert.ToInt64(g1.dc.TotalRecord)))
                                       }).ToList();

                HighColumnChartModel highColumnChartModel = new HighColumnChartModel();
                highColumnChartModel.chart = new HChart() { height = 300, width = 150, type = "column" };
                highColumnChartModel.title = new Title() { text = "Share of Coverage" };
                highColumnChartModel.legend = new Legend() { align = "center", borderWidth = "0", layout = "horizontal", verticalAlign = "bottom", width = 100 };
                highColumnChartModel.subtitle = new Subtitle() { text = "" };
                highColumnChartModel.xAxis = new XAxis() { categories = new List<string>(), labels = new labels() };
                highColumnChartModel.yAxis = new YAxis();
                highColumnChartModel.series = dataValue;
                highColumnChartModel.plotOptions = new PlotOptions() { column = new Column() { borderWidth = 0, pointPadding = 0.2 } };
                highColumnChartModel.tooltip = null;



                string jsonResult = CommonFunctions.SearializeJson(highColumnChartModel);
                /*XDocument xdoc = new XDocument(new XElement("chart", new XAttribute("yAxisName", ""), new XAttribute("caption", "Average Report"),
                                                  searchTermTotal.Select(s => new XElement("Set", new XAttribute("label", s.SearchTerm), new XAttribute("value", s.TotalRecord)))));*/
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string LineChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse, Boolean isHourData)
        {
            try
            {

                var totalRecords = (List<SearchTermTotalRecords>)(from x in
                                                                      (
                                                                          from sr in lstDiscoverySearchResponse
                                                                          from dc in sr.ListRecordData
                                                                          select new { sr.SearchTerm, dc }
                                                                      )
                                                                  group x by new { x.SearchTerm, x.dc.Date } into g
                                                                  select new SearchTermTotalRecords
                                                                  {
                                                                      SearchTerm = g.Key.SearchTerm,
                                                                      RecordDate = Convert.ToDateTime(g.Key.Date),
                                                                      TotalRecords = g.Sum(g1 => Convert.ToInt64(g1.dc.TotalRecord))
                                                                  }).ToList();
                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
                highLineChartOutput.title = new Title() { text = "Trend over Time", x = -20 };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                List<PlotLine> plotlines = new List<PlotLine>();

                PlotLine plotLine = new PlotLine();
                plotLine.color = "#808080";
                plotLine.value = "0";
                plotLine.width = "1";
                plotlines.Add(plotLine);


                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis{ title = new Title2(), plotLines = plotlines }};



                List<string> categories = new List<string>();

                var distinctDate = totalRecords.Select(d => d.RecordDate).Distinct().ToList();

                AllCategory allCategory = new AllCategory();
                allCategory.category = new List<Category2>();

                foreach (DateTime rDate in distinctDate)
                {


                    if (isHourData)
                    {
                        categories.Add(rDate.ToString("MM/dd/yyyy hh:mm tt"));
                    }
                    else
                    {
                        categories.Add(rDate.ToShortDateString());
                    }

                }

                highLineChartOutput.xAxis = new XAxis()
                {
                    categories = categories,
                    labels = new labels()
                        {
                            rotation = 270
                        }
                };
                highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "" };
                highLineChartOutput.legend = new Legend() { align = "center", borderWidth = "0", layout = "horizontal", verticalAlign = "bottom", width = 150 };
                highLineChartOutput.hChart = new HChart() { height = 300, width = 750 };
                highLineChartOutput.plotOption = new PlotOptions()
                {
                    column = null,
                    series = new PlotSeries()
                    {
                        cursor = "pointer",
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "ChartClick"
                            }
                        }
                    }
                };


                var distinctSearchTerm = totalRecords.Select(d => d.SearchTerm).Distinct().ToList();

                List<SeriesData> lstSeriesData = new List<SeriesData>();
                List<Series> lstSeries = new List<Series>();
                foreach (string sTerm in distinctSearchTerm)
                {
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = sTerm;



                    var sTermWiseRecord = totalRecords.Where(w => w.SearchTerm.Equals(sTerm)).Select(s => s).ToList();
                    foreach (SearchTermTotalRecords searchTermTotalRecord in sTermWiseRecord)
                    {
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = searchTermTotalRecord.TotalRecords;
                        highChartDatum.SearchTerm = sTerm.Trim();
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);

                }

                highLineChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                return jsonResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<PieChartResponse> PieChart(List<DiscoverySearchResponse> lstDiscoverySearchResponse, List<DiscoverySearchResponse> lstDiscoverySearchResponseFeedClass,
                                                string[] searchTerm, string medium,
                                                bool p_Isv4TV, bool p_Isv4NM,
                                                bool p_Isv4SM, bool p_Isv4TW)
        {
            try
            {
                List<PieChartResponse> lstPieChartResponse = new List<PieChartResponse>();
                HighPieChartModel highPieChartModel = new HighPieChartModel();
                highPieChartModel.chart = new PChart() { height = 300, width = 400 };
                highPieChartModel.title = new PTitle() { text = "Sources" };
                highPieChartModel.tooltip = new PTooltip() { pointFormat = "{series.name}: <b>{point.percentage:.1f}%</b>" };
                highPieChartModel.plotOptions = new PPlotOptions() { pie = new Pie() { allowPointSelect = true, cursor = "pointer", showInLegend = true, dataLabels = new DataLabels() { enabled = false } } };
                highPieChartModel.series = new List<PSeries>();

                List<SearchTermTotalRecords> totalRecords = new List<SearchTermTotalRecords>();
                if (lstDiscoverySearchResponseFeedClass != null)
                {
                    totalRecords = (List<SearchTermTotalRecords>)(from x in
                                                                      (
                                                                          from sr in lstDiscoverySearchResponseFeedClass
                                                                          from dc in sr.ListRecordData
                                                                          select new { sr.SearchTerm, dc }
                                                                      )
                                                                  group x by new { x.SearchTerm, x.dc.FeedClass } into g
                                                                  select new SearchTermTotalRecords
                                                                  {
                                                                      SearchTerm = g.Key.SearchTerm,
                                                                      FeedClass = g.Key.FeedClass,
                                                                      TotalRecords = g.Sum(g1 => Convert.ToInt64(g1.dc.TotalRecord))
                                                                  }).ToList();
                }


                //Get Distinct Search Term and By Looping through it , Get Data of Feedclass and its total

                //var distinctSearchTerm = totalRecords.Select(s => s.SearchTerm).Distinct().ToList();

                PSeries pSeries = new PSeries();

                pSeries.type = "pie";
                pSeries.name = "";
                //pSeries.data = new List<List<PSeriesData>>();
                pSeries.data = new List<object>();
                List<Object> lstObject = new List<object>();
                List<PSeries> lstPseries = new List<PSeries>();
                lstPseries.Add(pSeries);

                Dictionary<string, double> dictPie = new Dictionary<string, double>();

                foreach (string sTerm in searchTerm)
                {
                    lstObject = new List<object>();
                    pSeries.data = new List<object>();
                    //List<List<PSeriesData>> lstoflstPSeriesData = new List<List<PSeriesData>>();
                    // Get TV Data
                    if (p_Isv4TV && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.TV.ToString()))
                    {


                        lstObject.Add(new object[] { CommonFunctions.CategoryType.TV.ToString(), lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TV.ToString()) && w.SearchTerm.Equals(sTerm)).GroupBy(g => g.MediumType)
                                                                                .Select(s =>
                                                                                    Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalResult)))).FirstOrDefault()});


                    }


                    // Get NEWS Data
                    if (p_Isv4NM && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.NM.ToString()))
                    {

                        lstObject.Add(new object[] { CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.NM) ,
                        
                            lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.NM.ToString()) && w.SearchTerm.Equals(sTerm)).GroupBy(g => g.MediumType)
                                                                                .Select(s =>
                                                                                    Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalResult)))).FirstOrDefault()});

                    }

                    // Get TW Data
                    if (p_Isv4TW && (string.IsNullOrWhiteSpace(medium) || medium == CommonFunctions.CategoryType.TW.ToString()))
                    {
                        lstObject.Add(new object[]{CommonFunctions.GetEnumDescription(CommonFunctions.CategoryType.TW),
                            lstDiscoverySearchResponse.Where(w => w.MediumType.Equals(CommonFunctions.CategoryType.TW.ToString()) && String.Compare(w.SearchTerm, sTerm, true) == 0).GroupBy(g => g.MediumType)
                                                                                .Select(s =>
                                                                                   Convert.ToInt64(s.Sum(s1 => Convert.ToInt64(s1.TotalResult)))).FirstOrDefault()});
                    }




                    // Get Social Media Data
                    if (p_Isv4SM && (string.IsNullOrWhiteSpace(medium) || medium == "Social Media" ||
                            medium == CommonFunctions.CategoryType.Blog.ToString() ||
                        medium == CommonFunctions.CategoryType.Forum.ToString()))
                    {
                        if (totalRecords == null || totalRecords.Count <= 0)
                        {
                            lstObject.Add(new object[] { "Social Media", 0 });
                            lstObject.Add(new object[] { "Blog", 0 });
                            lstObject.Add(new object[] { "Forum", 0 });

                        }
                        else
                        {
                            lstObject.AddRange(totalRecords.Where(w => w.SearchTerm.Equals(sTerm)).Select(s => new object[]
                            {
                                s.FeedClass,s.TotalRecords
                            }).ToList());


                        }

                    }

                    /*List<List<object>> lstofListofObject = new List<List<object>>();
                    lstofListofObject.Add(lstObject);*/
                    pSeries.data = lstObject;
                    highPieChartModel.series = lstPseries;
                    string jsonResult = CommonFunctions.SearializeJson(highPieChartModel);

                    PieChartResponse pieChartResponse = new PieChartResponse();
                    pieChartResponse.JsonResult = jsonResult;
                    pieChartResponse.SearchTerm = sTerm;
                    /*pieChartReponse.SearchTerm = sTerm;
                    pieChartReponse.JsonResult = jsonResult;*/
                    lstPieChartResponse.Add(pieChartResponse);
                }


                return lstPieChartResponse;


            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion


        #region Result
        public List<DiscoveryMediaResult> SearchTVResult(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, string tvMarket, Guid clientGUID,
                                                         bool IsAllDmaAllowed, List<IQ_Dma> listDma,
                                                bool IsAllClassAllowed, List<IQ_Class> listClass,
                                                bool IsAllStationAllowed, List<Station_Affil> listStation, IQClient_ThresholdValueModel p_IQClient_ThresholdValueModel, Int32 p_PageSize, out List<String> tvMarketList)
        //, Int64 fromRecordID,
        {
            List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();
            tvMarketList = new List<string>();
            try
            {
                Log4NetLogger.Debug("Task: SearchTVResult -- Searchterm :" + searchTerm);

                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["PMGSearchUrl"].ToString());
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchRequest searchRequest = new SearchRequest();

                searchRequest.Terms = searchTerm;

                //searchRequest.Start = fromRecordID;
                searchRequest.SortFields = "date-";
                searchRequest.PageSize = p_PageSize;// Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryPageSize"]);

                bool isDmaValid = false;
                isDmaValid = IsAllDmaAllowed;

                searchRequest.IQDmaName = new List<string>();
                List<String> lstDMAString = listDma.Select(s => s.Name).ToList();

                Log4NetLogger.Debug("lstDMAString Count : " + lstDMAString.Count());
                Log4NetLogger.Debug("passed TV Market : " + tvMarket);
                Log4NetLogger.Debug("IsAllDmaAllowed : " + IsAllDmaAllowed);

                if (!string.IsNullOrWhiteSpace(tvMarket))
                {
                    if (IsAllDmaAllowed)
                    {
                        searchRequest.IQDmaName.Add(tvMarket);
                        isDmaValid = true;
                    }
                    else
                    {
                        if (lstDMAString.Contains(tvMarket))
                        {
                            isDmaValid = true;
                            searchRequest.IQDmaName.Add(tvMarket);
                        }
                        else
                        {
                            isDmaValid = false;
                        }
                    }
                }
                else
                {
                    if (!IsAllDmaAllowed)
                    {
                        isDmaValid = true;
                        searchRequest.IQDmaName = lstDMAString;
                    }
                }
                Log4NetLogger.Debug("isDmaValid : " + isDmaValid);
                Log4NetLogger.Debug("DMA List :" + string.Join(",", searchRequest.IQDmaName.ToArray()));

                if (!isDmaValid)
                {
                    throw new Exception();
                }

                if (!IsAllClassAllowed)
                {
                    searchRequest.IQClassNum = listClass.Select(s => s.Num).ToList();
                }

                if (!IsAllStationAllowed)
                {
                    searchRequest.StationAffil = listStation.Select(s => s.Name).ToList();
                }

                /*if (!string.IsNullOrWhiteSpace(tvMarket))
                {
                    searchRequest.IQDmaName = new List<string>();
                    searchRequest.IQDmaName.Add(tvMarket);
                }*/

                if (fromDate == null || toDate == null)
                {
                    searchRequest.StartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchRequest.EndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchRequest.StartDate = fromDate;
                    searchRequest.EndDate = toDate.Value.AddHours(23).AddMinutes(59);
                }

                searchRequest.EndDate = searchRequest.EndDate.Value.ToUniversalTime();

                //searchRequest.wt = ReponseType.json;
                searchRequest.IsSentiment = true;
                searchRequest.IsShowCC = true;
                searchRequest.LowThreshold = p_IQClient_ThresholdValueModel.TVLowThreshold;
                searchRequest.HighThreshold = p_IQClient_ThresholdValueModel.TVHighThreshold;

                SearchResult searchResult = searchEngine.Search(searchRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]), false);

                XDocument xDoc = new XDocument(new XElement("list"));

                //if (searchResult.Hits != null)
                // {
                foreach (Hit hit in searchResult.Hits)
                {
                    xDoc.Root.Add(new XElement("item", new XAttribute("iq_cc_key", hit.Iqcckey), new XAttribute("iq_dma", hit.IQDmaNum)));
                    DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                    discoveryMediaResult.Date = Convert.ToDateTime(hit.RLStationDateTime).ToUniversalTime();
                    discoveryMediaResult.VideoGuid = new Guid(hit.Guid);
                    discoveryMediaResult.Title = hit.Title120;
                    discoveryMediaResult.PositiveSentiment = hit.Sentiments.PositiveSentiment;
                    discoveryMediaResult.NegativeSentiment = hit.Sentiments.NegativeSentiment;
                    discoveryMediaResult.IQ_CC_Key = hit.Iqcckey;
                    discoveryMediaResult.Body = string.Join(" ", hit.TermOccurrences.Select(s => s.SurroundingText).ToArray());

                    discoveryMediaResult.MediumType = CommonFunctions.CategoryType.TV;
                    discoveryMediaResult.SearchTerm = searchTerm;
                    discoveryMediaResult.TotalRecords = searchResult.TotalHitCount;

                    discoveryMediaResult.StationLogo = ConfigurationManager.AppSettings["StationLogo"] + hit.StationId + ".jpg";
                    discoveryMediaResult.Market = hit.Market;
                    discoveryMediaResult.IsValid = true;
                    discoveryMediaResult.IncludeInResult = true;
                    discoveryMediaResult.TimeZone = hit.ClipTimeZone;
                    lstDiscoveryMediaResult.Add(discoveryMediaResult);
                }

                if (Convert.ToString(xDoc).Length > 0)
                {
                    IQNielsenDA iqNielsenDA = (IQNielsenDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNielsen);
                    lstDiscoveryMediaResult = iqNielsenDA.GetNielsenDataByXML(xDoc, lstDiscoveryMediaResult, clientGUID);
                }
                // }
                Log4NetLogger.Debug("Task: SearchTVResult -- Result_Count :" + lstDiscoveryMediaResult.Count() + "_" + DateTime.Now);
                return lstDiscoveryMediaResult;
            }
            catch (Exception)
            {
                throw;
            }


        }

        public List<DiscoveryMediaResult> SearchNewsResult(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, Guid clientGUID, IQClient_ThresholdValueModel p_IQClient_ThresholdValueModel, Int32 p_PageSize)// Int64 startRecordID, string fromRecordID,
        {
            List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();
            try
            {
                //foreach (string sterm in searchTerm)
                //{
                Log4NetLogger.Debug("Task: SearchNewsResult -- searchTerm :" + searchTerm);
                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["SolrNewsUrl"].ToString());
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchNewsRequest searchNewsRequest = new SearchNewsRequest();

                searchNewsRequest.SearchTerm = searchTerm;
                searchNewsRequest.Facet = false;
                searchNewsRequest.PageSize = p_PageSize;// Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryPageSize"]);
                //searchNewsRequest.Start = startRecordID;
                searchNewsRequest.SortFields = "date-";

                if (fromDate == null || toDate == null)
                {
                    searchNewsRequest.StartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchNewsRequest.EndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchNewsRequest.StartDate = fromDate;
                    searchNewsRequest.EndDate = toDate.Value.AddHours(23).AddMinutes(59);
                }

                searchNewsRequest.EndDate = searchNewsRequest.EndDate.Value.ToUniversalTime();

                /*if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    searchNewsRequest.FromRecordID = Convert.ToString(fromRecordID);
                }*/

                searchNewsRequest.IsSentiment = true;

                searchNewsRequest.HighThreshold = p_IQClient_ThresholdValueModel.NMHighThreshold;
                searchNewsRequest.LowThreshold = p_IQClient_ThresholdValueModel.NMLowThreshold;

                SearchNewsResults searchNewsResults = searchEngine.SearchNews(searchNewsRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));



                //if (searchNewsResults.newsResults != null)
                //{
                foreach (NewsResult newsResult in searchNewsResults.newsResults)
                {
                    DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                    discoveryMediaResult.Date = Convert.ToDateTime(newsResult.date);
                    discoveryMediaResult.Title = newsResult.Title;
                    discoveryMediaResult.PositiveSentiment = newsResult.Sentiments.PositiveSentiment;
                    discoveryMediaResult.NegativeSentiment = newsResult.Sentiments.NegativeSentiment;
                    discoveryMediaResult.Body = newsResult.Content;
                    discoveryMediaResult.ArticleURL = newsResult.Article;
                    discoveryMediaResult.Publication = newsResult.HomeurlDomain;
                    discoveryMediaResult.ArticleID = newsResult.IQSeqID;

                    discoveryMediaResult.MediumType = CommonFunctions.CategoryType.NM;
                    discoveryMediaResult.SearchTerm = searchTerm;
                    discoveryMediaResult.TotalRecords = searchNewsResults.TotalResults;
                    discoveryMediaResult.IsValid = true;
                    discoveryMediaResult.IncludeInResult = true;

                    lstDiscoveryMediaResult.Add(discoveryMediaResult);

                }

                Uri aPublicationUri;
                var distinctDisplayUrl = searchNewsResults.newsResults.Select(a => !string.IsNullOrWhiteSpace(a.HomeurlDomain) && Uri.TryCreate(a.HomeurlDomain, UriKind.Absolute, out aPublicationUri) ? aPublicationUri.Host.Replace("www.", "") : string.Empty).Distinct().ToList();

                var displyUrlXml = new XElement("list",
                                        from string websiteurl in distinctDisplayUrl
                                        select new XElement("item", new XAttribute("url", websiteurl)));

                IQCompeteAllDA iqCompeteAllDA = (IQCompeteAllDA)DataAccessFactory.GetDataAccess(DataAccessType.IQCompeteAll);
                List<IQCompeteAll> _ListOfIQ_CompeteAll = iqCompeteAllDA.GetArtileAdShareValueByClientGuidAndXml(clientGUID, Convert.ToString(displyUrlXml), CommonFunctions.CategoryType.NM.ToString());
                foreach (DiscoveryMediaResult discoveryMediaResult in lstDiscoveryMediaResult)
                {
                    Uri aUri;
                    string href = !string.IsNullOrWhiteSpace(discoveryMediaResult.Publication) && Uri.TryCreate(discoveryMediaResult.Publication, UriKind.Absolute, out aUri) ? aUri.Host.Replace("www.", "") : string.Empty;
                    IQCompeteAll _IQCompeteAll = _ListOfIQ_CompeteAll.Find(a => a.CompeteURL.Equals(href));



                    discoveryMediaResult.Audience = (_IQCompeteAll == null || (_IQCompeteAll.c_uniq_visitor == null || !_IQCompeteAll.IsUrlFound)) ? (int?)null : _IQCompeteAll.c_uniq_visitor;
                    if ((_IQCompeteAll != null && (_IQCompeteAll.c_uniq_visitor == -1)))
                    {
                        discoveryMediaResult.Audience = null;
                    }

                    discoveryMediaResult.CompeteImage = (_IQCompeteAll.IsCompeteAll ? "<img src=\"../Images/compete.jpg\" style=\"width:14px\"  title=\"Powered by Compete\" />" : "");

                    discoveryMediaResult.IQAdsharevalue = (_IQCompeteAll == null || (_IQCompeteAll.IQ_AdShare_Value == null || !_IQCompeteAll.IsUrlFound)) ? (decimal?)null : _IQCompeteAll.IQ_AdShare_Value.Value;
                    if ((_IQCompeteAll != null && (_IQCompeteAll.IQ_AdShare_Value == -1)))
                    {
                        discoveryMediaResult.IQAdsharevalue = null;
                    }
                }

                // }
                Log4NetLogger.Debug("Task: SearchNewsResult -- Result_Count :" + lstDiscoveryMediaResult.Count() + "_" + DateTime.Now);
                return lstDiscoveryMediaResult;


            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<DiscoveryMediaResult> SearchSocialMediaResult(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, Guid clientGUID, IQClient_ThresholdValueModel p_IQClient_ThresholdValueModel, Int32 p_PageSize)//Int64 startRecordID, string fromRecordID,
        {
            List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<DiscoveryMediaResult>();
            try
            {
                Log4NetLogger.Debug("Task: SearchSocialMediaResult -- searchTerm :" + searchTerm);
                bool isError = false;
                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["SolrSMUrl"].ToString());
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchSMRequest searchSMRequest = new SearchSMRequest();

                searchSMRequest.SearchTerm = searchTerm;
                searchSMRequest.Facet = true;
                searchSMRequest.FacetRangeOther = "all";

                //searchSMRequest.Start = startRecordID;
                searchSMRequest.PageSize = p_PageSize; // Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryPageSize"]);
                searchSMRequest.SortFields = "date-";

                if (fromDate == null || toDate == null)
                {
                    searchSMRequest.StartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchSMRequest.EndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchSMRequest.StartDate = fromDate;
                    searchSMRequest.EndDate = toDate.Value.AddHours(23).AddMinutes(59);
                }

                searchSMRequest.EndDate = searchSMRequest.EndDate.Value.ToUniversalTime();

                searchSMRequest.IsTaggingExcluded = true;
                if (!string.IsNullOrWhiteSpace(medium))
                {
                    searchSMRequest.SourceType = new List<string>();

                    if (medium == CommonFunctions.CategoryType.Blog.ToString())
                    {
                        searchSMRequest.SourceType.Add(CommonFunctions.CategoryType.Blog.ToString());
                        //searchSMRequest.SourceType.Add(medium);
                        //searchSMRequest.SourceType.Add("Comment");
                    }
                    else if (medium == CommonFunctions.CategoryType.Forum.ToString())
                    {
                        searchSMRequest.SourceType.Add(CommonFunctions.CategoryType.Forum.ToString());
                        //searchSMRequest.SourceType.Add(medium);
                        //searchSMRequest.SourceType.Add("Review");
                    }
                    else
                    {
                        searchSMRequest.SourceType.Add(CommonFunctions.CategoryType.SocialMedia.ToString());
                        /*searchSMRequest.ExcludedSourceType = new List<string>();
                        searchSMRequest.ExcludedSourceType.Add("Blog");
                        searchSMRequest.ExcludedSourceType.Add("Review");
                        searchSMRequest.ExcludedSourceType.Add("Forum");
                        searchSMRequest.ExcludedSourceType.Add("Comment");*/
                    }
                }

                /*if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    searchSMRequest.FromRecordID = Convert.ToString(fromRecordID);
                }*/

                searchSMRequest.IsSentiment = true;

                searchSMRequest.HighThreshold = p_IQClient_ThresholdValueModel.SMHighThreshold;
                searchSMRequest.LowThreshold = p_IQClient_ThresholdValueModel.SMLowThreshold;

                SearchSMResult searchSMResult = searchEngine.SearchSocialMedia(searchSMRequest, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));



                foreach (SMResult smResult in searchSMResult.smResults)
                {

                    DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                    discoveryMediaResult.Date = Convert.ToDateTime(smResult.itemHarvestDate_DT);
                    discoveryMediaResult.Title = smResult.description;
                    discoveryMediaResult.PositiveSentiment = smResult.Sentiments.PositiveSentiment;
                    discoveryMediaResult.NegativeSentiment = smResult.Sentiments.NegativeSentiment;
                    discoveryMediaResult.Body = smResult.content;
                    discoveryMediaResult.ArticleURL = smResult.link;
                    discoveryMediaResult.Publication = smResult.HomeurlDomain;
                    discoveryMediaResult.ArticleID = smResult.IQSeqID;
                    discoveryMediaResult.TotalRecords = searchSMResult.TotalResults;
                    discoveryMediaResult.IsValid = true;
                    discoveryMediaResult.IncludeInResult = true;

                    string mType = GetFeedClass(smResult.feedClass);
                    if (mType == "Social Media")
                    {
                        discoveryMediaResult.MediumType = CommonFunctions.CategoryType.SocialMedia;
                    }
                    else if (mType == CommonFunctions.CategoryType.Blog.ToString())
                    {
                        discoveryMediaResult.MediumType = CommonFunctions.CategoryType.Blog;
                    }
                    else if (mType == CommonFunctions.CategoryType.Forum.ToString())
                    {
                        discoveryMediaResult.MediumType = CommonFunctions.CategoryType.Forum;
                    }

                    discoveryMediaResult.SearchTerm = searchTerm;

                    lstDiscoveryMediaResult.Add(discoveryMediaResult);
                }

                List<SMResult> lstSMResults = new List<SMResult>();

                Uri aHomeLinkUri;
                lstSMResults = searchSMResult.smResults.Select(a => new SMResult()
                {
                    HomeurlDomain = !string.IsNullOrWhiteSpace(a.HomeurlDomain) && Uri.TryCreate(a.HomeurlDomain, UriKind.Absolute, out aHomeLinkUri) ? aHomeLinkUri.Host.Replace("www.", "") : string.Empty,
                    feedClass = !string.IsNullOrWhiteSpace(a.feedClass) ? a.feedClass : string.Empty
                }
                                                                                ).GroupBy(h => h.HomeurlDomain)
                                                                                    .Select(s => s.First()).ToList();

                var displyUrlXml = new XElement("list",
                                        from SMResult smres in lstSMResults
                                        select new XElement("item", new XAttribute("url", smres.HomeurlDomain), new XAttribute("sourceCategory", smres.feedClass)));
                IQCompeteAllDA iqCompeteAllDA = (IQCompeteAllDA)DataAccessFactory.GetDataAccess(DataAccessType.IQCompeteAll);
                List<IQCompeteAll> _ListOfIQ_CompeteAll = iqCompeteAllDA.GetArtileAdShareValueByClientGuidAndXml(clientGUID, Convert.ToString(displyUrlXml), CommonFunctions.CategoryType.NM.ToString());
                foreach (DiscoveryMediaResult discoveryMediaResult in lstDiscoveryMediaResult)
                {
                    Uri aPublicationUri;
                    string href = !string.IsNullOrWhiteSpace(discoveryMediaResult.Publication) && Uri.TryCreate(discoveryMediaResult.Publication, UriKind.Absolute, out aPublicationUri) ? aPublicationUri.Host.Replace("www.", "") : string.Empty;
                    IQCompeteAll _IQCompeteAll = _ListOfIQ_CompeteAll.Find(a => a.CompeteURL.Equals(href));



                    discoveryMediaResult.Audience = (_IQCompeteAll == null || (_IQCompeteAll.c_uniq_visitor == null || !_IQCompeteAll.IsUrlFound)) ? null : _IQCompeteAll.c_uniq_visitor;
                    if ((_IQCompeteAll != null && (_IQCompeteAll.c_uniq_visitor == -1)))
                    {
                        discoveryMediaResult.Audience = null;
                    }

                    discoveryMediaResult.CompeteImage = (_IQCompeteAll.IsCompeteAll ? "<img src=\"../Images/compete.jpg\" style=\"width:14px\"  title=\"Powered by Compete\" />" : "");

                    discoveryMediaResult.IQAdsharevalue = (_IQCompeteAll == null || (_IQCompeteAll.IQ_AdShare_Value == null || !_IQCompeteAll.IsUrlFound)) ? null : _IQCompeteAll.IQ_AdShare_Value;
                    if ((_IQCompeteAll != null && (_IQCompeteAll.IQ_AdShare_Value == -1)))
                    {
                        discoveryMediaResult.IQAdsharevalue = null;
                    }
                }

                Log4NetLogger.Debug("Task: SearchSocialMediaResult -- Result_Count :" + lstDiscoveryMediaResult.Count() + "_" + DateTime.Now);
                return lstDiscoveryMediaResult;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<DiscoveryMediaResult> SearchTwitterResult(string searchTerm, DateTime? fromDate, DateTime? toDate, string medium, IQClient_ThresholdValueModel p_IQClient_ThresholdValueModel, Int32 p_PageSize)//Int64 startRecordID, string fromRecordID,
        {
            List<DiscoveryMediaResult> lstDiscoveryMediaResult = new List<Model.DiscoveryMediaResult>();
            try
            {
                Log4NetLogger.Debug("Task: SearchTwitterResult -- searchTerm :" + searchTerm);
                bool isError = false;
                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["SolrTwitterUrl"].ToString());
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);
                SearchTwitterRequest searchTwitterRequest = new SearchTwitterRequest();

                searchTwitterRequest.SearchTerm = searchTerm;
                searchTwitterRequest.Facet = false;
                searchTwitterRequest.PageSize = p_PageSize; // Convert.ToInt32(ConfigurationManager.AppSettings["DiscoveryPageSize"]);
                //searchTwitterRequest.Start = startRecordID;
                searchTwitterRequest.SortFields = "date-";

                if (fromDate == null || toDate == null)
                {
                    searchTwitterRequest.StartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToShortDateString());
                    searchTwitterRequest.EndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddHours(23).AddMinutes(59);
                }
                else
                {
                    searchTwitterRequest.StartDate = fromDate;
                    searchTwitterRequest.EndDate = toDate.Value.AddHours(23).AddMinutes(59);
                }

                searchTwitterRequest.EndDate = searchTwitterRequest.EndDate.Value.ToUniversalTime();

                /*if (!string.IsNullOrWhiteSpace(fromRecordID))
                {
                    searchTwitterRequest.FromRecordID = Convert.ToString(fromRecordID);
                }*/

                searchTwitterRequest.IsSentiment = true;

                searchTwitterRequest.HighThreshold = p_IQClient_ThresholdValueModel.TwitterHighThreshold;
                searchTwitterRequest.LowThreshold = p_IQClient_ThresholdValueModel.TwitterLowThreshold;

                searchTwitterRequest.IsDeleted = false;

                SearchTwitterResult searchTwitterResult = searchEngine.SearchTwitter(searchTwitterRequest, false, out isError, Convert.ToInt32(ConfigurationManager.AppSettings["SolrRequestDuration"]));

                foreach (TwitterResult twitterResult in searchTwitterResult.TwitterResults)
                {
                    DiscoveryMediaResult discoveryMediaResult = new DiscoveryMediaResult();
                    discoveryMediaResult.Date = Convert.ToDateTime(twitterResult.tweet_postedDateTime);
                    discoveryMediaResult.TwitterDisplayName = twitterResult.actor_displayName;
                    discoveryMediaResult.TwitterPrefferedName = twitterResult.actor_prefferedUserName;
                    discoveryMediaResult.TwitterFollowers = Convert.ToInt32(twitterResult.followers_count);
                    discoveryMediaResult.TwitterFriends = Convert.ToInt32(twitterResult.friends_count);
                    discoveryMediaResult.TwitterKLOutScore = Convert.ToInt32(twitterResult.Klout_score);
                    discoveryMediaResult.Body = twitterResult.tweet_body;
                    discoveryMediaResult.TwitterUserURL = twitterResult.actor_image;
                    discoveryMediaResult.TwitterActorLink = twitterResult.actor_link;
                    discoveryMediaResult.PositiveSentiment = twitterResult.Sentiments.PositiveSentiment;
                    discoveryMediaResult.NegativeSentiment = twitterResult.Sentiments.NegativeSentiment;
                    discoveryMediaResult.TotalRecords = searchTwitterResult.TotalResults;
                    discoveryMediaResult.ArticleID = Convert.ToString(twitterResult.iqseqid);
                    discoveryMediaResult.MediumType = CommonFunctions.CategoryType.TW;
                    discoveryMediaResult.SearchTerm = searchTerm;
                    discoveryMediaResult.IsValid = true;
                    discoveryMediaResult.IncludeInResult = true;

                    lstDiscoveryMediaResult.Add(discoveryMediaResult);
                }

                Log4NetLogger.Debug("Task: SearchTwitterResult -- Result_Count :" + lstDiscoveryMediaResult.Count() + "_" + DateTime.Now);
                return lstDiscoveryMediaResult;
            }
            catch (Exception)
            {
                throw;
            }


        }

        #endregion

        public string GetFeedClass(string feedClass)
        {
            if (feedClass.Trim() == "Blog" || feedClass.Trim() == "Comment")
            {
                return "Blog";
            }
            else if (feedClass.Trim() == "Review" || feedClass.Trim() == "Forum")
            {
                return "Forum";
            }
            else
            {
                return "Social Media";
            }
        }

        #region GetFilter

        public IEnumerable GetDateFilter(List<DiscoverySearchResponse> lstDiscoverySearchResponse)
        {
            try
            {
                var distinctDateFilter = (from x in
                                              (
                                                  from sr in lstDiscoverySearchResponse
                                                  from dc in sr.ListRecordData
                                                  select new { dc.Date, dc.TotalRecord }

                                              )
                                          where Convert.ToInt64(x.TotalRecord) > 0
                                          group x by new { x.Date, x.TotalRecord } into g
                                          select new
                                          {
                                              Date = Convert.ToDateTime(g.Key.Date).ToString("MM/dd/yyyy")
                                          }).Distinct().ToList();




                return distinctDateFilter;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IEnumerable GetMediumFilter(List<DiscoverySearchResponse> lstDiscoverySearchResponse, List<DiscoverySearchResponse> lstDiscoveryFeedClass)
        {
            try
            {
                var distinctDateFilter = (from x in
                                              (
                                                  from sr in lstDiscoverySearchResponse
                                                  from dc in sr.ListRecordData
                                                  select new { dc.TotalRecord, sr.MediumType }

                                              )
                                          where Convert.ToInt64(x.TotalRecord) > 0
                                          && x.MediumType != "SocialMedia"
                                          group x by new { x.TotalRecord, x.MediumType } into g
                                          select new
                                          {
                                              //Medium = CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(g.Key.MediumType)),
                                              Medium = g.Key.MediumType
                                          }).Distinct().ToList();


                if (lstDiscoveryFeedClass != null && lstDiscoveryFeedClass.Count > 0 && lstDiscoveryFeedClass.FirstOrDefault().ListRecordData.Count > 0)
                {
                    var distinctFeedClass = (from x in
                                                 (
                                                     from sr in lstDiscoveryFeedClass
                                                     from dc in sr.ListRecordData
                                                     select new { dc.TotalRecord, dc.FeedClass }

                                                 )
                                             where Convert.ToInt64(x.TotalRecord) > 0
                                             group x by new { x.TotalRecord, x.FeedClass } into g
                                             select new
                                             {
                                                 //Medium = GetFeedClass(g.Key.FeedClass),
                                                 Medium = GetFeedClass(g.Key.FeedClass)
                                             }).Distinct().ToList();

                    distinctDateFilter.AddRange(distinctFeedClass);
                }
                //distinctDateFilter = distinctDateFilter.Distinct().ToList();
                List<MediumFilterData> lstMediumFilterData = distinctDateFilter.Select(s => new MediumFilterData()
                {
                    Medium = s.Medium,
                    MediumValue = CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonFunctions.CategoryType>(s.Medium.Replace(" ", string.Empty))),
                }).ToList();
                return lstMediumFilterData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public List<HighChartDatum> GetSeriesList(Int64 value)
        {
            HighChartDatum highChartDatum = new HighChartDatum();
            List<HighChartDatum> lsthighChartDatum = new List<HighChartDatum>();

            highChartDatum.y = value;

            lsthighChartDatum.Add(highChartDatum);
            return lsthighChartDatum;
        }

    }
}

