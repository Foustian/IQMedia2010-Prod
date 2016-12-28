using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Interface
{
    public interface IArchiveNMController
    {
        string InsertArchiveNM(ArchiveNM p_ArchiveClips);

        List<ArchiveNM> GetArchiveNMBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_FromDate, DateTime? p_ToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount);

        string UpdateArchiveNM(ArchiveNM p_ArchiveNM);

        string DeleteArchiveNM(string p_DeleteArchiveNM);

        string GetArticlePathByArticleID(string ArticleID);

        List<ArchiveNM> GetArchiveNMByArchiveNMKey(int p_ArchiveNMKey);

        string GetEmailContent(List<ArchiveNM> lstArchiveNM);

        string UpdateIQCoreNMStatus(string p_ArticleID);

        List<ArchiveNM> GetArchiveNMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        List<ArchiveNM> GetArchiveNMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid);

        XDocument GenerateListToXML(List<ArchiveNM> _ListOfArchiveNM);

        string InsertArchiveNMByList(SqlXml p_SqlXml, out int p_Status, out int p_RecordsInserted);
    }
}
