using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;
using System.Xml.Linq;
using System.Configuration;
using PMGSearch;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.Logic
{
    public class CompeteDataLogic : BaseLogic, ILogic
    {
        public bool? CheckClientCompeteDataAccess(Guid _ClientGuid)
        {
            try
            {
                var HasAccess = Context.CheckForCompeteDataAccessByClientGUID(_ClientGuid);

                return HasAccess.SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CompeteOutput GetCompeteByListSM(CompeteInput _CompeteInput, string p_PmgUrl)
        {
            CompeteOutput _CompeteOutput = new CompeteOutput();
            try
            {

                /*Dictionary<string, string> _DictiobnaryIDToCompeteUrl = _MediaType ==
                    IQMediaGroup.Common.Util.CommonConstants.MediaType.NM.ToString() ?
                    GetNewsCompeteUrlList(_CompeteInput.ArticleIDSet) : GetSocialMediaCompeteUrlList(_CompeteInput.ArticleIDSet);

                var _DistinctCompeteUrl = _DictiobnaryIDToCompeteUrl.Select(a => a.Value).Distinct().ToList();*/

                List<SMResult> listOfSMResult = GetSocialMediaCompeteUrlList(_CompeteInput.ArticleIDSet, p_PmgUrl);

                IEnumerable<SMResult> listOfUniqueSMResult = from smResult in listOfSMResult
                                                             group smResult by smResult.HomeurlDomain into uniqueSMResult
                                                             select new SMResult { HomeurlDomain = uniqueSMResult.Key, feedClass = uniqueSMResult.FirstOrDefault().feedClass };

                var competeXml = new XElement("list",
                                        from SMResult smres in listOfUniqueSMResult
                                        select new XElement("item", new XAttribute("url", smres.HomeurlDomain), new XAttribute("sourceCategory", smres.feedClass)));


                List<CompeteDataDB> _ListOfCompeteDataDB = Context.GetCompeteDataByClientGuidAndXml(_CompeteInput.ClientGuid, competeXml.ToString(), IQMediaGroup.Common.Util.CommonConstants.MediaType.SM.ToString()).ToList();

                List<CompeteData> _ListOfCompeteData = listOfSMResult.Join(_ListOfCompeteDataDB, q => q.HomeurlDomain.ToLower(), a => a.CompeteURL.ToLower(),
                                                    (q, a) => new CompeteData
                                                    {
                                                        ArticleID = q.IQSeqID,
                                                        c_uniq_visitor = (a.c_uniq_visitor == -1 ? string.Empty : (!a.IsUrlFound ? "NA" : Convert.ToString(a.c_uniq_visitor))),
                                                        IQ_AdShare_Value = (a.IQ_AdShare_Value == -1 ? string.Empty : (!a.IsUrlFound ? "NA" : Convert.ToString(a.IQ_AdShare_Value))),
                                                        IsCompeteAll = a.IsCompeteAll

                                                    }).ToList();

                _CompeteOutput.CompeteDataSet = _ListOfCompeteData;
                _CompeteOutput.Status = 0;
                _CompeteOutput.Message = "Success";
                return _CompeteOutput;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CompeteOutput GetCompeteByListNM(CompeteInput _CompeteInput, string p_PmgUrl)
        {
            CompeteOutput _CompeteOutput = new CompeteOutput();
            try
            {

                Dictionary<string, string> _DictiobnaryIDToCompeteUrl = GetNewsCompeteUrlList(_CompeteInput.ArticleIDSet, p_PmgUrl);

                var _DistinctCompeteUrl = _DictiobnaryIDToCompeteUrl.Select(a => a.Value).Distinct().ToList();

                var _CompeteXml = new XElement("list",
                                                from string websiteurl in _DistinctCompeteUrl
                                                select new XElement("item", new XAttribute("url", websiteurl)));

                List<CompeteDataDB> _ListOfCompeteDataDB = Context.GetCompeteDataByClientGuidAndXml(_CompeteInput.ClientGuid, _CompeteXml.ToString(), IQMediaGroup.Common.Util.CommonConstants.MediaType.NM.ToString()).ToList();

                List<CompeteData> _ListOfCompeteData = _DictiobnaryIDToCompeteUrl.Join(_ListOfCompeteDataDB, q => q.Value.ToLower(), a => a.CompeteURL.ToLower(),
                                                    (q, a) => new CompeteData
                                                    {
                                                        ArticleID = q.Key,
                                                        c_uniq_visitor = (a.c_uniq_visitor == -1 ? string.Empty : (!a.IsUrlFound ? "NA" : Convert.ToString(a.c_uniq_visitor))),
                                                        IQ_AdShare_Value = (a.IQ_AdShare_Value == -1 ? string.Empty : (!a.IsUrlFound ? "NA" : Convert.ToString(a.IQ_AdShare_Value))),
                                                        IsCompeteAll = a.IsCompeteAll

                                                    }).ToList();

                _CompeteOutput.CompeteDataSet = _ListOfCompeteData;
                _CompeteOutput.Status = 0;
                _CompeteOutput.Message = "Success";
                return _CompeteOutput;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CompeteUrlOutput GetCompeteByCompeteURL(CompeteUrlInput _CompeteUrlInput, IQMediaGroup.Common.Util.CommonConstants.MediaType _MediaType)
        {
            try
            {
                CompeteUrlOutput _CompeteUrlOutput = new CompeteUrlOutput();
                var _DistinctCompeteUrl = _CompeteUrlInput.CompeteUrlSet.Select(a => a).Distinct().ToList();

                var _CompeteXml = new XElement("list",
                                                from string websiteurl in _DistinctCompeteUrl
                                                select new
                                                        XElement("item",
                                                            new XAttribute("url", websiteurl),
                                                            _MediaType == Common.Util.CommonConstants.MediaType.SM ? new XAttribute("sourceCategory", "Blog") : null)
                                              );

                List<CompeteDataDB> _ListOfCompeteDataDB = null;

                try
                {
                    _ListOfCompeteDataDB = Context.GetCompeteDataByClientGuidAndXml(_CompeteUrlInput.ClientGuid, _CompeteXml.ToString(), _MediaType.ToString()).ToList();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Debug("CompeteLogicError: " + ex.ToString());
                    Context.Connection.Open();

                    _ListOfCompeteDataDB = Context.GetCompeteDataByClientGuidAndXml(_CompeteUrlInput.ClientGuid, _CompeteXml.ToString(), _MediaType.ToString()).ToList();
                }

                _CompeteUrlOutput.Status = 0;
                _CompeteUrlOutput.Message = "Success";
                _CompeteUrlOutput.CompeteDataSet = _ListOfCompeteDataDB.Select(a => new CompeteDataUrl
                                                   {
                                                       CompeteURL = a.CompeteURL,
                                                       c_uniq_visitor = (a.c_uniq_visitor == -1 ? string.Empty : (!a.IsUrlFound ? "NA" : Convert.ToString(a.c_uniq_visitor))),
                                                       IQ_AdShare_Value = (a.IQ_AdShare_Value == -1 ? string.Empty : (!a.IsUrlFound ? "NA" : Convert.ToString(a.IQ_AdShare_Value))),
                                                       IsCompeteAll = a.IsCompeteAll
                                                   }).ToList();

                return _CompeteUrlOutput;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<string, string> GetNewsCompeteUrlList(List<string> _CompeteUrls, string p_PmgUrl)
        {
            try
            {
                Uri _PMGSearchRequestUrl = new Uri(p_PmgUrl);
                SearchEngine _SearchEngine = new SearchEngine(_PMGSearchRequestUrl);

                SearchNewsRequest _SearchNewsRequest = new SearchNewsRequest();
                _SearchNewsRequest.IDs = _CompeteUrls;
                _SearchNewsRequest.PageSize = _CompeteUrls.Count();

                SearchNewsResults _SearchNewsResults = _SearchEngine.SearchNews(_SearchNewsRequest);

                Dictionary<string, string> _DictiobnaryIDToUrl = _SearchNewsResults.newsResults.ToDictionary(a => a.IQSeqID, a => a.HomeurlDomain);

                return _DictiobnaryIDToUrl;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SMResult> GetSocialMediaCompeteUrlList(List<string> _CompeteUrls, string p_PmgUrl)
        {
            try
            {
                Uri _PMGSearchRequestUrl = new Uri(p_PmgUrl);
                SearchEngine _SearchEngine = new SearchEngine(_PMGSearchRequestUrl);

                SearchSMRequest _SearchSMRequest = new SearchSMRequest();
                _SearchSMRequest.ids = _CompeteUrls;
                _SearchSMRequest.PageSize = _CompeteUrls.Count();

                SearchSMResult _SearchSMResult = _SearchEngine.SearchSocialMedia(_SearchSMRequest);
                //_SearchSMResult.smResults.ForEach(smr => smr.homeLink=new Uri(smr.homeLink).Host.Replace("www.", ""));

                return _SearchSMResult.smResults;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CompeteDemographicData> GetCompeteDemographic(Guid p_ClientGUID, List<string> p_CompeteURLList, string p_SubMediaType)
        {
            var urlXML = new XElement("list",
                                                from string url in p_CompeteURLList
                                                select new
                                                        XElement("item",
                                                            new XAttribute("url", url)
                                              ));

            return Context.GetCompeteDemographicData(p_ClientGUID, urlXML.ToString(), p_SubMediaType);
        }
    }
}
