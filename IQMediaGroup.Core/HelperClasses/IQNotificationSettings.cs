using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Represents property of IQNotificationSettings Table.
    /// </summary>
    public class IQNotificationSettings
    {
        /// <summary>
        /// Represnts Primary key of table.
        /// </summary>
        public long IQNotificationKey { get; set; }

        /// <summary>
        /// Represents SearchRequestID
        /// </summary>
        public long SearchRequestID { get; set; }

        /// <summary>
        /// Represents Type of entry. Ex. SMS or Email Notification
        /// </summary>
        public string TypeofEntry { get; set; }

        /// <summary>
        /// Represents  Notification Address. Ex. Email Address = xyz@abc.com or Mobile No like +00-123456
        /// </summary>
        public string Notification_Address { get; set; }


        /// <summary>
        /// Represents Frequency to sent noptification.Values are Immediate,Hourly,Once a Day,Once a Week
        /// </summary>
        public string Frequency { get; set; }

        /// <summary>
        /// Represents CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Represents Created By
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents ModifiedDate 
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Represents Modified By
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Represents IsActive
        /// </summary>
        public bool IsActive { get; set; }
    }
}
