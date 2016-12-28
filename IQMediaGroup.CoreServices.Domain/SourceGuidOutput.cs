using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.CoreServices.Domain
{
    public class SourceGuidOutput
    {
        public int status { get; set; }
        public string message { get; set; }
        public Guid? sourceGuid { get; set; }
    }
}
