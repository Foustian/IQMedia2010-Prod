using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Admin.Core.HelperClasses;

namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface for Role
    /// </summary>
    public interface IRoleModel
    {
        /// <summary>
        /// This method gets Role information.
        /// </summary>
        /// <returns></returns>
        DataSet GetRoleInfo(bool? p_IsActive);

        /// <summary>
        /// This method inserts Role.
        /// </summary>
        /// <param name="p_Role">Class That Represents Role Information</param>
        /// <returns>String Containing Role Key</returns>
        //string InsertRole(Role p_Role);

        /// <summary>
        /// This method updates Role.
        /// </summary>
        /// <param name="p_Role">Class That Represents Role Information</param>
        /// <returns>String Containing Role Key</returns>
        //string UpdateRole(Role p_Role);

        DataSet GetRoleName(string p_RoleName);
    }
}
