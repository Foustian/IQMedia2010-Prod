using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface for Client
    /// </summary>
    public interface ISearchLogModel
    {
        /// <summary>
        /// Description:This Method will Insert ServiceLog.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SearchLog">object of SearchLog</param>
        /// <returns>Primary Key of SearchLog</returns>
        string InsertServiceLog(SearchLog p_SearchLog);

        /// <summary>
        /// Description:This Method will Insert ServiceLog.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SearchLog">object of SearchLog</param>
        /// <param name="p_ConnectionString">ConnectionString</param>
        /// <returns>Primary Key of SearchLog</returns>
        string InsertServiceLog(SearchLog p_SearchLog, ConnectionStringKeys p_ConnectionStringKeys);
    }
}
