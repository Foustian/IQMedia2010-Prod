using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace IQMediaGroup.Core.HelperClasses
{
    [XmlRoot]
    public class CustomerSearchResult
    {
        public CustomerSearchResult()
        {
        }


        public string TimeZone { get; set; }

        public string Station { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

        public List<string> Markets { get; set; }

        public List<string> ProgramType { get; set; }

        public List<string> ProgramCategory { get; set; }

        public string SearchTerm { get; set; }

        public string Program { get; set; }
    }
}
