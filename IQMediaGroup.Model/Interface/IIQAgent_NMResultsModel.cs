using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Model.Interface
{
    public interface IIQAgent_NMResultsModel
    {
        DataSet GetIQAgentNMResultsBySearchRequestID(int p_SearchRequestID, int p_PageSize, int p_PageNumber, string p_SortField,bool p_IsAcending,out int p_TotalRecordsCount);

        string DeleteIQAgent_NMResults(string IQAgent_NMResultKey);

    }
}
