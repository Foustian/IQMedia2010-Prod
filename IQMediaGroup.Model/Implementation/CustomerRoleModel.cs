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
    /// Implementation of Interface ICustomerRoleModel
    /// </summary>
    internal class CustomerRoleModel : IQMediaGroupDataLayer, ICustomerRoleModel
    {
        /// <summary>
        /// This mehtod gets customer role access information.
        /// </summary>
        /// <param name="p_CustomerKey">CustomerKey</param>
        /// <returns>Dataset that contains access information.</returns>
        public DataSet GetCustomerRoleAccess(int p_CustomerKey)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int32, p_CustomerKey, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_CustomerRoles_SelectRoles", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Descritpion:This method will insert Customer Role.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_CustomerRoles">object of customer roles</param>
        /// <returns>customer role key</returns>
        public string InsertCustomerRole(CustomerRoles _CustomerRoles)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int32, _CustomerRoles.CustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RoleID", DbType.Int32, _CustomerRoles.RoleID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerRoleKey", DbType.Int32, _CustomerRoles.CustomerRoleKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_CustomerRoles_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Descrition:This method will get Customer Role
        /// </summary>
        /// <returns>Datatset of Customer Role</returns>
        public DataSet GetCustomerRoleAdmin() 
        {
            try
            {
                DataSet _DataSet = new DataSet();

                _DataSet = this.GetDataSetByProcedure("usp_CustomerRole_SelectAll");

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Descritpion:This method will update Customer Role.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_CustomerRole">object of customer roles</param>
        /// <returns>customer role key</returns>
        public string UpdateCustomerRole(CustomerRoles p_CustomerRole)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@CustomerRoleKey", DbType.Int32, p_CustomerRole.CustomerRoleKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAccess", DbType.Boolean, p_CustomerRole.IsAccess, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_CustomerRole_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Descrition:This method will get Customer client Role access
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_CustomerKey">Customer primary Key</param>
        /// <returns>Dataset</returns>
        public DataSet GetCustomerClientRoleAccess(int p_CustomerKey)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int32, p_CustomerKey, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_CustomerClientRole_Select", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
