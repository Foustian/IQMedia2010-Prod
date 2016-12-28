using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Web;

namespace IQMediaGroup.Reports.Controller.Interface
{
    public interface ISocialMediaController
    {
        List<ArchiveSM> GetArchiveSMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        List<ArchiveSM> GetArchiveSMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid, Boolean p_IsCompeteData);

        List<ArchiveSM> GetArchiveSMByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid, Boolean p_IsCompeteData);
    }
}
