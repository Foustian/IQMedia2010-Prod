using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Admin.Core.HelperClasses
{
    /// <summary>
    /// Contais Client Details
    /// </summary>
    [Serializable]
    public class Client
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public int ClientKey { get; set; }

        /// <summary>
        /// Represents Client Name
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Represents Status Of The Client
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public int RoleID { get; set; }

        public Guid ClientGUID { get; set; }

        public string DefaultCategory { get; set; }

        public bool IQBasic { get; set; }

        public bool AdvancedSearchAccess { get; set; }

        public bool GlobalAdminAccess { get; set; }

        public bool IQAgentUser { get; set; }

        public bool IQAgentAdminAccess { get; set; }

        public bool myIQAccess { get; set; }

        public bool IQAgentWebsiteAccess { get; set; }

        public bool IQCustomAccess { get; set; }

        public bool DownloadClips { get; set; }

        public bool UGCDownload { get; set; }

        public bool IframeMicroSite { get; set; }

        public bool UGCUploadEdit { get; set; }

        public Int64? PricingCodeID { get; set; }

        public Int64? BillFrequencyID { get; set; }

        public Int64? BillTypeID { get; set; }

        public Int64? IndustryID { get; set; }

        public Int64? StateID { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public string Attention { get; set; }

        public string Phone { get; set; }

        public string MasterClient { get; set; }

        public Int32? NoOfUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string CustomHeaderPath { get; set; }

        public bool IsCustomHeader { get; set; }

        public string PlayerLogoPath { get; set; }

        public bool IsActivePlayerLogo { get; set; }

        public bool CDNUpload { get; set; }

        public Int16? NoOfIQNotification { get; set; }

        public Int16? NoOfIQAgnet { get; set; }

        public decimal? CompeteMultiplier { get; set; }

        public decimal? OnlineNewsAdRate { get; set; }

        public decimal? OtherOnlineAdRate { get; set; }

        public decimal? UrlPercentRead { get; set; }
    }
}
