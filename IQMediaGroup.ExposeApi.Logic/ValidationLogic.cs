using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Configuration;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.ExposeApi.Logic
{
    public class ValidationLogic : BaseLogic, ILogic
    {
        private static string[] RawMediaSortField = { "datetime", "datetime-", "guid", "guid-", "station", "station-", "market", "market-" };
        private static string[] RadioRawMediaSortField = { "datetime", "datetime-", "guid", "guid-", "station", "station-", "market", "market-" };
        private static string[] RawMediaTimeZone = { "all", "cst", "mst", "pst", "est" };

        public bool ValidateRawMediaInput(RawMediaInput p_RawMediaInput, out string errorMessage)
        {
            try
            {

                errorMessage = string.Empty;

                /* Start SessionID Validation */

                if (string.IsNullOrEmpty(p_RawMediaInput.SessionID))
                {
                    errorMessage = Config.ConfigSettings.MessageSettings.SessionIDMissingMessage;
                    return false;
                }

                /* Stop SessionID Validation */

                /* Start Page Validation */

                if (p_RawMediaInput.PageNumber == null)
                {
                    p_RawMediaInput.PageNumber = 1;
                }

                if (p_RawMediaInput.PageNumber <= 0)
                {
                    errorMessage = Config.ConfigSettings.MessageSettings.InvalidPageNumberMessage;
                    return false;
                }

                int MaxPageSize = 10;

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxPageSize"]) && Convert.ToInt32(ConfigurationManager.AppSettings["MaxPageSize"]) > 0)
                {
                    int.TryParse(ConfigurationManager.AppSettings["MaxPageSize"], out MaxPageSize);
                }

                if (p_RawMediaInput.PageSize == null)
                {
                    p_RawMediaInput.PageSize = 10;
                }

                if (p_RawMediaInput.PageSize > MaxPageSize || p_RawMediaInput.PageSize <= 0)
                {
                    errorMessage = Config.ConfigSettings.MessageSettings.PageSizeLimitExceedMessage.Replace("{limit}", ConfigurationManager.AppSettings["MaxPageSize"]);
                    return false;
                }

                /* Stop Page Validation */

                /* Start SortFields Validation */

                if (!string.IsNullOrEmpty(p_RawMediaInput.SortField))
                {
                    string[] InputSortFields = p_RawMediaInput.SortField.Split(new char[] { ',' });

                    foreach (string SortField in InputSortFields)
                    {
                        if (!RawMediaSortField.Contains(SortField.ToLower()))
                        {
                            errorMessage = Config.ConfigSettings.MessageSettings.InvalidSortFieldMessage.Replace("{allowed}", string.Join("\r\n", RawMediaSortField));
                            return false;
                        }
                    }
                }
                else
                {
                    p_RawMediaInput.SortField = ConfigurationManager.AppSettings["RawMediaDefaultSort"];
                }

                /* Stop SortFields Validation */

                /* Start Params Validation */

                //if (p_RawMediaInput.IQ_Cat_Set == null && p_RawMediaInput.IQ_Cat_Set.Count <= 0)
                //{
                //    return false;
                //}

                //if (p_RawMediaInput.IQ_ClassSet == null && p_RawMediaInput.IQ_ClassSet.Count <= 0)
                //{
                //    return false;
                //}

                //if (p_RawMediaInput.IQ_DmaSet == null && p_RawMediaInput.IQ_DmaSet.Count <= 0)
                //{
                //    return false;
                //}

                //if (p_RawMediaInput.Station_AffilSet == null && p_RawMediaInput.Station_AffilSet.Count <= 0)
                //{
                //    return false;
                //}

                /* Stop Params Validation */

                /* Start Date Validation */


                DateTime TempDateTime = DateTime.Now;

                if (p_RawMediaInput.FromDateTime == null)
                {
                    p_RawMediaInput.FromDateTime = TempDateTime.AddDays(-1).Date;
                }

                if (p_RawMediaInput.ToDateTime == null)
                {
                    p_RawMediaInput.ToDateTime = new DateTime(TempDateTime.Year, TempDateTime.Month, TempDateTime.Day, DateTime.Now.Hour - 1, 0, 0);
                }

                if (p_RawMediaInput.ToDateTime > DateTime.Now)
                {
                    p_RawMediaInput.ToDateTime = DateTime.Now;
                }

                if (p_RawMediaInput.FromDateTime > DateTime.Now || p_RawMediaInput.FromDateTime > p_RawMediaInput.ToDateTime)
                {
                    errorMessage = Config.ConfigSettings.MessageSettings.InvalidDateRangeMessage;
                    return false;
                }

                /* Stop Date Validation */

                /* start time zone validation */

                if (!string.IsNullOrEmpty(p_RawMediaInput.TimeZone))
                {
                    if (RawMediaTimeZone.Contains(p_RawMediaInput.TimeZone.ToLower()))
                    {

                    }
                    else
                    {
                        errorMessage = Config.ConfigSettings.MessageSettings.InvalidTimeZoneMessage.Replace("{allowed}", string.Join("\r\n", RawMediaTimeZone));
                        return false;
                    }
                }
                else
                {
                    errorMessage = Config.ConfigSettings.MessageSettings.TimeZoneMissingMessage;
                    return false;
                }

                /* stop time zone validation */

                return true;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public bool ValidateRadioRawMediaInput(RadioMediaInput p_RadioRawMediaInput, out string errorMessage)
        {
            try
            {
                errorMessage = string.Empty;

                /* Start SessionID Validation */

                if (string.IsNullOrEmpty(p_RadioRawMediaInput.SessionID))
                {
                    errorMessage = Config.ConfigSettings.MessageSettings.SessionIDMissingMessage;
                    return false;
                }

                /* Stop SessionID Validation */

                /* Start Page Validation */

                if (p_RadioRawMediaInput.PageNumber == null)
                {
                    p_RadioRawMediaInput.PageNumber = 1;
                }

                if (p_RadioRawMediaInput.PageNumber <= 0)
                {
                    errorMessage = Config.ConfigSettings.MessageSettings.InvalidPageNumberMessage;
                    return false;
                }

                int MaxPageSize = 10;

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxPageSize"]) && Convert.ToInt32(ConfigurationManager.AppSettings["MaxPageSize"]) > 0)
                {
                    int.TryParse(ConfigurationManager.AppSettings["MaxPageSize"], out MaxPageSize);
                }

                if (p_RadioRawMediaInput.PageSize == null)
                {
                    p_RadioRawMediaInput.PageSize = 10;
                }

                if (p_RadioRawMediaInput.PageSize > MaxPageSize || p_RadioRawMediaInput.PageSize <= 0)
                {
                    errorMessage = Config.ConfigSettings.MessageSettings.PageSizeLimitExceedMessage.Replace("{limit}", ConfigurationManager.AppSettings["MaxPageSize"]);
                    return false;
                }

                /* Stop Page Validation */

                /* Start SortFields Validation */

                if (!string.IsNullOrEmpty(p_RadioRawMediaInput.SortField))
                {
                    string[] InputSortFields = p_RadioRawMediaInput.SortField.Split(new char[] { ',' });

                    foreach (string SortField in InputSortFields)
                    {
                        if (!RadioRawMediaSortField.Contains(SortField.ToLower()))
                        {
                            errorMessage = Config.ConfigSettings.MessageSettings.InvalidSortFieldMessage.Replace("{allowed}", string.Join("\r\n", RadioRawMediaSortField));
                            return false;
                        }
                    }
                }
                else
                {
                    p_RadioRawMediaInput.SortField = ConfigurationManager.AppSettings["RadioRawMediaDefaultSort"];
                }

                /* Stop SortFields Validation */

                /* Start Date Validation */

                DateTime TempDateTime = DateTime.Now;

                if (p_RadioRawMediaInput.FromDateTime == null)
                {
                    p_RadioRawMediaInput.FromDateTime = TempDateTime.AddDays(-1).Date;
                }

                if (p_RadioRawMediaInput.ToDateTime == null)
                {
                    p_RadioRawMediaInput.ToDateTime = new DateTime(TempDateTime.Year, TempDateTime.Month, TempDateTime.Day, DateTime.Now.Hour - 1, 0, 0);
                }

                if (p_RadioRawMediaInput.ToDateTime > DateTime.Now)
                {
                    p_RadioRawMediaInput.ToDateTime = DateTime.Now;
                }

                if (p_RadioRawMediaInput.FromDateTime > DateTime.Now || p_RadioRawMediaInput.FromDateTime > p_RadioRawMediaInput.ToDateTime)
                {
                    errorMessage = Config.ConfigSettings.MessageSettings.InvalidDateRangeMessage;
                    return false;
                }

                /* Stop Date Validation */

                return true;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public bool ValidateMediaPlayerURLInput(MediaPlayerURLInput p_MediaPlayerURLInput, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(p_MediaPlayerURLInput.SessionID))
            {
                errorMessage = Config.ConfigSettings.MessageSettings.SessionIDMissingMessage;
                return false;
            }

            if (p_MediaPlayerURLInput.SeqID == null)
            {
                if (string.IsNullOrWhiteSpace(p_MediaPlayerURLInput.StationID))
                {
                    errorMessage = Config.ConfigSettings.MessageSettings.StationIDOrSeqIDMissingMessage;
                    return false;
                }

                if (p_MediaPlayerURLInput.DateTime == null)
                {
                    errorMessage = Config.ConfigSettings.MessageSettings.DateTimeMissingMessage;
                    return false;
                }
            }

            return true;
        }

        public bool ValidateArchiveInput(ArchiveInput p_ArchiveInput, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrEmpty(p_ArchiveInput.SessionID))
            {
                errorMessage = Config.ConfigSettings.MessageSettings.SessionIDMissingMessage;
                return false;
            }

            /* Stop SessionID Validation */

            /* Start Page Validation */


            int MaxPageSize = 10;

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxPageSize"]) && Convert.ToInt32(ConfigurationManager.AppSettings["MaxPageSize"]) > 0)
            {
                int.TryParse(ConfigurationManager.AppSettings["MaxPageSize"], out MaxPageSize);
            }

            if (p_ArchiveInput.Rows == null)
            {
                p_ArchiveInput.Rows = 10;
            }

            if (p_ArchiveInput.Rows > MaxPageSize || p_ArchiveInput.Rows <= 0)
            {
                errorMessage = Config.ConfigSettings.MessageSettings.RowsLimitExceedMessage.Replace("{limit}", ConfigurationManager.AppSettings["MaxPageSize"]);
                return false;
            }

            if (p_ArchiveInput.FromDateTime != null && p_ArchiveInput.ToDateTime == null)
            {
                errorMessage = Config.ConfigSettings.MessageSettings.ToDateTimeMissingMessage;
                return false;
            }

            if (p_ArchiveInput.ToDateTime != null && p_ArchiveInput.FromDateTime == null)
            {
                errorMessage = Config.ConfigSettings.MessageSettings.FromDateTimeMissingMessage;
                return false;
            }

            if (p_ArchiveInput.FromDateTime > DateTime.Now || p_ArchiveInput.FromDateTime > p_ArchiveInput.ToDateTime)
            {
                errorMessage = Config.ConfigSettings.MessageSettings.InvalidDateRangeMessage;
                return false;
            }

            if (!string.IsNullOrEmpty(p_ArchiveInput.MediaType) && string.IsNullOrEmpty(CommonFunctions.GetValueFromDescription<IQMediaGroup.Common.Util.CommonConstants.SubMediaType>(p_ArchiveInput.MediaType)))
            {
                errorMessage = Config.ConfigSettings.MessageSettings.InvalidMediaType;
                return false;
            }

            return true;
        }

        public bool ValidateTVAgentDaySummaryDuration(TVAgentDaySummaryInput p_TVAgentDaySummaryInput, out string errorMessage)
        {
            errorMessage = string.Empty;


            if (p_TVAgentDaySummaryInput.FromDate == null)
            {
                errorMessage = Config.ConfigSettings.MessageSettings.FromDateMissingMessage;
                return false;
            }

            if (p_TVAgentDaySummaryInput.ToDate == null)
            {
                errorMessage = Config.ConfigSettings.MessageSettings.ToDateMissingMessage;
                return false;
            }

            if (p_TVAgentDaySummaryInput.FromDate > DateTime.Now.Date || p_TVAgentDaySummaryInput.FromDate > p_TVAgentDaySummaryInput.ToDate)
            {
                errorMessage = Config.ConfigSettings.MessageSettings.InvalidDateRangeMessage;
                return false;
            }

            TimeSpan DaySummaryDuration = (TimeSpan)(p_TVAgentDaySummaryInput.ToDate - p_TVAgentDaySummaryInput.FromDate);
            if (DaySummaryDuration.Days > Convert.ToInt32(ConfigurationManager.AppSettings["MaxDaySummaryDuration"]))
            {
                errorMessage = Config.ConfigSettings.MessageSettings.MaxDaySummaryDurationLimitExceedMessage.Replace("{limit}", (Convert.ToInt32(Convert.ToString(ConfigurationManager.AppSettings["MaxDaySummaryDuration"])) + 1).ToString());
                return false;
            }

            return true;
        }

        public bool ValidateTVAgentHourSummaryDuration(TVAgentHourSummaryInput p_TVAgentHourSummaryInput, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (p_TVAgentHourSummaryInput.FromDateTime == null)
            {
                errorMessage = Config.ConfigSettings.MessageSettings.FromDateTimeMissingMessage;
                return false;
            }

            if (p_TVAgentHourSummaryInput.ToDateTime == null)
            {
                errorMessage = Config.ConfigSettings.MessageSettings.ToDateTimeMissingMessage;
                return false;
            }

            if (p_TVAgentHourSummaryInput.FromDateTime > DateTime.Now || p_TVAgentHourSummaryInput.FromDateTime > p_TVAgentHourSummaryInput.ToDateTime)
            {
                errorMessage = Config.ConfigSettings.MessageSettings.InvalidDateRangeMessage;
                return false;
            }

            TimeSpan HourSummaryDuration = (TimeSpan)(p_TVAgentHourSummaryInput.ToDateTime - p_TVAgentHourSummaryInput.FromDateTime);
            if (HourSummaryDuration.TotalHours >= Convert.ToInt32(ConfigurationManager.AppSettings["MaxHourSummaryDuration"]))
            {
                errorMessage = Config.ConfigSettings.MessageSettings.MaxHourSummaryDurationLimitExceedMessage.Replace("{limit}", ConfigurationManager.AppSettings["MaxHourSummaryDuration"]);
                return false;
            }

            return true;
        }

        public bool ValidateTVAgentInput(Guid p_ClientGUID, out string errorMessage, CreateTVAgentInput p_CreateTVAgentInput = null, UpdateTVAgentInput p_UpdateTVAgentInput = null)
        {
            try
            {

                errorMessage = string.Empty;
                var statskedprogData = new StatskedprogData();
                StatskedprogLogic _StatskedprogLogic = (StatskedprogLogic)LogicFactory.GetLogic(LogicType.StatskedprogData);
                statskedprogData = _StatskedprogLogic.GetStatskedprogData(p_ClientGUID);

                dynamic tvAgentInput;
                if (p_CreateTVAgentInput != null)
                {
                    tvAgentInput = p_CreateTVAgentInput;
                }
                else
                {
                    tvAgentInput = p_UpdateTVAgentInput;
                    if (tvAgentInput.SRID != null)
                    {
                        if (tvAgentInput.SRID == 0)
                        {
                            errorMessage = Config.ConfigSettings.MessageSettings.RequiredFieldMessage;
                            return false;
                        }
                    }
                    else
                    {
                        errorMessage = Config.ConfigSettings.MessageSettings.RequiredFieldMessage;
                        return false;
                    }
                }

                if (tvAgentInput != null)
                {
                    if (tvAgentInput.SearchRequest != null && tvAgentInput.SearchRequest.TV != null && !string.IsNullOrWhiteSpace(tvAgentInput.SearchRequest.AgentName) && !string.IsNullOrWhiteSpace(tvAgentInput.SearchRequest.TV.SearchTerm))
                    {
                        if (tvAgentInput.SearchRequest.TV.CountryList != null)
                        {
                            foreach (var _Country in tvAgentInput.SearchRequest.TV.CountryList)
                            {
                                var InValidCountry = (from r in statskedprogData.CountryList.Where(r => r.name.Trim().ToLower() == _Country.name.Trim().ToLower() && r.num == _Country.num) select r).ToList();
                                if (InValidCountry.Count == 0)
                                {
                                    errorMessage = Config.ConfigSettings.MessageSettings.CountryDoesNotExistMessage;
                                    return false;
                                }
                            }
                        }

                        if (tvAgentInput.SearchRequest.TV.RegionList != null)
                        {
                            foreach (var _Region in tvAgentInput.SearchRequest.TV.RegionList)
                            {
                                var InValidRegion = (from r in statskedprogData.RegionList.Where(r => r.name.Trim().ToLower() == _Region.name.Trim().ToLower() && r.num == _Region.num) select r).ToList();
                                if (InValidRegion.Count == 0)
                                {
                                    errorMessage = Config.ConfigSettings.MessageSettings.RegionDoesNotExistMessage;
                                    return false;
                                }
                            }
                        }

                        if (tvAgentInput.SearchRequest.TV.AffiliateList != null)
                        {
                            foreach (var _Affiliate in tvAgentInput.SearchRequest.TV.AffiliateList)
                            {
                                var InValidAffiliate = (from r in statskedprogData.AffiliateList.Where(r => r.Name.Trim().ToLower() == _Affiliate.name.Trim().ToLower()) select r).ToList();
                                if (InValidAffiliate.Count == 0)
                                {
                                    errorMessage = Config.ConfigSettings.MessageSettings.AffiliateDoesNotExistMessage;
                                    return false;
                                }
                            }
                        }

                        if (tvAgentInput.SearchRequest.TV.DmaList != null)
                        {
                            foreach (var _Dma in tvAgentInput.SearchRequest.TV.DmaList)
                            {
                                var InValidDma = (from r in statskedprogData.DmaList.Where(r => r.Name.Trim().ToLower() == _Dma.name.Trim().ToLower() && r.Num == _Dma.num) select r).ToList();
                                if (InValidDma.Count == 0)
                                {
                                    errorMessage = Config.ConfigSettings.MessageSettings.DmaDoesNotExistMessage;
                                    return false;
                                }
                            }
                        }

                        if (tvAgentInput.SearchRequest.TV.ProgramCategoryList != null)
                        {
                            foreach (var _ProgramCategory in tvAgentInput.SearchRequest.TV.ProgramCategoryList)
                            {
                                var InValidProgramCategory = (from r in statskedprogData.ProgramCategoryList.Where(r => r.Name.Trim().ToLower() == _ProgramCategory.name.Trim().ToLower() && r.Num == _ProgramCategory.num) select r).ToList();
                                if (InValidProgramCategory.Count == 0)
                                {
                                    errorMessage = Config.ConfigSettings.MessageSettings.ProgramCategoryDoesNotExistMessage;
                                    return false;
                                }
                            }
                        }

                        if (tvAgentInput.SearchRequest.TV.StationList != null)
                        {
                            foreach (var _Station in tvAgentInput.SearchRequest.TV.StationList)
                            {
                                var InValidStation = (from r in statskedprogData.StationList.Where(r => r.name.Trim().ToLower() == _Station.name.Trim().ToLower()) select r).ToList();
                                if (InValidStation.Count == 0)
                                {
                                    errorMessage = Config.ConfigSettings.MessageSettings.StationDoesNotExistMessage;
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        errorMessage = Config.ConfigSettings.MessageSettings.RequiredFieldMessage;
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
