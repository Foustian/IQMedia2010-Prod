using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Xml;
using System.Xml.Linq;

namespace IQMediaGroup.Admin.Model.Interface
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

        DataSet GetClientInfoWithRole();

        DataSet GetClientInfoWithRoleByClientID(Int64 p_ClientID);

        DataSet GetClientInfoWithRoleByClientName(string p_ClientName);

        DataSet GetMasterClientInfoByClientName(string p_ClientName);

        DataSet GetClientInfoBySearchTerm(string p_prefixText);
        /// <summary>
        /// This method inserts client information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_Client">Object of core class containig client information.</param>
        /// <returns>ClientKey</returns>
        string InsertClient(Client p_Client,out int Status);

        /// <summary>
        /// This method updates client information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_Client">Object of core class containig client information.</param>
        /// <returns>ClientKey</returns>
        string UpdateClient(Client p_Client, out int Status, out int p_NotificationStatus, out int p_IQAgentStatus);

        /// <summary>
        /// This method gets client information By ClientID and Role Name.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <param name="p_RoleName">Role Name</param>
        /// <returns>Dataset of Client.</returns>
        DataSet GetClientByClientIDRoleName(long p_ClientID, string p_RoleName);

        DataSet GetMasterClientInfo();

        DataSet GetClientInfoForCDN();
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
        DataSet GetClientSearchSettingsByClientID(Int64 p_ClientID);

        DataSet GetAllDefaultSettings();
    }
}
