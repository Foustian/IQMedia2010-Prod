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
    public class RL_GUIDS_Caption
    {
        /// <summary>
        /// Represents RawMedia Class Info
        /// </summary>
        [XmlElement(ElementName = "RL_GUIDS")]
        public List<RL_GUIDS> RL_GUIDSRequestInfo;
    }
}
