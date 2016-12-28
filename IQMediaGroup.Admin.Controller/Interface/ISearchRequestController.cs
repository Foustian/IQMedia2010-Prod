using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Core.Enumeration;

namespace IQMediaGroup.Admin.Controller.Interface
{
    public interface ISearchRequestController
    {
        /// <summary>
        /// This method inserts Search Request Information
        /// </summary>
        /// <param name="p_SearchRequests">Object Of SearchRequests Class</param>
        /// <returns>SearchRequestKey</returns>
        string InsertSearchRequest(IQAgentSearchRequest p_SearchRequest);

        /// <summary>
        /// Description:This method update Search Request  Information.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_IQAgentSearchRequest">object of SearchRequest</param>
        /// <returns>string</returns>
       //string UpdateSearchRequest(IQAgentSearchRequest p_IQAgentSearchRequest);

        /// <summary>
        /// Description: This Methods gets Search request Information from DataSet.
        /// Added By:Maulik gandhi
        /// </summary>
        /// <returns>List of Object of Search Request</returns>
        List<IQAgentSearchRequest> GetSearchRequestByQueryName(IQAgentSearchRequest p_IQAgentSearchRequest);

        List<IQAgentSearchRequest> SelectByClientID(long ClientID);

        /// <summary>
        /// This method gets all SearchTerm and SearchRequestKey with unique QueryName and max Query Version
        /// </summary>
        /// <returns>List of object of IQAgentSearchRequest</returns>
        //List<IQAgentSearchRequest> GetSearchRequestAll();

        List<IQAgentSearchRequest> GetSearchRequestsByClientIDQueryName(long p_ClientID,string p_QueryName);        

        //int SearchRawMediaFromPMGSearch(string p_FilePath);

        string UpdateIsActive(long p_SearchRequestKey, bool p_IsActive,long p_ClientID,string p_Query_Name);

        List<IQAgentSearchRequest> SelectAllByClientID(Guid p_ClientGUID);

        /// <summary>
        /// This method gets all SearchTerm and SearchRequestKey with unique QueryName and max Query Version
        /// </summary>
        /// <param name="p_ConnectionStringKeys">Name Of ConnectionString</param>
        /// <returns>List of object of IQAgentSearchRequest</returns>
        List<IQAgentSearchRequest> GetSearchRequestAll(ConnectionStringKeys p_ConnectionStringKeys);
    }
}
