using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;

namespace IQMediaGroup.ExposeApi.Logic
{
    public class SolrEngineLogic : BaseLogic, ILogic
    {
        public enum PMGUrlType
        {
            TV,
            MO,
            TW,
            FE
        }

        public List<SolrEngines> GetSolrEngines()
        {
            try
            {
                return Context.GetSolrEngines("apiservices").ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<SolrEngines> ListOfSolrEngines
        {
            get
            {
                if (_ListOfSolrEngines == null)
                {
                    _ListOfSolrEngines = new SolrEngineLogic().GetSolrEngines();
                }
                return _ListOfSolrEngines;
            }
            set
            {
                _ListOfSolrEngines = value;
            }
        } static List<SolrEngines> _ListOfSolrEngines;

        public static string GeneratePMGUrl(string p_Type, DateTime? p_FromDate, DateTime? p_ToDate)
        {
            try
            {
                string pmgUrl = string.Empty;

                /*List<string> solrCoreUrls = Config.ConfigSettings.SolrSettings.SolrCores.Where(a => a.Type == p_Type).
                                                        Where(a=> 
                                                                (a.FromDate >= p_FromDate && a.FromDate <= p_ToDate) || 
                                                                (a.ToDate >= p_FromDate && a.ToDate <= p_ToDate)
                                                             ).OrderByDescending(a => a.ToDate).
                                                             Select(a => a.Url).ToList();*/

                List<string> solrCoreUrls = (from core in ListOfSolrEngines
                                             where core.MediaType == p_Type &&
                                                  (
                                                    (
                                                        (core.FromDate >= p_FromDate && core.FromDate <= p_ToDate) ||
                                                        (core.ToDate >= p_FromDate && core.ToDate <= p_ToDate)
                                                    )
                                                        ||
                                                    (
                                                        (p_FromDate >= core.FromDate && p_FromDate <= core.ToDate) ||
                                                        (p_ToDate >= core.FromDate && p_ToDate <= core.ToDate)
                                                    )
                                                  )
                                             orderby core.ToDate descending
                                             select core.BaseUrl
                 ).ToList();

                if (solrCoreUrls == null || solrCoreUrls.Count == 0)
                {
                    solrCoreUrls = ListOfSolrEngines.Where(a => a.MediaType == p_Type).OrderByDescending(a => a.ToDate).Select(a => a.BaseUrl).ToList();
                }

                pmgUrl = solrCoreUrls[0] + "select/";
                if (solrCoreUrls.Count > 1)
                {
                    pmgUrl = pmgUrl + "?shards=" + string.Join(",", solrCoreUrls).Replace("http://", "") + "&";
                }

                return pmgUrl;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
