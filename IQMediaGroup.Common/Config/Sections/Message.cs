using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace IQMediaGroup.Common.Config.Sections
{
    public class Message
    {
        [XmlAttribute]
        public string Key { get; set; }

        [XmlAttribute]
        public string Value { get; set; }   
    }
}