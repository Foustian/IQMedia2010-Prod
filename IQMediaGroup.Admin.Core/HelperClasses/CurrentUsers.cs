using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Admin.Core.HelperClasses
{
    public class CurrentUsers
    {
        /// <summary>
        /// Represents ClientKey
        /// </summary>
        public Int64 CustomerKey { get; set; }

        /// <summary>
        /// Represents Timeout
        /// </summary>
        public DateTime EndTime { get; set; }

        

        /// <summary>
        /// Represents SessionID
        /// </summary>
        public string SessionID { get; set; }

        /// <summary>
        /// Represents IsActive or not
        /// </summary>
        public bool IsActive { get; set; }

        public bool? MultiLogin { get; set; }
    }
}
