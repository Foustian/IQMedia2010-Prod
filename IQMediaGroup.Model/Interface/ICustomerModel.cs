using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface of Customer
    /// </summary>
    public interface ICustomerModel
    {
        /// <summary>
        /// This mehtod checks authentication for provided email and password.
        /// </summary>
        /// <param name="p_Email">Email</param>
        /// <param name="p_Password">Password</param>
        /// <returns>CustomerKey if authentication successful</returns>
        DataSet CheckAuthentication(string p_Email, string p_Password);

        /// <summary>
        /// This mehtod gets password for provided email.
        /// </summary>
        /// <param name="p_Email">Email</param>
        /// <returns>Password</returns>
        DataSet ForgotPassword(string p_Email);

        /// <summary>
        /// This method inserts customer details.
        /// </summary>
        /// <param name="p_Customer">Object of Core class of Customer</param>
        /// <returns>CustomerKey if record added.</returns>
        string InsertCustomer(Customer p_Customer);

        /// <summary>
        /// This method gets customername & customerkey by clientid.
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <returns>Dataset containing Customer information.</returns>
        DataSet GetCustomerNameByClientID(Int64 p_ClientID);

        /* /// <summary>
         /// This method gets clip information by clientid.
         /// </summary>
         /// <param name="p_Customer">Object of customer class.</param>
         /// <returns>Dataset containing Customer information.</returns>
         DataSet GetMyClipByCustomerID(Customer p_Customer);*/

        /// <summary>
        /// This method gets all customers.
        /// </summary>
        /// <returns>Dataset that contains Customer information.</returns>
        DataSet GetAllCustomers();

        /// <summary>
        /// This method gets All Active Customer.
        /// </summary>
        /// <returns>Dataset of Active Customer.</returns>
        DataSet GetAllActiveCustomers();

        /// <summary>
        /// This Method inserts customer information through Admin
        /// </summary>
        /// <param name="p_Customer">Object of Core class of Customer</param>
        /// <returns>CustomerKey if record added.</returns>
        string InsertAdminCustomer(Customer p_Customer);

        /// <summary>
        /// This method updates Customer.
        /// </summary>
        /// <param name="p_Customer">Object of Core class of Customer</param>
        /// <returns>List of object of Datatype</returns>
        string UpdateCustomer(Customer p_Customer);

        /// <summary>
        /// This metod gets customer details by ClientID
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <returns>List of object of Datatype</returns>
        DataSet GetCustomerByClientID(int p_ClientID);

        /// <summary>
        /// This metod gets customer details by ClientID & by specified role name
        /// </summary>
        /// <param name="p_ClientID">p_ClientID</param>
        /// <param name="p_RoleName">p_RoleName</param>
        /// <returns>List of object of Datatype</returns>
        DataSet GetCustomerByClientIDRoleName(long p_ClientID, string p_RoleName);

        /// <summary>
        /// This metod gets customer details by customerid & role name
        /// </summary>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <param name="p_RoleName">RoleName</param>
        /// <returns>Data Set of Customer.</returns>
        DataSet GetCustomerByCustomerIDRoleName(long p_CustomerID, string p_RoleName);

        /// <summary>
        /// This method gets Customer name by CustomerGUID
        /// </summary>
        /// <param name="p_CustomerGUID">ListOfCustomerGUID</param>
        /// <returns>Dataset of Customer</returns>
        DataSet GetCustomerNameByCustomerGUID(string p_CustomerGUID);

        DataSet GetCustomerByCustomerGUIDForAuthentication(Guid p_CustomerGUID);
    }
}
