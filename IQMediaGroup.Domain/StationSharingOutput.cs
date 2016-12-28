using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class StationSharingOutput
    {
        public string Message { get; set; }
        public Int32 Status { get; set; }
        public Boolean IsSharing { get; set; }
    }
}
