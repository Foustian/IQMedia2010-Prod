using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class TVAgentHourSummaryOutput
    {
        public string Message { get; set; }

        public int Status { get; set; }
        
        public List<HourSummary> HourSummaryList { get; set; }
    }
}
