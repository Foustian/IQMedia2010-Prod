using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// This class contains Customer details
    /// </summary>
    public class BillFrequency
    {
        public Int64 BillFrequencyKey { get; set; }

        public string Bill_Frequency { get; set; }

        public string Bill_Frequency_Description { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsActive { get; set; }

    }
}
