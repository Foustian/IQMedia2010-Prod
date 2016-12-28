using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class HighlightedCCOutput
    {
        public List<ClosedCaption> CC { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
