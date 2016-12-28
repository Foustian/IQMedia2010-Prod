using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class NielsenDemographicOutput
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public List<NielsenDemographicData> NielsenDemographicDataList { get; set; }
    }

    public class NielsenDemographicData
    {
        public string IQ_CC_Key { get; set; }

        public System.Guid? GUID { get; set; }

        public string SQAD_SHAREVALUE { get; set; }

        public Decimal AM18_20 { get; set; }

        public Decimal AM21_24 { get; set; }

        public Decimal AM25_34 { get; set; }

        public Decimal AM35_49 { get; set; }

        public Decimal AM50_54 { get; set; }

        public Decimal AM55_64 { get; set; }

        public Decimal AM65 { get; set; }

        public Decimal AF18_20 { get; set; }

        public Decimal AF21_24 { get; set; }

        public Decimal AF25_34 { get; set; }

        public Decimal AF35_49 { get; set; }

        public Decimal AF50_54 { get; set; }

        public Decimal AF55_64 { get; set; }

        public Decimal AF65 { get; set; }

    }
}
