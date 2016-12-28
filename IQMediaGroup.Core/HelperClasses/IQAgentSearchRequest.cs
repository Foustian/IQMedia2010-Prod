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
    public class IQAgentSearchRequest
    {
        public int? ID { get; set; }
        
        /// <summary>
        /// Represents ClientID
        /// </summary>
        public Guid? ClientGuid { get; set; }

        /// <summary>
        /// Represents Query_Name
        /// </summary>
        public string Query_Name { get; set; }

        /// <summary>
        /// Represents Query_Version
        /// </summary>
        public Int32? Query_Version { get; set; }

        /// <summary>
        /// Represents SearchTerm
        /// </summary>
        public string SearchTerm { get; set; }        

        /// <summary>
        /// Represents if Record is Avtive or not
        /// </summary>
        public bool? IsActive { get; set; }


    }
}
