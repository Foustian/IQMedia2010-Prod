using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;
namespace IQMediaGroup.Reports.Model.Interface
{
    /// <summary>
    /// Interface of Client Role
    /// </summary>
    public interface IArchiveClipModel
    {
        DataSet GetArchiveClipReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        DataSet GetArchiveClipByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid, Boolean p_IsNielSenData);

        DataSet GetArchiveClipByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid, Boolean p_IsNielSenData);
    }
}
