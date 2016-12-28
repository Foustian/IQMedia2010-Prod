using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class RadioMediaInput
    {
        /// <summary>
        /// List of RadioStation
        /// </summary>
        public List<RadioStation> RadioStationList { get; set; }

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
        public DateTime? FromDateTime { get; set; }

        /// <summary>
        /// ToDate
        /// </summary>
        public DateTime? ToDateTime { get; set; }

        /// <summary>
        /// SortField
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// SessionID
        /// </summary>
        public string SessionID { get; set; }
        
    }
}
