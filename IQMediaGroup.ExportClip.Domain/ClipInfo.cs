using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.ExportClip.Domain
{
    [Serializable]
    [XmlRoot]
    public class ClipInfo
    {
        [XmlElement]
        public int? StartOffset { get; set; }

        [XmlElement]
        public int? EndOffset { get; set; }

        [XmlElement]
        public Guid? ClientGuid { get; set; }

        [XmlElement]
        public Guid? CategoryGuid { get; set; }

        [XmlElement]
        public string ClipTitle { get; set; }

        [XmlElement]
        public string ClipDesc { get; set; }

        [XmlElement]
        public string ClipKeyword { get; set; }

        [XmlElement]
        public string FFMpegCommand { get; set; }
    }
}
