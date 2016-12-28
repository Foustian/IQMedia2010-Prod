using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class InboundReporting
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public long InboundReportingKey { get; set; }

        /// <summary>
        /// Represents Request Collection(SERVER_VARIABLES,Forms,Querystring)
        /// </summary>
        public string RequestCollection { get; set; }

        /// <summary>
        /// Represents Created Date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Represents Modified Date
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Represents Created By
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents Modified By
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
