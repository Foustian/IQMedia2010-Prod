using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMGSearch;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Controller.Interface
{
    public interface ISMController
    {
        List<SMResult> GetIQAgent_SMResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, Boolean p_IsCompeteData, out string Query_Name);
        string InsertArchiveSM(ArchiveSM p_ArchiveSM);
    }
}
