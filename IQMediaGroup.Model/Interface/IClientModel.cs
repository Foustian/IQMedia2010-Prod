using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface for Client
    /// </summary>
    public interface IClientModel
    {
        /// <summary>
        /// This method gets client information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <returns>Dataset containing Client information.</returns>
        DataSet GetClientInfo(bool? p_IsActive);

        /// <summary>
        /// This method inserts client information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_Client">Object of core class containig client information.</param>
        /// <returns>ClientKey</returns>
        string InsertClient(Client p_Client);

        /// <summary>
        /// This method updates client information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_Client">Object of core class containig client information.</param>
        /// <returns>ClientKey</returns>
        string UpdateClient(Client p_Client);

        /// <summary>
        /// This method gets client information By ClientID and Role Name.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <param name="p_RoleName">Role Name</param>
        /// <returns>Dataset of Client.</returns>
        DataSet GetClientByClientIDRoleName(long p_ClientID, string p_RoleName);

        /// <summary>
        /// This method gets ClientGUID by CustomerGUID
        /// </summary>
        /// <param name="p_CustomerGUID">CustomerGUID</param>
        /// <returns>DataSet</returns>
        DataSet GetClientGUIDByCustomerGUID(Guid p_CustomerGUID);

        /// <summary>
        /// this method gets UGCFtpDetail for Client by ClientID
        /// </summary>
        /// <param name="p_ClientID"></param>
        /// <returns></returns>
        DataSet GetClientFtpDetilByClientID(Int64 p_ClientID);

        string GetCustomHeaderByClipGuid(Guid p_ClipGuid);
    }
}
