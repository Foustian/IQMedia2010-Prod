using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsvcConsumeService
{
    public class RawMediaInput
    {
        /// <summary>
        /// Represents SearchTerm 
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Represents Title120
        /// </summary>
        public string Title120 { get; set; }

        /// <summary>
        /// Represents Desc100
        /// </summary>
        public string Desc100 { get; set; }

        /// <summary>
        /// Represents FromDate
        /// </summary>
        public string FromDate { get; set; }

        /// <summary>
        /// Represents ToDate
        /// </summary>
        public string ToDate { get; set; }

        /// <summary>
        /// Represents List Of IQ_Cat_Set
        /// </summary>
        public List<IQ_Cat_Set> IQ_Cat_Set { get; set; }

        /// <summary>
        /// Represents IQ_Class_Set
        /// </summary>
        public List<IQ_Class_Set> IQ_Class_Set { get; set; }

        /// <summary>
        /// Represents IQ_Dma_Set
        /// </summary>
        public List<IQ_Dma_Set> IQ_Dma_Set { get; set; }

        /// <summary>
        /// Represents Station_Affil_Set
        /// </summary>
        public List<Station_Affil_Set> Station_Affil_Set { get; set; }

        /// <summary>
        /// Represents IQ_Time_Zone
        /// </summary>
        public string IQ_Time_Zone { get; set; }

        /// <summary>
        /// Represents PageSize
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Represents PageNumber
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// Represents Sort Field
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// Represents SessionID
        /// </summary>
        public string SessionID { get; set; }

        /// <summary>
        /// Represents CustomerID
        /// </summary>
        public Int64 CustomerID { get; set; }

        /// <summary>
        /// Represents Appearing
        /// </summary>
        public string Appearing { get; set; }
    }
}
