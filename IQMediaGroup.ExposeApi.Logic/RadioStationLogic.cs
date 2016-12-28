using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Data.Objects;
using System.Configuration;

namespace IQMediaGroup.ExposeApi.Logic
{
    public class RadioStationLogic : BaseLogic, ILogic
    {
        public List<RadioStation> GetRadioStation()
        {
            try
            {
                List<RadioStation> RadioStation = Context.GetRadioStation().ToList().Select(RadioStationObj => new RadioStation
                {
                    DmaName = RadioStationObj.DmaName,
                    DmaNum = RadioStationObj.DmaNum,
                    StationID = RadioStationObj.StationID
                }
                                                                                                ).ToList<RadioStation>();
                return RadioStation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RadioMediaOutput GetRadioRawMedia(RadioMediaInput p_RadioRawMediaInput)
        {
            try
            {
                string IframeURL = "http://qa.iqmediacorp.com/IFrameRawMedia/Default.aspx";

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["IframeURL"]))
                {
                    IframeURL = ConfigurationManager.AppSettings["IframeURL"];
                }

                ObjectParameter TotalRecordsCount = new ObjectParameter("TotalRecordsCount", typeof(Int32));

                StringBuilder IQCCKey = new StringBuilder();

                DateTime FromDate = new DateTime(Convert.ToDateTime(p_RadioRawMediaInput.FromDateTime).Year, Convert.ToDateTime(p_RadioRawMediaInput.FromDateTime).Month, Convert.ToDateTime(p_RadioRawMediaInput.FromDateTime).Day);
                DateTime ToDate = new DateTime(Convert.ToDateTime(p_RadioRawMediaInput.ToDateTime).Year, Convert.ToDateTime(p_RadioRawMediaInput.ToDateTime).Month, Convert.ToDateTime(p_RadioRawMediaInput.ToDateTime).Day);

                int FromTime = Convert.ToDateTime(p_RadioRawMediaInput.FromDateTime).Hour;
                int ToTime = Convert.ToDateTime(p_RadioRawMediaInput.ToDateTime).Hour;

                int IndexFromTime = FromTime;
                int IndexToTime = ToTime;
                DateTime TempDateTime;

                string StationIDs = null;
                if (p_RadioRawMediaInput.RadioStationList != null && p_RadioRawMediaInput.RadioStationList.Count > 0)
                {
                    StationIDs = string.Join(",", p_RadioRawMediaInput.RadioStationList.Select(RadioStationObj => "'" + RadioStationObj.StationID + "'").ToArray());
                }

                List<RadioStationDB> ListOfRadioStation = Context.GetRadioStationWithTime(StationIDs).ToList().Select(RadioStationObj => new RadioStationDB()
                {
                    StationID = RadioStationObj.StationID,
                    gmt_adj = RadioStationObj.gmt_adj,
                    dst_adj = RadioStationObj.dst_adj
                }).ToList<RadioStationDB>();

                for (DateTime IndexFromDate = FromDate; IndexFromDate <= ToDate; )
                {
                    if (IndexFromDate == FromDate)
                    {
                        IndexFromTime = FromTime;
                    }
                    else
                    {
                        IndexFromTime = 0;
                    }

                    if (IndexFromDate == ToDate)
                    {
                        IndexToTime = ToTime;
                    }
                    else
                    {
                        IndexToTime = 23;
                    }

                    while (IndexFromTime <= IndexToTime)
                    {
                        TempDateTime = new DateTime(IndexFromDate.Year, IndexFromDate.Month, IndexFromDate.Day, IndexFromTime, 0, 0);

                        if ((DateTime.UtcNow - DateTime.Now).Hours == 5)
                        {
                            var _IQCCKey = from _RadioStation in ListOfRadioStation
                                           select (_RadioStation.StationID + "_" + TempDateTime.AddHours((-1) * (Convert.ToDouble(_RadioStation.gmt_adj))).ToString("yyyyMMdd") + "_" + TempDateTime.AddHours((-1) * Convert.ToInt32(_RadioStation.gmt_adj)).Hour.ToString().PadLeft(2, '0') + "00");


                            IQCCKey.Append(string.Join(",", (new List<string>(_IQCCKey).Select(StrIQCCKey => "'" + StrIQCCKey + "'"))));

                        }
                        else
                        {
                            var _IQCCKey = from _RadioStations in ListOfRadioStation
                                           select (_RadioStations.StationID + "_" + TempDateTime.AddHours(((-1) * (Convert.ToDouble(_RadioStations.gmt_adj))) - Convert.ToDouble(_RadioStations.dst_adj)).ToString("yyyyMMdd") + "_" + TempDateTime.AddHours(((-1) * Convert.ToInt32(_RadioStations.gmt_adj)) - Convert.ToDouble(_RadioStations.dst_adj)).Hour.ToString().PadLeft(2, '0') + "00");

                            IQCCKey.Append(string.Join(",", (new List<string>(_IQCCKey).Select(StrIQCCKey => "'" + StrIQCCKey + "'"))));
                        }

                        IndexFromTime++;
                    }

                    IndexFromDate = IndexFromDate.AddDays(1);
                }

                string SortFields = GenerateSortField(p_RadioRawMediaInput);

                List<RadioMedia> ListOfRadioRawMedia = Context.GetRadioRawMedia(IQCCKey.ToString(),
                                                                                    p_RadioRawMediaInput.PageNumber,
                                                                                    p_RadioRawMediaInput.PageSize,
                                                                                    SortFields,
                                                                                    TotalRecordsCount
                                                                                  ).ToList().Select(RadioRawMediaObj => new RadioMedia()
                                                                                  {
                                                                                      DateTime = Convert.ToDateTime(RadioRawMediaObj.DateTime),
                                                                                      DmaName = RadioRawMediaObj.DmaName,
                                                                                      RadioMediaID = RadioRawMediaObj.RawMediaID,
                                                                                      StationID = RadioRawMediaObj.StationID,
                                                                                      URL = IframeURL + "?RawMediaID=" + RadioRawMediaObj.RawMediaID
                                                                                  }
                                                                                                    ).ToList<RadioMedia>();

                RadioMediaOutput RadioRawMediaOutput = new RadioMediaOutput();

                RadioRawMediaOutput.RadioMediaList = ListOfRadioRawMedia;

                /* Start check for next page */

                if (Convert.ToInt32(TotalRecordsCount.Value) > (p_RadioRawMediaInput.PageNumber * p_RadioRawMediaInput.PageSize))
                {
                    RadioRawMediaOutput.HasNextPage = true;
                }
                else
                {
                    RadioRawMediaOutput.HasNextPage = false;
                }

                /* Stop check for next page */


                RadioRawMediaOutput.TotalResults = Convert.ToInt32(TotalRecordsCount.Value);

                return RadioRawMediaOutput;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string GenerateSortField(RadioMediaInput RadioRawMediaInput)
        {
            try
            {
                IDictionary<string, string> RadioStationSortFields = new Dictionary<string, string>();

                RadioStationSortFields.Add("datetime", "datetime");
                RadioStationSortFields.Add("datetime-", "datetime-");
                RadioStationSortFields.Add("guid", "RL_GUID");
                RadioStationSortFields.Add("guid-", "RL_GUID-");
                RadioStationSortFields.Add("station", "RL_GUIDS.RL_Station_ID");
                RadioStationSortFields.Add("station-", "RL_GUIDS.RL_Station_ID-");
                RadioStationSortFields.Add("market", "Dma_Name");
                RadioStationSortFields.Add("market-", "Dma_Name-");

                StringBuilder InputSortFields = new StringBuilder();

                string[] RadioStationSortFieldArray = RadioRawMediaInput.SortField.Split(new char[] { ',' });

                foreach (string SortField in RadioStationSortFieldArray)
                {
                    if (RadioStationSortFields.ContainsKey(SortField.ToLower()))
                    {
                        InputSortFields.Append(RadioStationSortFields[SortField.ToLower()] + ",");
                    }
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
    }
}
