using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using PMGSearch;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Interface
{
    public interface ISocialMediaController
    {
        SocialMedia GetSocialMediaFilterData();
        SearchSMResult GetSocialMediaGridData(SearchSMRequest searchSMRequest, SearchEngine searchEngine);
        string GetSocialMediaChartData(SearchSMRequest searchSMRequest, SearchEngine searchEngine, HttpContext currentContext, out Boolean isError);

        string InsertArchiveSM(ArchiveSM p_ArchiveSM);

        List<ArchiveSM> GetArchiveSMBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_FromDate, DateTime? p_ToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount);

        string UpdateArchiveSM(ArchiveSM p_ArchiveSM);

        string DeleteArchiveSM(string p_DeleteArchiveSM);

        string GetArticlePathByArticleID(string ArticleID);

        List<ArchiveSM> GetArchiveSMByArchiveSMKey(int p_ArchiveSMKey);

        string GetEmailContent(List<ArchiveSM> lstArchiveSM);

        string UpdateIQCoreSMStatus(string p_ArticleID);

        List<ArchiveSM> GetArchiveSMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        List<ArchiveSM> GetArchiveSMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid);

        XDocument GenerateListToXML(List<ArchiveSM> _ListOfArchiveSM);

        string InsertArchiveSMByList(SqlXml p_SqlXml, out int p_Status, out int p_RecordsInserted);
    }
}
