using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;
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
using IQMedia.Web.Common;

namespace IQMediaGroup.Logic
{
    public class RawMediaLogic_WithPage : BaseLogic, ILogic
    {
        private enum SearchCrieteria
        {
            IQ_Dma = 1,
            IQ_Cat = 2,
            IQ_Class = 3,
            Station_Affil = 4
        }

        public enum SortFieldType
        {
            PMGSearch,
            SSPSearch
        }

        string IframeURL = "http://qa.iqmediacorp.com/IFrameRawMedia/Default.aspx";

        public RawMediaOutput GetRawMedia(RawMediaInput RawMediaInput)
        {
            try
            {
                RawMediaOutput RawMediaOutput = null;

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["IframeURL"]))
                {
                    IframeURL = ConfigurationManager.AppSettings["IframeURL"];
                }

                if (string.IsNullOrEmpty(RawMediaInput.SearchTerm))
                {
                    /* Default Search */

                    RawMediaOutput = GetRawMediaDefaultSearch(RawMediaInput);
                }
                else
                {
                    StatskedprogLogic StatskedprogLogic = (StatskedprogLogic)LogicFactory.GetLogic(LogicType.StatskedprogData);

                    StatskedprogData StatskedprogData = StatskedprogLogic.GetStatskedprogData();

                    if (
                        (string.IsNullOrEmpty(RawMediaInput.Title120)) &&
                        (string.IsNullOrEmpty(RawMediaInput.Desc100)) &&
                        RawMediaInput.IQ_Time_Zone.ToLower().Trim() == "all" &&
                        Comparer(RawMediaInput.IQ_Cat_Set,
                                 StatskedprogData.IQ_Cat_Set,
                                 RawMediaInput.IQ_Class_Set,
                                 StatskedprogData.IQ_Class_Set,
                                 RawMediaInput.IQ_Dma_Set,
                                 StatskedprogData.IQ_Dma_Set,
                                 RawMediaInput.Station_Affil_Set,
                                 StatskedprogData.Station_Affil_Set
                                )
                        )
                    {
                        /* PMG Search Followed By SSP Search*/

                        RawMediaOutput = GetRawMediaPMGFBySSP(RawMediaInput);

                    }
                    else
                    {
                        /* SSP Search Followed By PMG Search */

                        RawMediaOutput = GetRawMediaSSPFByPMG(RawMediaInput);
                    }
                }

                return RawMediaOutput;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string GenerateSortField(RawMediaInput RawMediaInput, SortFieldType sortFieldType)
        {
            try
            {
                IDictionary<string, string> PMGSearchSortFields = new Dictionary<string, string>();
                IDictionary<string, string> SSPSearchSortFields = new Dictionary<string, string>();

                PMGSearchSortFields.Add("datetime", "date,hour");
                PMGSearchSortFields.Add("datetime-", "date-,hour-");
                PMGSearchSortFields.Add("guid", "guid");
                PMGSearchSortFields.Add("guid-", "guid-");
                PMGSearchSortFields.Add("station", "station");
                PMGSearchSortFields.Add("station-", "station-");
                PMGSearchSortFields.Add("market", "market");
                PMGSearchSortFields.Add("market-", "market-");

                SSPSearchSortFields.Add("datetime", "IQ_Local_Air_Date");
                SSPSearchSortFields.Add("datetime-", "IQ_Local_Air_Date-");
                SSPSearchSortFields.Add("guid", "RL_GUID");
                SSPSearchSortFields.Add("guid-", "RL_GUID-");
                SSPSearchSortFields.Add("station", "Station_ID");
                SSPSearchSortFields.Add("station-", "Station_ID-");
                SSPSearchSortFields.Add("market", "IQ_Dma_Name");
                SSPSearchSortFields.Add("market-", "IQ_Dma_Name-");

                StringBuilder InputSortFields = new StringBuilder();

                switch (sortFieldType)
                {
                    case SortFieldType.PMGSearch:
                        string[] PMGSearchSortField = RawMediaInput.SortField.Split(new char[] { ',' });

                        foreach (string SortField in PMGSearchSortField)
                        {
                            if (PMGSearchSortFields.ContainsKey(SortField.ToLower()))
                            {
                                InputSortFields.Append(PMGSearchSortFields[SortField.ToLower()] + ",");
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

        private RawMediaOutput GetRawMediaSSPFByPMG(RawMediaInput RawMediaInput)
        {
            try
            {
                Logger.LogInfo("Start GetRawMedia-SSP-to-PMG");

                Logger.LogInfo("Start Initialization");

                # region Variable Initialization

                ObjectParameter TotalRecordsCount = new ObjectParameter("TotalRecordsCount", typeof(Int32));

                RawMediaOutput RawMediaOutput = new RawMediaOutput();
                RawMediaOutput.RawMedia = new List<RawMedia>();

                RawMediaOutput RawMediaOutputPMG = null;

                bool IsExistingPage = false;

                string SearchCriteriaTxt = string.Empty;
                string SessionID = IQMedia.Web.Common.Authentication.IsAuthenticated ? HttpContext.Current.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName].Value : RawMediaInput.SessionID;

                string SortFieldsConvertedSSP = GenerateSortField(RawMediaInput, SortFieldType.SSPSearch);
                string SortFieldsConvertedPMG = GenerateSortField(RawMediaInput, SortFieldType.PMGSearch);

                bool IsFirst = true;

                # region Paging Variables

                int DBPageIndex = 1;
                int PMGPageIndex = 1;

                int MaxDBPage = 1;

                int DBPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfResultsFromDB"]);
                int PMGPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfGUIDsInRequest"]);

                int MaxPMGPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(DBPageSize / PMGPageSize)));

                int RecordIndex = 0;

                bool HasNextPage = false;

                # endregion Paging Variables

                # endregion Variable Initialization

                if (RawMediaInput.IQ_Time_Zone.Trim().ToLower() == "all")
                {
                    RawMediaInput.IQ_Time_Zone = null;
                }

                string PMGSearchRequestUrl = ConfigurationManager.AppSettings["PMGSearchUrl"].ToString();

                int _PMGMaxHighlights = 20;

                if (ConfigurationManager.AppSettings["PMGMaxHighlights"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["PMGMaxHighlights"], out _PMGMaxHighlights);
                }

                /* Start check for search criteria & validate requested datetime */

                int IsEqual = CheckForSerchCriteriaSame(RawMediaInput);
                AdvancedSearchServiceState AdvancedSearchServiceStateForSamePage=null;

                switch (IsEqual)
                {
                    case 0:

                        RawMediaOutput.IndexStatus = 1;
                        RawMediaOutput.PageNumber = 1;

                        break;
                    case 1:

                        RawMediaOutput.IndexStatus = 0;
                        RawMediaOutput.PageNumber = 1;

                        break;
                    case 2:

                        RawMediaOutput.IndexStatus = 0;

                        AdvancedSearchServiceStateForSamePage = Context.AdvancedSearchServiceStates.Where("it.SessionID='" + SessionID + "' and it.PageNo=" + Convert.ToString(RawMediaInput.PageNumber)).OrderByDescending(_AdvSearchServiceState => _AdvSearchServiceState.AdvancedSearchServiceStateKey).FirstOrDefault();

                        if (AdvancedSearchServiceStateForSamePage != null)
                        {
                            IsExistingPage = true;
                            RawMediaOutput.PageNumber = RawMediaInput.PageNumber.Value;

                            if (AdvancedSearchServiceStateForSamePage.IsSamePMGPage)
                            {
                                PMGPageIndex = AdvancedSearchServiceStateForSamePage.PMGPageNo;
                                RecordIndex = AdvancedSearchServiceStateForSamePage.PMGPageEndIndex + 1;
                                DBPageIndex = AdvancedSearchServiceStateForSamePage.SSPPageNo;
                            }
                            else
                            {
                                if (AdvancedSearchServiceStateForSamePage.IsSamePMGPage == false && AdvancedSearchServiceStateForSamePage.PMGPageNo == MaxPMGPage)
                                {
                                    DBPageIndex = AdvancedSearchServiceStateForSamePage.SSPPageNo + 1;
                                    PMGPageIndex = 1;
                                    RecordIndex = 0;
                                }
                                else
                                {
                                    DBPageIndex = AdvancedSearchServiceStateForSamePage.SSPPageNo;
                                    PMGPageIndex = AdvancedSearchServiceStateForSamePage.PMGPageNo + 1;
                                    RecordIndex = 0;
                                }
                            }
                        }
                        else
                        {
                            /*IsExistingPage = false;*/
                            throw new CustomException("Invalid page number.");
                        }

                        break;
                    default:
                        throw new CustomException("Some Error occurs, please try again.");
                }


                /* Stop check for search criteria & validate requested datetime */

                /* Start generate SSP_Index_Key -------- for pass into SSP table */

                string SSP_Index_Key = GenerateSSPIndexKey(RawMediaInput.IQ_Cat_Set, RawMediaInput.IQ_Class_Set, RawMediaInput.IQ_Dma_Set, RawMediaInput.Station_Affil_Set);

                /* Stop generate SSP_Index_Key */

                /* Start Serialize RawMediainput object ------- to store in AdvancedSearchServiceStaet table */

                DataContractJsonSerializer Serializer = new DataContractJsonSerializer(RawMediaInput.GetType());

                MemoryStream Stream = new MemoryStream();

                Serializer.WriteObject(Stream, RawMediaInput);

                Stream.Position = 0;

                StreamReader StreamReader = new StreamReader(Stream);

                SearchCriteriaTxt = StreamReader.ReadToEnd();

                /* if (IsEqual != 0 && IsExistingPage == false)
                 {
                     AdvancedSearchServiceState AdvancedSearchServiceState = Context.AdvancedSearchServiceStates.Where("it.SessionID='" + SessionID + "' and it.PageNo=" + Convert.ToString(RawMediaInput.PageNumber - 1)).OrderByDescending(TmpAdvServiceState => TmpAdvServiceState.AdvancedSearchServiceStateKey).FirstOrDefault();

                     if (IsEqual == 2 && AdvancedSearchServiceState == null)
                     {                        
                         throw new CustomException("Invalid page number, only consecutive page numbers can be requested.");
                     }
                     else if (IsEqual == 1)
                     {
                         RawMediaOutput.PageNumber = 1;
                     }
                     else if (IsEqual == 2 && AdvancedSearchServiceState != null)
                     {
                         RawMediaOutput.PageNumber = RawMediaInput.PageNumber.Value;

                         if (AdvancedSearchServiceState.IsSamePMGPage)
                         {
                             PMGPageIndex = AdvancedSearchServiceState.PMGPageNo;
                             RecordIndex = AdvancedSearchServiceState.PMGPageEndIndex + 1;
                             DBPageIndex = AdvancedSearchServiceState.SSPPageNo;
                         }
                         else
                         {
                             if (AdvancedSearchServiceState.IsSamePMGPage == false && AdvancedSearchServiceState.PMGPageNo == MaxPMGPage)
                             {
                                 DBPageIndex = AdvancedSearchServiceState.SSPPageNo + 1;
                                 PMGPageIndex = 1;
                                 RecordIndex = 0;
                             }
                             else
                             {
                                 DBPageIndex = AdvancedSearchServiceState.SSPPageNo;
                                 PMGPageIndex = AdvancedSearchServiceState.PMGPageNo + 1;
                                 RecordIndex = 0;
                             }
                         }
                     }
                 }*/

                Logger.LogInfo("End Initialization");

                int TotalPageNo = 1;

                do
                {
                    bool IsLastExistingPage = false;

                    Logger.LogInfo("Start DB Request");

                    List<RawMedia> RawMediaDB = (Context.GetSTATSKEDPROGtByParams(SessionID,
                                                                                    SearchCriteriaTxt,
                                                                                    IsEqual == 2 ? true : false,
                                                                                    IsFirst,
                                                                                    IsExistingPage,
                                                                                    RawMediaInput.PageNumber,
                                                                                    RawMediaInput.IQ_Time_Zone,
                                                                                    SSP_Index_Key,
                                                                                    Convert.ToDateTime(RawMediaInput.FromDate),
                                                                                    Convert.ToDateTime(RawMediaInput.ToDate),
                                                                                    RawMediaInput.Title120,
                                                                                    RawMediaInput.Desc100,
                                                                                    DBPageIndex,
                                                                                    DBPageSize,
                                                                                    SortFieldsConvertedSSP,
                                                                                    TotalRecordsCount
                                                                                  ).ToList().Select(RawMediaObj => new RawMedia()
                                                                                                                       {
                                                                                                                           DateTime = Convert.ToString(RawMediaObj.DateTime),
                                                                                                                           IQ_Dma_Name = RawMediaObj.IQ_Dma_Name,
                                                                                                                           RawMediaID = new Guid(RawMediaObj.RawMediaID),
                                                                                                                           StationLogo = RawMediaObj.StationLogo,
                                                                                                                           Title120 = RawMediaObj.Title120
                                                                                                                       }).ToList<RawMedia>());

                    MaxDBPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(TotalRecordsCount.Value) / Convert.ToDouble(DBPageSize))));
                    MaxPMGPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(RawMediaDB.Count) / Convert.ToDouble(PMGPageSize))));

                    Logger.LogInfo("End DB Request");

                    do
                    {
                        /* string RawMediaGUIDs = string.Join(",", RawMediaDB.Skip(((DBPageIndex - 1) * (DBPageSize)) - 1).Take(PMGPageSize).Select(RawMediaObj => RawMediaObj.RawMediaID).ToArray());*/

                        Logger.LogInfo("Start PMG Request");

                        string RawMediaGUIDs = string.Empty;

                        if (IsExistingPage == false)
                        {
                            /*RawMediaGUIDs = string.Join(",", RawMediaDB.GetRange((PMGPageIndex - 1) * PMGPageSize, PMGPageSize).Select(RawMediaObj => RawMediaObj.RawMediaID).ToArray());*/
                            RawMediaGUIDs = string.Join(",", RawMediaDB.GetRange((PMGPageIndex - 1) * PMGPageSize, PMGPageSize <= RawMediaDB.Count ? PMGPageSize : RawMediaDB.Count).Select(RawMediaObj => RawMediaObj.RawMediaID).ToArray());
                        }
                        else
                        {
                            RawMediaGUIDs = string.Join(",", RawMediaDB.Select(RawMediaObj => RawMediaObj.RawMediaID).ToArray());
                        }

                        if (!string.IsNullOrEmpty(RawMediaGUIDs))
                        {
                            RawMediaOutputPMG = GetRawMediaFromPMG(PMGSearchRequestUrl,
                                                                                           RawMediaInput.SearchTerm,
                                                                                           RawMediaGUIDs,
                                                                                           PMGPageIndex,
                                                                                           PMGPageSize,
                                                                                           _PMGMaxHighlights,
                                                                                          Convert.ToDateTime(RawMediaInput.FromDate),
                                                                                          Convert.ToDateTime(RawMediaInput.ToDate),
                                                                                           SortFieldsConvertedPMG,
                                                                                           SSP_Index_Key,
                                                                                           RawMediaInput.Title120,
                                                                                           RawMediaInput.Desc100,
                                                                                           RawMediaInput.IQ_Time_Zone
                                                                                       );

                            RawMediaOutputPMG.RawMedia.ForEach(delegate(RawMedia _RawMedia)
                            {
                                _RawMedia.URL = IframeURL + "?RawMediaID=" + _RawMedia.RawMediaID + "&SearchTerm=" + RawMediaInput.SearchTerm;

                                RawMedia _RawMediaTmp = RawMediaDB.Find(delegate(RawMedia _RawMediaObj) { return _RawMediaObj.RawMediaID == _RawMedia.RawMediaID; });

                                if (_RawMediaTmp != null)
                                {
                                    _RawMedia.Title120 = _RawMediaTmp.Title120;
                                }

                            });

                            Logger.LogInfo("End PMG Request");

                            /*RawMediaOutput.RawMedia.AddRange(RawMediaOutputPMG.RawMedia.Skip(RecordIndex==0?0:RecordIndex-1));*/

                            Logger.LogInfo("Start Insert RawMedia into Response");

                            if (IsExistingPage == false)
                            {

                                /*if (RawMediaOutput.RawMedia.Count < RawMediaInput.PageSize)
                                {
                                    if ((RawMediaOutputPMG.RawMedia.Count + RawMediaOutput.RawMedia.Count) > RawMediaInput.PageSize)
                                    {*/
                                List<RawMedia> _TmpRawMediaList = new List<RawMedia>();
                                   _TmpRawMediaList.AddRange(RawMediaOutputPMG.RawMedia);

                                   if (RecordIndex>0)
                                   {
                                       _TmpRawMediaList.RemoveRange(0, RecordIndex);
                                   }

                                int TmpPMGIndex = 0;

                                while (_TmpRawMediaList.Count > 0 && TotalPageNo <= Convert.ToInt32(ConfigurationManager.AppSettings["MaxNoOfPagesSaved"]))
                                {
                                    AdvancedSearchServiceState _AdvancedSearchServiceState = new Domain.AdvancedSearchServiceState();

                                    /*TmpPMGIndex = (RawMediaInput.PageSize.Value - RawMediaOutput.RawMedia.Count) >= _TmpRawMediaList.Count ? (_TmpRawMediaList.Count - 1) + (TmpPMGIndex == 0 ? 0 : TmpPMGIndex + 1) : ((RawMediaInput.PageSize.Value - RawMediaOutput.RawMedia.Count) - 1) + (TmpPMGIndex == 0 ? 0 : TmpPMGIndex + 1);*/
                                    TmpPMGIndex = (RawMediaInput.PageSize.Value - RawMediaOutput.RawMedia.Count) >= _TmpRawMediaList.Count ? (_TmpRawMediaList.Count - 1) + RecordIndex : ((RawMediaInput.PageSize.Value - RawMediaOutput.RawMedia.Count) - 1) + RecordIndex;

                                    _AdvancedSearchServiceState.PMGPageEndIndex = TmpPMGIndex;
                                    _AdvancedSearchServiceState.GUIDServed = string.Join(",", RawMediaOutputPMG.RawMedia.GetRange(RecordIndex == 0 ? 0 : RecordIndex, (TmpPMGIndex + 1-RecordIndex)).Select(RawMediaObj => "'" + RawMediaObj.RawMediaID + "'").ToArray());

                                    /*RawMediaOutput.RawMedia.AddRange(_TmpRawMediaList.GetRange(0, (TmpPMGIndex+1 - RecordIndex)));*/
                                    RawMediaOutput.RawMedia.AddRange(_TmpRawMediaList.GetRange(0, (TmpPMGIndex + 1)-RecordIndex));

                                    _AdvancedSearchServiceState.PMGPageNo = PMGPageIndex;
                                    _AdvancedSearchServiceState.SSPPageNo = DBPageIndex;
                                    _AdvancedSearchServiceState.PageNo = (RawMediaOutput.PageNumber+(TotalPageNo-1));
                                    _AdvancedSearchServiceState.RequestedDate = DateTime.Now;
                                    _AdvancedSearchServiceState.SearchCriteria = SearchCriteriaTxt;
                                    _AdvancedSearchServiceState.SessionID = SessionID;

                                    /*_AdvancedSearchServiceState.GUIDServed = string.Join(",", RawMediaOutput.RawMedia.GetRange(0, RawMediaInput.PageSize.Value).Select(RawMediaObj => "'" + RawMediaObj.RawMediaID + "'").ToArray());*/

                                    Context.AdvancedSearchServiceStates.AddObject(_AdvancedSearchServiceState);
                                    Context.SaveChanges();
                                    

                                    _TmpRawMediaList.RemoveRange(0, TmpPMGIndex+1-RecordIndex);
                                    

                                    if (_TmpRawMediaList.Count > 0 || RawMediaOutput.RawMedia.Count==RawMediaInput.PageSize)
                                    {
                                        TotalPageNo = TotalPageNo + 1;

                                        RawMediaOutput.RawMedia.RemoveRange(0, RawMediaOutput.RawMedia.Count);

                                        if (_TmpRawMediaList.Count>0)
                                        {
                                            _AdvancedSearchServiceState.IsSamePMGPage = true;
                                            RecordIndex = TmpPMGIndex + 1; 
                                        }
                                    }                    
                                }

                                /*
                                 _AdvancedSearchServiceState.PMGPageEndIndex = (RawMediaInput.PageSize.Value - RawMediaOutput.RawMedia.Count) - 1;
                                 HasNextPage = true;
                                 _AdvancedSearchServiceState.IsSamePMGPage = true;                                       

                                 _AdvancedSearchServiceState.GUIDServed = string.Join(",", RawMediaOutputPMG.RawMedia.GetRange(RecordIndex == 0 ? 0 : RecordIndex, (RawMediaInput.PageSize.Value - RawMediaOutput.RawMedia.Count)).Select(RawMediaObj => "'" + RawMediaObj.RawMediaID + "'").ToArray());

                                 RawMediaOutput.RawMedia.AddRange(RawMediaOutputPMG.RawMedia.GetRange(RecordIndex == 0 ? 0 : RecordIndex, (RawMediaInput.PageSize.Value - RawMediaOutput.RawMedia.Count)));
                                 */
                                /* }
                                 else
                                 {
                                     _AdvancedSearchServiceState.IsSamePMGPage = false;
                                     _AdvancedSearchServiceState.PMGPageEndIndex = RawMediaOutputPMG.RawMedia.Count - 1;                                       

                                     _AdvancedSearchServiceState.GUIDServed = string.Join(",", RawMediaOutputPMG.RawMedia.GetRange(RecordIndex == 0 ? 0 : RecordIndex, RawMediaOutputPMG.RawMedia.Count - RecordIndex).Select(RawMediaObj => "'" + RawMediaObj.RawMediaID + "'").ToArray());

                                     RawMediaOutput.RawMedia.AddRange(RawMediaOutputPMG.RawMedia.GetRange(RecordIndex == 0 ? 0 : RecordIndex, RawMediaOutputPMG.RawMedia.Count - RecordIndex));
                                 }

                                    
                             }
                             else if (RawMediaOutputPMG.RawMedia.Count > 0)
                             {
                                 HasNextPage = true;
                             }*/
                            }
                            else
                            {
                                RawMediaOutput.RawMedia.AddRange(RawMediaOutputPMG.RawMedia);
                                /*HasNextPage = true;*/

                                List<AdvancedSearchServiceState> _ListOfAdv = Context.AdvancedSearchServiceStates.Where("it.SessionID='" + SessionID + "'").ToList();
                                int MaxPageNo = _ListOfAdv.Max(_tmpMaxAdvnacedSearchServiceState => _tmpMaxAdvnacedSearchServiceState.PageNo);

                                if (RawMediaOutput.PageNumber < MaxPageNo)
                                {
                                    TotalPageNo = Convert.ToInt32(ConfigurationManager.AppSettings["MaxNoOfPagesSaved"]) + 1;
                                    IsLastExistingPage = true;
                                    continue;
                                }
                                else
                                {
                                    if (AdvancedSearchServiceStateForSamePage != null && AdvancedSearchServiceStateForSamePage.SSPPageNo == DBPageIndex && AdvancedSearchServiceStateForSamePage.PMGPageNo == MaxPMGPage && AdvancedSearchServiceStateForSamePage.IsSamePMGPage == false)
                                    {
                                        PMGPageIndex = PMGPageIndex + 1;
                                        DBPageIndex = DBPageIndex + 1;
                                        continue;
                                    }
                                    else
                                    {
                                        RawMediaOutput.RawMedia.RemoveRange(0, RawMediaOutput.RawMedia.Count);

                                        TotalPageNo = TotalPageNo + 1;
                                        IsExistingPage = false;
                                        IsLastExistingPage = true;
                                        break;
                                    }
                                }
                            }

                            Logger.LogInfo("End Insert RawMedia into Response");
                        }

                        RecordIndex = 0;

                        PMGPageIndex = PMGPageIndex + 1;

                    } while (RawMediaOutput != null && TotalPageNo <= Convert.ToInt32(ConfigurationManager.AppSettings["MaxNoOfPagesSaved"]) && PMGPageIndex <= MaxPMGPage);

                    if (IsLastExistingPage == false)
                    {
                        IsFirst = false;
                        PMGPageIndex = 1;
                        DBPageIndex = DBPageIndex + 1;
                    }
                    else
                    {
                        continue;
                    }

                } while (RawMediaOutput != null && TotalPageNo <= Convert.ToInt32(ConfigurationManager.AppSettings["MaxNoOfPagesSaved"]) && DBPageIndex <= MaxDBPage);

                Logger.LogInfo("End GetRawMedia-SSP-to-PMG");

                RawMediaOutput.HasNextPage = HasNextPage;

                return RawMediaOutput;

            }
            catch (CustomException _CustomException)
            {
                throw _CustomException;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private Int16 CheckForSerchCriteriaSame(RawMediaInput RawMediaInput)
        {
            try
            {
                bool IsEqual = false;

                string SessionID = IQMedia.Web.Common.Authentication.IsAuthenticated ? HttpContext.Current.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName].Value : RawMediaInput.SessionID;
                var ObjectResult = Context.AdvancedSearchServiceStates.Where(_AdvancedSearchServiceStates => _AdvancedSearchServiceStates.SessionID.Equals(SessionID));
                DateTime? MaxDate = ObjectResult.Max(_AdvancedSearchServiceStates => (DateTime?)(_AdvancedSearchServiceStates.RequestedDate));

                string ExistedSearchCriteria = string.Empty;

                ExistedSearchCriteria = ObjectResult != null && ObjectResult.Count() > 0 ? ObjectResult.ToList().First().SearchCriteria : string.Empty;
                //Context.GetSearchCriteriaBySessionID(SessionID).Single();                               
                RawMediaInput ExistedSearchCriteriaObj = null;

                if (string.IsNullOrEmpty(ExistedSearchCriteria))
                {
                    return 1;
                }

                DataContractJsonSerializer Deserializer = new DataContractJsonSerializer(RawMediaInput.GetType());
                MemoryStream MemoryStream = new System.IO.MemoryStream(Encoding.Unicode.GetBytes(ExistedSearchCriteria));

                ExistedSearchCriteriaObj = (RawMediaInput)Deserializer.ReadObject(MemoryStream);

                IsEqual = Comparer(RawMediaInput.IQ_Cat_Set,
                                        ExistedSearchCriteriaObj.IQ_Cat_Set,
                                        RawMediaInput.IQ_Class_Set,
                                        ExistedSearchCriteriaObj.IQ_Class_Set,
                                        RawMediaInput.IQ_Dma_Set,
                                        ExistedSearchCriteriaObj.IQ_Dma_Set,
                                        RawMediaInput.Station_Affil_Set,
                                        ExistedSearchCriteriaObj.Station_Affil_Set
                                       );


                if (IsEqual == false)
                {
                    return 1;
                }

                if (RawMediaInput.Desc100.Trim() != ExistedSearchCriteriaObj.Desc100.Trim())
                {
                    return 1;
                }

                if (!(RawMediaInput.IQ_Time_Zone == null && ExistedSearchCriteriaObj.IQ_Time_Zone == null) && RawMediaInput.IQ_Time_Zone.Trim() != ExistedSearchCriteriaObj.IQ_Time_Zone.Trim())
                {
                    return 1;
                }

                if (RawMediaInput.SearchTerm.Trim() != ExistedSearchCriteriaObj.SearchTerm.Trim())
                {
                    return 1;
                }

                if (RawMediaInput.Title120.Trim() != ExistedSearchCriteriaObj.Title120.Trim())
                {
                    return 1;
                }

                if (RawMediaInput.FromDate != ExistedSearchCriteriaObj.FromDate)
                {
                    return 1;
                }

                if (RawMediaInput.ToDate != ExistedSearchCriteriaObj.ToDate)
                {
                    return 1;
                }

                if (RawMediaInput.SortField.Trim() != ExistedSearchCriteriaObj.SortField.Trim())
                {
                    return 1;
                }

                if (RawMediaInput.PageSize != ExistedSearchCriteriaObj.PageSize)
                {
                    return 1;
                }

                if (DateTime.Now > MaxDate.Value.AddSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["SearchAPIRequestCacheTimeout"])))
                {
                    return 0;
                }

                return 2;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private RawMediaOutput GetRawMediaPMGFBySSP(RawMediaInput RawMediaInput)
        {
            try
            {
                Logger.LogInfo("Start GetRawMedia-PMG-to-SSP");

                Logger.LogInfo("Start PMG Request");

                string PMGSearchRequestUrl = ConfigurationManager.AppSettings["PMGSearchUrl"].ToString();

                int _PMGMaxHighlights = 20;

                if (ConfigurationManager.AppSettings["PMGMaxHighlights"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["PMGMaxHighlights"], out _PMGMaxHighlights);
                }

                string SSP_Index_Key = GenerateSSPIndexKey(RawMediaInput.IQ_Cat_Set, RawMediaInput.IQ_Class_Set, RawMediaInput.IQ_Dma_Set, RawMediaInput.Station_Affil_Set);

                string SortFieldsConverted = GenerateSortField(RawMediaInput, SortFieldType.PMGSearch);

                RawMediaOutput RawMediaOutput = GetRawMediaFromPMG(PMGSearchRequestUrl,
                                                                   RawMediaInput.SearchTerm,
                                                                   string.Empty,
                                                                    RawMediaInput.PageNumber,
                                                                    RawMediaInput.PageSize,
                                                                    _PMGMaxHighlights,
                                                                   Convert.ToDateTime(RawMediaInput.FromDate),
                                                                   Convert.ToDateTime(RawMediaInput.ToDate),
                                                                    SortFieldsConverted,
                                                                    SSP_Index_Key,
                                                                    RawMediaInput.Title120,
                                                                    RawMediaInput.Desc100,
                                                                    RawMediaInput.IQ_Time_Zone
                                                                    );
                Logger.LogInfo("End PMG Request");

                if (RawMediaOutput != null && RawMediaOutput.RawMedia != null && RawMediaOutput.RawMedia.Count > 0)
                {
                    string RawMediaGUIDs = string.Join(",", RawMediaOutput.RawMedia.Select(RawMediaObj => "'" + RawMediaObj.RawMediaID + "'").ToArray());

                    if (!string.IsNullOrEmpty(RawMediaGUIDs))
                    {
                        Logger.LogInfo("Start SSP Request");

                        List<RawMedia> RawMediaDB = (Context.GetStatskedprogByGUIDs(RawMediaGUIDs)).ToList().Select(RawMediaObj => new RawMedia()
                                                                                                                {
                                                                                                                    DateTime = Convert.ToString(RawMediaObj.DateTime),
                                                                                                                    IQ_Dma_Name = RawMediaObj.IQ_Dma_Name,
                                                                                                                    RawMediaID = new Guid(RawMediaObj.RawMediaID),
                                                                                                                    StationLogo = RawMediaObj.StationLogo,
                                                                                                                    Title120 = RawMediaObj.Title120
                                                                                                                }).ToList<RawMedia>();

                        Logger.LogInfo("End SSP Request");

                        Logger.LogInfo("Start Insert RawMedia into Response");

                        RawMediaOutput.RawMedia.ForEach(delegate(RawMedia _RawMedia)
                        {
                            _RawMedia.URL = IframeURL + "?RawMediaID=" + _RawMedia.RawMediaID + "&SearchTerm=" + RawMediaInput.SearchTerm;

                            RawMedia _RawMediaTmp = RawMediaDB.Find(delegate(RawMedia _RawMediaObj) { return _RawMediaObj.RawMediaID == _RawMedia.RawMediaID; });

                            if (_RawMediaTmp != null)
                            {
                                _RawMedia.Title120 = _RawMediaTmp.Title120;
                            }
                        });

                        Logger.LogInfo("End Insert RawMedia into Response");
                    }
                }

                Logger.LogInfo("End GetRawMedia-PMG-to-SSP");

                return RawMediaOutput;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private RawMediaOutput GetRawMediaFromPMG(string p_PMGSearchRequestUrl,
                                                  string p_SearchTerm,
                                                  string p_GUIDList,
                                                  int? p_PageNumber,
                                                  int? p_PageSize,
                                                  int p_PMGMaxHighlights,
                                                  DateTime? p_FromDate,
                                                  DateTime? p_ToDate,
                                                  string p_SortFields,
                                                  string p_SSP_Index_Key,
                                                  string p_Title120,
                                                  string p_Desc100,
                                                  string p_IQ_Time_Zone)
        {
            try
            {
                SearchEngine _SearchEngine = new SearchEngine(p_PMGSearchRequestUrl);

                SearchRequest _SearchRequest = new SearchRequest();

                _SearchRequest.Terms = p_SearchTerm.Trim();

                if (string.IsNullOrEmpty(p_GUIDList))
                {
                    _SearchRequest.PageNumber = p_PageNumber.Value;
                    _SearchRequest.PageSize = p_PageSize.Value;
                    _SearchRequest.StartDate = p_FromDate;
                    _SearchRequest.EndDate = p_ToDate;
                }
                else
                {
                    _SearchRequest.GUIDList = p_GUIDList;
                    _SearchRequest.PageNumber = 1;
                    _SearchRequest.PageSize = p_GUIDList.Count(C => C == ',') + 1;

                }

                _SearchRequest.MaxHighlights = p_PMGMaxHighlights;
                _SearchRequest.SortFields = p_SortFields;

                SearchResult _SearchResult = _SearchEngine.search(_SearchRequest);

                string _Responce = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();
                _XmlDocument.LoadXml(_SearchResult.ResponseXML);

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

                /*
                 
                 insert into pmgsearchlog

                SearchLog(_SearchRequest.Terms, _SearchRequest.PageNumber, _SearchRequest.PageSize, _SearchRequest.MaxHighlights, _SearchRequest.GUIDList, _SearchRequest.StartDate, _SearchRequest.EndDate, _Responce, "Advance2");
                 
                 */

                PMGSearchLogLogic PMGSearchLogLogicObj = (PMGSearchLogLogic)LogicFactory.GetLogic(LogicType.PMGSearchLog);

                PMGSearchLogLogicObj.InsertPMGSearchLog(_SearchRequest.Terms, _SearchRequest.PageNumber, _SearchRequest.PageSize, _SearchRequest.MaxHighlights, _SearchRequest.GUIDList,
                                                        _SearchRequest.StartDate, _SearchRequest.EndDate, _Responce, "Advance2",
                                                        p_SSP_Index_Key, p_Title120, p_Desc100, p_IQ_Time_Zone,Convert.ToInt32(Authentication.CurrentUser));

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

                var RawMediaList = (from _Hit in _SearchResult.Hits
                                    select new
                                    {
                                        RawMediaID = new Guid(_Hit.GUID),
                                        Hits = _Hit.TermOccurrences.Count,
                                        IQ_Dma_Name = _Hit.market,
                                        DateTime = new DateTime(_Hit.TimeStamp.Year, _Hit.TimeStamp.Month, _Hit.TimeStamp.Day, (_Hit.Hour / 100), 0, 0),
                                        StationLogo = GetStationLogo(_Hit.StationID)
                                    }).ToList().Select(RawMediaObj => new RawMedia()
                                                        {
                                                            DateTime = Convert.ToString(RawMediaObj.DateTime),
                                                            Hits = RawMediaObj.Hits,
                                                            IQ_Dma_Name = RawMediaObj.IQ_Dma_Name,
                                                            RawMediaID = RawMediaObj.RawMediaID,
                                                            StationLogo = RawMediaObj.StationLogo
                                                        });

                List<RawMedia> _ListOfRawMedia = new List<RawMedia>(RawMediaList);

                RawMediaOutput.RawMedia = _ListOfRawMedia;

                /* Start Flag for NextPage is available or not */

                if (_SearchResult.TotalHitCount > p_PageSize)
                {
                    RawMediaOutput.HasNextPage = true;
                }

                /* Stop Flag for NextPage is available or not */

                return RawMediaOutput;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private bool Comparer(List<IQ_Cat_Set> p_IQ_Cat_Set_Input,
                                List<IQ_Cat_Set> p_IQ_Cat_Set_Existed,
                                List<IQ_Class_Set> p_IQ_Class_Set_Input,
                                List<IQ_Class_Set> p_IQ_Class_Set_Existed,
                                List<IQ_Dma_Set> p_IQ_Dma_Set_Input,
                                List<IQ_Dma_Set> p_IQ_Dma_Set_Existed,
                                List<Station_Affil_Set> p_Station_Affil_Set_Input,
                                List<Station_Affil_Set> p_Station_Affil_Set_Existed
                            )
        {
            try
            {

                var IQ_Cat_Diff = (from _IQ_Cat_DB in p_IQ_Cat_Set_Existed select _IQ_Cat_DB.IQ_Cat_Num).Except
                            (from _IQ_Cat_Input in p_IQ_Cat_Set_Input select _IQ_Cat_Input.IQ_Cat_Num);

                if (IQ_Cat_Diff.Count() > 0)
                {
                    return false;
                }

                var IQ_Class_Diff = (from _IQ_Class_DB in p_IQ_Class_Set_Existed select _IQ_Class_DB.IQ_Class_Num).Except
                           (from _IQ_Class_Input in p_IQ_Class_Set_Input select _IQ_Class_Input.IQ_Class_Num);

                if (IQ_Class_Diff.Count() > 0)
                {
                    return false;
                }

                var IQ_Dma_Diff = (from _IQ_Dma_DB in p_IQ_Dma_Set_Existed select _IQ_Dma_DB.IQ_Dma_Num).Except
                            (from _IQ_Dma_Input in p_IQ_Dma_Set_Input select _IQ_Dma_Input.IQ_Dma_Num);

                if (IQ_Dma_Diff.Count() > 0)
                {
                    return false;
                }

                var Station_Affil_Diff = (from _Station_Affil_DB in p_Station_Affil_Set_Existed select _Station_Affil_DB.Station_Affil_Num).Except
                           (from _Station_Affil_Input in p_Station_Affil_Set_Input select _Station_Affil_Input.Station_Affil_Num);

                if (Station_Affil_Diff.Count() > 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private RawMediaOutput GetRawMediaDefaultSearch(RawMediaInput RawMediaInput)
        {
            try
            {
                Logger.LogInfo("Start GetRawMedia-Default Search");

                string SortFieldsConverted = GenerateSortField(RawMediaInput, SortFieldType.SSPSearch);

                ObjectParameter TotalRecordsCount = new ObjectParameter("TotalRecordsCount", typeof(Int32));

                string SSP_Index_Key = GenerateSSPIndexKey(RawMediaInput.IQ_Cat_Set, RawMediaInput.IQ_Class_Set, RawMediaInput.IQ_Dma_Set, RawMediaInput.Station_Affil_Set);

                if (RawMediaInput.IQ_Time_Zone.Trim().ToLower() == "all")
                {
                    RawMediaInput.IQ_Time_Zone = null;
                }

                Logger.LogInfo("Start DB Request");

                var RawMediaList = from RawMediaDBObj in Context.GetRawMediaDefaultSearch(RawMediaInput.IQ_Time_Zone,
                                                                                           SSP_Index_Key,
                                                                                          Convert.ToDateTime(RawMediaInput.FromDate),
                                                                                          Convert.ToDateTime(RawMediaInput.ToDate),
                                                                                           RawMediaInput.Title120,
                                                                                           RawMediaInput.Desc100,
                                                                                           RawMediaInput.PageNumber,
                                                                                           RawMediaInput.PageSize,
                                                                                           SortFieldsConverted,
                                                                                           TotalRecordsCount
                                                                                           )
                                   select new RawMedia
                                   {
                                       RawMediaID = new Guid(RawMediaDBObj.RawMediaID),
                                       DateTime = Convert.ToString(RawMediaDBObj.DateTime),
                                       StationLogo = GetStationLogo(RawMediaDBObj.StationLogo),
                                       Title120 = RawMediaDBObj.Title120,
                                       IQ_Dma_Name = RawMediaDBObj.IQ_Dma_Name,
                                       URL = IframeURL + "?RawMediaID=" + RawMediaDBObj.RawMediaID + "&SearchTerm=" + RawMediaInput.SearchTerm
                                   };

                RawMediaOutput RawMediaOutput = new RawMediaOutput();

                List<RawMedia> ListOfRawMedia = new List<RawMedia>(RawMediaList);

                /*foreach (RawMedia RawMedia in ListOfRawMedia)
                {
                    RawMedia.StationLogo = GetStationLogo(RawMedia.StationLogo);
                }*/

                Logger.LogInfo("End DB Request");

                Logger.LogInfo("Start Insert RawMedia into Response");

                RawMediaOutput.RawMedia = ListOfRawMedia;

                /* Start Flag for NextPage is available or not */

                if (Convert.ToInt32(TotalRecordsCount.Value) > (RawMediaInput.PageNumber * RawMediaInput.PageSize))
                {
                    RawMediaOutput.HasNextPage = true;
                }
                else
                {
                    RawMediaOutput.HasNextPage = false;
                }

                Logger.LogInfo("End Insert RawMedia into Response");

                Logger.LogInfo("End GetRawMedia-Default Search");

                /* Stop Flag for NextPage is available or not */

                return RawMediaOutput;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string GetStationLogo(string RawMediaStationID)
        {
            try
            {
                StringBuilder BaseURL = new StringBuilder();

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsProduction"]) && Convert.ToBoolean(ConfigurationManager.AppSettings["IsProduction"]) == true)
                {
                    BaseURL.Append("http://iqmediacorp.com/StationLogoImages/");
                }
                else
                {
                    BaseURL.Append("http://qa.iqmediacorp.com/StationLogoImages/");
                }

                return BaseURL.ToString() + RawMediaStationID + ".jpg";

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string GenerateSSPIndexKey(List<IQ_Cat_Set> ListOfIQ_Cat_Set, List<IQ_Class_Set> ListOfIQ_Class_Set, List<IQ_Dma_Set> ListOfIQ_Dma_Set, List<Station_Affil_Set> ListOfStationAffilSet)
        {
            try
            {
                StringBuilder SSPIndexKeys = new StringBuilder();

                List<string> ListOfSSPIndexKey = new List<string>();

                foreach (IQ_Dma_Set IQ_Dma in ListOfIQ_Dma_Set)
                {
                    foreach (Station_Affil_Set Station_Affil in ListOfStationAffilSet)
                    {
                        foreach (IQ_Cat_Set IQ_Cat in ListOfIQ_Cat_Set)
                        {
                            foreach (IQ_Class_Set IQ_Class in ListOfIQ_Class_Set)
                            {
                                SSPIndexKeys.Append("'1" + IQ_Dma.IQ_Dma_Num + Station_Affil.Station_Affil_Num + IQ_Cat.IQ_Cat_Num + IQ_Class.IQ_Class_Num + "',");
                            }
                        }
                    }
                }

                SSPIndexKeys.Remove(SSPIndexKeys.Length - 1, 1);

                //SSPIndexKeys = string.Join(",", _SSPIndexKeys.ToArray<string>());

                return SSPIndexKeys.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
