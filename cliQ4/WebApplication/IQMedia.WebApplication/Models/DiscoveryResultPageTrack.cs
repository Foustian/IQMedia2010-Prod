using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQMedia.WebApplication.Models
{
    [Serializable]
    public class DiscoveryResultRecordTrack
    {
        public string SearchName { get; set; }
        public string SearchTerm { get; set; }

        public Int64 TVRecordShownNum { get; set; }
        public Int64 NMRecordShownNum { get; set; }
        public Int64 SMRecordShownNum { get; set; }
        public Int64 PQRecordShownNum { get; set; }

        public Int64? TVRecordTotal { get; set; }
        public Int64? NMRecordTotal { get; set; }
        public Int64? SMRecordTotal { get; set; }
        public Int64? PQRecordTotal { get; set; }

        public string TVFromRecordID { get; set; }
        public string NMFromRecordID { get; set; }
        public string SMFromRecordID { get; set; }
        public string PQFromRecordID { get; set; }

        public Int64? TotalRecords { get; set; }

        public bool IsTVValid { get; set; }
        public bool IsNMValid { get; set; }
        public bool IsSMValid { get; set; }
        public bool IsPQValid { get; set; }

    }
}
