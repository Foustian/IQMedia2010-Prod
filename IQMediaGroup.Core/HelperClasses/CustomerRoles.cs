using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// This class contains Customer Role details
    /// </summary>
    
    public class CustomerRoles
    {
        /// <summary>
        /// Represents Customer Key
        /// </summary>
        public int CustomerRoleKey { get; set; }

        /// <summary>
        /// Represents Role Name
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Represents Status On The Role 
        /// </summary>
        public bool IsAccess { get; set; }

        /// <summary>
        /// Represents RoleID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// Represents CustomerID
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// Represents Customer's Full Name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Represents Client's Full Name.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Represents Client's ID.
        /// </summary>
        public int ClientID { get; set; }

    }
}
