using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface IIQAgent_NMResultsController
    {
        List<IQAgent_NMResult> GetIQAgentNMResultsBySearchRequestID(int p_SearchRequestID, int p_PageSize, int p_PageNumber, string p_SortField, bool p_IsAcending, out int p_TotalRecordCount);

        string DeleteIQAgent_NMResults(string IQAgent_NMResultKey);
    }
}
