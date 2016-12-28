using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;

namespace IQMediaGroup.ExposeApi.Logic
{
    internal class TMLogic
    {
        internal static TM GetTMFromFeedsResult(FeedsSearch.Hit p_FR)
        {
            var tm = new TM();

            tm.Duration = p_FR.Duration;
            tm.GMTDateTime = p_FR.MediaDate;
            tm.Highlights = p_FR.HighlightingText;
            tm.HitCount = p_FR.NumberOfHits;
            tm.ID = p_FR.ID;
            tm.LocalDateTime = p_FR.LocalDate;
            tm.Market = p_FR.Market;
            tm.NegativeSentiment = p_FR.NegativeSentiment;
            tm.ParentID = p_FR.ParentID;
            tm.PositiveSentiment = p_FR.PositiveSentiment;
            tm.SRID = p_FR.SearchRequestID;
            tm.Station = p_FR.StationID;
            tm.SubMediaType = p_FR.MediaCategory;
            tm.TimeZone = p_FR.TimeZone;
            tm.Title = p_FR.Title;
            tm.Url = p_FR.Url;

            return tm;
        }
    }
}
