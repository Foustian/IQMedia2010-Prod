using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Domain
{
    public class CompeteInput
    {
        public Guid? ClientGuid { get; set; }

        [XmlArrayItem("ArticleID")]
        public List<string> ArticleIDSet { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }
    }
}
