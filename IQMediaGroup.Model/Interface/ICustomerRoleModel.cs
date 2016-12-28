using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface of Customer Role
    /// </summary>
    public interface ICustomerRoleModel
    {
        /// <summary>
        /// This method gets the customer role access.
        /// </summary>
        /// <param name="p_CustomerKey">CustomerKey</param>
        /// <returns>Dataset that contains access information.</returns>
        DataSet GetCustomerRoleAccess(int p_CustomerKey);

        /// <summary>
        /// This method inserts Customer Role Information.
        /// </summary>
        /// <param name="_CustomerRoles"></param>
        /// <returns></returns>
        string InsertCustomerRole(CustomerRoles _CustomerRoles);

        /// <summary>
        /// This method gets all customer role information
        /// </summary>
        /// <returns></returns>
        DataSet GetCustomerRoleAdmin();

        /// <summary>
        /// This method updates CustomerRole 
        /// </summary>
        /// <param name="p_CustomerRole">Object of CustomerRoles</param>
        /// <returns></returns>
        string UpdateCustomerRole(CustomerRoles p_CustomerRole);

        /// <summary>
        /// This method gets Customer and Client Role Access Information
        /// </summary>
        /// <param name="p_CustomerKey"></param>
        /// <returns></returns>
        DataSet GetCustomerClientRoleAccess(int p_CustomerKey);
    }
}
