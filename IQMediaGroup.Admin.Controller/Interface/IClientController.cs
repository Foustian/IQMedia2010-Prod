using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Web;
using System.Net;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace IQMediaGroup.Admin.Controller.Interface
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
        DataSet GetClientInfoWithRole();
        DataSet GetClientInfoWithRoleByClientID(Int64 p_ClientID);
        DataSet GetClientInfoWithRole(string p_ClientName);

        List<Client> GetMasterClientInfoByClientName(string p_ClientName);

        List<Client> GetClientInfoBySearchTerm(string p_prefixText);
        /// <summary>
        /// This method inserts data into client.
        /// </summary>
        /// <param name="p_Client"></param>
        /// <returns></returns>
        string InsertClient(Client p_Client, out int Status);

        /// <summary>
        /// This method updates client information
        /// </summary>
        /// <param name="p_Client"></param>
        /// <returns></returns>
        string UpdateClient(Client p_Client, out int Status, out int p_NotificationStatus, out int p_IQAgentStatus);

        /// <summary>
        /// Description;This Method will get Client By ClientID and RoleName.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <param name="p_RoleName">RoleName</param>
        /// <returns>List of Object of class Clients</returns>
        List<Client> GetClientByClientIDRoleName(long p_ClientID, string p_RoleName);

        List<Client> GetMasterClientInformation();

        List<Client> GetClientInfoForCDN();
        Int64 UpdateClientInfoForCDN(XmlDocument xml, bool IsEnable);

        /// <summary>
        /// Insert And Update SearchSettings of Custom Client Settings by XML and ClientID
        /// </summary>
        /// <param name="p_ClientID">Int ClientID</param>
        /// <param name="xml">SearchSettings XML</param>
        /// <returns></returns>
        string SaveClientSearchSettingsByXml(Int64 p_ClientID, XDocument xml);

        /// <summary>
        /// Get SearchSettings From Custom Client Settings by CliendID
        /// </summary>
        /// <param name="p_ClientID"></param>
        /// <returns></returns>
        string GetClientSearchSettingsByClientID(Int64 p_ClientID);

        DataSet GetAllDefaultSettings();
    }
}
