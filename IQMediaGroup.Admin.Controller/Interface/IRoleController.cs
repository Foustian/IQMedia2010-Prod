using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Web;
using System.Net;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface for Role
    /// </summary>
    public interface IRoleController
    {
        /// <summary>
        /// This method gets Role Information
        /// Added By:Bhavik Barot   
        /// </summary>
        /// <param name="p_IsActive">Status of the role</param>
        /// <returns>List of object of Role Class</returns>
        List<Role> GetRoleInformation(bool? p_IsActive);

        /// <summary>
        /// This method inserts Role Information
        /// Added By:Bhavik Barot 
        /// </summary>
        /// <param name="p_Role">Object of Role details</param>
        /// <returns>Role Key</returns>
        //string InsertRole(Role p_Role);

        /// <summary>
        /// This method updates Role Information
        /// Added By:Bhavik Barot 
        /// </summary>
        /// <param name="p_Role">Object of Role details class</param>
        /// <returns>Role Key</returns>
        //string UpdateRole(Role p_Role);

        List<Role> GetRoleName(string p_RoleName);
    }
}
