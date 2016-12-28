using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQMediaGroup.AdvanceSearchServiceSample.Class
{
    public class RadioRawMediaInput
    {
        /// <summary>
        /// List of RadioStation
        /// </summary>
        public List<RadioStationOut> RadioStation { get; set; }

        /// <summary>
        /// PageNumber
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// PageSize
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// FromDate
        /// </summary>
        public string FromDate { get; set; }

        /// <summary>
        /// ToDate
        /// </summary>
        public string ToDate { get; set; }

        /// <summary>
        /// SortField
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// SessionID
        /// </summary>
        public string SessionID { get; set; }

        /// <summary>
        /// CustomerID
        /// </summary>
        public Int64 CustomerID { get; set; }
    }
}