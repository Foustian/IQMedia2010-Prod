using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Reports.Model.Interface
{
    public interface ISocialMediaModel
    {
        DataSet GetArchiveSMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        DataSet GetArchiveSMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid, Boolean p_IsCompeteData);

        DataSet GetArchiveSMByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid, Boolean p_IsCompeteData);
    }
}
