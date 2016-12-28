﻿using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Model.Interface;
using System.Data;
using IQMediaGroup.Admin.Core.Enumeration;


namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface ICustomerModel
    /// </summary>
    internal class IQAgentSearchRequestModel : IQMediaGroupDataLayer, ISearchRequestModel
    {
        /// <summary>
        /// Description:This method inserts Search Request  Information.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_SearchRequest">object of SearchRequest</param>
        /// <returns>Search request key</returns>
        public string InsertSearchRequest(IQAgentSearchRequest _IQAgentSearchRequest)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int32, _IQAgentSearchRequest.ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_Agent_UserID", DbType.Int32, _IQAgentSearchRequest.IQ_Agent_UserID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Query_Name", DbType.String, _IQAgentSearchRequest.Query_Name, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Query_Version", DbType.Int32, _IQAgentSearchRequest.Query_Version, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTerm", DbType.String, _IQAgentSearchRequest.SearchTerm, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchRequestKey", DbType.Int32, _IQAgentSearchRequest.SearchRequestKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_IQAgentSearchRequest_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method get Search Request  Information.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_SearchRequest">object of SearchRequest</param>
        /// <returns>Dataset of search request</returns>
        public DataSet GetSearchRequestByQueryName(IQAgentSearchRequest _IQAgentSearchRequest)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@Query_Name", DbType.String, _IQAgentSearchRequest.Query_Name, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_IQAgentSearchRequest_SelectByQuery", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Added By:Dharmesh
        /// </summary>
        /// <param name="ClientID">ClientID</param>
        /// <returns>Dataset of search request</returns>
        public DataSet SelectByClientID(long ClientID)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, ClientID, ParameterDirection.Input));
                _DataSet = this.GetDataSet("usp_IQAgentSearchRequest_SelectQueryByClientID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets all SearchTerm and SearchRequestKey with unique QueryName and max Query Version
        /// </summary>
        /// <returns>Dataset contains details of SearchRequest</returns>
        public DataSet GetSearchRequestAll()
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = this.GetDataSetByProcedure("usp_IQAgentSearchRequest_Select");

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will get SearchRequests by clientid and query name.
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <param name="p_QueryName">Query Name</param>
        /// <returns>DataSet of Search Request.</returns>
        public DataSet GetSearchRequestsByClientIDQueryName(long p_ClientID, string p_QueryName)
        {
            try
            {
                DataSet _DataSet = new DataSet();

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Query_Name", DbType.String, p_QueryName, ParameterDirection.Input));
                _DataSet = this.GetDataSet("usp_IQAgentSearchRequest_SelectByClientIDQueryName", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }       

        /// <summary>
        /// This method gets SearchRequest using parameters
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <param name="p_QueryName">Query Name</param>
        /// <param name="p_QueryVersion">Query Version</param>
        /// <param name="p_ConnectionStringKeys">Name of ConnectionString</param>
        /// <returns>Dataset Contains SearchRequest</returns>
        public DataSet GetSearchRequestsByClientIDQueryNameVersion(long p_ClientID, string p_QueryName, int p_QueryVersion, ConnectionStringKeys p_ConnectionStringKeys)
        {
            try
            {
                this.CONNECTION_STRING_KEY = p_ConnectionStringKeys.ToString();

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Query_Name", DbType.String, p_QueryName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Query_Version", DbType.Int32, p_QueryVersion, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_IQAgentSearchRequest_SelectByClientQueryVersion", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// this method update IsActive flag of record
        /// </summary>
        /// <param name="SearchRequestKey"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public string UpdateIsActive(long p_SearchRequestKey,bool p_IsActive,long p_ClientID,string p_Query_Name)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@SearchRequestKey", DbType.Int64, p_SearchRequestKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Int32, p_IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Query_Name", DbType.String, p_Query_Name, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_IQAgentSearchRequest_UpdateIsActive", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="ClientID">ClientID</param>
        /// <returns>Dataset of search request.This method will also select InActive records as well.</returns>
        public DataSet SelectAllByClientID(Guid ClientGUID)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, ClientGUID, ParameterDirection.Input));
                _DataSet = this.GetDataSet("usp_IQAgentSearchRequest_SelectAllByClientID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets all SearchTerm and SearchRequestKey with unique QueryName and max Query Version
        /// </summary>
        /// <param name="p_ConnectionStringKeys">ConnectionString Name</param>
        /// <returns>Dataset contains details of SearchRequest</returns>
        public DataSet GetSearchRequestAll(ConnectionStringKeys p_ConnectionStringKeys)
        {
            try
            {
                this.CONNECTION_STRING_KEY = p_ConnectionStringKeys.ToString();

                DataSet _DataSet = null;

                _DataSet = this.GetDataSetByProcedure("usp_IQAgentSearchRequest_Select");

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
