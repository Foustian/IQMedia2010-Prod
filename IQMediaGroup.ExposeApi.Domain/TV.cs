using System;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public class TVAgentResult : AgentMedia
    {
        [DataMember(EmitDefaultValue = false)]
        public int Audience { get; set; }

        [DataMember(Name = "AudienceValueType",EmitDefaultValue=false)]
        public string AudienceResult { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CC { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal IQProminence { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal IQProminenceMultiplier { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Market { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime MediaDateTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal IQAdShareValue { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal NationalAudience { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string NationalAudienceValueType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal NationalIQAdShareValue { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Station { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string StationCallSign { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string StationLogo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string TimeZone { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ThumbUrl { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Url { get; set; }

        [IgnoreDataMember]
        public Guid VideoGuid { get; set; }        
    }
}