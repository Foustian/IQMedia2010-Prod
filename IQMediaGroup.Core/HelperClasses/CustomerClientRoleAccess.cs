using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// This class contains Customer Client Role Access details
    /// </summary>
    public class CustomerClientRoleAccess
    {
        /// <summary>
        /// Represents Customer Role UniqueID
        /// </summary>
        public int CustomerRoleKey { get; set; }

        /// <summary>
        /// Represents Customer's UniqueID
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// Represents Customer's UniqueID
        /// </summary>
        public int CustomerRoleID { get; set; }

        /// <summary>
        /// Represents Customer's Role Access Status
        /// </summary>
        public bool CustomerAccess { get; set; }

        /// <summary>
        /// Represents CustomerRole's UniqueID
        /// </summary>
        public int ClientRoleKey { get; set; }

        /// <summary>
        /// Represents Customer's RoleID
        /// </summary>
        public int ClientRoleID { get; set; }

        /// <summary>
        /// Represents Client's Role Access Status
        /// </summary>
        public bool ClientAccess { get; set; }

        /// <summary>
        /// Represents Role's UniqueID
        /// </summary>
        public int RoleKey { get; set; }

        /// <summary>
        /// Represents Role Name
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Represents Role Status
        /// </summary>
        public bool RoleIsActive { get; set; }



    }
}
