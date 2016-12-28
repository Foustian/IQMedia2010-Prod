using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQMediaGroup.AdvanceSearchServiceSample.Class
{
    public class RadioRawMediaOutput
    {
        /// <summary>
        /// List of RadioRawMedia
        /// </summary>
        public List<RadioRawMedia> RadioRawMedia { get; set; }

        /// <summary>
        /// Flag for next page is available or not
        /// </summary>
        public int TotalRecordsCount { get; set; }

        public bool HasNextPage { get; set; }
    }
}