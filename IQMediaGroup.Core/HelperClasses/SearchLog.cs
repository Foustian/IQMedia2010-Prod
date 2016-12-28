using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contais Client Details
    /// </summary>
    public class SearchLog
    {
        /// <summary>
        /// Represents Primary Key
        /// </summary>
        public Int64 PMGSearchLogKey { get; set; }

        /// <summary>
        /// Represents CustomerID
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// Represents Search Type
        /// </summary>
        public string SearchType { get; set; }

        /// <summary>
        /// Represents RequestXML
        /// </summary>
        public string RequestXML { get; set; }

        /// <summary>
        /// Represents ResponseXML
        /// </summary>
        public string ErrorResponseXML { get; set; }
    }
}
