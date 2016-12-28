using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class IQ_Report
    {
        public int ID { get; set; }

        public string ReportType { get; set; }

        public string Title { get; set; }

        public string ReportRule { get; set; }

        public Guid ClientGuid { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool IsActive { get; set; }

        public int _ReportTypeID { get; set; }

        public string Identity { get; set; }

        public Guid ReportGUID { get; set; }
    }
}
