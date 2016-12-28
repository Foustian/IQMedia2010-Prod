using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface IClientRoleController
    {

        /// <summary>
        /// This method inserts Client Role Information
        /// </summary>
        /// <param name="p_ClientRoles">Object Of ClientRoles Class</param>
        /// <returns></returns>
        string InsertClientRole(ClientRoles p_ClientRoles);

        /// <summary>
        /// This method gets Client Role Details
        /// </summary>
        /// <returns>List of object of client role details</returns>
        List<ClientRoles> GetClientRoleAdmin();

        /// <summary>
        /// Description:This method update Search Request  Information.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_IQAgentSearchRequest">object of SearchRequest</param>
        /// <returns>string</returns>
        string UpdateClientRole(ClientRoles p_ClientRole);

        /// <summary>
        /// Description: This Methods gets Client Role Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">ClientID of Client</param>
        /// <returns>List of Object of Client Role</returns>
        List<ClientRoles> GetClientAccess(ClientRoles p_ClientRole);

        int GetClientRoleByClientGUIDRoleName(Guid ClientGUID, string RoleName);
    }
}
