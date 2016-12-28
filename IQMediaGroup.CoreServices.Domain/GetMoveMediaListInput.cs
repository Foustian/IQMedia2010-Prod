using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.CoreServices.Domain
{
    public class GetMoveMediaListInput
    {
        public string OriginSite { get; set; }

        public string DestinationSite { get; set; }

        public string OriginStatus { get; set; }

        public string DestinationStatus { get; set; }

        public int NumRecords { get; set; }
    }
}
