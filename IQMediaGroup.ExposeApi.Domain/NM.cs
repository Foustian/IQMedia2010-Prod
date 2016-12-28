using System;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public class NM : AgentMedia
    {
        [DataMember(EmitDefaultValue=false)]
        public int Audience { get; set; }

        [DataMember(Name = "AudienceValueType",EmitDefaultValue=false)]
        public string AudienceResult { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal IQAdShareValue { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal IQProminence { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal IQProminenceMultiplier { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Market { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Publication { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Url { get; set; }

    }
}
