using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Web;
using System.Net;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Controller.Interface
{
    /// <summary>
    /// Interface for Client
    /// </summary>
    public interface ISearchLogController
    {
        /// <summary>
        /// Description:This method will insert SearchLog.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SearchLog">Object of SearchLog</param>
        /// <returns>Primary key of SearchLog</returns>
        string InsertSearchLog(SearchLog p_SearchLog);

        /// <summary>
        /// Description:This method will insert SearchLog.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SearchLog">Object of SearchLog</param>
        /// <param name="p_ConnectionStringKeys">Name Of ConnectionString</param>
        /// <returns>Primary key of SearchLog</returns>
        string InsertSearchLog(SearchLog p_SearchLog, ConnectionStringKeys p_ConnectionStringKeys);
        string InsertSearchLog(HttpContext currentContext, string SearchType, string _Terms, int _PageNumber, int _PageSize, int _MaxHighlights, DateTime? _StartDate, DateTime? _EndDate, string _Response, String pmgSearchURL);
    }
}
