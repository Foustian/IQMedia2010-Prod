using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Xml.Linq;

namespace IQMediaGroup.ExposeApi.Logic
{
    internal class NMLogic
    {
        internal static NM GetNMFromFeedsResult(FeedsSearch.Hit p_FR, bool p_HasCompeteAccess)
        {
            var nm = new NM();

            nm.GMTDateTime = p_FR.MediaDate;
            nm.Highlights = !string.IsNullOrWhiteSpace(p_FR.HighlightingText) ? GetHighlights(p_FR.HighlightingText) : null;
            nm.HitCount = p_FR.NumberOfHits;            
            nm.ID = p_FR.ID;
            nm.IQProminence = p_FR.IQProminence;

            if (nm.IQProminence > 0)
            {
                nm.IQProminenceMultiplier = p_FR.IQProminenceMultiplier; 
            }

            nm.Market = p_FR.Market;
            nm.NegativeSentiment = p_FR.NegativeSentiment;
            nm.ParentID = p_FR.ParentID;
            nm.PositiveSentiment = p_FR.PositiveSentiment;
            nm.Publication = p_FR.Publication;
            nm.SRID = p_FR.SearchRequestID;
            nm.SubMediaType = p_FR.MediaCategory;
            nm.Title = p_FR.Title;
            nm.Url = p_FR.Url;            

            if (p_HasCompeteAccess)
            {
                nm.Audience = p_FR.Audience;
                nm.IQAdShareValue = p_FR.MediaValue;
                nm.AudienceResult = p_FR.AudienceType;
            }

            return nm;
        }

        private static string GetHighlights(string p_Highlights)
        {
            var highlights = "";

            XDocument xDoc = XDocument.Parse(p_Highlights);

            if (xDoc.Root.Element("Highlights") != null)
            {
                highlights = Convert.ToString(xDoc.Root.Element("Highlights"));
            }

            return highlights;
        }
    }
}
