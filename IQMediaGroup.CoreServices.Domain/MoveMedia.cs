using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.CoreServices.Domain
{
    public class MoveMedia
    {
        public Int64 ID { get; set; }

        public Guid RecordFileGUID { get; set; }

        public string OriginLocation { get; set; }

        public string OriginStatus { get; set; }

        public int OriginRPID { get; set; }

        public string OriginSite { get; set; }

        public string DestinationLocation { get; set; }

        public string DestinationStatus { get; set; }

        public int? DestinationRPID { get; set; }

        public string DestinationSite { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
    }
}
