using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
using System.Xml;
using System.Xml.Linq;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    internal class ClientController : IClientController 
    {
            private readonly ModelFactory _ModelFactory = new ModelFactory();
            private readonly IClientModel _IClientModel;

            public ClientController()
            {
                _IClientModel = _ModelFactory.CreateObject<IClientModel>();
            }

        /// <summary>
        /// This method gets client information.
        /// </summary>
        /// <param name="p_IsActive">Status Of The Client</param>
        /// <returns></returns>
        public List<Client> GetClientInformation(bool? p_IsActive)
        {
            try
            {
                List<Client> _ListOfClient = null;

                DataSet _DataSet = _IClientModel.GetClientInfo(p_IsActive);

                _ListOfClient = FillListOfClient(_DataSet);

                return _ListOfClient;
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
                
                DataSet _DataSet = _IClientModel.GetClientInfoWithRole();

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
                DataSet _DataSet = _IClientModel.GetClientInfoWithRoleByClientID(p_ClientID);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetClientInfoWithRole(string p_ClientName)
        {
            try
            {
                DataSet _DataSet = _IClientModel.GetClientInfoWithRoleByClientName(p_ClientName);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<Client> GetMasterClientInfoByClientName(string p_ClientName)
        {
            try
            {
                List<Client> _ListOfClient = null;

                DataSet _DataSet = _IClientModel.GetMasterClientInfoByClientName(p_ClientName);

                _ListOfClient = FillListOfMasterClientByClientName(_DataSet);

                return _ListOfClient;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<Client> FillListOfMasterClientByClientName(DataSet p_DataSet)
        {
            try
            {
                List<Client> _ListOfClient = new List<Client>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        Client _Client = new Client();

                        _Client.ClientKey = Convert.ToInt32(_DataRow["ClientKey"]);
                        _Client.ClientName = _DataRow["ClientName"].ToString();
                        _Client.MasterClient = Convert.ToString(_DataRow["MasterClient"]);

                        _ListOfClient.Add(_Client);
                    }
                }

                return _ListOfClient;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        private List<Client> FillListOfClientWithRole(DataSet p_DataSet)
        {
            try
            {
                List<Client> _ListOfClient = new List<Client>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        Client _Client = new Client();

                        _Client.ClientKey = Convert.ToInt32(_DataRow["ClientKey"]);
                        _Client.ClientName = _DataRow["ClientName"].ToString();
                        _Client.IQBasic = Convert.ToBoolean(_DataRow["IQBasic"]);
                        _Client.AdvancedSearchAccess = Convert.ToBoolean(_DataRow["AdvancedSearchAccess"]);
                        _Client.GlobalAdminAccess = Convert.ToBoolean(_DataRow["GlobalAdminAccess"]);
                        _Client.IQAgentUser = Convert.ToBoolean(_DataRow["IQAgentUser"]);
                        _Client.IQAgentAdminAccess = Convert.ToBoolean(_DataRow["IQAgentAdminAccess"]);
                        _Client.myIQAccess = Convert.ToBoolean(_DataRow["myIQAccess"]);
                        _Client.IQAgentWebsiteAccess = Convert.ToBoolean(_DataRow["IQAgentWebsiteAccess"]);
                        _Client.DownloadClips = Convert.ToBoolean(_DataRow["DownloadClips"]);
                        _Client.IQCustomAccess = Convert.ToBoolean(_DataRow["IQCustomAccess"]);
                        _Client.IQCustomAccess = Convert.ToBoolean(_DataRow["IQCustomAccess"]);
                        _Client.UGCDownload = Convert.ToBoolean(_DataRow["UGCDownload"]);
                        _Client.IframeMicroSite = Convert.ToBoolean(_DataRow["IframeMicroSite"]);

                        if (p_DataSet.Tables[0].Columns.Contains("UGCUploadEdit") && !_DataRow["UGCUploadEdit"].Equals(DBNull.Value))
                        {
                            _Client.UGCUploadEdit = Convert.ToBoolean(_DataRow["UGCUploadEdit"]);
                        }
                        
                        
                        _Client.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);

                        _ListOfClient.Add(_Client);
                    }
                }

                return _ListOfClient;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<Client> FillListOfClientWithRoleByClientName(DataSet p_DataSet)
        {
            try
            {
                List<Client> _ListOfClient = new List<Client>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        Client _Client = new Client();

                        _Client.ClientKey = Convert.ToInt32(_DataRow["ClientKey"]);
                        _Client.ClientName = _DataRow["ClientName"].ToString();
                        _Client.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        _Client.IQBasic = Convert.ToBoolean(_DataRow["IQBasic"]);
                        _Client.AdvancedSearchAccess = Convert.ToBoolean(_DataRow["AdvancedSearchAccess"]);
                        _Client.GlobalAdminAccess = Convert.ToBoolean(_DataRow["GlobalAdminAccess"]);
                        _Client.IQAgentUser = Convert.ToBoolean(_DataRow["IQAgentUser"]);
                        _Client.IQAgentAdminAccess = Convert.ToBoolean(_DataRow["IQAgentAdminAccess"]);
                        _Client.myIQAccess = Convert.ToBoolean(_DataRow["myIQAccess"]);
                        _Client.IQAgentWebsiteAccess = Convert.ToBoolean(_DataRow["IQAgentWebsiteAccess"]);
                        _Client.DownloadClips = Convert.ToBoolean(_DataRow["DownloadClips"]);
                        _Client.Address1 = Convert.ToString(_DataRow["Address1"]);
                        _Client.Address2 = Convert.ToString(_DataRow["Address2"]);
                        _Client.Attention = Convert.ToString(_DataRow["Attention"]);
                        _Client.City = Convert.ToString(_DataRow["City"]);
                        _Client.MasterClient = Convert.ToString(_DataRow["MasterClient"]);
                        _Client.IQCustomAccess = Convert.ToBoolean(_DataRow["IQCustomAccess"]);
                        _Client.UGCDownload = Convert.ToBoolean(_DataRow["UGCDownload"]);
                        _Client.IframeMicroSite = Convert.ToBoolean(_DataRow["IframeMicroSite"]);

                        if (p_DataSet.Tables[0].Columns.Contains("UGCUploadEdit") && !_DataRow["UGCUploadEdit"].Equals(DBNull.Value))
                        {
                            _Client.UGCUploadEdit = Convert.ToBoolean(_DataRow["UGCUploadEdit"]);
                        }

                        if (!_DataRow["NoOfUser"].Equals(DBNull.Value))
                        {
                            _Client.NoOfUser = Convert.ToInt32(_DataRow["NoOfUser"]);
                        }
                        _Client.Phone = Convert.ToString(_DataRow["Phone"]);
                        _Client.Zip = Convert.ToString(_DataRow["Zip"]);

                        if (!_DataRow["BillFrequencyID"].Equals(DBNull.Value))
                        {
                            _Client.BillFrequencyID = Convert.ToInt64(_DataRow["BillFrequencyID"]);
                        }
                        if (!_DataRow["BillTypeID"].Equals(DBNull.Value))
                        {
                            _Client.BillTypeID = Convert.ToInt64(_DataRow["BillTypeID"]);
                        }
                        if (!_DataRow["IndustryID"].Equals(DBNull.Value))
                        {
                            _Client.IndustryID = Convert.ToInt64(_DataRow["IndustryID"]);
                        }
                        if (!_DataRow["PricingCodeID"].Equals(DBNull.Value))
                        {
                            _Client.PricingCodeID = Convert.ToInt64(_DataRow["PricingCodeID"]);
                        }
                        if (!_DataRow["StateID"].Equals(DBNull.Value))
                        {
                            _Client.StateID = Convert.ToInt64(_DataRow["StateID"]);
                        }
                        _Client.CreatedDate = Convert.ToDateTime(_DataRow["CreatedDate"]);
                        _Client.CustomHeaderPath = Convert.ToString(_DataRow["CustomHeaderImage"]);
                        _Client.PlayerLogoPath = Convert.ToString(_DataRow["playerlogo"]);
                        _Client.IsCustomHeader = Convert.ToBoolean(_DataRow["IsCustomHeader"]);
                        _Client.IsActivePlayerLogo = Convert.ToBoolean(_DataRow["IsActivePlayerLogo"]);
                        
                        _ListOfClient.Add(_Client);
                    }
                }

                return _ListOfClient;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<Client> GetClientInfoBySearchTerm(string p_prefixText)
        {
            try
            {
                List<Client> _ListOfClient = null;

                DataSet _DataSet = _IClientModel.GetClientInfoBySearchTerm(p_prefixText);

                _ListOfClient = FillListOfClient(_DataSet);

                return _ListOfClient;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method fills List of object Client from DataSet
        /// </summary>
        /// <param name="p_DataSet">DataSet contains ClientInformation</param>
        /// <returns>List of Object of class Clients</returns>
        private List<Client> FillListOfClient(DataSet p_DataSet)
        {
            try
            {
                List<Client> _ListOfClient = new List<Client>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        Client _Client = new Client();

                        _Client.ClientKey = Convert.ToInt32(_DataRow["ClientKey"]);
                        _Client.ClientName = _DataRow["ClientName"].ToString();
                        _Client.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        if (_DataRow.Table.Columns.Contains("ClientGuid") && !string.IsNullOrEmpty(Convert.ToString(_DataRow["ClientGuid"])))
                            _Client.ClientGUID = new Guid(_DataRow["ClientGuid"].ToString());

                        _ListOfClient.Add(_Client);
                    }
                }

                return _ListOfClient;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method Inserts client information
        /// </summary>
        /// <param name="p_Client">Object of Client Details</param>
        /// <returns>Client Key</returns>
        public string InsertClient(Client p_Client, out int Status)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IClientModel.InsertClient(p_Client,out Status);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method updates Client Information
        /// </summary>
        /// <param name="p_Client">Object of Client Details</param>
        /// <returns>Client Key</returns>
        public string UpdateClient(Client p_Client, out int Status, out int p_NotificationStatus, out int p_IQAgentStatus)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IClientModel.UpdateClient(p_Client, out Status, out p_NotificationStatus, out p_IQAgentStatus);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description;This Method will get Client By ClientID and RoleName.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <param name="p_RoleName">RoleName</param>
        /// <returns>List of Object of class Clients</returns>
        public List<Client> GetClientByClientIDRoleName(long p_ClientID, string p_RoleName)
        {
            try
            {
                DataSet _DataSet = null;
                List<Client> _ListOfClient = null;
                _DataSet = _IClientModel.GetClientByClientIDRoleName(p_ClientID, p_RoleName);
                _ListOfClient = FillAllClientByClientID(_DataSet);
                return _ListOfClient;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This Method Fills Customer Information From Dataset.
        /// </summary>
        /// <param name="_DataSet">Object of Dataset</param>
        /// <returns></returns>
        private List<Client> FillAllClientByClientID(DataSet _DataSet)
        {
            List<Client> _ListOfClientInformation = new List<Client>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        Client _Client = new Client();
                        //_Client.ClientKey = Convert.ToInt32(_DataRow["ClientKey"]);
                        _Client.RoleID = Convert.ToInt32(_DataRow["RoleID"]);
                        //_Client.ClientName = Convert.ToString(_DataRow["ClientName"]);

                        _ListOfClientInformation.Add(_Client);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfClientInformation;
        }

        public List<Client> GetMasterClientInformation()
        {
            try
            {
                List<Client> _ListOfClient = null;

                DataSet _DataSet = _IClientModel.GetMasterClientInfo();

                _ListOfClient = FillListOfMasterClient(_DataSet);

                return _ListOfClient;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<Client> FillListOfMasterClient(DataSet p_DataSet)
        {
            try
            {
                List<Client> _ListOfClient = new List<Client>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        Client _Client = new Client();

                        _Client.MasterClient =_DataRow["MasterClient"].ToString();

                        _ListOfClient.Add(_Client);
                    }
                }

                return _ListOfClient;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<Client> GetClientInfoForCDN()
        {
            try
            {
                DataSet _DataSet = null;
                List<Client> _ListOfClient = null;
                _DataSet = _IClientModel.GetClientInfoForCDN();
                _ListOfClient = FillListOFClientCDN(_DataSet);
                return _ListOfClient;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<Client> FillListOFClientCDN(DataSet p_dataset)
        {
            List<Client> _lstclientlist = new List<Client>();

            if (p_dataset != null && p_dataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow _dr in p_dataset.Tables[0].Rows)
                {
                    Client _objclient = new Client();
                    _objclient.ClientKey = Convert.ToInt16(_dr["ClientKey"]);
                    _objclient.ClientName = Convert.ToString(_dr["ClientName"]);
                    _objclient.ClientGUID = (Guid)(string.IsNullOrEmpty(Convert.ToString(_dr["ClientGUID"])) == false ? _dr["ClientGUID"] : new Guid());
                    _objclient.CDNUpload = string.IsNullOrEmpty(Convert.ToString(_dr["CDNUpload"])) == false ? Convert.ToBoolean(_dr["CDNUpload"]) : false;
                    _lstclientlist.Add(_objclient);
                }
            }

            return _lstclientlist;

        }

        public Int64 UpdateClientInfoForCDN(XmlDocument xml, bool IsEnable)
        {
            try
            {

                Int64 _Result = _IClientModel.UpdateClientInfoForCDN(xml, IsEnable);
                return _Result;
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
                string _Result = _IClientModel.SaveClientSearchSettingsByXml(p_ClientID, xml);
                return _Result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetClientSearchSettingsByClientID(Int64 p_ClientID)
        {
            try
            {
                string result = string.Empty;
                DataSet _DataSet = _IClientModel.GetClientSearchSettingsByClientID(p_ClientID);
                if (_DataSet.Tables[0].Rows.Count > 0)
                {
                    result = Convert.ToString(_DataSet.Tables[0].Rows[0][0]);
                }
                return result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetAllDefaultSettings()
        {
            try
            {
                DataSet _DataSet = _IClientModel.GetAllDefaultSettings();

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


    }
}
