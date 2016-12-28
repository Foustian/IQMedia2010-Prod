using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class MediaPlayerURLInput
    {
        [XmlElement]
        public string SessionID { get; set; }

        [XmlElement]
        public string StationID { get; set; }

        [XmlElement]
        public DateTime? DateTime { get; set; }

        [XmlElement]
        public Int64? SeqID { get; set; }

        [XmlElement]
        public bool SearchOnGMTDateTime { get; set; }
    }
}
