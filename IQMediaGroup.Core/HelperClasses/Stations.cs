using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class Stations
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public Int64 StationKey { get; set; }

        public string Station_MediaService_ID { get; set; }

        public string station_time_zone { get; set; }

        public string station_name { get; set; }

        public string station_call_sign { get; set; }

        public string station_affil { get; set; }

        public string station_city { get; set; }

        public string station_state { get; set; }

        public string station_zip_code { get; set; }

        public string station_country { get; set; }

        public string station_language { get; set; }

        public Int32 CityID { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
