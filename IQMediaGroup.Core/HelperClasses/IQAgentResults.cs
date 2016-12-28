using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contais Clip Details
    /// </summary>
    [Serializable]
    public class IQAgentResults
    {
        public Int64 ID { get; set; }

        /// <summary>
        /// Represents SearchRequestID
        /// </summary>
        public Int64 SearchRequestID { get; set; }

        /// <summary>
        /// Represents RL_VedioGUID
        /// </summary>
        public Guid RL_VideoGUID { get; set; }

        /// <summary>
        /// Represents Search Term
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Represents Rl_Station
        /// </summary>
        public string Rl_Station { get; set; }

        /// <summary>
        ///  Represents RL_Date
        /// </summary>
        public DateTime RL_Date { get; set; }


        /// <summary>
        ///  Represents RL_Time
        /// </summary>
        public Int32 RL_Time { get; set; }

        /// <summary>
        /// Represents RL_Market
        /// </summary>
        public string RL_Market { get; set; }

        /// <summary>
        /// Represents Station Logo
        /// </summary>
        public string StationLogo { get; set; }

        /// <summary>
        /// Represents Program Title
        /// </summary>
        public string Title120 { get; set; }

        /// <summary>
        /// Represents Program IQCCKey
        /// </summary>
        public string iq_cc_key { get; set; }

        /// <summary>
        /// Represents Hits Count
        /// </summary>
        public Int32 Number_Hits { get; set; }

        /// <summary>
        /// Represents Communication_flag
        /// </summary>
        public bool Communication_flag { get; set; }

        /// <summary>
        /// Represents DatabaseKey 
        /// </summary>
        public string DatabaseKey { get; set; }

        /// <summary>
        /// Represents IQ_Local_Air_Date.Only Date Portion
        /// </summary>
        public DateTime? IQ_Local_AirDate { get; set; }

        /// <summary>
        /// Represents IQ_Local_Air_DateTime 
        /// </summary>
        public DateTime? IQ_Local_Air_DateTime { get; set; }

        /// <summary>
        /// Represents Complete
        /// </summary>
        public bool Complete { get; set; }

        /// <summary>
        /// Represents Created Date
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Represents Modified Date
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Represents Created By
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents Modified By
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Represents IsActive
        /// </summary>
        public bool? IsActive { get; set; }

        public string StationMarket { get; set; }

        public int PageNo { get; set; }

        public int PageSize { get; set; }

        public string SortField { get; set; }

        public bool IsAscending { get; set; }

        public string IQAgentResultUrl { get; set; }

        public List<IQAgentResults> ChildResults { get; set; }

        public string SQAD_SHAREVALUE { get; set; }

        public Int32? AUDIENCE { get; set; }

        public bool IsActualNielsen { get; set; }
    }
}
