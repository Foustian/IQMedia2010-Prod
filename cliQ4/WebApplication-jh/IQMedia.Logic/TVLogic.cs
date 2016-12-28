using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using System.Configuration;
using PMGSearch;
using System.Xml.Linq;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Web.Logic.Base;
using System.Xml;
using IQMedia.Shared.Utility;

namespace IQMedia.Web.Logic
{
    public class TVLogic : ILogic
    {
        public List<IQAgent_TVResultsModel> TimeshiftSearchResults(int p_CustomerKey, Guid p_ClientGUID, string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_StationID, string p_Class,
                                                                bool p_IsAsc, Int32 p_PageNumber, bool isallDmaAllowed,
                                                                bool isallClassAllowed, bool isallStationAllowed,
                                                                string p_SortColumn, ref int p_TotalResults, string pmgurl, List<int> IQTVRegion, List<int> IQTVCountry, int? p_RegionNum = null, int? p_CountryNum = null)
        {
            try
            {

                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TimeshiftPageSize"]);
                int FragSize = Convert.ToInt32(ConfigurationManager.AppSettings["FragSize"]);
                //TempData["PageNumber"] = null;
                //hasMoreResultPage = false;
                //hasPreviousResultPage = false;

                Uri PMGSearchRequestUrl = new Uri(pmgurl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchRequest tvRequest = new SearchRequest();
                tvRequest.IsShowCC = false;
                //tvRequest.IsTitleandCCSearchAllowed = true;
                tvRequest.IsSentiment = false;
                tvRequest.FragSize = FragSize;
                tvRequest.Terms = !string.IsNullOrEmpty(p_SearchTerm) ? p_SearchTerm : null;
                tvRequest.Title120 = p_Title;
                tvRequest.IncludeRegionsNum = IQTVRegion;
                tvRequest.CountryNums = IQTVCountry;
                tvRequest.PageSize = PageSize;
                tvRequest.PageNumber = p_PageNumber;
                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                bool p_IsAllDmaAllowed;
                bool p_IsAllClassAllowed;
                bool p_IsAllStationAllowed;

                Dictionary<string, object> dicSSP = sspLogic.GetSSPDataByClientGUID(p_ClientGUID, out p_IsAllDmaAllowed, out p_IsAllClassAllowed, out p_IsAllStationAllowed, IQTVRegion);

                var isRegionValid = false;
                if (p_RegionNum != null && p_RegionNum > 0)
                {
                    var regNum = ((List<IQ_Region>)dicSSP["IQ_Region"]).Where(r => r.Num == p_RegionNum.Value).FirstOrDefault();

                    if (regNum != null)
                    {
                        tvRequest.IncludeRegionsNum=new List<int>{regNum.Num};
                        isRegionValid = true;
                    }
                    else
                    {
                        tvRequest.IncludeRegionsNum = ((List<IQ_Region>)dicSSP["IQ_Region"]).Select(a => a.Num).ToList();
                        isRegionValid = true;
                    }
                }
                else if (dicSSP["IQ_Region"] != null && ((List<IQ_Region>)dicSSP["IQ_Region"]).ToList().Count > 0)
                {
                    tvRequest.IncludeRegionsNum = ((List<IQ_Region>)dicSSP["IQ_Region"]).Select(a => a.Num).ToList();
                    isRegionValid = true;
                }

                if (!isRegionValid)
                {
                    throw new CustomException("Invalid Region");
                }

                var isCountryValid = false;
                if (p_CountryNum != null && p_CountryNum > 0)
                {
                    var couNum = ((List<IQ_Country>)dicSSP["IQ_Country"]).Where(r => r.Num == p_CountryNum).FirstOrDefault();

                    if (couNum != null)
                    {
                        tvRequest.CountryNums=new List<int>{p_CountryNum.Value};
                        isCountryValid = true;
                    }
                    else
                    {
                        tvRequest.CountryNums = ((List<IQ_Country>)dicSSP["IQ_Country"]).Select(a => a.Num).ToList();
                        isCountryValid = true;
                    }
                }
                else if (dicSSP["IQ_Country"] != null && ((List<IQ_Country>)dicSSP["IQ_Country"]).ToList().Count > 0)
                {
                    tvRequest.CountryNums = ((List<IQ_Country>)dicSSP["IQ_Country"]).Select(a => a.Num).ToList();
                    isCountryValid = true;
                }

                if (!isCountryValid)
                {
                    throw new CustomException("Invalid Country");
                }

                bool isDmaValid = false;
                isDmaValid = p_IsAllDmaAllowed;
                if (p_Dma != null && p_Dma.Count > 0)
                {
                    if (!isallDmaAllowed)
                    {
                        List<string> lstdam = ((List<IQ_Dma>)dicSSP["IQ_Dma"]).Join(p_Dma, a => a.Name, b => b, (a, b) => b).ToList();
                        tvRequest.IQDmaName = lstdam;
                        isDmaValid = true;
                    }
                    else
                    {
                        tvRequest.IQDmaName = p_Dma;
                        isDmaValid = true;
                    }
                }
                else if (!isallDmaAllowed && dicSSP["IQ_Dma"] != null && ((List<IQ_Dma>)dicSSP["IQ_Dma"]).ToList().Count > 0)
                {
                    tvRequest.IQDmaName = ((List<IQ_Dma>)dicSSP["IQ_Dma"]).Select(a => a.Name).ToList();
                    isDmaValid = true;
                }

                if (!isDmaValid)
                {
                    throw new CustomException("Invalid Dma");
                }

                bool isAffiliateValid = false;
                isAffiliateValid = p_IsAllStationAllowed;
                if (p_Station != null && p_Station.Count > 0)
                {
                    if (!isallStationAllowed)
                    {
                        List<string> lststation = ((List<Station_Affil>)dicSSP["Station_Affil"]).Join(p_Station, a => a.Name, b => b, (a, b) => b).ToList();
                        tvRequest.StationAffil = lststation;
                        isAffiliateValid = true;
                    }
                    else
                    {
                        tvRequest.StationAffil = p_Station;
                        isAffiliateValid = true;
                    }
                }
                else if (!isallStationAllowed && dicSSP["Station_Affil"] != null && ((List<Station_Affil>)dicSSP["Station_Affil"]).ToList().Count > 0)
                {
                    tvRequest.StationAffil = ((List<Station_Affil>)dicSSP["Station_Affil"]).Select(a => a.Name).ToList();
                    isAffiliateValid = true;
                }

                if (!isAffiliateValid)
                {
                    throw new CustomException("Invalid Affiliate");
                }

                bool isStationValid = false;
                isStationValid = p_IsAllStationAllowed;
                if (p_StationID != null && p_StationID.Count > 0)
                {
                    if (!isallStationAllowed)
                    {
                        List<string> lststation = ((List<string>)dicSSP["IQ_Station"]).Join(p_StationID, a => a, b => b, (a, b) => b).ToList();
                        tvRequest.Stations = lststation;
                        isStationValid = true;
                    }
                    else
                    {
                        tvRequest.Stations = p_StationID;
                        isStationValid = true;
                    }
                }
                else if (!isallStationAllowed && dicSSP["IQ_Station"] != null && ((List<IQ_Station>)dicSSP["IQ_Station"]).ToList().Count > 0)
                {
                    tvRequest.Stations = ((List<IQ_Station>)dicSSP["IQ_Station"]).Select(s=>s.IQ_Station_ID).ToList();
                    isStationValid = true;
                }

                if (!isStationValid)
                {
                    throw new CustomException("Invalid Station");
                }

                if (string.IsNullOrWhiteSpace(p_Class) && !isallClassAllowed)
                {
                    List<IQ_Class> lstIQClass = (List<IQ_Class>)dicSSP["IQ_Class"];
                    if (lstIQClass != null && lstIQClass.Count > 0)
                    {
                        List<string> lstclass = new List<string>();
                        foreach (IQ_Class iqclass in lstIQClass)
                        {
                            lstclass.Add(iqclass.Num);
                        }
                        tvRequest.IQClassNum = lstclass;
                    }
                }


                if (!string.IsNullOrWhiteSpace(p_SortColumn))
                {
                    tvRequest.SortFields = p_SortColumn;
                    if (!p_IsAsc)
                    {
                        tvRequest.SortFields = tvRequest.SortFields + "-";
                    }

                }
                else
                {
                    if (p_IsAsc == true)
                    {
                        tvRequest.SortFields = "datetime";

                    }
                    else
                    {
                        tvRequest.SortFields = "datetime-";
                    }
                }


                tvRequest.ClientGuid = p_ClientGUID;
                if (p_FromDate != null)
                {
                    tvRequest.RLStationStartDate = p_FromDate;
                }
                if (p_ToDate != null)
                {
                    tvRequest.RLStationEndDate = p_ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                if (!string.IsNullOrEmpty(p_Class))
                {
                    List<string> tempClass = new List<string>();
                    tempClass.Add(p_Class);

                    tvRequest.IQClassNum = tempClass; // !string.IsNullOrEmpty(p_Dma) ? p_Dma.ToList() : null;
                }

                string CommaSepratedDmaList = string.Empty;
                string CommaSepratedClassList = string.Empty;
                string CommaSepratedStationList = string.Empty;

                if (tvRequest.IQDmaName != null)
                {
                    CommaSepratedDmaList = string.Join(",", tvRequest.IQDmaName.ToArray()); ;
                }

                if (tvRequest.IQClassNum != null)
                {
                    CommaSepratedClassList = string.Join(",", tvRequest.IQClassNum.ToArray());
                }

                if (tvRequest.StationAffil != null)
                {
                    CommaSepratedStationList = string.Join(",", tvRequest.StationAffil.ToArray());
                }


                SearchResult tvResult = searchEngine.Search(tvRequest);

                #region insert pmg log
                string _Responce = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(tvResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Responce = _XmlDocument.InnerXml;
                            _Responce = _Responce.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Responce = null;
                }

                UtilityLogic.InsertPMGSearchLog(p_CustomerKey, tvRequest.Terms, tvRequest.Title120, CommaSepratedDmaList, CommaSepratedStationList, CommaSepratedClassList, tvRequest.PageNumber, tvRequest.PageSize, tvRequest.MaxHighlights, tvRequest.StartDate, tvRequest.EndDate, IQMedia.Shared.Utility.CommonFunctions.SearchType.TimeShift.ToString(), _Responce);


                #endregion

                p_TotalResults = tvResult.TotalHitCount;
                List<IQAgent_TVResultsModel> listOfTVResult = new List<IQAgent_TVResultsModel>();

                if (tvResult != null && tvResult.TotalHitCount > 0)
                {

                    XDocument xDoc = new XDocument(new XElement("list"));
                    foreach (Hit hit in tvResult.Hits)
                    {
                        IQAgent_TVResultsModel iqagent = new IQAgent_TVResultsModel();

                        iqagent.StationID = hit.StationId;
                        iqagent.Title120 = hit.Title120;
                        iqagent.Market = hit.Market;
                        iqagent.Hits = hit.TotalNoOfOccurrence;
                        if (!string.IsNullOrEmpty(hit.RLStationDateTime))
                        {
                            iqagent.Date = Convert.ToDateTime(hit.RLStationDateTime);
                        }
                        iqagent.RL_VideoGUID = new Guid(hit.Guid);
                        iqagent.IQ_CC_Key = hit.Iqcckey;
                        iqagent.TimeZone = hit.ClipTimeZone;

                        listOfTVResult.Add(iqagent);

                        xDoc.Root.Add(new XElement("item", new XAttribute("iq_cc_key", hit.Iqcckey), new XAttribute("iq_dma", hit.IQDmaNum)));


                    }
                    IQNielsenDA iqNielsenDA = (IQNielsenDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNielsen);
                    List<NielsenDataModel> listOfNielsen = iqNielsenDA.GetTimeshiftNielsenDataByXML(xDoc, p_ClientGUID);

                    foreach (var tvr in listOfTVResult)
                    {
                        NielsenDataModel ns = listOfNielsen.Where(nso => String.Compare(nso.IQ_CC_Key, tvr.IQ_CC_Key, true).Equals(0)).FirstOrDefault();
                        if (ns != null)
                        {
                            tvr.AUDIENCE = ns.Audience;
                            tvr.SQAD_SHAREVALUE = ns.IQAdsharevalue;
                            tvr.IsActualNielsen = ns.IsActualNielsen;
                        }
                    }

                    //lstDiscoveryMediaResult = iqNielsenDA.GetNielsenDataByXML(xDoc, lstDiscoveryMediaResult, clientGUID);

                }

                return listOfTVResult;
            }
            catch (IQMedia.Shared.Utility.CustomException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //return Json(new object());
        }

        public List<IQAgent_TVResultsModel> KantorSearchResults(int p_CustomerKey, Guid p_ClientGUID, List<String> p_lstIQ_CC_Key, Int32 p_PageSize, Int32 p_PageNumber, ref int p_TotalResults, string pmgurl, List<int> IQTVRegion, List<int> IQTVCountry)
        {
            try
            {
                int PageSize = p_PageSize;

                Uri PMGSearchRequestUrl = new Uri(pmgurl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchRequest tvRequest = new SearchRequest();
                tvRequest.IsShowCC = false;
                tvRequest.IsSentiment = false;
                tvRequest.IncludeRegionsNum = IQTVRegion;
                tvRequest.CountryNums = IQTVCountry;
                tvRequest.PageSize = PageSize;
                tvRequest.PageNumber = p_PageNumber;
                tvRequest.IQCCKeyList = string.Join(",", p_lstIQ_CC_Key);

                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                bool p_IsAllDmaAllowed;
                bool p_IsAllClassAllowed;
                bool p_IsAllStationAllowed;

                Dictionary<string, object> dicSSP = sspLogic.GetSSPDataByClientGUID(p_ClientGUID, out p_IsAllDmaAllowed, out p_IsAllClassAllowed, out p_IsAllStationAllowed, IQTVRegion);

                if (!p_IsAllDmaAllowed)
                {
                    tvRequest.IQDmaName = ((List<IQ_Dma>)dicSSP["IQ_Dma"]).Select(a => a.Name).ToList();
                }

                if (!p_IsAllStationAllowed)
                {
                    tvRequest.StationAffil = ((List<Station_Affil>)dicSSP["Station_Affil"]).Select(a => a.Name).ToList();
                }

                if (!p_IsAllClassAllowed)
                {
                    List<IQ_Class> lstIQClass = (List<IQ_Class>)dicSSP["IQ_Class"];
                    if (lstIQClass != null && lstIQClass.Count > 0)
                    {
                        List<string> lstclass = new List<string>();
                        foreach (IQ_Class iqclass in lstIQClass)
                        {
                            lstclass.Add(iqclass.Num);
                        }
                        tvRequest.IQClassNum = lstclass;
                    }
                }

                tvRequest.ClientGuid = p_ClientGUID;

                SearchResult tvResult = searchEngine.Search(tvRequest);

                #region insert pmg log
                string _Responce = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(tvResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Responce = _XmlDocument.InnerXml;
                            _Responce = _Responce.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Responce = null;
                }

                UtilityLogic.InsertPMGSearchLog(p_CustomerKey, tvRequest.Terms, tvRequest.Title120, string.Empty, string.Empty, string.Empty, tvRequest.PageNumber, tvRequest.PageSize, tvRequest.MaxHighlights, tvRequest.StartDate, tvRequest.EndDate, IQMedia.Shared.Utility.CommonFunctions.SearchType.TimeShift.ToString(), _Responce, string.Join(",", p_lstIQ_CC_Key));


                #endregion

                p_TotalResults = tvResult.TotalHitCount;
                List<IQAgent_TVResultsModel> listOfTVResult = new List<IQAgent_TVResultsModel>();

                if (tvResult != null && tvResult.TotalHitCount > 0)
                {

                    foreach (Hit hit in tvResult.Hits)
                    {
                        IQAgent_TVResultsModel iqagent = new IQAgent_TVResultsModel();

                        iqagent.StationID = hit.StationId;
                        iqagent.Title120 = hit.Title120;
                        iqagent.Market = hit.Market;
                        iqagent.Hits = hit.TotalNoOfOccurrence;
                        if (!string.IsNullOrEmpty(hit.RLStationDateTime))
                        {
                            iqagent.Date = Convert.ToDateTime(hit.RLStationDateTime);
                        }
                        iqagent.RL_VideoGUID = new Guid(hit.Guid);
                        iqagent.IQ_CC_Key = hit.Iqcckey;
                        iqagent.TimeZone = hit.ClipTimeZone;

                        listOfTVResult.Add(iqagent);
                    }
                }

                return listOfTVResult.OrderBy(a => a.StationID).OrderByDescending(a => a.Date).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int SelectDownloadLimit(string CustomerGUID)
        {
            TVDA tvDA = (TVDA)DataAccessFactory.GetDataAccess(DataAccessType.TV);
            return tvDA.SelectDownloadLimit(CustomerGUID);
        }

        public string Insert_ClipDownload(string CustomerGUID, long ID)
        {
            TVDA tvDA = (TVDA)DataAccessFactory.GetDataAccess(DataAccessType.TV);
            return tvDA.Insert_ClipDownload(CustomerGUID, ID);
        }

        public List<ClipDownload> SelectClipDownloadByCustomer(string CustomerGUID)
        {
            TVDA tvDA = (TVDA)DataAccessFactory.GetDataAccess(DataAccessType.TV);
            return tvDA.SelectClipDownloadByCustomer(CustomerGUID);
        }

        public string SelectClipLocationFromIQCore_Meta(long ID, Guid CustomerGuid)
        {
            TVDA tvDA = (TVDA)DataAccessFactory.GetDataAccess(DataAccessType.TV);
            return tvDA.SelectClipLocationFromIQCore_Meta(ID, CustomerGuid);
        }

        public bool CheckIntoIQServiceAndIQRemoetService_Export(string ClipGUID, string Extension)
        {
            TVDA tvDA = (TVDA)DataAccessFactory.GetDataAccess(DataAccessType.TV);
            return tvDA.CheckIntoIQServiceAndIQRemoetService_Export(ClipGUID, Extension);
        }

        public string Update_ClipDownload(long ClipdownloadKey, string FileLocation, string FileExtension, int DownloadStatus, Guid CustomerGuid)
        {
            TVDA tvDA = (TVDA)DataAccessFactory.GetDataAccess(DataAccessType.TV);
            return tvDA.Update_ClipDownload(ClipdownloadKey, FileLocation, FileExtension, DownloadStatus, CustomerGuid);
        }

        public string Delete_ClipDownload(string CustomerGUID, long ClipdownloadKey)
        {
            TVDA tvDA = (TVDA)DataAccessFactory.GetDataAccess(DataAccessType.TV);
            return tvDA.Delete_ClipDownload(CustomerGUID, ClipdownloadKey);
        }

        public ClipDownload SelectByClipDownloadKey(long ClipDownloadKey, Guid CustomerGuid)
        {
            TVDA tvDA = (TVDA)DataAccessFactory.GetDataAccess(DataAccessType.TV);
            return tvDA.SelectByClipDownloadKey(ClipDownloadKey, CustomerGuid);
        }

        #region TAds
        public List<string> GetTAdsStations()
        {
            TVDA tvDA = (TVDA)DataAccessFactory.GetDataAccess(DataAccessType.TV);
            return tvDA.GetTAdsStations();
        }

        public List<IQAgent_TVResultsModel> TAdsSearchResults(int p_CustomerKey, Guid p_ClientGUID, string p_SearchTerm, string p_Title, DateTime? p_FromDate, DateTime? p_ToDate, List<string> p_Dma, List<string> p_Station, List<string> p_StationID, string p_Class, bool p_IsAsc, Int32 p_PageNumber, bool isallDmaAllowed, bool isallClassAllowed, bool isallStationAllowed, string p_SortColumn, ref int p_TotalResults, string pmgurl, List<int> IQTVRegion, List<int> IQTVCountry, int? p_RegionNum = null, int? p_CountryNum = null)
        {
            try
            {

                int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["TAdsPageSize"]);
                int FragSize = Convert.ToInt32(ConfigurationManager.AppSettings["FragSize"]);

                Uri PMGSearchRequestUrl = new Uri(pmgurl);
                SearchEngine searchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchRequest tvRequest = new SearchRequest();
                tvRequest.IsShowCC = false;
                tvRequest.IsSentiment = false;
                tvRequest.FragSize = FragSize;
                tvRequest.Terms = !string.IsNullOrEmpty(p_SearchTerm) ? p_SearchTerm : null;
                tvRequest.Title120 = p_Title;
                tvRequest.IncludeRegionsNum = IQTVRegion;
                tvRequest.CountryNums = IQTVCountry;
                tvRequest.PageSize = PageSize;
                tvRequest.PageNumber = p_PageNumber;
                SSPLogic sspLogic = (SSPLogic)LogicFactory.GetLogic(LogicType.SSP);
                bool p_IsAllDmaAllowed;
                bool p_IsAllClassAllowed;
                bool p_IsAllStationAllowed;

                Dictionary<string, object> dicSSP = sspLogic.GetSSPDataByClientGUID(p_ClientGUID, out p_IsAllDmaAllowed, out p_IsAllClassAllowed, out p_IsAllStationAllowed, IQTVRegion);

                var isRegionValid = false;
                if (p_RegionNum != null && p_RegionNum > 0)
                {
                    var regNum = ((List<IQ_Region>)dicSSP["IQ_Region"]).Where(r => r.Num == p_RegionNum.Value).FirstOrDefault();

                    if (regNum != null)
                    {
                        tvRequest.IncludeRegionsNum = new List<int> { regNum.Num };
                        isRegionValid = true;
                    }
                    else
                    {
                        tvRequest.IncludeRegionsNum = ((List<IQ_Region>)dicSSP["IQ_Region"]).Select(a => a.Num).ToList();
                        isRegionValid = true;
                    }
                }
                else if (dicSSP["IQ_Region"] != null && ((List<IQ_Region>)dicSSP["IQ_Region"]).ToList().Count > 0)
                {
                    tvRequest.IncludeRegionsNum = ((List<IQ_Region>)dicSSP["IQ_Region"]).Select(a => a.Num).ToList();
                    isRegionValid = true;
                }

                if (!isRegionValid)
                {
                    throw new CustomException("Invalid Region");
                }

                var isCountryValid = false;
                if (p_CountryNum != null && p_CountryNum > 0)
                {
                    var couNum = ((List<IQ_Country>)dicSSP["IQ_Country"]).Where(r => r.Num == p_CountryNum).FirstOrDefault();

                    if (couNum != null)
                    {
                        tvRequest.CountryNums = new List<int> { p_CountryNum.Value };
                        isCountryValid = true;
                    }
                    else
                    {
                        tvRequest.CountryNums = ((List<IQ_Country>)dicSSP["IQ_Country"]).Select(a => a.Num).ToList();
                        isCountryValid = true;
                    }
                }
                else if (dicSSP["IQ_Country"] != null && ((List<IQ_Country>)dicSSP["IQ_Country"]).ToList().Count > 0)
                {
                    tvRequest.CountryNums = ((List<IQ_Country>)dicSSP["IQ_Country"]).Select(a => a.Num).ToList();
                    isCountryValid = true;
                }

                if (!isCountryValid)
                {
                    throw new CustomException("Invalid Country");
                }

                bool isDmaValid = false;
                isDmaValid = p_IsAllDmaAllowed;
                if (p_Dma != null && p_Dma.Count > 0)
                {
                    if (!isallDmaAllowed)
                    {
                        List<string> lstdam = ((List<IQ_Dma>)dicSSP["IQ_Dma"]).Join(p_Dma, a => a.Name, b => b, (a, b) => b).ToList();
                        tvRequest.IQDmaName = lstdam;
                        isDmaValid = true;
                    }
                    else
                    {
                        tvRequest.IQDmaName = p_Dma;
                        isDmaValid = true;
                    }
                }
                else if (!isallDmaAllowed && dicSSP["IQ_Dma"] != null && ((List<IQ_Dma>)dicSSP["IQ_Dma"]).ToList().Count > 0)
                {
                    tvRequest.IQDmaName = ((List<IQ_Dma>)dicSSP["IQ_Dma"]).Select(a => a.Name).ToList();
                    isDmaValid = true;
                }

                if (!isDmaValid)
                {
                    throw new CustomException("Invalid Dma");
                }

                bool isAffiliateValid = false;
                isAffiliateValid = p_IsAllStationAllowed;
                if (p_Station != null && p_Station.Count > 0)
                {
                    if (!isallStationAllowed)
                    {
                        List<string> lststation = ((List<Station_Affil>)dicSSP["Station_Affil"]).Join(p_Station, a => a.Name, b => b, (a, b) => b).ToList();
                        tvRequest.StationAffil = lststation;
                        isAffiliateValid = true;
                    }
                    else
                    {
                        tvRequest.StationAffil = p_Station;
                        isAffiliateValid = true;
                    }
                }
                else if (!isallStationAllowed && dicSSP["Station_Affil"] != null && ((List<Station_Affil>)dicSSP["Station_Affil"]).ToList().Count > 0)
                {
                    tvRequest.StationAffil = ((List<Station_Affil>)dicSSP["Station_Affil"]).Select(a => a.Name).ToList();
                    isAffiliateValid = true;
                }

                if (!isAffiliateValid)
                {
                    throw new CustomException("Invalid Affiliate");
                }

                bool isStationValid = false;
                isStationValid = p_IsAllStationAllowed;
                if (p_StationID != null && p_StationID.Count > 0)
                {
                    if (!isallStationAllowed)
                    {
                        List<string> lststation = ((List<string>)dicSSP["IQ_Station"]).Join(p_StationID, a => a, b => b, (a, b) => b).ToList();
                        tvRequest.Stations = lststation;
                        isStationValid = true;
                    }
                    else
                    {
                        tvRequest.Stations = p_StationID;
                        isStationValid = true;
                    }
                }
                else if (!isallStationAllowed && dicSSP["IQ_Station"] != null && ((List<IQ_Station>)dicSSP["IQ_Station"]).ToList().Count > 0)
                {
                    tvRequest.Stations = ((List<IQ_Station>)dicSSP["IQ_Station"]).Select(s => s.IQ_Station_ID).ToList();
                    isStationValid = true;
                }

                if (!isStationValid)
                {
                    throw new CustomException("Invalid Station");
                }

                if (string.IsNullOrWhiteSpace(p_Class) && !isallClassAllowed)
                {
                    List<IQ_Class> lstIQClass = (List<IQ_Class>)dicSSP["IQ_Class"];
                    if (lstIQClass != null && lstIQClass.Count > 0)
                    {
                        List<string> lstclass = new List<string>();
                        foreach (IQ_Class iqclass in lstIQClass)
                        {
                            lstclass.Add(iqclass.Num);
                        }
                        tvRequest.IQClassNum = lstclass;
                    }
                }


                if (!string.IsNullOrWhiteSpace(p_SortColumn))
                {
                    tvRequest.SortFields = p_SortColumn;
                    if (!p_IsAsc)
                    {
                        tvRequest.SortFields = tvRequest.SortFields + "-";
                    }

                }
                else
                {
                    if (p_IsAsc == true)
                    {
                        tvRequest.SortFields = "datetime";

                    }
                    else
                    {
                        tvRequest.SortFields = "datetime-";
                    }
                }


                tvRequest.ClientGuid = p_ClientGUID;
                if (p_FromDate != null)
                {
                    tvRequest.RLStationStartDate = p_FromDate;
                }
                if (p_ToDate != null)
                {
                    tvRequest.RLStationEndDate = p_ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                if (!string.IsNullOrEmpty(p_Class))
                {
                    List<string> tempClass = new List<string>();
                    tempClass.Add(p_Class);

                    tvRequest.IQClassNum = tempClass; // !string.IsNullOrEmpty(p_Dma) ? p_Dma.ToList() : null;
                }

                string CommaSepratedDmaList = string.Empty;
                string CommaSepratedClassList = string.Empty;
                string CommaSepratedStationList = string.Empty;

                if (tvRequest.IQDmaName != null)
                {
                    CommaSepratedDmaList = string.Join(",", tvRequest.IQDmaName.ToArray()); ;
                }

                if (tvRequest.IQClassNum != null)
                {
                    CommaSepratedClassList = string.Join(",", tvRequest.IQClassNum.ToArray());
                }

                if (tvRequest.StationAffil != null)
                {
                    CommaSepratedStationList = string.Join(",", tvRequest.StationAffil.ToArray());
                }


                SearchResult tvResult = searchEngine.Search(tvRequest);

                #region insert pmg log
                string _Responce = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(tvResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Responce = _XmlDocument.InnerXml;
                            _Responce = _Responce.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Responce = null;
                }

                UtilityLogic.InsertPMGSearchLog(p_CustomerKey, tvRequest.Terms, tvRequest.Title120, CommaSepratedDmaList, CommaSepratedStationList, CommaSepratedClassList, tvRequest.PageNumber, tvRequest.PageSize, tvRequest.MaxHighlights, tvRequest.StartDate, tvRequest.EndDate, IQMedia.Shared.Utility.CommonFunctions.SearchType.TAds.ToString(), _Responce);


                #endregion

                p_TotalResults = tvResult.TotalHitCount;
                List<IQAgent_TVResultsModel> listOfTVResult = new List<IQAgent_TVResultsModel>();

                if (tvResult != null && tvResult.TotalHitCount > 0)
                {

                    XDocument xDoc = new XDocument(new XElement("list"));
                    foreach (Hit hit in tvResult.Hits)
                    {
                        IQAgent_TVResultsModel iqagent = new IQAgent_TVResultsModel();

                        iqagent.StationID = hit.StationId;
                        iqagent.Title120 = hit.Title120;
                        iqagent.Market = hit.Market;
                        iqagent.Hits = hit.TotalNoOfOccurrence;
                        if (!string.IsNullOrEmpty(hit.RLStationDateTime))
                        {
                            iqagent.Date = Convert.ToDateTime(hit.RLStationDateTime);
                        }
                        iqagent.RL_VideoGUID = new Guid(hit.Guid);
                        iqagent.IQ_CC_Key = hit.Iqcckey;
                        iqagent.TimeZone = hit.ClipTimeZone;

                        listOfTVResult.Add(iqagent);

                        xDoc.Root.Add(new XElement("item", new XAttribute("iq_cc_key", hit.Iqcckey), new XAttribute("iq_dma", hit.IQDmaNum)));


                    }
                    IQNielsenDA iqNielsenDA = (IQNielsenDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNielsen);
                    List<NielsenDataModel> listOfNielsen = iqNielsenDA.GetTAdsNielsenDataByXML(xDoc, p_ClientGUID);

                    foreach (var tvr in listOfTVResult)
                    {
                        NielsenDataModel ns = listOfNielsen.Where(nso => String.Compare(nso.IQ_CC_Key, tvr.IQ_CC_Key, true).Equals(0)).FirstOrDefault();
                        if (ns != null)
                        {
                            tvr.AUDIENCE = ns.Audience;
                            tvr.SQAD_SHAREVALUE = ns.IQAdsharevalue;
                            tvr.IsActualNielsen = ns.IsActualNielsen;
                        }
                    }

                }

                return listOfTVResult;
            }
            catch (IQMedia.Shared.Utility.CustomException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<IQTAdsResultModel> GetTadsResultByIQCCKey(string IQ_CC_Key)
        {
            TVDA tvDA = (TVDA)DataAccessFactory.GetDataAccess(DataAccessType.TV);
            return tvDA.GetTadsResultByIQCCKey(IQ_CC_Key);
        }

        public string TAdsHighLineChart(List<IQTAdsResultModel> lstAds_Results)
        {
            try
            {

                var yAxisData = new List<HighChartDatum>();
                var xAxisData = new List<string>();

                double numOfTicks = (60 * 60) + 1;
                int yAxis = 1;

                foreach (var result in lstAds_Results)
                {
                    int counter = 0;
                    foreach (var hit in result.Hits)
                    {
                        while (counter < hit.startOffset)
                        {
                            var datum = new HighChartDatum();
                            datum.y = null;
                            yAxisData.Add(datum);

                            xAxisData.Add(counter.ToString());

                            counter++;
                        }
                        while (counter <= hit.endOffset)
                        {
                            var datum = new HighChartDatum();
                            datum.y = yAxis;
                            yAxisData.Add(datum);

                            xAxisData.Add(counter.ToString());

                            counter++;
                        }
                    }
                    while (counter < numOfTicks)
                    {
                        var datum = new HighChartDatum();
                        datum.y = null;
                        yAxisData.Add(datum);

                        xAxisData.Add(counter.ToString());

                        counter++;
                    }
                    yAxis ++;
                }


                Series series = new Series();
                series.name = "AD";
                series.data = yAxisData;

                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();

                // set chart title and title style
                highLineChartOutput.title = new Title()
                {
                    text = "AD Data",
                    x = 0,
                    y = 10,
                    align = "left",
                    style = new HStyle
                    {
                        color = "#555555",
                        fontWeight = "bold",
                        fontFamily = "Verdana",
                        fontSize = "13px"
                    }
                };
                //highLineChartOutput.subtitle = new Subtitle() { text = p_GraphStructureModel.ChartSubTitle, x = 80, y = 20, align = "left", };

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 labels will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(xAxisData.Count()) / 20)),
                    tickmarkPlacement = "off",
                    //tickPixelInterval = 50,
                    categories = xAxisData,
                    labels = new labels()
                    {
                        rotation = 0,
                        formatter = "FormatTime"
                    },
                    tickWidth = 1
                };

                highLineChartOutput.yAxis = new List<YAxis>() { 
                    new YAxis{
                        title = new Title2(){ text = "" },
                        labels = new labels()
                        {
                            enabled = false
                        }
                    }
                };


                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { formatter = "tooltipFormat", shared = true, crosshairs = false };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { enabled = false };

                // set chart with height and width
                highLineChartOutput.hChart = new HChart() { height = lstAds_Results.Count * 120, zoomType = "x", width = 980};

                // set plot options and click event for series points (which will again assigned in JS as this is string value)
                // legendItemClick event to show / hide column chart series on line legend click
                highLineChartOutput.plotOption = new PlotOptions()
                {
                    column = null,
                    series = new PlotSeries()
                    {
                        events = new PlotEvents()
                        {
                            hide = "HandleSeriesHide",
                            show = "HandleSeriesShow"
                        },
                        lineWidth = 6,
                        marker = new PlotMarker
                        {
                            enabled = false,
                            radius = 6
                        },
                        states = new PlotSeriesStates()
                        {
                            hover = new SeriesState()
                            {
                                halo = new PlotStateHalo()
                                {
                                    size = 2
                                }
                            }
                        },
                        cursor = "pointer",
                        turboThreshold = 40000,
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "ChartClick",
                                mouseOver = "ChartHoverManage",
                                mouseOut = "ChartHoverOutManage"
                            }
                        }
                    }
                };

                // start to set series of data for multiline search term chart 
                List<Series> lstSeries = new List<Series>();

                lstSeries.Add(series);

                highLineChartOutput.series = lstSeries;

                string jsonResult = CommonFunctions.SearializeJson(highLineChartOutput);
                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
