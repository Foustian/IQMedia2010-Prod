using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Admin.Core.HelperClasses
{
    /// <summary>
    /// This class contains Customer details
    /// </summary>
    [Serializable]
    public class Customer   
    {
        /// <summary> 
        /// Represents Primary Key
        /// </summary>
        public int CustomerKey { get; set; }

        public string CustomerGUID { get; set; }
        /// <summary>
        /// Represents First Name of Customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary> 
        /// Represents LastName of Customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents Full Name Of The Customer
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Represents Email of Customer
        /// </summary>
        public string Email { get; set; }

         /// <summary>
        /// Represents Password of Customer
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Represents Password of Customer
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// Represents ContactNo of Customer
        /// </summary>
        public string ContactNo { get; set; }

        /// <summary>
        /// Represents Comment given by Customer
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Represents CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Represents CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Represents ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Represents Flag for particular record is active or not.
        /// </summary>
        public bool? IsActive { get; set; }        

        /// <summary>
        /// Represents Unique Key associated with client
        /// </summary>
        public int ClientID { get; set; }

        public string ClientGUID { get; set; }

        /// <summary>
        /// Represents Unique Key associated with client
        /// </summary>
        public long RoleID { get; set; }

        /// <summary>
        /// Represents Client Name Associated with the customer
        /// </summary>
        public string ClientName { get; set; }

        public string MasterClient { get; set; }

        /// <summary>
        ///  Represents Role Name Associated with the customer
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Represents Client Name Associated with the customer
        /// </summary>
        public string CustomHeaderImage { get; set; }

        public bool IsCustomHeader { get; set; }

        public bool? MultiLogin { get; set; }

        public bool IQBasic { get; set; }

        public bool AdvancedSearchAccess { get; set; }

        public bool GlobalAdminAccess { get; set; }

        public bool IQAgentUser { get; set; }

        public bool IQAgentAdminAccess { get; set; }

        public bool IQCustomAccess { get; set; }

        public bool myIQAccess { get; set; }

        public bool IQAgentWebsiteAccess { get; set; }

        public bool DownloadClips { get; set; }

        public string DefaultPage { get; set; }

        public bool UGCDownload { get; set; }

        public bool? IsClientPlayerLogoActive { get; set; }

        public string ClientPlayerLogoImage { get; set; }

        public bool UGCUploadEdit { get; set; }

    }
}
