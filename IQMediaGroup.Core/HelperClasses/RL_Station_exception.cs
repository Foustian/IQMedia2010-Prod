using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class RL_Station_exception
    {

        ///
        /// Represents RL_Station_exceptionKey Field 
        ///
        public Int64? RL_Station_exceptionKey { get; set; }

        ///
        /// Represents RL_Station_ID Field 
        ///
        public String RL_Station_ID { get; set; }

        ///
        /// Represents RL_Station_Date Field 
        ///
        public DateTime RL_Station_Date { get; set; }

        ///
        /// Represents RL_Station_Time Field 
        ///
        public Int32 RL_Station_Time { get; set; }

        ///
        /// Represents Time_zone Field 
        ///
        public String Time_zone { get; set; }

        ///
        /// Represents GMT_Adj Field 
        ///
        public String GMT_Adj { get; set; }

        ///
        /// Represents DST_Adj Field 
        ///
        public String DST_Adj { get; set; }

        ///
        /// Represents IQ_Process Field 
        ///
        public String IQ_Process { get; set; }

        ///
        /// Represents Pass_count Field 
        ///
        public String Pass_count { get; set; }

        ///
        /// Represents CreatedBy Field 
        ///
        public String CreatedBy { get; set; }

        ///
        /// Represents ModifiedBy Field 
        ///
        public String ModifiedBy { get; set; }

        ///
        /// Represents CreatedDate Field 
        ///
        public DateTime CreatedDate { get; set; }

        ///
        /// Represents ModifiedDate Field 
        ///
        public DateTime ModifiedDate { get; set; }

        ///
        /// Represents IsActive Field 
        ///
        public Boolean IsActive { get; set; }

        public string RL_CC_TEXT_FileName { get; set; }

        /// <summary>
        /// Represents Converted Date for Request
        /// </summary>
        public DateTime? RQ_Converted_Date { get; set; }

        /// <summary>
        /// Represents Converted Time for Request
        /// </summary>
        public int? RQ_Converted_Time { get; set; }
    }
}