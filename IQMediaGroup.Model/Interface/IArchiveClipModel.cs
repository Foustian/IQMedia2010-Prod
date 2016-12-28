using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;
namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface of Client Role
    /// </summary>
    public interface IArchiveClipModel
    {
        /// <summary>
        /// Description:This Method will Insert Clip.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_ArchiveClip">Object of ArchiveClip</param>
        /// <returns>Primary Key of Archive Clip.</returns>
        string InsertClip(ArchiveClip _ArchiveClip);

        /// <summary>
        /// Description:This Method will get Archive Clip.
        /// </summary>
        /// <param name="_ArchiveClip">Object of ArchiveClip</param>
        /// <returns>DataSet of ArchiveClip.</returns>
        DataSet GetArchiveClip(ArchiveClip _ArchiveClip);

        /// <summary>
        /// Description:This Method will get Archive clip by ClipID
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_ArchiveClip">object of ArchiveClip</param>
        /// <returns>DataSet of ArchiveClip.</returns>
        DataSet GetArchiveClipByClipID(ArchiveClip _ArchiveClip);


        /// <summary>
        /// Description:This Method will get Archive clip by Date.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_StartDate">Start Date of Clip.</param>
        /// <param name="p_EndDate">End Date of Clip.</param>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <returns>DataSet of ArchiveClip.</returns>
        DataSet GetArchiveClipByDate(DateTime p_StartDate, DateTime p_EndDate, int p_CustomerID);

        /// <summary>
        /// Description:This Method will get Archive clip by CustomerID.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <returns>DataSet of ArchiveClip.</returns>
        DataSet GetArchiveClipByCustomer(Int64 p_CustomerID);

        /// <summary>
        /// Description:This Method will Insert Clip.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SqlXml">XML</param>
        /// <returns>Primary Key of Archive Clip</returns>
        string InsertArchiveClip(SqlXml p_SqlXml);

        DataSet GetArchiveClipFromXML(SqlXml ClipXML);

        /// <summary>
        ///  Description:This Method will Delete Clip.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_DeleteArchiveClip">Primary Key of Archive Clip</param>
        /// <returns>Count</returns>
        string DeleteArchiveClip(string p_DeleteArchiveClip);

        DataSet GetAllArchiveClip(ArchiveClip _ArchiveClip);        

        string UpdateArchiveClip(ArchiveClip p_ArchiveClip);

        DataSet GetArchiveClipByCustomerGUID(Guid p_CustomerGUID);

        string InsertArchiveClipFromConsole(ArchiveClip _ArchiveClip);

        DataSet GetData();

        DataSet GetArchiveClipBySearchTerm(Guid p_ClientGUID, string p_SearchTerm, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount);

        DataSet GetArchiveClipBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_ClipFromDate, DateTime? p_ClipToDate, string p_ListCategory1GUID, string p_ListCategory2GUID, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending,bool p_IsNielSenRights, out int p_TotalRecordsCount);

        DataSet GetArchiveClipBySearchNew(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_ClipFromDate, DateTime? p_ClipToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, bool p_IsNielSenRights, out int p_TotalRecordsCount);

        DataSet GetArchiveClipByParams(Guid p_ClientGUID, string p_ListCategoryGUID, string p_ListSubCategory1GUID, string p_ListSubCategory2GUID, string p_ListSubCategory3GUID, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, string p_SearchText, string ClipTitle, out Guid? _ClipID, out int p_TotalRecordsCount);

        DataSet GetArchiveClipReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date);

        DataSet GetArchiveClipByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid);
    }
}
