using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class IQAgentIFrame
    {
        public Guid iQAgentiFrameID { get; set; }

        public Guid RawMediaGuid { get; set; }

        public DateTime Expiry_Date { get; set; }

        public string SearchTerm { get; set; }        
    }

}