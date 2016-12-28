using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Admin.Core.HelperClasses
{
    /// <summary>
    /// This class contains Customer details
    /// </summary>
    public class PricingCode
    {
        public Int64 PricingCodeKey { get; set; }

        public string Pricing_Code { get; set; }

        public string Pricing_Code_Description { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsActive { get; set; }

    }
}
