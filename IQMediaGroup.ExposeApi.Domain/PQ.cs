using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public class PQ : AgentMedia
    {
        [DataMember(EmitDefaultValue = false)]
        public List<string> Authors { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime AvailableDateTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Copyright { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Publication { get; set; }        
    }
}