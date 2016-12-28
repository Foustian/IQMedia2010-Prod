using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Xml.Linq;
using System.Configuration;

namespace IQMediaGroup.ExposeApi.Logic
{
    internal class PMLogic
    {
        internal static PM GetPMFromFeedsResult(FeedsSearch.Hit p_FR)
        {
            var pm = new PM();

            pm.Authors = pm.Authors;            
            pm.Circulation = p_FR.Audience;
            pm.Content = p_FR.Content;
            pm.GMTDateTime = p_FR.MediaDate;
            pm.Highlights = !string.IsNullOrWhiteSpace(p_FR.HighlightingText) ? GetHighlights(p_FR.HighlightingText) : null;
            pm.HitCount = p_FR.NumberOfHits;
            pm.ID = p_FR.ID;
            pm.NegativeSentiment = p_FR.NegativeSentiment;
            pm.ParentID = p_FR.ParentID;
            pm.PositiveSentiment = p_FR.PositiveSentiment;
            pm.Publication = p_FR.Publication;
            pm.SRID = p_FR.SearchRequestID;
            pm.SubMediaType = p_FR.MediaCategory;
            pm.Title = p_FR.Title;
            pm.Url = !string.IsNullOrEmpty(p_FR.FileLocation) ? ConfigurationManager.AppSettings["BLPMBaseUrl"] + p_FR.FileLocation.Replace("\\","/") : null;

            return pm;
        }

        private static string GetHighlights(string p_Highlights)
        {
            XDocument xdoc = XDocument.Parse(p_Highlights);
            string hilightedText = string.Join(" ", xdoc.Descendants("text").Select(e => e.Value));
            return hilightedText.Trim();
        }
    }
}
