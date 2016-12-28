using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Controller.Interface
{
    public interface IArchiveNMController
    {
        List<ArchiveNM> GetArchiveNMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        List<ArchiveNM> GetArchiveNMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid, Boolean p_IsCompeteData);

        string InsertArchiveNM(ArchiveNM p_ArchiveClips);

        List<ArchiveNM> GetArchiveNMByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid, Boolean p_IsCompeteData);
    }
}
