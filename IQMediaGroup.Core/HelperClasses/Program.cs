using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class Program
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public Int64 ProgramKey { get; set; }

        public string Program_MediaService_ID { get; set; }      
        
        public Int32 StationID { get; set; }

        public string StationMarket { get; set; }

        public Int64 StationMarketID { get; set; }

        public string station_time_zone { get; set; }

        public string title120 { get; set; }

        public string title70 { get; set; }

        public string title40 { get; set; }

        public string title20 { get; set; }

        public string title10 { get; set; }

        public string desc100 { get; set; }

        public string desc60 { get; set; }

        public string desc40 { get; set; }


        public string genre_desc1 { get; set; }

        public string genre_desc2 { get; set; }

        public string genre_desc3 { get; set; }

        public string genre_desc4 { get; set; }

        public string genre_desc5 { get; set; }

        public string genre_desc6 { get; set; }

        public string description { get; set; }

        public string mpaa_rating { get; set; }

        public string star_rating { get; set; }

        public string run_time { get; set; }


        public string source_tv { get; set; }

        public string show_type { get; set; }

        public string holiday { get; set; }

        public string syn_epi_num { get; set; }

        public string alt_syn_epi_num { get; set; }

        public string epi_title { get; set; }

        public string desc255 { get; set; }

        public string reduced_desc100 { get; set; }

        public string org_air_date { get; set; }

       
        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
