using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IQMediaGroup.Reports.Controller.Interface
{
    public interface IArchiveClipController
    {
        List<ArchiveClip> GetArchiveClipReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        List<ArchiveClip> GetArchiveClipByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid, Boolean p_IsNielSenData);

        List<ArchiveClip> GetArchiveClipByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid, Boolean p_IsCompeteData);
    }
}
