using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Admin.Core.Enumeration;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    internal class SearchLogController : ISearchLogController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ISearchLogModel _ISearchLogModel;

        public SearchLogController()
        {
            _ISearchLogModel = _ModelFactory.CreateObject<ISearchLogModel>();
        }

        /// <summary>
        /// Description:This method will insert SearchLog.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SearchLog">Object of SearchLog</param>
        /// <returns>Primary key of SearchLog</returns>
        public string InsertSearchLog(SearchLog p_SearchLog)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _ISearchLogModel.InsertServiceLog(p_SearchLog);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will insert SearchLog.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SearchLog">Object of SearchLog</param>
        /// <param name="p_ConnectionStringKeys">Name Of ConnectionString</param>
        /// <returns>Primary key of SearchLog</returns>
        public string InsertSearchLog(SearchLog p_SearchLog, ConnectionStringKeys p_ConnectionStringKeys)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _ISearchLogModel.InsertServiceLog(p_SearchLog, p_ConnectionStringKeys);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
