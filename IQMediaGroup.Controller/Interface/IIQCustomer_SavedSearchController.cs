using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using IQMediaGroup.Core.HelperClasses;


namespace IQMediaGroup.Controller.Interface
{
    public interface IIQCustomer_SavedSearchController
    {
        /// <summary>
        /// Inserts Data to Table
        /// </summary>
        /// <param name="CustomerGuid">Guid Of Customer</param>
        /// <param name="xml">Xml For Search Settings</param>
        /// <returns></returns>
        string InsertCustomerSearch(Guid p_CustomerGuid, XDocument p_IQPremiumSearchRequestXml, string p_Title, string p_Description, Guid p_CategoryGuid, Boolean p_IsDefualtSearch, Boolean p_IsIQAgent, Guid p_ClientGUID, out int p_OutputStatus, out string p_OutputTitle, out int P_ID, out int p_IQAgentStatus);
        List<SavedSearch> GetSavedSearchBasedOnCustomerGUID(Guid customerGUID, int pageNumber, int pageSize, int? DefaultSavedSearchID, out int TotalRecords);
        List<SavedSearch> GetDataByID(Int32 ID);
        string UpdateCustomerSearch(SavedSearch savedSearch, Guid p_ClientGUID, bool isSearchTermEqual, out int p_OutputStatus, out string p_OutputTitle, out int p_IQAgentStatus);
        String DeleteCustomerSavedSearch(String ID);


        /// <summary>
        /// Get IQPremium Default Search by CustomerGuid
        /// </summary>
        /// <param name="p_CustomerGuid">Guid Of Customer</param>
        /// <returns></returns>
        List<SavedSearch> GetDefaultSearchByCustomerGuid(Guid p_CustomerGuid);

        List<SavedSearch> GetSavedSearchBasedOnClientGUID(Guid customerGUID, Guid clientGUID, int pageNumber, int pageSize, int? DefaultSavedSearchID, out int TotalRecords);
        string GetSearchTermByCustomerSavedSearchID(Int64 ID, Guid ClientGUID);
    }
}
