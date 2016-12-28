using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Admin.Model.Base;
using System.Data;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Core.Enumeration;

namespace IQMediaGroup.Admin.Model.Implementation
{
    internal class SearchLogModel : IQMediaGroupDataLayer, ISearchLogModel
    {
        /// <summary>
        /// Description:This Method will Insert ServiceLog.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SearchLog">object of SearchLog</param>
        /// <returns>Primary Key of SearchLog</returns>
        public string InsertServiceLog(SearchLog p_SearchLog)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int32, p_SearchLog.CustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchType", DbType.String, p_SearchLog.SearchType, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RequestXML", DbType.Xml, p_SearchLog.RequestXML, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ErrorResponseXML", DbType.String, p_SearchLog.ErrorResponseXML, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@PMGSearchLogKey", DbType.Int32, p_SearchLog.PMGSearchLogKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_PMGSearchLog_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will Insert ServiceLog.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SearchLog">object of SearchLog</param>
        /// <param name="p_ConnectionString">ConnectionString</param>
        /// <returns>Primary Key of SearchLog</returns>
        public string InsertServiceLog(SearchLog p_SearchLog, ConnectionStringKeys p_ConnectionStringKeys)
        {
            try
            {
                this.CONNECTION_STRING_KEY = p_ConnectionStringKeys.ToString();

                string _Result = string.Empty;             
                

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int32, p_SearchLog.CustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchType", DbType.String, p_SearchLog.SearchType, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RequestXML", DbType.Xml, p_SearchLog.RequestXML, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ErrorResponseXML", DbType.String, p_SearchLog.ErrorResponseXML, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@PMGSearchLogKey", DbType.Int32, p_SearchLog.PMGSearchLogKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_PMGSearchLog_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
