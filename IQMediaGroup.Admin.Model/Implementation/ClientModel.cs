using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Admin.Model.Base;
using System.Data;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Xml;
using System.Xml.Linq;

namespace IQMediaGroup.Admin.Model.Implementation
{
    internal class ClientModel : IQMediaGroupDataLayer, IClientModel
    {
        /// <summary>
        /// This method gets client information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <returns>Dataset containing Client information.</returns>
        public DataSet GetClientInfo(bool? p_IsActive)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_IsActive, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Client_SelectAll", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetClientInfoWithRole()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_Client_SelectAllClientWithRole", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public DataSet GetClientInfoWithRoleByClientID(Int64 p_ClientID)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientID", DbType.String, p_ClientID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Client_SelectAlClientWithRoleByClientID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetClientInfoWithRoleByClientName(string p_ClientName)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientName", DbType.String, p_ClientName, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Client_SelectClientWithRoleByClientName", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetMasterClientInfoByClientName(string p_ClientName)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientName", DbType.String, p_ClientName, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Client_SelectMasterClientByClientName", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetClientInfoBySearchTerm(string p_prefixText)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@prefixText", DbType.String, p_prefixText, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Client_SelectBySearchTerm", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method inserts client information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_Client">Object of core class containig client information.</param>
        /// <returns>ClientKey</returns>
        public string InsertClient(Client p_Client, out int Status)
        {
            try
            {
                Status = 0;
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientName", DbType.String, p_Client.ClientName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_Client.ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultCategory", DbType.String, p_Client.DefaultCategory, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PricingCodeID", DbType.Int64, p_Client.PricingCodeID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@BillFrequencyID", DbType.Int64, p_Client.@BillFrequencyID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@BillTypeID", DbType.Int64, p_Client.BillTypeID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IndustryID", DbType.Int64, p_Client.IndustryID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@StateID", DbType.Int64, p_Client.StateID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Address1", DbType.String, p_Client.Address1, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Address2", DbType.String, p_Client.Address2, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@City", DbType.String, p_Client.City, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Zip", DbType.String, p_Client.Zip, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Attention", DbType.String, p_Client.Attention, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Phone", DbType.String, p_Client.Phone, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MasterClient", DbType.String, p_Client.MasterClient, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfUser", DbType.Int32, p_Client.NoOfUser, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomHeader", DbType.String, p_Client.CustomHeaderPath, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsCustomHeader", DbType.Boolean, p_Client.IsCustomHeader, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PlayerLogo", DbType.String, p_Client.PlayerLogoPath, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActivePlayerLogo", DbType.Boolean, p_Client.IsActivePlayerLogo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfIQNotification", DbType.Int16, p_Client.NoOfIQNotification, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfIQAgnet", DbType.Int16, p_Client.NoOfIQAgnet, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CompeteMultiplier", DbType.Decimal, p_Client.CompeteMultiplier, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@OnlineNewsAdRate", DbType.Decimal, p_Client.OnlineNewsAdRate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@OtherOnlineAdRate", DbType.Decimal, p_Client.OtherOnlineAdRate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UrlPercentRead", DbType.Decimal, p_Client.UrlPercentRead, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientKey", DbType.Int32, p_Client.ClientKey, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@Status", DbType.Int32, p_Client.ClientKey, ParameterDirection.Output));

                Dictionary<string, string> _outputParams;

                _Result = ExecuteNonQuery("usp_Client_Insert", _ListOfDataType, out _outputParams);

                if (_outputParams != null && _outputParams.Count > 0)
                {
                    Status = Convert.ToInt32(_outputParams["@Status"]);
                    _Result = _outputParams["@ClientKey"].ToString();
                }

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method updates client information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_Client">Object of core class containig client information.</param>
        /// <returns>ClientKey</returns>
        public string UpdateClient(Client p_Client, out int Status, out int p_NotificationStatus, out int p_IQAgentStatus)
        {
            try
            {
                Status = 0;
                p_NotificationStatus = 0;
                p_IQAgentStatus = 0;
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientName", DbType.String, p_Client.ClientName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Active", DbType.Boolean, p_Client.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PricingCodeID", DbType.Int64, p_Client.PricingCodeID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@BillFrequencyID", DbType.Int64, p_Client.@BillFrequencyID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@BillTypeID", DbType.Int64, p_Client.BillTypeID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IndustryID", DbType.Int64, p_Client.IndustryID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@StateID", DbType.Int64, p_Client.StateID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Address1", DbType.String, p_Client.Address1, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Address2", DbType.String, p_Client.Address2, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@City", DbType.String, p_Client.City, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Zip", DbType.String, p_Client.Zip, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Attention", DbType.String, p_Client.Attention, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Phone", DbType.String, p_Client.Phone, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MasterClient", DbType.String, p_Client.MasterClient, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfUser", DbType.Int32, p_Client.NoOfUser, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ModifiedDate", DbType.DateTime, p_Client.ModifiedDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientKey", DbType.Int64, p_Client.ClientKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomHeader", DbType.String, p_Client.CustomHeaderPath, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsCustomHeader", DbType.Boolean, p_Client.IsCustomHeader, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PlayerLogo", DbType.String, p_Client.PlayerLogoPath, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActivePlayerLogo", DbType.Boolean, p_Client.IsActivePlayerLogo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfIQNotification", DbType.Int16, p_Client.NoOfIQNotification, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfIQAgnet", DbType.Int16, p_Client.NoOfIQAgnet, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CompeteMultiplier", DbType.Decimal, p_Client.CompeteMultiplier, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@OnlineNewsAdRate", DbType.Decimal, p_Client.OnlineNewsAdRate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@OtherOnlineAdRate", DbType.Decimal, p_Client.OtherOnlineAdRate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UrlPercentRead", DbType.Decimal, p_Client.UrlPercentRead, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Boolean, Status, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@NotificationStatus", DbType.Int32, 0, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@IQAgentStatus", DbType.Int32, 0, ParameterDirection.Output));

                Dictionary<string, string> _outputParams;
                _Result = ExecuteNonQuery("usp_Client_Update", _ListOfDataType, out _outputParams);

                if (_outputParams != null && _outputParams.Count > 0)
                {
                    Status = Convert.ToInt32(Convert.ToBoolean(_outputParams["@Status"].ToString()));
                    p_NotificationStatus = Convert.ToInt32(_outputParams["@NotificationStatus"].ToString());
                    p_IQAgentStatus = Convert.ToInt32(_outputParams["@IQAgentStatus"].ToString());
                }

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets client information By ClientID and Role Name.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <param name="p_RoleName">Role Name</param>
        /// <returns>Dataset of Client.</returns>
        public DataSet GetClientByClientIDRoleName(long p_ClientID, string p_RoleName)
        {
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int32, p_ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RoleName", DbType.String, p_RoleName, ParameterDirection.Input));

                DataSet _DataSet = new DataSet();

                _DataSet = this.GetDataSet("usp_Client_SelectByClientRoleName", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetMasterClientInfo()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_Client_SelectMasterClient", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetClientInfoForCDN()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_Client_SelectAllClientWithCDNUpload", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public Int64 UpdateClientInfoForCDN(XmlDocument xml, bool IsEnable)
        {
            try
            {
                int Status = 0;
                Client pclient = new Client();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@xml", DbType.Xml, xml.InnerXml.ToString(), ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsEnable", DbType.String, IsEnable, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Boolean, Status, ParameterDirection.Output));

                Dictionary<string, string> _outputParams;
                string _Result = this.ExecuteNonQuery("usp_Client_UpdateCDNUploadByClientList", _ListOfDataType, out _outputParams);

                if (_outputParams != null && _outputParams.Count > 0)
                {
                    Status = Convert.ToInt32(Convert.ToBoolean(_outputParams["@Status"].ToString()));
                }
                return Convert.ToInt64(Status);

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string SaveClientSearchSettingsByXml(Int64 p_ClientID, XDocument xml)
        {
            try
            {

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchSettings", DbType.Xml, xml.ToString(), ParameterDirection.Input));

                string _Result = this.ExecuteNonQuery("usp_IQClient_CustomSettings_SaveSearchSettings", _ListOfDataType);
                return _Result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetClientSearchSettingsByClientID(Int64 p_ClientID)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientID", DbType.String, p_ClientID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_IQClient_CustomSettings_SelectSearchSettingsByClientID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetAllDefaultSettings()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_IQClient_CustomSettings_SelectAllDefaultSettings", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
