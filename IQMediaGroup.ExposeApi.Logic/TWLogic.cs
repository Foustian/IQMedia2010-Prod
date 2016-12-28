using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Xml.Linq;

namespace IQMediaGroup.ExposeApi.Logic
{
    internal class TWLogic
    {
        internal static TW GetTWFromFeedsResult(FeedsSearch.Hit p_FR)
        {
            var tw = new TW();

            tw.Content = p_FR.Content;
            tw.DisplayName = p_FR.Title;
            tw.FollowersCount = p_FR.Audience;
            tw.FriendsCount = p_FR.ActorFriendsCount;
            tw.GMTDateTime = p_FR.MediaDate;
            tw.Highlights = !string.IsNullOrWhiteSpace(p_FR.HighlightingText) ? GetHighlights(p_FR.HighlightingText) : null;
            tw.HitCount = p_FR.NumberOfHits;
            tw.ID = p_FR.ID;
            tw.IQProminence = p_FR.IQProminence;

            if (tw.IQProminence > 0)
            {
                tw.IQProminenceMultiplier = p_FR.IQProminenceMultiplier; 
            }

            tw.KloutScore = Convert.ToInt64(p_FR.MediaValue);
            tw.NegativeSentiment = p_FR.NegativeSentiment;
            tw.ParentID = p_FR.ParentID;
            tw.PositiveSentiment = p_FR.PositiveSentiment;
            tw.PreferredName = p_FR.ActorPreferredName;
            tw.SRID = p_FR.SearchRequestID;
            tw.SubMediaType = p_FR.MediaCategory;
            tw.ThumbUrl = p_FR.ThumbnailUrl;
            tw.Url = p_FR.Url;

            return tw;
        }

        private static string GetHighlights(string p_Highlights)
        {
            var highlights = "";

            XDocument xDoc = XDocument.Parse(p_Highlights);

            if (xDoc.Root.Element("Text") != null)
            {
                highlights = xDoc.Root.Element("Text").Value;
            }

            return highlights;
        }
    }
}
