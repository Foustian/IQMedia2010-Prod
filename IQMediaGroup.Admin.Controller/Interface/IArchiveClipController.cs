using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Admin.Controller.Interface
{
    public interface IArchiveClipController
    {
        /// <summary>
        /// This method inserts Client Role Information
        /// </summary>
        /// <param name="p_ArchiveClips">Object Of ArchiveClips Class</param>
        /// <returns></returns>
        string InsertArchiveClip(ArchiveClip p_ArchiveClips);

        /// <summary>
        /// Description: This Methods gets Archive Clip Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">List of Object of Archive Clip</param>
        /// <returns>List of Object of Archive Clip</returns>
        List<ArchiveClip> GetArchiveClip(ArchiveClip p_ArchiveClip);

        /// <summary>
        /// Description: This Methods gets Archive Clip by ClipID Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">ClientID of Client</param>
        /// <returns>List of Object of Archive Clip </returns>
        List<ArchiveClip> GetArchiveClipByClipID(ArchiveClip p_ArchiveClip);

        /// <summary>
        /// Description: This Methods gets Archive Clip by ClipID Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">ClientID of Client</param>
        /// <returns>List of Object of Archive Clip by ClipID</returns>
        List<ArchiveClip> GetArchiveClipBySearchText(ArchiveClip p_ArchiveClip);

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
        /// </summary>
        /// <param name="p_DeleteArchiveClip">DeleteArchiveClip</param>
        /// <returns>Count</returns>
        string DeleteArchiveClip(string p_DeleteArchiveClip);
    }
}
