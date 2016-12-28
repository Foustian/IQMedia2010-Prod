using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Net;
using System.Configuration;
using PMGSearch;
using System.Xml;
using System.Data.Objects;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using IQMediaGroup.Common;
using IQMediaGroup.Common.Util;
using System.Xml.Linq;

namespace IQMediaGroup.ExposeApi.Logic
{
    public class RawMediaLogic : BaseLogic, ILogic
    {
        public enum SortFieldType
        {
            PMGSearch,
            SSPSearch
        }

        string IframeURL = "http://qa.iqmediacorp.com/IFrameServiceRawMedia/Default.aspx";

        public RawMediaOutput GetPMGRawMedia(RawMediaInput RawMediaInput, string p_USeqID = null)
        {
            try
            {
                Log4NetLogger.Debug(p_USeqID + " - Start GetRawMedia-PMG");

                Log4NetLogger.Debug(p_USeqID + " - Start PMG Request");

                Uri PMGSearchRequestUrl = new Uri(SolrEngineLogic.GeneratePMGUrl(SolrEngineLogic.PMGUrlType.TV.ToString(), RawMediaInput.FromDateTime, RawMediaInput.ToDateTime));

                int _PMGMaxHighlights = 20;

                if (ConfigurationManager.AppSettings["IframeURL"] != null)
                {
                    IframeURL = ConfigurationManager.AppSettings["IframeURL"];
                }

                if (ConfigurationManager.AppSettings["PMGMaxHighlights"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["PMGMaxHighlights"], out _PMGMaxHighlights);
                }

                string SortFieldsConverted = GenerateSortField(RawMediaInput, SortFieldType.PMGSearch);

                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchRequest _SearchRequest = new SearchRequest();

                _SearchRequest.Title120 = !string.IsNullOrEmpty(RawMediaInput.ProgramTitle) ? RawMediaInput.ProgramTitle.Trim() : null;
                _SearchRequest.Terms = !string.IsNullOrEmpty(RawMediaInput.SearchTerm) ? RawMediaInput.SearchTerm.Trim() : null;

                if (!string.IsNullOrWhiteSpace(RawMediaInput.SearchTerm))
                {
                    _SearchRequest.IsShowCC = true;
                }

                _SearchRequest.TimeZone = RawMediaInput.TimeZone.ToLower();

                _SearchRequest.PageNumber = RawMediaInput.PageNumber.Value - 1;
                _SearchRequest.PageSize = RawMediaInput.PageSize.Value;
                _SearchRequest.StartDate = Convert.ToDateTime(RawMediaInput.FromDateTime);
                _SearchRequest.EndDate = Convert.ToDateTime(RawMediaInput.ToDateTime);

                StatskedprogLogic StatskedprogLogic = (StatskedprogLogic)LogicFactory.GetLogic(LogicType.StatskedprogData);                

                StatskedprogData StatskedprogData = StatskedprogLogic.GetStatskedprogData(RawMediaInput.ClientGUID);                

                if (!StatskedprogData.IsAllDma)
                {
                    if (RawMediaInput.DmaList != null && RawMediaInput.DmaList.Count > 0)
                    {
                        IQ_DmaEqualityComparerByName iqdmaEqualityComparerNM = new IQ_DmaEqualityComparerByName();
                        _SearchRequest.IQDmaName = RawMediaInput.DmaList.Intersect(StatskedprogData.DmaList, iqdmaEqualityComparerNM).Select(iqdma => iqdma.Name).ToList();
                        if (_SearchRequest.IQDmaName.Count == 0)
                        {
                            _SearchRequest.IQDmaName = StatskedprogData.DmaList.Select(iqdma => iqdma.Name).ToList();
                        }
                    }
                    else
                    {
                        _SearchRequest.IQDmaName = StatskedprogData.DmaList.Select(iqdma => iqdma.Name).ToList();
                    }
                }
                else if (RawMediaInput.DmaList != null && RawMediaInput.DmaList.Count > 0)
                {
                    _SearchRequest.IQDmaName = RawMediaInput.DmaList.Select(iqdma => iqdma.Name).ToList();
                }


                if (!StatskedprogData.IsAllAffiliate)
                {
                    if (RawMediaInput.AffiliateList != null && RawMediaInput.AffiliateList.Count > 0)
                    {
                        Station_AffilEqualityComparerByName stationaffilEqualityComparerNM = new Station_AffilEqualityComparerByName();
                        _SearchRequest.StationAffil = RawMediaInput.AffiliateList.Intersect(StatskedprogData.AffiliateList, stationaffilEqualityComparerNM).Select(stationaffil => stationaffil.Name).ToList();
                        if (_SearchRequest.StationAffil.Count == 0)
                        {
                            _SearchRequest.StationAffil = StatskedprogData.AffiliateList.Select(stationaffil => stationaffil.Name).ToList();
                        }
                    }
                    else
                    {
                        _SearchRequest.StationAffil = StatskedprogData.AffiliateList.Select(stationaffil => stationaffil.Name).ToList();
                    }
                }
                else if (RawMediaInput.AffiliateList != null && RawMediaInput.AffiliateList.Count > 0)
                {
                    _SearchRequest.StationAffil = RawMediaInput.AffiliateList.Select(stationaffil => stationaffil.Name).ToList();
                }

                
                _SearchRequest.MaxHighlights = _PMGMaxHighlights;
                _SearchRequest.SortFields = SortFieldsConverted;

                bool IsPmgLogging = true;
                if (ConfigurationManager.AppSettings["IsPMGLogging"] != null)
                {
                    bool.TryParse(ConfigurationManager.AppSettings["IsPMGLogging"], out IsPmgLogging);
                }
                _SearchRequest.IsPmgLogging = IsPmgLogging;
                _SearchRequest.PmgLogFileLocation = ConfigurationManager.AppSettings["PMGLogFileLocation"];

                SearchResult _SearchResult = _SearchEngine.Search(_SearchRequest);

                string _Responce = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();
                _XmlDocument.LoadXml(_SearchResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Responce = _XmlDocument.InnerXml;
                            //_Responce = _Responce.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                            _Responce = _Responce.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Responce = null;
                }
                string _SearchType = "Service";

                PMGSearchLogLogic PMGSearchLogLogicObj = (PMGSearchLogLogic)LogicFactory.GetLogic(LogicType.PMGSearchLog);

                PMGSearchLogLogicObj.InsertPMGSearchLog(_SearchRequest.Terms, _SearchRequest.PageNumber, _SearchRequest.PageSize, _SearchRequest.MaxHighlights,
                                                        _SearchRequest.StartDate, _SearchRequest.EndDate, _Responce,
                                                        _SearchRequest.Title120, _SearchRequest.Appearing, _SearchRequest.TimeZone, RawMediaInput.CustomerID, _SearchRequest.IQDmaNum, _SearchRequest.IQClassNum, _SearchRequest.StationAffil, _SearchType);

                RawMediaOutput RawMediaOutput = new Domain.RawMediaOutput();

                bool _IsPmgSearchTotalHitsFromConfig = true;
                int _MaxPMGHitsCount = 100;

                if (ConfigurationManager.AppSettings["PMGSearchTotalHitsFromConfig"] != null)
                {
                    bool.TryParse(ConfigurationManager.AppSettings["PMGSearchTotalHitsFromConfig"], out _IsPmgSearchTotalHitsFromConfig);
                }

                if (_IsPmgSearchTotalHitsFromConfig == true)
                {
                    if (ConfigurationManager.AppSettings["PMGMaxListCount"] != null)
                    {
                        int.TryParse(ConfigurationManager.AppSettings["PMGMaxListCount"], out _MaxPMGHitsCount);
                    }

                    _SearchResult.TotalHitCount = _MaxPMGHitsCount;
                }

                string SearchTermQS = string.Empty;

                SearchTermQS = !string.IsNullOrEmpty(RawMediaInput.SearchTerm) ? "&SearchTerm=" + RawMediaInput.SearchTerm.Trim() : string.Empty;

                var RawMediaList = (from _Hit in _SearchResult.Hits
                                    select new RawMedia
                                    {
                                        RawMediaID = new Guid(_Hit.Guid),
                                        Hits = _Hit.TotalNoOfOccurrence,
                                        DmaName = _Hit.Market,
                                        DateTime = _Hit.GmtDateTime,
                                        StationLogo = ConfigurationManager.AppSettings["StationLogoURL"] + _Hit.StationId + ".jpg",
                                        ProgramTitle = _Hit.Title120,
                                        URL = IframeURL + "?RawMediaID=" + _Hit.Guid + SearchTermQS,
                                        Station=StatskedprogData.StationList.Where(s=>s.name==_Hit.StationId).FirstOrDefault().StationCallSign,
                                        Highlights=(_Hit.TermOccurrences!=null && _Hit.TermOccurrences.Count()>0)?(new XDocument( new XElement("CC", from highlight in _Hit.TermOccurrences
                                                        select new XElement ("ClosedCaption", new XElement("Text",highlight.SurroundingText),new XElement("Offset",highlight.TimeOffset))))).ToString():""
           
                                    });

                List<RawMedia> _ListOfRawMedia = new List<RawMedia>(RawMediaList);

                RawMediaOutput.RawMediaList = _ListOfRawMedia;


                /* Start Flag for NextPage is available or not */
                RawMediaOutput.TotalResults = _SearchResult.TotalHitCount;

                if ((_SearchResult.TotalHitCount - ((RawMediaInput.PageNumber.Value - 1) * RawMediaInput.PageSize.Value)) > RawMediaInput.PageSize.Value)
                {
                    RawMediaOutput.HasNextPage = true;
                }

                /* Stop Flag for NextPage is available or not */

                Log4NetLogger.Debug(p_USeqID + " - End PMG Request");


                RawMediaOutput.PageNumber = RawMediaInput.PageNumber.Value;

                Log4NetLogger.Debug(p_USeqID + " - End GetRawMedia-PMG");

                return RawMediaOutput;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string GenerateSortField(RawMediaInput RawMediaInput, SortFieldType sortFieldType, bool IsRequiredGUIDSort = false)
        {
            try
            {
                IDictionary<string, string> PMGSearchSortFields = new Dictionary<string, string>();
                IDictionary<string, string> SSPSearchSortFields = new Dictionary<string, string>();

                PMGSearchSortFields.Add("datetime", "datetime");
                PMGSearchSortFields.Add("datetime-", "datetime-");
                PMGSearchSortFields.Add("guid", "guid");
                PMGSearchSortFields.Add("guid-", "guid-");
                PMGSearchSortFields.Add("station", "station");
                PMGSearchSortFields.Add("station-", "station-");
                PMGSearchSortFields.Add("market", "market");
                PMGSearchSortFields.Add("market-", "market-");
                //PMGSearchSortFields.Add("title120", "title120");
                //PMGSearchSortFields.Add("title120-", "title120-");                

                SSPSearchSortFields.Add("datetime", "IQ_Local_Air_Date");
                SSPSearchSortFields.Add("datetime-", "IQ_Local_Air_Date-");

                if (IsRequiredGUIDSort)
                {
                    SSPSearchSortFields.Add("guid", "RL_GUID");
                    SSPSearchSortFields.Add("guid-", "RL_GUID-");
                }

                SSPSearchSortFields.Add("station", "Station_ID");
                SSPSearchSortFields.Add("station-", "Station_ID-");
                SSPSearchSortFields.Add("market", "IQ_Dma_Name");
                SSPSearchSortFields.Add("market-", "IQ_Dma_Name-");
                SSPSearchSortFields.Add("title120", "title120");
                SSPSearchSortFields.Add("title120-", "title120-");

                StringBuilder InputSortFields = new StringBuilder();

                int MaxNoofSortFields = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxSortField"]) ? Convert.ToInt32(ConfigurationManager.AppSettings["MaxSortField"]) : 3;
                int Index = 0;

                switch (sortFieldType)
                {
                    case SortFieldType.PMGSearch:
                        string[] PMGSearchSortField = RawMediaInput.SortField.Split(new char[] { ',' });

                        foreach (string SortField in PMGSearchSortField)
                        {
                            if (PMGSearchSortFields.ContainsKey(SortField.ToLower()))
                            {
                                InputSortFields.Append(PMGSearchSortFields[SortField.ToLower()] + ",");

                                Index = Index + 1;

                                if (Index >= MaxNoofSortFields)
                                {
                                    break;
                                }
                            }
                        }

                        break;
                    case SortFieldType.SSPSearch:
                        string[] SSPSearchSortField = RawMediaInput.SortField.Split(new char[] { ',' });

                        foreach (string SortField in SSPSearchSortField)
                        {
                            if (SSPSearchSortFields.ContainsKey(SortField.ToLower()))
                            {
                                InputSortFields.Append(SSPSearchSortFields[SortField.ToLower()] + ",");

                                Index = Index + 1;

                                if (Index >= MaxNoofSortFields)
                                {
                                    break;
                                }
                            }
                        }

                        break;
                }

                if (InputSortFields.Length > 0)
                {
                    InputSortFields.Remove(InputSortFields.Length - 1, 1);
                }

                return InputSortFields.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public Guid? GetRecordFileGUIDByStationIDANDDatetime(string stationID, DateTime dtTime,bool isGMTDateTime)
        {
            try
            {
                Guid? recordFileGUID = Context.GetRecordFileGUIDByStatioIDANDDatetime(dtTime, stationID, dtTime.IsDaylightSavingTime(),isGMTDateTime).FirstOrDefault();
                return recordFileGUID;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public Int64? IQAgentMediaResultsVerifyIDByClientGUID(Int64 p_SeqID, Guid p_ClientGUID)
        {
            try
            {
                Int64? SeqID = Context.IQAgentMediaResultsVerifyIDByClientGUID(p_SeqID, p_ClientGUID).FirstOrDefault();
                return SeqID;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


    }
}
