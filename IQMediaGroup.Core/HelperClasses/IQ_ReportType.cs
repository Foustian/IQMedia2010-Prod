using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class IQ_ReportType
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Identity { get; set; }

        public string Description { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool IsActive { get; set; }
    }
}
