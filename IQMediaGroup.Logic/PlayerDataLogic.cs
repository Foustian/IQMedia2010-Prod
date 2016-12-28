using System;
using IQMediaGroup.Domain;
using IQMediaGroup.Logic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using PMGSearch;
using System.Collections.Generic;
using IQMediaGroup.Common;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.Logic
{
    public class PlayerDataLogic : BaseLogic, ILogic
    {
        public Guid? GetVideoGuidByiQAgentiFrameID(Guid iQAgentiFrameGUID)
        {
            Guid? rlVideoGuid = Context.GetVideoGuidByiQAgentiFrameID(iQAgentiFrameGUID).FirstOrDefault();
            return rlVideoGuid;
        }

        public dynamic GetPlayerData(Guid? p_GUID, string p_Type, string p_PmgUrl, string p_RadioSolrURL="")
        {
            try
            {
                switch (p_Type.Trim().ToLower())
                {
                    case "rawmedia":
                        //PlayerRawMediaData PlayerRawMediaData = Context.GetPlayerDataForRawMedia(p_GUID).ElementAtOrDefault(0);
                        PlayerRawMediaData PlayerRawMediaData = (PlayerRawMediaData)GetPlayerDataFromPMG(p_GUID, true, p_PmgUrl);

                        var isRadio = false;

                        if (PlayerRawMediaData == null)
                        {
                            isRadio = true;
                            PlayerRawMediaData = (PlayerRawMediaData)GetPlayerDataForRadio(p_GUID, p_RadioSolrURL);
                        }

                        if (!isRadio)
                        {
                            try
                            {
                                Dictionary<string, string> stationDetail = Context.GetStationAffiliateNCallSign(PlayerRawMediaData.StationID);

                                PlayerRawMediaData.StationAffiliate = stationDetail["Affiliate"];
                                PlayerRawMediaData.StationCallSign = stationDetail["CallSign"];
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Error(ex);
                            } 
                        }

                        return PlayerRawMediaData;

                    case "clip":

                        //PlayerClipData PlayerClipData = Context.GetPlayerDataForClip(p_GUID).ElementAtOrDefault(0);
                        PlayerRawMediaData PlayerClipData = (PlayerRawMediaData)GetPlayerDataFromPMG(p_GUID, false, p_PmgUrl);
                        return PlayerClipData;

                    case "ugc":
                        PlayerUGCRawMediaData PlayerUGCRawMediaData = Context.GetPlayerDataForIQUGCArchive(p_GUID).ElementAtOrDefault(0);

                        return PlayerUGCRawMediaData;

                    case "iqagentiframe":
                        Guid rmGUID = (Guid)Context.GetVideoGuidByiQAgentiFrameID(p_GUID).FirstOrDefault();
                        PlayerRawMediaData playerRawMediaData = (PlayerRawMediaData)GetPlayerDataFromPMG(rmGUID, true, p_PmgUrl);
                        if (playerRawMediaData == null)
                        {
                            playerRawMediaData = (PlayerRawMediaData)GetPlayerDataForRadio(rmGUID, p_RadioSolrURL);
                        }
                        return playerRawMediaData;

                    default:

                        return "No Results Found.";
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private object GetPlayerDataForRadio(Guid? p_GUID, string p_SolrURL)
        {
            object result = new object();

            Guid? RawMediaGUID;

            if (Context.GetRecordfileByGuid(p_GUID).SingleOrDefault() != null)
            {
                RawMediaGUID = p_GUID;
            }
            else
            {
                RawMediaGUID = Context.GetRecordfileGUIDbyClipGUID(p_GUID).FirstOrDefault();
            }

            if (RawMediaGUID.HasValue)
            {
                Uri solrURL = new Uri(p_SolrURL);

                IQRadioSearch.SearchRequest sr = new IQRadioSearch.SearchRequest();

                sr.GUIDList = new List<Guid> { RawMediaGUID.Value };
                sr.PageSize = 1;

                IQRadioSearch.SearchEngine se = new IQRadioSearch.SearchEngine(solrURL);

                Dictionary<string, object> dictResult = se.Search(sr,CustomSolrFl:ConfigurationManager.AppSettings["IQRadioSolrFl"]);
                
                IQRadioSearch.SearchResult sresult = new IQRadioSearch.SearchResult();
                if (dictResult.ContainsKey("SearchResult"))
                    sresult = (IQRadioSearch.SearchResult)dictResult["SearchResult"];

                if (sresult.Status == 0 && sresult.Hits != null && sresult.Hits.Count > 0)
                {
                    IQRadioSearch.Hit hit = sresult.Hits[0];

                    var playerData = new PlayerRawMediaData { IQ_Dma_Name = hit.Market, IQ_Local_Air_Date = hit.DateTime, StationID = hit.StationID, TimeZone = hit.TimeZone };

                    result = playerData;
                }
                else
                {
                    if (sresult.Status < 0)
                    {
                        throw new Exception(sresult.Message);
                    }
                    else
                    {
                        throw new CustomException("Radio Clip doesn't exit : " + p_GUID.Value);
                    }
                }
            }

            return result;
        }

        public object GetPlayerDataFromPMG(Guid? p_Guid, bool _IsRawMedia, string p_PmgUrl)
        {
            object _Returnobject = new object();

            try
            {

                Guid? RawMediaGUID;

                if (Context.GetRecordfileByGuid(p_Guid).SingleOrDefault()!=null)
                {
                    RawMediaGUID = p_Guid;
                }
                else
                {
                    RawMediaGUID = Context.GetRecordfileGUIDbyClipGUID(p_Guid).FirstOrDefault();
                }

                if (RawMediaGUID.HasValue)
                {
                    Uri PMGSearchRequestUrl = new Uri(p_PmgUrl);

                    int _PMGMaxHighlights = 20;
                    if (ConfigurationManager.AppSettings["PMGMaxHighlights"] != null)
                    {
                        int.TryParse(ConfigurationManager.AppSettings["PMGMaxHighlights"], out _PMGMaxHighlights);
                    }

                    SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                    SearchRequest _SearchRequest = new SearchRequest();

                    _SearchRequest.GuidList = Convert.ToString(RawMediaGUID);

                    if (_IsRawMedia)
                    {
                        _SearchRequest.IsTitle120List = true;
                    }

                    _SearchRequest.MaxHighlights = _PMGMaxHighlights;

                    SearchResult _SearchResult = _SearchEngine.Search(_SearchRequest);

                    var RawMediaList = (from _Hit in _SearchResult.Hits
                                        select new PlayerRawMediaData
                                        {
                                            IQ_Dma_Name = _Hit.Market,
                                            IQ_Dma_Num = _Hit.IQDmaNum,
                                            IQ_Local_Air_Date = new DateTime(_Hit.Timestamp.Year, _Hit.Timestamp.Month, _Hit.Timestamp.Day, (_Hit.Hour / 100), 0, 0),
                                            Title120s = _Hit.ListOfTitle120,
                                            IQ_Start_Points = _Hit.ListOfIQStartPoint,
                                            IQ_Start_Minutes = _Hit.ListOfIQStartMinute,
                                            StationID = _Hit.StationId,
                                            TimeZone = _Hit.ClipTimeZone
                                        });
                    if (RawMediaList != null)
                    {
                        _Returnobject = RawMediaList.FirstOrDefault();
                    }
                }
                else
                {
                    throw new CustomException("Clip doesn't exist.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _Returnobject;
        }
    }
}