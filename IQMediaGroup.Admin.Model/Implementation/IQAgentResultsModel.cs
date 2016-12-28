using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Admin.Model.Interface;
using System.Data;
using System.Data.SqlTypes;
using IQMediaGroup.Core.Enumeration;


namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface ICustomerModel
    /// </summary>
    internal class IQAgentResultsModel : IQMediaGroupDataLayer, IIQAgentResultsModel
    {
        /// <summary>
        /// Description:This method get Search Request  Information.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_SearchRequest">object of SearchRequest</param>
        /// <returns>Dataset of search request</returns>
        public DataSet GetRequestByQueryName(IQAgentResults _IQAgentResults)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.String, _IQAgentResults.SearchRequestID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_IQAgentResults_SelectByQueryName", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

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
        public DataSet GetIQAgentResultBySearchRequest(int p_SearchRequestID,int p_PageNumber,int p_PageSize,string p_SortField,bool p_IsAscSortDirection,out string p_SearchTerm)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                p_SearchTerm = string.Empty;

                _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.Int32, p_SearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, p_PageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortDirection", DbType.Boolean, p_IsAscSortDirection, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@SearchTerm", DbType.String, p_SearchTerm, ParameterDirection.Output));

                _DataSet = this.GetDataSet("usp_IQAgentResults_SelectByQueryName", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method delete IQAgentResult.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_IQAgentResultKeys">Primary Key of IQAgentResult</param>
        /// <returns>Count</returns>
        public string DeleteIQAgentResult(string p_IQAgentResultKeys)
        {
            try
            {

                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@IQAgentResultKeys", DbType.String, p_IQAgentResultKeys, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_IQAgentResults_Delete", _ListOfDataType);

                return _Result;

            }
            catch (Exception _Exception)
            {
                
                throw _Exception;
            }
        }

        ///// <summary>
        ///// This method inserts IQAgentResults details
        ///// </summary>
        ///// <param name="p_IQAgentResults">Object of Class IQAgentResults</param>
        ///// <returns>return Primary Key if details inserted successfully</returns>
        //public string InsertIQAgentResult(IQAgentResults p_IQAgentResults)
        //{
        //    try
        //    {
        //        string _Result = string.Empty;

        //        List<DataType> _ListOfDataType = new List<DataType>();

        //        _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.Int64, p_IQAgentResults.SearchRequestID, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@RL_VideoGUID", DbType.Guid, p_IQAgentResults.RL_VideoGUID, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@Rl_Station", DbType.String, p_IQAgentResults.Rl_Station, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@RL_Date", DbType.Date, p_IQAgentResults.RL_Date, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@RL_Time", DbType.Int32, p_IQAgentResults.RL_Time, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@RL_Market", DbType.String, p_IQAgentResults.RL_Market, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@Number_Hits", DbType.Int32, p_IQAgentResults.Number_Hits, ParameterDirection.Input));
        //        //_ListOfDataType.Add(new DataType("@CC_Text", DbType.String, p_IQAgentResults.CC_Text, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@Communication_flag", DbType.Boolean, p_IQAgentResults.Communication_flag, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@CreatedDate", DbType.DateTime, p_IQAgentResults.CreatedDate, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@ModifiedDate", DbType.DateTime, p_IQAgentResults.ModifiedDate, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@CreatedBy", DbType.String, p_IQAgentResults.CreatedBy, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@ModifiedBy", DbType.String, p_IQAgentResults.ModifiedBy, ParameterDirection.Input));
        //        _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_IQAgentResults.IsActive, ParameterDirection.Input));

        //        _ListOfDataType.Add(new DataType("@IQAgentResultKey", DbType.Int64, p_IQAgentResults.IQAgentResultKey, ParameterDirection.Output));

        //        _Result = ExecuteNonQuery("usp_IQAgentResults_Insert", _ListOfDataType);

        //        return _Result;

        //    }
        //    catch (Exception _Exception)
        //    {                
        //        throw _Exception; 
        //    }
        //}

        /// <summary>
        /// Description:This method will insert IQAgentResultsList.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SqlXml">XML</param>
        /// <returns>Primary Key of IQAgentResult.</returns>
        public string InsertIQAgentResultsList(SqlXml p_SqlXml)
        {
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@XmlData", DbType.Xml, p_SqlXml, ParameterDirection.Input));

                string _ReturnValue = this.ExecuteNonQuery("usp_IQAgentResult_InsertList", _ListOfDataType);

                return _ReturnValue;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will insert IQAgentResultsList.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SqlXml">XML</param>
        /// <param name="p_ConnectionStringKeys">Name Of ConnectionString</param>
        /// <returns>Primary Key of IQAgentResult.</returns>
        public string InsertIQAgentResultsList(SqlXml p_SqlXml, ConnectionStringKeys p_ConnectionStringKeys)
        {
            try
            {
                this.CONNECTION_STRING_KEY = p_ConnectionStringKeys.ToString();

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@XmlData", DbType.Xml, p_SqlXml, ParameterDirection.Input));

                string _ReturnValue = this.ExecuteNonQuery("usp_IQAgentResult_InsertList", _ListOfDataType);

                return _ReturnValue;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method get Search Request  Information.
        /// </summary>
        /// <param name="_SearchRequest">object of SearchRequest</param>
        /// <returns>Dataset of search request</returns>
        public DataSet SelectForParentChildRelationship(IQAgentResults _IQAgentResults)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.Int64, _IQAgentResults.SearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNo", DbType.Int32, _IQAgentResults.PageNo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, _IQAgentResults.PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, _IQAgentResults.SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, _IQAgentResults.IsAscending, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_IQAgentSearchRequest_SelectForParentChildRelationship", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
