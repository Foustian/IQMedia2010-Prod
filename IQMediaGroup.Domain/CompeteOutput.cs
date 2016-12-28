using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class CompeteOutput
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public List<CompeteData> CompeteDataSet { get; set; }
    }

    public class CompeteData
    {
        public string ArticleID
        {
            get;
            set;
        }

        public bool IsCompeteAll
        {
            get;
            set;
        }

        public String IQ_AdShare_Value
        {
            get;
            set;
        }

        public String c_uniq_visitor
        {
            get;
            set;
        }
    }
}
