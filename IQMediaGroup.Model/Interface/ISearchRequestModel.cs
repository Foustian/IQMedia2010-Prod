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
    /// Interface of Client Role
    /// </summary>
    public interface ISearchRequestModel
    {
        /// <summary>
        /// Description:This method inserts Search Request  Information.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_SearchRequest">object of SearchRequest</param>
        /// <returns>Search request key</returns>
        string InsertSearchRequest(IQAgentSearchRequest _SearchRequest);

        /// <summary>
        /// Description:This method get Search Request  Information.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_SearchRequest">object of SearchRequest</param>
        /// <returns>Dataset of search request</returns>
        DataSet GetSearchRequestByQueryName(IQAgentSearchRequest _IQAgentSearchRequest);

        /// <summary>
        /// This method gets all SearchTerm and SearchRequestKey with unique QueryName and max Query Version
        /// </summary>
        /// <returns>Dataset contains details of SearchRequest</returns>
        DataSet GetSearchRequestAll();

        /// <summary>
        /// Added By:Dharmesh
        /// </summary>
        /// <param name="ClientGuid">ClientGuid</param>
        /// <returns>Dataset of search request</returns>
        DataSet SelectByClientID(Guid ClientGuid);

        /// <summary>
        /// Description:This Method will get SearchRequests by clientid and query name.
        /// </summary>
        /// <param name="p_ClientGuid">ClientGuid</param>
        /// <param name="p_QueryName">Query Name</param>
        /// <returns>DataSet of Search Request.</returns>
        DataSet GetSearchRequestsByClientIDQueryName(Guid p_ClientGuid, string p_QueryName);

        /// <summary>
        /// This method gets SearchRequest using parameters
        /// </summary>
        /// <param name="p_ClientGuid">ClientGuid</param>
        /// <param name="p_QueryName">Query Name</param>
        /// <param name="p_QueryVersion">Query Version</param>
        /// <returns></returns>
        DataSet GetSearchRequestsByClientIDQueryNameVersion(Guid p_ClientGuid, string p_QueryName, int p_QueryVersion);

        /// <summary>
        /// this method update IsActive flag of record
        /// </summary>
        /// <param name="SearchRequestKey"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        string UpdateIsActive(long p_SearchRequestKey, bool p_IsActive);

        /// <summary>
        /// </summary>
        /// <param name="p_ClientGuid">ClientGuid</param>
        /// <returns>Dataset of search request.This method will also select InActive records as well.</returns>
        DataSet SelectAllByClientID(Guid p_ClientGuid);

        /// <summary>
        /// This method gets all SearchTerm and SearchRequestKey with unique QueryName and max Query Version
        /// </summary>
        /// <param name="p_ConnectionStringKeys">ConnectionString Name</param>
        /// <returns>Dataset contains details of SearchRequest</returns>
        DataSet GetSearchRequestAll(ConnectionStringKeys p_ConnectionStringKeys);

        /// <summary>
        /// This method gets SearchRequest using parameters
        /// </summary>
        /// <param name="p_ClientGuid">ClientGuid</param>
        /// <param name="p_QueryName">Query Name</param>
        /// <param name="p_QueryVersion">Query Version</param>
        /// <param name="p_ConnectionStringKeys">Name of ConnectionString</param>
        /// <returns>Dataset Contains SearchRequest</returns>
        DataSet GetSearchRequestsByClientIDQueryNameVersion(Guid p_ClientGuid, string p_QueryName, int p_QueryVersion, ConnectionStringKeys p_ConnectionStringKeys);

        DataSet GetIqagentIframeData_ByiQAgentiFrameID(Guid iQAgentiFrameID);
    }
}
