using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Interface
{
    public interface IArchiveClipController
    {
        /// <summary>
        /// This method inserts ArcheiveClip Record
        /// </summary>
        /// <param name="p_ArchiveClips">Object Of ArchiveClips Class</param>
        /// <returns></returns>
        string InsertArchiveClip(ArchiveClip p_ArchiveClips);

        /// <summary>
        /// this method inserts list of ArcheiveClip Records from XML
        /// </summary>
        /// <param name="p_SqlXml"></param>
        /// <returns></returns>
        string InsertArchiveClip(SqlXml p_SqlXml);

        /// <summary>
        /// Description: This Methods gets Archive Clip Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">List of Object of Archive Clip</param>
        /// <returns>List of Object of Archive Clip</returns>
        List<ArchiveClip> GetArchiveClip(ArchiveClip p_ArchiveClip);


        List<ArchiveClip> GetAllArchiveClip(ArchiveClip p_ArchiveClip);
        /// <summary>
        /// Description: This Methods gets Archive Clip by ClipID Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">ClientID of Client</param>
        /// <returns>List of Object of Archive Clip </returns>
        List<ArchiveClip> GetArchiveClipByClipID(ArchiveClip p_ArchiveClip);

        

        /// <summary>
        /// This method gets Archive clip data between Start Date And End Date
        /// </summary>
        /// <param name="p_StartDate">Start Date</param>
        /// <param name="p_EndDate">End Date</param>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <returns>List of object of Archive Clip Details</returns>
        List<ArchiveClipExport> GetArchiveClipByDate(DateTime p_StartDate, DateTime p_EndDate, int p_CustomerID);        

        /// <summary>
        ///  Description:This method Get Archive Clip By Customer.
        /// </summary>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <returns>List of object of ArchiveClip</returns>
        List<ArchiveClip> GetArchiveClipByCustomer(Int64 p_CustomerID);

        /// <summary>
        /// Description:This method DeleteArchiveClip
        /// </summary>
        /// <param name="p_DeleteArchiveClip">DeleteArchiveClip</param>
        /// <returns>Count</returns>
        string DeleteArchiveClip(string p_DeleteArchiveClip);

        string EmailContent(string ClipGUID, string mailAddress, string PageName);

        string EmailContent(System.Data.SqlTypes.SqlXml ClipXML, string mailAddress, string PageName);

        string EmailContent(string URL, string FileName, string _imagePath, string FileID, string mailAddress, string PageName);        

        List<ArchiveClip> GetArchiveClipFromXML(System.Data.SqlTypes.SqlXml ClipXML);

        string UpdateArchiveClip(ArchiveClip p_ArchiveClip);

        List<ArchiveClip> GetArchiveClipByCustomerGUID(Guid p_CustomerGUID);

        XDocument GenerateListToXML(List<ArchiveClip> _ListOfArchiveClip);

        string InsertArchiveClipFromConsole(ArchiveClip p_ArchiveClip);

        List<ArchiveClip> GetData();


        List<ArchiveClip> GetArchiveClipBySearchTerm(Guid p_ClientGUID, string p_SearchTerm, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount);

        List<ArchiveClip> GetArchiveClipBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_ClipFromDate, DateTime? p_ClipToDate, string p_ListCategory1GUID, string p_ListCategory2GUID, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, bool p_IsNielSenRights, out int p_TotalRecordsCount);

        List<ArchiveClip> GetArchiveClipBySearchNew(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_ClipFromDate, DateTime? p_ClipToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, bool p_IsNielSenRights, out int p_TotalRecordsCount);

        List<ArchiveClip> GetArchiveClipByParams(Guid p_ClientGUID, string p_ListCategoryGUID, string p_ListSubCategory1GUID, string p_ListSubCategory2GUID, string p_ListSubCategory3GUID, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending,string p_SearchText,string ClipTitle,out Guid? _ClipID, out int p_TotalRecordsCount);

        List<ArchiveClip> GetArchiveClipReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        List<ArchiveClip> GetArchiveClipByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid);

    }
}
