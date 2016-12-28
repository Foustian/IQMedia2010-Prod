using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.Enumeration;
using System.Web;

namespace IQMediaGroup.Controller.Implementation
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

        public string InsertSearchLog(HttpContext currentContext, string SearchType, string _Terms, int _PageNumber, int _PageSize, int _MaxHighlights, DateTime? _StartDate, DateTime? _EndDate, string _Response, String pmgSearchURL)
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = (SessionInformation)currentContext.Session["SessionInformation"];

                string _FileContent = string.Empty;
                _FileContent = "<PMGRequest>";
                _FileContent += "<Terms>" + HttpUtility.UrlEncode(_Terms) + "</Terms>";
                _FileContent += "<PageNumber>" + _PageNumber + "</PageNumber>";
                _FileContent += "<PageSize>" + _PageSize + "</PageSize>";
                _FileContent += "<MaxHighlights>" + _MaxHighlights + "</MaxHighlights>";
                _FileContent += "<PMGSearchURL>" + HttpUtility.UrlEncode(pmgSearchURL) + "</PMGSearchURL>";
                if (_StartDate.HasValue)
                {
                    _FileContent += "<StartDate>" + _StartDate + "</StartDate>";
                }
                else
                {
                    _FileContent += "<StartDate></StartDate>";
                }
                if (_EndDate.HasValue)
                {
                    _FileContent += "<EndDate>" + _EndDate + "</EndDate>";
                }
                else
                {
                    _FileContent += "<EndDate></EndDate>";
                }
                _FileContent += "</PMGRequest>";

                string _Result = string.Empty;

                SearchLog _SearchLog = new SearchLog();
                _SearchLog.CustomerID = _SessionInformation.CustomerKey;
                _SearchLog.SearchType = SearchType;
                _SearchLog.RequestXML = _FileContent;
                _SearchLog.ErrorResponseXML = _Response;
                _Result = InsertSearchLog(_SearchLog);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
