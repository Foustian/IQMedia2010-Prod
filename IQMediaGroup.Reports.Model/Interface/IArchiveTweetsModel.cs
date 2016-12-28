using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Reports.Model.Interface
{
    public interface IArchiveTweetsModel
    {
        DataSet GetArchiveTweetsReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        DataSet GetArchiveTweetsByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid);

        DataSet GetArchiveTweetsByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid);
    }
}
