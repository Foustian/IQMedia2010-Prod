using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Model.Interface
{
    public interface ISMModel
    {
        DataSet GetIQAgent_SMResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, Boolean p_IsCompeteData, out string Query_Name);
        string InsertArchiveSM(ArchiveSM p_ArchiveSM);
    }
}
