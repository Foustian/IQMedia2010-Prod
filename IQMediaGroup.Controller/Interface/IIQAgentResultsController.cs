using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Controller.Interface
{
    public interface IIQAgentResultsController
    {
        /// <summary>
        /// This method inserts Search Responce Information
        /// </summary>
        /// <param name="p_SearchRequests">Object Of Search Responce Class</param>
        /// <returns>SearchResponceKey</returns>
        //string InsertSearchResponce(IQAgentSearchResponce p_SearchResponce);
        MasterIQResults GetSearchRequestByQueryName(IQAgentResults p_IQAgentResults);

        /// <summary>
        /// Description:This Method will Delete IQAgentResult
        /// </summary>
        /// <param name="p_IQAgentResultKeys">IQAgentResultKeys</param>
        /// <returns>Count</returns>
        string DeleteIQAgentResult(string p_IQAgentResultKeys);

        /// <summary>
        /// This method inserts IQAgentResults details
        /// </summary>
        /// <param name="p_IQAgentResults">Object of Class IQAgentResults</param>
        /// <returns>return Primary Key if details inserted successfully</returns>
        string InsertIQAgentResult(IQAgentResults p_IQAgentResults);

        /// <summary>
        /// Description:This Method will Insert IQAgentResult
        /// </summary>
        /// <param name="p_SqlXml">XML</param>
        /// <returns>Primary key of IQAgentResult</returns>
        string InsertIQAgentResultsList(SqlXml p_SqlXml);

        /// <summary>
        /// This method gets IQAgentResults and SearchTerm by SearchRequestID
        /// </summary>
        /// <param name="p_SearchRequestID">SearchRequestID</param>
        /// <param name="p_PageNumber">Page Number</param>
        /// <param name="p_PageSize">Page Size</param>
        /// <param name="p_SortField">Field on which sorting applied</param>
        /// <param name="p_IsAscSortDirection">Sort Direction is Ascending or not</param>
        /// <param name="p_SearchTerm">Search Term</param>
        /// <returns>DataSet contains IQAgentResults details</returns>
        List<IQAgentResults> GetIQAgentResultBySearchRequest(int p_SearchRequestID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_SortDirection, out string p_SearchTerm);

        /// <summary>
        /// Description:This Method will Insert IQAgentResult
        /// </summary>
        /// <param name="p_SqlXml">XML</param>
        /// <param name="p_ConnectionStringKeys">Name of ConnectionString</param>
        /// <returns>Primary key of IQAgentResult</returns>
        string InsertIQAgentResultsList(SqlXml p_SqlXml, ConnectionStringKeys p_ConnectionStringKeys);

        /// <summary>
        /// This represents Parent Child relation ship for result grid.
        /// </summary>
        /// <param name="_IQAgentResults">Contains details of IQAgentResults</param>
        /// <param name="p_SearchTerm">Search Term</param>
        /// <param name="p_TotalRecords">Total Records</param>
        /// <returns></returns>
        List<IQAgentResults> SelectForParentChildRelationship(IQAgentResults _IQAgentResults, out int p_TotalRecords);
    }

}
