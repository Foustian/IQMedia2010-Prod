using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contais Clip Details
    /// </summary>
    public class MediaCategory
    {
        /// <summary>
        /// Represents CategoryID
        /// </summary>
        public Int32 CategoryKey { get; set; }

        /// <summary>
        /// Represents Name of Category
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Represents CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Represents CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Represents ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Represents Flag for particular record is active or not.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Represents Name of Category
        /// </summary>
        public string CategoryCode { get; set; }

    }
}
