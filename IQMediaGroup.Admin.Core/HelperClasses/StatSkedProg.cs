using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Admin.Core.HelperClasses
{
    public class StatSkedProg
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public Int64 StatSkedProgKey { get; set; }

        public string Station_ID { get; set; }

        public string Station_Call_Sign { get; set; }

        public string IQ_Cluster { get; set; }

        public string Station_Num { get; set; }

        public string Station_Name { get; set; }

        public string station_time_zone { get; set; }

        public string Station_Affil { get; set; }

        public string Station_Affil_Num { get; set; }

        public string IQ_Dma_Name { get; set; }

        public string IQ_Dma_Num { get; set; }

        public string IQ_Time_Zone { get; set; }

        public string IQ_Time_Zone_Num { get; set; }

        public string Database_key { get; set; }      

        public DateTime Air_ToDate { get; set; }

        public string Title120 { get; set; }

        public string Desc100 { get; set; }

        public string IQ_Cat { get; set; }

        public string IQ_Cat_Num { get; set; }

        public string IQ_class { get; set; }

        public string IQ_Class_Num { get; set; }

        public string IQ_Air_ToTime { get; set; }

        public string IQ_Segs { get; set; }

        public DateTime IQ_Local_Air_Date { get; set; }

        public string IQ_Local_Air_Time { get; set; }

        public string IQ_Rec_Type { get; set; }

        public string IQ_Master_key { get; set; }

        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }

        /// <summary>
        /// Represents IQ_CC_Key
        /// </summary>
        public string IQ_CC_Key { get; set; }

        /// <summary>
        /// Represents Current Page No.
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// Represents Page Size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Represents Sort order of result.By default its ASC order.If specified '-' at end of field name it will be DESC.
        /// </summary>
        public string SortField { get; set; }

        public string RL_GUID { get; set; }

        public bool? IsAscending { get; set; }
    }
}
