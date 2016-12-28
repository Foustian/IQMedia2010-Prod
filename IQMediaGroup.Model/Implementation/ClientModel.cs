using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Base;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Implementation
{
    internal class ClientModel : IQMediaGroupDataLayer,IClientModel
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

        /// <summary>
        /// This method inserts client information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_Client">Object of core class containig client information.</param>
        /// <returns>ClientKey</returns>
        public string InsertClient(Client p_Client)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientName", DbType.String, p_Client.ClientName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientKey", DbType.Int32, p_Client.ClientKey, ParameterDirection.Output));
                

                _Result = ExecuteNonQuery("usp_Client_Insert", _ListOfDataType);

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
        public string UpdateClient(Client p_Client)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientName", DbType.String, p_Client.ClientName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Active", DbType.Boolean, p_Client.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientKey", DbType.Int32, p_Client.ClientKey, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_Client_Update", _ListOfDataType);

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

        
        public DataSet GetClientGUIDByCustomerGUID(Guid p_CustomerGUID)
        {
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                
                
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));


                DataSet _Result = this.GetDataSet("usp_Client_SelectByCustomerGUID", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetClientFtpDetilByClientID(Int64 p_ClientID)
        {
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_ClientID, ParameterDirection.Input));
                DataSet _Result = this.GetDataSet("usp_Client_SelectUGCFtpUploadLocationByClientID", _ListOfDataType);

                return _Result;
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
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClipGuid", DbType.Guid, p_ClipGuid, ParameterDirection.Input));
                DataSet _DataSet = this.GetDataSet("usp_Client_SelectCustomHeaderByClipGuid", _ListOfDataType);

                if (_DataSet.Tables[0].Rows.Count > 0)
                {
                    _Result = Convert.ToString(_DataSet.Tables[0].Rows[0]["CustomHeaderImage"]);
                }

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
