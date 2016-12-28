using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Service.TVEyes.Download
{
    public class DownloadTask : IEquatable<DownloadTask>
    {
        public Int64 ID { get; set; }
        public Int64 _RootPathID { get; set; }
        public DateTime UTCDateTime { get; set; }
        public string Package { get; set; }
        public Guid TMGuid { get; set; }

        public bool Equals(DownloadTask other)
        {
            return ID.Equals(other.ID);
        }
    }
}
