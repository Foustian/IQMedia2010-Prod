using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Represents RequestResponseRawMedia Details
    /// </summary>
    [XmlType("RequestResponseRawMedia")]
    public class RequestResponseRawMedia
    {
        /// <summary>
        /// Represents List Of RequestURL Information.
        /// </summary>
        [XmlElement(ElementName = "Request")]
        public List<RequestURL> ListOfRequestURL { get; set; }

        /// <summary>
        /// Represents List Of RawMedia Information.
        /// </summary>
        [XmlElement(ElementName = "Response")]
        public List<RawMedia> ListOfRawMedia { get; set; }
    }
}
