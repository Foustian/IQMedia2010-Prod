using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contains RedlassoStation Information
    /// </summary>
    public class RedlassoStationMarket
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public Int64? RedlassoStationMarketKey { get; set; }

        /// <summary>
        /// Represents Station Market
        /// </summary>
        public string StationMarketName { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
