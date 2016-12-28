using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class RawMediaInput
    {
        /// <summary>
        /// Represents SearchTerm 
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Represents Title120
        /// </summary>
        public string ProgramTitle { get; set; }

        /// <summary>
        /// Represents FromDate
        /// </summary>
        public DateTime? FromDateTime { get; set; }

        /// <summary>
        /// Represents ToDate
        /// </summary>
        public DateTime? ToDateTime { get; set; }

        /// <summary>
        /// Represents IQ_Dma_Set
        /// </summary>
        public List<Dma> DmaList { get; set; }

        /// <summary>
        /// Represents Station_Affil_Set
        /// </summary>
        public List<Affiliate> AffiliateList { get; set; }

        /// <summary>
        /// Represents IQ_Time_Zone
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Represents PageSize
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Represents PageNumber
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// Represents Sort Field
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// Represents SessionID
        /// </summary>
        public string SessionID { get; set; }

        public Guid ClientGUID { get; set; }

        public Int64 CustomerID { get; set; }
    }
}
