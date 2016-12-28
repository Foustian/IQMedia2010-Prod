using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;
using System.Xml.Linq;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Model.Implementation
{
    internal class IQCustomer_SavedSearchModel : IQMediaGroupDataLayer, IIQCustomer_SavedSearchModel
    {
        public string InsertCustomerSearch(Guid p_CustomerGuid, XDocument p_IQPremiumSearchRequestXml, string p_Title, string p_Description, Guid p_CategoryGuid, Boolean p_IsDefualtSearch, Boolean p_IsIQAgent, Guid p_ClientGUID, out int p_OutputStatus, out string p_OutputTitle, out int P_ID, out int p_IQAgentStatus)
        {
            try
            {
                string _Result = string.Empty;
                p_OutputStatus = 0;
                p_OutputTitle = null;
                P_ID = 0;
                p_IQAgentStatus = 0;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQPremiumSearchRequest", DbType.Xml, p_IQPremiumSearchRequestXml.ToString(), ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title", DbType.String, p_Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, p_Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGuid", DbType.Guid, p_CategoryGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsDefualtSearch", DbType.Boolean, p_IsDefualtSearch, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsIQAgent", DbType.Boolean, p_IsIQAgent, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@OutputStatus", DbType.Int32, p_OutputStatus, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@OutputTitle", DbType.String, p_OutputTitle, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@OutputID", DbType.String, P_ID, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@IQAgentStatus", DbType.Int32, p_IQAgentStatus, ParameterDirection.Output));


                Dictionary<string, string> _OutputParams = null;

                _Result = ExecuteNonQuery("usp_IQCustomer_SavedSearch_Insert", _ListOfDataType, out _OutputParams);

                if (_OutputParams != null && _OutputParams.Count > 0)
                {
                    if (string.IsNullOrEmpty(_OutputParams["@OutputStatus"]))
                    {
                        p_OutputStatus = 0;
                    }
                    else
                    {
                        p_OutputStatus = Convert.ToInt32(_OutputParams["@OutputStatus"]);
                    }

                    if (string.IsNullOrEmpty(_OutputParams["@OutputID"]))
                    {
                        P_ID = 0;
                    }
                    else
                    {
                        P_ID = Convert.ToInt32(_OutputParams["@OutputID"]);
                    }

                    if (string.IsNullOrEmpty(_OutputParams["@IQAgentStatus"]))
                    {
                        p_IQAgentStatus = 0;
                    }
                    else
                    {
                        p_IQAgentStatus = Convert.ToInt32(_OutputParams["@IQAgentStatus"]);
                    }


                    p_OutputTitle = Convert.ToString(_OutputParams["@OutputTitle"]);
                }

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateCustomerSearch(SavedSearch savedSearch, Guid p_ClientGUID, bool isSearchTermEqual, out int p_OutputStatus, out string p_OutputTitle, out int p_IQAgentStatus)
        {
            try
            {
                string _Result = string.Empty;
                p_OutputStatus = 0;
                p_IQAgentStatus = 0;
                p_OutputTitle = null;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, savedSearch.CustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQPremiumSearchRequest", DbType.Xml, savedSearch.IQPremiumSearchRequestXml.ToString(), ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title", DbType.String, savedSearch.Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, savedSearch.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGuid", DbType.Guid, savedSearch.CategoryGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsDefualtSearch", DbType.Boolean, savedSearch.IsDefualtSearch, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsIQAgent", DbType.Boolean, savedSearch.IsIQAgent, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ID", DbType.Int32, savedSearch.ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsSearchTermEqual", DbType.Boolean, isSearchTermEqual, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@OutputStatus", DbType.Int32, p_OutputStatus, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@OutputTitle", DbType.String, p_OutputTitle, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@IQAgentStatus", DbType.Int32, p_IQAgentStatus, ParameterDirection.Output));

                Dictionary<string, string> _OutputParams = null;

                _Result = ExecuteNonQuery("usp_IQCustomer_SavedSearch_Update", _ListOfDataType, out _OutputParams);

                if (_OutputParams != null && _OutputParams.Count > 0)
                {
                    if (string.IsNullOrEmpty(_OutputParams["@OutputStatus"]))
                    {
                        p_OutputStatus = 0;
                    }
                    else
                    {
                        p_OutputStatus = Convert.ToInt32(_OutputParams["@OutputStatus"]);
                    }

                    if (string.IsNullOrEmpty(_OutputParams["@IQAgentStatus"]))
                    {
                        p_IQAgentStatus = 0;
                    }
                    else
                    {
                        p_IQAgentStatus = Convert.ToInt32(_OutputParams["@IQAgentStatus"]);
                    }


                    p_OutputTitle = Convert.ToString(_OutputParams["@OutputTitle"]);
                }




                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetSavedSearchBasedOnCustomerGUID(Guid customerGUID, int pageNumber, int pageSize, int? DefaultSavedSearchID, out int TotalRecords)
        {
            try
            {
                TotalRecords = 0;
                DataSet _Result;

                List<DataType> _ListOfDataType = new List<DataType>();
                Dictionary<string, string> _OutputParams = null;


                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, customerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, pageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultSavedSearchID", DbType.Int32, DefaultSavedSearchID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TotalRecords", DbType.Int32, TotalRecords, ParameterDirection.Output));

                _Result = GetDataSetWithOutParam("usp_IQCustomer_SavedSearch_ByCustomerGUID", _ListOfDataType, out _OutputParams);

                if (_OutputParams != null && _OutputParams.Count > 0)
                {
                    TotalRecords = Convert.ToInt32(_OutputParams["@TotalRecords"]);
                }
                return _Result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataSet GetDataByID(Int32 ID)
        {
            try
            {
                DataSet _Result;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ID", DbType.Int32, ID, ParameterDirection.Input));
                _Result = GetDataSet("usp_IQCustomer_SavedSearch_GetDataByID", _ListOfDataType);

                return _Result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public String DeleteCustomerSavedSearch(String ID)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ID", DbType.String, ID, ParameterDirection.Input));
                _Result = ExecuteNonQuery("usp_IQCustomer_SavedSearch_Delete", _ListOfDataType);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet GetDefaultSearchByCustomerGuid(Guid p_CustomerGuid)
        {

            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_IQCustomer_DefaultSettings_SelectDefaultSettingsByCustomerGuid", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataSet GetSavedSearchBasedOnClientGUID(Guid customerGUID, Guid clientGUID, int pageNumber, int pageSize, int? DefaultSavedSearchID, out int TotalRecords)
        {
            try
            {
                TotalRecords = 0;
                DataSet _Result;

                List<DataType> _ListOfDataType = new List<DataType>();
                Dictionary<string, string> _OutputParams = null;


                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, customerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, clientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, pageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultSavedSearchID", DbType.Int32, DefaultSavedSearchID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TotalRecords", DbType.Int32, TotalRecords, ParameterDirection.Output));

                _Result = GetDataSetWithOutParam("usp_IQCustomer_SavedSearch_SelectByClientGUID", _ListOfDataType, out _OutputParams);

                if (_OutputParams != null && _OutputParams.Count > 0)
                {
                    TotalRecords = Convert.ToInt32(_OutputParams["@TotalRecords"]);
                }
                return _Result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataSet GetSearchTermByCustomerSavedSearchID(Int64 ID, Guid ClientGUID)
        {
            try
            {
                DataSet _Dataset = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@CustomerSavedSearchID", DbType.String, ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, ClientGUID, ParameterDirection.Input));
                _Dataset = this.GetDataSet("usp_IQAgent_SearchRequest_SelectSearchTermByCustomerSavedSearchID", _ListOfDataType);
                return _Dataset;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
