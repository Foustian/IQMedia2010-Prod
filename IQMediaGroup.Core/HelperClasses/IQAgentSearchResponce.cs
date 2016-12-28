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
    public class IQAgentSearchResponce
    {
        public int SearchResponceKey { get; set; }

        /// <summary>
        /// Represents SearchRequestID
        /// </summary>
        public Int64 SearchRequestID { get; set; }

        /// <summary>
        /// Represents CustomerID
        /// </summary>
        public Int32 CustomerID { get; set; }

        /// <summary>
        /// Represents Raw Media GUID
        /// </summary>
        public Guid RawMediaGUID { get; set; }

        /// <summary>
        /// Represents Hits Count
        /// </summary>
        public Int32 HitsCount { get; set; }

        /// <summary>
        /// Represents Json Status
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Represents CCText
        /// </summary>
        public string CCText { get; set; }

        /// <summary>
        /// Represents Loop2Status
        /// </summary>
        public string RedllasoStationCode { get; set; }

        /// <summary>
        /// Represents Responce Date
        /// </summary>
        public DateTime ResponceDate { get; set; }

    }
}
