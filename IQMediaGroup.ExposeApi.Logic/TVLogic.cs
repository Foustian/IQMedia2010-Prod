using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Configuration;
using System.Xml.Linq;

namespace IQMediaGroup.ExposeApi.Logic
{
    internal class TVLogic
    {
        internal static TVAgentResult GetTVFromFeedsResult(FeedsSearch.Hit p_FR, bool p_HasNilesenAccess)
        {
            var tv = new TVAgentResult();

            tv.GMTDateTime = p_FR.MediaDate;
            tv.Highlights = !string.IsNullOrWhiteSpace(p_FR.HighlightingText) ? GetCCHighLights(p_FR.HighlightingText) : null;
            tv.HitCount = p_FR.NumberOfHits;
            tv.ID = p_FR.ID;            
            tv.IQProminence = p_FR.IQProminence;

            if (tv.IQProminence > 0)
            {
                tv.IQProminenceMultiplier = p_FR.IQProminenceMultiplier; 
            }

            tv.Market = p_FR.Market;
            tv.MediaDateTime = p_FR.LocalDate;
            tv.NationalAudience = p_FR.NationalAudience;
            tv.NationalAudienceValueType = p_FR.NationalAudienceType;
            tv.NationalIQAdShareValue = p_FR.NationalMediaValue;
            tv.NegativeSentiment = p_FR.NegativeSentiment;
            tv.ParentID = p_FR.ParentID;
            tv.PositiveSentiment = p_FR.PositiveSentiment;
            tv.Station = p_FR.StationID;
            tv.StationCallSign = p_FR.Outlet;
            tv.StationLogo = ConfigurationManager.AppSettings["StationLogoURL"] + p_FR.StationID + ".jpg";
            tv.SRID = p_FR.SearchRequestID;
            tv.SubMediaType = p_FR.MediaCategory;
            tv.ThumbUrl = p_FR.ThumbnailUrl;
            tv.TimeZone = p_FR.TimeZone;
            tv.Title = p_FR.Title;
            tv.Url = ConfigurationManager.AppSettings["IframeURL"] + "?SeqID=" + p_FR.ID;
            tv.VideoGuid = p_FR.VideoGUID;
            
            if (p_HasNilesenAccess)
            {
                tv.Audience = p_FR.Audience;
                tv.AudienceResult = p_FR.AudienceType;
                tv.IQAdShareValue = p_FR.MediaValue;
            }

            return tv;
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
