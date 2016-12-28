using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
//using PMGSearch;

namespace IQMediaGroup.Admin.Core.HelperClasses
{
    public class SessionInformation
    {
        /// <summary>
        /// Represents ClientID
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// Represents Client Name
        /// </summary>
        public string ClientName { get; set; }

        public string ClientGUID { get; set; }

        public int AdminUserKey { get; set; }

        /// <summary>
        /// Represents Custome RoleInformation
        /// </summary>
        public List<CustomerRoles> CustomerRoles { get; set; }

        /// <summary>
        /// Contains the Sort Expression
        /// </summary>
        public string SortExpression { get; set; }

        /// <summary>
        /// Contains the Sort Direction
        /// </summary>
        public string SortDirection { get; set; }

        public List<Customer> _ListOfAdminCustomer { get; set; }

        public List<Customer> _ListOfSelectedAdminCustomer { get; set; }

        public bool IsAdminLogin { get; set; }

        public bool IsIQMediaAdmin { get; set; }
    }
}
