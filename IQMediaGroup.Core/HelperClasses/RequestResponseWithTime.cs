using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contains Request URL With Start Time And End Time
    /// </summary>
    public class RequestResponseWithTime
    {
        /// <summary>
        /// Represents WebRequestURL
        /// </summary>
        public string WebRequestURL { get; set; }

        /// <summary>
        /// Represents Start Time Of The Request
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Represents End Time Of The Request
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Represents Response
        /// </summary>
        public string Response { get; set; }

    }
}
