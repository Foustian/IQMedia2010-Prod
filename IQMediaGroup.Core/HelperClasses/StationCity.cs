using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contains Station Details
    /// </summary>
    public class StationCity
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public Int64? StationCityKey { get; set; }

        /// <summary>
        /// Represents Name of Station
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// Represents Code of Station
        /// </summary>
        public string StationCode { get; set; }

        /// <summary>
        /// Represents Description of Station
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Represents Date of Creation
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Represents Date of Modification
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Represents Flag active or not
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
