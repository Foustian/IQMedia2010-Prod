using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Service.NM.GeneratePDF
{
    public class GeneratePDFTask : IEquatable<GeneratePDFTask>
    {
        public string ArticleID { get; set; }

        public string Url { get; set; }

        public DateTime Harvest_Time { get; set; }

        public int RootPathID { get; set; }

        public bool Equals(GeneratePDFTask other)
        {
            return ArticleID.Equals(other.ArticleID);
        }
    }
}
