using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// This class contains Customer Role details
    /// </summary>
    [Serializable]
    public class ClientRoles
    {
        /// <summary>
        /// Represents Customer Key
        /// </summary>
        public int ClientRoleKey { get; set; }

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
        public int ClientID { get; set; }

        /// <summary>
        /// Represents Customer's Full Name.
        /// </summary>
        public string ClientName { get; set; }

        //public string RoleName { get; set; }

    }
}
