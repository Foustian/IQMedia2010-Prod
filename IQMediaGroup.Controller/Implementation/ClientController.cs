using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;

namespace IQMediaGroup.Controller.Implementation
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

        private List<Client> FillListOfClientLogoInformation(DataSet p_DataSet)
        {
            try
            {
                List<Client> _ListOfClient = new List<Client>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        Client _Client = new Client();

                        if (p_DataSet.Tables[0].Columns.Contains("ClientGUID") && !_DataRow["ClientGUID"].Equals(DBNull.Value))
                        {
                            _Client.ClientGUID = new Guid(_DataRow["ClientGUID"].ToString());
                        }

                        if (p_DataSet.Tables[0].Columns.Contains("IsActivePlayerLogo") && !_DataRow["IsActivePlayerLogo"].Equals(DBNull.Value))
                        {
                            _Client.IsActivePlayerLogo = Convert.ToBoolean(_DataRow["IsActivePlayerLogo"]);
                        }

                        if (p_DataSet.Tables[0].Columns.Contains("PlayerLogo") && !_DataRow["PlayerLogo"].Equals(DBNull.Value))
                        {
                            _Client.PlayerLogoPath = Convert.ToString(_DataRow["PlayerLogo"]);
                        }

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
        public string InsertClient(Client p_Client)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IClientModel.InsertClient(p_Client);

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
        public string UpdateClient(Client p_Client)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IClientModel.UpdateClient(p_Client);

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

        public List<Client> GetClientGUIDByCustomerGUID(Guid p_CustomerGUID)
        {
            try
            {
                List<Client> _ListOfClient = null;
                DataSet _DataSet= _IClientModel.GetClientGUIDByCustomerGUID(p_CustomerGUID);
                _ListOfClient = FillListOfClientLogoInformation(_DataSet);
                return _ListOfClient;

            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        public string GetClientFtpDetilByClientID(Int64 p_ClientID)
        {
            try
            {
                string _UGCFtpDetail = string.Empty;
                DataSet _DataSet = _IClientModel.GetClientFtpDetilByClientID(p_ClientID);
                if (_DataSet.Tables[0].Rows.Count > 0 && _DataSet.Tables[0].Columns.Contains("UGCFtpUploadLocation"))
                    _UGCFtpDetail = string.IsNullOrEmpty(Convert.ToString(_DataSet.Tables[0].Rows[0]["UGCFtpUploadLocation"])) ? string.Empty : Convert.ToString(_DataSet.Tables[0].Rows[0]["UGCFtpUploadLocation"]);
                return _UGCFtpDetail;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string GetCustomHeaderByClipGuid(Guid p_ClipGuid)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IClientModel.GetCustomHeaderByClipGuid(p_ClipGuid);

                return _Result;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
