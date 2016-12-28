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
    internal class CustomerController : ICustomerController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ICustomerModel _ICustomerModel;
        string _SuccessMessage = "Your password has been sent to your email.";
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
                        _Customer.CustomerKey = Convert.ToInt32(_DataRow["CustomerKey"]);
                        _Customer.ClientID = Convert.ToInt32(_DataRow["ClientID"]);
                        _Customer.ClientName = Convert.ToString(_DataRow["ClientName"]);
                        _Customer.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        _Customer.Email = Convert.ToString(_DataRow["Email"]);
                        _Customer.ClientGUID = Convert.ToString(_DataRow["ClientGUID"]);
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
        public List<Customer> GetMyClipByClientID(Customer p_Customer)
        {
            DataSet _DataSet = null;
            List<Customer> _ListOfCustomer = null;

            try
            {
                _DataSet = _ICustomerModel.GetMyClipByClientID(p_Customer);
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



        /*/// <summary>
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


        public List<Customer> GetCustomerInfoBySearchTerm(string p_prefixText)
        {
            try
            {
                List<Customer> _ListOfCustomer = null;

                DataSet _DataSet = _ICustomerModel.GetCustomerInfoBySearchTerm(p_prefixText);

                _ListOfCustomer = FillAllCustomerBySearchTerm(_DataSet);

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
        private List<Customer> FillAllCustomerBySearchTerm(DataSet _DataSet)
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
                        //_Customer.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        //_Customer.LastName = Convert.ToString(_DataRow["LastName"]);
                        _Customer.Email = Convert.ToString(_DataRow["Email"]);
                        //_Customer.Comment = Convert.ToString(_DataRow["CustomerComment"]);
                        //_Customer.Password = Convert.ToString(_DataRow["CustomerPassword"]);
                        //_Customer.ContactNo = Convert.ToString(_DataRow["ContactNo"]);
                        //_Customer.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        //_Customer.ClientName = Convert.ToString(_DataRow["ClientName"]);
                        //_Customer.ClientID = Convert.ToInt32(_DataRow["ClientID"]);
                        //_Customer.FullName = Convert.ToString(_DataRow["FirstName"]) + Convert.ToString(_DataRow["LastName"]);
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

        public List<Customer> GetCustomerInfoByFirstName(string p_Email)
        {
            try
            {
                List<Customer> _ListOfCustomer = null;

                DataSet _DataSet = _ICustomerModel.GetCustomerInfoByFirstName(p_Email);

                _ListOfCustomer = FillAllCustomerByFirstName(_DataSet);

                return _ListOfCustomer;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<Customer> FillAllCustomerByFirstName(DataSet _DataSet)
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
                        //_Customer.LastName = Convert.ToString(_DataRow["LastName"]);
                        _Customer.Email = Convert.ToString(_DataRow["Email"]);
                        //_Customer.Comment = Convert.ToString(_DataRow["CustomerComment"]);
                        //_Customer.Password = Convert.ToString(_DataRow["CustomerPassword"]);
                        //_Customer.ContactNo = Convert.ToString(_DataRow["ContactNo"]);
                        //_Customer.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        _Customer.ClientName = Convert.ToString(_DataRow["ClientName"]);
                        //_Customer.ClientID = Convert.ToInt32(_DataRow["ClientID"]);
                        //_Customer.FullName = Convert.ToString(_DataRow["FirstName"]) + Convert.ToString(_DataRow["LastName"]);
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

        public DataSet GetAllCustomerWithRoleByClientID(string p_ClientName)
        {
            DataSet _DataSet = null;
            try
            {
                _DataSet = _ICustomerModel.GetAllCustomerWithRoleByClientID(p_ClientName);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _DataSet;
        }


        public DataSet GetAllCustomerWithRoleByClientGUID(Guid p_ClientGUID)
        {
            DataSet _DataSet = null;
           
            try
            {
                _DataSet = _ICustomerModel.GetAllCustomerWithRoleByClientGUID(p_ClientGUID);
              
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _DataSet;
        }

        public DataSet GetCustomerInfoWithRoleByCustomerID(Int64 p_CustomerID)
        {
            DataSet _DataSet;
            try
            {
                _DataSet = _ICustomerModel.GetCustomerInfoWithRoleByCustomerID(p_CustomerID);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _DataSet;
        }
        
        private List<Customer> FillAllCustomerWithRole(DataSet _DataSet)
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
                        //_Customer.ClientName = Convert.ToString(_DataRow["ClientName"]);
                        _Customer.ClientID = Convert.ToInt32(_DataRow["ClientID"]);
                        _Customer.FullName = Convert.ToString(_DataRow["FirstName"]) + Convert.ToString(_DataRow["LastName"]);
                        _Customer.IQBasic = Convert.ToBoolean(_DataRow["IQBasic"]);
                        _Customer.AdvancedSearchAccess = Convert.ToBoolean(_DataRow["AdvancedSearchAccess"]);
                        _Customer.GlobalAdminAccess = Convert.ToBoolean(_DataRow["GlobalAdminAccess"]);

                        if (_DataSet.Tables[0].Columns.Contains("UGCUploadEdit") && !_DataRow["UGCUploadEdit"].Equals(DBNull.Value))
                        {
                            _Customer.UGCUploadEdit = Convert.ToBoolean(_DataRow["UGCUploadEdit"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQAgentUser") && !_DataRow["IQAgentUser"].Equals(DBNull.Value))
                        {
                            _Customer.IQAgentUser = Convert.ToBoolean(_DataRow["IQAgentUser"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("DefaultPage") && !_DataRow["DefaultPage"].Equals(DBNull.Value))
                        {
                            _Customer.DefaultPage = Convert.ToString(_DataRow["DefaultPage"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQAgentAdminAccess") && !_DataRow["IQAgentAdminAccess"].Equals(DBNull.Value))
                        {
                            _Customer.IQAgentAdminAccess = Convert.ToBoolean(_DataRow["IQAgentAdminAccess"]);
                        }
                        _Customer.myIQAccess = Convert.ToBoolean(_DataRow["myIQAccess"]);
                        _Customer.IQAgentWebsiteAccess = Convert.ToBoolean(_DataRow["IQAgentWebsiteAccess"]);
                        _Customer.DownloadClips = Convert.ToBoolean(_DataRow["DownloadClips"]);
                        _Customer.IQCustomAccess = Convert.ToBoolean(_DataRow["IQCustomAccess"]);
                        _Customer.UGCDownload = Convert.ToBoolean(_DataRow["UGCDownload"]);

                        if (_DataSet.Tables[0].Columns.Contains("MultiLogin") && !_DataRow["MultiLogin"].Equals(DBNull.Value))
                        {
                            _Customer.MultiLogin = Convert.ToBoolean(_DataRow["MultiLogin"]); 
                        }
                        else
                        {
                            _Customer.MultiLogin = false;
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

        private List<Customer> FillCustomerWithRoleByCustomerID(DataSet _DataSet)
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
                        //_Customer.ClientName = Convert.ToString(_DataRow["ClientName"]);
                        _Customer.ClientID = Convert.ToInt32(_DataRow["ClientID"]);
                        _Customer.FullName = Convert.ToString(_DataRow["FirstName"]) + Convert.ToString(_DataRow["LastName"]);
                        _Customer.IQBasic = Convert.ToBoolean(_DataRow["IQBasic"]);
                        _Customer.AdvancedSearchAccess = Convert.ToBoolean(_DataRow["AdvancedSearchAccess"]);
                        _Customer.GlobalAdminAccess = Convert.ToBoolean(_DataRow["GlobalAdminAccess"]);
                        _Customer.IQAgentUser = Convert.ToBoolean(_DataRow["IQAgentUser"]);
                        _Customer.IQAgentAdminAccess = Convert.ToBoolean(_DataRow["IQAgentAdminAccess"]);
                        _Customer.myIQAccess = Convert.ToBoolean(_DataRow["myIQAccess"]);
                        _Customer.IQAgentWebsiteAccess = Convert.ToBoolean(_DataRow["IQAgentWebsiteAccess"]);
                        _Customer.DownloadClips = Convert.ToBoolean(_DataRow["DownloadClips"]);
                        _Customer.CreatedDate = Convert.ToDateTime(_DataRow["CreatedDate"]);
                        _Customer.ClientName = Convert.ToString(_DataRow["ClientName"]);
                        _Customer.MasterClient = Convert.ToString(_DataRow["MasterClient"]);

                        _Customer.UGCDownload = Convert.ToBoolean(_DataRow["UGCDownload"]);

                        if (_DataSet.Tables[0].Columns.Contains("UGCUploadEdit") && !_DataRow["UGCUploadEdit"].Equals(DBNull.Value))
                        {
                            _Customer.UGCUploadEdit = Convert.ToBoolean(_DataRow["UGCUploadEdit"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQCustomAccess") && !_DataRow["IQCustomAccess"].Equals(DBNull.Value))
                        {
                            _Customer.IQCustomAccess = Convert.ToBoolean(_DataRow["IQCustomAccess"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("DefaultPage") && !_DataRow["DefaultPage"].Equals(DBNull.Value))
                        {
                            _Customer.DefaultPage = Convert.ToString(_DataRow["DefaultPage"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("MultiLogin") && !_DataRow["MultiLogin"].Equals(DBNull.Value))
                        {
                            _Customer.MultiLogin = Convert.ToBoolean(_DataRow["MultiLogin"]);
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

        public string UpdateAdminCustomer(Customer p_Customer)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ICustomerModel.UpdateAdminCustomer(p_Customer);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string UpdateClientAdminCustomer(Customer p_Customer, out int EmailCout)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ICustomerModel.UpdateClientAdminCustomer(p_Customer, out EmailCout);

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

    }
}
