using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PMGSearch;
using IQMediaGroup.Domain;

namespace IQMediaGroup.Logic
{
    public class NewsLogic : BaseLogic, ILogic
    {
        public IQAgentSearchTerm GetSearchTermByIQAgent_NMResultsID(Int64? ID)
        {
            try
            {
                var iQAgentSearchTerm = (IQAgentSearchTerm)Context.GetSearchTermBy_IQAgent_NMResultsID(ID).FirstOrDefault();
                return iQAgentSearchTerm;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<string> GetNewsHighlight(string p_SearchTerm, string p_ArtileID, int? p_FragSize, out bool p_HasResult,string p_PmgUrl)
        {
            try
            {
                p_HasResult = false;
                Uri PMGSearchRequestUrl = new Uri(p_PmgUrl);
                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchNewsRequest searchNMReq = new SearchNewsRequest();
                searchNMReq.IDs = new List<string> { p_ArtileID };
                searchNMReq.SearchTerm = p_SearchTerm;
                searchNMReq.IsReturnHighlight = true;
                searchNMReq.FragSize = p_FragSize;
                searchNMReq.IsTitleNContentSearch = true;

                SearchNewsResults _SearchNewsResults = _SearchEngine.SearchNews(searchNMReq);
                List<string> listofHighlight = new List<string>();

                if (!string.IsNullOrEmpty(_SearchNewsResults.ResponseXml) && _SearchNewsResults.newsResults.Count > 0)
                {
                    p_HasResult = true;
                    listofHighlight = _SearchNewsResults.newsResults[0].Highlights;
                }

                return listofHighlight;
            }
            catch (Exception exception)
            {

                throw exception;
            }

        }
    }
}
