using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contais Client Details
    /// </summary>
    public class OutboundReporting
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public Int64 OutboundReportingKey { get; set; }

        /// <summary>
        /// Represents Query_Name
        /// </summary>
        public string Query_Name { get; set; }

        /// <summary>
        /// Represents EmailAddress
        /// </summary>
        public string FromEmailAddress { get; set; }

        public string ToEmailAddress { get; set; }

      
        /// <summary>
        /// Represents MailContent
        /// </summary>
        public string MailContent { get; set; }

        /// <summary>
        /// Represents ServiceType
        /// </summary>
        public string ServiceType { get; set; }
    }
}
