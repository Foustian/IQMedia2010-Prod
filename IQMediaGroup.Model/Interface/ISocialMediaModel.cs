using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Model.Interface
{
    public interface ISocialMediaModel
    {
        DataSet GetSocialMediaFilterData();

        string InsertArchiveSM(ArchiveSM p_ArchiveSM);

        DataSet GetArchiveSMBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_FromDate, DateTime? p_ToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount);

        string UpdateArchiveSM(ArchiveSM p_ArchiveSM);

        string UpdateIQCoreSMStatus(string p_ArticleID);

        string DeleteArchiveSM(string p_DeleteArchiveSM);

        string GetArticlePathByArticleID(string ArticleID);

        DataSet GetArchiveSMByArchiveSMKey(int p_ArchiveSMKey);

        DataSet GetArchiveSMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        DataSet GetArchiveSMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid);

        string InsertArchiveSMByList(SqlXml p_SqlXml, out int p_Status, out int p_RecordsInserted);
    }
}
