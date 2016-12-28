using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMGSearch;
using System.Configuration;
using IQMediaGroup.Domain;
using System.Web;

namespace IQMediaGroup.Logic
{
    public class TWLogic : BaseLogic, ILogic
    {
        public IQAgentSearchTerm GetSearchTermByIQAgent_TwitterResultsID(Int64 p_ID)
        {
            try
            {
                var iQAgentSearchTerm = (IQAgentSearchTerm)Context.GetSearchTermBy_IQAgent_TwitterResultsID(p_ID).FirstOrDefault();
                return iQAgentSearchTerm;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetTWHighlight(string p_SearchTerm, string p_TweetID, int? p_FragSize, out bool p_HasResult, string p_PmgUrl)
        {
            try
            {
                p_HasResult = false;
                bool hasError = false;
                string highlight = string.Empty;

                Uri pmgSearchRequestUrl = new Uri(p_PmgUrl);
                SearchEngine searchEngine = new SearchEngine(pmgSearchRequestUrl);

                SearchTwitterRequest twSearchRequest = new SearchTwitterRequest();
                twSearchRequest.IDs = new List<long> { Convert.ToInt64(p_TweetID) };
                twSearchRequest.SearchTerm = p_SearchTerm;
                twSearchRequest.IsHighlighting = true;
                twSearchRequest.FragSize = p_FragSize;

                SearchTwitterResult twResult = searchEngine.SearchTwitter(twSearchRequest, false, out hasError);

                if (!string.IsNullOrEmpty(twResult.ResponseXml) && twResult.TwitterResults.Count > 0)
                {
                    p_HasResult = true;
                    highlight = twResult.TwitterResults.First().Highlight;
                }

                return highlight;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }
    }
}
