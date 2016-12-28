using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Configuration;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Core.Enumeration;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    internal class CustomerRoleController : ICustomerRoleController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ICustomerRoleModel _ICustomerRoleModel;

        public CustomerRoleController()
        {
            _ICustomerRoleModel = _ModelFactory.CreateObject<ICustomerRoleModel>();
        }

        /// <summary>
        /// Description: This Methods gets Customer Role Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_CustomerKey">CustomerID of Customer</param>
        /// <returns>List of Object of Customer Role</returns>
        public List<CustomerRoles> GetCustomerRoleAccess(int p_CustomerKey)
        {
            DataSet _DataSet = null;
            List<CustomerRoles> _ListOfCustomerRoles = null;
            try
            {
                _DataSet = _ICustomerRoleModel.GetCustomerRoleAccess(p_CustomerKey);
                _ListOfCustomerRoles = FillCustomerRoleInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerRoles;
        }

        /// <summary>
        /// Description: This Methods Fills Customer Role Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="_DataSet">Dataset for Customer Role Infromarmation</param>
        /// <returns>List of Object of Customer Role</returns>
        private List<CustomerRoles> FillCustomerRoleInformation(DataSet _DataSet)
        {
            List<CustomerRoles> _ListOfCustomerRoles = new List<CustomerRoles>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        CustomerRoles _CustomerRoles = new CustomerRoles();
                        _CustomerRoles.CustomerRoleKey = Convert.ToInt32(_DataRow["CustomerRoleKey"]);
                        _CustomerRoles.RoleName = Convert.ToString(_DataRow["RoleName"]);
                        _CustomerRoles.IsAccess = Convert.ToBoolean(_DataRow["IsAccess"]);

                        _ListOfCustomerRoles.Add(_CustomerRoles);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerRoles;
        }

        /// <summary>
        /// This method inserts Customer Role Information
        /// Added By: bhavik barot
        /// </summary>
        /// <param name="p_CustomerRoles">Object Of CustomerRoles Class</param>
        /// <returns></returns>
        public string InsertCustomerRole(CustomerRoles p_CustomerRoles)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _ICustomerRoleModel.InsertCustomerRole(p_CustomerRoles);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets customer Role information
        /// Added By: bhavik barot
        /// </summary>
        /// <returns>List of object of CustomerRoles details</returns>
        public List<CustomerRoles> GetCustomerRoleAdmin()
        {
            DataSet _DataSet = null;
            List<CustomerRoles> _ListOfCustomerRoles = null;
            try
            {
                _DataSet = _ICustomerRoleModel.GetCustomerRoleAdmin();
                _ListOfCustomerRoles = FillCustomerRoleInfo(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerRoles;
        }

        /// <summary>
        /// Description: This Methods Fills Customer Role Information from DataSet.
        /// Added By: bhavik barot
        /// </summary>
        /// <param name="_DataSet">Dataset for Customer Role Infromarmation</param>
        /// <returns>List of Object of Customer Role</returns>
        private List<CustomerRoles> FillCustomerRoleInfo(DataSet _DataSet)
        {
            List<CustomerRoles> _ListOfCustomerRoles = new List<CustomerRoles>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        CustomerRoles _CustomerRoles = new CustomerRoles();
                        _CustomerRoles.CustomerRoleKey = Convert.ToInt32(_DataRow["CustomerRoleKey"]);
                        _CustomerRoles.RoleID = Convert.ToInt32(_DataRow["RoleID"]);
                        _CustomerRoles.RoleName = Convert.ToString(_DataRow["RoleName"]);
                        _CustomerRoles.FullName = Convert.ToString(_DataRow["FirstName"]) + " " + Convert.ToString(_DataRow["LastName"]);
                        _CustomerRoles.ClientName = Convert.ToString(_DataRow["ClientName"]);
                        _CustomerRoles.IsAccess = Convert.ToBoolean(_DataRow["IsAccess"]);
                        _CustomerRoles.CustomerID = Convert.ToInt32(_DataRow["CustomerID"]);
                        _CustomerRoles.ClientID = Convert.ToInt32(_DataRow["ClientKey"]);
                        _ListOfCustomerRoles.Add(_CustomerRoles);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerRoles;
        }

        /// <summary>
        /// This method updates customer role information
        /// Added By: bhavik barot
        /// </summary>
        /// <param name="p_CustomerRole">Object of CustomerRol details</param>
        /// <returns>CustomerRole Key</returns>
        public string UpdateCustomerRole(CustomerRoles p_CustomerRole)
        {
            try
            {
               string _Result =  _ICustomerRoleModel.UpdateCustomerRole(p_CustomerRole);
               return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets Client and customer role details
        /// Added By: bhavik barot
        /// </summary>
        /// <param name="p_CustomerKey">Customer Key</param>
        /// <returns>List of object of CustomerClientRoleAccess</returns>
        public List<CustomerClientRoleAccess> GetCustomerClientRoleAccess(int p_CustomerKey)
        {
            DataSet _DataSet = null;
            List<CustomerClientRoleAccess> _ListOfCustomerClientRoleAccess = null;
            try
            {
                _DataSet = _ICustomerRoleModel.GetCustomerClientRoleAccess(p_CustomerKey);
                _ListOfCustomerClientRoleAccess = FillCustomerClientRoleInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerClientRoleAccess;
        }

        /// <summary>
        /// Description: This Methods Fills Customer And Client Role Information from DataSet.
        /// Added By: bhavik barot
        /// </summary>
        /// <param name="_DataSet">Dataset for Customer Role Infromarmation</param>
        /// <returns>List of Object of Customer Role</returns>
        private List<CustomerClientRoleAccess> FillCustomerClientRoleInformation(DataSet _DataSet)
        {
            List<CustomerClientRoleAccess> _ListOfCustomerClientRoleAccess = new List<CustomerClientRoleAccess>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        CustomerClientRoleAccess _CustomerClientRoleAccess = new CustomerClientRoleAccess();
                        _CustomerClientRoleAccess.CustomerRoleKey = Convert.ToInt32(_DataRow["CustomerRoleKey"]);
                        _CustomerClientRoleAccess.CustomerID = Convert.ToInt32(_DataRow["CustomerID"]);
                        _CustomerClientRoleAccess.CustomerRoleID = Convert.ToInt32(_DataRow["CustomerRoleID"]);
                        _CustomerClientRoleAccess.CustomerAccess = Convert.ToBoolean(_DataRow["CustomerAccess"]);
                        _CustomerClientRoleAccess.ClientRoleKey = Convert.ToInt32(_DataRow["ClientRoleKey"]);
                        _CustomerClientRoleAccess.ClientRoleID = Convert.ToInt32(_DataRow["ClientRoleID"]);
                        _CustomerClientRoleAccess.ClientAccess = Convert.ToBoolean(_DataRow["ClientAccess"]);
                        _CustomerClientRoleAccess.RoleKey = Convert.ToInt32(_DataRow["RoleKey"]);
                        _CustomerClientRoleAccess.RoleName = Convert.ToString(_DataRow["RoleName"]);
                        _CustomerClientRoleAccess.RoleIsActive = Convert.ToBoolean(_DataRow["RoleIsActive"]);

                        _ListOfCustomerClientRoleAccess.Add(_CustomerClientRoleAccess);
                       
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerClientRoleAccess;
        }

        public string UpdateCustomerRoleByClientIDRoleID(CustomerRoles p_CustomerRoles)
        {
            try
            {
                string _Result = _ICustomerRoleModel.UpdateCustomerRoleByClientIDRoleID(p_CustomerRoles);
                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
