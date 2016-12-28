using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Net;

namespace IQMediaGroup.Controller.Interface
{
    /// <summary>
    /// Interface for Customer Controller
    /// </summary>
    public interface ICustomerController
    {
        /// <summary>
        /// This method authenticats Customer.
        /// </summary>
        /// <param name="p_Email">Email</param>
        /// <param name="p_Password">Password</param>
        /// <returns>CustomerKey if authentication successful</returns>
        List<Customer> CheckAuthentication(string p_Email, string p_Password);

        /// <summary>
        /// This mehtod gets password for provided email.
        /// </summary>
        /// <param name="p_Email">Email</param>
        /// <returns>Password</returns>
        List<Customer> ForgotPassword(string p_Email);
        
        /// <summary>
        /// This method adds new customer.
        /// </summary>
        /// <param name="p_Customer">Object of Core class Customer</param>
        /// <returns>CustomerKey if added successfully.</returns>
        string InsertCustomer(Customer p_Customer);              

        /// <summary>
        /// This method gets Clip information on the basis of ClientID
        /// </summary>
        /// <param name="p_Customer">Dataset containing Customer information.</param>
        /// <returns></returns>
        List<Customer> GetCustomerNameByClientID(Int64 p_ClientID);

        /// <summary>
        /// This method select all customer
        /// </summary>
        /// <returns>Object of List Of Customer.</returns>
        List<Customer> GetAllCustomers();

        /// <summary>
        /// This method select all Active customer
        /// </summary>
        /// <returns>Object of List Of Customer.</returns>
        List<Customer> GetAllActiveCustomers();

        /// <summary>
        /// Description: This Methods Insert Admin Customer.
        /// </summary>
        /// <param name="p_Customer">Object of Customer</param>
        /// <returns>Primary Key of Customer.</returns>
        string InsertAdminCustomer(Customer p_Customer);

        /// <summary>
        /// Description:This method will update customer.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_Customer">object of Customer.</param>
        /// <returns>Count</returns>
        string UpdateCustomer(Customer p_Customer);

        /// <summary>
        /// Description:This method will get Asp Roles.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <returns>List of object of customer.</returns>
        List<Customer> GetCustomerByClientID(int p_ClientID);

       /* /// <summary>
        /// This method gets Clip information on the basis of CustomerID
        /// </summary>
        /// <param name="p_Customer">Dataset containing Customer information.</param>
        /// <returns></returns>
        List<Customer> GetMyClipByCustomerID(Customer p_Customer);*/       

        /// <summary>
        /// Description:This method will Get Customer By clientID and RoleName.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ClientID">ClientID</param>
        /// <param name="p_RoleName">RoleName</param>
        /// <returns>List of object of customer.</returns>
        List<Customer> GetCustomerByClientIDRoleName(long p_ClientID, string p_RoleName);

        /// <summary>
        /// Description:This method will Fill Customer By CustomerID and RoleName.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <param name="p_RoleName">RoleName</param>
        /// <returns>List of object of customer.</returns>
        List<Customer> GetCustomerByCustomerIDRoleName(long p_CustomerID, string p_RoleName);

        List<Customer> GetCustomerNameByCustomerGUID(string p_CustomerGUID);

        List<Customer> GetCustomerByCustomerGUIDForAuthentication(Guid p_CustomerGUID);
    }
}

