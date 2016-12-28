using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contais Role Details
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// Represents Role Name
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Represents Status Of The Role
        /// </summary>
        public bool IsActive { get; set; }

        public Int32 CustomerID { get; set; }
    }
}
