using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class RL_CC_TEXT
    {
        /// <summary>
        /// Represents Primary key
        /// </summary>
        public Int64? RL_CC_TEXTKey { get; set; }

        /// <summary>
        /// Represents RL_Station_ID
        /// </summary>
        public String RL_Station_ID { get; set; }

        /// <summary>
        /// Represents RL_Station_Date
        /// </summary>
        public DateTime RL_Station_Date { get; set; }

        /// <summary>
        /// Represents RL_Station_Time
        /// </summary>
        public Int32 RL_Station_Time { get; set; }

        /// <summary>
        /// Represents RL_Time_Zone
        /// </summary>
        public String RL_Time_Zone { get; set; }

        /// <summary>
        /// Represents RL_CC_FileName
        /// </summary>
        public String RL_CC_FileName { get; set; }

        /// <summary>
        /// Represents RL_CC_File_Location
        /// </summary>
        public String RL_CC_File_Location { get; set; }

        /// <summary>
        /// Represents GMT_Date
        /// </summary>
        public DateTime GMT_Date { get; set; }

        /// <summary>
        /// Represents GMT_Time
        /// </summary>
        public Int32 GMT_Time { get; set; }

        /// <summary>
        /// Represents IQ_CC_Key
        /// </summary>
        public String IQ_CC_Key { get; set; }

        /// <summary>
        /// Represents CC_File_Status
        /// </summary>
        public String CC_File_Status { get; set; }

        /// <summary>
        /// Represents CC_Ingest_State
        /// </summary>
        public DateTime? CC_Ingest_Date { get; set; }

        /// <summary>
        /// Represents RL_Station_Exception_Date
        /// </summary>
        public DateTime? RL_Station_Exception_Date { get; set; }

        /// <summary>
        /// Represents RL_Station_Exception_Time
        /// </summary>
        public Int32? RL_Station_Exception_Time { get; set; }
    }
}
