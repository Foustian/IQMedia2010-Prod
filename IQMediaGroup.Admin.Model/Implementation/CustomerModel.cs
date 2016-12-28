using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Model.Interface;
using System.Data;


namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface ICustomerModel
    /// </summary>
    internal class CustomerModel : IQMediaGroupDataLayer, ICustomerModel
    {
        /// <summary>
        /// This mehtod checks authentication for provided email and password.
        /// </summary>
        /// <param name="p_Email">Email</param>
        /// <param name="p_Password">Password</param>
        /// <returns>CustomerKey if authentication successful</returns>
        public DataSet CheckAuthentication(string p_Email, string p_Password)
        {
            DataSet _DataSet = new DataSet();
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@Email", DbType.String, p_Email, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerPassword", DbType.String, p_Password, ParameterDirection.Input));
                _DataSet = this.GetDataSet("usp_Customer_CheckAuthentication", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This mehtod gets password for provided email.
        /// </summary>
        /// <param name="p_Email">Email</param>
        /// <returns>Password</returns>
        public DataSet ForgotPassword(string p_Email)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@Email", DbType.String, p_Email, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Customer_PasswordReminder", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method inserts customer details.
        /// </summary>
        /// <param name="p_Customer">Object of Core class of Customer</param>
        /// <returns>CustomerKey if record added.</returns>
        public string InsertCustomer(Customer p_Customer)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@FirstName", DbType.String, p_Customer.FirstName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LastName", DbType.String, p_Customer.LastName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Email", DbType.String, p_Customer.Email, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerPassword", DbType.String, p_Customer.Password, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ContactNo", DbType.String, p_Customer.ContactNo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Comments", DbType.String, p_Customer.Comment, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, p_Customer.CustomerGUID, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@CreatedBy", DbType.String, p_Customer.CreatedBy, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int32, p_Customer.CustomerKey, ParameterDirection.Output));

                _Result =  ExecuteNonQuery("usp_Customer_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets clip information by clientid.
        /// </summary>
        /// <param name="p_Customer">Object of customer class.</param>
        /// <returns>Dataset containing Customer information.</returns>
        public DataSet GetMyClipByClientID(Customer p_Customer)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int32, p_Customer.ClientID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_GetMyClip_ByClientID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        

       /* /// <summary>
        /// This method gets clip information by CustomerID.
        /// </summary>
        /// <param name="p_Customer">Object of customer class.</param>
        /// <returns>Dataset containing Customer information.</returns>
        public DataSet GetMyClipByCustomerID(Customer p_Customer)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int32, p_Customer.CustomerKey, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Customer_SelectByCustomerID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }*/

        /// <summary>
        /// This method gets all customers.
        /// </summary>
        /// <returns>Dataset that contains Customer information.</returns>
        public DataSet GetAllCustomers()
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                DataSet _DataSet = new DataSet();

                _DataSet = this.GetDataSetByProcedure("usp_Customer_SelectAll");

                 return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetCustomerInfoBySearchTerm(string p_prefixText)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@prefixText", DbType.String, p_prefixText, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Customer_SelectBySearchTerm", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetCustomerInfoByFirstName(string p_Email)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@Email", DbType.String, p_Email, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Customer_SelectByFirstName", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets All Active Customer.
        /// </summary>
        /// <returns>Dataset of Active Customer.</returns>
        public DataSet GetAllActiveCustomers()
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                DataSet _DataSet = new DataSet();

                _DataSet = this.GetDataSetByProcedure("usp_Customer_SelectAllActiveCustomer");

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This Method inserts customer information through Admin
        /// </summary>
        /// <param name="p_Customer">Object of Core class of Customer</param>
        /// <returns>CustomerKey if record added.</returns>
        public string InsertAdminCustomer(Customer p_Customer)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@FirstName", DbType.String, p_Customer.FirstName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LastName", DbType.String, p_Customer.LastName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Email", DbType.String, p_Customer.Email, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerPassword", DbType.String, p_Customer.Password, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ContactNo", DbType.String, p_Customer.ContactNo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Comments", DbType.String, p_Customer.Comment, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int32, p_Customer.ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.String, p_Customer.CustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int32, p_Customer.CustomerKey, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@DefaultPage", DbType.String, p_Customer.DefaultPage, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MultiLogin", DbType.String, p_Customer.MultiLogin, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_CustomerAdmin_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method updates Customer.
        /// </summary>
        /// <param name="p_Customer">Object of Core class of Customer</param>
        /// <returns>List of object of Datatype</returns>
        public string UpdateCustomer(Customer p_Customer)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@FirstName", DbType.String, p_Customer.FirstName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LastName", DbType.String, p_Customer.LastName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Email", DbType.String, p_Customer.Email, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerPassword", DbType.String, p_Customer.Password, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ContactNo", DbType.String, p_Customer.ContactNo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.String, p_Customer.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientName", DbType.String, p_Customer.ClientName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int32, p_Customer.CustomerKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int32, p_Customer.ClientID, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_Customer_Update", _ListOfDataType);

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
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@FirstName", DbType.String, p_Customer.FirstName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LastName", DbType.String, p_Customer.LastName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Email", DbType.String, p_Customer.Email, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerPassword", DbType.String, p_Customer.Password, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ContactNo", DbType.String, p_Customer.ContactNo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerComment", DbType.String, p_Customer.Comment, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.String, p_Customer.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int32, p_Customer.CustomerKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ModifiedDate", DbType.DateTime, p_Customer.ModifiedDate, ParameterDirection.Input));
                //_ListOfDataType.Add(new DataType("@ClientID", DbType.Int32, p_Customer.ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultPage", DbType.String, p_Customer.DefaultPage, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MultiLogin", DbType.Boolean, p_Customer.MultiLogin, ParameterDirection.Input));


                _Result = ExecuteNonQuery("usp_CustomerAdmin_Update", _ListOfDataType);

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
                List<DataType> _ListOfDataType = new List<DataType>();
                int _output=0;
                EmailCout = 0;
                _ListOfDataType.Add(new DataType("@FirstName", DbType.String, p_Customer.FirstName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LastName", DbType.String, p_Customer.LastName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Email", DbType.String, p_Customer.Email, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerPassword", DbType.String, p_Customer.Password, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ContactNo", DbType.String, p_Customer.ContactNo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerComment", DbType.String, p_Customer.Comment, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.String, p_Customer.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerKey", DbType.Int32, p_Customer.CustomerKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ModifiedDate", DbType.DateTime, p_Customer.ModifiedDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultPage", DbType.String, p_Customer.DefaultPage, ParameterDirection.Input));
                //_ListOfDataType.Add(new DataType("@ClientID", DbType.Int32, p_Customer.ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@EmailCount", DbType.Int32, _output, ParameterDirection.Output));


                Dictionary<string, string> _OutputParams = null;

                _Result = ExecuteNonQuery("usp_CustomerAdmin_CustomerUpdate", _ListOfDataType,out _OutputParams);


                if (_OutputParams!=null && _OutputParams.Count>0)
                {
                    EmailCout = Convert.ToInt32(_OutputParams["@EmailCount"]);
                }

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This metod gets customer details by ClientID
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <returns>List of object of Datatype</returns>
        public DataSet GetCustomerByClientID(int p_ClientID)
        {
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int32, p_ClientID, ParameterDirection.Input));
                
                DataSet _DataSet = new DataSet();

                _DataSet = this.GetDataSet("usp_Customer_SelectByClientID",_ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This metod gets customer details by ClientID & by specified role name
        /// </summary>
        /// <param name="p_ClientID">p_ClientID</param>
        /// <param name="p_RoleName">p_RoleName</param>
        /// <returns>List of object of Datatype</returns>
        public DataSet GetCustomerByClientIDRoleName(long p_ClientID,string p_RoleName)
        {
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int32, p_ClientID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RoleName", DbType.String, p_RoleName, ParameterDirection.Input));

                DataSet _DataSet = new DataSet();

                _DataSet = this.GetDataSet("usp_Customer_SelectByClientIDRoleName", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This metod gets customer details by customerid & role name
        /// </summary>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <param name="p_RoleName">RoleName</param>
        /// <returns>Data Set of Customer.</returns>
        public DataSet GetCustomerByCustomerIDRoleName(long p_CustomerID, string p_RoleName)
        {
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int32, p_CustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RoleName", DbType.String, p_RoleName, ParameterDirection.Input));

                DataSet _DataSet = new DataSet();

                _DataSet = this.GetDataSet("usp_Customer_SelectByCustomerRoleName", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetAllCustomerWithRoleByClientID(string p_ClientName)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientName", DbType.String, p_ClientName, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Customer_SelectAllCustomerWithRole", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetAllCustomerWithRoleByClientGUID(Guid p_ClientGUID)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Customer_SelectAllCustomerWithRoleByClientGUID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetCustomerInfoWithRoleByCustomerID(Int64 p_CustomerID)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerID", DbType.String, p_CustomerID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_Customer_SelectAllCustomerWithRoleByCustomerID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
