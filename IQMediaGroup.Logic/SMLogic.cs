using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PMGSearch;
using IQMediaGroup.Domain;

namespace IQMediaGroup.Logic
{
    public class SMLogic : BaseLogic, ILogic
    {
        public IQAgentSearchTerm GetSearchTermByIQAgent_SMResultsID(Int64? ID)
        {
            try
            {
                var iQAgentSearchTerm = (IQAgentSearchTerm)Context.GetSearchTermBy_IQAgent_SMResultsID(ID).FirstOrDefault();
                return iQAgentSearchTerm;
            }
            catch (Exception exception)
            {
                throw exception;
            }


        }

        public List<string> GetSMHighlight(string p_SearchTerm, string p_ArtileID, int? p_FragSize, out bool p_HasResult,string p_PmgUrl)
        {
            try
            {
                p_HasResult = false;

                Uri PMGSearchRequestUrl = new Uri(p_PmgUrl);
                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchSMRequest searchSMReq = new SearchSMRequest();
                searchSMReq.ids = new List<string> { p_ArtileID };
                searchSMReq.SearchTerm = p_SearchTerm;
                searchSMReq.IsReturnHighlight = true;
                searchSMReq.FragSize = p_FragSize;
                searchSMReq.IsTitleNContentSearch = true;

                SearchSMResult _SearchSMResult = _SearchEngine.SearchSocialMedia(searchSMReq);
                List<string> listofHighlight = new List<string>();

                if (!string.IsNullOrEmpty(_SearchSMResult.ResponseXml) && _SearchSMResult.smResults.Count > 0)
                {
                    p_HasResult = true;
                    listofHighlight = _SearchSMResult.smResults[0].Highlights;
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
