using System.Collections.Generic;
using System;

namespace IQMediaGroup.Domain
{
    public class CompeteDemographicOutput : CommandOutput
    {
        public List<CompeteDemographicData> CompeteDemographicDataList { get; set; }
    }

    public class CompeteDemographicData
    {
        public string CompeteURL { get; set; }

        public bool IsCompeteAll { get; set; }

        public decimal? IQ_AdShare_Value { get; set; }

        public Int64? c_uniq_visitor { get; set; }

        public bool IsUrlFound { get; set; }

        public decimal? AM18_24 { get; set; }

        public decimal? AM25_34 { get; set; }

        public decimal? AM35_44 { get; set; }

        public decimal? AM45_54 { get; set; }

        public decimal? AM55_64 { get; set; }

        public decimal? AM65 { get; set; }

        public decimal? AF18_24 { get; set; }

        public decimal? AF25_34 { get; set; }

        public decimal? AF35_44 { get; set; }

        public decimal? AF45_54 { get; set; }

        public decimal? AF55_64 { get; set; }

        public decimal? AF65 { get; set; }
    }
}