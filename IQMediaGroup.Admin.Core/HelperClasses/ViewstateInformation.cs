using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;

namespace IQMediaGroup.Admin.Core.HelperClasses
{
    [Serializable]
    public class ViewstateInformation
    {
        /// <summary>
        /// Contains the Sort Direction
        /// </summary>
        public string SortDirection { get; set; }

        /// <summary>
        /// Represents ClientName
        /// </summary>
        public string VSClientName { get; set; }

        public string VSClientNameGrid { get; set; }

        public Int64 AdminClientID { get; set; }

        public Int64 AdminCustomerID { get; set; }

        /// <summary>
        /// Represents List of objects of ClientRoles
        /// </summary>
        public List<ClientRoles> _ListOfClientRoles { get; set; }

        public List<Client> _ListOfClient { get; set; }

        public int? CurrentPage { get; set; }

        public int EnabledClientCount { get; set; }

        public int TotalPages { get; set; }

        public MasterStatSkedProg _MasterStatSkedProg { get; set; }

        public string SortDirAffil { get; set; }
        public string SortDirDma { get; set; }

        public Boolean IsAllDmaAllowed { get; set; }

        public Boolean IsAllStationAllowed { get; set; }

        public Boolean IsAllClassAllowed { get; set; }

        public int DefaultIQAgentNotificationCount { get; set; }

        public int DefaultIQAgentCount { get; set; }

        public decimal DefaultCompeteMultipier { get; set; }

        public decimal DefaultOnlineNewsAdRate { get; set; }

        public decimal DefaultOtherOnlineAdRate { get; set; }

        public decimal DefaultURLPercentRead { get; set; }

    }
}
