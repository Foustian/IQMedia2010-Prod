using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class IQAgent_NMResult
    {
        public int ID { get; set; }

        public int IQAgentSearchRequestID { get; set; }

        public string ArticleID { get; set; }

        public string Url { get; set; }

        public string Publication { get; set; }

        public string Title { get; set; }

        public DateTime harvest_time { get; set; }

        public string Category { get; set; }

        public string Genre { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
