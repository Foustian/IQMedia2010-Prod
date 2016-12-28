using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;

namespace IQMediaGroup.Logic
{
    public class SolrEngineLogic : BaseLogic, ILogic
    {
        public List<SolrEngines> GetSolrEngines(string requestor)
        {
            try
            {
                return Context.GetSolrEngines(requestor).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
