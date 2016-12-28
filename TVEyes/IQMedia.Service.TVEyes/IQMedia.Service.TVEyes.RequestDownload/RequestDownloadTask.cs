using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Service.TVEyes.RequestDownload
{
    public class RequestDownloadTask : IEquatable<RequestDownloadTask>
    {
        public Int64 ID { get; set; }
        public string StationID { get; set; }
        public DateTime LocalDateTime { get; set; }
        public int Duration { get; set; }

        public bool Equals(RequestDownloadTask other)
        {
            return ID.Equals(other.ID);
        }
    }
}
