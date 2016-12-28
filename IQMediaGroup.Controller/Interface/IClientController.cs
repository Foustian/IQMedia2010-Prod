using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Web;
using System.Net;

namespace IQMediaGroup.Controller.Interface
{
    /// <summary>
    /// Interface for Client
    /// </summary>
    public interface IClientController
    {
        /// <summary>
        /// This method gets Client Information.
        /// </summary>
        /// <returns></returns>
        List<Client> GetClientInformation(bool? p_IsActive);

        /// <summary>
        /// This method inserts data into client.
        /// </summary>
        /// <param name="p_Client"></param>
        /// <returns></returns>
        string InsertClient(Client p_Client);

        /// <summary>
        /// This method updates client information
        /// </summary>
        /// <param name="p_Client"></param>
        /// <returns></returns>
        string UpdateClient(Client p_Client);

        /// <summary>
        /// Description;This Method will get Client By ClientID and RoleName.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <param name="p_RoleName">RoleName</param>
        /// <returns>List of Object of class Clients</returns>
        List<Client> GetClientByClientIDRoleName(long p_ClientID, string p_RoleName);

        List<Client> GetClientGUIDByCustomerGUID(Guid p_CustomerGUID);

        string GetClientFtpDetilByClientID(Int64 p_ClientID);

        string GetCustomHeaderByClipGuid(Guid p_ClipGuid);
    }
}
