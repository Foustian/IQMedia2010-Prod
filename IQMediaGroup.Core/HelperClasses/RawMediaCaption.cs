using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contais RawMedia Caption Details
    /// </summary>
    [XmlType("SearchResponse")]
    public class RawMediaCaption
    {
        /// <summary>
        /// Represents RawMedia Class Info
        /// </summary>
        [XmlElement(ElementName = "RawMedia")]
        public List<RawMedia> RawMediaRequestInfo;
    }
}
