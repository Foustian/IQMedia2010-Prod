using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class IQMediaGroupExceptions
    {
        /// <summary>
        /// Represents Primary key field
        /// </summary>
        public Int32 IQMediaGroupExceptionsKey { get; set; }

        /// <summary>
        /// Represents ExceptionStackTrace of Exception
        /// </summary>
        public string ExceptionStackTrace { get; set; }

        /// <summary>
        /// Represents Exceptiom Message of Exception
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// Represents Date Of Exception
        /// </summary>
        public DateTime? ExceptionDate { get; set; }

        /// <summary>
        /// Represents CreatedBy for Record
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents ModifiedBy for Record
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Represents Date Of Creation of particular Record
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Represents Date Of Modification of particular Record
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Represents particular record is currently Active or not
        /// </summary>
        public bool IsActive { get; set; }
    }
}
