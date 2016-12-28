using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;
using IQMediaGroup.Core.Enumeration;
namespace IQMediaGroup.Reports.Model.Interface
{
    /// <summary>
    /// Interface of Client Role
    /// </summary>
    public interface IIQAgentResultsModel
    {
        /// <summary>
        /// This method gets IQAgentResults by Date Range
        /// </summary>
        /// <param name="p_FromDate"></param>
        /// <param name="p_ToDate"></param>
        /// <returns>DataSet contains IQAgentResults details</returns>
        DataSet GetIQAgentResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, Boolean p_IsNielSenData, out string Query_Name, out string SearchTerm);
    }
}
