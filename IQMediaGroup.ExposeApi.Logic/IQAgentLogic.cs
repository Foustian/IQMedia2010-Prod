using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Data.Objects;
using PMGSearch;
using System.Configuration;
using System.Xml.Linq;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.ExposeApi.Logic
{
    public class IQAgentLogic : BaseLogic, ILogic
    {
        public List<TVAgent> GetIQAgents(Guid p_ClientGUID, string p_SRIDList)
        {
            return Context.GetTVRequestsByClientGuid(p_ClientGUID, p_SRIDList).ToList();
        }

        public TVAgentResultsOutput GetTVAgentResults(TVAgentResultsInput p_TVIQAgentResultsInput, Guid p_ClientGUID, Guid p_CustomerGuid, string p_HttpRequestClientIP = null)
        {
            TVAgentResultsOutput tvIQAgentResultsOutput = new TVAgentResultsOutput();
            Int64 MaxID = 0;

            Log4NetLogger.Debug(p_HttpRequestClientIP + " - p_ClientGUID: " + p_ClientGUID + ", p_CustomerGuid: " + p_CustomerGuid + ", SRID: " + p_TVIQAgentResultsInput.SRID + ", SeqID: " + p_TVIQAgentResultsInput.SeqID + ", Rows: " + p_TVIQAgentResultsInput.Rows);

            //tvIQAgentResultsOutput.TVResultList = Context.GetIQAgentTVResultsBySearchRequestID(p_ClientGUID, p_CustomerGuid, p_TVIQAgentResultsInput.SRID, p_TVIQAgentResultsInput.SeqID, p_TVIQAgentResultsInput.Rows, out TotalResults, out MaxID).ToList();

            Uri feedsSearchRequestUrl = new Uri(SolrEngineLogic.GeneratePMGUrl(SolrEngineLogic.PMGUrlType.FE.ToString(), null, null));

            Dictionary<string, List<int>> pendingDeleteResults = Context.GetPendingDeleteAgentRequestsNResults(p_ClientGUID);

            FeedsSearch.SearchEngine se = new FeedsSearch.SearchEngine(feedsSearchRequestUrl);

            FeedsSearch.SearchRequest feedsSR = new FeedsSearch.SearchRequest();

            if (pendingDeleteResults.ContainsKey("MediaResult") && pendingDeleteResults["MediaResult"].Count > 0)
            {
                feedsSR.ExcludeIDs = pendingDeleteResults["MediaResult"].Select(mr => mr.ToString()).ToList();
            }

            if (pendingDeleteResults.ContainsKey("SearchRequest") && pendingDeleteResults["SearchRequest"].Count > 0)
            {
                feedsSR.ExcludeSearchRequestIDs = pendingDeleteResults["SearchRequest"].Select(mr => mr.ToString()).ToList();
            }

            if (p_TVIQAgentResultsInput.SRID.HasValue)
            {
                feedsSR.SearchRequestIDs = new List<string> { p_TVIQAgentResultsInput.SRID.Value.ToString() };
            }

            feedsSR.IsInitialSearch = false;
            feedsSR.IsOnlyParents = false;
            feedsSR.IsFaceting = false;
            feedsSR.MediaCategories = new List<string> { "TV" };
            feedsSR.SortType = FeedsSearch.SortType.IQSEQID;
            feedsSR.IsSortAsc = false;
            feedsSR.PageSize = 1;
            feedsSR.ClientGUID = p_ClientGUID;

            var feedsMaxResult = se.Search(feedsSR);

            if (feedsMaxResult["Results"].TotalHitCount > 0)
            {
                Log4NetLogger.Debug(p_HttpRequestClientIP + " - TotalResults: " + feedsMaxResult["Results"].TotalHitCount + " - MaxID :" + MaxID);

                tvIQAgentResultsOutput.TotalResults = feedsMaxResult["Results"].TotalHitCount;

                MaxID = feedsMaxResult["Results"].Hits.FirstOrDefault().ID;

                feedsSR.PageSize = p_TVIQAgentResultsInput.Rows.Value;
                feedsSR.IsSortAsc = true;

                if (p_TVIQAgentResultsInput.SeqID.HasValue)
                {
                    if (p_TVIQAgentResultsInput.SeqID > 0)
                    {
                        p_TVIQAgentResultsInput.SeqID = p_TVIQAgentResultsInput.SeqID + 1;
                    }
                }
                else
                {
                    p_TVIQAgentResultsInput.SeqID = 0;
                }

                feedsSR.SinceIDAsc = p_TVIQAgentResultsInput.SeqID;

                var feedsResult = se.Search(feedsSR)["Results"];

                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
                var isNilsenAccess = authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4TV, p_CustomerGuid);

                tvIQAgentResultsOutput.TVResultList = PopulateTVResultsFromFeedsResults<TVResult>(feedsResult, isNilsenAccess);

                if (p_TVIQAgentResultsInput.IsBoolFetchFullCCData.HasValue && p_TVIQAgentResultsInput.IsBoolFetchFullCCData == true)
                {
                    List<String> lstOfGuids = tvIQAgentResultsOutput.TVResultList.Select(a => a.VideoGuid.ToString()).ToList();
                    tvIQAgentResultsOutput.TVResultList = GetTVAgentClosedCaptions(lstOfGuids, tvIQAgentResultsOutput.TVResultList);
                }
            }

            if (tvIQAgentResultsOutput.TVResultList != null && tvIQAgentResultsOutput.TVResultList.Count > 0 && tvIQAgentResultsOutput.TVResultList.Max(a => a.SeqID) < MaxID)
            {
                tvIQAgentResultsOutput.HasNextPage = true;
            }
            else
            {
                tvIQAgentResultsOutput.HasNextPage = false;
            }

            return tvIQAgentResultsOutput;

        }

        private List<T> PopulateTVResultsFromFeedsResults<T>(FeedsSearch.SearchResult feedsResult, bool isNielsenAccess)
        {
            var tvResultList = new List<T>();

            foreach (var fr in feedsResult.Hits)
            {
                dynamic tvResult = Activator.CreateInstance<T>();

                tvResult.SeqID = fr.ID;
                tvResult.ProgramTitle = fr.Title;
                tvResult.SRID = fr.SearchRequestID;
                tvResult.GMTDateTime = fr.MediaDate;
                tvResult.MediaDateTime = fr.LocalDate;
                tvResult.StationID = fr.StationID;
                tvResult.Station = fr.Outlet;
                tvResult.DmaName = fr.Market;
                tvResult.HitCount = fr.NumberOfHits;

                if (isNielsenAccess)
                {
                    tvResult.Audience = fr.Audience;
                    tvResult.MediaValue = fr.MediaValue;
                }

                tvResult.PositiveSentiment = fr.PositiveSentiment;
                tvResult.NegativeSentiment = fr.NegativeSentiment;
                tvResult.VideoGuid = fr.VideoGUID;
                tvResult.ParentID = fr.ParentID;
                tvResult.ThumbUrl = fr.ThumbnailUrl;

                tvResult.StationLogo = ConfigurationManager.AppSettings["StationLogoURL"] + fr.StationID + ".jpg";
                tvResult.URL = ConfigurationManager.AppSettings["IframeURL"] + "?SeqID=" + fr.ID;
                tvResult.HighLights = !string.IsNullOrWhiteSpace(fr.HighlightingText) ? GetCCHighLights(fr.HighlightingText) : null;

                tvResultList.Add(tvResult);
            }

            return tvResultList;
        }

        public TVAgentResultsUpdateOutput GetTVAgentResultsUpdate(TVAgentResultsUpdateInput p_TVAgentResultsUpdateInput, Guid p_ClientGUID, Guid p_CustomerGUID)
        {
            TVAgentResultsUpdateOutput tvIQAgentResultsUpdateOutput = new TVAgentResultsUpdateOutput();

            Int64 totalResults = 0;
            Int64 maxID = 0;

            var updatedIDList = Context.GetIQAgentTVResultsUpdate(p_ClientGUID, p_CustomerGUID, p_TVAgentResultsUpdateInput.USeqID, p_TVAgentResultsUpdateInput.Rows, out totalResults, out maxID).ToList();

            Uri feedsSearchRequestUrl = new Uri(SolrEngineLogic.GeneratePMGUrl(SolrEngineLogic.PMGUrlType.FE.ToString(), null, null));

            Dictionary<string, List<int>> pendingDeleteResults = Context.GetPendingDeleteAgentRequestsNResults(p_ClientGUID);

            FeedsSearch.SearchEngine se = new FeedsSearch.SearchEngine(feedsSearchRequestUrl);

            FeedsSearch.SearchRequest feedsSR = new FeedsSearch.SearchRequest();

            if (pendingDeleteResults.ContainsKey("MediaResult") && pendingDeleteResults["MediaResult"].Count > 0)
            {
                feedsSR.ExcludeIDs = pendingDeleteResults["MediaResult"].Select(mr => mr.ToString()).ToList();
            }

            if (pendingDeleteResults.ContainsKey("SearchRequest") && pendingDeleteResults["SearchRequest"].Count > 0)
            {
                feedsSR.ExcludeSearchRequestIDs = pendingDeleteResults["SearchRequest"].Select(mr => mr.ToString()).ToList();
            }

            feedsSR.IsInitialSearch = false;
            feedsSR.IsOnlyParents = false;
            feedsSR.IsFaceting = false;
            feedsSR.MediaCategories = new List<string> { "TV" };
            feedsSR.PageSize = p_TVAgentResultsUpdateInput.Rows.Value;
            feedsSR.MediaIDs = updatedIDList.Select(a => a.Value.ToString()).ToList();

            var feedsResult = se.Search(feedsSR)["Results"];

            AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);
            var isNilsenAccess = authenticationLogic.AuthenticateRoleAccess(CommonConstants.Roles.v4TV, p_CustomerGUID);

            tvIQAgentResultsUpdateOutput.TVResultList = PopulateTVResultsFromFeedsResults<UpdatedTVResult>(feedsResult, isNilsenAccess);

            tvIQAgentResultsUpdateOutput.TVResultList.ForEach(tvr => tvr.USeqID = updatedIDList.Where(uid => uid.Value == tvr.SeqID).Last().Key);

            tvIQAgentResultsUpdateOutput.TVResultList = tvIQAgentResultsUpdateOutput.TVResultList.OrderBy(tvr => tvr.USeqID).ToList();

            tvIQAgentResultsUpdateOutput.TotalResults = totalResults;

            if (p_TVAgentResultsUpdateInput.IsBoolFetchFullCCData.HasValue && p_TVAgentResultsUpdateInput.IsBoolFetchFullCCData == true)
            {
                List<String> lstOfGuids = tvIQAgentResultsUpdateOutput.TVResultList.Select(a => a.VideoGuid.ToString()).ToList();
                tvIQAgentResultsUpdateOutput.TVResultList = GetTVAgentClosedCaptions(lstOfGuids, tvIQAgentResultsUpdateOutput.TVResultList);
            }

            if (tvIQAgentResultsUpdateOutput.TVResultList != null && tvIQAgentResultsUpdateOutput.TVResultList.Count > 0 && tvIQAgentResultsUpdateOutput.TVResultList.Max(a => a.USeqID) < maxID)
            {
                tvIQAgentResultsUpdateOutput.HasNextPage = true;
            }
            else
            {
                tvIQAgentResultsUpdateOutput.HasNextPage = false;
            }

            return tvIQAgentResultsUpdateOutput;
        }

        public List<DaySummary> GetTVAgentDailySummary(TVAgentDaySummaryInput p_TVAgentDaySummaryInput, Guid p_ClientGUID)
        {
            return Context.GetTVIQAgentDaySummary(p_ClientGUID, p_TVAgentDaySummaryInput.FromDate, p_TVAgentDaySummaryInput.ToDate, p_TVAgentDaySummaryInput.SRID).ToList();
        }

        public List<HourSummary> GetTVAgentHourSummary(TVAgentHourSummaryInput p_TVAgentHourSummaryInput, Guid p_ClientGUID)
        {
            return Context.GetTVAgentHourSummary(p_ClientGUID, p_TVAgentHourSummaryInput.FromDateTime, p_TVAgentHourSummaryInput.ToDateTime, p_TVAgentHourSummaryInput.SRID).ToList();
        }

        public List<TVResult> GetTVAgentClosedCaptions(List<string> p_Guids, List<TVResult> p_TVResultList)
        {
            try
            {

                Uri PMGSearchRequestUrl = new Uri(SolrEngineLogic.GeneratePMGUrl(SolrEngineLogic.PMGUrlType.TV.ToString(), null, null));

                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchRequest _SearchRequest = new SearchRequest();


                _SearchRequest.PageSize = p_Guids.Count();
                _SearchRequest.GuidList = string.Join(",", p_Guids);


                string customFl = ConfigurationManager.AppSettings["SolrFLCC"];
                SearchResult _SearchResult = _SearchEngine.Search(_SearchRequest, null, false, customFl);

                if (_SearchResult != null && _SearchResult.Hits != null)
                {
                    foreach (Hit hit in _SearchResult.Hits)
                    {
                        var tvResult = p_TVResultList.Where(a => a.VideoGuid.Equals(new Guid(hit.Guid))).FirstOrDefault();
                        if (tvResult != null)
                        {
                            tvResult.CC = string.Join(" ", hit.ClosedCaption.Select(a => a.TimeOffset + "s:" + a.SurroundingText));
                        }
                    }
                }

                return p_TVResultList;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

        public List<UpdatedTVResult> GetTVAgentClosedCaptions(List<string> p_Guids, List<UpdatedTVResult> p_TVResultList)
        {
            try
            {

                Uri PMGSearchRequestUrl = new Uri(SolrEngineLogic.GeneratePMGUrl(SolrEngineLogic.PMGUrlType.TV.ToString(), null, null));

                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchRequest _SearchRequest = new SearchRequest();


                _SearchRequest.PageSize = p_Guids.Count();
                _SearchRequest.GuidList = string.Join(",", p_Guids);


                string customFl = ConfigurationManager.AppSettings["SolrFLCC"];
                SearchResult _SearchResult = _SearchEngine.Search(_SearchRequest, null, false, customFl);

                if (_SearchResult != null && _SearchResult.Hits != null)
                {
                    foreach (Hit hit in _SearchResult.Hits)
                    {
                        var tvResult = p_TVResultList.Where(a => a.VideoGuid.Equals(new Guid(hit.Guid))).FirstOrDefault();
                        if (tvResult != null)
                        {
                            tvResult.CC = string.Join(" ", hit.ClosedCaption.Select(a => a.TimeOffset + "s:" + a.SurroundingText));
                        }
                    }
                }

                return p_TVResultList;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

        public Dictionary<string, object> CreateTVAgents(CreateTVAgentInput p_CreateTVAgentInput, string p_SearchTerm, Guid p_ClientGUID)
        {
            return Context.CreateTVAgent(p_CreateTVAgentInput, p_SearchTerm, p_ClientGUID);
        }

        public int UpdateTVAgents(UpdateTVAgentInput p_UpdateTVAgentInput, string p_SearchTerm, Guid p_ClientGUID)
        {
            return Context.UpdateTVAgent(p_UpdateTVAgentInput, p_SearchTerm, p_ClientGUID);
        }

        public int DeleteTVAgents(DeleteTVAgentInput p_DeleteTVAgentInput, Guid p_ClientGUID, Guid p_CustomerGUID)
        {
            return Context.DeleteTVAgent(p_DeleteTVAgentInput, p_ClientGUID, p_CustomerGUID);
        }

        public int SuspendTVAgent(SuspendTVAgentInput p_SuspendTVAgentInput, Guid p_ClientGUID, Guid p_CustomerGUID, out short? o_PreviousIsActive)
        {
            return Context.SuspendTVAgent(p_SuspendTVAgentInput, p_ClientGUID, p_CustomerGUID, out o_PreviousIsActive);
        }

        public int UnSuspendTVAgent(UnSuspendTVAgentInput p_unSuspendTVAgentInput, Guid p_ClientGUID, Guid p_CustomerGUID, out short? o_PreviousIsActive)
        {
            return Context.UnSuspendTVAgent(p_unSuspendTVAgentInput.SRID, p_ClientGUID, p_CustomerGUID, out o_PreviousIsActive);
        }

        public string ChangeNodeName(string searchRequest)
        {
            XDocument docTVAgent = XDocument.Parse(searchRequest);
            try
            {
                if (docTVAgent.Element("SearchRequest") != null)
                {
                    XElement searchTerm = new XElement(docTVAgent.Element("SearchRequest"));
                    if (docTVAgent.Root.Element("TV").Element("SearchTerm") != null)
                    {
                        searchTerm.Name = "SearchTerm";
                        searchTerm.Value = docTVAgent.Root.Element("TV").Element("SearchTerm").Value;
                        docTVAgent.Element("SearchRequest").Add(searchTerm);
                    }
                }

                if (docTVAgent.Root.Element("TV").Element("DmaList") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("DmaList").Elements("Dma") != null)
                    {
                        if (docTVAgent.Root.Element("TV").Element("DmaList").Elements("Dma").Count() > 0)
                        {
                            docTVAgent.Root.Element("TV").Element("DmaList").SetAttributeValue("IsAllowAll", "false");
                        }
                        else
                        {
                            docTVAgent.Root.Element("TV").Element("DmaList").SetAttributeValue("IsAllowAll", "true");
                        }
                        foreach (var Dma in docTVAgent.Root.Element("TV").Element("DmaList").Elements("Dma"))
                        {
                            Dma.Name = "IQ_Dma";
                        }
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("DmaList").SetAttributeValue("IsAllowAll", "true");
                    }
                    docTVAgent.Root.Element("TV").Element("DmaList").Name = "IQ_Dma_Set";
                }
                else
                {
                    XElement xDma = new XElement("IQ_Dma_Set");
                    xDma.SetAttributeValue("IsAllowAll", "true");
                    docTVAgent.Root.Element("TV").Add(xDma);
                }

                if (docTVAgent.Root.Element("TV").Element("AffiliateList") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("AffiliateList").Elements("Affiliate") != null)
                    {
                        if (docTVAgent.Root.Element("TV").Element("AffiliateList").Elements("Affiliate").Count() > 0)
                        {
                            docTVAgent.Root.Element("TV").Element("AffiliateList").SetAttributeValue("IsAllowAll", "false");
                        }
                        else
                        {
                            docTVAgent.Root.Element("TV").Element("AffiliateList").SetAttributeValue("IsAllowAll", "true");
                        }
                        foreach (var Affiliate in docTVAgent.Root.Element("TV").Element("AffiliateList").Elements("Affiliate"))
                        {
                            Affiliate.Name = "Station_Affil";
                        }
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("AffiliateList").SetAttributeValue("IsAllowAll", "true");
                    }
                    docTVAgent.Root.Element("TV").Element("AffiliateList").Name = "Station_Affiliate_Set";
                }
                else
                {
                    XElement xAffiliate = new XElement("Station_Affiliate_Set");
                    xAffiliate.SetAttributeValue("IsAllowAll", "true");
                    docTVAgent.Root.Element("TV").Add(xAffiliate);
                }

                if (docTVAgent.Root.Element("TV").Element("ProgramCategoryList") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("ProgramCategoryList").Elements("ProgramCategory") != null)
                    {
                        if (docTVAgent.Root.Element("TV").Element("ProgramCategoryList").Elements("ProgramCategory").Count() > 0)
                        {
                            docTVAgent.Root.Element("TV").Element("ProgramCategoryList").SetAttributeValue("IsAllowAll", "false");
                        }
                        else
                        {
                            docTVAgent.Root.Element("TV").Element("ProgramCategoryList").SetAttributeValue("IsAllowAll", "true");
                        }
                        foreach (var Class in docTVAgent.Root.Element("TV").Element("ProgramCategoryList").Elements("ProgramCategory"))
                        {
                            Class.Name = "IQ_Class";
                        }
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("ProgramCategoryList").SetAttributeValue("IsAllowAll", "true");
                    }
                    docTVAgent.Root.Element("TV").Element("ProgramCategoryList").Name = "IQ_Class_Set";
                }
                else
                {
                    XElement xClass = new XElement("IQ_Class_Set");
                    xClass.SetAttributeValue("IsAllowAll", "true");
                    docTVAgent.Root.Element("TV").Add(xClass);
                }

                if (docTVAgent.Root.Element("TV").Element("RegionList") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("RegionList").Elements("Region") != null)
                    {
                        if (docTVAgent.Root.Element("TV").Element("RegionList").Elements("Region").Count() > 0)
                        {
                            docTVAgent.Root.Element("TV").Element("RegionList").SetAttributeValue("IsAllowAll", "false");
                        }
                        else
                        {
                            docTVAgent.Root.Element("TV").Element("RegionList").SetAttributeValue("IsAllowAll", "true");
                        }
                        foreach (var region in docTVAgent.Root.Element("TV").Element("RegionList").Elements("Region"))
                        {
                            region.Name = "IQ_Region";
                        }
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("RegionList").SetAttributeValue("IsAllowAll", "true");
                    }
                    docTVAgent.Root.Element("TV").Element("RegionList").Name = "IQ_Region_Set";
                }
                else
                {
                    XElement xRegion = new XElement("IQ_Region_Set");
                    xRegion.SetAttributeValue("IsAllowAll", "true");
                    docTVAgent.Root.Element("TV").Add(xRegion);
                }

                if (docTVAgent.Root.Element("TV").Element("CountryList") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("CountryList").Elements("Country") != null)
                    {
                        if (docTVAgent.Root.Element("TV").Element("CountryList").Elements("Country").Count() > 0)
                        {
                            docTVAgent.Root.Element("TV").Element("CountryList").SetAttributeValue("IsAllowAll", "false");
                        }
                        else
                        {
                            docTVAgent.Root.Element("TV").Element("CountryList").SetAttributeValue("IsAllowAll", "true");
                        }
                        foreach (var Country in docTVAgent.Root.Element("TV").Element("CountryList").Elements("Country"))
                        {
                            Country.Name = "IQ_Country";
                        }
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("CountryList").SetAttributeValue("IsAllowAll", "true");
                    }
                    docTVAgent.Root.Element("TV").Element("CountryList").Name = "IQ_Country_Set";
                }
                else
                {
                    XElement xCountry = new XElement("IQ_Country_Set");
                    xCountry.SetAttributeValue("IsAllowAll", "true");
                    docTVAgent.Root.Element("TV").Add(xCountry);
                }

                if (docTVAgent.Root.Element("TV").Element("StationList") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("StationList").Elements("Station") != null)
                    {
                        if (docTVAgent.Root.Element("TV").Element("StationList").Elements("Station").Count() > 0)
                        {
                            docTVAgent.Root.Element("TV").Element("StationList").SetAttributeValue("IsAllowAll", "false");
                        }
                        else
                        {
                            docTVAgent.Root.Element("TV").Element("StationList").SetAttributeValue("IsAllowAll", "true");
                        }
                        foreach (XElement node in docTVAgent.Root.Element("TV").Element("StationList").Elements("Station").ToList())
                        {
                            var childNode = node.Elements().FirstOrDefault();
                            if (childNode != null)
                            {
                                childNode.Name = "IQ_Station_ID";
                                node.AddBeforeSelf(childNode);
                                node.Remove();
                            }
                        }
                        docTVAgent.Root.Element("TV").Element("StationList").Name = "IQ_Station_Set";
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("StationList").SetAttributeValue("IsAllowAll", "true");
                    }
                }
                else
                {
                    XElement xStation = new XElement("IQ_Station_Set");
                    xStation.SetAttributeValue("IsAllowAll", "true");
                    docTVAgent.Root.Element("TV").Add(xStation);
                }

                if (docTVAgent.Root.Element("TV").Element("SearchTerm") != null)
                {
                    docTVAgent.Root.Element("TV").Element("SearchTerm").SetAttributeValue("IsUserMaster", "false");
                }
                else
                {
                    docTVAgent.Root.Element("TV").Element("SearchTerm").SetAttributeValue("IsUserMaster", "True");
                }

                if (docTVAgent.Root.Element("AgentName") != null)
                {
                    docTVAgent.Root.Element("AgentName").Remove();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return docTVAgent.ToString();
        }

        public string ChangeXmlNodeName(string searchRequest)
        {
            XDocument docTVAgent = XDocument.Parse(searchRequest);
            try
            {
                if (docTVAgent.Root.Element("TV").Element("IQ_Dma_Set") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("IQ_Dma_Set").Elements("Dma") != null)
                    {
                        foreach (var Dma in docTVAgent.Root.Element("TV").Element("IQ_Dma_Set").Elements("IQ_Dma"))
                        {
                            Dma.Name = "Dma";
                        }
                        docTVAgent.Root.Element("TV").Element("IQ_Dma_Set").SetAttributeValue("IsAllowAll", "false");
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("IQ_Dma_Set").SetAttributeValue("IsAllowAll", "true");
                    }
                    docTVAgent.Root.Element("TV").Element("IQ_Dma_Set").Name = "DmaList";
                }

                if (docTVAgent.Root.Element("TV").Element("Station_Affiliate_Set") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("Station_Affiliate_Set").Elements("Station_Affil") != null)
                    {
                        foreach (var Affiliate in docTVAgent.Root.Element("TV").Element("Station_Affiliate_Set").Elements("Station_Affil"))
                        {
                            Affiliate.Name = "Affiliate";
                        }
                        docTVAgent.Root.Element("TV").Element("Station_Affiliate_Set").SetAttributeValue("IsAllowAll", "false");
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("Station_Affiliate_Set").SetAttributeValue("IsAllowAll", "true");
                    }
                    docTVAgent.Root.Element("TV").Element("Station_Affiliate_Set").Name = "AffiliateList";
                }

                if (docTVAgent.Root.Element("TV").Element("IQ_Class_Set") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("IQ_Class_Set").Elements("IQ_Class") != null)
                    {
                        foreach (var Class in docTVAgent.Root.Element("TV").Element("IQ_Class_Set").Elements("IQ_Class"))
                        {
                            Class.Name = "ProgramCategory";
                        }
                        docTVAgent.Root.Element("TV").Element("IQ_Class_Set").SetAttributeValue("IsAllowAll", "false");
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("IQ_Class_Set").SetAttributeValue("IsAllowAll", "true");
                    }
                    docTVAgent.Root.Element("TV").Element("IQ_Class_Set").Name = "ProgramCategoryList";
                }

                if (docTVAgent.Root.Element("TV").Element("IQ_Region_Set") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("IQ_Region_Set").Elements("IQ_Region") != null)
                    {
                        foreach (var region in docTVAgent.Root.Element("TV").Element("IQ_Region_Set").Elements("IQ_Region"))
                        {
                            region.Name = "Region";
                        }
                        docTVAgent.Root.Element("TV").Element("IQ_Region_Set").SetAttributeValue("IsAllowAll", "false");
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("IQ_Region_Set").SetAttributeValue("IsAllowAll", "true");
                    }
                    docTVAgent.Root.Element("TV").Element("IQ_Region_Set").Name = "RegionList";
                }

                if (docTVAgent.Root.Element("TV").Element("IQ_Country_Set") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("IQ_Country_Set").Elements("IQ_Country") != null)
                    {
                        foreach (var Country in docTVAgent.Root.Element("TV").Element("IQ_Country_Set").Elements("IQ_Country"))
                        {
                            Country.Name = "Country";
                        }
                        docTVAgent.Root.Element("TV").Element("IQ_Country_Set").SetAttributeValue("IsAllowAll", "false");
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("IQ_Country_Set").SetAttributeValue("IsAllowAll", "true");
                    }
                    docTVAgent.Root.Element("TV").Element("IQ_Country_Set").Name = "CountryList";
                }

                if (docTVAgent.Root.Element("TV").Element("IQ_Station_Set") != null)
                {
                    if (docTVAgent.Root.Element("TV").Element("IQ_Station_Set").Elements("IQ_Station_ID") != null)
                    {
                        foreach (XElement node in docTVAgent.Root.Element("TV").Element("IQ_Station_Set").Elements("IQ_Station_ID").ToList())
                        {
                            XElement station = new XElement(node);
                            station.Name = "Station";
                            station.Value = "";
                            node.Name = "name";
                            station.Add(node);
                            docTVAgent.Root.Element("TV").Element("IQ_Station_Set").Add(station);
                            node.Remove();
                        }
                        docTVAgent.Root.Element("TV").Element("IQ_Station_Set").SetAttributeValue("IsAllowAll", "false");
                        docTVAgent.Root.Element("TV").Element("IQ_Station_Set").Name = "StationList";
                    }
                    else
                    {
                        docTVAgent.Root.Element("TV").Element("IQ_Station_Set").SetAttributeValue("IsAllowAll", "true");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return docTVAgent.ToString();
        }        

        public AgentResultsOutput GetAgentResults(AgentResultsInput p_agentResultsInput, Guid p_ClientGUID, Guid p_CustomerGUID, Dictionary<string, bool> p_Roles, string p_USeqIDLog)
        {
            var agentResultsOutput = new AgentResultsOutput();

            Int64 MaxID = 0;

            Log4NetLogger.Debug(p_USeqIDLog + " - p_ClientGUID: " + p_ClientGUID + ", p_CustomerGuid: " + p_CustomerGUID + ", SRID: " + p_agentResultsInput.SRID + ", SeqID: " + p_agentResultsInput.SeqID + ", Rows: " + p_agentResultsInput.Rows);

            //tvIQAgentResultsOutput.TVResultList = Context.GetIQAgentTVResultsBySearchRequestID(p_ClientGUID, p_CustomerGuid, p_TVIQAgentResultsInput.SRID, p_TVIQAgentResultsInput.SeqID, p_TVIQAgentResultsInput.Rows, out TotalResults, out MaxID).ToList();

            Uri feedsSearchRequestUrl = new Uri(SolrEngineLogic.GeneratePMGUrl(SolrEngineLogic.PMGUrlType.FE.ToString(), null, null));

            FeedsSearch.SearchEngine se = new FeedsSearch.SearchEngine(feedsSearchRequestUrl);

            FeedsSearch.SearchRequest feedsSR = new FeedsSearch.SearchRequest();

            if (p_Roles[CommonConstants.Roles.v4Feeds.ToString()])
            {
                Dictionary<string, List<int>> pendingDeleteResults = Context.GetPendingDeleteAgentRequestsNResults(p_ClientGUID);

                if (pendingDeleteResults.ContainsKey("MediaResult") && pendingDeleteResults["MediaResult"].Count > 0)
                {
                    feedsSR.ExcludeIDs = pendingDeleteResults["MediaResult"].Select(mr => mr.ToString()).ToList();
                }

                if (pendingDeleteResults.ContainsKey("SearchRequest") && pendingDeleteResults["SearchRequest"].Count > 0)
                {
                    feedsSR.ExcludeSearchRequestIDs = pendingDeleteResults["SearchRequest"].Select(mr => mr.ToString()).ToList();
                }
            }

            if (p_agentResultsInput.SRID.HasValue)
            {
                feedsSR.SearchRequestIDs = new List<string> { p_agentResultsInput.SRID.Value.ToString() };
            }

            //if (!string.IsNullOrEmpty(p_agentResultsInput.SubMediaTypeEnum) && p_Roles[Enum.Parse(typeof(CommonConstants.SubMediaType), p_agentResultsInput.SubMediaTypeEnum).ToString()])
            if (!string.IsNullOrEmpty(p_agentResultsInput.SubMediaTypeEnum) && p_Roles[CommonConstants.SubMediaCategoryRoles[((CommonConstants.SubMediaCategory)Enum.Parse(typeof(CommonConstants.SubMediaCategory), p_agentResultsInput.SubMediaTypeEnum))].ToString()])
            {
                feedsSR.MediaCategories = new List<string>() { p_agentResultsInput.SubMediaTypeEnum };
            }
            else
            {
                feedsSR.ExcludeMediaCategories = new List<string>((CommonConstants.SubMediaCategoryRoles.Keys.Where(r => !p_Roles.Keys.Contains(CommonConstants.SubMediaCategoryRoles[((CommonConstants.SubMediaCategory)Enum.Parse(typeof(CommonConstants.SubMediaCategory), r.ToString()))].ToString()) || p_Roles[CommonConstants.SubMediaCategoryRoles[((CommonConstants.SubMediaCategory)Enum.Parse(typeof(CommonConstants.SubMediaCategory), r.ToString()))].ToString()] == false)).Select(s => s.ToString()));
            }

            feedsSR.IsInitialSearch = false;
            feedsSR.IsOnlyParents = false;
            feedsSR.IsFaceting = false;
            feedsSR.SortType = FeedsSearch.SortType.IQSEQID;
            feedsSR.IsSortAsc = false;
            feedsSR.PageSize = 1;
            feedsSR.ClientGUID = p_ClientGUID;

            var feedsMaxResult = se.Search(feedsSR);

            if (feedsMaxResult["Results"].TotalHitCount > 0)
            {
                Log4NetLogger.Debug(p_USeqIDLog + " - TotalResults: " + feedsMaxResult["Results"].TotalHitCount + " - MaxID :" + MaxID);

                agentResultsOutput.TotalResults = feedsMaxResult["Results"].TotalHitCount;

                MaxID = feedsMaxResult["Results"].Hits.FirstOrDefault().ID;

                feedsSR.PageSize = p_agentResultsInput.Rows.Value;
                feedsSR.IsSortAsc = true;

                if (p_agentResultsInput.SeqID.HasValue)
                {
                    if (p_agentResultsInput.SeqID > 0)
                    {
                        p_agentResultsInput.SeqID = p_agentResultsInput.SeqID + 1;
                    }
                }
                else
                {
                    p_agentResultsInput.SeqID = 0;
                }

                feedsSR.SinceIDAsc = p_agentResultsInput.SeqID;

                var feedsResult = se.Search(feedsSR)["Results"];

                AuthenticationLogic authenticationLogic = (AuthenticationLogic)LogicFactory.GetLogic(LogicType.Authentication);

                agentResultsOutput.AgentResultList = PopulateAgentResultsFromFeedsResults(feedsResult, p_Roles[CommonConstants.Roles.NielsenData.ToString()], p_Roles[CommonConstants.Roles.CompeteData.ToString()]);                
            }

            if (agentResultsOutput.AgentResultList != null && agentResultsOutput.AgentResultList.Count > 0 && agentResultsOutput.AgentResultList.Max(a => a.ID) < MaxID)
            {
                agentResultsOutput.HasNextPage = true;
            }
            else
            {
                agentResultsOutput.HasNextPage = false;
            }            

            return agentResultsOutput;
        }

        private List<AgentMedia> PopulateAgentResultsFromFeedsResults(FeedsSearch.SearchResult feedsResult, bool isNilsenAccess, bool isCompeteAccess)
        {
            var agentMediaList = new List<AgentMedia>();

            foreach (var fr in feedsResult.Hits)
            {
                var subMediaType = (CommonConstants.SubMediaCategory)Enum.Parse(typeof(CommonConstants.SubMediaCategory), fr.MediaCategory);

                switch (subMediaType)
                {
                    case CommonConstants.SubMediaCategory.TV:

                        var tvResult = TVLogic.GetTVFromFeedsResult(fr, isNilsenAccess);
                        agentMediaList.Add(tvResult);

                        break;
                    case CommonConstants.SubMediaCategory.NM:

                        var nmResult = NMLogic.GetNMFromFeedsResult(fr, isCompeteAccess);
                        agentMediaList.Add(nmResult);

                        break;
                    case CommonConstants.SubMediaCategory.Blog:

                        var blogResult = SMLogic.GetSMFromFeedsResult(fr, isCompeteAccess);
                        agentMediaList.Add(blogResult);

                        break;
                    case CommonConstants.SubMediaCategory.Forum:

                        var forumResult = SMLogic.GetSMFromFeedsResult(fr, false);
                        agentMediaList.Add(forumResult);

                        break;
                    case CommonConstants.SubMediaCategory.SocialMedia:

                        var smResult = SMLogic.GetSMFromFeedsResult(fr, false);
                        agentMediaList.Add(smResult);

                        break;
                    case CommonConstants.SubMediaCategory.FB:

                        var fbResult = SMLogic.GetSMFromFeedsResult(fr, false);
                        agentMediaList.Add(fbResult);

                        break;
                    case CommonConstants.SubMediaCategory.IG:

                        var igResult = SMLogic.GetSMFromFeedsResult(fr, false);
                        agentMediaList.Add(igResult);

                        break;
                    case CommonConstants.SubMediaCategory.TW:

                        var twResult = TWLogic.GetTWFromFeedsResult(fr);
                        agentMediaList.Add(twResult);

                        break;
                    case CommonConstants.SubMediaCategory.Radio:

                        var tmResult = TMLogic.GetTMFromFeedsResult(fr);
                        agentMediaList.Add(tmResult);

                        break;
                    case CommonConstants.SubMediaCategory.PM:

                        var pmResult = PMLogic.GetPMFromFeedsResult(fr);
                        agentMediaList.Add(pmResult);

                        break;
                    case CommonConstants.SubMediaCategory.PQ:

                        var pqResult = PQLogic.GetPQFromFeedsResult(fr);
                        agentMediaList.Add(pqResult);

                        break;
                }
            }

            return agentMediaList;
        }

        private static string GetCCHighLights(string p_HighLights)
        {
            try
            {
                string highLights = string.Empty;
                XDocument xDoc = XDocument.Parse(p_HighLights);

                if (xDoc.Root.Element("CC") != null)
                {
                    highLights = Convert.ToString(xDoc.Root.Element("CC"));
                }

                return highLights;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
