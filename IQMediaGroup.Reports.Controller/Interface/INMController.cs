using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using PMGSearch;


namespace IQMediaGroup.Reports.Controller.Interface
{
    public interface INMController
    {
        List<NewsResult> GetIQAgent_NMResultBySearchDate(Guid p_ClientGuid,int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay,  Boolean p_IsCompeteData,out string Query_Name);
    }
}
