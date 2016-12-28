using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace IsvcConsumeService
{       
    public partial class RawMediaOutput 
    {
        /// <summary>
        /// Response for PageNumber
        /// </summary>        
        public int PageNumber { get; set; }

        /// <summary>
        /// List of rawmedia
        /// </summary>        
        public List<RawMedia> RawMedia { get; set; }

        /// <summary>
        /// Number of total Records Count
        /// </summary>
        //public int TotalRecordsCount { get; set; }

        /// <summary>
        /// Next Page Avaibility
        /// </summary>
        public bool HasNextPage { get; set; }

    }
}
