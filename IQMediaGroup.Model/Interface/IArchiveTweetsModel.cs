using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Interface
{
    public interface IArchiveTweetsModel
    {
        DataSet GetArchiveTweetsBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_FromDate, DateTime? p_ToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount);
        string DeleteArchiveTweets(string p_DeleteArchiveTweets);
        DataSet GetArchiVeTweetsByArchiveTweets_Key(Int64 p_ArchiveTweetsKey);
        string UpdateArchiveTweets(ArchiveTweets archiveTweets);

        string InsertArchiveTweet(ArchiveTweets _ArchiveTweets);
    }
}
