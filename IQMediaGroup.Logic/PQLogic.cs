using System;
using System.Collections.Generic;
using System.Linq;
using IQMediaGroup.Domain;
using PMGSearch;

namespace IQMediaGroup.Logic
{
    public class PQLogic : BaseLogic, ILogic
    {
        public IQAgentSearchTerm GetSearchTermByIQAgent_PQResultsID(Int64? ID)
        {
            try
            {
                var iQAgentSearchTerm = (IQAgentSearchTerm)Context.GetSearchTermBy_IQAgent_PQResultsID(ID).FirstOrDefault();
                return iQAgentSearchTerm;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<string> GetPQHighlight(string p_SearchTerm, string p_ProQuestID, int? p_FragSize, out bool p_HasResult, string p_PmgUrl)
        {
            try
            {
                bool isError = false;

                p_HasResult = false;
                Uri PMGSearchRequestUrl = new Uri(p_PmgUrl);
                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchProQuestRequest searchPQReq = new SearchProQuestRequest();
                searchPQReq.IDs = new List<string> { p_ProQuestID };
                searchPQReq.SearchTerm = p_SearchTerm;
                searchPQReq.IsReturnHighlight = true;
                searchPQReq.FragSize = p_FragSize;

                SearchProQuestResult _SearchPQResults = _SearchEngine.SearchProQuest(searchPQReq, false, out isError);
                List<string> listofHighlight = new List<string>();

                if (!string.IsNullOrEmpty(_SearchPQResults.ResponseXml) && _SearchPQResults.ProQuestResults.Count > 0)
                {
                    p_HasResult = true;
                    listofHighlight = _SearchPQResults.ProQuestResults[0].Highlights;
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
