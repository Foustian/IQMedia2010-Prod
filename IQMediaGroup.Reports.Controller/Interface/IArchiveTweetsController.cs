using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Controller.Interface
{
    public interface IArchiveTweetsController
    {
        List<ArchiveTweets> GetArchiveTweetsReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        List<ArchiveTweets> GetArchiveTweetsByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid);

        List<ArchiveTweets> GetArchiveTweetsByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid);
    }
}
