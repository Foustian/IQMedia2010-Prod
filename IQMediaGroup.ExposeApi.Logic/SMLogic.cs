using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using System.Xml.Linq;

namespace IQMediaGroup.ExposeApi.Logic
{
    internal class SMLogic
    {
        internal static SM GetSMFromFeedsResult(FeedsSearch.Hit p_FR, bool p_HasCompeteAccess)
        {
            var sm = new SM();
           
            sm.GMTDateTime = p_FR.MediaDate;
            sm.Highlights = !string.IsNullOrWhiteSpace(p_FR.HighlightingText) ? GetHighlights(p_FR.HighlightingText) : null;
            sm.HitCount = p_FR.NumberOfHits;
            sm.ID = p_FR.ID;
            sm.IQProminence = p_FR.IQProminence;

            if (sm.IQProminence > 0)
            {
                sm.IQProminenceMultiplier = p_FR.IQProminenceMultiplier; 
            }

            sm.NegativeSentiment = p_FR.NegativeSentiment;
            sm.ParentID = p_FR.ParentID;
            sm.PositiveSentiment = p_FR.PositiveSentiment;
            sm.Publication = p_FR.Publication;
            sm.SRID = p_FR.SearchRequestID;
            sm.SubMediaType = p_FR.MediaCategory;
            sm.Title = p_FR.Title;
            sm.ThumbUrl = p_FR.ThumbnailUrl;
            sm.Url = p_FR.Url;

            if (p_HasCompeteAccess)
            {
                sm.Audience = p_FR.Audience;
                sm.IQAdShareValue = p_FR.MediaValue;
                sm.AudienceResult = p_FR.AudienceType;
            }

            if (p_FR.MediaCategory.ToUpper() == "FB" || p_FR.MediaCategory.ToUpper() == "IG")
            {
                var articleStatus = new ArticleStatus();

                articleStatus = (ArticleStatus)Common.Util.CommonFunctions.DeserialiazeXml(p_FR.ArticleStats, articleStatus);

                sm.Likes = articleStatus.Likes;
                sm.Comments = articleStatus.Comments;
                sm.Shares = articleStatus.Shares;
            }

            return sm;
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
