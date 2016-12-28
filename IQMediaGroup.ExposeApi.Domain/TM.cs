using System;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public class TM : AgentMedia
    {
        [DataMember(EmitDefaultValue = false)]
        public int Duration { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Market { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime LocalDateTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Station { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string TimeZone { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Url { get; set; }
    }
}