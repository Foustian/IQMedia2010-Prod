using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    public partial class RawMediaOutput 
    {
        public string Message { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// Number of total Records Count
        /// </summary>
        public int TotalResults { get; set; }

        /// <summary>
        /// Next Page Avaibility
        /// </summary>
        public bool HasNextPage { get; set; }
        
        /// <summary>
        /// Response for PageNumber
        /// </summary>        
        public int PageNumber { get; set; }

        /// <summary>
        /// List of rawmedia
        /// </summary>            
        public List<RawMedia> RawMediaList { get; set; }
    }
}
