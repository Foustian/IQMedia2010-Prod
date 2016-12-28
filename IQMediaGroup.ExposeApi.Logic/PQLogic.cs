using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Xml.Linq;

namespace IQMediaGroup.ExposeApi.Logic
{
    internal class PQLogic
    {
        internal static PQ GetPQFromFeedsResult(FeedsSearch.Hit p_FR)
        {
            var pq = new PQ();

            pq.Authors = p_FR.Authors;
            pq.AvailableDateTime = p_FR.AvailableDate;
            pq.Content = p_FR.Content;
            pq.Copyright = p_FR.Copyright;
            pq.GMTDateTime = p_FR.MediaDate;
            pq.Highlights = !string.IsNullOrWhiteSpace(p_FR.HighlightingText) ? GetHighlights(p_FR.HighlightingText) : null;
            pq.HitCount = p_FR.NumberOfHits;
            pq.ID = p_FR.ID;
            pq.NegativeSentiment = p_FR.NegativeSentiment;
            pq.ParentID = p_FR.ParentID;
            pq.PositiveSentiment = p_FR.PositiveSentiment;
            pq.Publication = p_FR.Publication;
            pq.SRID = p_FR.SearchRequestID;
            pq.SubMediaType = p_FR.MediaCategory;
            pq.Title = p_FR.Title;

            return pq;
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
