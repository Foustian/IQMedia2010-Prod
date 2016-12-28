using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Admin.Core.HelperClasses;

namespace IQMediaGroup.Admin.Controller.Interface
{
    public interface ICustomerRoleController
    {
        /// <summary>
        /// Description: This Methods gets Customer Role Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_CustomerKey">CustomerID of Customer</param>
        /// <returns>List of Object of Customer Role</returns>
        List<CustomerRoles> GetCustomerRoleAccess(int p_CustomerKey);

        /// <summary>
        /// This method inserts Customer Role Information
        /// Added By: bhavik barot
        /// </summary>
        /// <param name="p_CustomerRoles">Object Of CustomerRoles Class</param>
        /// <returns></returns>
        string InsertCustomerRole(CustomerRoles p_CustomerRoles);

        /// <summary>
        /// This method gets customer Role information
        /// Added By: bhavik barot
        /// </summary>
        /// <returns>List of object of CustomerRoles details</returns>
        List<CustomerRoles> GetCustomerRoleAdmin();

        /// <summary>
        /// This method updates customer role information
        /// Added By: bhavik barot
        /// </summary>
        /// <param name="p_CustomerRole">Object of CustomerRol details</param>
        /// <returns>CustomerRole Key</returns>
        string UpdateCustomerRole(CustomerRoles p_CustomerRole);

        /// <summary>
        /// This method gets Client and customer role details
        /// Added By: bhavik barot
        /// </summary>
        /// <param name="p_CustomerKey">Customer Key</param>
        /// <returns>List of object of CustomerClientRoleAccess</returns>
        List<CustomerClientRoleAccess> GetCustomerClientRoleAccess(int p_CustomerKey);

        string UpdateCustomerRoleByClientIDRoleID(CustomerRoles p_CustomerRoles);
    }
}
