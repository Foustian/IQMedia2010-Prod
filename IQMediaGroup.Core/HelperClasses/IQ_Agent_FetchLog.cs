using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contains Request URL With Start Time And End Time
    /// </summary>
    public class IQ_Agent_FetchLog
    {
        /// <summary>
        /// Represent StartTime
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Represents AdvancedSearchParams
        /// </summary>
        //public AdvancedSearch AdvancedSearchParams { get; set; }

        /// <summary>
        /// Represents PMGRequest
        /// </summary>
        public string PMGRequest { get; set; }
        
        /// <summary>
        /// Represents PMGResponse
        /// </summary>
        public string PMGResponse  { get; set; }                        

        /// <summary>
        /// Represent EndTime
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// This represents Exception
        /// </summary>
        public string Exception { get; set; }
    }

    public class AdvancedSearch
    {
        /// <summary>
        /// Represents Query Name
        /// </summary>
        public string Query { get; set; }
        
        /// <summary>
        /// Represents Query Name
        /// </summary>
        public string QueryName { get; set; }

        /// <summary>
        /// Represents Query Version
        /// </summary>
        public string QueryVersion { get; set; }

        /// <summary>
        /// Represents ClientID
        /// </summary>
        public string ClientID { get; set; }
    }
}
