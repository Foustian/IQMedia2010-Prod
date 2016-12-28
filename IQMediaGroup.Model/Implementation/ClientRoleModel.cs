using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Data;


namespace IQMediaGroup.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface IClientRoleModel
    /// </summary>
    internal class ClientRoleModel : IQMediaGroupDataLayer, IClientRoleModel
    {

        /// <summary>
        /// Description:This Method will insert Client Role.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_ClientRoles">Object of Client Role.</param>
        /// <returns>Primary Key of Client Role.</returns>
        public string InsertClientRole(ClientRoles _ClientRoles)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int32, _ClientRoles.ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RoleID", DbType.Int32, _ClientRoles.RoleID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientRoleKey", DbType.Int32, _ClientRoles.ClientRoleKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_ClientRole_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will get Client Role.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>DataSet of client role.</returns>
        public DataSet GetClientRoleAdmin()
        {
            try
            {
                DataSet _DataSet = new DataSet();

                _DataSet = this.GetDataSetByProcedure("usp_ClientRole_SelectAll");

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will update Client Role.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ClientRole">Object of Client Role.</param>
        /// <returns>count</returns>
        public string UpdateClientRole(ClientRoles p_ClientRole)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientRoleKey", DbType.Int32, p_ClientRole.ClientRoleKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAccess", DbType.Boolean, p_ClientRole.IsAccess, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_ClientRole_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will get Client Access.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_ClientRoles">Object of Client Role.</param>
        /// <returns>DataSet of client role.</returns>
        public DataSet GetClientAccess(ClientRoles _ClientRoles)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int32, _ClientRoles.ClientID, ParameterDirection.Input));
                _DataSet = this.GetDataSet("usp_ClientRole_SelectClientByClientID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetClientRoleByClientGUIDRoleName(Guid ClientGUID, string RoleName)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RoleName", DbType.String, RoleName, ParameterDirection.Input));
                _DataSet = this.GetDataSet("usp_ClientRole_SelectRoleByClientGUIDRoleName", _ListOfDataType);
                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
