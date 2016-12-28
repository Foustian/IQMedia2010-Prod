using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMGSearch;
using System.Web.UI.HtmlControls;
using IQMediaGroup.Domain;
using System.Configuration;


namespace IQMediaGroup.Logic
{
    public class CCLogic : BaseLogic, ILogic
    {
        public IQAgentRawMedia GetSearchTermByIQAgent_TVResultsID(Int64? ID)
        {
            try
            {
                var iQAgentRawMedia = (IQAgentRawMedia)Context.GetSearchTermByIQAgent_TVResultsID(ID).FirstOrDefault();
                return iQAgentRawMedia;
            }
            catch (Exception exception)
            {
                throw exception;
            }


        }

        public List<ClosedCaption> GetClosedCaption(string p_SearchTerm, Guid p_RawMediaGuid, string p_IQ_CC_Key, int? p_FragOffset, out bool p_HasResult,string p_PmgUrl)
        {
            try
            {
                p_HasResult = false;

                Uri PMGSearchRequestUrl = new Uri(p_PmgUrl);
                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                SearchRequest _SearchRequest = new SearchRequest();
                if (p_RawMediaGuid != Guid.Empty)
                {
                    _SearchRequest.GuidList = Convert.ToString(p_RawMediaGuid);
                }
                else
                {
                    _SearchRequest.IQCCKeyList = p_IQ_CC_Key;
                }

                if (p_FragOffset.HasValue)
                    _SearchRequest.FragOffset = p_FragOffset.Value;

                _SearchRequest.Terms = p_SearchTerm;
                _SearchRequest.IsShowCC = true;


                int _PMGMaxHighlights = 20;

                if (ConfigurationManager.AppSettings["PMGMaxHighlights"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["PMGMaxHighlights"], out _PMGMaxHighlights);
                }

                _SearchRequest.MaxHighlights = _PMGMaxHighlights;

                SearchResult _SearchResult = _SearchEngine.Search(_SearchRequest);
                List<ClosedCaption> listofCC = new List<ClosedCaption>();

                if (!string.IsNullOrEmpty(_SearchResult.ResponseXml) && _SearchResult.Hits.Count > 0)
                {
                    p_HasResult = true;

                    if (_SearchResult.Hits[0].TermOccurrences.Count > 0)
                    {
                        foreach (TermOccurrence _TermOccurrence in _SearchResult.Hits[0].TermOccurrences)
                        {
                            ClosedCaption closedCaption = new ClosedCaption();
                            closedCaption.Text = _TermOccurrence.SurroundingText;
                            closedCaption.Offset = _TermOccurrence.TimeOffset;
                            listofCC.Add(closedCaption);
                        }
                    }
                }

                return listofCC;
            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

    }
}
