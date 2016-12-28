using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class IQNotificationTracking_FetchLog
    {
        /// <summary>
        /// Represents Start Time Of The Request
        /// </summary>
        public string Startdatetimestamp { get; set; }

        public string SearchRequestID { get; set; }

        public string Notification_Address { get; set; }

        public string Frequency { get; set; }

        public string IsEmailSent { get; set; }

        /// <summary>
        /// Represents Databasereadorwritefailure
        /// </summary>
        public string Databasereadorwritefailure { get; set; }

        /// <summary>
        /// Represents End Time Of The Request
        /// </summary>
        public string Enddatetimestamp { get; set; }

    }
}
