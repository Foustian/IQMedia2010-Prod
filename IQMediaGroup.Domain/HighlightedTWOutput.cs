using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Domain
{
    public class HighlightedTWOutput
    {
        public string Text { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
