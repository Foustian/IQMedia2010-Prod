using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;
namespace IQMediaGroup.Admin.Model.Interface
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
        /// Description:This Method will get Archive clip by Search Text.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_ArchiveClip">object of ArchiveClip</param>
        /// <returns>DataSet of ArchiveClip.</returns>
        DataSet GetArchiveClipBySearchText(ArchiveClip _ArchiveClip);

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

        /// <summary>
        ///  Description:This Method will Delete Clip.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_DeleteArchiveClip">Primary Key of Archive Clip</param>
        /// <returns>Count</returns>
        string DeleteArchiveClip(string p_DeleteArchiveClip);
    }
}
