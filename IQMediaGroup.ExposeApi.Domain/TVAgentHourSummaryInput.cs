using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class TVAgentHourSummaryInput
    {
        public string SessionID { get; set; }

        public Int64? SRID { get; set; }

        public DateTime? FromDateTime { get; set; }

        public DateTime? ToDateTime { get; set; }
    }
}
