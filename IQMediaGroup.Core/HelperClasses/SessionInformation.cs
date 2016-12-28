using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
//using PMGSearch;

namespace IQMediaGroup.Core.HelperClasses
{
    public class SessionInformation
    {
        /// <summary>
        /// Contains the Email of Customer
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Contains Details  of Login or logout.
        /// </summary>
        public bool IsLogIn { get; set; }

        /// <summary>
        /// Represents RawMedia Search Result
        /// </summary>
        public DataTable RawMediaSearchResult { get; set; }

        /// <summary>
        /// Represents Tab Active Key
        /// </summary>
        public string TabActiceKey { get; set; }

        /// <summary>
        /// Represents ClientID
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// Represents Client Name
        /// </summary>
        public string ClientName { get; set; }

        public string ClientGUID { get; set; }

        /// <summary>
        /// Contains Detail of Client Player Logo Active or Not
        /// </summary>
        public bool? IsClientPlayerLogoActive { get; set; }

        /// <summary>
        /// Represents Name of Image for Client Player Logo
        /// </summary>
        public string ClientPlayerLogoImage { get; set; }

        /// <summary>
        /// Represents Customer Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents customer's key.
        /// </summary>
        public int CustomerKey { get; set; }

        public string CustomerGUID { get; set; }

        /// <summary>
        /// Represents Custome RoleInformation
        /// </summary>
        public List<CustomerRoles> CustomerRoles { get; set; }

        /// <summary>
        /// Represents Custome And Client RoleInformation
        /// </summary>
        public List<CustomerClientRoleAccess> CustomerClientRoles { get; set; }

        /// <summary>
        /// represent collection of sorted data
        /// </summary>
        public DataView SortTable { get; set; }

        /// <summary>
        /// Contains the Sort Expression
        /// </summary>
        public string SortExpression { get; set; }

        /// <summary>
        /// Contains the Sort Direction
        /// </summary>
        public string SortDirection { get; set; }

        public List<IQAgentResults> _ListOfIQAgentResults { get; set; }

        public string ErrorMessage { get; set; }

        public List<RawMedia> ListOfRawMedia { get; set; }

        public string CustomeHeaderImage { get; set; }

        public bool ISCustomeHeader { get; set; }

        public List<Role> _ListOfRoleName { get; set; }

        public bool IsmyIQ { get; set; }

        public bool IsiQBasic { get; set; }

        public bool IsiQAdvance { get; set; }

        public bool IsiQAgent { get; set; }

        public bool IsiQCustom { get; set; }

        public bool? MultiLogin { get; set; }

        public List<Guid> ListOfSelectedClipsFDownlLoad { get; set; }

        public List<string> ListOfSelectedArchiveNMDownload { get; set; }

        public List<string> ListOfSelectedArchiveSMDownload { get; set; }

        public List<string> ListOfSelectedArchiveBLPMDownload { get; set; }

        public bool IsDownloadClips { get; set; }

        public bool IsUGCDownload { get; set; }

        public bool IsUGCUploadEdit { get; set; }

        public bool IsUgcAutoClip { get; set; }

        public bool IsiQPremium { get; set; }

        public bool IsMyIQnew { get; set; }

        public bool IsNielSenData { get; set; }

        public bool IsiQPremiumSM { get; set; }

        public bool IsiQPremiumNM { get; set; }

        public bool IsmyiQSM { get; set; }

        public bool IsmyiQNM { get; set; }

        public bool IsmyiQPM { get; set; }

        public bool IsiQAgentReport { get; set; }

        public bool IsMyiQReport { get; set; }

        public bool IsMyIQTwitter { get; set; }

        public bool IsiQPremiumTwitter { get; set; }

        public bool IsiQPremiumAgent { get; set; }

        public bool IsiQPremiumRadio { get; set; }

        public bool IsiQPremiumSentiment { get; set; }

        public Int16? AuthorizedVersion { get; set; }

        public string LastName { get; set; }

        public bool IsCompeteData { get; set; }
    }
}
