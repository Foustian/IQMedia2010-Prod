using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Reports.Model.Interface
{
    public interface IArchiveNMModel
    {
        DataSet GetArchiveNMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        DataSet GetArchiveNMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid, Boolean p_IsCompeteData);

        string InsertArchiveNM(ArchiveNM p_ArchiveClips);

        DataSet GetArchiveNMByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid, Boolean p_IsCompeteData);
        
    }
}
