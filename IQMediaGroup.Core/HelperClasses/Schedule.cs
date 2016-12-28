using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class Schedule
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public Int64 ScheduleKey { get; set; }

        public string StationID { get; set; }

        public string ProgramID { get; set; }

        public DateTime air_date { get; set; }

        public string air_time { get; set; }

        public string duration { get; set; }

        public string tv_rating { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
