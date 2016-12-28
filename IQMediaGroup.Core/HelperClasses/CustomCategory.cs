using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contais AboutUs Details
    /// </summary>
    [Serializable]
    public class CustomCategory
    {
        /// <summary>
        /// Represents PK of Table
        /// </summary>
        public Int64 CategoryKey { get; set; }


        /// <summary>
        /// Represents ClientGUID
        /// </summary>
        public Guid ClientGUID { get; set; }


        /// <summary>
        /// Represents CategoryGUID
        /// </summary>
        public Guid CategoryGUID { get; set; }


        /// <summary>
        /// Represents Category Name
        /// </summary>
        public String CategoryName { get; set; }

        public String DefaultCategory { get; set; }

        /// <summary>
        /// Represents Category Description
        /// </summary>
        public String CategoryDescription { get; set; }

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
        public bool IsActive { get; set; }
    }
}
