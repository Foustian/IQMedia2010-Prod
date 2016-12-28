using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Controller.Interface
{
    public interface ISearchRequestController
    {
        //COMMENTED BY MEGHANA ON 15-MARCH-2012 
        ///// <summary>
        ///// This method inserts Search Request Information
        ///// </summary>
        ///// <param name="p_SearchRequests">Object Of SearchRequests Class</param>
        ///// <returns>SearchRequestKey</returns>
        //string InsertSearchRequest(IQAgentSearchRequest p_SearchRequest);

        ///// <summary>
        ///// Description:This method update Search Request  Information.
        ///// Added By:Maulik Gandhi
        ///// </summary>
        ///// <param name="p_IQAgentSearchRequest">object of SearchRequest</param>
        ///// <returns>string</returns>
       //string UpdateSearchRequest(IQAgentSearchRequest p_IQAgentSearchRequest);

        ///// <summary>
        ///// Description: This Methods gets Search request Information from DataSet.
        ///// Added By:Maulik gandhi
        ///// </summary>
        ///// <returns>List of Object of Search Request</returns>
        //List<IQAgentSearchRequest> GetSearchRequestByQueryName(IQAgentSearchRequest p_IQAgentSearchRequest);

        List<IQAgentSearchRequest> SelectByClientID(Guid ClientGuid);

        /// <summary>
        /// This method gets all SearchTerm and SearchRequestKey with unique QueryName and max Query Version
        /// </summary>
        /// <returns>List of object of IQAgentSearchRequest</returns>
        //List<IQAgentSearchRequest> GetSearchRequestAll();

        
        List<IQAgentSearchRequest> GetSearchRequestsByClientIDQueryName(Guid p_ClientGuid,string p_QueryName);

        /// <summary>
        /// This method gets SearchRequest using parameters
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <param name="p_QueryName">Query Name</param>
        /// <param name="p_QueryVersion">Query Version</param>
        /// <returns></returns>
        List<IQAgentSearchRequest> GetSearchRequestsByClientIDQueryNameVersion(Guid p_ClientGuid, string p_QueryName, int p_QueryVersion);

        int SearchRawMediaFromPMGSearch(string p_FilePath);

        //COMMENTED BY MEGHANA ON 15-MARCH-2012 
        //string UpdateIsActive(long p_SearchRequestKey, bool p_IsActive);

        //List<IQAgentSearchRequest> SelectAllByClientID(long p_ClientID);

        ///// <summary>
        ///// This method gets all SearchTerm and SearchRequestKey with unique QueryName and max Query Version
        ///// </summary>
        ///// <param name="p_ConnectionStringKeys">Name Of ConnectionString</param>
        ///// <returns>List of object of IQAgentSearchRequest</returns>
        //List<IQAgentSearchRequest> GetSearchRequestAll(ConnectionStringKeys p_ConnectionStringKeys);
    }
}
