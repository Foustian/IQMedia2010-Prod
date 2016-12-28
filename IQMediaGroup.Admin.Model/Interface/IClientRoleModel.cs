using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Admin.Core.HelperClasses;
namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface of Client Role
    /// </summary>
    public interface IClientRoleModel
    {
        
        /// <summary>
        /// This method inserts Client Role Information.
        /// </summary>
        /// <param name="_ClientRoles"></param>
        /// <returns></returns>
        string InsertClientRole(ClientRoles _ClientRoles);

        /// <summary>
        /// This method gets all Client role information
        /// </summary>
        /// <returns></returns>
        DataSet GetClientRoleAdmin();

        /// <summary>
        /// Description:This Method will update Client Role.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ClientRole">Object of Client Role.</param>
        /// <returns>count</returns>
        string UpdateClientRole(ClientRoles p_ClientRole);
        string UpdateClientRoleByClientIDRoleID(ClientRoles p_ClientRole);
        /// <summary>
        /// Description:This Method will get Client Access.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_ClientRoles">Object of Client Role.</param>
        /// <returns>DataSet of client role.</returns>
        DataSet GetClientAccess(ClientRoles _ClientRoles);

        DataSet GetClientRoleByClientID(Int64 p_ClientID);
    }
}
