using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Reports.Model.Interface
{
    public interface INMModel
    {
        DataSet GetIQAgent_NMResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, Boolean p_IsCompeteData, out string Query_Name);
    }
}
