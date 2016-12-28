using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Xml.Linq;
using System.Data;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQCustomer_SavedSearchController : IIQCustomer_SavedSearchController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQCustomer_SavedSearchModel _IIQCustomer_SavedSearchModel;

        public IQCustomer_SavedSearchController()
        {
            _IIQCustomer_SavedSearchModel = _ModelFactory.CreateObject<IIQCustomer_SavedSearchModel>();
        }

        public string InsertCustomerSearch(Guid p_CustomerGuid, XDocument p_IQPremiumSearchRequestXml, string p_Title, string p_Description, Guid p_CategoryGuid, Boolean p_IsDefualtSearch, Boolean p_IsIQAgent, Guid p_ClientGUID, out int p_OutputStatus, out string p_OutputTitle, out int P_ID, out int p_IQAgentStatus)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IIQCustomer_SavedSearchModel.InsertCustomerSearch(p_CustomerGuid, p_IQPremiumSearchRequestXml, p_Title, p_Description, p_CategoryGuid, p_IsDefualtSearch, p_IsIQAgent, p_ClientGUID, out p_OutputStatus, out p_OutputTitle, out P_ID, out p_IQAgentStatus);
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
                _Result = _IIQCustomer_SavedSearchModel.UpdateCustomerSearch(savedSearch, p_ClientGUID,isSearchTermEqual, out p_OutputStatus, out p_OutputTitle, out p_IQAgentStatus);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<SavedSearch> GetSavedSearchBasedOnCustomerGUID(Guid customerGUID, int pageNumber, int pageSize, int? DefaultSavedSearchID, out int TotalRecords)
        {
            try
            {
                DataSet _Result;

                _Result = _IIQCustomer_SavedSearchModel.GetSavedSearchBasedOnCustomerGUID(customerGUID, pageNumber, pageSize, DefaultSavedSearchID, out TotalRecords);

                var listOfSavedSearch = FillSavedSearchDictData(_Result);
                return listOfSavedSearch;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SavedSearch> FillSavedSearchDictData(DataSet dtset)
        {
            try
            {
                // Dictionary<int, String> dictData = new Dictionary<int, String>();
                var listOfSavedSearch = new List<SavedSearch>();

                if (dtset != null && dtset.Tables != null && dtset.Tables.Count > 0)
                {
                    foreach (DataRow dr in dtset.Tables[0].Rows)
                    {
                        var savedSearch = new SavedSearch();

                        if (dtset.Tables[0].Columns.Contains("ID"))
                        {
                            savedSearch.ID = Convert.ToInt32(dr["ID"]);
                        }

                        if (dtset.Tables[0].Columns.Contains("Title"))
                        {
                            savedSearch.Title = Convert.ToString(dr["Title"]);
                        }

                        if (dtset.Tables[0].Columns.Contains("CustomerGUID"))
                        {
                            savedSearch.CustomerGUID = new Guid(Convert.ToString(dr["CustomerGUID"]));
                        }

                        if (dtset.Tables[0].Columns.Contains("IsIQAgent"))
                        {
                            savedSearch.IsIQAgent = string.IsNullOrWhiteSpace(Convert.ToString(dr["IsIQAgent"])) ? false : Convert.ToBoolean(dr["IsIQAgent"]);
                        }

                        int ID = 0;
                        string Title = string.Empty;
                        //if (dtset.Tables[0].Columns.Contains("ID"))
                        //{
                        //    ID = Convert.ToInt32(dr["ID"]);
                        //}

                        //if (dtset.Tables[0].Columns.Contains("Title"))
                        //{
                        //    Title = Convert.ToString(dr["Title"]);
                        //}
                        //dictData.Add(ID, Title);

                        listOfSavedSearch.Add(savedSearch);
                    }

                }
                return listOfSavedSearch;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<SavedSearch> GetDataByID(Int32 ID)
        {
            try
            {
                DataSet _Result;
                List<SavedSearch> lstSavedSearch = new List<SavedSearch>();
                _Result = _IIQCustomer_SavedSearchModel.GetDataByID(ID);
                lstSavedSearch = FillSavedSearchList(_Result);
                return lstSavedSearch;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SavedSearch> FillSavedSearchList(DataSet dtSavedSearch)
        {
            try
            {

                List<SavedSearch> lstSavedSearch = new List<SavedSearch>();
                if (dtSavedSearch != null && dtSavedSearch.Tables != null && dtSavedSearch.Tables.Count > 0)
                {
                    foreach (DataRow dr in dtSavedSearch.Tables[0].Rows)
                    {
                        SavedSearch savedSearch = new SavedSearch();
                        int ID = 0;
                        string Title = string.Empty;
                        if (dtSavedSearch.Tables[0].Columns.Contains("CustomerGuid"))
                        {
                            savedSearch.CustomerGUID = new Guid(Convert.ToString(dr["CustomerGuid"]));
                        }

                        if (dtSavedSearch.Tables[0].Columns.Contains("IQPremiumSearchRequest"))
                        {
                            savedSearch.IQPremiumSearchRequestXml = Convert.ToString(dr["IQPremiumSearchRequest"]);
                        }

                        if (dtSavedSearch.Tables[0].Columns.Contains("Title"))
                        {
                            savedSearch.Title = Convert.ToString(dr["Title"]);
                        }

                        if (dtSavedSearch.Tables[0].Columns.Contains("Description"))
                        {
                            savedSearch.Description = Convert.ToString(dr["Description"]);
                        }

                        if (dtSavedSearch.Tables[0].Columns.Contains("CategoryGuid"))
                        {
                            savedSearch.CategoryGuid = new Guid(Convert.ToString(dr["CategoryGuid"]));
                        }

                        if (dtSavedSearch.Tables[0].Columns.Contains("ID"))
                        {
                            savedSearch.ID = Convert.ToInt32(dr["ID"]);
                        }

                        if (dtSavedSearch.Tables[0].Columns.Contains("IsDefualtSearch"))
                        {
                            savedSearch.IsDefualtSearch = Convert.ToBoolean(dr["IsDefualtSearch"]);
                        }

                        if (dtSavedSearch.Tables[0].Columns.Contains("IsIQAgent") && !dr["IsIQAgent"].Equals(DBNull.Value))
                        {
                            savedSearch.IsIQAgent = Convert.ToBoolean(dr["IsIQAgent"]);
                        }


                        lstSavedSearch.Add(savedSearch);
                    }

                }

                return lstSavedSearch;
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
                String _Result = string.Empty;
                _Result = _IIQCustomer_SavedSearchModel.DeleteCustomerSavedSearch(ID);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SavedSearch> GetDefaultSearchByCustomerGuid(Guid p_CustomerGuid)
        {
            try
            {
                string _Result = string.Empty;

                DataSet _DataSet = _IIQCustomer_SavedSearchModel.GetDefaultSearchByCustomerGuid(p_CustomerGuid);
                List<SavedSearch> _ListOfSavedSearch = FillSavedSearchList(_DataSet);
                return _ListOfSavedSearch;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SavedSearch> GetSavedSearchBasedOnClientGUID(Guid customerGUID, Guid clientGUID, int pageNumber, int pageSize, int? DefaultSavedSearchID, out int TotalRecords)
        {
            try
            {
                DataSet _Result;

                _Result = _IIQCustomer_SavedSearchModel.GetSavedSearchBasedOnClientGUID(customerGUID, clientGUID, pageNumber, pageSize, DefaultSavedSearchID, out TotalRecords);

                var listOfSavedSearch = FillSavedSearchDictData(_Result);
                return listOfSavedSearch;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetSearchTermByCustomerSavedSearchID(Int64 ID, Guid ClientGUID)
        {
            try
            {
                DataSet _Result;
                string returnString = string.Empty;

                _Result = _IIQCustomer_SavedSearchModel.GetSearchTermByCustomerSavedSearchID(ID, ClientGUID);
                if (_Result != null && _Result.Tables.Count > 0 && _Result.Tables[0] != null && _Result.Tables[0].Rows.Count > 0)
                {
                    returnString = Convert.ToString(_Result.Tables[0].Rows[0]["SearchTerm"]);
                }

                return returnString;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
