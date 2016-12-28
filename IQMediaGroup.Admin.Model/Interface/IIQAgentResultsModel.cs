using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;
using IQMediaGroup.Core.Enumeration;
namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface of Client Role
    /// </summary>
    public interface IIQAgentResultsModel
    {
        /// <summary>
        /// Description:This method get Search Request  Information.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_SearchRequest">object of SearchRequest</param>
        /// <returns>Dataset of search request</returns>
        DataSet GetRequestByQueryName(IQAgentResults _IQAgentResults);

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
        DataSet GetIQAgentResultBySearchRequest(int p_SearchRequestID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscSortDirection, out string p_SearchTerm);

        /// <summary>
        /// Description:This method delete IQAgentResult.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_IQAgentResultKeys">Primary Key of IQAgentResult</param>
        /// <returns>Count</returns>
        string DeleteIQAgentResult(string p_IQAgentResultKeys);

        ///// <summary>
        ///// This method inserts IQAgentResults details
        ///// </summary>
        ///// <param name="p_IQAgentResults">Object of Class IQAgentResults</param>
        ///// <returns>return Primary Key if details inserted successfully</returns>
        //string InsertIQAgentResult(IQAgentResults p_IQAgentResults);

        /// <summary>
        /// Description:This method will insert IQAgentResultsList.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SqlXml">XML</param>
        /// <returns>Primary Key of IQAgentResult.</returns>
        string InsertIQAgentResultsList(SqlXml p_SqlXml);

        /// <summary>
        /// Description:This method will insert IQAgentResultsList.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SqlXml">XML</param>
        /// <param name="p_ConnectionStringKeys">Name Of ConnectionString</param>
        /// <returns>Primary Key of IQAgentResult.</returns>
        string InsertIQAgentResultsList(SqlXml p_SqlXml, ConnectionStringKeys p_ConnectionStringKeys);

        /// <summary>
        /// This represents parent child relation ship result grid.
        /// </summary>
        /// <param name="_IQAgentResults"></param>
        /// <returns></returns>
        DataSet SelectForParentChildRelationship(IQAgentResults _IQAgentResults);
    }
}
