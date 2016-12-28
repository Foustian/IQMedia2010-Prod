using System.Collections.Generic;
using System.Xml.Serialization;

namespace IQMediaGroup.Domain
{
    public class HighlightedPQOutput
    {
        [XmlArrayItem("Text")]
        public List<string> Highlights { get; set; }

        public string Message { get; set; }
        public int Status { get; set; }
    }
}
