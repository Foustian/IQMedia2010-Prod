using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class Cluster
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public Int64? ClusterKey { get; set; }

        /// <summary>
        /// Represents ClusterName
        /// </summary>
        public string ClusterName { get; set; }       

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
        /// Represents Flag for particular record is active or not
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
