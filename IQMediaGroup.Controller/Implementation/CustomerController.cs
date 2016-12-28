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
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;

namespace IQMediaGroup.Controller.Implementation
{
    internal class CustomerController : ICustomerController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ICustomerModel _ICustomerModel;
        public CustomerController()
        {
            _ICustomerModel = _ModelFactory.CreateObject<ICustomerModel>();
        }

        /// <summary>
        /// This method authenticats Customer.
        /// </summary>
        /// <param name="p_Email">Email</param>
        /// <param name="p_Password">Password</param>
        /// <returns>CustomerKey if authentication successful</returns>
        public List<Customer> CheckAuthentication(string p_Email, string p_Password)
        {
            DataSet _DataSet = null;
            List<Customer> _ListOfCustomerInformation = null;
            try
            {
                string _Result = string.Empty;

                _DataSet = _ICustomerModel.CheckAuthentication(p_Email, p_Password);

                _ListOfCustomerInformation = FillCustomerInformation(_DataSet);

                return _ListOfCustomerInformation;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods Fills Customer Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="_DataSet">Dataset for Customer Infromarmation</param>
        /// <returns></returns>
        private List<Customer> FillCustomerInformation(DataSet _DataSet)
        {
            List<Customer> _ListOfCustomerInformation = new List<Customer>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        Customer _Customer = new Customer();

                        if (_DataSet.Tables[0].Columns.Contains("CustomerKey") && !_DataRow["CustomerKey"].Equals(DBNull.Value))
                        {
                            _Customer.CustomerKey = Convert.ToInt32(_DataRow["CustomerKey"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("ClientID") && !_DataRow["ClientID"].Equals(DBNull.Value))
                        {
                            _Customer.ClientID = Convert.ToInt32(_DataRow["ClientID"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("ClientName") && !_DataRow["ClientName"].Equals(DBNull.Value))
                        {
                            _Customer.ClientName = Convert.ToString(_DataRow["ClientName"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("FirstName") && !_DataRow["FirstName"].Equals(DBNull.Value))
                        {
                            _Customer.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Email") && !_DataRow["Email"].Equals(DBNull.Value))
                        {
                            _Customer.Email = Convert.ToString(_DataRow["Email"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("CustomHeaderImage") && !_DataRow["CustomHeaderImage"].Equals(DBNull.Value))
                        {
                            _Customer.CustomHeaderImage = Convert.ToString(_DataRow["CustomHeaderImage"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IsCustomHeader") && !_DataRow["IsCustomHeader"].Equals(DBNull.Value))
                        {
                            _Customer.IsCustomHeader = Convert.ToBoolean(_DataRow["IsCustomHeader"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("ClientGUID") && !_DataRow["ClientGUID"].Equals(DBNull.Value))
                        {
                            _Customer.ClientGUID = Convert.ToString(_DataRow["ClientGUID"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("CustomerGUID") && !_DataRow["CustomerGUID"].Equals(DBNull.Value))
                        {
                            _Customer.CustomerGUID = Convert.ToString(_DataRow["CustomerGUID"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IsActivePlayerLogo") && !_DataRow["IsActivePlayerLogo"].Equals(DBNull.Value))
                        {
                            _Customer.IsClientPlayerLogoActive = Convert.ToBoolean(_DataRow["IsActivePlayerLogo"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("PlayerLogo") && !_DataRow["PlayerLogo"].Equals(DBNull.Value))
                        {
                            _Customer.ClientPlayerLogoImage = Convert.ToString(_DataRow["PlayerLogo"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("MultiLogin") && !_DataRow["MultiLogin"].Equals(DBNull.Value))
                        {
                            _Customer.MultiLogin = Convert.ToBoolean(_DataRow["MultiLogin"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("DefaultPage") && !_DataRow["DefaultPage"].Equals(DBNull.Value))
                        {
                            _Customer.DefaultPage = Convert.ToString(_DataRow["DefaultPage"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("LastName") && !_DataRow["LastName"].Equals(DBNull.Value))
                        {
                            _Customer.LastName = Convert.ToString(_DataRow["LastName"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("AuthorizedVersion") && !_DataRow["AuthorizedVersion"].Equals(DBNull.Value))
                        {
                            _Customer.AuthorizedVersion = Convert.ToInt16(_DataRow["AuthorizedVersion"]);
                        }
                        else
                        {
                            _Customer.AuthorizedVersion = null;
                        }

                        _ListOfCustomerInformation.Add(_Customer);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerInformation;
        }

        /// <summary>
        /// This method authenticats Customer.
        /// </summary>
        /// <param name="p_Email">Email</param>
        /// <param name="p_Password">Password</param>
        /// <returns>CustomerKey if authentication successful</returns>
        public List<Customer> ForgotPassword(string p_Email)
        {
            DataSet _DataSet = null;
            List<Customer> _ListOfCustomerInformation = null;
            try
            {
                string _Result = string.Empty;

                _DataSet = _ICustomerModel.ForgotPassword(p_Email);
                _ListOfCustomerInformation = FillCustomer(_DataSet);

                return _ListOfCustomerInformation;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods Fills Customer Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="_DataSet">Dataset for Customer Infromarmation</param>
        /// <returns></returns>
        private List<Customer> FillCustomer(DataSet _DataSet)
        {
            List<Customer> _ListOfCustomerInformation = new List<Customer>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        Customer _Customer = new Customer();
                        _Customer.CustomerKey = Convert.ToInt32(_DataRow["CustomerKey"]);
                        _Customer.ClientID = Convert.ToInt32(_DataRow["ClientID"]);
                        _Customer.Pwd = Convert.ToString(_DataRow["CustomerPassword"]);
                        _Customer.FirstName = Convert.ToString(_DataRow["FirstName"]);

                        _ListOfCustomerInformation.Add(_Customer);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerInformation;
        }

        /// <summary>
        /// This method adds new customer.
        /// </summary>
        /// <param name="p_Customer">Object of Core class Customer</param>
        /// <returns>CustomerKey if added successfully.</returns>
        public string InsertCustomer(Customer p_Customer)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ICustomerModel.InsertCustomer(p_Customer);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets Clip information on the basis of ClientID
        /// </summary>
        /// <param name="p_Customer">Dataset containing Customer information.</param>
        /// <returns></returns>
        public List<Customer> GetCustomerNameByClientID(Int64 p_ClientID)
        {
            DataSet _DataSet = null;
            List<Customer> _ListOfCustomer = null;

            try
            {
                _DataSet = _ICustomerModel.GetCustomerNameByClientID(p_ClientID);
                _ListOfCustomer = FillCustomerMyclipInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfCustomer;
        }

        /// <summary>
        /// Description: This Methods Fills Station Information from DataSet.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="_DataSet">Dataset for Customer Infromarmation</param>
        /// <returns></returns>
        private List<Customer> FillCustomerMyclipInformation(DataSet _DataSet)
        {
            List<Customer> _ListOfCustomerInformation = new List<Customer>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        Customer _Customer = new Customer();
                        _Customer.CustomerKey = Convert.ToInt32(_DataRow["CustomerKey"]);
                        _Customer.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        _Customer.LastName = Convert.ToString(_DataRow["LastName"]);

                        _Customer.CustomerGUID = Convert.ToString(_DataRow["CustomerGUID"]);


                        _ListOfCustomerInformation.Add(_Customer);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerInformation;
        }

        /* /// <summary>
         /// This method gets Clip information on the basis of CustomerID
         /// </summary>
         /// <param name="p_Customer">Dataset containing Customer information.</param>
         /// <returns></returns>
         public List<Customer> GetMyClipByCustomerID(Customer p_Customer)
         {
             DataSet _DataSet = null;
             List<Customer> _ListOfCustomer = null;

             try
             {
                 _DataSet = _ICustomerModel.GetMyClipByCustomerID(p_Customer);
                 _ListOfCustomer = FillCustomerforMyClips(_DataSet);
             }
             catch (Exception _Exception)
             {
                 throw _Exception;
             }

             return _ListOfCustomer;
         }*/

        /// <summary>
        /// Description: This Methods Fills Customer Information from DataSet.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="_DataSet">Dataset for Customer Infromarmation</param>
        /// <returns></returns>
        private List<Customer> FillCustomerforMyClips(DataSet _DataSet)
        {
            List<Customer> _ListOfCustomerInformation = new List<Customer>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        Customer _Customer = new Customer();
                        _Customer.CustomerKey = Convert.ToInt32(_DataRow["CustomerKey"]);
                        _ListOfCustomerInformation.Add(_Customer);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerInformation;
        }

        /// <summary>
        /// This method select all customer
        /// </summary>
        /// <returns>Object of List Of Customer.</returns>
        public List<Customer> GetAllCustomers()
        {
            DataSet _DataSet = null;
            List<Customer> _ListOfCustomer = null;

            try
            {
                _DataSet = _ICustomerModel.GetAllCustomers();
                _ListOfCustomer = FillAllCustomer(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfCustomer;
        }

        /// <summary>
        /// This Method Fills Customer Information From Dataset.
        /// </summary>
        /// <param name="_DataSet">Object of Dataset</param>
        /// <returns></returns>
        private List<Customer> FillAllCustomer(DataSet _DataSet)
        {
            List<Customer> _ListOfCustomerInformation = new List<Customer>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        Customer _Customer = new Customer();
                        _Customer.CustomerKey = Convert.ToInt32(_DataRow["CustomerKey"]);
                        _Customer.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        _Customer.LastName = Convert.ToString(_DataRow["LastName"]);
                        _Customer.Email = Convert.ToString(_DataRow["Email"]);
                        _Customer.Comment = Convert.ToString(_DataRow["CustomerComment"]);
                        _Customer.Password = Convert.ToString(_DataRow["CustomerPassword"]);
                        _Customer.ContactNo = Convert.ToString(_DataRow["ContactNo"]);
                        _Customer.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        _Customer.ClientName = Convert.ToString(_DataRow["ClientName"]);
                        _Customer.ClientID = Convert.ToInt32(_DataRow["ClientID"]);
                        _Customer.FullName = Convert.ToString(_DataRow["FirstName"]) + Convert.ToString(_DataRow["LastName"]);
                        _ListOfCustomerInformation.Add(_Customer);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerInformation;
        }

        /// <summary>
        /// This method select all Active customer
        /// </summary>
        /// <returns>Object of List Of Customer.</returns>
        public List<Customer> GetAllActiveCustomers()
        {
            DataSet _DataSet = null;
            List<Customer> _ListOfCustomer = null;

            try
            {
                _DataSet = _ICustomerModel.GetAllActiveCustomers();
                _ListOfCustomer = FillAllActiveCustomer(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfCustomer;
        }

        /// <summary>
        /// Description: This Methods Fills Station Information from DataSet.
        /// </summary>
        /// <param name="_DataSet">Object of Dataset</param>
        /// <returns></returns>
        private List<Customer> FillAllActiveCustomer(DataSet _DataSet)
        {
            List<Customer> _ListOfCustomerInformation = new List<Customer>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        Customer _Customer = new Customer();
                        _Customer.CustomerKey = Convert.ToInt32(_DataRow["CustomerKey"]);
                        _Customer.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        _Customer.LastName = Convert.ToString(_DataRow["LastName"]);
                        _Customer.Email = Convert.ToString(_DataRow["Email"]);
                        _Customer.Comment = Convert.ToString(_DataRow["CustomerComment"]);
                        _Customer.Password = Convert.ToString(_DataRow["CustomerPassword"]);
                        _Customer.ContactNo = Convert.ToString(_DataRow["ContactNo"]);
                        _Customer.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        _Customer.FullName = Convert.ToString(_DataRow["FirstName"]) + Convert.ToString(_DataRow["LastName"]);
                        _ListOfCustomerInformation.Add(_Customer);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerInformation;
        }

        /// <summary>
        /// Description: This Methods Insert Admin Customer.
        /// </summary>
        /// <param name="p_Customer">Object of Customer</param>
        /// <returns>Primary Key of Customer.</returns>
        public string InsertAdminCustomer(Customer p_Customer)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ICustomerModel.InsertAdminCustomer(p_Customer);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will update customer.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_Customer">object of Customer.</param>
        /// <returns>Count</returns>
        public string UpdateCustomer(Customer p_Customer)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ICustomerModel.UpdateCustomer(p_Customer);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will get Asp Roles.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <returns>List of object of customer.</returns>
        public List<Customer> GetCustomerByClientID(int p_ClientID)
        {
            try
            {
                DataSet _DataSet = null;
                List<Customer> _ListOfCustomer = null;
                _DataSet = _ICustomerModel.GetCustomerByClientID(p_ClientID);
                _ListOfCustomer = FillAllCustomerByClientID(_DataSet);
                return _ListOfCustomer;
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
        private List<Customer> FillAllCustomerByClientID(DataSet _DataSet)
        {
            List<Customer> _ListOfCustomerInformation = new List<Customer>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        Customer _Customer = new Customer();
                        _Customer.CustomerKey = Convert.ToInt32(_DataRow["CustomerKey"]);
                        _Customer.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        _Customer.LastName = Convert.ToString(_DataRow["LastName"]);
                        _Customer.Email = Convert.ToString(_DataRow["Email"]);
                        _Customer.Password = Convert.ToString(_DataRow["CustomerPassword"]);
                        _Customer.FullName = Convert.ToString(_DataRow["FirstName"]) + " " + Convert.ToString(_DataRow["LastName"]);
                        _ListOfCustomerInformation.Add(_Customer);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerInformation;
        }

        /// <summary>
        /// Description:This method will Get Customer By clientID and RoleName.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <param name="p_RoleName">RoleName</param>
        /// <returns>List of object of customer.</returns>
        public List<Customer> GetCustomerByClientIDRoleName(long p_ClientID, string p_RoleName)
        {
            try
            {
                DataSet _DataSet = null;
                List<Customer> _ListOfCustomer = null;
                _DataSet = _ICustomerModel.GetCustomerByClientIDRoleName(p_ClientID, p_RoleName);
                _ListOfCustomer = FillAllCustomerByClientID(_DataSet);
                return _ListOfCustomer;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will Fill Customer By CustomerID and RoleName.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <param name="p_RoleName">RoleName</param>
        /// <returns>List of object of customer.</returns>
        public List<Customer> GetCustomerByCustomerIDRoleName(long p_CustomerID, string p_RoleName)
        {
            try
            {
                DataSet _DataSet = null;
                List<Customer> _ListOfCustomer = null;
                _DataSet = _ICustomerModel.GetCustomerByCustomerIDRoleName(p_CustomerID, p_RoleName);
                _ListOfCustomer = FillAllCustomerByCustomerIDRole(_DataSet);
                return _ListOfCustomer;
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
        private List<Customer> FillAllCustomerByCustomerIDRole(DataSet _DataSet)
        {
            List<Customer> _ListOfCustomerInformation = new List<Customer>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        Customer _Customer = new Customer();

                        _Customer.RoleID = Convert.ToInt64(_DataRow["RoleID"]);

                        _ListOfCustomerInformation.Add(_Customer);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfCustomerInformation;
        }

        /// <summary>
        /// Description:This method Get CustomerData
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_CustomerGUID">CustomerGUID</param>        
        /// <returns>List of object of customer.</returns>
        public List<Customer> GetCustomerNameByCustomerGUID(string p_CustomerGUID)
        {
            try
            {
                DataSet _DataSet = null;
                List<Customer> _ListOfCustomer = null;
                _DataSet = _ICustomerModel.GetCustomerNameByCustomerGUID(p_CustomerGUID);
                _ListOfCustomer = FillCustomerInformation(_DataSet);
                return _ListOfCustomer;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public List<Customer> GetCustomerByCustomerGUIDForAuthentication(Guid p_CustomerGUID)
        {
            try
            {
                DataSet _DataSet = null;
                List<Customer> _ListOfCustomer = null;
                _DataSet = _ICustomerModel.GetCustomerByCustomerGUIDForAuthentication(p_CustomerGUID);
                _ListOfCustomer = FillCustomerInformation(_DataSet);
                return _ListOfCustomer;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
