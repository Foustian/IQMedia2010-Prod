using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class IQAgent_SMResult
    {
        public int ID { get; set; }

        public int IQAgentSearchRequestID { get; set; }

        public string SeqID { get; set; }

        public string link { get; set; }

        public string homelink { get; set; }

        public string description { get; set; }

        public DateTime itemHarvestDate_DT { get; set; }

        public string feedCategories { get; set; }

        public string feedClass { get; set; }

        public int feedRank { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
